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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace o2g.Tests.Services
{
    public class UsersRestTests : ServiceTestBase
    {
        private static readonly System.Uri UsersUri = new("https://fake-o2g/api/users");

        private UsersRest Service() =>
            DependancyResolver.Resolve(new UsersRest(UsersUri));

        #region GetByLoginNameAsync

        [Fact]
        public async Task GetByLoginNameAsync_ReturnsUser_WhenFound()
        {
            SetupHttpClient("""
                {
                    "loginName": "jdoe",
                    "firstName": "John",
                    "lastName": "Doe"
                }
                """);

            var user = await Service().GetByLoginNameAsync("jdoe");

            user.Should().NotBeNull();
            user.LoginName.Should().Be("jdoe");
            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Doe");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/users/jdoe");
        }

        [Fact]
        public async Task GetByLoginNameAsync_ReturnsNull_WhenNotFound()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var user = await Service().GetByLoginNameAsync("unknown");

            user.Should().BeNull();
        }

        #endregion

        #region GetByCompanyPhoneAsync

        [Fact]
        public async Task GetByCompanyPhoneAsync_ReturnsUser_WhenFound()
        {
            SetupHttpClient("""
                {
                    "loginName": "jdoe",
                    "companyPhone": "1001"
                }
                """);

            var user = await Service().GetByCompanyPhoneAsync("1001");

            user.Should().NotBeNull();
            user.LoginName.Should().Be("jdoe");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/users?companyPhone=1001");
        }

        [Fact]
        public async Task GetByCompanyPhoneAsync_ReturnsNull_WhenNotFound()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var user = await Service().GetByCompanyPhoneAsync("9999");

            user.Should().BeNull();
        }

        #endregion

        #region GetLoginsAsync

        [Fact]
        public async Task GetLoginsAsync_WithNoFilter_SendsCorrectRequest()
        {
            SetupHttpClient("""{ "loginNames": ["jdoe", "asmith", "bwilson"] }""");

            var logins = await Service().GetLoginsAsync();

            logins.Should().HaveCount(3);
            logins.Should().Contain("jdoe");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/logins");
        }

        [Fact]
        public async Task GetLoginsAsync_WithIntNodeIds_AppendsNodeIdsQueryParam()
        {
            SetupHttpClient("""{ "loginNames": ["jdoe"] }""");

            await Service().GetLoginsAsync(new[] { 1, 2 });

            AssertRequest().Uri("/api/logins?nodeIds=1%3B2");
        }

        [Fact]
        public async Task GetLoginsAsync_WithOnlyACD_AppendsOnlyACDQueryParam()
        {
            SetupHttpClient("""{ "loginNames": ["jdoe"] }""");

            await Service().GetLoginsAsync(null, onlyACD: true);

            AssertRequest().Uri("/api/logins?onlyACD");
        }

        [Fact]
        public async Task GetLoginsAsync_WithNodeIdsAndOnlyACD_AppendsBothQueryParams()
        {
            SetupHttpClient("""{ "loginNames": ["jdoe"] }""");

            await Service().GetLoginsAsync(new[] { 1 }, onlyACD: true);

            AssertRequest().Uri("/api/logins?nodeIds=1&onlyACD");
        }

        [Fact]
        public async Task GetLoginsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var logins = await Service().GetLoginsAsync();

            logins.Should().BeNull();
        }

        #endregion

        #region ChangePasswordAsync

        [Fact]
        public async Task ChangePasswordAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().ChangePasswordAsync("jdoe", "oldPass", "newPass");

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/users/jdoe/password")
                .JsonBody(json =>
                {
                    json.AssertValue("$.oldPassword", "oldPass");
                    json.AssertValue("$.newPassword", "newPass");
                });
        }

        [Fact]
        public async Task ChangePasswordAsync_ReturnsFalse_OnFailure()
        {
            SetupHttpClient("", HttpStatusCode.Forbidden);

            var result = await Service().ChangePasswordAsync("jdoe", "wrongPass", "newPass");

            result.Should().BeFalse();
        }

        #endregion

        #region GetPreferencesAsync

        [Fact]
        public async Task GetPreferencesAsync_ReturnsPreferences()
        {
            SetupHttpClient("""
                {
                    "guiLanguage": "en",
                    "oxeLanguage": "en"
                }
                """);

            var prefs = await Service().GetPreferencesAsync("jdoe");

            prefs.Should().NotBeNull();
            prefs.GuiLanguage.Should().Be("en");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/users/jdoe/preferences");
        }

        [Fact]
        public async Task GetPreferencesAsync_ReturnsNull_OnError()
        {
            SetupHttpClient("", HttpStatusCode.NotFound);

            var prefs = await Service().GetPreferencesAsync("unknown");

            prefs.Should().BeNull();
        }

        #endregion

        #region GetSupportedLanguagesAsync

        [Fact]
        public async Task GetSupportedLanguagesAsync_ReturnsLanguages()
        {
            SetupHttpClient("""
                {
                    "supportedLanguages": ["en", "fr"],
                    "supportedGuiLanguages": ["en", "fr", "de"]
                }
                """);

            var languages = await Service().GetSupportedLanguagesAsync("jdoe");

            languages.Should().NotBeNull();
            languages.Languages.Should().Contain("en");
            languages.GuiLanguages.Should().HaveCount(3);
            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/users/jdoe/preferences/supportedLanguages");
        }

        #endregion
    }
}
