/*
* Copyright 2021 ALE International
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

namespace o2g.Types.AnalyticsNS
{
    /// <summary>
    /// <c>TimeRange</c> represents an interval between two dates. It is used to filter analytics requests
    /// </summary>
    /// <see cref="IAnalytics.GetChargingsAsync(int, TimeRange, int?, bool)"/>
    public class TimeRange
    {
        /// <summary>
        /// Return the "from" date.
        /// </summary>
        /// <value>
        /// The <see cref="DateTime"/> object that represents the "from" date of this range.
        /// </value>
        public DateTime from { get; init; }

        /// <summary>
        /// Return the "to" date.
        /// </summary>
        /// <value>
        /// The <see cref="DateTime"/> object that represents the "to" date of this range.
        /// </value>
        public DateTime to { get; init; }

        /// <summary>
        /// Construct a new <c>TimeRange</c>, with the specified "from" date and "to" date.
        /// </summary>
        /// <param name="from">The "from" date.</param>
        /// <param name="to">The "to" date.</param>
        public TimeRange(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Construct a new <c>TimeRange</c> with the specified "from" date and a specified number of days.
        /// </summary>
        /// <param name="from">The "from" date.</param>
        /// <param name="days">The number of days in this range.</param>
        public TimeRange(DateTime from, int days)
        {
            this.from = from;
            this.to = this.from.AddDays(days);
        }
    }
}
