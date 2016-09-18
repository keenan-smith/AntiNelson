using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;

namespace PointBlank.PB_Extensions
{
    public class RCONClient
    {
        #region Variables
        public TcpClient client;

        public bool inConsole;
        public bool Auth;
        public string IP;

        public Queue<string> execute = new Queue<string>();
        #endregion

        public RCONClient(TcpClient client)
        {
            this.client = client;
            inConsole = false;
            Auth = false;
            IP = client.Client.RemoteEndPoint.ToString();
        }

        #region Functions
        public void write(string message, bool newLine = false)
        {
            try
            {
                if (newLine)
                    message = message + (!message.Contains('\n') ? "\n" : "");
                if (string.IsNullOrEmpty(message))
                    return;

                byte[] bytes = Encoding.Unicode.GetBytes(message);
                client.GetStream().Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Could not write message! RCON!", ex, false);
            }
        }

        public void writeLog(string message)
        {
            write("[PointBlankRCON] " + message, true);
        }

        public string read()
        {
            try
            {
                string data = "";
                byte[] ch_Byte = new byte[1];
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    int fChar = stream.Read(ch_Byte, 0, 1);
                    if (fChar == 0)
                        return "";
                    string aChar = Encoding.Unicode.GetString(ch_Byte);
                    data = data + aChar;
                    if (aChar == "\n")
                        break;
                }

                return data;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Could not read message! RCON!", ex, false);
                return "";
            }
        }

        public void handle()
        {
            try
            {
                string command = "";
                while (client.Connected)
                {
                    command = read();
                    command = command.TrimEnd('\n', ' ', '\t');

                    if (command == "")
                        continue;
                    if ((command == "quit" || command == "exit" || command == "disconnect") && !inConsole)
                        break;
                    if (command == "login" && !inConsole)
                    {
                        if (Auth)
                            writeLog("You are already logged in!");
                        else
                            writeLog("Usage: login <password>");
                        continue;
                    }
                    if (!Auth)
                    {
                        writeLog("You aren't logged in!");
                        continue;
                    }
                    if (command == "logout" && !inConsole)
                    {
                        if (Auth)
                            Auth = false;
                        else
                            writeLog("You aren't logged in!");
                        continue;
                    }
                    if (command == "console")
                    {
                        if (Auth)
                            inConsole = !inConsole;
                        else
                            writeLog("You aren't logged in!");
                        continue;
                    }
                    if (inConsole)
                        lock (execute)
                            execute.Enqueue(command);
                }
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Handle broken! RCON!", ex, false);
            }
        }
        #endregion
    }
}
