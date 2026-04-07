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
    /// Represents the possible attributes for a CCD pilot.
    /// <para>
    /// CallCenterRealtimeService can report realtime on each of these attributes.
    /// </para>
    /// </summary>
    public enum PilotAttributes
    {
        /// <summary>
        /// Service state (Open, blocked, ...).
        /// </summary>
        State,

        /// <summary>
        /// Service level.
        /// </summary>
        ServiceLevel,

        /// <summary>
        /// Current rule name.
        /// </summary>
        CurrentRuleName,

        /// <summary>
        /// Number of waiting calls.
        /// </summary>
        NbOfWaitingCalls,

        /// <summary>
        /// Number of calls rerouted for mutual aid.
        /// </summary>
        NbOfMutualAidCalls,

        /// <summary>
        /// Number of calls in conversation.
        /// </summary>
        NbOfCallsInConversation,

        /// <summary>
        /// Number of calls being processed in a remote PG.
        /// </summary>
        NbOfCallsInRemotePG,

        /// <summary>
        /// Average waiting time before answering (in seconds).
        /// </summary>
        AverageWaitingTime,

        /// <summary>
        /// Number of calls in progress.
        /// </summary>
        NbOfRunningCalls,

        /// <summary>
        /// Number of ringing ACD calls.
        /// </summary>
        NbOfRingingACDCalls,

        /// <summary>
        /// Number of dissuaded calls.
        /// </summary>
        NbOfDissuadedCalls,

        /// <summary>
        /// Number of calls in general forwarding.
        /// </summary>
        NbOfCallsInGeneralForwarding,

        /// <summary>
        /// Efficiency (average/best/worst).
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
