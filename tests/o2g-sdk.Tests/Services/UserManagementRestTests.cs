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
using o2g.Types.CommonNS;
using o2g.Types.UsersNS;
using System.Net;
using System.Net.Http;

namespace o2g.Tests.Services
{
    public class UserManagementRestTests : ServiceTestBase
    {
        private static readonly Uri UserManagementUri = new("https://fake-o2g/api/usermanagement");

        private UserManagementRest Service() =>
            DependancyResolver.Resolve(new UserManagementRest(UserManagementUri));

        private const string SingleUserJson = """
            {
                "companyPhone": "3000",
                "firstName": "John",
                "lastName": "Doe",
                "loginName": "jdoe",
                "externalLogin": "john.doe@example.com",
                "voicemail": { "number": "4000", "type": "VM_4635" },
                "devices": [
                    { "type": "DESKPHONE", "id": "3000", "subType": "8082" }
                ],
                "nodeId": "1"
            }
            """;

        private const string UsersListJson = """
            {
                "users": [
                    {
                        "loginName": "jdoe",
                        "firstName": "John",
                        "lastName": "Doe",
                        "nodeId": "1"
                    },
                    {
                        "loginName": "jsmith",
                        "firstName": "Jane",
                        "lastName": "Smith",
                        "nodeId": "1"
                    }
                ]
            }
            """;

        #region GetLoginsAsync

        [Fact]
        public async Task GetLoginsAsync_WithoutNodeIds_SendsGetToBaseUri()
        {
            SetupHttpClient("""{"loginNames":["jdoe","jsmith"]}""");

            await Service().GetLoginsAsync();

            AssertRequest().Method(HttpMethod.Get).Uri("/api/usermanagement");
        }

        [Fact]
        public async Task GetLoginsAsync_WithNodeIds_AppendsSemicolonSeparatedNodeIdsParam()
        {
            SetupHttpClient("""{"loginNames":["jdoe"]}""");

            await Service().GetLoginsAsync(new[] { 1, 2 });

            AssertRequest().Uri("/api/usermanagement?nodeIds=1%3B2");
        }

        [Fact]
        public async Task GetLoginsAsync_SingleNodeId_AppendsNodeIdWithoutSemicolon()
        {
            SetupHttpClient("""{"loginNames":["jdoe"]}""");

            await Service().GetLoginsAsync(new[] { 1 });

            AssertRequest().Uri("/api/usermanagement?nodeIds=1");
        }

        [Fact]
        public async Task GetLoginsAsync_ReturnsLoginsList()
        {
            SetupHttpClient("""{"loginNames":["jdoe","jsmith"]}""");

            var logins = await Service().GetLoginsAsync();

            logins.Should().HaveCount(2);
            logins.Should().Contain("jdoe");
            logins.Should().Contain("jsmith");
        }

        [Fact]
        public async Task GetLoginsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var result = await Service().GetLoginsAsync();

            result.Should().BeNull();
        }

        #endregion

        #region GetLoginAsync

        [Fact]
        public async Task GetLoginAsync_SendsGetWithDeviceNumberAsQueryNameAndValue()
        {
            SetupHttpClient("""{"loginNames":["jdoe"]}""");

            await Service().GetLoginAsync("3000");

            AssertRequest().Method(HttpMethod.Get).Uri("/api/usermanagement?3000=3000");
        }

        [Fact]
        public async Task GetLoginAsync_ReturnsFirstLoginName()
        {
            SetupHttpClient("""{"loginNames":["jdoe","jsmith"]}""");

            var login = await Service().GetLoginAsync("3000");

            login.Should().Be("jdoe");
        }

        [Fact]
        public async Task GetLoginAsync_OnEmptyLoginNames_ReturnsNull()
        {
            SetupHttpClient("""{"loginNames":[]}""");

            var login = await Service().GetLoginAsync("3000");

            login.Should().BeNull();
        }

        [Fact]
        public async Task GetLoginAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var login = await Service().GetLoginAsync("3000");

