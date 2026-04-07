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
using o2g.Internal.Utility;
using o2g.Types.MessagingNS;
using o2g.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{

    internal class MailBoxes
    {
        public List<MailBox> Mailboxes { get; set; }
    }

    internal class ConnectRequest
    {
        public string Password { get; set; }
    }

    internal class VoicemailsList
    {
        public List<VoiceMessage> Voicemails { get; set; }
    }

    internal class MessagingRest : AbstractRESTService, IMessaging
    {
        public MessagingRest(Uri uri) : base(uri)
        {

        }

        public async Task<bool> DeleteVoiceMessageAsync(string mailboxId, string voicemailId, string loginName)
        {
            Uri uriDelete = uri.Append(
                AssertUtil.NotNullOrEmpty(mailboxId, "mailboxId"), 
                "voicemails",
                AssertUtil.NotNullOrEmpty(voicemailId, "voicemailId"));
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteVoiceMessagesAsync(string mailboxId, string[] msgIds, string loginName)
        {
            Uri uriDelete = uri.Append(AssertUtil.NotNullOrEmpty(mailboxId, "mailboxId"), "voicemails");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            if (msgIds != null)
            {
                uriDelete = uriDelete.AppendQuery("msgIds", string.Join(';', msgIds));
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return response.IsSuccessStatusCode;
        }

        public Task<bool> DeleteVoiceMessagesAsync(string mailboxId, List<string> msgIds, string loginName)
        {
            return DeleteVoiceMessagesAsync(mailboxId, msgIds.ToArray(), loginName);
        }

        public async Task<string> DownloadVoiceMessageAsync(string mailboxId, string voicemailId, string wavPath, string loginName)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(mailboxId, "mailboxId"), 
                "voicemails",
                AssertUtil.NotNullOrEmpty(voicemailId, "voicemailId"));
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await DownloadFile(response, wavPath, "wav");
        }

        public async Task<List<MailBox>> GetMailboxesAsync(string loginName)
        {
            Uri uriGet = uri;
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            MailBoxes mailboxes = await GetResult<MailBoxes>(response);
            if (mailboxes == null)
            {
                return null;
            }
            else
            {
                return mailboxes.Mailboxes;
            }
        }

        public async Task<MailBoxInfo> GetMailboxInfoAsync(string mailboxId, string password, string loginName)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(mailboxId, "mailboxId"));
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response;
            if (password != null)
            {
                ConnectRequest req = new()
                {
                    Password = password
                };

                var json = JsonSerializer.Serialize(req, serializeOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                response = await httpClient.PostAsync(uriPost, content);
            }
            else
            {
                uriPost = uriPost.AppendQuery("withUserPwd");
                response = await httpClient.PostAsync(uriPost, null);
            }

            return await GetResult<MailBoxInfo>(response);
        }

        public async Task<List<VoiceMessage>> GetVoiceMessagesAsync(string mailboxId, bool newOnly, int? offset, int? limit, string loginName)
        {
            Uri uriGet = uri.Append(AssertUtil.NotNullOrEmpty(mailboxId, "mailboxId"), "voicemails");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }
            if (offset != null)
            {
                uriGet = uriGet.AppendQuery("offset", offset.ToString());
            }
            if (limit != null)
            {
                uriGet = uriGet.AppendQuery("limit", limit.ToString());
            }
            if (newOnly)
            {
                uriGet = uriGet.AppendQuery("newOnly", newOnly.ToString());
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);

            response.EnsureSuccessStatusCode();
            string jsonCode = await response.Content.ReadAsStringAsync();

            VoicemailsList voicemails = JsonSerializer.Deserialize<VoicemailsList>(jsonCode, serializeOptions);
            return voicemails.Voicemails;
        }

        private static string MakeMsgQuery(string[] msgIds)
        {
            StringBuilder sb = new();
            for (int i = 0; i < msgIds.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(';');
                }
                sb.Append(msgIds[i]);
            }

            return sb.ToString();
        }

    }
}
