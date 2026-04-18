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



using FluentAssertions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace o2g.Tests.Helpers
{
    public class RequestAssert
    {
        private readonly HttpRequestMessage _request;

        internal RequestAssert(HttpRequestMessage request)
        {
            _request = request;
        }

        /// <summary>
        /// Asserts the HTTP method.
        /// </summary>
        public RequestAssert Method(HttpMethod method)
        {
            _request.Method.Should().Be(method);
            return this;
        }

        /// <summary>
        /// Asserts the request URI contains the expected path.
        /// </summary>
        public RequestAssert Uri(string expectedUri)
        {
            _request.RequestUri!.PathAndQuery.Should().Be(expectedUri);
            return this;
        }

        /// <summary>
        /// Asserts the JSON body using path-based assertions.
        /// </summary>
        public async Task<RequestAssert> JsonBody(Action<JsonBodyAssert> assertions)
        {
            _request.Content.Should().NotBeNull("request should have a body");
            var body = await _request.Content!.ReadAsStringAsync();
            var jsonAssert = new JsonBodyAssert(body);
            assertions(jsonAssert);
            return this;
        }

        /// <summary>
        /// Asserts the raw body equals the expected string exactly.
        /// </summary>
        public async Task<RequestAssert> Body(string expected)
        {
            _request.Content.Should().NotBeNull("request should have a body");
            var body = await _request.Content!.ReadAsStringAsync();
            body.Should().Be(expected);
            return this;
        }

        /// <summary>
        /// Asserts the request has no body.
        /// </summary>
        public RequestAssert NoBody()
        {
            _request.Content.Should().BeNull("request should have no body");
            return this;
        }
    }
}