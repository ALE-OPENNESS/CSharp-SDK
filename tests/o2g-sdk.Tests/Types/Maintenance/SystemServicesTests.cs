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
    public class SystemServicesTests : JsonTestBase
    {
        private const string FullSystemServices = """
            {
                "services": [
                    { "name": "ctiService", "status": "running", "mode": "active" },
                    { "name": "monitoring", "status": "stopped", "mode": "passive" }
                ],
                "globalIPAdress": "192.168.1.100",
                "drbd": "synced"
            }
            """;

        private const string MinimalSystemServices = """
            {
                "services": []
            }
            """;

        #region Services list

        [Fact]
        public void Deserialize_FullServices_HasTwoServices()
        {
            var ss = Deserialize<SystemServices>(FullSystemServices);
            ss.Services.Should().HaveCount(2);
        }

        [Fact]
        public void Deserialize_FullServices_MapsFirstServiceFields()
        {
            var ss = Deserialize<SystemServices>(FullSystemServices);
            var svc = ss.Services[0];

            svc.Name.Should().Be("ctiService");
            svc.Status.Should().Be("running");
            svc.Mode.Should().Be("active");
        }

        [Fact]
        public void Deserialize_EmptyServices_ReturnsEmptyList()
        {
            var ss = Deserialize<SystemServices>(MinimalSystemServices);
            ss.Services.Should().BeEmpty();
        }

        #endregion

        #region HA fields

        [Fact]
        public void Deserialize_FullServices_MapsGlobalIpAddress()
        {
            var ss = Deserialize<SystemServices>(FullSystemServices);
            ss.GlobalIPAdress.Should().Be("192.168.1.100");
        }

        [Fact]
        public void Deserialize_DrbdKey_MapsToDrbdStatus()
        {
            var ss = Deserialize<SystemServices>(FullSystemServices);
            ss.DrbdStatus.Should().Be("synced");
        }

        [Fact]
        public void Deserialize_Minimal_HaFieldsAreNull()
        {
            var ss = Deserialize<SystemServices>(MinimalSystemServices);
            ss.GlobalIPAdress.Should().BeNull();
            ss.DrbdStatus.Should().BeNull();
        }

        #endregion
    }
}
