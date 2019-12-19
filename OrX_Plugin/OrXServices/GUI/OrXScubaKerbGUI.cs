using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXScubaKerbGUI : MonoBehaviour
    {
        public static OrXScubaKerbGUI instance;

        private const float WindowWidth = 220;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public bool GuiEnabledScuba;
        public static bool TBBadded;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 250;
        private Rect _windowRect;

        public bool narcosis = false;
        public bool bends = false;
        string narcosisText = "Nominal";

        public float oxygen = 100.0f;
        public double _scubaLevel = 1;

        public bool drunk = false;
        public double martiniLevel = 0;

        static GUIStyle rightLabel = new GUIStyle
        {
            fontSize = 11,
            alignment = TextAnchor.UpperCenter,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        };
        static GUIStyle centerLabelOrange = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = XKCDColors.OrangeRed }
        };

        static GUIStyle titleStyle = new GUIStyle(centerLabel)
        {
            fontSize = 11,
            alignment = TextAnchor.MiddleCenter
        };
        static GUIStyle titleStyleOrange = new GUIStyle(centerLabelOrange)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        public void Awake()
        {
            if (instance)
                Destroy(instance);
            instance = this;
        }

        void Start()
        {
            _windowRect = new Rect((Screen.width / 2) + WindowWidth, 10, WindowWidth, _windowHeight);
        }

        public void Update()
        {
            if (FlightGlobals.ActiveVessel.isEVA)
            {
                if (FlightGlobals.ActiveVessel.Splashed)
                {
                    GuiEnabledScuba = true;
                }
                else
                {
                    GuiEnabledScuba = false;
                }
            }
            else
            {
                GuiEnabledScuba = false;
            }
        }

        private void OnGUI()
        {
            if (PauseMenu.isOpen) return;
            GUI.backgroundColor = XKCDColors.DarkGrey;
            GUI.contentColor = XKCDColors.DarkGrey;
            GUI.color = XKCDColors.DarkGrey;

            if (GuiEnabledScuba)
            {
                _windowRect = GUI.Window(628263315, _windowRect, GuiWindowOrXScuba, "");
            }
        }

        private void GuiWindowOrXScuba(int OrXScuba)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Scuba Kerb", titleStyleOrange);
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "OXYGEN %", titleStyle);
            line++;
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0");
            GUI.Label(new Rect(95, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|");
            GUI.Label(new Rect(175, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100");
            oxygen = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), oxygen, 0, 100);
            line++;
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Status: ", titleStyle);

            if (!drunk)
            {
                if (martiniLevel <= 0.2f)
                {
                    narcosisText = "Nominal";

                    rightLabel.normal.textColor = XKCDColors.Green;
                }
                else
                {
                    if (martiniLevel <= 4)
                    {
                        if (martiniLevel >= 2.49)
                        {
                            rightLabel.normal.textColor = XKCDColors.Yellow;
                        }
                        else
                        {
                            rightLabel.normal.textColor = XKCDColors.LightGreen;
                        }
                    }
                    else
                    {
                        rightLabel.normal.textColor = XKCDColors.Orange;
                    }

                    narcosisText = Math.Round(martiniLevel, 1) + " Martini's";
                }
            }
            else
            {
                narcosisText = "You're Drunk";
                rightLabel.normal.textColor = XKCDColors.OrangeRed;
            }

            GUI.Label(new Rect((WindowWidth / 2) - LeftIndent, ContentTop + line * entryHeight, 140, entryHeight), narcosisText, rightLabel);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void DrawNarcosis(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperCenter;
            leftLabel.normal.textColor = Color.white;
        }

        private void DrawOxygenText(float line)
        {
        }

        private void DrawOxygen(float line)
        {
        }

        private void DrawTitle()
        {
        }

        private void Blank()
        {
        }
    }
}