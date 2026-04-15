namespace Declaraties;

public partial class App : Application
{
    private const string ThemePreferenceKey = "AppTheme";

    public App()
    {
        InitializeComponent();

        // Load saved theme or default to Light
        var savedTheme = Preferences.Get(ThemePreferenceKey, "Light");
        ApplyTheme(savedTheme);

        MainPage = new AppShell();
    }

    public void ApplyTheme(string themeName)
    {
        // Prevent unnecessary theme re-application
        var current = Preferences.Get(ThemePreferenceKey, "Light");
        if (current == themeName)
            return;

        // Apply theme
        Application.Current.UserAppTheme = themeName switch
        {
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Light
        };

        // Save preference
        Preferences.Set(ThemePreferenceKey, themeName);
    }
}
