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
using System.Collections.Generic;

namespace o2g.Types.CallCenterStatisticsNS.Scheduled
{
    /// <summary>
    /// Defines how often a scheduled statistics report recurs.
    /// </summary>
    public enum RecurrenceType
    {
        /// <summary>The report runs every day.</summary>
        DAILY,
        /// <summary>The report runs on specific days of the week.</summary>
        WEEKLY,
        /// <summary>The report runs on a specific day each month.</summary>
        MONTHLY
    }

    /// <summary>
    /// Represents a recurrence schedule for a scheduled statistics report.
    /// Use the static factory methods to create instances.
    /// </summary>
    public class Recurrence
    {
        /// <summary>The recurrence type.</summary>
        public RecurrenceType Type { get; }

        /// <summary>The days of the week for a weekly recurrence; <see langword="null"/> otherwise.</summary>
        public IReadOnlyList<DayOfWeek> DaysInWeek { get; }

        /// <summary>The day of the month (1–31) for a monthly recurrence; -1 otherwise.</summary>
        public int DayInMonth { get; }

        private Recurrence(RecurrenceType type, List<DayOfWeek> daysInWeek, int dayInMonth)
        {
            Type = type;
            DaysInWeek = daysInWeek?.AsReadOnly();
            DayInMonth = dayInMonth;
        }

        /// <summary>Creates a daily recurrence.</summary>
        public static Recurrence Daily() => new Recurrence(RecurrenceType.DAILY, null, -1);

        /// <summary>Creates a weekly recurrence on the specified days.</summary>
        public static Recurrence Weekly(params DayOfWeek[] days)
        {
            if (days == null || days.Length == 0)
                throw new ArgumentException("days must not be null or empty");
            return new Recurrence(RecurrenceType.WEEKLY, new List<DayOfWeek>(days), -1);
        }

        /// <summary>Creates a monthly recurrence on the specified day of the month (1–31).</summary>
        public static Recurrence Monthly(int day)
        {
            if (day < 1 || day > 31)
                throw new ArgumentOutOfRangeException(nameof(day), "day must be 1-31");
            return new Recurrence(RecurrenceType.MONTHLY, null, day);
        }
    }
}
