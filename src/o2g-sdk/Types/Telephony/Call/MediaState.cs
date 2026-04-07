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
    /// <c>MediaState</c> represents a media state. A media can be audio or video.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: MediaState.Unknown)]
    public enum MediaState
    {
        /// <summary>
        /// Unknown media state. O2G server is unable to provide the information.
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown,
        /// <summary>
        /// The <c>OffHook</c> state is used when the device is busy for other reasons than a call; typically during service activation.
        /// </summary>
        [EnumMember(Value = "OFF_HOOK")]
        OffHook,
        /// <summary>
        /// Idle state, no activity on the media.
        /// </summary>
        [EnumMember(Value = "IDLE")]
        Idle,
        /// <summary>
        /// The call is releasing.
        /// </summary>
        [EnumMember(Value = "RELEASING")]
        Releasing,
        /// <summary>
        /// A make call is in progress.
        /// </summary>
        [EnumMember(Value = "DIALING")]
        Dialing,
        /// <summary>
        /// The call has been placed on hold.
        /// </summary>
        [EnumMember(Value = "HELD")]
        Held,
        /// <summary>
        /// An incoming call is ringing.
        /// </summary>
        [EnumMember(Value = "RINGING_INCOMING")]
        RingingIncoming,
        /// <summary>
        /// An outgoing call is in progress and the other party is ringing.
        /// </summary>
        [EnumMember(Value = "RINGING_OUTGOING")]
        RingingOutgoing,
        /// <summary>
        /// The call is active.
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        Active
    }
}
