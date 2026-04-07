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


using System.Text;
using System.Linq;
using System;

namespace o2g.Internal.Utility
{
    internal class HexaUtil
    {
        private const string HexAlphabet = "0123456789abcdef";

        public static string FromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            StringBuilder result = new(data.Length * 2);
            foreach (byte b in data)
            {
                result.Append(HexAlphabet[(int)(b >> 4)]);
                result.Append(HexAlphabet[(int)(b & 0xF)]);
            }

            return result.ToString();
        }

        public static byte[] ToByteArray(string data)
        {
            if (data == null)
            {
                return null;
            }
            else
            {
                string toTransform;
                if (data.Length % 2 != 0)
                {
                    toTransform = "0" + data;
                }
                else
                {
                    toTransform = data;
                }

                return Enumerable.Range(0, toTransform.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(toTransform.Substring(x, 2), 16))
                         .ToArray();
            }
        }
    }
}
