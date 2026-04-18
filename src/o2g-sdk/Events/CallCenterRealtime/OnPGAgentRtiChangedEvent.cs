/*
* Copyright 2022 ALE International
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


using o2g.Types.CommonNS;

namespace o2g.Events.CallCenterRealtimeNS
{
    /// <summary>
    /// <c>OnPGAgentRtiChangedEvent</c> represents the event that is triggered
    /// periodically when the agent processing group set defined in the <see cref="o2g.Types.CallCenterRealtimeNS.RtiContext">RtiContext</see> has real-time information updates.
    /// </summary>
    /// <para>
    /// It contains details about the CCD agent processing group that are monitored in the real-time
    /// context and whose state or attributes have changed.
    /// </para>
    /// <remarks>
    /// <para>Since: 2.7.4</para>
    /// </remarks>   
    public class OnPGAgentRtiChangedEvent : O2GEvent
    {
        /// <summary>
        /// Gets the name of the agent processing group.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the number identifying the agent processing group.
        /// </summary>
        public string Number { get; init; }

        /// <summary>
        /// Gets the type of the agent processing group.
        /// </summary>
        public AgentProcessingGroupType Type { get; init; }

        /// <summary>
        /// Gets the current service state of the agent processing group.
        /// </summary>
        public ServiceState State { get; init; }

        /// <summary>
        /// Gets the number of withdrawn agents.
        /// </summary>
        public int NbOfWithdrawnAgents { get; init; }

        /// <summary>
        /// Gets the number of agents currently in private call.
        /// </summary>
        public int NbOfAgentsInPrivateCall { get; init; }

        /// <summary>
        /// Gets the number of agents currently in ACD call.
        /// </summary>
        public int NbOfAgentsInACDCall { get; init; }

        /// <summary>
        /// Gets the number of agents currently in ACD ringing.
        /// </summary>
        public int NbOfAgentsInACDRinging { get; init; }

        /// <summary>
        /// Gets the number of agents currently in ACD conversation.
        /// </summary>
        public int NbOfAgentsInACDConv { get; init; }

        /// <summary>
        /// Gets the number of agents in wrap-up or transaction state.
        /// </summary>
        public int NbOfAgentsInWrapupAndTransaction { get; init; }

        /// <summary>
        /// Gets the number of agents currently in pause.
        /// </summary>
        public int NbOfAgentsInPause { get; init; }

        /// <summary>
        /// Gets the number of busy agents (ACD or in private call).
        /// </summary>
        public int NbOfBusyAgents { get; init; }

        /// <summary>
        /// Gets the number of logged-on agents.
        /// </summary>
        public int NbOfLoggedOnAgents { get; init; }

        /// <summary>
        /// Gets the number of free agents (withdrawn or not).
        /// </summary>
        public int NbOfFreeAgents { get; init; }

        /// <summary>
        /// Gets the number of idle agents, excluding withdrawn agents.
        /// </summary>
        public int NbOfIdleAgents { get; init; }

        /// <summary>
        /// Gets the number of logged-on and not withdrawn agents.
        /// </summary>
        public int NbOfLoggedOnAndNotWithdrawnAgents { get; init; }

        /// <summary>
        /// Gets the number of incoming calls during the last minute.
        /// </summary>
        public int IncomingTraffic { get; init; }

        /// <summary>
        /// Gets the consolidated pilots service level.
        /// </summary>
        public int ConsolidatedPilotsServiceLevel { get; init; }

        /// <summary>
        /// Gets the consolidated pilots efficiency.
        /// </summary>
        public int ConsolidatedPilotsEfficiency { get; init; }

        /// <summary>
        /// Gets the consolidated queues waiting time.
        /// </summary>
        public int ConsolidatedQueuesWaitingTime { get; init; }

        /// <summary>
        /// Gets the consolidated number of waiting calls in queues.
        /// </summary>
        public int ConsolidatedQueuesNbOfWaitingCalls { get; init; }

        /// <summary>
        /// Gets the consolidated expected waiting time in queues.
        /// </summary>
        public int ConsolidatedQueuesEWT { get; init; }

        /// <summary>
        /// Gets the worst service level on pilots serving this processing group.
        /// </summary>
        public int PilotsWorstServiceLevel { get; init; }

        /// <summary>
        /// Gets the worst efficiency on pilots serving this processing group.
        /// </summary>
        public int PilotsWorstEfficiency { get; init; }

        /// <summary>
        /// Gets the best service level on pilots serving this processing group.
        /// </summary>
        public int PilotsBestServiceLevel { get; init; }

        /// <summary>
        /// Gets the best efficiency on pilots serving this processing group.
        /// </summary>
        public int PilotsBestEfficiency { get; init; }

        /// <summary>
        /// Gets the longest waiting time in queues serving this processing group.
        /// </summary>
        public int QueuesLongestWaitingTime { get; init; }

        /// <summary>
        /// Gets the CCD key.
        /// </summary>
        public int AfeKey { get; init; }
    }
}
