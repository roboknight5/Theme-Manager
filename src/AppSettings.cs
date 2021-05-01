using System;
using System.IO;
using System.Text.Json;

namespace ThemeManager
{
    public class AppSettings
    {
        private readonly string  _pathToSettings=$"/home/{Environment.UserName}/.local/share/Theme Manager/";
        private const string _settingsName = "settings.json";
        
        public AppSettings()
        {
            if (!Directory.Exists(_pathToSettings))
            {
                try
                {
                    Directory.CreateDirectory(_pathToSettings);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.Out.WriteLine("Unable to create directory.");
                    throw;
                }
            }
            
            else
            {
                if (!File.Exists(_pathToSettings + _settingsName))
                {
                    var jsonString = JsonSerializer.Serialize(new SettingsData());
                    File.WriteAllText(_pathToSettings + _settingsName,jsonString);
                }
            }
        }

        public SettingsData GetSettings()
        {
            try
            {
                var jsonString = File.ReadAllText(_pathToSettings + _settingsName);

                var data = JsonSerializer.Deserialize<SettingsData>(jsonString);
                return data;
            }
            catch (JsonException)
            {
                Console.Out.WriteLine("Error while getting settings.");
                throw;
            }
            catch (FileNotFoundException)
            {
                Console.Out.WriteLine("Error while getting settings.");
                throw;
            }
        }

        public void SetSettings(SettingsData settingsData)
        {
            var jsonString = JsonSerializer.Serialize(settingsData);
            File.WriteAllText(_pathToSettings+_settingsName,jsonString);
        }
    }
}