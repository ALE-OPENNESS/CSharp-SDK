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
using o2g.Types.EventSummaryNS;
using Xunit;

namespace o2g.Tests.Types.EventSummaryNS
{
    public class EventSummaryTests : JsonTestBase
    {
        #region JSON fixtures

        private const string FullEventSummaryJson = """
        {
            "missedCallsNb": 3,
            "voiceMessagesNb": 1,
            "callBackRequestsNb": 2,
            "faxNb": 4,
            "newTextNb": 5,
            "oldTextNb": 10,
            "eventWaiting": true
        }
        """;

        private const string PartialEventSummaryJson = """
        {
            "missedCallsNb": 7,
            "voiceMessagesNb": 0,
            "eventWaiting": true
        }
        """;

        private const string MinimalEventSummaryJson = """
        {
            "eventWaiting": false
        }
        """;

        #endregion

        [Fact]
        public void Deserialize_FullEventSummary_MapsAllFields()
        {
            var summary = Deserialize<EventSummary>(FullEventSummaryJson);

            summary.Should().NotBeNull();
            summary.MissedCallsNb.Should().Be(3);
            summary.VoiceMessagesNb.Should().Be(1);
            summary.CallBackRequestsNb.Should().Be(2);
            summary.FaxNb.Should().Be(4);
            summary.NewTextNb.Should().Be(5);
            summary.OldTextNb.Should().Be(10);
            summary.EventWaiting.Should().BeTrue();
        }

        [Fact]
        public void Deserialize_PartialEventSummary_MissingFieldsAreNull()
        {
            var summary = Deserialize<EventSummary>(PartialEventSummaryJson);

            summary.Should().NotBeNull();
            summary.MissedCallsNb.Should().Be(7);
            summary.VoiceMessagesNb.Should().Be(0);
            summary.EventWaiting.Should().BeTrue();
            summary.CallBackRequestsNb.Should().BeNull();
            summary.FaxNb.Should().BeNull();
            summary.NewTextNb.Should().BeNull();
            summary.OldTextNb.Should().BeNull();
        }

        [Fact]
        public void Deserialize_MinimalEventSummary_AllCountersAreNull()
        {
            var summary = Deserialize<EventSummary>(MinimalEventSummaryJson);

            summary.Should().NotBeNull();
            summary.EventWaiting.Should().BeFalse();
            summary.MissedCallsNb.Should().BeNull();
            summary.VoiceMessagesNb.Should().BeNull();
            summary.CallBackRequestsNb.Should().BeNull();
            summary.FaxNb.Should().BeNull();
            summary.NewTextNb.Should().BeNull();
            summary.OldTextNb.Should().BeNull();
        }
    }
}
