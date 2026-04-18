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
using o2g.Internal.Types.Routing;
using o2g.Tests.Helpers;
using o2g.Types.RoutingNS;
using Xunit;

namespace o2g.Tests.Types.Routing
{
    public class RoutingStateTests : JsonTestBase
    {
        #region Forward

        [Fact]
        public void ToRoutingState_WithForward_MapsForward()
        {
            var json = """
                {
                    "forwardRoutes": [
                        {
                            "forwardType": "BUSY",
                            "destinations": [ { "type": "NUMBER", "number": "1234" } ]
                        }
                    ]
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.Forward.Should().NotBeNull();
            state.Forward.Destination.Should().Be(Destination.Number);
            state.Forward.Number.Should().Be("1234");
            state.Forward.Condition.Should().Be(Forward.ForwardCondition.Busy);
        }

        [Fact]
        public void ToRoutingState_NoForwardRoutes_ForwardIsNone()
        {
            var json = """{}""";

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.Forward.Should().NotBeNull();
            state.Forward.Destination.Should().Be(Destination.None);
            state.Forward.Condition.Should().BeNull();
        }

        [Fact]
        public void ToRoutingState_EmptyForwardRoutes_ForwardIsNone()
        {
            var json = """{ "forwardRoutes": [] }""";

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.Forward.Destination.Should().Be(Destination.None);
        }

        [Fact]
        public void ToRoutingState_ForwardOnVoiceMail_MapsCorrectly()
        {
            var json = """
                {
                    "forwardRoutes": [
                        {
                            "forwardType": "NO_ANSWER",
                            "destinations": [ { "type": "VOICEMAIL" } ]
                        }
                    ]
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.Forward.Destination.Should().Be(Destination.VoiceMail);
            state.Forward.Number.Should().BeNull();
            state.Forward.Condition.Should().Be(Forward.ForwardCondition.NoAnswer);
        }

        #endregion

        #region Overflow

        [Fact]
        public void ToRoutingState_WithOverflow_MapsOverflow()
        {
            var json = """
                {
                    "overflowRoutes": [
                        {
                            "overflowType": "BUSY_NO_ANSWER",
                            "destinations": [ { "type": "VOICEMAIL" } ]
                        }
                    ]
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.Overflow.Should().NotBeNull();
            state.Overflow.Destination.Should().Be(Destination.VoiceMail);
            state.Overflow.Condition.Should().Be(Overflow.OverflowCondition.BusyOrNoAnswer);
        }

        [Fact]
        public void ToRoutingState_NoOverflowRoutes_OverflowIsNone()
        {
            var json = """{}""";

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.Overflow.Should().NotBeNull();
            state.Overflow.Destination.Should().Be(Destination.None);
            state.Overflow.Condition.Should().BeNull();
        }

        [Fact]
        public void ToRoutingState_EmptyOverflowRoutes_OverflowIsNone()
        {
            var json = """{ "overflowRoutes": [] }""";

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.Overflow.Destination.Should().Be(Destination.None);
        }

        #endregion

        #region DndState

        [Fact]
        public void ToRoutingState_DndActivated_MapsDndState()
        {
            var json = """
                {
                    "dndState": { "activate": true }
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.DndState.Should().NotBeNull();
            state.DndState.Activate.Should().BeTrue();
        }

        [Fact]
        public void ToRoutingState_NoDndState_DndStateIsNull()
        {
            var json = """{}""";

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.DndState.Should().BeNull();
        }

        #endregion

        #region RemoteExtension

        [Fact]
        public void ToRoutingState_MobileSelected_RemoteExtensionIsTrue()
        {
            var json = """
                {
                    "presentationRoutes": [
                        {
                            "destinations": [ { "type": "MOBILE", "selected": true } ]
                        }
                    ]
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.RemoteExtensionActivated.Should().BeTrue();
        }

        [Fact]
        public void ToRoutingState_MobileNotSelected_RemoteExtensionIsFalse()
        {
            var json = """
                {
                    "presentationRoutes": [
                        {
                            "destinations": [ { "type": "MOBILE", "selected": false } ]
                        }
                    ]
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.RemoteExtensionActivated.Should().BeFalse();
        }

        [Fact]
        public void ToRoutingState_NoPresentationRoutes_RemoteExtensionIsNull()
        {
            var json = """{}""";

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.RemoteExtensionActivated.Should().BeNull();
        }

        [Fact]
        public void ToRoutingState_PresentationRouteWithoutMobile_RemoteExtensionIsNull()
        {
            var json = """
                {
                    "presentationRoutes": [
                        {
                            "destinations": [ { "type": "NUMBER", "number": "1234" } ]
                        }
                    ]
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.RemoteExtensionActivated.Should().BeNull();
        }

        #endregion

        #region Full state

        [Fact]
        public void ToRoutingState_FullState_MapsAllProperties()
        {
            var json = """
                {
                    "presentationRoutes": [
                        {
                            "destinations": [ { "type": "MOBILE", "selected": true } ]
                        }
                    ],
                    "forwardRoutes": [
                        {
                            "forwardType": "BUSY",
                            "destinations": [ { "type": "NUMBER", "number": "1234" } ]
                        }
                    ],
                    "overflowRoutes": [
                        {
                            "overflowType": "NO_ANSWER",
                            "destinations": [ { "type": "VOICEMAIL" } ]
                        }
                    ],
                    "dndState": { "activate": false }
                }
                """;

            var state = Deserialize<O2GRoutingState>(json).ToRoutingState();

            state.RemoteExtensionActivated.Should().BeTrue();
            state.Forward.Destination.Should().Be(Destination.Number);
            state.Forward.Number.Should().Be("1234");
            state.Forward.Condition.Should().Be(Forward.ForwardCondition.Busy);
            state.Overflow.Destination.Should().Be(Destination.VoiceMail);
            state.Overflow.Condition.Should().Be(Overflow.OverflowCondition.NoAnswer);
            state.DndState.Activate.Should().BeFalse();
        }

        #endregion
    }
}
