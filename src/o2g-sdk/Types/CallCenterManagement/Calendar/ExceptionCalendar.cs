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
using System.Collections.ObjectModel;

namespace o2g.Types.CallCenterManagementNS.CalendarNS
{
    /// <summary>
    /// Represents the exceptional calendar associated with a CCD pilot.
    /// <para>
    /// This calendar defines special days — such as holidays or other exceptions —
    /// that override the normal calendar behaviour. Each exceptional day can have
    /// up to 10 transitions (time slots) specifying changes in the pilot's operating mode.
    /// </para>
    /// </summary>
    public class ExceptionCalendar : AbstractCalendar<DateTime>
    {
        /// <summary>
        /// Initializes the calendar with the given transitions map.
        /// </summary>
        /// <param name="transitions">A dictionary mapping exception dates to transition lists.</param>
        internal ExceptionCalendar(Dictionary<DateTime, List<Transition>> transitions)
            : base(transitions) { }

        /// <summary>
        /// The exceptional dates configured in this calendar.
        /// </summary>
        /// <value>
        /// A read-only collection of <see cref="DateTime"/> values representing exception days.
        /// </value>
        public IReadOnlyCollection<DateTime> ExceptionDates
            => new ReadOnlyCollection<DateTime>(new List<DateTime>(Transitions.Keys));

        /// <summary>
        /// Returns the transition at the specified index for the given exception date.
        /// </summary>
        /// <param name="date">The exception date.</param>
        /// <param name="index">The zero-based index of the transition (0–9).</param>
        /// <returns>
        /// The <see cref="Transition"/> at the given index, or <see langword="null"/> if not found.
        /// </returns>
        public Transition GetTransitionAt(DateTime date, int index)
            => GetItemAt(date.Date, index);

        /// <summary>
        /// Returns all transitions for the specified exception date.
        /// </summary>
        /// <param name="date">The exception date.</param>
        /// <returns>
        /// A read-only list of <see cref="Transition"/> objects, or <see langword="null"/> if none are configured.
        /// </returns>
        public new IReadOnlyList<Transition> GetTransitions(DateTime date)
        {
            var list = base.GetTransitions(date.Date);
            return list != null ? new ReadOnlyCollection<Transition>(list) : null;
        }
    }
}