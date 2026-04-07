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
using o2g.Types.TelephonyNS.DeviceNS;
using o2g.Types.TelephonyNS.UserNS;
using System.Collections.Generic;

namespace o2g.Types.TelephonyNS
{
    /// <summary>
    /// <c>TelephonicState</c> represents a user's telephonic state. 
    /// <para>The telephonic state is the list of all call involving the user, and the associated capability.</para>
    /// </summary>
    /// <remarks>
    /// On application startup, state of the user must be retrieve to align the application with the real user state.
    /// </remarks>
    public class TelephonicState
    {
        /// <summary>
        /// This property gives the calls in progress.
        /// </summary>
        /// <value>
        /// A list of <see cref="PbxCall"/>.
        /// </value>
        public List<PbxCall> Calls { get; set; }

        /// <summary>
        /// This property gives the capabilities of the user's devices
        /// </summary>
        /// <value>
        /// A list of <see cref="DeviceCapabilities"/>. 
        /// </value>
        public List<DeviceCapabilities> DeviceCapabilities { get; set; }

        /// <summary>
        /// This property gives the user's state.
        /// </summary>
        /// <value>
        /// A <see cref="UserState"/> that represents the current user's state
        /// </value>
        public UserState UserState { get; set; }
    }
}
