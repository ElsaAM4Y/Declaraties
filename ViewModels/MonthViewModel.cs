using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Declaraties.Models;
using Declaraties.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Declaraties.ViewModels
{
    public partial class MonthViewModel : ObservableObject
    {
        private readonly IMonthRecordRepository _repo;
        private readonly TotalsViewModel totalsVM;

        private bool _isLoaded;
        private bool _isRestoringState;

        public List<string> Months { get; } =
            new() { "Jan", "Feb", "Mrt", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" };

        public List<int> Years { get; } =
            Enumerable.Range(DateTime.Now.Year - 10, 20).ToList();

        [ObservableProperty] private int year;
        [ObservableProperty] private int month; // zero-based index
        [ObservableProperty] private decimal ratePerDay;
        [ObservableProperty] private ObservableCollection<MonthRecord> records = new();
        [ObservableProperty] private string totalsText;

        public MonthViewModel(IMonthRecordRepository repo, TotalsViewModel totalsVM)
        {
            _repo = repo;
            this.totalsVM = totalsVM;
        }

        private string RateKey => $"Rate_{Year}_{Month + 1}";

        // ⭐ Save month/year state
        public void SaveState()
        {
            Preferences.Set("State_Year", Year);
            Preferences.Set("State_Month", Month);
        }

        // ⭐ Restore month/year state (NO triggers)
        public void RestoreState()
        {
            _isRestoringState = true;

            Year = Preferences.Get("State_Year", DateTime.Now.Year);
            Month = Preferences.Get("State_Month", DateTime.Now.Month - 1);

            _isRestoringState = false;
        }

        private void ResetMonth()
        {
            Records.Clear();
            TotalsText = "Aantal: 0 | Totaal: € 0,00";
        }

        // ⭐ Month changed
        partial void OnMonthChanged(int value)
        {
            if (_isRestoringState) return;

            SaveState();
            _isLoaded = false;
            ResetMonth();
            LoadAsync();
        }

        // ⭐ Year changed
        partial void OnYearChanged(int value)
        {
            if (_isRestoringState) return;

            SaveState();
            _isLoaded = false;
            ResetMonth();
            LoadAsync();
        }

        // ⭐ Load month data
        [RelayCommand]
        public async Task LoadAsync()
        {
            if (_isLoaded)
                return;

            _isLoaded = true;

            // Load rate
            var raw = Preferences.Get(RateKey, "1,23");
            if (!decimal.TryParse(raw, NumberStyles.Any, new CultureInfo("nl-NL"), out var parsed))
                parsed = 1.23m;

            RatePerDay = parsed;

            int realMonth = Month + 1;

            // Load records
            var list = await _repo.GetForMonthAsync(Year, realMonth);

            // ⭐ If no records → generate empty month
            if (list == null || list.Count == 0)
            {
                int days = DateTime.DaysInMonth(Year, realMonth);

                list = Enumerable.Range(1, days)
                    .Select(d => new MonthRecord
                    {
                        Date = new DateTime(Year, realMonth, d),
                        DayTimesWorking = 0,
                        DayNote = "",
                        RatePerDay = RatePerDay
                    })
                    .ToList();
            }

            Records.Clear();
            foreach (var item in list)
                Records.Add(item);

            UpdateTotals();
        }

        private void UpdateTotals()
        {
            int totalTimes = Records.Sum(r => r.DayTimesWorking);
            decimal totalAmount = totalTimes * RatePerDay;

            TotalsText = $"Aantal: {totalTimes} | Totaal: € {totalAmount:F2}";
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            SaveState();

            Preferences.Set(RateKey, RatePerDay.ToString(new CultureInfo("nl-NL")));

            foreach (var r in Records)
                r.RatePerDay = RatePerDay;

            await _repo.SaveAllAsync(Records);

            UpdateTotals();

            totalsVM.Reset();

            _isLoaded = false;
        }
    }
}
