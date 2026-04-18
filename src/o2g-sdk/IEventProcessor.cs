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

namespace o2g
{
    /// <summary>
    /// Processes raw event JSON received at the webhook endpoint.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The SDK provides an implementation of this interface to the application via
    /// <see cref="IWebHook.ConnectProcessor"/>. The application must call <see cref="Process"/>
    /// for each HTTP POST body received at its webhook endpoint.
    /// </para>
    /// </remarks>
    /// <seealso cref="IWebHook"/>
    public interface IEventProcessor
    {
        /// <summary>
        /// Processes a raw JSON event string received from the O2G server.
        /// </summary>
        /// <param name="rawEvent">The raw JSON string from the HTTP POST body.</param>
        /// <remarks>
        /// Call this method inside your HTTP endpoint handler, passing the full request body.
        /// The SDK will parse the event, identify its type, and dispatch it to the registered
        /// event handlers on <see cref="O2G.Application"/>.
        /// </remarks>
        void Process(string rawEvent);
    }
}
