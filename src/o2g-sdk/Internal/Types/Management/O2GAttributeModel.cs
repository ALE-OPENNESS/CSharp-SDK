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

using o2g.Types.ManagementNS;
using System;
using System.Collections.Generic;

namespace o2g.Internal.Types.Management
{
    internal class O2GAttributeModel
    {
        public string Name { get; init; }
        public bool Mandatory { get; init; }
        public string TypeValue { get; init; }
        public bool MultiValue { get; init; }
        public List<string> AllowedValues { get; init; }
        public string LengthValue { get; init; }
        public string DefaultValue { get; init; }
        public bool Filtering { get; init; }
        public string UsedWhen { get; init; }


        // The LengthValue is either a simple name, or a
        // range in the form x..y
        private static OctetStringLength ParseLengthValue(String value)
        {
            if (value == null)
            {
                return null;
            }

            string[] values = value.Split("..");
            if (values.Length == 2)
            {
                return new OctetStringLength()
                {
                    Min = int.Parse(values[0]),
                    Max = int.Parse(values[1])
                };
            }
            else if (values.Length == 1)
            {
                return new OctetStringLength()
                {
                    Min = 0,
                    Max = int.Parse(values[0])
                };
            }
            else
            {
                throw new O2GException(string.Format("Unable to parse LengthValue: {0}", value));
            }
        }

        internal ModelAttribute ToAttributeModel(string objectName)
        {
            return new ModelAttribute()
            {
                Name = Name,
                Mandatory = Mandatory,
                MultiValue = MultiValue,
                AllowedValues = AllowedValues,
                OctetStringLength = ParseLengthValue(LengthValue),
                DefaultValue = DefaultValue,
                Filtering = Filtering,
                UsedWhen = UsedWhen,
                Type = Enum.Parse<AttributeType>(TypeValue)
            };
        }
    }
}
