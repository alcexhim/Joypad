using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joypad
{
    [Flags()]
    public enum SixAxisJoypadButton
    {
        None = 0,
        Square = 1,
        Cross = 2,
        Circle = 4,
        Triangle = 8,
        TriggerLeft1 = 16,
        TriggerRight1 = 32,
        TriggerLeft2 = 64,
        TriggerRight2 = 128,
        Select = 256,
        Start = 512,
        AnalogStickLeft = 1024,
        AnalogStickRight = 2048
    }
}
