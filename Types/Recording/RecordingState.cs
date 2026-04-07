/*
* Copyright 2025 ALE International
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
    /// <c>RecordingState</c> represents the recording state associated to a call on a recordable device.
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: RecordingState.Unknown)]
    public enum RecordingState
    {
        /// <summary>
        /// Recording is in progress.
        /// </summary>
        [EnumMember(Value = "RECORDING_IN_PROGRESS")]
        RecordingInProgress,

        /// <summary>
        /// Recording has been pause.
        /// </summary>
        [EnumMember(Value = "RECORDING_IN_PAUSE")]
        RecordingInPause,

        /// <summary>
        /// The Recording of the device is controlled by the system: no user action is possible.
        /// </summary>
        [EnumMember(Value = "RECORDING_BY_SYSTEM")]
        RecordingBySystem,

        /// <summary>
        /// No recording in progress.
        /// </summary>
        [EnumMember(Value = "NO_RECORDING_IN_PROGRESS")]
        NoRecording,

        /// <summary>
        /// The RecordingStartType can not be retrieved.
        /// </summary>
        [EnumMember(Value = "UNKNOWN")]
        Unknown
    }
}
