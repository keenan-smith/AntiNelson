using System;
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
        private static int assetsLoadCount;
		private static int assetsScanCount;
		private static float lastLoading;
		private static Local localization;
        private static Local _local_Tips;
		private static ELoadingTip tip;
		private static readonly byte TIP_COUNT = 31;
		private static bool _isInitialized;

        private GUISkin _skin;
        private Color _windowColor;
        private Texture _backgroundImage;
        private Rect _backgroundImage_Rect;
        private Texture _logoImage;
        private Rect _logoImage_Rect;
        private Rect _currentMusic_Window;
        private Rect _currentMusic_Text;
        private Rect _tip_Window;
        private Rect _tip_Text;
        private Rect _map_Window;
        private Rect _map_Text;
        private Rect _currentlyLoading_Window;
        private Rect _currentlyLoading_Text;
        #endregion

        #region Properties
        public static bool isBlocked
		{
            //[CodeReplace("get_isBlocked", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
			get
			{
				return Time.realtimeSinceStartup - lastLoading < 0.1f;
			}
		}

		public static bool isInitialized
		{
            //[CodeReplace("get_isInitialized", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
			get
			{
				return _isInitialized;
			}
		}

		public static GameObject loader
		{
            //[CodeReplace("get_loader", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
			get;
            //[CodeReplace("set_loader", typeof(LoadingUI), BindingFlags.NonPublic | BindingFlags.Static)]
			private set;
		}
        #endregion

        #region Functions
        //[CodeReplace("assetsLoad", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
        public static void assetsLoad(string key, int count, float progress, float step)
		{
		}

        //[CodeReplace("assetsScan", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void assetsScan(string key, int count)
		{
		}

        public void Awake()
		{
            if (isInitialized)
            {
                GameObject.Destroy(loader);
                return;
            }
            _isInitialized = true;
            DontDestroyOnLoad(loader);
		}

        public void OnDestroy()
		{
		}

        public void OnGUI()
		{
            if (_backgroundImage == null || _logoImage == null || _skin == null)
                return;

            string tip_text = "";

            #region Tip Text
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

            if (GUI.skin != _skin)
                GUI.skin = _skin;

            GUI.DrawTexture(_backgroundImage_Rect, _backgroundImage, ScaleMode.ScaleToFit);
            GUI.DrawTexture(_logoImage_Rect, _logoImage, ScaleMode.ScaleToFit);

            GUI.Box(_currentMusic_Window, "");
            GUI.Box(_currentlyLoading_Window, "");
            GUI.Box(_tip_Window, "");
            GUI.Box(_map_Window, "");

            Debug.Log(tip_text);
            GUI.Label(_tip_Text, tip_text);
		}

        public void Update()
        {
            if (Variables.bundle == null)
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
		}

        //[CodeReplace("rebuild", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void rebuild()
		{
		}

		public void Start()
		{
            _local_Tips = Localization.read("/Menu/MenuTips.dat");
            localization = Localization.read("/Menu/MenuLoading.dat");
            loader = gameObject;

            _windowColor = Color.white;
            _windowColor.a = 0.7f;

            _backgroundImage_Rect = new Rect(0f, 0f, Screen.width, Screen.height);
            _logoImage_Rect = new Rect((Screen.width / 2f) - 150f, (Screen.height / 2f) - 150f, 300f, 300f);
            _currentMusic_Window = new Rect(1f, Screen.height - 81f, 500f, 80f);
            _currentlyLoading_Window = new Rect(_logoImage_Rect.x - 100f, _logoImage_Rect.y + _logoImage_Rect.height + 15f, _logoImage_Rect.width + 200f, 50f);
            _tip_Window = new Rect(_logoImage_Rect.x - 150f, _logoImage_Rect.y - 50f - 15f, _logoImage_Rect.width + 300f, 50f);
            _map_Window = new Rect(1f, 1f, 300f, 80f);

            _tip_Text = new Rect(_tip_Window.x + 5f, _tip_Window.y - (_tip_Window.height / 2f), _tip_Window.width - 10f, _tip_Window.height - _tip_Window.y);

            Provider.onQueuePositionUpdated += new Provider.QueuePositionUpdated(onQueuePositionUpdated);

            GameObject.Destroy((UnityEngine.Object.FindObjectOfType(typeof(LoadingUI)) as LoadingUI).gameObject);
		}

        //[CodeReplace("updateKey", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void updateKey(string key)
		{
		}

        //[CodeReplace("updateProgress", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void updateProgress(float progress)
		{
		}

        //[CodeReplace("updateScene", typeof(LoadingUI), BindingFlags.Public | BindingFlags.Static)]
		public static void updateScene()
		{
            byte tip_byte;

            updateProgress(0f);

            do
            {
                tip_byte = (byte)UnityEngine.Random.Range(1, (int)(TIP_COUNT + 1));
            } while (tip_byte == (byte)tip);

            OV_LoadingUI.tip = (ELoadingTip)tip_byte;
		}
        #endregion
    }
}
