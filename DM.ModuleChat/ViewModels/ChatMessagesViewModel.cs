using DM.ModuleChat.Events;
using Prism.Events;
using Prism.Mvvm;
using SecuredChat;
using System;
using System.Collections.ObjectModel;

namespace DM.ModuleChat.ViewModels
{
    public class ChatMessagesViewModel : BindableBase
    {
        private ObservableCollection<ChatMessage> messages;

        public ObservableCollection<ChatMessage> Messages
        {
            get { return messages; }
            set { SetProperty(ref messages, value); }
        }

        public ChatMessagesViewModel(IEventAggregator ea)
        {
            messages = new ObservableCollection<ChatMessage>();

            ea.GetEvent<ChatEvent>().Subscribe(ClientChanged, ThreadOption.UIThread, true, dm => (dm is ChatJoin || dm is ChatLeave));
            ea.GetEvent<ChatEvent>().Subscribe(MessageReceived, ThreadOption.UIThread, true, dm => dm is ChatMessage);
        }

        private void ClientChanged(DataModel data)
        {
            Messages.Add(new ChatMessage { Sender = data.Sender, Message = data.ToString() });
        }

        private void MessageReceived(DataModel data)
        {
            Messages.Add((ChatMessage)data);
        }
    }
}
