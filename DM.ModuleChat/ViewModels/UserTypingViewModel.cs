using DM.ModuleChat.Events;
using Prism.Events;
using Prism.Mvvm;
using SecuredChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DM.ModuleChat.ViewModels
{
    public class UserTypingViewModel : BindableBase
    {
        public UserTypingViewModel(IEventAggregator ea)
        {
            ea.GetEvent<ChatEvent>().Subscribe(UserIsTyping, ThreadOption.UIThread, true, dm => dm is ChatTyping);
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private void UserIsTyping(DataModel dataModel)
        {
            Status = $"{dataModel.Sender.Name} is typing ..";
            Timer timer = new Timer();
            timer.Interval = 1400;
            timer.Elapsed += (s, en) =>
            {
                Status = string.Empty;
                timer.Stop();
            };
            timer.Start();
        }
    }
}
