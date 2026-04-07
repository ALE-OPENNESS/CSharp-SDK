using o2g.Types.RoutingNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Types.Routing
{
    internal class O2GDestination
    {
        public string Type { get; set; }
        public string Number { get; set; }
        public bool? Selected { get; set; }

        internal static O2GDestination CreateMobileDestination(bool active)
        {
            return new()
            {
                Type = "MOBILE",
                Selected = active
            };
        }


        internal static O2GDestination CreateNumberDestination(string number)
        {
            return new()
            {
                Type = "NUMBER",
                Number = number,
                Selected = null
            };
        }

        internal static O2GDestination CreateVoiceMailDestination()
        {
            return new()
            {
                Type = "VOICEMAIL",
                Selected = null
            };
        }
        internal static O2GDestination CreateAssociateDestination()
        {
            return new()
            {
                Type = "ASSOCIATE",
                Selected = null
            };
        }

        internal Destination GetDestination()
        {
            if (Type == "NUMBER") return Destination.Number;
            else if (Type == "VOICEMAIL") return Destination.VoiceMail;
//            else if (Type == "ASSOCIATE") return Destination.Associate;
            else return Destination.None;
        }

    }
}
