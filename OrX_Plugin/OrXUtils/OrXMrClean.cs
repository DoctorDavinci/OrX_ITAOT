﻿using KSP.UI.Screens;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace MrClean
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXMrClean : MonoBehaviour
    {
        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXMrClean instance;
        public static bool GuiEnableMrClean;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;

        public bool guiActive;
        private bool auto = false;
        private float _delayTimer = 0.0f;


        public int debris = 0;
        private bool triggered = false;

        private void Start()
        {
            _windowRect = new Rect(Screen.width - (WindowWidth * 2.5f), 10, WindowWidth, _windowHeight);
            AddToolbarButton();
            GameEvents.onHideUI.Add(DisableGuiMrClean);
            GameEvents.onShowUI.Add(EnableGuiMrClean);
            _gameUiToggle = true;
        }

        private void AddToolbarButton()
        {
            string textureDir = "MrClean/Plugin/";

            if (!HasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(textureDir + "MrClean_icon", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(DisableGuiMrClean, EnableGuiMrClean, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT, buttonTexture);
                HasAddedButton = true;
            }
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (auto && !triggered)
                {
                    triggered = true;
                    StartCoroutine(RealTimeCheck());
                }
            }
        }

        IEnumerator RealTimeCheck()
        {
            var RTC_ = HighLogic.CurrentGame.flightState.universalTime;
            yield return new WaitForSecondsRealtime(2);
            var _RTC = HighLogic.CurrentGame.flightState.universalTime;
            var RTC = _RTC - RTC_;

            var IGC_ = HighLogic.CurrentGame.flightState.universalTime;
            yield return new WaitForSeconds(2);
            var _IGC = HighLogic.CurrentGame.flightState.universalTime;
            var IGC = _IGC - IGC_;

            yield return new WaitForFixedUpdate();

            if (IGC <= RTC * 0.80)
            {
                ClearDebris();
            }
            else
            {
                yield return new WaitForSeconds(25);
                triggered = false;
            }
        }

        private void ClearDebris()
        {
            Debug.LogError("Mr Clean: Start Clearing Debris");

            if (!auto)
            {
                ScreenMsg("<color=#cfc100ff><b>Mr Clean: Getting a Mop and Bucket .......</b></color>");
            }

            StartCoroutine(ClearDebrisRoutine());
        }

        IEnumerator ClearDebrisRoutine()
        {
            debris = 0;

            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
            while (v.MoveNext())
            {
                if (v.Current == null) continue;

                if (!v.Current.isEVA)
                {
                    if (v.Current.vesselName.Contains("Debris"))
                    {
                        if (v.Current.missionTime >= 30)
                        {
                            debris += 1;
                            var count = 0;
                            List<Part>.Enumerator p = v.Current.parts.GetEnumerator();
                            while (p.MoveNext())
                            {
                                if (count == 0)
                                {
                                    count += 1;

                                    p.Current.AddModule("ModuleDestroyVessel", true);
                                }
                            }

                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
            }
            v.Dispose();
            yield return new WaitForEndOfFrame();

            Debug.LogError("MR CLEAN FINISHED ... REMOVING " + debris + " TOTAL PIECES OF DEBRIS FROM GAME");

            if (auto)
            {
                yield return new WaitForSeconds(25);
                triggered = false;
            }
            else
            {
                ScreenMsg("<color=#cfc100ff><b>MR CLEAN REMOVING " + debris + " PIECES OF DEBRIS FROM GAME ... LEMONY FRESH<color></b>");
                triggered = false;
            }
        }

        private void Dummy()
        {

        }

        private void OnGUI()
        {
            if (GuiEnableMrClean && _gameUiToggle)
            {
                _windowRect = GUI.Window(616781412, _windowRect, GuiWindowMrClean, "");
            }
        }

        public void ToggleGUI()
        {
            if (GuiEnableMrClean)
            {
                GameUiDisableMrClean();

            }
            else
            {
                GameUiEnableMrClean();
            }
        }

        /// <summary>
        /// /////////////////////////
        /// </summary>


        void Awake()
        {
            Debug.LogError("Mr Clean: Awake");

            if (instance) Destroy(instance);
            instance = this;
        }


        /////////////////////////////////////////////

        #region GUI
        /// <summary>
        /// GUI
        /// </summary>

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 6, ScreenMessageStyle.UPPER_CENTER));
        }

        private void GuiWindowMrClean(int MrClean)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawMrCleanTitle(line);
            DrawLemonsText(line);
            line++;
            DrawGetMop(line);
            line++;
            AutoScrubToggle(line);


            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void DrawLemonsText(float line)
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
                "Now with a fresh lemon scent",
                titleStyle);
        }

        private void DrawGetMop(float line)
        {

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "Get Mop n Bucket"))
            {
                ClearDebris();
                DisableGuiMrClean();
            }
        }

        private void ToggleAuto()
        {
            if (auto)
            {
                    auto = false;
            }
            else
            {
                    auto = true;
            }
        }

        private void AutoScrubToggle(float line)
        {
            GUIStyle style = auto ? HighLogic.Skin.box : HighLogic.Skin.button;
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "Auto Scrub", style))
            {
                ToggleAuto();
            }
        }

        private void EnableGuiMrClean()
        {
            GuiEnableMrClean = true;
            guiActive = true;
            _gameUiToggle = true;


            Debug.Log("[MrClean]: Showing MrClean GUI");
        }

        private void DisableGuiMrClean()
        {
            GuiEnableMrClean = false;
            _gameUiToggle = false;

            guiActive = false;

            Debug.Log("[MrClean]: Hiding MrClean GUI");
        }

        private void GameUiEnableMrClean()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisableMrClean()
        {
            _gameUiToggle = false;
        }

        private void DrawMrCleanTitle(float line)
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
            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Mr Clean Magic Eraser", titleStyle);
        }

        #endregion

    }
}