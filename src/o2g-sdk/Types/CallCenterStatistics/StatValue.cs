/*
* Copyright 2026 ALE International
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
using System.Text.Json;

namespace o2g.Types.CallCenterStatisticsNS
{
    /// <summary>
    /// Wraps a raw statistical value and provides typed accessors.
    /// </summary>
    public class StatValue
    {
        private readonly JsonElement? _element;

        internal StatValue(JsonElement? element)
        {
            _element = element;
        }

        /// <summary>
        /// Returns the value as an integer, or <see langword="null"/> if not available or not convertible.
        /// </summary>
        public int? AsInteger()
        {
            if (_element == null) return null;
            var el = _element.Value;
            if (el.ValueKind == JsonValueKind.Number)
                return (int)el.GetDouble();
            if (el.ValueKind == JsonValueKind.String)
            {
                if (int.TryParse(el.GetString(), out int result)) return result;
            }
            return null;
        }

        /// <summary>
        /// Returns the value as a float, or <see langword="null"/> if not available or not convertible.
        /// </summary>
        public float? AsFloat()
        {
            if (_element == null) return null;
            var el = _element.Value;
            if (el.ValueKind == JsonValueKind.Number)
                return (float)el.GetDouble();
            if (el.ValueKind == JsonValueKind.String)
            {
                if (float.TryParse(el.GetString(), System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out float result)) return result;
            }
            return null;
        }

        /// <summary>
        /// Returns the value as a string, or <see langword="null"/> if not available.
        /// </summary>
        public string AsString()
        {
            if (_element == null) return null;
            var el = _element.Value;
            if (el.ValueKind == JsonValueKind.String) return el.GetString();
            if (el.ValueKind == JsonValueKind.Number) return el.GetDouble().ToString();
            return null;
        }

        /// <summary>
        /// Returns the value as a duration string in <c>hh:mm:ss</c> format, or <see langword="null"/> if not available.
        /// </summary>
        public string AsDuration()
        {
            if (_element == null) return null;
            var el = _element.Value;
            if (el.ValueKind == JsonValueKind.String) return el.GetString();
            return null;
        }
    }
}
