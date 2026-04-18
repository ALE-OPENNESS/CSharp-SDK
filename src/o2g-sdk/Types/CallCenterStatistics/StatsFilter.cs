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
    /// Abstract base class for statistics filters.
    /// Use the factory methods to create filter instances.
    /// </summary>
    public abstract class StatsFilter
    {
        /// <summary>The directory numbers included in this filter.</summary>
        public List<string> Numbers { get; } = new();

        /// <summary>
        /// Creates a filter for agent statistics.
        /// </summary>
        public static AgentFilter CreateAgentFilter() => new AgentFilter();

        /// <summary>
        /// Creates a filter for pilot statistics.
        /// </summary>
        public static PilotFilter CreatePilotFilter() => new PilotFilter();

        /// <summary>
        /// Creates a filter for pilot abandoned-calls statistics.
        /// </summary>
        public static PilotAbandonedCallsFilter CreatePilotAbandonedCallsFilter() => new PilotAbandonedCallsFilter();
    }

    /// <summary>
    /// Filter for agent statistics.
    /// </summary>
    public sealed class AgentFilter : StatsFilter
    {
        /// <summary>Agent-level attributes to include.</summary>
        public HashSet<AgentAttributes> AgentAttributes { get; } = new();

        /// <summary>Agent-by-pilot attributes to include.</summary>
        public HashSet<AgentByPilotAttributes> ByPilotAttributes { get; } = new();

        /// <summary>Adds agent directory numbers to the filter.</summary>
        public AgentFilter AddNumbers(params string[] numbers)
        {
            Numbers.AddRange(numbers);
            return this;
        }

        /// <summary>Sets the agent attributes to collect.</summary>
        public AgentFilter SetAgentAttributes(params AgentAttributes[] attributes)
        {
            AgentAttributes.Clear();
            foreach (var a in attributes) AgentAttributes.Add(a);
            return this;
        }

        /// <summary>Sets the agent-by-pilot attributes to collect.</summary>
        public AgentFilter SetAgentByPilotAttributes(params AgentByPilotAttributes[] attributes)
        {
            ByPilotAttributes.Clear();
            foreach (var a in attributes) ByPilotAttributes.Add(a);
            return this;
        }
    }

    /// <summary>
    /// Filter for pilot statistics.
    /// </summary>
    public sealed class PilotFilter : StatsFilter
    {
        /// <summary>Pilot-level attributes to include.</summary>
        public HashSet<PilotAttributes> PilotAttributes { get; } = new();

        /// <summary>Adds pilot directory numbers to the filter.</summary>
        public PilotFilter AddNumbers(params string[] numbers)
        {
            Numbers.AddRange(numbers);
            return this;
        }

        /// <summary>Sets the pilot attributes to collect.</summary>
        public PilotFilter SetPilotAttributes(params PilotAttributes[] attributes)
        {
            PilotAttributes.Clear();
            foreach (var a in attributes) PilotAttributes.Add(a);
            return this;
        }
    }

    /// <summary>
    /// Filter for pilot abandoned-calls statistics.
    /// </summary>
    public sealed class PilotAbandonedCallsFilter : StatsFilter
    {
        /// <summary>Abandoned-calls attributes to include.</summary>
        public HashSet<PilotAbandonedCallsAttributes> Attributes { get; } = new();

        /// <summary>Adds pilot directory numbers to the filter.</summary>
        public PilotAbandonedCallsFilter AddNumbers(params string[] numbers)
        {
            Numbers.AddRange(numbers);
            return this;
        }

        /// <summary>Sets the abandoned-calls attributes to collect.</summary>
        public PilotAbandonedCallsFilter SetAttributes(params PilotAbandonedCallsAttributes[] attributes)
        {
            Attributes.Clear();
            foreach (var a in attributes) Attributes.Add(a);
            return this;
        }
    }
}
