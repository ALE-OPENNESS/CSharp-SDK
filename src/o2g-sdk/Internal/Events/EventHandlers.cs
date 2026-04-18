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
using o2g.Events.CallCenterRealtimeNS;
using o2g.Events.Rsi;
using o2g.Events.Common;
using o2g.Events.CommunicationLog;
using o2g.Events.EventSummary;
using o2g.Events.Maintenance;
using o2g.Events.Management;
using o2g.Events.Routing;
using o2g.Events.Telephony;
using o2g.Events.Users;
using System;
using System.Reflection;

namespace o2g.Internal.Events
{
    internal class EventHandlers
    {
#pragma warning disable CS0067
        // Channel event handlers
        public event EventHandler<O2GEventArgs<OnChannelInformationEvent>> ChannelInformation;

        // Routing event handlers
        public event EventHandler<O2GEventArgs<OnRoutingStateChangedEvent>> RoutingStateChanged;

        // telephony event handlers
        public event EventHandler<O2GEventArgs<OnCallCreatedEvent>> CallCreated;
        public event EventHandler<O2GEventArgs<OnCallModifiedEvent>> CallModified;
        public event EventHandler<O2GEventArgs<OnCallRemovedEvent>> CallRemoved;
        public event EventHandler<O2GEventArgs<OnUserStateModifiedEvent>> UserStateModified;
        public event EventHandler<O2GEventArgs<OnTelephonyStateEvent>> TelephonyState;
        public event EventHandler<O2GEventArgs<OnDeviceStateModifiedEvent>> DeviceStateModified;
        public event EventHandler<O2GEventArgs<OnDynamicStateChangedEvent>> DynamicStateChanged;

        // event summary event handlers
        public event EventHandler<O2GEventArgs<OnEventSummaryUpdatedEvent>> EventSummaryUpdated;

        // users event handlers
        public event EventHandler<O2GEventArgs<OnUserInfoChangedEvent>> UserInfoChanged;
        public event EventHandler<O2GEventArgs<OnUserCreatedEvent>> UserCreated;
        public event EventHandler<O2GEventArgs<OnUserDeletedEvent>> UserDeleted;

        // Call center agent
        public event System.EventHandler<O2GEventArgs<OnAgentStateChangedEvent>> AgentStateChanged;
        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpRequestedEvent>> SupervisorHelpRequested;
        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpCancelledEvent>> SupervisorHelpCancelled;
        public event System.EventHandler<O2GEventArgs<OnAgentSkillChangedEvent>> AgentSkillChanged;

        // Call center pilot
        public event System.EventHandler<O2GEventArgs<OnPilotCallCreatedEvent>> PilotCallCreated;
        public event System.EventHandler<O2GEventArgs<OnPilotCallQueuedEvent>> PilotCallQueued;
        public event System.EventHandler<O2GEventArgs<OnPilotCallRemovedEvent>> PilotCallRemoved;

        // call center Rsi
        public event EventHandler<O2GEventArgs<OnDigitCollectedEvent>> DigitCollected;
        public event EventHandler<O2GEventArgs<OnToneGeneratedStartEvent>> ToneGeneratedStart;
        public event EventHandler<O2GEventArgs<OnToneGeneratedStopEvent>> ToneGeneratedStop;
        public event EventHandler<O2GEventArgs<OnRouteEndEvent>> RouteEnd;
        public event EventHandler<O2GEventArgs<OnRouteRequestEvent>> RouteRequest;

        // maintenance
        public event EventHandler<O2GEventArgs<OnCtiLinkDownEvent>> CtiLinkDown;
        public event EventHandler<O2GEventArgs<OnCtiLinkUpEvent>> CtiLinkUp;
        public event EventHandler<O2GEventArgs<OnPbxLoadedEvent>> PbxLoaded;
        public event EventHandler<O2GEventArgs<OnPbxLinkDownEvent>> PbxLinkDown;
        public event EventHandler<O2GEventArgs<OnPbxLinkUpEvent>> PbxLinkUp;
        public event EventHandler<O2GEventArgs<OnRemoteServerLinkDownEvent>> RemoteServerLinkDown;
        public event EventHandler<O2GEventArgs<OnRemoteServerLinkUpEvent>> RemoteServerLinkUp;
        public event EventHandler<O2GEventArgs<OnServerStartEvent>> ServerStart;
        public event EventHandler<O2GEventArgs<OnLicenseExpirationEvent>> LicenseExpiration;

        // pbx management
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceCreatedEvent>> PbxObjectInstanceCreated;
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceDeletedEvent>> PbxObjectInstanceDeleted;
        public event EventHandler<O2GEventArgs<OnPbxObjectInstanceModifiedEvent>> PbxObjectInstanceModified;

        // communication log
        public event EventHandler<O2GEventArgs<OnComRecordCreatedEvent>> ComRecordCreated;
        public event EventHandler<O2GEventArgs<OnComRecordModifiedEvent>> ComRecordModified;
        public event EventHandler<O2GEventArgs<OnComRecordsDeletedEvent>> ComRecordsDeleted;
        public event EventHandler<O2GEventArgs<OnComRecordsAckEvent>> ComRecordsAck;
        public event EventHandler<O2GEventArgs<OnComRecordsUnAckEvent>> ComRecordsUnAck;

        // Call center realtime
        public event EventHandler<O2GEventArgs<OnAgentRtiChangedEvent>> AgentRtiChanged;
        public event EventHandler<O2GEventArgs<OnPilotRtiChangedEvent>> PilotRtiChanged;
        public event EventHandler<O2GEventArgs<OnQueueRtiChangedEvent>> QueueRtiChanged;
        public event EventHandler<O2GEventArgs<OnPGAgentRtiChangedEvent>> PGAgentRtiChanged;
        public event EventHandler<O2GEventArgs<OnPGOtherRtiChangedEvent>> PGOtherRtiChanged;

        // Call center statistics
        public event EventHandler<O2GEventArgs<OnAcdStatsProgressEvent>> AcdStatsProgress;

#pragma warning restore CS0067


        private object[] CreateEventArg(O2GEvent ev, Type eventType)
        {
            Type eventArgType = typeof(O2GEventArgs<>);
            Type[] typeArgs = { eventType };

            Type generic = eventArgType.MakeGenericType(typeArgs);

            var objectEventArg = Activator.CreateInstance(generic, ev);
            return new object[] { this, objectEventArg };
        }

        internal bool Throw(O2GEventDescriptor o2gEventDescriptor)
        {
            Type eventType = o2gEventDescriptor.Event.GetType();

            string eventHandlerName = o2gEventDescriptor.Event.EventName.Substring(2); 

            FieldInfo fieldEvent = GetType().GetField(eventHandlerName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldEvent != null)
            {
                object[] raiseParam = CreateEventArg(o2gEventDescriptor.Event, eventType);

                MulticastDelegate eventDelegates = (MulticastDelegate)(fieldEvent.GetValue(this));
                if (eventDelegates != null)
                {
                    Delegate[] delegates = eventDelegates.GetInvocationList();
                    if (delegates.Length > 0)
                    {
                        foreach (Delegate delegateHandler in delegates)
                        {
                            delegateHandler.Method.Invoke(delegateHandler.Target, raiseParam);
                        }

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
