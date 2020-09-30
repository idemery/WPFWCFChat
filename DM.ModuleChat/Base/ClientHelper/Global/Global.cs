using Aced.Compression;
namespace idemery.Remoot.ClientHelper
{
    public static class Global
    {
        public static AcedCompressionLevel CompressionLevel { get; set; }
        public static AcedCompressionLevel DefaultCompressionLevel = AcedCompressionLevel.Normal;
    }
}