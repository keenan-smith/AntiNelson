using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank.PB_Threads
{
    public class RCON
    {
        #region Variables
        private TcpListener listener;
        private Thread connectThread;

        private Queue<string> commands = new Queue<string>();
        #endregion

        public RCON()
        {
            listener = new TcpListener(IPAddress.Any, Instances.RCON.port);
            connectThread = new Thread(new ThreadStart(thread_Connect));
        }

        #region Functions
        public void startHooking()
        {
            listener.Start();
        }

        public void stopHooking()
        {

        }

        private void thread_Connect()
        {

        }
        #endregion
    }
}
