using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace idemery.Remoot.ClientHelper
{
    public partial class Desktop : UserControl
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        const int MOUSEEVENTF_MOVE = 0x00000001;
        const int MOUSEEVENTF_LEFTDOWN = 0x00000002;
        const int MOUSEEVENTF_LEFTUP = 0x00000004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x00000008;
        const int MOUSEEVENTF_RIGHTUP = 0x00000010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x00000020;
        const int MOUSEEVENTF_MIDDLEUP = 0x00000040;
        const int MOUSEEVENTF_WHEEL = 0x00000800;
        const int MOUSEEVENTF_ABSOLUTE = 0x00008000;
        Point LocalPosition = new Point();
        #region >> Variables <<
            ViewerDesktop    DesktopHelper;
        #endregion
        #region >> Properties <<
            public bool EnableDebug { get; set; }
            public Aced.Compression.AcedCompressionLevel CompressionLevel
            {
                get { return(Global.CompressionLevel); }
                set { Global.CompressionLevel = value; } 
            } 
            public CompressedDesktop SetReferenceDesktop
            {
                set 
                {
                    DecompressedDesktop _Desktop = DesktopHelper.GetDecompressedReferenceDesktop(value);
                    Preview.BackgroundImage = _Desktop.Image;

                    if (EnableDebug)
                    {
                        StringBuilder Builder = new StringBuilder();
                        Builder.Append("\n- ### Presenter Details ###" + "\n");
                        Builder.Append("- Size Before Compression: " + value.CompressionSizes.SizeBefore / 1024 + " KB\n");
                        Builder.Append("- Size After Compression: " + value.CompressionSizes.SizeAfter / 1024 + " KB\n");
                        Builder.Append("- Size After Compression: " + value.CompressionSizes.SizeAfter + " Bytes\n");
                        Builder.Append("- Size Saved Due Compression: " + (value.CompressionSizes.SizeBefore - value.CompressionSizes.SizeAfter) / 1024 + " KB\n\n");

                        Builder.Append("- ### Presenter Times ###" + "\n");
                        Builder.Append("- Ticks To Capture: " + value.PackingTime.CapturingTime + " \n");
                        Builder.Append("- Ticks To XOR: " + value.PackingTime.XoringTime + " \n");
                        Builder.Append("- Ticks To Compress: " + value.PackingTime.CompressionTime + " \n\n");

                        Builder.Append("- ####################################\n\n");

                        Builder.Append("- ### Viewer Details ###" + "\n");
                        Builder.Append("- Size Before Decompression: " + _Desktop.DecompressionSizes.SizeBefore + " Bytes\n");
                        Builder.Append("- Size Before Decompression: " + _Desktop.DecompressionSizes.SizeBefore / 1024 + " KB\n");
                        Builder.Append("- Size After Decompression: " + _Desktop.DecompressionSizes.SizeAfter / 1024 + " KB\n");
                        Builder.Append("- Size Added Due Decompression: " + (_Desktop.DecompressionSizes.SizeAfter - _Desktop.DecompressionSizes.SizeBefore) / 1024 + " KB\n\n");

                        Builder.Append("- ### Presenter Times ###" + "\n");
                        Builder.Append("- Ticks To Decompress: " + _Desktop.UnpackingTime.CompressionTime + " \n");
                        Builder.Append("- Ticks To XOR: " + _Desktop.UnpackingTime.XoringTime + " ");

                        DebugInfo = Builder.ToString();
                    }
                }
            }
            public CompressedDesktop SetDifferenceDesktop
            {
                set
                {
                    DecompressedDesktop _Desktop = DesktopHelper.GetDecompressedDifferenceDesktop(value);
                    Preview.BackgroundImage = _Desktop.Image;

                    if (EnableDebug)
                    {
                        StringBuilder Builder = new StringBuilder();
                        Builder.Append("\n- ### Presenter Details ###" + "\n");
                        Builder.Append("- Size Before Compression: " + value.CompressionSizes.SizeBefore / 1024 + " KB\n");
                        Builder.Append("- Size After Compression: " + value.CompressionSizes.SizeAfter / 1024 + " KB\n");
                        Builder.Append("- Size After Compression: " + value.CompressionSizes.SizeAfter + " Bytes\n");
                        Builder.Append("- Size Saved Due Compression: " + (value.CompressionSizes.SizeBefore - value.CompressionSizes.SizeAfter) / 1024 + " KB\n\n");

                        Builder.Append("- ### Presenter Times ###" + "\n");
                        Builder.Append("- Ticks To Capture: " + value.PackingTime.CapturingTime + " \n");
                        Builder.Append("- Ticks To XOR: " + value.PackingTime.XoringTime + " \n");
                        Builder.Append("- Ticks To Compress: " + value.PackingTime.CompressionTime + " \n");
                        Builder.Append("- ------------------------------------\n");
                        Builder.Append("- Total Packing Time: " + (value.PackingTime.CapturingTime + value.PackingTime.XoringTime + value.PackingTime.CompressionTime).ToString() + " ms\n\n");


                        Builder.Append("- ####################################\n\n");

                        Builder.Append("- ### Viewer Details ###" + "\n");
                        Builder.Append("- Size Before Decompression: " + _Desktop.DecompressionSizes.SizeBefore + " Bytes\n");
                        Builder.Append("- Size Before Decompression: " + _Desktop.DecompressionSizes.SizeBefore / 1024 + " KB\n");
                        Builder.Append("- Size After Decompression: " + _Desktop.DecompressionSizes.SizeAfter / 1024 + " KB\n");
                        Builder.Append("- Size Added Due Decompression: " + (_Desktop.DecompressionSizes.SizeAfter - _Desktop.DecompressionSizes.SizeBefore) / 1024 + " KB\n\n");

                        Builder.Append("- ### Presenter Times ###" + "\n");
                        Builder.Append("- Ticks To Decompress: " + _Desktop.UnpackingTime.CompressionTime + " \n");
                        Builder.Append("- Ticks To XOR: " + _Desktop.UnpackingTime.XoringTime + " \n");
                        Builder.Append("- ------------------------------------\n");
                        Builder.Append("- Total Unpacking Time: " + (_Desktop.UnpackingTime.CompressionTime + _Desktop.UnpackingTime.XoringTime).ToString() + " ms\n\n");

                        Builder.Append("- ####################################\n\n");
                        Builder.Append("- Total Process Time: " + (value.PackingTime.CapturingTime + value.PackingTime.XoringTime + value.PackingTime.CompressionTime + _Desktop.UnpackingTime.CompressionTime + _Desktop.UnpackingTime.XoringTime).ToString() + " \n\n");

                        DebugInfo = Builder.ToString();
                    }
                }
            }
            public string DebugInfo { get; set; }
        #endregion
        #region >> Constructor & Control Events <<
            public Desktop()
            {
                InitializeComponent();
                DesktopHelper = new ViewerDesktop();
                Global.CompressionLevel = Global.DefaultCompressionLevel;
            }
        #endregion
            private void Preview_Click(object sender, EventArgs e)
            {
                //Cursor.Position = LocalPosition;
                //mouse_event(MOUSEEVENTF_LEFTDOWN, LocalPosition.X, LocalPosition.Y, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, LocalPosition.X, LocalPosition.Y, 0, 0);
            }
            private void Preview_DoubleClick(object sender, EventArgs e)
            {
                //Cursor.Position = LocalPosition;
                //mouse_event(MOUSEEVENTF_LEFTDOWN, LocalPosition.X, LocalPosition.Y, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, LocalPosition.X, LocalPosition.Y, 0, 0);

                //mouse_event(MOUSEEVENTF_LEFTDOWN, LocalPosition.X, LocalPosition.Y, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, LocalPosition.X, LocalPosition.Y, 0, 0);
            }
            private void Preview_MouseMove(object sender, MouseEventArgs e)
            {
                //LocalPosition.X = e.X;
                //LocalPosition.Y = e.Y;
            }
        #region >> Helper Functions <<
        #endregion
    }
}