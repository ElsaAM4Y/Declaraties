using Declaraties.ViewModels;
using System.Diagnostics;

namespace Declaraties.Views;

public partial class TotalsPage : ContentPage
{
    public TotalsPage(TotalsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("TotalsPage OnAppearing");

        if (BindingContext is TotalsViewModel vm)
            await vm.LoadTotalsAsync();
    }
}
