using System.Drawing;
using Aced.Compression;
using System;
namespace idemery.Remoot.ClientHelper
{
    #region >> Helper Structures <<
        public struct Sizes
        {
            public long     SizeBefore;
            public long     SizeAfter;
        }
        public struct Times
        {
            public TimeSpan CompressionTime;
            public TimeSpan CapturingTime;
            public TimeSpan XoringTime;
        }
        public struct CompressedDesktop
        {
            public Sizes    CompressionSizes;
            public Times    PackingTime;
            public byte[]   ImageBytes;

        }
        public struct DecompressedDesktop
        {
            public Sizes    DecompressionSizes;
            public Times    UnpackingTime;
            public Bitmap   Image;
        }
    #endregion
    class DesktopCompressionHelper
    {
        #region >> Pack Desktop <<
            public CompressedDesktop CompressDesktop(Bitmap DesktopImage)
            {
                // New instance of compressed image structure
                CompressedDesktop Desktop = new CompressedDesktop();
                // Convert the given image to byte[]
                byte[] ImageToBytes = ImageConverter.ImageToByteArray(DesktopImage);
                // Store the size of image before compression
                Desktop.CompressionSizes.SizeBefore = ImageToBytes.LongLength;
                // Compressing the image
                byte[] CompressedImage = AcedDeflator.Instance.Compress(ImageToBytes, 0, ImageToBytes.Length, Global.CompressionLevel, 0, 0);
                // Store the size of image after compression
                Desktop.CompressionSizes.SizeAfter = CompressedImage.LongLength;
                // Set image byte[]
                Desktop.ImageBytes = CompressedImage;
                // Return compressed image as a CompressedDesktop object
                return (Desktop);
            }
        #endregion
        #region >> Unpack Desktop <<
            public DecompressedDesktop DecompressDesktop(CompressedDesktop CDesktop)
            {
                // New instance of decompressed image structure
                DecompressedDesktop DDesktop = new DecompressedDesktop();
                // Store the size of image before decompression
                DDesktop.DecompressionSizes.SizeBefore = CDesktop.ImageBytes.LongLength;
                // Decompressing the image
                byte[] DecompressedImageBytes = AcedInflator.Instance.Decompress(CDesktop.ImageBytes, 0, 0, 0);
                // Store the size of image after decompression
                DDesktop.DecompressionSizes.SizeAfter = DecompressedImageBytes.LongLength;
                // Set image object
                DDesktop.Image = new Bitmap(ImageConverter.ByteArrayToImage(DecompressedImageBytes));
                // Return decompressed image
                return (DDesktop);
            }
        #endregion
    }
}