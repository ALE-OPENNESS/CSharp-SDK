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
using o2g.Events.CallCenterAgent;
using o2g.Events.CallCenterPilot;
using o2g.Events.CommunicationLog;
using o2g.Events.EventSummary;
using o2g.Events.Maintenance;
using o2g.Events.Management;
using o2g.Events.Routing;
using o2g.Events.Telephony;
using o2g.Internal.Events;
using System.Collections.Generic;
using System.Linq;

namespace o2g
{
    /// <summary>
    /// This delegate represent the default delegate that can be used to receive event.
    /// </summary>
    /// <param name="o2gEvent"></param>
    internal delegate void OnEvent(O2GEvent o2gEvent);

    /// <summary>
    /// <c>Subscription</c> represents a subscription request used to subscribe events from the O2G Server.
    /// <para>
    /// The application builds a subscription using the <see cref="Builder"/> object. This object provides a builder pattern that eases
    /// the creation of a subscription.
    /// </para>
    /// </summary>
    /// <example>
    /// To create a subscription, uses the methods provided by the <c>Builder</c> object.
    /// <code>
    ///         Subscription subscription = Subscription.Builder
    ///                                         .AddRoutingEvents()         // Add routing service events
    ///                                         .AddTelephonyEvents()       // Add telephony service events
    ///                                         .AddEventSummaryEvents()    // Add event summary events
    ///                                         .SetTimeout(10)             // Set the chunk connection timeout
    ///                                         .Build();                   // Build the subscription
    /// </code>
    /// </example>
    /// <seealso cref="O2G.Application.SubscribeAsync(Subscription)"/>
    public abstract class Subscription
    {
        /// <summary>
        /// <c>EventFilter</c> represents a filter used to subscribe to O2G events.
        /// </summary>
        public class EventFilter
        {
            public class Selector
            {
                public List<string> Ids { get; init; }
                public List<string> Names { get; init; }
            }

            public List<Selector> Selectors { get; set; }


            private List<Selector> NotNullSelectors()
            {
                if (Selectors == null)
                {
                    Selectors = new();
                }
                return Selectors;
            }

            private EventFilter Add(string[] ids, List<string> names)
            {
                NotNullSelectors().Add(new()
                {
                    Ids = new List<string>(ids),
                    Names = names
                });
                return this;
            }
            private EventFilter Add(List<string> ids, string[] names)
            {
                NotNullSelectors().Add(new()
                {
                    Ids = ids,
                    Names = new List<string>(names),
                });
                return this;
            }
            private EventFilter Add(List<string> ids, List<string> names)
            {
                NotNullSelectors().Add(new()
                {
                    Ids = ids,
                    Names = names
                });
                return this;
            }
            private EventFilter Add(string[] ids, List<EventPackage> eventPackages)
            {
                return Add(ids, eventPackages.Select(ev => ev.Value).ToList());
            }
            private EventFilter Add(List<EventPackage> eventPackages)
            {
                return Add(eventPackages.Select(ev => ev.Value).ToList());
            }

            private EventFilter Add(List<string> names)
            {
                NotNullSelectors().Add(new()
                {
                    Names = names
                });
                return this;
            }

            private EventFilter Add(string[] ids, string[] names)
            {
                return Add(new List<string>(ids), new List<string>(names));
            }

            private EventFilter Add(string[] ids, EventPackage[] eventPackages)
            {
                return Add(ids, eventPackages.ToList().Select(ev => ev.Value).ToList());
            }
            private EventFilter Add(string[] names)
            {
                return Add(new List<string>(names));
            }
            private EventFilter Add(string id, string name)
            {
                return Add(new[] { id }, new[] { name });
            }

            private EventFilter Add(string id, EventPackage eventPackage)
            {
                return Add(id, eventPackage.Value);
            }

            private EventFilter Add(string name)
            {
                return Add(new[] { name });
            }

            /// <summary>
            /// Add the specified eventPackage to the subscription.
            /// </summary>
            /// <param name="ids">The id to filter on.</param>
            /// <param name="eventPackage">The event package.</param>
            /// <returns>
            /// The <see cref="EventFilter"/> this event package has been added to.
            /// </returns>
            public EventFilter Add(string[] ids, EventPackage eventPackage)
            {
                return Add(ids, new[] { eventPackage.Value });
            }

            /// <summary>
            /// Add the specified eventPackage to the subscription.
            /// </summary>
            /// <param name="eventPackage">The event package.</param>
            /// <returns>
            /// The <see cref="EventFilter"/> this event package has been added to.
            /// </returns>
            public EventFilter Add(EventPackage eventPackage)
            {
                return Add(eventPackage.Value);
            }
        }

