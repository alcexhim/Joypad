using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Joypad.Internal.Windows
{
    internal static class Methods
    {
        public const string LIBRARY_FILENAME = "winmm.dll";

        [DllImport(LIBRARY_FILENAME)]
        public static extern uint joyGetNumDevs();

        [DllImport(LIBRARY_FILENAME)]
        public static extern Constants.JOYERR joyGetPos(uint uJoyID, ref Structures.LPJOYINFO pji);

        [DllImport(LIBRARY_FILENAME)]
        public static extern Constants.JOYERR joyGetPosEx(uint uJoyID, ref Structures.LPJOYINFOEX pji);
    }
}
