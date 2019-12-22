using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CursorControl
{
    class MouseAction
    {
        public MouseButtons mouseButton { get; set; }

        public Point mouseLocation { get; set; }

        public bool isDouble { get; set; }
    }
}
