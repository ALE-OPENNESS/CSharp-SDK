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

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace o2g.Types.CallCenterRealtimeNS
{
    /// <summary>
    /// Specifies, for each category of CCD object, which objects should be included in realtime eventing
    /// and which parameters of each object should be included.
    /// </summary>
    public class RtiFilter
    {
        private AgentRtiFilter _agentFilter;
        private PilotRtiFilter _pilotFilter;
        private QueueRtiFilter _queueFilter;
        private AgentProcessingGroupRtiFilter _agentProcessingGroupFilter;
        private OtherProcessingGroupRtiFilter _otherProcessingGroupFilter;

        // --- Public setters with XML documentation ---

        /// <summary>
        /// Sets the specified set of <see cref="AgentAttributes"/> in this filter.
        /// </summary>
        /// <param name="attributes">The attributes to include for agents.</param>
        public void SetAgentAttributes(params AgentAttributes[] attributes) =>
            GetAgentFilter().AddAttributes(attributes);

        /// <summary>
        /// Sets the specified list of agent directory numbers in this filter.
        /// </summary>
        /// <param name="numbers">The directory numbers of agents.</param>
        public void SetAgentNumbers(params string[] numbers) =>
            GetAgentFilter().AddNumbers(numbers);

        /// <summary>
        /// Sets the specified set of <see cref="PilotAttributes"/> in this filter.
        /// </summary>
        /// <param name="attributes">The attributes to include for pilots.</param>
        public void SetPilotAttributes(params PilotAttributes[] attributes) =>
            GetPilotFilter().AddAttributes(attributes);

        /// <summary>
        /// Sets the specified list of pilot directory numbers in this filter.
        /// </summary>
        /// <param name="numbers">The directory numbers of pilots.</param>
        public void SetPilotNumbers(params string[] numbers) =>
            GetPilotFilter().AddNumbers(numbers);

        /// <summary>
        /// Sets the specified set of <see cref="QueueAttributes"/> in this filter.
        /// </summary>
        /// <param name="attributes">The attributes to include for queues.</param>
        public void SetQueueAttributes(params QueueAttributes[] attributes) =>
            GetQueueFilter().AddAttributes(attributes);

        /// <summary>
        /// Sets the specified list of queue directory numbers in this filter.
        /// </summary>
        /// <param name="numbers">The directory numbers of queues.</param>
        public void SetQueueNumbers(params string[] numbers) =>
            GetQueueFilter().AddNumbers(numbers);

        /// <summary>
        /// Sets the specified set of <see cref="AgentProcessingGroupAttributes"/> in this filter.
        /// </summary>
        /// <param name="attributes">The attributes to include for agent processing groups.</param>
        public void SetAgentProcessingGroupAttributes(params AgentProcessingGroupAttributes[] attributes) =>
            GetAgentProcessingGroupFilter().AddAttributes(attributes);

        /// <summary>
        /// Sets the specified list of agent processing group directory numbers in this filter.
        /// </summary>
        /// <param name="numbers">The directory numbers of agent processing groups.</param>
        public void SetAgentProcessingGroupNumbers(params string[] numbers) =>
            GetAgentProcessingGroupFilter().AddNumbers(numbers);

        /// <summary>
        /// Sets the specified set of <see cref="OtherProcessingGroupAttributes"/> in this filter.
        /// </summary>
        /// <param name="attributes">The attributes to include for other processing groups.</param>
        public void SetOtherProcessingGroupAttributes(params OtherProcessingGroupAttributes[] attributes) =>
            GetOtherProcessingGroupFilter().AddAttributes(attributes);

        /// <summary>
        /// Sets the specified list of other processing group directory numbers in this filter.
        /// </summary>
        /// <param name="numbers">The directory numbers of other processing groups.</param>
        public void SetOtherProcessingGroupNumbers(params string[] numbers) =>
            GetOtherProcessingGroupFilter().AddNumbers(numbers);

        private AgentRtiFilter GetAgentFilter() => _agentFilter ??= new AgentRtiFilter();
        private PilotRtiFilter GetPilotFilter() => _pilotFilter ??= new PilotRtiFilter();
        private QueueRtiFilter GetQueueFilter() => _queueFilter ??= new QueueRtiFilter();
        private AgentProcessingGroupRtiFilter GetAgentProcessingGroupFilter() =>
            _agentProcessingGroupFilter ??= new AgentProcessingGroupRtiFilter();
        private OtherProcessingGroupRtiFilter GetOtherProcessingGroupFilter() =>
            _otherProcessingGroupFilter ??= new OtherProcessingGroupRtiFilter();

        private class AgentRtiFilter : AbstractRtiFilter<AgentAttributes> { }
        private class PilotRtiFilter : AbstractRtiFilter<PilotAttributes> { }
        private class QueueRtiFilter : AbstractRtiFilter<QueueAttributes> { }
        private class AgentProcessingGroupRtiFilter : AbstractRtiFilter<AgentProcessingGroupAttributes> { }
        private class OtherProcessingGroupRtiFilter : AbstractRtiFilter<OtherProcessingGroupAttributes> { }
    }

}
