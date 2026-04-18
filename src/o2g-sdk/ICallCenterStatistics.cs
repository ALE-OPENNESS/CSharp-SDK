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

using o2g.Types.AnalyticsNS;
using o2g.Types.CallCenterStatisticsNS;
using o2g.Types.CallCenterStatisticsNS.Scheduled;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace o2g
{
    /// <summary>
    /// Provides access to OmniPCX Enterprise call-center statistics for CCD agents and pilots.
    /// </summary>
    /// <remarks>
    /// This service supports two modes of data retrieval:
    /// <list type="bullet">
    ///   <item><description><b>Immediate reports</b> — statistics retrieved on demand, returned as
    ///   in-memory data or exported as CSV or Excel files.</description></item>
    ///   <item><description><b>Scheduled reports</b> — recurring statistics delivered as email
    ///   attachments to predefined recipients.</description></item>
    /// </list>
    /// <para>
    /// Statistics are accessed through a two-level hierarchy:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>A <see cref="StatsRequester"/> defines the scope of agents whose data
    ///   can be accessed.</description></item>
    ///   <item><description>A <see cref="StatsContext"/> defines the filter criteria (pilots, agents,
    ///   queues) for which statistics are collected.</description></item>
    /// </list>
    /// <para>
    /// The typical usage sequence is:
    /// </para>
    /// <list type="number">
    ///   <item><description>Create a requester with <see cref="CreateRequesterAsync"/>, specifying
    ///   the agents in scope.</description></item>
    ///   <item><description>Create a context with <see cref="CreateContextAsync"/>, specifying the
    ///   filter criteria.</description></item>
    ///   <item><description>Retrieve data with <see cref="GetDayDataAsync"/> for a single day, or
    ///   <see cref="GetDaysDataAsync"/> for a date range.</description></item>
    ///   <item><description>Delete the context and requester when done.</description></item>
    /// </list>
    /// <para>
    /// Using this service requires a <c>CONTACTCENTER_SERVICE</c> license in CAPEX mode, or a
    /// <c>40 api-tel-f</c> subscription in OPEX mode (Purple On Demand).
    /// </para>
    /// </remarks>
    public interface ICallCenterStatistics
    {
        /// <summary>
        /// Creates a new <see cref="StatsRequester"/> with the specified identifier, language,
        /// time zone, and agent scope.
        /// </summary>
        /// <remarks>
        /// The agent scope determines which agents' statistics the requester is authorized to access.
        /// Once the requester is created, a <see cref="StatsContext"/> can be built on top of it to
        /// narrow the filter criteria further.
        /// </remarks>
        /// <param name="id">The unique identifier for this requester (e.g., a supervisor ID).</param>
        /// <param name="language">The language used for report generation.</param>
        /// <param name="timezone">The time zone for the statistics (e.g. <c>"Europe/Paris"</c>).</param>
        /// <param name="agents">The list of agent directory numbers that define the scope of accessible statistics.</param>
        /// <returns>The newly created <see cref="StatsRequester"/>, or <see langword="null"/> on failure.</returns>
        /// <seealso cref="DeleteRequesterAsync"/>
        Task<StatsRequester> CreateRequesterAsync(string id, Language language, string timezone, List<string> agents);

        /// <summary>
        /// Removes the specified requester and all its associated contexts.
        /// </summary>
        /// <remarks>
        /// After this call, the requester no longer has access to any agent statistics defined
        /// under its scope.
        /// </remarks>
        /// <param name="requester">The requester to delete.</param>
        /// <returns><see langword="true"/> if the requester was successfully removed; otherwise <see langword="false"/>.</returns>
        Task<bool> DeleteRequesterAsync(StatsRequester requester);

        /// <summary>
        /// Returns the requester with the specified identifier.
        /// </summary>
        /// <remarks>
        /// The returned requester represents the scope of agents for which statistics can be
        /// accessed.
        /// </remarks>
        /// <param name="id">The unique identifier of the requester.</param>
        /// <returns>The <see cref="StatsRequester"/> corresponding to the ID, or <see langword="null"/> if not found.</returns>
        Task<StatsRequester> GetRequesterAsync(string id);

        /// <summary>
        /// Creates a new statistics context for the specified requester.
        /// </summary>
        /// <remarks>
        /// A context defines the filter criteria (pilots, agents, queues) for which call-center
        /// statistics are collected and analyzed.
        /// </remarks>
        /// <param name="requester">The requester for whom the context is created.</param>
        /// <param name="label">A short label identifying this context.</param>
        /// <param name="description">A detailed description of the context.</param>
        /// <param name="filter">The filter defining the selection criteria for the context.</param>
        /// <returns>The created <see cref="StatsContext"/>, or <see langword="null"/> on failure.</returns>
        Task<StatsContext> CreateContextAsync(StatsRequester requester, string label, string description, StatsFilter filter);

        /// <summary>
        /// Returns all statistics contexts for the specified requester.
        /// </summary>
        /// <param name="requester">The requester whose contexts are retrieved.</param>
        /// <returns>A list of <see cref="StatsContext"/> objects, or <see langword="null"/> on failure.</returns>
        /// <seealso cref="CreateContextAsync"/>
        Task<List<StatsContext>> GetContextsAsync(StatsRequester requester);

        /// <summary>
        /// Deletes all statistics contexts associated with the specified requester.
        /// </summary>
        /// <param name="requester">The requester whose contexts should be deleted.</param>
        /// <returns><see langword="true"/> if all contexts were successfully deleted; otherwise <see langword="false"/>.</returns>
        /// <seealso cref="CreateContextAsync"/>
        Task<bool> DeleteContextsAsync(StatsRequester requester);

        /// <summary>
        /// Returns the statistics context with the specified identifier.
        /// </summary>
        /// <param name="requester">The requester who owns the context.</param>
        /// <param name="contextId">The unique identifier of the context.</param>
        /// <returns>The <see cref="StatsContext"/> if found, or <see langword="null"/> if not found.</returns>
        Task<StatsContext> GetContextAsync(StatsRequester requester, string contextId);

        /// <summary>
        /// Deletes the specified statistics context.
        /// </summary>
        /// <param name="context">The context to delete.</param>
        /// <returns><see langword="true"/> if the context was successfully deleted; otherwise <see langword="false"/>.</returns>
        /// <seealso cref="CreateContextAsync"/>
        Task<bool> DeleteContextAsync(StatsContext context);

        /// <summary>
        /// Returns statistics for a single day.
        /// </summary>
        /// <remarks>
        /// Statistics are provided in time slots according to <paramref name="timeInterval"/>.
        /// When <paramref name="date"/> is <see langword="null"/>, the current day is used.
        /// When <paramref name="timeInterval"/> is <see langword="null"/>, no time-slot breakdown
        /// is applied and data is returned as a single daily aggregate.
        /// </remarks>
        /// <param name="context">The statistics context defining the scope and filters.</param>
        /// <param name="shortHeader">When <see langword="true"/>, statistics headers are condensed.</param>
        /// <param name="date">The day to retrieve; defaults to today when <see langword="null"/>.</param>
        /// <param name="timeInterval">The slot granularity (e.g., 15 or 30 minutes); <see langword="null"/> for a single daily aggregate.</param>
        /// <returns>A <see cref="StatisticsData"/> object containing the data, or <see langword="null"/> on failure.</returns>
        Task<StatisticsData> GetDayDataAsync(StatsContext context, bool shortHeader, DateTime? date = null, TimeInterval? timeInterval = null);

        /// <summary>
        /// Returns aggregated statistics for a range of days.
        /// </summary>
        /// <remarks>
        /// Multi-day reports provide one row of aggregated data per agent or pilot per day.
        /// The range can cover up to 31 consecutive days within the last 12 months and may
        /// span month boundaries.
        /// </remarks>
        /// <param name="context">The statistics context defining the scope and filters.</param>
        /// <param name="shortHeader">When <see langword="true"/>, statistics headers are condensed.</param>
        /// <param name="range">The date range to retrieve.</param>
        /// <returns>A <see cref="StatisticsData"/> object containing the aggregated data, or <see langword="null"/> on failure.</returns>
        Task<StatisticsData> GetDaysDataAsync(StatsContext context, bool shortHeader, DateRange range);

        /// <summary>
        /// Asynchronously downloads statistics for a single day as a report file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The server generates the file asynchronously and delivers the result as a ZIP archive.
        /// The single entry within the archive is extracted into <paramref name="directory"/>.
        /// The returned task completes when the extracted file has been saved to disk.
        /// </para>
        /// <para>
        /// Statistics are reported in time slots defined by <paramref name="timeInterval"/>,
        /// spanning from 00:00 until the last completed interval of the specified day.
        /// When <paramref name="timeInterval"/> is <see langword="null"/>, no time-slot breakdown
        /// is applied.
        /// </para>
        /// <para>
        /// Only one asynchronous report generation request can be active at a time. Any attempt
        /// to start another request while one is already in progress will cause the returned task
        /// to fault with an <c>InvalidOperationException</c>. Use <see cref="CancelRequestAsync"/>
        /// to abort an in-progress request.
        /// </para>
        /// </remarks>
        /// <param name="context">The statistics context defining the scope and filters.</param>
        /// <param name="shortHeader">When <see langword="true"/>, statistics headers are condensed.</param>
        /// <param name="date">The day for which to generate the report.</param>
        /// <param name="timeInterval">The slot granularity; <see langword="null"/> for a single daily aggregate.</param>
        /// <param name="format">The output file format (<see cref="StatsFormat.CSV"/> or <see cref="StatsFormat.EXCEL"/>).</param>
        /// <param name="directory">
        /// The destination directory. The extracted file is placed inside this directory.
        /// When <see langword="null"/>, the system Downloads folder is used.
        /// </param>
        /// <param name="progressCallback">
        /// An optional callback invoked as the server processes the request.
        /// The first argument is the current <see cref="ProgressStep"/>; the second is the
        /// estimated completion percentage (0–100).
        /// </param>
        /// <returns>The full path to the extracted report file, or <see langword="null"/> on failure.</returns>
        /// <seealso cref="GetDaysFileDataAsync"/>
        /// <seealso cref="CancelRequestAsync"/>
        Task<string> GetDayFileDataAsync(StatsContext context, bool shortHeader, DateTime date, TimeInterval? timeInterval, StatsFormat format, string directory = null, Action<ProgressStep, int> progressCallback = null);

        /// <summary>
        /// Asynchronously downloads statistics for a range of days as a report file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The server generates the file asynchronously and delivers the result as a ZIP archive.
        /// The single entry within the archive is extracted into <paramref name="directory"/>.
        /// The returned task completes when the extracted file has been saved to disk.
        /// </para>
        /// <para>
        /// Multi-day reports provide one row of aggregated data per agent or pilot per day.
        /// The range can cover up to 31 consecutive days within the last 12 months and may
        /// span month boundaries.
        /// </para>
        /// <para>
        /// Only one asynchronous report generation request can be active at a time. Any attempt
        /// to start another request while one is already in progress will cause the returned task
        /// to fault with an <c>InvalidOperationException</c>. Use <see cref="CancelRequestAsync"/>
        /// to abort an in-progress request.
        /// </para>
        /// </remarks>
        /// <param name="context">The statistics context defining the scope and filters.</param>
        /// <param name="shortHeader">When <see langword="true"/>, statistics headers are condensed.</param>
        /// <param name="range">The date range to retrieve.</param>
        /// <param name="format">The output file format (<see cref="StatsFormat.CSV"/> or <see cref="StatsFormat.EXCEL"/>).</param>
        /// <param name="directory">
        /// The destination directory. The extracted file is placed inside this directory.
        /// When <see langword="null"/>, the system Downloads folder is used.
        /// </param>
        /// <param name="progressCallback">
        /// An optional callback invoked as the server processes the request.
        /// The first argument is the current <see cref="ProgressStep"/>; the second is the
        /// estimated completion percentage (0–100).
        /// </param>
        /// <returns>The full path to the extracted report file, or <see langword="null"/> on failure.</returns>
        /// <seealso cref="GetDayFileDataAsync"/>
        /// <seealso cref="CancelRequestAsync"/>
        Task<string> GetDaysFileDataAsync(StatsContext context, bool shortHeader, DateRange range, StatsFormat format, string directory = null, Action<ProgressStep, int> progressCallback = null);

        /// <summary>
        /// Attempts to cancel an ongoing asynchronous statistics report generation for the
        /// specified context.
        /// </summary>
        /// <remarks>
        /// Cancellation may succeed only if the server-side process has not already completed.
        /// The method returns immediately and does not block until the process is fully terminated.
        /// </remarks>
        /// <param name="context">The statistics context identifying the request to cancel.</param>
        /// <returns>
        /// <see langword="true"/> if a running request was found and cancellation was successfully
        /// requested; <see langword="false"/> if there was no running request for the specified
        /// context or if cancellation could not be applied.
        /// </returns>
        Task<bool> CancelRequestAsync(StatsContext context);

        /// <summary>
        /// Creates a new recurring scheduled report for the specified context.
        /// </summary>
        /// <remarks>
        /// The report is generated repeatedly according to <paramref name="recurrence"/> and
        /// <paramref name="observationPeriod"/>, formatted in the specified output format, and
        /// sent as a ZIP file attachment to the provided recipients.
        /// </remarks>
        /// <param name="context">The statistics context defining which data and counters to include.</param>
        /// <param name="id">A unique identifier for the scheduled report.</param>
        /// <param name="description">A human-readable description of the report.</param>
        /// <param name="observationPeriod">The observation period over which statistics are collected.</param>
        /// <param name="recurrence">The recurrence pattern for generating the report.</param>
        /// <param name="format">The output file format.</param>
        /// <param name="recipients">The email addresses of the report recipients.</param>
        /// <returns>The newly created <see cref="ScheduledReport"/>, or <see langword="null"/> on failure.</returns>
        /// <seealso cref="CreateOneTimeScheduledReportAsync"/>
        /// <seealso cref="DeleteScheduledReportAsync"/>
        /// <seealso cref="SetScheduledReportEnabledAsync"/>
        Task<ScheduledReport> CreateRecurrentScheduledReportAsync(
            StatsContext context,
            string id,
            string description,
            ReportObservationPeriod observationPeriod,
            Recurrence recurrence,
            StatsFormat format,
            List<string> recipients);

        /// <summary>
        /// Creates a new one-time scheduled report for the specified context.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="CreateRecurrentScheduledReportAsync"/>, this report is generated
        /// only once for the specified <paramref name="observationPeriod"/> and is no longer
        /// active afterwards.
        /// </remarks>
        /// <param name="context">The statistics context defining which data and counters to include.</param>
        /// <param name="id">A unique identifier for the scheduled report.</param>
        /// <param name="description">A human-readable description of the report.</param>
        /// <param name="observationPeriod">The observation period over which statistics are collected.</param>
        /// <param name="format">The output file format.</param>
        /// <param name="recipients">The email addresses of the report recipients.</param>
        /// <returns>The newly created <see cref="ScheduledReport"/>, or <see langword="null"/> on failure.</returns>
        /// <seealso cref="CreateRecurrentScheduledReportAsync"/>
        /// <seealso cref="DeleteScheduledReportAsync"/>
        Task<ScheduledReport> CreateOneTimeScheduledReportAsync(
            StatsContext context,
            string id,
            string description,
            ReportObservationPeriod observationPeriod,
            StatsFormat format,
            List<string> recipients);

        /// <summary>
        /// Returns all scheduled reports associated with the specified context.
        /// </summary>
        /// <param name="context">The statistics context whose reports are retrieved.</param>
        /// <returns>A list of <see cref="ScheduledReport"/> objects, or <see langword="null"/> on failure.</returns>
        Task<List<ScheduledReport>> GetScheduledReportsAsync(StatsContext context);

        /// <summary>
        /// Returns the scheduled report with the specified identifier.
        /// </summary>
        /// <param name="context">The statistics context that owns the report.</param>
        /// <param name="scheduleReportId">The unique identifier of the scheduled report.</param>
        /// <returns>The <see cref="ScheduledReport"/> corresponding to the ID, or <see langword="null"/> if not found.</returns>
        Task<ScheduledReport> GetScheduledReportAsync(StatsContext context, string scheduleReportId);

        /// <summary>
        /// Deletes the specified scheduled report.
        /// </summary>
        /// <param name="report">The scheduled report to delete.</param>
        /// <returns><see langword="true"/> if the report was successfully deleted; otherwise <see langword="false"/>.</returns>
        Task<bool> DeleteScheduledReportAsync(ScheduledReport report);

        /// <summary>
        /// Enables or disables the specified scheduled report.
        /// </summary>
        /// <param name="report">The scheduled report to update.</param>
        /// <param name="enabled"><see langword="true"/> to enable the report; <see langword="false"/> to disable it.</param>
        /// <returns><see langword="true"/> if the report state was successfully updated; otherwise <see langword="false"/>.</returns>
        Task<bool> SetScheduledReportEnabledAsync(ScheduledReport report, bool enabled);

        /// <summary>
        /// Persists any changes made to the specified scheduled report.
        /// </summary>
        /// <remarks>
        /// Fields that can be updated include the description, observation period, recurrence
        /// pattern, output format, and recipient list.
        /// </remarks>
        /// <param name="report">The <see cref="ScheduledReport"/> instance containing the updated fields.</param>
        /// <returns><see langword="true"/> if the update was successful; <see langword="false"/> if the report does not exist or the update could not be applied.</returns>
        Task<bool> UpdateScheduledReportAsync(ScheduledReport report);
    }
}
