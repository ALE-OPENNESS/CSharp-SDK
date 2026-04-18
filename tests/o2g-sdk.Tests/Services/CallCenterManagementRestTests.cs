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
using o2g.Types.CallCenterManagementNS.CalendarNS;
using System.Net;


namespace o2g.Tests.Services
{
    public class CallCenterManagementRestTests : ServiceTestBase
    {
        private static readonly Uri ManagementUri = new("https://fake-o2g/api/ccm");

        private CallCenterManagementRest Service() =>
            DependancyResolver.Resolve(new CallCenterManagementRest(ManagementUri));

        #region GetPilotsAsync

        [Fact]
        public async Task GetPilotsAsync_ReturnsList()
        {
            SetupHttpClient("""
                {
                    "pilotList": [
                        {
                            "number": "3000",
                            "name": "Support",
                            "waitingTime": 10,
                            "saturation": false,
                            "possibleTransfer": true,
                            "supervisedTransfer": false
                        },
                        {
                            "number": "3001",
                            "name": "Sales",
                            "waitingTime": 5,
                            "saturation": false,
                            "possibleTransfer": false,
                            "supervisedTransfer": false
                        }
                    ]
                }
                """);

            var pilots = await Service().GetPilotsAsync(1);

            pilots.Should().HaveCount(2);
            pilots[0].Number.Should().Be("3000");
            pilots[0].Name.Should().Be("Support");
            pilots[1].Number.Should().Be("3001");
            AssertRequest().Method(HttpMethod.Get).Uri("/api/ccm/1/pilots");
        }

        [Fact]
        public async Task GetPilotsAsync_OnError_ReturnsNull()
        {
            SetupHttpClient("", HttpStatusCode.InternalServerError);

            var pilots = await Service().GetPilotsAsync(1);

            pilots.Should().BeNull();
        }

        #endregion

        #region GetPilotAsync

        [Fact]
        public async Task GetPilotAsync_ReturnsPilot()
        {
            SetupHttpClient("""
                {
                    "number": "3000",
                    "name": "Support",
                    "waitingTime": 10,
                    "saturation": false,
                    "possibleTransfer": true,
                    "supervisedTransfer": true
                }
                """);

            var pilot = await Service().GetPilotAsync(1, "3000");

            pilot.Should().NotBeNull();
            pilot.Number.Should().Be("3000");
            pilot.PossibleTransfer.Should().BeTrue();
            pilot.SupervisedTransfer.Should().BeTrue();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/ccm/1/pilots/3000");
        }

        [Fact]
        public async Task GetPilotAsync_WithRules_MapsRules()
        {
            SetupHttpClient("""
                {
                    "number": "3000",
                    "waitingTime": 0,
                    "saturation": false,
                    "possibleTransfer": false,
                    "supervisedTransfer": false,
                    "rules": {
                        "ruleList": [
                            { "ruleNumber": "1", "name": "Default", "active": true },
                            { "ruleNumber": "2", "name": "Overflow", "active": false }
                        ]
                    }
                }
                """);

            var pilot = await Service().GetPilotAsync(1, "3000");

            pilot.Rules.Should().NotBeNull();
            pilot.Rules.Count.Should().Be(2);
            pilot.Rules.Get(1)!.Name.Should().Be("Default");
            pilot.Rules.Get(2)!.Active.Should().BeFalse();
        }

        #endregion

        #region GetCalendarAsync

        [Fact]
        public async Task GetCalendarAsync_SendsCorrectRequest()
        {
            SetupHttpClient("""
                {
                    "normalDays": {
                        "calendar": [
                            {
                                "day": "monday",
                                "list": [
                                    { "number": 1, "transition": { "time": "08:00", "ruleNumber": 1, "mode": "normal" } }
                                ]
                            }
                        ]
                    }
                }
                """);

            var calendar = await Service().GetCalendarAsync(1, "3000");

            calendar.Should().NotBeNull();
            calendar.NormalDays.Should().NotBeNull();
            calendar.ExceptionDays.Should().BeNull();
            AssertRequest().Method(HttpMethod.Get).Uri("/api/ccm/1/pilots/3000/calendar");
        }

        #endregion

        #region GetExceptionCalendarAsync

        [Fact]
        public async Task GetExceptionCalendarAsync_SendsCorrectRequest()
        {
            SetupHttpClient("""
                {
                    "calendar": [
                        {
                            "date": "20260714",
                            "list": [
                                { "number": 1, "transition": { "time": "08:00", "ruleNumber": 1, "mode": "closed" } }
                            ]
                        }
                    ]
                }
                """);

            var calendar = await Service().GetExceptionCalendarAsync(1, "3000");

            calendar.Should().NotBeNull();
            calendar.ExceptionDates.Should().HaveCount(1);
            calendar.ExceptionDates.Should().Contain(new DateTime(2026, 7, 14));
            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/ccm/1/pilots/3000/calendar/exception");
        }

