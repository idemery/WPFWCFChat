using DM.Core.Events;
using DM.ModuleChat.Events;
using DM.ModuleChat.Helpers;
using Prism.Events;
using SecuredChat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class HostService : IHostService, IHostActionsService
    {
        #region Fields
        IEventAggregator _eventAggregator;
        private readonly ServiceHost serviceHost;
        private ObservableCollection<ClientModel> clients;
        private readonly string endPointAddress;
        #endregion
        #region Constructor
        public HostService(IEventAggregator ea)
        {
            clients = new ObservableCollection<ClientModel>();
            _eventAggregator = ea;
            serviceHost = new ServiceHost(this, new Uri("net.tcp://localhost:8080/SecuredChat/"));
            endPointAddress = ChatServiceHelper.GenerateEndPointAddress();
            serviceHost.AddServiceEndpoint(typeof(IHostService), ChatServiceHelper.GetBinding(), endPointAddress);

            serviceHost.Opened += ServiceHost_StateChanegd;
            serviceHost.Closed += ServiceHost_StateChanegd;
            serviceHost.Faulted += ServiceHost_StateChanegd;

            _eventAggregator.GetEvent<AppExitEvent>().Subscribe((code) => serviceHost.Abort());
        }
        #endregion
        #region Properties
        public ObservableCollection<ClientModel> Clients
        {
            get { return clients; }
            set { clients = value; }
        }

        public string CurrentSessionId { get { return OperationContext.Current?.SessionId; } }
        public string EndPointAddress { get { return endPointAddress; } }
        #endregion
        #region Event handlers
        private void ServiceHost_StateChanegd(object sender, EventArgs e)
        {
            ServiceHost host = sender as ServiceHost;
            if (host != null)
            {
                _eventAggregator.GetEvent<HostConnectionEvent>().Publish(host.State);
            }
        }

        private void Channel_Faulted(object sender, EventArgs e)
        {
            IContextChannel channel = sender as IContextChannel;
            DisconnectClient(channel.SessionId);
        }
        #endregion
        #region Private Methods
        private void DisconnectClient(string session_id)
        {
            var client = Clients.SingleOrDefault(c => c.SessionId == session_id);

            if (client.Context != null && client.Context.Channel != null && client.Context.Channel.State == CommunicationState.Opened)
            {
                client.Context.Channel.Close();
            }

            RemoveClient(session_id);
            Receive(new ChatLeave { Sender = client });
        }

        private void Receive(DataModel data, params string[] except_session_ids)
        {
            var sessions = Clients.Select(c => c.SessionId).Except(except_session_ids).ToList();

            List<string> faulted = new List<string>();

            foreach (var sid in sessions)
            {
                var client = Clients.FirstOrDefault(c => c.SessionId == sid);
                try
                {
                    Task.Factory.StartNew(() => client.Callback.Receive(data));
                }
                catch
                {
                    faulted.Add(sid);
                }
            }

            if (faulted.Any())
            {
                foreach (var item in faulted)
                {
                    DisconnectClient(item);
                }
            }
        }

        private void ReceiveTo(DataModel data, params string[] session_ids)
        {
            var sessions = Clients.Select(c => c.SessionId).Where(sid => session_ids.Contains(sid)).ToList();
            foreach (var sid in sessions)
            {
                var client = Clients.FirstOrDefault(c => c.SessionId == sid);
                try
                {
                    Task.Factory.StartNew(() => client.Callback.Receive(data));
                }
                catch { }
            }
        }

        private void RemoveClient(string session_id)
        {
            if (Clients.Any(c => c.SessionId == session_id))
            {
                Clients.Remove(Clients.Single(c => c.SessionId == session_id));
            }
        }
        #endregion
        #region Public Methods
        public void Start()
        {
            if (serviceHost.State != CommunicationState.Opened)
            {
                serviceHost.Open(TimeSpan.FromSeconds(5));
            }
        }

        public void Stop()
        {
            if (serviceHost.State == CommunicationState.Opened)
            {
                serviceHost.Close(TimeSpan.FromMilliseconds(1500));
            }
        }
        #endregion
        #region IHostService
        public void Connect(ClientModel clientModel)
        {
            clientModel.Context = OperationContext.Current;
            clientModel.SessionId = clientModel.Context.SessionId;
            clientModel.Context.Channel.Faulted += Channel_Faulted;

            Clients.Add(clientModel);

            Receive(new ChatJoin { Sender = clientModel }, CurrentSessionId);
            ReceiveTo(new ChatJoin { Sender = clientModel, Clients = Clients.ToList() }, CurrentSessionId);
        }

        public void Disconnect()
        {
            DisconnectClient(CurrentSessionId);
        }

        public void Send(DataModel data)
        {
            var sender = Clients.SingleOrDefault(c => c.SessionId == CurrentSessionId);

            data.Sender = sender;

            List<string> send_to_all_except = new List<string>();

            if (!(data is ChatMessage))
            {
                send_to_all_except.Add(sender.SessionId);
            }

            Receive(data, send_to_all_except.ToArray());
        }
        #endregion
    }
}
