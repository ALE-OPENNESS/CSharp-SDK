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

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace o2g.Types.MaintenanceNS
{
    /// <summary>
    /// Defines the license control mode for the O2G system.
    /// </summary>
    public enum LicenseType
    {
        /// <summary>License controlled via an external FlexLM server (CAPEX mode).</summary>
        [EnumMember(Value = "FLEXLM")] Flexlm,

        /// <summary>License controlled via the License Manager Server (OPEX mode).</summary>
        [EnumMember(Value = "LMS")] Lms
    }

    /// <summary>
    /// Represents the license status of the O2G server.
    /// </summary>
    /// <seealso cref="SystemStatus.License"/>
    public class LicenseStatus
    {
        /// <summary>The license control mode.</summary>
        public LicenseType? Type { get; init; }

        /// <summary>The operational context for this license (e.g. "production", "test").</summary>
        public string Context { get; init; }

        /// <summary>The hostname or identifier of the currently active license server.</summary>
        public string CurrentServer { get; init; }

        /// <summary>The overall license status (e.g. "active", "expired").</summary>
        public string Status { get; init; }

        /// <summary>A detailed status message providing additional information about the license state.</summary>
        public string StatusMessage { get; init; }

        /// <summary>The individual licenses associated with this server.</summary>
        [JsonPropertyName("lics")]
        public List<License> Licenses { get; init; }
    }
}
