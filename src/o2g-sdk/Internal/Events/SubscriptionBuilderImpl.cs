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
using static o2g.Subscription;

namespace o2g.Internal.Events
{
    internal class SubscriptionBuilderImpl : Subscription.IBuilder
    {
        private readonly EventFilter _filter = new();
        private string _version = "1.0";
        private int _timeout = 10;

        private OnEvent defaultEventHandler;
        private IWebHook _webHook;

        public IBuilder AddCallCenterAgentEvents(string[] ids)
        {
            if (ids == null)
            {
                _filter.Add(EventPackage.Agent);
            }
            else
            {
                _filter.Add(ids, EventPackage.Agent);
            }
            return this;
        }

        IBuilder IBuilder.AddCallCenterPilotEvents(string[] ids)
        {
            if (ids == null)
            {
                _filter.Add(EventPackage.Pilot);
            }
            else
            {
                _filter.Add(ids, EventPackage.Pilot);
            }
            return this;
        }


        /*
        public IBuilder AddCallCenterRsiEvents(string[] ids)
        {
            if (ids == null)
            {
                _filter.Add(EventPackage.Rsi);
            }
            else
            {
                _filter.Add(ids, EventPackage.Rsi);
            }
            return this;
        }
        */

        public IBuilder AddCommunicationLogEvents(string[] ids)
        {
            if (ids == null)
            {
                _filter.Add(EventPackage.CommunicationLog);
            }
            else
            {
                _filter.Add(ids, EventPackage.CommunicationLog);
            }
            return this;
        }

        public IBuilder AddDefaultEventHandler(OnEvent eventHandler)
        {
            defaultEventHandler = eventHandler;
            return this;
        }

        public IBuilder AddEventSummaryEvents(string[] ids)
        {
            if (ids == null)
            {
                _filter.Add(EventPackage.EventSummary);
            }
            else
            {
                _filter.Add(ids, EventPackage.EventSummary);
            }
            return this;
        }

        public IBuilder AddMaintenanceEvents()
        {
            _filter.Add(EventPackage.System);
            return this;
        }

        public IBuilder AddPbxManagementEvents()
        {
            _filter.Add(EventPackage.PbxManagement);
            return this;
        }

        public IBuilder AddRoutingEvents(string[] ids)
        {
            if (ids == null)
            {
                _filter.Add(EventPackage.Routing);
            }
            else
            {
                _filter.Add(ids, EventPackage.Routing);
            }
            return this;
        }

        public IBuilder AddTelephonyEvents(string[] ids)
        {
            if (ids == null)
            {
                _filter.Add(EventPackage.Telephony);
            }
            else
            {
                _filter.Add(ids, EventPackage.Telephony);
            }
            return this;
        }
        public IBuilder SetWebHook(IWebHook webHook)
        {
            _webHook = webHook;
            return this;
        }

        public Subscription Build()
        {
            SubscriptionImpl subscription = new();

            subscription.SetFilter(_filter);
            subscription.SetVersion(_version);
            subscription.SetTimeout(_timeout);
            subscription.SetWebHook(_webHook);

            return subscription;
        }

        public IBuilder SetTimeout(int timeout)
        {
            this._timeout = timeout;
            return this;
        }

        public IBuilder SetVersion(string version)
        {
            this._version = version;
            return this;
        }
    }
}
