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
using o2g.Events.CallCenterRealtimeNS;
using o2g.Internal.Services;
using o2g.Types.CallCenterRealtimeNS;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// Provides realtime information on a CCD through the <see cref="ICallCenterRealtime"/> service.
    /// </summary>
    /// <remarks>
    /// Using this service requires a <c>CONTACTCENTER_SERVICES</c> license.
    /// </remarks>
    public interface ICallCenterRealtime : IService
    {
        /// <summary>
        /// Occurs each time an agent realtime data change.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnAgentRtiChangedEvent>> AgentRtiChanged;

        /// <summary>
        /// Occurs each time a pilot realtime data change.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotRtiChangedEvent>> PilotRtiChanged;

        /// <summary>
        /// Occurs each time a queue realtime data change.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnQueueRtiChangedEvent>> QueueRtiChanged;

        /// <summary>
        /// Occurs each time an agent processing group realtime data change.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPGAgentRtiChangedEvent>> PGAgentRtiChanged;

        /// <summary>
        /// Occurs each time a processing group, other than agent, realtime data change.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPGOtherRtiChangedEvent>> PGOtherRtiChanged;


        /// <summary>
        /// Gets the directory numbers of all CCD objects that provide realtime information.
        /// </summary>
        /// <returns>An <see cref="RtiObjects"/> instance containing the CCD objects.</returns>
        Task<RtiObjects> GetRtiObjectsAsync();


        /// <summary>
        /// Updates the RTI context associated with this administrator. 
        /// If there is no associated RTI context, a new one is created.
        /// </summary>
        /// <param name="context">The RTI context to update.</param>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> UpdateContextAsync(RtiContext context);

        /// <summary>
        /// Deletes the RTI context associated with this administrator.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        Task<bool> DeleteContextAsync();

        /// <summary>
        /// Gets the RTI context associated with this administrator.
        /// </summary>
        /// <returns>The current <see cref="RtiContext"/> for this administrator, or <c>null</c> if none exists.</returns>
        Task<RtiContext> GetContextAsync();
    }
}
