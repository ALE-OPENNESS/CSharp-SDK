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

using o2g.Types.TelephonyNS;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace o2g.Types.CommunicationLogNS
{
    /// <summary>
    /// <c>ComRecord</c> class represents a communication record, that is an history call ticket stored by the O2G server for each conversation.
    /// </summary>
    public class ComRecord
    {
        /// <summary>
        /// Return the identifier of the com record.
        /// </summary>
        /// <value>
        /// A <see langword="long"/> value that uniquely identify this com record.
        /// </value>
        [JsonPropertyName("recordId")]
        public long Id { get; init; }

        /// <summary>
        /// Return the call reference associated to this com record.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that is the identifier of the call at the origin of this com record.
        /// </value>
        /// <seealso cref="PbxCall.CallRef"/>
        [JsonPropertyName("comRef")]
        public string CallRef { get; init; }

        /// <summary>
        /// Return whether this com record represents a non aknowledged missed call.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the com record is related to an unanswered call not acknowledged; <see langword="false"/> otherwise.
        /// </value>
        public bool Acknowledged { get; init; }

        /// <summary>
        /// The list of participants associated to this call.
        /// </summary>
        /// <value>
        /// A list of <see cref="ComRecordParticipant"/>. A <c>Participant</c> represents the other party in this call.
        /// </value>
        public List<ComRecordParticipant> Participants { get; init; }

        /// <summary>
        /// Return the date the call has began.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the date the call has began.
        /// </value>
        [JsonPropertyName("beginDate")]
        public DateTime Begin { get; init; }

        /// <summary>
        /// Return the date the call has ended.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the date the call has ended.
        /// </value>
        [JsonPropertyName("endDate")]
        public DateTime End { get; init; }
    }
}
