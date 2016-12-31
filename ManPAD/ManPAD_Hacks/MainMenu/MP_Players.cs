using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{
    [MenuOption(13, "Players", 300f)]
    public class MP_Players : MenuOption
    {
        #region Variables
        private List<SteamPlayer> open = new List<SteamPlayer>();

        public static List<ulong> friends = new List<ulong>();
        #endregion

        #region Mono Functions
        public void Start()
        {
            friends.AddRange(MP_Config.instance.getFriends());
        }
        #endregion

        #region Functions
        public override void runGUI()
        {
            for (int i = 0; i < Provider.clients.Count; i++)
            {
                if (Provider.clients[i] != Variables.LocalSteamPlayer)
                {
                    if (GUILayout.Button(Provider.clients[i].playerID.characterName))
                    {
                        if (open.Contains(Provider.clients[i]))
                            open.Remove(Provider.clients[i]);
                        else
                            open.Add(Provider.clients[i]);
                    }
                    if (open.Contains(Provider.clients[i]))
                        drawUI(Provider.clients[i]);
                }
            }
        }

        private void drawUI(SteamPlayer player)
        {
            if(GUILayout.Button((isFriend(player) ? "Remove" : "Add") + " Friend"))
            {
                if (isFriend(player))
                    friends.Remove(player.playerID.steamID.m_SteamID);
                else
                    friends.Add(player.playerID.steamID.m_SteamID);

                MP_Config.instance.setFriends(friends.ToArray());
            }
            GUILayout.Space(12f);
        }
        #endregion

        #region Static Functions
        public static bool isFriend(SteamPlayer player)
        {
            return friends.Contains(player.playerID.steamID.m_SteamID);
        }

        public static bool isFriend(ulong ID)
        {
            return friends.Contains(ID);
        }

        public static bool isFriend(Player player)
        {
            return friends.Contains(player.channel.owner.playerID.steamID.m_SteamID);
        }
        #endregion
    }
}
