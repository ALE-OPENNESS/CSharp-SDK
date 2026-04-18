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
using o2g.Internal.Events;
using o2g.Internal.Rest;
using o2g.Internal.Utility;
using o2g.Tests.Helpers;
using o2g.Types.AnalyticsNS;
using o2g.Types.CallCenterStatisticsNS;
using o2g.Types.CallCenterStatisticsNS.Scheduled;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace o2g.Tests.Services
{
    public class CallCenterStatisticsRestTests : ServiceTestBase
    {
        private static readonly Uri StatisticsUri = new("https://fake-o2g/api/acdstatistics");

        private CallCenterStatisticsRest Service() =>
            DependancyResolver.Resolve(new CallCenterStatisticsRest(StatisticsUri));

        private static StatsRequester TestRequester() => new StatsRequester
        {
            Id = "myId",
            Language = Language.EN,
            Timezone = "Europe/Paris"
        };

        private static StatsContext TestContext() => new StatsContext
        {
            Id = "ctx1",
            RequesterId = "myId",
            Label = "myLabel",
            Description = "myDesc"
        };

        private static ScheduledReport TestReport() => new ScheduledReport
        {
            Id = "report1",
            Description = "My report",
            ObservationPeriod = ReportObservationPeriod.OnCurrentDay(),
            Recurrence = Recurrence.Daily(),
            Format = StatsFormat.CSV,
            Recipients = new List<string> { "user@example.com" },
            Context = TestContext()
        };

        private void FireProgressEvent(AcdProgressStep step, string fullResPath = null, string xlsFullResPath = null, int nbTot = 0, int nbProcessed = 0)
        {
            EventHandlers.Throw(new O2GEventDescriptor(
                new OnAcdStatsProgressEvent
                {
                    EventName = "OnAcdStatsProgress",
                    Step = step,
                    NbTotObjects = nbTot,
                    NbProcessedObjects = nbProcessed,
                    FullResPath = fullResPath,
                    XlsFullResPath = xlsFullResPath
                }, null, null));
        }

        #region CreateRequesterAsync

        [Fact]
        public async Task CreateRequesterAsync_SendsPostToScopeEndpoint()
        {
            SetupHttpClient("{}");

            await Service().CreateRequesterAsync("myId", Language.EN, "Europe/Paris",
                new List<string> { "3000" });

            AssertRequest().Method(HttpMethod.Post).Uri("/api/acdstatistics/scope");
        }

        [Fact]
        public async Task CreateRequesterAsync_BodyContainsSupervisorFields()
        {
            SetupHttpClient("{}");

            await Service().CreateRequesterAsync("myId", Language.EN, "Europe/Paris",
                new List<string> { "3000" });

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.supervisor.identifier", "myId");
                j.AssertValue("$.supervisor.language", "EN");
                j.AssertValue("$.supervisor.timezone", "Europe/Paris");
            });
        }

        [Fact]
        public async Task CreateRequesterAsync_BodyContainsAgents()
        {
            SetupHttpClient("{}");

            await Service().CreateRequesterAsync("myId", Language.EN, "Europe/Paris",
                new List<string> { "3000", "3001" });

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.agents[0].number", "3000");
                j.AssertValue("$.agents[1].number", "3001");
            });
        }

        [Fact]
        public async Task CreateRequesterAsync_OnSuccess_ReturnsRequesterWithInputValues()
        {
            SetupHttpClient("{}");

            var result = await Service().CreateRequesterAsync("myId", Language.EN, "Europe/Paris",
                new List<string> { "3000" });

            result.Should().NotBeNull();
            result.Id.Should().Be("myId");
            result.Language.Should().Be(Language.EN);
            result.Timezone.Should().Be("Europe/Paris");
        }

        [Fact]
        public async Task CreateRequesterAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().CreateRequesterAsync("myId", Language.EN, "Europe/Paris",
                new List<string> { "3000" });

            result.Should().BeNull();
        }

        #endregion

        #region DeleteRequesterAsync

        [Fact]
        public async Task DeleteRequesterAsync_SendsDeleteWithRequesterId()
        {
            SetupHttpClient("{}");

            await Service().DeleteRequesterAsync(TestRequester());

            AssertRequest().Method(HttpMethod.Delete).Uri("/api/acdstatistics/scope/myId");
        }

        [Fact]
        public async Task DeleteRequesterAsync_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("{}");

            var result = await Service().DeleteRequesterAsync(TestRequester());

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteRequesterAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var result = await Service().DeleteRequesterAsync(TestRequester());

            result.Should().BeFalse();
        }

        #endregion

        #region GetRequesterAsync

        [Fact]
        public async Task GetRequesterAsync_SendsGetWithRequesterId()
        {
            SetupHttpClient("""{"identifier":"myId","language":"EN","timezone":"Europe/Paris"}""");

            await Service().GetRequesterAsync("myId");

            AssertRequest().Method(HttpMethod.Get).Uri("/api/acdstatistics/scope/myId");
        }

        [Fact]
        public async Task GetRequesterAsync_MapsResponseFields()
        {
            SetupHttpClient("""{"identifier":"myId","language":"EN","timezone":"Europe/Paris"}""");

            var result = await Service().GetRequesterAsync("myId");

            result.Should().NotBeNull();
            result.Id.Should().Be("myId");
            result.Language.Should().Be(Language.EN);
            result.Timezone.Should().Be("Europe/Paris");
        }

        [Fact]
        public async Task GetRequesterAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var result = await Service().GetRequesterAsync("myId");

            result.Should().BeNull();
        }

        #endregion

        #region CreateContextAsync

        [Fact]
        public async Task CreateContextAsync_SendsPostToScopeCtxEndpoint()
        {
            SetupHttpClient("""{"id":"ctx1"}""");

            await Service().CreateContextAsync(TestRequester(), "myLabel", "myDesc",
                StatsFilter.CreateAgentFilter());

            AssertRequest().Method(HttpMethod.Post).Uri("/api/acdstatistics/scope/myId/ctx");
        }

        [Fact]
        public async Task CreateContextAsync_BodyContainsSupervisorIdAndFilter()
        {
            SetupHttpClient("""{"id":"ctx1"}""");

            await Service().CreateContextAsync(TestRequester(), "myLabel", "myDesc",
                StatsFilter.CreateAgentFilter());

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.supervisorId", "myId");
                j.AssertValue("$.label", "myLabel");
                j.AssertValue("$.description", "myDesc");
            });
        }

        [Fact]
        public async Task CreateContextAsync_OnSuccess_ReturnsContextWithIdFromResponse()
        {
            SetupHttpClient("""{"id":"ctx1"}""");

            var ctx = await Service().CreateContextAsync(TestRequester(), "myLabel", "myDesc",
                StatsFilter.CreateAgentFilter());

            ctx.Should().NotBeNull();
            ctx.Id.Should().Be("ctx1");
            ctx.RequesterId.Should().Be("myId");
            ctx.Label.Should().Be("myLabel");
            ctx.Description.Should().Be("myDesc");
        }

        [Fact]
        public async Task CreateContextAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var ctx = await Service().CreateContextAsync(TestRequester(), null, null,
                StatsFilter.CreateAgentFilter());

            ctx.Should().BeNull();
        }

        #endregion

        #region GetContextsAsync

        [Fact]
        public async Task GetContextsAsync_SendsGetToScopeCtxEndpoint()
        {
            SetupHttpClient("""{"contexts":[]}""");

            await Service().GetContextsAsync(TestRequester());

            AssertRequest().Method(HttpMethod.Get).Uri("/api/acdstatistics/scope/myId/ctx");
        }

        [Fact]
        public async Task GetContextsAsync_MapsResponseContexts()
        {
            SetupHttpClient("""
                {
                    "contexts": [
                        {
                            "ctxId": "ctx1",
                            "supervisorId": "myId",
                            "label": "myLabel",
                            "description": "myDesc",
                            "isScheduled": false,
                            "shortHeader": false
                        }
                    ]
                }
                """);

            var contexts = await Service().GetContextsAsync(TestRequester());

            contexts.Should().HaveCount(1);
            contexts[0].Id.Should().Be("ctx1");
            contexts[0].RequesterId.Should().Be("myId");
            contexts[0].Label.Should().Be("myLabel");
        }

        [Fact]
        public async Task GetContextsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetContextsAsync(TestRequester());

            result.Should().BeNull();
        }

        #endregion

        #region DeleteContextsAsync

        [Fact]
        public async Task DeleteContextsAsync_SendsDeleteToScopeCtxEndpoint()
        {
            SetupHttpClient("{}");

            await Service().DeleteContextsAsync(TestRequester());

            AssertRequest().Method(HttpMethod.Delete).Uri("/api/acdstatistics/scope/myId/ctx");
        }

        #endregion

        #region GetContextAsync

        [Fact]
        public async Task GetContextAsync_SendsGetToScopeCtxIdEndpoint()
        {
            SetupHttpClient("""
                {
                    "ctxId": "ctx1",
                    "supervisorId": "myId",
                    "label": "myLabel",
                    "isScheduled": false,
                    "shortHeader": false
                }
                """);

            await Service().GetContextAsync(TestRequester(), "ctx1");

            AssertRequest().Method(HttpMethod.Get).Uri("/api/acdstatistics/scope/myId/ctx/ctx1");
        }

        [Fact]
        public async Task GetContextAsync_MapsResponseFields()
        {
            SetupHttpClient("""
                {
                    "ctxId": "ctx1",
                    "supervisorId": "myId",
                    "label": "myLabel",
                    "description": "myDesc",
                    "isScheduled": true,
                    "shortHeader": true
                }
                """);

            var ctx = await Service().GetContextAsync(TestRequester(), "ctx1");

            ctx.Id.Should().Be("ctx1");
            ctx.RequesterId.Should().Be("myId");
            ctx.IsScheduled.Should().BeTrue();
            ctx.ShortHeader.Should().BeTrue();
        }

        #endregion

        #region DeleteContextAsync

        [Fact]
        public async Task DeleteContextAsync_SendsDeleteToScopeCtxIdEndpoint()
        {
            SetupHttpClient("{}");

            await Service().DeleteContextAsync(TestContext());

            AssertRequest().Method(HttpMethod.Delete).Uri("/api/acdstatistics/scope/myId/ctx/ctx1");
        }

        #endregion

        #region GetDayDataAsync

        [Fact]
        public async Task GetDayDataAsync_SendsGetWithDateAndFormat()
        {
            SetupHttpClient("""{"supervisor":"myId"}""");

            await Service().GetDayDataAsync(TestContext(), false, new DateTime(2024, 1, 15));

            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/oneday/data?date=2024-01-15&format=json");
        }

        [Fact]
        public async Task GetDayDataAsync_WithTimeInterval_AppendsSlotTypeParam()
        {
            SetupHttpClient("""{"supervisor":"myId"}""");

            await Service().GetDayDataAsync(TestContext(), false, new DateTime(2024, 1, 15),
                TimeInterval.QuarterHour);

            AssertRequest().Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/oneday/data?date=2024-01-15&slotType=aQuarterOfAnHour&format=json");
        }

        [Fact]
        public async Task GetDayDataAsync_WithShortHeader_AppendsShortHeaderParam()
        {
            SetupHttpClient("""{"supervisor":"myId"}""");

            await Service().GetDayDataAsync(TestContext(), true, new DateTime(2024, 1, 15));

            AssertRequest().Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/oneday/data?date=2024-01-15&format=json&shortHeader=true");
        }

        [Fact]
        public async Task GetDayDataAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetDayDataAsync(TestContext(), false, DateTime.Today);

            result.Should().BeNull();
        }

        #endregion

        #region GetDaysDataAsync

        [Fact]
        public async Task GetDaysDataAsync_SendsGetWithDateRange()
        {
            SetupHttpClient("""{"supervisor":"myId"}""");

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            await Service().GetDaysDataAsync(TestContext(), false, range);

            AssertRequest().Method(HttpMethod.Get).Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/days/data" +
                "?begindate=2024-01-01%2000%3A00&enddate=2024-01-31%2000%3A00&format=json");
        }

        [Fact]
        public async Task GetDaysDataAsync_WithShortHeader_AppendsShortHeaderParam()
        {
            SetupHttpClient("""{"supervisor":"myId"}""");

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            await Service().GetDaysDataAsync(TestContext(), true, range);

            AssertRequest().Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/days/data" +
                "?begindate=2024-01-01%2000%3A00&enddate=2024-01-31%2000%3A00&format=json&shortHeader=true");
        }

        #endregion

        #region GetDayFileDataAsync

        [Fact]
        public async Task GetDayFileDataAsync_WithCsv_SendsGetWithCsvFormatAndAsyncParam()
        {
            SetupHttpClient("");

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.ERROR);
            await task;

            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/oneday/data?date=2024-01-15&format=csv&async=true");
        }

        [Fact]
        public async Task GetDayFileDataAsync_WithExcel_SendsGetWithXlsFormatAndAsyncParam()
        {
            SetupHttpClient("");

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.EXCEL);
            FireProgressEvent(AcdProgressStep.ERROR);
            await task;

            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/oneday/data?date=2024-01-15&format=xls&async=true");
        }

        [Fact]
        public async Task GetDayFileDataAsync_WithTimeInterval_AppendsSlotTypeBeforeFormat()
        {
            SetupHttpClient("");

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), TimeInterval.QuarterHour, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.ERROR);
            await task;

            AssertRequest().Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/oneday/data?date=2024-01-15&slotType=aQuarterOfAnHour&format=csv&async=true");
        }

        [Fact]
        public async Task GetDayFileDataAsync_WithShortHeader_AppendsShortHeaderParam()
        {
            SetupHttpClient("");

            var task = Service().GetDayFileDataAsync(TestContext(), true, new DateTime(2024, 1, 15), null, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.ERROR);
            await task;

            AssertRequest().Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/oneday/data?date=2024-01-15&format=csv&shortHeader=true&async=true");
        }

        [Fact]
        public async Task GetDayFileDataAsync_OnHttpError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.CSV);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDayFileDataAsync_WhenErrorEventFires_ReturnsNull()
        {
            SetupHttpClient("");

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.ERROR);

            var result = await task;

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDayFileDataAsync_WhenCancelledEventFires_ReturnsNull()
        {
            SetupHttpClient("");

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.CANCELLED);

            var result = await task;

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDayFileDataAsync_WhenFormattedEventFires_DownloadsFromFullResPath()
        {
            SetupHttpClient(
                ("", HttpStatusCode.OK),
                ("", HttpStatusCode.OK)
            );

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.FORMATED, fullResPath: "https://fake-o2g/api/stats/file.csv");
            await task;

            AssertRequest(1).Uri("/api/stats/file.csv");
        }

        [Fact]
        public async Task GetDayFileDataAsync_WithExcel_WhenFormattedEventFires_DownloadsFromXlsPath()
        {
            SetupHttpClient(
                ("", HttpStatusCode.OK),
                ("", HttpStatusCode.OK)
            );

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.EXCEL);
            FireProgressEvent(AcdProgressStep.FORMATED,
                fullResPath: "https://fake-o2g/api/stats/file.csv",
                xlsFullResPath: "https://fake-o2g/api/stats/file.xls");
            await task;

            AssertRequest(1).Uri("/api/stats/file.xls");
        }

        [Fact]
        public async Task GetDayFileDataAsync_ProgressCallback_InvokedWithCollectAndProcessedAndFormatted()
        {
            SetupHttpClient(
                ("", HttpStatusCode.OK),
                ("", HttpStatusCode.OK)
            );

            var steps = new List<(ProgressStep step, int progress)>();
            Action<ProgressStep, int> callback = (s, p) => steps.Add((s, p));

            var task = Service().GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null,
                StatsFormat.CSV, null, callback);

            FireProgressEvent(AcdProgressStep.COLLECT, nbTot: 10);
            FireProgressEvent(AcdProgressStep.PROCESSED, nbTot: 0, nbProcessed: 5);
            FireProgressEvent(AcdProgressStep.FORMATED, fullResPath: "https://fake-o2g/api/stats/file.csv");

            await task;

            steps.Should().HaveCount(3);
            steps[0].Should().Be((ProgressStep.Collecting, 0));
            steps[1].step.Should().Be(ProgressStep.Processed);
            steps[2].Should().Be((ProgressStep.Formatted, 100));
        }

        [Fact]
        public async Task GetDayFileDataAsync_WhenAlreadyRunning_ReturnsNullImmediately()
        {
            SetupHttpClient("");

            var service = Service();
            var task1 = service.GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 15), null, StatsFormat.CSV);

            var result2 = await service.GetDayFileDataAsync(TestContext(), false, new DateTime(2024, 1, 16), null, StatsFormat.CSV);

            result2.Should().BeNull();

            FireProgressEvent(AcdProgressStep.ERROR);
            await task1;
        }

        #endregion

        #region GetDaysFileDataAsync

        [Fact]
        public async Task GetDaysFileDataAsync_WithCsv_SendsGetWithCsvFormatAndAsyncParam()
        {
            SetupHttpClient("");

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            var task = Service().GetDaysFileDataAsync(TestContext(), false, range, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.ERROR);
            await task;

            AssertRequest().Method(HttpMethod.Get).Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/days/data" +
                "?begindate=2024-01-01%2000%3A00&enddate=2024-01-31%2000%3A00&format=csv&async=true");
        }

        [Fact]
        public async Task GetDaysFileDataAsync_WithExcel_SendsGetWithXlsFormatAndAsyncParam()
        {
            SetupHttpClient("");

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            var task = Service().GetDaysFileDataAsync(TestContext(), false, range, StatsFormat.EXCEL);
            FireProgressEvent(AcdProgressStep.ERROR);
            await task;

            AssertRequest().Method(HttpMethod.Get).Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/days/data" +
                "?begindate=2024-01-01%2000%3A00&enddate=2024-01-31%2000%3A00&format=xls&async=true");
        }

        [Fact]
        public async Task GetDaysFileDataAsync_WithShortHeader_AppendsShortHeaderParam()
        {
            SetupHttpClient("");

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            var task = Service().GetDaysFileDataAsync(TestContext(), true, range, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.ERROR);
            await task;

            AssertRequest().Uri(
                "/api/acdstatistics/scope/myId/ctx/ctx1/days/data" +
                "?begindate=2024-01-01%2000%3A00&enddate=2024-01-31%2000%3A00&format=csv&shortHeader=true&async=true");
        }

        [Fact]
        public async Task GetDaysFileDataAsync_OnHttpError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            var result = await Service().GetDaysFileDataAsync(TestContext(), false, range, StatsFormat.CSV);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDaysFileDataAsync_WhenErrorEventFires_ReturnsNull()
        {
            SetupHttpClient("");

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            var task = Service().GetDaysFileDataAsync(TestContext(), false, range, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.ERROR);

            var result = await task;

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDaysFileDataAsync_WhenFormattedEventFires_DownloadsFromFullResPath()
        {
            SetupHttpClient(
                ("", HttpStatusCode.OK),
                ("", HttpStatusCode.OK)
            );

            var range = new DateRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 31));
            var task = Service().GetDaysFileDataAsync(TestContext(), false, range, StatsFormat.CSV);
            FireProgressEvent(AcdProgressStep.FORMATED, fullResPath: "https://fake-o2g/api/stats/file.csv");
            await task;

            AssertRequest(1).Uri("/api/stats/file.csv");
        }

        #endregion

        #region CancelRequestAsync

        [Fact]
        public async Task CancelRequestAsync_SendsDeleteToDataRequestEndpoint()
        {
            SetupHttpClient("{}");

            await Service().CancelRequestAsync(TestContext());

            AssertRequest().Method(HttpMethod.Delete)
                .Uri("/api/acdstatistics/scope/myId/data/request");
        }

        [Fact]
        public async Task CancelRequestAsync_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("{}");

            var result = await Service().CancelRequestAsync(TestContext());

            result.Should().BeTrue();
        }

        #endregion

        #region CreateRecurrentScheduledReportAsync

        [Fact]
        public async Task CreateRecurrentScheduledReportAsync_SendsPostToScheduleEndpoint()
        {
            SetupHttpClient("""{"id":"report1"}""");

            await Service().CreateRecurrentScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                Recurrence.Daily(),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            AssertRequest().Method(HttpMethod.Post)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule");
        }

        [Fact]
        public async Task CreateRecurrentScheduledReportAsync_BodyContainsScheduleFields()
        {
            SetupHttpClient("""{"id":"report1"}""");

            await Service().CreateRecurrentScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                Recurrence.Daily(),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.name", "report1");
                j.AssertValue("$.obsPeriod.periodType", "currentDay");
                j.AssertValue("$.frequency.periodicity", "daily");
                j.AssertValue("$.fileType", "csv");
            });
        }

        [Fact]
        public async Task CreateRecurrentScheduledReportAsync_WeeklyRecurrence_BodyContainsDaysInWeek()
        {
            SetupHttpClient("""{"id":"report1"}""");

            await Service().CreateRecurrentScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                Recurrence.Weekly(DayOfWeek.Monday, DayOfWeek.Wednesday),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.frequency.periodicity", "weekly");
                j.AssertArrayContains("$.frequency.daysInWeek",
                    new List<object> { "monday", "wednesday" });
            });
        }

        [Fact]
        public async Task CreateRecurrentScheduledReportAsync_MonthlyRecurrence_BodyContainsDayInMonth()
        {
            SetupHttpClient("""{"id":"report1"}""");

            await Service().CreateRecurrentScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                Recurrence.Monthly(15),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.frequency.periodicity", "monthly");
                j.AssertValue("$.frequency.dayInMonth", 15);
            });
        }

        [Fact]
        public async Task CreateRecurrentScheduledReportAsync_OnSuccess_ReturnsReportWithIdFromResponse()
        {
            SetupHttpClient("""{"id":"report1"}""");

            var report = await Service().CreateRecurrentScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                Recurrence.Daily(),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            report.Should().NotBeNull();
            report.Id.Should().Be("report1");
            report.Once.Should().BeFalse();
            report.Format.Should().Be(StatsFormat.CSV);
        }

        [Fact]
        public async Task CreateRecurrentScheduledReportAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var report = await Service().CreateRecurrentScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                Recurrence.Daily(),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            report.Should().BeNull();
        }

        #endregion

        #region CreateOneTimeScheduledReportAsync

        [Fact]
        public async Task CreateOneTimeScheduledReportAsync_SendsPostToScheduleEndpoint()
        {
            SetupHttpClient("""{"id":"report1"}""");

            await Service().CreateOneTimeScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            AssertRequest().Method(HttpMethod.Post)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule");
        }

        [Fact]
        public async Task CreateOneTimeScheduledReportAsync_BodyHasOnceFrequency()
        {
            SetupHttpClient("""{"id":"report1"}""");

            await Service().CreateOneTimeScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.frequency.periodicity", "once");
            });
        }

        [Fact]
        public async Task CreateOneTimeScheduledReportAsync_OnSuccess_ReturnedReportHasOnceTrue()
        {
            SetupHttpClient("""{"id":"report1"}""");

            var report = await Service().CreateOneTimeScheduledReportAsync(
                TestContext(), "report1", null,
                ReportObservationPeriod.OnCurrentDay(),
                StatsFormat.CSV,
                new List<string> { "user@example.com" });

            report.Should().NotBeNull();
            report.Once.Should().BeTrue();
        }

        #endregion

        #region GetScheduledReportsAsync

        [Fact]
        public async Task GetScheduledReportsAsync_SendsGetToScheduleEndpoint()
        {
            SetupHttpClient("""{"schedules":[]}""");

            await Service().GetScheduledReportsAsync(TestContext());

            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule");
        }

        [Fact]
        public async Task GetScheduledReportsAsync_MapsResponseReports()
        {
            SetupHttpClient("""
                {
                    "schedules": [
                        {
                            "name": "report1",
                            "description": "My report",
                            "obsPeriod": { "periodType": "currentDay" },
                            "frequency": { "periodicity": "daily" },
                            "recipients": ["user@example.com"],
                            "state": "Not_executed",
                            "enable": true,
                            "fileType": "csv",
                            "shortHeader": false
                        }
                    ]
                }
                """);

            var reports = await Service().GetScheduledReportsAsync(TestContext());

            reports.Should().HaveCount(1);
            reports[0].Id.Should().Be("report1");
            reports[0].Description.Should().Be("My report");
            reports[0].Enabled.Should().BeTrue();
            reports[0].Format.Should().Be(StatsFormat.CSV);
            reports[0].State.Should().Be(ScheduledReportState.NotExecuted);
            reports[0].ObservationPeriod.PeriodType.Should().Be(ReportObservationPeriodType.CurrentDay);
            reports[0].Recurrence.Type.Should().Be(RecurrenceType.DAILY);
        }

        [Fact]
        public async Task GetScheduledReportsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetScheduledReportsAsync(TestContext());

            result.Should().BeNull();
        }

        #endregion

        #region GetScheduledReportAsync

        [Fact]
        public async Task GetScheduledReportAsync_SendsGetToScheduleIdEndpoint()
        {
            SetupHttpClient("""
                {
                    "name": "report1",
                    "obsPeriod": { "periodType": "currentDay" },
                    "frequency": { "periodicity": "once" },
                    "recipients": ["user@example.com"],
                    "state": "Executed",
                    "enable": false,
                    "fileType": "xls",
                    "shortHeader": false
                }
                """);

            await Service().GetScheduledReportAsync(TestContext(), "report1");

            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule/report1");
        }

        [Fact]
        public async Task GetScheduledReportAsync_OnceReport_ReturnedReportHasOnceTrue()
        {
            SetupHttpClient("""
                {
                    "name": "report1",
                    "obsPeriod": { "periodType": "currentDay" },
                    "frequency": { "periodicity": "once" },
                    "recipients": [],
                    "state": "Executed",
                    "enable": false,
                    "fileType": "csv",
                    "shortHeader": false
                }
                """);

            var report = await Service().GetScheduledReportAsync(TestContext(), "report1");

            report.Should().NotBeNull();
            report.Once.Should().BeTrue();
            report.State.Should().Be(ScheduledReportState.Executed);
            report.Format.Should().Be(StatsFormat.CSV);
        }

        [Fact]
        public async Task GetScheduledReportAsync_WeeklyReport_RecurrenceDaysAreMapped()
        {
            SetupHttpClient("""
                {
                    "name": "report1",
                    "obsPeriod": { "periodType": "currentDay" },
                    "frequency": {
                        "periodicity": "weekly",
                        "daysInWeek": ["monday", "friday"]
                    },
                    "recipients": [],
                    "state": "Not_executed",
                    "enable": true,
                    "fileType": "csv",
                    "shortHeader": false
                }
                """);

            var report = await Service().GetScheduledReportAsync(TestContext(), "report1");

            report.Recurrence.Type.Should().Be(RecurrenceType.WEEKLY);
            report.Recurrence.DaysInWeek.Should().BeEquivalentTo(
                new[] { DayOfWeek.Monday, DayOfWeek.Friday });
        }

        #endregion

        #region DeleteScheduledReportAsync

        [Fact]
        public async Task DeleteScheduledReportAsync_SendsDeleteToScheduleIdEndpoint()
        {
            SetupHttpClient("{}");

            await Service().DeleteScheduledReportAsync(TestReport());

            AssertRequest().Method(HttpMethod.Delete)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule/report1");
        }

        [Fact]
        public async Task DeleteScheduledReportAsync_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("{}");

            var result = await Service().DeleteScheduledReportAsync(TestReport());

            result.Should().BeTrue();
        }

        #endregion

        #region SetScheduledReportEnabledAsync

        [Fact]
        public async Task SetScheduledReportEnabledAsync_Enable_SendsPostWithEnableTrueParam()
        {
            SetupHttpClient("{}");

            await Service().SetScheduledReportEnabledAsync(TestReport(), true);

            AssertRequest().Method(HttpMethod.Post)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule/report1/enable?enable=true");
        }

        [Fact]
        public async Task SetScheduledReportEnabledAsync_Disable_SendsPostWithEnableFalseParam()
        {
            SetupHttpClient("{}");

            await Service().SetScheduledReportEnabledAsync(TestReport(), false);

            AssertRequest()
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule/report1/enable?enable=false");
        }

        [Fact]
        public async Task SetScheduledReportEnabledAsync_HasNoBody()
        {
            SetupHttpClient("{}");

            await Service().SetScheduledReportEnabledAsync(TestReport(), true);

            AssertRequest().NoBody();
        }

        #endregion

        #region UpdateScheduledReportAsync

        [Fact]
        public async Task UpdateScheduledReportAsync_SendsPutToScheduleIdEndpoint()
        {
            SetupHttpClient("{}");

            await Service().UpdateScheduledReportAsync(TestReport());

            AssertRequest().Method(HttpMethod.Put)
                .Uri("/api/acdstatistics/scope/myId/ctx/ctx1/schedule/report1");
        }

        [Fact]
        public async Task UpdateScheduledReportAsync_BodyContainsScheduleFields()
        {
            SetupHttpClient("{}");

            await Service().UpdateScheduledReportAsync(TestReport());

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.name", "report1");
                j.AssertValue("$.obsPeriod.periodType", "currentDay");
                j.AssertValue("$.frequency.periodicity", "daily");
                j.AssertValue("$.fileType", "csv");
            });
        }

        [Fact]
        public async Task UpdateScheduledReportAsync_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("{}");

            var result = await Service().UpdateScheduledReportAsync(TestReport());

            result.Should().BeTrue();
        }

        #endregion
    }
}
