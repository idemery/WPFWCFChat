using DM.ModuleChat.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace DM.ModuleChat.ViewModels
{
    public class ChatUsersViewModel : BindableBase
    {
        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public ChatUsersViewModel()
        {
            _users = new ObservableCollection<User> { new User { Name = "Islam" }, new User { Name = "Ahmed" } };
        }
    }
}
