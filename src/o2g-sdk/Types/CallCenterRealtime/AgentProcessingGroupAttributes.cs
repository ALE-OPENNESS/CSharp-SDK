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

namespace o2g.Types.CallCenterRealtimeNS
{
    /// <summary>
    /// Represents the possible attributes for a CCD agent processing group.
    /// <para>
    /// CallCenterRealtimeService can report realtime on each of these attributes.
    /// </para>
    /// </summary>
    public enum AgentProcessingGroupAttributes
    {
        /// <summary>
        /// Service state (Open, blocked, ...).
        /// </summary>
        State,

        /// <summary>
        /// Number of withdrawn agents.
        /// </summary>
        NbOfWithdrawnAgents,

        /// <summary>
        /// Number of agents in private call.
        /// </summary>
        NbOfAgentsInPrivateCall,

        /// <summary>
        /// Number of agents in ACD call.
        /// </summary>
        NbOfAgentsInACDCall,

        /// <summary>
        /// Number of agents in ringing ACD call.
        /// </summary>
        NbOfAgentsInACDRinging,

        /// <summary>
        /// Number of agents in established ACD call.
        /// </summary>
        NbOfAgentsInACDConv,

        /// <summary>
        /// Number of agents in wrap-up and in entering transaction code.
        /// </summary>
        NbOfAgentsInWrapupAndTransaction,

        /// <summary>
        /// Number of agents in pause.
        /// </summary>
        NbOfAgentsInPause,

        /// <summary>
        /// Number of busy agents (ACD or private call).
        /// </summary>
        NbOfBusyAgents,

        /// <summary>
        /// Number of logged agents.
        /// </summary>
        NbOfLoggedOnAgents,

        /// <summary>
        /// Number of free agents (withdrawn or not).
        /// </summary>
        NbOfFreeAgents,

        /// <summary>
        /// Number of free agents (excluding withdrawn).
        /// </summary>
        NbOfIdleAgents,

        /// <summary>
        /// Number of logged agents (excluding withdrawn and free).
        /// </summary>
        NbOfLoggedOnAndNotWithdrawnAgents,

        /// <summary>
        /// Current waiting time on the queues possibly serving this team.
        /// </summary>
        ConsolidatedQueuesWaitingTime,

        /// <summary>
        /// Number of waiting calls on the queues possibly serving this team.
        /// </summary>
        ConsolidatedQueuesNbOfWaitingCalls,

        /// <summary>
        /// Expected waiting time on the queues possibly serving this team.
        /// </summary>
        ConsolidatedQueuesEWT,

        /// <summary>
        /// Service level on all the pilots possibly serving this team (average/best/worst).
        /// </summary>
        ServiceLevel,

        /// <summary>
        /// Efficiency on all the pilots possibly serving this team (average/best/worst).
        /// </summary>
        Efficiency,

        /// <summary>
        /// Number of incoming calls within the minute.
        /// </summary>
        IncomingTraffic,

        /// <summary>
        /// All attributes.
        /// </summary>
        ALL
    }
}
