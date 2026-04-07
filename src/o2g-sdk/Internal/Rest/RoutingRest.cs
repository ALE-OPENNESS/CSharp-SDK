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
using o2g.Events.Routing;
using o2g.Internal.Events;
using o2g.Internal.Types.Routing;
using o2g.Internal.Utility;
using o2g.Types;
using o2g.Types.RoutingNS;
using o2g.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{

    class O2GObjSetForwardRouteRequest
    {
        public ForwardRoute ForwardRoute { get; set; }
    }

    class O2GObjSetOverflowRouteRequest
    {
        public List<OverflowRoute> OverflowRoutes { get; set; }
    }

    class O2GObjSetRouteRequest
    {
        public List<PresentationRoute> PresentationRoutes { get; set; }
        public List<ForwardRoute> ForwardRoutes { get; set; }
    }


    internal class RoutingRest : AbstractRESTService, IRouting
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnRoutingStateChangedEvent>> RoutingStateChanged
        {
            add => _eventHandlers.RoutingStateChanged += value;
            remove => _eventHandlers.RoutingStateChanged -= value;
        }

        public RoutingRest(Uri uri) : base(uri)
        {

        }

        public async Task<bool> ActivateDndAsync(string loginName)
        {
            Uri uriPost = uri.Append("dnd");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> CancelDndAsync(string loginName)
        {
            Uri uriDelete = uri.Append("dnd");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> CancelForwardAsync(string loginName)
        {
            Uri uriDelete = uri.Append("forwardroute");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> ForwardOnNumberAsync(string number, Forward.ForwardCondition condition, string loginName)
        {
            Uri uriPost = uri.Append("forwardroute");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            O2GObjSetForwardRouteRequest req = new()
            {
                ForwardRoute = ForwardRoute.CreateForwardOnNumber(number, condition)
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> ForwardOnVoiceMailAsync(Forward.ForwardCondition condition, string loginName)
        {
            Uri uriPost = uri.Append("forwardroute");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery(loginName);
            }

            O2GObjSetForwardRouteRequest req = new()
            {
                ForwardRoute = ForwardRoute.CreateForwardOnVoiceMail(condition)
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }


        public async Task<DndState> GetDndStateAsync(string loginName)
        {
            Uri uriGet = uri.Append("dnd");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<DndState>(response);
        }


        public async Task<Forward> GetForwardAsync(string loginName)
        {
            Uri uriGet = uri.Append("forwardroute");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);

            string jsonCode = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                SetLastError(null);
                if (jsonCode.Length == 0)
                {
                    return new Forward()
                    {
                        Destination = Destination.None,
                        Condition = null,
                        Number = null
                    };
                }
                else
                {
                    ForwardRoute objForwardRoute = JsonSerializer.Deserialize<ForwardRoute>(jsonCode, serializeOptions);
                    return objForwardRoute.ToForward();
                }
            }
            else
            {
                SetLastError(JsonSerializer.Deserialize<RestErrorInfo>(jsonCode, serializeOptions));
                return null;
            }
        }

        public async Task<RoutingCapabilities> GetCapabilitiesAsync(string loginName)
        {
            Uri uriGet = uri;
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<RoutingCapabilities>(response);
        }

        private async Task<bool> SetRemoteExtensionActivationAsync(bool active, string loginName)
        {
            Uri uriPost = uri;
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            O2GObjSetRouteRequest req = new()
            {
                PresentationRoutes = new(),
                ForwardRoutes = null
            };
            req.PresentationRoutes.Add(PresentationRoute.CreateMobileRouteActivation(active));

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> ActivateRemoteExtensionAsync(string loginName)
        {
            return await this.SetRemoteExtensionActivationAsync(true, loginName);
        }
        public async Task<bool> DeactivateRemoteExtensionAsync(string loginName)
        {
            return await this.SetRemoteExtensionActivationAsync(false, loginName);
        }

        public async Task<bool> CancelOverflowAsync(string loginName)
        {
            Uri uriDelete = uri.Append("overflowroute");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<Overflow> GetOverflowAsync(string loginName)
        {
            Uri uriGet = uri.Append("overflowroute");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            string jsonCode = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                SetLastError(null);
                if (jsonCode.Length == 0)
                {
                    return new Overflow()
                    {
                        Destination = Destination.None,
                        Condition = null
                    };
                }
                else
                {
                    OverflowRoute objForwardRoute = JsonSerializer.Deserialize<OverflowRoute>(jsonCode, serializeOptions);
                    return objForwardRoute.ToOverflow();
                }
            }
            else
            {
                SetLastError(JsonSerializer.Deserialize<RestErrorInfo>(jsonCode, serializeOptions));
                return null;
            }
        }

        public async Task<bool> OverflowOnVoiceMailAsync(Overflow.OverflowCondition condition, string loginName)
        {
            Uri uriPost = uri.Append("overflowroute");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            O2GObjSetOverflowRouteRequest req = new()
            {
                OverflowRoutes = new()
            };
            req.OverflowRoutes.Add(OverflowRoute.CreateOverflowOnVoiceMail(condition));

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> OverflowOnAssociateAsync(Overflow.OverflowCondition condition, string loginName)
        {
            Uri uriPost = uri.Append("overflowroute");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            O2GObjSetOverflowRouteRequest req = new()
            {
                OverflowRoutes = new()
            };
            req.OverflowRoutes.Add(OverflowRoute.CreateOverflowOnAssociate(condition));

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<RoutingState> GetRoutingStateAsync(string loginName = null)
        {
            Uri uriGet = uri.Append("state");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GRoutingState o2gObjRoutingState = await GetResult<O2GRoutingState>(response);
            if (o2gObjRoutingState == null)
            {
                return null;
            }
            else
            {
                return o2gObjRoutingState.ToRoutingState();
            }
        }

        public async Task<bool> RequestSnapshotAsync(string loginName = null)
        {
            Uri uriPost = uri.Append("state/snapshot");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }
    }
}