        /// <summary>
        /// Return the version of the events to subscribe to.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the events version.
        /// </value>
        /// <remarks>
        /// By default the event version is "1.0".
        /// </remarks>
        public abstract string Version { get; }

        /// <summary>
        /// Return the default lifetime of the event channel.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the events channel lifetime in minute.
        /// </value>
        /// <remarks>
        /// <para>
        /// When chunk eventing is used, the server closes the connection in case of no activity after the timeout. In this case, the event channel
        /// is automatically re-established.
        /// </para>
        /// <para>
        /// Allowed values are 2 minutes and 60 minutes, with an exception for the value 0.
        /// For this special case, the lifetime of the notification channel is not maintained by the given timeout, 
        /// but depends on the lifetime of the user's session. A KeepAlive event is sent by the server to maintain the connection. 
        /// </para>
        /// </remarks>
        public abstract int Timeout { get; }

        /// <summary>
        /// Return the filter associated to this subscription.
        /// </summary>
        /// <value>
        /// A <see cref="EventFilter"/> object that represents the event filter for this subscription.
        /// </value>
        public abstract EventFilter Filter { get; }

        /// <summary>
        /// Returns the Webhook URL associated with this subscription, or <see langword="null"/> if
        /// chunk eventing is used instead.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> containing the Webhook URL, or <see langword="null"/> when
        /// chunk eventing is configured.
        /// </value>
        public abstract string WebHookUrl { get; }

        /// <summary>
        /// Return a subscription builder.
        /// </summary>
        /// <value>
        /// A <see cref="IBuilder"/> object used to build a subscription.
        /// </value>
        /// <remarks>
        /// The <c>IBuilder</c> object implements a builder pattern to easily build a subscription.
        /// </remarks>
        public static IBuilder Builder
        {
            get => new SubscriptionBuilderImpl();
        }

        /// <summary>
        /// The <c>IBuilder</c> interface provides methods to easily build a subscription.
        /// </summary>
        public interface IBuilder
        {
            /// <summary>
            /// Add the routing events to this subscription.
            /// </summary>
            /// <param name="ids">The Ids to filter events on.</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following event associated to the <see cref="IRouting"/> service is added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnRoutingStateChangedEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the routing event requires having a <b>TELEPHONY_ADVANCED</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddRoutingEvents(string[] ids = null);

            /*
            /// <summary>
            /// Add the rsi events to this subscription.
            /// </summary>
            /// <param name="ids">The Ids to filter events on.</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following event associated to the <see cref="ICallCenterRsi"/> service is added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnDigitCollectedEvent"/></description></item>
            /// <item><description><see cref="OnRouteEndEvent"/></description></item>
            /// <item><description><see cref="OnRouteRequestEvent"/></description></item>
            /// <item><description><see cref="OnToneGeneratedStartEvent"/></description></item>
            /// <item><description><see cref="OnToneGeneratedStopEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the rsi events requires having a <b>CONTACTCENTER_RSI</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddCallCenterRsiEvents(string[] ids = null);
            */

            /// <summary>
            /// Add the telephony events to this subscription.
            /// </summary>
            /// <param name="ids">The Ids to filter events on.</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following events associated to the <see cref="ITelephony"/> service are added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnTelephonyStateEvent"/></description></item>
            /// <item><description><see cref="OnCallCreatedEvent"/></description></item>
            /// <item><description><see cref="OnCallModifiedEvent"/></description></item>
            /// <item><description><see cref="OnCallRemovedEvent"/></description></item>
            /// <item><description><see cref="OnDeviceStateModifiedEvent"/></description></item>
            /// <item><description><see cref="OnDynamicStateChangedEvent"/></description></item>
            /// <item><description><see cref="OnUserStateModifiedEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the telephony events requires having a <b>TELEPHONY_ADVANCED</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddTelephonyEvents(string[] ids = null);

            /// <summary>
            /// Add the event summary events to this subscription.
            /// </summary>
            /// <param name="ids">The Ids to filter events on.</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following event associated to the <see cref="IEventSummary"/> service is added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnEventSummaryUpdatedEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the event summary events requires having a <b>TELEPHONY_ADVANCED</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddEventSummaryEvents(string[] ids = null);

