/*
* Copyright 2021 ALE International
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

using o2g.Types.CallCenterAgentNS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace o2g.Internal.Types.CallCenterAgent
{
    // Internal class to adapt the public Operator config model
    internal class O2GAgentConfig
    {
        public OperatorType Type { get; init; }
        public string Proacd { get; init; }
        public AgentGroups ProcessingGroups { get; init; }
        public O2GAgentSkills Skills { get; init; }
        public bool SelfAssign { get; init; }
        public bool Headset { get; init; }
        public bool Help { get; init; }
        public bool Multiline { get; init; }

        internal OperatorConfiguration ToOperatorConfiguration()
        {
            // Transform the AgentSkills to a SkillSet
            var mapSkills = Skills?.Skills?.ToDictionary(s => s.Number)
                            ?? new Dictionary<int, AgentSkill>();

            return new()
            {
                Type = Type,
                Proacd = Proacd,
                Groups = ProcessingGroups,
                SelfAssign = SelfAssign,
                Headset = Headset,
                Help = Help,
                Multiline = Multiline,
                Skills = new()
                {
                    Map = mapSkills
                }
            };
        }
    }
}
