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

        // ✔ Months list restored
        public List<string> Months { get; } =
            new() { "Jan", "Feb", "Mrt", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" };

        // ✔ Years list restored
        public List<int> Years { get; } =
            Enumerable.Range(DateTime.Now.Year - 10, 20).ToList();

        [ObservableProperty] private int year;
        [ObservableProperty] private int month;
        [ObservableProperty] private decimal ratePerDay;
        [ObservableProperty] private ObservableCollection<MonthRecord> records = new();
        [ObservableProperty] private string totalsText;

        public MonthViewModel(IMonthRecordRepository repo, TotalsViewModel totalsVM)
        {
            _repo = repo;
            this.totalsVM = totalsVM;

            Year = DateTime.Now.Year;
            Month = DateTime.Now.Month - 1; // zero-based index for picker
        }

        private string RateKey => $"Rate_{Year}_{Month + 1}";

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
            // Save rate
            Preferences.Set(RateKey, RatePerDay.ToString(new CultureInfo("nl-NL")));

            // Update rate in all records
            foreach (var r in Records)
                r.RatePerDay = RatePerDay;

            // Save to DB
            await _repo.SaveAllAsync(Records);

            UpdateTotals();

            // ⭐ Tell TotalsPage to reload next time
            totalsVM.Reset();

            // Allow reload when month/year changes
            _isLoaded = false;
        }

        // Reset when user changes month/year
        partial void OnMonthChanged(int value) => _isLoaded = false;
        partial void OnYearChanged(int value) => _isLoaded = false;
    }
}
