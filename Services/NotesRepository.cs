using Declaraties.Models;
using SQLite;

namespace Declaraties.Services;

public interface INotesRepository
{
    Task InitializeAsync();
    Task<List<NoteRecord>> GetNotesAsync();
    Task SaveNoteAsync(NoteRecord note);
    Task DeleteNoteAsync(NoteRecord note);
}

public class NotesRepository : INotesRepository
{
    private SQLiteAsyncConnection? _db;

    public async Task InitializeAsync()
    {
        if (_db != null) return;

        var path = Path.Combine(FileSystem.AppDataDirectory, "notes.db3");
        _db = new SQLiteAsyncConnection(path);

        await _db.CreateTableAsync<NoteRecord>();
    }

    public async Task<List<NoteRecord>> GetNotesAsync()
    {
        await InitializeAsync();

        return await _db!
            .Table<NoteRecord>()
            .OrderByDescending(n => n.Updated)   // ⭐ latest first
            .ToListAsync();
    }

    public async Task SaveNoteAsync(NoteRecord note)
    {
        await InitializeAsync();

        if (note.Id == 0)
            await _db!.InsertAsync(note);
        else
            await _db!.UpdateAsync(note);
    }

    public async Task DeleteNoteAsync(NoteRecord note)
    {
        await InitializeAsync();
        await _db!.DeleteAsync(note);
    }
}
