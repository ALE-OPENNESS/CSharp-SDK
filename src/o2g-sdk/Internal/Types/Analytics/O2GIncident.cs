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

using o2g.Types.AnalyticsNS;
using System;
using System.Globalization;

namespace o2g.Internal.Types.Analytics
{
    internal class O2GIncident
    {
        public string Date { get; set; }
        public string Hour { get; set; }
        public int Severity { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int NbOccurs { get; set; }
        public string Node { get; set; }
        public bool Main { get; set; }
        public string Rack { get; set; }
        public string Board { get; set; }
        public string Equipement { get; set; }
        public string Termination { get; set; }

        internal Incident ToIncident()
        {
            return new()
            {
                Id = int.Parse(Value),
                Date = DateTime.ParseExact(string.Format("{0}-{1}", Date.Trim(), Hour.Trim()), "dd/MM/yy-HH:mm:ss", CultureInfo.InvariantCulture),
                Severity = Severity,
                Description = Type,
                NbOccurs = NbOccurs,
                Node = int.Parse(Node),
                Main = Main,
                Rack = Rack,
                Board = Board,
                Equipment = Equipement,
                Termination = Termination
            };
        }
    }
}
