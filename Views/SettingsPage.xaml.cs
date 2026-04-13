using Declaraties.ViewModels;

namespace Declaraties.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel _vm;

    public SettingsPage()
    {
        InitializeComponent();
        _vm = new SettingsViewModel();
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Reload the selected theme from Preferences
        _vm.SelectedTheme = Preferences.Get("AppTheme", "Light");

        Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
    }
}
