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
using o2g.Events.CallCenterAgent;
using o2g.Internal.Events;
using o2g.Internal.Types.CallCenterAgent;
using o2g.Internal.Utility;
using o2g.Types.CallCenterAgentNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    class AgentSkillActivation
    {
        public List<int> Skills { get; set; }
    }


    class LoggOnAgentRequest 
    {
        public string ProAcdDeviceNumber { get; set; }
        public string PgGroupNumber { get; set; }
        public bool Headset { get; set; }
    }

    class PgRequest
    {
        public string PgGroupNumber { get; set; }
    }

    class WithdrawReasons
    {
        public int Number { get; set; }
        public List<WithdrawReason> Reasons { get; set; }
    }

    class WithdrawAgentRequest
    {
        public int ReasonIndex { get; set; }
    }

    class PermanentListeningRequest
    {
        public string AgentNumber { get; set; }
    }

    class IntrusionRequest
    {
        public string AgentNumber { get; set; }
        public IntrusionMode Mode { get; set; }
    }

    class ChangeIntrusionModeRequest
    {
        public IntrusionMode Mode { get; set; }
    }


    internal class CallCenterAgentRest : AbstractRESTService, ICallCenterAgent
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnAgentStateChangedEvent>> AgentStateChanged
        {
            add => _eventHandlers.AgentStateChanged += value;
            remove => _eventHandlers.AgentStateChanged -= value;
        }

        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpRequestedEvent>> SupervisorHelpRequested
        {
            add => _eventHandlers.SupervisorHelpRequested += value;
            remove => _eventHandlers.SupervisorHelpRequested -= value;
        }

        public event System.EventHandler<O2GEventArgs<OnSupervisorHelpCancelledEvent>> SupervisorHelpCancelled
        {
            add => _eventHandlers.SupervisorHelpCancelled += value;
            remove => _eventHandlers.SupervisorHelpCancelled -= value;
        }

        public event EventHandler<O2GEventArgs<OnAgentSkillChangedEvent>> AgentSkillChanged
        {
            add => _eventHandlers.AgentSkillChanged += value;
            remove => _eventHandlers.AgentSkillChanged -= value;
        }

        public CallCenterAgentRest(Uri uri) : base(uri)
        {
        }

        public async Task<OperatorState> GetStateAsync(string loginName)
        {
            Uri uriGet = uri.Append("state");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<OperatorState>(response);
        }

        public async Task<OperatorConfiguration> GetConfigurationAsync(string loginName)
        {
            Uri uriGet = uri.Append("config");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GAgentConfig agentConfig = await GetResult<O2GAgentConfig>(response);
            if (agentConfig == null)
            {
                return null;
            }
            else
            {
                return agentConfig.ToOperatorConfiguration();
            }
        }

        public async Task<bool> LogonAsync(string proAcdNumber, string pgNumber, bool headset, string loginName)
        {
            Uri uriPost = uri.Append("logon");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            LoggOnAgentRequest req = new()
            {
                ProAcdDeviceNumber = AssertUtil.NotNullOrEmpty(proAcdNumber, "proAcdNumber"),
                PgGroupNumber = pgNumber,
                Headset = headset
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> LogoffAsync(string loginName)
        {
            Uri uriPost = uri.Append("logoff");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> EnterAsync(string pgNumber, string loginName)
        {
            Uri uriPost = uri.Append("enterPG");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            PgRequest req = new()
            {
                PgGroupNumber = AssertUtil.NotNullOrEmpty(pgNumber, "pgNumber")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        // Exit agent group need a call to GetOperatorState to retrieve the agent group
        public async Task<bool> ExitAsync(string loginName)
        {
            // First get the operator state to get the processing group
            OperatorState operatorState = await this.GetStateAsync(loginName);
            if (operatorState.PgNumber == null)
            {
                // The supervisor is NOT in a group return an error
                SetLastError(new o2g.Types.RestErrorInfo
                {
                    CanRetry = false,
                    HelpMessage = "Supervisor is not in a group"
                });
                return false;
            }
            else
            {
                Uri uriPost = uri.Append("exitPG");
                if (loginName != null)
                {
                    uriPost = uriPost.AppendQuery("loginName", loginName);
                }

                PgRequest req = new()
                {
                    PgGroupNumber = operatorState.PgNumber
                };

                var json = JsonSerializer.Serialize(req, serializeOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
                return await IsSucceeded(response);
            }
        }

        public async Task<bool> RequestSnaphotAsync(string loginName = null)
        {
            Uri uriPost = uri.Append("state/snapshot");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        private async Task<bool> DoAgentActionAsync(string action, string loginName)
        {
            Uri uriPost = uri.Append(action);
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public Task<bool> SetPauseAsync(string loginName)
        {
            return this.DoAgentActionAsync("pause", loginName);
        }

        public Task<bool> SetWrapupAsync(string loginName)
        {
            return this.DoAgentActionAsync("wrapUp", loginName);
        }

        public Task<bool> SetReadyAsync(string loginName)
        {
            return this.DoAgentActionAsync("ready", loginName);
        }

        public Task<bool> RequestSupervisorHelpAsync(string loginName)
        {
            return this.DoAgentActionAsync("supervisorHelp", loginName);
        }

        private async Task<bool> DoCancelSupervisorHelpRequestAsync(string otherNumber, string loginName)
        {
            Uri uriDelete = uri.Append("supervisorHelp").AppendQuery("other", otherNumber);
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public Task<bool> CancelSupervisorHelpRequestAsync(string supervisorNumber, string loginName)
        {
            return this.DoCancelSupervisorHelpRequestAsync(AssertUtil.NotNullOrEmpty(supervisorNumber, "supervisorNumber"), loginName);
        }

        public Task<bool> RejectAgentHelpRequestAsync(string agentNumber, string loginName)
        {
            return this.DoCancelSupervisorHelpRequestAsync(AssertUtil.NotNullOrEmpty(agentNumber, "agentNumber"), loginName);
        }

        public async Task<bool> RequestPermanentListeningAsync(string agentNumber, string loginName)
        {
            Uri uriPost = uri.Append("permanentListening");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            PermanentListeningRequest req = new()
            {
                AgentNumber = AssertUtil.NotNullOrEmpty(agentNumber, "agentNumber")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> CancelPermanentListeningAsync(string loginName = null)
        {
            Uri uriDelete = uri.Append("permanentListening");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> RequestIntrusionAsync(string agentNumber, IntrusionMode intrusionMode, string loginName)
        {
            Uri uriPost = uri.Append("intrusion");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            IntrusionRequest req = new()
            {
                AgentNumber = AssertUtil.NotNullOrEmpty(agentNumber, "agentNumber"),
                Mode = intrusionMode
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> ChangeIntrusionModeAsync(IntrusionMode newIntrusionMode, string loginName)
        {
            Uri uriPut = uri.Append("intrusion");
            if (loginName != null)
            {
                uriPut = uriPut.AppendQuery("loginName", loginName);
            }

            ChangeIntrusionModeRequest req = new()
            {
                Mode = newIntrusionMode
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public async Task<List<WithdrawReason>> GetWithdrawReasonsAsync(string pgNumber, string loginName)
        {
            Uri uriGet = uri
                .Append("withdrawReasons")
                .AppendQuery("pgNumber", AssertUtil.NotNullOrEmpty(pgNumber, "pgNumber"));
            
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            WithdrawReasons reasons = await GetResult<WithdrawReasons>(response);
            if (reasons == null)
            {
                return null;
            }
            else
            {
                return reasons.Reasons;
            }
        }

        public async Task<bool> SetWithdrawAsync(WithdrawReason reason, string loginName)
        {
            Uri uriPost = uri.Append("withdraw");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            WithdrawAgentRequest req = new()
            {
                ReasonIndex = AssertUtil.NotNull(reason, "reason").Index
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> ActivateSkillsAsync(List<int> skillNumbers, string loginName = null)
        {
            Uri uriPost = uri.Append("config/skills/activate");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            AgentSkillActivation req = new()
            {
                Skills = skillNumbers
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeactivateSkillsAsync(List<int> skillNumbers, string loginName = null)
        {
            Uri uriPost = uri.Append("config/skills/deactivate");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            AgentSkillActivation req = new()
            {
                Skills = skillNumbers
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }
    }
}
