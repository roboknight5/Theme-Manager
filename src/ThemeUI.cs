using System;
using System.Collections.Generic;
using Gtk;

namespace ThemeManager
{
    public class ThemeUI : ScrolledWindow
    {
        private ThemeMode _currentMode;
        private List<string> _currentArray;
        private string _currentTheme;

        public ThemeUI(ThemeMode currentMode)
            => Initialize(currentMode);

        public void Reload() => Initialize(_currentMode);

        private void Initialize(ThemeMode currentMode)
        {
            foreach (var widget in Children)
            {
                Remove(widget);
            }
            
            _currentMode = currentMode;
            var bashHandler = BashHandler.Instance;

#if DEBUG
            Console.WriteLine(bashHandler.UserThemeExtensionExists);
#endif

            if (currentMode == ThemeMode.ShellTheme && !bashHandler.CheckUserThemeExtExists())
            {
                var vBox = new VBox
                {
                    new Label($"Please Install The User Themes Extension{Environment.NewLine} to Use This Feature On Gnome")
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
                        _currentArray = bashHandler.ThemeList;
                        _currentTheme = bashHandler.GetTheme();
                        break;

                    case ThemeMode.IconTheme:
                        _currentArray = bashHandler.IconList;
                        _currentTheme = bashHandler.GetIconTheme();
                        break;

                    case ThemeMode.ShellTheme:
                        _currentArray = bashHandler.ShellList;
                        _currentTheme = bashHandler.GetShellTheme();
                        break;

                    case ThemeMode.CursorTheme:
                        _currentArray = bashHandler.CursorList;
                        _currentTheme = bashHandler.GetCursorTheme();
                        break;
                }

                var box = new ListBox();

                var radioButton = new RadioButton("");
                box.SelectionMode = SelectionMode.None;

                foreach (var theme in _currentArray)
                {
                    var row = new ListBoxRow();
                    var eventBox = new EventBox();
                    var boxItem = new BoxItem(theme, radioButton);

                    row.Child = boxItem;
                    eventBox.Add(row);

                    if (_currentTheme == boxItem.ItemName)
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