        #endregion

        #region GetNormalCalendarAsync

        [Fact]
        public async Task GetNormalCalendarAsync_SendsCorrectRequest()
        {
            SetupHttpClient("""
                {
                    "calendar": [
                        {
                            "day": "monday",
                            "list": [
                                { "number": 1, "transition": { "time": "08:00", "ruleNumber": 1, "mode": "normal" } },
                                { "number": 2, "transition": { "time": "18:00", "ruleNumber": 2, "mode": "closed" } }
                            ]
                        }
                    ]
                }
                """);

            var calendar = await Service().GetNormalCalendarAsync(1, "3000");

            calendar.Should().NotBeNull();
            calendar.Days.Should().Contain(DayOfWeek.Monday);
            calendar.GetTransitions(DayOfWeek.Monday).Should().HaveCount(2);
            AssertRequest().Method(HttpMethod.Get)
                .Uri("/api/ccm/1/pilots/3000/calendar/normal");
        }

        #endregion

        #region AddExceptionTransitionAsync

        [Fact]
        public async Task AddExceptionTransitionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var transition = new Transition
            {
                TransitionTime = Transition.Time.Parse("09:00"),
                RuleNumber = 1,
                Mode = PilotOperatingMode.Closed
            };

            var result = await Service().AddExceptionTransitionAsync(
                1, "3000", new DateTime(2026, 7, 14), transition);

            result.Should().BeTrue();
            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/ccm/1/pilots/3000/calendar/exception/20260714/transitions")
                .JsonBody(json =>
                {
                    json.AssertValue("$.time", "09:00");
                    json.AssertValue("$.ruleNumber", 1);
                    json.AssertValue("$.mode", "closed");
                });
        }

        #endregion

        #region DeleteExceptionTransitionAsync

        [Fact]
        public async Task DeleteExceptionTransitionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            // transitionIndex is 0-based — should be sent as 1-based (index+1=1)
            await Service().DeleteExceptionTransitionAsync(
                1, "3000", new DateTime(2026, 7, 14), 0);

