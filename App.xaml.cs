
namespace Declaraties;

public partial class App : Application
{
    private const string ThemePreferenceKey = "AppTheme";

    public App()
    {
        InitializeComponent();

        var savedTheme = Preferences.Get(ThemePreferenceKey, "Light");
        ApplyTheme(savedTheme);

        MainPage = new AppShell();
    }

    public void ApplyTheme(string themeName)
    {
        // Clear current theme dictionaries, keep styles added in XAML
        Resources.MergedDictionaries.Clear();

        // Add the correct theme dictionary
        if (themeName == "Dark")
        {
            Resources.MergedDictionaries.Add(new Resources.Styles.DarkTheme());
        }
        else
        {
            Resources.MergedDictionaries.Add(new Resources.Styles.LightTheme());
        }

        // Re‑add global styles
        Resources.MergedDictionaries.Add(new Resources.Styles.GlobalStyles());

        // Optional: keep MAUI's AppTheme in sync
        Application.Current.UserAppTheme = themeName == "Dark"
            ? AppTheme.Dark
            : AppTheme.Light;

        Preferences.Set(ThemePreferenceKey, themeName);
    }
}
