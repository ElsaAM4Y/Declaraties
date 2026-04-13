using System;
using System.Collections.Generic;
using System.Text;

namespace Declaraties.Models
{
    public class TotalSummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalTimesWorking { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }

        public string MonthName =>
            new DateTime(Year, Month, 1).ToString("MMMM");
    }

}
