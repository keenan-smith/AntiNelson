// SOME(MOST) OF THE CODE WAS BORROWED FROM ROCKETMOD!
// IT HAS MOSTLY BEEN MODIFIED THOUGH ;D
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.PB_Threads;

namespace PointBlank.PB_Extensions
{
    public class RCONClient
    {
        #region Variables
        public TcpClient client;
        public RCON rcon;
        public Thread thread;

        public bool inConsole;
        public bool Auth;
        public string IP;

        public Queue<string> execute = new Queue<string>();
        #endregion

        public RCONClient(TcpClient client, RCON rcon)
        {
            this.client = client;
            this.rcon = rcon;
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
                    message = message + (!message.Contains('\n') ? "\r\n" : "");
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
                    char aChar = Convert.ToChar(ch_Byte[0]);
                    data = data + aChar;
                    if (aChar == '\n')
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
                    command = command.TrimEnd('\n', '\r', ' ', '\t');
                    string[] cmds = command.Split(' ');

                    if (command == "")
                        continue;
                    if ((command == "quit" || command == "exit" || command == "disconnect") && !inConsole)
                        break;
                    if ((command == "help" || command == "?") && !inConsole)
                    {
                        writeLog("quit/exit/disconnects - Disconnects you from the RCON");
                        writeLog("help/? - Shows this");
                        writeLog("login <password> - Login to the system");
                        writeLog("logout - logs you out of the system");
                        writeLog("console - Opens/closes the unturned console");
                    }
                    if (command == "login" && !inConsole)
                    {
                        if (Auth)
                            writeLog("You are already logged in!");
                        else
                            writeLog("Usage: login <password>");
                        continue;
                    }
                    if (cmds.Length > 1 && !inConsole)
                    {
                        if (cmds[0] == "login")
                        {
                            if (Auth)
                            {
                                writeLog("You are already logged in!");
                                continue;
                            }
                            else
                            {
                                if (cmds[1] == Instances.RCON.password)
                                {
                                    Auth = true;
                                }
                                else
                                {
                                    writeLog("Invalid password!");
                                    continue;
                                }
                            }
                        }
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

                rcon.clients.Remove(this);
                writeLog("Disconnected!");
                client.Close();
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Handle broken! RCON!", ex, false);
            }
        }
        #endregion
    }
}
