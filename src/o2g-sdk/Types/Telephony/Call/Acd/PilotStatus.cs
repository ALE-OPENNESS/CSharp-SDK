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

namespace o2g.Types.TelephonyNS.CallNS.AcdNS
{
    /// <summary>
    /// <c>PilotStatus</c> represents the possible state of a CCD pilot.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: PilotStatus.Other)]
    public enum PilotStatus
    {
        /// <summary>
        /// The pilot is opened.
        /// </summary>
        [EnumMember(Value = "OPEN")]
        Opened,

        /// <summary>
        /// The pilot is blocked.
        /// </summary>
        [EnumMember(Value = "BLOCKED")]
        Blocked,

        /// <summary>
        /// The pilot is on a blocked on a rule.
        /// </summary>
        [EnumMember(Value = "BLOCKED_ON_RULE")]
        BlockedOnRule,

        /// <summary>
        /// The pilot is blocked on a blocking rule.
        /// </summary>
        [EnumMember(Value = "BLOCKED_ON_BLOCKED_RULE")]
        BlockedOnBlockedRule,

        /// <summary>
        /// The pilot is in general forwarding.
        /// </summary>
        [EnumMember(Value = "GENERAL_FORWARDING")]
        GeneralForwarding,

        /// <summary>
        /// The pilot is in general forwarding on a rule.
        /// </summary>
        [EnumMember(Value = "GENERAL_FORWARDING_ON_RULE")]
        GeneralForwardingOnRule,

        /// <summary>
        /// The pilot is blocked while in general forwarding on a rule.
        /// </summary>
        [EnumMember(Value = "BLOCKED_ON_GENERAL_FORWARDING_RULE")]
        BlockedOnGGeneralForwardingRule,

        /// <summary>
        /// Other state
        /// </summary>
        [EnumMember(Value = "OTHER")]
        Other
    }
}
