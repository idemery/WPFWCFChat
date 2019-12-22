using DM.ModuleChat.Events;
using Prism.Events;
using SecuredChat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public class ClientService : IClientService
    {
        IEventAggregator _eventAggregator;
        
        public ClientService(IEventAggregator ea)
        {
            _eventAggregator = ea;
        }

        public void Receive(DataModel data)
        {
            _eventAggregator.GetEvent<ChatEvent>().Publish(data);
        }
    }
}
