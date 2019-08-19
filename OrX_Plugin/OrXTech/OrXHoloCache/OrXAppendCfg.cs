using System;
using UnityEngine;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXAppendCfg : MonoBehaviour
    {
        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXAppendCfg instance;
        public bool GuiEnabledOrXAppendCfg = false;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public double distance = 0;

        public string hcName = "";
        public bool append = false;
        public bool save = false;
        public bool cancel = false;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }
        
        private void Start()
        {
            _windowRect = new Rect((Screen.width / 4) * 2 - (WindowWidth * 3) + 10, 50, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisableOrXAppendCfg);
            GameEvents.onShowUI.Add(GameUiEnableOrXAppendCfg);
            _gameUiToggle = true;
            distance = 0;
        }

        private void OnGUI()
        {
            if (GuiEnabledOrXAppendCfg && _gameUiToggle)
            {
                _windowRect = GUI.Window(416937212, _windowRect, GuiWindowOrXAppendCfg, "");
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

        public void EnableGui()
        {
            save = false;
            append = false;
            GuiEnabledOrXAppendCfg = true;
            Debug.Log("[OrX]: Showing OrXAppendCfg GUI");
        }

        public void DisableGui()
        {
            hcName = "";
            cancel = false;
            save = false;
            append = false;
            GuiEnabledOrXAppendCfg = false;
            Debug.Log("[OrX]: Hiding OrXAppendCfg GUI");
        }

        private void GameUiEnableOrXAppendCfg()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisableOrXAppendCfg()
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
                hcName + " already exists .....",
                titleStyle);
        }

        private void DrawTitle2(float line)
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
                hcName + "What would you like to do?",
                titleStyle);
        }

        private void DrawTitle4(float line)
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
                "Rename your HoloCache below",
                titleStyle);
        }

        private void DrawAppend(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "Add to " + hcName, HighLogic.Skin.button))
            {
                append = true;
            }
        }

        private void GuiWindowOrXAppendCfg(int OrXAppendCfg)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            DrawTitle2(line);
            line++;
            DrawAppend(line);
            line++;
            DrawTitle4(line);
            line++;
            DrawHoloCacheName(line);
            line++;
            DrawSave(line);
            line++;
            DrawCancel(line);
            line++;

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
            {
                if (hcName == "")
                {
                    ScreenMsg("Unable to save HoloCache with no name");
                }
                else
                {
                    save = true;
                }
            }
        }

        private void DrawHoloCacheName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Holo Name: ",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            hcName = GUI.TextField(fwdFieldRect, hcName);
        }

        private void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "CANCEL", HighLogic.Skin.button))
            {
                cancel = true;
            }
        }

        #endregion

        private void Dummy()
        {
        }
    }
}