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
    /// Represents a statistics context associated with a requester.
    /// </summary>
    public class StatsContext
    {
        /// <summary>The unique identifier of this context.</summary>
        public string Id { get; set; }

        /// <summary>The identifier of the requester (supervisor) owning this context.</summary>
        public string RequesterId { get; set; }

        /// <summary>A human-readable label for this context.</summary>
        public string Label { get; set; }

        /// <summary>A description of this context.</summary>
        public string Description { get; set; }

        /// <summary>Indicates whether this context is associated with a scheduled report.</summary>
        public bool IsScheduled { get; set; }

        /// <summary>
        /// When <see langword="true"/>, statistics headers are condensed (fewer column names).
        /// </summary>
        public bool ShortHeader { get; set; }

        /// <summary>The filter defining which agents or pilots are monitored.</summary>
        public StatsFilter Filter { get; set; }
    }
}
