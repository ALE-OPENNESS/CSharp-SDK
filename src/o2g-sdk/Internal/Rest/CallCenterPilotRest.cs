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
using o2g.Internal.Events;
using o2g.Internal.Utility;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    internal class CallCenterPilotRest : AbstractRESTService, ICallCenterPilot
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnPilotCallCreatedEvent>> PilotCallCreated
        {
            add => _eventHandlers.PilotCallCreated += value;
            remove => _eventHandlers.PilotCallCreated -= value;
        }

        public event EventHandler<O2GEventArgs<OnPilotCallQueuedEvent>> PilotCallQueued
        {
            add => _eventHandlers.PilotCallQueued += value;
            remove => _eventHandlers.PilotCallQueued -= value;
        }

        public event EventHandler<O2GEventArgs<OnPilotCallRemovedEvent>> PilotCallRemoved
        {
            add => _eventHandlers.PilotCallRemoved += value;
            remove => _eventHandlers.PilotCallRemoved -= value;
        }

        public CallCenterPilotRest(Uri uri) : base(uri)
        {
        }

        public async Task<bool> MonitorStartAsync(string pilotNumber)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"));

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> MonitorStopAsync(string pilotNumber)
        {
            Uri uriDelete = uri.Append(AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"));

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }
    }
}
