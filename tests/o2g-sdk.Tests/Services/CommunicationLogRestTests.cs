/*
* Copyright 2026 ALE International
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

using FluentAssertions;
using o2g.Internal.Rest;
using o2g.Internal.Utility;
using o2g.Tests.Helpers;
using o2g.Types.CommunicationLogNS;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class CommunicationLogRestTests : ServiceTestBase
    {
        private static readonly System.Uri ComLogUri = new("https://fake-o2g/api/comlog");

        private CommunicationLogRest Service() =>
            DependancyResolver.Resolve(new CommunicationLogRest(ComLogUri));

        #region GetComRecordsAsync

        [Fact]
        public async Task GetComRecordsAsync_ReturnsQueryResult()
        {
            SetupHttpClient("""
                {
                    "comHistoryRecords": [
                        {
                            "recordId": 1,
                            "comRef": "ref-001",
                            "acknowledged": true,
                            "participants": [],
                            "beginDate": "2026-01-15T10:00:00Z",
                            "endDate": "2026-01-15T10:05:00Z"
                        }
                    ],
                    "offset": 0,
                    "limit": 10,
                    "totalCount": 1
                }
                """);

            var result = await Service().GetComRecordsAsync();

            result.Should().NotBeNull();
            result.Records.Should().HaveCount(1);
            result.Records[0].Id.Should().Be(1);
            result.Records[0].CallRef.Should().Be("ref-001");
            result.Records[0].Acknowledged.Should().BeTrue();
            result.Offset.Should().Be(0);
            result.Limit.Should().Be(10);
            result.Count.Should().Be(1);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/comlog");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(loginName: "jdoe");

            AssertRequest().Uri("/api/comlog?loginName=jdoe");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithUnacknowledgedFilter_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(filter: new QueryFilter { Options = Option.Unacknowledged });

            AssertRequest().Uri("/api/comlog?unacknowledged=true");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithUnansweredFilter_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(filter: new QueryFilter { Options = Option.Unanswered });

            AssertRequest().Uri("/api/comlog?unanswered=true");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithRoleCalleeFilter_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(filter: new QueryFilter { Role = Role.Callee });

            AssertRequest().Uri("/api/comlog?role=CALLEE");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithRoleCallerFilter_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(filter: new QueryFilter { Role = Role.Caller });

            AssertRequest().Uri("/api/comlog?role=CALLER");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithCallRefFilter_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(filter: new QueryFilter { CallRef = "call-ref-001" });

            AssertRequest().Uri("/api/comlog?comRef=call-ref-001");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithRemotePartyId_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(filter: new QueryFilter { RemotePartyId = "1001" });

            AssertRequest().Uri("/api/comlog?remotePartyId=1001");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithPage_AppendsOffsetAndLimit()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 10, "limit": 20, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(page: new Page(10, 20));

            AssertRequest().Uri("/api/comlog?offset=10&limit=20");
        }

        [Fact]
        public async Task GetComRecordsAsync_WithOptimized_AppendsQueryParam()
        {
            SetupHttpClient("""{ "comHistoryRecords": [], "offset": 0, "limit": 10, "totalCount": 0 }""");

            await Service().GetComRecordsAsync(optimized: true);

            AssertRequest().Uri("/api/comlog?optimized=true");
        }

        [Fact]
        public async Task GetComRecordsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetComRecordsAsync();

            result.Should().BeNull();
        }

        #endregion

        #region GetComRecordAsync

        [Fact]
        public async Task GetComRecordAsync_ReturnsComRecord()
        {
            SetupHttpClient("""
                {
                    "recordId": 42,
                    "comRef": "ref-042",
                    "acknowledged": false,
                    "participants": [],
                    "beginDate": "2026-01-15T10:00:00Z",
                    "endDate": "2026-01-15T10:05:00Z"
                }
                """);

            var record = await Service().GetComRecordAsync(42);

            record.Should().NotBeNull();
            record.Id.Should().Be(42);
            record.CallRef.Should().Be("ref-042");
            record.Acknowledged.Should().BeFalse();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/comlog/42");
        }

        [Fact]
        public async Task GetComRecordAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{ "recordId": 42, "comRef": "ref-042", "acknowledged": false, "participants": [], "beginDate": "2026-01-15T10:00:00Z", "endDate": "2026-01-15T10:05:00Z" }""");

            await Service().GetComRecordAsync(42, "jdoe");

            AssertRequest().Uri("/api/comlog/42?loginName=jdoe");
        }

        [Fact]
        public async Task GetComRecordAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var record = await Service().GetComRecordAsync(42);

            record.Should().BeNull();
        }

        #endregion

        #region DeleteComRecordAsync

        [Fact]
        public async Task DeleteComRecordAsync_ReturnsTrue()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().DeleteComRecordAsync(42);

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete, "/api/comlog/42");
        }

        [Fact]
        public async Task DeleteComRecordAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeleteComRecordAsync(42, "jdoe");

            await AssertCalledWith(HttpMethod.Delete, "/api/comlog/42?loginName=jdoe");
        }

        [Fact]
        public async Task DeleteComRecordAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().DeleteComRecordAsync(42);

            result.Should().BeFalse();
        }

        #endregion

        #region DeleteComRecordsAsync (QueryFilter)

        [Fact]
        public async Task DeleteComRecordsAsync_WithFilter_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().DeleteComRecordsAsync(
                new QueryFilter { CallRef = "ref-001" }, "jdoe");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete, "/api/comlog?loginName=jdoe&comRef=ref-001");
        }

        [Fact]
        public async Task DeleteComRecordsAsync_NoFilter_SendsDeleteWithNoBody()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeleteComRecordsAsync((QueryFilter?)null, null);

            await AssertCalledWith(HttpMethod.Delete, "/api/comlog");
        }

        #endregion

        #region DeleteComRecordsAsync (List<long>)

        [Fact]
        public async Task DeleteComRecordsAsync_WithRecordIds_SendsIdListQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().DeleteComRecordsAsync(new List<long> { 1, 2, 3 });

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete, "/api/comlog?recordIdList=1%2C2%2C3");
        }

        [Fact]
        public async Task DeleteComRecordsAsync_WithRecordIdsAndLoginName_AppendsQueryParams()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeleteComRecordsAsync(new List<long> { 1, 2 }, "jdoe");

            await AssertCalledWith(HttpMethod.Delete, "/api/comlog?recordIdList=1%2C2&loginName=jdoe");
        }

        #endregion

        #region AcknowledgeComRecordsAsync

        [Fact]
        public async Task AcknowledgeComRecordsAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().AcknowledgeComRecordsAsync(new List<long> { 1, 2 });

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/comlog?acknowledge=true")
                .JsonBody(json =>
                {
                    json.AssertValue("$.recordIds[0]", 1);
                    json.AssertValue("$.recordIds[1]", 2);
                });
        }

        [Fact]
        public async Task AcknowledgeComRecordsAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().AcknowledgeComRecordsAsync(new List<long> { 42 }, "jdoe");

            AssertRequest().Uri("/api/comlog?acknowledge=true&loginName=jdoe");
        }

        [Fact]
        public async Task AcknowledgeComRecordAsync_SendsSingleIdList()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().AcknowledgeComRecordAsync(42);

            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/comlog?acknowledge=true")
                .JsonBody(json => json.AssertValue("$.recordIds[0]", 42));
        }

        #endregion

        #region UnacknowledgeComRecordsAsync

        [Fact]
        public async Task UnacknowledgeComRecordsAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().UnacknowledgeComRecordsAsync(new List<long> { 1, 2 });

            result.Should().BeTrue();
            AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/comlog?acknowledge=false");
        }

        [Fact]
        public async Task UnacknowledgeComRecordAsync_SendsSingleIdList()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().UnacknowledgeComRecordAsync(42);

            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/comlog?acknowledge=false")
                .JsonBody(json => json.AssertValue("$.recordIds[0]", 42));
        }

        #endregion
    }
}
