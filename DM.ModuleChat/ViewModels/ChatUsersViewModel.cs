using DM.ModuleChat.Events;
using Prism.Events;
using Prism.Mvvm;
using SecuredChat;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace DM.ModuleChat.ViewModels
{
    public class ChatUsersViewModel : BindableBase
    {
        private ObservableCollection<ClientModel> _users;

        public ObservableCollection<ClientModel> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        public ChatUsersViewModel(IEventAggregator ea)
        {
            _users = new ObservableCollection<ClientModel>();
            ea.GetEvent<ChatEvent>().Subscribe(ClientChanged, ThreadOption.UIThread, true, dm => (dm is ChatJoin || dm is ChatLeave));
            ea.GetEvent<ClientConnectionEvent>().Subscribe(StateChanged, ThreadOption.UIThread);
        }

        private void ClientChanged(DataModel data)
        {
            if (data is ChatJoin)
            {
                ChatJoin join = data as ChatJoin;

                if (join.Clients != null && join.Clients.Any())
                {
                    Users.AddRange(join.Clients);
                }
                else
                {
                    Users.Add(data.Sender);
                }
            }
            else if (data is ChatLeave)
            {
                if (Users.Any(c => c.SessionId == data.Sender.SessionId))
                {
                    Users.Remove(Users.Single(c => c.SessionId == data.Sender.SessionId));
                }
            }
        }
        
        private void StateChanged(CommunicationState state)
        {
            if (state != CommunicationState.Opened)
            {
                Users.Clear();
            }
        }
    }
}
