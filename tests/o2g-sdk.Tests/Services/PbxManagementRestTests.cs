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
using o2g.Types.ManagementNS;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class PbxManagementRestTests : ServiceTestBase
    {
        private static readonly System.Uri PbxMgmtUri = new("https://fake-o2g/api/pbxmanagement");

        private PbxManagementRest Service() =>
            DependancyResolver.Resolve(new PbxManagementRest(PbxMgmtUri));

        #region GetPbxsAsync

        [Fact]
        public async Task GetPbxsAsync_ReturnsNodeIdList()
        {
            SetupHttpClient("""{ "nodeIds": ["1", "2"] }""");

            var nodes = await Service().GetPbxsAsync();

            nodes.Should().NotBeNull();
            nodes.Should().HaveCount(2);
            nodes.Should().ContainInOrder(1, 2);
            AssertRequest().Method(HttpMethod.Get).Uri("/api/pbxmanagement");
        }

        [Fact]
        public async Task GetPbxsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var nodes = await Service().GetPbxsAsync();

            nodes.Should().BeNull();
        }

        #endregion

        #region GetPbxAsync

        [Fact]
        public async Task GetPbxAsync_ReturnsPbx()
        {
            SetupHttpClient("""{ "nodeId": 1, "fqdn": "pbx01.example.com" }""");

            var pbx = await Service().GetPbxAsync(1);

            pbx.Should().NotBeNull();
            pbx.NodeId.Should().Be(1);
            pbx.Fqdn.Should().Be("pbx01.example.com");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/pbxmanagement/1");
        }

        [Fact]
        public async Task GetPbxAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var pbx = await Service().GetPbxAsync(1);

            pbx.Should().BeNull();
        }

        #endregion

        #region GetNodeObjectAsync

        [Fact]
        public async Task GetNodeObjectAsync_ReturnsPbxObject()
        {
            SetupHttpClient("""
                {
                    "objectName": "Node",
                    "objectId": "1",
                    "objectNames": ["Subscriber", "ACD2"],
                    "delete": false,
                    "set": false
                }
                """);

            var obj = await Service().GetNodeObjectAsync(1);

            obj.Should().NotBeNull();
            obj.ObjectName.Should().Be("Node");
            obj.Id.Should().Be("1");
            obj.ObjectNames.Should().Contain("Subscriber");
            obj.Delete.Should().BeFalse();
            obj.Set.Should().BeFalse();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/pbxmanagement/1/instances");
        }

        [Fact]
        public async Task GetNodeObjectAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var obj = await Service().GetNodeObjectAsync(1);

            obj.Should().BeNull();
        }

        #endregion

        #region GetObjectAsync

        [Fact]
        public async Task GetObjectAsync_ReturnsPbxObject()
        {
            SetupHttpClient("""
                {
                    "objectName": "Subscriber",
                    "objectId": "1001",
                    "delete": true,
                    "set": true,
                    "attributes": [
                        { "name": "Mnemonic", "value": ["JDOE"] }
                    ]
                }
                """);

            var obj = await Service().GetObjectAsync(1, "Subscriber", "1001", (string?)null);

            obj.Should().NotBeNull();
            obj.ObjectName.Should().Be("Subscriber");
            obj.Id.Should().Be("1001");
            obj.Delete.Should().BeTrue();
            obj.Set.Should().BeTrue();
            obj.Attributes.Should().NotBeNull();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/pbxmanagement/1/instances/Subscriber/1001");
        }

        [Fact]
        public async Task GetObjectAsync_WithAttributes_AppendsQueryParam()
        {
            SetupHttpClient("""{ "objectName": "Subscriber", "objectId": "1001", "delete": false, "set": false }""");

            await Service().GetObjectAsync(1, "Subscriber", "1001", "Mnemonic,StationRslt");

            AssertRequest().Uri("/api/pbxmanagement/1/instances/Subscriber/1001?attributes=Mnemonic%2CStationRslt");
        }

        [Fact]
        public async Task GetObjectAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var obj = await Service().GetObjectAsync(1, "Subscriber", "1001", (string?)null);

            obj.Should().BeNull();
        }

        #endregion

        #region GetObjectInstancesAsync

        [Fact]
        public async Task GetObjectInstancesAsync_ReturnsIdList()
        {
            SetupHttpClient("""{ "objectIds": ["1001", "1002", "1003"] }""");

            var ids = await Service().GetObjectInstancesAsync(1, "Subscriber", (string?)null);

            ids.Should().NotBeNull();
            ids.Should().HaveCount(3);
            ids.Should().Contain("1001");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/pbxmanagement/1/instances/Subscriber");
        }

        [Fact]
        public async Task GetObjectInstancesAsync_WithFilter_AppendsQueryParam()
        {
            SetupHttpClient("""{ "objectIds": ["1001"] }""");

            await Service().GetObjectInstancesAsync(1, "Subscriber", "Mnemonic=JDOE");

            AssertRequest().Uri("/api/pbxmanagement/1/instances/Subscriber?filter=Mnemonic%3DJDOE");
        }

        [Fact]
        public async Task GetObjectInstancesAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var ids = await Service().GetObjectInstancesAsync(1, "Subscriber", (string?)null);

            ids.Should().BeNull();
        }

        #endregion

        #region GetObjectModelAsync

        [Fact]
        public async Task GetObjectModelAsync_ReturnsModel()
        {
            SetupHttpClient("""
                {
                    "name": "Node",
                    "hidden": false,
                    "create": false,
                    "delete": false,
                    "set": true,
                    "get": true
                }
                """);

            var model = await Service().GetObjectModelAsync(1);

            model.Should().NotBeNull();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/pbxmanagement/1/model");
        }

        [Fact]
        public async Task GetObjectModelAsync_WithObjectName_AppendsPathSegment()
        {
            SetupHttpClient("""
                {
                    "name": "Subscriber",
                    "hidden": false,
                    "create": true,
                    "delete": true,
                    "set": true,
                    "get": true
                }
                """);

            var model = await Service().GetObjectModelAsync(1, "Subscriber");

            model.Should().NotBeNull();
            AssertRequest().Uri("/api/pbxmanagement/1/model/Subscriber");
        }

        [Fact]
        public async Task GetObjectModelAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var model = await Service().GetObjectModelAsync(1);

            model.Should().BeNull();
        }

        #endregion

        #region SetObjectAsync

        [Fact]
        public async Task SetObjectAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var attrs = new List<PbxAttribute> { PbxAttribute.Create("Mnemonic", "JDOE") };
            var result = await Service().SetObjectAsync(1, "Subscriber", "1001", attrs);

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/pbxmanagement/1/instances/Subscriber/1001")
                .JsonBody(json =>
                {
                    json.AssertValue("$.attributes[0].name", "Mnemonic");
                    json.AssertValue("$.attributes[0].value[0]", "JDOE");
                });
        }

        [Fact]
        public async Task SetObjectAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().SetObjectAsync(
                1, "Subscriber", "1001",
                new List<PbxAttribute> { PbxAttribute.Create("Mnemonic", "JDOE") });

            result.Should().BeFalse();
        }

        #endregion

        #region CreateObjectAsync

        [Fact]
        public async Task CreateObjectAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var attrs = new List<PbxAttribute> { PbxAttribute.Create("Mnemonic", "NEWUSER") };
            var result = await Service().CreateObjectAsync(1, "Subscriber", attrs);

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/pbxmanagement/1/instances/Subscriber")
                .JsonBody(json =>
                {
                    json.AssertValue("$.attributes[0].name", "Mnemonic");
                    json.AssertValue("$.attributes[0].value[0]", "NEWUSER");
                });
        }

        #endregion

        #region DeleteObjectAsync

        [Fact]
        public async Task DeleteObjectAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().DeleteObjectAsync(1, "Subscriber", "1001");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Delete, "/api/pbxmanagement/1/instances/Subscriber/1001");
        }

        [Fact]
        public async Task DeleteObjectAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().DeleteObjectAsync(1, "Subscriber", "1001");

            result.Should().BeFalse();
        }

        #endregion
    }
}
