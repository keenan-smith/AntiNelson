using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Server;
using PointBlank.API;

namespace PointBlank.PB_Library
{
    public class lib_EventInitalizer
    {
        public lib_EventInitalizer()
        {
            PBLogging.log("Loading EventInitalizer...");
            initChat();
            initServer();
        }

        #region Init Functions
        private void initChat()
        {
            if (!PB.isServer())
                return;
        }

        private void initServer()
        {
            if (!PB.isServer())
                return;
            Provider.onEnemyConnected += new Provider.EnemyConnected(PBServer.ClientConnect);
            Provider.onEnemyDisconnected += new Provider.EnemyDisconnected(PBServer.ClientDisconnect);
            PBServer.OnPlayerJoin += new PBServer.ClientJoinHandler(PBServer.PlayerJoin);
            PBServer.OnPlayerLeave += new PBServer.ClientLeaveHandler(PBServer.PlayerLeave);
            PB.OnPBPostInit += new PB.PBPostInit(PBServer.PBPostInit);
            PB.OnPBPreInit += new PB.PBPreInit(PBServer.PBPreInit);
        }
        #endregion
    }
}
