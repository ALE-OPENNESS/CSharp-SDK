/*
* Copyright 2026 ALE International
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

namespace o2g.Types.MaintenanceNS
{
    /// <summary>
    /// Defines the policy used by O2G to automatically load users from OmniPCX Enterprise subscribers.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: SubscriberFilter.Unknown)]
    public enum SubscriberFilter
    {
        /// <summary>Only OXE subscribers with the A4980 attribute are automatically loaded.</summary>
        [EnumMember(Value = "A4980")] A4980,

        /// <summary>All OXE subscribers are automatically loaded.</summary>
        [EnumMember(Value = "ALL")] All,

        /// <summary>No OXE subscribers are automatically loaded.</summary>
        [EnumMember(Value = "NONE")] None,

        /// <summary>The subscriber filter policy could not be determined.</summary>
        [EnumMember(Value = "UNKNOWN")] Unknown
    }
}
