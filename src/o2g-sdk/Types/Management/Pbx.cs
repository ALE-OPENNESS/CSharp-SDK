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

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>Pbx</c> represents basic information of a Pbx (OmniPCX Enterprise).
    /// </summary>
    /// <remarks>In an OmniPcx Enterprise sub-network, each OXE node is identifier by a unique node id.</remarks>
    public class Pbx
    {
        /// <summary>
        /// Return the OmniPcx Enterprise node id.
        /// </summary>
        /// <value>The integer that represents the node id.</value>
        public int NodeId { get; init; }

        /// <summary>
        /// Return the OmniPCX Enterprise node Fqdn.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that reprsents the OXE node Fqdn.
        /// </value>
        public string Fqdn { get; init; }
    }
}
