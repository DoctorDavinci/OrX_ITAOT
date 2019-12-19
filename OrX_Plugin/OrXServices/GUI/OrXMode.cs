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
        public static OrXMode instance;

        #region Variables

        private const float WindowWidth = 400;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        private bool _modeEnabled = false;
        public bool _guiEnabled = false;

        public static bool TBBadded;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 250;
        public static Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        string _pKarma = "";
        public bool _Karma = false;
        int victimCount = 0;
        int count = 0;

        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = XKCDColors.OrangeRed }
        };
        static GUIStyle titleStyleL = new GUIStyle(centerLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        #endregion

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
            GUI.backgroundColor = XKCDColors.DarkGrey;
            GUI.contentColor = XKCDColors.DarkGrey;
            GUI.color = XKCDColors.DarkGrey;

            if (_guiEnabled)
            {
                _windowRect = GUI.Window(225311375, _windowRect, OrXModeGUI, "");
            }
        }

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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum Modes", titleStyleL);
            line += 0.2f;
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "The Loot Box Controversy", OrXGUISkin.button))
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Take those Loot Boxes by any means necessary", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
            }
            line++;
            line += 0.2f;

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Pirates of the Kontinuum", OrXGUISkin.button))
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Are you salty enough for this sailor?", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
            }
            line++;
            line += 0.2f;
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "The Hunt for Red Oktober", OrXGUISkin.button))
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Now understand, Commander, that torpedo did not self-destruct", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("I did not say this ...  I was never here ..... ", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
            }
            line++;
            line += 0.2f;

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Big Trouble in Little China", OrXGUISkin.button))
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Oh, my god, no. Please! What is that? Don’t tell me!", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
            }

            line++;
            line += 0.2f;
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Karmageddon", OrXGUISkin.button))
            {
                _modeEnabled = false;
                _Karma = true;
                _guiEnabled = false;
                OrXHoloKron.instance.OrXHCGUIEnabled = false;
                OrXHoloKron.instance.MainMenu();
                OrXHoloKron.instance.StartTimer();
                OrX_KC.instance.ToggleKarma(true);
                ScreenMessages.PostScreenMessage(new ScreenMessage("Does this really need clarifying ???", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("Watch out for pedestrians .....", 4, ScreenMessageStyle.UPPER_CENTER));
                //FlightGlobals.ActiveVessel.rootPart.AddModule("ModuleKarma", true);
            }

            line++;
            line += 0.2f;
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Balls of Steel", OrXGUISkin.button))
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Do you have the balls for this ???", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
            }

            line++;
            line += 0.2f;

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Where's Waldo?", OrXGUISkin.button))
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("If you need someone to talk to .....", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("..... you know where to find me", 4, ScreenMessageStyle.UPPER_CENTER));
                ScreenMessages.PostScreenMessage(new ScreenMessage("Coming Soon to a Kontinuum near you .....", 4, ScreenMessageStyle.UPPER_CENTER));
            }

            line++;
            line++;
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Close Window", OrXGUISkin.button))
            {
                _guiEnabled = false;
                OrXHoloKron.instance.MainMenu();
                OrXHoloKron.instance._showSettings = true;
            }


            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
    }
}