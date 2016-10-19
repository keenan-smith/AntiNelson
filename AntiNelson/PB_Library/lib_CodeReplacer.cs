using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Extensions;
using PointBlank.API.Attributes;
using PointBlank.PB_Library;
using SDG.Unturned;
using UnityEngine;

namespace PointBlank.PB_Library
{
    internal class lib_CodeReplacer
    {
        #region Variables
        private List<ReplaceCodeAttribute> rcas = new List<ReplaceCodeAttribute>();
        private List<ReplaceCodeAttribute> ignores = new List<ReplaceCodeAttribute>();
        #endregion

        public lib_CodeReplacer()
        {
            PBLogging.log("Loading CodeReplacer...");
            loadCodes(AppDomain.CurrentDomain);
        }

        #region Functions
        public void reload()
        {
            loadCodes(AppDomain.CurrentDomain);
        }

        public bool loadCodes(AppDomain domain)
        {
            try
            {
                foreach (Assembly asm in domain.GetAssemblies())
                {
                    foreach (Type t in asm.GetTypes())
                    {
                        if (t.IsClass)
                        {
                            foreach (MethodInfo mi in t.GetMethods())
                            {
                                ReplaceCodeAttribute rca = (ReplaceCodeAttribute)Attribute.GetCustomAttribute(mi, typeof(ReplaceCodeAttribute));
                                if (rca != null)
                                {
                                    if (Array.Exists(rcas.ToArray(), a => a.method == rca.method) || Array.Exists(ignores.ToArray(), b => b.method == rca.method))
                                        continue;
                                    rca.callState = RedirectionHelper.RedirectCalls(rca.method, mi);
                                    if (t.FullName.ToLower().StartsWith("pointblank.pb_overridables"))
                                        ignores.Add(rca);
                                    else
                                        rcas.Add(rca);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while trying to load codes!", ex);
                return false;
            }
        }

        public bool loadCodes(Assembly asm)
        {
            try
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsClass)
                    {
                        foreach (MethodInfo mi in t.GetMethods())
                        {
                            ReplaceCodeAttribute rca = (ReplaceCodeAttribute)Attribute.GetCustomAttribute(mi, typeof(ReplaceCodeAttribute));
                            if (rca != null)
                            {
                                if (Array.Exists(rcas.ToArray(), a => a.method == rca.method) || Array.Exists(ignores.ToArray(), b => b.method == rca.method))
                                    continue;
                                rca.callState = RedirectionHelper.RedirectCalls(rca.method, mi);
                                if (t.FullName.ToLower().StartsWith("pointblank.pb_overridables"))
                                    ignores.Add(rca);
                                else
                                    rcas.Add(rca);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while trying to load codes!", ex);
                return false;
            }
        }

        public bool shutdown()
        {
            try
            {
                foreach (ReplaceCodeAttribute rca in rcas)
                {
                    RedirectionHelper.RevertRedirect(rca.method, rca.callState);
                }
                rcas.Clear();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR: Exception while trying to remove redirections!", ex);
                return false;
            }
        }
        #endregion
    }
}
