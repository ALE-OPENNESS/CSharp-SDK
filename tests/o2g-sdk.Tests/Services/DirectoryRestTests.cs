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
using o2g.Types.DirectoryNS;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class DirectoryRestTests : ServiceTestBase
    {
        private static readonly System.Uri DirectoryUri = new("https://fake-o2g/api/directory");

        private DirectoryRest Service() =>
            DependancyResolver.Resolve(new DirectoryRest(DirectoryUri));

        #region SearchAsync

        [Fact]
        public async Task SearchAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var criteria = Criteria.Create(
                AttributeFilter.LastName, OperationFilter.BeginsWith, "Doe");

            var result = await Service().SearchAsync(criteria, 10, "jdoe");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/directory/search?loginName=jdoe")
                .JsonBody(json =>
                {
                    json.AssertValue("$.filter.field", "lastName");
                    json.AssertValue("$.filter.operation", "BEGIN_WITH");
                    json.AssertValue("$.filter.operand", "Doe");
                    json.AssertValue("$.limit", 10);
                });
        }

        [Fact]
        public async Task SearchAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var criteria = Criteria.Create(
                AttributeFilter.FirstName, OperationFilter.EqualIgnoreCase, "John");

            await Service().SearchAsync(criteria, null, null);

            AssertRequest().Uri("/api/directory/search");
        }

        [Fact]
        public async Task SearchAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().SearchAsync(
                Criteria.Create(AttributeFilter.LastName, OperationFilter.BeginsWith, "Doe"),
                null, null);

            result.Should().BeFalse();
        }

        #endregion

        #region GetResultsAsync

        [Fact]
        public async Task GetResultsAsync_ReturnsSearchResult()
        {
            SetupHttpClient("""
                {
                    "resultCode": "OK",
                    "resultElements": [
                        {
                            "contacts": [
                                {
                                    "id": { "loginName": "jdoe", "phoneNumber": "1001" },
                                    "firstName": "John",
                                    "lastName": "Doe"
                                }
                            ]
                        }
                    ]
                }
                """);

            var result = await Service().GetResultsAsync(null);

            result.Should().NotBeNull();
            result.ResultCode.Should().Be(SearchResult.Code.Ok);
            result.ResultElements.Should().HaveCount(1);
            result.ResultElements[0].Contacts.Should().HaveCount(1);
            result.ResultElements[0].Contacts[0].FirstName.Should().Be("John");
            result.ResultElements[0].Contacts[0].LastName.Should().Be("Doe");
            result.ResultElements[0].Contacts[0].Id.LoginName.Should().Be("jdoe");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/directory/search");
        }

        [Fact]
        public async Task GetResultsAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{ "resultCode": "FINISH", "resultElements": [] }""");

            await Service().GetResultsAsync("jdoe");

            AssertRequest().Uri("/api/directory/search?loginName=jdoe");
        }

        [Fact]
        public async Task GetResultsAsync_FinishCode_ReturnsFinish()
        {
            SetupHttpClient("""{ "resultCode": "FINISH", "resultElements": [] }""");

            var result = await Service().GetResultsAsync(null);

            result.ResultCode.Should().Be(SearchResult.Code.Finish);
        }

        [Fact]
        public async Task GetResultsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetResultsAsync(null);

            result.Should().BeNull();
        }

        #endregion

        #region CancelAsync

        [Fact]
        public async Task CancelAsync_ReturnsTrue()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().CancelAsync(null);

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete, "/api/directory/search");
        }

        [Fact]
        public async Task CancelAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CancelAsync("jdoe");

            await AssertCalledWith(HttpMethod.Delete, "/api/directory/search?loginName=jdoe");
        }

        [Fact]
        public async Task CancelAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().CancelAsync(null);

            result.Should().BeFalse();
        }

        #endregion
    }
}
