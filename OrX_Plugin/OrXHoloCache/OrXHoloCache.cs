using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;
using OrX.spawn;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXHoloCache : MonoBehaviour
    {
        public enum OrXCoords
        {
            Kerbol,
            Moho,
            Eve,
            Gilly,
            Kerbin,
            Mun,
            Minmus,
            Duna,
            Ike,
            Dres,
            Jool,
            Laythe,
            Vall,
            Tylo,
            Bop,
            Pol,
            Eeloo,
            All,
            None
        }

        #region GUI Styles

        //gui styles
        GUIStyle centerLabel;
        GUIStyle centerLabelRed;
        GUIStyle centerLabelOrange;
        GUIStyle centerLabelBlue;
        GUIStyle leftLabel;
        GUIStyle leftLabelRed;
        GUIStyle rightLabelRed;
        GUIStyle leftLabelGray;
        GUIStyle rippleSliderStyle;
        GUIStyle rippleThumbStyle;
        GUIStyle kspTitleLabel;
        GUIStyle middleLeftLabel;
        GUIStyle middleLeftLabelOrange;
        GUIStyle targetModeStyle;
        GUIStyle targetModeStyleSelected;
        GUIStyle redErrorStyle;
        GUIStyle redErrorShadowStyle;

        #endregion

        private bool unlocked = false;

        public static bool showTargets = true;
        public static Rect WindowRectToolbar;
        public static Rect WindowRectHCGUI;
        public static OrXHoloCache instance;
        //public static bool GAME_UI_ENABLED;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        public static bool hasAddedButton = false;
        public static bool OrXHCGUIEnabled;

        private bool scanning = false;
        public bool spawnHoloCache = false;

        float toolWindowWidth = 250;
        float toolWindowHeight = 100;
        bool showWindowHCGUI = true;
        bool maySavethisInstance = false;

        private int count = 0;

        float HCGUIHeight;
        public bool reload;
        private string HoloCacheName = string.Empty;

        private double lat = 0;
        private double lon = 0;
        private double alt = 0;

        public float minLoadRange = 2000;

        float HCGUIEntryCount;
        float HCGUIEntryHeight = 24;
        float HCGUIBorder = 5;
        bool TargetHCGUI;
        int TargetHCGUIIndex;
        bool resetTargetHCGUI;
        string newHCGUIName = string.Empty;
        bool validHCGUIName = true;
        public OrXHoloCacheinfo designatedHCGUIInfo;
        Vessel OrXHCGUICoords;
        private string soi = "";
        Guid vid;

        private static Vector2 _displayViewerPosition = Vector2.zero;
        public Vector3d designatedHCGUICoords => designatedHCGUIInfo.gpsCoordinates;

        Vector3 worldPos;
        Vector3d SpawnCoords;

        private Texture2D redDot;
        public Texture2D HoloTargetTexture
        {
            get { return redDot ? redDot : redDot = GameDatabase.Instance.GetTexture("OrX/Plugin/HoloTarget", false); }
        }

        public bool sth = true;
        public bool hide = false;
        CelestialBody SOIcurrent;
        DestroyOnSceneSwitch ds;

        public void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        void Start()
        {
            OrXHCGUIEnabled = false;
            AddToolbarButton();
            TargetHCGUI = true;
            spawnHoloCache = false;
            scanning = false;
            
            if (HighLogic.LoadedSceneIsFlight)
                maySavethisInstance = true;     //otherwise later we should NOT save the current window positions!

            // window position settings
            WindowRectToolbar = new Rect((Screen.width / 16) * 2.5f, 140, toolWindowWidth, toolWindowHeight);
            // Default, if not in file.
            WindowRectHCGUI = new Rect(0, 0, WindowRectToolbar.width - 10, 0);

            WindowRectHCGUI.width = WindowRectToolbar.width - 10;

            //setup gui styles
            centerLabel = new GUIStyle();
            centerLabel.alignment = TextAnchor.UpperCenter;
            centerLabel.normal.textColor = Color.white;

            centerLabelRed = new GUIStyle();
            centerLabelRed.alignment = TextAnchor.UpperCenter;
            centerLabelRed.normal.textColor = Color.red;

            centerLabelOrange = new GUIStyle();
            centerLabelOrange.alignment = TextAnchor.UpperCenter;
            centerLabelOrange.normal.textColor = XKCDColors.BloodOrange;

            centerLabelBlue = new GUIStyle();
            centerLabelBlue.alignment = TextAnchor.UpperCenter;
            centerLabelBlue.normal.textColor = XKCDColors.AquaBlue;

            leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            middleLeftLabel = new GUIStyle(leftLabel);
            middleLeftLabel.alignment = TextAnchor.MiddleLeft;

            middleLeftLabelOrange = new GUIStyle(middleLeftLabel);
            middleLeftLabelOrange.normal.textColor = XKCDColors.BloodOrange;

            targetModeStyle = new GUIStyle();
            targetModeStyle.alignment = TextAnchor.MiddleRight;
            targetModeStyle.fontSize = 9;
            targetModeStyle.normal.textColor = Color.white;

            targetModeStyleSelected = new GUIStyle(targetModeStyle);
            targetModeStyleSelected.normal.textColor = XKCDColors.BloodOrange;

            leftLabelRed = new GUIStyle();
            leftLabelRed.alignment = TextAnchor.UpperLeft;
            leftLabelRed.normal.textColor = Color.red;

            rightLabelRed = new GUIStyle();
            rightLabelRed.alignment = TextAnchor.UpperRight;
            rightLabelRed.normal.textColor = Color.red;

            leftLabelGray = new GUIStyle();
            leftLabelGray.alignment = TextAnchor.UpperLeft;
            leftLabelGray.normal.textColor = Color.gray;

            rippleSliderStyle = new GUIStyle(OrXGUISkin.horizontalSlider);
            rippleThumbStyle = new GUIStyle(OrXGUISkin.horizontalSliderThumb);
            rippleSliderStyle.fixedHeight = rippleThumbStyle.fixedHeight = 0;

            kspTitleLabel = new GUIStyle();
            kspTitleLabel.normal.textColor = OrXGUISkin.window.normal.textColor;
            kspTitleLabel.font = OrXGUISkin.window.font;
            kspTitleLabel.fontSize = OrXGUISkin.window.fontSize;
            kspTitleLabel.fontStyle = OrXGUISkin.window.fontStyle;
            kspTitleLabel.alignment = TextAnchor.UpperCenter;

            redErrorStyle = new GUIStyle(OrXGUISkin.label);
            redErrorStyle.normal.textColor = Color.red;
            redErrorStyle.fontStyle = FontStyle.Bold;
            redErrorStyle.fontSize = 22;
            redErrorStyle.alignment = TextAnchor.UpperCenter;

            redErrorShadowStyle = new GUIStyle(redErrorStyle);
            redErrorShadowStyle.normal.textColor = new Color(0, 0, 0, 0.75f);
            GameEvents.onHideUI.Add(HideGameUI);
            GameEvents.onShowUI.Add(ShowGameUI);
        }

        private bool paused = false;

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (PauseMenu.isOpen)
                {
                    paused = true;
                    OrXHCGUIEnabled = false;
                }
                else
                {
                    paused = false;
                }

                if (TargetHCGUI && scanning)
                {
                    DrawTextureOnWorldPos(worldPos, instance.HoloTargetTexture, new Vector2(8, 8));
                }

                if (OrXHCGUIEnabled)
                {
                    WindowRectToolbar = GUI.Window(265227765, WindowRectToolbar, OrXHCGUI, "HoloCache Locations", OrXGUISkin.window);
                    UseMouseEventInRect(WindowRectToolbar);
                }
            }
            else
            {
                if (!OrXHCGUIEnabled) return;
                WindowRectToolbar = GUI.Window(266227765, WindowRectToolbar, OrXHCGUI, "HoloCache Locations", OrXGUISkin.window);
                UseMouseEventInRect(WindowRectToolbar);
            }
        }

        private int delay = 300;

        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!passive && !scanning)
                {
                    passive = true;
                    delay = 300;
                    StartCoroutine(PassiveCheck());
                }
            }
        }

        public static void UseMouseEventInRect(Rect rect)
        {
            if (MouseIsInRect(rect) && Event.current.isMouse && (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp))
            {
                Event.current.Use();
            }
        }

        public static bool MouseIsInRect(Rect rect)
        {
            Vector3 inverseMousePos = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0);
            return rect.Contains(inverseMousePos);
        }

        private bool reloadWorldPos = false;
        private string targetLabel;

        OrXCoords coords;

        private bool passive = false;

        IEnumerator PassiveCheck()
        {
            CheckSOI();
            yield return new WaitForSeconds(delay);
            passive = false;
        }

        public void CheckSOI()
        {
            var SOIcheck = FlightGlobals.ActiveVessel.orbitDriver.orbit.referenceBody;
            if (SOIcheck != SOIcurrent)
            {
                SOIcurrent = FlightGlobals.ActiveVessel.orbitDriver.orbit.referenceBody;

                if (FlightGlobals.ActiveVessel.mainBody.name == "Bop")
                {
                    coords = OrXCoords.Bop;
                    soi = "Bop";
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.mainBody.name == "Dres")
                    {
                        coords = OrXCoords.Dres;
                        soi = "Dres";
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.mainBody.name == "Duna")
                        {
                            soi = "Duna";
                            coords = OrXCoords.Duna;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.mainBody.name == "Eeloo")
                            {
                                soi = "Eeloo";
                                coords = OrXCoords.Eeloo;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.mainBody.name == "Eve")
                                {
                                    soi = "Eve";
                                    coords = OrXCoords.Eve;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Gilly")
                                    {
                                        soi = "Gilly";
                                        coords = OrXCoords.Gilly;
                                    }
                                    else
                                    {
                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Ike")
                                        {
                                            soi = "Ike";
                                            coords = OrXCoords.Ike;
                                        }
                                        else
                                        {
                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Jool")
                                            {
                                                soi = "Jool";
                                                coords = OrXCoords.Jool;
                                            }
                                            else
                                            {
                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbin")
                                                {
                                                    soi = "Kerbin";
                                                    coords = OrXCoords.Kerbin;
                                                }
                                                else
                                                {
                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbol")
                                                    {
                                                        soi = "Kerbol";
                                                        coords = OrXCoords.Kerbol;
                                                    }
                                                    else
                                                    {
                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Laythe")
                                                        {
                                                            soi = "Laythe";
                                                            coords = OrXCoords.Laythe;
                                                        }
                                                        else
                                                        {
                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Minmus")
                                                            {
                                                                soi = "Minmus";
                                                                coords = OrXCoords.Minmus;
                                                            }
                                                            else
                                                            {
                                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Moho")
                                                                {
                                                                    soi = "Moho";
                                                                    coords = OrXCoords.Moho;
                                                                }
                                                                else
                                                                {
                                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Mun")
                                                                    {
                                                                        soi = "Mun";
                                                                        coords = OrXCoords.Mun;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Pol")
                                                                        {
                                                                            soi = "Pol";
                                                                            coords = OrXCoords.Pol;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Tylo")
                                                                            {
                                                                                soi = "Tylo";
                                                                                coords = OrXCoords.Tylo;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Vall")
                                                                                {
                                                                                    soi = "Vall";
                                                                                    coords = OrXCoords.Vall;
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
                }
            }
        }

        #region Target Location Spawning

        private float timer = 1;
        public string craftFile = string.Empty;

        private Vector3d _SpawnCoords()
        {
            return FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)lat, (double)lon, (double)alt);
        }

        public static Camera GetMainCamera()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                return FlightCamera.fetch.mainCamera;
            }
            else
            {
                return Camera.main;
            }
        }

        public static string FormattedGeoPosShort(Vector3d geoPos, bool altitude)
        {
            string finalString = string.Empty;
            //lat
            double lat = geoPos.x;
            double latSign = Math.Sign(lat);
            double latMajor = latSign * Math.Floor(Math.Abs(lat));
            double latMinor = 100 * (Math.Abs(lat) - Math.Abs(latMajor));
            string latString = latMajor.ToString("0") + " " + latMinor.ToString("0");
            finalString += "N:" + latString;


            //longi
            double longi = geoPos.y;
            double longiSign = Math.Sign(longi);
            double longiMajor = longiSign * Math.Floor(Math.Abs(longi));
            double longiMinor = 100 * (Math.Abs(longi) - Math.Abs(longiMajor));
            string longiString = longiMajor.ToString("0") + " " + longiMinor.ToString("0");
            finalString += " E:" + longiString;

            if (altitude)
            {
                finalString += " ASL:" + geoPos.z.ToString("0");
            }

            return finalString;
        }

        public static void DrawTextureOnWorldPos(Vector3 worldPos, Texture texture, Vector2 size)
        {
            Vector3 screenPos = GetMainCamera().WorldToViewportPoint(worldPos);
            if (screenPos.z < 0) return; //dont draw if point is behind camera
            if (screenPos.x != Mathf.Clamp01(screenPos.x)) return; //dont draw if off screen
            if (screenPos.y != Mathf.Clamp01(screenPos.y)) return;
            float xPos = screenPos.x * Screen.width - (0.5f * size.x);
            float yPos = (1 - screenPos.y) * Screen.height - (0.5f * size.y);
            Rect iconRect = new Rect(xPos, yPos, size.x, size.y);
            GUI.DrawTexture(iconRect, texture);
        }

        public bool checking = false;
        private double targetDistance = 0;

        private void TargetDistance()
        {
            checking = true;

            SpawnCoords = _SpawnCoords();

            if (scanning)
            {
                StartCoroutine(CheckTargetDistance());
            }
        }

        IEnumerator CheckTargetDistance()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (checking)
                {
                    reloadWorldPos = true;
                    targetDistance = Vector3d.Distance(FlightGlobals.ActiveVessel.GetWorldPos3D(), SpawnCoords);

                    if (targetDistance <= minLoadRange)
                    {
                        scanning = false;
                        checking = false;
                        worldPos = Vector3d.zero;
                        HideGameUI();
                        StartCoroutine(HoloSpawn());
                    }
                    else
                    {
                        yield return new WaitForSeconds(timer);
                        StartCoroutine(CheckTargetDistance());
                    }
                }
            }
        }
         
        IEnumerator HoloSpawn()
        {
            if (!spawnHoloCache)
            {
                spawnHoloCache = true;
                yield return new WaitForFixedUpdate();
                ConfigNode node = null;
                ConfigNode fileNode = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                Debug.Log("[OrX Holocache] === Data Loaded ===");
                bool spawned = true;
                int hcCount = 0;
                Debug.Log("[OrX Holocache] === Checking if HoloCache #" + hcCount + " has already spawned ===");
                bool empty = false;

                if (fileNode != null && fileNode.HasNode("OrX"))
                {
                    node = fileNode.GetNode("OrX");

                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (spawned)
                        {
                            if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                            {
                                Debug.Log("[OrX Target Manager] === FOUND HOLOCACHE === " + hcCount); ;
                                foreach (ConfigNode.Value cv in spawnCheck.values)
                                {
                                    if (cv.name == "spawned")
                                    {
                                        if (cv.value == "False")
                                        {
                                            Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has not spawned ... SPAWNING"); ;
                                            cv.value = "True";
                                            fileNode.Save("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                                            spawned = false;
                                            break;
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has spawned ... CHECKING FOR EXTRAS"); ;

                                            if (spawnCheck.HasValue("extras"))
                                            {
                                                var t = spawnCheck.GetValue("extras");
                                                if (t == "False")
                                                {
                                                    Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION"); ;
                                                    spawned = false;
                                                    hcCount += 1;
                                                    break;
                                                }
                                                else
                                                {
                                                    Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;

                                                    spawned = true;
                                                    hcCount += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("[OrX Holocache] === HoloCache List is empty ===");
                    empty = true;
                }

                Debug.Log("[OrX Holocache] === SEARCHING FOR HOLOCACHE #" + hcCount + " ===");

                fileNode = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                node = fileNode.GetNode("OrX");

                ConfigNode hc = node.GetNode("HoloCache" + hcCount);

                if (hc == null || empty)
                {
                    Debug.Log("[OrX Holocache] === ERROR === HoloCache #" + hcCount + " doesn't exist ===");
                }
                else
                {
                    Debug.Log("[OrX Holocache] === FOUND HOLOCACHE #" + hcCount + " ... LOADING ===");

                    foreach (ConfigNode.Value cv in hc.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in hc.nodes)
                    {
                        foreach (ConfigNode.Value cv in cn.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        foreach (ConfigNode cn2 in cn.nodes)
                        {
                            foreach (ConfigNode.Value cv2 in cn2.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                cv2.name = cvEncryptedName;
                                cv2.value = cvEncryptedValue;
                            }
                        }
                    }

                    hc.Save("GameData/OrX/Holocache/" + HoloCacheName + "/" + HoloCacheName + ".craft");
                    yield return new WaitForFixedUpdate();
                    SpawnOrX_HoloCache.instance.holo = false;
                    SpawnOrX_HoloCache.instance.craftFile = craftFile;
                    SpawnOrX_HoloCache.instance.SpawnCoords = SpawnCoords;
                    SpawnOrX_HoloCache.instance.CheckSpawnTimer();
                    spawnHoloCache = false;
                }
            }
        }

        #endregion

        #region GUI

        private void AddToolbarButton()
        {
            string OrXDir = "OrX/Plugin/";

            if (!hasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(OrXDir + "OrX_HoloCache", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.VAB | 
                    ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH |
                    ApplicationLauncher.AppScenes.TRACKSTATION, buttonTexture);
                hasAddedButton = true;
            }
        }

        private bool pauseCheck = false;

        private void ToggleGUI()
        {
            if (OrXHCGUIEnabled)
            {
                if (!paused)
                {
                    pauseCheck = false;
                    HideGameUI();
                }
                else
                {
                    pauseCheck = true;
                    HideGameUI();
                }

            }
            else
            {
                if (!pauseCheck)
                {
                    paused = false;
                    ShowGameUI();
                }
                else
                {
                    pauseCheck = false;
                    paused = false;
                    //HideGameUI();
                }
            }
        }

        private void Dummy() { }

        public void HideGameUI()
        {
            // OrXTargetManager.instance.ClearDatabase();
            OrXHCGUIEnabled = false;
        }

        void ShowGameUI()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                vid = FlightGlobals.ActiveVessel.id;
            }
            OrXHCGUIEnabled = true;
            //OrXTargetManager.instance.LoadHoloCacheTargets();
        }

        void OrXHCGUI(int OrX_HCGUI)
        {
            GUI.DragWindow(new Rect(30, 0, toolWindowWidth - 90, 30));

            float line = 0;
            float leftIndent = 10;
            float contentWidth = toolWindowWidth - leftIndent;
            float contentTop = 10;
            float entryHeight = 20;
            float HCGUILines = 0;

            line += 0.6f;

            if (!reload)
            {
                GUI.BeginGroup(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth, WindowRectHCGUI.height));
                WindowHCGUI();
                GUI.EndGroup();

                if (!hide)
                {
                    HCGUILines = WindowRectHCGUI.height / entryHeight;

                    HCGUIHeight = Mathf.Lerp(HCGUIHeight, HCGUILines, 0.15f);
                    line += HCGUIHeight;

                    line += 0.25f;

                    if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Load HoloCache Data", OrXGUISkin.button))
                    {
                        TargetHCGUI = false;
                        reload = true;
                        ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                        OrXTargetManager.instance.ClearDatabase();
                        OrXTargetManager.instance.LoadHoloCacheTargets();
                    }
                }
            }
            else
            {
                if (!hide)
                {
                    if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "HoloCache Data Loading", OrXGUISkin.box))
                    {
                        // do nothing ... reload will turn false after OrXHoloCache is finished loading targets
                    }
                }
            }

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!hide)
                {
                    line += 1.25f;

                    if (!spawnHoloCache)
                    {
                        if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Create HoloCache", OrXGUISkin.button))
                        {
                            if(FlightGlobals.ActiveVessel.LandedOrSplashed)
                            {
                                spawnHoloCache = true;
                                ScreenMsg("<color=#cc4500ff><b>Creating HoloCache</b></color>");
                                SpawnOrX_HoloCache.instance.HoloCacheName = "";
                                SpawnOrX_HoloCache.instance.emptyholo = true;
                                SpawnOrX_HoloCache.instance.SpawnEmptyHoloCache();
                                HideGameUI();
                            }
                            else
                            {
                                ScreenMsg("<color=#cc4500ff><b>You must be Landed or Splashed</b></color>");
                                ScreenMsg("<color=#cc4500ff><b>to create a HoloCache</b></color>");
                            }
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Creating HoloCache", OrXGUISkin.box))
                        {
                            // do nothing ... spawnHoloCache will turn false after OrXHoloCache is finished spawning empty HoloCache
                        }
                    }
                    line += 1F;
                }
            }
            else
            {

            }

            toolWindowHeight = Mathf.Lerp(toolWindowHeight, contentTop + (line*entryHeight) + 5, 1);
            WindowRectToolbar.height = toolWindowHeight;
        }

        public void WindowHCGUI()
        {
            GUI.Box(WindowRectHCGUI, GUIContent.none, OrXGUISkin.box);
            HCGUIEntryCount = 0;
            Rect listRect = new Rect(HCGUIBorder, HCGUIBorder, WindowRectHCGUI.width - (2 * HCGUIBorder),
                WindowRectHCGUI.height - (2 * HCGUIBorder));
            GUI.BeginGroup(listRect);

            if (HighLogic.LoadedSceneIsFlight)
            {
                targetLabel = "SOI: " + FlightGlobals.ActiveVessel.orbitDriver.orbit.referenceBody;
            }
            else
            {
                targetLabel = "Holocaches";
            }

            GUI.Label(new Rect(0, 0, listRect.width, HCGUIEntryHeight), targetLabel, kspTitleLabel);

            // Expand/Collapse Target Toggle button
            if (GUI.Button(new Rect(listRect.width - (HCGUIEntryHeight * 2), 0, HCGUIEntryHeight, HCGUIEntryHeight), showTargets ? "-" : "+", OrXGUISkin.button))
                showTargets = !showTargets;

            HCGUIEntryCount += 1.2f;
            int indexToRemove = -1;
            int index = 0;

            if (HighLogic.LoadedSceneIsEditor)
            {
                if (showTargets)
                {
                    Color origWColor = GUI.color;
                    List<OrXHoloCacheinfo>.Enumerator coordinate = OrXTargetManager.HoloCacheTargets[coords].GetEnumerator();
                    while (coordinate.MoveNext())
                    {
                        string label = FormattedGeoPosShort(coordinate.Current.gpsCoordinates, false);
                        float nameWidth = 120;
                        if (scanning)
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.worldPos;
                                    }

                                    if (!reload && !hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                        {
                                            scanning = false;
                                            checking = false;
                                            worldPos = Vector3d.zero;
                                            TargetHCGUI = false;
                                            reload = true;
                                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Description</b></color>");




                                            OrXTargetManager.instance.LoadHoloCacheTargets();
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                        {
                                            scanning = false;
                                            checking = false;
                                            //worldPos = Vector3d.zero;
                                            TargetHCGUI = false;
                                            reload = true;
                                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Description</b></color>");
                                            OrXTargetManager.instance.LoadHoloCacheTargets();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.worldPos;
                                    }

                                    if (!hide)
                                    {
                                        if (!reload)
                                        {
                                            if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                worldPos = Vector3d.zero;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                OrXTargetManager.instance.LoadHoloCacheTargets();
                                            }

                                            if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                worldPos = Vector3d.zero;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                OrXTargetManager.instance.LoadHoloCacheTargets();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                        {
                                            HoloCacheName = coordinate.Current.name;

                                            TargetHCGUIIndex = index;
                                            TargetHCGUI = true;
                                            resetTargetHCGUI = false;
                                            scanning = true;
                                            craftFile = coordinate.Current.name;
                                            // LOAD HOLOCACHE DESCRIPTION WINDOW
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                        {
                                            HoloCacheName = coordinate.Current.name;

                                            TargetHCGUIIndex = index;
                                            TargetHCGUI = true;
                                            resetTargetHCGUI = false;
                                            scanning = true;
                                            craftFile = coordinate.Current.name;
                                            // LOAD HOLOCACHE DESCRIPTION WINDOW
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!hide)
                                {
                                    if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                    {
                                        HoloCacheName = coordinate.Current.name;

                                        TargetHCGUIIndex = index;
                                        TargetHCGUI = true;
                                        resetTargetHCGUI = false;
                                        scanning = true;
                                        craftFile = coordinate.Current.name;
                                        // LOAD HOLOCACHE DESCRIPTION WINDOW
                                    }

                                    if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                    {
                                        HoloCacheName = coordinate.Current.name;

                                        TargetHCGUIIndex = index;
                                        TargetHCGUI = true;
                                        resetTargetHCGUI = false;
                                        scanning = true;
                                        craftFile = coordinate.Current.name;
                                        // LOAD HOLOCACHE DESCRIPTION WINDOW
                                    }
                                }
                            }
                        }

                        HCGUIEntryCount++;
                        index++;
                        GUI.color = origWColor;
                    }
                    coordinate.Dispose();
                }
            }
            else
            {
                if (showTargets)
                {
                    List<OrXHoloCacheinfo>.Enumerator coordinate = OrXTargetManager.HoloCacheTargets[coords].GetEnumerator();
                    while (coordinate.MoveNext())
                    {
                        Color origWColor = GUI.color;

                        string label = FormattedGeoPosShort(coordinate.Current.gpsCoordinates, false);
                        float nameWidth = 120;
                        if (scanning)
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.worldPos;
                                    }

                                    if (!reload && !hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.DisableGui();
                                                }
                                                HoloCacheName = "";

                                                scanning = false;
                                                checking = false;
                                                worldPos = Vector3d.zero;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                OrXTargetManager.instance.LoadHoloCacheTargets();
                                            }
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.DisableGui();
                                                }
                                                HoloCacheName = "";

                                                TargetHCGUI = false;
                                                scanning = false;
                                                checking = false;
                                                worldPos = Vector3d.zero;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                OrXTargetManager.instance.LoadHoloCacheTargets();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.worldPos;
                                    }

                                    if (!hide)
                                    {
                                        if (!reload)
                                        {
                                            if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                            {
                                                if (HighLogic.LoadedSceneIsFlight)
                                                {
                                                    if (OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.DisableGui();
                                                    }
                                                    HoloCacheName = "";

                                                    scanning = false;
                                                    worldPos = Vector3d.zero;
                                                    TargetHCGUI = false;
                                                    reload = true;
                                                    ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                    OrXTargetManager.instance.LoadHoloCacheTargets();
                                                }
                                            }

                                            if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                            {
                                                if (HighLogic.LoadedSceneIsFlight)
                                                {
                                                    if (OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.DisableGui();
                                                    }
                                                    HoloCacheName = "";

                                                    TargetHCGUI = false;
                                                    scanning = false;
                                                    worldPos = Vector3d.zero;
                                                    reload = true;
                                                    ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                    OrXTargetManager.instance.LoadHoloCacheTargets();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (soi == FlightGlobals.currentMainBody.name)
                                                {
                                                    if (!OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.EnableGui();
                                                    }
                                                    HoloCacheName = coordinate.Current.name;

                                                    worldPos = coordinate.Current.worldPos;

                                                    lat = coordinate.Current.gpsCoordinates.x;
                                                    lon = coordinate.Current.gpsCoordinates.y;
                                                    alt = coordinate.Current.gpsCoordinates.z;

                                                    SpawnOrX_HoloCache.instance.HoloCacheName = coordinate.Current.name;
                                                    SpawnOrX_HoloCache.instance.craftFile = coordinate.Current.name;
                                                    SpawnOrX_HoloCache.instance._lat = lat;
                                                    SpawnOrX_HoloCache.instance._lon = lon;
                                                    SpawnOrX_HoloCache.instance._alt = alt;

                                                    TargetHCGUIIndex = index;
                                                    TargetHCGUI = true;
                                                    resetTargetHCGUI = false;
                                                    scanning = true;
                                                    craftFile = coordinate.Current.name;

                                                    if (!checking)
                                                    {
                                                        checking = true;
                                                        TargetDistance();
                                                    }
                                                }
                                                else
                                                {
                                                    ScreenMsg("You must be in the same SOI as this HoloCache");
                                                }
                                            }
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (soi == FlightGlobals.currentMainBody.name)
                                                {
                                                    if (!OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.EnableGui();
                                                    }
                                                    HoloCacheName = coordinate.Current.name;

                                                    lat = coordinate.Current.gpsCoordinates.x;
                                                    lon = coordinate.Current.gpsCoordinates.y;
                                                    alt = coordinate.Current.gpsCoordinates.z;
                                                    worldPos = coordinate.Current.worldPos;

                                                    SpawnOrX_HoloCache.instance.HoloCacheName = coordinate.Current.name;
                                                    SpawnOrX_HoloCache.instance.craftFile = coordinate.Current.name;
                                                    SpawnOrX_HoloCache.instance._lat = lat;
                                                    SpawnOrX_HoloCache.instance._lon = lon;
                                                    SpawnOrX_HoloCache.instance._alt = alt;

                                                    TargetHCGUIIndex = index;
                                                    TargetHCGUI = true;
                                                    resetTargetHCGUI = false;
                                                    scanning = true;
                                                    craftFile = coordinate.Current.name;

                                                    if (!checking)
                                                    {
                                                        checking = true;
                                                        TargetDistance();
                                                    }
                                                }
                                                else
                                                {
                                                    ScreenMsg("You must be in the same SOI as this HoloCache");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!hide)
                                {
                                    if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            if (soi == FlightGlobals.currentMainBody.name)
                                            {
                                                if (!OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.EnableGui();
                                                }
                                                HoloCacheName = coordinate.Current.name;

                                                worldPos = coordinate.Current.worldPos;

                                                lat = coordinate.Current.gpsCoordinates.x;
                                                lon = coordinate.Current.gpsCoordinates.y;
                                                alt = coordinate.Current.gpsCoordinates.z;

                                                SpawnOrX_HoloCache.instance.HoloCacheName = coordinate.Current.name;
                                                SpawnOrX_HoloCache.instance.craftFile = coordinate.Current.name;
                                                SpawnOrX_HoloCache.instance._lat = lat;
                                                SpawnOrX_HoloCache.instance._lon = lon;
                                                SpawnOrX_HoloCache.instance._alt = alt;

                                                TargetHCGUIIndex = index;
                                                TargetHCGUI = true;
                                                resetTargetHCGUI = false;
                                                scanning = true;
                                                craftFile = coordinate.Current.name;

                                                if (!checking)
                                                {
                                                    checking = true;
                                                    TargetDistance();
                                                }
                                            }
                                            else
                                            {
                                                ScreenMsg("You must be in the same SOI as this HoloCache");
                                            }
                                        }
                                    }

                                    if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            if (soi == FlightGlobals.currentMainBody.name)
                                            {
                                                if (!OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.EnableGui();
                                                }
                                                HoloCacheName = coordinate.Current.name;

                                                lat = coordinate.Current.gpsCoordinates.x;
                                                lon = coordinate.Current.gpsCoordinates.y;
                                                alt = coordinate.Current.gpsCoordinates.z;
                                                worldPos = coordinate.Current.worldPos;

                                                SpawnOrX_HoloCache.instance.HoloCacheName = coordinate.Current.name;
                                                SpawnOrX_HoloCache.instance.craftFile = coordinate.Current.name;
                                                SpawnOrX_HoloCache.instance._lat = lat;
                                                SpawnOrX_HoloCache.instance._lon = lon;
                                                SpawnOrX_HoloCache.instance._alt = alt;

                                                TargetHCGUIIndex = index;
                                                TargetHCGUI = true;
                                                resetTargetHCGUI = false;
                                                scanning = true;
                                                craftFile = coordinate.Current.name;

                                                if (!checking)
                                                {
                                                    checking = true;
                                                    TargetDistance();
                                                }
                                            }
                                            else
                                            {
                                                ScreenMsg("You must be in the same SOI as this HoloCache");
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        HCGUIEntryCount++;
                        index++;
                        GUI.color = origWColor;
                    }
                    coordinate.Dispose();
                }
            }

            if (resetTargetHCGUI)
            {
                checking = false;
                scanning = false;
                resetTargetHCGUI = false;
                TargetHCGUIIndex = 0;
            }

            GUI.EndGroup();
            WindowRectHCGUI.height = (2 * HCGUIBorder) + (HCGUIEntryCount * HCGUIEntryHeight);
        }

        #endregion

        internal void OnDestroy()
        {
            GameEvents.onHideUI.Remove(HideGameUI);
            GameEvents.onShowUI.Remove(ShowGameUI);
        }

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 5, ScreenMessageStyle.UPPER_CENTER));
        }
    }
}