using DM.ModuleChat.Events;
using DM.ModuleChat.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DM.ModuleChat.ViewModels
{
    public class ChatHostViewModel : BindableBase
    {
        private readonly IHostActionsService hostService;
        CommunicationState _state;

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }

        public ChatHostViewModel(IHostActionsService hostService, IEventAggregator ea)
        {
            this.hostService = hostService;
            _state = CommunicationState.Created;

            ea.GetEvent<HostConnectionEvent>().Subscribe(HostConnectionStateChanged, ThreadOption.UIThread);
            StartCommand = new DelegateCommand(Start, () => State != CommunicationState.Opened);
            StopCommand = new DelegateCommand(Stop, () => State == CommunicationState.Opened);
        }

        public CommunicationState State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);
                StartCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        }

        public string EndPointAddress => hostService.EndPointAddress;

        private void HostConnectionStateChanged(CommunicationState state) => State = state;

        public async void Start() => await Task.Run(() => hostService.Start());
        public async void Stop() => await Task.Run(() => hostService.Stop());
    }
}
