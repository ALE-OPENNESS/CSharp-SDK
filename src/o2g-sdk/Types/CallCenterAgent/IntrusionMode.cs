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

namespace o2g.Types.CallCenterAgentNS
{
    /// <summary>
    /// <c>IntrusionMode</c> represents the possible intrusion mode.
    /// </summary>
    /// <remarks>
    /// A supervisor can intrude in an established CCD call. The intrusion depends on the <c>IntrusionMode</c>.
    /// </remarks>
    /// <seealso cref="ICallCenterAgent.RequestIntrusionAsync(string, IntrusionMode, string)"/>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: IntrusionMode.Unknown)]
    public enum IntrusionMode
    {
        /// <summary>
        /// The supervisor can talk to both the agent and the remote customer.
        /// </summary>
        [EnumMember(Value = "NORMAL")]
        Normal,

        /// <summary>
        /// The supervisor can only talk to the agent but listen to the remote customer.
        /// </summary>
        [EnumMember(Value = "RESTRICTED")]
        Restricted,

        /// <summary>
        /// The supervisor only listens to the conversation between agent and customer.
        /// </summary>
        [EnumMember(Value = "DISCRETE")]
        Discrete,

        /// <summary>
        /// Unknown intrusion mode
        /// </summary>
        Unknown
    }
}
