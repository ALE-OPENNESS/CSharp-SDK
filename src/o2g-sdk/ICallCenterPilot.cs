/*
* Copyright 2024 ALE International
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
using o2g.Events.CallCenterPilot;
using o2g.Internal.Services;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// <c>ICallCenterPilot</c> allows an administrator to monitor CCD pilots.
    /// Using this service requires having a <b>CONTACTCENTER_SERVICE</b> license in CAPEX mode, or a minimum amount of 40 api-tel 
    /// subscriptions available in OPEX mode (Purple On Demand Offer)
    /// </summary>
    public interface ICallCenterPilot : IService
    {
        /// <summary>
        /// Occurs when a new call arrive on a CCD pilot.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotCallCreatedEvent>> PilotCallCreated;

        /// <summary>
        /// Occurs when a CCD call is routed in a queue.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotCallQueuedEvent>> PilotCallQueued;

        /// <summary>
        /// Occurs when a CCD call has been removed from the queue. Either being distributed or rerouted in case of queue overflow. 
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotCallRemovedEvent>> PilotCallRemoved;

        /// <summary>
        /// Start the monitoring on the specified pilot. 
        /// </summary>
        /// <param name="nodeId">The pro-acd device number.</param>
        /// <param name="pilotNumber">The pilot number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <seealso cref="monitorStop(int, string)"/>
        /// <seealso cref="OnPilotCallCreatedEvent"/>
        /// <seealso cref="OnPilotCallQueuedEvent"/>
        /// <seealso cref="OnPilotCallRemovedEvent"/>
        Task<bool> monitorStart(int nodeId, string pilotNumber);

        /// <summary>
        /// Stop the monitoring on the specified pilot. 
        /// </summary>
        /// <param name="nodeId">The pro-acd device number.</param>
        /// <param name="pilotNumber">The pilot number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <seealso cref="monitorStart(int, string)"/>
        Task<bool> monitorStop(int nodeId, string pilotNumber);

        /// <summary>
        /// Start the monitoring on the specified pilot. 
        /// </summary>
        /// <param name="pilotNumber">The pilot number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <seealso cref="monitorStop(string)"/>
        /// <seealso cref="OnPilotCallCreatedEvent"/>
        /// <seealso cref="OnPilotCallQueuedEvent"/>
        /// <seealso cref="OnPilotCallRemovedEvent"/>
        Task<bool> monitorStart(string pilotNumber);

        /// <summary>
        /// Stop the monitoring on the specified pilot. 
        /// </summary>
        /// <param name="pilotNumber">The pilot number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise</returns>
        /// <seealso cref="monitorStart(string)"/>
        Task<bool> monitorStop( string pilotNumber);
    }
}
