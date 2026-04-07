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

namespace o2g.Types.CallCenterRsiNS
{
    /// <summary>
    /// <c>Tones</c> represents tones that can be generated.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: Tones.Unknown)]
    public enum Tones
    {
        /// <summary>
        /// The busy tone.
        /// </summary>
        [EnumMember(Value = "BUSY")]
        Busy,

        /// <summary>
        /// The ring back tone.
        /// </summary>
        [EnumMember(Value = "RINGBACK")]
        Ringback,

        /// <summary>
        /// The silent tone.
        /// </summary>
        [EnumMember(Value = "SILENCE")]
        Silence,

        /// <summary>
        /// The music tone.
        /// </summary>
        [EnumMember(Value = "MUSIC")]
        Music,

        /// <summary>
        /// The annoucement tone.
        /// </summary>
        [EnumMember(Value = "ANNOUNCEMENT")]
        Announcement,

        /// <summary>
        /// Unknown tone.
        /// </summary>
        Unknown
    }
}
