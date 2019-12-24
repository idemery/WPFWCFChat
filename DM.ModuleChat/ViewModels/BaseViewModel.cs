using DM.ModuleChat.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DM.ModuleChat.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        protected List<DelegateCommand> commandsToNotify = new List<DelegateCommand>();

        public BaseViewModel(IEventAggregator ea)
        {
            ea.GetEvent<ClientConnectionEvent>().Subscribe(StateChanged, ThreadOption.UIThread);
        }

        protected void StateChanged(CommunicationState state) => Connected = state == CommunicationState.Opened;

        private bool _connected;
        public bool Connected
        {
            get => _connected;
            set
            {
                SetProperty(ref _connected, value);
                foreach (var cmd in commandsToNotify)
                {
                    cmd.RaiseCanExecuteChanged();
                }
            }
        }
    }
}
