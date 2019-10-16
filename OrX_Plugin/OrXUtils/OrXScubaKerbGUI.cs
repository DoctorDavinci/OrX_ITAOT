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
        public static bool HasAddedButton;
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

            DrawTitle();
            DrawOxygenText(line);
            line++;
            DrawOxygen(line);
            line++;
            DrawNarcosis(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void DrawNarcosis(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperCenter;
            leftLabel.normal.textColor = Color.white;
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Status: ",
                leftLabel);

            var rightLabel = new GUIStyle();
            rightLabel.alignment = TextAnchor.UpperCenter;
            rightLabel.fontStyle = FontStyle.Bold;

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
                rightLabel.normal.textColor = XKCDColors.PaleMagenta;
            }

            GUI.Label(new Rect((WindowWidth / 2) - LeftIndent, ContentTop + line * entryHeight, 140, entryHeight),
                narcosisText, rightLabel);
        }

        private void DrawOxygenText(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                "OXYGEN %",
                titleStyle);
        }

        private void DrawOxygen(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0");
            GUI.Label(new Rect(95, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|");
            GUI.Label(new Rect(175, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100");
            oxygen = GUI.HorizontalSlider(saveRect, oxygen, 0, 100);
        }

        private void DrawTitle()
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Scuba Kerb", titleStyle);
        }

        private void Dummy()
        {
        }
    }
}