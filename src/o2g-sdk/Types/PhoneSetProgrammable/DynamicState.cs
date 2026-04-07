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

namespace o2g.Types.PhoneSetProgrammableNS
{
    /// <summary>
    /// <c>DynamicState</c> represents the dynamic state of a device. This state provide information on the associate, and the locked and campon state.
    /// </summary>
    public class DynamicState
    {
        /// <summary>
        /// Return whether the device is locked.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the device is locked; <see langword="false"/> otherwise.
        /// </value>
        public bool Lock { get; init; }

        /// <summary>
        /// Return whether the camp on is activated.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the camp on is activated; <see langword="false"/> otherwise.
        /// </value>
        public bool Campon { get; init; }

        /// <summary>
        /// Return the associated phone number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the associate phone numnber or <see langword="null"/> if there is no associate.
        /// </value>
        public string Associate { get; init; }
    }
}
