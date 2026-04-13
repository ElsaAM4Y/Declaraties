using SQLite;
using Declaraties.Models;

namespace Declaraties.Services;

public interface IMonthRecordRepository
{
    Task InitializeAsync();
    Task<List<MonthRecord>> GetForMonthAsync(int year, int month);
    Task<MonthRecord?> GetByIdAsync(int id);
    Task InsertAsync(MonthRecord record);
    Task UpdateAsync(MonthRecord record);
    Task DeleteAsync(MonthRecord record);
    Task SaveAllAsync(IEnumerable<MonthRecord> records);
    Task<List<MonthRecord>> GetAllAsync();
}

public class MonthRecordRepository : IMonthRecordRepository
{
    private SQLiteAsyncConnection _db;

    public async Task InitializeAsync()
    {
        if (_db != null) return;

        var path = Path.Combine(FileSystem.AppDataDirectory, "reiskosten.db3");
        _db = new SQLiteAsyncConnection(path);

        await _db.CreateTableAsync<MonthRecord>();
    }

    public async Task<List<MonthRecord>> GetForMonthAsync(int year, int month)
    {
        await InitializeAsync();

        // ⭐ FIX: SQLite cannot evaluate Date.Year or Date.Month
        // So we filter using a BETWEEN range instead.
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        return await _db.Table<MonthRecord>()
            .Where(x => x.Date >= start && x.Date < end)
            .OrderBy(x => x.Date)
            .ToListAsync();
    }

    public async Task<MonthRecord?> GetByIdAsync(int id)
    {
        await InitializeAsync();
        return await _db.FindAsync<MonthRecord>(id);
    }

    public async Task InsertAsync(MonthRecord record)
    {
        await InitializeAsync();
        await _db.InsertAsync(record);
    }

    public async Task UpdateAsync(MonthRecord record)
    {
        await InitializeAsync();
        await _db.UpdateAsync(record);
    }

    public async Task DeleteAsync(MonthRecord record)
    {
        await InitializeAsync();
        await _db.DeleteAsync(record);
    }

    public async Task SaveAllAsync(IEnumerable<MonthRecord> records)
    {
        await InitializeAsync();

        foreach (var r in records)
        {
            if (r.Id == 0)
                await _db.InsertAsync(r);
            else
                await _db.UpdateAsync(r);
        }
    }

    public async Task<List<MonthRecord>> GetAllAsync()
    {
        await InitializeAsync();
        return await _db.Table<MonthRecord>().ToListAsync();
    }
}
