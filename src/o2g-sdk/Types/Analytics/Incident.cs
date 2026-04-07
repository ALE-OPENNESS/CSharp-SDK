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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Types.AnalyticsNS
{
    /// <summary>
    /// <c>Incident</c> class represents an incident on an OmniPCX Enterprise node.
    /// </summary>
    public class Incident
    {
        /// <summary>
        /// Return this incident Id.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that is this incident identifier.
        /// </value>
        /// <remarks>
        /// Please refer to the OmniPCX Enterprise documentation for detail on the incidents.
        /// </remarks>
        public int Id { get; init; }

        /// <summary>
        /// Return the date this incident occurs.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that is the date and hour this incident occurs.
        /// </value>
        public DateTime Date { get; init; }

        /// <summary>
        /// Return the severity of this incident.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that is this incident severity.
        /// </value>
        public int Severity { get; init; }

        /// <summary>
        /// Return a textual description of this incident.
        /// </summary>
        /// <value>
        /// An <see langword="string"/> value that provides description of the incident.
        /// </value>
        public string Description { get; init; }

        /// <summary>
        /// Return the number of occurences of this incident.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that is the number of occurences of this incident.
        /// </value>
        public int NbOccurs { get; init; }

        /// <summary>
        /// Return the OmniPCX Enterprise node on which this incident has been raised.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that is the OmniPCX Enterprise node number.
        /// </value>
        public int Node { get; init; }

        /// <summary>
        /// Return whether this incident has been raised on teh main OmniPCX Enterprise call server.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the incident has been raised on the main call server; <see langword="false"/> otherwise.
        /// </value>
        public bool Main { get; init; }

        /// <summary>
        /// Return the rack related to this incident.
        /// </summary>
        /// <value>
        /// An <see langword="string"/> value that provides information on the rack.
        /// </value>
        public string Rack { get; init; }

        /// <summary>
        /// Return the board related to this incident.
        /// </summary>
        /// <value>
        /// An <see langword="string"/> value that provides information on the board.
        /// </value>
        public string Board { get; init; }

        /// <summary>
        /// Return the equipment related to this incident.
        /// </summary>
        /// <value>
        /// An <see langword="string"/> value that provides information on the equipment.
        /// </value>
        public string Equipment { get; init; }

        /// <summary>
        /// Return the termination related to this incident.
        /// </summary>
        /// <value>
        /// An <see langword="string"/> value that provides information on the termination.
        /// </value>
        public string Termination { get; init; }
    }
}
