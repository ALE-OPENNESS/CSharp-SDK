using o2g.Types.RoutingNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Types.Routing
{
    internal class OverflowRoute
    {
        public string OverflowType { get; set; }
        public List<O2GDestination> Destinations { get; set; }

        public static OverflowRoute CreateOverflowOnAssociate(Overflow.OverflowCondition condition)
        {
            OverflowRoute objOverflowRoute = new();
            objOverflowRoute.Destinations = new();
            objOverflowRoute.Destinations.Add(O2GDestination.CreateAssociateDestination());

            objOverflowRoute.OverflowType = GetOverflowType(condition);

            return objOverflowRoute;
        }
        public static OverflowRoute CreateOverflowOnVoiceMail(Overflow.OverflowCondition condition)
        {
            OverflowRoute objOverflowRoute = new();
            objOverflowRoute.Destinations = new();
            objOverflowRoute.Destinations.Add(O2GDestination.CreateVoiceMailDestination());

            objOverflowRoute.OverflowType = GetOverflowType(condition);

            return objOverflowRoute;
        }

        private static string GetOverflowType(Overflow.OverflowCondition condition)
        {
            if (condition == Overflow.OverflowCondition.Busy) return "BUSY";
            else if (condition == Overflow.OverflowCondition.NoAnswer) return "NO_ANSWER";
            else return "BUSY_NO_ANSWER";
        }

        internal Overflow.OverflowCondition GetCondition()
        {
            if (OverflowType == "BUSY") return Overflow.OverflowCondition.Busy;
            else if (OverflowType == "NO_ANSWER") return Overflow.OverflowCondition.NoAnswer;
            else return Overflow.OverflowCondition.BusyOrNoAnswer;
        }

        internal Overflow ToOverflow()
        {
            Overflow overflow = new();

            if (Destinations.Count == 1)
            {
                O2GDestination destination = Destinations[0];
                overflow.Destination = destination.GetDestination();
            }

            overflow.Condition = GetCondition();

            return overflow;
        }
    }
}
