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
using o2g.Events.Maintenance;
using o2g.Events.Management;
using o2g.Events.Routing;
using o2g.Internal.Types.CallCenterAgent;
using o2g.Internal.Types.Routing;
using o2g.Types.CallCenterAgentNS;
using o2g.Types.ManagementNS;
using System;
using System.Collections.Generic;

namespace o2g.Internal.Events
{
    internal class OnInternalRoutingStateChangedEvent : O2GEvent
    {
        public string LoginName { get; set; }

        public O2GRoutingState RoutingState { get; set; }
    }

    internal class OnInternalStringNodeIdEvent : O2GEvent
    {
        public string NodeId { get; set; }
    }


    internal class OnInternalPbxObjectInstanceEvent : O2GEvent
    {
        public string NodeId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectId { get; set; }
        public PbxObjectDefinition Father { get; set; }
    }

    internal class OnInternalAgentSkillChangedEvent : O2GEvent
    {
        public string LoginName { get; set; }
        public O2GAgentSkills Skills { get; init; }
    }

    internal class EventAdapters
    {
        public static O2GEvent PbxObjectInstanceCreatedAdapter(O2GEvent ev)
        {
            if (ev is OnInternalPbxObjectInstanceEvent)
            {
                OnInternalPbxObjectInstanceEvent org = (OnInternalPbxObjectInstanceEvent)ev;

                return new OnPbxObjectInstanceCreatedEvent()
                {
                    EventName = org.EventName,
                    Object = new()
                    {
                        ObjectName = org.ObjectName,
                        ObjectId = org.ObjectId
                    },
                    Father = org.Father,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception: {0}", ev.GetType().Name));
            }
        }

        public static O2GEvent PbxObjectInstanceDeletedAdapter(O2GEvent ev)
        {
            if (ev is OnInternalPbxObjectInstanceEvent)
            {
                OnInternalPbxObjectInstanceEvent org = (OnInternalPbxObjectInstanceEvent)ev;

                return new OnPbxObjectInstanceDeletedEvent()
                {
                    EventName = org.EventName,
                    Object = new()
                    {
                        ObjectName = org.ObjectName,
                        ObjectId = org.ObjectId
                    },
                    Father = org.Father,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception: {0}", ev.GetType().Name));
            }
        }
        public static O2GEvent PbxObjectInstanceModifiedAdapter(O2GEvent ev)
        {
            if (ev is OnInternalPbxObjectInstanceEvent)
            {
                OnInternalPbxObjectInstanceEvent org = (OnInternalPbxObjectInstanceEvent)ev;

                return new OnPbxObjectInstanceModifiedEvent()
                {
                    EventName = org.EventName,
                    Object = new()
                    {
                        ObjectName = org.ObjectName,
                        ObjectId = org.ObjectId
                    },
                    Father = org.Father,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception: {0}", ev.GetType().Name));
            }
        }


        public static O2GEvent RoutingStateChangedAdapter(O2GEvent ev)
        {
            if (ev is OnInternalRoutingStateChangedEvent)
            {
                OnInternalRoutingStateChangedEvent org = (OnInternalRoutingStateChangedEvent)ev;

                return new OnRoutingStateChangedEvent()
                {
                    EventName = org.EventName,
                    LoginName = org.LoginName,
                    RoutingState = org.RoutingState.ToRoutingState()
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception {0}", ev.GetType().Name));
            }
        }

        public static O2GEvent CtiLinkDownAdapter(O2GEvent ev)
        {
            if (ev is OnInternalStringNodeIdEvent)
            {
                OnInternalStringNodeIdEvent org = (OnInternalStringNodeIdEvent)ev;

                return new OnCtiLinkDownEvent()
                {
                    EventName = org.EventName,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception {0}", ev.GetType().Name));
            }
        }

        public static O2GEvent CtiLinkUpAdapter(O2GEvent ev)
        {
            if (ev is OnInternalStringNodeIdEvent)
            {
                OnInternalStringNodeIdEvent org = (OnInternalStringNodeIdEvent)ev;

                return new OnCtiLinkUpEvent()
                {
                    EventName = org.EventName,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception {0}", ev.GetType().Name));
            }
        }

        public static O2GEvent PbxLoadedAdapter(O2GEvent ev)
        {
            if (ev is OnInternalStringNodeIdEvent)
            {
                OnInternalStringNodeIdEvent org = (OnInternalStringNodeIdEvent)ev;

                return new OnPbxLoadedEvent()
                {
                    EventName = org.EventName,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception {0}", ev.GetType().Name));
            }
        }

        public static O2GEvent PbxLinkDownAdapter(O2GEvent ev)
        {
            if (ev is OnInternalStringNodeIdEvent)
            {
                OnInternalStringNodeIdEvent org = (OnInternalStringNodeIdEvent)ev;

                return new OnPbxLinkDownEvent()
                {
                    EventName = org.EventName,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception {0}", ev.GetType().Name));
            }
        }

        public static O2GEvent PbxLinkUpAdapter(O2GEvent ev)
        {
            if (ev is OnInternalStringNodeIdEvent)
            {
                OnInternalStringNodeIdEvent org = (OnInternalStringNodeIdEvent)ev;

                return new OnPbxLinkUpEvent()
                {
                    EventName = org.EventName,
                    NodeId = int.Parse(org.NodeId)
                };
            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception {0}", ev.GetType().Name));
            }
        }

        public static O2GEvent AgentSkillChangedAdapter(O2GEvent ev)
        {
            if (ev is OnInternalAgentSkillChangedEvent)
            {
                OnInternalAgentSkillChangedEvent org = (OnInternalAgentSkillChangedEvent)ev;

                // Transform the AgentSkills to a SkillSet
                Dictionary<int, AgentSkill> mapSkills = new();
                if (org.Skills.Skills != null)
                {
                    org.Skills.Skills.ForEach(s => mapSkills.Add(s.Number, s));
                }

                return new OnAgentSkillChangedEvent()
                {
                    EventName = org.EventName,
                    LoginName = org.LoginName,
                    Skills = new()
                    {
                        Map = mapSkills
                    }
                };

            }
            else
            {
                throw new O2GException(string.Format("Invalid translator exception {0}", ev.GetType().Name));
            }
        }
    }
}
