using System;
using System.IO;
using System.Text.Json;

namespace ThemeManager
{
    public class SettingsData
    {
        public bool DarkModeEnabled { get; set; } = true;


    }
    public class AppSettings
    {
        private readonly string  _pathToSettings=$"/home/{Environment.UserName}/.local/share/Theme Manager/";
        private readonly string _settingsName = "settings.json";
        public AppSettings()
        {
            if (!Directory.Exists(_pathToSettings))
            {
                Directory.CreateDirectory(_pathToSettings);
                
            }
            else
            {
                if (!File.Exists(_pathToSettings+_settingsName))
                {
                    string jsonString = JsonSerializer.Serialize(new SettingsData());
                    File.WriteAllText(_pathToSettings+_settingsName,jsonString);

                    
                }
            }
        }

        public SettingsData GetSettings()
        {
            string jsonString = File.ReadAllText(_pathToSettings + _settingsName);
            SettingsData data = JsonSerializer.Deserialize<SettingsData>(jsonString);
            return data;

        }

        public void SetSettings(SettingsData settingsData)
        {
            string jsonString = JsonSerializer.Serialize(settingsData);
            File.WriteAllText(_pathToSettings+_settingsName,jsonString);

            
        }
    }
    
}