            await AssertCalledWith(HttpMethod.Delete,
                "/api/ccm/1/pilots/3000/calendar/exception/20260714/transitions/1");
        }

        [Fact]
        public async Task DeleteExceptionTransitionAsync_SecondIndex_SendsOneBasedIndex()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            // transitionIndex 1 (0-based) → sent as 2 (1-based)
            await Service().DeleteExceptionTransitionAsync(
                1, "3000", new DateTime(2026, 7, 14), 1);

            await AssertCalledWith(HttpMethod.Delete,
                "/api/ccm/1/pilots/3000/calendar/exception/20260714/transitions/2");
        }

        #endregion

        #region SetExceptionTransitionAsync

        [Fact]
        public async Task SetExceptionTransitionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var transition = new Transition
            {
                TransitionTime = Transition.Time.Parse("10:00"),
                RuleNumber = 2,
                Mode = PilotOperatingMode.Forward
            };

            await Service().SetExceptionTransitionAsync(
                1, "3000", new DateTime(2026, 7, 14), 0, transition);

            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/ccm/1/pilots/3000/calendar/exception/20260714/transitions/1")
                .JsonBody(json =>
                {
                    json.AssertValue("$.time", "10:00");
                    json.AssertValue("$.ruleNumber", 2);
                    json.AssertValue("$.mode", "forward");
                });
        }

        #endregion

        #region AddNormalTransitionAsync

        [Fact]
        public async Task AddNormalTransitionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var transition = new Transition
            {
                TransitionTime = Transition.Time.Parse("08:00"),
                RuleNumber = 1,
                Mode = PilotOperatingMode.Normal
            };

            await Service().AddNormalTransitionAsync(1, "3000", DayOfWeek.Monday, transition);

            await AssertRequest()
                .Method(HttpMethod.Post)
                .Uri("/api/ccm/1/pilots/3000/calendar/normal/monday/transitions")
                .JsonBody(json =>
                {
                    json.AssertValue("$.time", "08:00");
                    json.AssertValue("$.ruleNumber", 1);
                    json.AssertValue("$.mode", "normal");
                });
        }

        [Fact]
        public async Task AddNormalTransitionAsync_Saturday_UsesLowercaseDayName()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var transition = new Transition
            {
                TransitionTime = Transition.Time.Parse("09:00"),
                RuleNumber = 1,
                Mode = PilotOperatingMode.Closed
            };

            await Service().AddNormalTransitionAsync(1, "3000", DayOfWeek.Saturday, transition);

            AssertRequest().Uri("/api/ccm/1/pilots/3000/calendar/normal/saturday/transitions");
        }

        #endregion

        #region DeleteNormalTransitionAsync

        [Fact]
        public async Task DeleteNormalTransitionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            // transitionIndex 0 (0-based) → sent as 1 (1-based)
            await Service().DeleteNormalTransitionAsync(1, "3000", DayOfWeek.Monday, 0);

            await AssertCalledWith(HttpMethod.Delete,
                "/api/ccm/1/pilots/3000/calendar/normal/monday/transitions/1");
        }

        [Fact]
        public async Task DeleteNormalTransitionAsync_SecondIndex_SendsOneBasedIndex()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            // transitionIndex 1 (0-based) → sent as 2 (1-based)
            await Service().DeleteNormalTransitionAsync(1, "3000", DayOfWeek.Friday, 1);

            await AssertCalledWith(HttpMethod.Delete,
                "/api/ccm/1/pilots/3000/calendar/normal/friday/transitions/2");
        }

        #endregion

        #region SetNormalTransitionAsync

        [Fact]
        public async Task SetNormalTransitionAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var transition = new Transition
            {
                TransitionTime = Transition.Time.Parse("18:00"),
                RuleNumber = 2,
                Mode = PilotOperatingMode.Closed
            };

            await Service().SetNormalTransitionAsync(1, "3000", DayOfWeek.Monday, 1, transition);

            await AssertRequest()
                .Method(HttpMethod.Put)
                .Uri("/api/ccm/1/pilots/3000/calendar/normal/monday/transitions/2")
                .JsonBody(json =>
                {
                    json.AssertValue("$.time", "18:00");
                    json.AssertValue("$.ruleNumber", 2);
                    json.AssertValue("$.mode", "closed");
                });
        }

        #endregion

        #region OpenPilotAsync / ClosePilotAsync

        [Fact]
        public async Task OpenPilotAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().OpenPilotAsync(1, "3000");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Post, "/api/ccm/1/pilots/3000/open");
        }

        [Fact]
        public async Task ClosePilotAsync_SendsCorrectRequest()
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var result = await Service().ClosePilotAsync(1, "3000");

            result.Should().BeTrue();
            await AssertCalledWith(HttpMethod.Post, "/api/ccm/1/pilots/3000/close");
        }

        [Fact]
        public async Task OpenPilotAsync_OnError_ReturnsFalse()
        {
            SetupHttpClient("", HttpStatusCode.BadRequest);

            var result = await Service().OpenPilotAsync(1, "3000");

            result.Should().BeFalse();
        }

        #endregion

        #region Index conversion (0-based to 1-based)

        [Theory]
        [InlineData(0, "1")]
        [InlineData(1, "2")]
        [InlineData(9, "10")]
        public async Task DeleteNormalTransition_IndexConversion_IsOneBasedInUrl(
            int zeroBasedIndex, string expectedInUrl)
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            await Service().DeleteNormalTransitionAsync(1, "3000", DayOfWeek.Monday, zeroBasedIndex);

            AssertRequest().Uri(
                $"/api/ccm/1/pilots/3000/calendar/normal/monday/transitions/{expectedInUrl}");
        }

        [Theory]
        [InlineData(DayOfWeek.Monday, "monday")]
        [InlineData(DayOfWeek.Tuesday, "tuesday")]
        [InlineData(DayOfWeek.Wednesday, "wednesday")]
        [InlineData(DayOfWeek.Thursday, "thursday")]
        [InlineData(DayOfWeek.Friday, "friday")]
        [InlineData(DayOfWeek.Saturday, "saturday")]
        [InlineData(DayOfWeek.Sunday, "sunday")]
        public async Task AddNormalTransition_AllDays_UsesLowercaseDayName(
            DayOfWeek day, string expectedDayInUrl)
        {
            SetupHttpClient("", HttpStatusCode.NoContent);

            var transition = new Transition
            {
                TransitionTime = Transition.Time.Parse("08:00"),
                RuleNumber = 1,
                Mode = PilotOperatingMode.Normal
            };

            await Service().AddNormalTransitionAsync(1, "3000", day, transition);

            AssertRequest().Uri(
                $"/api/ccm/1/pilots/3000/calendar/normal/{expectedDayInUrl}/transitions");
        }

        #endregion
    }
}