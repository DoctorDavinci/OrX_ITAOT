using System;
using UnityEngine;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXMissions : MonoBehaviour
    {
        private const float WindowWidth = 100;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXMissions instance;
        public bool GuiEnabledOrXMissions = false;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public double distance = 0;

        private bool scubaKerb = false;
        private bool tractorBeam = false;
        private bool jetPack = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        
        private void Start()
        {
            _windowRect = new Rect((Screen.width / 4) * 2 - (WindowWidth * 3) + 10, 50, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisableOrXMissions);
            GameEvents.onShowUI.Add(GameUiEnableOrXMissions);
            _gameUiToggle = true;
            distance = 0;
        }

        private void OnGUI()
        {
            if (GuiEnabledOrXMissions && _gameUiToggle)
            {
                _windowRect = GUI.Window(492127212, _windowRect, GuiWindowOrXMissions, "");
            }
        }

        public void Update()
        {
        }


        #region GUI
        /// <summary>
        /// GUI
        /// </summary>

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }

        private void GuiWindowOrXMissions(int OrXMissions)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            if (!scubaKerb)
            {
                DrawScubaKerb(line);
                line++;
            }
            if (!tractorBeam)
            {
                DrawTractorBeam(line);
                line++;
            }

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        public void ToggleGUI()
        {
            if (GuiEnabledOrXMissions)
            {
                DisableGui();
            }
            else
            {
                EnableGui();
            }
        }

        public void EnableGui()
        {
            GuiEnabledOrXMissions = true;
            Debug.Log("[OrX]: Showing OrXMissions GUI");
        }

        public void DisableGui()
        {
            GuiEnabledOrXMissions = false;
            Debug.Log("[OrX]: Hiding OrXMissions GUI");
        }

        private void GameUiEnableOrXMissions()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisableOrXMissions()
        {
            _gameUiToggle = false;
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
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20),
                "",
                titleStyle);
        }

        private void DrawScubaKerb(float line)
        {
            GUIStyle style = scubaKerb ? HighLogic.Skin.box : HighLogic.Skin.button;

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!scubaKerb)
            {
                if (GUI.Button(saveRect, "Unlock Scuba Kerb", style))
                {
                    DisableGui();
                    OrXScubaKerbMissions.instance.EnableGui();
                }
            }
            else
            {
            }
        }

        private void DrawJetpack(float line)
        {
            GUIStyle style = jetPack ? HighLogic.Skin.box : HighLogic.Skin.button;

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!jetPack)
            {
                if (GUI.Button(saveRect, "Unlock Jet Pack", style))
                {
                    //DisableGui();
                    //OrXScubaKerbMissions.instance.EnableGui();
                }
            }
            else
            {
            }
        }

        private void DrawTractorBeam(float line)
        {
            GUIStyle style = tractorBeam ? HighLogic.Skin.box : HighLogic.Skin.button;

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!tractorBeam)
            {
                if (GUI.Button(saveRect, "Unlock Tractor Beam", style))
                {
                    //DisableGui();
                    //OrXScubaKerbMissions.instance.EnableGui();
                }
            }
            else
            {
            }
        }


        #endregion

        private void Dummy()
        {
        }
    }
}