using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace o2g.Internal.Services
{
    internal interface ISessions : IService
    {
        Task<SessionInfo> Open(string applicationName);
        Task<bool> Close();
        Task<SessionInfo> Get();
        Task<bool> SendKeepAlive();
    }
}
