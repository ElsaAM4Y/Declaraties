using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Declaraties.Models;
using Declaraties.Services;
using System.Collections.ObjectModel;

namespace Declaraties.ViewModels;

public partial class NotesViewModel : ObservableObject
{
    private readonly INotesRepository _repo;
    private bool _isLoaded;

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string content = string.Empty;
    [ObservableProperty] private NoteRecord? selectedNote;
    [ObservableProperty] private ObservableCollection<NoteRecord> notes = new();

    public NotesViewModel(INotesRepository repo)
    {
        _repo = repo;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (_isLoaded)
            return;

        _isLoaded = true;

        var list = await _repo.GetNotesAsync();

        Notes.Clear();
        foreach (var n in list.OrderByDescending(n => n.Updated))
            Notes.Add(n);
    }

    [RelayCommand]
    public void NewNote()
    {
        SelectedNote = null;
        Title = string.Empty;
        Content = string.Empty;
    }

    partial void OnSelectedNoteChanged(NoteRecord? value)
    {
        if (value == null)
        {
            Title = string.Empty;
            Content = string.Empty;
            return;
        }

        Title = value.Title;
        Content = value.Content;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        NoteRecord note = SelectedNote ?? new NoteRecord { Created = DateTime.UtcNow };

        note.Title = Title;
        note.Content = Content;
        note.Updated = DateTime.UtcNow;

        await _repo.SaveNoteAsync(note);

        _isLoaded = false;
        await LoadAsync();

        SelectedNote = null;
        Title = string.Empty;
        Content = string.Empty;
    }

    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (SelectedNote == null)
            return;

        await _repo.DeleteNoteAsync(SelectedNote);

        _isLoaded = false;
        await LoadAsync();

        SelectedNote = null;
        Title = string.Empty;
        Content = string.Empty;
    }
}

