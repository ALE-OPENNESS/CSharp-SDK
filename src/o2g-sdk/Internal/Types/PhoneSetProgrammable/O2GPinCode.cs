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

using o2g.Types.PhoneSetProgrammableNS;

namespace o2g.Internal.Types.PhoneSetProgrammable
{
    internal class O2GPinCode
    {
        public string PINNumber { get; set; }
        public bool WithSecretCode { get; set; }
        public string PinTypeOfControl { get; set; }
        public int PinGroup { get; set; }

        internal Pin ToPin()
        {
            return new()
            {
                Number = PINNumber,
                WithSecretCode = WithSecretCode,
                Group = PinGroup,
                Control = MakeControl(PinTypeOfControl)
            };
        }

        private static Pin.PinControl MakeControl(string pinTypeOfControl)
        {
            if (pinTypeOfControl == "PIN_Category") return Pin.PinControl.ByCategory;
            else if (pinTypeOfControl == "PIN_Restricted_To_Owner_Set") return Pin.PinControl.RestrictedToOwner;
            else if (pinTypeOfControl == "PIN_Universal_Access") return Pin.PinControl.UniversalAccess;
            else return Pin.PinControl.ByGroup;
        }

        internal static O2GPinCode From(Pin code)
        {
            return new()
            {
                PINNumber = code.Number,
                PinGroup = code.Group,
                WithSecretCode = code.WithSecretCode,
                PinTypeOfControl = FromControl(code.Control)
            };
        }

        private static string FromControl(Pin.PinControl control)
        {
            if (control == Pin.PinControl.ByCategory) return "PIN_Category";
            else if (control == Pin.PinControl.RestrictedToOwner) return "PIN_Restricted_To_Owner_Set";
            else if (control == Pin.PinControl.UniversalAccess) return "PIN_Universal_Access";
            else return "PIN_Group_Control";
        }
    }
}
