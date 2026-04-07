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

using System.Text.Json.Serialization;

namespace o2g.Types.RoutingNS
{
    /// <summary>
    /// <c>RoutingCapability</c> represents the routing capability of a user.
    /// </summary>
    public class RoutingCapabilities
    {
        /// <summary>
        /// Return whether the user can manage the remote extension.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can manage the remote extension; <see langword="false"/> otherwise.
        /// </value>
        [JsonPropertyName("presentationRoute")]
        public bool CanManageRemoteExtension { get; set; }

        /// <summary>
        /// Return whether the user can manage the forward.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can manage the forward; <see langword="false"/> otherwise.
        /// </value>
        [JsonPropertyName("forwardRoute")]
        public bool CanManageForward { get; set; }

        /// <summary>
        /// Return whether the user can manage the overflow.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can manage the overflow; <see langword="false"/> otherwise.
        /// </value>
        [JsonPropertyName("overflowRoute")]
        public bool CanManageOverflow { get; set; }

        /// <summary>
        /// Return whether the user can manage the do not disturb.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the user can manage the do not disturb; <see langword="false"/> otherwise.
        /// </value>
        [JsonPropertyName("dnd")] 
        public bool CanManageDnd { get; set; }
    }
}
