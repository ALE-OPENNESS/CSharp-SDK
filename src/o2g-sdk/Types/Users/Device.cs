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
    /// <c>Device</c> class represents a device of a user.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// The device type
        /// </summary>
        [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: DeviceType.Unknown)]
        public enum DeviceType
        {
            /// <summary>
            /// The device is a DECT device
            /// </summary>
            [EnumMember(Value = "DECT")]
            Dect,

            /// <summary>
            /// The device is a deskphone
            /// </summary>
            [EnumMember(Value = "DESKPHONE")]
            Deskphone,

            /// <summary>
            /// The device is a mobile device
            /// </summary>
            [EnumMember(Value = "MOBILE")]
            Mobile,

            /// <summary>
            /// The device is a softphone
            /// </summary>
            [EnumMember(Value = "SOFTPHONE")]
            Softphone,

            /// <summary>
            /// Unknown device type
            /// </summary>
            Unknown
        }

        /// <summary>
        /// This property gives the device type
        /// </summary>
        /// <value>
        /// A <see cref="DeviceType"/> enum that represents the device type.
        /// </value>
        public DeviceType Type { get; init; }

        /// <summary>
        /// This property gives the device identifier which is used to identify the device in telephony requests and events.
        /// </summary>
        /// <value>
        /// A unique <see langword="string"/> that represents this device.
        /// </value>
        public string Id { get; init; }

        /// <summary>
        /// Ths property gives the device sub-type.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the device sub-type.
        /// </value>
        /// <remarks>
        /// When set, the device sub-type provide information about the device model.
        /// </remarks>
        public string SubType { get; init; }
    }
}
