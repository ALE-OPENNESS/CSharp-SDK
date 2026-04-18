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
using o2g.Types.CallCenterAgentNS;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class CallCenterAgentRestTests : ServiceTestBase
    {
        private static readonly System.Uri AgentUri = new("https://fake-o2g/api/cca");

        private CallCenterAgentRest Service() =>
            DependancyResolver.Resolve(new CallCenterAgentRest(AgentUri));

        #region GetStateAsync

        [Fact]
        public async Task GetStateAsync_ReturnsState()
        {
            SetupHttpClient("""
                {
                    "mainState": "LOG_ON",
                    "subState": "READY",
                    "proAcdDeviceNumber": "1000",
                    "pgNumber": "PG001",
                    "withdraw": false
                }
                """);

            var state = await Service().GetStateAsync(null);

            state.Should().NotBeNull();
            state.MainState.Should().Be(OperatorState.OperatorMainState.Logon);
            state.SubState.Should().Be(OperatorState.OperatorDynamicState.Ready);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/cca/state");
        }

        [Fact]
        public async Task GetStateAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{ "mainState": "LOG_OFF", "withdraw": false }""");

            await Service().GetStateAsync("oxe1000");

            AssertRequest().Uri("/api/cca/state?loginName=oxe1000");
        }

        #endregion

        #region GetConfigurationAsync

        [Fact]
        public async Task GetConfigurationAsync_ReturnsConfiguration()
        {
            SetupHttpClient("""
                {
                    "type": "Agent",
                    "proacd": "1000",
                    "selfAssign": true,
                    "headset": false,
                    "help": true,
                    "multiline": false
                }
                """);

            var config = await Service().GetConfigurationAsync(null);

            config.Should().NotBeNull();
            config.Type.Should().Be(OperatorType.Agent);
            config.Proacd.Should().Be("1000");
            config.SelfAssign.Should().BeTrue();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/cca/config");
        }

        [Fact]
        public async Task GetConfigurationAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var config = await Service().GetConfigurationAsync(null);

            config.Should().BeNull();
        }

        #endregion

        #region LogonAsync / LogoffAsync

        [Fact]
        public async Task LogonAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().LogonAsync("1000", "PG001", true, "oxe1000");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/cca/logon?loginName=oxe1000")
                .JsonBody(json =>
                {
                    json.AssertValue("$.proAcdDeviceNumber", "1000");
                    json.AssertValue("$.pgGroupNumber", "PG001");
                    json.AssertValue("$.headset", true);
                });
        }

        [Fact]
        public async Task LogonAsync_WithoutPgNumber_OmitsPgGroupNumber()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().LogonAsync("1000", null, false, null);

            await AssertRequest()
                .JsonBody(json =>
                {
                    json.AssertValue("$.proAcdDeviceNumber", "1000");
                    json.AssertNull("$.pgGroupNumber");
                });
        }

        [Fact]
        public async Task LogoffAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().LogoffAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post, "/api/cca/logoff?loginName=oxe1000");
        }

        #endregion

        #region EnterAsync / ExitAsync

        [Fact]
        public async Task EnterAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().EnterAsync("PG001", "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/cca/enterPG?loginName=oxe1000",
                "{\"pgGroupNumber\":\"PG001\"}");
        }

        [Fact]
        public async Task ExitAsync_WhenInGroup_SendsExitRequest()
        {
            // ExitAsync first calls GetStateAsync then exitPG
            SetupHttpClient(
                ("""{ "mainState": "LOG_ON", "subState": "READY", "pgNumber": "PG001", "withdraw": false }""", HttpStatusCode.OK),
                ("", HttpStatusCode.NoContent)
            );

            var result = await Service().ExitAsync("oxe1000");

            result.Should().BeTrue();
            AssertRequest(0).Method(HttpMethod.Get).Uri("/api/cca/state?loginName=oxe1000");
            await AssertRequest(1)
                .Method(HttpMethod.Post)
                .Uri("/api/cca/exitPG?loginName=oxe1000")
                .JsonBody(json => json.AssertValue("$.pgGroupNumber", "PG001"));
        }

        [Fact]
        public async Task ExitAsync_WhenNotInGroup_ReturnsFalse()
        {
            SetupHttpClient("""{ "mainState": "LOG_ON", "subState": "READY", "withdraw": false }""");

            var result = await Service().ExitAsync(null);

            result.Should().BeFalse();
        }

        #endregion

        #region Agent state actions

        [Fact]
        public async Task SetPauseAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().SetPauseAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post, "/api/cca/pause?loginName=oxe1000");
        }

        [Fact]
        public async Task SetWrapupAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().SetWrapupAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post, "/api/cca/wrapUp?loginName=oxe1000");
        }

        [Fact]
        public async Task SetReadyAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().SetReadyAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post, "/api/cca/ready?loginName=oxe1000");
        }

        [Fact]
        public async Task RequestSnapshotAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestSnaphotAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/cca/state/snapshot?loginName=oxe1000");
        }

        #endregion

        #region Supervisor actions

        [Fact]
        public async Task RequestSupervisorHelpAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestSupervisorHelpAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/cca/supervisorHelp?loginName=oxe1000");
        }

        [Fact]
        public async Task CancelSupervisorHelpRequestAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CancelSupervisorHelpRequestAsync("sup001", "oxe1000");

            await AssertCalledWith(HttpMethod.Delete,
                "/api/cca/supervisorHelp?other=sup001&loginName=oxe1000");
        }

        [Fact]
        public async Task RejectAgentHelpRequestAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RejectAgentHelpRequestAsync("agent001", "oxe1000");

            await AssertCalledWith(HttpMethod.Delete,
                "/api/cca/supervisorHelp?other=agent001&loginName=oxe1000");
        }

        [Fact]
        public async Task RequestPermanentListeningAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestPermanentListeningAsync("agent001", "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/cca/permanentListening?loginName=oxe1000",
                "{\"agentNumber\":\"agent001\"}");
        }

        [Fact]
        public async Task CancelPermanentListeningAsync_WithLoginName_SendsDeleteWithLoginName()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CancelPermanentListeningAsync("oxe1000");

            AssertRequest().Method(HttpMethod.Delete)
                .Uri("/api/cca/permanentListening?loginName=oxe1000");
        }

        [Fact]
        public async Task CancelPermanentListeningAsync_WithoutLoginName_SendsDeleteWithoutQuery()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CancelPermanentListeningAsync();

            AssertRequest().Method(HttpMethod.Delete)
                .Uri("/api/cca/permanentListening");
        }

        [Fact]
        public async Task CancelPermanentListeningAsync_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().CancelPermanentListeningAsync("oxe1000");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CancelPermanentListeningAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.Forbidden);

            var result = await Service().CancelPermanentListeningAsync("oxe1000");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task RequestIntrusionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestIntrusionAsync("agent001", IntrusionMode.Discrete, "oxe1000");

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/cca/intrusion?loginName=oxe1000")
                .JsonBody(json =>
                {
                    json.AssertValue("$.agentNumber", "agent001");
                    json.AssertValue("$.mode", "DISCRETE");
                });
        }

        [Fact]
        public async Task ChangeIntrusionModeAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ChangeIntrusionModeAsync(IntrusionMode.Restricted, "oxe1000");

            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/cca/intrusion?loginName=oxe1000")
                .JsonBody(json => json.AssertValue("$.mode", "RESTRICTED"));
        }

        #endregion

        #region Withdraw

        [Fact]
        public async Task GetWithdrawReasonsAsync_ReturnsList()
        {
            SetupHttpClient("""
                {
                    "number": 2,
                    "reasons": [
                        { "index": 0, "label": "Lunch break" },
                        { "index": 1, "label": "Training" }
                    ]
                }
                """);

            var reasons = await Service().GetWithdrawReasonsAsync("PG001", null);

            reasons.Should().HaveCount(2);
            reasons[0].Index.Should().Be(0);
            reasons[0].Label.Should().Be("Lunch break");
            reasons[1].Index.Should().Be(1);
            reasons[1].Label.Should().Be("Training");
            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/cca/withdrawReasons?pgNumber=PG001");
        }

        [Fact]
        public async Task GetWithdrawReasonsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var reasons = await Service().GetWithdrawReasonsAsync("PG001", null);

            reasons.Should().BeNull();
        }

        [Fact]
        public async Task SetWithdrawAsync_SendsReasonIndex()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var reason = new WithdrawReason { Index = 1, Label = "Training" };
            await Service().SetWithdrawAsync(reason, "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/cca/withdraw?loginName=oxe1000",
                "{\"reasonIndex\":1}");
        }

        #endregion

        #region Skills

        [Fact]
        public async Task ActivateSkillsAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ActivateSkillsAsync(new List<int> { 101, 102 }, "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/cca/config/skills/activate?loginName=oxe1000",
                "{\"skills\":[101,102]}");
        }

        [Fact]
        public async Task DeactivateSkillsAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeactivateSkillsAsync(new List<int> { 101 }, "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/cca/config/skills/deactivate?loginName=oxe1000",
                "{\"skills\":[101]}");
        }

        #endregion

        #region Error handling

        [Fact]
        public async Task LogonAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.Forbidden);

            var result = await Service().LogonAsync("1000", null, false, null);

            result.Should().BeFalse();
        }

        #endregion
    }
}
