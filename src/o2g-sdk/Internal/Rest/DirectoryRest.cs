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
using o2g.Types.DirectoryNS;
using o2g.Utility;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{

    internal class SearchRequest
    {
        public int? Limit { get; set; }
        public Criteria Filter { get; set; }
    }


    internal class DirectoryRest : AbstractRESTService, IDirectory
    {
        public DirectoryRest(Uri uri) : base(uri)
        {

        }

        public async Task<bool> CancelAsync(string loginName)
        {
            Uri uriDelete = uri.Append("search");
            if (loginName != null)
            {
                uriDelete = uriDelete.AppendQuery("loginName", loginName);
            }
            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<SearchResult> GetResultsAsync(string loginName)
        {
            Uri uriGet = uri.Append("search");
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<SearchResult>(response);
        }

        public async Task<bool> SearchAsync(Criteria filter, int? limit, string loginName)
        {
            Uri uriPost = uri.Append("search");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            SearchRequest req = new()
            {
                Limit = limit,
                Filter = filter
            };

            var json = JsonSerializer.Serialize<object>(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }
    }
}
