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

using System.Collections.Generic;

namespace o2g.Types.CallCenterManagementNS.CalendarNS
{
    /// <summary>
    /// Abstract base class for pilot calendars.
    /// <para>
    /// A calendar maps keys (either a <see cref="System.DayOfWeek"/> for normal calendars,
    /// or a <see cref="System.DateTime"/> for exception calendars) to a list of
    /// <see cref="Transition"/> objects defining the pilot's operating mode changes.
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">The key type used to index transitions.</typeparam>
    public abstract class AbstractCalendar<TKey>
    {
        /// <summary>
        /// The transitions indexed by key.
        /// </summary>
        protected readonly Dictionary<TKey, List<Transition>> Transitions;

        /// <summary>
        /// Initializes the calendar with the given transitions map.
        /// </summary>
        /// <param name="transitions">A dictionary mapping keys to transition lists.</param>
        protected AbstractCalendar(Dictionary<TKey, List<Transition>> transitions)
        {
            Transitions = transitions;
        }

        /// <summary>
        /// Returns all transitions for the specified key.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <returns>
        /// A list of <see cref="Transition"/> objects, or <see langword="null"/> if none exist.
        /// </returns>
        protected List<Transition> GetTransitions(TKey key)
            => Transitions.TryGetValue(key, out var list) ? list : null;

        /// <summary>
        /// Returns the transition at the specified index for the given key.
        /// </summary>
        /// <param name="key">The key to look up.</param>
        /// <param name="index">The zero-based index of the transition.</param>
        /// <returns>
        /// The <see cref="Transition"/> at the given index, or <see langword="null"/> if not found.
        /// </returns>
        protected Transition GetItemAt(TKey key, int index)
        {
            var list = GetTransitions(key);
            return list != null && index < list.Count ? list[index] : null;
        }
    }
}