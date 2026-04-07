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
using System.Collections.Generic;
using System.Globalization;

namespace o2g.Internal.Types.Analytics
{
    class TelFacilities
    {
        public List<TelFacility> Facilities { get; set; }
    }


    internal class O2GCharging
    {
        public string Caller { get; set; }
        public string Name { get; set; }
        public string Called { get; set; }
        public string InitialDialledNumber { get; set; }
        public int CallNumber { get; set; }
        public int ChargingUnits { get; set; }
        public float Cost { get; set; }
        public string StartDate { get; set; }
        public int Duration { get; set; }
        public CallType CallType { get; set; }
        public int EffectiveCallDuration { get; set; }
        public int ActingExtensionNumberNode { get; set; }
        public TelFacilities InternalFacilities { get; set; }
        public TelFacilities ExternalFacilities { get; set; }

        internal Charging ToCharging()
        {
            return new()
            {
                Caller = Caller,
                Name = Name,
                Called = Called,
                InitialDialedNumber = InitialDialledNumber,
                CallNumber = CallNumber,
                ChargingUnits = ChargingUnits,
                Cost = Cost,
                StartDate = DateTime.ParseExact(StartDate, "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture),
                Duration = Duration,
                CallType = CallType,
                EffectiveCallDuration = EffectiveCallDuration,
                ActingExtensionNumberNode = ActingExtensionNumberNode,
                InternalFacilities = InternalFacilities?.Facilities,
                ExternalFacilities = ExternalFacilities?.Facilities
            };
        }
    }
}
