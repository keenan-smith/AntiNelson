using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;

namespace PointBlank.API.Server
{
    public class PBAudio
    {
        #region Variables
        private static byte[] buffer;
        #endregion

        #region Functions
        public static void sendBytes(string[] bytes, PBPlayer player)
        {
            player.player.voice.channel.send("tellVoice", ESteamCall.OWNER, player.player.transform.position, EffectManager.MEDIUM, ESteamPacket.UPDATE_VOICE, bytes, bytes.Length);
        }
        #endregion

        #region Event Functions
        public static void Init()
        {
            buffer = new byte[8004];
        }
        #endregion
    }
}
