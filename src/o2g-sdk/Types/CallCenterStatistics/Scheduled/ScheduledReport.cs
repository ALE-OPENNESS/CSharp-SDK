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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace o2g.Types.CallCenterStatisticsNS.Scheduled
{
    /// <summary>
    /// Represents the execution state of a scheduled statistics report.
    /// </summary>
    public enum ScheduledReportState
    {
        /// <summary>Report has not yet been executed.</summary>
        [EnumMember(Value = "Not_executed")] NotExecuted,
        /// <summary>Report has been executed successfully.</summary>
        [EnumMember(Value = "Executed")] Executed,
        /// <summary>Execution failed during data retrieval.</summary>
        [EnumMember(Value = "Failed_On_Get_Data")] FailedOnGetData,
        /// <summary>Execution failed while sending the report via email.</summary>
        [EnumMember(Value = "Failed_On_Send_Mail")] FailedOnSendMail,
        /// <summary>Report execution is currently in progress.</summary>
        [EnumMember(Value = "In_progress")] InProgress,
        /// <summary>Scheduled report has expired and will no longer execute.</summary>
        [EnumMember(Value = "Expired")] Expired
    }

    /// <summary>
    /// Represents a scheduled statistics report.
    /// </summary>
    public class ScheduledReport
    {
        /// <summary>The unique identifier of the scheduled report.</summary>
        public string Id { get; set; }

        /// <summary>The description of the scheduled report.</summary>
        public string Description { get; set; }

        /// <summary>The observation period used by this scheduled report.</summary>
        public ReportObservationPeriod ObservationPeriod { get; set; }

        /// <summary>The recurrence pattern; <see langword="null"/> if the report runs only once.</summary>
        public Recurrence Recurrence { get; set; }

        /// <summary><see langword="true"/> if the report is executed only once.</summary>
        public bool Once => Recurrence == null;

        /// <summary>The output format of the report.</summary>
        public StatsFormat Format { get; set; }

        /// <summary>The email addresses of the recipients.</summary>
        public List<string> Recipients { get; set; }

        /// <summary>The current execution state of the scheduled report.</summary>
        public ScheduledReportState State { get; set; }

        /// <summary><see langword="true"/> if the scheduled report is enabled.</summary>
        public bool Enabled { get; set; }

        /// <summary><see langword="true"/> if statistics headers are condensed.</summary>
        public bool ShortHeader { get; set; }

        /// <summary>The date and time of the last execution; <see langword="null"/> if never executed.</summary>
        public DateTime? LastExecutionDate { get; set; }

        /// <summary>The statistics context used to generate this report.</summary>
        public StatsContext Context { get; set; }
    }
}
