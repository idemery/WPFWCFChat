using DM.ModuleTfsWorkItems.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DM.ModuleTfsWorkItems.ViewModels
{
    public class TfsSettingsViewModel : BindableBase
    {
        public DelegateCommand<object> SaveSettingsCommand { get; private set; }
        public DelegateCommand StartListenForNotificationsCommand { get; private set; }
        public DelegateCommand StopListenForNotificationsCommand { get; private set; }

        CancellationTokenSource cts;
        private readonly ITfsSettingsService settingsService;
        public ITfsSettingsService Settings { get => settingsService; }

        public TfsSettingsViewModel(ITfsSettingsService settingsService)
        {
            this.settingsService = settingsService;

            SaveSettingsCommand = new DelegateCommand<object>(SaveSettings);
            StartListenForNotificationsCommand = new DelegateCommand(StartListenForNotifications, CanStartListenForNotifications);
            StopListenForNotificationsCommand = new DelegateCommand(StopListenForNotifications, CanStopListenForNotifications);
        }

        private void SaveSettings(object args)
        {
            if (!(args is PasswordBox))
            {
                return;
            }

            Settings.Password = ((PasswordBox)args).Password;
        }

        private void StartListenForNotifications()
        {
            cts = new CancellationTokenSource();

            var task = StartListener(cts.Token);

            task.Wait();
        }

        private bool CanStartListenForNotifications()
        {
            return cts != null;
        }

        private void StopListenForNotifications()
        {
            cts.Cancel();
            cts = null;
        }

        private bool CanStopListenForNotifications()
        {
            return cts != null && !cts.Token.IsCancellationRequested;
        }

        private async Task StartListener(CancellationToken token)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://*:8081/");
            listener.Start();

            token.Register(() => listener.Abort());

            while (!token.IsCancellationRequested)
            {
                HttpListenerContext context;

                try
                {
                    context = await listener.GetContextAsync().ConfigureAwait(false);

                    HandleRequest(context); // Note that this is *not* awaited
                }
                catch
                {
                    // Handle errors
                }
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            // Handle the request, ideally in an asynchronous way.
            // Even if not asynchronous, though, this is still run 
            // on a different (thread pool) thread
            // todo
        }
    }
}
