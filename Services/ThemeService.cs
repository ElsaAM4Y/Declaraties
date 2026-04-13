using Microsoft.Maui.Controls;

namespace Declaraties.Services;

public static class ThemeService
{
    public static void SetTheme(string theme)
    {
        Preferences.Set("AppTheme", theme);

        Application.Current.UserAppTheme =
            theme == "Dark" ? AppTheme.Dark : AppTheme.Light;
    }
}
