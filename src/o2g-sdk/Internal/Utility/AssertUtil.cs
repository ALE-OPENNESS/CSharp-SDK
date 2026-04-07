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

namespace o2g.Internal.Utility
{
    internal class AssertUtil
    {
        internal static string NotNullOrEmpty(string parameter, string name)
        {
            if (String.IsNullOrEmpty(parameter) || String.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException(String.Format("{0} must not be null or empty", name));
            }

            return parameter;
        }

        internal static List<T> NotNullList<T>(List<T> aList, string name)
        {
            if ((aList == null) || (aList.Count == 0))
            {
                throw new ArgumentException(String.Format("{0} must not be null or empty", name));
            }
            return aList;
        }

        internal static T NotNull<T>(T anObject, string name)
        {
            if (anObject == null)
            {
                throw new ArgumentException(String.Format("{0} must not be null or empty", name));
            }
            return anObject;
        }

        internal static int AssertPositive(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentException(String.Format("{0} must not be >= 0", name));
            }
            return value;
        }

        internal static int AssertPositiveStrict(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentException(String.Format("{0} must not be > 0", name));
            }
            return value;
        }
    }
}
