using Declaraties.ViewModels;

namespace Declaraties.Views;

public partial class MonthPage : ContentPage
{
    public MonthPage(MonthViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MonthViewModel vm)
        {
            vm.RestoreState();   // ⭐ juiste plek
            await vm.LoadAsync();
        }
    }
}
