using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Joypad.TestProject
{
    /// <summary>
    /// Joypad plugin for VLC
    /// </summary>
    public class VLCRemote
    {

        private static int mvarPort = 14212;
        public static int Port { get { return mvarPort; } set { mvarPort = value; } }

        public static void ChangeVolumeRelative(int volume)
        {
            if (volume > 0)
            {
                SendCommand("volup " + volume.ToString());
            }
            else if (volume < 0)
            {
                SendCommand("voldown " + volume.ToString());
            }
        }

        public static void Snapshot()
        {
            SendCommand("snapshot");
        }

        public static void Seek(int seconds)
        {
            string value = SendCommand("get_time");
            while (value == String.Empty) value = SendCommand("get_time");

            int elapsed = Int32.Parse(value);
            seconds += elapsed;
            string value2 = SendCommand("seek " + seconds.ToString());
        }

        public static void PreviousChapter()
        {
            SendCommand("chapter_p");
        }
        public static void NextChapter()
        {
            SendCommand("chapter_n");
        }

        public static void ToggleFullscreen()
        {
            SendCommand("f");
        }

        public static string SendCommand(string command)
        {
            // clear out the buffer
            string oldresp = GetResponse();

            mvarWriter.WriteLine(command);
            mvarWriter.Flush();
            return GetResponse();
        }
        public static string GetResponse()
        {
            // wait for available data
            // System.Threading.Thread.Sleep(500);

            if (mvarClient.Available == 0) return String.Empty;

            StringBuilder sb = new StringBuilder();
            while (mvarClient.Available > 0)
            {
                int available = mvarClient.Available;
                for (int i = 0; i < available; i++)
                {
                    char c = (char)(mvarReader.Read());
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static TcpClient mvarClient = null;
        private static System.IO.Stream mvarStream = null;
        private static System.IO.StreamReader mvarReader = null;
        private static System.IO.StreamWriter mvarWriter = null;

        public static void Connect()
        {
            if (mvarClient != null) return;
            
            mvarClient = new TcpClient();
            mvarClient.Connect("localhost", mvarPort);
            mvarStream = mvarClient.GetStream();
            mvarReader = new System.IO.StreamReader(mvarStream);
            mvarWriter = new System.IO.StreamWriter(mvarStream);
        }

        public static void Pause()
        {
            SendCommand("pause");
        }
    }
}
