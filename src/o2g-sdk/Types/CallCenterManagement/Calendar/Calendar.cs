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

namespace o2g.Types.CallCenterManagementNS.CalendarNS
{
    /// <summary>
    /// Represents a pilot's calendar, combining normal and exceptional days.
    /// <para>
    /// The <b>normal calendar</b> defines standard behaviour for each day of the week.
    /// The <b>exceptional calendar</b> defines special days (e.g. holidays) that override
    /// the normal calendar.
    /// </para>
    /// </summary>
    public class Calendar
    {
        /// <summary>
        /// The normal days calendar defining standard weekly behaviour.
        /// </summary>
        /// <value>
        /// A <see cref="NormalCalendar"/> instance, or <see langword="null"/> if not set.
        /// </value>
        public NormalCalendar NormalDays { get; init; }

        /// <summary>
        /// The exceptional days calendar defining holiday or override behaviour.
        /// </summary>
        /// <value>
        /// An <see cref="ExceptionCalendar"/> instance, or <see langword="null"/> if not set.
        /// </value>
        public ExceptionCalendar ExceptionDays { get; init; }
    }
}