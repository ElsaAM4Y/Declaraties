using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Declaraties.Models;
using Declaraties.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Declaraties.ViewModels;

public partial class TotalsViewModel : ObservableObject
{
    private readonly IMonthRecordRepository _repo;
    private bool _isLoaded;

    [ObservableProperty]
    private ObservableCollection<TotalSummary> monthTotals = new();

    public TotalsViewModel(IMonthRecordRepository repo)
    {
        _repo = repo;
    }

    public void Reset()
    {
        _isLoaded = false;
    }

    [RelayCommand]
    public async Task LoadTotalsAsync()
    {
        Debug.WriteLine($"TotalsViewModel.LoadTotalsAsync called. Loaded={_isLoaded}");

        if (_isLoaded)
            return;

        _isLoaded = true;

        MonthTotals.Clear();

        var allDays = await _repo.GetAllAsync();
        Debug.WriteLine($"Total days loaded from DB: {allDays.Count}");

        var summaries =
            allDays
            .GroupBy(d => new { d.Date.Year, d.Date.Month })
            .Select(g => new TotalSummary
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalTimesWorking = g.Sum(x => x.DayTimesWorking),
                Rate = g.First().RatePerDay,
                TotalAmount = g.Sum(x => x.DayTimesWorking) * g.First().RatePerDay
            })
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToList();

        Debug.WriteLine($"Summaries created: {summaries.Count}");

        foreach (var s in summaries)
            MonthTotals.Add(s);

        Debug.WriteLine($"MonthTotals.Count after fill: {MonthTotals.Count}");
    }
}
