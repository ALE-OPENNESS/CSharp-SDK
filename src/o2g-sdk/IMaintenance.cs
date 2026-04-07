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
    /// <c>IMaintenance</c> service is used to retrieve information about the system state, in particular, information on the 
    /// OmniPCX Enterprise nodes and their connection state.
    /// <c>IMaintenance</c> service doesn't require any specific license on the O2G server.
    /// <para>
    /// It also provides informations about licenses: total allocated licenses, numbers of current used and expiration date.
    /// </para>
    /// </summary>
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
        /// Occurs when datas are fully loaded from an OmniPCX Enterprise node. 
        /// </summary>
        public event EventHandler<O2GEventArgs<OnPbxLoadedEvent>> PbxLoaded;

        /// <summary>
        /// Get information about system status. 
        /// <para>
        /// This operation provides information about the system state, and the total number of each license available 
        /// for the system. This operation is restricted to an admininistrator only.
        /// </para>
        /// </summary>
        /// <returns>
        /// A <see cref="SystemStatus"/> objects containing system information.
        /// </returns>
        Task<SystemStatus> GetSystemStatusAsync();
    }
}
