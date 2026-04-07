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

namespace o2g.Types.MessagingNS
{
    /// <summary>
    /// <c>MailBox</c> class represents a mail box in a voice mail system. A voice mail is assigned to a user.
    /// </summary>
    public class MailBox
    {
        /// <summary>
        /// <c>MailBoxCapabilities</c> represents the capabilities of the voice mail.
        /// </summary>
        public class MailBoxCapabilities
        {
            /// <summary>
            /// Return whether the voicemail server can return the list of messages.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voicemail server can return the list of messages; <see langword="false"/> otherwise.
            /// </value>
            public bool ListMessages { get; init; }

            /// <summary>
            /// Return wheather the voice messages can be downloaded.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voice messages can be downloaded; <see langword="false"/> otherwise.
            /// </value>
            public bool GetMessages { get; init; }

            /// <summary>
            /// Return whether the recorded message can be downloaded.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the recorded message can be downloaded; <see langword="false"/> otherwise.
            /// </value>
            public bool GetRecord { get; init; }

            /// <summary>
            /// Return whether the voicemail server can play voice messages.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voicemail server can play voice messages; <see langword="false"/> otherwise.
            /// </value>
            public bool Play { get; init; }

            /// <summary>
            /// Return whether a played voice message can be paused and resumes from the position it has been paused.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if a playing voice message can be paused; <see langword="false"/> otherwise.
            /// </value>
            public bool Pause { get; init; }

            /// <summary>
            /// Return whether the media session can be terminate.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the media session can be terminate; <see langword="false"/> otherwise.
            /// </value>
            public bool Hangup { get; init; }

            /// <summary>
            /// Return whether the voicemail server can record voice messages.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voicemail server can record voice messages; <see langword="false"/> otherwise.
            /// </value>
            public bool Record { get; init; }

            /// <summary>
            /// Return wheter a voice message can be resumed to the position it has been paused.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voice message can be resumed; <see langword="false"/> otherwise.
            /// </value>
            public bool Resume { get; init; }

            /// <summary>
            /// Return whether the current recording can be cancelled.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the current recording can be cancelled; <see langword="false"/> otherwise.
            /// </value>
            public bool Cancel { get; init; }

            /// <summary>
            /// Return wheter the voicemail server can forward voice messages.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voicemail server can forward voice messages; <see langword="false"/> otherwise.
            /// </value>
            public bool Forward { get; init; }

            /// <summary>
            /// Return whether the voicemail server can call back the originator of the voice message.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voicemail server can call back the originator of the voice message; <see langword="false"/> otherwise.
            /// </value>
            public bool Callback { get; init; }

            /// <summary>
            /// Return whether a voice message or a record can be sent to recipients.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if a voice message or a record can be sent to recipients; <see langword="false"/> otherwise.
            /// </value>
            public bool Send { get; init; }

            /// <summary>
            /// Return whether the voicemail server can send events in case of message deposit / removal.
            /// </summary>
            /// <value>
            /// <see langword="true"/> if the voicemail server can send events in case of message deposit / removal; <see langword="false"/> otherwise.
            /// </value>
            public bool Events { get; init; }
        }

        /// <summary>
        /// Return this mail box identifier.
        /// </summary>
        /// <value>
        /// A unique <see langword="string"/> identifying the mail box.
        /// </value>
        public string Id { get; init; }

        /// <summary>
        /// Return this mail box name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents mail box name
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Return this mail box capabilities.
        /// </summary>
        /// <value>
        /// A <see cref="MailBoxCapabilities"/> object that gives this mail box capabilities.
        /// </value>
        public MailBoxCapabilities Capabilities { get; init; }
    }
}
