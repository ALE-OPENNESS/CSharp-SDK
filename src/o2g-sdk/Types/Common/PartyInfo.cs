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

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace o2g.Types.CommonNS
{
    /// <summary>
    /// Represents a party involved in a call.
    /// </summary>
    public class PartyInfo
    {
        /// <summary>
        /// Represents a type of participant to a call. A participant is identified by its <see cref="MainType"/> type, and by an optional <c>SubType</c>.
        /// </summary>
        public class ParticipantType
        {
            /// <summary>
            /// <c>MainType</c> Represents the main type a participant can be.
            /// </summary>
            [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: MainType.Unknown)]
            public enum MainType
            {
                /// <summary>
                /// The participant is a user of the system.
                /// </summary>
                [EnumMember(Value = "USER")]
                User,
                /// <summary>
                /// The participant is a device of the system.
                /// </summary>
                [EnumMember(Value = "DEVICE")]
                Device,
                /// <summary>
                /// The participant is a service of the system. For exemple the voice mail.
                /// </summary>
                [EnumMember(Value = "SERVICE")]
                Service,
                /// <summary>
                /// The participant is not a user of the system.
                /// </summary>
                [EnumMember(Value = "EXTERNAL")]
                External,
                /// <summary>
                /// The participant type has not been identified.
                /// </summary>
                [EnumMember(Value = "UNKNOWN")]
                Unknown
            }

            /// <summary>
            /// The main type of the participant.
            /// </summary>
            /// <value>
            /// A <see cref="MainType"/> enum that is this participant main type.
            /// </value>
            public MainType Main { get; set; }

            /// <summary>
            /// The SubType gives a supplementary information.
            /// </summary>
            /// <value>
            /// A <see langword="string"/> that gives this participant sub-type. It can be "pbx", "public", "pre-off-hook", telephony-services", "voicemail", "voice-homepage", "voice-it", "sip".
            /// </value>
            public string SubType { get; set; }
        }

        /// <summary>
        /// Represent the information used to uniquely identify a participant.
        /// </summary>
        public class Identifier
        {
            /// <summary>
            /// The participant login name.
            /// </summary>
            /// <value>
            /// A <see langword="string"/> that is this the login name of this participant.
            /// </value>
            public string LoginName { get; set; }

            /// <summary>
            /// The participant phone number.
            /// </summary>
            /// <value>
            /// A <see langword="string"/> that is the main phone number of this participant.
            /// </value>
            public string PhoneNumber { get; set; }
        }


        /// <summary>
        /// This property is the identifier of this party.
        /// </summary>
        /// <value>
        /// A <see cref="Identifier"/> that gives the unique identity of this participant.
        /// </value>
        public Identifier Id { get; set; }

        /// <summary>
        /// This property gives the party first name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the party first name.
        /// </value>
        public string FirstName { get; set; }


        /// <summary>
        /// This property gives the party last name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the party last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// The party display name. 
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the party display name. If <see cref="FirstName"/> and <see cref="LastName"/> are filled, the <c>DisplayName</c> is <see langword="null"/>.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// The party type.
        /// </summary>
        /// <value>
        /// A <see cref="ParticipantType"/> enum value thats is this party participant type.
        /// </value>
        public ParticipantType Type { get; set; }
    }
}
