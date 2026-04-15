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
}
