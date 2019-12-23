using DM.ModuleChat.Events;
using DM.ModuleChat.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SecuredChat;
using System.ServiceModel;

namespace DM.ModuleChat.ViewModels
{
    public class ChatViewModel : BindableBase
    {
        private readonly IProxyService proxy;
        public DelegateCommand SendMessageCommand { get; private set; }
        public DelegateCommand IsTypingCommand { get; private set; }
        private bool _connected;

        public bool Connected
        {
            get { return _connected; }
            set { SetProperty(ref _connected, value); SendMessageCommand.RaiseCanExecuteChanged(); }
        }

        private string _msg;
        public string Message
        {
            get { return _msg; }
            set { SetProperty(ref _msg, value); }
        }

        public ChatViewModel(IEventAggregator ea, IProxyService proxy)
        {
            this.proxy = proxy;

            SendMessageCommand = new DelegateCommand(SendMessage, () => Connected && !string.IsNullOrWhiteSpace(Message));
            IsTypingCommand = new DelegateCommand(IsTyping);

            ea.GetEvent<ClientConnectionEvent>().Subscribe(StateChanged, ThreadOption.UIThread);
        }

        public void SendMessage()
        {
            proxy.Send(new ChatMessage { Message = Message });
            Message = string.Empty;
        }

        public void IsTyping() => proxy.Send(new ChatTyping());

        private void StateChanged(CommunicationState state) => Connected = state == CommunicationState.Opened;
    }
}
