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
    /// The <c>EventSummary</c> service allows a user to get its new message indicators (missed call, voice mails, callback request, fax). 
    /// IEventSummary service is available from a connected application, it requires <b>TELEPHONY_ADVANCED</b> license.
    /// </summary>
    /// <example>
    /// <code>
    /// // Get the EventSummary service from a connected application
    /// IEventSummary eventSummaryService = myApp.EventSummaryService.
    /// </code>
    /// </example>
    /// <seealso cref="O2G.Application.LoginAsync(string, string)"/>
    public interface IEventSummary : IService
    {
        /// <summary>
        /// <c>EventSummaryUpdated</c> event is raised each time the user's counters have changed.
        /// </summary>
        public event EventHandler<O2GEventArgs<OnEventSummaryUpdatedEvent>> EventSummaryUpdated;


        /// <summary>
        /// Retrieve the <c>EventSummary</c> that gives the user's event counters.
        /// </summary>
        /// <param name="loginName">Login name of the user for whom the request is invoked. This parameter is mandatory for an administrator session.</param>
        /// <returns>The <see cref="EventSummary"/> object that gives the user's event counters on success, or <c>null</c> in case of error.</returns>
        Task<EventSummary> GetAsync(string loginName = null);
    }
}
