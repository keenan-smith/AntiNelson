using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Server;
using PointBlank.API;

namespace PointBlank.PB_Library
{
    public class lib_EventInitalizer
    {
        public lib_EventInitalizer()
        {
            initChat();
        }

        #region Init Functions
        private void initChat()
        {
            if (!PB.isServer())
                return;
            PBChat.OnMessageReceived += new PBChat.ChatMessageHandler(PBChat.ProcessCommands);
        }
        #endregion
    }
}
