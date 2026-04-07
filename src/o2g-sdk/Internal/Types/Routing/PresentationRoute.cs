using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Types.Routing
{
    internal class PresentationRoute
    {
        public List<O2GDestination> Destinations { get; set; }

        public static PresentationRoute CreateMobileRouteActivation(bool active)
        {
            PresentationRoute objPresentationRoute = new();
            objPresentationRoute.Destinations = new();
            objPresentationRoute.Destinations.Add(O2GDestination.CreateMobileDestination(active));

            return objPresentationRoute;
        }

    }
}
