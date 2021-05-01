using System;
using System.Collections.Generic;
using Gtk;
using ThemeManager;

namespace ThemeManager
{

    public class ThemeUI : ScrolledWindow
    {
        private ThemeMode _currentMode;
        private List<string> _currentArray;
        private string _currentTheme;
        
        private static ListBox _box;
        private static readonly List<BoxItem> _boxItems = new();

        public ThemeUI(ThemeMode currentMode) => Initialize(currentMode);

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
            // Console.WriteLine(bashHandler.UserThemeExtensionExists);
            #endif

            if (currentMode == ThemeMode.ShellTheme && !bashHandler.CheckUserThemeExtExists())
            {
                var vBox = new VBox
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
                
                var radioButton = new RadioButton("");
                _box = new ListBox
                {
                    SelectionMode = SelectionMode.None,
                };

                foreach (var theme in _currentArray)
                {
                    var row = new ListBoxRow();
                    var eventBox = new EventBox();
                    var boxItem = new BoxItem(theme, radioButton,currentMode);
                    _boxItems.Add(boxItem);

                    row.Child = boxItem;
                    eventBox.Add(row);
                    
                    if (_currentTheme == boxItem.ItemName)
                    {
                        _box.UnselectAll();
                        boxItem.RadioButton.Active = true;
                        #if DEBUG
                        // Console.WriteLine(boxItem.ItemName);
                        #endif
                    }

                    boxItem.RadioButton.Clicked += (_, _) =>
                    {
                        _box.UnselectAll();
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
                        
                    eventBox.ButtonPressEvent += (_, _) =>
                    {
                        _box.UnselectAll();
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
                    _box.Add(eventBox);
                }
                _box.ShowAll();
                Add(_box);
                Show();
            }
        }

        public static void ResetSelection(string name,ThemeMode type)
        {
            _box.UnselectAll();
            foreach (var boxItem in _boxItems)
            {
                if (boxItem.ItemName == name && boxItem.ItemType == type)
                {
                    boxItem.RadioButton.Active = true;
                    break;
                }
            }
        }
    }
}
