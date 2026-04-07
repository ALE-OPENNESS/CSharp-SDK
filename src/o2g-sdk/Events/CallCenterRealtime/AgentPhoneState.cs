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


using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace o2g.Events.CallCenterRealtimeNS
{
    /// <summary>
    /// <c>AgentType</c> represents the possible type of agent in a realtime event.
    /// </summary>
    public enum AgentPhoneState
    {
        /// <summary>
        /// Idle
        /// </summary>
        Idle,

        /// <summary>
        /// For analog devices
        /// </summary>
        LineLockout,

        /// <summary>
        /// Out of service
        /// </summary>
        OutOfOrder,

        /// <summary>
        /// Ringing
        /// </summary>
        AcdRinging,

        /// <summary>
        /// Call in progress
        /// </summary>
        AcdConversation,

        /// <summary>
        /// In call or on hold, establishing another connection (double call) in progress
        /// </summary>
        AcdConsultation,

        /// <summary>
        /// Request for help
        /// </summary>
        Help,

        /// <summary>
        /// In conference
        /// </summary>
        AcdConference,

        /// <summary>
        /// Dialing his transaction code
        /// </summary>
        TransactionOnDial,

        /// <summary>
        /// In pause
        /// </summary>
        Pause,

        /// <summary>
        /// In wrap-up
        /// </summary>
        WrapUp,

        /// <summary>
        /// Only for supervisor : discreet listening
        /// </summary>
        SupervisorDiscreteListening,

        /// <summary>
        /// Agent "victim" of discreet listening
        /// </summary>
        AgentDiscreteListening,

        /// <summary>
        /// Recording call
        /// </summary>
        Recording,

        /// <summary>
        /// Log off
        /// </summary>
        LoggedOut,

        /// <summary>
        /// Agent put in hold by his correspondent or having put his correspondent on hold
        /// </summary>
        Held,

        /// <summary>
        /// In dialing
        /// </summary>
        Dialling,

        /// <summary>
        /// In ringing
        /// </summary>
        PrivateRinging,

        /// <summary>
        /// Local call
        /// </summary>
        PrivateLocalConversation,

        /// <summary>
        /// External call
        /// </summary>
        PrivateExternalConversation,

        /// <summary>
        /// Call established or on hold, and a second in the establishment phase or established
        /// </summary>
        PrivateConsultation,

        /// <summary>
        /// In conference
        /// </summary>
        PrivateConference,

        /// <summary>
        /// Busy tone
        /// </summary>
        BusyTone,

        /// <summary>
        /// Reserved by the attendant
        /// </summary>
        Reserved,

        /// <summary>
        /// Simple outgoing call
        /// </summary>
        AcdOutgoingConversation,

        /// <summary>
        /// Supervisor inaccessible because in "listening on agent"
        /// </summary>
        ContinuousSupervision,

        /// <summary>
        /// Fake call but does not block the GT (IVR)
        /// </summary>
        Unavailable,

        /// <summary>
        /// Unknown
        /// </summary>
        Unknown
    }
}
