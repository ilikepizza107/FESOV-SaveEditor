using System;

namespace Theme.WPF.Themes
{
    public enum ThemeType
    {
        DarkTheme,
        LightTheme,
    }

    public static class ThemeTypeExtension
    {
        public static string GetName(this ThemeType type)
        {
            switch (type)
            {
                case ThemeType.DarkTheme: return "DarkTheme";
                case ThemeType.LightTheme: return "LightTheme";
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}