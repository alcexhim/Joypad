using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Joypad.TestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            mvarDevice = Joypad.Device.GetDefaultDevice();
            if (mvarDevice == null)
            {
                MessageBox.Show("Device not found~");
                return;
            }
            mvarDevice.ButtonPressed += new ButtonEventHandler(mvarDevice_ButtonPressed);
            mvarDevice.ButtonReleased += new ButtonEventHandler(mvarDevice_ButtonReleased);
            mvarDevice.POVChanged += new POVChangedEventHandler(mvarDevice_POVChanged);
            mvarDevice.AnalogChanged += new AnalogChangedEventHandler(mvarDevice_AnalogChanged);

            VLCRemote.Connect();
        }

        private void mvarDevice_AnalogChanged(object sender, AnalogChangedEventArgs e)
        {
            Invoke(new Action<AnalogChangedEventArgs>(kkAnalogChanged), e);
            return;
            if (e.Y < 10000)
            {
                // SendKeys.SendWait("^{UP}");
                VLCRemote.ChangeVolumeRelative(5);
            }
            else if (e.Y > 50000)
            {
                // SendKeys.SendWait("^{DOWN}");
                VLCRemote.ChangeVolumeRelative(-5);
            }
        }

        private Joypad.Device mvarDevice = null;
        public Joypad.Device Device { get { return mvarDevice; } }

        private void kkButtonPressed(ButtonEventArgs e, bool state)
        {
            if (state)
            {
                listBox1.Items.Add("Button " + ((SaturnJoypadButton)e.Button).ToString() + " pressed!");
            }
            else
            {
                listBox1.Items.Add("Button " + ((SaturnJoypadButton)e.Button).ToString() + " released!");
            }
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }
        private void kkAnalogChanged(AnalogChangedEventArgs e)
        {
            lblX.Text = e.X.ToString();
            lblY.Text = e.Y.ToString();
            lblZ.Text = e.Z.ToString();
            lblR.Text = e.Rudder.ToString();
            lblU.Text = e.U.ToString();
            lblV.Text = e.V.ToString();
        }

        private void kkPovChanged(POVChangedEventArgs e)
        {
            lblPov.Text = e.Value.ToString();
        }

        private void mvarDevice_ButtonPressed(object sender, ButtonEventArgs e)
        {
            Invoke(new Action<ButtonEventArgs, bool>(kkButtonPressed), e, true);
            return;

            SixAxisJoypadButton button = (SixAxisJoypadButton)e.Button;
            switch (button)
            {
                case SixAxisJoypadButton.Select:
                {
                    // SendKeys.SendWait("^o");
                    // VLCRemote.OpenFile();
                    break;
                }
                case SixAxisJoypadButton.Start:
                {
                    // SendKeys.SendWait(" ");
                    VLCRemote.Pause();
                    break;
                }
                case SixAxisJoypadButton.Square:
                {
                    // SendKeys.SendWait("S");
                    VLCRemote.Snapshot();
                    // VLCRemote.SendCommand("help");
                    break;
                }
                case SixAxisJoypadButton.Circle:
                {
                    SendKeys.SendWait("m");
                    break;
                }
                case SixAxisJoypadButton.Triangle:
                {
                    VLCRemote.ToggleFullscreen();
                    break;
                }
                case SixAxisJoypadButton.TriggerLeft2:
                {
                    SendKeys.SendWait("{HOME}");
                    break;
                }
                case SixAxisJoypadButton.TriggerRight2:
                {
                    SendKeys.SendWait("{END}");
                    break;
                }
            }
        }
        private void mvarDevice_ButtonReleased(object sender, ButtonEventArgs e)
        {
            Invoke(new Action<ButtonEventArgs, bool>(kkButtonPressed), e, false);
            return;
        }
        private void mvarDevice_POVChanged(object sender, POVChangedEventArgs e)
        {
            Invoke(new Action<POVChangedEventArgs>(kkPovChanged), e);
            return;

            switch (e.Direction)
            {
                case POVDirection.Left:
                {
                    if (((SixAxisJoypadButton)mvarDevice.Buttons & SixAxisJoypadButton.TriggerLeft1) != 0)
                    {
                        VLCRemote.PreviousChapter();
                    }
                    else
                    {
                        VLCRemote.Seek(-5);
                    }
                    break;
                }
                case POVDirection.Right:
                {
                    if (((SixAxisJoypadButton)mvarDevice.Buttons & SixAxisJoypadButton.TriggerLeft1) != 0)
                    {
                        VLCRemote.NextChapter();
                    }
                    else
                    {
                        VLCRemote.Seek(5);
                    }
                    break;
                }
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            
            mvarDevice.Stop();
        }
    }
}
