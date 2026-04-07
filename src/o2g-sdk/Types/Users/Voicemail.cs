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

namespace o2g.Types.UsersNS
{
    /// <summary>
    /// <c>Voicemail</c> represents a voice mail box associated to a user.
    /// </summary>
    public class Voicemail
    {
        /// <summary>
        /// The voice mail type
        /// </summary>
        [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: VoiceMailType.External)]
        public enum VoiceMailType
        {
            /// <summary>
            /// The voice mail is a 4635.
            /// </summary>
            VM_4635, 

            /// <summary>
            /// The voice mail is a 4645.
            /// </summary>
            VM_4645,

            /// <summary>
            /// An external (third party) voice mail type.
            /// </summary>
            [EnumMember(Value = "EXTERNAL")]
            External
        }

        /// <summary>
        /// This property gives the voice mail phone number.
        /// </summary>
        /// <value>
        /// The <see langword="string"/> that represents this voice mail phone number.
        /// </value>
        public string Number { get; init; }

        /// <summary>
        /// This property gives the voice mail type.
        /// </summary>
        /// <value>
        /// An enum value that indicated the type of voice mail.
        /// </value>
        public VoiceMailType Type { get; init; }
    }
}
