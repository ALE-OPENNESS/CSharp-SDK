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
using o2g.Events.Telephony;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types.TelephonyNS;
using o2g.Types.TelephonyNS.CallNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using o2g.Types.TelephonyNS.DeviceNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    class MakeBasicCallRequest
    {
        public string DeviceId { get; set; }
        public string Callee { get; set; }
        public bool AutoAnswer { get; set; }
    }

    class ParticipantsList
    {
        public List<Participant> Participants { get; set; }
    }

    class CallsList
    {
        public List<PbxCall> Calls { get; set; }
    }

    class LegsList
    {
        public List<Leg> Legs { get; set; }
    }

    class DeviceStatesList
    {
        public List<DeviceState> DeviceStates { get; set; }
    }

    class CallbacksList
    {
        public List<Callback> Callbacks { get; set; }
    }

    class DeviceIdRequest
    {
        public string DeviceId { get; set; }
    }

    class HeldCallRefRequest
    {
        public string HeldCallRef { get; set; }
    }


    class SendAssociatedDataRequest
    {
        public string DeviceId { get; set; }
        public string AssociatedData { get; set; }
        public string HexaBinaryAssociatedData { get; set; }
    }

    class BlindTransferRequest
    {
        public string TransferTo { get; set; }
        public bool Anonymous { get; set; }
    }

    class ParkRequest
    {
        public string ParkTo { get; set; }
    }

    class PickupRequest
    {
        public string OtherCallRef { get; set; }
        public string OtherPhoneNumber { get; set; }
        public bool AutoAnswer { get; set; }
    }

    class DSLogOnRequest
    {
        public string DssDeviceNumber { get; set; }
    }

    class RedirectRequest
    {
        public string RedirectTo { get; set; }
        public bool Anonymous { get; set; }
    }

    class CallbackRequest
    {
        public string Callee { get; set; }
    }

    class ReconnectRequest
    {
        public string DeviceId { get; set; }
        public string EnquiryCallRef { get; set; }
    }

    class SendAccountInfoRequest
    {
        public string DeviceId { get; set; }
        public string AccountInfo { get; set; }
    }

    class SendDtmfRequest
    {
        public string DeviceId { get; set; }
        public string Number { get; set; }
    }

    class MiniMessageRequest
    {
        public string Recipient { get; set; }
        public string Message { get; set; }
    }


    class ACRSkills
    {
        public List<AcrSkill> Skills { get; set; }
    }


    class AcdCallParam
    {
        public bool SupervisedTransfer { get; set; }
        public ACRSkills Skills { get; set; }
        public bool CallToSupervisor { get; set; }
    }


    class MakeCallRequest
    {
        public string DeviceId { get; set; }
        public string Callee { get; set; }
        public bool AutoAnswer { get; set; }
        public bool InhibitProgressTone { get; set; }
        public string AssociatedData { get; set; }
        public string HexaBinaryAssociatedData { get; set; }
        public string Pin { get; set; }
        public string SecretCode { get; set; }
        public string BusinessCode { get; set; }
        public String CallingNumber { get; set; }
        public AcdCallParam AcdCall { get; set; }
    }


    internal class TelephonyRest : AbstractRESTService, ITelephony
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnCallCreatedEvent>> CallCreated
        {
            add => _eventHandlers.CallCreated += value;
            remove => _eventHandlers.CallCreated -= value;
        }
        public event EventHandler<O2GEventArgs<OnCallModifiedEvent>> CallModified
        {
            add => _eventHandlers.CallModified += value;
            remove => _eventHandlers.CallModified -= value;
        }
        public event EventHandler<O2GEventArgs<OnCallRemovedEvent>> CallRemoved
        {
            add => _eventHandlers.CallRemoved += value;
            remove => _eventHandlers.CallRemoved -= value;
        }
        public event EventHandler<O2GEventArgs<OnUserStateModifiedEvent>> UserStateModified
        {
            add => _eventHandlers.UserStateModified += value;
            remove => _eventHandlers.UserStateModified -= value;
        }
        public event EventHandler<O2GEventArgs<OnTelephonyStateEvent>> TelephonyState
        {
            add => _eventHandlers.TelephonyState += value;
            remove => _eventHandlers.TelephonyState -= value;
        }
        public event EventHandler<O2GEventArgs<OnDeviceStateModifiedEvent>> DeviceStateModified
        {
            add => _eventHandlers.DeviceStateModified += value;
            remove => _eventHandlers.DeviceStateModified -= value;
        }
        public event EventHandler<O2GEventArgs<OnDynamicStateChangedEvent>> DynamicStateChanged
        {
            add => _eventHandlers.DynamicStateChanged += value;
            remove => _eventHandlers.DynamicStateChanged -= value;
        }


        public TelephonyRest(Uri uri) : base(uri)
        {
        }

        public async Task<bool> AddHuntingGroupMemberAsync(string hgNumber, string loginName)
        {
            Uri uriPost = uri.Append("huntingGroupMember", AssertUtil.NotNullOrEmpty(hgNumber, "hgNumber"));
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> AlternateAsync(string callRef, string deviceId)
        {
            DeviceIdRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "alternate"), content);
            return await IsSucceeded(response);
        }

        public async Task<bool> AnswerAsync(string callRef, string deviceId)
        {
            DeviceIdRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "answer"), content);
            return await IsSucceeded(response);
        }


        public async Task<bool> AttachDataAsync(string callRef, string deviceId, string associatedData)
        {
            SendAssociatedDataRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                AssociatedData = AssertUtil.NotNullOrEmpty(associatedData, "associatedData")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "attachdata"), content);
            return await IsSucceeded(response);
        }

        public async Task<bool> AttachDataAsync(string callRef, string deviceId, byte[] associatedData)
        {
            SendAssociatedDataRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                HexaBinaryAssociatedData = HexaUtil.FromByteArray(AssertUtil.NotNull<byte[]>(associatedData, "associatedData"))
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "attachdata"), content);
            return await IsSucceeded(response);
        }

        public async Task<bool> BasicAnswerCallAsync(string deviceId)
        {
            DeviceIdRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri.Append("basicCall/answer"), content);
            return await IsSucceeded(response);
        }

        public async Task<bool> BasicDropMeAsync(string loginName = null)
        {
            Uri uriPost = uri.Append("basicCall", "dropme");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> BasicMakeCallAsync(string deviceId, string callee, bool autoAnswer)
        {
            MakeBasicCallRequest mcr = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(callee, "callee"),
                AutoAnswer = autoAnswer
            };

            var json = JsonSerializer.Serialize(mcr, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri.Append("basicCall"), content);
            return await IsSucceeded(response);
        }


        public async Task<bool> BlindTransferAsync(string callRef, string transferTo, bool anonymous, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "blindtransfer");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            BlindTransferRequest req = new()
            {
                TransferTo = AssertUtil.NotNullOrEmpty(transferTo, "transferTo"),
                Anonymous = anonymous
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> CallbackAsync(string callRef, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "callback");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteHuntingGroupMemberAsync(string hgNumber, string loginName)
        {
            Uri uriDelete = uri.Append("huntingGroupMember", AssertUtil.NotNullOrEmpty(hgNumber, "hgNumber"));
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteCallbacksAsync(string loginName)
        {
            Uri uriDelete = uri.Append("incomingCallbacks");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteCallbackAsync(string callbackId, string loginName)
        {
            Uri uriDelete = uri.Append("incomingCallbacks", AssertUtil.NotNullOrEmpty(callbackId, "callbackId"));
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeskSharingLogOffAsync(string loginName)
        {
            Uri uriDelete = uri.Append("deskSharing");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeskSharingLogOnAsync(string dssDeviceNumber, string loginName)
        {
            Uri uriPost = uri.Append("deskSharing");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            DSLogOnRequest req = new()
            {
                DssDeviceNumber = AssertUtil.NotNullOrEmpty(dssDeviceNumber, "dssDeviceNumber")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> DropmeAsync(string callRef, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "dropme");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> DropParticipantAsync(string callRef, string participantId, string loginName)
        {
            Uri uriDelete = uri.Append(
                "calls", 
                AssertUtil.NotNullOrEmpty(callRef, "callRef"), 
                "participants",
                AssertUtil.NotNullOrEmpty(participantId, "participantId"));
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<PbxCall> GetCallAsync(string callRef, string loginName)
        {
            Uri uriGet = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"));
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<PbxCall>(response);
        }

        public async Task<List<PbxCall>> GetCallsAsync(string loginName)
        {
            Uri uriGet = uri.Append("calls");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            CallsList calls = await GetResult<CallsList>(response);
            if (calls == null)
            {
                return null;
            }
            else
            {
                return calls.Calls;
            }
        }

        public async Task<Leg> GetDeviceLegAsync(string callRef, string legId, string loginName)
        {
            Uri uriGet = uri.Append(
                "calls", 
                AssertUtil.NotNullOrEmpty(callRef, "callRef"), 
                "deviceLegs",
                AssertUtil.NotNullOrEmpty(legId, "legId"));

            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<Leg>(response);
        }

        public async Task<List<Leg>> GetDeviceLegsAsync(string callRef, string loginName)
        {
            Uri uriGet = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "deviceLegs");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            LegsList legs = await GetResult<LegsList>(response);
            if (legs == null)
            {
                return null;
            }
            else
            {
                return legs.Legs;
            }
        }

        public async Task<List<DeviceState>> GetDevicesStateAsync(string loginName)
        {
            Uri uriGet = uri.Append("devices");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            DeviceStatesList states = await GetResult<DeviceStatesList>(response);
            if (states == null)
            {
                return null;
            }
            else
            {
                return states.DeviceStates;
            }
        }

        public async Task<DeviceState> GetDeviceStateAsync(string deviceId, string loginName)
        {
            Uri uriGet = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"));
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<DeviceState>(response);
        }

        public async Task<List<Callback>> GetCallbacksAsync(string loginName)
        {
            Uri uriGet = uri.Append("incomingCallbacks");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            CallbacksList callbacks = await GetResult<CallbacksList>(response);
            if (callbacks == null)
            {
                return null;
            }
            else
            {
                return callbacks.Callbacks;
            }
        }

        public async Task<MiniMessage> GetMiniMessageAsync(string loginName)
        {
            Uri uriGet = uri.Append("miniMessages");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<MiniMessage>(response);
        }

        public async Task<Participant> GetParticipantAsync(string callRef, string participantId, string loginName)
        {
            Uri uriGet = uri.Append(
                "calls", 
                AssertUtil.NotNullOrEmpty(callRef, "callRef"), 
                "participants",
                AssertUtil.NotNullOrEmpty(participantId, "participantId"));

            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<Participant>(response);
        }

        public async Task<TelephonicState> GetStateAsync(string loginName)
        {
            Uri uriGet = uri.Append("state");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<TelephonicState>(response);
        }

        public async Task<bool> HoldAsync(string callRef, string deviceId, string loginName)
        {
            Uri postUri = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "hold");

            DeviceIdRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(postUri, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> HuntingGroupLogOffAsync(string loginName)
        {
            Uri uriDelete = uri.Append("huntingGroupLogOn");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> HuntingGroupLogOnAsync(string loginName)
        {
            Uri uriPost = uri.Append("huntingGroupLogOn");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<HuntingGroupStatus> GetHuntingGroupStatusAsync(string loginName)
        {
            Uri uriGet = uri.Append("huntingGroupLogOn");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);

            response.EnsureSuccessStatusCode();
            string jsonCode = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<HuntingGroupStatus>(jsonCode, serializeOptions);
        }

        public async Task<bool> MergeAsync(string callRef, string heldCallRef, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "merge");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HeldCallRefRequest req = new()
            {
                HeldCallRef = AssertUtil.NotNullOrEmpty(heldCallRef, "heldCallRef")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> ReleaseCallAsync(string callRef, string loginName)
        {
            Uri uriDelete = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"));
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }


        public async Task<bool> OverflowToVoiceMailAsync(string callRef, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "overflowToVoiceMail");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> ParkAsync(string callRef, string parkTo, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "park");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            ParkRequest req = new()
            {
                ParkTo = AssertUtil.NotNullOrEmpty(parkTo, "parkTo")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> PickUpAsync(string deviceId, string otherCallRef, string otherPhoneNumber, bool autoAnswer)
        {
            Uri uriPost = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"), "pickup");

            PickupRequest req = new()
            {
                OtherCallRef = AssertUtil.NotNullOrEmpty(otherCallRef, "otherCallRef"),
                OtherPhoneNumber = AssertUtil.NotNullOrEmpty(otherPhoneNumber, "otherPhoneNumber"),
                AutoAnswer = autoAnswer
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }


        public async Task<bool> IntrusionAsync(string deviceId)
        {
            Uri uriPost = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"), "intrusion");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }


        public async Task<bool> ToggleInterphonyAsync(string deviceId)
        {
            Uri uriPut = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"), "ithmicro");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, null);
            return await IsSucceeded(response);
        }


        public async Task<HuntingGroups> QueryHuntingGroupsAsync(string loginName)
        {
            Uri uriGet = uri.Append("huntingGroups");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<HuntingGroups>(response);
        }

        public async Task<bool> ReconnectAsync(string callRef, string deviceId, string enquiryCallRef, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "reconnect");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            ReconnectRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                EnquiryCallRef = AssertUtil.NotNullOrEmpty(enquiryCallRef, "enquiryCallRef")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> RecordingAsync(string callRef, RecordingAction action, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "recording");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            uriPost = uriPost.AppendQuery("action", action.ToString().ToLower());

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> RedirectAsync(string callRef, string redirectTo, bool anonymous, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "redirect");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            RedirectRequest req = new()
            {
                RedirectTo = AssertUtil.NotNullOrEmpty(redirectTo, "redirectTo"),
                Anonymous = anonymous
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> RequestCallbackAsync(string callee, string loginName)
        {
            Uri uriPost = uri.Append("outgoingCallbacks");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            CallbackRequest req = new()
            {
                Callee = AssertUtil.NotNullOrEmpty(callee, "callee")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> RequestSnapshotAsync(string loginName)
        {
            Uri uriPost = uri.Append("state/snapshot");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> RetrieveAsync(string callRef, string deviceId, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "retrieve");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            DeviceIdRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> SendAccountInfoAsync(string callRef, string deviceId, string accountInfo)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "sendaccountinfo");

            SendAccountInfoRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                AccountInfo = AssertUtil.NotNullOrEmpty(accountInfo, "accountInfo")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> SendDtmfAsync(string callRef, string deviceId, string number)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "sendDtmf");

            SendDtmfRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Number = AssertUtil.NotNullOrEmpty(number, "number")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> SendMiniMessageAsync(string recipient, string message, string loginName)
        {
            Uri uriPost = uri.Append("miniMessages");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            MiniMessageRequest req = new()
            {
                Recipient = AssertUtil.NotNullOrEmpty(recipient, "recipient"),
                Message = AssertUtil.NotNullOrEmpty(message, "message")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> TransferAsync(string callRef, string heldCallRef, string loginName)
        {
            Uri uriPost = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "transfer");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HeldCallRefRequest req = new()
            {
                HeldCallRef = AssertUtil.NotNullOrEmpty(heldCallRef, "heldCallRef")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> UnParkAsync(string deviceId, string heldCallRef)
        {
            Uri uriPost = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"), "unpark");

            HeldCallRefRequest req = new()
            {
                HeldCallRef = AssertUtil.NotNullOrEmpty(heldCallRef, "heldCallRef")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakeCallAsync(string deviceId, string callee, bool autoAnswer, bool inhibitProgressTone, string associatedData, string callingNumber, string loginName)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(callee, "callee"),
                AutoAnswer = autoAnswer,
                InhibitProgressTone = inhibitProgressTone,
                AssociatedData = associatedData,
                CallingNumber = callingNumber
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakeCallAsync(string deviceId, string callee, bool autoAnswer, bool inhibitProgressTone, byte[] associatedData, string callingNumber, string loginName)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(callee, "callee"),
                AutoAnswer = autoAnswer,
                InhibitProgressTone = inhibitProgressTone,
                HexaBinaryAssociatedData = HexaUtil.FromByteArray(associatedData),
                CallingNumber = callingNumber
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakePrivateCallAsync(string deviceId, string callee, string pin, bool autoAnswer = true, string secretCode = null, string loginName = null)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(callee, "callee"),
                AutoAnswer = autoAnswer,
                Pin = AssertUtil.NotNullOrEmpty(pin, "pin"),
                SecretCode = secretCode
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakeBusinessCallAsync(string deviceId, string callee, string businessCode, bool autoAnswer = true, string loginName = null)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(callee, "callee"),
                BusinessCode = AssertUtil.NotNullOrEmpty(businessCode, "businessCode"),
                AutoAnswer = autoAnswer
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakeSupervisorCallAsync(string deviceId, bool autoAnswer = true, string loginName = null)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                AutoAnswer = autoAnswer,
                AcdCall = new()
                {
                    CallToSupervisor = true
                }
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakePilotOrRSISupervisedTransferCallAsync(string deviceId, string pilot, string associatedData = null, List<AcrSkill> callProfile = null, string loginName = null)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            ACRSkills acrSkills = null;
            if (callProfile != null)
            {
                acrSkills = new()
                {
                    Skills = callProfile
                };
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(pilot, "pilot"),
                AssociatedData = associatedData,
                AcdCall = new()
                {
                    SupervisedTransfer = true,
                    Skills = acrSkills
                }
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }


        public async Task<bool> MakePilotOrRSISupervisedTransferCallAsync(string deviceId, string pilot, byte[] associatedData = null, List<AcrSkill> callProfile = null, string loginName = null)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            ACRSkills acrSkills = null;
            if (callProfile != null)
            {
                acrSkills = new()
                {
                    Skills = callProfile
                };
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(pilot, "pilot"),
                HexaBinaryAssociatedData = HexaUtil.FromByteArray(associatedData),
                AcdCall = new()
                {
                    SupervisedTransfer = true,
                    Skills = acrSkills
                }
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakePilotOrRSICallAsync(string deviceId, string pilot, bool autoAnswer = true, string associatedData = null, List<AcrSkill> callProfile = null, string loginName = null)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            ACRSkills acrSkills = null;
            if (callProfile != null)
            {
                acrSkills = new()
                {
                    Skills = callProfile
                };
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(pilot, "pilot"),
                AutoAnswer = autoAnswer,
                AssociatedData = associatedData,
                AcdCall = new()
                {
                    Skills = acrSkills
                }
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> MakePilotOrRSICallAsync(string deviceId, string pilot, bool autoAnswer = true, byte[] associatedData = null, List<AcrSkill> callProfile = null, string loginName = null)
        {
            Uri uriPost = uri.Append("calls");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            ACRSkills acrSkills = null;
            if (callProfile != null)
            {
                acrSkills = new()
                {
                    Skills = callProfile
                };
            }

            MakeCallRequest req = new()
            {
                DeviceId = AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                Callee = AssertUtil.NotNullOrEmpty(pilot, "pilot"),
                AutoAnswer = autoAnswer,
                HexaBinaryAssociatedData = HexaUtil.FromByteArray(associatedData),
                AcdCall = new()
                {
                    Skills = acrSkills
                }
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }


        public async Task<List<Participant>> GetParticipantsAsync(string callRef, string loginName = null)
        {
            Uri uriGet = uri.Append("calls", AssertUtil.NotNullOrEmpty(callRef, "callRef"), "participants");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ParticipantsList participants = await GetResult<ParticipantsList>(response);
            if (participants == null)
            {
                return null;
            }
            else
            {
                return participants.Participants;
            }
        }
    }
}
