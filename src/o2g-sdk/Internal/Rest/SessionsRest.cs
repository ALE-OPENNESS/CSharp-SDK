using o2g.Internal.Events;
using o2g.Internal.Services;
using o2g.Internal.Utility;
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
    internal class SessionsRest : AbstractRESTService, ISessions
    {
        public SessionsRest(Uri uri) : base(uri)
        {

        }

        async Task<bool> ISessions.Close()
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);
            return await IsSucceeded(response);
        }

        async Task<SessionInfo> ISessions.Get()
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return await GetResult<SessionInfo>(response);
        }

        async Task<SessionInfo> ISessions.Open(string applicationName)
        {
            var content = new StringContent(String.Format("{{\"applicationName\":\"{0}\"}}", applicationName), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            return await GetResult<SessionInfo>(response);
        }

        async Task<bool> ISessions.SendKeepAlive()
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri.Append("keepalive"), null);
            return await IsSucceeded(response);
        }
    }
}
