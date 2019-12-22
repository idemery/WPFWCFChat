using DM.ModuleChat.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace DM.ModuleChat.ViewModels
{
    public class ChatViewModel : BindableBase
    {
        public string Header => "CHAT";
        public string Description => "Collaborate with your team members by chatting and screen sharing";

        public DelegateCommand SendMessageCommand { get; private set; }

        public ChatViewModel(IChatCommands chatCommands)
        {
            SendMessageCommand = new DelegateCommand(SendMessage);
            chatCommands.SendMessage.RegisterCommand(SendMessageCommand);
        }

        public void SendMessage()
        {

        }
    }
}
