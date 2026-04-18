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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// Provides real-time information about CCD objects from an OXE system in the form of events.
    /// </summary>
    /// <remarks>
    /// This service is available only to administrators and delivers the same level of information
    /// as the legacy RTI interface available in the CCS.
    /// <para>
    /// The CCD objects that can be monitored include CCD agents, pilots, waiting queues, processing
    /// groups associated with agents, and other processing groups (e.g. forward, guide).
    /// </para>
    /// <para>
    /// Typical usage sequence:
    /// <list type="number">
    ///   <item>Build an <see cref="RtiFilter"/> specifying which objects and attributes to monitor.</item>
    ///   <item>Create an <see cref="RtiContext"/> with the filter and the desired notification frequency.</item>
    ///   <item>Set the context with <see cref="SetContextAsync"/>.</item>
    ///   <item>Register event handlers for the RTI events of interest.</item>
    ///   <item>Start monitoring with <see cref="StartAsync"/>.</item>
    /// </list>
    /// </para>
    /// <para>
    /// After initialization, the application is notified whenever one or more monitored attributes
    /// change. Each event contains only the attributes that have changed since the previous notification.
    /// </para>
    /// <para>
    /// Access to this service requires a valid license: <c>CONTACTCENTER_SERVICE</c> in CAPEX mode,
    /// or 40 api-tel-f subscriptions in OPEX mode (Purple On Demand).
    /// </para>
    /// </remarks>
    public interface ICallCenterRealtime : IService
    {
        /// <summary>
        /// Occurs when the real-time information of a CCD agent has changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnAgentRtiChangedEvent>> AgentRtiChanged;

        /// <summary>
        /// Occurs when the real-time information of a CCD pilot has changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPilotRtiChangedEvent>> PilotRtiChanged;

        /// <summary>
        /// Occurs when the real-time information of a CCD queue has changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnQueueRtiChangedEvent>> QueueRtiChanged;

        /// <summary>
        /// Occurs when the real-time information of a CCD agent processing group has changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPGAgentRtiChangedEvent>> PGAgentRtiChanged;

        /// <summary>
        /// Occurs when the real-time information of a CCD other processing group has changed.
        /// </summary>
        public event System.EventHandler<O2GEventArgs<OnPGOtherRtiChangedEvent>> PGOtherRtiChanged;


        /// <summary>
        /// Retrieves all CCD objects that currently provide real-time information.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="RtiObjects"/> includes collections of agents, pilots, queues,
        /// and processing groups that can be monitored.
        /// </remarks>
        /// <returns>
        /// An <see cref="RtiObjects"/> instance containing the CCD objects, or <see langword="null"/>
        /// if no objects are available or an error occurs.
        /// </returns>
        Task<RtiObjects> GetRtiObjectsAsync();

        /// <summary>
        /// Retrieves all CCD agents that provide real-time information.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="RtiObjectIdentifier"/> representing agents,
        /// or an empty list if none exist.
        /// </returns>
        Task<IReadOnlyList<RtiObjectIdentifier>> GetAgentsAsync();

        /// <summary>
        /// Retrieves all CCD pilots that provide real-time information.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="RtiObjectIdentifier"/> representing pilots,
        /// or an empty list if none exist.
        /// </returns>
        Task<IReadOnlyList<RtiObjectIdentifier>> getPilotsAsync();

        /// <summary>
        /// Retrieves all CCD queues that provide real-time information.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="RtiObjectIdentifier"/> representing queues,
        /// or an empty list if none exist.
        /// </returns>
        Task<IReadOnlyList<RtiObjectIdentifier>> getQueuesAsync();

        /// <summary>
        /// Retrieves all CCD agent processing groups that provide real-time information.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="RtiObjectIdentifier"/> representing agent processing groups,
        /// or an empty list if none exist.
        /// </returns>
        Task<IReadOnlyList<RtiObjectIdentifier>> getAgentProcessingGroupsAsync();

        /// <summary>
        /// Retrieves all CCD processing groups (other than agents) that provide real-time information.
        /// </summary>
        /// <returns>
        /// A read-only list of <see cref="RtiObjectIdentifier"/> representing other processing groups,
        /// or an empty list if none exist.
        /// </returns>
        Task<IReadOnlyList<RtiObjectIdentifier>> getOtherProcessingGroupsAsync();

        /// <summary>
        /// Returns the monitoring context associated with this administrator.
        /// </summary>
        /// <returns>
        /// The current <see cref="RtiContext"/> for this administrator, or <see langword="null"/> if none exists.
        /// </returns>
        Task<RtiContext> GetContextAsync();

        /// <summary>
        /// Deletes the monitoring context associated with this administrator, stopping any RTI event notifications.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the deletion was successful; <see langword="false"/> otherwise.
        /// </returns>
        Task<bool> DeleteContextAsync();

        /// <summary>
        /// Associates or updates the monitoring context for this administrator.
        /// If no context exists, a new one is created.
        /// </summary>
        /// <remarks>
        /// The context defines which objects and attributes are monitored and the notification frequency
        /// for RTI events.
        /// </remarks>
        /// <param name="context">The <see cref="RtiContext"/> to associate with this administrator.</param>
        /// <returns>
        /// <see langword="true"/> if the update was successful; <see langword="false"/> otherwise.
        /// </returns>
        Task<bool> SetContextAsync(RtiContext context);

        /// <summary>
        /// Starts the monitoring of CCD objects according to the associated context.
        /// </summary>
        /// <remarks>
        /// After calling this method, RTI events will be raised to any registered event handlers.
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if the monitoring started successfully; <see langword="false"/> otherwise.
        /// </returns>
        Task<bool> StartAsync();
    }
}
