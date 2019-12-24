using DM.Core.Events;
using DM.ModuleChat.Events;
using DM.ModuleChat.Helpers;
using Prism.Events;
using SecuredChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    public class ProxyService : IProxyService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionSettingsService settings;
        private readonly IClientService clientService;

        public ProxyService(IClientService clientService, IConnectionSettingsService settings, IEventAggregator ea)
        {
            _eventAggregator = ea;
            this.clientService = clientService;
            this.settings = settings;

            _eventAggregator.GetEvent<AppExitEvent>().Subscribe((code) => { if (_proxy != null) { Disconnect(); } });
        }

        private Proxy _proxy;

        public Proxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new Proxy(new InstanceContext(clientService), ChatServiceHelper.GetBinding(),
                    new EndpointAddress($"net.tcp://{settings.IP}:8080/SecuredChat/{settings.EndPointAddress}"), settings, _eventAggregator);
                }
                return _proxy;
            }
        }

        public CommunicationState State => Proxy?.State ?? CommunicationState.Created;

        public void Connect(ClientModel clientModel)
        {
            if (State == CommunicationState.Faulted)
            {
                _proxy = null;
            }

            Proxy.Connect(clientModel);
        }

        public void Disconnect()
        {
            Proxy.Disconnect();
        }

        public void Send(DataModel data)
        {
            Proxy.Send(data);
        }
    }

    public class Proxy : DuplexClientBase<IHostService>, IProxyService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionSettingsService settings;

        public Proxy(InstanceContext instanceContext, Binding binding, EndpointAddress endpointAddress, 
            IConnectionSettingsService settings, IEventAggregator ea)
            : base(instanceContext, binding, endpointAddress)
        {
            this.settings = settings;
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
                CommunicationState state = duplexContext.State;
                _eventAggregator.GetEvent<ClientConnectionEvent>().Publish(state);
            }
        }


        public void Connect(ClientModel clientModel)
        {
            if (State != CommunicationState.Opened)
            {
                Channel.Connect(clientModel ?? new ClientModel { Name = settings.UserName });
            }
        }

        public void Disconnect()
        {
            if (State == CommunicationState.Opened)
            {
                Channel.Disconnect();
            }
        }

        public void Send(DataModel data)
        {
            if (State == CommunicationState.Opened)
            {
                Task.Factory.StartNew(() => Channel.Send(data));
            }
        }
    }
}
