using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OrXWind
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class WindDirectionIndicator : MonoBehaviour
    {
        private const float WindowWidth = 100;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static WindDirectionIndicator instance;
        public bool GuiEnabledWindDI = false;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;

        public Vector3 windDirection;
        public string direction = "";
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
            _windowRect = new Rect(WindowWidth / 4, 50, WindowWidth, _windowHeight);
            _gameUiToggle = true;
            speed = 0;
        }

        private void OnGUI()
        {
            if (WindGUI.instance.enableWind)
            {
                _windowRect = GUI.Window(415237212, _windowRect, GuiWindowWindDI, "");
            }
        }

        private void GuiWindowWindDI(int WindDI)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "W[ind/S] " + direction, titleStyle);
            line++;
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "" + Math.Round(degrees, 1) + " degrees", titleStyle);
            line++;
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "" + Math.Round(speed, 1) + " m/s", titleStyle);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
    }
}