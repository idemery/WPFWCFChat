using DM.ModuleChat.Events;
using DM.ModuleChat.Services;
using idemery.Remoot.ClientHelper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SecuredChat;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DM.ModuleChat.ViewModels
{
    public class ScreenCastingViewModel : BaseViewModel
    {
        private ImageSource _imgSrc;

        public ImageSource ImageSource
        {
            get { return _imgSrc; }
            set { SetProperty(ref _imgSrc, value); }
        }

        private Screen screen;

        public DelegateCommand StartScreenCastCommand { get; private set; }


        BackgroundWorker Worker = new BackgroundWorker();

        private PresenterDesktop Desktop = new PresenterDesktop();
        private IProxyService proxyService;

        public ScreenCastingViewModel(IEventAggregator ea, IProxyService proxyService) : base(ea)
        {
            this.proxyService = proxyService;

            Worker.WorkerSupportsCancellation = true;


            screen = new Screen();

            ea.GetEvent<ChatEvent>().Subscribe(ScreenReceived, ThreadOption.UIThread, true, dm => dm is ScreenModel);

            StartScreenCastCommand = new DelegateCommand(StartScreenCast, () => Connected);
            commandsToNotify.Add(StartScreenCastCommand);
        }

        public string StartStopName => Casting ? "Stop Screen Casting" : "Start Screen Casting";

        private bool _casting;

        public bool Casting
        {
            get { return _casting; }
            set { SetProperty(ref _casting, value); RaisePropertyChanged("StartStopName"); }
        }

        private void StartScreenCast()
        {
            if (Casting)
            {
                Worker.CancelAsync();
                Worker.DoWork -= new DoWorkEventHandler(Worker_DoWork);
                Worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);

                proxyService.Send(new ScreenModel { Stopped = true });
            }
            else
            {
                proxyService.Send(new ScreenModel { Reference = true, Desktop = Desktop.GetCompressedReferenceDesktop() });
                System.Threading.Thread.Sleep(500);

                Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
                Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
                Worker.RunWorkerAsync();
            }

            Casting = !Casting;
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Worker.CancellationPending || proxyService.State != CommunicationState.Opened || !Casting)
            {
                e.Cancel = true;
                return;
            }

            proxyService.Send(new ScreenModel { Reference = false, Desktop = Desktop.GetCompressedDifferenceDesktop() });
        }
        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled || !Casting)
            {
                return;
            }

            Worker.RunWorkerAsync();
        }

        private void ScreenReceived(DataModel dataModel)
        {
            ScreenModel screenModel = dataModel as ScreenModel;

            if (screenModel.Stopped)
            {
                ImageSource = null;
                return;
            }

            if (screenModel.Desktop.ImageBytes == null)
            {
                return;
            }

            if (screenModel.Reference)
            {
                ImageSource = ToBitmapSource(screen.GetReferenceDesktop(screenModel.Desktop));
            }
            else
            {
                ImageSource = ToBitmapSource(screen.GetDifferenceDesktop(screenModel.Desktop));
            }
        }

        public static ImageSource ToBitmapSource(Bitmap source)
        {
            var hBitmap = source.GetHbitmap();

            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                return null;
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);
    }

    public class Screen
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x00000001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x00000002;
        private const int MOUSEEVENTF_LEFTUP = 0x00000004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x00000008;
        private const int MOUSEEVENTF_RIGHTUP = 0x00000010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x00000020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x00000040;
        private const int MOUSEEVENTF_WHEEL = 0x00000800;
        private const int MOUSEEVENTF_ABSOLUTE = 0x00008000;
        private System.Drawing.Point LocalPosition = new System.Drawing.Point();

        private ViewerDesktop DesktopHelper;

        public bool EnableDebug { get; set; }
        public Aced.Compression.AcedCompressionLevel CompressionLevel
        {
            get { return (Global.CompressionLevel); }
            set { Global.CompressionLevel = value; }
        }
        public Bitmap GetReferenceDesktop(CompressedDesktop value) => DesktopHelper.GetDecompressedReferenceDesktop(value).Image;

        public Bitmap GetDifferenceDesktop(CompressedDesktop value) => DesktopHelper.GetDecompressedDifferenceDesktop(value).Image;

        public Screen()
        {
            DesktopHelper = new ViewerDesktop();
            Global.CompressionLevel = Global.DefaultCompressionLevel;
        }
    }
}
