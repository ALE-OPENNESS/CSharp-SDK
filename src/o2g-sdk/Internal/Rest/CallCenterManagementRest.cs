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

using o2g.Internal.Types.CallCenterManagementNS;
using o2g.Internal.Utility;
using o2g.Types.CallCenterManagementNS;
using o2g.Types.CallCenterManagementNS.CalendarNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    class O2GPilotTransitionRequest
    {
        public string Time { get; set; }
        public int RuleNumber { get; set; }
        public PilotOperatingMode Mode { get; set; }

        internal static O2GPilotTransitionRequest From(Transition transition)
        {
            return new O2GPilotTransitionRequest
            {
                Time = transition.TransitionTime.ToString(),
                RuleNumber = transition.RuleNumber,
                Mode = transition.Mode
            };
        }
    }

    class O2GPilotList
    {
        public List<O2GPilot> PilotList { get; set; }
    }

    internal class CallCenterManagementRest : AbstractRESTService, ICallCenterManagement
    {
        public CallCenterManagementRest(Uri uri) : base(uri)
        {
        }

        public async Task<IReadOnlyList<Pilot>> GetPilotsAsync(int nodeId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GPilotList pilotList = await GetResult<O2GPilotList>(response);
            if (pilotList?.PilotList == null)
                return null;

            return pilotList.PilotList
                .Select(p => p.ToPilot())
                .ToList()
                .AsReadOnly();
        }

        public async Task<Pilot> GetPilotAsync(int nodeId, string pilotNumber)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"));

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GPilot o2gPilot = await GetResult<O2GPilot>(response);
            return o2gPilot?.ToPilot();
        }

        public async Task<Calendar> GetCalendarAsync(int nodeId, string pilotNumber)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GCalendar calendar = await GetResult<O2GCalendar>(response);
            return calendar?.ToCalendar();
        }

        public async Task<ExceptionCalendar> GetExceptionCalendarAsync(int nodeId, string pilotNumber)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "exception");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GExceptionCalendar calendar = await GetResult<O2GExceptionCalendar>(response);
            return calendar?.ToExceptionCalendar();
        }

        public async Task<bool> AddExceptionTransitionAsync(int nodeId, string pilotNumber,
            DateTime date, Transition transition)
        {
            Uri uriPost = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "exception",
                AssertUtil.NotNull(date, "date").ToString("yyyyMMdd"),
                "transitions");

            var content = Serialize(transition);
            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteExceptionTransitionAsync(int nodeId, string pilotNumber,
            DateTime date, int transitionIndex)
        {
            Uri uriDelete = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "exception",
                AssertUtil.NotNull(date, "date").ToString("yyyyMMdd"),
                "transitions",
                (AssertUtil.AssertPositive(transitionIndex, "transitionIndex") + 1).ToString());

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> SetExceptionTransitionAsync(int nodeId, string pilotNumber,
            DateTime date, int transitionIndex, Transition transition)
        {
            Uri uriPut = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "exception",
                AssertUtil.NotNull(date, "date").ToString("yyyyMMdd"),
                "transitions",
                (AssertUtil.AssertPositive(transitionIndex, "transitionIndex") + 1).ToString());

            var content = Serialize(transition);
            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public async Task<NormalCalendar> GetNormalCalendarAsync(int nodeId, string pilotNumber)
        {
            Uri uriGet = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "normal");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GNormalCalendar calendar = await GetResult<O2GNormalCalendar>(response);
            return calendar?.ToNormalCalendar();
        }

        public async Task<bool> AddNormalTransitionAsync(int nodeId, string pilotNumber,
            DayOfWeek day, Transition transition)
        {
            Uri uriPost = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "normal",
                day.ToString().ToLower(),
                "transitions");

            var content = Serialize(transition);
            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteNormalTransitionAsync(int nodeId, string pilotNumber,
            DayOfWeek day, int transitionIndex)
        {
            Uri uriDelete = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "normal",
                day.ToString().ToLower(),
                "transitions",
                (AssertUtil.AssertPositive(transitionIndex, "transitionIndex") + 1).ToString());

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> SetNormalTransitionAsync(int nodeId, string pilotNumber,
            DayOfWeek day, int transitionIndex, Transition transition)
        {
            Uri uriPut = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "calendar", "normal",
                day.ToString().ToLower(),
                "transitions",
                (AssertUtil.AssertPositive(transitionIndex, "transitionIndex") + 1).ToString());

            var content = Serialize(transition);
            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> OpenPilotAsync(int nodeId, string pilotNumber)
        {
            Uri uriPost = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "open");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        public async Task<bool> ClosePilotAsync(int nodeId, string pilotNumber)
        {
            Uri uriPost = uri.Append(
                AssertUtil.AssertPositive(nodeId, "nodeId").ToString(),
                "pilots",
                AssertUtil.NotNullOrEmpty(pilotNumber, "pilotNumber"),
                "close");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, null);
            return await IsSucceeded(response);
        }

        private StringContent Serialize(Transition transition)
        {
            var req = O2GPilotTransitionRequest.From(
                AssertUtil.NotNull(transition, "transition"));
            var json = JsonSerializer.Serialize(req, serializeOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}