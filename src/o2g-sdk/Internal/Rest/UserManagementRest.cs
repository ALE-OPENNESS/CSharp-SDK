/*
* Copyright 2025 ALE International
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
using o2g.Events.Users;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types.UsersNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{

    class UserCreateRequest
    {
        public string NodeId { get; set; }
        public string[] DeviceNumbers { get; set; }
        public Boolean All { get; set; }
    }

    class Users
    {
        public List<User> users { get; set; }
    }


    internal class UserManagementRest : AbstractRESTService, IUserManagement
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnUserCreatedEvent>> UserCreated
        {
            add => _eventHandlers.UserCreated += value;
            remove => _eventHandlers.UserCreated -= value;
        }

        public event EventHandler<O2GEventArgs<OnUserDeletedEvent>> UserDeleted
        {
            add => _eventHandlers.UserDeleted += value;
            remove => _eventHandlers.UserDeleted -= value;
        }

        public event EventHandler<O2GEventArgs<OnUserInfoChangedEvent>> UserInfoChanged
        {
            add => _eventHandlers.UserInfoChanged += value;
            remove => _eventHandlers.UserInfoChanged -= value;
        }

        public UserManagementRest(Uri uri) : base(uri)
        {

        }

        public async Task<User> GetByLoginNameAsync(string loginName)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri.Append(loginName));
            return await GetResult<User>(response);
        }

        private static string MakeNodeQuery(int[] nodeIds)
        {
            StringBuilder sb = new();
            for (int i = 0; i < nodeIds.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(';');
                }
                sb.Append(nodeIds[i]);
            }

            return sb.ToString();
        }


        public async Task<List<string>> GetLoginsAsync(int[] nodeIds = null)
        {
            Uri uriGet = uri;

            if (nodeIds != null)
            {
                uriGet = uriGet.AppendQuery("nodeIds", MakeNodeQuery(nodeIds));
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            LoginsResponse logins = await GetResult<LoginsResponse>(response);
            if (logins == null)
            {
                return null;
            }
            else
            {
                return logins.LoginNames;
            }
        }

        public async Task<string> GetLoginAsync(string deviceNumber)
        {
            Uri uriGet = uri.AppendQuery(deviceNumber, AssertUtil.NotNullOrEmpty(deviceNumber, "deviceNumber"));

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            LoginsResponse logins = await GetResult<LoginsResponse>(response);
            if ((logins == null) || (logins.LoginNames == null) || (logins.LoginNames.Count == 0))
            {
                return null;
            }
            else
            {
                return logins.LoginNames.First();
            }
        }

        public async Task<User> GetUserAsync(string loginName)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri.Append(AssertUtil.NotNullOrEmpty(loginName, "loginName")));
            return await GetResult<User>(response);
        }

        public async Task<List<User>> CreateUsersAsync(int nodeId, string[] deviceNumbers)
        {
            UserCreateRequest req = new()
            {
                NodeId = AssertUtil.AssertPositiveStrict(nodeId, "nodeId").ToString(),
                DeviceNumbers = AssertUtil.NotNull(deviceNumbers, "deviceNumber"),
                All = false
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            Users users = await GetResult<Users>(response);
            if (users == null)
            {
                return null;
            }
            else
            {
                return users.users;
            }
        }

        public async Task<List<User>> CreateAllUsersAsync(int nodeId)
        {
            UserCreateRequest req = new()
            {
                NodeId = AssertUtil.AssertPositiveStrict(nodeId, "nodeId").ToString(),
                DeviceNumbers = null,
                All = true
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            Users users = await GetResult<Users>(response);
            if (users == null)
            {
                return null;
            }
            else
            {
                return users.users;
            }
        }

        public async Task<bool> DeleteUserAsync(string loginName)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri.Append(AssertUtil.NotNullOrEmpty(loginName, "loginName")));
            return await IsSucceeded(response);
        }
    }
}
