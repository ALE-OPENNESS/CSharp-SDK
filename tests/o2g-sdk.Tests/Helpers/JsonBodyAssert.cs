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

using FluentAssertions;
using Json.Path;
using o2g.Internal.Rest;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace o2g.Tests.Helpers
{
    public class JsonBodyAssert
    {
        private readonly JsonNode _root;

        internal JsonBodyAssert(string json)
        {
            _root = JsonNode.Parse(json)!;
        }

        /// <summary>
        /// Asserts that the value at the given JSONPath equals the expected value.
        /// </summary>
        public JsonBodyAssert AssertValue(string path, object expected)
        {
            var result = Evaluate(path);
            result.Should().HaveCount(1, $"path '{path}' should match exactly one value");

            var expectedStr = JsonSerializer.Serialize(expected, AbstractRESTService.serializeOptions);

            result[0]?.ToJsonString().Should().Be(expectedStr);
            return this;
        }

        /// <summary>
        /// Asserts that the array at the given JSONPath contains all the expected values.
        /// </summary>
        public JsonBodyAssert AssertArrayContains(string path, IEnumerable<object> expected)
        {
            var result = Evaluate(path);
            result.Should().HaveCount(1, $"path '{path}' should match exactly one value");

            var array = result[0]!.AsArray()
                .Select(e => e?.ToString())
                .ToList();

            foreach (var item in expected)
            {
                array.Should().Contain(item?.ToString(),
                    $"array at '{path}' should contain '{item}'");
            }
            return this;
        }

        /// <summary>
        /// Asserts that the value at the given JSONPath is null or missing.
        /// </summary>
        public JsonBodyAssert AssertNull(string path)
        {
            var result = Evaluate(path);
            result.Should().BeEmpty($"path '{path}' should be null or missing");
            return this;
        }

        private List<JsonNode?> Evaluate(string path)
        {
            var jsonPath = JsonPath.Parse(path);
            var results = jsonPath.Evaluate(_root);
            return results.Matches?.Select(m => m.Value).ToList() ?? new List<JsonNode?>();
        }
    }
}

