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
using o2g.Internal.Rest;
using o2g.Internal.Utility;
using o2g.Tests.Helpers;
using o2g.Types.AnalyticsNS;
using System.Net;

namespace o2g.Tests.Services
{
    public class AnalyticsRestTests : ServiceTestBase
    {
        private static readonly Uri AnalyticsUri = new("https://fake-o2g/api/analytics");

        private AnalyticsRest Service() =>
            DependancyResolver.Resolve(new AnalyticsRest(AnalyticsUri));

        #region GetChargingFilesAsync

        [Fact]
        public async Task GetChargingFilesAsync_WithoutFilter_SendsCorrectRequest()
        {
            SetupHttpClient("""
                {
                    "files": [
                        { "name": "charging_01.txt", "date": "02/26/24", "time": "15:30:00" },
                        { "name": "charging_02.txt", "date": "02/27/24", "time": "08:00:00" }
                    ]
                }
                """);

            var files = await Service().GetChargingFilesAsync(1, null);

            files.Should().HaveCount(2);
            files[0].Name.Should().Be("charging_01.txt");
            files[0].Timestamp.Should().Be(new DateTime(2024, 2, 26, 15, 30, 0));
            files[1].Name.Should().Be("charging_02.txt");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/analytics/charging/files?nodeId=1");
        }

        [Fact]
        public async Task GetChargingFilesAsync_WithFilter_AppendsDateQueryParams()
        {
            SetupHttpClient("""{ "files": [] }""");

            var filter = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            await Service().GetChargingFilesAsync(1, filter);

            AssertRequest().Uri(
                "/api/analytics/charging/files?nodeId=1&fromDate=20240101&toDate=20240131");
        }

        [Fact]
        public async Task GetChargingFilesAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var files = await Service().GetChargingFilesAsync(1, null);

            files.Should().BeNull();
        }

        #endregion

        #region GetChargingsAsync — with DateRange

        [Fact]
        public async Task GetChargingsAsync_WithDateRange_SendsCorrectRequest()
        {
            SetupHttpClient("""
                {
                    "chargings": [
                        {
                            "caller": "1234",
                            "duration": 120,
                            "callType": "LocalNode",
                            "startDate": "20240226 15:30:00"
                        }
                    ],
                    "fromDate": "20240101",
                    "toDate": "20240131",
                    "nbChargingFiles": 2,
                    "totalTicketNb": 100,
                    "valuableTicketNb": 42
                }
                """);

            var filter = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            var result = await Service().GetChargingsAsync(1, filter, null, false);

            result.Should().NotBeNull();
            result.Chargings.Should().HaveCount(1);
            result.Chargings[0].Caller.Should().Be("1234");
            result.Chargings[0].CallType.Should().Be(CallType.LocalNode);
            result.Range.Should().NotBeNull();
            result.Range.from.Should().Be(new DateTime(2024, 1, 1));
            result.Range.to.Should().Be(new DateTime(2024, 1, 31));
            result.ChargingFileCount.Should().Be(2);
            result.TotalTicketCount.Should().Be(100);
            result.ValuableTicketCount.Should().Be(42);
            AssertRequest().Uri(
                "/api/analytics/charging?nodeId=1&fromDate=20240101&toDate=20240131");
        }

        [Fact]
        public async Task GetChargingsAsync_WithTopResults_AppendsTopQueryParam()
        {
            SetupHttpClient("""
                {
                    "chargings": [],
                    "nbChargingFiles": 0,
                    "totalTicketNb": 0,
                    "valuableTicketNb": 0
                }
                """);

            await Service().GetChargingsAsync(1, filter: null, 10, false);

            AssertRequest().Uri("/api/analytics/charging?nodeId=1&top=10");
        }

        [Fact]
        public async Task GetChargingsAsync_WithAll_AppendsAllQueryParam()
        {
            SetupHttpClient("""
                {
                    "chargings": [],
                    "nbChargingFiles": 0,
                    "totalTicketNb": 0,
                    "valuableTicketNb": 0
                }
                """);

            await Service().GetChargingsAsync(1, filter: null, null, true);

            AssertRequest().Uri("/api/analytics/charging?nodeId=1&all=true");
        }

