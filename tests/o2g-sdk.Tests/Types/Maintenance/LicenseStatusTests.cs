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
using o2g.Tests.Helpers;
using o2g.Types.MaintenanceNS;
using Xunit;

namespace o2g.Tests.Types.Maintenance
{
    public class LicenseStatusTests : JsonTestBase
    {
        private const string FullLicenseStatus = """
            {
                "type": "FLEXLM",
                "context": "production",
                "currentServer": "license-server.example.com",
                "status": "active",
                "statusMessage": "License valid",
                "lics": [
                    { "name": "O2G-CTI", "total": 100, "currentUsed": 42, "expiration": "2025-12-31" },
                    { "name": "O2G-CC", "total": 50, "currentUsed": 10, "expiration": "2025-12-31" }
                ]
            }
            """;

        private const string LmsLicenseStatus = """
            {
                "type": "LMS",
                "context": "test",
                "lics": []
            }
            """;

        #region Type

        [Fact]
        public void Deserialize_FlexlmType_MapsToFlexlm()
        {
            var status = Deserialize<LicenseStatus>(FullLicenseStatus);
            status.Type.Should().Be(LicenseType.Flexlm);
        }

        [Fact]
        public void Deserialize_LmsType_MapsToLms()
        {
            var status = Deserialize<LicenseStatus>(LmsLicenseStatus);
            status.Type.Should().Be(LicenseType.Lms);
        }

        #endregion

        #region Scalar fields

        [Fact]
        public void Deserialize_FullStatus_MapsScalarFields()
        {
            var status = Deserialize<LicenseStatus>(FullLicenseStatus);

            status.Context.Should().Be("production");
            status.CurrentServer.Should().Be("license-server.example.com");
            status.Status.Should().Be("active");
            status.StatusMessage.Should().Be("License valid");
        }

        #endregion

        #region Licenses list (lics)

        [Fact]
        public void Deserialize_FullStatus_MapsLicensesList()
        {
            var status = Deserialize<LicenseStatus>(FullLicenseStatus);

            status.Licenses.Should().HaveCount(2);
        }

        [Fact]
        public void Deserialize_FullStatus_MapsFirstLicenseFields()
        {
            var status = Deserialize<LicenseStatus>(FullLicenseStatus);
            var lic = status.Licenses[0];

            lic.Name.Should().Be("O2G-CTI");
            lic.Total.Should().Be(100);
            lic.CurrentUsed.Should().Be(42);
            lic.Expiration.Should().Be("2025-12-31");
        }

        [Fact]
        public void Deserialize_EmptyLics_ReturnsEmptyList()
        {
            var status = Deserialize<LicenseStatus>(LmsLicenseStatus);
            status.Licenses.Should().BeEmpty();
        }

        #endregion

        #region Null / minimal

        [Fact]
        public void Deserialize_NoStatusMessage_IsNull()
        {
            var status = Deserialize<LicenseStatus>(LmsLicenseStatus);
            status.StatusMessage.Should().BeNull();
            status.CurrentServer.Should().BeNull();
        }

        #endregion
    }
}
