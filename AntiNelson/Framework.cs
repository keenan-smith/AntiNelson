using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace PointBlank
{
    public class Framework : MonoBehaviour
    {
        public Framework()
        {
            if (Dedicator.isDedicated || Provider.isServer)
            {
                try
                {
                    if (!Directory.Exists(Variables.modsPathServer))
                        Directory.CreateDirectory(Variables.modsPathServer);
                    if (!Directory.Exists(Variables.pluginsPathServer))
                        Directory.CreateDirectory(Variables.pluginsPathServer);
                    Variables.isServer = true;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            else
            {
                try
                {
                    if (!Directory.Exists(Variables.modsPathClient))
                        Directory.CreateDirectory(Variables.modsPathClient);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            try
            {
                Variables.ads = new AppDomainSetup();
                Variables.ads.ApplicationBase = Directory.GetCurrentDirectory() + @"\Unturned_Data\Managed";
                Variables.ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                Variables.ads.DisallowBindingRedirects = !Variables.ads.DisallowCodeDownload;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            if (Variables.isServer)
            {
                foreach (string plugin in Directory.GetFiles(Variables.pluginsPathServer, ".dll"))
                {
                    if (File.Exists(plugin))
                    {
                        try
                        {
                            AppDomain ad = AppDomain.CreateDomain(Path.GetFileName(plugin).ToUpper(), null, Variables.ads);
                            Type prxyType = typeof(ASSProxy);
                            ASSProxy prxy = (ASSProxy)ad.CreateInstanceAndUnwrap(prxyType.Assembly.FullName, prxyType.FullName);
                            Assembly asm = prxy.GetAssembly(plugin);
                            
                            Variables.plugins.Add(asm, ad);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                }
            }
        }

        public void _Update()
        {
        }

        public void _OnGUI()
        {
        }
    }
}
