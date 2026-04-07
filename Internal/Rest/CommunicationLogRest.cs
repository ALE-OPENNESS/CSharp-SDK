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
using o2g.Events.CommunicationLog;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types.CommunicationLogNS;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    record UpdateComRecordsRequest(List<long> RecordIds) {}

    internal class CommunicationLogRest : AbstractRESTService, ICommunicationLog
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnAgentRtiChangedEvent>> ComRecordCreated
        {
            add => _eventHandlers.ComRecordCreated += value;
            remove => _eventHandlers.ComRecordCreated -= value;
        }

        public event EventHandler<O2GEventArgs<OnComRecordModifiedEvent>> ComRecordModified
        {
            add => _eventHandlers.ComRecordModified += value;
            remove => _eventHandlers.ComRecordModified -= value;
        }

        public event EventHandler<O2GEventArgs<OnComRecordsDeletedEvent>> ComRecordsDeleted
        {
            add => _eventHandlers.ComRecordsDeleted += value;
            remove => _eventHandlers.ComRecordsDeleted -= value;
        }

        public event EventHandler<O2GEventArgs<OnComRecordsAckEvent>> ComRecordsAck
        {
            add => _eventHandlers.ComRecordsAck += value;
            remove => _eventHandlers.ComRecordsAck -= value;
        }

        public event EventHandler<O2GEventArgs<OnComRecordsUnAckEvent>> ComRecordsUnAck
        {
            add => _eventHandlers.ComRecordsUnAck += value;
            remove => _eventHandlers.ComRecordsUnAck -= value;
        }

        public CommunicationLogRest(Uri uri) : base(uri)
        {

        }

        private async Task<bool> AckOrUnAckComRecordsAsync(bool ack, List<long> recordIds, string loginName = null)
        {
            Uri uriPut = uri.AppendQuery("acknowledge", ack ? "true" : "false");
            if (loginName != null)
            {
                uriPut = uriPut.AppendQuery("loginName", loginName);
            }

            UpdateComRecordsRequest ucrm = new(recordIds);

            var json = JsonSerializer.Serialize(ucrm, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public Task<bool> AcknowledgeComRecordsAsync(List<long> recordIds, string loginName = null)
        {
            return this.AckOrUnAckComRecordsAsync(true, recordIds, loginName);
        }

        public Task<bool> UnacknowledgeComRecordsAsync(List<long> recordIds, string loginName = null)
        {
            return this.AckOrUnAckComRecordsAsync(false, recordIds, loginName);
        }

        public Task<bool> AcknowledgeComRecordAsync(long recordId, string loginName = null)
        {
            return this.AcknowledgeComRecordsAsync(new List<long> { recordId }, loginName);
        }

        public Task<bool> UnacknowledgeComRecordAsync(long recordId, string loginName = null)
        {
            return this.UnacknowledgeComRecordsAsync(new List<long> { recordId }, loginName);
        }


        public async Task<bool> DeleteComRecordAsync(long recordId, string loginName = null)
        {
            Uri uriDelete = uri.Append(recordId.ToString());
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<ComRecord> GetComRecordAsync(long recordId, string loginName = null)
        {
            Uri uriGet = uri.Append(recordId.ToString());
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<ComRecord>(response);
        }

        public async Task<QueryResult> GetComRecordsAsync(QueryFilter filter, Page page, bool optimized, string loginName)
        {
            Uri uriGet = uri;
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            if (filter != null)
            {
                if (filter.After.HasValue)
                {
                    uriGet = uriGet.AppendQuery("afterDate", filter.After.Value.ToString());
                }
                if (filter.Before.HasValue)
                {
                    uriGet = uriGet.AppendQuery("beforeDate", filter.Before.Value.ToString());
                }

                if (filter.Options.HasValue)
                {
                    if (filter.Options.Value.HasFlag(Option.Unacknowledged))
                    {
                        uriGet = uriGet.AppendQuery("unacknowledged", "true");
                    }

                    if (filter.Options.Value.HasFlag(Option.Unanswered))
                    {
                        uriGet = uriGet.AppendQuery("unanswered", "true");
                    }
                }

                if (filter.Role.HasValue)
                {
                    if (filter.Role.Value == Role.Callee)
                    {
                        uriGet = uriGet.AppendQuery("role", "CALLEE");
                    }
                    else if (filter.Role.Value == Role.Caller)
                    {
                        uriGet = uriGet.AppendQuery("role", "CALLER");
                    }
                }

                if (filter.CallRef != null)
                {
                    uriGet = uriGet.AppendQuery("comRef", filter.CallRef);
                }

                if (filter.RemotePartyId != null)
                {
                    uriGet = uriGet.AppendQuery("remotePartyId", filter.RemotePartyId);
                }
            }

            if (page != null)
            {
                if (AssertUtil.AssertPositive(page.Offset, "page.Offset") > 0)
                {
                    uriGet = uriGet.AppendQuery("offset", page.Offset.ToString());
                }

                if (AssertUtil.AssertPositive(page.Limit, "page.Limit") > 0)
                {
                    uriGet = uriGet.AppendQuery("limit", page.Limit.ToString());
                }
            }

            if (optimized)
            {
                uriGet = uriGet.AppendQuery("optimized", "true");
            }


            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<QueryResult>(response);
        }

        public async Task<bool> DeleteComRecordsAsync(QueryFilter filter, string loginName)
        {
            Uri uriDelete = uri;
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            if (filter != null)
            {
                if (filter.After.HasValue)
                {
                    uriDelete = uriDelete.AppendQuery("afterDate", filter.After.Value.ToString());
                }
                if (filter.Before.HasValue)
                {
                    uriDelete = uriDelete.AppendQuery("beforeDate", filter.Before.Value.ToString());
                }

                if (filter.Options.HasValue)
                {
                    if (filter.Options.Value.HasFlag(Option.Unacknowledged))
                    {
                        uriDelete = uriDelete.AppendQuery("unacknowledged", "true");
                    }

                    if (filter.Options.Value.HasFlag(Option.Unanswered))
                    {
                        uriDelete = uriDelete.AppendQuery("unanswered", "true");
                    }
                }

                if (filter.Role.HasValue)
                {
                    if (filter.Role.Value == Role.Callee)
                    {
                        uriDelete = uriDelete.AppendQuery("role", "CALLEE");
                    }
                    else if (filter.Role.Value == Role.Caller)
                    {
                        uriDelete = uriDelete.AppendQuery("role", "CALLER");
                    }
                }

                if (filter.CallRef != null)
                {
                    uriDelete = uriDelete.AppendQuery("comRef", filter.CallRef);
                }

                if (filter.RemotePartyId != null)
                {
                    uriDelete = uriDelete.AppendQuery("remotePartyId", filter.RemotePartyId);
                }
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteComRecordsAsync(List<long> recordIds, string loginName = null)
        {
            Uri uriDelete = uri.AppendQuery("recordIdList", string.Join(',', recordIds));
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }
    }
}
