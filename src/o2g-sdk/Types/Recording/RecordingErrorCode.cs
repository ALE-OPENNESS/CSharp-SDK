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

namespace o2g.Types.RecordingNS
{
    /// <summary>
    /// <c>RecordingErrorCode</c> Lists the different error causes that can be returned.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: RecordingErrorCode.UnexpectedError)]
    public enum RecordingErrorCode
    {
        /// <summary>
        /// Unable to connect to the recorder.
        /// </summary>
        [EnumMember(Value = "CONNECTION_ERROR")]
        ConnectionError,

        /// <summary>
        /// Configuration issue (verify OXR parameters either in O2G or in OXR).
        /// </summary>
        [EnumMember(Value = "CONFIGURATION_ERROR")]
        ConfigurationError,

        /// <summary>
        /// Incorrect response to a recorder request.
        /// </summary>
        [EnumMember(Value = "OXR_ERROR")]
        RecorderError,

        /// <summary>
        /// Incorrect parameter in request.
        /// </summary>
        [EnumMember(Value = "REQUEST_ERROR")]
        RequestError,

        /// <summary>
        /// Unexpected error.
        /// </summary>
        [EnumMember(Value = "UNEXPECTED_ERROR")]
        UnexpectedError
    }
}
