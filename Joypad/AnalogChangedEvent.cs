using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joypad
{
    public delegate void AnalogChangedEventHandler(object sender, AnalogChangedEventArgs e);
    public class AnalogChangedEventArgs : EventArgs
    {
        private int mvarX = 0;
        public int X { get { return mvarX; } }
        private int mvarY = 0;
        public int Y { get { return mvarY; } }
        private int mvarZ = 0;
        public int Z { get { return mvarZ; } }
        private int mvarRudder = 0;
        public int Rudder { get { return mvarRudder; } }
        private int mvarU = 0;
        public int U { get { return mvarU; } }
        private int mvarV = 0;
        public int V { get { return mvarV; } }

        public AnalogChangedEventArgs(int x, int y, int z, int r, int u, int v)
        {
            mvarX = x;
            mvarY = y;
            mvarZ = z;
            mvarRudder = r;
            mvarU = u;
            mvarV = v;
        }
    }
}
