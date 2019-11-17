using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class WindDirectionIndicator : MonoBehaviour
    {
        private const float WindowWidth = 140;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static WindDirectionIndicator instance;
        public bool GuiEnabledWindDI = false;
        public static bool TBBadded;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;

        public Vector3 windDirection;
        private string direction = "";
        public float speed = 0;
        public float degrees = 0;

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
            _windowRect = new Rect(WindowWidth / 2, 80, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisableWindDI);
            GameEvents.onShowUI.Add(GameUiEnableWindDI);
            _gameUiToggle = true;
            speed = 0;
        }

        private void OnGUI()
        {
            if (GuiEnabledWindDI && _gameUiToggle)
            {
                _windowRect = GUI.Window(415237212, _windowRect, GuiWindowWindDI, "");
            }
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (WindGUI.instance.enableWind)
                {
                    if (!GuiEnabledWindDI)
                    {
                        GuiEnabledWindDI = true;
                    }

                    IndicatorCheck();
                }
                else
                {
                    if (GuiEnabledWindDI)
                    {
                        GuiEnabledWindDI = false;
                    }
                }
            }
        }

        private void IndicatorCheck()
        {
            degrees = Convert.ToSingle(Math.Round((decimal)WindGUI.instance.heading, 1));  //WindGUI.instance.heading;
            speed = Convert.ToSingle(Math.Round((decimal)WindGUI.instance._wi, 2));

            if (degrees >= 349 && degrees < 11) // 0
            {
                direction = "- N -";
            }

            if (degrees >= 11 && degrees < 34) // 22.5
            {
                direction = "- NNE -";
            }

            if (degrees >= 35 && degrees < 57) // 45
            {
                direction = "- NE -";
            }

            if (degrees >= 57 && degrees < 79) // 47.5
            {
                direction = "- ENE -";
            }

            if (degrees >= 80 && degrees < 100) // 90
            {
                direction = "- E -";
            }

            if (degrees >= 100 && degrees < 122) // 112.5
            {
                direction = "- ESE -";
            }

            if (degrees >= 122 && degrees < 146) // 135
            {
                direction = "- SE -";
            }

            if (degrees >= 146 && degrees < 169) // 157.5
            {
                direction = "- SSE -";
            }

            if (degrees >= 169 && degrees < 191) // 180
            {
                direction = "- S -";
            }

            if (degrees >= 191 && degrees < 214) // 202.5
            {
                direction = "- SSW -";
            }

            if (degrees >= 214 && degrees < 236) // 225
            {
                direction = "- SW -";
            }

            if (degrees >= 236 && degrees < 259) // 247.5
            {
                direction = "- WSW -";
            }

            if (degrees >= 259 && degrees < 281) // 270
            {
                direction = "- W -";
            }

            if (degrees >= 281 && degrees < 303) // 292.5
            {
                direction = "- WNW -";
            }

            if (degrees >= 303 && degrees < 326) // 315
            {
                direction = "- NW -";
            }

            if (degrees >= 326 && degrees < 349) // 315
            {
                direction = "- NNW -";
            }
        }

        #region GUI
        /// <summary>
        /// GUI
        /// </summary>

        private void OnScrnMsgUC(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }

        private void GuiWindowWindDI(int WindDI)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            Direction(line);
            line++;
            DirectionDegrees(line);
            line++;
            Speed(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void EnableGui()
        {
            GuiEnabledWindDI = true;
            Debug.Log("[OrX]: Showing Wind Direction GUI");
        }

        private void DisableGui()
        {
            GuiEnabledWindDI = false;
            Debug.Log("[OrX]: Hiding Wind Direction GUI");
        }

        private void GameUiEnableWindDI()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisableWindDI()
        {
            _gameUiToggle = false;
        }

        private void Direction(float line)
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
                direction,
                titleStyle);
        }

        private void DirectionDegrees(float line)
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
                "" + degrees + " deg",
                titleStyle);
        }

        private void Speed(float line)
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
                "" + speed + " m/s",
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
                "Wind Direction",
                titleStyle);
        }

        #endregion

        private void Blank()
        {
        }
    }
}