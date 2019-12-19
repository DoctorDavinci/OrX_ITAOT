using KSP.UI.Screens;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class WindGUI : MonoBehaviour
    {
        private const float WindowWidth = 220;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static WindGUI instance;
        public static bool GuiEnabled;
        public static bool TBBadded;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;

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

        public Vector3 East;
        public Vector3 North;
        public Vector3 UpVect;

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
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), 250, WindowWidth, _windowHeight);
            TBBAdd();
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
                if (!FlightGlobals.ready) GuiEnabled = false;
                if (PauseMenu.isOpen) return;
                GUI.backgroundColor = XKCDColors.DarkGrey;
                GUI.contentColor = XKCDColors.DarkGrey;
                GUI.color = XKCDColors.DarkGrey;

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

        #region Core

        private void ToggleWind()
        {
            _degrees = "0";

            if (enableWind) 
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

                List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                while (v.MoveNext())
                {
                    if (v.Current == null) continue;
                    if (!v.Current.loaded || v.Current.packed) continue;

                    Debug.Log("[Wind} ... Adding Wind Module to " + v.Current.vesselName);
                    v.Current.rootPart.AddModule("ModuleWind", true);
                }
                v.Dispose(); 
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
            East = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            North = Vector3.Cross(East, UpVect).normalized;
            heading = Vector3.Angle(windDirection, North);
            if (Math.Sign(Vector3.Dot(windDirection, East)) < 0)
            {
                heading = 360 - heading; // westward headings become angles greater than 180
            }

            if (setDirection)
            {
                setDirection = false;
                var _heading = float.Parse(_degrees);
                windDirection = North;
                originalWindDirection = windDirection;

                if (_heading >= 360 || _heading <= 0)
                {
                    _heading = 0;
                    _degrees = "0";
                }
                else
                {
                    windDirection = Quaternion.AngleAxis(_heading, UpVect) * North;
                    originalWindDirection = windDirection;
                }
            }
        }

        #endregion

        /// /////////////////////////////////////////////////////////////////////////////
        /// Weather and Seasons
        /// /////////////////////////////////////////////////////////////////////////////

        #region Weather Patterns and Seasons

        double atmDensity = 0;

        public string CurrentBiome()
        {
            if (FlightGlobals.ActiveVessel.landedAt != string.Empty)
            {
                return Vessel.GetLandedAtString(FlightGlobals.ActiveVessel.landedAt);
            }
            string biome = ScienceUtil.GetExperimentBiome(FlightGlobals.ActiveVessel.mainBody, FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude);
            return "" + biome;
        }

        public static float TerrainHeight(double lat, double lon, CelestialBody body)
        {
            return 0;
        }


        private void Humidity()
        {
            // get terrain height at location
            var terrainAltitude = FlightGlobals.ActiveVessel.terrainAltitude;
            var surfaceAlt = FlightGlobals.ActiveVessel.altitude;
        }

        private void HeatMap()
        {
            // 
        }

        private void OceanCurrents()
        {
            // 
        }

        private void StormSeason()
        {
            // 
        }

        private void TornadoSeason()
        {
            // 
        }

        private void HurricaneSeason()
        {
            // 
        }

        #endregion

        #region Wind GUI

        static GUIStyle titleStyle = new GUIStyle(centerLabelYellow)
        {
            fontSize = 11,
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.yellow }
        };
        static GUIStyle leftLabel = new GUIStyle() { alignment = TextAnchor.UpperLeft, normal = { textColor = Color.white } };

        static GUIStyle centerLabelYellow = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.yellow }
        };

        static GUIStyle centerLabelOrange = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = XKCDColors.OrangeRed }
        };

        static GUIStyle titleStyleOrange = new GUIStyle(centerLabelOrange)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold

        };

        private void GuiWindow(int Wind)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX W[ind/S]", titleStyleOrange);
            //line++;
            //Draw360Random(line);
            //line++;
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "W[ind/S] Intensity: " + windIntensity, titleStyle);
            line++;
            windIntensity = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), windIntensity, 0, 100, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalScrollbarThumb);
            line++;
            line += 0.2f;
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "W[ind/S] Variability: " + windVariability, titleStyle);
            line++;
            windVariability = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), windVariability, 0, 100, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalScrollbarThumb);
            line++;
            line += 0.2f;

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Variation Intesity: " + variationIntensity, titleStyle);
            line++;
            variationIntensity = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), variationIntensity, 0, 100, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalScrollbarThumb);
            line++;
            line += 0.2f;

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Tease Timer: " + teaseDelay, titleStyle);
            line++;
            teaseDelay = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), teaseDelay, 0, 100, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalScrollbarThumb);
            line++;
            line += 0.2f;

            if (!enableWind)
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "Enable W[ind/S]", OrXGUISkin.button))
                {
                    ToggleWind();
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "Disable W[ind/S]", OrXGUISkin.box))
                {
                    ToggleWind();
                }
            }
            line++;
            line += 0.2f;

            if (!random360)
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "Random W[ind/S]", OrXGUISkin.button))
                {
                    random360 = true;
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "Disable Random", OrXGUISkin.box))
                {
                    random360 = false;
                }
            }

            if (!random360)
            {
                line++;
                line++;
                GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, entryHeight), "Heading", leftLabel);
                _degrees = GUI.TextField(new Rect(WindowWidth - 100, ContentTop + line * entryHeight, 80, entryHeight), _degrees);
                line++;

                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "Set Direction", OrXGUISkin.button))
                {
                    setDirection = true;
                    manual = true;
                }
            }

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void TBBAdd()
        {
            string textureDir = "OrX/Plugin/";

            if (!TBBadded)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(textureDir + "Wind_normal", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(EnableGui, DisableGui, Blank, Blank, Blank, Blank,
                    ApplicationLauncher.AppScenes.FLIGHT, buttonTexture);
                TBBadded = true;
            }
        }
        public void EnableGui()
        {
            _degrees = "0";
            GuiEnabled = true;
            guiOpen = true;
            Debug.Log("[OrX W[ind/S]]: Showing GUI");
        }
        public void DisableGui()
        {
            _degrees = "0";
            guiOpen = false;
            GuiEnabled = false;
            Debug.Log("[OrX W[ind/S]]: Hiding GUI");
        }

        #endregion

        private void Blank() { }


    }
}