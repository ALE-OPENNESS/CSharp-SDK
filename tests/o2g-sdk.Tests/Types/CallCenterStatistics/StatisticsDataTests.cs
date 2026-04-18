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
using o2g.Types.CallCenterStatisticsNS;
using Xunit;

namespace o2g.Tests.Types.CallCenterStatistics
{
    public class StatisticsDataTests : JsonTestBase
    {
        private const string FullStatisticsData = """
            {
                "supervisor": "myId",
                "agentsStats": [
                    {
                        "selectedPeriod": {
                            "periodType": "oneDay",
                            "slotType": "aQuarterOfAnHour",
                            "beginDate": "2025-09-02",
                            "endDate": "2025-09-02"
                        },
                        "timeSlot": "2025-09-02T10:00",
                        "rows": [
                            {
                                "date": "2025-09-02",
                                "login": "3000",
                                "operator": "op1",
                                "firstName": "John",
                                "lastName": "Doe",
                                "number": "3000",
                                "group": "GRP1",
                                "nbRotating": 5,
                                "nbPickedUp": 12
                            }
                        ]
                    }
                ],
                "pilotsStats": [
                    {
                        "selectedPeriod": {
                            "periodType": "oneDay",
                            "slotType": "aQuarterOfAnHour",
                            "beginDate": "2025-09-02",
                            "endDate": "2025-09-02"
                        },
                        "timeSlot": "2025-09-02T10:00",
                        "rows": [
                            {
                                "date": "2025-09-02",
                                "queueName": "CQ1",
                                "pilotName": "Pilot1",
                                "pilotNumber": "2000",
                                "nbCallsOpen": 42
                            }
                        ]
                    }
                ],
                "pilotAbandonedCalls": {
                    "selectedPeriod": {
                        "periodType": "oneDay",
                        "beginDate": "2025-09-02",
                        "endDate": "2025-09-02"
                    },
                    "rows": [
                        {
                            "date": "2025-09-02",
                            "queueName": "CQ1",
                            "pilotName": "Pilot1",
                            "pilotNumber": "2000",
                            "waitingTime": 30,
                            "abandonedOnGreetingVG": 3
                        }
                    ]
                }
            }
            """;

        #region StatisticsData root fields

        [Fact]
        public void Deserialize_SupervisorJsonField_MapsToRequesterId()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            data.RequesterId.Should().Be("myId");
        }

        [Fact]
        public void Deserialize_AgentsStats_HasOneEntry()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            data.AgentsStats.Should().HaveCount(1);
        }

        [Fact]
        public void Deserialize_PilotsStats_HasOneEntry()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            data.PilotsStats.Should().HaveCount(1);
        }

        [Fact]
        public void Deserialize_PilotAbandonedCalls_IsNotNull()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            data.PilotAbandonedCalls.Should().NotBeNull();
        }

        #endregion

        #region ObjectStatistics / SelectedPeriod

        [Fact]
        public void Deserialize_AgentsStats_TimeSlotIsMapped()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            data.AgentsStats[0].TimeSlot.Should().Be("2025-09-02T10:00");
        }

        [Fact]
        public void Deserialize_AgentsStats_SelectedPeriodDatesAreMapped()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var period = data.AgentsStats[0].SelectedPeriod;

            period.BeginDate.Should().Be("2025-09-02");
            period.EndDate.Should().Be("2025-09-02");
        }

        [Fact]
        public void Deserialize_AgentsStats_SelectedPeriodTypeIsMapped()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            data.AgentsStats[0].SelectedPeriod.PeriodType.Should().Be(DataObservationPeriod.OneDay);
        }

        [Fact]
        public void Deserialize_AgentsStats_SlotTypeIsMapped()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            data.AgentsStats[0].SelectedPeriod.SlotType.Should().Be(TimeInterval.QuarterHour);
        }

        #endregion

        #region AgentStatisticsRow

        [Fact]
        public void Deserialize_AgentRow_MapsFixedProperties()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.AgentsStats[0].Rows[0];

            row.Date.Should().Be("2025-09-02");
            row.Login.Should().Be("3000");
            row.Operator.Should().Be("op1");
            row.FirstName.Should().Be("John");
            row.LastName.Should().Be("Doe");
            row.Number.Should().Be("3000");
            row.Group.Should().Be("GRP1");
        }

        [Fact]
        public void Deserialize_AgentRow_DynamicStatsAreCaptured()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.AgentsStats[0].Rows[0];

            row.Stats.Should().ContainKey("nbRotating");
            row.Stats.Should().ContainKey("nbPickedUp");
        }

        [Fact]
        public void AgentRow_Get_ReturnsCorrectIntegerValue()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.AgentsStats[0].Rows[0];

            row.Get(AgentAttributes.nbRotating).AsInteger().Should().Be(5);
        }

        [Fact]
        public void AgentRow_Get_UnknownAttribute_ReturnsNullValue()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.AgentsStats[0].Rows[0];

            row.Get(AgentAttributes.nbHelp).AsInteger().Should().BeNull();
        }

        #endregion

        #region PilotStatisticsRow

        [Fact]
        public void Deserialize_PilotRow_MapsFixedProperties()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.PilotsStats[0].Rows[0];

            row.Date.Should().Be("2025-09-02");
            row.QueueName.Should().Be("CQ1");
            row.PilotName.Should().Be("Pilot1");
            row.PilotNumber.Should().Be("2000");
        }

        [Fact]
        public void Deserialize_PilotRow_DynamicStatsAreCaptured()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.PilotsStats[0].Rows[0];

            row.Stats.Should().ContainKey("nbCallsOpen");
        }

        [Fact]
        public void PilotRow_Get_ReturnsCorrectIntegerValue()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.PilotsStats[0].Rows[0];

            row.Get(PilotAttributes.nbCallsOpen).AsInteger().Should().Be(42);
        }

        #endregion

        #region PilotAbandonedCallsStatisticsRow

        [Fact]
        public void Deserialize_AbandonedCallRow_MapsFixedProperties()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.PilotAbandonedCalls.Rows[0];

            row.Date.Should().Be("2025-09-02");
            row.QueueName.Should().Be("CQ1");
            row.PilotName.Should().Be("Pilot1");
            row.PilotNumber.Should().Be("2000");
        }

        [Fact]
        public void Deserialize_AbandonedCallRow_MapsWaitingTime()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.PilotAbandonedCalls.Rows[0];

            row.WaitingTime.Should().Be(30);
        }

        [Fact]
        public void Deserialize_AbandonedCallRow_DynamicStatsAreCaptured()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.PilotAbandonedCalls.Rows[0];

            row.Stats.Should().ContainKey("abandonedOnGreetingVG");
        }

        [Fact]
        public void AbandonedCallRow_Get_ReturnsCorrectValue()
        {
            var data = Deserialize<StatisticsData>(FullStatisticsData);
            var row = data.PilotAbandonedCalls.Rows[0];

            row.Get(PilotAbandonedCallsAttributes.abandonedOnGreetingVG).AsInteger().Should().Be(3);
        }

        [Fact]
        public void Deserialize_AbandonedCallRow_NullWaitingTime_IsNull()
        {
            var json = """
                {
                    "supervisor": "myId",
                    "pilotAbandonedCalls": {
                        "rows": [
                            { "date": "2025-09-02", "queueName": "CQ1" }
                        ]
                    }
                }
                """;

            var data = Deserialize<StatisticsData>(json);
            data.PilotAbandonedCalls.Rows[0].WaitingTime.Should().BeNull();
        }

        #endregion
    }
}
