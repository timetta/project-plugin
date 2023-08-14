using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Report date.
    /// </summary>
    public class ReportDate
    {
        /// <summary>
        /// Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Week.
        /// </summary>
        public DateTime Week { get; set; }

        /// <summary>
        /// Month.
        /// </summary>
        public DateTime Month { get; set; }

        /// <summary>
        /// Quarter.
        /// </summary>
        public DateTime Quarter { get; set; }

        /// <summary>
        /// Year.
        /// </summary>
        public DateTime Year { get; set; }

        /// <summary>
        /// Weak number.
        /// </summary>
        public int WeekNumber { get; set; }

        /// <summary>
        /// Month number.
        /// </summary>
        public int MonthNumber { get; set; }

        /// <summary>
        /// Quarter number.
        /// </summary>
        public int QuarterNumber { get; set; }

        /// <summary>
        /// Year number.
        /// </summary>
        public int YearNumber { get; set; }
    }
}