        [Fact]
        public async Task GetChargingsAsync_WithNoDates_RangeIsNull()
        {
            SetupHttpClient("""
                {
                    "chargings": [],
                    "nbChargingFiles": 0,
                    "totalTicketNb": 0,
                    "valuableTicketNb": 0
                }
                """);

            var result = await Service().GetChargingsAsync(1, filter: null, null, false);

            result.Range.Should().BeNull();
        }

        [Fact]
        public async Task GetChargingsAsync_WithDateRange_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetChargingsAsync(1, filter: null, null, false);

            result.Should().BeNull();
        }

        #endregion

        #region GetChargingsAsync — with files

        [Fact]
        public async Task GetChargingsAsync_WithFiles_AppendsFileNamesQueryParam()
        {
            SetupHttpClient("""
                {
                    "chargings": [],
                    "nbChargingFiles": 2,
                    "totalTicketNb": 0,
                    "valuableTicketNb": 0
                }
                """);

            var files = new List<ChargingFile>
            {
                new() { Name = "charging_01.txt" },
                new() { Name = "charging_02.txt" }
            };

            await Service().GetChargingsAsync(1, files);

            AssertRequest().Uri(
                "/api/analytics/charging?nodeId=1&files=charging_01.txt%2Ccharging_02.txt");
        }

        [Fact]
        public async Task GetChargingsAsync_WithFilesAndTopResults_AppendsBothQueryParams()
        {
            SetupHttpClient("""
                {
                    "chargings": [],
                    "nbChargingFiles": 0,
                    "totalTicketNb": 0,
                    "valuableTicketNb": 0
                }
                """);

            var files = new List<ChargingFile> { new() { Name = "charging_01.txt" } };

            await Service().GetChargingsAsync(1, files, topResults: 5);

            AssertRequest().Uri(
                "/api/analytics/charging?nodeId=1&files=charging_01.txt&top=5");
        }

        #endregion

        #region GetIncidentsAsync

        [Fact]
        public async Task GetIncidentsAsync_WithoutLast_SendsCorrectRequest()
        {
            SetupHttpClient("""
                {
                    "incidents": [
                        {
                            "date": "26/02/24",
                            "hour": "15:30:00",
                            "severity": 2,
                            "value": "1042",
                            "type": "Link failure",
                            "nbOccurs": 3,
                            "node": "1",
                            "main": true,
                            "rack": "R1",
                            "board": "B2",
                            "equipement": "E3",
                            "termination": "T4"
                        }
                    ]
                }
                """);

            var incidents = await Service().GetIncidentsAsync(1, 0);

            incidents.Should().HaveCount(1);
            incidents[0].Id.Should().Be(1042);
            incidents[0].Date.Should().Be(new DateTime(2024, 2, 26, 15, 30, 0));
            incidents[0].Severity.Should().Be(2);
            incidents[0].Description.Should().Be("Link failure");
            incidents[0].NbOccurs.Should().Be(3);
            incidents[0].Node.Should().Be(1);
            incidents[0].Main.Should().BeTrue();
            incidents[0].Rack.Should().Be("R1");
            incidents[0].Board.Should().Be("B2");
            incidents[0].Equipment.Should().Be("E3");
            incidents[0].Termination.Should().Be("T4");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/analytics/incidents?nodeId=1");
        }

        [Fact]
        public async Task GetIncidentsAsync_WithLast_AppendsLastQueryParam()
        {
            SetupHttpClient("""{ "incidents": [] }""");

            await Service().GetIncidentsAsync(1, 10);

            AssertRequest().Uri("/api/analytics/incidents?nodeId=1&last=10");
        }

        [Fact]
        public async Task GetIncidentsAsync_WhenIncidentsNull_ReturnsNull()
        {
            SetupHttpClient("""{}""");

            var incidents = await Service().GetIncidentsAsync(1, 0);

            incidents.Should().BeNull();
        }

        [Fact]
        public async Task GetIncidentsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var incidents = await Service().GetIncidentsAsync(1, 0);

            incidents.Should().BeNull();
        }

        #endregion
    }
}