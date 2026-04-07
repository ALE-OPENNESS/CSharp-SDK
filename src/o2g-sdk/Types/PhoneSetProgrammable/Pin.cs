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
    /// <c>Pin</c> class represents a PIN (Personal Identification Number)
    /// </summary>
    /// <remarks>
    /// <para>
    /// This service allows the user to announce that the call he is making is a personal (external) call and not a business call. 
    /// Using pulse metering tickets, the company is then able to invoice this call again as a personal call.
    /// </para>
    /// <para>
    /// This service can be applied to local users (employees) or visitors (in this case, the created user set is a virtual set without a physical address). 
    /// </para>
    /// <para>
    /// To use this service, users must dial the personal prefix followed by the PIN No. and then the secret code followed by the external number. 
    /// </para>
    /// <para>
    /// The Personal Identification Number is private and secret; only the administrator and, in certain cases, the pulse metering administrator may know it.
    /// </para>
    /// </remarks>
    public class Pin
    {
        /// <summary>
        /// The type of access this PIN code provide.
        /// </summary>
        public enum PinControl
        {
            /// <summary>
            /// PIN code can be used from any set according to categories.
            /// </summary>
            ByCategory, 

            /// <summary>
            /// PIN code is restricted to the users set.
            /// </summary>
            RestrictedToOwner, 

            /// <summary>
            /// PIN code can be used on any set in the system.
            /// </summary>
            UniversalAccess, 

            /// <summary>
            /// PIN code is limited to specific group.
            /// </summary>
            ByGroup
        }

        /// <summary>
        /// Return the PIN number.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the PIN number.
        /// </value>
        public string Number { get; init; }

        /// <summary>
        /// Return whether the PIN code request to be validated by the user secret code.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the secret code is requested; <see langword="false"/> otherwise.
        /// </value>
        public bool WithSecretCode { get; init; }

        /// <summary>
        /// Return the access control this PIN code provide.
        /// </summary>
        /// <value>
        /// A <see cref="PinControl"/> that represents the PIN access control.
        /// </value>
        public PinControl Control { get; init; }

        /// <summary>
        /// Return the PIN group associatde to this PIN code.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that represents the PIN group, or -1 if the PIN group is not associated to a group.
        /// </value>
        public int Group { get; init; }
    }
}
