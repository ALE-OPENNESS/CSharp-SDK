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
using o2g.Internal.Rest;
using o2g.Internal.Utility;
using o2g.Tests.Helpers;
using o2g.Types.MaintenanceNS;
using System.Net;
using System.Net.Http;

namespace o2g.Tests.Services
{
    public class MaintenanceRestTests : ServiceTestBase
    {
        private static readonly Uri MaintenanceUri = new("https://fake-o2g/api/maintenance");

        private MaintenanceRest Service() =>
            DependancyResolver.Resolve(new MaintenanceRest(MaintenanceUri));

        private const string FullSystemStatus = """
            {
                "logicalAddress": { "fqdn": "o2g.example.com", "ip": "10.0.0.1" },
                "systemResources": { "fqdn": "res.example.com", "ip": "10.0.0.2" },
                "startDate": "2024-01-15T10:30:00",
                "ha": true,
                "primary": "primary.example.com",
                "primaryVersion": "3.5.0",
                "primaryServicesStatus": {
                    "services": [
                        { "name": "ctiService", "status": "running", "mode": "active" }
                    ],
                    "globalIPAdress": "192.168.1.100",
                    "drbd": "synced"
                },
                "pbxs": [],
                "license": {
                    "type": "FLEXLM",
                    "context": "production",
                    "currentServer": "license-server.example.com",
                    "status": "active",
                    "statusMessage": "License valid",
                    "lics": [
                        { "name": "O2G-CTI", "total": 100, "currentUsed": 42, "expiration": "2025-12-31" }
                    ]
                },
                "configurationType": "FULL_SERVICES",
                "applicationId": "o2g-app-001",
                "subscriberFilter": "ALL"
            }
            """;

        #region GetSystemStatusAsync — request

        [Fact]
        public async Task GetSystemStatusAsync_SendsGetToStatusEndpoint()
        {
            SetupHttpClient(FullSystemStatus);

            await Service().GetSystemStatusAsync();

            AssertRequest().Method(HttpMethod.Get).Uri("/api/maintenance/status");
        }

        #endregion

        #region GetSystemStatusAsync — existing fields

        [Fact]
        public async Task GetSystemStatusAsync_MapsLogicalAddress()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.LogicalAddress.Fqdn.Should().Be("o2g.example.com");
            status.LogicalAddress.Ip.Should().Be("10.0.0.1");
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsHa()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.Ha.Should().BeTrue();
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsPrimaryFields()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.Primary.Should().Be("primary.example.com");
            status.PrimaryVersion.Should().Be("3.5.0");
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsConfigurationType()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.ConfigurationType.Should().Be(ConfigurationType.FullServices);
        }

        #endregion

        #region GetSystemStatusAsync — new fields

        [Fact]
        public async Task GetSystemStatusAsync_MapsSystemResources()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.SystemResources.Should().NotBeNull();
            status.SystemResources.Fqdn.Should().Be("res.example.com");
            status.SystemResources.Ip.Should().Be("10.0.0.2");
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsPrimaryServicesStatus()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.PrimaryServicesStatus.Should().NotBeNull();
            status.PrimaryServicesStatus.Services.Should().HaveCount(1);
            status.PrimaryServicesStatus.Services[0].Name.Should().Be("ctiService");
            status.PrimaryServicesStatus.Services[0].Status.Should().Be("running");
            status.PrimaryServicesStatus.GlobalIPAdress.Should().Be("192.168.1.100");
            status.PrimaryServicesStatus.DrbdStatus.Should().Be("synced");
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsApplicationId()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.ApplicationId.Should().Be("o2g-app-001");
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsSubscriberFilter()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.SubscriberFilter.Should().Be(SubscriberFilter.All);
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsLicenseStatus()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.License.Should().NotBeNull();
            status.License.Type.Should().Be(LicenseType.Flexlm);
            status.License.Context.Should().Be("production");
            status.License.CurrentServer.Should().Be("license-server.example.com");
            status.License.Status.Should().Be("active");
        }

        [Fact]
        public async Task GetSystemStatusAsync_MapsLicensesList()
        {
            SetupHttpClient(FullSystemStatus);

            var status = await Service().GetSystemStatusAsync();

            status.License.Licenses.Should().HaveCount(1);
            status.License.Licenses[0].Name.Should().Be("O2G-CTI");
            status.License.Licenses[0].Total.Should().Be(100);
            status.License.Licenses[0].CurrentUsed.Should().Be(42);
        }

        [Fact]
        public async Task GetSystemStatusAsync_UnknownSubscriberFilter_FallsBackToUnknown()
        {
            SetupHttpClient("""
                {
                    "logicalAddress": {},
                    "pbxs": [],
                    "license": { "lics": [] },
                    "subscriberFilter": "FUTURE_VALUE"
                }
                """);

            var status = await Service().GetSystemStatusAsync();

            status.SubscriberFilter.Should().Be(SubscriberFilter.Unknown);
        }

        [Fact]
        public async Task GetSystemStatusAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var status = await Service().GetSystemStatusAsync();

            status.Should().BeNull();
        }

        #endregion

        #region IsLicenseExist

        [Fact]
        public async Task IsLicenseExist_SendsGetWithLicQueryParam()
        {
            SetupHttpClient("{}");

            await Service().IsLicenseExist("O2G-CTI");

            AssertRequest().Method(HttpMethod.Get).Uri("/api/maintenance/status?lic=O2G-CTI");
        }

        [Fact]
        public async Task IsLicenseExist_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("{}");

            var result = await Service().IsLicenseExist("O2G-CTI");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsLicenseExist_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var result = await Service().IsLicenseExist("O2G-CTI");

            result.Should().BeFalse();
        }

        #endregion
    }
}
