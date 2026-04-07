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

namespace o2g.Types.TelephonyNS.CallNS
{
    /// <summary>
    /// <c>Cause</c> lists the different call causes.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Cause.Unknown)]
    public enum Cause
    {
        /// <summary>
        /// Caller in a two-party call has disconnected before the call was answered.
        /// </summary>
        [EnumMember(Value = "ABANDONED")]
        Abandoned,

        /// <summary>
        /// The call is receiving the network congestion tone.
        /// </summary>
        [EnumMember(Value = "ALL_TRUNK_BUSY")]
        AllTrunkBusy,

        /// <summary>
        /// The call is receiving the busy tone.
        /// </summary>
        [EnumMember(Value = "BUSY")]
        Busy,

        /// <summary>
        /// One party in a two-party call has disconnected after the call was answered.
        /// </summary>
        [EnumMember(Value = "CLEARED")]
        Cleared,

        /// <summary>
        /// One party has left the conference call.
        /// </summary>
        [EnumMember(Value = "PARTICIPANT_LEFT")]
        ParticipantLeft,

        /// <summary>
        /// This is a multi-party call.
        /// </summary>
        [EnumMember(Value = "CONFERENCED")]
        Conferenced,

        /// <summary>
        /// The call is receiving the invalid number tone.
        /// </summary>
        [EnumMember(Value = "INVALID_NUMBER")]
        InvalidNumber,

        /// <summary>
        /// The destination cannot be reached.
        /// </summary>
        [EnumMember(Value = "DESTINATION_NOT_OBTAINABLE")]
        DestinationNotObtainable,

        /// <summary>
        /// The destination is in do not disturb.
        /// </summary>
        [EnumMember(Value = "DO_NOT_DISTURB")]
        DoNotDisturb,

        /// <summary>
        /// The call has been forwarded.
        /// </summary>
        [EnumMember(Value = "FORWARDED")]
        Forwarded,

        /// <summary>
        /// The call has been picked up.
        /// </summary>
        [EnumMember(Value = "PICKED_UP")]
        PickedUp,

        /// <summary>
        /// The call has been parked.
        /// </summary>
        [EnumMember(Value = "PARKED")]
        Parked,

        /// <summary>
        /// The call has been redirected.
        /// </summary>
        [EnumMember(Value = "REDIRECTED")]
        Redirected,

        /// <summary>
        /// This call goes on overflow destination.
        /// </summary>
        [EnumMember(Value = "OVERFLOWN")]
        Overflown,

        /// <summary>
        /// This is a transferred call.
        /// </summary>
        [EnumMember(Value = "TRANSFERRED")]
        Transferred,

        /// <summary>
        /// The call has been put in wait..
        /// </summary>
        [EnumMember(Value = "CAMP_ON")]
        CampOn,

        /// <summary>
        /// Unknown cause.
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown,

        /// <summary>
        /// Picked up tandem.
        /// </summary>
        [EnumMember(Value = "PICKED_UP_TANDEM")]
        PickedUpTandem,

        /// <summary>
        /// The call is a call back.
        /// </summary>
        [EnumMember(Value = "CALL_BACK")]
        CallBack,

        /// <summary>
        /// The call is recall (e.g. on HELD call indicates that device rings back).
        /// </summary>
        [EnumMember(Value = "RECALL")]
        Recall,

        /// <summary>
        /// CCD context: call distribution.
        /// </summary>
        [EnumMember(Value = "DISTRIBUTED")]
        Distributed,

        /// <summary>
        /// CCD context: call enters in distribution.
        /// </summary>
        [EnumMember(Value = "ACD_ENTER_DISTRIBUTION")]
        AcdEnterDistribution,

        /// <summary>
        /// CCD context: pilot is not open.
        /// </summary>
        [EnumMember(Value = "RESOURCES_NOT_AVAILABLE")]
        ResourcesNotAvailable,

        /// <summary>
        /// CCD context: supervisor is listening the agent conversation.
        /// </summary>
        [EnumMember(Value = "SUPERVISOR_LISTENING")]
        SupervisorListening,

        /// <summary>
        /// CCD context: supervisor is fully intruded in the agent conversation
        /// </summary>
        [EnumMember(Value = "SUPERVISOR_INTRUSION")]
        SupervisorIntrusion,

        /// <summary>
        /// CCD context: supervisor can speak to the agent
        /// </summary>
        [EnumMember(Value = "SUPERVISOR_RESTRICT_INTRUSION")]
        SupervisorRestrictIntrusion,

        /// <summary>
        /// CCD context: No available agent
        /// </summary>
        [EnumMember(Value = "NO_AVAILABLE_AGENT")]
        NoAvailableAgent,

        /// <summary>
        /// Physical phone set device is off the hook
        /// </summary>
        [EnumMember(Value = "LOCKOUT")]
        Lockout
    }
}