            /// <summary>
            /// Add the communication log events to this subscription.
            /// </summary>
            /// <param name="ids">The Ids to filter events on.</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following events associated to the <see cref="ICommunicationLog"/> service are added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnAgentRtiChangedEvent"/></description></item>
            /// <item><description><see cref="OnComRecordModifiedEvent"/></description></item>
            /// <item><description><see cref="OnComRecordsDeletedEvent"/></description></item>
            /// <item><description><see cref="OnComRecordsAckEvent"/></description></item>
            /// <item><description><see cref="OnComRecordsUnAckEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the communication log events requires having a <b>TELEPHONY_ADVANCED</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddCommunicationLogEvents(string[] ids = null);

            /// <summary>
            /// Add the call center agent events to this subscription.
            /// </summary>
            /// <param name="ids">The Ids to filter events on.</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following events associated to the <see cref="ICallCenterAgent"/> service are added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnAgentStateChangedEvent"/></description></item>
            /// <item><description><see cref="OnAgentSkillChangedEvent"/></description></item>
            /// <item><description><see cref="OnSupervisorHelpCancelledEvent"/></description></item>
            /// <item><description><see cref="OnSupervisorHelpRequestedEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the call center agent events requires having a <b>CONTACTCENTER_AGENT</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddCallCenterAgentEvents(string[] ids = null);

            /// <summary>
            /// Add the call center pilot events to this subscription.
            /// </summary>
            /// <param name="ids">The Ids to filter events on.</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following events associated to the <see cref="ICallCenterPilot"/> service are added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnPilotCallCreatedEvent"/></description></item>
            /// <item><description><see cref="OnPilotCallQueuedEvent"/></description></item>
            /// <item><description><see cref="OnPilotCallRemovedEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the call center agent events requires having a <b>CONTACTCENTER_AGENT</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddCallCenterPilotEvents(string[] ids = null);

            /// <summary>
            /// Add the pbx management events to this subscription.
            /// </summary>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following events associated to the <see cref="IPbxManagement"/> service are added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnPbxObjectInstanceCreatedEvent"/></description></item>
            /// <item><description><see cref="OnPbxObjectInstanceDeletedEvent"/></description></item>
            /// <item><description><see cref="OnPbxObjectInstanceModifiedEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the call center agent events requires having a <b>MANAGEMENT</b> license.
            /// </para>
            /// </remarks>
            IBuilder AddPbxManagementEvents();

            /// <summary>
            /// Add the maintenance events to this subscription.
            /// </summary>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// <para>
            /// The following events associated to the <see cref="IMaintenance"/> service are added to the subscription:
            /// <list type="bullet">
            /// <item><description><see cref="OnCtiLinkDownEvent"/></description></item>
            /// <item><description><see cref="OnCtiLinkUpEvent"/></description></item>
            /// <item><description><see cref="OnPbxLoadedEvent"/></description></item>
            /// </list>
            /// </para>
            /// <para>
            /// Subscribing to the maintenance events does not require any license.
            /// </para>
            /// </remarks>
            IBuilder AddMaintenanceEvents();

            /// <summary>
            /// Set the required event version. By default, the version "1.0" is used.
            /// </summary>
            /// <param name="version">The event version</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <seealso cref="Subscription.Version"/>
            IBuilder SetVersion(string version);

            /// <summary>
            /// Set lifetime of the event channel.
            /// </summary>
            /// <param name="timeout">The lifetime of the event channel in minutes</param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <seealso cref="Subscription.Timeout"/>
            IBuilder SetTimeout(int timeout);

            /// <summary>
            /// Configure the subscription to use Webhook eventing.
            /// </summary>
            /// <param name="webHook">
            /// The <see cref="IWebHook"/> instance that provides the endpoint URL and receives
            /// the <see cref="IEventProcessor"/> once the subscription is established.
            /// </param>
            /// <returns>
            /// The <see cref="IBuilder"/> object to chain the build.
            /// </returns>
            /// <remarks>
            /// When a Webhook is configured, the O2G server POSTs events to the URL returned by
            /// <see cref="IWebHook.Url"/> instead of using chunk eventing. The SDK calls
            /// <see cref="IWebHook.ConnectProcessor"/> after the subscription is accepted, providing
            /// an <see cref="IEventProcessor"/> that the application must invoke for each incoming
            /// POST body.
            /// </remarks>
            IBuilder SetWebHook(IWebHook webHook);

            /// <summary>
            /// Build a Subscription
            /// </summary>
            /// <returns>
            /// A <see cref="Subscription"/> object that represents the subscription built by this builder.
            /// </returns>
            Subscription Build();
        }
    }
}
