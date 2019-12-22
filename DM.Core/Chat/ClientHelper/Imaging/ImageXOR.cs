using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
namespace idemery.Remoot.ClientHelper
{
    public class XOR
    {
        Bitmap bmpimg;
		public Bitmap XORing( Bitmap bmp )
		{
			BitmapData bmpData = bmpimg.LockBits( new Rectangle( 0 , 0 , bmpimg.Width , bmpimg.Height ) , ImageLockMode.ReadWrite , PixelFormat.Format24bppRgb );
			BitmapData bmpData2 = bmp.LockBits( new Rectangle( 0 , 0 , bmp.Width , bmp.Height ) , ImageLockMode.ReadWrite , PixelFormat.Format24bppRgb );
			int width = bmpData.Width;
			int height = bmpData.Height;
			if( bmpData2.Width > width )
				width = bmpData2.Width;
			if( bmpData2.Height > height )
				height = bmpData2.Height;
			bmpimg.UnlockBits( bmpData );
			bmp.UnlockBits( bmpData2 );
			Bitmap bit1 = new Bitmap( bmpimg , width , height );
			Bitmap bit2 = new Bitmap( bmp , width , height );
			Bitmap bmpresult = new Bitmap( width , height );
			BitmapData data1 = bit1.LockBits( new Rectangle( 0 , 0 , bit1.Width , bit1.Height ) , ImageLockMode.ReadWrite , PixelFormat.Format24bppRgb );
			BitmapData data2 = bit2.LockBits( new Rectangle( 0 , 0 , bit2.Width , bit2.Height ) , ImageLockMode.ReadWrite , PixelFormat.Format24bppRgb );
			BitmapData data3 = bmpresult.LockBits( new Rectangle( 0 , 0 , bmpresult.Width , bmpresult.Height ) , ImageLockMode.ReadWrite , PixelFormat.Format24bppRgb );
			unsafe
			{
			    int remain1 = data1.Stride - data1.Width * 3;
				int remain2 = data2.Stride - data2.Width * 3;
				int remain3 = data3.Stride - data3.Width * 3;
                byte* ptr1 = ( byte* )data1.Scan0;
				byte* ptr2 = ( byte* )data2.Scan0;
				byte* ptr3 = ( byte* )data3.Scan0;
				for( int i = 0 ; i < height ; i ++ )
				{
					for( int j = 0 ; j < width * 3 ; j ++ )
					{
						ptr3[ 0 ] = ( byte ) ( XOR_Operator( ptr1[ 0 ] , ptr2[ 0 ] ) );
						ptr1 ++;
						ptr2 ++;
						ptr3 ++;
					}
					ptr1 += remain1;
					ptr2 += remain2;
					ptr3 += remain3;
				}
			}
		    bit1.UnlockBits( data1 );
			bit2.UnlockBits( data2 );
			bmpresult.UnlockBits( data3 );
			return bmpresult;
		}		
		public byte XOR_Operator( byte a , byte b )
		{
			byte A = ( byte )( 255 - a );
			byte B = ( byte )( 255 - b );
			return ( byte )( ( a & B ) | ( A & b ) );
		}
		public Bitmap XNORing( Bitmap bmp )
		{
            // NANDing of 2 images is done by ANDing 2 images then negating the resultant
			Bitmap orimg = XORing( bmp );
            BitmapData data = orimg.LockBits( new Rectangle( 0 , 0 , orimg.Width , orimg.Height ) , ImageLockMode.ReadWrite , PixelFormat.Format24bppRgb );
			unsafe
			{
				int remain = data.Stride - data.Width * 3;
				byte* ptr = ( byte* )data.Scan0;
				for( int i = 0 ; i < data.Height ; i ++ )
				{
					for( int j = 0 ; j < data.Width * 3 ; j ++ )
					{
						ptr[ 0 ] = invert( ptr[ 0 ] );
                        ptr ++;
					}
					ptr += remain;
				}
			}
			orimg.UnlockBits( data );
			return orimg;
		}
		public byte invert( byte b )
		{
			return ( byte )( 255 - b );
		}
		public void SetRefImage( Bitmap bmp )
		{
            bmpimg = ( Bitmap )bmp.Clone( );  
		}
		public Bitmap getImage( )
		{
		    return ( Bitmap )bmpimg.Clone( );
		}
    }
}
