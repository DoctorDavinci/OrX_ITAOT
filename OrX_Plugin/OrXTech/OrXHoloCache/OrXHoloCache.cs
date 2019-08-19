using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;
using OrX.spawn;
using System.IO;
using System.Text;

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

        public bool buildingMission = false;
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
        public string HoloCacheName = string.Empty;

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
            holoCache = null;
            resetHoloCache = false;
        }

        void Start()
        {
            //legacy targetDatabase
            TargetDatabase = new Dictionary<OrXHoloCache.OrXCoords, List<OrXTargetInfo>>();
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Bop, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Dres, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Duna, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Eeloo, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Eve, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Gilly, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Ike, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Jool, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Kerbin, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Kerbol, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Laythe, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Minmus, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Moho, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Mun, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Pol, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Tylo, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Vall, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.All, new List<OrXTargetInfo>());

            if (HoloCacheTargets == null)
            {
                HoloCacheTargets = new Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>>();
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Bop, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Dres, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Duna, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eeloo, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eve, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Gilly, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Ike, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Jool, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbin, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbol, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Laythe, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Minmus, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Moho, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Mun, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Pol, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Tylo, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Vall, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.All, new List<OrXHoloCacheinfo>());

            }

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/");

            //StartCoroutine(loadHolo());

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
            // ClearDatabase();
            OrXHCGUIEnabled = false;
        }

        void ShowGameUI()
        {
            if (!scanning)
            {
                ClearDatabase();
                LoadHoloCacheTargets();
            }

            if (HighLogic.LoadedSceneIsFlight)
            {
                vid = FlightGlobals.ActiveVessel.id;
            }
            OrXHCGUIEnabled = true;
            //LoadHoloCacheTargets();
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
                        ClearDatabase();
                        LoadHoloCacheTargets();
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
                                buildingMission = true;
                                OrXMissions.instance.StartMissionBuilder(FlightGlobals.ActiveVessel.GetTransform().position, new Guid());
                                HideGameUI();
                            }
                            else
                            {
                                //buildingMission = true;
                                //OrXMissions.instance.StartMissionBuilder(FlightGlobals.ActiveVessel.GetTransform().position, new Guid());
                                //HideGameUI();

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
                    List<OrXHoloCacheinfo>.Enumerator coordinate = HoloCacheTargets[coords].GetEnumerator();
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




                                            LoadHoloCacheTargets();
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                        {
                                            scanning = false;
                                            checking = false;
                                            //worldPos = Vector3d.zero;
                                            TargetHCGUI = false;
                                            reload = true;
                                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Description</b></color>");
                                            LoadHoloCacheTargets();
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
                                                LoadHoloCacheTargets();
                                            }

                                            if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                worldPos = Vector3d.zero;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                LoadHoloCacheTargets();
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
                    List<OrXHoloCacheinfo>.Enumerator coordinate = HoloCacheTargets[coords].GetEnumerator();
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
                                                LoadHoloCacheTargets();
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
                                                LoadHoloCacheTargets();
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
                                                    LoadHoloCacheTargets();
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
                                                    LoadHoloCacheTargets();
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

        public static Dictionary<OrXHoloCache.OrXCoords, List<OrXTargetInfo>> TargetDatabase;
        public static Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>> HoloCacheTargets;

        public bool resetHoloCache = false;
        public ConfigNode craft = null;
        public string shipDescription = string.Empty;

        private StringBuilder debugString = new StringBuilder();
        public string craftToSpawn = string.Empty;
        public string cfgToLoad = string.Empty;
        string OrXv = "OrXv";

        private float updateTimer = 0;

        public Vessel holoCache;

        public double _lat = 0f;
        public double _lon = 0f;
        public double _alt = 0f;

        void OnDestroy()
        {
            GameEvents.onHideUI.Remove(HideGameUI);
            GameEvents.onShowUI.Remove(ShowGameUI);

            HoloCacheTargets = new Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>>();
            TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Bop);
            TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Dres);
            TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Duna);
            TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eeloo);
            TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eve);
            TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Gilly);
            TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Ike);
            TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Jool);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbin);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbol);
            TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Laythe);
            TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Minmus);
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Moho);
            TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Mun);
            TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Pol);
            TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Tylo);
            TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Vall);
            TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.All);

        }

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 5, ScreenMessageStyle.UPPER_CENTER));
        }

        IEnumerator loadHolo()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                LoadHoloCacheTargets();
            }
            else
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(loadHolo());
            }
        }

        public void LoadHoloCacheTargets()
        {
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/");

            if (HoloCacheTargets == null)
            {
                HoloCacheTargets = new Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>>();
            }
            HoloCacheTargets.Clear();
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Bop, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Dres, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Duna, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eeloo, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eve, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Gilly, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Ike, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Jool, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbin, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbol, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Laythe, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Minmus, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Moho, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Mun, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Pol, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Tylo, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Vall, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.All, new List<OrXHoloCacheinfo>());

            string soi = FlightGlobals.currentMainBody.name;
            string holoCacheLoc = UrlDir.ApplicationRootPath + "GameData/";
            var files = new List<string>(Directory.GetFiles(holoCacheLoc, "*.orx", SearchOption.AllDirectories));
            bool spawned = true;
            bool extras = false;
            int hcCount = 0;

            if (files != null)
            {
                List<string>.Enumerator cfgsToAdd = files.GetEnumerator();
                while (cfgsToAdd.MoveNext())
                {
                    try
                    {
                        ConfigNode fileNode = ConfigNode.Load(cfgsToAdd.Current);

                        if (fileNode != null && fileNode.HasNode("OrX"))
                        {
                            ConfigNode node = fileNode.GetNode("OrX");

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
                                                    Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has not spawned ... "); ;

                                                    spawned = false;
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
                                                            extras = false;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;

                                                            extras = true;
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

                            foreach (ConfigNode HoloCacheNode in node.GetNodes("OrXHoloCacheCoords" + hcCount))
                            {
                                if (HoloCacheNode.HasValue("SOI"))
                                {
                                    if (HoloCacheNode.HasValue("Targets"))
                                    {
                                        string targetString = HoloCacheNode.GetValue("Targets");
                                        if (targetString == string.Empty)
                                        {
                                            Debug.Log("[OrX HoloCache] OrX HoloCache Target string was empty!");
                                            return;
                                        }
                                        StringToHoloCacheList(targetString);
                                        Debug.Log("[OrX HoloCache] Loaded OrX HoloCache Targets");
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX HoloCache] No OrX HoloCache Targets value found!");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[OrX Target Manager] HoloCache Targets Out Of Range ...... Continuing");
                    }
                }
                cfgsToAdd.Dispose();
            }
            else
            {
                Debug.Log("[OrX Target Manager] === HoloCache List is empty ===");
            }
            OrXHoloCache.instance.reload = false;
        }

        private void StringToHoloCacheList(string listString)
        {
            if (FlightGlobals.ActiveVessel.mainBody.name == "Bop")
            {
                coords = OrXHoloCache.OrXCoords.Bop;
            }
            else
            {
                if (FlightGlobals.ActiveVessel.mainBody.name == "Dres")
                {
                    coords = OrXHoloCache.OrXCoords.Dres;
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.mainBody.name == "Duna")
                    {
                        coords = OrXHoloCache.OrXCoords.Duna;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.mainBody.name == "Eeloo")
                        {
                            coords = OrXHoloCache.OrXCoords.Eeloo;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.mainBody.name == "Eve")
                            {
                                coords = OrXHoloCache.OrXCoords.Eve;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.mainBody.name == "Gilly")
                                {
                                    coords = OrXHoloCache.OrXCoords.Gilly;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Ike")
                                    {
                                        coords = OrXHoloCache.OrXCoords.Ike;
                                    }
                                    else
                                    {
                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Jool")
                                        {
                                            coords = OrXHoloCache.OrXCoords.Jool;
                                        }
                                        else
                                        {
                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbin")
                                            {
                                                coords = OrXHoloCache.OrXCoords.Kerbin;
                                            }
                                            else
                                            {
                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbol")
                                                {
                                                    coords = OrXHoloCache.OrXCoords.Kerbol;
                                                }
                                                else
                                                {
                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Laythe")
                                                    {
                                                        coords = OrXHoloCache.OrXCoords.Laythe;
                                                    }
                                                    else
                                                    {
                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Minmus")
                                                        {
                                                            coords = OrXHoloCache.OrXCoords.Minmus;
                                                        }
                                                        else
                                                        {
                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Moho")
                                                            {
                                                                coords = OrXHoloCache.OrXCoords.Moho;
                                                            }
                                                            else
                                                            {
                                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Mun")
                                                                {
                                                                    coords = OrXHoloCache.OrXCoords.Mun;
                                                                }
                                                                else
                                                                {
                                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Pol")
                                                                    {
                                                                        coords = OrXHoloCache.OrXCoords.Pol;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Tylo")
                                                                        {
                                                                            coords = OrXHoloCache.OrXCoords.Tylo;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Vall")
                                                                            {
                                                                                coords = OrXHoloCache.OrXCoords.Vall;
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

            if (listString == null || listString == string.Empty)
            {
                Debug.Log("[OrX Target Manager] === HoloCache List string was empty or null ===");
                return;
            }

            string[] OrbitalBodyNames = listString.Split(new char[] { ':' });

            Debug.Log("[OrX Target Manager] Loading HoloCache Targets ..........");

            try
            {
                if (OrbitalBodyNames[0] != null && OrbitalBodyNames[0].Length > 0 && OrbitalBodyNames[0] != "null")
                {
                    string[] OrbitalBodyNameACoords = OrbitalBodyNames[0].Split(new char[] { ';' });
                    for (int i = 0; i < OrbitalBodyNameACoords.Length; i++)
                    {
                        if (OrbitalBodyNameACoords[i] != null && OrbitalBodyNameACoords[i].Length > 0)
                        {
                            string[] data = OrbitalBodyNameACoords[i].Split(new char[] { ',' });
                            string name = data[0];
                            craftToSpawn = data[1];
                            _sth = data[1];
                            double lat = double.Parse(data[3]);
                            double longi = double.Parse(data[4]);
                            double alt = double.Parse(data[5]);
                            OrXHoloCacheinfo newInfo = new OrXHoloCacheinfo(new Vector3d(lat, longi, alt), craftToSpawn);
                            HoloCacheTargets[coords].Add(newInfo);
                            HoloCacheTargets[OrXHoloCache.OrXCoords.All].Add(newInfo);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log("[OrX Target Manager] HoloCache config file processed ...... ");
            }
        }

        string _sth = string.Empty;

        IEnumerator CleanDatabaseRoutine()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(5);

                TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Bop);
                TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Dres);
                TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Duna);
                TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eeloo);
                TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eve);
                TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Gilly);
                TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Ike);
                TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Jool);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbin);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbol);
                TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Laythe);
                TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Minmus);
                TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Moho);
                TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Mun);
                TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Pol);
                TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Tylo);
                TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Vall);
                TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.All);

            }
        }

        public void RemoveTarget(OrXTargetInfo target, OrXHoloCache.OrXCoords OrbitalBodyName)
        {
            TargetDatabase[OrbitalBodyName].Remove(target);

        }

        public void Vreport(Vessel v)
        {
            ReportVessel(v);
        }

        public static void ReportVessel(Vessel v)
        {
            if (!v) return;

            OrXTargetInfo info = v.gameObject.GetComponent<OrXTargetInfo>();
            if (!info)
            {
                List<ModuleOrXHoloCache>.Enumerator jdi = v.FindPartModulesImplementing<ModuleOrXHoloCache>().GetEnumerator();
                while (jdi.MoveNext())
                {
                    if (jdi.Current == null) continue;
                    if (jdi.Current.getGPS)
                    {
                        info = v.gameObject.AddComponent<OrXTargetInfo>();
                        break;
                    }

                }
                jdi.Dispose();
            }

            // add target to database
            if (info)
            {
                AddTarget(info);
                info.detectedTime = Time.time;
            }
        }

        public static void AddTarget(OrXTargetInfo target)
        {
            if (FlightGlobals.currentMainBody.name == "Bop")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Bop].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Dres")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Dres].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Duna")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Duna].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Eeloo")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Eve")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Eve].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Gilly")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Gilly].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Ike")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Ike].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Jool")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Jool].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Kerbin")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Kerbol")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Laythe")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Laythe].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Minmus")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Minmus].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Moho")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Moho].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Mun")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Mun].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Pol")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Pol].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Tylo")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Tylo].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Vall")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Vall].Add(target);
            }

            TargetDatabase[OrXHoloCache.OrXCoords.All].Add(target);

        }

        public void ClearDatabase()
        {
            foreach (OrXHoloCache.OrXCoords t in TargetDatabase.Keys)
            {
                foreach (OrXTargetInfo target in TargetDatabase[t])
                {
                    target.detectedTime = 0;
                }
            }

            TargetDatabase[OrXHoloCache.OrXCoords.Bop].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Dres].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Duna].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Eve].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Gilly].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Ike].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Jool].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Laythe].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Minmus].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Mun].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Pol].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Tylo].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Vall].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.All].Clear();

        }

    }
}