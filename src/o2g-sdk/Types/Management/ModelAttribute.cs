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

using System.Collections.Generic;

namespace o2g.Types.ManagementNS
{
    /// <summary>
    /// <c>OctetStringLength</c> object represents a length constraint for an OctetString object.
    /// </summary>
    public class OctetStringLength
    {
        /// <summary>
        /// Return the minimal length.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that represents the string minimal length.
        /// </value>
        public int Min { get; init; }

        /// <summary>
        /// Return the maximum length.
        /// </summary>
        /// <value>
        /// An <see langword="int"/> value that represents the string maximal length.
        /// </value>
        public int Max { get; init; }
    }


    /// <summary>
    /// <c>AttributeModel</c> represents an attribute in an OmniPCX Enterprise object model.
    /// </summary>
    public class ModelAttribute
    {
        /// <summary>
        /// Return the attribute name.
        /// </summary>
        /// <value>
        /// The <see langword="string"/> value of the attribute name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Return whether the attribute is mandatory.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the attribute is mandatory; <see langword="false"/> otherwise.
        /// </value>
        /// <remarks>
        /// A mandatory attribute is an Object identifier.
        /// </remarks>
        public bool Mandatory { get; init; }

        /// <summary>
        /// Return the attribute type.
        /// </summary>
        /// <value>
        /// A <see cref="AttributeType"/> value that represents this attribute type.
        /// </value>
        public AttributeType Type { get; init; }

        /// <summary>
        /// Return whether the attribute is a list of values.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the attribute is a list; <see langword="false"/> otherwise.
        /// </value>
        public bool MultiValue { get; init; }

        /// <summary>
        /// Return the list of allowed values for this attribute.
        /// </summary>
        /// <value>
        /// A list of <see langword="string"/> that represents the allowed values for this attribute.
        /// </value>
        public List<string> AllowedValues { get; init; }

        /// <summary>
        /// Return the maximum length of the attribute value when the attribute is an byte string.
        /// </summary>
        /// <value>
        /// A <see langword="int"/> value that is the maximum length of a byte string attribute.
        /// </value>
        public OctetStringLength OctetStringLength { get; init; }

        /// <summary>
        /// Return the attribute default value.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> value that is this attribute default value.
        /// </value>
        public string DefaultValue { get; init; }

        /// <summary>
        /// Return whether the attribute can be filtered.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the attribute can be filtered; <see langword="false"/> otherwise.
        /// </value>
        public bool Filtering { get; init; }

        /// <summary>
        /// Return in which context this attribute is used.
        /// </summary>
        /// <value>
        /// A <see langword="string"/> that indicate when this attribute can be used.
        /// </value>
        public string UsedWhen { get; init; }
    }
}
