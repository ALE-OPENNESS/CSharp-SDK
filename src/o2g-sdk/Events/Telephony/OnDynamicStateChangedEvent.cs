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
using o2g.Types.TelephonyNS;

namespace o2g.Events.Telephony
{
    /// <summary>
    /// <c>OnDynamicStateChangedEvent</c> is raised when the user's dynamic state change. (hunting group logon state)
    /// </summary>
    public class OnDynamicStateChangedEvent : O2GEvent
    {
        /// <summary>
        /// Return the login name of the user for which the event is raised.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the user login name.
        /// </value>
        public string LoginName { get; init; }

        /// <summary>
        /// Return the new hunting group status
        /// </summary>
        /// <value>
        /// A <see cref="HuntingGroupStatus"/> that represents the new hunting group status.
        /// </value>
        public HuntingGroupStatus HuntingGroupState { get; init; }
    }
}
