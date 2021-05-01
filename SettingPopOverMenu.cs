using System;
using System.IO;
using System.Text.Json;
using Gtk;

namespace ThemeManager
{
    public class SettingPopOverMenu : PopoverMenu
    {
        
        public SettingPopOverMenu()
        {
            AppSettings appSettings=new AppSettings();
            SettingsData settingsData = appSettings.GetSettings();
            

            
            CheckButton darkMode=new CheckButton();
            darkMode.Label = "Dark Mode";
            darkMode.Active = settingsData.DarkModeEnabled;
            darkMode.Toggled += (sender, args) =>
            {
                settingsData.DarkModeEnabled = !settingsData.DarkModeEnabled;
                Settings.Default.ApplicationPreferDarkTheme = settingsData.DarkModeEnabled;
                appSettings.SetSettings(settingsData);
            };
            
            
            
            VBox vBox= new VBox();
            vBox.Add(darkMode);
            vBox.ShowAll();
            vBox.Margin = 12;
            Add(vBox);
            
        }
    }
}