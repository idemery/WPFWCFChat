using DM.ModuleChat.Events;
using DM.ModuleChat.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SecuredChat;
using System.Collections.Generic;
using System.ServiceModel;

namespace DM.ModuleChat.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private readonly IProxyService proxy;
        public DelegateCommand SendMessageCommand { get; private set; }
        public DelegateCommand IsTypingCommand { get; private set; }

        private string _msg;
        public string Message
        {
            get { return _msg; }
            set { SetProperty(ref _msg, value); }
        }

        public ChatViewModel(IEventAggregator ea, IProxyService proxy) : base(ea)
        {
            this.proxy = proxy;

            SendMessageCommand = new DelegateCommand(SendMessage, () => Connected && !string.IsNullOrWhiteSpace(Message));
            commandsToNotify.Add(SendMessageCommand);

            IsTypingCommand = new DelegateCommand(IsTyping);
        }

        public void SendMessage()
        {
            proxy.Send(new ChatMessage { Message = Message });
            Message = string.Empty;
        }

        public void IsTyping() => proxy.Send(new ChatTyping());
    }
}
