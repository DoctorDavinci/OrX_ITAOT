using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXMode : MonoBehaviour
    {
        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXMode instance;
        private bool _modeEnabled = false;
        public bool _guiEnabled = false;

        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        string _pKarma = "";
        public bool _Karma = false;

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
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);
        }
        private void OnGUI()
        {
            if (_guiEnabled)
            {
                _windowRect = GUI.Window(225311375, _windowRect, OrXModeGUI, "");
            }
        }

        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = XKCDColors.BoogerGreen }
        };
        static GUIStyle titleStyleL = new GUIStyle(centerLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        public void SetMode()
        {
            _modeEnabled = true;
            _guiEnabled = true;
        }

        private void OrXModeGUI(int ModeGUI)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            if (_modeEnabled)
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum Modes", titleStyleL);
                line++;
                line += 0.2f;
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Balls of Steel", HighLogic.Skin.button))
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Do you have the balls for this ???", 4, ScreenMessageStyle.UPPER_CENTER));
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Twisted Metal", HighLogic.Skin.button))
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Something Twisted like a Sister .....", 4, ScreenMessageStyle.UPPER_CENTER));
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
                }
                line++;
                line += 0.2f;
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Karmageddon", HighLogic.Skin.button))
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Does this really need clarifying ???", 4, ScreenMessageStyle.UPPER_CENTER));
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
                    count = 0;
                    FlightGlobals.ActiveVessel.rootPart.AddModule("ModuleKarma", true);
                    StartCoroutine(SpawnKarma());
                    _modeEnabled = false;
                    OrXHoloKron.instance.MainMenu();
                    _Karma = true;
                }

                line++;
                line++;
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
                {

                    _modeEnabled = false;
                    OrXHoloKron.instance.MainMenu();
                }


            }
            else
            {
                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, entryHeight), "Password:",
                    leftLabel);
                float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
                _pKarma = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight), _pKarma);
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Enter the void", HighLogic.Skin.button))
                {
                    if (_pKarma == OrXHoloKron.instance.Karma)
                    {
                        OrXLog.instance.DebugLog("[OrX Karma] === UNLOCKING ===");
                        _modeEnabled = true;
                        OrXHoloKron.instance._pKarma = _pKarma;
                    }
                    else
                    {
                        OrXLog.instance.DebugLog("[OrX Karma] === WRONG PASSWORD ===");
                        OrXHoloKron.instance.ScreenMsg("WRONG PASSWORD");
                    }
                }
            }
            line++;
            line += 0.2f;

            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Close Window", HighLogic.Skin.button))
            {
                _guiEnabled = false;
                _modeEnabled = false;
                OrXHoloKron.instance.MainMenu();
                OrXHoloKron.instance._showSettings = true;
            }


            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        int count = 0;

        IEnumerator SpawnKarma()
        {
            if (count <= 5)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Karmageddon victim #" + count + " spawning .....", 4, ScreenMessageStyle.UPPER_CENTER));

                count += 1;
                spawn.OrXSpawn.instance.SpawnInfected();
                yield return new WaitForFixedUpdate();
                while (spawn.OrXSpawn.instance.spawning)
                {
                    yield return null;
                }
                StartCoroutine(SpawnKarma());
            }
        }
    }
}