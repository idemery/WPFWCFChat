using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
namespace idemery.Remoot.ClientHelper
{
    static class ImageConverter
    {
        public static byte[] ImageToByteArray(Bitmap Img)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                Img.Save(Stream, ImageFormat.Bmp);
                return (Stream.ToArray());
            }
        }
        public static Bitmap ByteArrayToImage(byte[] Bytes)
        {
            using (MemoryStream Stream = new MemoryStream(Bytes))
            {
                Bitmap Img = new Bitmap(Image.FromStream(Stream));
                return Img;
            }
        }
    }
}