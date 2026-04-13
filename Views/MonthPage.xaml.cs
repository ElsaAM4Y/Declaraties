using Declaraties.ViewModels;
using System.Diagnostics;

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
        Debug.WriteLine("MonthPage OnAppearing");
        base.OnAppearing();

        if (BindingContext is MonthViewModel vm)
            await vm.LoadAsync();
    }
}
