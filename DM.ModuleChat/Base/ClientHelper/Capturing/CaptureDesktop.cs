using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace idemery.Remoot.ClientHelper
{
    public class Capture
    {
        #region >> Internal Classes & Structures <<
            class CursorInfo
            {
                public Point CursorPoint { get; set; }
                public Bitmap CursorBitmap { get; set; }
            }
            struct SIZE
            {
                public int X;
                public int Y;
            } 
        #endregion
        #region >> Public Static Functions <<
            public static Bitmap CaptureDesktop()
            {
                SIZE size;
                IntPtr hBitmap;
                IntPtr hDC = Win32.GetDC(Win32.GetDesktopWindow());
                IntPtr hMemDC = GDI.CreateCompatibleDC(hDC);
                size.X = Win32.GetSystemMetrics(Win32.SM_CXSCREEN);
                size.Y = Win32.GetSystemMetrics(Win32.SM_CYSCREEN);
                hBitmap = GDI.CreateCompatibleBitmap(hDC, size.X, size.Y);
                if (hBitmap != IntPtr.Zero)
                {
                    IntPtr hOld = (IntPtr)GDI.SelectObject(hMemDC, hBitmap);
                    GDI.BitBlt(hMemDC, 0, 0, size.X, size.Y, hDC, 0, 0, GDI.SRCCOPY);
                    GDI.SelectObject(hMemDC, hOld);
                    GDI.DeleteDC(hMemDC);
                    Win32.ReleaseDC(Win32.GetDesktopWindow(), hDC);
                    Bitmap Bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                    GDI.DeleteObject(hBitmap);
                    GC.Collect();
                    try
                    {
                        return (MergeDesktopAndCursor(Bmp, CaptureCursor()));
                    }
                    catch { return (Bmp); }
                }
                return null;
            } 
        #endregion
        #region >> Private Helper Functions <<
            static CursorInfo CaptureCursor()
            {
                Bitmap bmp;
                IntPtr hicon;
                Point point = new Point();
                CursorInfo Cursor = new CursorInfo();
                Win32.CURSORINFO ci = new Win32.CURSORINFO();
                Win32.ICONINFO icInfo;
                ci.cbSize = Marshal.SizeOf(ci);
                if (Win32.GetCursorInfo(out ci))
                {
                    if (ci.flags == Win32.CURSOR_SHOWING)
                    {
                        hicon = Win32.CopyIcon(ci.hCursor);
                        if (Win32.GetIconInfo(hicon, out icInfo))
                        {
                            point.X = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                            point.Y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);
                            Icon ic = Icon.FromHandle(hicon);
                            bmp = ic.ToBitmap();
                            Cursor.CursorBitmap = bmp;
                            Cursor.CursorPoint = point;
                            return Cursor;
                        }
                    }
                }
                return null;
            }
            static Bitmap MergeDesktopAndCursor(Bitmap DesktopBMP, CursorInfo Cursor)
            {
                Graphics g;
                Rectangle r;
                if (DesktopBMP != null)
                {
                    if (Cursor != null && Cursor.CursorBitmap != null)
                    {
                        r = new Rectangle(Cursor.CursorPoint.X, Cursor.CursorPoint.Y, Cursor.CursorBitmap.Width, Cursor.CursorBitmap.Height);
                        g = Graphics.FromImage(DesktopBMP);
                        g.DrawImage(Cursor.CursorBitmap, r);
                        g.Flush();
                        return DesktopBMP;
                    }
                    else
                        return DesktopBMP;
                }
                return null;
            } 
        #endregion
    }
}