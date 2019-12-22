using DM.ModuleChat.Models;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace DM.ModuleChat.ViewModels
{
    public class ChatMessagesViewModel : BindableBase
    {
        private ObservableCollection<Message> messages;

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        public ChatMessagesViewModel()
        {
            messages = new ObservableCollection<Message>
            {
                new Message { Content = "Hi, this is a message", Time = DateTime.Now, FromUser = new User { Name = "Islam" } },
                new Message { Content = "Hey, Whats up?", Time = DateTime.Now, FromUser = new User { Name = "Ahmed" } }
            };
        }
    }
}
