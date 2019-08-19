using System;
using UnityEngine;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXDC : MonoBehaviour
    {
        private const float WindowWidth = 100;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXDC instance;
        public bool GuiEnabledOrXDC = false;
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
            if (instance) Destroy(instance);
            instance = this;
        }
        
        private void Start()
        {
            _windowRect = new Rect((Screen.width / 4) * 2 - (WindowWidth * 3) + 10, 50, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisableOrXDC);
            GameEvents.onShowUI.Add(GameUiEnableOrXDC);
            _gameUiToggle = true;
            distance = 0;
        }

        private void OnGUI()
        {
            if (GuiEnabledOrXDC && _gameUiToggle)
            {
                _windowRect = GUI.Window(416937212, _windowRect, GuiWindowOrXDC, "");
            }
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && OrXHoloCache.instance.checking)
            {
                if (!GuiEnabledOrXDC)
                {
                    GuiEnabledOrXDC = true;
                }
            }
            else
            {
                GuiEnabledOrXDC = false;
            }
        }


        #region GUI
        /// <summary>
        /// GUI
        /// </summary>

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }

        private void GuiWindowOrXDC(int OrXDC)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            ShowDistance(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        public void ToggleGUI()
        {
            if (GuiEnabledOrXDC)
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
            GuiEnabledOrXDC = true;
            Debug.Log("[OrX]: Showing OrXDC GUI");
        }

        public void DisableGui()
        {
            GuiEnabledOrXDC = false;
            Debug.Log("[OrX]: Hiding OrXDC GUI");
        }

        private void GameUiEnableOrXDC()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisableOrXDC()
        {
            _gameUiToggle = false;
        }

        private void ShowDistance(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                "" + distance,
                titleStyle);
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
                "Target Distance",
                titleStyle);
        }

        #endregion

        private void Dummy()
        {
        }
    }
}