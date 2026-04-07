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

namespace o2g.Types.CommunicationLogNS
{
    /// <summary>
    /// <c>Reason</c> defines the why reason the communication has been released, established or rerouted.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Reason.Unknown)]
    public enum Reason
    {
        /// <summary>
        /// The call was abandonned because there was no Available trunk.
        /// </summary>
        [EnumMember(Value = "ALL_TRUNK_BUSY")]
        AllTrunkBusy,

        /// <summary>
        /// The call was refused because the dialed number is not valid.
        /// </summary>
        [EnumMember(Value = "INVALID_NUMBER")]
        InvalidNumber,

        /// <summary>
        /// The call was canceled by the caller.
        /// </summary>
        [EnumMember(Value = "ABANDONED")]
        Abandoned,

        /// <summary>
        /// The call failed because the called party is busy.
        /// </summary>
        [EnumMember(Value = "BUSY")]
        Busy,

        /// <summary>
        /// The call was set to be a conference.
        /// </summary>
        [EnumMember(Value = "CONFERENCED")]
        Conferenced,

        /// <summary>
        /// The call was picked up.
        /// </summary>
        [EnumMember(Value = "PICKUP")]
        Pickup,

        /// <summary>
        /// The call was forwarded to another destination.
        /// </summary>
        [EnumMember(Value = "FORWARDED")]
        Forwarded,

        /// <summary>
        /// The call was redirected to another destination.
        /// </summary>
        [EnumMember(Value = "REDIRECTED")]
        Redirected,

        /// <summary>
        /// The call was released since redirection to another destination fails.
        /// </summary>
        [EnumMember(Value = "RELEASED_ON_REDIRECT")]
        ReleasedOnRedirect,

        /// <summary>
        /// The call was transferred.
        /// </summary>
        [EnumMember(Value = "TRANSFERRED")]
        Transferred,

        /// <summary>
        /// The call was released since transfer to another destination fails.
        /// </summary>
        [EnumMember(Value = "RELEASED_ON_TRANSFER")]
        ReleasedOnTransfer,

        /// <summary>
        /// The call ended on voicemail.
        /// </summary>
        [EnumMember(Value = "VOICEMAIL")]
        VoiceMail,

        /// <summary>
        /// The call normally ended.
        /// </summary>
        [EnumMember(Value = "NORMAL")]
        Normal,

        /// <summary>
        /// The reason is unknown.
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown
    }
}
