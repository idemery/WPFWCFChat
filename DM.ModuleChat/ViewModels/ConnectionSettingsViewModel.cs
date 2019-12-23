using DM.ModuleChat.Events;
using DM.ModuleChat.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.DirectoryServices.AccountManagement;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DM.ModuleChat.ViewModels
{
    public class ConnectionSettingsViewModel : BindableBase
    {
        private readonly IConnectionSettingsService connectionSettings;
        private readonly IProxyService proxy;

        public DelegateCommand ConnectCommand { get; private set; }
        public DelegateCommand DisconnectCommand { get; private set; }

        public IConnectionSettingsService ConnectionSettings { get { return connectionSettings; } }

        public ConnectionSettingsViewModel(IConnectionSettingsService connectionSettings, IProxyService proxy, IEventAggregator ea)
        {
            this.proxy = proxy;
            this.connectionSettings = connectionSettings;
            this.connectionSettings.UserName = UserPrincipal.Current?.DisplayName ?? Environment.UserName;
            this.connectionSettings.IP = "localhost";

            ea.GetEvent<ClientConnectionEvent>().Subscribe(ClientConnectionStateChanged, ThreadOption.UIThread);

            ConnectCommand = new DelegateCommand(Connect, () =>
            State != CommunicationState.Opened
            && !string.IsNullOrWhiteSpace(ConnectionSettings.EndPointAddress)
            && !string.IsNullOrWhiteSpace(ConnectionSettings.IP)
            && !string.IsNullOrWhiteSpace(ConnectionSettings.UserName));

            DisconnectCommand = new DelegateCommand(Disconnect, () => State == CommunicationState.Opened);
        }

        CommunicationState _state;
        public CommunicationState State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);
                ConnectCommand.RaiseCanExecuteChanged();
                DisconnectCommand.RaiseCanExecuteChanged();
            }
        }
        private void ClientConnectionStateChanged(CommunicationState state) => State = state;

        private async void Connect() => await Task.Run(() => proxy.Connect(null));
        private async void Disconnect() => await Task.Run(() => proxy.Disconnect());
    }
}
