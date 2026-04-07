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
using o2g.Events.EventSummary;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types.EventSummaryNS;
using o2g.Utility;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    internal class EventSummaryRest : AbstractRESTService, IEventSummary
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnEventSummaryUpdatedEvent>> EventSummaryUpdated
        {
            add => _eventHandlers.EventSummaryUpdated += value;
            remove => _eventHandlers.EventSummaryUpdated -= value;
        }

        public EventSummaryRest(Uri uri) : base(uri)
        {
        }

        public async Task<EventSummary> GetAsync(string loginName)
        {
            Uri uriGet = uri;
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<EventSummary>(response);
        }
    }
}
