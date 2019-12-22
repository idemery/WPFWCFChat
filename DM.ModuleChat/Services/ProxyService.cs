using DM.ModuleChat.Events;
using DM.ModuleChat.Helpers;
using Prism.Events;
using SecuredChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public class ProxyService : DuplexClientBase<IHostService>, IProxyService
    {
        IEventAggregator _eventAggregator;

        public ProxyService(IClientService clientService, IConnectionSettingsService settings, IEventAggregator ea)
            : base(new InstanceContext(clientService), ChatServiceHelper.GetBinding(),
                  new EndpointAddress($"net.tcp://{settings.IP}:8080/SecuredChat/{settings.EndPointAddress}"))
        {
            _eventAggregator = ea;

            InnerDuplexChannel.Opened += InnerDuplexChannel_StateChanged;
            InnerDuplexChannel.Closed += InnerDuplexChannel_StateChanged;
            InnerDuplexChannel.Faulted += InnerDuplexChannel_StateChanged;
        }

        private void InnerDuplexChannel_StateChanged(object sender, EventArgs e)
        {
            IDuplexContextChannel duplexContext = sender as IDuplexContextChannel;
            if (duplexContext != null)
            {
                _eventAggregator.GetEvent<ClientConnectionEvent>().Publish(duplexContext.State);
            }
        }

        public void Connect(ClientModel clientModel)
        {
            Channel.Connect(clientModel);
        }

        public void Disconnect()
        {
            Channel.Disconnect();
        }

        public void Send(object data)
        {
            Channel.Send(data);
        }
    }
}
