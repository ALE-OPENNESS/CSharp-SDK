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

using o2g.Events;
using o2g.Events.Maintenance;
using o2g.Internal.Services;
using o2g.Types.MaintenanceNS;
using System;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// The <c>IMaintenance</c> service provides information about the system state, in particular information on the
    /// OmniPCX Enterprise nodes and their connection state. Information about licenses is also provided per item:
    /// total allocated licenses, number currently in use, and expiration date.
    /// </summary>
    /// <remarks>
    /// This service does not require any specific license on the O2G server.
    /// </remarks>
    public interface IMaintenance : IService
    {
        /// <summary>
        /// Occurs when a CTI link is down.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnCtiLinkDownEvent>> CtiLinkDown;

        /// <summary>
        /// Occurs when a CTI link is up.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnCtiLinkUpEvent>> CtiLinkUp;

        /// <summary>
        /// Occurs when data is fully loaded from an OmniPCX Enterprise node.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnPbxLoadedEvent>> PbxLoaded;

        /// <summary>
        /// Occurs when the CMIS link to an OmniPCX Enterprise node goes down.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnPbxLinkDownEvent>> PbxLinkDown;

        /// <summary>
        /// Occurs when the CMIS link to an OmniPCX Enterprise node is re-established.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnPbxLinkUpEvent>> PbxLinkUp;

        /// <summary>
        /// Occurs when the connection to the remote twin O2G server is lost.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnRemoteServerLinkDownEvent>> RemoteServerLinkDown;

        /// <summary>
        /// Occurs when the connection to the remote twin O2G server is recovered.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnRemoteServerLinkUpEvent>> RemoteServerLinkUp;

        /// <summary>
        /// Occurs when the O2G server has started (all OmniPCX Enterprise nodes are connected and loaded).
        /// </summary>
        public event EventHandler<O2GEventArgs<OnServerStartEvent>> ServerStart;

        /// <summary>
        /// Occurs when a license is about to expire or has expired.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnLicenseExpirationEvent>> LicenseExpiration;

        /// <summary>
        /// Retrieves information about the system state and the total number of each license type available for the system.
        /// </summary>
        /// <returns>
        /// A <see cref="SystemStatus"/> object on success, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// This operation is restricted to an administrator session only.
        /// </remarks>
        Task<SystemStatus> GetSystemStatusAsync();
    }
}
