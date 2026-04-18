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
using o2g.Types.TelephonyNS;
using o2g.Types.TelephonyNS.CallNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using o2g.Types.TelephonyNS.DeviceNS;
using o2g.Types.TelephonyNS.UserNS;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class TelephonyRestTests : ServiceTestBase
    {
        private TelephonyRest Service() =>
            DependancyResolver.Resolve(new TelephonyRest(BaseUri));

        #region Basic calls

        [Fact]
        public async Task BasicMakeCallAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().BasicMakeCallAsync("1000", "2000", true);

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Post, "/api/basicCall",
                "{\"deviceId\":\"1000\",\"callee\":\"2000\",\"autoAnswer\":true}");
        }

        [Fact]
        public async Task BasicAnswerCallAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().BasicAnswerCallAsync("1000");

            await AssertCalledWith(HttpMethod.Post, "/api/basicCall/answer",
                "{\"deviceId\":\"1000\"}");
        }

        [Fact]
        public async Task BasicDropMeAsync_WithoutLoginName_SendsNoQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().BasicDropMeAsync();

            await AssertCalledWith(HttpMethod.Post, "/api/basicCall/dropme");
        }

        [Fact]
        public async Task BasicDropMeAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().BasicDropMeAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post, "/api/basicCall/dropme?loginName=oxe1000");
        }

        #endregion

        #region MakeCallAsync

        [Fact]
        public async Task MakeCallAsync_WithCorrelatorData_SendsHexaBinaryAssociatedData()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakeCallAsync("1000", "2000", true, false,
                new CorrelatorData("txId=abc"), null, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.callee", "2000");
                    json.AssertValue("$.autoAnswer", true);
                    json.AssertValue("$.inhibitProgressTone", false);
                });
        }

        [Fact]
        public async Task MakeCallAsync_WithoutCorrelatorData_OmitsAssociatedData()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakeCallAsync("1000", "2000", true, false, null, null, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertNull("$.hexaBinaryAssociatedData");
                });
        }

        [Fact]
        public async Task MakeCallAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakeCallAsync("1000", "2000", true, false, null, null, "oxe1000");

            AssertRequest().Uri("/api/calls?loginName=oxe1000");
        }

        [Fact]
        public async Task MakeCallAsync_WithCallingNumber_IncludesCallingNumber()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakeCallAsync("1000", "2000", true, false, null, "9000", null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json => json.AssertValue("$.callingNumber", "9000"));
        }

        [Fact]
        public async Task MakePrivateCallAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakePrivateCallAsync("1000", "2000", "1234", true, "secret", null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.callee", "2000");
                    json.AssertValue("$.pin", "1234");
                    json.AssertValue("$.secretCode", "secret");
                });
        }

        [Fact]
        public async Task MakePrivateCallAsync_WithoutSecretCode_OmitsSecretCode()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakePrivateCallAsync("1000", "2000", "1234");

            await AssertRequest()
                .JsonBody(json =>
                {
                    json.AssertValue("$.pin", "1234");
                    json.AssertNull("$.secretCode");
                });
        }

        [Fact]
        public async Task MakeBusinessCallAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakeBusinessCallAsync("1000", "2000", "BIZ42", true, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.callee", "2000");
                    json.AssertValue("$.businessCode", "BIZ42");
                });
        }

        [Fact]
        public async Task MakeSupervisorCallAsync_SetsCallToSupervisorFlag()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakeSupervisorCallAsync("1000", true, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.acdCall.callToSupervisor", true);
                });
        }

        [Fact]
        public async Task MakePilotOrRSICallAsync_WithCallProfile_IncludesSkills()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var profile = new CallProfile(
                new CallProfile.Skill(101, level: 3, mandatory: true)
            );

            await Service().MakePilotOrRSICallAsync("1000", "3000", true, null, profile, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.callee", "3000");
                    json.AssertValue("$.acdCall.skills.skills[0].skillNumber", 101);
                    json.AssertValue("$.acdCall.skills.skills[0].acrStatus", true);
                });
        }

        [Fact]
        public async Task MakePilotOrRSISupervisedTransferCallAsync_SetsSupervisedTransferFlag()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MakePilotOrRSISupervisedTransferCallAsync("1000", "3000", null, null, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.acdCall.supervisedTransfer", true);
                });
        }

        #endregion

        #region Call operations

        [Fact]
        public async Task HoldAsync_WithLoginName_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().HoldAsync("abcdef12345", "12000", "oxe1000");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/abcdef12345/hold?loginName=oxe1000",
                "{\"deviceId\":\"12000\"}");
        }

        [Fact]
        public async Task HoldAsync_WithoutLoginName_OmitsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().HoldAsync("abcdef12345", "12000", null);

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/abcdef12345/hold",
                "{\"deviceId\":\"12000\"}");
        }

        [Fact]
        public async Task AlternateAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().AlternateAsync("call-001", "1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/alternate",
                "{\"deviceId\":\"1000\"}");
        }

        [Fact]
        public async Task AnswerAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().AnswerAsync("call-001", "1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/answer",
                "{\"deviceId\":\"1000\"}");
        }

        [Fact]
        public async Task AttachDataAsync_SendsHexaBinaryAssociatedData()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().AttachDataAsync("call-001", "1000",
                new CorrelatorData("txId=abc"));

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls/call-001/attachdata")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                });
        }

        [Fact]
        public async Task ReleaseCallAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ReleaseCallAsync("call-001", "oxe1000");

            await AssertCalledWith(HttpMethod.Delete,
                "/api/calls/call-001?loginName=oxe1000");
        }

        [Fact]
        public async Task BlindTransferAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().BlindTransferAsync("call-001", "3000", false, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls/call-001/blindtransfer")
                .JsonBody(json =>
                {
                    json.AssertValue("$.transferTo", "3000");
                    json.AssertValue("$.anonymous", false);
                });
        }

        [Fact]
        public async Task BlindTransferAsync_Anonymous_SetsFlag()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().BlindTransferAsync("call-001", "3000", true, null);

            await AssertRequest()
                .JsonBody(json => json.AssertValue("$.anonymous", true));
        }

        [Fact]
        public async Task MergeAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().MergeAsync("call-001", "call-002", "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/merge?loginName=oxe1000",
                "{\"heldCallRef\":\"call-002\"}");
        }

        [Fact]
        public async Task TransferAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().TransferAsync("call-001", "call-002", null);

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/transfer",
                "{\"heldCallRef\":\"call-002\"}");
        }

        [Fact]
        public async Task ParkAsync_WithTarget_SendsBody()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ParkAsync("call-001", "3000", null);

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/park",
                "{\"parkTo\":\"3000\"}");
        }

        [Fact]
        public async Task ParkAsync_WithoutTarget_SendsNoBody()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ParkAsync("call-001", null, null);

            await AssertCalledWith(HttpMethod.Post, "/api/calls/call-001/park");
        }

        [Fact]
        public async Task UnParkAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().UnParkAsync("1000", "call-001");

            await AssertCalledWith(HttpMethod.Post,
                "/api/devices/1000/unpark",
                "{\"heldCallRef\":\"call-001\"}");
        }

        [Fact]
        public async Task OverflowToVoiceMailAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().OverflowToVoiceMailAsync("call-001", null);

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/overflowToVoiceMail");
        }

        [Fact]
        public async Task RedirectAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RedirectAsync("call-001", "3000", false, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls/call-001/redirect")
                .JsonBody(json =>
                {
                    json.AssertValue("$.redirectTo", "3000");
                    json.AssertValue("$.anonymous", false);
                });
        }

        [Fact]
        public async Task RetrieveAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RetrieveAsync("call-001", "1000", null);

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/retrieve",
                "{\"deviceId\":\"1000\"}");
        }

        [Fact]
        public async Task ReconnectAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ReconnectAsync("call-001", "1000", "call-002", null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls/call-001/reconnect")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.enquiryCallRef", "call-002");
                });
        }

        [Fact]
        public async Task DropmeAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DropmeAsync("call-001", "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/dropme?loginName=oxe1000");
        }

        [Fact]
        public async Task DropParticipantAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DropParticipantAsync("call-001", "part-001", null);

            await AssertCalledWith(HttpMethod.Delete,
                "/api/calls/call-001/participants/part-001");
        }

        [Fact]
        public async Task CallbackAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().CallbackAsync("call-001", "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/callback?loginName=oxe1000");
        }

        [Fact]
        public async Task SendDtmfAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().SendDtmfAsync("call-001", "1000", "1234#");

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls/call-001/sendDtmf")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.number", "1234#");
                });
        }

        [Fact]
        public async Task SendAccountInfoAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().SendAccountInfoAsync("call-001", "1000", "ACC42");

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/calls/call-001/sendaccountinfo")
                .JsonBody(json =>
                {
                    json.AssertValue("$.deviceId", "1000");
                    json.AssertValue("$.accountInfo", "ACC42");
                });
        }

        [Fact]
        public async Task DoRecordActionAsync_AppendsActionQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DoRecordActionAsync("call-001", RecordingAction.Start, "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/calls/call-001/recording?loginName=oxe1000&action=start");
        }

        [Fact]
        public async Task PickUpAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().PickUpAsync("1000", "call-001", "2000", true);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/devices/1000/pickup")
                .JsonBody(json =>
                {
                    json.AssertValue("$.otherCallRef", "call-001");
                    json.AssertValue("$.otherPhoneNumber", "2000");
                    json.AssertValue("$.autoAnswer", true);
                });
        }

        [Fact]
        public async Task IntrusionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().IntrusionAsync("1000");

            await AssertCalledWith(HttpMethod.Post, "/api/devices/1000/intrusion");
        }

        [Fact]
        public async Task ToggleInterphonyAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().ToggleInterphonyAsync("1000");

            await AssertCalledWith(HttpMethod.Put, "/api/devices/1000/ithmicro");
        }

        #endregion

        #region Get calls and participants

        [Fact]
        public async Task GetCallsAsync_ReturnsList()
        {
            SetupHttpClient("""
                {
                    "calls": [
                        { "callRef": "call-001" },
                        { "callRef": "call-002" }
                    ]
                }
                """);

            var calls = await Service().GetCallsAsync(null);

            calls.Should().HaveCount(2);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/calls");
        }

        [Fact]
        public async Task GetCallsAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{"calls": []}""");

            await Service().GetCallsAsync("oxe1000");

            AssertRequest().Uri("/api/calls?loginName=oxe1000");
        }

        [Fact]
        public async Task GetCallsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var calls = await Service().GetCallsAsync(null);

            calls.Should().BeNull();
        }

        [Fact]
        public async Task GetCallAsync_SendsCorrectRequest()
        {
            SetupHttpClient("""{ "callRef": "call-001" }""");

            var call = await Service().GetCallAsync("call-001", null);

            call.Should().NotBeNull();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/calls/call-001");
        }

        [Fact]
        public async Task GetLegsAsync_ReturnsList()
        {
            SetupHttpClient("""
                {
                    "legs": [
                        { "deviceId": "1000" }
                    ]
                }
                """);

            var legs = await Service().GetLegsAsync("call-001", null);

            legs.Should().HaveCount(1);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/calls/call-001/deviceLegs");
        }

        [Fact]
        public async Task GetLegAsync_SendsCorrectRequest()
        {
            SetupHttpClient("""{ "deviceId": "1000" }""");

            var leg = await Service().GetLegAsync("call-001", "leg-001", null);

            leg.Should().NotBeNull();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/calls/call-001/deviceLegs/leg-001");
        }

        [Fact]
        public async Task GetParticipantsAsync_ReturnsList()
        {
            SetupHttpClient("""
                {
                    "participants": [
                        { "participantId": "part-001" }
                    ]
                }
                """);

            var participants = await Service().GetParticipantsAsync("call-001", null);

            participants.Should().HaveCount(1);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/calls/call-001/participants");
        }

        [Fact]
        public async Task GetParticipantAsync_SendsCorrectRequest()
        {
            SetupHttpClient("""{ "participantId": "part-001" }""");

            var participant = await Service().GetParticipantAsync("call-001", "part-001", null);

            participant.Should().NotBeNull();
            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/calls/call-001/participants/part-001");
        }

        #endregion

        #region Devices

        [Fact]
        public async Task GetDevicesStateAsync_ReturnsList()
        {
            SetupHttpClient("""
                {
                    "deviceStates": [
                        { "deviceId": "1000", "state": "IN_SERVICE" }
                    ]
                }
                """);

            var states = await Service().GetDevicesStateAsync(null);

            states.Should().HaveCount(1);
            states[0].DeviceId.Should().Be("1000");
            states[0].State.Should().Be(OperationalState.InService);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/devices");
        }

        [Fact]
        public async Task GetDeviceStateAsync_SendsCorrectRequest()
        {
            SetupHttpClient("""{ "deviceId": "1000", "state": "IN_SERVICE" }""");

            var state = await Service().GetDeviceStateAsync("1000", null);

            state.Should().NotBeNull();
            state.DeviceId.Should().Be("1000");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/devices/1000");
        }

        #endregion

        #region Hunting groups

        [Fact]
        public async Task HuntingGroupLogOnAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().HuntingGroupLogOnAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/huntingGroupLogOn?loginName=oxe1000");
        }

        [Fact]
        public async Task HuntingGroupLogOffAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().HuntingGroupLogOffAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Delete,
                "/api/huntingGroupLogOn?loginName=oxe1000");
        }

        [Fact]
        public async Task GetHuntingGroupStatusAsync_ReturnsStatus()
        {
            SetupHttpClient("""{ "Logon": true }""");

            var status = await Service().GetHuntingGroupStatusAsync(null);

            status.Should().NotBeNull();
            status.LoggedOn.Should().BeTrue();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/huntingGroupLogOn");
        }

        [Fact]
        public async Task AddMeToHuntingGroupAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().AddMeToHuntingGroupAsync("5000", null);

            await AssertCalledWith(HttpMethod.Post,
                "/api/huntingGroupMember/5000");
        }

        [Fact]
        public async Task AddMeToHuntingGroupAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().AddMeToHuntingGroupAsync("5000", "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/huntingGroupMember/5000?loginName=oxe1000");
        }

        [Fact]
        public async Task RemoveMeFromHuntingGroupAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RemoveMeFromHuntingGroupAsync("5000", null);

            await AssertCalledWith(HttpMethod.Delete,
                "/api/huntingGroupMember/5000");
        }

        [Fact]
        public async Task QueryHuntingGroupsAsync_ReturnsHuntingGroups()
        {
            SetupHttpClient("""
                {
                    "hgList": ["5000", "5001"],
                    "currentHg": "5000"
                }
                """);

            var groups = await Service().QueryHuntingGroupsAsync(null);

            groups.Should().NotBeNull();
            groups.List.Should().HaveCount(2);
            groups.Current.Should().Be("5000");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/huntingGroups");
        }

        #endregion

        #region Callbacks

        [Fact]
        public async Task GetCallbacksAsync_ReturnsList()
        {
            SetupHttpClient("""
                {
                    "callbacks": [
                        { "callbackId": "cb-001" }
                    ]
                }
                """);

            var callbacks = await Service().GetCallbacksAsync(null);

            callbacks.Should().HaveCount(1);
            callbacks[0].CallbackId.Should().Be("cb-001");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/incomingCallbacks");
        }

        [Fact]
        public async Task DeleteCallbacksAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeleteCallbacksAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Delete,
                "/api/incomingCallbacks?loginName=oxe1000");
        }

        [Fact]
        public async Task DeleteCallbackAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeleteCallbackAsync("cb-001", null);

            await AssertCalledWith(HttpMethod.Delete,
                "/api/incomingCallbacks/cb-001");
        }

        [Fact]
        public async Task RequestCallbackAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestCallbackAsync("2000", null);

            await AssertCalledWith(HttpMethod.Post,
                "/api/outgoingCallbacks",
                "{\"callee\":\"2000\"}");
        }

        #endregion

        #region Mini messages

        [Fact]
        public async Task SendMiniMessageAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().SendMiniMessageAsync("2000", "Hello!", null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/miniMessages")
                .JsonBody(json =>
                {
                    json.AssertValue("$.recipient", "2000");
                    json.AssertValue("$.message", "Hello!");
                });
        }

        [Fact]
        public async Task GetMiniMessageAsync_ReturnsMessage()
        {
            SetupHttpClient("""
                {
                    "sender": "2000",
                    "message": "Hello!"
                }
                """);

            var msg = await Service().GetMiniMessageAsync(null);

            msg.Should().NotBeNull();
            msg.Sender.Should().Be("2000");
            msg.Message.Should().Be("Hello!");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/miniMessages");
        }

        #endregion

        #region Desk sharing

        [Fact]
        public async Task DeskSharingLogOnAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeskSharingLogOnAsync("9000", "oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/deskSharing?loginName=oxe1000",
                "{\"dssDeviceNumber\":\"9000\"}");
        }

        [Fact]
        public async Task DeskSharingLogOffAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeskSharingLogOffAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Delete,
                "/api/deskSharing?loginName=oxe1000");
        }

        #endregion

        #region State and snapshot

        [Fact]
        public async Task GetStateAsync_ReturnsTelephonicState()
        {
            SetupHttpClient("""
                {
                    "calls": [],
                    "deviceCapabilities": [],
                    "userState": "FREE",
                    "deviceStates": []
                }
                """);

            var state = await Service().GetStateAsync(null);

            state.Should().NotBeNull();
            state.UserState.Should().Be(UserState.Free);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/state");
        }

        [Fact]
        public async Task RequestSnapshotAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestSnapshotAsync(null);

            await AssertCalledWith(HttpMethod.Post, "/api/state/snapshot");
        }

        [Fact]
        public async Task RequestSnapshotAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().RequestSnapshotAsync("oxe1000");

            await AssertCalledWith(HttpMethod.Post,
                "/api/state/snapshot?loginName=oxe1000");
        }

        #endregion

        #region GetPilotInfoAsync

        [Fact]
        public async Task GetPilotInfoAsync_WithoutParameters_SendsNoBody()
        {
            SetupHttpClient("""
                {
                    "number": "3000",
                    "waitingTime": 12,
                    "saturation": false,
                    "supervisedTransfer": true
                }
                """);

            var info = await Service().GetPilotInfoAsync(1, "3000", null, null);

            info.Should().NotBeNull();
            info.Number.Should().Be("3000");
            info.WaitingTime.Should().Be(12);
            info.SupervisedTransfer.Should().BeTrue();
            AssertRequest().Method(HttpMethod.Post).Uri("/api/pilots/1/3000/transferInfo");
        }

        [Fact]
        public async Task GetPilotInfoAsync_WithAgentNumber_SendsBody()
        {
            SetupHttpClient("""{ "number": "3000" }""");

            var parameters = new PilotTransferQueryParameters()
                .SetAgentNumber("1234");

            await Service().GetPilotInfoAsync(1, "3000", parameters, null);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/pilots/1/3000/transferInfo")
                .JsonBody(json => json.AssertValue("$.agentNumber", "1234"));
        }

        [Fact]
        public async Task GetPilotInfoAsync_WithPriorityTransfer_IncludesFlag()
        {
            SetupHttpClient("""{ "number": "3000" }""");

            var parameters = new PilotTransferQueryParameters()
                .SetPriorityTransfer(true);

            await Service().GetPilotInfoAsync(1, "3000", parameters, null);

            await AssertRequest()
                .JsonBody(json => json.AssertValue("$.priorityTransfer", true));
        }

        [Fact]
        public async Task GetPilotInfoAsync_WithCallProfile_IncludesSkills()
        {
            SetupHttpClient("""{ "number": "3000" }""");

            var profile = new CallProfile(
                new CallProfile.Skill(101, level: 3, mandatory: true)
            );
            var parameters = new PilotTransferQueryParameters()
                .SetCallProfile(profile);

            await Service().GetPilotInfoAsync(1, "3000", parameters, null);

            await AssertRequest()
                .JsonBody(json =>
                {
                    json.AssertValue("$.skills.skills[0].skillNumber", 101);
                    json.AssertValue("$.skills.skills[0].acrStatus", true);
                });
        }

        [Fact]
        public async Task GetPilotInfoAsync_WithLoginName_AppendsQueryParam()
        {
            SetupHttpClient("""{ "number": "3000" }""");

            await Service().GetPilotInfoAsync(1, "3000", null, "oxe1000");

            AssertRequest().Uri("/api/pilots/1/3000/transferInfo?loginName=oxe1000");
        }

        #endregion

        #region Error handling

        [Fact]
        public async Task HoldAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.BadRequest);

            var result = await Service().HoldAsync("call-001", "1000", null);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetDevicesStateAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var states = await Service().GetDevicesStateAsync(null);

            states.Should().BeNull();
        }

        #endregion
    }
}