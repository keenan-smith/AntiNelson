using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Attributes;
using System.Reflection;
using Steamworks;
using System.Collections;

namespace ManPAD.ManPAD_Overridables
{
#if !FREE
    public class OV_Player : MonoBehaviour
    {
        [CodeReplace("askScreenshot", typeof(Player), BindingFlags.Instance | BindingFlags.Public)]
        public void askScreenshot(CSteamID id)
        {
            if (Player.player.channel.checkServer(id))
            {
                StartCoroutine(spyStuff());
            }
        }

        public IEnumerator spyStuff()
        {
            Variables.isSpying = true;
            ManPAD_Library.iESP.removeHighlighters();
            yield return new WaitForSeconds(0.01f);
            Player.player.StartCoroutine("takeScreenshot");
            yield return new WaitForSeconds(0.01f); // nib if they spam they can see da menu df lol
            Variables.isSpying = false;
        }
    }
#endif
}
