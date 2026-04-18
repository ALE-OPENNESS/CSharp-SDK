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
using o2g.Types.RoutingNS;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class RoutingRestTests : ServiceTestBase
    {
        private static readonly System.Uri RoutingUri = new("https://fake-o2g/api/routing");

        private RoutingRest Service() =>
            DependancyResolver.Resolve(new RoutingRest(RoutingUri));

        #region GetCapabilitiesAsync

        [Fact]
        public async Task GetCapabilitiesAsync_ReturnsCapabilities()
        {
            SetupHttpClient("""
                {
                    "presentationRoute": true,
                    "forwardRoute": true,
                    "overflowRoute": false,
                    "dnd": true
                }
                """);

            var caps = await Service().GetCapabilitiesAsync();

            caps.Should().NotBeNull();
            caps.CanManageRemoteExtension.Should().BeTrue();
            caps.CanManageForward.Should().BeTrue();
            caps.CanManageOverflow.Should().BeFalse();
            caps.CanManageDnd.Should().BeTrue();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/routing");
        }

        [Fact]
        public async Task GetCapabilitiesAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""
                {
                    "presentationRoute": false,
                    "forwardRoute": false,
                    "overflowRoute": false,
                    "dnd": false
                }
                """);

            await Service().GetCapabilitiesAsync("jdoe");

            AssertRequest().Uri("/api/routing?loginName=jdoe");
        }

        [Fact]
        public async Task GetCapabilitiesAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var caps = await Service().GetCapabilitiesAsync();

            caps.Should().BeNull();
        }

        #endregion

        #region GetDndStateAsync

        [Fact]
        public async Task GetDndStateAsync_ReturnsState()
        {
            SetupHttpClient("""{ "activate": true }""");

            var state = await Service().GetDndStateAsync();

            state.Should().NotBeNull();
            state.Activate.Should().BeTrue();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/routing/dnd");
        }

        [Fact]
        public async Task GetDndStateAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{ "activate": false }""");

            await Service().GetDndStateAsync("jdoe");

            AssertRequest().Uri("/api/routing/dnd?loginName=jdoe");
        }

        [Fact]
        public async Task GetDndStateAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("""{ "activate": false }""");

            await Service().GetDndStateAsync();

            AssertRequest().Uri("/api/routing/dnd");
        }

        #endregion

        #region ActivateDndAsync / CancelDndAsync

        [Fact]
        public async Task ActivateDndAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().ActivateDndAsync("jdoe");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Post, "/api/routing/dnd?loginName=jdoe");
        }

        [Fact]
        public async Task ActivateDndAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ActivateDndAsync();

            await AssertCalledWith(HttpMethod.Post, "/api/routing/dnd");
        }

        [Fact]
        public async Task CancelDndAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().CancelDndAsync("jdoe");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete, "/api/routing/dnd?loginName=jdoe");
        }

        [Fact]
        public async Task CancelDndAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CancelDndAsync();

            await AssertCalledWith(HttpMethod.Delete, "/api/routing/dnd");
        }

        #endregion

        #region GetForwardAsync

        [Fact]
        public async Task GetForwardAsync_WithForwardOnNumber_ReturnsForward()
        {
            SetupHttpClient("""
                {
                    "forwardType": "BUSY",
                    "destinations": [ { "type": "NUMBER", "number": "1234" } ]
                }
                """);

            var forward = await Service().GetForwardAsync();

            forward.Should().NotBeNull();
            forward.Destination.Should().Be(Destination.Number);
            forward.Number.Should().Be("1234");
            forward.Condition.Should().Be(Forward.ForwardCondition.Busy);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/routing/forwardroute");
        }

        [Fact]
        public async Task GetForwardAsync_WithForwardOnVoiceMail_ReturnsForward()
        {
            SetupHttpClient("""
                {
                    "forwardType": "NO_ANSWER",
                    "destinations": [ { "type": "VOICEMAIL" } ]
                }
                """);

            var forward = await Service().GetForwardAsync();

            forward.Destination.Should().Be(Destination.VoiceMail);
            forward.Number.Should().BeNull();
            forward.Condition.Should().Be(Forward.ForwardCondition.NoAnswer);
        }

        [Fact]
        public async Task GetForwardAsync_EmptyBody_ReturnsNoneDestination()
        {
            SetupHttpClient("");

            var forward = await Service().GetForwardAsync();

            forward.Should().NotBeNull();
            forward.Destination.Should().Be(Destination.None);
            forward.Condition.Should().BeNull();
            forward.Number.Should().BeNull();
        }

        [Fact]
        public async Task GetForwardAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var forward = await Service().GetForwardAsync();

            forward.Should().BeNull();
        }

        [Fact]
        public async Task GetForwardAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""
                {
                    "forwardType": "BUSY",
                    "destinations": [ { "type": "VOICEMAIL" } ]
                }
                """);

            await Service().GetForwardAsync("jdoe");

            AssertRequest().Uri("/api/routing/forwardroute?loginName=jdoe");
        }

        #endregion

        #region ForwardOnNumberAsync

        [Fact]
        public async Task ForwardOnNumberAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().ForwardOnNumberAsync(
                "1234", Forward.ForwardCondition.Busy, "jdoe");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/routing/forwardroute?loginName=jdoe")
                .JsonBody(json =>
                {
                    json.AssertValue("$.forwardRoute.forwardType", "BUSY");
                    json.AssertValue("$.forwardRoute.destinations[0].type", "NUMBER");
                    json.AssertValue("$.forwardRoute.destinations[0].number", "1234");
                });
        }

        [Fact]
        public async Task ForwardOnNumberAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ForwardOnNumberAsync(
                "5678", Forward.ForwardCondition.NoAnswer, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/routing/forwardroute")
                .JsonBody(json =>
                {
                    json.AssertValue("$.forwardRoute.forwardType", "NO_ANSWER");
                    json.AssertValue("$.forwardRoute.destinations[0].number", "5678");
                });
        }

        [Fact]
        public async Task ForwardOnNumberAsync_BusyOrNoAnswer_SendsCorrectForwardType()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ForwardOnNumberAsync(
                "1234", Forward.ForwardCondition.BusyOrNoAnswer, null);

            await AssertRequest()
                .JsonBody(json =>
                    json.AssertValue("$.forwardRoute.forwardType", "BUSY_NO_ANSWER"));
        }

        #endregion

        #region ForwardOnVoiceMailAsync

        [Fact]
        public async Task ForwardOnVoiceMailAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().ForwardOnVoiceMailAsync(
                Forward.ForwardCondition.BusyOrNoAnswer, "jdoe");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/routing/forwardroute?loginName=jdoe")
                .JsonBody(json =>
                {
                    json.AssertValue("$.forwardRoute.forwardType", "BUSY_NO_ANSWER");
                    json.AssertValue("$.forwardRoute.destinations[0].type", "VOICEMAIL");
                });
        }

        [Fact]
        public async Task ForwardOnVoiceMailAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ForwardOnVoiceMailAsync(Forward.ForwardCondition.Busy);

            AssertRequest().Uri("/api/routing/forwardroute");
        }

        #endregion

        #region CancelForwardAsync

        [Fact]
        public async Task CancelForwardAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().CancelForwardAsync("jdoe");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete,
                "/api/routing/forwardroute?loginName=jdoe");
        }

        [Fact]
        public async Task CancelForwardAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CancelForwardAsync();

            await AssertCalledWith(HttpMethod.Delete, "/api/routing/forwardroute");
        }

        #endregion

        #region GetOverflowAsync

        [Fact]
        public async Task GetOverflowAsync_WithOverflow_ReturnsOverflow()
        {
            SetupHttpClient("""
                {
                    "overflowType": "NO_ANSWER",
                    "destinations": [ { "type": "VOICEMAIL" } ]
                }
                """);

            var overflow = await Service().GetOverflowAsync();

            overflow.Should().NotBeNull();
            overflow.Destination.Should().Be(Destination.VoiceMail);
            overflow.Condition.Should().Be(Overflow.OverflowCondition.NoAnswer);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/routing/overflowroute");
        }

        [Fact]
        public async Task GetOverflowAsync_EmptyBody_ReturnsNoneDestination()
        {
            SetupHttpClient("");

            var overflow = await Service().GetOverflowAsync();

            overflow.Should().NotBeNull();
            overflow.Destination.Should().Be(Destination.None);
            overflow.Condition.Should().BeNull();
        }

        [Fact]
        public async Task GetOverflowAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var overflow = await Service().GetOverflowAsync();

            overflow.Should().BeNull();
        }

        [Fact]
        public async Task GetOverflowAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""
                {
                    "overflowType": "BUSY",
                    "destinations": [ { "type": "VOICEMAIL" } ]
                }
                """);

            await Service().GetOverflowAsync("jdoe");

            AssertRequest().Uri("/api/routing/overflowroute?loginName=jdoe");
        }

        #endregion

        #region OverflowOnVoiceMailAsync

        [Fact]
        public async Task OverflowOnVoiceMailAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().OverflowOnVoiceMailAsync(
                Overflow.OverflowCondition.Busy, "jdoe");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/routing/overflowroute?loginName=jdoe")
                .JsonBody(json =>
                {
                    json.AssertValue("$.overflowRoutes[0].overflowType", "BUSY");
                    json.AssertValue("$.overflowRoutes[0].destinations[0].type", "VOICEMAIL");
                });
        }

        [Fact]
        public async Task OverflowOnVoiceMailAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().OverflowOnVoiceMailAsync(Overflow.OverflowCondition.NoAnswer);

            AssertRequest().Uri("/api/routing/overflowroute");
        }

        #endregion

        #region CancelOverflowAsync

        [Fact]
        public async Task CancelOverflowAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().CancelOverflowAsync("jdoe");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete,
                "/api/routing/overflowroute?loginName=jdoe");
        }

        [Fact]
        public async Task CancelOverflowAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CancelOverflowAsync();

            await AssertCalledWith(HttpMethod.Delete, "/api/routing/overflowroute");
        }

        #endregion

        #region Remote extension

        [Fact]
        public async Task ActivateRemoteExtensionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().ActivateRemoteExtensionAsync("jdoe");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/routing?loginName=jdoe")
                .JsonBody(json =>
                {
                    json.AssertValue(
                        "$.presentationRoutes[0].destinations[0].type", "MOBILE");
                    json.AssertValue(
                        "$.presentationRoutes[0].destinations[0].selected", true);
                });
        }

        [Fact]
        public async Task ActivateRemoteExtensionAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ActivateRemoteExtensionAsync();

            AssertRequest().Uri("/api/routing");
        }

        [Fact]
        public async Task DeactivateRemoteExtensionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().DeactivateRemoteExtensionAsync("jdoe");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/routing?loginName=jdoe")
                .JsonBody(json =>
                {
                    json.AssertValue(
                        "$.presentationRoutes[0].destinations[0].type", "MOBILE");
                    json.AssertValue(
                        "$.presentationRoutes[0].destinations[0].selected", false);
                });
        }

        #endregion

        #region GetRoutingStateAsync

        [Fact]
        public async Task GetRoutingStateAsync_ReturnsFullState()
        {
            SetupHttpClient("""
                {
                    "presentationRoutes": [
                        { "destinations": [ { "type": "MOBILE", "selected": true } ] }
                    ],
                    "forwardRoutes": [
                        {
                            "forwardType": "BUSY",
                            "destinations": [ { "type": "NUMBER", "number": "1234" } ]
                        }
                    ],
                    "overflowRoutes": [
                        {
                            "overflowType": "NO_ANSWER",
                            "destinations": [ { "type": "VOICEMAIL" } ]
                        }
                    ],
                    "dndState": { "activate": false }
                }
                """);

            var state = await Service().GetRoutingStateAsync();

            state.Should().NotBeNull();
            state.RemoteExtensionActivated.Should().BeTrue();
            state.Forward.Destination.Should().Be(Destination.Number);
            state.Forward.Number.Should().Be("1234");
            state.Overflow.Destination.Should().Be(Destination.VoiceMail);
            state.DndState.Activate.Should().BeFalse();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/routing/state");
        }

        [Fact]
        public async Task GetRoutingStateAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var state = await Service().GetRoutingStateAsync();

            state.Should().BeNull();
        }

        [Fact]
        public async Task GetRoutingStateAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{ "dndState": { "activate": false } }""");

            await Service().GetRoutingStateAsync("jdoe");

            AssertRequest().Uri("/api/routing/state?loginName=jdoe");
        }

        #endregion

        #region RequestSnapshotAsync

        [Fact]
        public async Task RequestSnapshotAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().RequestSnapshotAsync("jdoe");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Post,
                "/api/routing/state/snapshot?loginName=jdoe");
        }

        [Fact]
        public async Task RequestSnapshotAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestSnapshotAsync();

            await AssertCalledWith(HttpMethod.Post, "/api/routing/state/snapshot");
        }

        #endregion
    }
}
