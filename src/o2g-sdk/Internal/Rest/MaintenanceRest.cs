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
using o2g.Events.Maintenance;
using o2g.Internal.Events;
using o2g.Internal.Types.Maintenance;
using o2g.Internal.Utility;
using o2g.Types.MaintenanceNS;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace o2g.Internal.Rest
{
    internal class MaintenanceRest : AbstractRESTService, IMaintenance
    {
#pragma warning disable CS0067, CS0649
        [Injection]
        private readonly EventHandlers _eventHandlers;
#pragma warning restore CS0067, CS0649

        public event EventHandler<O2GEventArgs<OnCtiLinkDownEvent>> CtiLinkDown
        {
            add => _eventHandlers.CtiLinkDown += value;
            remove => _eventHandlers.CtiLinkDown -= value;
        }

        public event EventHandler<O2GEventArgs<OnCtiLinkUpEvent>> CtiLinkUp
        {
            add => _eventHandlers.CtiLinkUp += value;
            remove => _eventHandlers.CtiLinkUp -= value;
        }

        public event EventHandler<O2GEventArgs<OnPbxLoadedEvent>> PbxLoaded
        {
            add => _eventHandlers.PbxLoaded += value;
            remove => _eventHandlers.PbxLoaded -= value;
        }


        public MaintenanceRest(Uri uri) : base(uri)
        {

        }

        public async Task<SystemStatus> GetSystemStatusAsync()
        {
            Uri uriGet = uri.Append("status");

            HttpResponseMessage response = await httpClient.GetAsync(uriGet);

            O2GSystemStatus systemStatus = await GetResult<O2GSystemStatus>(response);
            if (systemStatus == null)
            {
                return null;
            }
            else
            {
                return new SystemStatus()
                {
                    LogicalAddress = systemStatus.LogicalAddress,
                    StartDate = systemStatus.StartDate,
                    HaMode = systemStatus.Ha,
                    Primary = systemStatus.Primary,
                    PrimaryVersion = systemStatus.PrimaryVersion,
                    Secondary = systemStatus.Secondary,
                    SecondaryVersion = systemStatus.SecondaryVersion,
                    Pbxs = systemStatus.Pbxs,
                    Licenses = systemStatus.License.lics,
                    ConfigurationType = systemStatus.ConfigurationType
                };
            }
        }

        public async Task<bool> IsLicenseExist(string license)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri.Append("status").AppendQuery("lic", license));
            return await IsSucceeded(response);
        }
    }
}
