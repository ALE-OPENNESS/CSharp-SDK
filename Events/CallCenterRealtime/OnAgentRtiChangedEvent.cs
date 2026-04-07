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

namespace o2g.Events.CallCenterRealtimeNS
{
    /// <summary>
    /// <c>OnAgentRtiChangedEvent</c> represents the event that is triggered
    /// periodically when the agent set defined in the <see cref="o2g.Types.CallCenterRealtimeNS.RtiContext">RtiContext</see> has real-time information updates.
    /// </summary>
    /// <para>
    /// It contains details about the CCD agents that are monitored in the real-time
    /// context and whose state or attributes have changed.
    /// </para>
    /// <remarks>
    /// <para>Since: 2.7.4</para>
    /// </remarks>   
    public class OnAgentRtiChangedEvent : O2GEvent
    {
        /// <summary>
        /// Gets the agent name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the agent first name.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Gets the agent directory number.
        /// </summary>
        public string Number { get; init; }

        /// <summary>
        /// Gets the agent type.
        /// </summary>
        public AgentType Type { get; init; }

        /// <summary>
        /// Gets the logon date.
        /// </summary>
        public int LogonDate { get; init; }

        /// <summary>
        /// Gets the agent service state.
        /// </summary>
        public AgentServiceState ServiceState { get; init; }

        /// <summary>
        /// Gets the state date.
        /// </summary>
        public int StateDate { get; init; }

        /// <summary>
        /// Gets the agent phone state.
        /// </summary>
        public AgentPhoneState PhoneState { get; init; }

        /// <summary>
        /// Gets the phone state date.
        /// </summary>
        public int PhoneStateDate { get; init; }

        /// <summary>
        /// Gets the name of the pilot that distributed the call.
        /// </summary>
        public string PilotName { get; init; }

        /// <summary>
        /// Gets the name of the queue that distributed the call.
        /// </summary>
        public string QueueName { get; init; }

        /// <summary>
        /// Gets the number of times the agent has entered the withdrawal state.
        /// </summary>
        public int NbOfWithdrawals { get; init; }

        /// <summary>
        /// Gets the total duration in seconds that the agent was in the withdrawal state.
        /// </summary>
        public int WithdrawalsTotalDuration { get; init; }

        /// <summary>
        /// Gets the number of private calls handled by the agent.
        /// </summary>
        public int NbOfPrivateCalls { get; init; }

        /// <summary>
        /// Gets the total duration in seconds of private calls handled by the agent.
        /// </summary>
        public int PrivateCallsTotalDuration { get; init; }

        /// <summary>
        /// Gets the number of ACD calls handled by the agent.
        /// </summary>
        public int NbOfServedAcdCalls { get; init; }

        /// <summary>
        /// Gets the number of outgoing ACD calls placed by the agent.
        /// </summary>
        public int NbOfOutgoingAcdCalls { get; init; }

        /// <summary>
        /// Gets the number of ACD calls refused by the agent.
        /// </summary>
        public int NbOfRefusedAcdCalls { get; init; }

        /// <summary>
        /// Gets the number of ACD calls picked up by the agent.
        /// </summary>
        public int NbOfInterceptedAcdCalls { get; init; }

        /// <summary>
        /// Gets the number of ACD calls transferred by the agent.
        /// </summary>
        public int NbOfTransferredAcdCalls { get; init; }

        /// <summary>
        /// Gets the processing group in which the agent is logged in.
        /// </summary>
        public string CurrentProcessingGroup { get; init; }

        /// <summary>
        /// Gets the agent associated set.
        /// </summary>
        public string AssociatedSet { get; init; }

        /// <summary>
        /// Gets the withdraw reason.
        /// </summary>
        public int WithdrawReason { get; init; }

        /// <summary>
        /// Gets the CCD object key.
        /// </summary>
        public int AfeKey { get; init; }
    }
}
