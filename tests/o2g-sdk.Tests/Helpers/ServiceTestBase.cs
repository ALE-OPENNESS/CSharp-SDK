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
using o2g.Internal.Events;
using o2g.Internal.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace o2g.Tests.Helpers
{
    public abstract class ServiceTestBase : IDisposable
    {
        protected static readonly Uri BaseUri = new("https://fake-o2g/api/");

        private FakeHttpMessageHandler? _handler;

        internal EventHandlers EventHandlers { get; private set; }

        protected FakeHttpMessageHandler SetupHttpClient(string jsonResponse,
            HttpStatusCode status = HttpStatusCode.OK)
        {
            DependancyResolver.Reset();
            _handler = new FakeHttpMessageHandler(jsonResponse, status);
            DependancyResolver.RegisterService(new HttpClient(_handler));
            EventHandlers = new EventHandlers();
            DependancyResolver.RegisterService(EventHandlers);
            return _handler;
        }

        protected FakeHttpMessageHandler SetupHttpClient(
            params (string jsonBody, HttpStatusCode statusCode)[] responses)
        {
            DependancyResolver.Reset();
            _handler = new FakeHttpMessageHandler(responses);
            DependancyResolver.RegisterService(new HttpClient(_handler));
            EventHandlers = new EventHandlers();
            DependancyResolver.RegisterService(EventHandlers);
            return _handler;
        }

        /// <summary>
        /// Returns a fluent assert for the last request received.
        /// </summary>
        protected RequestAssert AssertRequest()
        {
            _handler.Should().NotBeNull("SetupHttpClient must be called before AssertRequest");
            _handler!.LastRequest.Should().NotBeNull("no request was made");
            return new RequestAssert(_handler.LastRequest);
        }

        /// <summary>
        /// Returns a fluent assert for the request at the given index.
        /// </summary>
        protected RequestAssert AssertRequest(int index)
        {
            _handler.Should().NotBeNull("SetupHttpClient must be called before AssertRequest");
            _handler!.Requests.Should().HaveCountGreaterThan(index,
                $"expected at least {index + 1} requests");
            return new RequestAssert(_handler.Requests[index]);
        }

        /// <summary>
        /// Simplified assertion for method, URI and exact JSON body — equivalent to
        /// assertCalledWith() in the Java tests.
        /// </summary>
        protected async Task AssertCalledWith(HttpMethod method, string uri, string? expectedBody = null)
        {
            var req = AssertRequest();
            req.Method(method).Uri(uri);
            if (expectedBody != null)
                await req.Body(expectedBody);
            else
                req.NoBody();
        }

        public void Dispose()
        {
            DependancyResolver.Reset();
        }
    }
}

