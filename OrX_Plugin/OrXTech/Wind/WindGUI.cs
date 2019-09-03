using KSP.UI.Screens;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Wind
{
    [KSPAddon(KSPAddon.Startup.Flight, true)]
    public class WindGUI : MonoBehaviour
    {
        private const float WindowWidth = 220;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static WindGUI instance;
        public static bool GuiEnabled;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;

        /// /////////////////////////////////////////////////////////////////////////////

        public bool guiActive;
        public bool guiOpen = false;

        public bool enableWind = false;
        public float windIntensity = 10;
        public float _wi = 0;
        public float windVariability = 50;
        public float variationIntensity = 50;
        public bool blowing = false;
        public bool random360 = false;
        public string _degrees = "";
        public int variationCount = 0;
        public float teaseDelay = 0;

        public float heading = 0;
        private bool setDirection = false;
        private bool manual = false;

        public Vector3 windDirection;
        public Vector3 originalWindDirection;

        public Vector3 EastVect;
        public Vector3 NorthVect;
        public Vector3 UpVect;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        private void Start()
        {
            _windowRect = new Rect(Screen.width - (WindowWidth * 1.5f), 100, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisable);
            GameEvents.onShowUI.Add(GameUiEnable);
            AddToolbarButton();
            _wi = windIntensity;
            teaseDelay = 20;
            heading = 0;
            _degrees = "0";
            _gameUiToggle = true;
        }

        private void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (GuiEnabled)
                {
                    _windowRect = GUI.Window(693427116, _windowRect, GuiWindow, "");
                }
            }
        }

        private void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (enableWind && !blowing)
                {
                    blowing = true;
                    Blow();
                }

                if (blowing)
                {
                    BlowIntensity();
                    CalcCoords();
                }
            }
        }

        /// /////////////////////////////////////////////////////////////////////////////
        /// CORE
        /// /////////////////////////////////////////////////////////////////////////////

        private void ToggleWind()
        {
            _degrees = "0";

            if (enableWind) // if Wind is enabled
            {
                blowing = false;
                enableWind = false;
            }
            else // if Wind is not enabled
            {
                enableWind = true;
                Debug.Log("[Wind} ... Setting up weather");

                if (!manual)
                {
                    windDirection = UnityEngine.Random.onUnitSphere; // Generate a random direction for the wind to start blowing in
                    originalWindDirection = windDirection;
                }
                else
                {
                    manual = false;
                }

                List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator(); // creat a list of vessels in game and scrutinize each one
                while (v.MoveNext()) // while scrutinizing a vessel
                {
                    if (v.Current == null) continue; // if vessel is null/non existant then move to the next vessel
                    if (!v.Current.loaded || v.Current.packed) continue; // if current vessel is not loaded or vessel is packed move on to the next vessel

                    Debug.Log("[Wind} ... Adding Wind Module to " + v.Current.vesselName);
                    v.Current.rootPart.AddModule("ModuleWind", true); // add ModuleWind to the current vessel and activate the code

                    // TO DO: add in additional module for sails ... either add separate module or add the code to ModuleWind

                }
                v.Dispose(); // dispose of vessel list ... remove list from RAM and KSP without it going to the garbage heap/collector
            }
        }

        private void BlowIntensity()
        {
            if (windIntensity <= 1)
            {
                windIntensity = 1;
            }

            int windSpeedMod = new System.Random().Next(1, 50); // generate random wind speed modifier for adjusting intensity of wind to simulate lulls and gusts
            int random = new System.Random().Next(1, 100); // for deciding if wind speed is to increae or decrease using the wind speed modifier

            // The below if statement allows for wind intensity changes over time within + or - 10% of the wind intensity setting
            if (random >= 50) // if random is above 50
            {
                //Debug.Log("[Wind} ... Changing wind speed");
                _wi = windIntensity + (windSpeedMod / 50); // increase wind speed by adding the wind speed modifier divided by 50 to wind speed
            }
            else // if random is below 51
            {
                //Debug.Log("[Wind} ... Changing wind speed");
                _wi = windIntensity - (windSpeedMod / 50);// decrese wind speed by subtracting the wind speed modifier divided by 50 to wind speed
            }
        }

        private void Blow()
        {
            int randomDirection = new System.Random().Next(1, 10); // randomizer for variable wind direction ... for determining if wind direction should change and in which direction
            int randomYaw = new System.Random().Next(1, 100); // amount of wind direction change, if any

            // the following code determines any wind direction changes over time
            if (randomDirection <= 6) // if random direction is below 6
            {
                if (randomDirection >= 2) // if random direction is above 2
                {
                    float angle = Vector3.Angle(windDirection, originalWindDirection);

                    if (angle <= windVariability / 100)
                    {
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by subtracting the randomized yaw divided by 1000 from the wind direction Y vector
                    }
                    else
                    {
                        variationCount += 1;
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector

                        if (variationCount >= 3) // && random360)
                        {
                            originalWindDirection = windDirection;
                            variationCount = 0;
                        }
                    }
                }
            }
            else// if random direction is above 5
            {
                if (randomDirection <= 9) // if random direction is below 9
                {
                    float angle = Vector3.Angle(windDirection, originalWindDirection);

                    if (angle <= windVariability / 100)
                    {
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                    }
                    else
                    {
                        variationCount += 1;
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by subtracting the randomized yaw divided by 1000 from the wind direction Y vector

                        if (variationCount >= 3) // && random360)
                        {
                            originalWindDirection = windDirection;
                            variationCount = 0;
                        }
                    }
                }
            }

            StartCoroutine(Tease());
        }

        IEnumerator Tease()
        {
            if (blowing)
            {
                yield return new WaitForSeconds(teaseDelay);
                Blow();
            }
        }

        private void CalcCoords()
        {
            if (windVariability <= 1)
            {
                windVariability = 1;
            }

            if (variationIntensity <= 1)
            {
                variationIntensity = 1;
            }

            if (teaseDelay <= 1)
            {
                teaseDelay = 1;
            }

            // using up and east (assumed direction of planetary rotation) I can calculate a north vector
            UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            heading = Vector3.Angle(windDirection, NorthVect);
            if (Math.Sign(Vector3.Dot(windDirection, EastVect)) < 0)
            {
                heading = 360 - heading; // westward headings become angles greater than 180
            }

            if (setDirection)
            {
                setDirection = false;
                var _heading = float.Parse(_degrees);
                windDirection = NorthVect;
                originalWindDirection = windDirection;

                if (_heading >= 360 || _heading <= 0)
                {
                    _heading = 0;
                    _degrees = "0";
                }
                else
                {
                    windDirection = Quaternion.AngleAxis(_heading, UpVect) * NorthVect;
                    originalWindDirection = windDirection;
                }
            }
        }


        // FROM SPANNER
        //
        // already got a list of  wants,  wind direction speed indicator,  selectable wind options,
        // direction variability value  as it tends to blow generally the same way for a few hours.   
        // selectable initial wind direction  to avoid ending up on the beach before yopu even get going (lee shores are bastards)  ,
        // and  oof course wind strength, because while it takes max force to move that ship  at 12ms   , 
        // the same wind will make an aircraft  a nightmare to fly .   Tested  PAI  ,  it couldnt cope with a prop aircraft may have better luck  with a jet


        /// /////////////////////////////////////////////////////////////////////////////

        #region Wind GUI

        private void GuiWindow(int Wind)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            //line++;
            //Draw360Random(line);
            //line++;
            DrawIntensity(line);
            line++;
            DrawWindIntensity(line);
            line++;
            DrawVariability(line);
            line++;
            DrawWindVariability(line);
            line++;
            DrawVariationIntensity(line);
            line++;
            DrawVIntensity(line);
            line++;
            DrawBlowNTeaseText(line);
            line++;
            DrawBlowNTeaseTimer(line);
            line++;
            DrawEnableWind(line);

            if (!random360)
            {
                line++;
                line++;
                DrawDegrees(line);
                line++;
                line++;
                DrawWindSetDirection(line);
            }

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void AddToolbarButton()
        {
            string textureDir = "Wind/Plugin/";

            if (!HasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(textureDir + "Wind_normal", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(EnableGui, DisableGui, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT, buttonTexture);
                HasAddedButton = true;
            }
        }

        public void EnableGui()
        {
            _degrees = "0";
            GuiEnabled = true;
            guiOpen = true;
            Debug.Log("[Wind]: Showing GUI");
        }

        public void DisableGui()
        {
            _degrees = "0";
            guiOpen = false;
            GuiEnabled = false;
            Debug.Log("[Wind]: Hiding GUI");
        }

        private void GameUiEnable()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisable()
        {
            _gameUiToggle = false;
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
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Wind", titleStyle);
        }

        private void DrawIntensity(float line)
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Wind Intensity", titleStyle);
        }

        private void DrawWindIntensity(float line)
        {
            var Label = new GUIStyle
            {
                normal = { textColor = Color.white }
            };

            var Style = new GUIStyle(Label)
            {
                fontSize = 12,
            };

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0", Style);
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|", Style);
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100", Style);
            windIntensity = GUI.HorizontalSlider(saveRect, windIntensity, 0, 100);
        }

        private void DrawVariability(float line)
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Wind Variability", titleStyle);
        }

        private void DrawWindVariability(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0");
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|");
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100");
            windVariability = GUI.HorizontalSlider(saveRect, windVariability, 0, 100);
        }

        private void DrawVariationIntensity(float line)
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Variation Intesity", titleStyle);
        }

        private void DrawVIntensity(float line)
        {
            var Label = new GUIStyle
            {
                normal = { textColor = Color.white }
            };

            var Style = new GUIStyle(Label)
            {
                fontSize = 12,
            };

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0", Style);
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|", Style);
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100", Style);
            variationIntensity = GUI.HorizontalSlider(saveRect, variationIntensity, 0, 100);
        }

        private void Draw360Random(float line)
        {
            GUIStyle style = random360 ? HighLogic.Skin.box : HighLogic.Skin.button;

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!random360)
            {
                if (GUI.Button(saveRect, "Enable 360 Random", style))
                {
                    random360 = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Disable 360 Random", style))
                {
                    random360 = false;
                }
            }
        }

        private void DrawBlowNTeaseText(float line)
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Blow n Tease Timer", titleStyle);
        }

        private void DrawBlowNTeaseTimer(float line)
        {
            var Label = new GUIStyle
            {
                normal = { textColor = Color.white }
            };

            var Style = new GUIStyle(Label)
            {
                fontSize = 12,
            };

            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0", Style);
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|", Style);
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100", Style);
            teaseDelay = GUI.HorizontalSlider(saveRect, teaseDelay, 0, 100);
        }

        private void DrawEnableWind(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!enableWind)
            {
                if (GUI.Button(saveRect, "Enable Wind"))
                {
                    ToggleWind();
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Disable Wind"))
                {
                    ToggleWind();
                }
            }
        }

        private void DrawDegrees(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Heading",
                leftLabel);
            float textFieldWidth = 80;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            _degrees = GUI.TextField(fwdFieldRect, _degrees);
        }

        private void DrawWindSetDirection(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "Set Direction"))
            {
                enableWind = false;
                setDirection = true;
                manual = true;
                enableWind = true;
            }
        }

        #endregion

        /// /////////////////////////////////////////////////////////////////////////////

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 5, ScreenMessageStyle.UPPER_CENTER));
        }

        private void Dummy() { }


    }
}