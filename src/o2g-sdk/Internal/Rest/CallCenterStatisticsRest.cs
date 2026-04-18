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

using o2g.Events;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types.AnalyticsNS;
using o2g.Types.CallCenterStatisticsNS;
using o2g.Types.CallCenterStatisticsNS.Scheduled;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    // ---------------------------------------------------------------------------
    // Internal JSON types for request serialization
    // ---------------------------------------------------------------------------

    class SupervisedJson
    {
        public string Number { get; init; }
    }

    class SupervisorJson
    {
        public string Identifier { get; init; }
        public Language Language { get; init; }
        public string Timezone { get; init; }
    }

    class CreateScopeRequestJson
    {
        public SupervisorJson Supervisor { get; init; }
        public List<SupervisedJson> Agents { get; init; }
    }

    class AgentFilterJson
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Numbers { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<AgentAttributes> AgentAttributes { get; init; }
        [JsonPropertyName("pilotAttributes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<AgentByPilotAttributes> PilotAttributes { get; init; }
    }

    class PilotFilterJson
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Numbers { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<PilotAttributes> Attributes { get; init; }
    }

    class PilotAbandonedCallFilterJson
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Numbers { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<PilotAbandonedCallsAttributes> Attributes { get; init; }
    }

    class FilterJson
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AgentFilterJson AgentFilter { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PilotFilterJson PilotFilter { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PilotAbandonedCallFilterJson PilotAbandonedCallFilter { get; init; }
    }

    class CreateContextRequestJson
    {
        public string SupervisorId { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Label { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; init; }
        public FilterJson Filter { get; init; }
    }

    class RespIdJson
    {
        public string Id { get; init; }
    }

    // ---------------------------------------------------------------------------
    // Internal JSON types for response deserialization
    // ---------------------------------------------------------------------------

    class ContextJson
    {
        [JsonPropertyName("ctxId")]
        public string Id { get; init; }

        [JsonPropertyName("supervisorId")]
        public string RequesterId { get; init; }

        public string Label { get; init; }
        public string Description { get; init; }
        public bool IsScheduled { get; init; }
        public bool ShortHeader { get; init; }
    }

    class ContextsJson
    {
        public List<ContextJson> Contexts { get; init; }
    }

    class SupervisorInfoJson
    {
        public string Identifier { get; init; }
        public Language Language { get; init; }
        public string Timezone { get; init; }
    }

    class FrequencyJson
    {
        public string Periodicity { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> DaysInWeek { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? DayInMonth { get; init; }
    }

    class ScheduledPeriodJson
    {
        public ReportObservationPeriodType PeriodType { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LastNb { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string BeginDate { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EndDate { get; init; }
    }

    class ScheduleJson
    {
        [JsonPropertyName("name")]
        public string Id { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; init; }

        [JsonPropertyName("obsPeriod")]
        public ScheduledPeriodJson ObsPeriod { get; init; }

        [JsonPropertyName("frequency")]
        public FrequencyJson Frequency { get; init; }

        public List<string> Recipients { get; init; }

        public ScheduledReportState State { get; init; }

        [JsonPropertyName("enable")]
        public bool Enabled { get; init; }

        [JsonPropertyName("lastExecDate")]
        public string LastExecDate { get; init; }

        [JsonPropertyName("fileType")]
        public StatsFormat Format { get; init; }

        public bool ShortHeader { get; init; }
    }

    class SchedulesJson
    {
        public List<ScheduleJson> Schedules { get; init; }
    }

    class CreateScheduleRequestJson
    {
        [JsonPropertyName("name")]
        public string Id { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; init; }

        [JsonPropertyName("obsPeriod")]
        public ScheduledPeriodJson ObsPeriod { get; init; }

        [JsonPropertyName("frequency")]
        public FrequencyJson Frequency { get; init; }

        public List<string> Recipients { get; init; }

        [JsonPropertyName("fileType")]
        public StatsFormat FileType { get; init; }
    }

    // ---------------------------------------------------------------------------
    // REST service implementation
    // ---------------------------------------------------------------------------

    internal class CallCenterStatisticsRest : AbstractRESTService, ICallCenterStatistics
    {
#pragma warning disable CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0649

        private int _subscribed = 0;
        private int _running = 0;
        private volatile StatAsyncRequest _pendingRequest;

        private sealed class StatAsyncRequest
        {
            public TaskCompletionSource<string> Tcs { get; } = new();
            public string Directory { get; }
            public StatsFormat Format { get; }
            public Action<ProgressStep, int> ProgressCallback { get; }
            public int NbTotObjects { get; set; }

            public StatAsyncRequest(string directory, StatsFormat format, Action<ProgressStep, int> progressCallback)
            {
                Directory = directory;
                Format = format;
                ProgressCallback = progressCallback;
            }
        }

        private void EnsureSubscribed()
        {
            if (Interlocked.CompareExchange(ref _subscribed, 1, 0) == 0)
            {
                _eventHandlers.AcdStatsProgress += HandleAcdStatsProgress;
            }
        }

        private void HandleAcdStatsProgress(object sender, O2GEventArgs<OnAcdStatsProgressEvent> e)
        {
            var ev = e.Event;
            var req = _pendingRequest;
            if (req == null) return;

            switch (ev.Step)
            {
                case AcdProgressStep.COLLECT:
                    req.NbTotObjects = ev.NbTotObjects;
                    req.ProgressCallback?.Invoke(ProgressStep.Collecting, 0);
                    break;

                case AcdProgressStep.PROCESSED:
                    int progress = req.NbTotObjects > 0
                        ? (ev.NbProcessedObjects * 100) / req.NbTotObjects
                        : 0;
                    req.ProgressCallback?.Invoke(ProgressStep.Processed, progress);
                    break;

                case AcdProgressStep.FORMATED:
                    req.ProgressCallback?.Invoke(ProgressStep.Formatted, 100);
                    string url = req.Format == StatsFormat.EXCEL ? ev.XlsFullResPath : ev.FullResPath;
                    _ = DownloadAndComplete(req, url);
                    break;

                case AcdProgressStep.ERROR:
                case AcdProgressStep.CANCELLED:
                    _pendingRequest = null;
                    Interlocked.Exchange(ref _running, 0);
                    req.Tcs.TrySetResult(null);
                    break;
            }
        }

        private async Task DownloadAndComplete(StatAsyncRequest req, string url)
        {
            try
            {
                if (url == null)
                {
                    req.Tcs.TrySetResult(null);
                    return;
                }
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(url));
                if (!response.IsSuccessStatusCode)
                {
                    req.Tcs.TrySetResult(null);
                    return;
                }
                byte[] zipBytes = await response.Content.ReadAsByteArrayAsync();
                string filePath = await ExtractZipToDirectory(zipBytes, req.Directory);
                req.Tcs.TrySetResult(filePath);
            }
            catch (Exception)
            {
                req.Tcs.TrySetResult(null);
            }
            finally
            {
                _pendingRequest = null;
                Interlocked.Exchange(ref _running, 0);
            }
        }

        private static async Task<string> ExtractZipToDirectory(byte[] zipBytes, string directory)
        {
            string targetDir = directory ?? FileUtil.GetSystemPath(SystemFolder.Downloads);
            System.IO.Directory.CreateDirectory(targetDir);

            using var archive = new ZipArchive(new MemoryStream(zipBytes), ZipArchiveMode.Read);
            ZipArchiveEntry entry = archive.Entries.Count > 0 ? archive.Entries[0] : null;
            if (entry == null) return null;

            string filePath = WithTimestamp(targetDir, entry.Name);

            using var entryStream = entry.Open();
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await entryStream.CopyToAsync(fileStream);

            return filePath;
        }

        private static string WithTimestamp(string directory, string fileName)
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(directory, $"{baseName}_{timestamp}{ext}");
        }

        public CallCenterStatisticsRest(Uri uri) : base(uri) { }

        // Format date as "yyyy-MM-dd HH:mm"
        private static string FormatDateTime(DateTime date) =>
            date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        // Format date as "yyyy-MM-dd"
        private static string FormatDate(DateTime date) =>
            date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        // Convert System.DayOfWeek to lowercase string for JSON
        private static string DayOfWeekToJson(DayOfWeek day) =>
            day.ToString().ToLowerInvariant();

        private static FilterJson BuildFilterJson(StatsFilter filter)
        {
            if (filter is AgentFilter af)
            {
                return new FilterJson
                {
                    AgentFilter = new AgentFilterJson
                    {
                        Numbers = af.Numbers.Count > 0 ? af.Numbers : null,
                        AgentAttributes = af.AgentAttributes.Count > 0 ? af.AgentAttributes.ToList() : null,
                        PilotAttributes = af.ByPilotAttributes.Count > 0 ? af.ByPilotAttributes.ToList() : null
                    }
                };
            }
            else if (filter is PilotFilter pf)
            {
                return new FilterJson
                {
                    PilotFilter = new PilotFilterJson
                    {
                        Numbers = pf.Numbers.Count > 0 ? pf.Numbers : null,
                        Attributes = pf.PilotAttributes.Count > 0 ? pf.PilotAttributes.ToList() : null
                    }
                };
            }
            else if (filter is PilotAbandonedCallsFilter acf)
            {
                return new FilterJson
                {
                    PilotAbandonedCallFilter = new PilotAbandonedCallFilterJson
                    {
                        Numbers = acf.Numbers.Count > 0 ? acf.Numbers : null,
                        Attributes = acf.Attributes.Count > 0 ? acf.Attributes.ToList() : null
                    }
                };
            }
            return new FilterJson();
        }

        private static ScheduledPeriodJson ToScheduledPeriodJson(ReportObservationPeriod period)
        {
            return new ScheduledPeriodJson
            {
                PeriodType = period.PeriodType,
                LastNb = period.LastUnits > 0 ? (int?)period.LastUnits : null,
                BeginDate = period.BeginDate.HasValue ? period.BeginDate.Value.ToString("o") : null,
                EndDate = period.EndDate.HasValue ? period.EndDate.Value.ToString("o") : null
            };
        }

        private static FrequencyJson ToFrequencyJson(Recurrence recurrence)
        {
            if (recurrence == null)
                return new FrequencyJson { Periodicity = "once" };

            switch (recurrence.Type)
            {
                case RecurrenceType.DAILY:
                    return new FrequencyJson { Periodicity = "daily" };

                case RecurrenceType.WEEKLY:
                    return new FrequencyJson
                    {
                        Periodicity = "weekly",
                        DaysInWeek = recurrence.DaysInWeek?.Select(DayOfWeekToJson).ToList()
                    };

                case RecurrenceType.MONTHLY:
                    return new FrequencyJson
                    {
                        Periodicity = "monthly",
                        DayInMonth = recurrence.DayInMonth > 0 ? (int?)recurrence.DayInMonth : null
                    };

                default:
                    return new FrequencyJson { Periodicity = "once" };
            }
        }

        private static ReportObservationPeriod FromScheduledPeriodJson(ScheduledPeriodJson json)
        {
            if (json == null) return null;
            switch (json.PeriodType)
            {
                case ReportObservationPeriodType.CurrentDay: return ReportObservationPeriod.OnCurrentDay();
                case ReportObservationPeriodType.CurrentWeek: return ReportObservationPeriod.OnCurrentWeek();
                case ReportObservationPeriodType.CurrentMonth: return ReportObservationPeriod.OnCurrentMonth();
                case ReportObservationPeriodType.LastDays: return ReportObservationPeriod.OnLastDays(json.LastNb ?? 1);
                case ReportObservationPeriodType.LastWeeks: return ReportObservationPeriod.OnLastWeeks(json.LastNb ?? 1);
                case ReportObservationPeriodType.LastMonth: return ReportObservationPeriod.OnLastMonth();
                case ReportObservationPeriodType.FromDateToDate:
                    if (json.BeginDate != null && json.EndDate != null)
                    {
                        DateTime begin = DateTime.Parse(json.BeginDate, CultureInfo.InvariantCulture);
                        DateTime end = DateTime.Parse(json.EndDate, CultureInfo.InvariantCulture);
                        int days = (int)(end - begin).TotalDays;
                        return ReportObservationPeriod.FromDate(begin, Math.Max(1, days));
                    }
                    return ReportObservationPeriod.OnCurrentDay();
                default:
                    return ReportObservationPeriod.OnCurrentDay();
            }
        }

        private static Recurrence FromFrequencyJson(FrequencyJson json)
        {
            if (json == null || json.Periodicity == "once") return null;
            switch (json.Periodicity)
            {
                case "daily": return Recurrence.Daily();
                case "weekly":
                    if (json.DaysInWeek != null && json.DaysInWeek.Count > 0)
                    {
                        DayOfWeek[] days = json.DaysInWeek
                            .Select(d => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d, ignoreCase: true))
                            .ToArray();
                        return Recurrence.Weekly(days);
                    }
                    return Recurrence.Daily();
                case "monthly":
                    return Recurrence.Monthly(json.DayInMonth ?? 1);
                default:
                    return null;
            }
        }

        private static StatsContext ToStatsContext(ContextJson json)
        {
            return new StatsContext
            {
                Id = json.Id,
                RequesterId = json.RequesterId,
                Label = json.Label,
                Description = json.Description,
                IsScheduled = json.IsScheduled,
                ShortHeader = json.ShortHeader
            };
        }

        private static ScheduledReport ToScheduledReport(ScheduleJson json, StatsContext context)
        {
            DateTime? lastExec = null;
            if (!string.IsNullOrEmpty(json.LastExecDate))
            {
                if (DateTime.TryParse(json.LastExecDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    lastExec = dt;
            }

            return new ScheduledReport
            {
                Id = json.Id,
                Description = json.Description,
                ObservationPeriod = FromScheduledPeriodJson(json.ObsPeriod),
                Recurrence = FromFrequencyJson(json.Frequency),
                Format = json.Format,
                Recipients = json.Recipients,
                State = json.State,
                Enabled = json.Enabled,
                ShortHeader = json.ShortHeader,
                LastExecutionDate = lastExec,
                Context = context
            };
        }

        private StringContent Serialize<T>(T obj)
        {
            string json = JsonSerializer.Serialize(obj, serializeOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        // -----------------------------------------------------------------------
        // Requester (scope) management
        // -----------------------------------------------------------------------

        public async Task<StatsRequester> CreateRequesterAsync(string id, Language language, string timezone, List<string> agents)
        {
            Uri uriPost = uri.Append("scope");

            var req = new CreateScopeRequestJson
            {
                Supervisor = new SupervisorJson
                {
                    Identifier = AssertUtil.NotNullOrEmpty(id, "id"),
                    Language = language,
                    Timezone = AssertUtil.NotNullOrEmpty(timezone, "timezone")
                },
                Agents = AssertUtil.NotNullList(agents, "agents")
                    .Select(n => new SupervisedJson { Number = n })
                    .ToList()
            };

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, Serialize(req));
            if (!await IsSucceeded(response)) return null;

            return new StatsRequester { Id = id, Language = language, Timezone = timezone };
        }

        public async Task<bool> DeleteRequesterAsync(StatsRequester requester)
        {
            Uri uriDelete = uri.Append("scope", Uri.EscapeDataString(AssertUtil.NotNull(requester, "requester").Id));
            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<StatsRequester> GetRequesterAsync(string id)
        {
            Uri uriGet = uri.Append("scope", Uri.EscapeDataString(AssertUtil.NotNullOrEmpty(id, "id")));
            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            SupervisorInfoJson json = await GetResult<SupervisorInfoJson>(response);
            if (json == null) return null;
            return new StatsRequester { Id = json.Identifier, Language = json.Language, Timezone = json.Timezone };
        }

        // -----------------------------------------------------------------------
        // Context management
        // -----------------------------------------------------------------------

        public async Task<StatsContext> CreateContextAsync(StatsRequester requester, string label, string description, StatsFilter filter)
        {
            Uri uriPost = uri.Append("scope", Uri.EscapeDataString(AssertUtil.NotNull(requester, "requester").Id), "ctx");

            var req = new CreateContextRequestJson
            {
                SupervisorId = requester.Id,
                Label = label,
                Description = description,
                Filter = BuildFilterJson(AssertUtil.NotNull(filter, "filter"))
            };

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, Serialize(req));
            RespIdJson respId = await GetResult<RespIdJson>(response);
            if (respId == null) return null;

            return new StatsContext
            {
                Id = respId.Id,
                RequesterId = requester.Id,
                Label = label,
                Description = description,
                Filter = filter
            };
        }

        public async Task<List<StatsContext>> GetContextsAsync(StatsRequester requester)
        {
            Uri uriGet = uri.Append("scope", Uri.EscapeDataString(AssertUtil.NotNull(requester, "requester").Id), "ctx");
            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ContextsJson json = await GetResult<ContextsJson>(response);
            if (json?.Contexts == null) return null;
            return json.Contexts.Select(ToStatsContext).ToList();
        }

        public async Task<bool> DeleteContextsAsync(StatsRequester requester)
        {
            Uri uriDelete = uri.Append("scope", Uri.EscapeDataString(AssertUtil.NotNull(requester, "requester").Id), "ctx");
            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<StatsContext> GetContextAsync(StatsRequester requester, string contextId)
        {
            Uri uriGet = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(requester, "requester").Id),
                "ctx",
                AssertUtil.NotNullOrEmpty(contextId, "contextId"));

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ContextJson json = await GetResult<ContextJson>(response);
            if (json == null) return null;
            return ToStatsContext(json);
        }

        public async Task<bool> DeleteContextAsync(StatsContext context)
        {
            Uri uriDelete = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id);

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        // -----------------------------------------------------------------------
        // Data retrieval
        // -----------------------------------------------------------------------

        public async Task<StatisticsData> GetDayDataAsync(StatsContext context, bool shortHeader, DateTime? date = null, TimeInterval? timeInterval = null)
        {
            Uri uriGet = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "oneday/data");

            uriGet = uriGet.AppendQuery("date", FormatDate(date ?? DateTime.Today));

            if (timeInterval.HasValue)
                uriGet = uriGet.AppendQuery("slotType", JsonSerializer.Serialize(timeInterval.Value, serializeOptions).Trim('"'));

            uriGet = uriGet.AppendQuery("format", "json");

            if (shortHeader)
                uriGet = uriGet.AppendQuery("shortHeader", "true");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<StatisticsData>(response);
        }

        public async Task<StatisticsData> GetDaysDataAsync(StatsContext context, bool shortHeader, DateRange range)
        {
            AssertUtil.NotNull(range, "range");

            Uri uriGet = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "days/data");

            uriGet = uriGet.AppendQuery("begindate", FormatDateTime(range.from));
            uriGet = uriGet.AppendQuery("enddate", FormatDateTime(range.to));
            uriGet = uriGet.AppendQuery("format", "json");

            if (shortHeader)
                uriGet = uriGet.AppendQuery("shortHeader", "true");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<StatisticsData>(response);
        }

        private static string FormatToQueryValue(StatsFormat format) =>
            format == StatsFormat.CSV ? "csv" : "xls";

        public async Task<string> GetDayFileDataAsync(StatsContext context, bool shortHeader, DateTime date,
            TimeInterval? timeInterval, StatsFormat format, string directory = null,
            Action<ProgressStep, int> progressCallback = null)
        {
            if (Interlocked.CompareExchange(ref _running, 1, 0) != 0)
                return null;

            EnsureSubscribed();
            _pendingRequest = new StatAsyncRequest(directory, format, progressCallback);

            Uri uriGet = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "oneday/data");

            uriGet = uriGet.AppendQuery("date", FormatDate(date));

            if (timeInterval.HasValue)
                uriGet = uriGet.AppendQuery("slotType", JsonSerializer.Serialize(timeInterval.Value, serializeOptions).Trim('"'));

            uriGet = uriGet.AppendQuery("format", FormatToQueryValue(format));

            if (shortHeader)
                uriGet = uriGet.AppendQuery("shortHeader", "true");

            uriGet = uriGet.AppendQuery("async", "true");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            if (!response.IsSuccessStatusCode)
            {
                _pendingRequest = null;
                Interlocked.Exchange(ref _running, 0);
                return null;
            }

            return await _pendingRequest.Tcs.Task;
        }

        public async Task<string> GetDaysFileDataAsync(StatsContext context, bool shortHeader, DateRange range,
            StatsFormat format, string directory = null,
            Action<ProgressStep, int> progressCallback = null)
        {
            AssertUtil.NotNull(range, "range");

            if (Interlocked.CompareExchange(ref _running, 1, 0) != 0)
                return null;

            EnsureSubscribed();
            _pendingRequest = new StatAsyncRequest(directory, format, progressCallback);

            Uri uriGet = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "days/data");

            uriGet = uriGet.AppendQuery("begindate", FormatDateTime(range.from));
            uriGet = uriGet.AppendQuery("enddate", FormatDateTime(range.to));
            uriGet = uriGet.AppendQuery("format", FormatToQueryValue(format));

            if (shortHeader)
                uriGet = uriGet.AppendQuery("shortHeader", "true");

            uriGet = uriGet.AppendQuery("async", "true");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            if (!response.IsSuccessStatusCode)
            {
                _pendingRequest = null;
                Interlocked.Exchange(ref _running, 0);
                return null;
            }

            return await _pendingRequest.Tcs.Task;
        }

        public async Task<bool> CancelRequestAsync(StatsContext context)
        {
            Uri uriDelete = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "data/request");

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        // -----------------------------------------------------------------------
        // Scheduled reports
        // -----------------------------------------------------------------------

        public async Task<ScheduledReport> CreateRecurrentScheduledReportAsync(
            StatsContext context,
            string id,
            string description,
            ReportObservationPeriod observationPeriod,
            Recurrence recurrence,
            StatsFormat format,
            List<string> recipients)
        {
            Uri uriPost = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "schedule");

            AssertUtil.NotNullList(recipients, "recipients");

            var req = new CreateScheduleRequestJson
            {
                Id = AssertUtil.NotNullOrEmpty(id, "id"),
                Description = description,
                ObsPeriod = ToScheduledPeriodJson(AssertUtil.NotNull(observationPeriod, "observationPeriod")),
                Frequency = ToFrequencyJson(AssertUtil.NotNull(recurrence, "recurrence")),
                Recipients = recipients,
                FileType = format
            };

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, Serialize(req));
            RespIdJson respId = await GetResult<RespIdJson>(response);
            if (respId == null) return null;

            return new ScheduledReport
            {
                Id = respId.Id,
                Description = description,
                ObservationPeriod = observationPeriod,
                Recurrence = recurrence,
                Format = format,
                Recipients = recipients,
                Context = context
            };
        }

        public async Task<ScheduledReport> CreateOneTimeScheduledReportAsync(
            StatsContext context,
            string id,
            string description,
            ReportObservationPeriod observationPeriod,
            StatsFormat format,
            List<string> recipients)
        {
            Uri uriPost = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "schedule");

            AssertUtil.NotNullList(recipients, "recipients");

            var req = new CreateScheduleRequestJson
            {
                Id = AssertUtil.NotNullOrEmpty(id, "id"),
                Description = description,
                ObsPeriod = ToScheduledPeriodJson(AssertUtil.NotNull(observationPeriod, "observationPeriod")),
                Frequency = new FrequencyJson { Periodicity = "once" },
                Recipients = recipients,
                FileType = format
            };

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, Serialize(req));
            RespIdJson respId = await GetResult<RespIdJson>(response);
            if (respId == null) return null;

            return new ScheduledReport
            {
                Id = respId.Id,
                Description = description,
                ObservationPeriod = observationPeriod,
                Recurrence = null,
                Format = format,
                Recipients = recipients,
                Context = context
            };
        }

        public async Task<List<ScheduledReport>> GetScheduledReportsAsync(StatsContext context)
        {
            Uri uriGet = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "schedule");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            SchedulesJson json = await GetResult<SchedulesJson>(response);
            if (json?.Schedules == null) return null;
            return json.Schedules.Select(s => ToScheduledReport(s, context)).ToList();
        }

        public async Task<ScheduledReport> GetScheduledReportAsync(StatsContext context, string scheduleReportId)
        {
            Uri uriGet = uri.Append(
                "scope",
                Uri.EscapeDataString(AssertUtil.NotNull(context, "context").RequesterId),
                "ctx",
                context.Id,
                "schedule",
                Uri.EscapeDataString(AssertUtil.NotNullOrEmpty(scheduleReportId, "scheduleReportId")));

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ScheduleJson json = await GetResult<ScheduleJson>(response);
            if (json == null) return null;
            return ToScheduledReport(json, context);
        }

        public async Task<bool> DeleteScheduledReportAsync(ScheduledReport report)
        {
            StatsContext context = AssertUtil.NotNull(report, "report").Context;
            Uri uriDelete = uri.Append(
                "scope",
                Uri.EscapeDataString(context.RequesterId),
                "ctx",
                context.Id,
                "schedule",
                Uri.EscapeDataString(report.Id));

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> SetScheduledReportEnabledAsync(ScheduledReport report, bool enabled)
        {
            StatsContext context = AssertUtil.NotNull(report, "report").Context;
            Uri uriPost = uri.Append(
                "scope",
                Uri.EscapeDataString(context.RequesterId),
                "ctx",
                context.Id,
                "schedule",
                Uri.EscapeDataString(report.Id),
                "enable");

            uriPost = uriPost.AppendQuery("enable", enabled ? "true" : "false");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> UpdateScheduledReportAsync(ScheduledReport report)
        {
            StatsContext context = AssertUtil.NotNull(report, "report").Context;
            Uri uriPut = uri.Append(
                "scope",
                Uri.EscapeDataString(context.RequesterId),
                "ctx",
                context.Id,
                "schedule",
                Uri.EscapeDataString(report.Id));

            AssertUtil.NotNullList(report.Recipients, "report.Recipients");

            var req = new CreateScheduleRequestJson
            {
                Id = report.Id,
                Description = report.Description,
                ObsPeriod = ToScheduledPeriodJson(report.ObservationPeriod),
                Frequency = ToFrequencyJson(report.Recurrence),
                Recipients = report.Recipients,
                FileType = report.Format
            };

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, Serialize(req));
            return await IsSucceeded(response);
        }
    }
}
