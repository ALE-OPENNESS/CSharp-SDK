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

namespace o2g.Types.MaintenanceNS
{
    /// <summary>
    /// <c>ConfigurationType</c> represents the possible O2G server configurations. 
    /// </summary>
    [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: ConfigurationType.Unknown)]
    public enum ConfigurationType
    {
        /// <summary>
        /// O2G Server is configured for management.
        /// </summary>
        /// <remarks>
        /// An O2G server configured for management does not monitor devices on the OmniPCX Enterprise.
        /// </remarks>
        [EnumMember(Value = "PBX_MANAGEMENT")]
        PbxManagement,

        /// <summary>
        /// O2G Server is configured with full services.
        /// </summary>
        [EnumMember(Value = "FULL_SERVICES")]
        FullServices,

        /// <summary>
        /// Unknown configuration
        /// </summary>
        Unknown
    }
}
