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

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>PbxObjectDefinition</c> represent an OmniPCX Enterprise object definition.
    /// </summary>
    public class PbxObjectDefinition
    {
        /// <summary>
        /// Return the object instance name.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the object instance definition.
        /// </value>
        public string ObjectName { get; init; }

        /// <summary>
        /// Return the object instance identifier.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that represents the object instance identifier.
        /// </value>
        public string ObjectId { get; init; }
    }
}
