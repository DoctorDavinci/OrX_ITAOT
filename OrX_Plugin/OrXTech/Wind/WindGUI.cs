using KSP.UI.Screens;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OrXWind
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

        private int pointCountX = 0;
        private int pointCountY = 0;

        List<Vector3d> scanPoints;

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
                if (enableWind)
                {
                    if (!blowing)
                    {
                        blowing = true;
                        setDirection = true;
                        _degrees = "90";
                        Blow(FlightGlobals.ActiveVessel);
                    }
                }
                else
                {
                    blowing = false;
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
                Debug.Log("[OrX Wind] ... Setting up weather");

                if (random360)
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
                    try
                    {
                    if (v.Current == null) continue; // if vessel is null/non existant then move to the next vessel
                    if (!v.Current.loaded || v.Current.packed) continue; // if current vessel is not loaded or vessel is packed move on to the next vessel

                    Debug.Log("[OrX Wind] ... Adding Wind Module to " + v.Current.vesselName);
                    v.Current.rootPart.AddModule("ModuleWind", true); // add ModuleWind to the current vessel and activate the code

                    List<Part>.Enumerator p = v.Current.parts.GetEnumerator();
                    while (p.MoveNext())
                    {
                        if (p.Current.Modules.Contains<ModuleLiftingSurface>())
                        {
                            if (!p.Current.Modules.Contains<ModuleSail>())
                            {
                                p.Current.AddModule("ModuleSail");
                            }
                        }
                    }
                    p.Dispose();
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
                v.Dispose(); // dispose of vessel list ... remove list from RAM and KSP without it going to the garbage heap/collector
            }
        }

        IEnumerator Tease(Vessel v)
        {
            if (blowing)
            {
                yield return new WaitForSeconds(teaseDelay);
                Blow(v);
            }
        }

        private void Blow(Vessel v)
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

            if (windIntensity <= 1)
            {
                windIntensity = 1;
            }

            int windSpeedMod = new System.Random().Next(1, 50); // generate random wind speed modifier for adjusting intensity of wind to simulate lulls and gusts
            int random = new System.Random().Next(1, 100); // for deciding if wind speed is to increae or decrease using the wind speed modifier

            // The below if statement allows for wind intensity changes over time within + or - 10% of the wind intensity setting
            if (random >= 50) // if random is above 50
            {
                //Debug.Log("[OrX Wind] ... Changing wind speed");
                _wi = (windIntensity + (windSpeedMod / 50)) / 10; // increase wind speed by adding the wind speed modifier divided by 50 to wind speed
            }
            else // if random is below 51
            {
                //Debug.Log("[OrX Wind] ... Changing wind speed");
                _wi = (windIntensity - (windSpeedMod / 50)) / 10;// decrese wind speed by subtracting the wind speed modifier divided by 50 to wind speed
            }

            // using up and east (assumed direction of planetary rotation) I can calculate a north vector
            UpVect = (v.transform.position - v.mainBody.position).normalized;
            EastVect = v.mainBody.getRFrmVel(v.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            heading = Vector3.Angle(windDirection, NorthVect);

            if (Math.Sign(Vector3.Dot(windDirection, EastVect)) < 0)
            {
                heading = 360 - heading; // westward headings become angles greater than 180
            }

            if (!random360)
            {
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
            else
            {
                teaseDelay = windSpeedMod;
                if (teaseDelay <= 10)
                {
                    teaseDelay = 10;
                }

                // using the north vector we can establish the general direction of the trade winds and westerlies
                NorthWesterlies = ((NorthVect - EastVect).normalized - EastVect).normalized;
                SouthWesterlies = ((-NorthVect - EastVect).normalized - EastVect).normalized;
                NorthTrades = ((NorthVect - (-EastVect)).normalized - (-EastVect)).normalized;
                SouthTrades = ((-NorthVect - (-EastVect)).normalized - (-EastVect)).normalized;

                // get the current vessels position ... create clean new vector
                currentPos = new Vector3d(v.latitude,
                    v.longitude, v.altitude);

                // declare a virtual position to rotate around the active vessel based off of coords
                Vector3d virtualPos = new Vector3d();

                // get how many degrees in latitude difference active vessel is in relation to equator
                // 0.0055555556f is approx 1 degree
                degOffset = v.latitude / 0.0055555556f;

                if (currentPos.x >= 0) // if in the northern hemisphere
                {
                    if (currentPos.x <= 0.6)
                    {
                        TropoSphere = NorthWesterlies;
                    }
                    else
                    {
                        TropoSphere = NorthTrades;
                    }

                    if (currentPos.x <= 1 - (0.0055555556f * 3)) // if more than 3 degree from the north pole
                    {
                        if (currentPos.y >= 0) // if in eastern quadrant
                        {
                            if (currentPos.y <= 1 - (0.0055555556f / 2)) // if not more than half a degree from the eastern most point in coords
                            {
                                virtualPos = new Vector3d(v.latitude + 0.01111111111,
                   v.longitude + 0.02222222222 - (0.0002469136 * degOffset), v.altitude);

                            }
                        }
                        else
                        {
                            if (currentPos.y >= -1 + (0.0055555556f / 2)) // if more than half a degree from the western most point in coords
                            {
                                virtualPos = new Vector3d(v.latitude + 0.01111111111,
                   v.longitude - 0.02222222222 + (0.0002469136 * degOffset), v.altitude);

                            }
                        }

                        MesoSphere = (virtualPos - currentPos).normalized;
                    }
                    else
                    {
                        // MesoSphere wind direction should be east while less than 3 degrees from the poles
                        MesoSphere = EastVect;
                    }
                }
                else // if in southern hemisphere
                {
                    if (currentPos.x >= -0.6)
                    {
                        TropoSphere = SouthWesterlies;
                    }
                    else
                    {
                        TropoSphere = SouthTrades;
                    }

                    if (currentPos.x >= -1 + (0.0055555556f * 3)) // if more than 3 degree from the south pole
                    {
                        if (currentPos.y >= 0) // if in eastern quadrant
                        {
                            if (currentPos.y <= 1 - (0.0055555556f / 2)) // if not more than half a degree from the eastern most point in coords
                            {
                                virtualPos = new Vector3d(v.latitude - 0.01111111111,
                   v.longitude + 0.02222222222 - (0.0002469136 * degOffset), v.altitude);

                            }
                        }
                        else
                        {
                            if (currentPos.y >= -1 + (0.0055555556f / 2)) // if more than half a degree from the western most point in coords
                            {
                                virtualPos = new Vector3d(v.latitude - 0.01111111111,
                   v.longitude - 0.02222222222 + (0.0002469136 * degOffset),  v.altitude);

                            }
                        }
                        MesoSphere = (virtualPos - currentPos).normalized;

                    }
                    else
                    {
                        // MesoSphere wind direction should be east while less than 3 degrees from the poles
                        MesoSphere = EastVect;
                    }
                }

                GeneralWindDirection = (TropoSphere - MesoSphere).normalized;
                windDirection = GeneralWindDirection;
            }

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
                        Debug.Log("[OrX Wind] ... Changing direction");
                        if (!random360)
                        {
                            windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by subtracting the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                        else
                        {
                            windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * GeneralWindDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                    }
                    else
                    {
                        if (!random360)
                        {
                            variationCount += 1;
                            Debug.Log("[OrX Wind] ... Changing direction for " + v.vesselName);
                            windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                        else
                        {
                            variationCount += 1;
                            Debug.Log("[OrX Wind] ... Changing direction for " + v.vesselName);
                            windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * GeneralWindDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                    }

                    if (variationCount >= 3) // && random360)
                    {
                        originalWindDirection = windDirection;
                        variationCount = 0;
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
                        Debug.Log("[OrX Wind] ... Changing direction for " + v.vesselName);
                        if (!random360)
                        {
                            windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by subtracting the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                        else
                        {
                            windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * GeneralWindDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                    }
                    else
                    {
                        variationCount += 1;
                        Debug.Log("[OrX Wind] ... Changing direction for " + v.vesselName);
                        if (!random360)
                        {
                            windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by subtracting the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                        else
                        {
                            windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * GeneralWindDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                    }

                    if (variationCount >= 3) // && random360)
                    {
                        originalWindDirection = windDirection;
                        variationCount = 0;
                    }
                }
            }

            pointCountX = 0;
            pointCountY = 0;
            GetPoints(v);
        }

        private void GetPoints(Vessel v)
        {
            StartCoroutine(Tease(v));
        }

        // FROM SPANNER
        //
        // already got a list of  wants,  wind direction speed indicator,  selectable wind options,
        // direction variability value  as it tends to blow generally the same way for a few hours.   
        // selectable initial wind direction  to avoid ending up on the beach before yopu even get going (lee shores are bastards)  ,
        // and  oof course wind strength, because while it takes max force to move that ship  at 12ms   , 
        // the same wind will make an aircraft  a nightmare to fly .   Tested  PAI  ,  it couldnt cope with a prop aircraft may have better luck  with a jet

        // TO DO =================================================
        // START WEATHER CALCULATIONS
        // SET BASE WIND DIRECTION OF TRAVEL TO MATCH PLANETARY ROTATION
        // IF VESSEL IS SPLASHED OR BELOW 100 METERS IN AN ATMOSPHERE WINS TRAVELS FROM HIGHEST TO LOWEST POINT
        // IF VESSEL IS AIRBORNE WIND TRAVELS FROM LOWEST TO HIGHEST TO SIMULATE UPDRAFTS (HANG GLIDING AND GLIDERS)
        // ALL WIND DIRECTIONS WILL BE SUBJECT TO THE BASE WIND DIRECTION (EAST)

        // PLOT GROUND ELEVATION AT 32 POINTS AROUND VESSEL
        // USE EASTVECT AS BASE WIND DIRECTION AND MODIFY ACCORDING TO MEASURES POINT ELEVATIONS
        // IF WIND BLOWING EAST ACROSS HILLY TERRAIN CREATE UPDRAFTS


        Vector3d NorthWesterlies;
        Vector3d SouthWesterlies;
        Vector3d NorthTrades;
        Vector3d SouthTrades;
        Vector3d GeneralWindDirection;
        Vector3d currentPos;
        Vector3d MesoSphere;
        Vector3d TropoSphere;

        double degOffset = 0;

        private void CalcWindDirection()
        {
        }

        /// /////////////////////////////////////////////////////////////////////////////

        #region Wind GUI

        private void GuiWindow(int Wind)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            DrawEnableWind(line);
            if (!random360)
            {
                line++;
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
                line++;
                DrawDegrees(line);
                line++;
                line++;
                DrawWindSetDirection(line);
            }
            line++;
            line++;
            Draw360Random(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
        private void AddToolbarButton()
        {
            string textureDir = "OrX/Plugin/";

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
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!random360)
            {
                if (GUI.Button(saveRect, "Simulate Weather", HighLogic.Skin.button))
                {
                    ScreenMsg("Simulated weather is experimental");
                    random360 = true;
                    CalcWindDirection();
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Weather Sim Active", HighLogic.Skin.box))
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