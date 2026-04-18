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

using o2g.Types.CallCenterManagementNS;
using o2g.Types.CommonNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using System.Collections.Generic;

namespace o2g.Internal.Types.CallCenterManagementNS
{
    internal class O2GPilotRules
    {
        public List<O2GPilotRule> RuleList { get; set; }
    }

    internal class O2GPilotRule
    {
        public string RuleNumber { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        internal int GetNumber() =>
            RuleNumber == null ? -1 : int.Parse(RuleNumber);

        internal PilotRule ToPilotRule() => new PilotRule
        {
            RuleNumber = GetNumber(),
            Name = Name,
            Active = Active
        };
    }

    internal class O2GPilot
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public ServiceState? State { get; set; }
        public PilotStatus? DetailedState { get; set; }
        public int WaitingTime { get; set; }
        public bool Saturation { get; set; }
        public O2GPilotRules Rules { get; set; }
        public bool PossibleTransfer { get; set; }
        public bool SupervisedTransfer { get; set; }

        internal Pilot ToPilot()
        {
            var mapRules = new Dictionary<int, PilotRule>();
            if (Rules?.RuleList != null)
            {
                foreach (var r in Rules.RuleList)
                    mapRules[r.GetNumber()] = r.ToPilotRule();
            }

            return new Pilot
            {
                Number = Number,
                Name = Name,
                State = State,
                WaitingTime = WaitingTime,
                Saturation = Saturation,
                DetailedState = DetailedState,
                Rules = new PilotRuleSet(mapRules.Values),
                PossibleTransfer = PossibleTransfer,
                SupervisedTransfer = SupervisedTransfer
            };
        }
    }
}