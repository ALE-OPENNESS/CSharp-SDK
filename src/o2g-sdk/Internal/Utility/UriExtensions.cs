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
using System.Linq;

namespace o2g.Internal.Utility
{
    internal static class UriExtensions
    {
        public static Uri Append(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }

        public static Uri AppendQuery(this Uri uri, string queryName, string queryValue = null)
        {
            if (uri.AbsoluteUri.Contains('?'))
            {
                if (queryValue == null)
                {
                    return new Uri(String.Format("{0}&{1}", uri.AbsoluteUri, queryName));
                }
                else
                {
                    return new Uri(String.Format("{0}&{1}={2}", uri.AbsoluteUri, queryName, queryValue));
                }
            }
            else
            {
                if (queryValue == null)
                {
                    return new Uri(String.Format("{0}?{1}", uri.AbsoluteUri, queryName));
                }
                else
                {
                    return new Uri(String.Format("{0}?{1}={2}", uri.AbsoluteUri, queryName, queryValue));
                }
            }
        }
    }
}
