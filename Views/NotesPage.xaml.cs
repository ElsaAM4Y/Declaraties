using Declaraties.ViewModels;
using Declaraties.Models;

namespace Declaraties.Views;

public partial class NotesPage : ContentPage
{
    private NotesViewModel Vm => (NotesViewModel)BindingContext;

    public NotesPage(NotesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!Vm.HasNotesLoaded)
            await Vm.LoadAsync();
    }

    private void OnNoteTapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border && border.BindingContext is NoteRecord note)
        {
            Vm.SelectedNote = note;
        }
    }
}
