using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CursorControl
{
    class SimulateAction
    {
        [DllImport("user32.dll")]

        #region MouseEvents&Enum
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
        #endregion

        #region MouseClickEvent
        private void mouseclick(MouseAction myMouseAction)
        {
            System.Windows.Forms.Cursor.Position = myMouseAction.mouseLocation;
            if (myMouseAction.isDouble)
            {
                switch (myMouseAction.mouseButton)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        mouseLeftClick();
                        //mouseLeftClick();
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        mouseRightClick();
                        //mouseRightClick();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (myMouseAction.mouseButton)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        mouseLeftClick();
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        mouseRightClick();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        private void mouseRightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        private void mouseLeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void mouseClick(MouseAction myMouseAction)
        {
            mouseclick(myMouseAction);
        }
    }
}
