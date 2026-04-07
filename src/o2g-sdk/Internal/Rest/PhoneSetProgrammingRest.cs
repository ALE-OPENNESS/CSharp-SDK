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

using o2g.Internal.Types.PhoneSetProgrammable;
using o2g.Internal.Utility;
using o2g.Types.PhoneSetProgrammableNS;
using o2g.Types.UsersNS;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{

    record Associate(string Number) { }

    class DeviceList
    {
        public List<Device> Devices { get; init; }
    }

    class ProgrammableKeyList
    {
        public List<ProgrammableKey> Pkeys { get; init; }
    }

    class SoftKeyList
    {
        public List<SoftKey> Skeys { get; init; }
    }

    internal class PhoneSetProgrammingRest : AbstractRESTService, IPhoneSetProgramming
    {

        public PhoneSetProgrammingRest(Uri uri) : base(uri)
        {
        }

        public async Task<List<Device>> GetDevicesAsync(string loginName)
        {
            Uri uriGet = uri.Append(AssertUtil.NotNullOrEmpty(loginName, "loginName"), "devices");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            DeviceList devices = await GetResult<DeviceList>(response);
            if (devices == null)
            {
                return null;
            }
            else
            {
                return devices.Devices;
            }
        }

        public async Task<Device> GetDeviceAsync(string loginName, string deviceId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"), 
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"));

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<Device>(response);
        }

        public async Task<List<ProgrammableKey>> GetProgrammableKeysAsync(string loginName, string deviceId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "programmeableKeys");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ProgrammableKeyList pkeys = await GetResult<ProgrammableKeyList>(response);
            if (pkeys == null)
            {
                return null;
            }
            else
            {
                return pkeys.Pkeys;
            }
        }

        public async Task<List<ProgrammableKey>> GetProgrammedKeysAsync(string loginName, string deviceId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "programmedKeys");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            ProgrammableKeyList pkeys = await GetResult<ProgrammableKeyList>(response);
            if (pkeys == null)
            {
                return null;
            }
            else
            {
                return pkeys.Pkeys;
            }
        }


        public async Task<List<SoftKey>> GetSoftKeys(string loginName, string deviceId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "softKeys");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            SoftKeyList skeys = await GetResult<SoftKeyList>(response);
            if (skeys == null)
            {
                return null;
            }
            else
            {
                return skeys.Skeys;
            }
        }

        public async Task<bool> SetProgrammableKeyAsync(string loginName, string deviceId, ProgrammableKey key)
        {
            Uri uriPut = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "programmeableKeys",
                AssertUtil.AssertPositiveStrict(AssertUtil.NotNull<ProgrammableKey>(key, "key").Position, "Key.Position").ToString());

            var json = JsonSerializer.Serialize(key, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteProgrammableKeyAsync(string loginName, string deviceId, int position)
        {
            Uri uriDelete = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "programmeableKeys",
                AssertUtil.AssertPositiveStrict(position, "position").ToString());

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }

        public async Task<bool> SetSoftKeyAsync(string loginName, string deviceId, SoftKey key)
        {
            Uri uriPut = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "softKeys",
                AssertUtil.AssertPositiveStrict(AssertUtil.NotNull<SoftKey>(key, "key").Position, "Key.Position").ToString());

            var json = JsonSerializer.Serialize(key, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public async Task<bool> DeleteSoftKeyAsync(string loginName, string deviceId, int position)
        {
            Uri uriDelete = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "softKeys",
                AssertUtil.AssertPositiveStrict(position, "position").ToString());

            HttpResponseMessage response = await httpClient.DeleteAsync(uriDelete);
            return await IsSucceeded(response);
        }


        private async Task<bool> DoDeviceAction(string loginName, string deviceId, string action, bool activate)
        {
            Uri uriPut = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                action);

            uriPut = uriPut.AppendQuery("activate", activate ? "true" : "false");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, null);
            return await IsSucceeded(response);
        }

        public Task<bool> LockDeviceAsync(string loginName, string deviceId)
        {
            return this.DoDeviceAction(loginName, deviceId, "lock", true);
        }

        public Task<bool> UnLockDeviceAsync(string loginName, string deviceId)
        {
            return this.DoDeviceAction(loginName, deviceId, "lock", false);
        }

        public Task<bool> EnableCamponAsync(string loginName, string deviceId)
        {
            return this.DoDeviceAction(loginName, deviceId, "campon", true);
        }

        public Task<bool> DisableCamponAsync(string loginName, string deviceId)
        {
            return this.DoDeviceAction(loginName, deviceId, "campon", false);
        }

        public Task<bool> ActivateRemoteExtensionAsync(string loginName, string deviceId)
        {
            return this.DoDeviceAction(loginName, deviceId, "REActive", true);
        }

        public Task<bool> DeactivateRemoteExtensionAsync(string loginName, string deviceId)
        {
            return this.DoDeviceAction(loginName, deviceId, "REActive", false);
        }


        public async Task<Pin> GetPinCodeAsync(string loginName, string deviceId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "pin");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            O2GPinCode pin = await GetResult<O2GPinCode>(response);

            if (pin == null)
            {
                return null;
            }
            else
            {
                return pin.ToPin();
            }
        }

        public async Task<bool> SetPinCodeAsync(string loginName, string deviceId, Pin code)
        {
            Uri uriPut = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "pin");

            O2GPinCode o2gPinCode = O2GPinCode.From(code);

            var json = JsonSerializer.Serialize(o2gPinCode, serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }

        public async Task<DynamicState> GetDynamicStateAsync(string loginName, string deviceId)
        {
            Uri uriGet = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "dynamicState");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);
            return await GetResult<DynamicState>(response);
        }

        public async Task<bool> SetAssociateAsync(string loginName, string deviceId, string associate)
        {
            Uri uriPut = uri.Append(
                AssertUtil.NotNullOrEmpty(loginName, "loginName"),
                "devices",
                AssertUtil.NotNullOrEmpty(deviceId, "deviceId"),
                "associate");

            var json = JsonSerializer.Serialize(new Associate(AssertUtil.NotNullOrEmpty(associate, "associate")), serializeOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(uriPut, content);
            return await IsSucceeded(response);
        }
    }
}
