using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using PointBlank.API.Server;
using PointBlank.API;
using PointBlank.PB_Extensions;

namespace PointBlank.PB_Library
{
    public class lib_EventInitalizer
    {
        public lib_EventInitalizer()
        {
            PBLogging.log("Loading EventInitalizer...");
            initChat();
            initServer();
            initAutoSave();
            initRCON();
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
            PB.OnPBPostInit += new PB.PBPostInit(PBAudio.Init);
            PBServer.OnConsoleInput += new PBServer.ConsoleInputTextHandler(PBServer.ParseInputCommand);
        }

        private void initRCON()
        {
            if (!PB.isServer())
                return;
            PBServer.OnConsoleOutput += new PBServer.ConsoleOutputTextHandler(Instances.RCON.RCONOutputUpdate);
        }

        private void initAutoSave()
        {
            if (!PB.isServer())
                return;
            Level.onPostLevelLoaded += new PostLevelLoaded(Instances.autoSave.autosave.LevelLoadedEvent);
        }
        #endregion
    }
}
