using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using PointBlank.Anticheat.Extensions;
using PointBlank.API.Server;

namespace PointBlank.Anticheat.Modules
{
    internal class VeAC_AntiScreenshot : VeAC_Module
    {
        #region Variables
        private Dictionary<PBPlayer, DateTime> _queue = new Dictionary<PBPlayer, DateTime>();
        private Dictionary<byte[], DateTime> _screenshots = new Dictionary<byte[], DateTime>();

        private List<byte[]> _database = new List<byte[]>();
        #endregion

        #region Mono Functions
        public void Start()
        {
            if(VeAC_Settings.antiscreenshot_usescreenshotdatabase)
                downloadDatabase();
            PBServer.OnPlayerJoin += new PBServer.ClientJoinHandler(Event_PlayerJoin);
        }

        public void Update()
        {
            if (_queue.Count > 0)
            {
                foreach (KeyValuePair<PBPlayer, DateTime> que in _queue)
                {
                    if ((DateTime.Now - que.Value).TotalMilliseconds >= 3000)
                    {
                        que.Key.player.sendScreenshot(CSteamID.Nil, Event_PlayerReady);
                        _queue.Remove(que.Key);
                    }
                }
            }

            if (_screenshots.Count > 0)
                foreach (KeyValuePair<byte[], DateTime> screenshot in _screenshots)
                    if ((DateTime.Now - screenshot.Value).TotalMilliseconds >= 10000)
                        _screenshots.Remove(screenshot.Key);
        }
        #endregion

        #region Event Functions
        private void Event_PlayerJoin(PBPlayer player)
        {
            if (_queue.ContainsKey(player))
                return;

            _queue.Add(player, DateTime.Now);
            player.setCustomVariable("screenshotsTaken", 0);
        }

        private void Event_PlayerReady(CSteamID steamID, byte[] data)
        {
            PBPlayer player = PBServer.findPlayer(steamID);

            if (VeAC_Settings.antiscreenshot_usescreenshotdatabase)
            {
                if (Array.Exists(_database.ToArray(), a => a == data))
                {
                    punish(player);
                    return;
                }
            }

            if (_screenshots.ContainsKey(data))
            {
                punish(player);
                return;
            }
            else
            {
                _screenshots.Add(data, DateTime.Now);
                int takenScreenshots = (int)player.getCustomVariable("screenshotsTaken");
                if (takenScreenshots < 3)
                {
                    _queue.Add(player, DateTime.Now);
                    player.setCustomVariable("screenshotsTaken", ++takenScreenshots);
                }
                return;
            }
        }
        #endregion

        #region Functions
        public override bool check()
        {
            return VeAC_Settings.anti_AntiScreenshot;
        }

        private void downloadDatabase()
        {

        }

        private void punish(PBPlayer player)
        {
            if (VeAC_Settings.ban_user)
                player.ban("AntiScreenshot", 0, true, CSteamID.Nil);
            if (VeAC_Settings.kick_user)
                player.kick("AntiScreenshot");
            if (VeAC_Settings.warn_admins)
                foreach (PBPlayer ply in PBServer.players)
                    if (ply.steamPlayer.isAdmin || ply.hasPermission("pointblank.showHackers"))
                        ply.sendChatMessage("Player: " + player.playerID.playerName + " has been detected for antispy.", Color.magenta);
            if (VeAC_Settings.warn_user)
                player.sendChatMessage("You have been detected for antispy!", Color.magenta);
        }
        #endregion
    }
}
