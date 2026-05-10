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
using o2g.Types.CommonNS;
using o2g.Types.UsersNS;
using Xunit;

namespace o2g.Tests.Types.Users
{
    public class UserTests : JsonTestBase
    {
        private const string FullUser = """
            {
                "companyPhone": "3000",
                "firstName": "John",
                "lastName": "Doe",
                "loginName": "jdoe",
                "externalLogin": "john.doe@example.com",
                "eMailAddress": "john.doe@mycompany.com",
                "voicemail": {
                    "number": "4000",
                    "type": "VM_4635"
                },
                "devices": [
                    { "type": "DESKPHONE", "id": "3000", "subType": "8082" },
                    { "type": "MOBILE", "id": "mob001" }
                ],
                "nodeId": "1"
            }
            """;

        private const string MinimalUser = """
            {
                "loginName": "jdoe",
                "nodeId": "2"
            }
            """;

        #region Scalar properties

        [Fact]
        public void Deserialize_FullUser_MapsScalarProperties()
        {
            var user = Deserialize<User>(FullUser);

            user.LoginName.Should().Be("jdoe");
            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Doe");
            user.CompanyPhone.Should().Be("3000");
            user.ExternalLogin.Should().Be("john.doe@example.com");
            user.EmailAddress.Should().Be("john.doe@mycompany.com");
        }

        [Fact]
        public void Deserialize_FullUser_NodeIdConvertedFromString()
        {
            var user = Deserialize<User>(FullUser);

            user.NodeId.Should().Be(1);
        }

        [Fact]
        public void Deserialize_MinimalUser_NullablePropertiesAreNull()
        {
            var user = Deserialize<User>(MinimalUser);

            user.FirstName.Should().BeNull();
            user.LastName.Should().BeNull();
            user.CompanyPhone.Should().BeNull();
            user.ExternalLogin.Should().BeNull();
            user.Voicemail.Should().BeNull();
            user.Devices.Should().BeNull();
        }

        [Fact]
        public void Deserialize_MinimalUser_NodeIdConvertedFromString()
        {
            var user = Deserialize<User>(MinimalUser);

            user.NodeId.Should().Be(2);
        }

        #endregion

        #region Voicemail

        [Fact]
        public void Deserialize_FullUser_MapsVoicemailNumber()
        {
            var user = Deserialize<User>(FullUser);

            user.Voicemail.Number.Should().Be("4000");
        }

        [Fact]
        public void Deserialize_Voicemail_VM4635Type_MapsCorrectly()
        {
            var user = Deserialize<User>(FullUser);

            user.Voicemail.Type.Should().Be(Voicemail.VoiceMailType.VM_4635);
        }

        [Fact]
        public void Deserialize_Voicemail_ExternalType_MapsCorrectly()
        {
            var json = """
                {
                    "loginName": "jdoe",
                    "nodeId": "1",
                    "voicemail": { "number": "4001", "type": "EXTERNAL" }
                }
                """;

            var user = Deserialize<User>(json);

            user.Voicemail.Type.Should().Be(Voicemail.VoiceMailType.External);
        }

        [Fact]
        public void Deserialize_Voicemail_VM4645Type_MapsCorrectly()
        {
            var json = """
                {
                    "loginName": "jdoe",
                    "nodeId": "1",
                    "voicemail": { "number": "4001", "type": "VM_4645" }
                }
                """;

            var user = Deserialize<User>(json);

            user.Voicemail.Type.Should().Be(Voicemail.VoiceMailType.VM_4645);
        }

        [Fact]
        public void Deserialize_Voicemail_UnknownType_FallsBackToExternal()
        {
            var json = """
                {
                    "loginName": "jdoe",
                    "nodeId": "1",
                    "voicemail": { "number": "4001", "type": "SOME_FUTURE_VM" }
                }
                """;

            var user = Deserialize<User>(json);

            user.Voicemail.Type.Should().Be(Voicemail.VoiceMailType.External);
        }

        #endregion

        #region Devices

        [Fact]
        public void Deserialize_FullUser_HasTwoDevices()
        {
            var user = Deserialize<User>(FullUser);

            user.Devices.Should().HaveCount(2);
        }

        [Fact]
        public void Deserialize_Device_DeskphoneType_MapsCorrectly()
        {
            var user = Deserialize<User>(FullUser);
            var device = user.Devices[0];

            device.Type.Should().Be(Device.DeviceType.Deskphone);
            device.Id.Should().Be("3000");
            device.SubType.Should().Be("8082");
        }

        [Fact]
        public void Deserialize_Device_MobileType_MapsCorrectly()
        {
            var user = Deserialize<User>(FullUser);
            var device = user.Devices[1];

            device.Type.Should().Be(Device.DeviceType.Mobile);
            device.Id.Should().Be("mob001");
        }

        [Fact]
        public void Deserialize_Device_DectType_MapsCorrectly()
        {
            var json = """
                {
                    "loginName": "jdoe",
                    "nodeId": "1",
                    "devices": [{ "type": "DECT", "id": "dect001" }]
                }
                """;

            var user = Deserialize<User>(json);

            user.Devices[0].Type.Should().Be(Device.DeviceType.Dect);
        }

        [Fact]
        public void Deserialize_Device_SoftphoneType_MapsCorrectly()
        {
            var json = """
                {
                    "loginName": "jdoe",
                    "nodeId": "1",
                    "devices": [{ "type": "SOFTPHONE", "id": "soft001" }]
                }
                """;

            var user = Deserialize<User>(json);

            user.Devices[0].Type.Should().Be(Device.DeviceType.Softphone);
        }

        [Fact]
        public void Deserialize_Device_UnknownType_FallsBackToUnknown()
        {
            var json = """
                {
                    "loginName": "jdoe",
                    "nodeId": "1",
                    "devices": [{ "type": "FUTURE_DEVICE", "id": "dev001" }]
                }
                """;

            var user = Deserialize<User>(json);

            user.Devices[0].Type.Should().Be(Device.DeviceType.Unknown);
        }

        [Fact]
        public void Deserialize_Device_SubTypeIsNullWhenMissing()
        {
            var user = Deserialize<User>(FullUser);

            user.Devices[1].SubType.Should().BeNull();
        }

        #endregion
    }
}
