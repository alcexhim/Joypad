using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joypad
{
    public delegate void POVChangedEventHandler(object sender, POVChangedEventArgs e);
    public class POVChangedEventArgs : EventArgs
    {
        private int mvarValue = -1;
        public int Value { get { return mvarValue; } set { mvarValue = value; } }

        private POVDirection mvarDirection = POVDirection.None;
        public POVDirection Direction { get { return mvarDirection; } set { mvarDirection = value; } }

        public POVChangedEventArgs(int value)
        {
            mvarValue = value;
            mvarDirection = (POVDirection)value;
        }
    }
}
