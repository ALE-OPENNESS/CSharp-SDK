using o2g.Types.RoutingNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Types.Routing
{
    internal class O2GRoutingState
    {
        public List<PresentationRoute> PresentationRoutes { get; set; }
        public List<ForwardRoute> ForwardRoutes { get; set; }
        public List<OverflowRoute> OverflowRoutes { get; set; }

        public DndState DndState { get; set; }

        private bool? GetRemoteExtensionActivation()
        {
            if ((PresentationRoutes == null) || (PresentationRoutes.Count == 0))
            {
                return null;
            }
            for (int i = 0; i < PresentationRoutes.Count; i++)
            {
                PresentationRoute presentationRoute = PresentationRoutes[i];
                for (int j = 0; j < PresentationRoutes[i].Destinations.Count; j++)
                {
                    O2GDestination destination = PresentationRoutes[i].Destinations[j];
                    if (destination.Type == "MOBILE")
                    {
                        if (destination.Selected.HasValue)
                        {
                            return destination.Selected.Value;
                        }
                    }
                }
            }

            return null;
        }

        public RoutingState ToRoutingState()
        {
            RoutingState routingState = new();

            routingState.DndState = DndState;

            if ((ForwardRoutes != null) && (ForwardRoutes.Count > 0))
            {
                routingState.Forward = ForwardRoutes[0].ToForward();
            }
            else
            {
                routingState.Forward = new()
                {
                    Destination = Destination.None
                };
            }
            if ((OverflowRoutes != null) && (OverflowRoutes.Count > 0))
            {
                routingState.Overflow = OverflowRoutes[0].ToOverflow();
            }
            else
            {
                routingState.Overflow = new()
                {
                    Destination = Destination.None
                };
            }

            routingState.RemoteExtensionActivated = GetRemoteExtensionActivation();

            return routingState;
        }
    }
}
