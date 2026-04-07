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

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// <c>HuntingGroups</c> gives the hunting group information for a user.
    /// <para>A user can be member of only one hunting group.</para>
    /// </summary>
    public class HuntingGroups
    {
        /// <summary>
        /// Give the list of existing hunting groups.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the phone number of each existing hunting group on the OXE node the user is configured.
        /// </value>
        public List<string> HgList { get; init; }

        /// <summary>
        /// This property gives the hunting group which the user is a member. 
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the phone number of the hunting group which the user is a member, or <see langword="null"/> if the user is not member of any hunting group.
        /// </value>
        public string CurrentHg { get; init; }
    }
}
