using DM.ModuleChat.Events;
using DM.ModuleChat.Services;
using idemery.Remoot.ClientHelper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SecuredChat;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DM.ModuleChat.ViewModels
{
    public class ScreenCastingViewModel : BindableBase
    {
        private ImageSource _imgSrc;

        public ImageSource ImageSource
        {
            get { return _imgSrc; }
            set { SetProperty(ref _imgSrc, value); }
        }



        private Screen screen;

        public DelegateCommand StartScreenCastCommand { get; private set; }

        private Timer screenCastTimer = new Timer();
        private PresenterDesktop Desktop = new PresenterDesktop();
        private IProxyService proxyService;

        public ScreenCastingViewModel(IEventAggregator ea, IProxyService proxyService)
        {
            this.proxyService = proxyService;
            screenCastTimer.Interval = 1000;

            screenCastTimer.Elapsed += ScreenCastTimer_Elapsed;

            screen = new Screen();

            ea.GetEvent<ChatEvent>().Subscribe(ScreenReceived, ThreadOption.UIThread, true, dm => dm is ScreenModel);

            StartScreenCastCommand = new DelegateCommand(StartScreenCast);
        }

        private void ScreenCastTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            proxyService.Send(new ScreenModel { Reference = false, Desktop = Desktop.GetCompressedDifferenceDesktop() });
        }

        private void StartScreenCast()
        {
            if (screenCastTimer.Enabled)
            {
                screenCastTimer.Stop();
                proxyService.Send(new ScreenModel { Stopped = true });
            }
            else
            {
                proxyService.Send(new ScreenModel { Reference = true, Desktop = Desktop.GetCompressedReferenceDesktop() });
                System.Threading.Thread.Sleep(1000);
                screenCastTimer.Start();
            }
        }

        private void ScreenReceived(DataModel dataModel)
        {
            ScreenModel screenModel = dataModel as ScreenModel;

            if (screenModel.Reference)
            {
                ImageSource = GetBitmapSource(screen.GetReferenceDesktop(screenModel.Desktop));
            }
            else
            {
                ImageSource = GetBitmapSource(screen.GetDifferenceDesktop(screenModel.Desktop));
            }
        }

        private static ImageSource GetBitmapSource(Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                                              bitmap.GetHbitmap(),
                                              IntPtr.Zero,
                                              Int32Rect.Empty,
                                              BitmapSizeOptions.FromEmptyOptions());
        }
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
