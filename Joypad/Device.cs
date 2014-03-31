using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Joypad
{
    public class Device
    {
        public static Device[] GetDevices()
        {
            List<Device> _devices = new List<Device>();
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                {
                    break;
                }
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                {
                    uint numdevs = Internal.Windows.Methods.joyGetNumDevs();
                    for (uint i = 0; i < numdevs; i++)
                    {
                        Device device = new Device(i);
                        if (!device.IsPresent) continue;
                        _devices.Add(device);
                    }
                    break;
                }
                case PlatformID.Xbox:
                {
                    break;
                }
            }
            return _devices.ToArray();
        }
        public static Device GetDefaultDevice()
        {
            Device[] devices = GetDevices();
            if (devices.Length > 0)
            {
                return devices[0];
            }
            return null;
        }

        private int mvarPollingInterval = 50;

        private uint mvarHandle = 0;
        public uint Handle { get { return mvarHandle; } set { mvarHandle = value; } }

        private bool mvarIsPresent = true;
        public bool IsPresent { get { return mvarIsPresent; } }

        private Internal.Windows.Structures.LPJOYINFOEX _prev_pji = new Internal.Windows.Structures.LPJOYINFOEX();
        
        private void Update()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                {
                    break;
                }
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                {
                    Internal.Windows.Structures.LPJOYINFOEX pji = new Internal.Windows.Structures.LPJOYINFOEX();
                    pji.dwSize = (uint)(Marshal.SizeOf(pji));
                    pji.dwFlags = Internal.Windows.Constants.JoystickInformationFlags.All;

                    Internal.Windows.Constants.JOYERR result = Internal.Windows.Methods.joyGetPosEx(mvarHandle, ref pji);
                    if (result == Internal.Windows.Constants.JOYERR.Unplugged || result == Internal.Windows.Constants.JOYERR.InvalidParameter)
                    {
                        bool wasPresent = false;
                        if (mvarIsPresent) wasPresent = true;

                        mvarIsPresent = false;
                        if (wasPresent) OnDisconnected(EventArgs.Empty);
                        return;
                    }
                    else
                    {
                        mvarButtons = pji.dwButtons;

                        if (!mvarIsPresent)
                        {
                            mvarIsPresent = true;
                            OnConnected(EventArgs.Empty);
                        }
                        if (pji.dwButtons != _prev_pji.dwButtons)
                        {
                            int dwButtonsPressed = (int)(pji.dwButtons & ~_prev_pji.dwButtons);
                            int dwButtonsReleased = (int)(_prev_pji.dwButtons & ~pji.dwButtons);

                            if (dwButtonsPressed > 0)
                            {
                                OnButtonPressed(new ButtonEventArgs(dwButtonsPressed));
                            }
                            if (dwButtonsReleased > 0)
                            {
                                OnButtonReleased(new ButtonEventArgs(dwButtonsReleased));
                            }
                        }
                        if (pji.dwPOV != _prev_pji.dwPOV)
                        {
                            OnPOVChanged(new POVChangedEventArgs((int)(pji.dwPOV)));
                        }
                        if ((pji.dwXpos != _prev_pji.dwXpos) || (pji.dwYpos != _prev_pji.dwYpos) || (pji.dwZpos != _prev_pji.dwZpos) || (pji.dwRpos != _prev_pji.dwRpos) || (pji.dwUpos != _prev_pji.dwUpos) || (pji.dwVpos != _prev_pji.dwVpos))
                        {
                            OnAnalogChanged(new AnalogChangedEventArgs((int)(pji.dwXpos), (int)(pji.dwYpos), (int)(pji.dwZpos), (int)(pji.dwRpos), (int)(pji.dwUpos), (int)(pji.dwVpos)));
                        }
                    }
                    _prev_pji = pji;
                    return;
                }
                case PlatformID.Xbox:
                {
                    break;
                }
            }
            throw new PlatformNotSupportedException();
        }

        private void _t_ThreadStart()
        {
            while (true)
            {
                Update();
                System.Threading.Thread.Sleep(mvarPollingInterval);
            }
        }

        private System.Threading.Thread _t = null;

        public Device(uint handle)
        {
            mvarHandle = handle;
            _prev_pji.dwPOV = 65535;

            Update();

            if (mvarIsPresent) Start();
        }

        public void Start()
        {
            if (_t != null)
            {
                _t.Abort();
                _t = null;
            }
            _t = new System.Threading.Thread(_t_ThreadStart);
            _t.Start();
        }
        public void Stop()
        {
            if (_t == null) return;
            _t.Abort();
            _t = null;
        }

        public event EventHandler Connected;
        protected virtual void OnConnected(EventArgs e)
        {
            if (Connected != null) Connected(this, e);
        }
        public event EventHandler Disconnected;
        protected virtual void OnDisconnected(EventArgs e)
        {
            if (Disconnected != null) Disconnected(this, e);
        }
        public event ButtonEventHandler ButtonPressed;
        protected virtual void OnButtonPressed(ButtonEventArgs e)
        {
            if (ButtonPressed != null) ButtonPressed(this, e);
        }
        public event ButtonEventHandler ButtonReleased;
        protected virtual void OnButtonReleased(ButtonEventArgs e)
        {
            if (ButtonReleased != null) ButtonReleased(this, e);
        }

        public event POVChangedEventHandler POVChanged;
        protected virtual void OnPOVChanged(POVChangedEventArgs e)
        {
            if (POVChanged != null) POVChanged(this, e);
        }

        public event AnalogChangedEventHandler AnalogChanged;
        protected virtual void OnAnalogChanged(AnalogChangedEventArgs e)
        {
            if (AnalogChanged != null) AnalogChanged(this, e);
        }

        private uint mvarButtons = 0;
        public uint Buttons { get { return mvarButtons; } }
    }
}
