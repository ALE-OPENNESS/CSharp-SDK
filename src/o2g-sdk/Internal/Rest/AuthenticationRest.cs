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

using o2g.Internal.Services;
using o2g.Types;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    internal class AuthenticationRest : AbstractRESTService, IAuthentication
    {
        public AuthenticationRest(Uri uri) : base(uri)
        {
        }

        async Task<O2GAuthenticateResult> IAuthentication.Authenticate(Credential credential)
        {
            string challenge = credential.BuildAuthenticationChallenge();
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", challenge);

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            // Remove the authorization header
            httpClient.DefaultRequestHeaders.Authorization = null;

            if (response.IsSuccessStatusCode)
            {
                string jsonCode = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<O2GAuthenticateResult>(jsonCode, serializeOptions);
            }
            else
            {
                throw new O2GAuthenticationException();
            }
        }
    }
}
