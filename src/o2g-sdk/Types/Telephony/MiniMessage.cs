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
using System;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// <c>MiniMessage</c> class represents a mini message exchanged between two users.
    /// </summary>
    public class MiniMessage
    {
        /// <summary>
        /// This property represents the message sender.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the phone number of the user who has sent the message.
        /// </value>
        public string Sender { get; set; }

        /// <summary>
        /// This property gives the date the mini message has been sent.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that gives the date the message has been sent.
        /// </value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// This property gives the text of the mini message
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the mini message text.
        /// </value>
        public string Message { get; set; }
    }
}
