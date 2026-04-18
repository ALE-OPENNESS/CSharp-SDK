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
    public class ForwardTests : JsonTestBase
    {
        #region ToForward — Number destination

        [Theory]
        [InlineData("BUSY", Forward.ForwardCondition.Busy)]
        [InlineData("NO_ANSWER", Forward.ForwardCondition.NoAnswer)]
        [InlineData("BUSY_NO_ANSWER", Forward.ForwardCondition.BusyOrNoAnswer)]
        public void ToForward_NumberDestination_AllConditions_MapsCorrectly(
            string jsonType, Forward.ForwardCondition expectedCondition)
        {
            var json = $$"""
                {
                    "forwardType": "{{jsonType}}",
                    "destinations": [
                        { "type": "NUMBER", "number": "1234" }
                    ]
                }
                """;

            var forward = Deserialize<ForwardRoute>(json).ToForward();

            forward.Destination.Should().Be(Destination.Number);
            forward.Number.Should().Be("1234");
            forward.Condition.Should().Be(expectedCondition);
        }

        [Fact]
        public void ToForward_NumberDestination_Immediate_MapsCorrectly()
        {
            // ForwardType null/unknown maps to Immediate
            var json = """
                {
                    "forwardType": null,
                    "destinations": [
                        { "type": "NUMBER", "number": "5678" }
                    ]
                }
                """;

            var forward = Deserialize<ForwardRoute>(json).ToForward();

            forward.Destination.Should().Be(Destination.Number);
            forward.Number.Should().Be("5678");
            forward.Condition.Should().Be(Forward.ForwardCondition.Immediate);
        }

        #endregion

        #region ToForward — VoiceMail destination

        [Theory]
        [InlineData("BUSY", Forward.ForwardCondition.Busy)]
        [InlineData("NO_ANSWER", Forward.ForwardCondition.NoAnswer)]
        [InlineData("BUSY_NO_ANSWER", Forward.ForwardCondition.BusyOrNoAnswer)]
        public void ToForward_VoiceMailDestination_AllConditions_MapsCorrectly(
            string jsonType, Forward.ForwardCondition expectedCondition)
        {
            var json = $$"""
                {
                    "forwardType": "{{jsonType}}",
                    "destinations": [
                        { "type": "VOICEMAIL" }
                    ]
                }
                """;

            var forward = Deserialize<ForwardRoute>(json).ToForward();

            forward.Destination.Should().Be(Destination.VoiceMail);
            forward.Number.Should().BeNull();
            forward.Condition.Should().Be(expectedCondition);
        }

        #endregion

        #region ToForward — None destination

        [Fact]
        public void ToForward_UnknownDestinationType_MapsToNone()
        {
            var json = """
                {
                    "forwardType": "BUSY",
                    "destinations": [
                        { "type": "UNKNOWN" }
                    ]
                }
                """;

            var forward = Deserialize<ForwardRoute>(json).ToForward();

            forward.Destination.Should().Be(Destination.None);
            forward.Number.Should().BeNull();
        }

        #endregion

        #region CreateForwardOnNumber

        [Theory]
        [InlineData(Forward.ForwardCondition.Busy, "BUSY")]
        [InlineData(Forward.ForwardCondition.NoAnswer, "NO_ANSWER")]
        [InlineData(Forward.ForwardCondition.BusyOrNoAnswer, "BUSY_NO_ANSWER")]
        public void CreateForwardOnNumber_AllConditions_SetsCorrectForwardType(
            Forward.ForwardCondition condition, string expectedType)
        {
            var route = ForwardRoute.CreateForwardOnNumber("1234", condition);

            route.ForwardType.Should().Be(expectedType);
            route.Destinations.Should().HaveCount(1);
            route.Destinations[0].Type.Should().Be("NUMBER");
            route.Destinations[0].Number.Should().Be("1234");
        }

        [Fact]
        public void CreateForwardOnNumber_Immediate_ForwardTypeIsNull()
        {
            var route = ForwardRoute.CreateForwardOnNumber("1234",
                Forward.ForwardCondition.Immediate);

            route.ForwardType.Should().BeNull();
            route.Destinations[0].Type.Should().Be("NUMBER");
        }

        #endregion

        #region CreateForwardOnVoiceMail

        [Theory]
        [InlineData(Forward.ForwardCondition.Busy, "BUSY")]
        [InlineData(Forward.ForwardCondition.NoAnswer, "NO_ANSWER")]
        [InlineData(Forward.ForwardCondition.BusyOrNoAnswer, "BUSY_NO_ANSWER")]
        public void CreateForwardOnVoiceMail_AllConditions_SetsCorrectForwardType(
            Forward.ForwardCondition condition, string expectedType)
        {
            var route = ForwardRoute.CreateForwardOnVoiceMail(condition);

            route.ForwardType.Should().Be(expectedType);
            route.Destinations.Should().HaveCount(1);
            route.Destinations[0].Type.Should().Be("VOICEMAIL");
        }

        [Fact]
        public void CreateForwardOnVoiceMail_Immediate_ForwardTypeIsNull()
        {
            var route = ForwardRoute.CreateForwardOnVoiceMail(
                Forward.ForwardCondition.Immediate);

            route.ForwardType.Should().BeNull();
            route.Destinations[0].Type.Should().Be("VOICEMAIL");
        }

        #endregion
    }
}
