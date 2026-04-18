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
    /// </summary>
    /// <remarks>
    /// Monitoring a pilot consists of starting the monitoring with <see cref="MonitorStartAsync(string)"/>,
    /// then receiving events on calls arriving on the pilot, calls being queued, and calls being removed
    /// from the queue. When monitoring is no longer needed, stop it with <see cref="MonitorStopAsync(string)"/>.
    /// <para>
    /// Using this service requires having a <b>CONTACTCENTER_SERVICE</b> license in CAPEX mode,
    /// or 40 api-tel-f subscriptions in OPEX mode (Purple On Demand).
    /// </para>
    /// </remarks>
    public interface ICallCenterPilot : IService
    {
        /// <summary>
        /// Occurs when a new call arrives on a CCD pilot.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotCallCreatedEvent>> PilotCallCreated;

        /// <summary>
        /// Occurs when a CCD call is routed into a queue.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotCallQueuedEvent>> PilotCallQueued;

        /// <summary>
        /// Occurs when a CCD call has been removed from the queue, either by distribution, cancellation, or overflow.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotCallRemovedEvent>> PilotCallRemoved;

        /// <summary>
        /// Starts the monitoring of the specified pilot.
        /// </summary>
        /// <param name="pilotNumber">The pilot number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>If the pilot is already being monitored, no error is returned.</remarks>
        /// <seealso cref="MonitorStopAsync(string)"/>
        /// <seealso cref="OnPilotCallCreatedEvent"/>
        /// <seealso cref="OnPilotCallQueuedEvent"/>
        /// <seealso cref="OnPilotCallRemovedEvent"/>
        Task<bool> MonitorStartAsync(string pilotNumber);

        /// <summary>
        /// Stops the monitoring of the specified pilot.
        /// </summary>
        /// <param name="pilotNumber">The pilot number.</param>
        /// <returns><see langword="true"/> in case of success; <see langword="false"/> otherwise.</returns>
        /// <remarks>If the pilot is not being monitored, no error is returned.</remarks>
        /// <seealso cref="MonitorStartAsync(string)"/>
        Task<bool> MonitorStopAsync(string pilotNumber);
    }
}
