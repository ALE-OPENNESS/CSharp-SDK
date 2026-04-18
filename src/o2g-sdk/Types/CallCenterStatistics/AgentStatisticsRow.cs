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
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Statistics row for a single agent in a statistics response.
    /// </summary>
    public class AgentStatisticsRow
    {
        /// <summary>Date of the statistics row (e.g. "2025-09-02").</summary>
        public string Date { get; init; }

        /// <summary>Login name of the agent.</summary>
        public string Login { get; init; }

        /// <summary>Operator of the agent.</summary>
        public string Operator { get; init; }

        /// <summary>First name of the agent.</summary>
        public string FirstName { get; init; }

        /// <summary>Last name of the agent.</summary>
        public string LastName { get; init; }

        /// <summary>Directory number of the agent.</summary>
        public string Number { get; init; }

        /// <summary>Directory number of the group the agent is logged into.</summary>
        public string Group { get; init; }

        /// <summary>Per-pilot breakdown statistics for this agent.</summary>
        public List<AgentByPilotStatisticsRow> PilotAgentStatsRows { get; init; }

        /// <summary>Extension data capturing all dynamic statistics fields.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Stats { get; set; }

        /// <summary>
        /// Returns the value for the given agent attribute, or an empty <see cref="StatValue"/> if not present.
        /// </summary>
        public StatValue Get(AgentAttributes attr)
        {
            string key = GetJsonKey(attr);
            if (Stats != null && Stats.TryGetValue(key, out JsonElement element))
                return new StatValue(element);
            return new StatValue(null);
        }

        private static string GetJsonKey(AgentAttributes attr)
        {
            var memberInfo = typeof(AgentAttributes).GetMember(attr.ToString());
            if (memberInfo.Length > 0)
            {
                var enumMember = memberInfo[0].GetCustomAttribute<EnumMemberAttribute>();
                if (enumMember?.Value != null) return enumMember.Value;
            }
            return attr.ToString();
        }
    }
}
