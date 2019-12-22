using Prism.Commands;
using Prism.Mvvm;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DM.ModuleTfsWorkItems.ViewModels
{
    public class TfsSettingsViewModel : BindableBase
    {
        public DelegateCommand StartListenForNotificationsCommand { get; private set; }
        public DelegateCommand StopListenForNotificationsCommand { get; private set; }
        CancellationTokenSource cts;

        public TfsSettingsViewModel()
        {
            StartListenForNotificationsCommand = new DelegateCommand(StartListenForNotifications, CanStartListenForNotifications);
            StopListenForNotificationsCommand = new DelegateCommand(StopListenForNotifications, CanStopListenForNotifications);
        }

        public void StartListenForNotifications()
        {
            cts = new CancellationTokenSource();

            var task = StartListener(cts.Token);

            task.Wait();
        }

        public bool CanStartListenForNotifications()
        {
            return cts != null;
        }

        public void StopListenForNotifications()
        {
            cts.Cancel();
            cts = null;
        }

        public bool CanStopListenForNotifications()
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
