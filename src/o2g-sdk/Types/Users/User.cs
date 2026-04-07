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

namespace o2g.Types.UsersNS
{
    /// <summary>
    /// <c>User</c> class represents a user in the system. A user is an OmniPCX Enterprise subscriber. He can have one or several devices, an a mail box.
    /// </summary>
    public class User
    {
        /// <summary>
        /// This property gives the user company phone number. This company phone number is the phone number of the main device when the user has a multi-device configuration.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the user company phone number.
        /// </value>
        public string CompanyPhone { get; init; }

        /// <summary>
        /// The user first name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the user first name.
        /// </value>
        /// <remarks>
        /// Corresponds to the "UTF8_Phone_Book_First_Name" parameter. If this one is empty, it is the parameter "Annu_First_Name" which is taken into account.
        /// </remarks>
        public string FirstName { get; init; }

        /// <summary>
        /// The user last name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the user last name.
        /// </value>
        /// <remarks>
        /// Corresponds to the "UTF8_Phone_Book_Last_Name" parameter. If this one is empty, it is the parameter "Annu_Last_Name" which is taken into account.
        /// </remarks>
        public string LastName { get; init; }

        /// <summary>
        /// The user login name that identifies the user account.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the user login name.
        /// </value>
        public string LoginName { get; init; }

        /// <summary>
        /// External login of the user if set.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the user external login.
        /// </value>
        public string ExternalLogin { get; init; }

        /// <summary>
        /// The voice mail associated to this user.
        /// </summary>
        /// <value>
        /// A <see cref="Voicemail"/> object that represents the voice mail, or <see langword="null"/> if the user has no voice mail.
        /// </value>
        public Voicemail Voicemail { get; init; }

        /// <summary>
        /// The list of devices of this user. 
        /// </summary>
        /// <value>
        /// A list of <see cref="Device"/>.
        /// </value>
        public List<Device> Devices { get; init; }

        /// <summary>
        /// This property give the OmniPCX Enterprise node the user belongs to.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> that is the OXE node number.
        /// </value>
        public string NodeId { get; init; }
    }
}
