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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class EventSummaryRestTests : ServiceTestBase
    {
        private static readonly System.Uri EventSummaryUri = new("https://fake-o2g/api/eventsummary");

        private EventSummaryRest Service() =>
            DependancyResolver.Resolve(new EventSummaryRest(EventSummaryUri));

        #region GetAsync

        [Fact]
        public async Task GetAsync_ReturnsEventSummary()
        {
            SetupHttpClient("""
                {
                    "missedCallsNb": 3,
                    "voiceMessagesNb": 1,
                    "callBackRequestsNb": 0,
                    "faxNb": 2,
                    "newTextNb": 5,
                    "oldTextNb": 10,
                    "eventWaiting": true
                }
                """);

            var summary = await Service().GetAsync();

            summary.Should().NotBeNull();
            summary.MissedCallsNb.Should().Be(3);
            summary.VoiceMessagesNb.Should().Be(1);
            summary.CallBackRequestsNb.Should().Be(0);
            summary.FaxNb.Should().Be(2);
            summary.NewTextNb.Should().Be(5);
            summary.OldTextNb.Should().Be(10);
            summary.EventWaiting.Should().BeTrue();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/eventsummary");
        }

        [Fact]
        public async Task GetAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""
                {
                    "missedCallsNb": 1,
                    "eventWaiting": false
                }
                """);

            await Service().GetAsync("jdoe");

            AssertRequest().Uri("/api/eventsummary?loginName=jdoe");
        }

        [Fact]
        public async Task GetAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("""
                {
                    "eventWaiting": false
                }
                """);

            await Service().GetAsync();

            AssertRequest().Uri("/api/eventsummary");
        }

        [Fact]
        public async Task GetAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var summary = await Service().GetAsync();

            summary.Should().BeNull();
        }

        #endregion
    }
}
