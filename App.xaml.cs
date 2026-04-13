namespace Declaraties;

public partial class App : Application
{
    const string ThemePreferenceKey = "AppTheme";

    public App()
    {
        InitializeComponent();

        var saved = Preferences.Get(ThemePreferenceKey, "Light");
        ApplyTheme(saved);

        MainPage = new AppShell();
    }

    public void ApplyTheme(string themeName)
    {
        Application.Current.UserAppTheme = themeName switch
        {
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Light
        };

        Preferences.Set(ThemePreferenceKey, themeName);
    }
}

