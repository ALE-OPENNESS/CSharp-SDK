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
using o2g.Internal.Utility;
using o2g.Types.CommonNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace o2g.Types.TelephonyNS.CallNS
{
    /// <summary>
    /// <c>CallData</c> represents the data associated to a call.
    /// </summary>
    public class CallData
    {
        /// <summary>
        /// This property gives the initial callee.
        /// </summary>
        /// <value>
        /// a <see cref="PartyInfo"/> object that gives the identity of the initial calle.
        /// </value>
        public PartyInfo InitialCalled { get; init; }

        /// <summary>
        /// Return whether the call is a device call or a user call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if it's a device call; <see langword="false"/> otherwise.
        /// </value>
        public bool DeviceCall { get; init; }

        /// <summary>
        /// Return whether the this call is anonymous.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the call is anonymous; <see langword="false"/> otherwise.
        /// </value>
        public bool Anonymous { get; init; }

        /// <summary>
        /// This property gives the call state.
        /// </summary>
        /// <value>
        /// A <see cref="MediaState"/> that represents the call state.
        /// </value>
        public MediaState State { get; init; }

        /// <summary>
        /// This property give the call recording state.
        /// </summary>
        /// <value>
        /// A <see cref="RecordState"/> value that indicated the recording state.
        /// </value>
        public RecordState RecordState { get; init; }

        /// <summary>
        /// This property return the tag list associated to this call.
        /// </summary>
        /// <value>
        /// A list of <see cref="Tag"/>.
        /// </value>
        public List<Tag> Tags { get; init; }

        /// <summary>
        /// This property return the capabilities available on this call.
        /// </summary>
        /// <value>
        /// A <see cref="CallCapabilities"/> object that represents the capability available in this call.
        /// </value>
        public CallCapabilities Capabilities { get; init; }

        /// <summary>
        /// This property return the string correlator data associated to this call.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the associated correlator data.
        /// </value>
        public string AssociatedData { get; init; }

        /// <summary>
        /// This property return the correlator data associated to this call.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the associated correlator data.
        /// </value>
        public string HexaBinaryAssociatedData { get; init; }

        /// <summary>
        /// Return the correlator data associated to this call.
        /// </summary>
        /// <returns>A <see langword="byte[]"/> that is the associated correlator data.
        /// </returns>
        public byte[] AttachedData()
        {
            if (AssociatedData != null)
            {
                return Encoding.Default.GetBytes(AssociatedData);
            }
            else if (HexaBinaryAssociatedData != null) 
            {
                return HexaUtil.ToByteArray(HexaBinaryAssociatedData);
            }
            else
            { 
                return null; 
            }
        }

        /// <summary>
        /// This property returns the account info associated to this call.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that is the account info.
        /// </value>
        public string AccountInfo { get; init; }

        /// <summary>
        /// This property gives the acd extension in case of an acd call.
        /// </summary>
        /// <value>
        /// A <see cref="AcdData"/> object that provide this call acd extension data.
        /// </value>
        public AcdData AcdCallData { get; init; }
    }
}
