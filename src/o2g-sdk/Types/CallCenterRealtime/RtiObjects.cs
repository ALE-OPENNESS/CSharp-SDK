/*
* Copyright 2025 ALE International
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
using System.Linq;
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterRealtimeNS
{
    /// <summary>
    /// <c>RtiObjects</c> represents the global configured object that can be monitoried by the CallCenterRealtime service.
    /// </summary>
    public class RtiObjects
    {
        /// <summary>
        /// Return the list of CCD operators.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the configured CCD operators.
        /// </value>
        public List<RtiObjectIdentifier> Agents { get; init; }

        /// <summary>
        /// Return the list of CCD pilots.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the configured CCD pilots.
        /// </value>
        public List<RtiObjectIdentifier> Pilots { get; init; }

        /// <summary>
        /// Return the list of CCD queues.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the configured CCD queues.
        /// </value>
        public List<RtiObjectIdentifier> Queues { get; init; }

        /// <summary>
        /// Return the list of agent processing groups.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the configured agent processing groups.
        /// </value>
        [JsonPropertyName("pgAgents")]
        public List<RtiObjectIdentifier> AgentProcessingGroups { get; init; }

        /// <summary>
        /// Return the list of other processing groups.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the configured other processing groups.
        /// </value>
        [JsonPropertyName("pgOthers")]
        public List<RtiObjectIdentifier> OtherProcessingGroups { get; init; }

        /// <summary>
        /// Creates a new <see cref="RtiFilter"/> pre-populated with all CCD objects
        /// available in this instance.
        /// </summary>
        /// <returns>A fully populated <see cref="RtiFilter"/> object.</returns>
        public RtiFilter CreateFilter()
        {
            var filter = new RtiFilter();

            if (Agents?.Count > 0)
            {
                filter.SetAgentNumbers(Agents.Select(a => a.Number).ToArray());
                filter.SetAgentAttributes(AgentAttributes.ALL);
            }

            if (Pilots?.Count > 0)
            {
                filter.SetPilotNumbers(Pilots.Select(a => a.Number).ToArray());
                filter.SetPilotAttributes(PilotAttributes.ALL);
            }

            if (Queues?.Count > 0)
            {
                filter.SetQueueNumbers(Queues.Select(a => a.Number).ToArray());
                filter.SetQueueAttributes(QueueAttributes.ALL);
            }

            if (AgentProcessingGroups?.Count > 0)
            {
                filter.SetAgentProcessingGroupNumbers(AgentProcessingGroups.Select(a => a.Number).ToArray());
                filter.SetAgentProcessingGroupAttributes(AgentProcessingGroupAttributes.ALL);
            }

            if (OtherProcessingGroups?.Count > 0)
            {
                filter.SetOtherProcessingGroupNumbers(OtherProcessingGroups.Select(a => a.Number).ToArray());
                filter.SetOtherProcessingGroupAttributes(OtherProcessingGroupAttributes.ALL);
            }

            return filter;
        }
    }
}
