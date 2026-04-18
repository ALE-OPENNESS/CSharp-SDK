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

using o2g.Types.CommonNS;
using o2g.Types.TelephonyNS;
using o2g.Types.TelephonyNS.CallNS;
using o2g.Types.TelephonyNS.CallNS.AcdNS;
using System.Collections.Generic;
using System.Text;

namespace o2g.Internal.Types.Telephony
{
    internal class O2GCallData
    {
        public PartyInfo InitialCalled { get; set; }
        public PartyInfo LastRedirecting { get; set; }
        public bool DeviceCall { get; set; }
        public bool Anonymous { get; set; }
        public string CallUUID { get; set; }
        public MediaState State { get; set; }
        public RecordState RecordState { get; set; }
        public List<Tag> Tags { get; set; }
        public CallCapabilities Capabilities { get; set; }
        public string AssociateData { get; set; }
        public string HexaBinaryAssociatedData { get; set; }
        public string AccountInfo { get; set; }
        public AcdData AcdCallData { get; set; }
        public TrunkIdentification TrunkIdentification { get; set; }

        public CallData ToCallData()
        {
            CorrelatorData correlatorData = null;
            if (AssociateData != null)
            {
                correlatorData = new CorrelatorData(AssociateData);
            }
            else if (HexaBinaryAssociatedData != null)
            {
                correlatorData = new CorrelatorData(Encoding.UTF8.GetBytes(HexaBinaryAssociatedData));
            }

            return new CallData
            {
                InitialCalled = InitialCalled,
                LastRedirecting = LastRedirecting,
                DeviceCall = DeviceCall,
                Anonymous = Anonymous,
                CallUUID = CallUUID,
                State = State,
                RecordState = RecordState,
                Tags = Tags,
                Capabilities = Capabilities,
                CorrelatorData = correlatorData,
                AccountInfo = AccountInfo,
                AcdCallData = AcdCallData,
                TrunkIdentification = TrunkIdentification
            };
        }
    }
}