/*
* Copyright 2026 ALE International
*
* Permission is hereby granted, free of charge, to any person obtaining a copy of this
* software and associated documentation files (the "Software"), to deal in the Software
* without restriction, including without limitation the rights to use, copy, modify, merge,
* publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
* to whom the Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all copies or
* substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
* BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
* DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Runtime.Serialization;

namespace o2g.Types.CallCenterStatisticsNS.Scheduled
{
    /// <summary>
    /// Defines the type of observation period for a scheduled report.
    /// </summary>
    public enum ReportObservationPeriodType
    {
        /// <summary>The current day.</summary>
        [EnumMember(Value = "currentDay")] CurrentDay,
        /// <summary>The current week.</summary>
        [EnumMember(Value = "currentWeek")] CurrentWeek,
        /// <summary>The current month.</summary>
        [EnumMember(Value = "currentMonth")] CurrentMonth,
        /// <summary>The last N days.</summary>
        [EnumMember(Value = "lastDays")] LastDays,
        /// <summary>The last N weeks.</summary>
        [EnumMember(Value = "lastWeeks")] LastWeeks,
        /// <summary>The last month.</summary>
        [EnumMember(Value = "lastMonth")] LastMonth,
        /// <summary>A custom range defined by a start and end date.</summary>
        [EnumMember(Value = "fromDateToDate")] FromDateToDate
    }

    /// <summary>
    /// Represents an observation period used in a scheduled statistics report.
    /// Use the static factory methods to create instances.
    /// </summary>
    public class ReportObservationPeriod
    {
        /// <summary>The type of observation period.</summary>
        public ReportObservationPeriodType PeriodType { get; }

        /// <summary>The number of units (days or weeks) for last-period types; -1 otherwise.</summary>
        public int LastUnits { get; }

        /// <summary>The start date for <see cref="ReportObservationPeriodType.FromDateToDate"/>; <see langword="null"/> otherwise.</summary>
        public DateTime? BeginDate { get; }

        /// <summary>The end date for <see cref="ReportObservationPeriodType.FromDateToDate"/>; <see langword="null"/> otherwise.</summary>
        public DateTime? EndDate { get; }

        private ReportObservationPeriod(ReportObservationPeriodType periodType, int lastUnits = -1, DateTime? beginDate = null, DateTime? endDate = null)
        {
            PeriodType = periodType;
            LastUnits = lastUnits;
            BeginDate = beginDate;
            EndDate = endDate;
        }

        /// <summary>Creates an observation period for the current day.</summary>
        public static ReportObservationPeriod OnCurrentDay() =>
            new ReportObservationPeriod(ReportObservationPeriodType.CurrentDay);

        /// <summary>Creates an observation period for the current week.</summary>
        public static ReportObservationPeriod OnCurrentWeek() =>
            new ReportObservationPeriod(ReportObservationPeriodType.CurrentWeek);

        /// <summary>Creates an observation period for the current month.</summary>
        public static ReportObservationPeriod OnCurrentMonth() =>
            new ReportObservationPeriod(ReportObservationPeriodType.CurrentMonth);

        /// <summary>Creates an observation period for the last N days (1–31).</summary>
        public static ReportObservationPeriod OnLastDays(int nbDays)
        {
            if (nbDays < 1 || nbDays > 31) throw new ArgumentOutOfRangeException(nameof(nbDays), "nbDays must be 1-31");
            return new ReportObservationPeriod(ReportObservationPeriodType.LastDays, nbDays);
        }

        /// <summary>Creates an observation period for the last N weeks (1–4).</summary>
        public static ReportObservationPeriod OnLastWeeks(int nbWeeks)
        {
            if (nbWeeks < 1 || nbWeeks > 4) throw new ArgumentOutOfRangeException(nameof(nbWeeks), "nbWeeks must be 1-4");
            return new ReportObservationPeriod(ReportObservationPeriodType.LastWeeks, nbWeeks);
        }

        /// <summary>Creates an observation period for the last month.</summary>
        public static ReportObservationPeriod OnLastMonth() =>
            new ReportObservationPeriod(ReportObservationPeriodType.LastMonth, 1);

        /// <summary>Creates a custom observation period from a given date for a number of days.</summary>
        public static ReportObservationPeriod FromDate(DateTime from, int nbDays)
        {
            if (from > DateTime.Now) throw new ArgumentException("'from' must be in the past", nameof(from));
            if (nbDays < 1 || nbDays > 31) throw new ArgumentOutOfRangeException(nameof(nbDays), "nbDays must be 1-31");
            return new ReportObservationPeriod(ReportObservationPeriodType.FromDateToDate, -1, from, from.AddDays(nbDays));
        }
    }
}
