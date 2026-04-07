using o2g.Types.RoutingNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Types.Routing
{
    internal class ForwardRoute
    {
        public string ForwardType { get; set; }
        public List<O2GDestination> Destinations { get; set; }

        public static ForwardRoute CreateForwardOnNumber(string number, Forward.ForwardCondition condition)
        {
            ForwardRoute objForwardRoute = new();
            objForwardRoute.Destinations = new();
            objForwardRoute.Destinations.Add(O2GDestination.CreateNumberDestination(number));

            objForwardRoute.ForwardType = GetForwardType(condition);

            return objForwardRoute;
        }
        public static ForwardRoute CreateForwardOnVoiceMail(Forward.ForwardCondition condition)
        {
            ForwardRoute objForwardRoute = new();
            objForwardRoute.Destinations = new();
            objForwardRoute.Destinations.Add(O2GDestination.CreateVoiceMailDestination());

            objForwardRoute.ForwardType = GetForwardType(condition);

            return objForwardRoute;
        }

        private static string GetForwardType(Forward.ForwardCondition condition)
        {
            switch (condition)
            {
                case Forward.ForwardCondition.Busy: return "BUSY";
                case Forward.ForwardCondition.NoAnswer: return "NO_ANSWER";
                case Forward.ForwardCondition.BusyOrNoAnswer: return "BUSY_NO_ANSWER";
                default: return null;
            }
        }

        internal Forward.ForwardCondition GetCondition()
        {
            if (ForwardType == "BUSY") return Forward.ForwardCondition.Busy;
            else if (ForwardType == "NO_ANSWER") return Forward.ForwardCondition.NoAnswer;
            else if (ForwardType == "BUSY_NO_ANSWER") return Forward.ForwardCondition.BusyOrNoAnswer;
            else return Forward.ForwardCondition.Immediate;
        }

        internal Forward ToForward()
        {
            Forward forward = new();

            if (Destinations.Count == 1)
            {
                O2GDestination destination = Destinations[0];

                forward.Destination = destination.GetDestination();
                if (forward.Destination == Destination.Number)
                {
                    forward.Number = destination.Number;
                }
                else
                {
                    forward.Number = null;
                }
            }

            forward.Condition = GetCondition();

            return forward;
        }
    }
}
