using System;
using System.Collections.Generic;
using Gtk;

namespace ThemeManager
{
    public class ThemeUI : ScrolledWindow
    {
        public ThemeMode CurrentMode;
        private List<String> currentArray;
        private String currentTheme;

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

                    if (currentTheme == boxItem.ItemName)
                    {
                        box.UnselectAll();
                        box.SelectionMode = SelectionMode.Single;
                        boxItem.RadioButton.Active = true;
#if DEBUG
                        Console.WriteLine(boxItem.ItemName);
#endif
                    }

                    eventBox.ButtonPressEvent += (o, args) =>
                    {
                        box.UnselectAll();

#if DEBUG
                        Console.WriteLine(boxItem.ItemName);
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
                    box.Add(eventBox);
                }
                box.ShowAll();
                Add(box);
                Show();
            }
        }
    }
}
