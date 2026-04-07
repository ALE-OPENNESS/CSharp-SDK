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

using o2g.Events;
using o2g.Events.CallCenterRsi;
using o2g.Internal.Events;
using o2g.Internal.Utility;
using o2g.Types.CallCenterRsiNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    record RouteEnd(string routeCrid) { }

    class RsiPointList
    {
        public List<RsiPoint> RsiPoints { get; set; }
    }

    class CallRefRequest
    {
        public string CallRef { get; set; }
    }


    class StartDataCollectionRequest : CallRefRequest
    {
        public int? NumChars { get; set; }
        public char? FlushChar { get; set; }
        public int? Timeout { get; set; }
        public AdditionalDigitCollectionCriteria AdditionalCriteria { get; set; }
    }

    class PlayToneRequest : CallRefRequest
    {
        public Tones Type { get; set; }
        public int Duration { get; set; }
    }


    class PlayVoiceGuideRequest : CallRefRequest
    {
        public int GuideNumber { get; set; }
        public int? Duration { get; set; }
    }

    class RouteSelect
    {
        public string RouteCrid { get; set; }
        public string SelectedRoute { get; set; }
        public string CallingLine { get; set; }
        public string AssociatedData { get; set; }
        public bool? RouteToVoicemail { get; set; }
    }

    class RouteSessionList
    {
        public List<RouteSession> RouteSessions { get; set; }
    }



    internal class CallCenterRsiRest : AbstractRESTService, ICallCenterRsi
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnDigitCollectedEvent>> DigitCollected
        {
            add => _eventHandlers.DigitCollected += value;
            remove => _eventHandlers.DigitCollected -= value;
        }

        public event EventHandler<O2GEventArgs<OnToneGeneratedStartEvent>> ToneGeneratedStart
        {
            add => _eventHandlers.ToneGeneratedStart += value;
            remove => _eventHandlers.ToneGeneratedStart -= value;
        }

        public event EventHandler<O2GEventArgs<OnToneGeneratedStopEvent>> ToneGeneratedStop
        {
            add => _eventHandlers.ToneGeneratedStop += value;
            remove => _eventHandlers.ToneGeneratedStop -= value;
        }

        public event EventHandler<O2GEventArgs<OnRouteEndEvent>> RouteEnd
        {
            add => _eventHandlers.RouteEnd += value;
            remove => _eventHandlers.RouteEnd -= value;
        }

        public event EventHandler<O2GEventArgs<OnRouteRequestEvent>> RouteRequest
        {
            add => _eventHandlers.RouteRequest += value;
            remove => _eventHandlers.RouteRequest -= value;
        }


        public CallCenterRsiRest(Uri uri) : base(uri)
        {
        }

        public async Task<List<RsiPoint>> GetRsiPointsAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            RsiPointList rsiPoints = await GetResult<RsiPointList>(response);
            if (rsiPoints == null)
            {
                return null;
            }
            else
            {
                return rsiPoints.RsiPoints;
            }
        }

        public async Task<bool> EnableRsiPointAsync(string rsiNumber)
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "enable"), null);
            return await IsSucceeded(response);
        }

        public async Task<bool> DisableRsiPointAsync(string rsiNumber)
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "disable"), null);
            return await IsSucceeded(response);
        }

        public async Task<string> StartCollectDigitsAsync(string rsiNumber, string callRef, int? numChars, char? flushChar, int? timeout, AdditionalDigitCollectionCriteria additionalCriteria)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "collectDigits");

            StartDataCollectionRequest req = new()
            {
                CallRef = AssertUtil.NotNullOrEmpty(callRef, "callRef"),
                NumChars = numChars,
                FlushChar = flushChar,
                Timeout = timeout,
                AdditionalCriteria = additionalCriteria
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return null;
        }

        public async Task<bool> StopCollectDigitsAsync(string rsiNumber, string collCrid)
        {
            Uri uriDelete = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "collectDigits", AssertUtil.NotNullOrEmpty(collCrid, "collCrid"));

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> PlayToneAsync(string rsiNumber, string callRef, Tones tone, int duration)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "playTone");

            PlayToneRequest req = new()
            {
                CallRef = AssertUtil.NotNullOrEmpty(callRef, "callRef"),
                Type = tone,
                Duration = duration
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> CancelToneAsync(string rsiNumber, string callRef)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "stopTone");

            CallRefRequest req = new()
            {
                CallRef = AssertUtil.NotNullOrEmpty(callRef, "callRef"),
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }



        public async Task<bool> PlayVoiceGuideAsync(string rsiNumber, string callRef, int guideNumber, int? duration)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "playVoiceGuide");

            PlayVoiceGuideRequest req = new()
            {
                CallRef = AssertUtil.NotNullOrEmpty(callRef, "callRef"),
                GuideNumber = guideNumber,
                Duration = duration
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> RouteEndAsync(string rsiNumber, string routeCrid)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "routeEnd");

            RouteEnd routeEnd = new(AssertUtil.NotNullOrEmpty(routeCrid, "routeCrid"));

            var json = JsonSerializer.Serialize(routeEnd, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> RouteSelectAsync(string rsiNumber, string routeCrid, string selectedRoute, string callingLine = null, string associatedData = null, bool? routeToVoiceMail = null)
        {
            Uri uriPost = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "routeSelect");

            RouteSelect routeSelect = new()
            {
                RouteCrid = AssertUtil.NotNullOrEmpty(routeCrid, "routeCrid"),
                SelectedRoute = AssertUtil.NotNullOrEmpty(selectedRoute, "selectedRoute"),
                CallingLine = callingLine,
                AssociatedData = associatedData,
                RouteToVoicemail = routeToVoiceMail
            };

            var json = JsonSerializer.Serialize(routeSelect, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await IsSucceeded(response);
        }

        public async Task<List<RouteSession>> GetRouteSessionsAsync(string rsiNumber)
        {
            Uri uriGet = uri.Append(AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), "routeSessions");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            RouteSessionList routeSessions = await GetResult<RouteSessionList>(response);
            if (routeSessions == null)
            {
                return null;
            }
            else
            {
                return routeSessions.RouteSessions;
            }
        }

        public async Task<RouteSession> GetRouteSessionAsync(string rsiNumber, string routeCrid)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(rsiNumber, "rsiNumber"), 
                "routeSessions",
                AssertUtil.NotNullOrEmpty(routeCrid, "routeCrid"));

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<RouteSession>(response);
        }

    }
}
