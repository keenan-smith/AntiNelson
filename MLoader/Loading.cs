using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace MLoader
{

    public class Loading
    {
        public static byte[] response;
        public static bool httpDone;
        public static GameObject go_http;
        public static http instance_http;

        /*[DllImport("evb31E4.tmp", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr __HWID();

        public static string getHWID()
        {

        String curDir = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(Environment.ExpandEnvironmentVariables("%temp%"));
        File.WriteAllBytes("evb31E4.tmp", IntPtr.Size == 8 ? HWID.ArrayContainer.hwid64 : HWID.ArrayContainer.hwid32);
        IntPtr hwid = __HWID();
        Directory.SetCurrentDirectory(curDir);

        int read = 0;
        while (Marshal.ReadByte(hwid, read) != 0)
        read++;

        byte[] ba = new byte[read];
        Marshal.Copy(hwid, ba, 0, read);

        return Encoding.UTF8.GetString(ba);

        }*/

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
                string url = "http://manpad.net16.net/download.php";

                WWWForm form = new WWWForm();
                form.AddField("stage", "2");
                form.AddField("steam_name", SteamFriends.GetPersonaName());
                form.AddField("steam_64", SteamUser.GetSteamID().m_SteamID.ToString());
                form.AddField("version", Provider.APP_VERSION);
                WWW www = new WWW(url, form);

                StartCoroutine(WaitForRequest(www));
            }

            IEnumerator WaitForRequest(WWW www)
            {
                yield return www;
                    
                if (www.bytes != null)
                {
                    response = www.bytes;
                    AttachManPAD.attach();
                    AttachManPAD.logBytes(www.bytes);
                }
            }
        }

        public static class AttachManPAD
        {
            public static void attach()
            {
                byte[] ba = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\ManPAD.dll");

                Type t = Assembly.Load(ba).GetType("ManPAD.ManPAD_Loading.Hook");
                t.GetMethod("callMeToHook").Invoke(Activator.CreateInstance(t), null);
            }

            public static void logBytes(byte[] bytes)
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\log.txt", System.Text.Encoding.UTF8.GetString(bytes));
            }
        }
    }
}
