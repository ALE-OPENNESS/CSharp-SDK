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
    /// Statistics for a single agent broken down by an individual pilot.
    /// </summary>
    public class AgentByPilotStatisticsRow
    {
        /// <summary>The directory number of the pilot.</summary>
        public string PilotNumber { get; init; }

        /// <summary>The name of the pilot.</summary>
        public string PilotName { get; init; }

        /// <summary>Extension data capturing all dynamic statistics fields.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Stats { get; set; }

        /// <summary>
        /// Returns the value for the given attribute, or an empty <see cref="StatValue"/> if not present.
        /// </summary>
        public StatValue Get(AgentByPilotAttributes attr)
        {
            string key = attr.ToString();
            if (Stats != null && Stats.TryGetValue(key, out JsonElement element))
                return new StatValue(element);
            return new StatValue(null);
        }
    }
}
