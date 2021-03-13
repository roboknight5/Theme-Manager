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
            => Initalize(currentMode);

        public void Reload() => Initalize(currentMode);

        private void Initalize(ThemeMode currentMode)
        {
            foreach (var widget in Children)
                Remove(widget);
            this.currentMode = currentMode;
            BashHandler bashHandler = BashHandler.Instance;

#if DEBUG
            Console.WriteLine(bashHandler.UserThemeExtensionExists);
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
#if DEBUG
                        Console.WriteLine(boxItem.itemName);
#endif
                    }

                    eventBox.ButtonPressEvent += (o, args) =>
                    {
                        box.UnselectAll();

#if DEBUG
                        Console.WriteLine(boxItem.itemName);
#endif

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
