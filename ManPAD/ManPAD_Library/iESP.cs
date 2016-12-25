using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API.Enumerables;
using ManPAD.ManPAD_API.Types;
using ManPAD.ManPAD_Hacks.MainMenu;
using ManPAD.ManPAD_API;
using HighlightingSystem;

namespace ManPAD.ManPAD_Library
{
    public class iESP : MonoBehaviour
    {
        public static SteamPlayer[] players;
        public static InteractableItem[] items;
        public static InteractableVehicle[] vehicles;
        public static InteractableStorage[] storages;
        public static InteractableBed[] beds;
        public static InteractableGenerator[] geners;
        public static InteractableClaim[] claims;

        private static List<Highlighter> highlighters = new List<Highlighter>();

        public Camera main;

        public Material mat = null;

        public static void DrawLabel(Vector3 pos, string text)
        {
            GUIContent content = new GUIContent(text);
            if (MP_ic3ESP.CenteredLabels)
            {
                pos.x -= GUIStyle.none.CalcSize(content).x / 2f;
            }
            GUI.Label(new Rect(pos.x, pos.y, 300f, 80f), content);
        }

        public void ScanObjects()
        {
            if (Provider.isConnected && !Provider.isLoading)
            {
                players = Provider.clients.ToArray();
                items = FindObjectsOfType<InteractableItem>();
                vehicles = FindObjectsOfType<InteractableVehicle>();
                storages = FindObjectsOfType<InteractableStorage>();
                beds = FindObjectsOfType<InteractableBed>();
                geners = FindObjectsOfType<InteractableGenerator>();
                claims = FindObjectsOfType<InteractableClaim>();
            }
        }

        public void Start()
        {
            InvokeRepeating("ScanObjects", 2, 5);
            mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            mat.hideFlags = HideFlags.HideAndDontSave;
            mat.SetInt("_SrcBlend", 5);
            mat.SetInt("_DstBlend", 10);
            mat.SetInt("_Cull", 0);
            mat.SetInt("_ZWrite", 0);
            StartCoroutine(DoGlowESP());
        }

        public void OnGUI()
        {
            if (Variables.isSpying)
                return;

            if (MP_ic3ESP.ESP_Enabled)
            {
                drawThings();
            }
        }

        private void drawThings()
        {
            main = Camera.main;
            if (Event.current.type == EventType.repaint)
            {
                if (MP_ic3ESP.PlayerESP)
                    drawPlayers();
                if (MP_ic3ESP.ItemESP)
                    drawItems();
                if (MP_ic3ESP.VehicleESP)
                    drawVehicles();
                if (MP_ic3ESP.StorageESP)
                    drawStorages();
                if (MP_ic3ESP.BedESP)
                    drawBeds();
                if (MP_ic3ESP.GeneratorESP)
                    drawGenerators();
                if (MP_ic3ESP.ClaimFlagESP)
                    drawClaims();
            }
        }

