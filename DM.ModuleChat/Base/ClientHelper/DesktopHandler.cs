using System.Drawing;
using System;
using System.Diagnostics;

namespace idemery.Remoot.ClientHelper
{
    public class PresenterDesktop
    {
        #region >> Variables <<
            XOR                         Xor;
            Bitmap                      ReferenceImage;
            Bitmap                      ResultImage;
            DesktopCompressionHelper    Compressor;
            Stopwatch                   Watch;
        #endregion
        #region >> Constructor <<
            public PresenterDesktop()
            {
                // Intialize compressor object
                Compressor = new DesktopCompressionHelper();
                // Intialize the stop watch
                Watch = new Stopwatch();
            } 
        #endregion
        #region >> Public Functions <<
            public CompressedDesktop GetCompressedReferenceDesktop()
            {
                // New compressed desktop
                CompressedDesktop Desktop = new CompressedDesktop();
                // Get ref image
                Xor = new XOR();
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // Capture desktop with mouse
                ReferenceImage = Capture.CaptureDesktop();
                // Calc capturing time
                Desktop.PackingTime.CapturingTime = StopWatch();
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Save the ref image in Xor object for later comparison
                Xor.SetRefImage(new Bitmap(ReferenceImage));
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // Temp compressed desktop
                CompressedDesktop _Desktop = Compressor.CompressDesktop(ReferenceImage);
                // Calc compression time
                Desktop.PackingTime.CompressionTime = StopWatch();
                // Assign compressed image
                Desktop.ImageBytes = _Desktop.ImageBytes;
                // Assign compressed sizes
                Desktop.CompressionSizes = _Desktop.CompressionSizes;
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Return compressed desktop
                return Desktop;
            }
            public CompressedDesktop GetCompressedDifferenceDesktop()
            {
                // New compressed desktop
                CompressedDesktop Desktop = new CompressedDesktop();
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // Capture the desktop
                ReferenceImage = Capture.CaptureDesktop();
                // Calc capturing time
                Desktop.PackingTime.CapturingTime = StopWatch();
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // XOR ref image with newly captured image
                ResultImage = Xor.XORing(new Bitmap(ReferenceImage));
                // Calc Xoring time
                Desktop.PackingTime.XoringTime = StopWatch();
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // Temp compressed desktop
                CompressedDesktop _Desktop = Compressor.CompressDesktop(ResultImage);
                // Calc compression time
                Desktop.PackingTime.CompressionTime = StopWatch();
                // Save the ref image to use it later.
                Xor.SetRefImage(new Bitmap(ReferenceImage));
                // Assign compressed image
                Desktop.ImageBytes = _Desktop.ImageBytes;
                // Assign compressed sizes
                Desktop.CompressionSizes = _Desktop.CompressionSizes;
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Clean
                ReferenceImage = null;
                ResultImage = null;
                // Return
                return Desktop;
            }
        #endregion
        #region >> Helper Functions <<
            private void StartWatch()
            {
                // Reset the watch
                Watch.Reset();
                // Start the watch
                Watch.Start();
            }
            private TimeSpan StopWatch()
            {
                // Stop the watch
                Watch.Stop();
                // Return the interval
                return (Watch.Elapsed);
            }
        #endregion
    }
    public class ViewerDesktop
    {
        #region >> Variables <<
            XOR                         Xor;
            DesktopCompressionHelper    Decompressor;
            Stopwatch                   Watch;
        #endregion
        #region >> Constructor <<
            public ViewerDesktop()
            {
                // Intialize decompressor object
                Decompressor = new DesktopCompressionHelper();
                // Intialize the stop watch
                Watch = new Stopwatch();
            } 
        #endregion
        #region >> Public Functions <<
            public DecompressedDesktop GetDecompressedReferenceDesktop(CompressedDesktop PresenterDesktop)
            {
                // New decompressed desktop
                DecompressedDesktop Desktop = new DecompressedDesktop();
                // New XOR object
                Xor = new XOR();
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // Unpack compressed desktop
                DecompressedDesktop _Desktop = Decompressor.DecompressDesktop(PresenterDesktop);
                // Calc decompression time
                Desktop.UnpackingTime.CompressionTime = StopWatch();
                // Assign decompressed image
                Desktop.Image = _Desktop.Image;
                // Assign decompressed sizes
                Desktop.DecompressionSizes = _Desktop.DecompressionSizes;
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Save the ref image in the XOR object
                Xor.SetRefImage(new Bitmap(Desktop.Image));
                // return ready to use desktop image, with cursor included!
                return Desktop;
            }
            public DecompressedDesktop GetDecompressedDifferenceDesktop(CompressedDesktop PresenterDesktop)
            {
                // New decompressed desktop
                DecompressedDesktop Desktop = new DecompressedDesktop();

                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // Unpack compressed desktop
                DecompressedDesktop _Desktop = Decompressor.DecompressDesktop(PresenterDesktop);
                // Calc decompression time
                Desktop.UnpackingTime.CompressionTime = StopWatch();
                // Assign decompressed image
                Desktop.Image = _Desktop.Image;
                // Assign decompressed sizes
                Desktop.DecompressionSizes = _Desktop.DecompressionSizes;
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Start Watch
                StartWatch();
                // Xor the difference image with the reference image, and assign the result to desktop.image
                Desktop.Image = Xor.XORing(new Bitmap(Desktop.Image));
                // Calc Xoring time
                Desktop.UnpackingTime.XoringTime = StopWatch();
                // ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                // Keep instance of reference image in XOR object to compare it later with new difference
                Xor.SetRefImage(new Bitmap(Desktop.Image));
                // return ready to use desktop image, with cursor included!
                return Desktop;
            }
        #endregion
        #region >> Helper Functions <<
            private void StartWatch()
            {
                // Reset the watch
                Watch.Reset();
                // Start the watch
                Watch.Start();
            }
            private TimeSpan StopWatch()
            {
                // Stop the watch
                Watch.Stop();
                // Return the interval
                return (Watch.Elapsed);
            }
        #endregion
    }
}