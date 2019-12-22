using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
namespace idemery.Remoot.ClientHelper
{
    static class ImageConverter
    {
        public static byte[] ImageToByteArray(Bitmap Img)
        {
            // Memory stream instance
            MemoryStream Stream = new MemoryStream();
            // Save image to memory stream with BMP format
            Img.Save(Stream, ImageFormat.Bmp);
            // Convert stream to byte[]
            return (Stream.ToArray());
        }
        public static Bitmap ByteArrayToImage(byte[] Bytes)
        {
            // Memory stream instance
            MemoryStream Stream = new MemoryStream(Bytes);
            // Get image from memory stream
            Bitmap Img = new Bitmap(Image.FromStream(Stream));
            // Return image
            return Img;
        }
    }
}