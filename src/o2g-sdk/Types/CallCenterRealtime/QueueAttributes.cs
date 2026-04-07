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
    /// Represents the possible attributes for a CCD queue.
    /// <para>
    /// CallCenterRealtimeService can report realtime on each of these attributes.
    /// </para>
    /// </summary>
    public enum QueueAttributes
    {
        /// <summary>
        /// Service state.
        /// </summary>
        State,

        /// <summary>
        /// Number of agents in the distribution of the waiting queue.
        /// </summary>
        NbOfAgentsInDistribution,

        /// <summary>
        /// Number of incoming calls within the minute.
        /// </summary>
        IncomingTraffic,

        /// <summary>
        /// Number of outgoing calls within the minute.
        /// </summary>
        OutgoingTraffic,

        /// <summary>
        /// Number of waiting calls.
        /// </summary>
        NbOfWaitingCalls,

        /// <summary>
        /// Current waiting time.
        /// </summary>
        CurrentWaitingTime,

        /// <summary>
        /// Expected waiting time.
        /// </summary>
        ExpectedWaitingTime,

        /// <summary>
        /// Filling rate.
        /// </summary>
        FillingRate,

        /// <summary>
        /// Longest time among the queues of a super waiting queue.
        /// </summary>
        LongestWaitingTimeInList,

        /// <summary>
        /// All attributes.
        /// </summary>
        ALL
    }
}
