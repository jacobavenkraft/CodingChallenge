using CodingChallenge.Extensions;
using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace CodingChallenge.Configuration
{
    public class Settings
    {
        private static Settings settings;
        
        public static Settings Instance => settings ?? Initialize();

        private static Settings Initialize()
        {
            settings = new Settings();
            return settings;
        }

        protected Settings()
        {
        }

        protected string ReadSetting([CallerMemberName] string settingName = "")
        {
            if (string.IsNullOrWhiteSpace(settingName))
            {
                return string.Empty;
            }

            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[settingName] ?? string.Empty;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine($"Unexpected Error Reading: {settingName}" + Environment.NewLine + e.ToString());
            }

            return default;
        }

        protected void WriteSetting(string value, [CallerMemberName] string settingName = "")
        {
            if (string.IsNullOrWhiteSpace(settingName))
            {
                return;
            }

            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[settingName] == null)
                {
                    settings.Add(settingName, value);
                }
                else
                {
                    settings[settingName].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine($"Unexpected Error Writing: {settingName}" + Environment.NewLine + e.ToString());
            }
        }

        public int RoamingSpeed
        {
            get
            {
                if(!int.TryParse(ReadSetting(), out var settingValue))
                {
                    return 1;
                }
                return settingValue;
            }
            set => WriteSetting(value.ToString());
        }

        public string ImageUri { get => ReadSetting(); set => WriteSetting(value); }

        public string DarkThemeUri => ReadSetting();

        public string LightThemeUri => ReadSetting();

        public ThemeMode CurrentTheme { get => ReadSetting().ToThemeMode(); set => WriteSetting(value.ToString()); }
    }
}
