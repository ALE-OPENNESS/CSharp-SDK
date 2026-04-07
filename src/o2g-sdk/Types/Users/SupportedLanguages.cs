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
using System.Text.Json.Serialization;

namespace o2g.Types.UsersNS
{
    /// <summary>
    /// <c>SupportedLanguages</c> represents the list of languages supported by a user.
    /// </summary>
    public class SupportedLanguages
    {
        /// <summary>
        /// The supported languages.
        /// </summary>
        /// <value>
        /// The list <see langword="string"/> that represents the supported languages.
        /// </value>
        [JsonPropertyName("SupportedLanguages")]
        public List<string> Languages { get; init; }

        /// <summary>
        /// The supported GUI languages.
        /// </summary>
        /// <value>
        /// The list <see langword="string"/> that represents the supported GUI languages.
        /// </value>
        [JsonPropertyName("SupportedGuiLanguages")]
        public List<string> GuiLanguages { get; init; }
    }
}
