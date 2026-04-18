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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Represents a single row of abandoned call statistics for a CCD pilot.
    /// </summary>
    public class PilotAbandonedCallsStatisticsRow
    {
        /// <summary>Date of the statistics row (e.g. "2025-09-02").</summary>
        public string Date { get; init; }

        /// <summary>Queue name associated with this abandoned call.</summary>
        public string QueueName { get; init; }

        /// <summary>Name of the pilot on which the call was abandoned.</summary>
        public string PilotName { get; init; }

        /// <summary>Directory number of the pilot on which the call was abandoned.</summary>
        public string PilotNumber { get; init; }

        /// <summary>Total time in seconds the caller waited before hanging up.</summary>
        public int? WaitingTime { get; init; }

        /// <summary>Extension data capturing all dynamic statistics fields (abandoned-on flags, etc.).</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Stats { get; set; }

        /// <summary>
        /// Returns the value for the given abandoned-calls attribute, or an empty <see cref="StatValue"/> if not present.
        /// </summary>
        public StatValue Get(PilotAbandonedCallsAttributes attr)
        {
            string key = attr.ToString();
            if (Stats != null && Stats.TryGetValue(key, out JsonElement element))
                return new StatValue(element);
            return new StatValue(null);
        }
    }
}
