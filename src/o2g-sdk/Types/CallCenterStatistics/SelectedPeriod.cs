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

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Defines the time range and granularity for a statistics result.
    /// </summary>
    public class SelectedPeriod
    {
        /// <summary>The observation period type (one day or several days).</summary>
        public DataObservationPeriod PeriodType { get; init; }

        /// <summary>The time slot granularity used for aggregating statistics.</summary>
        public TimeInterval SlotType { get; init; }

        /// <summary>The start of the observation period as a string (e.g. "2025-09-02").</summary>
        public string BeginDate { get; init; }

        /// <summary>The end of the observation period as a string (e.g. "2025-09-02").</summary>
        public string EndDate { get; init; }
    }
}
