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
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace o2g.Internal.Events
{
    delegate O2GEvent EventAdapter(O2GEvent o2gEvent);
    
    class EventTranslator
    {
        public Type DeserializableType { get; set; }
        public EventAdapter Adapter { get; set; }
    }

    internal class EventRegistrar
    {
        private readonly Dictionary<string, Type> eventInterfaces = new();
        private readonly Dictionary<string, String> eventClasses = new();
        private readonly Dictionary<string, String> eventMethods = new();
        private readonly Dictionary<string, EventTranslator> Adapters = new();


        public void RegisterEvent(Type eventType)
        {
            eventClasses.Add(eventType.Name, eventType.FullName);
        }

        public void RegisterAdapter(Type eventType, EventAdapter adapter, Type deserializeType)
        {
            Adapters.Add(eventType.FullName, new()
            {
                DeserializableType = deserializeType,
                Adapter = adapter
            });
        }


        public Type GetInterface(string eventName)
        {
            if (eventInterfaces.ContainsKey(eventName))
            {
                return eventInterfaces[eventName];
            }
            else
            {
                return null;
            }
        }

        internal EventTranslator GetAdaptor(string eventName)
        {
            if (Adapters.ContainsKey(eventName))
            {
                return Adapters[eventName];
            }

            return null;
        }

        internal string GetQualifiedName(string eventName)
        {
            if (eventClasses.ContainsKey(eventName))
            {
                return eventClasses[eventName];
            }

            return null;
        }

        public String GetMethodName(string eventName)
        {
            if (eventMethods.ContainsKey(eventName))
            {
                return eventMethods[eventName];
            }

            return null;
        }
    }

    internal class EventBuilder
    {
        static readonly EventRegistrar EventRegistrar;

        static EventBuilder()
        {
            EventRegistrar = new();

            EventRegistrar.RegisterEvent(typeof(OnChannelInformationEvent));

            EventRegistrar.RegisterEvent(typeof(OnRoutingStateChangedEvent));
            EventRegistrar.RegisterAdapter(typeof(OnRoutingStateChangedEvent), EventAdapters.RoutingStateChangedAdapter, typeof(OnInternalRoutingStateChangedEvent));

            EventRegistrar.RegisterEvent(typeof(OnPbxObjectInstanceCreatedEvent));
            EventRegistrar.RegisterAdapter(typeof(OnPbxObjectInstanceCreatedEvent), EventAdapters.PbxObjectInstanceCreatedAdapter, typeof(OnInternalPbxObjectInstanceEvent));
            EventRegistrar.RegisterEvent(typeof(OnPbxObjectInstanceDeletedEvent));
            EventRegistrar.RegisterAdapter(typeof(OnPbxObjectInstanceDeletedEvent), EventAdapters.PbxObjectInstanceDeletedAdapter, typeof(OnInternalPbxObjectInstanceEvent));
            EventRegistrar.RegisterEvent(typeof(OnPbxObjectInstanceModifiedEvent));
            EventRegistrar.RegisterAdapter(typeof(OnPbxObjectInstanceModifiedEvent), EventAdapters.PbxObjectInstanceModifiedAdapter, typeof(OnInternalPbxObjectInstanceEvent));

            EventRegistrar.RegisterEvent(typeof(OnTelephonyStateEvent));
            EventRegistrar.RegisterEvent(typeof(OnCallCreatedEvent));
            EventRegistrar.RegisterEvent(typeof(OnCallModifiedEvent));
            EventRegistrar.RegisterEvent(typeof(OnCallRemovedEvent));
            EventRegistrar.RegisterEvent(typeof(OnDeviceStateModifiedEvent));
            EventRegistrar.RegisterEvent(typeof(OnDynamicStateChangedEvent));
            EventRegistrar.RegisterEvent(typeof(OnUserStateModifiedEvent));

            EventRegistrar.RegisterEvent(typeof(OnEventSummaryUpdatedEvent));

            EventRegistrar.RegisterEvent(typeof(OnAgentStateChangedEvent));
            EventRegistrar.RegisterEvent(typeof(OnAgentSkillChangedEvent));
            EventRegistrar.RegisterAdapter(typeof(OnAgentSkillChangedEvent), EventAdapters.AgentSkillChangedAdapter, typeof(OnInternalAgentSkillChangedEvent));
            EventRegistrar.RegisterEvent(typeof(OnSupervisorHelpRequestedEvent));
            EventRegistrar.RegisterEvent(typeof(OnSupervisorHelpCancelledEvent));

            EventRegistrar.RegisterEvent(typeof(OnPilotCallCreatedEvent));
            EventRegistrar.RegisterEvent(typeof(OnPilotCallQueuedEvent));
            EventRegistrar.RegisterEvent(typeof(OnPilotCallRemovedEvent));

            EventRegistrar.RegisterEvent(typeof(OnDigitCollectedEvent));
            EventRegistrar.RegisterEvent(typeof(OnRouteEndEvent));
            EventRegistrar.RegisterEvent(typeof(OnRouteRequestEvent));
            EventRegistrar.RegisterEvent(typeof(OnToneGeneratedStartEvent));
            EventRegistrar.RegisterEvent(typeof(OnToneGeneratedStopEvent));

            EventRegistrar.RegisterEvent(typeof(OnComRecordCreatedEvent));
            EventRegistrar.RegisterEvent(typeof(OnComRecordModifiedEvent));
            EventRegistrar.RegisterEvent(typeof(OnComRecordsDeletedEvent));
            EventRegistrar.RegisterEvent(typeof(OnComRecordsAckEvent));
            EventRegistrar.RegisterEvent(typeof(OnComRecordsUnAckEvent));

            EventRegistrar.RegisterEvent(typeof(OnUserCreatedEvent));
            EventRegistrar.RegisterEvent(typeof(OnUserDeletedEvent));
            EventRegistrar.RegisterEvent(typeof(OnUserInfoChangedEvent));

            EventRegistrar.RegisterEvent(typeof(OnCtiLinkUpEvent));
            EventRegistrar.RegisterAdapter(typeof(OnCtiLinkUpEvent), EventAdapters.CtiLinkUpAdapter, typeof(OnInternalStringNodeIdEvent));
            EventRegistrar.RegisterEvent(typeof(OnCtiLinkDownEvent));
            EventRegistrar.RegisterAdapter(typeof(OnCtiLinkDownEvent), EventAdapters.CtiLinkDownAdapter, typeof(OnInternalStringNodeIdEvent));
            EventRegistrar.RegisterEvent(typeof(OnPbxLoadedEvent));
            EventRegistrar.RegisterAdapter(typeof(OnPbxLoadedEvent), EventAdapters.PbxLoadedAdapter, typeof(OnInternalStringNodeIdEvent));
            EventRegistrar.RegisterEvent(typeof(OnPbxLinkDownEvent));
            EventRegistrar.RegisterAdapter(typeof(OnPbxLinkDownEvent), EventAdapters.PbxLinkDownAdapter, typeof(OnInternalStringNodeIdEvent));
            EventRegistrar.RegisterEvent(typeof(OnPbxLinkUpEvent));
            EventRegistrar.RegisterAdapter(typeof(OnPbxLinkUpEvent), EventAdapters.PbxLinkUpAdapter, typeof(OnInternalStringNodeIdEvent));
            EventRegistrar.RegisterEvent(typeof(OnRemoteServerLinkDownEvent));
            EventRegistrar.RegisterEvent(typeof(OnRemoteServerLinkUpEvent));
            EventRegistrar.RegisterEvent(typeof(OnServerStartEvent));
            EventRegistrar.RegisterEvent(typeof(OnLicenseExpirationEvent));

            EventRegistrar.RegisterEvent(typeof(OnAcdStatsProgressEvent));
        }

        protected static JsonSerializerOptions serializeOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {
                  new JsonStringEnumMemberConverter()
            }
        };

        public static O2GEventDescriptor Get (string evJson)
        {
            string eventName = string.Format("{0}Event", JsonSerializer.Deserialize<O2GEvent>(evJson, serializeOptions).EventName);
            string qualifiedEventName = EventRegistrar.GetQualifiedName(eventName);


            // We have the name of the event, check if there is a translator for this event
            EventTranslator translator = EventRegistrar.GetAdaptor(qualifiedEventName);

            Type typeEvent;
            if (translator != null)
            {
                typeEvent = translator.DeserializableType;
            }
            else
            {
                // No translation, 
                typeEvent = Type.GetType(qualifiedEventName);
            }

            if (typeEvent == null)
            {
                return null;
            }

            // Now we can deserialize the object
            O2GEvent ev = (O2GEvent)JsonSerializer.Deserialize(evJson, typeEvent, serializeOptions);
            if (ev == null)
            {
                return null;
            }

            // We have an object try to adapt it if necessary
            if (translator != null)
            {
                ev = translator.Adapter(ev);
            }

            // Now search the object interface
            return new O2GEventDescriptor(ev, EventRegistrar.GetInterface(qualifiedEventName), EventRegistrar.GetMethodName(qualifiedEventName));
        }
    }
}
