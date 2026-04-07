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

namespace o2g.Types.MaintenanceNS
{
    /// <summary>
    /// <c>License</c> class represents an O2G license.
    /// </summary>
    public class License
    {
        /// <summary>
        /// Return the license name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the license name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Return the number of licenses.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the number of licenses.
        /// </value>
        public int Total { get; init; }

        /// <summary>
        /// Return the number of licenses used.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that represents the number of licenses used.
        /// </value>
        public int CurrentUsed { get; init; }

        /// <summary>
        /// Return the license expiration date.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that represents the license expiration date.
        /// </value>
        public string Expiration { get; init; }
    }
}
