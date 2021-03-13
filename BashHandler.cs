using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gtk;

namespace Gtk_Theme_Manager
{
    public class BashHandler
    {
        private static BashHandler instance = null;
        private String IconOutput { get; set; }
        private String ShellOutput { get; set; }
        private String ThemeOutput { get; set; }

        private String CursorOutput { get; set; }
        public List<String> IconList { get; set; }
        public List<String> ShellList { get; set; }
        public List<String> ThemeList { get; set; }
        public List<String> CursorList { get; set; }

        public bool UserThemeExtensionExists { get; set; }

        private BashHandler()
        {
            Initalize();
#if DEBUG
            CursorList.ForEach(s => Console.WriteLine(s)); // Would run
#endif
        }

        public static BashHandler Instance
        {
            get
            {
                if (instance == null) return new BashHandler();
                return instance;
            }
        }

        private void Initalize()
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

            Box box = new Box(Orientation.Vertical, 6);

            ThemeList = ThemeOutput.Split("\n").ToList();
            ThemeList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            // ThemeList.ForEach(s=>Console.Write(s+"\n"));
            ThemeList = ThemeList.Distinct().ToList();
            ThemeList.Sort();

            IconList = IconOutput.Split("\n").ToList();
            IconList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            // IconList.ForEach(s=>Console.Write(s+"\n"));
            IconList = IconList.Distinct().ToList();
            IconList.Sort();

            ShellList = ShellOutput.Split("\n").ToList();
            ShellList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            // iconList.ForEach(s=>Console.Write(s+"\n"));
            ShellList = ShellList.Distinct().ToList();
            ShellList.Sort();

            CursorList = CursorOutput.Split("\n").ToList();
            CursorList.RemoveAll(s => string.IsNullOrWhiteSpace(s));
            CursorList = CursorList.Distinct().ToList();
            CursorList.Sort();
            UserThemeExtensionExists = CheckUserThemeExtExists();
        }

        public void Reload() => Initalize();

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

        public String GetCursorTheme()
            => Bash("gsettings get  org.gnome.desktop.interface cursor-theme")
                .Replace("\'", "")
                .Trim();

        private string Bash(string cmd)
        {
            // NOTE: Is it necessary to create a new variable
            // just to use it once instead of overriding it?
            // Might wnat to change it to:
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

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
