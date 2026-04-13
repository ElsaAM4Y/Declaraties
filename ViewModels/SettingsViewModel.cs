using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Declaraties.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string selectedTheme;

    public ObservableCollection<string> Themes { get; } =
        new() { "Light", "Dark", "System" };

    public SettingsViewModel()
    {
        SelectedTheme = Preferences.Get("SelectedTheme", "System");
        ApplyTheme(SelectedTheme);
    }

    [RelayCommand]
    private async Task Save()
    {
        Preferences.Set("SelectedTheme", SelectedTheme);
        ApplyTheme(SelectedTheme);
    }

    private void ApplyTheme(string theme)
    {
        Application.Current.UserAppTheme = theme switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }
}
