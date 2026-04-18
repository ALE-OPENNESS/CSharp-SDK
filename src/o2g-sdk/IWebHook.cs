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
    /// Represents a Webhook configuration used for receiving O2G events.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When eventing is configured via a Webhook, the application must expose an HTTP
    /// endpoint and provide its URL to the SDK through this interface.
    /// </para>
    /// <para>
    /// Once the subscription is successfully established, the SDK invokes
    /// <see cref="ConnectProcessor"/> to supply an <see cref="IEventProcessor"/> instance.
    /// The application is then responsible for forwarding incoming HTTP POST request
    /// bodies to this processor.
    /// </para>
    /// </remarks>
    /// <seealso cref="IEventProcessor"/>
    /// <seealso cref="Subscription.IBuilder.SetWebHook"/>
    public interface IWebHook
    {
        /// <summary>
        /// Gets the URL of the Webhook endpoint exposed by the application.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> containing the Webhook endpoint URL that the O2G server
        /// will use to POST events.
        /// </value>
        string Url { get; }

        /// <summary>
        /// Called by the SDK when the event subscription is established.
        /// </summary>
        /// <param name="processor">
        /// The <see cref="IEventProcessor"/> that the application must call for each
        /// HTTP POST body received at its Webhook endpoint.
        /// </param>
        /// <remarks>
        /// The application should store the provided <paramref name="processor"/> and invoke
        /// <see cref="IEventProcessor.Process"/> inside its HTTP endpoint handler for every
        /// incoming event POST.
        /// </remarks>
        void ConnectProcessor(IEventProcessor processor);
    }
}
