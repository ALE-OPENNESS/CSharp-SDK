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

using System;
using System.Collections.Generic;

namespace o2g.Types.MaintenanceNS
{
    /// <summary>
    /// <c>SystemStatus</c> class provides a full status of the O2G server and its connections.
    /// </summary>
    /// <seealso cref="IMaintenance.GetSystemStatusAsync"/>
    public class SystemStatus
    {
        /// <summary>
        /// Return this O2G server logical address.
        /// </summary>
        /// <value>
        /// A <see cref="ServerAddress"/> that represents the O2G server logical address.
        /// </value>
        public ServerAddress LogicalAddress { get; init; }

        /// <summary>
        /// Return the system resource address of this O2G server.
        /// </summary>
        /// <value>
        /// A <see cref="ServerAddress"/> that provides access to internal server resources and monitoring endpoints.
        /// </value>
        public ServerAddress SystemResources { get; init; }

        /// <summary>
        /// Return the start date of the O2G server.
        /// </summary>
        /// <value>
        /// A <see langword="DateTime"/> object that represents the O2G server start date.
        /// </value>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Return whether this O2G is deployed in high availability mode.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the O2G is in HA mode; <see langword="false"/> otherwise.
        /// </value>
        public bool Ha { get; init; }

        /// <summary>
        /// Return the FQDN of the primary O2G server node.
        /// </summary>
        /// <value>
        /// The FQDN of the primary node, or <see langword="null"/> if not in HA mode.
        /// </value>
        public string Primary { get; init; }

        /// <summary>
        /// Return the software version of the primary server node.
        /// </summary>
        /// <value>
        /// The version string of the primary node, or <see langword="null"/> if not in HA mode.
        /// </value>
        public string PrimaryVersion { get; init; }

        /// <summary>
        /// Return the status of services running on the primary server node.
        /// </summary>
        /// <value>
        /// The <see cref="SystemServices"/> status of the primary node, or <see langword="null"/> if not in HA mode.
        /// </value>
        public SystemServices PrimaryServicesStatus { get; init; }

        /// <summary>
        /// Return the FQDN of the secondary O2G server node.
        /// </summary>
        /// <value>
        /// The FQDN of the secondary node, or <see langword="null"/> if not in HA mode.
        /// </value>
        public string Secondary { get; init; }

        /// <summary>
        /// Return the software version of the secondary server node.
        /// </summary>
        /// <value>
        /// The version string of the secondary node, or <see langword="null"/> if not in HA mode.
        /// </value>
        public string SecondaryVersion { get; init; }

        /// <summary>
        /// Return the status of services running on the secondary server node.
        /// </summary>
        /// <value>
        /// The <see cref="SystemServices"/> status of the secondary node, or <see langword="null"/> if not in HA mode.
        /// </value>
        public SystemServices SecondaryServicesStatus { get; init; }

        /// <summary>
        /// Return the list of OmniPCX Enterprise nodes connected to this O2G server.
        /// </summary>
        /// <value>
        /// A list of <see cref="PbxStatus"/> that represents the connected nodes and their connection status.
        /// </value>
        public List<PbxStatus> Pbxs { get; init; }

        /// <summary>
        /// Return the license status of this O2G server.
        /// </summary>
        /// <value>
        /// A <see cref="LicenseStatus"/> object that represents the O2G server license information.
        /// </value>
        public LicenseStatus License { get; init; }

        /// <summary>
        /// Return the O2G server configuration type.
        /// </summary>
        /// <value>
        /// The <see cref="ConfigurationType"/> that corresponds to the O2G server configuration.
        /// </value>
        public ConfigurationType ConfigurationType { get; init; }

        /// <summary>
        /// Return the application identifier of this O2G server instance.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that uniquely identifies this O2G instance within a deployment.
        /// </value>
        public string ApplicationId { get; init; }

        /// <summary>
        /// Return the subscriber filter policy applied by this O2G server.
        /// </summary>
        /// <value>
        /// The <see cref="SubscriberFilter"/> controlling which OmniPCX Enterprise subscribers are automatically imported.
        /// </value>
        public SubscriberFilter SubscriberFilter { get; init; }
    }
}