        private void drawPlayers()
        {
            try
            {
                for (int i = 0; i < players.Length; i++)
                {
                    SteamPlayer plr = players[i];
                    if (plr != null && plr.player != Player.player && !plr.player.life.isDead && plr.player.transform != null)
                    {
                        string color = MP_ic3ESP.isFriend(plr) ? "#0CDCE8" : "red";
                        Vector3 position = plr.player.transform.position;
                        Vector3 vec = main.WorldToScreenPoint(position);
                        float dist = Vector3.Distance(main.transform.position, position);
                        if (vec.z > 0)
                        {
                            if (MP_ic3ESP.InfiniteDistance || dist < MP_ic3ESP.Distance)
                            {
                                vec.y = Screen.height - vec.y;
                                ItemAsset asset = plr.player.equipment.asset;
                                string extraText = "";
                                if (asset != null)
                                {
                                    extraText = "\n<size=11>" + asset.itemName + "</size>";
                                }
                                DrawLabel(vec, string.Format("<size=12><color={3}>{0}{1}</color></size>\n{2}", plr.playerID.playerName, extraText, Mathf.Round(dist), color));
                                if (MP_ic3ESP.PlayerBox)
                                {
                                    Bounds bounds = new Bounds(plr.player.transform.position + new Vector3(0, 1.1f, 0), plr.player.transform.localScale + new Vector3(0, .95f, 0));
                                    DrawOutline(bounds);
                                }
                                if (MP_ic3ESP.PlayerLine)
                                {
                                    GL.PushMatrix();
                                    GL.Begin(1);
                                    mat.SetPass(0);
                                    GL.Color(Color.red);

                                    GL.Vertex3(Screen.width / 2, Screen.height / 2, 0);
                                    GL.Vertex3(vec.x, vec.y, 0);

                                    GL.End();
                                    GL.PopMatrix();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void drawItems()
        {
            for (int i = 0; i < items.Length; i++)
            {
                InteractableItem item = items[i];
                if (item != null && item.gameObject != null)
                {
                    bool shown = false;
                    if (MP_ic3ESP.FilterItems && MP_iItemFilters.isItemWhitelisted(item.asset))
                        shown = true;
                    if (!MP_ic3ESP.FilterItems)
                        shown = true;
                    if (shown)
                    {
                        Vector3 position = item.transform.position;
                        Vector3 vec = main.WorldToScreenPoint(position);
                        float dist = Vector3.Distance(main.transform.position, position);
                        if (vec.z > 0)
                        {
                            if (MP_ic3ESP.InfiniteDistance || dist < MP_ic3ESP.Distance)
                            {
                                vec.y = Screen.height - vec.y;
                                DrawLabel(vec, string.Format("<size=12><color=lime>{0}</color></size>\n{1}", item.asset.itemName, Mathf.Round(dist)));
                            }
                        }
                    }
                }
            }
        }

        private void drawVehicles()
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                InteractableVehicle veh = vehicles[i];
                if (veh != null)
                {
                    Vector3 position = veh.transform.position;
                    Vector3 vec = main.WorldToScreenPoint(position);
                    float dist = Vector3.Distance(main.transform.position, position);
                    if (vec.z > 0)
                    {
                        if (MP_ic3ESP.InfiniteDistance || dist < MP_ic3ESP.Distance || (MP_ic3ESP.FilterLockedVehicles && veh.checkUseable()))
                        {
                            vec.y = Screen.height - vec.y;
                            DrawLabel(vec, string.Format("<size=12><color=yellow>{0}</color></size>\n{1}", veh.asset.vehicleName, Mathf.Round(dist)));
                        }
                    }
                }
            }
        }

        private void drawStorages()
        {
            for (int i = 0; i < storages.Length; i++)
            {
                InteractableStorage storage = storages[i];
                if (storage != null)
                {
                    Vector3 position = storage.transform.position;
                    Vector3 vec = main.WorldToScreenPoint(position);
                    float dist = Vector3.Distance(main.transform.position, position);
                    if (vec.z > 0)
                    {
                        if (MP_ic3ESP.InfiniteDistance || dist < MP_ic3ESP.Distance || (MP_ic3ESP.FilterLockedStorages && storage.checkUseable()))
                        {
                            vec.y = Screen.height - vec.y;
                            DrawLabel(vec, string.Format("<size=12><color=cyan>{0}</color></size>\n{1}", "Storage " + (storage.checkUseable() ? "<color=lime>[Unlocked]</color>" : "<color=red>[Locked]</color>"), Mathf.Round(dist)));
                        }
                    }
                }
            }
        }

        private void drawBeds()
        {
            for (int i = 0; i < beds.Length; i++)
            {
                InteractableBed bed = beds[i];
                if (bed != null)
                {
                    Vector3 position = bed.transform.position;
                    Vector3 vec = main.WorldToScreenPoint(position);
                    float dist = Vector3.Distance(main.transform.position, position);
                    if (vec.z > 0)
                    {
                        if (MP_ic3ESP.InfiniteDistance || dist < MP_ic3ESP.Distance)
                        {
                            vec.y = Screen.height - vec.y;
                            string txt = bed.isClaimed ? "<color=red>Claimed</color>" : "<color=lime>Unclaimed</color>";
                            DrawLabel(vec, string.Format("<size=12><color=#3F8CBC>{0}</color></size>\n{1}", string.Format("Bed {0}", txt), Mathf.Round(dist)));
                        }
                    }
                }
            }
        }

        private void drawGenerators()
        {
            for (int i = 0; i < geners.Length; i++)
            {
                InteractableGenerator generator = geners[i];
                if (generator != null)
                {
                    Vector3 position = generator.transform.position;
                    Vector3 vec = main.WorldToScreenPoint(position);
                    float dist = Vector3.Distance(main.transform.position, position);
                    if (vec.z > 0)
                    {
                        if (MP_ic3ESP.InfiniteDistance || dist < MP_ic3ESP.Distance)
                        {
                            vec.y = Screen.height - vec.y;
                            string txt = generator.isPowered ? "On" : "Off";
                            DrawLabel(vec, string.Format("<size=12><color=#43A260>{0}</color></size>\n{1}", "Generator - " + txt, Mathf.Round(dist)));
                        }
                    }
                }
            }
        }

        private void drawClaims()
        {
            for (int i = 0; i < claims.Length; i++)
            {
                InteractableClaim claim = claims[i];
                if (claim != null)
                {
                    Vector3 position = claim.transform.position;
                    Vector3 vec = main.WorldToScreenPoint(position);
                    float dist = Vector3.Distance(main.transform.position, position);
                    if (vec.z > 0)
                    {
                        if (MP_ic3ESP.InfiniteDistance || dist < MP_ic3ESP.Distance)
                        {
                            vec.y = Screen.height - vec.y;
                            DrawLabel(vec, string.Format("<size=12><color=cyan>{0}</color></size>\n{1}", "Claim Flag", Mathf.Round(dist)));
                        }
                    }
                }
            }
        }

        private IEnumerator DoGlowESP()
        {
            while (true)
            {
                if (!MP_ic3ESP.GlowESP || !Provider.isConnected || Provider.isLoading || LoadingUI.isBlocked || Player.player == null || Provider.clients.Count < 1)
                {
                    yield return new WaitForSeconds(2f);
                    continue;
                }

                if (MP_ic3ESP.PlayerESP)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        Player plr = players[i].player;
                        if (plr != null && plr.gameObject != null && !plr.life.isDead)
                        {
                            Highlighter highlighter = plr.gameObject.GetComponent<Highlighter>();
                            if (highlighter == null)
                            {
                                highlighter = plr.gameObject.AddComponent<Highlighter>();
                                highlighter.OccluderOn();
                                highlighter.SeeThroughOn();
                                highlighter.ConstantOnImmediate(MP_ic3ESP.isFriend(plr) ? Color.cyan : Color.red);
                                highlighters.Add(highlighter);
                            }
                        }
                    }
                }
                if (MP_ic3ESP.ItemESP)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        InteractableItem item = items[i];
                        if (item != null && item.gameObject != null)
                        {
                            bool shown = false;
                            if (MP_ic3ESP.FilterItems && MP_iItemFilters.isItemWhitelisted(item.asset))
                                shown = true;
                            if (!MP_ic3ESP.FilterItems)
                                shown = true;
                            Highlighter highlighter = item.gameObject.GetComponent<Highlighter>();
                            if (highlighter == null && shown)
                            {
                                highlighter = item.gameObject.AddComponent<Highlighter>();
                                highlighter.OccluderOn();
                                highlighter.SeeThroughOn();
                                highlighter.ConstantOnImmediate(Color.green);
                                highlighters.Add(highlighter);
                            }
                            else if (highlighter != null && !shown)
                            {
                                highlighter.ConstantOffImmediate();
                                Destroy(highlighter);
                                highlighters.Remove(highlighter);
                            }
                        }
                    }
                }
                if (MP_ic3ESP.VehicleESP)
                {
                    for (int i = 0; i < vehicles.Length; i++)
                    {
                        InteractableVehicle vehicle = vehicles[i];
                        if (vehicle != null && vehicle.gameObject != null)
                        {
                            Highlighter highlighter = vehicle.gameObject.GetComponent<Highlighter>();
                            if (highlighter == null)
                            {
                                highlighter = vehicle.gameObject.AddComponent<Highlighter>();
                                highlighter.OccluderOn();
                                highlighter.SeeThroughOn();
                                highlighter.ConstantOnImmediate(Color.yellow);
                                highlighters.Add(highlighter);
                            }
                        }
                    }
                }
                if (MP_ic3ESP.StorageESP)
                {
                    for (int i = 0; i < storages.Length; i++)
                    {
                        InteractableStorage storage = storages[i];
                        if (storage != null && storage.gameObject != null)
                        {
                            Highlighter highlighter = storage.gameObject.GetComponent<Highlighter>();
                            if (highlighter == null)
                            {
                                highlighter = storage.gameObject.AddComponent<Highlighter>();
                                highlighter.OccluderOn();
                                highlighter.SeeThroughOn();
                                highlighter.ConstantOnImmediate(storage.checkUseable() ? Color.cyan : Color.red);
                                highlighters.Add(highlighter);
                            }
                        }
                    }
                }
                if (MP_ic3ESP.BedESP)
                {
                    for (int i = 0; i < beds.Length; i++)
                    {
                        InteractableBed bed = beds[i];
                        if (bed != null && bed.gameObject != null)
                        {
                            Highlighter highlighter = bed.gameObject.GetComponent<Highlighter>();
                            if (highlighter == null)
                            {
                                highlighter = bed.gameObject.AddComponent<Highlighter>();
                                highlighter.OccluderOn();
                                highlighter.SeeThroughOn();
                                highlighter.ConstantOnImmediate(bed.isClaimed ? Color.red : Color.green);
                                highlighters.Add(highlighter);
                            }
                        }
                    }
                }
                if (MP_ic3ESP.GeneratorESP)
                {
                    for (int i = 0; i < geners.Length; i++)
                    {
                        InteractableGenerator generator = geners[i];
                        if (generator != null && generator.gameObject != null)
                        {
                            Highlighter highlighter = generator.gameObject.GetComponent<Highlighter>();
                            if (highlighter == null)
                            {
                                highlighter = generator.gameObject.AddComponent<Highlighter>();
                                highlighter.OccluderOn();
                                highlighter.SeeThroughOn();
                                highlighter.ConstantOnImmediate(generator.isPowered ? Color.green : Color.red);
                                highlighters.Add(highlighter);
                            }
                        }
                    }
                }
                if (MP_ic3ESP.ClaimFlagESP)
                {
                    for (int i = 0; i < claims.Length; i++)
                    {
                        InteractableClaim claim = claims[i];
                        if (claim != null && claim.gameObject != null)
                        {
                            Highlighter highlighter = claim.gameObject.GetComponent<Highlighter>();
                            if (highlighter == null)
                            {
                                highlighter = claim.gameObject.AddComponent<Highlighter>();
                                highlighter.OccluderOn();
                                highlighter.SeeThroughOn();
                                highlighter.ConstantOnImmediate(Color.cyan);
                                highlighters.Add(highlighter);
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(3f);
            }
        }

        public static void removeHighlighters()
        {
            for (int i = 0; i < highlighters.ToArray().Length; i++)
            {
                Highlighter hl = highlighters[i];
                if (hl != null)
                {
                    hl.ConstantOffImmediate();
                    Destroy(hl);
                }
            }
            highlighters.Clear();
        }

        public void DrawOutline(Bounds b)
        {
            Vector3[] pts = new Vector3[8];
            Camera cam = Camera.main;
            float margin = 0;

            pts[0] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[1] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[2] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[3] = cam.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
            pts[4] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
            pts[5] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
            pts[6] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
            pts[7] = cam.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));

            for (int i = 0; i < pts.Length; i++) pts[i].y = Screen.height - pts[i].y;

            Vector3 min = pts[0];
            Vector3 max = pts[0];
            for (int i = 1; i < pts.Length; i++)
            {
                min = Vector3.Min(min, pts[i]);
                max = Vector3.Max(max, pts[i]);
            }

            Rect r = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
            r.xMin -= margin;
            r.xMax += margin;
            r.yMin -= margin;
            r.yMax += margin;

            GL.PushMatrix();
            GL.Begin(1);
            mat.SetPass(0);
            GL.Color(Color.red);

            GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y + (r.size.y / 2), 0);
            GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y + (r.size.y / 2), 0);
            GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y + (r.size.y / 2), 0);
            GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
            GL.Vertex3(r.center.x + (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
            GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
            GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y - (r.size.y / 2), 0);
            GL.Vertex3(r.center.x - (r.size.x / 2), r.center.y + (r.size.y / 2), 0);

            GL.End();
            GL.PopMatrix();
        }
    }
}
