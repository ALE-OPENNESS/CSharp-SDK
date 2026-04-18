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
    /// <c>OnPilotRtiChangedEvent</c> represents the event that is triggered
    /// periodically when the pilot set defined in the <see cref="o2g.Types.CallCenterRealtimeNS.RtiContext">RtiContext</see> has real-time information updates.
    /// </summary>
    /// <para>
    /// It contains details about the CCD pilot that are monitored in the real-time
    /// context and whose state or attributes have changed.
    /// </para>
    /// <remarks>
    /// <para>Since: 2.7.4</para>
    /// </remarks>   
    public class OnPilotRtiChangedEvent : O2GEvent
    {
        /// <summary>
        /// Gets the name of the pilot.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the pilot directory number.
        /// </summary>
        public string Number { get; init; }

        /// <summary>
        /// Gets the current service state of the pilot.
        /// </summary>
        public ServiceState State { get; init; }

        /// <summary>
        /// Gets the number of calls currently in progress for the pilot.
        /// </summary>
        public int NbOfRunningCalls { get; init; }

        /// <summary>
        /// Gets the service level of the pilot.
        /// </summary>
        public int ServiceLevel { get; init; }

        /// <summary>
        /// Gets the efficiency indicator of the pilot.
        /// </summary>
        public int Efficiency { get; init; }

        /// <summary>
        /// Gets the number of waiting calls for the pilot.
        /// </summary>
        public int NbOfWaitingCalls { get; init; }

        /// <summary>
        /// Gets the number of ringing ACD calls for the pilot.
        /// </summary>
        public int NbOfRingingACDCalls { get; init; }

        /// <summary>
        /// Gets the number of calls rerouted for mutual aid for the pilot.
        /// </summary>
        public int NbOfMutualAidCalls { get; init; }

        /// <summary>
        /// Gets the number of dissuaded calls for the pilot.
        /// </summary>
        public int NbOfDissuadedCalls { get; init; }

        /// <summary>
        /// Gets the number of calls currently in conversation for the pilot.
        /// </summary>
        public int NbOfCallsInConversation { get; init; }

        /// <summary>
        /// Gets the number of calls in general forwarding for the pilot.
        /// </summary>
        public int NbOfCallsInGeneralForwarding { get; init; }

        /// <summary>
        /// Gets the number of calls being processed in a remote processing group for the pilot.
        /// </summary>
        public int NbOfCallsInRemoteProcessingGroup { get; init; }

        /// <summary>
        /// Gets the number of incoming calls within the minute for the pilot.
        /// </summary>
        public int IncomingTraffic { get; init; }

        /// <summary>
        /// Gets the average waiting time before answering for the pilot.
        /// </summary>
        public int AverageWaitingTime { get; init; }

        /// <summary>
        /// Gets the worst service level among the pilots of a super pilot.
        /// </summary>
        public int WorstServiceLevelInList { get; init; }

        /// <summary>
        /// Gets the worst efficiency among the pilots of a super pilot.
        /// </summary>
        public int WorstEfficiencyInList { get; init; }

        /// <summary>
        /// Gets the best service level among the pilots of a super pilot.
        /// </summary>
        public int BestServiceLevelInList { get; init; }

        /// <summary>
        /// Gets the best efficiency among the pilots of a super pilot.
        /// </summary>
        public int BestEfficiencyInList { get; init; }

        /// <summary>
        /// Gets the CCD key.
        /// </summary>
        public int AfeKey { get; init; }
    }
}
