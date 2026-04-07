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

using o2g.Events.Users;

namespace o2g.Events
{
    /// <summary>
    /// <c>EventPackage</c> class represents a set of event the application can subscribe on.
    /// </summary>
    public class EventPackage
    {
        private EventPackage(string value) { Value = value; }

        /// <summary>
        /// Return the value for this event package
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the event package.
        /// </value>
        public string Value { get; private set; }

        /// <summary>
        /// The event package value to subscribe to telephony events.
        /// </summary>
        /// <seealso cref="ITelephony"/>
        public static EventPackage Telephony { get { return new EventPackage("telephony"); } }

        /// <summary>
        /// The event package value to subscribe to event summary events.
        /// </summary>
        /// <seealso cref="IEventSummary"/>
        public static EventPackage EventSummary { get { return new EventPackage("eventSummary"); } }

        /// <summary>
        /// The event package value to subscribe to communictaion log events.
        /// </summary>
        /// <seealso cref="ICommunicationLog"/>
        public static EventPackage CommunicationLog { get { return new EventPackage("unifiedComLog"); } }

        /// <summary>
        /// The event package value to subscribe to <see cref="OnUserInfoChangedEvent"/> events.
        /// </summary>
        /// <seealso cref="IUsers"/>
        public static EventPackage User { get { return new EventPackage("user"); } }

        /// <summary>
        /// The event package value to subscribe to users events.
        /// </summary>
        /// <seealso cref="IUsers"/>
        public static EventPackage Users { get { return new EventPackage("userManagement"); } }

        /// <summary>
        /// The event package value to subscribe to routing events.
        /// </summary>
        /// <seealso cref="IRouting"/>
        public static EventPackage Routing { get { return new EventPackage("routingManagement"); } }

        /// <summary>
        /// The event package value to subscribe to call center agent events.
        /// </summary>
        public static EventPackage Agent { get { return new EventPackage("agent"); } }

        /// <summary>
        /// The event package value to subscribe to call center pilot events.
        /// </summary>
        public static EventPackage Pilot { get { return new EventPackage("pilot"); } }

        /*
        /// <summary>
        /// The Event package value to subscribe to call center rsi events.
        /// </summary>
        public static EventPackage Rsi { get { return new EventPackage("rsi"); } }
        */

        /// <summary>
        /// The event package value to subscribe to system events.
        /// </summary>
        /// <seealso cref="ICommunicationLog"/>
        public static EventPackage System { get { return new EventPackage("system"); } }

        /// <summary>
        /// The event package value to subscribe to pbx management events.
        /// </summary>
        /// <seealso cref="IPbxManagement"/>
        public static EventPackage PbxManagement { get { return new EventPackage("pbxManagement"); } }
    }
}
