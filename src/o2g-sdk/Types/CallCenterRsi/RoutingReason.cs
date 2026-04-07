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

namespace o2g.Types.CallCenterRsiNS
{
    /// <summary>
    /// <c>RoutingReason</c> represents the possible reason associated to a route request.
    /// </summary>
    /// <seealso cref="RouteRequest"/>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: RoutingReason.Unknown)]
    public enum RoutingReason
    {
        /// <summary>
        /// First route request.
        /// </summary>
        [EnumMember(Value = "INITIAL")]
        Initial,

        /// <summary>
        /// The call is rerouted because of no answer from initial route.
        /// </summary>
        [EnumMember(Value = "NOANSWER")]
        NoAnswer,

        /// <summary>
        /// The call is rerouted because of busy initial route.
        /// </summary>
        [EnumMember(Value = "BUSY")]
        Busy,

        /// <summary>
        /// The call is rerouted because of not obtainable initial route.
        /// </summary>
        [EnumMember(Value = "NOT_OBTAINABLE")]
        NotObainable,

        /// <summary>
        /// Unknown reason.
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown
    }
}
