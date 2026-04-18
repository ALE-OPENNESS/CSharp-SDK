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

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Represents statistical results grouped by observation period, without a time slot.
    /// Used for pilot abandoned-calls statistics.
    /// </summary>
    /// <typeparam name="T">The type of the statistics row.</typeparam>
    public class BasicObjectStatistics<T>
    {
        /// <summary>The selected period during which these statistics were collected.</summary>
        public SelectedPeriod SelectedPeriod { get; init; }

        /// <summary>The list of statistics rows collected for this period.</summary>
        public List<T> Rows { get; init; }
    }

    /// <summary>
    /// Represents statistical results for a specific observation period and time slot.
    /// Used for agent and pilot statistics grouped by time slot.
    /// </summary>
    /// <typeparam name="T">The type of the statistics row.</typeparam>
    public class ObjectStatistics<T> : BasicObjectStatistics<T>
    {
        /// <summary>
        /// The start date and time of this time slot (e.g. "2025-09-02T10:00").
        /// </summary>
        public string TimeSlot { get; init; }
    }
}
