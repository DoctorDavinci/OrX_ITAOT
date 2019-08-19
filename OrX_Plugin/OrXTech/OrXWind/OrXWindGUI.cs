using KSP.UI.Screens;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OrX.wind
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXWindGUI : MonoBehaviour
    {
        private const float WindowWidth = 220;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXWindGUI instance;
        public static bool OrXWindGUIEnabled;
        public bool HasAddedButton = false;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public bool guiActive;
        public bool guiOpen = false;

        /// /////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            if (instance)
            {
                Destroy(instance);
            }
            instance = this;
        }

        private void Start()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), 250, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(DisableGui);
            GameEvents.onShowUI.Add(EnableGui);

            if (OrXLog.instance.unlockedWind)
            {
                AddToolbarButton();
            }
            else
            {
                OrXWindGUIEnabled = false;
            }
        }

        private void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (OrXWindGUIEnabled && OrXLog.instance.unlockedWind)
                {
                    _windowRect = GUI.Window(693427236, _windowRect, GuiWindow, "");
                }
            }
        }

        private void Update()
        {
        }

        // FROM SPANNER
        //
        // already got a list of  wants,  wind direction speed indicator,  selectable wind options,
        // direction variability value  as it tends to blow generally the same way for a few hours.   
        // selectable initial wind direction  to avoid ending up on the beach before yopu even get going (lee shores are bastards)  ,
        // and  oof course wind strength, because while it takes max force to move that ship  at 12ms   , 
        // the same wind will make an aircraft  a nightmare to fly .   Tested  PAI  ,  it couldnt cope with a prop aircraft may have better luck  with a jet


        /// /////////////////////////////////////////////////////////////////////////////

        #region Wind GUI

        private void GuiWindow(int Wind)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            DrawEnableWind(line);
            line++;
            DrawIntensity(line);
            line++;
            DrawWindIntensity(line);
            line++;
            DrawVariability(line);
            line++;
            DrawWindVariability(line);
            line++;
            DrawVariationIntensity(line);
            line++;
            DrawVIntensity(line);
            line++;
            DrawBlowNTeaseText(line);
            line++;
            DrawBlowNTeaseTimer(line);
            if (!OrXWeatherSim.instance.random360)
            {
                line++;
                line++;
                DrawDegrees(line);
                line++;
                line++;
                DrawWindSetDirection(line);
            }
            line++;
            Draw360Random(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        public void AddToolbarButton()
        {
            string textureDir = "OrX/Plugin/";

            if (!HasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(textureDir + "Wind_normal", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(EnableGui, DisableGui, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT, buttonTexture);
                HasAddedButton = true;
            }
        }

        public void EnableGui()
        {
            OrXWeatherSim.instance._degrees = "0";
            OrXWindGUIEnabled = true;
            guiOpen = true;
            Debug.Log("[Wind]: Showing GUI");
        }

        public void DisableGui()
        {
            OrXWeatherSim.instance._degrees = "0";
            guiOpen = false;
            OrXWindGUIEnabled = false;
            Debug.Log("[Wind]: Hiding GUI");
        }

        private void DrawTitle(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Wind", titleStyle);
        }

        private void DrawIntensity(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Wind Intensity", titleStyle);
        }

        private void DrawWindIntensity(float line)
        {
            var Label = new GUIStyle
            {
                normal = { textColor = Color.white }
            };

            var Style = new GUIStyle(Label)
            {
                fontSize = 12,
            };

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0", Style);
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|", Style);
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100", Style);
            OrXWeatherSim.instance.windIntensity = GUI.HorizontalSlider(saveRect, OrXWeatherSim.instance.windIntensity, 1, 100);
        }

        private void DrawVariability(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Wind Variability", titleStyle);
        }

        private void DrawWindVariability(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0");
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|");
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100");
            OrXWeatherSim.instance.windVariability = GUI.HorizontalSlider(saveRect, OrXWeatherSim.instance.windVariability, 1, 100);
        }

        private void DrawVariationIntensity(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Variation Intesity", titleStyle);
        }

        private void DrawVIntensity(float line)
        {
            var Label = new GUIStyle
            {
                normal = { textColor = Color.white }
            };

            var Style = new GUIStyle(Label)
            {
                fontSize = 12,
            };

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0", Style);
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|", Style);
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100", Style);
            OrXWeatherSim.instance.variationIntensity = GUI.HorizontalSlider(saveRect, OrXWeatherSim.instance.variationIntensity, 1, 100);
        }

        private void Draw360Random(float line)
        {
            GUIStyle style = OrXWeatherSim.instance.random360 ? HighLogic.Skin.box : HighLogic.Skin.button;

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!OrXWeatherSim.instance.random360)
            {
                if (GUI.Button(saveRect, "Enable 360 Random", style))
                {
                    OrXWeatherSim.instance.random360 = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Disable 360 Random", style))
                {
                    OrXWeatherSim.instance.random360 = false;
                }
            }
        }

        private void DrawBlowNTeaseText(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Blow n Tease Timer", titleStyle);
        }

        private void DrawBlowNTeaseTimer(float line)
        {
            var Label = new GUIStyle
            {
                normal = { textColor = Color.white }
            };

            var Style = new GUIStyle(Label)
            {
                fontSize = 12,
            };

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0", Style);
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|", Style);
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100", Style);
            OrXWeatherSim.instance.teaseDelay = GUI.HorizontalSlider(saveRect, OrXWeatherSim.instance.teaseDelay, 1, 100);
        }

        private void DrawEnableWind(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!OrXWeatherSim.instance.enableWind)
            {
                if (GUI.Button(saveRect, "Enable Wind"))
                {
                    OrXWeatherSim.instance.ToggleWind();
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Disable Wind"))
                {
                    OrXWeatherSim.instance.ToggleWind();
                }
            }
        }

        private void DrawDegrees(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Heading",
                leftLabel);
            float textFieldWidth = 80;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            OrXWeatherSim.instance._degrees = GUI.TextField(fwdFieldRect, OrXWeatherSim.instance._degrees);
        }

        private void DrawWindSetDirection(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "Set Direction"))
            {
                OrXWeatherSim.instance.enableWind = false;
                OrXWeatherSim.instance.setDirection = true;
                OrXWeatherSim.instance.manual = true;
                OrXWeatherSim.instance.enableWind = true;
            }
        }

        #endregion

        /// /////////////////////////////////////////////////////////////////////////////

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 5, ScreenMessageStyle.UPPER_CENTER));
        }

        private void Dummy() { }


    }
}