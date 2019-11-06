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

        public string HoloKronName = "";
        public string _HoloKronName = "";

        public bool append = false;
        public bool save = false;
        public bool cancel = false;
        public int hkCount = 0;
        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }
        
        private void Start()
        {
            _windowRect = new Rect((Screen.width / 4) * 2 - (WindowWidth * 3) + 10, 50, WindowWidth, _windowHeight);
            //GameEvents.onHideUI.Add(GameUiDisableOrXAppendCfg);
            //GameEvents.onShowUI.Add(GameUiEnableOrXAppendCfg);
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

        private void GuiWindowOrXAppendCfg(int OrXAppendCfg)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            DrawTitle2(line);
            line++;
            OrXHoloKron.instance.DrawPlayPassword(line);
            line++;
            DrawAppend(line);
            line++;
            DrawTitle4(line);
            line++;
            DrawHoloKronName(line);
            line++;
            DrawSave(line);
            line++;
            DrawCancel(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        public void EnableGui(int _hkCount, string holoName)
        {
            hkCount = _hkCount;
            _HoloKronName = holoName;
            OrXHoloKron.instance.OrXHCGUIEnabled = false;
            save = false;
            append = false;
            GuiEnabledOrXAppendCfg = true;
            OrXLog.instance.DebugLog("[OrX]: Showing OrXAppendCfg GUI");
        }

        public void DisableGui()
        {
            OrXHoloKron.instance.OrXHCGUIEnabled = true;
            HoloKronName = "";
            cancel = false;
            save = false;
            append = false;
            GuiEnabledOrXAppendCfg = false;
            OrXLog.instance.DebugLog("[OrX]: Hiding OrXAppendCfg GUI");
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, entryHeight),
                _HoloKronName + " contains " + hkCount + " HoloKrons",
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, entryHeight),
                "What would you like to do?",
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, entryHeight),
                "Rename your HoloKron below",
                titleStyle);
        }

        private void DrawAppend(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "Add to " + _HoloKronName, HighLogic.Skin.button))
            {
                if (OrXHoloKron.instance.Password == OrXHoloKron.instance.pas)
                {
                    if (OrXHoloKron.instance.spawningStartGate)
                    {
                        OrXHoloKron.instance.hkCount = hkCount;
                        spawn.OrXVesselMove.Instance.StartMove(OrXHoloKron.instance._HoloKron, false, 0, false);
                    }
                    else
                    {
                        OrXHoloKron.instance.hkCount = hkCount;
                        OrXHoloKron.instance.SaveConfig(_HoloKronName, true);
                        DisableGui();
                    }

                    OrXHoloKron.instance.hkCount = hkCount;
                }
                else
                {
                    OrXHoloKron.instance.ScreenMsg("WRONG PASSWORD");
                }
            }
        }

        private void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
            {
                if (HoloKronName == "")
                {
                    OrXHoloKron.instance.ScreenMsg("Unable to create HoloKron with no name");
                }
                else
                {
                    if (OrXHoloKron.instance.CheckExports(HoloKronName))
                    {
                        OrXHoloKron.instance.ScreenMsg(HoloKronName + " also exists .....");
                        OrXHoloKron.instance.ScreenMsg("What would you like to do?");

                        _HoloKronName = HoloKronName;
                    }
                    else
                    {

                        if (OrXHoloKron.instance.spawningStartGate)
                        {
                            OrXHoloKron.instance.hkCount = 0;
                            spawn.OrXVesselMove.Instance.StartMove(OrXHoloKron.instance._HoloKron, false, 0, false);
                        }
                        else
                        {
                            OrXHoloKron.instance.hkCount = 0;
                            OrXHoloKron.instance.SaveConfig(HoloKronName, false);
                            DisableGui();
                        }
                    }
                }
            }
        }

        private void DrawHoloKronName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Name: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            HoloKronName = GUI.TextField(fwdFieldRect, HoloKronName);
        }

        private void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "CANCEL", HighLogic.Skin.button))
            {
                DisableGui();
            }
        }

        #endregion

        private void Dummy()
        {
        }
    }
}