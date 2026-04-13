using System;
using SQLite;
using System.Text;

namespace Declaraties.Models
{
    public class MonthRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int DayTimesWorking { get; set; }

        public string DayNote { get; set; } = "";

        public decimal RatePerDay { get; set; }   // stored once per month (same for all days)
    }
}
