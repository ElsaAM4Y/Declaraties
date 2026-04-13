using Declaraties.ViewModels;
using System.Diagnostics;

namespace Declaraties.Views;

public partial class NotesPage : ContentPage
{
    public NotesPage(NotesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        Debug.WriteLine("NotesPage OnAppearing");
        base.OnAppearing();

        if (BindingContext is NotesViewModel vm)
            await vm.LoadAsync();
    }
}

