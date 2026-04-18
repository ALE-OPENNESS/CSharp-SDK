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
using o2g.Events.CallCenterRealtimeNS;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types.CallCenterRealtimeNS;
using o2g.Types.EventSummaryNS;
using o2g.Types.ManagementNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    internal class CallCenterRealtimeRest : AbstractRESTService, ICallCenterRealtime
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnAgentRtiChangedEvent>> AgentRtiChanged
        {
            add => _eventHandlers.AgentRtiChanged += value;
            remove => _eventHandlers.AgentRtiChanged -= value;
        }

        public event EventHandler<O2GEventArgs<OnPilotRtiChangedEvent>> PilotRtiChanged
        {
            add => _eventHandlers.PilotRtiChanged += value;
            remove => _eventHandlers.PilotRtiChanged -= value;
        }

        public event EventHandler<O2GEventArgs<OnQueueRtiChangedEvent>> QueueRtiChanged
        {
            add => _eventHandlers.QueueRtiChanged += value;
            remove => _eventHandlers.QueueRtiChanged -= value;
        }

        public event EventHandler<O2GEventArgs<OnPGAgentRtiChangedEvent>> PGAgentRtiChanged
        {
            add => _eventHandlers.PGAgentRtiChanged += value;
            remove => _eventHandlers.PGAgentRtiChanged -= value;
        }

        public event EventHandler<O2GEventArgs<OnPGOtherRtiChangedEvent>> PGOtherRtiChanged
        {
            add => _eventHandlers.PGOtherRtiChanged += value;
            remove => _eventHandlers.PGOtherRtiChanged -= value;
        }


        public CallCenterRealtimeRest(Uri uri) : base(uri)
        {
        }

        public async Task<RtiObjects> GetRtiObjectsAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return await GetResult<RtiObjects>(response);
        }

        private async Task<IReadOnlyList<T>> GetRtiListAsync<T>(
            string path,
            Func<RtiObjects, List<T>> selector)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri.Append(path));

            RtiObjects objects = await GetResult<RtiObjects>(response);

            if (objects == null)
                return Array.Empty<T>();

            var list = selector(objects);
            if (list == null)
                return Array.Empty<T>();

            return list;
        }

        public Task<IReadOnlyList<RtiObjectIdentifier>> GetAgentsAsync()
        {
            return GetRtiListAsync("agents", o => o.Agents);
        }

        public Task<IReadOnlyList<RtiObjectIdentifier>> getPilotsAsync()
        {
            return GetRtiListAsync("pilots", o => o.Pilots);
        }

        public Task<IReadOnlyList<RtiObjectIdentifier>> getQueuesAsync()
        {
            return GetRtiListAsync("queues", o => o.Queues);
        }

        public Task<IReadOnlyList<RtiObjectIdentifier>> getAgentProcessingGroupsAsync()
        {
            return GetRtiListAsync("pgAgents", o => o.AgentProcessingGroups);
        }

        public Task<IReadOnlyList<RtiObjectIdentifier>> getOtherProcessingGroupsAsync()
        {
            return GetRtiListAsync("pgOthers", o => o.OtherProcessingGroups);
        }


        public async Task<RtiContext> GetContextAsync()
        {
            Uri uriGet = uri.Append("ctx");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<RtiContext>(response);
        }

        public async Task<bool> DeleteContextAsync()
        {
            Uri uriDelete = uri.Append("ctx");

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> SetContextAsync(RtiContext context)
        {
            Uri uriPost = uri.Append("ctx");

            var json = JsonSerializer.Serialize(AssertUtil.NotNull(context, "context"), serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> StartAsync()
        {
            Uri uriPost = uri.Append("snapshot");
            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }
    }
}
