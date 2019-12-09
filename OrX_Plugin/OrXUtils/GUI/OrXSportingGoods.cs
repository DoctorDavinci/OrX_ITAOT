using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXSportingGoods : MonoBehaviour
    {
        private const float WindowWidth = 180;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXSportingGoods instance;
        public bool GuiEnabledOrXSG = false;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 250;
        private static Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;

        List<string> airSupport = new List<string>();
        List<string> airSupportNames = new List<string>();

        
        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }

        private void Start()
        {
            airSupport = new List<string>();
            airSupportNames = new List<string>();
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), 10, WindowWidth, _windowHeight);
        }

        private void OnGUI()
        {
            if (GuiEnabledOrXSG)
            {
                GUI.backgroundColor = XKCDColors.DarkGrey;
                GUI.contentColor = XKCDColors.DarkGrey;
                GUI.color = XKCDColors.DarkGrey;

                _windowRect = GUI.Window(409937212, _windowRect, GuiWindowOrXSG, "");
            }
        }

        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        };
        static GUIStyle titleStyle = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter
        };
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
        public Vector2 scrollPosition = Vector2.zero;

        public void OpenSportingGoods(List<string> _airSupport, List<string> _airSupportNames)
        {
            airSupportNames = _airSupportNames;
            airSupport = _airSupport;
            GuiEnabledOrXSG = true;
        }

        private void GuiWindowOrXSG(int OrXSG)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            int scrollIndex = 0;
            int pmScrollIndex = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Sporting Goods", titleStyleMedNoItal);
            line++;
            line += 0.5f;
            GUI.Label(new Rect(0, (ContentTop + line * entryHeight), WindowWidth / 2, 20), "Salt: ", titleStyleMedNoItal);
            GUI.Label(new Rect(WindowWidth / 2, (ContentTop + line * entryHeight), WindowWidth / 2, 20), Math.Round(OrXHoloKron.instance.salt, 3).ToString(), titleStyleMedGreen);
            line++;
            line += 0.5f;

            scrollPosition = GUI.BeginScrollView(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 140), scrollPosition, new Rect(15, 0, WindowWidth - 30, airSupportNames.Count * 20));

            List<string>.Enumerator _airSupportNames = airSupportNames.GetEnumerator();
            while (_airSupportNames.MoveNext())
            {
                if (_airSupportNames.Current != null)
                {
                    scrollIndex += 1;

                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), _airSupportNames.Current, OrXGUISkin.button))
                    {
                        if (HighLogic.LoadedSceneIsFlight)
                        {
                            List<string>.Enumerator _airSupport = airSupport.GetEnumerator();
                            while (_airSupport.MoveNext())
                            {
                                if (_airSupport.Current != null)
                                {
                                    if (_airSupport.Current.Contains(_airSupportNames.Current))
                                    {
                                        spawn.OrXSpawnHoloKron.instance.SpawnFile(_airSupport.Current, true, false, true);
                                    }
                                }
                            }
                            _airSupport.Dispose();
                        }
                    }
                    line++;
                    line += 0.2f;
                }
            }
            _airSupportNames.Dispose();

            GUI.EndScrollView();
            if (scrollIndex >= 7)
            {
                line += 7;
            }
            else
            {
                line += scrollIndex;
            }
            line++;
            line += 0.5f;

            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE UP SHOP", OrXGUISkin.button))
            {
                GuiEnabledOrXSG = false;
            }

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
    }
}