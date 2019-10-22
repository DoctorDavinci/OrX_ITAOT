using KSP.UI.Screens;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OrXWind
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
        public string _heading = "";
        public int variationCount = 0;
        public float teaseDelay = 0;
        private float _headingSlider = 0;
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
            if (instance) Destroy(instance);
            instance = this;
        }
        private void Start()
        {
            _windowRect = new Rect(Screen.width - (WindowWidth * 1.5f), 100, WindowWidth, _windowHeight);
            //GameEvents.onHideUI.Add(DisableGui);
            AddToolbarButton();
            _wi = windIntensity;
            teaseDelay = 20;
            heading = 0;
            _heading = "0";
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

        private void ToggleWind()
        {
            OrX.OrXHoloKron.instance.ScreenMsg("W[ind/S] is currently unavailable .....");
            OrX.OrXHoloKron.instance.ScreenMsg("Please check back next update .....");

            /*
                        if (enableWind)
                        {
                            blowing = false;
                            enableWind = false;
                            WindDirectionIndicator.instance.GuiEnabledWindDI = false;
                        }
                        else
                        {
                            Debug.Log("[OrX W[ind/S]] ... Setting up weather");
                            enableWind = true;

                            if (random360)
                            {
                                windDirection = UnityEngine.Random.onUnitSphere; 
                                originalWindDirection = windDirection;
                            }
                            else
                            {
                                manual = false;
                            }

                            AddModule();
                        }*/
        }

        private void AddModule()
        {
            bool listError = false;
            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
            while (v.MoveNext())
            {
                try
                {
                    if (v.Current == null) continue;
                    if (v.Current.packed && !v.Current.loaded) continue;
                    Debug.Log("[OrX W[ind/S]] ... waking the weatherman");

                    if (!v.Current.rootPart.Modules.Contains<KerbalEVA>())
                    {
                        List<Part>.Enumerator part = v.Current.parts.GetEnumerator();
                        while (part.MoveNext())
                        {
                            if (part.Current != null)
                            {
                                if (part.Current.Modules.Contains<ModuleLiftingSurface>() && !part.Current.Modules.Contains<ModuleSail>() && !part.Current.Modules.Contains<KerbalEVA>())
                                {
                                    part.Current.AddModule("ModuleSail", true);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("[OrX W[ind/S]] ERROR ... RETRYING ... " + e);
                    listError = true;
                }
            }
            v.Dispose();

            if (!listError)
            {
                blowing = true;
                StartCoroutine(Tease(FlightGlobals.ActiveVessel));
            }
            else
            {
                AddModule();
            }
        }

        IEnumerator Tease(Vessel v)
        {
            if (blowing)
            {
                Debug.Log("[OrX W[ind/S]] TEASING FOR " + Math.Round(teaseDelay, 0) + " seconds ...");
                setDirection = false;
                manual = false;
                UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
                EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
                NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
                yield return new WaitForSeconds(teaseDelay);
                Blow(v);
            }
        }
        private void Blow(Vessel v)
        {
            Debug.Log("[OrX W[ind/S]] BLOWING ... INTENSITY: " + _wi);

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

            int windSpeedMod = new System.Random().Next(1, 50);
            int random = new System.Random().Next(1, 100); 

            if (random <= 30) 
            {
                _wi = (windIntensity + (windSpeedMod / 50)) / 10;
                Debug.Log("[OrX W[ind/S]] ... Changing wind speed " + _wi);
            }
            else
            {
                if (random >= 70)
                {
                    _wi = (windIntensity - (windSpeedMod / 50)) / 10;
                    Debug.Log("[OrX W[ind/S]] ... Changing wind speed " + _wi);
                }
            }

            if (!random360)
            {
                if (setDirection)
                {
                    setDirection = false;
                    heading = _headingSlider;

                    if (heading >= 359 || heading <= 0)
                    {
                        heading = 0;
                        windDirection = NorthVect;
                        originalWindDirection = windDirection;
                    }
                    else
                    {
                        windDirection = Quaternion.AngleAxis(heading, UpVect) * NorthVect;
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
                Debug.Log("[OrX W[ind/S]] ... Tease Delay = " + teaseDelay);

                // get the current vessel position ... create clean new vector
                currentPos = new Vector3((float)v.latitude, (float)v.longitude, (float)v.altitude);

                // declare a virtual position to rotate around the active vessel based off of coords
                Vector3 virtualPos = new Vector3();
                
                // get how many heading in latitude difference active vessel is in relation to equator
                // 0.0055555556f is approx 1 degree
                degOffset = (float)v.latitude * 0.0055555556f;

                if (1 / currentPos.x >= 0) // if in the northern hemisphere
                {
                    if (1 / currentPos.x <= 0.6)
                    {
                        Debug.Log("[OrX W[ind/S]] ... TropoSphere = NorthWesterlies");

                        TropoSphere = NorthWesterlies;
                    }
                    else
                    {
                        Debug.Log("[OrX W[ind/S]] ... TropoSphere = NorthTrades");

                        TropoSphere = NorthTrades;
                    }

                    if (1 / currentPos.x <= 1 - (0.0055555556f * 3)) // if more than 3 degree from the north pole
                    {
                        if (1 / currentPos.y >= 0) // if in eastern quadrant
                        {
                            if (1 / currentPos.y <= 1 - (0.0055555556f / 2)) // if not more than half a degree from the eastern most point in coords
                            {
                                virtualPos = new Vector3((float)v.latitude + 0.01111111111f,(float)v.longitude + 0.02222222222f - (0.0002469136f * degOffset), (float)v.altitude);

                            }
                        }
                        else
                        {
                            if (1 / currentPos.y >= -1 + (0.0055555556f / 2)) // if more than half a degree from the western most point in coords
                            {
                                virtualPos = new Vector3((float)v.latitude + 0.01111111111f, (float)v.longitude - 0.02222222222f + (0.0002469136f * degOffset), (float)v.altitude);

                            }
                        }

                        MesoSphere = (virtualPos - currentPos).normalized;
                    }
                    else
                    {
                        // MesoSphere wind direction should be east while less than 3 heading from the poles
                        MesoSphere = EastVect;
                    }
                }
                else // if in southern hemisphere
                {
                    if (1 / currentPos.x >= -0.6)
                    {
                        Debug.Log("[OrX W[ind/S]] ... TropoSphere = SouthWesterlies");

                        TropoSphere = SouthWesterlies;
                    }
                    else
                    {
                        Debug.Log("[OrX W[ind/S]] ... TropoSphere = SouthTrades");

                        TropoSphere = SouthTrades;
                    }

                    if (1 / currentPos.x >= -1 + (0.0055555556f * 3)) // if more than 3 degree from the south pole
                    {
                        if (1 / currentPos.y >= 0) // if in eastern quadrant
                        {
                            if (1 / currentPos.y <= 1 - (0.0055555556f / 2)) // if not more than half a degree from the eastern most point in coords
                            {
                                virtualPos = new Vector3((float)v.latitude - 0.01111111111f, (float)v.longitude + 0.02222222222f - (0.0002469136f * degOffset), (float)v.altitude);

                            }
                        }
                        else
                        {
                            if (currentPos.y >= -1 + (0.0055555556f / 2)) // if more than half a degree from the western most point in coords
                            {
                                virtualPos = new Vector3((float)v.latitude - 0.01111111111f, (float)v.longitude - 0.02222222222f + (0.0002469136f * degOffset), (float)v.altitude);

                            }
                        }
                        MesoSphere = (virtualPos - currentPos).normalized;

                    }
                    else
                    {
                        // MesoSphere wind direction should be east while less than 3 heading from the poles
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
                        Debug.Log("[OrX W[ind/S]] ... Changing direction");
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
                            Debug.Log("[OrX W[ind/S]] ... Changing direction for " + v.vesselName);
                            windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                        }
                        else
                        {
                            variationCount += 1;
                            Debug.Log("[OrX W[ind/S]] ... Changing direction for " + v.vesselName);
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
                if (randomDirection <= 11) // if random direction is below 9
                {
                    float angle = Vector3.Angle(windDirection, originalWindDirection);

                    if (angle <= windVariability / 100)
                    {
                        Debug.Log("[OrX W[ind/S] ... Changing direction for " + v.vesselName);
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
                        Debug.Log("[OrX W[ind/S] ... Changing direction for " + v.vesselName);
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

            if (random360)
            {
                heading = Vector3.Angle(windDirection, NorthVect);
            }

            if (Math.Sign(Vector3.Dot(windDirection, EastVect)) < 0)
            {
                heading = 360 - heading;
            }
            string direction = "";

            if (heading >= 349 && heading < 11) // 0
            {
                direction = "- N -";
            }
            else
            {
                if (heading >= 11 && heading < 34) // 22.5
                {
                    direction = "- NNE -";
                }
                else
                {
                    if (heading >= 35 && heading < 57) // 45
                    {
                        direction = "- NE -";
                    }
                    else
                    {
                        if (heading >= 57 && heading < 79) // 47.5
                        {
                            direction = "- ENE -";
                        }
                        else
                        {
                            if (heading >= 80 && heading < 100) // 90
                            {
                                direction = "- E -";
                            }
                            else
                            {
                                if (heading >= 100 && heading < 122) // 112.5
                                {
                                    direction = "- ESE -";
                                }
                                else
                                {
                                    if (heading >= 122 && heading < 146) // 135
                                    {
                                        direction = "- SE -";
                                    }
                                    else
                                    {
                                        if (heading >= 146 && heading < 169) // 157.5
                                        {
                                            direction = "- SSE -";
                                        }
                                        else
                                        {
                                            if (heading >= 169 && heading < 191) // 180
                                            {
                                                direction = "- S -";
                                            }
                                            else
                                            {
                                                if (heading >= 191 && heading < 214) // 202.5
                                                {
                                                    direction = "- SSW -";
                                                }
                                                else
                                                {
                                                    if (heading >= 214 && heading < 236) // 225
                                                    {
                                                        direction = "- SW -";
                                                    }
                                                    else
                                                    {
                                                        if (heading >= 236 && heading < 259) // 247.5
                                                        {
                                                            direction = "- WSW -";
                                                        }
                                                        else
                                                        {
                                                            if (heading >= 259 && heading < 281) // 270
                                                            {
                                                                direction = "- W -";
                                                            }
                                                            else
                                                            {
                                                                if (heading >= 281 && heading < 303) // 292.5
                                                                {
                                                                    direction = "- WNW -";
                                                                }
                                                                else
                                                                {
                                                                    if (heading >= 303 && heading < 326) // 315
                                                                    {
                                                                        direction = "- NW -";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (heading >= 326 && heading < 349) // 315
                                                                        {
                                                                            direction = "- NNW -";
                                                                        }
                                                                    }


                                                                }

                                                            }

                                                        }

                                                    }

                                                }

                                            }

                                        }

                                    }

                                }

                            }

                        }


                    }

                }

            }

            WindDirectionIndicator.instance.direction = direction;
            WindDirectionIndicator.instance.degrees = heading;
            WindDirectionIndicator.instance.speed = _wi;

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


        Vector3 NorthWesterlies;
        Vector3 SouthWesterlies;
        Vector3 NorthTrades;
        Vector3 SouthTrades;
        Vector3 GeneralWindDirection;
        Vector3 currentPos;
        Vector3 MesoSphere;
        Vector3 TropoSphere;

        float degOffset = 0;

        /// /////////////////////////////////////////////////////////////////////////////

        #region Wind GUI

        private void GuiWindow(int Wind)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            Draw360Random(line);

            if (!random360)
            {
                line++;
                DrawEnableWind(line);
                line++;
                DrawHeadingText(line);
                line++;
                DrawHeadingSlider(line);
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
                DrawWindSetDirection(line);
            }

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
            //OrX.OrXHoloKron.instance.ScreenMsg("W[ind/S] is currently unavailable ..... Please check back next update");

            
            UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            NorthWesterlies = ((NorthVect - EastVect).normalized - EastVect).normalized;
            SouthWesterlies = ((-NorthVect - EastVect).normalized - EastVect).normalized;
            NorthTrades = ((NorthVect - (-EastVect)).normalized - (-EastVect)).normalized;
            SouthTrades = ((-NorthVect - (-EastVect)).normalized - (-EastVect)).normalized;

            GuiEnabled = true;
            guiOpen = true;
            Debug.Log("[Wind]: Showing GUI");
            
        }
        public void DisableGui()
        {
            //OrX.OrXHoloKron.instance.ScreenMsg("W[ind/S] is currently unavailable ..... Please check back next update");
           
            guiOpen = false;
            GuiEnabled = false;
            Debug.Log("[Wind]: Hiding GUI");
            
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
            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX W[ind/S]", titleStyle);
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "W[ind/S] Intensity", titleStyle);
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "W[ind/S] Variability", titleStyle);
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
                if (GUI.Button(saveRect, "W[ind/S] Weather Sim", HighLogic.Skin.button))
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Simulated weather is experimental", 5, ScreenMessageStyle.UPPER_CENTER));
                    random360 = true;
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Tease/Blow Timer", titleStyle);
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
                if (GUI.Button(saveRect, "Enable W[ind/S]", HighLogic.Skin.button))
                {
                    ToggleWind();
                }
            }
            else
            {
                if (GUI.Button(saveRect, "W[ind/S] Enabled", HighLogic.Skin.box))
                {
                    ToggleWind();
                }
            }
        }
        private void DrawHeadingText(float line)
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
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "W[ind/S] Heading: " + heading, titleStyle);
        }
        private void DrawHeadingSlider(float line)
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
            GUI.Label(new Rect(105, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|", Style);
            GUI.Label(new Rect(176, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "359", Style);
            _headingSlider = GUI.HorizontalSlider(saveRect, _headingSlider, 0, 359);
        }
        private void DrawWindSetDirection(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!setDirection)
            {
                if (GUI.Button(saveRect, "Update W[ind/S] Settings", HighLogic.Skin.button))
                {
                    setDirection = true;
                    manual = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Updating W[ind/S]", HighLogic.Skin.box))
                {
                    if (enableWind)
                    {

                    }
                    else
                    {
                        setDirection = true;
                        manual = true;
                        enableWind = true;
                    }
                }
            }
        }

        #endregion

        /// /////////////////////////////////////////////////////////////////////////////

        private void Dummy() { }


    }
}