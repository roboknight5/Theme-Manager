using Gtk;

namespace ThemeManager
{
    public class SettingPopOverMenu : PopoverMenu
    {
        public SettingPopOverMenu()
        {
            var appSettings = new AppSettings();
            var settingsData = appSettings.GetSettings();

            var darkMode = new CheckButton
            {
                Label = "Dark Mode",
                Active = settingsData.DarkModeEnabled,
                Toggled += (_, _) =>
                {
                    settingsData.DarkModeEnabled = !settingsData.DarkModeEnabled;
                    Settings.Default.ApplicationPreferDarkTheme = settingsData.DarkModeEnabled;
                    appSettings.SetSettings(settingsData);
                },
            };

            var vBox = new VBox { darkMode };
            vBox.ShowAll();
            vBox.Margin = 12;
            Add(vBox);
        }
    }
}