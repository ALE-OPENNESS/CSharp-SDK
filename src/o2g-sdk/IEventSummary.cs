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
using o2g.Events;
using o2g.Events.EventSummary;
using o2g.Internal.Services;
using o2g.Types.EventSummaryNS;
using System;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// The <c>IEventSummary</c> service allows a user to retrieve new message indicators such as missed calls, voice mails, callback requests, and faxes.
    /// Using this service requires a <b>TELEPHONY_ADVANCED</b> license.
    /// </summary>
    public interface IEventSummary : IService
    {
        /// <summary>
        /// Occurs each time the user's event counters have changed.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnEventSummaryUpdatedEvent>> EventSummaryUpdated;

        /// <summary>
        /// Retrieves the main event counters for the specified user.
        /// </summary>
        /// <param name="loginName">The login name of the user for whom the request is invoked.</param>
        /// <returns>
        /// The <see cref="EventSummary"/> object containing the event counters on success, or <see langword="null"/> in case of error.
        /// </returns>
        /// <remarks>
        /// If the session has been opened for a user, the <c>loginName</c> parameter is ignored, but it is mandatory if the session has been opened by an administrator.
        /// </remarks>
        Task<EventSummary> GetAsync(string loginName = null);
    }
}
