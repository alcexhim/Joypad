using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joypad.Internal.Windows
{
    internal class Constants
    {
        public enum JOYERR
        {
            None = 0,
            Base = 160,
            InvalidParameter = Base + 5,
            DriverNotFound = Base + 6,
            Unplugged = Base + 7
        }
        public enum JoystickInformationFlags
        {
            X = 0x1,
            Y = 0x2,
            Z = 0x4,
            R = 0x8,
            U = 0x10,
            V = 0x20,
            Pov = 0x40,
            Buttons = 0x80,
            RawData = 0x100,
            PovCTS = 0x200,
            Centered = 0x400,
            All = (X | Y | Z | R | U | V | Pov | Buttons)
        }
    }
}
