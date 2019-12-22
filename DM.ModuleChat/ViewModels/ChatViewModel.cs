using DM.ModuleChat.Events;
using DM.ModuleChat.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.ServiceModel;

namespace DM.ModuleChat.ViewModels
{
    public class ChatViewModel : BindableBase
    {
        private readonly IProxyService proxy;
        public DelegateCommand SendMessageCommand { get; private set; }
        private bool _connected;

        public bool Connected
        {
            get { return _connected; }
            set { SetProperty(ref _connected, value); SendMessageCommand.RaiseCanExecuteChanged(); }
        }
        public string Message { get; set; }

        public ChatViewModel(IEventAggregator ea, IProxyService proxy)
        {
            this.proxy = proxy;

            SendMessageCommand = new DelegateCommand(SendMessage, CanSendMessage);

            ea.GetEvent<ClientConnectionEvent>().Subscribe(StateChanged, ThreadOption.UIThread);
        }

        public void SendMessage() => proxy.Send(Message);

        public bool CanSendMessage() => Connected && !string.IsNullOrWhiteSpace(Message);

        private void StateChanged(CommunicationState state) => Connected = state == CommunicationState.Opened;
    }
}