            login.Should().BeNull();
        }

        #endregion

        #region GetUserAsync

        [Fact]
        public async Task GetUserAsync_SendsGetWithLoginName()
        {
            SetupHttpClient(SingleUserJson);

            await Service().GetUserAsync("jdoe");

            AssertRequest().Method(HttpMethod.Get).Uri("/api/usermanagement/jdoe");
        }

        [Fact]
        public async Task GetUserAsync_MapsUserFields()
        {
            SetupHttpClient(SingleUserJson);

            var user = await Service().GetUserAsync("jdoe");

            user.Should().NotBeNull();
            user.LoginName.Should().Be("jdoe");
            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Doe");
            user.CompanyPhone.Should().Be("3000");
            user.ExternalLogin.Should().Be("john.doe@example.com");
            user.NodeId.Should().Be(1);
        }

        [Fact]
        public async Task GetUserAsync_MapsVoicemail()
        {
            SetupHttpClient(SingleUserJson);

            var user = await Service().GetUserAsync("jdoe");

            user.Voicemail.Should().NotBeNull();
            user.Voicemail.Number.Should().Be("4000");
            user.Voicemail.Type.Should().Be(Voicemail.VoiceMailType.VM_4635);
        }

        [Fact]
        public async Task GetUserAsync_MapsDevices()
        {
            SetupHttpClient(SingleUserJson);

            var user = await Service().GetUserAsync("jdoe");

            user.Devices.Should().HaveCount(1);
            user.Devices[0].Id.Should().Be("3000");
            user.Devices[0].Type.Should().Be(Device.DeviceType.Deskphone);
            user.Devices[0].SubType.Should().Be("8082");
        }

        [Fact]
        public async Task GetUserAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var user = await Service().GetUserAsync("jdoe");

            user.Should().BeNull();
        }

        #endregion

        #region CreateUsersAsync

        [Fact]
        public async Task CreateUsersAsync_SendsPostToBaseUri()
        {
            SetupHttpClient(UsersListJson);

            await Service().CreateUsersAsync(1, new[] { "3000", "3001" });

            AssertRequest().Method(HttpMethod.Post).Uri("/api/usermanagement");
        }

        [Fact]
        public async Task CreateUsersAsync_BodyContainsNodeIdDeviceNumbersAndAllFalse()
        {
            SetupHttpClient(UsersListJson);

            await Service().CreateUsersAsync(1, new[] { "3000", "3001" });

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.nodeId", "1");
                j.AssertValue("$.deviceNumbers[0]", "3000");
                j.AssertValue("$.deviceNumbers[1]", "3001");
                j.AssertValue("$.all", false);
            });
        }

        [Fact]
        public async Task CreateUsersAsync_ReturnsCreatedUsersList()
        {
            SetupHttpClient(UsersListJson);

            var users = await Service().CreateUsersAsync(1, new[] { "3000", "3001" });

            users.Should().HaveCount(2);
            users[0].LoginName.Should().Be("jdoe");
            users[1].LoginName.Should().Be("jsmith");
        }

        [Fact]
        public async Task CreateUsersAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var users = await Service().CreateUsersAsync(1, new[] { "3000" });

            users.Should().BeNull();
        }

        #endregion

        #region CreateAllUsersAsync

        [Fact]
        public async Task CreateAllUsersAsync_SendsPostToBaseUri()
        {
            SetupHttpClient(UsersListJson);

            await Service().CreateAllUsersAsync(1);

            AssertRequest().Method(HttpMethod.Post).Uri("/api/usermanagement");
        }

        [Fact]
        public async Task CreateAllUsersAsync_BodyHasAllTrueAndNoDeviceNumbers()
        {
            SetupHttpClient(UsersListJson);

            await Service().CreateAllUsersAsync(1);

            await AssertRequest().JsonBody(j =>
            {
                j.AssertValue("$.nodeId", "1");
                j.AssertValue("$.all", true);
                j.AssertNull("$.deviceNumbers");
            });
        }

        [Fact]
        public async Task CreateAllUsersAsync_ReturnsCreatedUsersList()
        {
            SetupHttpClient(UsersListJson);

            var users = await Service().CreateAllUsersAsync(1);

            users.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateAllUsersAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var users = await Service().CreateAllUsersAsync(1);

            users.Should().BeNull();
        }

        #endregion

        #region DeleteUserAsync

        [Fact]
        public async Task DeleteUserAsync_SendsDeleteWithLoginName()
        {
            SetupHttpClient("{}");

            await Service().DeleteUserAsync("jdoe");

            AssertRequest().Method(HttpMethod.Delete).Uri("/api/usermanagement/jdoe");
        }

        [Fact]
        public async Task DeleteUserAsync_OnSuccess_ReturnsTrue()
        {
            SetupHttpClient("{}");

            var result = await Service().DeleteUserAsync("jdoe");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var result = await Service().DeleteUserAsync("jdoe");

            result.Should().BeFalse();
        }

        #endregion
    }
}
