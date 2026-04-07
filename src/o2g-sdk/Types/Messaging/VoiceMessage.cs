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
using o2g.Types.CommonNS;
using System;

namespace o2g.Types.MessagingNS
{
    /// <summary>
    /// <c>VoiceMessage</c> represents a message stored in a voice Mail box.
    /// </summary>
    /// <see cref="MailBoxInfo"></see>
    public class VoiceMessage
    {
        /// <summary>
        /// This propery gives the unique identifier of this message.
        /// </summary>
        /// <value>
        /// A string value that identify the message.
        /// </value>
        public string VoicemailId { get; init; }

        /// <summary>
        /// This property is <see cref="PartyInfo"/> that deposited this voice mail.
        /// </summary>
        /// <value>
        /// A <see cref="PartyInfo"/> object.
        /// </value>
        public PartyInfo From { get; init; }

        /// <summary>
        /// This property gives the duration of this message.
        /// </summary>
        /// <value>
        /// The message duration in seconds.
        /// </value>
        public int Duration { get; init; }

        /// <summary>
        /// This property gives the date and time the message has been deposited.
        /// </summary>
        /// <value>
        /// A <see cref="Date"/> object represnting the time the message has been deposited.
        /// </value>
        public DateTime Date { get; init; }

        /// <summary>
        /// This property indicates whether the message has been read.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the message is unread; <see langword="false"/> otherwise.
        /// </value>
        public bool Unread { get; init; }

        /// <summary>
        /// This property indicates whether the message has been tagged as urgent.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the message is urgent; <see langword="false"/> otherwise.
        /// </value>
        public bool HighPriority { get; init; }
    }
}
