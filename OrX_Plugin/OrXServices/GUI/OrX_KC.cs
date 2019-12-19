using System;
using UnityEngine;

using System.Collections.Generic;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrX_KC : MonoBehaviour
    {
        private const float WindowWidth = 180;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrX_KC instance;
        public bool GuiEnabledOrX_KC = false;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private static Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        public float salt = 0;
        public bool _Karma = false;
        public int victimCount = 0;

        static GUIStyle titleStyleMedNoItal = new GUIStyle(centerLabelOrange)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        static GUIStyle centerLabelOrange = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = XKCDColors.OrangeRed }
        };
        static GUIStyle titleStyleMedGreen = new GUIStyle(centerLabelGreen)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic,
            normal = { textColor = XKCDColors.BoogerGreen }
        };
        static GUIStyle centerLabelGreen = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = XKCDColors.BoogerGreen }
        };

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }
        private void Start()
        {
            _windowRect = new Rect((Screen.width / 4) * 2 - (WindowWidth * 3) + 10, 10, WindowWidth, _windowHeight);
            salt = 0;
        }
        private void OnGUI()
        {
            if (GuiEnabledOrX_KC)
            {
                GUI.backgroundColor = XKCDColors.DarkGrey;
                GUI.contentColor = XKCDColors.DarkGrey;
                GUI.color = XKCDColors.DarkGrey;

                _windowRect = GUI.Window(416937212, _windowRect, GuiWindowOrX_KC, "");
            }
        }

        public void ToggleKarma(bool _karma)
        {
            if (GuiEnabledOrX_KC)
            {
                _Karma = false;
                GuiEnabledOrX_KC = false;
            }
            else
            {
                GuiEnabledOrX_KC = true;
                salt = 0;
                victimCount = 0;
                if (_karma)
                {
                    _Karma = true;
                    StartCoroutine(SpawnKarma());
                }
            }
        }
        private void GuiWindowOrX_KC(int OrX_KC)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            GUI.Label(new Rect(0, 0, WindowWidth / 2, 20), "Time: ", titleStyleMedNoItal);
            GUI.Label(new Rect(WindowWidth / 2, 1, WindowWidth / 2, 20), OrXHoloKron.instance._timerTotalTime, titleStyleMedGreen);
            GUI.Label(new Rect(0, (ContentTop + line * entryHeight), WindowWidth / 2, 20), "SALT: ", titleStyleMedNoItal);
            GUI.Label(new Rect(WindowWidth / 2, (ContentTop + line * entryHeight), WindowWidth / 2, 20), Math.Round(salt, 3).ToString(), titleStyleMedGreen);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
        IEnumerator SpawnKarma()
        {
            if (_Karma)
            {
                if (victimCount <= 5)
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Spawning pedestrian .....", 4, ScreenMessageStyle.UPPER_CENTER));

                    victimCount += 1;
                    spawn.OrXSpawnHoloKron.instance.SpawnFile("", true, true, false, false, false, 0, 0, 0, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                    yield return new WaitForSeconds(2);
                    StartCoroutine(SpawnKarma());
                }
                else
                {
                    yield return new WaitForSeconds(15);
                    StartCoroutine(SpawnKarma());
                }
            }
        }
    }
}