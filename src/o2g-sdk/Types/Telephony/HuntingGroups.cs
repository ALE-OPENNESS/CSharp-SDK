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
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// <c>HuntingGroups</c> gives the hunting group information for a user.
    /// <para>A user can be member of only one hunting group.</para>
    /// </summary>
    public class HuntingGroups
    {
        /// <summary>
        /// The list of existing hunting groups the user can join.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> representing the phone number of each 
        /// hunting group available on the OXE node the user is configured on.
        /// </value>
        [JsonPropertyName("hgList")]
        public IReadOnlyList<string> List { get; init; }

        /// <summary>
        /// The hunting group the user is currently a member of.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the phone number of the current hunting group,
        /// or <see langword="null"/> if the user is not a member of any hunting group.
        /// </value>
        [JsonPropertyName("currentHg")]
        public string Current { get; init; }
    }
}
