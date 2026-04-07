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

namespace o2g.Types.EventSummaryNS
{
    /// <summary>
    /// <c>EventSummary</c> represents event counters associated to the user. It allows a user to get its new message 
    /// indicators (missed call, voice mails, callback request, fax).
    /// </summary>
    public class EventSummary
    {
        /// <summary>
        /// Return the number of missed calls.
        /// </summary>
        /// <value>
        /// A optional <see langword="int"/> value that gives the number of missed calls.
        /// </value>
        /// <remarks>
        /// <para>
        /// If this attribute is not specified, it means that the server is not able to provide that information.
        /// </para>
        /// <para>
        /// <b>CAUTION</b>: This attribute doesn't reflect the missed call number managed by the call server itself but is related to the unanswered and non acknowledged
        /// incoming calls in the history call. Therefore, either only the explicit acknowledgment of these history com records through the communication log API
        /// service, or a new answered call with the same user will decrease this counter. Moreover, the counter is incremented for each non answered incoming call,
        /// including successive attempts from the same caller.
        /// </para>
        /// </remarks>
        public int? MissedCallsNb { get; set; }

        /// <summary>
        /// Return the number of new voice messages.
        /// </summary>
        /// <value>
        /// A optional <see langword="int"/> value that gives the number of new voice messages.
        /// </value>
        /// <remarks>
        /// If this attribute is not specified, it means that the server is not able to provide that information.
        /// </remarks>
        public int? VoiceMessagesNb { get; set; }

        /// <summary>
        /// Return the number of new callback requests.
        /// </summary>
        /// <value>
        /// A optional <see langword="int"/> value that gives the number of new callback requests.
        /// </value>
        /// <remarks>
        /// If this attribute is not specified, it means that the server is not able to provide that information.
        /// </remarks>
        public int? CallBackRequestsNb { get; set; }

        /// <summary>
        /// Return the number of new faxes.
        /// </summary>
        /// <value>
        /// A optional <see langword="int"/> value that gives the number of new faxes.
        /// </value>
        /// <remarks>
        /// If this attribute is not specified, it means that the server is not able to provide that information.
        /// </remarks>
        public int? FaxNb { get; set; }

        /// <summary>
        /// Return the number of new text messages.
        /// </summary>
        /// <value>
        /// A optional <see langword="int"/> value that gives the number of new text messages.
        /// </value>
        /// <remarks>
        /// If this attribute is not specified, it means that the server is not able to provide that information.
        /// </remarks>
        public int? NewTextNb { get; set; }

        /// <summary>
        /// Return the number of old text messages.
        /// </summary>
        /// <value>
        /// A optional <see langword="int"/> value that gives the number of old text messages.
        /// </value>
        /// <remarks>
        /// If this attribute is not specified, it means that the server is not able to provide that information.
        /// </remarks>
        public int? OldTextNb { get; set; }

        /// <summary>
        /// Return whether an event is waiting.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if an event is waiting; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// <para>
        /// This flags can be used to notify the application that there are new events waiting. 
        /// </para>
        /// </remarks>
        public bool EventWaiting { get; set; }
    }
}
