using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;
namespace MLoader
{
    public class Data
    {
        public int error_ID { get; set; }
        public string error_message { get; set; }
        public string hack { get; set; }
    }

    public class RootObject
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }


    public class Loading
    {
        public static byte[] response;
        public static bool httpDone;
        public static GameObject go_http;
        public static http instance_http;
        public static string HWID;

        public static void executeLoad()
        {
            Thread httpThread = new Thread(new ThreadStart(httpthread));
            httpThread.Start();
        }

        public static void httpthread()
        {
            go_http = new GameObject();
            instance_http = go_http.AddComponent<http>();
            UnityEngine.Object.DontDestroyOnLoad(go_http);
        }

        public class http : MonoBehaviour
        {
            void Start()
            {
                attach.log("Starting!");
                string hwid = "";
                Exception ex = null;
                if (attach.getHWID(out hwid, out ex) == 0)
                {
                    string url = "http://manpad.net16.net/download.php";

                    WWWForm form = new WWWForm();
                    form.AddField("stage", "2");
                    form.AddField("steam_name", SteamFriends.GetPersonaName());
                    form.AddField("steam_64", SteamUser.GetSteamID().m_SteamID.ToString());
                    form.AddField("version", Provider.APP_VERSION);
                    form.AddField("HWID", hwid);
                    WWW www = new WWW(url, form);

                    StartCoroutine(WaitForRequest(www));
                }
                else if (attach.getHWID(out hwid, out ex) == 1)
                {
                    attach.log(ex.ToString());
                }
                else
                {
                    attach.log("Exception: Not Run From Loader!");
                }
            }

            IEnumerator WaitForRequest(WWW www)
            {
                yield return www;
                    
                if (www.bytes != null)
                {
                    response = www.bytes;
                    attach.logBytes(response);
                    try
                    {
                        RootObject ro = attach.getRO(attach.convertUTF8(response));
                        if (ro.success)
                        {
                            attach.attachMP(attach.getBA(ro));
                        }
                        else
                        {
                            attach.log("Error " + ro.data.error_ID.ToString() + ", " + ro.data.error_message);
                        }
                    }
                    catch(Exception ex)
                    {
                        attach.log(ex.ToString());
                    }
                }
            }
        }

        public static class attach
        {
            public static List<string> logs = new List<string>();
            public static void attachMP(byte[] ba)
            {
                Type t = Assembly.Load(ba).GetType("ManPAD.ManPAD_Loading.Hook");
                t.GetMethod("callMeToHook").Invoke(Activator.CreateInstance(t), null);
            }

            public static void logBytes(byte[] bytes)
            {
                string log = Encoding.UTF8.GetString(bytes);
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\log.txt", Encoding.UTF8.GetString(bytes));
            }

            public static string convertUTF8(byte[] bytes)
            {
                return Encoding.UTF8.GetString(bytes);
            }

            public static RootObject getRO(string json)
            {
                return JsonConvert.DeserializeObject<RootObject>(json);
            }

            public static byte[] getBA(RootObject data)
            {
                return Convert.FromBase64String(data.data.hack);
            }

            public static int getHWID(out string hwid, out Exception ex)
            {
                string dir = Directory.GetCurrentDirectory() + @"\Bundles\Level\Hidden.dat";
                int toReturn;
                try
                {
                    //if(System.IO.File.GetLastWriteTime(dir) <= DateTime.Now.AddSeconds(5))
                    //{
                        hwid = File.ReadAllText(dir);
                        ex = null;
                        toReturn = 0;
                    /*}
                    else
                    {
                        hwid = null;
                        ex = null;
                        toReturn = 2;
                    }*/
                    return toReturn;
                }
                catch(Exception x)
                {
                    hwid = null;
                    ex = x;
                    return 1;
                }
            }
            public static void log(string toLog)
            {
                logs.Add(DateTime.Now.ToString("h:mm:ss tt") + ": " + toLog);
                File.WriteAllLines(Directory.GetCurrentDirectory() + @"\log.txt", logs.ToArray());
            }
        }
    }
}
