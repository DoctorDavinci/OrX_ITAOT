using System;
using UnityEngine;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXScubaKerbMissions : MonoBehaviour
    {
        private const float WindowWidth = 100;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXScubaKerbMissions instance;
        public bool GuiEnabledOrXScubaKerbMissions = false;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public double distance = 0;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        
        private void Start()
        {
            _windowRect = new Rect((Screen.width / 4) * 2 - (WindowWidth * 3) + 10, 50, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisableOrXScubaKerbMissions);
            GameEvents.onShowUI.Add(GameUiEnableOrXScubaKerbMissions);
            _gameUiToggle = true;
            distance = 0;
        }

        private void OnGUI()
        {
            if (GuiEnabledOrXScubaKerbMissions && _gameUiToggle)
            {
                _windowRect = GUI.Window(490317212, _windowRect, GuiWindowOrXScubaKerbMissions, "");
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

        private bool h1 = true;

        private void GuiWindowOrXScubaKerbMissions(int OrXScubaKerbMissions)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;

            DrawText(line);
            line++;

            DrawStart(line);
            line++;
            DrawCancel(line);
            line++;

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        public void ToggleGUI()
        {
            if (GuiEnabledOrXScubaKerbMissions)
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
            GuiEnabledOrXScubaKerbMissions = true;
            Debug.Log("[OrX]: Showing OrXScubaKerbMissions GUI");
        }

        public void DisableGui()
        {
            GuiEnabledOrXScubaKerbMissions = false;
            Debug.Log("[OrX]: Hiding OrXScubaKerbMissions GUI");
        }

        private void GameUiEnableOrXScubaKerbMissions()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisableOrXScubaKerbMissions()
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
                "Scuba Kerb",
                titleStyle);
        }


        private void DrawText(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), 
                "TEXT",
                leftLabel);
        }




















        private void DrawStart(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "Unlock Scuba Kerb", HighLogic.Skin.button))
            {
                DisableGui();
                OrXScubaKerbMissions.instance.EnableGui();
            }
        }

        private void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "Cancel", HighLogic.Skin.button))
            {
                DisableGui();
                OrXMissions.instance.EnableGui();
            }
        }

        #endregion

        private void Dummy()
        {
        }
    }
}