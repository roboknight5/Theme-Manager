using System;
using System.Collections.Generic;
using System.IO;
using ThemeManager;

namespace Gtk_Theme_Manager
{
    public class LayoutHandler
    {
        private string PathToSettings=$"/home/{Environment.UserName}/.local/share/Theme Manager/";
        public List<LayoutItem> LayoutItems;
        private BashHandler BashHandler;
        public LayoutHandler()
        {
          BashHandler = BashHandler.Instance;
          LayoutItems = new List<LayoutItem>();
          if (!Directory.Exists(PathToSettings))
          {
              Directory.CreateDirectory(PathToSettings);

          }
          else
          {
              foreach (var file in Directory.EnumerateFiles(PathToSettings))
              {
                  string contents = File.ReadAllText(file);
                  string []text = contents.Split();
                  LayoutItem layoutItem = new LayoutItem(
                      text[0]
                      ,text[1]
                      ,text[2]
                      ,text[3]);
                  LayoutItems.Add(layoutItem);

              }
              
              
          }
        }

        public void ApplyLayout(LayoutItem layoutItem)
        {
            BashHandler.ChangeTheme(layoutItem.Theme);
            BashHandler.ChangeIcon(layoutItem.IconTheme);
            BashHandler.ChangeCursor(layoutItem.CursorTheme);
            BashHandler.ChangeShell(layoutItem.ShellTheme);
            ThemeUI.ResetSelection(layoutItem.Theme,ThemeMode.GtkTheme);
            ThemeUI.ResetSelection(layoutItem.IconTheme,ThemeMode.IconTheme);
            ThemeUI.ResetSelection(layoutItem.CursorTheme,ThemeMode.CursorTheme);
            ThemeUI.ResetSelection(layoutItem.ShellTheme,ThemeMode.ShellTheme);

        }
        public LayoutItem AddLayoutItem()
        {
            LayoutItem layoutItem = new LayoutItem(
                BashHandler.GetTheme(),
                BashHandler.GetIconTheme(),
                BashHandler.GetCursorTheme(),
                BashHandler.GetShellTheme());
            
            Console.WriteLine(LayoutItems.Count);
            if (LayoutItems.Count==0)
            {
                LayoutItems.Add(layoutItem);
                using (StreamWriter streamWriter = new StreamWriter(PathToSettings + $"Layout_0.txt"))
                {
                    streamWriter.WriteLine(layoutItem.Theme);
                    streamWriter.WriteLine(layoutItem.IconTheme);
                    streamWriter.WriteLine(layoutItem.CursorTheme);
                    streamWriter.WriteLine(layoutItem.ShellTheme);
                        
                }
                
                Console.WriteLine(layoutItem);
                return layoutItem;

            }
            
            int index = Directory.GetFiles(PathToSettings).Length;
            int count=0;
            foreach (var i in LayoutItems)
            {
                if (i.Theme == layoutItem.Theme && i.IconTheme == layoutItem.IconTheme &&
                    i.CursorTheme == layoutItem.CursorTheme && i.ShellTheme == layoutItem.ShellTheme)
                {
                    count++;
                }

            }

            if (count==0)
            {
                using (StreamWriter streamWriter = new StreamWriter(PathToSettings + $"Layout_{index}.txt"))
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
            foreach (var file in Directory.EnumerateFiles(PathToSettings))
            {
                string contents = File.ReadAllText(file);
                string []text = contents.Split();
                if (layoutItem.Theme==text[0]&&
                    layoutItem.IconTheme==text[1]&&
                    layoutItem.CursorTheme==text[2]&&
                    layoutItem.ShellTheme==text[3])
                {
                    File.Delete(file);
                    
                }
               
            }
            
            LayoutItems.Remove(layoutItem);
        }
       
    }
}