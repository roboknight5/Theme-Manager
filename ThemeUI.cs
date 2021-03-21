using System;
using System.Collections.Generic;
using Gtk;
using Gtk_Theme_Manager;

namespace ThemeManager
{

    public class ThemeUI : ScrolledWindow
    {
        public ThemeMode CurrentMode;
        private List<String> currentArray;
        private String currentTheme;
        static private ListBox Box;
        static private List<BoxItem> BoxItems=new List<BoxItem>();

        public ThemeUI(ThemeMode currentMode)
            => Initalize(currentMode);

        public void Reload() => Initalize(CurrentMode);

        private void Initalize(ThemeMode currentMode)
        {
           
            foreach (var widget in Children)
                Remove(widget);
            this.CurrentMode = currentMode;
            BashHandler bashHandler = BashHandler.Instance;

#if DEBUG
            // Console.WriteLine(bashHandler.UserThemeExtensionExists);
#endif

            if (currentMode == ThemeMode.ShellTheme && !bashHandler.CheckUserThemeExtExists())
            {
                VBox vBox = new VBox
                {
                    new Label("Please Install The User Themes Extension"
                        + Environment.NewLine
                        + " to Use This Feature On Gnome")
                };
                Add(vBox);
                vBox.ShowAll();
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

                 

                RadioButton radioButton = new RadioButton("");
                Box = new ListBox();
                Box.SelectionMode = SelectionMode.None;
                // Box.SelectionMode = SelectionMode.None;

                foreach (var theme in currentArray)
                {
                    ListBoxRow row = new ListBoxRow();
                    EventBox eventBox = new EventBox();
                    BoxItem boxItem = new BoxItem(theme, radioButton,currentMode);
                    BoxItems.Add(boxItem);

                    row.Child = boxItem;
                    eventBox.Add(row);
                    

                    if (currentTheme == boxItem.ItemName)
                    {
                        Box.UnselectAll();
                        // Box.SelectionMode = SelectionMode.Single;
                        boxItem.RadioButton.Active = true;
#if DEBUG
                        // Console.WriteLine(boxItem.ItemName);
#endif
                    }

                    boxItem.RadioButton.Clicked += (sender, args) =>
                    {
                        Box.UnselectAll();

#if DEBUG
                        // Console.WriteLine(boxItem.ItemName);
#endif

                        switch (currentMode)
                        {
                            case ThemeMode.GtkTheme:
                                bashHandler.ChangeTheme(boxItem.ItemName);
                                break;

                            case ThemeMode.IconTheme:
                                bashHandler.ChangeIcon(boxItem.ItemName);
                                break;

                            case ThemeMode.ShellTheme:
                                bashHandler.ChangeShell(boxItem.ItemName);
                                break;

                            case ThemeMode.CursorTheme:
                                bashHandler.ChangeCursor(boxItem.ItemName);
                                break;
                            
                        }
                        
                    };
                        
                    eventBox.ButtonPressEvent += (o, args) =>
                    {
                        Box.UnselectAll();

#if DEBUG
                        // Console.WriteLine(boxItem.ItemName);
#endif

                        boxItem.RadioButton.Active = true;
                        switch (currentMode)
                        {
                            case ThemeMode.GtkTheme:
                                bashHandler.ChangeTheme(boxItem.ItemName);
                                break;

                            case ThemeMode.IconTheme:
                                bashHandler.ChangeIcon(boxItem.ItemName);
                                break;

                            case ThemeMode.ShellTheme:
                                bashHandler.ChangeShell(boxItem.ItemName);
                                break;

                            case ThemeMode.CursorTheme:
                                bashHandler.ChangeCursor(boxItem.ItemName);
                                break;
                            
                        }
                    };
                    Box.Add(eventBox);
                }
                Box.ShowAll();
                Add(Box);
                Show();
            }
        }

        public static void ResetSelection(string name,ThemeMode type)
        {
            Box.UnselectAll();
            foreach (var boxItem in BoxItems)
            {
                if (boxItem.ItemName==name&&boxItem.ItemType==type)
                {
                    boxItem.RadioButton.Active = true;
                    break;

                }
                
            }
            
            

        }
    }
}
