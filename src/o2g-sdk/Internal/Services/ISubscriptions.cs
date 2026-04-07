using o2g.Events;
using o2g.Internal.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace o2g.Internal.Services
{
    internal interface ISubscriptions : IService
    {
        Task<SubscriptionResult> Create(Subscription request);
        Task<bool> Delete(String subscriptionId);
    }
}
