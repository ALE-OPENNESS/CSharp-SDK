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
    public class OverflowTests : JsonTestBase
    {
        #region ToOverflow — VoiceMail destination

        [Fact]
        public void ToOverflow_VoiceMailBusy_MapsCorrectly()
        {
            var json = """
                {
                    "overflowType": "BUSY",
                    "destinations": [
                        { "type": "VOICEMAIL" }
                    ]
                }
                """;

            var overflow = Deserialize<OverflowRoute>(json).ToOverflow();

            overflow.Destination.Should().Be(Destination.VoiceMail);
            overflow.Condition.Should().Be(Overflow.OverflowCondition.Busy);
        }

        [Fact]
        public void ToOverflow_VoiceMailNoAnswer_MapsCorrectly()
        {
            var json = """
                {
                    "overflowType": "NO_ANSWER",
                    "destinations": [
                        { "type": "VOICEMAIL" }
                    ]
                }
                """;

            var overflow = Deserialize<OverflowRoute>(json).ToOverflow();

            overflow.Destination.Should().Be(Destination.VoiceMail);
            overflow.Condition.Should().Be(Overflow.OverflowCondition.NoAnswer);
        }

        [Fact]
        public void ToOverflow_VoiceMailBusyOrNoAnswer_MapsCorrectly()
        {
            var json = """
                {
                    "overflowType": "BUSY_NO_ANSWER",
                    "destinations": [
                        { "type": "VOICEMAIL" }
                    ]
                }
                """;

            var overflow = Deserialize<OverflowRoute>(json).ToOverflow();

            overflow.Destination.Should().Be(Destination.VoiceMail);
            overflow.Condition.Should().Be(Overflow.OverflowCondition.BusyOrNoAnswer);
        }

        #endregion

        #region ToOverflow — None destination

        [Fact]
        public void ToOverflow_UnknownDestinationType_MapsToNone()
        {
            var json = """
                {
                    "overflowType": "BUSY",
                    "destinations": [
                        { "type": "UNKNOWN" }
                    ]
                }
                """;

            var overflow = Deserialize<OverflowRoute>(json).ToOverflow();

            overflow.Destination.Should().Be(Destination.None);
        }

        #endregion

        #region CreateOverflowOnVoiceMail

        [Theory]
        [InlineData(Overflow.OverflowCondition.Busy, "BUSY")]
        [InlineData(Overflow.OverflowCondition.NoAnswer, "NO_ANSWER")]
        [InlineData(Overflow.OverflowCondition.BusyOrNoAnswer, "BUSY_NO_ANSWER")]
        public void CreateOverflowOnVoiceMail_AllConditions_SetsCorrectOverflowType(
            Overflow.OverflowCondition condition, string expectedType)
        {
            var route = OverflowRoute.CreateOverflowOnVoiceMail(condition);

            route.OverflowType.Should().Be(expectedType);
            route.Destinations.Should().HaveCount(1);
            route.Destinations[0].Type.Should().Be("VOICEMAIL");
        }

        #endregion

        #region CreateOverflowOnAssociate

        [Theory]
        [InlineData(Overflow.OverflowCondition.Busy, "BUSY")]
        [InlineData(Overflow.OverflowCondition.NoAnswer, "NO_ANSWER")]
        [InlineData(Overflow.OverflowCondition.BusyOrNoAnswer, "BUSY_NO_ANSWER")]
        public void CreateOverflowOnAssociate_AllConditions_SetsCorrectOverflowType(
            Overflow.OverflowCondition condition, string expectedType)
        {
            var route = OverflowRoute.CreateOverflowOnAssociate(condition);

            route.OverflowType.Should().Be(expectedType);
            route.Destinations.Should().HaveCount(1);
            route.Destinations[0].Type.Should().Be("ASSOCIATE");
        }

        #endregion
    }
}