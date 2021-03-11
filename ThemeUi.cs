using System;
using System.Collections.Generic;
using Gtk;
namespace Gtk_Theme_Manager
{
    public class ThemeUi : ScrolledWindow
    {
        public ThemeMode currentMode;
        private List<String> currentArray;
        private String currentTheme;
        

        public ThemeUi(ThemeMode currentMode)
        {
            Initalize(currentMode);
            
        }

        public void Reload()
        {   
            // QueueDraw();
            Initalize(currentMode);
        }

        private void Initalize(ThemeMode currentMode)
        {

            foreach (var widget in Children)
            {
                Remove(widget);
                
            }
            this.currentMode = currentMode;
            BashHandler bashHandler = BashHandler.Instance;
            // Console.WriteLine(bashHandler.UserThemeExtensionExists);
            if (currentMode == ThemeMode.ShellTheme && !bashHandler.CheckUserThemeExtExists())
            {
                
                VBox v=new VBox();
                v.Add(new Label("Please Install The User Themes Extension"+Environment.NewLine+" to Use This Feature On Gnome"));
                Add(v);
                v.ShowAll();
                Show();
            }
            else
            {
                switch (currentMode)
                {
                    case ThemeMode.GtkTheme:
                        currentArray = bashHandler.ThemeList;
                        currentTheme = bashHandler.GetTheme();
                        break;
                    case ThemeMode.IconTheme:
                        currentArray = bashHandler.IconList;
                        currentTheme = bashHandler.GetIconTheme();
                        break;
                    case ThemeMode.ShellTheme:
                        currentArray = bashHandler.ShellList;
                        currentTheme = bashHandler.GetShellTheme();
                        break;
                    case ThemeMode.CursorTheme:
                        currentArray = bashHandler.CursorList;
                        currentTheme = bashHandler.GetCursorTheme();
                        break;

                }

                ListBox box = new ListBox();
                RadioButton radioButton = new RadioButton("");
                box.SelectionMode = SelectionMode.None;
                foreach (var theme in currentArray)
                {
                    ListBoxRow row = new ListBoxRow();
                    EventBox eventBox = new EventBox();
                    BoxItem boxItem = new BoxItem(theme, radioButton);
                    row.Child = boxItem;
                    eventBox.Add(row);
                    if (currentTheme == boxItem.itemName)
                    {

                        box.UnselectAll();
                        box.SelectionMode = SelectionMode.Single;
                        boxItem.radioButton.Active = true;

                        // Console.WriteLine(boxItem.itemName);

                    }

                    eventBox.ButtonPressEvent += (o, args) =>
                    {
                        box.UnselectAll();
                        // Console.WriteLine(boxItem.itemName);
                        boxItem.radioButton.Active = true;
                        switch (currentMode)
                        {
                            case ThemeMode.GtkTheme:
                                bashHandler.ChangeTheme(boxItem.itemName);
                                break;
                            case ThemeMode.IconTheme:
                                bashHandler.ChangeIcon(boxItem.itemName);
                                break;
                            case ThemeMode.ShellTheme:
                                bashHandler.ChangeShell(boxItem.itemName);
                                break;
                            case ThemeMode.CursorTheme:
                                bashHandler.ChangeCursor(boxItem.itemName);
                                break;
                        }
                    };


                    box.Add(eventBox);

                }

                box.ShowAll();
                Add(box);
                Show();
            }

            
        }
        
    }
}