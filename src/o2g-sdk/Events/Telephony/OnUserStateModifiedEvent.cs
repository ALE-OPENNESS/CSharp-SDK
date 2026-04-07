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
using o2g.Types.TelephonyNS.UserNS;
using System;

namespace o2g.Events.Telephony
{
    /// <summary>
    /// <c>OnUserStateModifiedEvent</c> is raised when a user's state has been modified (FREE, BUSY ...).
    /// </summary>
    public class OnUserStateModifiedEvent : O2GEvent
    {
        /// <summary>
        /// Return the login name of the user for which the event is raised.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the user login name.
        /// </value>
        public String LoginName;

        /// <summary>
        /// Return the new user's state.
        /// </summary>
        /// <value>
        /// A <see cref="UserState"/> object that represents the new user's state.
        /// </value>
        public UserState State;
    }
}
