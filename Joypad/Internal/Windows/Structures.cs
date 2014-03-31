using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joypad.Internal.Windows
{
    internal class Structures
    {
        public struct LPJOYINFO
        {
            public uint wXpos;
            public uint wYpos;
            public uint wZpos;
            public uint wButtons;
        }
        public struct LPJOYINFOEX
        {
            public uint dwSize;
            public Constants.JoystickInformationFlags dwFlags;
            public uint dwXpos;
            public uint dwYpos;
            public uint dwZpos;
            public uint dwRpos;
            public uint dwUpos;
            public uint dwVpos;
            public uint dwButtons;
            public uint dwButtonNumber;
            public uint dwPOV;
            public uint dwReserved1;
            public uint dwReserved2;
        }
    }
}
