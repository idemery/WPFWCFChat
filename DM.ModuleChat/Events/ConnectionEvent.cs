using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Events
{
    public class ConnectionEvent : PubSubEvent<CommunicationState>
    {
    }

    public class HostConnectionEvent : ConnectionEvent
    {

    }

    public class ClientConnectionEvent : ConnectionEvent
    {

    }
}
