using SecuredChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public interface IProxyService : IHostService
    {
        CommunicationState State { get; }
    }
}
