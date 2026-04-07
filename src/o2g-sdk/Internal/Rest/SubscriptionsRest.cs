using o2g.Internal.Events;
using o2g.Internal.Services;
using o2g.Internal.Types;
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
    internal class SubscriptionsRest : AbstractRESTService, ISubscriptions
    {
        public SubscriptionsRest(Uri uri) : base(uri)
        {

        }

        async Task<SubscriptionResult> ISubscriptions.Create(Subscription request)
        {
            var json = JsonSerializer.Serialize(request, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            return await GetResult<SubscriptionResult>(response);
        }

        async Task<bool> ISubscriptions.Delete(string subscriptionId)
        {
            Uri uriDelete = uri.Append(subscriptionId);

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

    }
}
