using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joypad
{
    public delegate void ButtonEventHandler(object sender, ButtonEventArgs e);
    public class ButtonEventArgs : EventArgs
    {
        private int mvarButton = 0;
        public int Button { get { return mvarButton; } set { mvarButton = value; } }

        public ButtonEventArgs(int button)
        {
            mvarButton = button;
        }
    }
}
