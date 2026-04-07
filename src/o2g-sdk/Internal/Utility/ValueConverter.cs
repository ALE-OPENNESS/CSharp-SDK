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

namespace o2g.Internal.Utility
{
    internal static class ValueConverter
    {
        private static string AssertUnique(List<string> values)
        {
            if ((values == null) || (values.Count != 1))
            {
                throw new FormatException("Value is not an int : null or not unique");
            }

            return values[0];
        }


        public static List<T> ToListConverter<T>(List<string> values) 
        {
            if (values == null)
            {
                throw new FormatException("Value is not a list : null");
            }

            if (typeof(T) == typeof(string))
            {
                return values.Select((v) => (T)Convert.ChangeType(v, typeof(T))).ToList();
            }
            else if (typeof(T) == typeof(int))
            {
                return values.Select((v) => (T)Convert.ChangeType(int.Parse(v), typeof(T))).ToList();
            }
            else if (typeof(T) == typeof(bool))
            {
                return values.Select((v) => (T)Convert.ChangeType(ToBoolConverter(v), typeof(T))).ToList();
            }
            else
            {
                throw new InvalidCastException(string.Format("Unable to transform in a list of {0} : No acceptable conversion", typeof(T).ToString()));
            }
        }



        public static string ToEnumConverter(List<string> values)
        {
            return ToStringConverter(values);
        }

        public static List<string> FromEnumConverter(string value)
        {
            return FromStringConverter(value);
        }


        public static string ToStringConverter(List<string> values)
        {
            return AssertUnique(values);
        }

        public static List<string> FromStringConverter(string value)
        {
            return new List<string> { value };
        }

        public static int ToIntConverter(List<string> values)
        {
            return int.Parse(AssertUnique(values));
        }

        public static List<string> FromIntConverter(int value)
        {
            return FromStringConverter(value.ToString());
        }


        private static bool ToBoolConverter(string value)
        {
            if (value == "true")
            {
                return true;
            }
            else if (value == "false")
            {
                return false;
            }
            else
            {
                throw new FormatException(string.Format("Value [{0}] is not a boolean", value));
            }
        }

        public static bool ToBoolConverter(List<string> values)
        {
            return ToBoolConverter(AssertUnique(values));
        }
        public static List<string> FromBooleanConverter(bool value)
        {
            return FromStringConverter(value ? "true" : "false");
        }


        public static List<string> FromNative<T>(T value, Func<T, List<string>> converter)
        {
            return converter(value);
        }


        public static T ToNative<T>(List<string> values, Func<List<string>, T> converter)
        {
            return converter(values);
        }
    }
}
