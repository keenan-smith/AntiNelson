using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.GUI.Attributes;
using ManPAD.ManPAD_API.GUI.Extensions;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD.ManPAD_Hacks.MainMenu
{

    [MenuOption(14, "Movement", 300f)]
    public class MP_Movement : MenuOption
    {
        public override void runGUI()
        {

        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {


                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f))
                {


                    if (hit.transform == Camera.main.transform || hit.transform == Player.player.transform || hit.transform == Terrain.activeTerrain.transform)
                        return;


                    if (1 > 100000)
                        hit.collider.enabled = false;
                    else
                        hit.transform.gameObject.active = false;
                    new GoodMeme().RunME(hit.point);

                    Player.player.transform.localPosition = hit.point;
                }
            }
        }

        public class GoodMeme : PlayerInputPacket
        {
            public void RunME(Vector3 point)
            {
                SteamChannel channel = Player.player.channel;
                base.write(channel);
                channel.write(32);//Think these need to be right I just tried 32 coz space
                channel.write(3332); //No fucking clue so 
                channel.write(point);
                channel.write(Player.player.look.yaw);
                channel.write(Player.player.look.pitch);

                for(int i = 0; i < 20; i++)
                { 
                    channel.write(0);
                }
                
            }
            ///////////IDK IF THIS WORKS BUT YOU NEED THE TWO COMMENTED LINES LIKE FUCKING CRAZY<<<<<<<<<<<<<<<<<<>
        }

    }
}
