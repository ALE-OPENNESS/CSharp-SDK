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
using Xunit;
using o2g.Tests.Helpers;
using o2g.Types.TelephonyNS.CallNS;
using o2g.Types.CommonNS;
using System.Text;
using o2g.Internal.Types.Telephony;

namespace o2g.Tests.Types.Telephony.Call
{

    public class CallDataTests : JsonTestBase
    {
        #region JSON fixtures

        private const string MinimalCallData = """
        {
            "deviceCall": false,
            "anonymous": false,
            "callUUID": "uuid-001",
            "state": "ACTIVE",
            "recordState": "UNKNOWN"
        }
        """;

        private const string FullCallData = """
        {
            "deviceCall": true,
            "anonymous": false,
            "callUUID": "uuid-002",
            "state": "HELD",
            "recordState": "RECORDING",
            "initialCalled": {
                "id": { "loginName": "jdoe", "phoneNumber": "1001" },
                "firstName": "John",
                "lastName": "Doe",
                "type": { "main": "USER", "subType": "pbx" }
            },
            "lastRedirecting": {
                "id": { "loginName": "asmith", "phoneNumber": "1002" },
                "firstName": "Alice",
                "lastName": "Smith",
                "type": { "main": "USER", "subType": "pbx" }
            },
            "tags": [
                { "name": "transactionId", "value": "abc123", "visibilities": ["jdoe"] }
            ],
            "accountInfo": "ACC-456",
            "acdCallData": {
                "pilotNumber": "3000",
                "rsiNumber": "4000",
                "supervisedTransfer": false
            },
            "trunkIdentification": {
                "networkTimeslot": 5,
                "trunkNeqt": [10, 20]
            }
        }
        """;

        private const string WithStringAssociatedData = """
        {
            "deviceCall": false,
            "anonymous": false,
            "callUUID": "uuid-003",
            "state": "ACTIVE",
            "recordState": "UNKNOWN",
            "associateData": "transactionId=abc123"
        }
        """;

        private const string WithHexaBinaryAssociatedData = """
        {
            "deviceCall": false,
            "anonymous": false,
            "callUUID": "uuid-004",
            "state": "ACTIVE",
            "recordState": "UNKNOWN",
            "hexaBinaryAssociatedData": "transactionId=abc123"
        }
        """;

        #endregion

        [Fact]
        public void Deserialize_MinimalCallData_MapsScalarProperties()
        {
            var callData = Deserialize<O2GCallData>(MinimalCallData).ToCallData();

            callData.Should().NotBeNull();
            callData.CallUUID.Should().Be("uuid-001");
            callData.DeviceCall.Should().BeFalse();
            callData.Anonymous.Should().BeFalse();
            callData.State.Should().Be(MediaState.Active);
            callData.RecordState.Should().Be(RecordState.Unknown);
        }

        [Fact]
        public void Deserialize_MinimalCallData_NullablePropertiesAreNull()
        {
            var callData = Deserialize<O2GCallData>(MinimalCallData).ToCallData();

            callData.InitialCalled.Should().BeNull();
            callData.LastRedirecting.Should().BeNull();
            callData.Tags.Should().BeNull();
            callData.AccountInfo.Should().BeNull();
            callData.AcdCallData.Should().BeNull();
            callData.TrunkIdentification.Should().BeNull();
        }

        [Fact]
        public void Deserialize_FullCallData_MapsAllProperties()
        {
            var callData = Deserialize<O2GCallData>(FullCallData).ToCallData();

            callData.CallUUID.Should().Be("uuid-002");
            callData.DeviceCall.Should().BeTrue();
            callData.State.Should().Be(MediaState.Held);
            callData.RecordState.Should().Be(RecordState.Recording);
        }

        [Fact]
        public void Deserialize_FullCallData_MapsInitialCalled()
        {
            var callData = Deserialize<O2GCallData>(FullCallData).ToCallData();

            callData.InitialCalled.Should().NotBeNull();
            callData.InitialCalled.FirstName.Should().Be("John");
            callData.InitialCalled.LastName.Should().Be("Doe");
            callData.InitialCalled.Id.LoginName.Should().Be("jdoe");
            callData.InitialCalled.Id.PhoneNumber.Should().Be("1001");
            callData.InitialCalled.Type.Main.Should().Be(PartyInfo.ParticipantType.MainType.User);
            callData.InitialCalled.Type.SubType.Should().Be("pbx");
        }

        [Fact]
        public void Deserialize_FullCallData_MapsLastRedirecting()
        {
            var callData = Deserialize<O2GCallData>(FullCallData).ToCallData();

            callData.LastRedirecting.Should().NotBeNull();
            callData.LastRedirecting.Id.LoginName.Should().Be("asmith");
        }

        [Fact]
        public void Deserialize_FullCallData_MapsTags()
        {
            var callData = Deserialize<O2GCallData>(FullCallData).ToCallData();

            callData.Tags.Should().HaveCount(1);
            callData.Tags[0].Name.Should().Be("transactionId");
            callData.Tags[0].Value.Should().Be("abc123");
            callData.Tags[0].Visibilities.Should().ContainSingle().Which.Should().Be("jdoe");
        }

        [Fact]
        public void Deserialize_FullCallData_MapsAcdCallData()
        {
            var callData = Deserialize<O2GCallData>(FullCallData).ToCallData();

            callData.AcdCallData.Should().NotBeNull();
            callData.AcdCallData.PilotNumber.Should().Be("3000");
            callData.AcdCallData.RsiNumber.Should().Be("4000");
            callData.AcdCallData.SupervisedTransfer.Should().BeFalse();
        }

        [Fact]
        public void Deserialize_FullCallData_MapsTrunkIdentification()
        {
            var callData = Deserialize<O2GCallData>(FullCallData).ToCallData();

            callData.TrunkIdentification.Should().NotBeNull();
            callData.TrunkIdentification.NetworkTimeslot.Should().Be(5);
            callData.TrunkIdentification.TrunkNeqt.Should().BeEquivalentTo(new[] { 10, 20 });
        }

        [Fact]
        public void GetCorrelatorData_FromStringAssociatedData_ReturnsCorrectValue()
        {
            var callData = Deserialize<O2GCallData>(WithStringAssociatedData).ToCallData();

            var correlator = callData.CorrelatorData;

            correlator.Should().NotBeNull();
            correlator.AsString().Should().Be("transactionId=abc123");
        }

        [Fact]
        public void GetCorrelatorData_FromHexaBinaryAssociatedData_ReturnsCorrectBytes()
        {
            var callData = Deserialize<O2GCallData>(WithHexaBinaryAssociatedData).ToCallData();

            var correlator = callData.CorrelatorData;

            correlator.Should().NotBeNull();
            correlator.AsByteArray().Should().BeEquivalentTo(
                Encoding.UTF8.GetBytes("transactionId=abc123"));
        }

        [Fact]
        public void GetCorrelatorData_WhenAbsent_ReturnsNull()
        {
            var callData = Deserialize<O2GCallData>(MinimalCallData).ToCallData();
            callData.CorrelatorData.Should().BeNull();
        }

        [Fact]
        public void Deserialize_UnknownMediaState_FallsBackToUnknown()
        {
            var json = """
            {
                "deviceCall": false,
                "anonymous": false,
                "callUUID": "uuid-005",
                "state": "SOME_FUTURE_VALUE",
                "recordState": "UNKNOWN"
            }
            """;

            var callData = Deserialize<O2GCallData>(json).ToCallData();
            callData.State.Should().Be(MediaState.Unknown);
        }
    }
}