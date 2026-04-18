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
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Represents the statistical data returned for a requester, including agent-level
    /// and pilot-level statistics grouped by time slot and observation period.
    /// </summary>
    public class StatisticsData
    {
        /// <summary>The identifier of the requester (supervisor) for whom the statistics were retrieved.</summary>
        [JsonPropertyName("supervisor")]
        public string RequesterId { get; init; }

        /// <summary>The agent-level statistics, grouped by time slot and observation period.</summary>
        public List<ObjectStatistics<AgentStatisticsRow>> AgentsStats { get; init; }

        /// <summary>The pilot-level statistics, grouped by time slot and observation period.</summary>
        public List<ObjectStatistics<PilotStatisticsRow>> PilotsStats { get; init; }

        /// <summary>The pilot abandoned-calls statistics.</summary>
        public BasicObjectStatistics<PilotAbandonedCallsStatisticsRow> PilotAbandonedCalls { get; init; }
    }
}
