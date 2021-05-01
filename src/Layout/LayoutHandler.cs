using System;
using System.Collections.Generic;
using System.IO;
using ThemeManager;

namespace ThemeManager.Layout
{
    public class LayoutHandler
    {
        private readonly string _pathToSettings = $"/home/{Environment.UserName}/.local/share/Theme Manager/"; 
        private readonly BashHandler _bashHandler;
        
        public LayoutHandler()
        { 
            _bashHandler = BashHandler.Instance;
            LayoutItems = new List<LayoutItem>();
            if (!Directory.Exists(_pathToSettings))
            {
                Directory.CreateDirectory(_pathToSettings);
            }
            else
            {
                foreach (var file in Directory.EnumerateFiles(_pathToSettings))
                {
                    if (file != _pathToSettings+"settings.json")
                    {
                        var contents = File.ReadAllText(file);
                        var text = contents.Split();
                        var layoutItem = new LayoutItem(
                            text[0], 
                            text[1],
                            text[2],
                            text[3]);
                        LayoutItems.Add(layoutItem);
                    }
                }
            }
        }

        public List<LayoutItem> LayoutItems { get; }

        public void ApplyLayout(LayoutItem layoutItem)
        {
            _bashHandler.ChangeTheme(layoutItem.Theme);
            _bashHandler.ChangeIcon(layoutItem.IconTheme);
            _bashHandler.ChangeCursor(layoutItem.CursorTheme);
            _bashHandler.ChangeShell(layoutItem.ShellTheme);
            
            ThemeUI.ResetSelection(layoutItem.Theme,ThemeMode.GtkTheme);
            ThemeUI.ResetSelection(layoutItem.IconTheme,ThemeMode.IconTheme);
            ThemeUI.ResetSelection(layoutItem.CursorTheme,ThemeMode.CursorTheme);
            ThemeUI.ResetSelection(layoutItem.ShellTheme,ThemeMode.ShellTheme);

        }
        public LayoutItem AddLayoutItem()
        {
            var layoutItem = new LayoutItem(
                _bashHandler.GetTheme(),
                _bashHandler.GetIconTheme(),
                _bashHandler.GetCursorTheme(),
                _bashHandler.GetShellTheme());
            
            var index = Directory.GetFiles(_pathToSettings).Length;
            var count = 0;
            foreach (var i in LayoutItems)
            {
                if (i.Theme == layoutItem.Theme && 
                    i.IconTheme == layoutItem.IconTheme &&
                    i.CursorTheme == layoutItem.CursorTheme && 
                    i.ShellTheme == layoutItem.ShellTheme)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                using (var streamWriter = new StreamWriter(_pathToSettings + $"Layout_{index}.txt"))
                {
                    streamWriter.WriteLine(layoutItem.Theme);
                    streamWriter.WriteLine(layoutItem.IconTheme);
                    streamWriter.WriteLine(layoutItem.CursorTheme);
                    streamWriter.WriteLine(layoutItem.ShellTheme);
                        
                }
                LayoutItems.Add(layoutItem);
                index++;
                Console.WriteLine(layoutItem);
                   
                return layoutItem;
            }

            return null;
        }

        public void DeleteLayout(LayoutItem layoutItem)
        {
            foreach (var file in Directory.EnumerateFiles(_pathToSettings))
            {
                var contents = File.ReadAllText(file);
                var text = contents.Split();
                if (layoutItem.Theme==text[0] &&
                    layoutItem.IconTheme==text[1] &&
                    layoutItem.CursorTheme==text[2] &&
                    layoutItem.ShellTheme==text[3])
                {
                    File.Delete(file);
                }
            }
            
            LayoutItems.Remove(layoutItem);
        }
    }
}