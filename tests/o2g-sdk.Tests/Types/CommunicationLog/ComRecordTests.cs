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
using o2g.Types.CommunicationLogNS;
using System;
using Xunit;

namespace o2g.Tests.Types.CommunicationLog
{
    public class ComRecordTests : JsonTestBase
    {
        #region ComRecord

        [Fact]
        public void Deserialize_ComRecord_MapsCustomJsonNames()
        {
            var json = """
                {
                    "recordId": 42,
                    "comRef": "ref-001",
                    "acknowledged": true,
                    "participants": [],
                    "beginDate": "2026-01-15T10:00:00Z",
                    "endDate": "2026-01-15T10:05:00Z"
                }
                """;

            var record = Deserialize<ComRecord>(json);

            record.Should().NotBeNull();
            record.Id.Should().Be(42);
            record.CallRef.Should().Be("ref-001");
            record.Acknowledged.Should().BeTrue();
            record.Participants.Should().NotBeNull().And.BeEmpty();
            record.Begin.Should().Be(new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc));
            record.End.Should().Be(new DateTime(2026, 1, 15, 10, 5, 0, DateTimeKind.Utc));
        }

        [Fact]
        public void Deserialize_ComRecord_UnacknowledgedMissedCall()
        {
            var json = """
                {
                    "recordId": 7,
                    "comRef": "ref-missed",
                    "acknowledged": false,
                    "participants": [],
                    "beginDate": "2026-03-10T08:00:00Z",
                    "endDate": "2026-03-10T08:00:05Z"
                }
                """;

            var record = Deserialize<ComRecord>(json);

            record.Id.Should().Be(7);
            record.Acknowledged.Should().BeFalse();
        }

        [Fact]
        public void Deserialize_ComRecord_WithParticipant_MapsIdentity()
        {
            var json = """
                {
                    "recordId": 1,
                    "comRef": "ref-001",
                    "acknowledged": false,
                    "participants": [
                        {
                            "role": "CALLER",
                            "answered": true,
                            "anonymous": false,
                            "identity": {
                                "id": { "loginName": "jdoe", "phoneNumber": "1001" },
                                "firstName": "John",
                                "lastName": "Doe"
                            }
                        }
                    ],
                    "beginDate": "2026-01-15T10:00:00Z",
                    "endDate": "2026-01-15T10:05:00Z"
                }
                """;

            var record = Deserialize<ComRecord>(json);

            record.Participants.Should().HaveCount(1);
            record.Participants[0].Role.Should().Be(Role.Caller);
            record.Participants[0].Answered.Should().BeTrue();
            record.Participants[0].Anonymous.Should().BeFalse();
            record.Participants[0].Identity.FirstName.Should().Be("John");
            record.Participants[0].Identity.LastName.Should().Be("Doe");
            record.Participants[0].Identity.Id.LoginName.Should().Be("jdoe");
            record.Participants[0].Identity.Id.PhoneNumber.Should().Be("1001");
        }

        #endregion

        #region QueryResult

        [Fact]
        public void Deserialize_QueryResult_MapsCustomJsonNames()
        {
            var json = """
                {
                    "comHistoryRecords": [
                        {
                            "recordId": 1,
                            "comRef": "ref-001",
                            "acknowledged": true,
                            "participants": [],
                            "beginDate": "2026-01-15T10:00:00Z",
                            "endDate": "2026-01-15T10:05:00Z"
                        }
                    ],
                    "offset": 0,
                    "limit": 10,
                    "totalCount": 1
                }
                """;

            var result = Deserialize<QueryResult>(json);

            result.Should().NotBeNull();
            result.Records.Should().HaveCount(1);
            result.Records[0].Id.Should().Be(1);
            result.Records[0].CallRef.Should().Be("ref-001");
            result.Offset.Should().Be(0);
            result.Limit.Should().Be(10);
            result.Count.Should().Be(1);
        }

        [Fact]
        public void Deserialize_QueryResult_EmptyRecords()
        {
            var json = """
                {
                    "comHistoryRecords": [],
                    "offset": 20,
                    "limit": 10,
                    "totalCount": 0
                }
                """;

            var result = Deserialize<QueryResult>(json);

            result.Records.Should().BeEmpty();
            result.Offset.Should().Be(20);
            result.Limit.Should().Be(10);
            result.Count.Should().Be(0);
        }

        #endregion
    }
}
