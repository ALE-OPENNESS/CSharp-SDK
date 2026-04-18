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
using o2g.Tests.Helpers;
using o2g.Types.TelephonyNS;
using o2g.Types.TelephonyNS.DeviceNS;
using o2g.Types.TelephonyNS.UserNS;
using System.Text.Json;
using Xunit;

namespace o2g.Tests.Services
{
    public class TelephonicStateTests : JsonTestBase
    {
        [Fact]
        public void Deserialize_FullState_MapsAllProperties()
        {
            var json = """
                {
                    "calls": [
                        {
                            "callRef": "call-001",
                            "legs": [],
                            "participants": []
                        }
                    ],
                    "deviceCapabilities": [
                        {
                            "deviceId": "1001",
                            "makeCall": true,
                            "makeBusinessCall": false,
                            "makePrivateCall": false,
                            "unParkCall": true
                        }
                    ],
                    "userState": "BUSY",
                    "deviceStates": [
                        {
                            "deviceId": "1001",
                            "state": "IN_SERVICE"
                        }
                    ]
                }
                """;

            var state = Deserialize<TelephonicState>(json);

            state.Should().NotBeNull();

            state.Calls.Should().HaveCount(1);
            state.Calls[0].CallRef.Should().Be("call-001");

            state.DeviceCapabilities.Should().HaveCount(1);
            state.DeviceCapabilities[0].DeviceId.Should().Be("1001");
            state.DeviceCapabilities[0].MakeCall.Should().BeTrue();
            state.DeviceCapabilities[0].UnParkCall.Should().BeTrue();
            state.DeviceCapabilities[0].MakeBusinessCall.Should().BeFalse();

            state.UserState.Should().Be(UserState.Busy);

            state.DeviceStates.Should().HaveCount(1);
            state.DeviceStates[0].DeviceId.Should().Be("1001");
            state.DeviceStates[0].State.Should().Be(OperationalState.InService);
        }

        [Fact]
        public void Deserialize_EmptyState_ReturnsEmptyCollections()
        {
            var json = """
                {
                    "calls": [],
                    "deviceCapabilities": [],
                    "userState": "FREE",
                    "deviceStates": []
                }
                """;

            var state = Deserialize<TelephonicState>(json);

            state.Calls.Should().BeEmpty();
            state.DeviceCapabilities.Should().BeEmpty();
            state.UserState.Should().Be(UserState.Free);
            state.DeviceStates.Should().BeEmpty();
        }

        [Fact]
        public void Deserialize_NullCollections_ReturnsNullCollections()
        {
            var json = """
                {
                    "userState": "FREE"
                }
                """;

            var state = Deserialize<TelephonicState>(json);

            state.Calls.Should().BeNull();
            state.DeviceCapabilities.Should().BeNull();
            state.DeviceStates.Should().BeNull();
        }

        [Fact]
        public void Deserialize_UnknownUserState_FallsBackToUnknown()
        {
            var json = """
                {
                    "calls": [],
                    "deviceCapabilities": [],
                    "userState": "SOME_FUTURE_VALUE",
                    "deviceStates": []
                }
                """;

            var state = Deserialize<TelephonicState>(json);

            state.UserState.Should().Be(UserState.Unknown);
        }

        [Fact]
        public void Deserialize_UnknownOperationalState_FallsBackToUnknown()
        {
            var json = """
                {
                    "calls": [],
                    "deviceCapabilities": [],
                    "userState": "FREE",
                    "deviceStates": [
                        {
                            "deviceId": "1001",
                            "state": "SOME_FUTURE_VALUE"
                        }
                    ]
                }
                """;

            var state = Deserialize<TelephonicState>(json);

            state.DeviceStates[0].State.Should().Be(OperationalState.Unknown);
        }
    }
}