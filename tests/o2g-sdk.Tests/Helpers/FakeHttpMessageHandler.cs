/*
* Copyright 2026 ALE International
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

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace o2g.Tests.Helpers
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses = new();
        private readonly List<HttpRequestMessage> _requests = new();

        /// <summary>
        /// All requests received by this handler, in order.
        /// </summary>
        public IReadOnlyList<HttpRequestMessage> Requests => _requests;

        /// <summary>
        /// The most recent request received.
        /// </summary>
        public HttpRequestMessage LastRequest => _requests.Count > 0 ? _requests[^1] : null!;

        /// <summary>
        /// Creates a handler with a single response returned for every call.
        /// </summary>
        public FakeHttpMessageHandler(string jsonBody,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _responses.Enqueue(BuildResponse(jsonBody, statusCode));
        }

        /// <summary>
        /// Creates a handler with a queue of responses returned in order.
        /// </summary>
        public FakeHttpMessageHandler(params (string jsonBody, HttpStatusCode statusCode)[] responses)
        {
            foreach (var (jsonBody, statusCode) in responses)
            {
                _responses.Enqueue(BuildResponse(jsonBody, statusCode));
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _requests.Add(request);

            var response = _responses.Count > 0
                ? _responses.Dequeue()
                : BuildResponse("", HttpStatusCode.OK);

            return Task.FromResult(response);
        }

        private static HttpResponseMessage BuildResponse(string jsonBody, HttpStatusCode statusCode)
            => new(statusCode)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };
    }
}
