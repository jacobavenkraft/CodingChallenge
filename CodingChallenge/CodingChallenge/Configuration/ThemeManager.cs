using System;
using System.Windows;
using WK.Libraries.WTL;

namespace CodingChallenge.Configuration
{
    public enum ThemeMode
    {
        Dark,
        Light,
        Auto
    }

    public class ThemeManager
    {
        private static ThemeMode mode = ThemeMode.Auto;

        public static ThemeMode ConfiguredMode
        {
            get => mode;
            set
            {
                if (mode == value)
                {
                    return;
                }
                mode = value;
                RefreshTheme();
            }
        }

        public static ThemeMode ActualMode
        {
            get
            {
                if (ConfiguredMode == ThemeMode.Auto)
                {
                    switch (ThemeListener.AppMode)
                    {
                        case ThemeListener.ThemeModes.Dark:
                            return ThemeMode.Dark;
                        default:
                            return ThemeMode.Light;
                    }
                }
                return ConfiguredMode;
            }
        }

        public static void Initialize()
        {
            mode = Settings.Instance.CurrentTheme;
            ThemeListener.Enabled = true;
            ThemeListener.ThemeSettingsChanged -= ThemeListener_ThemeSettingsChanged;
            ThemeListener.ThemeSettingsChanged += ThemeListener_ThemeSettingsChanged;
            RefreshTheme();
        }

        private static void ThemeListener_ThemeSettingsChanged(object sender, ThemeListener.ThemeSettingsChangedEventArgs e)
        {
            RefreshTheme();
        }

        private static bool IsThemeUri(Uri uri)
        {
            var uriString = uri?.OriginalString ?? string.Empty;

            if(uriString == Settings.Instance.DarkThemeUri
            || uriString == Settings.Instance.LightThemeUri)
            {
                return true;
            }

            return false;
        }

        public static void RefreshTheme()
        {
            Settings.Instance.CurrentTheme = ConfiguredMode;

            string uriString;

            switch (ActualMode)
            {
                case ThemeMode.Dark:
                    uriString = Settings.Instance.DarkThemeUri;
                    break;
                default:
                    uriString = Settings.Instance.LightThemeUri;
                    break;
            }

            var uri = new Uri(uriString, UriKind.Relative);

            ResourceDictionary newThemeResource = Application.LoadComponent(uri) as ResourceDictionary;
            newThemeResource.Source = uri;

            ResourceDictionary oldThemeResource = null;

            foreach (var mergedDictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (IsThemeUri(mergedDictionary.Source))
                {
                    oldThemeResource = mergedDictionary;
                }
            }

            Application.Current.Resources.MergedDictionaries.Add(newThemeResource);
            if (oldThemeResource != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(oldThemeResource);
            }
        }
    }
}
