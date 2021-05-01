using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gtk;

namespace ThemeManager
{
    public class BashHandler
    {
        private static readonly BashHandler _instance = null;
        
        private BashHandler()
        {
            Initialize();
#if DEBUG
            CursorList.ForEach(Console.WriteLine); // Would run
#endif
        }

        private string IconOutput { get; set; }
        private string ShellOutput { get; set; }
        private string ThemeOutput { get; set; }
        private string CursorOutput { get; set; }
        
        public List<string> IconList { get; private set; }
        public List<string> ShellList { get; private set; }
        public List<string> ThemeList { get; private set; }
        public List<string> CursorList { get; private set; }
        
        public bool UserThemeExtensionExists { get; private set; }
        
        public static BashHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new BashHandler();
                }
                
                return _instance;
            }
        }

        private void Initialize()
        {
            ThemeOutput = Bash(@"
                        for f in ~/.themes/*
                        do
                            if [[ -d ""$f/gtk-3.0"" ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done ") +
                                Bash(@"
                        for f in /usr/share/themes/*
                        do
                            if [[ -d ""$f/gtk-3.0"" ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done");
            IconOutput = Bash(@"for f in /usr/share/icons/*
                        do
                            if [[ -f ""$f/index.theme""     ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done ")
                       + Bash(@"for f in ~/.local/share/icons/*
                        do
                            if [[ -f ""$f/index.theme""  ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done ")
                       + Bash(@"for f in ~/.icons/*
                        do
                            if [[ -f ""$f/index.theme""  ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done ");

            CursorOutput = Bash(@"for f in /usr/share/icons/*
                        do
                            if [[ -f ""$f/index.theme""  &&    -d ""$f/cursors"" ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done ")
                       + Bash(@"for f in ~/.local/share/icons/*
                       do
                            if [[ -f ""$f/index.theme""  &&    -d ""$f/cursors"" ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done ")
                       + Bash(@"for f in ~/.icons/*
                       do
                            if [[ -f ""$f/index.theme""  &&    -d ""$f/cursors"" ]]
                            then
                                        echo $(basename -- ""$f"")
                            fi
                        done ");
            
            ShellOutput = Bash(@"
                        for f in /usr/share/themes/*
                        do
                            if [[ -d ""$f/gnome-shell"" ]]
                                    then
                                        
                                        echo $(basename -- ""$f"")
                                        fi
                                    done") + Bash(@"
                        for f in ~/.themes/*
                        do
                            if [[ -d ""$f/gnome-shell"" ]]
                                    then
                                        echo $(basename -- ""$f"")
                                        fi
                                    done");

            var box = new Box(Orientation.Vertical, 6);

            ThemeList = ThemeOutput.Split("\n").ToList();
            ThemeList.RemoveAll(string.IsNullOrWhiteSpace);
            // ThemeList.ForEach(s=>Console.Write(s+"\n"));
            ThemeList = ThemeList.Distinct().ToList();
            ThemeList.Sort();

            IconList = IconOutput.Split("\n").ToList();
            IconList.RemoveAll(string.IsNullOrWhiteSpace);
            // IconList.ForEach(s=>Console.Write(s+"\n"));
            IconList = IconList.Distinct().ToList();
            IconList.Sort();

            ShellList = ShellOutput.Split("\n").ToList();
            ShellList.RemoveAll(string.IsNullOrWhiteSpace);
            // iconList.ForEach(s=>Console.Write(s+"\n"));
            ShellList = ShellList.Distinct().ToList();
            ShellList.Sort();

            CursorList = CursorOutput.Split("\n").ToList();
            CursorList.RemoveAll(string.IsNullOrWhiteSpace);
            CursorList = CursorList.Distinct().ToList();
            CursorList.Sort();
            UserThemeExtensionExists = CheckUserThemeExtExists();
        }

        public void Reload() => Initialize();

        public bool CheckUserThemeExtExists() =>
            Bash("gnome-extensions list --enabled | grep user-theme@gnome-shell-extensions.gcampax.github.com")
                .Replace("\n", "") == "user-theme@gnome-shell-extensions.gcampax.github.com";

        public void ChangeTheme(string theme)
            => Bash($"gsettings set org.gnome.desktop.interface gtk-theme \"{theme}\" ");

        public void ChangeIcon(string theme)
            => Bash($"gsettings set org.gnome.desktop.interface icon-theme \"{theme}\" ");

        public void ChangeShell(string theme)
            => Bash($"gsettings set org.gnome.shell.extensions.user-theme name \"{theme}\" ");

        public void ChangeCursor(String theme)
            => Bash($"gsettings set  org.gnome.desktop.interface cursor-theme \"{theme}\" ");

        public string GetTheme()
            => Bash("gsettings get org.gnome.desktop.interface gtk-theme")
                .Replace("\'", "")
                .Trim();

        public string GetIconTheme()
            => Bash("gsettings get org.gnome.desktop.interface icon-theme")
                .Replace("\'", "")
                .Trim();

        public string GetShellTheme()
            => Bash("gsettings get org.gnome.shell.extensions.user-theme name")
                .Replace("\'", "")
                .Trim();

        public string GetCursorTheme()
            => Bash("gsettings get  org.gnome.desktop.interface cursor-theme")
                .Replace("\'", "")
                .Trim();

        private string Bash(string cmd)
        {
            // NOTE: Is it necessary to create a new variable
            // just to use it once instead of overriding it?
            // Might want to change it to:
            // cmd = cmd.Replace("\"", "\\\"");
            // Do not forget to change arguments from
            // `escapedArgs` to `cmd` if you plan to do so.
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"", // Look at the note above.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    ErrorDialog = false
                }
            };
            process.Start();

            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
