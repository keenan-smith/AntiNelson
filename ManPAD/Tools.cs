using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Resources;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace ManPAD
{
    public class Tools
    {
        public static GUIStyle labelstyle = new GUIStyle();

        public static float getDistance(Vector3 point)
        {
            Vector3 firstPosition = MainCamera.instance.transform.position;
            Vector3 heading;
            float distanceSquared;
            float distance;

            heading.x = firstPosition.x - point.x;
            heading.y = firstPosition.y - point.y;
            heading.z = firstPosition.z - point.z;

            distanceSquared = heading.x * heading.x + heading.y * heading.y + heading.z * heading.z;
            distance = Mathf.Sqrt(distanceSquared);
            
            return distance;
        }

        public static void DrawLabel(Vector3 point, string label, Color color, int size = 15, float fromY = 0f)
        {
            labelstyle.fontSize = size;
            labelstyle.normal.textColor = color;
            GUI.Label(new Rect(point.x, point.y + fromY, Screen.width, Screen.height), label, labelstyle);
        }

        public static void Outline(Rect r, Texture2D lineTex)
        {
            Rect rect = new Rect(r.xMin, r.yMax, r.width, 1f);
            Rect rect2 = new Rect(r.xMin, r.yMin, r.width, 1f);
            Rect rect3 = new Rect(r.xMax, r.yMin, 1f, r.height);
            Rect rect4 = new Rect(r.xMin, r.yMin, 1f, r.height);
            GUI.DrawTexture(rect, lineTex);
            GUI.DrawTexture(rect3, lineTex);
            GUI.DrawTexture(rect2, lineTex);
            GUI.DrawTexture(rect4, lineTex);
        }

        public static Rect BoundsToScreenRect(Bounds b)
        {
            Camera main = MainCamera.instance;
            Vector3[] array = new Vector3[]
            {
                main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z)),
                main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z)),
                main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z)),
                main.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z)),
                main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z)),
                main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z)),
                main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z)),
                main.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z))
            };
            for (int i = 0; i < array.Length; i++)
            {
                array[i].y = (float)Screen.height - array[i].y;
            }
            Vector3 vector = array[0];
            Vector3 vector2 = array[0];
            for (int j = 1; j < array.Length; j++)
            {
                vector = Vector3.Min(vector, array[j]);
                vector2 = Vector3.Max(vector2, array[j]);
            }
            Rect result = Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
            result.xMin = result.xMin - 1f;
            result.xMax = result.xMax + 1f;
            result.yMin = result.yMin - 1f;
            result.yMax = result.yMax + 1f;
            return result;
        }

        public static string getZombieName(Zombie z)
        {
            string str = "";
            switch (z.speciality)
            {
                case EZombieSpeciality.ACID:
                    str = "Acid Zombie";
                    break;
                case EZombieSpeciality.BURNER:
                    str = "Burner Zombie";
                    break;
                case EZombieSpeciality.CRAWLER:
                    str = "Crawler Zombie";
                    break;
                case EZombieSpeciality.FLANKER_FRIENDLY:
                    str = "Friendly Flanker Zombie";
                    break;
                case EZombieSpeciality.FLANKER_STALK:
                    str = "Flanker Zombie";
                    break;
                case EZombieSpeciality.MEGA:
                    str = "Mega Zombie";
                    break;
                case EZombieSpeciality.SPRINTER:
                    str = "Sprinter Zombie";
                    break;
                default:
                    str = "Normal Zombie";
                    break;
            }
            return str;
        }

        public static Player getNearestPlayer(float maxDistance = float.MaxValue )
        {
            float distance = float.MaxValue;
            Player toAttack = null;
            float fov = ManPAD_Hacks.MainMenu.MP_Aimbot.FOV;
            bool IgnoreSomeFovLikeThing = ManPAD_Hacks.MainMenu.MP_Aimbot.ignoreFOV;

            for (int i = 0; i < Provider.clients.Count; i++)
            {
                SteamPlayer p = Provider.clients[i];
                if (p.player == Player.player || p.player.life.isDead)
                    continue;
       
                float dist = getDistance(p.player.transform.position);
                Vector3 v2dist = Camera.main.WorldToScreenPoint(p.player.transform.position);

                Vector2 pos = new Vector2(v2dist.x, v2dist.y);
                float vdist = Vector2.Distance(new Vector2(Screen.width / 2, Screen.height / 2), pos);

                if (dist > maxDistance || vdist >= fov && !IgnoreSomeFovLikeThing)
                    continue;

                if (dist < distance)
                {
                    toAttack = p.player;
                    distance = dist;
                }
            }

            return toAttack;
        }

        public static void IncStat(EPlayerStat stat, float amount)
        {
            amount = Mathf.Round(amount);
            for (int i = 0; i < amount; i++)
            {
                Player.player.sendStat(stat);
            }
            Debug.Log("Successfully increased stat: " + stat.ToString() + " " + amount + " times.");
        }
    }
}
