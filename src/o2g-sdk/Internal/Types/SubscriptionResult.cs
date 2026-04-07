using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Types
{
    internal class SubscriptionResult
    {
        public string SubscriptionId { get; set; }
        public string Message { get; set; }
        public string PublicPollingUrl { get; set; }
        public string PrivatePollingUrl { get; set; }
        public string Status { get; set; }
    }
}
