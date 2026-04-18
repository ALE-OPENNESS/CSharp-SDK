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
using System.Text.Json.Serialization;

namespace o2g.Types.MaintenanceNS
{
    /// <summary>
    /// Represents a single system service running in the O2G environment.
    /// </summary>
    public class SystemService
    {
        /// <summary>The name of this system service.</summary>
        public string Name { get; init; }

        /// <summary>The current status of this system service (e.g. "running", "stopped").</summary>
        public string Status { get; init; }

        /// <summary>The mode in which this service is running (e.g. "active", "passive").</summary>
        public string Mode { get; init; }
    }

    /// <summary>
    /// Represents the status of all O2G system services on a server node.
    /// </summary>
    /// <seealso cref="SystemStatus.PrimaryServicesStatus"/>
    /// <seealso cref="SystemStatus.SecondaryServicesStatus"/>
    public class SystemServices
    {
        /// <summary>The individual system services running on this node.</summary>
        public List<SystemService> Services { get; init; }

        /// <summary>The status of the global IP address when the system is in HA mode.</summary>
        public string GlobalIPAdress { get; init; }

        /// <summary>The status of the DRBD service when the system is in HA mode.</summary>
        [JsonPropertyName("drbd")]
        public string DrbdStatus { get; init; }
    }
}
