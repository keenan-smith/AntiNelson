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
using PointBlank.API;

namespace PointBlank.PB_Threads
{
    public class RCON
    {
        #region Variables
        private TcpListener listener;
        private Thread connectThread;

        private bool running = true;

        private List<string> _output = new List<string>();
        private List<RCONClient> _clients = new List<RCONClient>();

        private ushort port;
        private string password;
        private bool canReadLogs;
        private bool canSendCommands;
        #endregion

        #region Properties
        public List<string> output
        {
            get
            {
                return _output;
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

        public RCON(ushort port, string password, bool canReadLogs, bool canSendCommands)
        {
            this.port = port;
            this.password = password;
            this.canReadLogs = canReadLogs;
            this.canSendCommands = canSendCommands;

            listener = new TcpListener(IPAddress.Any, port);
            connectThread = new Thread(new ThreadStart(thread_Connect));
        }

        #region Functions
        public void startHooking()
        {
            PBLogging.log("Starting RCON....", false);
            running = true;

            listener.Start();
            connectThread.Start();
            PBLogging.log("RCON started! PORT: " + port.ToString() + " Password: " + password, false);
        }

        public void stopHooking()
        {
            PBLogging.log("Shutting down RCON....");
            running = false;

            foreach (RCONClient client in clients)
            {
                client.client.Close();
                clients.Remove(client);
            }
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
                    RCONClient client = new RCONClient(tcp, this);
                    clients.Add(client);
                    client.write("PointBlankRCON v1.0 loaded!", true);
                    client.write("PointBlank v" + Assembly.GetExecutingAssembly().GetName().Version, true);
                    ThreadPool.QueueUserWorkItem(runCons, client);
                    PBLogging.log("User connected! IP: " + client.IP);
                }
            }
        }

        private static void runCons(object obj)
        {
            PBLogging.log("Initalizing client...");
            RCONClient client = (RCONClient)obj;
            client.handle();
            PBLogging.log("Client finished!");
        }
        #endregion
    }
}
