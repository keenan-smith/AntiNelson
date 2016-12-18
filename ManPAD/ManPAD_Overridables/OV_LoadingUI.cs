using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using ManPAD.ManPAD_API.Attributes;
using ManPAD.ManPAD_API;
using ManPAD.ManPAD_API.GUI.Enumerables;

namespace ManPAD.ManPAD_Overridables
{
    public class OV_LoadingUI : MonoBehaviour
    {
        #region Variables
		private static float lastLoading;
		private static Local localization;
        private static Local _local_Tips;
		private static ELoadingTip tip;
		private static readonly byte TIP_COUNT = 31;
		private static bool _isInitialized;
        private static string tip_text;
        private static List<string> loading_texts = new List<string>();
        private static int assetsLoadCount;
        private static int assetsScanCount;
        private static GameObject _loader;
        private static bool _loaded = false;
        private static string nowPlaying = "PLEASE INSTALL MANPADSONGS";
        private static AudioSource sourceSong;

        private GUISkin _skin;
        private Color _windowColor;
        private Texture _backgroundImage;
        private Rect _backgroundImage_Rect;
        private Texture _logoImage;
        private Rect _logoImage_Rect;
        private float _logoImage_angle = 0f;
        private Rect _currentMusic_Text;
        private Rect _tip_Text;
        private Rect _map_Text;
        private Rect _currentlyLoading_Text;
        private Rect _cancel_Button;
        private int song = 0;
        #endregion

