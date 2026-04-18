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
using o2g.Tests.Helpers;
using o2g.Types.DirectoryNS;
using Xunit;

namespace o2g.Tests.Types.Directory
{
    public class SearchResultTests : JsonTestBase
    {
        [Fact]
        public void Deserialize_SearchResult_OkCode_MapsResultCode()
        {
            var json = """
                {
                    "resultCode": "OK",
                    "resultElements": []
                }
                """;

            var result = Deserialize<SearchResult>(json);

            result.Should().NotBeNull();
            result.ResultCode.Should().Be(SearchResult.Code.Ok);
            result.ResultElements.Should().BeEmpty();
        }

        [Fact]
        public void Deserialize_SearchResult_NokCode_MapsResultCode()
        {
            var json = """{ "resultCode": "NOK", "resultElements": [] }""";

            var result = Deserialize<SearchResult>(json);

            result.ResultCode.Should().Be(SearchResult.Code.Nok);
        }

        [Fact]
        public void Deserialize_SearchResult_FinishCode_MapsResultCode()
        {
            var json = """{ "resultCode": "FINISH", "resultElements": [] }""";

            var result = Deserialize<SearchResult>(json);

            result.ResultCode.Should().Be(SearchResult.Code.Finish);
        }

        [Fact]
        public void Deserialize_SearchResult_TimeoutCode_MapsResultCode()
        {
            var json = """{ "resultCode": "TIMEOUT", "resultElements": [] }""";

            var result = Deserialize<SearchResult>(json);

            result.ResultCode.Should().Be(SearchResult.Code.Timeout);
        }

        [Fact]
        public void Deserialize_SearchResult_UnknownCode_FallsBack()
        {
            var json = """{ "resultCode": "SOME_UNKNOWN_VALUE", "resultElements": [] }""";

            var result = Deserialize<SearchResult>(json);

            result.ResultCode.Should().Be(SearchResult.Code.Unknown);
        }

        [Fact]
        public void Deserialize_SearchResult_WithContacts_MapsPartyInfo()
        {
            var json = """
                {
                    "resultCode": "OK",
                    "resultElements": [
                        {
                            "contacts": [
                                {
                                    "id": { "loginName": "jdoe", "phoneNumber": "1001" },
                                    "firstName": "John",
                                    "lastName": "Doe"
                                },
                                {
                                    "id": { "loginName": "asmith", "phoneNumber": "1002" },
                                    "firstName": "Alice",
                                    "lastName": "Smith"
                                }
                            ]
                        }
                    ]
                }
                """;

            var result = Deserialize<SearchResult>(json);

            result.ResultCode.Should().Be(SearchResult.Code.Ok);
            result.ResultElements.Should().HaveCount(1);
            result.ResultElements[0].Contacts.Should().HaveCount(2);

            var first = result.ResultElements[0].Contacts[0];
            first.Id.LoginName.Should().Be("jdoe");
            first.Id.PhoneNumber.Should().Be("1001");
            first.FirstName.Should().Be("John");
            first.LastName.Should().Be("Doe");

            var second = result.ResultElements[0].Contacts[1];
            second.Id.LoginName.Should().Be("asmith");
        }

        [Fact]
        public void Deserialize_SearchResult_MultipleResultElements()
        {
            var json = """
                {
                    "resultCode": "OK",
                    "resultElements": [
                        { "contacts": [ { "id": { "loginName": "jdoe" }, "firstName": "John", "lastName": "Doe" } ] },
                        { "contacts": [ { "id": { "loginName": "asmith" }, "firstName": "Alice", "lastName": "Smith" } ] }
                    ]
                }
                """;

            var result = Deserialize<SearchResult>(json);

            result.ResultElements.Should().HaveCount(2);
            result.ResultElements[0].Contacts[0].Id.LoginName.Should().Be("jdoe");
            result.ResultElements[1].Contacts[0].Id.LoginName.Should().Be("asmith");
        }
    }
}
