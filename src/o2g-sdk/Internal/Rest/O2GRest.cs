using o2g.Internal.Events;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{

    internal class O2GRest : AbstractRESTService, IO2G
    {
        public O2GRest(Uri uri) : base(uri)
        {

        }
        public async Task<RoxeRestApiDescriptor> Get()
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            string jsonCode = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RoxeRestApiDescriptor>(jsonCode, serializeOptions);
        }
    }
}