        #region Properties
        public static bool isBlocked
		{
            [CodeReplace("get_isBlocked", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
			get
			{
				return Time.realtimeSinceStartup - lastLoading < 0.1f;
			}
		}

		public static bool isInitialized
		{
            [CodeReplace("get_isInitialized", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
			get
			{
				return _isInitialized;
			}
		}

	    public static GameObject loader
		{
            [CodeReplace("get_loader", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
            get
            {
                return _loader;
            }
            [CodeReplace("set_loader", typeof(LoadingUI), BindingFlags.NonPublic | BindingFlags.Static)]
            private set
            {
                _loader = value;
            }
		}
        #endregion

        #region Functions
        private static void addText(string text)
        {
            loading_texts.Add(text);

            if (loading_texts.Count > 10)
                loading_texts.Remove(loading_texts[0]);
        }

        [CodeReplace("assetsLoad", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
        public static void assetsLoad(string key, int count, float progress, float step)
		{
            assetsLoadCount = assetsScanCount - count;
            addText(localization.format("Assets_Load", new object[]{
                localization.format(key),
                assetsLoadCount,
                assetsScanCount
            }));
		}

        [CodeReplace("assetsScan", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void assetsScan(string key, int count)
		{
            assetsScanCount = count;
            addText(localization.format("Assets_Scan", new object[]{
                localization.format(key),
                assetsScanCount
            }));
		}

        public void Awake()
		{
            if (isInitialized)
            {
                GameObject.Destroy(gameObject);
                return;
            }
            _isInitialized = true;
            DontDestroyOnLoad(gameObject);
		}

        public void OnGUI()
		{
            if (_backgroundImage == null || _logoImage == null || _skin == null || string.IsNullOrEmpty(tip_text) || !(Level.info != null ? (!_loaded || isBlocked) : isBlocked))
                return;

            _logoImage_angle += 0.2f;

            if (GUI.skin != _skin)
                GUI.skin = _skin;

            GUI.DrawTexture(_backgroundImage_Rect, _backgroundImage, ScaleMode.ScaleToFit);
            Matrix4x4 matrixBackup = GUI.matrix;
            GUIUtility.RotateAroundPivot(_logoImage_angle, new Vector2(Screen.width / 2f, Screen.height / 2f));
            GUI.DrawTexture(_logoImage_Rect, _logoImage, ScaleMode.ScaleToFit);
            GUI.matrix = matrixBackup;

            GUIStyle label_Style = new GUIStyle(GUI.skin.label);

            label_Style.alignment = TextAnchor.UpperLeft;
            label_Style.fontSize = 15;
            label_Style.fontStyle = FontStyle.Bold;
            label_Style.normal.textColor = Color.white;

            if (Level.info != null && Provider.isConnected && !Provider.isServer)
                GUI.Label(_map_Text, "Map: " + Level.info.name +
                    "\nVAC: " + (Provider.currentServerInfo.IsVACSecure ? localization.format("VAC_Secure") : localization.format("VAC_Insecure")) +
                    "\nBattleye: " + (Provider.currentServerInfo.IsBattlEyeSecure ? localization.format("BattlEye_Secure") : localization.format("BattlEye_Insecure")) + 
                    "\nPro: " + (Provider.currentServerInfo.isPro ? "Yes" : "No") + 
                    "\nPVP: " + (Provider.currentServerInfo.isPvP ? "Yes" : "No") +
                    "\nWorkshop: " + (Provider.currentServerInfo.isWorkshop ? "Yes" : "No") +
                    "\nQueue: " + (Provider.queuePosition + 1), label_Style);

            /*label_Style.alignment = TextAnchor.MiddleLeft;

            GUI.Label(_currentMusic_Text, "Playing: " + nowPlaying, label_Style);*/

            label_Style.alignment = TextAnchor.MiddleCenter;

            GUI.Label(_tip_Text, tip_text, label_Style);
            if (loading_texts.Count > 0)
            {
                GUI.Label(_currentlyLoading_Text, loading_texts[loading_texts.Count - 1], label_Style);
                for (int i = loading_texts.Count - 2; i >= 0; i--)
                {
                    Rect rct = new Rect(_currentlyLoading_Text.x, _currentlyLoading_Text.y + (float)(((loading_texts.Count - 1) - i) * 20f), _currentlyLoading_Text.width, _currentlyLoading_Text.height);
                    Color col = Color.white;

                    col.a -= (float)(((loading_texts.Count - 2) - i) / 4f);
                    label_Style.normal.textColor = col;

                    GUI.Label(rct, loading_texts[i], label_Style);
                }
            }
		}

        public void Update()
        {
            if (Assets.isLoading || Provider.isLoading || Level.isLoading || Player.isLoading)
            {
                lastLoading = Time.realtimeSinceStartup;
            }

            if (Variables.bundle == null || !(Level.info != null ? (!_loaded || isBlocked) : isBlocked))
                return;

            EThemes _theme = MP_Config.instance.getTheme();

            if(_backgroundImage == null)
                _backgroundImage = Variables.bundle.LoadAsset("manpad.png") as Texture;
            if(_logoImage == null)
                _logoImage = Variables.bundle.LoadAsset("ManpadLogo.png") as Texture;
            if (_theme == EThemes.WHITE)
                _skin = Variables.bundle.LoadAsset("White.guiskin") as GUISkin;
            else if (_theme == EThemes.INVERTED)
                _skin = Variables.bundle.LoadAsset("Inverted.guiskin") as GUISkin;
            else if (_theme == EThemes.AQUA)
                _skin = Variables.bundle.LoadAsset("Aqua.guiskin") as GUISkin;
            else if (_theme == EThemes.MAGIC)
                _skin = Variables.bundle.LoadAsset("Magic.guiskin") as GUISkin;
        }

        public static void onQueuePositionUpdated()
		{
            updateKey(localization.format("Queue_Position", new object[] {
                (int)(Provider.queuePosition + 1)
            }));
		}

        [CodeReplace("rebuild", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void rebuild()
		{
            // DO FUCKING NOTHING THIS SHIT IS FOR NELSON'S SHIT GUI! IF YOU WANT PUT WHATEVER THE FUCK YOU WANT HERE
		}

		public void Start()
		{
            _local_Tips = Localization.read("/Menu/MenuTips.dat");
            localization = Localization.read("/Menu/MenuLoading.dat");
            _loader = Variables.LoadingUI_gameobject;

            _windowColor = Color.white;
            _windowColor.a = 0.7f;

            _backgroundImage_Rect = new Rect(0f, 0f, Screen.width, Screen.height);
            _logoImage_Rect = new Rect((Screen.width / 2f) - 150f, (Screen.height / 2f) - 150f, 300f, 300f);

            _tip_Text = new Rect(1f, _logoImage_Rect.y - 50f - 15f, Screen.width - 2f, 50f);
            _currentMusic_Text = new Rect(1f, Screen.height - 40f, Screen.width - 2f, 39f);
            _currentlyLoading_Text = new Rect(1f, _logoImage_Rect.y + _logoImage_Rect.height + 15f, Screen.width - 2f, 40f);
            _map_Text = new Rect(1f, 1f, Screen.width - 2f, Screen.height - 2f);

            Provider.onQueuePositionUpdated += new Provider.QueuePositionUpdated(onQueuePositionUpdated);

            GameObject.Destroy((UnityEngine.Object.FindObjectOfType(typeof(LoadingUI)) as LoadingUI));
            updateScene();
            /*if (ReadWrite.folderExists(Directory.GetCurrentDirectory() + "/ManPADSongs", false))
                StartCoroutine(songCoroutine());*/
		}

        [CodeReplace("updateKey", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void updateKey(string key)
		{
            addText(localization.format(key));
		}

        [CodeReplace("updateProgress", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void updateProgress(float progress)
		{
            if (progress == 1f)
            {
                _loaded = true;
                Variables.LoadingUI_Script._logoImage_angle = 0f;
            }
		}

        [CodeReplace("updateScene", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void updateScene()
		{
            byte tip_byte;

            updateProgress(0f);
            loading_texts.Clear();

            #region Tips
            do
            {
                tip_byte = (byte)UnityEngine.Random.Range(1, (int)(TIP_COUNT + 1));
            } while (tip_byte == (byte)tip);

            tip = (ELoadingTip)tip_byte;

            switch (tip)
            {
                case ELoadingTip.HOTKEY:
                    tip_text = _local_Tips.format("Hotkey");
                    break;
                case ELoadingTip.EQUIP:
                    tip_text = _local_Tips.format("Equip", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				    });
                    break;
                case ELoadingTip.DROP:
                    tip_text = _local_Tips.format("Drop", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				    });
                    break;
                case ELoadingTip.SIRENS:
                    tip_text = _local_Tips.format("Sirens", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				    });
                    break;
                case ELoadingTip.TRANSFORM:
                    tip_text = _local_Tips.format("Transform");
                    break;
                case ELoadingTip.QUALITY:
                    tip_text = _local_Tips.format("Quality");
                    break;
                case ELoadingTip.UMBRELLA:
                    tip_text = _local_Tips.format("Umbrella");
                    break;
                case ELoadingTip.HEAL:
                    tip_text = _local_Tips.format("Heal");
                    break;
                case ELoadingTip.ROTATE:
                    tip_text = _local_Tips.format("Rotate");
                    break;
                case ELoadingTip.BASE:
                    tip_text = _local_Tips.format("Base");
                    break;
                case ELoadingTip.DEQUIP:
                    tip_text = _local_Tips.format("Dequip", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.dequip)
				    });
                    break;
                case ELoadingTip.NIGHTVISION:
                    tip_text = _local_Tips.format("Nightvision", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.vision)
				    });
                    break;
                case ELoadingTip.TRANSFER:
                    tip_text = _local_Tips.format("Transfer", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				    });
                    break;
                case ELoadingTip.SURFACE:
                    tip_text = _local_Tips.format("Surface", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.jump)
				    });
                    break;
                case ELoadingTip.ARREST:
                    tip_text = _local_Tips.format("Arrest", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.leanLeft),
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.leanRight)
				    });
                    break;
                case ELoadingTip.SAFEZONE:
                    tip_text = _local_Tips.format("Safezone");
                    break;
                case ELoadingTip.CLAIM:
                    tip_text = _local_Tips.format("Claim");
                    break;
                case ELoadingTip.GROUP:
                    tip_text = _local_Tips.format("Group");
                    break;
                case ELoadingTip.MAP:
                    tip_text = _local_Tips.format("Map", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.map)
				    });
                    break;
                case ELoadingTip.BEACON:
                    tip_text = _local_Tips.format("Beacon");
                    break;
                case ELoadingTip.HORN:
                    tip_text = _local_Tips.format("Horn", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary)
				    });
                    break;
                case ELoadingTip.LIGHTS:
                    tip_text = _local_Tips.format("Lights", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary)
				    });
                    break;
                case ELoadingTip.SNAP:
                    tip_text = _local_Tips.format("Snap", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.snap)
				    });
                    break;
                case ELoadingTip.UPGRADE:
                    tip_text = _local_Tips.format("Upgrade", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				    });
                    break;
                case ELoadingTip.GRAB:
                    tip_text = _local_Tips.format("Grab", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				    });
                    break;
                case ELoadingTip.SKYCRANE:
                    tip_text = _local_Tips.format("Skycrane", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other)
				    });
                    break;
                case ELoadingTip.SEAT:
                    tip_text = _local_Tips.format("Seat");
                    break;
                case ELoadingTip.RARITY:
                    tip_text = _local_Tips.format("Rarity");
                    break;
                case ELoadingTip.ORIENTATION:
                    tip_text = _local_Tips.format("Orientation", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.rotate)
				    });
                    break;
                case ELoadingTip.RED:
                    tip_text = _local_Tips.format("Red");
                    break;
                case ELoadingTip.STEADY:
                    tip_text = _local_Tips.format("Steady", new object[]
				    {
					    MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.sprint)
				    });
                    break;
                default:
                    tip_text = "#" + tip.ToString();
                    break;
            }
            #endregion

            _loaded = false;
        }
        #endregion

        #region Coroutines
        private IEnumerator songCoroutine()
        {
            while (true)
            {
                if (sourceSong != null && !(Level.info != null ? (!_loaded || isBlocked) : isBlocked))
                {
                    sourceSong.Stop();
                    GameObject.Destroy(sourceSong);
                    sourceSong = null;
                }
                if (sourceSong != null || !(Level.info != null ? (!_loaded || isBlocked) : isBlocked))
                {
                    yield return null;
                    continue;
                }

                int song = UnityEngine.Random.Range(0, Variables.songs.Count - 1);
                nowPlaying = Variables.songs.Keys.ToArray()[song];
                WWW req = new WWW("file://" + Directory.GetCurrentDirectory() + @"\ManPADSongs\" + Variables.songs.Values.ToArray()[song]);

                yield return req;

                AudioClip clip = req.GetAudioClip(false, false);
                sourceSong = gameObject.AddComponent<AudioSource>();

                sourceSong.volume = 0.10f;
                sourceSong.PlayOneShot(clip);
            }
        }
        #endregion
    }
}
