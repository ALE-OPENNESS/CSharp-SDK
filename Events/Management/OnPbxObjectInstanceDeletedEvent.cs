﻿/*
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

using o2g.Types.Management;

namespace o2g.Events.Management
{
    /// <summary>
    /// <c>OnPbxObjectInstanceDeletedEvent</c> is sent when a PBX object instance is deleted.
    /// </summary>
    /// <remarks>
    /// <b>The current O2G send this event only on Subscriber deletion.</b>
    /// </remarks>
    public class OnPbxObjectInstanceDeletedEvent : O2GEvent
    {
        /// <summary>
        /// The OmniPCX Enterprise node id that send this event.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that is the OmniPC Enterprise node number.
        /// </value>
        public int NodeId { get; init; }

        /// <summary>
        /// Return the deleted object definition.
        /// </summary>
        /// <value>
        /// A <see cref="PbxObjectDefinition"/> object that represents the deleted object.
        /// </value>
        public PbxObjectDefinition Object { get; init; }

        /// <summary>
        /// Return the object father of the deleted object.
        /// </summary>
        /// <value>
        /// A <see cref="PbxObjectDefinition"/> object that represents the father of the object.
        /// </value>
        public PbxObjectDefinition Father { get; init; }
    }
}
