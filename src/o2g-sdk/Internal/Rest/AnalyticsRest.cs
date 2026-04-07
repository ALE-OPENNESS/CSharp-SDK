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

using o2g.Internal.Types.Analytics;
using o2g.Internal.Utility;
using o2g.Types.AnalyticsNS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    class O2GIncidents
    {
        public List<O2GIncident> Incidents { get; set; }

        public int Severity0 { get; set; }
        public int Severity1 { get; set; }
        public int Severity2 { get; set; }
        public int Severity3 { get; set; }
        public int Severity4 { get; set; }
        public int Severity5 { get; set; }

        internal List<Incident> ToIncidents()
        {
            if (Incidents == null)
            {
                return null;
            }
            else
            {
                return Incidents.Select((inc) => inc.ToIncident()).ToList();
            }
        }
    }

    class ChargingFileList
    {
        public List<O2GChargingFile> Files { get; set; }

        public List<ChargingFile> ToChargingFiles()
        {
            return Files.Select(f => new ChargingFile()
            {
                Name = f.Name,
                Timestamp = f.GetTimeStamp()
            }).ToList();
        }
    }

    class O2GChargingResult
    {
        public List<O2GCharging> Chargings { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int NbChargingFiles { get; init; }
        public int TotalTicketNb { get; init; }
        public int ValuableTicketNb { get; init; }

        public ChargingResult ToChargingResult()
        {
            TimeRange range = null;
            if ((FromDate != null) && (ToDate != null))
            {
                range = new TimeRange(DateTime.ParseExact(FromDate, "yyyyMMdd", CultureInfo.InvariantCulture), DateTime.ParseExact(ToDate, "yyyyMMdd", CultureInfo.InvariantCulture));
            }

            return new()
            {
                Range = range,
                Chargings = Chargings.Select((c) => c.ToCharging()).ToList(),
                ChargingFileCount = NbChargingFiles,
                TotalTicketCount = TotalTicketNb,
                ValuableTicketCount = ValuableTicketNb
            };
        } 
    }



    internal class AnalyticsRest : AbstractRESTService, IAnalytics
    {
        public AnalyticsRest(Uri uri) : base(uri)
        {
        }

        public async Task<List<ChargingFile>> GetChargingFilesAsync(int nodeId, TimeRange filter)
        {
            Uri uriGet = uri
                .Append("charging", "files")
                .AppendQuery("nodeId", AssertUtil.AssertPositive(nodeId, "nodeId").ToString());

            if (filter != null)
            {
                uriGet = uriGet.AppendQuery("fromDate", filter.from.ToString("yyyyMMdd"));
                uriGet = uriGet.AppendQuery("toDate", filter.to.ToString("yyyyMMdd"));
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ChargingFileList chargingFiles = await GetResult<ChargingFileList>(response);
            if (chargingFiles == null)
            {
                return null;
            }
            else
            {
                return chargingFiles.ToChargingFiles();
            }
        }

        public async Task<ChargingResult> GetChargingsAsync(int nodeId, TimeRange filter, int? topResults, bool all)
        {
            Uri uriGet = uri
                .Append("charging")
                .AppendQuery("nodeId", AssertUtil.AssertPositive(nodeId, "nodeId").ToString());

            if (filter != null)
            {
                uriGet = uriGet.AppendQuery("fromDate", filter.from.ToString("yyyyMMdd"));
                uriGet = uriGet.AppendQuery("toDate", filter.to.ToString("yyyyMMdd"));
            }

            if (topResults.HasValue)
            {
                uriGet = uriGet.AppendQuery("top", topResults.Value.ToString());
            }

            if (all)
            {
                uriGet = uriGet.AppendQuery("all", "true");
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GChargingResult chargingResult = await GetResult<O2GChargingResult>(response);
            if (chargingResult == null)
            {
                return null;
            }
            else
            {
                return chargingResult.ToChargingResult();
            }
        }

        public async Task<ChargingResult> GetChargingsAsync(int nodeId, List<ChargingFile> files, int? topResults = null, bool all = false)
        {
            Uri uriGet = uri
                .Append("charging")
                .AppendQuery("nodeId", AssertUtil.AssertPositive(nodeId, "nodeId").ToString())
                .AppendQuery("files", string.Join(',', AssertUtil.NotNullList(files, "files").Select(f => f.Name).ToList()));

            if (topResults.HasValue)
            {
                uriGet = uriGet.AppendQuery("top", topResults.Value.ToString());
            }

            if (all)
            {
                uriGet = uriGet.AppendQuery("all", "true");
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GChargingResult chargingResult = await GetResult<O2GChargingResult>(response);
            if (chargingResult == null)
            {
                return null;
            }
            else
            {
                return chargingResult.ToChargingResult();
            }
        }

        public async Task<List<Incident>> GetIncidentsAsync(int nodeId, int last)
        {
            Uri uriGet = uri
                .Append("incidents")
                .AppendQuery("nodeId", AssertUtil.AssertPositive(nodeId, "nodeId").ToString());

            if (last > 0)
            {
                uriGet = uriGet.AppendQuery("last", last.ToString());
            }


            HttpResponseMessage response = await httpClient.GetAsync(uriGet);

            O2GIncidents o2gIncidents = await GetResult<O2GIncidents>(response);
            if (o2gIncidents == null)
            {
                return null;
            }
            else
            {
                return o2gIncidents.ToIncidents();
            }
        }
    }
}
