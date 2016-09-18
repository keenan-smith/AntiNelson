using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.PB_Extensions;

namespace PointBlank.PB_Threads
{
    public class RCON
    {
        #region Variables
        private TcpListener listener;
        private Thread connectThread;

        private bool running = true;

        private Queue<string> _commands = new Queue<string>();
        private List<string> _output = new List<string>();
        private List<RCONClient> _clients = new List<RCONClient>();
        #endregion

        #region Properties
        public List<string> output
        {
            get
            {
                return _output;
            }
        }

        public Queue<string> commands
        {
            get
            {
                return _commands;
            }
        }

        public List<RCONClient> clients
        {
            get
            {
                return _clients;
            }
        }
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
            connectThread.Start();
        }

        public void stopHooking()
        {
            running = false;

            connectThread.Abort();
            listener.Stop();
        }

        private void thread_Connect()
        {
            while (running)
            {
                TcpClient tcp = listener.AcceptTcpClient();
                if (tcp != null)
                {
                    RCONClient client = new RCONClient(tcp);
                    clients.Add(client);
                    client.write("PointBlankRCON v1.0 loaded!", true);
                    client.write("PointBlank v" + Assembly.GetExecutingAssembly().GetName().Version, true);
                }
            }
        }
        #endregion
    }
}
