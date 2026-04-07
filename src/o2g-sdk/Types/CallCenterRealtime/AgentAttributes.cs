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
    /// Represents the possible attributes for a CCD agent.
    /// <para>
    /// CallCenterRealtimeService can report realtime on each of these attributes.
    /// </para>
    /// </summary>
    public enum AgentAttributes
    {
        /// <summary>
        /// Associated Set (pro acd).
        /// </summary>
        AssociatedSet,

        /// <summary>
        /// Current processing group.
        /// </summary>
        CurrentPG,

        /// <summary>
        /// Phone state (ringing, talking, ...).
        /// </summary>
        PhoneState,

        /// <summary>
        /// UTC logon date.
        /// </summary>
        LogonDate,

        /// <summary>
        /// Duration of all private calls (in seconds).
        /// </summary>
        PrivateCallsTotalDuration,

        /// <summary>
        /// UTC date of entering communication.
        /// </summary>
        ComDate,

        /// <summary>
        /// Duration of communication.
        /// </summary>
        ComDuration,

        /// <summary>
        /// Number of private calls.
        /// </summary>
        NBOfPrivateCalls,

        /// <summary>
        /// Number of answered ACD calls.
        /// </summary>
        NbOfServedACDCalls,

        /// <summary>
        /// Number of non-answered ACD calls.
        /// </summary>
        NbOfRefusedACDCalls,

        /// <summary>
        /// Number of transferred ACD calls.
        /// </summary>
        NbOfTransferedACDCalls,

        /// <summary>
        /// Number of outgoing ACD calls.
        /// </summary>
        NbOfOutgoingACDCalls,

        /// <summary>
        /// Number of picked-up ACD calls.
        /// </summary>
        NbOfInterceptedACDCalls,

        /// <summary>
        /// Agent service state (loggedIn, assigned, ...).
        /// </summary>
        ServiceState,

        /// <summary>
        /// Number of withdrawals.
        /// </summary>
        NbOfWithdrawals,

        /// <summary>
        /// Total duration of withdrawals.
        /// </summary>
        WithdrawalsTotalDuration,

        /// <summary>
        /// Last withdrawal reason.
        /// </summary>
        WithdrawReason,

        /// <summary>
        /// Pilot name.
        /// </summary>
        PilotName,

        /// <summary>
        /// Queue name.
        /// </summary>
        QueueName,

        /// <summary>
        /// All attributes.
        /// </summary>
        ALL
    }
}
