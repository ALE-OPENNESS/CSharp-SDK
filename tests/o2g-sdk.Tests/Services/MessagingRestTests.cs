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
using System.Net.Http.Headers;

namespace o2g.Tests.Services
{
    public class MessagingRestTests : ServiceTestBase
    {
        private static readonly Uri MessagingUri = new("https://fake-o2g/api/messaging");

        private MessagingRest Service() =>
            DependancyResolver.Resolve(new MessagingRest(MessagingUri));

        #region AcknowledgeVoiceMessageAsync

        [Fact]
        public async Task AcknowledgeVoiceMessageAsync_SendsGetToVoicemailUri()
        {
            SetupHttpClient("", HttpStatusCode.PartialContent);

            await Service().AcknowledgeVoiceMessageAsync("mbx1", "vm42");

            AssertRequest().Method(HttpMethod.Get).Uri("/api/messaging/mbx1/voicemails/vm42");
        }

        [Fact]
        public async Task AcknowledgeVoiceMessageAsync_WithLoginName_AppendsLoginNameQuery()
        {
            SetupHttpClient("", HttpStatusCode.PartialContent);

            await Service().AcknowledgeVoiceMessageAsync("mbx1", "vm42", "jdoe");

            AssertRequest().Method(HttpMethod.Get).Uri("/api/messaging/mbx1/voicemails/vm42?loginName=jdoe");
        }

        [Fact]
        public async Task AcknowledgeVoiceMessageAsync_SetsRangeBytesZeroToOne()
        {
            var handler = SetupHttpClient("", HttpStatusCode.PartialContent);

            await Service().AcknowledgeVoiceMessageAsync("mbx1", "vm42");

            handler.LastRequest.Headers.Range.Should().Be(new RangeHeaderValue(0, 1));
        }

        [Fact]
        public async Task AcknowledgeVoiceMessageAsync_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("", HttpStatusCode.PartialContent);

            var result = await Service().AcknowledgeVoiceMessageAsync("mbx1", "vm42");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task AcknowledgeVoiceMessageAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var result = await Service().AcknowledgeVoiceMessageAsync("mbx1", "vm42");

            result.Should().BeFalse();
        }

        #endregion
    }
}
