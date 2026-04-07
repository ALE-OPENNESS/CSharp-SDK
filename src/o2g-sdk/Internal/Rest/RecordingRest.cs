/*
* Copyright 2025 ALE International
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
using o2g.Internal.Utility;
using o2g.Types.RecordingNS;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Types.Recording;

namespace o2g.Internal.Rest
{
    class ListRecordingDevices
    {
        public List<string> RecordingDevices { get; set; }
    }

    class RecordRequest
    {
        public string CallRef { get; set; }
    }

    class StartRecordRequest : RecordRequest
    {
        public RecordingStartType StartType { get; set; }
    }



    internal class RecordingRest : AbstractRESTService, IRecording
    {
        public RecordingRest(Uri uri) : base(uri)
        {

        }

        public async Task<List<string>> GetRecordedDevicesAsync()
        {
            Uri uriGet = uri.Append("devices");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ListRecordingDevices devices = await GetResult<ListRecordingDevices>(response);
            if (devices == null)
            {
                return null;
            }
            else
            {
                return devices.RecordingDevices;
            }
        }

        public async Task<DeviceRecordingInfo> GetDeviceRecordingInfoAsync(string deviceId, string loginName)
        {
            Uri uriGet = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"));
            if (loginName != null)
            {
                uriGet = uriGet.AppendQuery("loginName", loginName);
            }

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<DeviceRecordingInfo>(response);
        }

        public async Task<DeviceRecordingInfo> PauseRecordingAsync(string deviceId, string callRef, string loginName)
        {
            Uri uriPost = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"), "pause");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            RecordRequest req = new()
            {
                CallRef = AssertUtil.NotNullOrEmpty(callRef, "callRef")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await GetResult<DeviceRecordingInfo>(response);
        }

        public async Task<DeviceRecordingInfo> ResumeRecordingAsync(string deviceId, string callRef, string loginName)
        {
            Uri uriPost = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"), "resume");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            RecordRequest req = new()
            {
                CallRef = AssertUtil.NotNullOrEmpty(callRef, "callRef")
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await GetResult<DeviceRecordingInfo>(response);
        }

        public async Task<DeviceRecordingInfo> StartRecordingAsync(string deviceId, string callRef, RecordingStartType startType, string loginName)
        {
            Uri uriPost = uri.Append("devices", AssertUtil.NotNullOrEmpty(deviceId, "deviceId"), "start");
            if (loginName != null)
            {
                uriPost = uriPost.AppendQuery("loginName", loginName);
            }

            StartRecordRequest req = new()
            {
                CallRef = AssertUtil.NotNullOrEmpty(callRef, "callRef"),
                StartType = startType
            };

            var json = JsonSerializer.Serialize(req, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(uriPost, content);
            return await GetResult<DeviceRecordingInfo>(response);
        }

        public async Task<RecordingStatus> GetRecordingStatusAsync()
        {
            Uri uriGet = uri.Append("status");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<RecordingStatus>(response);
        }
    }
}
