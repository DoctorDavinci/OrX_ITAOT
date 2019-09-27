using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;
using OrX.spawn;
using System.IO;
using System.Text;
using System.Linq;
using FinePrint;
using FinePrint.Utilities;


namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXHoloCache : MonoBehaviour
    {
        #region Fields

        public static Dictionary<OrXCoords, List<OrXTargetInfo>> TargetDatabase;
        public static Dictionary<OrXCoords, List<OrXHoloCacheinfo>> HoloCacheTargets;
        OrXCoords coords;
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

        string _sth = string.Empty;
        bool showScores = false;

        private bool unlocked = false;

        public bool buildingMission = false;
        public bool showTargets = true;
        public static Rect WindowRectToolbar;
        public static Rect WindowRectHCGUI;
        public static OrXHoloCache instance;
        //public static bool GAME_UI_ENABLED;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        public static bool hasAddedButton = false;
        public static bool OrXHCGUIEnabled;

        public bool scanning = false;
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

        public float minLoadRange = 1000;

        float HCGUIEntryCount;
        float HCGUIEntryHeight = 24;
        float HCGUIBorder = 5;
        public bool TargetHCGUI = false;
        int TargetHCGUIIndex;
        bool resetTargetHCGUI;
        public OrXHoloCacheinfo designatedHCGUIInfo;
        private string soi = "";
        Guid vid;

        //private static Vector2 _displayViewerPosition = Vector2.zero;
        public Vector3d designatedHCGUICoords => designatedHCGUIInfo.gpsCoordinates;
        public Vector3d tpoint;

        private Texture2D redDot;
        public Texture2D HoloTargetTexture
        {
            get { return redDot ? redDot : redDot = GameDatabase.Instance.GetTexture("OrX/Plugin/HoloTarget", false); }
        }

        public Vessel _HoloKron;

        public bool addCoords = false;
        Rigidbody rigidBody;
        private bool passive = false;
        private bool paused = false;
        private int delay = 300;
        public bool sth = true;
        public bool hide = false;
        CelestialBody SOIcurrent;
        DestroyOnSceneSwitch ds;


        private bool reloadWorldPos = false;
        private string targetLabel;

        private float timer = 1;
        public string _craftFile = string.Empty;
        public bool checking = false;
        public double targetDistance = 0;

        public bool challengeRunning = false;
        public bool setup = false;
        public bool completed = false;
        public string missionName = string.Empty;
        public string missionType = string.Empty;
        public string challengeType = string.Empty;
        public string tech = string.Empty;
        public int mCount = 0;
        public bool spawned = false;
        public string Gold = string.Empty;
        public string Silver = string.Empty;
        public string Bronze = string.Empty;

        [KSPField(isPersistant = true)]
        public bool blueprintsAdded = false;
        string crafttosave = string.Empty;
        string blueprintsLabel = "Add Blueprints to Holo";

        public bool saveShip = false;
        private string craftToAddMission = string.Empty;

        public double altitude = 0;
        public double latitude = 0;
        public double longitude = 0;

        public double _altitude = 0;
        public double _latitude = 0;
        public double _longitude = 0;

        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public bool GuiEnabledOrXMissions = false;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public double distance = 0;
        private double missionTime = 0;

        public string techToAdd = string.Empty;

        public string missionDescription0 = string.Empty;
        public string missionDescription1 = string.Empty;
        public string missionDescription2 = string.Empty;
        public string missionDescription3 = string.Empty;
        public string missionDescription4 = string.Empty;
        public string missionDescription5 = string.Empty;
        public string missionDescription6 = string.Empty;
        public string missionDescription7 = string.Empty;
        public string missionDescription8 = string.Empty;
        public string missionDescription9 = string.Empty;

        public string textBox = string.Empty;

        public bool DAKARRacing = false;
        public bool Scuba = false;
        public bool windRacing = false;
        public bool snowballFight = false;
        private bool PlayOrXMission = false;
        private bool craftBrowserOpen = false;
        public bool building = false;
        private bool pauseCheck = false;
        public bool resetHoloCache = false;
        public bool checkingMission = false;
        private bool geoCache = true;
        private bool addingBluePrints = false;
        private bool locAdded = false;
        private bool holoSelected = false;
        private bool holoSpawned = false;
        private bool editDescription = false;

        public float pitch = 0;
        public float heading = 0;
        public float windIntensity = 10;
        public float teaseDelay = 0;
        public float windVariability = 50;
        public float variationIntensity = 50;

        public static Rect WindowRectBrowser;
        double _latMission = 0;
        double _lonMission = 0;
        double _altMission = 0;
        Vessel challengeHolo;
        bool missionHoloSpawned = false;

        public string Password = "OrX";
        public string pas = string.Empty;

        public string missionCraftFile = string.Empty;
        public string blueprintsFile = string.Empty;
        public string holoToAdd = string.Empty;

        int gpsCount = 0;
        double latMission = 0;
        double lonMission = 0;
        double altMission = 0;

        Vector3 lastCoord;
        int locCount = 0;

        string NextCoord;
        List<string> CoordDatabase;
        int coordCount = 0;

        Vector3 startLocation;
        Guid id;

        List<string> stageTimes;
        private double maxDepth = 0;

        List<string> _scoreboard;
        private string challengersName = string.Empty;
        private double topSurfaceSpeed = 0;

        private int ec = 0;

        ConfigNode _file;
        ConfigNode _mission;
        ConfigNode _scoreboard_;
        ConfigNode _blueprints_;

        ConfigNode scoreboard0;
        ConfigNode scoreboard1;
        ConfigNode scoreboard2;
        ConfigNode scoreboard3;
        ConfigNode scoreboard4;
        ConfigNode scoreboard5;
        ConfigNode scoreboard6;
        ConfigNode scoreboard7;
        ConfigNode scoreboard8;
        ConfigNode scoreboard9;

        string nameSB0 = string.Empty;
        string timeSB0 = string.Empty;
        string nameSB1 = string.Empty;
        string timeSB1 = string.Empty;
        string nameSB2 = string.Empty;
        string timeSB2 = string.Empty;
        string nameSB3 = string.Empty;
        string timeSB3 = string.Empty;
        string nameSB4 = string.Empty;
        string timeSB4 = string.Empty;
        string nameSB5 = string.Empty;
        string timeSB5 = string.Empty;
        string nameSB6 = string.Empty;
        string timeSB6 = string.Empty;
        string nameSB7 = string.Empty;
        string timeSB7 = string.Empty;
        string nameSB8 = string.Empty;
        string timeSB8 = string.Empty;
        string nameSB9 = string.Empty;
        string timeSB9 = string.Empty;

        bool updatingScores = false;

        Vector3d nextLocation;
        private double targetDistanceMission = 0;
        public string _targetDistance = string.Empty;
        Vessel targetCube;

        Quaternion rot;
        double la = 0;
        double lo = 0;
        double al = 0;
        public double _lat = 0f;
        public double _lon = 0f;
        public double _alt = 0f;

        public ConfigNode craft = null;
        public string shipDescription = string.Empty;

        private StringBuilder debugString = new StringBuilder();
        public string craftToSpawn = string.Empty;
        public string cfgToLoad = string.Empty;
        string OrXv = "OrXv";

        public Vessel holoCache;
        string holocacheCraftLoc = string.Empty;
        List<string> holocacheFiles;
        string sphLoc = string.Empty;
        List<string> sphFiles;
        string vabLoc = string.Empty;
        List<string> vabFiles;
        bool sph = true;
        bool holoHangar = false;

        int _hcCount = 0;
        bool saveLocalVessels = false;
        string saveLocalLabel = "Save Local Craft";
        private float localSaveRange = 500;

        internal static List<ProtoCrewMember> SelectedCrewData;

        public string craftFile = string.Empty;
        private string flagURL = string.Empty;
        private float spawnTimer = 0.0f;

        private double _lat_ = 0.0f;
        private double _lon_ = 0.0f;

        public bool holo = true;
        public bool emptyholo = true;
        int hcCount = 0;
        public bool showHoloTargets = false;
        private int boidCount = 10;
        private int spawnRadius = 0;
        public bool boid = false;
        public string missionCraftLoc = string.Empty;
        bool spawningMissionCraft = false;
        bool savingToHoloKron = false;


        double mPerDegree = 0;
        double degPerMeter = 0;
        double altOffset = 0;

        #endregion

        #region Core

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
            TargetDatabase = new Dictionary<OrXCoords, List<OrXTargetInfo>>();
            TargetDatabase.Add(OrXCoords.Bop, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Dres, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Duna, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Eeloo, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Eve, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Gilly, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Ike, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Jool, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Kerbin, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Kerbol, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Laythe, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Minmus, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Moho, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Mun, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Pol, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Tylo, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.Vall, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXCoords.All, new List<OrXTargetInfo>());

            if (HoloCacheTargets == null)
            {
                HoloCacheTargets = new Dictionary<OrXCoords, List<OrXHoloCacheinfo>>();
                HoloCacheTargets.Add(OrXCoords.Bop, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Dres, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Duna, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Eeloo, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Eve, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Gilly, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Ike, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Jool, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Kerbin, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Kerbol, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Laythe, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Minmus, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Moho, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Mun, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Pol, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Tylo, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.Vall, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXCoords.All, new List<OrXHoloCacheinfo>());
            }

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Import/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Import/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Export/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/");

            OrXHCGUIEnabled = false;
            AddToolbarButton();
            TargetHCGUI = false;
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
            GameEvents.onVesselSOIChanged.Add(checkSOI);
            //GameEvents.OnFlightGlobalsReady.Add(checkHoloTargets);
            //LoadHoloCacheTargets();
        }

        /*public void Update()
        {
            if (HighLogic.LoadedScene == GameScenes.LOADING) return;

            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (triggerKerbSetup && triggerVessel.isActiveVessel)
                {
                    var kerb = triggerVessel.rootPart.FindModuleImplementing<ModuleOrX>();
                    if (kerb != null)
                    {
                        kerb.holoSave = true;
                    }
                }
            }
        }
        */
        private void checkHoloTargets(bool data)
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                CheckSOI();
            }
        }

        void OnDestroy()
        {
            //GameEvents.onHideUI.Remove(HideGameUI);
            //ameEvents.onShowUI.Remove(ShowGameUI);

            HoloCacheTargets = new Dictionary<OrXCoords, List<OrXHoloCacheinfo>>();
            TargetDatabase[OrXCoords.Bop].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Bop].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Bop);
            TargetDatabase[OrXCoords.Dres].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Dres].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Dres);
            TargetDatabase[OrXCoords.Duna].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Duna].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Duna);
            TargetDatabase[OrXCoords.Eeloo].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Eeloo].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Eeloo);
            TargetDatabase[OrXCoords.Eve].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Eve].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Eve);
            TargetDatabase[OrXCoords.Gilly].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Gilly].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Gilly);
            TargetDatabase[OrXCoords.Ike].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Ike].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Ike);
            TargetDatabase[OrXCoords.Jool].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Jool].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Jool);
            TargetDatabase[OrXCoords.Kerbin].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Kerbin].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Kerbin);
            TargetDatabase[OrXCoords.Kerbol].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Kerbol].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Kerbol);
            TargetDatabase[OrXCoords.Laythe].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Laythe].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Laythe);
            TargetDatabase[OrXCoords.Minmus].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Minmus].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Minmus);
            TargetDatabase[OrXCoords.Moho].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Moho].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Moho);
            TargetDatabase[OrXCoords.Mun].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Mun].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Mun);
            TargetDatabase[OrXCoords.Pol].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Pol].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Pol);
            TargetDatabase[OrXCoords.Tylo].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Tylo].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Tylo);
            TargetDatabase[OrXCoords.Vall].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.Vall].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Vall);
            TargetDatabase[OrXCoords.All].RemoveAll(target => target == null);
            TargetDatabase[OrXCoords.All].RemoveAll(target => target.OrbitalBodyName == OrXCoords.All);

        }

        #endregion

        #region Utilities

        private void Dummy() { }
        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }

        private string HoloCacheListToString()
        {
            string finalString = string.Empty;
            string aString = string.Empty;
            aString += FlightGlobals.currentMainBody.name;
            aString += ",";
            aString += HoloCacheName;
            aString += ",";
            aString += ""; // Password;
            aString += ",";
            aString += _lat;
            aString += ",";
            aString += _lon;
            aString += ",";
            aString += _alt;
            aString += ";";
            aString += missionName;
            aString += ";";
            aString += missionType;
            aString += ";";
            aString += challengeType;
            aString += ";";

            finalString += aString;
            finalString += ":";

            string bString = string.Empty;
            bString += FlightGlobals.currentMainBody.name;
            bString += ",";
            bString += HoloCacheName;
            bString += ",";
            bString += "";// Password;
            bString += ",";
            bString += _lat;
            bString += ",";
            bString += _lon;
            bString += ",";
            bString += _alt;
            bString += ";";
            bString += missionName;
            bString += ";";
            bString += missionType;
            bString += ";";
            bString += challengeType;
            bString += ";";

            finalString += bString;

            return finalString;
        }
        private void StringToHoloCacheList(string listString)
        {
            if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbin")
            {
                coords = OrXCoords.Kerbin;
            }
            else
            {
                if (FlightGlobals.ActiveVessel.mainBody.name == "Mun")
                {
                    coords = OrXCoords.Mun;
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.mainBody.name == "Minmus")
                    {
                        coords = OrXCoords.Minmus;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbol")
                        {
                            coords = OrXCoords.Kerbol;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.mainBody.name == "Duna")
                            {
                                coords = OrXCoords.Duna;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.mainBody.name == "Gilly")
                                {
                                    coords = OrXCoords.Gilly;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Ike")
                                    {
                                        coords = OrXCoords.Ike;
                                    }
                                    else
                                    {
                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Jool")
                                        {
                                            coords = OrXCoords.Jool;
                                        }
                                        else
                                        {
                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Bop")
                                            {
                                                coords = OrXCoords.Bop;
                                            }
                                            else
                                            {
                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Dres")
                                                {
                                                    coords = OrXCoords.Dres;
                                                }
                                                else
                                                {
                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Laythe")
                                                    {
                                                        coords = OrXCoords.Laythe;
                                                    }
                                                    else
                                                    {
                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Eeloo")
                                                        {
                                                            coords = OrXCoords.Eeloo;
                                                        }
                                                        else
                                                        {
                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Moho")
                                                            {
                                                                coords = OrXCoords.Moho;
                                                            }
                                                            else
                                                            {
                                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Eve")
                                                                {
                                                                    coords = OrXCoords.Eve;
                                                                }
                                                                else
                                                                {
                                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Pol")
                                                                    {
                                                                        coords = OrXCoords.Pol;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Tylo")
                                                                        {
                                                                            coords = OrXCoords.Tylo;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Vall")
                                                                            {
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
                            HoloCacheTargets[OrXCoords.All].Add(newInfo);
                            Vector3d surfacePoint = new Vector3d(lat, longi, alt);
                            Waypoint waypoint = new Waypoint();
                            System.Random r = new System.Random();
                            waypointColor = (int)(r.NextDouble() * seeds.Count());
                            waypoint.id = "marker";
                            waypoint.seed = seeds[waypointColor];
                            waypoint.name = craftToSpawn;
                            waypoint.celestialName = name;
                            waypoint.longitude = longi;
                            waypoint.latitude = lat;
                            waypoint.height = FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude;//TerrainHeight(lat, longi, FlightGlobals.ActiveVessel.mainBody);
                            waypoint.altitude = alt - waypoint.height;

                            Debug.Log("[OrX Target Manager] ====================================== ADDING WAYPOINT FOR " + craftToSpawn + " ===");
                            ScenarioCustomWaypoints.RemoveWaypoint(waypoint);
                            ScenarioCustomWaypoints.AddWaypoint(waypoint);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log("[OrX Target Manager] HoloCache config file processed ...... ");
            }
        }

        private void ResetData()
        {
            if (OrXLog.instance.keysSet)
            {
                OrXLog.instance.ResetFocusKeys();
            }
            addCoords = false;
            addCoordSetup = false;
            holo = false;
            spawnByOrx = false;
            challengeRunning = false;

            nameSB0 = string.Empty;
            timeSB0 = string.Empty;
            nameSB1 = string.Empty;
            timeSB1 = string.Empty;
            nameSB2 = string.Empty;
            timeSB2 = string.Empty;
            nameSB3 = string.Empty;
            timeSB3 = string.Empty;
            nameSB4 = string.Empty;
            timeSB4 = string.Empty;
            nameSB5 = string.Empty;
            timeSB5 = string.Empty;
            nameSB6 = string.Empty;
            timeSB6 = string.Empty;
            nameSB7 = string.Empty;
            timeSB7 = string.Empty;
            nameSB8 = string.Empty;
            timeSB8 = string.Empty;
            nameSB9 = string.Empty;
            timeSB9 = string.Empty;

            _file = null;
            _mission = null;
            _scoreboard_ = null;

            scoreboard0 = null;
            scoreboard1 = null;
            scoreboard2 = null;
            scoreboard3 = null;
            scoreboard4 = null;
            scoreboard5 = null;
            scoreboard6 = null;
            scoreboard7 = null;
            scoreboard8 = null;
            scoreboard9 = null;

            coordCount = 0;

            if (_scoreboard != null)
            {
                _scoreboard.Clear();

            }

            if (stageTimes != null)
            {
                stageTimes.Clear();
            }

            if (CoordDatabase != null)
            {
                CoordDatabase.Clear();
            }

            missionHoloSpawned = false;
            missionDescription0 = string.Empty;
            missionDescription1 = string.Empty;
            missionDescription2 = string.Empty;
            missionDescription3 = string.Empty;
            missionDescription4 = string.Empty;
            missionDescription5 = string.Empty;
            missionDescription6 = string.Empty;
            missionDescription7 = string.Empty;
            missionDescription8 = string.Empty;
            missionDescription9 = string.Empty;
        }

        IEnumerator CleanDatabaseRoutine()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(5);

                TargetDatabase[OrXCoords.Bop].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Bop].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Bop);
                TargetDatabase[OrXCoords.Dres].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Dres].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Dres);
                TargetDatabase[OrXCoords.Duna].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Duna].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Duna);
                TargetDatabase[OrXCoords.Eeloo].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Eeloo].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Eeloo);
                TargetDatabase[OrXCoords.Eve].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Eve].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Eve);
                TargetDatabase[OrXCoords.Gilly].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Gilly].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Gilly);
                TargetDatabase[OrXCoords.Ike].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Ike].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Ike);
                TargetDatabase[OrXCoords.Jool].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Jool].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Jool);
                TargetDatabase[OrXCoords.Kerbin].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Kerbin].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Kerbin);
                TargetDatabase[OrXCoords.Kerbol].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Kerbol].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Kerbol);
                TargetDatabase[OrXCoords.Laythe].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Laythe].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Laythe);
                TargetDatabase[OrXCoords.Minmus].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Minmus].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Minmus);
                TargetDatabase[OrXCoords.Moho].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Moho].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Moho);
                TargetDatabase[OrXCoords.Mun].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Mun].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Mun);
                TargetDatabase[OrXCoords.Pol].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Pol].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Pol);
                TargetDatabase[OrXCoords.Tylo].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Tylo].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Tylo);
                TargetDatabase[OrXCoords.Vall].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.Vall].RemoveAll(target => target.OrbitalBodyName == OrXCoords.Vall);
                TargetDatabase[OrXCoords.All].RemoveAll(target => target == null);
                TargetDatabase[OrXCoords.All].RemoveAll(target => target.OrbitalBodyName == OrXCoords.All);

            }
        }
        public void RemoveTarget(OrXTargetInfo target, OrXCoords OrbitalBodyName)
        {
            TargetDatabase[OrbitalBodyName].Remove(target);

        }
        public static void AddTarget(OrXTargetInfo target)
        {
            if (FlightGlobals.currentMainBody.name == "Bop")
            {
                TargetDatabase[OrXCoords.Bop].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Dres")
            {
                TargetDatabase[OrXCoords.Dres].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Duna")
            {
                TargetDatabase[OrXCoords.Duna].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Eeloo")
            {
                TargetDatabase[OrXCoords.Eeloo].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Eve")
            {
                TargetDatabase[OrXCoords.Eve].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Gilly")
            {
                TargetDatabase[OrXCoords.Gilly].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Ike")
            {
                TargetDatabase[OrXCoords.Ike].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Jool")
            {
                TargetDatabase[OrXCoords.Jool].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Kerbin")
            {
                TargetDatabase[OrXCoords.Kerbin].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Kerbol")
            {
                TargetDatabase[OrXCoords.Kerbol].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Laythe")
            {
                TargetDatabase[OrXCoords.Laythe].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Minmus")
            {
                TargetDatabase[OrXCoords.Minmus].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Moho")
            {
                TargetDatabase[OrXCoords.Moho].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Mun")
            {
                TargetDatabase[OrXCoords.Mun].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Pol")
            {
                TargetDatabase[OrXCoords.Pol].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Tylo")
            {
                TargetDatabase[OrXCoords.Tylo].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Vall")
            {
                TargetDatabase[OrXCoords.Vall].Add(target);
            }

            TargetDatabase[OrXCoords.All].Add(target);

        }
        public void ClearDatabase()
        {
            foreach (OrXCoords t in TargetDatabase.Keys)
            {
                foreach (OrXTargetInfo target in TargetDatabase[t])
                {
                    target.detectedTime = 0;
                }
            }

            TargetDatabase[OrXCoords.Bop].Clear();
            TargetDatabase[OrXCoords.Dres].Clear();
            TargetDatabase[OrXCoords.Duna].Clear();
            TargetDatabase[OrXCoords.Eeloo].Clear();
            TargetDatabase[OrXCoords.Eve].Clear();
            TargetDatabase[OrXCoords.Gilly].Clear();
            TargetDatabase[OrXCoords.Ike].Clear();
            TargetDatabase[OrXCoords.Jool].Clear();
            TargetDatabase[OrXCoords.Kerbin].Clear();
            TargetDatabase[OrXCoords.Kerbol].Clear();
            TargetDatabase[OrXCoords.Laythe].Clear();
            TargetDatabase[OrXCoords.Minmus].Clear();
            TargetDatabase[OrXCoords.Moho].Clear();
            TargetDatabase[OrXCoords.Moho].Clear();
            TargetDatabase[OrXCoords.Mun].Clear();
            TargetDatabase[OrXCoords.Pol].Clear();
            TargetDatabase[OrXCoords.Tylo].Clear();
            TargetDatabase[OrXCoords.Vall].Clear();
            TargetDatabase[OrXCoords.All].Clear();
            CheckSOI();
        }

        public void LoadHoloCacheTargets()
        {
            HoloCacheName = "";
            //CheckSOI();
            TargetHCGUI = false;
            scanning = false;
            checking = false;
            reload = true;

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/");

            if (HoloCacheTargets == null)
            {
                HoloCacheTargets = new Dictionary<OrXCoords, List<OrXHoloCacheinfo>>();
            }
            HoloCacheTargets.Clear();
            HoloCacheTargets.Add(OrXCoords.Bop, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Dres, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Duna, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Eeloo, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Eve, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Gilly, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Ike, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Jool, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Kerbin, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Kerbol, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Laythe, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Minmus, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Moho, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Mun, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Pol, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Tylo, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.Vall, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXCoords.All, new List<OrXHoloCacheinfo>());

            string holoCacheLoc = UrlDir.ApplicationRootPath + "GameData/";
            var files = new List<string>(Directory.GetFiles(holoCacheLoc, "*.orx", SearchOption.AllDirectories));
            bool _spawned = true;
            List<string> holoList = new List<string>();

            if (files != null)
            {
                List<string>.Enumerator cfgsToAdd = files.GetEnumerator();
                while (cfgsToAdd.MoveNext())
                {
                    bool loadWaypoint = false;

                    try
                    {
                        ConfigNode fileNode = ConfigNode.Load(cfgsToAdd.Current);
                        hcCount = 0;

                        if (fileNode != null && fileNode.HasNode("OrX"))
                        {
                            ConfigNode node = fileNode.GetNode("OrX");

                            foreach (ConfigNode spawnCheck in node.nodes)
                            {
                                if (_spawned)
                                {
                                    if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                                    {
                                        Debug.Log("[OrX Holo Loader] === FOUND HOLOCACHE === " + hcCount);
                                        foreach (ConfigNode.Value cv in spawnCheck.values)
                                        {
                                            if (cv.name == "spawned")
                                            {
                                                if (cv.value == "False")
                                                {
                                                    Debug.Log("[OrX Holo Loader] === HOLOCACHE " + hcCount + " has not spawned ... ");
                                                    loadWaypoint = true;
                                                    _spawned = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    var complete = spawnCheck.GetValue("completed");
                                                    if (complete == "False")
                                                    {
                                                        Debug.Log("[OrX Holo Loader] === HOLOCACHE " + hcCount + " has spawned but has not been completed ... END TRANSMISSION");
                                                        _spawned = false;
                                                        hcCount = int.MaxValue;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("[OrX Holo Loader] === HOLOCACHE " + hcCount + " has been completed ... CHECKING FOR EXTRAS");
                                                        if (spawnCheck.HasValue("extras"))
                                                        {
                                                            var t = spawnCheck.GetValue("extras");
                                                            if (t == "False")
                                                            {
                                                                Debug.Log("[OrX Holo Loader] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION");
                                                                _spawned = false;
                                                                hcCount = int.MaxValue;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Debug.Log("[OrX Holo Loader] === HOLOCACHE " + hcCount + " has extras ... CONTINUING");
                                                                _spawned = true;
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

                            if (node.HasNode("OrXHoloCacheCoords" + hcCount))
                            {
                                foreach (ConfigNode HoloCacheNode in node.GetNodes("OrXHoloCacheCoords" + hcCount))
                                {
                                    if (HoloCacheNode.HasValue("SOI"))
                                    {
                                        string _soi = HoloCacheNode.GetValue("SOI");

                                        if (_soi == FlightGlobals.currentMainBody.name)
                                        {
                                            if (HoloCacheNode.HasValue("Targets"))
                                            {
                                                string targetString = HoloCacheNode.GetValue("Targets");
                                                if (targetString == string.Empty)
                                                {
                                                    Debug.Log("[OrX Holo Loader] OrX HoloCache Target string was empty!");
                                                    break;
                                                }

                                                if (loadWaypoint)
                                                {
                                                    Debug.Log("[OrX Holo Loader] Loaded OrX HoloCache Targets");

                                                    StringToHoloCacheList(targetString);
                                                }
                                                else
                                                {
                                                    Debug.Log("[OrX Holo Loader] HoloCache has already been spawned .... SKIPPING");
                                                }
                                            }
                                            else
                                            {
                                                Debug.Log("[OrX Holo Loader] No OrX HoloCache Targets value found!");
                                            }
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Holo Loader] HoloCache Target Outside Of Current SOI ...... SKIPPING");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log("[OrX Holo Loader] HoloCache " + hcCount + " doesn't exist ...... Continuing");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[OrX Holo Loader] HoloCache Targets Out Of Range ...... Continuing");
                    }
                }
                cfgsToAdd.Dispose();

                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (!challengeRunning && !buildingMission)
                    {
                        TargetDistance();
                    }
                }
            }
            else
            {
                Debug.Log("[OrX Target Manager] === HoloCache List is empty ===");
            }
            reload = false;
        }

        private void checkSOI(GameEvents.HostedFromToAction<Vessel, CelestialBody> data)
        {
            CheckSOI();
        }
        public void CheckSOI()
        {
            soi = FlightGlobals.ActiveVessel.mainBody.name;

            if (soi == "Bop")
            {
                coords = OrXCoords.Bop;
            }
            else
            {
                if (soi == "Dres")
                {
                    coords = OrXCoords.Dres;
                }
                else
                {
                    if (soi == "Duna")
                    {
                        coords = OrXCoords.Duna;
                    }
                    else
                    {
                        if (soi == "Eeloo")
                        {
                            coords = OrXCoords.Eeloo;
                        }
                        else
                        {
                            if (soi == "Eve")
                            {
                                coords = OrXCoords.Eve;
                            }
                            else
                            {
                                if (soi == "Gilly")
                                {
                                    coords = OrXCoords.Gilly;
                                }
                                else
                                {
                                    if (soi == "Ike")
                                    {
                                        coords = OrXCoords.Ike;
                                    }
                                    else
                                    {
                                        if (soi == "Jool")
                                        {
                                            coords = OrXCoords.Jool;
                                        }
                                        else
                                        {
                                            if (soi == "Kerbin")
                                            {
                                                coords = OrXCoords.Kerbin;
                                            }
                                            else
                                            {
                                                if (soi == "Kerbol")
                                                {
                                                    coords = OrXCoords.Kerbol;
                                                }
                                                else
                                                {
                                                    if (soi == "Laythe")
                                                    {
                                                        coords = OrXCoords.Laythe;
                                                    }
                                                    else
                                                    {
                                                        if (soi == "Minmus")
                                                        {
                                                            coords = OrXCoords.Minmus;
                                                        }
                                                        else
                                                        {
                                                            if (soi == "Moho")
                                                            {
                                                                coords = OrXCoords.Moho;
                                                            }
                                                            else
                                                            {
                                                                if (soi == "Mun")
                                                                {
                                                                    coords = OrXCoords.Mun;
                                                                }
                                                                else
                                                                {
                                                                    if (soi == "Pol")
                                                                    {
                                                                        coords = OrXCoords.Pol;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (soi == "Tylo")
                                                                        {
                                                                            coords = OrXCoords.Tylo;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (soi == "Vall")
                                                                            {
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

            if (!challengeRunning)
            {
                LoadHoloCacheTargets();
            }
        }

        private static int[] seeds = new int[] {
            269, 316, 876, 9, 569, 159, 262, 822, 412, 972, 105, 665, 255, 358, 1375, 51,
            98, 1115, 248, 808, 398, 501, 91, 651, 241, 344, 904, 37, 597, 187, 747, 337,
            384, 487, 77, 180, 1197, 330, 890, 23, 583, 173, 276, 1293, 426, 16, 119, 679,
        };

        public static double TerrainHeight(double latitude, double longitude, CelestialBody body)
        {
            if (body.pqsController == null)
            {
                return 0;
            }

            double latRads = Math.PI / 180.0 * latitude;
            double lonRads = Math.PI / 180.0 * longitude;
            Vector3d radialVector = new Vector3d(Math.Cos(latRads) * Math.Cos(lonRads), Math.Sin(latRads), Math.Cos(latRads) * Math.Sin(lonRads));
            return Math.Max(body.pqsController.GetSurfaceHeight(radialVector) - body.pqsController.radius, 0.0);
        }

        internal class CrewData
        {
            public string name = null;
            public ProtoCrewMember.Gender? gender = null;
            public bool addToRoster = true;

            public CrewData() { }
            public CrewData(CrewData cd)
            {
                name = cd.name;
                gender = cd.gender;
                addToRoster = cd.addToRoster;
            }
        }
        private class HoloCacheData
        {
            public string name = null;
            public Guid? id = null;
            public string craftURL = null;
            public AvailablePart craftPart = null;
            public string flagURL = null;
            public VesselType vesselType = VesselType.Ship;
            public CelestialBody body = null;
            public Orbit orbit = null;
            public double latitude = 0.0;
            public double longitude = 0.0;
            public double? altitude = null;
            public float height = 0.0f;
            public bool orbiting = false;
            public bool owned = false;
            public List<CrewData> crew = new List<CrewData>();
            public PQSCity pqsCity = null;
            public Vector3d pqsOffset;
            public float heading;
            public float pitch;
            public float roll;

            public HoloCacheData() { }
            public HoloCacheData(HoloCacheData vd)
            {
                name = vd.name;
                id = vd.id;
                craftURL = vd.craftURL;
                craftPart = vd.craftPart;
                flagURL = vd.flagURL;
                vesselType = vd.vesselType;
                body = vd.body;
                orbit = vd.orbit;
                latitude = vd.latitude;
                longitude = vd.longitude;
                altitude = vd.altitude;
                height = vd.height;
                orbiting = vd.orbiting;
                owned = vd.owned;
                pqsCity = vd.pqsCity;
                pqsOffset = vd.pqsOffset;
                heading = vd.heading;
                pitch = vd.pitch;
                roll = vd.roll;

                foreach (CrewData cd in vd.crew)
                {
                    crew.Add(new CrewData(cd));
                }
            }
        }

        #endregion

        /// 

        #region Missions

        List<Waypoint> waypoints = new List<Waypoint>();

        public void SaveConfig()
        {
            triggerKerbSetup = false;
            _triggered = false;
            addCoordSetup = false;

            string hConfigLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"
                + HoloCacheName + "/" + HoloCacheName + ".orx";

            if (OrXLog.instance.keysSet)
            {
                OrXLog.instance.ResetFocusKeys();
            }

            mCount = 0;
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName))
            {
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName);
            }
            _file = ConfigNode.Load(hConfigLoc);
            if (_file == null)
            {
                _file = new ConfigNode();
                _file.AddValue("name", HoloCacheName);
                _file.AddNode("OrX");
                _file.Save("GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
            }

            _file = ConfigNode.Load(hConfigLoc);

            ConfigNode _missionMain = new ConfigNode();

            ConfigNode node = null;
            if (_file != null && _file.HasNode("OrX"))
            {
                node = _file.GetNode("OrX");

                hcCount = 0;
                mCount = 0;
                ConfigNode HoloCacheNode = null;

                foreach (ConfigNode cn in node.nodes)
                {
                    if (cn.name.Contains("OrXHoloCacheCoords"))
                    {
                        Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " FOUND ===");

                        hcCount += 1;
                    }

                    if (cn.name.Contains("mission"))
                    {

                        mCount += 1;
                        Debug.Log("[OrX Mission] === MISSION " + mCount + " FOUND ===");

                    }
                }

                if (node.HasNode("OrXHoloCacheCoords" + hcCount))
                {
                    Debug.Log("[OrX Mission] === ERROR === HOLOCACHE " + hcCount + " FOUND AGAIN ===");

                    foreach (ConfigNode n in node.GetNodes("OrXHoloCacheCoords" + hcCount))
                    {
                        if (n.GetValue("SOI") == FlightGlobals.ActiveVessel.mainBody.name)
                        {
                            HoloCacheNode = n;
                            break;
                        }
                    }

                    if (HoloCacheNode == null)
                    {
                        HoloCacheNode = node.AddNode("OrXHoloCacheCoords" + hcCount);
                        HoloCacheNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                        HoloCacheNode.AddValue("spawned", "False");
                        HoloCacheNode.AddValue("extras", "False");
                        HoloCacheNode.AddValue("unlocked", "False");
                        HoloCacheNode.AddValue("tech", tech);

                        HoloCacheNode.AddValue("missionName", missionName);
                        HoloCacheNode.AddValue("missionType", missionType);
                        HoloCacheNode.AddValue("challengeType", challengeType);

                        HoloCacheNode.AddValue("missionDescription0", missionDescription0);
                        HoloCacheNode.AddValue("missionDescription1", missionDescription1);
                        HoloCacheNode.AddValue("missionDescription2", missionDescription2);
                        HoloCacheNode.AddValue("missionDescription3", missionDescription3);
                        HoloCacheNode.AddValue("missionDescription4", missionDescription4);
                        HoloCacheNode.AddValue("missionDescription5", missionDescription5);
                        HoloCacheNode.AddValue("missionDescription6", missionDescription6);
                        HoloCacheNode.AddValue("missionDescription7", missionDescription7);
                        HoloCacheNode.AddValue("missionDescription8", missionDescription8);
                        HoloCacheNode.AddValue("missionDescription9", missionDescription9);

                        HoloCacheNode.AddValue("gold", Gold);
                        HoloCacheNode.AddValue("silver", Silver);
                        HoloCacheNode.AddValue("bronze", Bronze);

                        HoloCacheNode.AddValue("completed", completed);
                        HoloCacheNode.AddValue("count", mCount);

                        HoloCacheNode.AddValue("lat", _lat);
                        HoloCacheNode.AddValue("lon", _lon);
                        HoloCacheNode.AddValue("alt", _alt);
                    }
                }
                else
                {
                    Debug.Log("[OrX Mission] === CREATING HOLOCACHE " + hcCount + " ===");

                    HoloCacheNode = node.AddNode("OrXHoloCacheCoords" + hcCount);
                    HoloCacheNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                    HoloCacheNode.AddValue("spawned", "False");
                    HoloCacheNode.AddValue("extras", "False");
                    HoloCacheNode.AddValue("unlocked", "False");
                    HoloCacheNode.AddValue("tech", tech);

                    HoloCacheNode.AddValue("missionName", missionName);
                    HoloCacheNode.AddValue("missionType", missionType);
                    HoloCacheNode.AddValue("challengeType", challengeType);

                    HoloCacheNode.AddValue("missionDescription0", missionDescription0);
                    HoloCacheNode.AddValue("missionDescription1", missionDescription1);
                    HoloCacheNode.AddValue("missionDescription2", missionDescription2);
                    HoloCacheNode.AddValue("missionDescription3", missionDescription3);
                    HoloCacheNode.AddValue("missionDescription4", missionDescription4);
                    HoloCacheNode.AddValue("missionDescription5", missionDescription5);
                    HoloCacheNode.AddValue("missionDescription6", missionDescription6);
                    HoloCacheNode.AddValue("missionDescription7", missionDescription7);
                    HoloCacheNode.AddValue("missionDescription8", missionDescription8);
                    HoloCacheNode.AddValue("missionDescription9", missionDescription9);

                    HoloCacheNode.AddValue("gold", Gold);
                    HoloCacheNode.AddValue("silver", Silver);
                    HoloCacheNode.AddValue("bronze", Bronze);

                    HoloCacheNode.AddValue("completed", completed);
                    HoloCacheNode.AddValue("count", mCount);

                    HoloCacheNode.AddValue("lat", _lat);
                    HoloCacheNode.AddValue("lon", _lon);
                    HoloCacheNode.AddValue("alt", _alt);
                }
                _file.Save(hConfigLoc);
                string targetString = HoloCacheListToString();
                HoloCacheNode.SetValue("Targets", targetString, true);

                _mission = _file.GetNode("mission" + mCount);
                if (_mission == null)
                {
                    Debug.Log("[OrX Mission] === ADDING NODE 'mission" + mCount + "' ===");

                    _file.AddNode("mission" + mCount);
                    _mission = _file.GetNode("mission" + mCount);

                    if (CoordDatabase != null)
                    {
                        if (CoordDatabase.Count >= 0)
                        {
                            Debug.Log("[OrX Mission] ========================== CoordDatabase.Count >= 0");

                            int c = 0;

                            string[] coords = CoordDatabase.ToArray();
                            foreach (string s in coords)
                            {
                                c += 1;
                                _mission.AddValue("stage" + c, s);
                                Debug.Log("[OrX Mission] === stage" + c + " added to " + HoloCacheName + ".orx === " + s);
                            }
                        }
                    }

                    _mission.AddNode("scoreboard");
                    _scoreboard_ = _mission.GetNode("scoreboard");

                    _scoreboard_.AddNode("scoreboard0");
                    _scoreboard_.AddNode("scoreboard1");
                    _scoreboard_.AddNode("scoreboard2");
                    _scoreboard_.AddNode("scoreboard3");
                    _scoreboard_.AddNode("scoreboard4");
                    _scoreboard_.AddNode("scoreboard5");
                    _scoreboard_.AddNode("scoreboard6");
                    _scoreboard_.AddNode("scoreboard7");
                    _scoreboard_.AddNode("scoreboard8");
                    _scoreboard_.AddNode("scoreboard9");

                    scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                    scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                    scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                    scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                    scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                    scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                    scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                    scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                    scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                    scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                    scoreboard0.AddValue("name", "<empty>");
                    scoreboard0.AddValue("time", "");
                    scoreboard1.AddValue("name", "<empty>");
                    scoreboard1.AddValue("time", "");
                    scoreboard2.AddValue("name", "<empty>");
                    scoreboard2.AddValue("time", "");
                    scoreboard3.AddValue("name", "<empty>");
                    scoreboard3.AddValue("time", "");
                    scoreboard4.AddValue("name", "<empty>");
                    scoreboard4.AddValue("time", "");
                    scoreboard5.AddValue("name", "<empty>");
                    scoreboard5.AddValue("time", "");
                    scoreboard6.AddValue("name", "<empty>");
                    scoreboard6.AddValue("time", "");
                    scoreboard7.AddValue("name", "<empty>");
                    scoreboard7.AddValue("time", "");
                    scoreboard8.AddValue("name", "<empty>");
                    scoreboard8.AddValue("time", "");
                    scoreboard9.AddValue("name", "<empty>");
                    scoreboard9.AddValue("time", "");
                }
                _file.Save(hConfigLoc);

                _file = ConfigNode.Load(hConfigLoc);
                node = _file.GetNode("OrX");
                Debug.Log("[OrX Mission] === ADDING NODE 'HoloCache" + hcCount + "' ===");
                node.AddNode("HoloCache" + hcCount);
                ConfigNode HCnode = node.GetNode("HoloCache" + hcCount);
                ConfigNode holoFileLoc = ConfigNode.Load(holoToAdd);
                holoFileLoc.CopyTo(HCnode);

                // ADD ENCRYPTION

                foreach (ConfigNode.Value cv in HCnode.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }

                foreach (ConfigNode cn in HCnode.nodes)
                {
                    foreach (ConfigNode.Value cv in cn.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn2 in cn.nodes)
                    {
                        foreach (ConfigNode.Value cv2 in cn2.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                            cv2.name = cvEncryptedName;
                            cv2.value = cvEncryptedValue;
                        }
                    }
                }

                Debug.Log("[OrX Mission] === HOLO CRAFT ENCRYPTED ===");

                _file.Save(hConfigLoc);
                Debug.Log("[OrX Mission] " + HoloCacheName + " Saved");
                _file = ConfigNode.Load(hConfigLoc);
                node = _file.GetNode("OrX");
                ConfigNode craftData = node.AddNode("HC" + hcCount + "OrXv0");
                ConfigNode location = craftData.AddNode("coords");
                craftData.AddValue("vesselName", craftToAddMission);
                location.AddValue("holo", hcCount);
                location.AddValue("pas", Password);
                location.AddValue("lat", FlightGlobals.ActiveVessel.latitude);
                location.AddValue("lon", FlightGlobals.ActiveVessel.longitude);
                location.AddValue("alt", FlightGlobals.ActiveVessel.altitude);
                location.AddValue("heading", 0);
                location.AddValue("pitch", 0);
                location.AddValue("rot", "null");
                location.AddValue("pos", "null");

                foreach (ConfigNode.Value cv in location.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }

                if (blueprintsAdded)
                {
                    ConfigNode addedCraft = ConfigNode.Load(blueprintsFile);

                    if (addedCraft != null)
                    {
                        ConfigNode craftFile = craftData.AddNode("craft");
                        ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloCacheName + "</b></color>");
                        Debug.Log("[OrX Mission] Saving: " + craftToAddMission);
                        addedCraft.CopyTo(craftFile);

                        foreach (ConfigNode.Value cv in craftFile.values)
                        {
                            if (cv.name == "ship")
                            {
                                cv.value = craftToAddMission;
                                break;
                            }
                        }

                        // ADD ENCRYPTION

                        foreach (ConfigNode.Value cv in craftFile.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        foreach (ConfigNode cn in craftFile.nodes)
                        {
                            foreach (ConfigNode.Value cv in cn.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                cv.name = cvEncryptedName;
                                cv.value = cvEncryptedValue;
                            }

                            foreach (ConfigNode cn2 in cn.nodes)
                            {
                                foreach (ConfigNode.Value cv2 in cn2.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                                    string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                                    cv2.name = cvEncryptedName;
                                    cv2.value = cvEncryptedValue;
                                }
                            }
                        }
                        _file.Save(hConfigLoc);
                        Debug.Log("[OrX Mission] " + craftToAddMission + " Saved to " + HoloCacheName);
                        ScreenMsg("<color=#cfc100ff><b>" + craftToAddMission + " Saved</b></color>");
                    }
                }

                _hcCount = hcCount;
            }
            coordCount = 0;

            if (saveLocalVessels)
            {
                Debug.Log("[OrX Mission] === SAVING LOCAL VESELS ===");

                int count = 0;
                _file = ConfigNode.Load(hConfigLoc);
                node = _file.GetNode("OrX");
                Debug.Log("[OrX Mission] === '" + HoloCacheName + "' found ===");

                double _latDiff = 0;
                double _lonDiff = 0;
                double _altDiff = 0;

                List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                while (v.MoveNext())
                {
                    try
                    {
                        if (v.Current == null) continue;
                        if (!v.Current.loaded || v.Current.packed) continue;
                        if (!v.Current.rootPart.Modules.Contains<ModuleOrXMission>() && !v.Current.rootPart.Modules.Contains<KerbalEVA>())
                        {
                            try
                            {
                                if (v.Current.altitude <= _altMission)
                                {
                                    _altDiff = _altMission - v.Current.altitude;
                                }
                                else
                                {
                                    _altDiff = v.Current.altitude - _altMission;
                                }

                                if (_latMission >= 0)
                                {
                                    if (v.Current.latitude >= _latMission)
                                    {
                                        _latDiff = v.Current.latitude - _latMission;
                                    }
                                    else
                                    {
                                        _latDiff = _latMission - v.Current.latitude;
                                    }
                                }
                                else
                                {
                                    if (v.Current.latitude >= 0)
                                    {
                                        _latDiff = v.Current.latitude - _latMission;
                                    }
                                    else
                                    {
                                        if (v.Current.latitude <= _latMission)
                                        {
                                            _latDiff = v.Current.latitude - _latMission;
                                        }
                                        else
                                        {

                                            _latDiff = _latMission - v.Current.latitude;
                                        }
                                    }
                                }

                                if (_lonMission >= 0)
                                {
                                    if (v.Current.longitude >= _lonMission)
                                    {
                                        _lonDiff = v.Current.longitude - _lonMission;
                                    }
                                    else
                                    {
                                        _lonDiff = _lonMission - v.Current.latitude;
                                    }
                                }
                                else
                                {
                                    if (v.Current.longitude >= 0)
                                    {
                                        _lonDiff = v.Current.longitude - _lonMission;
                                    }
                                    else
                                    {
                                        if (v.Current.longitude <= _lonMission)
                                        {
                                            _lonDiff = v.Current.longitude - _lonMission;
                                        }
                                        else
                                        {

                                            _lonDiff = _lonMission - v.Current.longitude;
                                        }
                                    }
                                }

                                double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                                double _altDiffDeg = _altDiff * degPerMeter;
                                double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                                double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                                //double _targetDistance = Vector3d.Distance(HoloKron.GetWorldPos3D(), v.Current.GetWorldPos3D()); ;

                                if (_targetDistance <= localSaveRange) // && v.Current.parts.Count != 1)
                                {
                                    bool kerbalAttached = false;

                                    List<Part>.Enumerator p = v.Current.parts.GetEnumerator();
                                    while (p.MoveNext())
                                    {
                                        if (p.Current.Modules.Contains<KerbalEVA>())
                                        {
                                            kerbalAttached = true;
                                            break;
                                        }
                                    }

                                    if (kerbalAttached)
                                    {
                                        Debug.Log("[OrX Mission] " + v.Current.vesselName + " has kerbal attached ... skipping");
                                    }
                                    else
                                    {
                                        count += 1;

                                        Vessel toSave = v.Current;
                                        Debug.Log("[OrX Mission] Vessel " + v.Current.vesselName + " Identified .......................");
                                        string shipDescription = v.Current.vesselName + " blueprints from " + HoloCacheName;

                                        Vector3 UpVect = (toSave.transform.position - toSave.mainBody.position).normalized;
                                        Vector3 EastVect = toSave.mainBody.getRFrmVel(toSave.CoM).normalized;
                                        Vector3 NorthVect = Vector3.Cross(EastVect, UpVect).normalized;

                                        float _pitch = Vector3.Angle(toSave.ReferenceTransform.forward, UpVect);
                                        float _left = Vector3.Angle(-toSave.ReferenceTransform.right, NorthVect); // left is 90 degrees behind vessel heading

                                        if (Math.Sign(Vector3.Dot(-toSave.ReferenceTransform.right, EastVect)) < 0)
                                        {
                                            _left = 360 - _left; // westward headings become angles greater than 180
                                        }

                                        // Be sure to subtract 90 degrees from pitch and left as the vessel reference transform is offset 90 degrees
                                        // from the respective vectors due to reasons

                                        ShipConstruct ConstructToSave = new ShipConstruct(HoloCacheName, shipDescription, v.Current.parts[0]);
                                        ConfigNode craftConstruct = new ConfigNode("Craft");
                                        craftConstruct = ConstructToSave.SaveShip();
                                        craftConstruct.RemoveValue("persistentId");
                                        craftConstruct.RemoveValue("steamPublishedFileId");
                                        craftConstruct.RemoveValue("rot");
                                        craftConstruct.RemoveValue("missionFlag");
                                        craftConstruct.RemoveValue("vesselType");
                                        craftConstruct.RemoveValue("OverrideDefault");
                                        craftConstruct.RemoveValue("OverrideActionControl");
                                        craftConstruct.RemoveValue("OverrideAxisControl");
                                        craftConstruct.RemoveValue("OverrideGroupNames");


                                        foreach (ConfigNode cn in craftConstruct.nodes)
                                        {
                                            if (cn.name == "PART")
                                            {
                                                cn.RemoveValue("persistentId");
                                                cn.RemoveValue("sameVesselCollision");
                                            }
                                        }

                                        Debug.Log("[OrX Mission] Saving: " + v.Current.vesselName);
                                        ScreenMsg("<color=#cfc100ff><b>Saving: " + v.Current.vesselName + "</b></color>");

                                        ConfigNode craftData = node.AddNode("HC" + _hcCount + "OrXv" + count);
                                        craftData.AddValue("vesselName", v.Current.vesselName);
                                        ConfigNode location = craftData.AddNode("coords");
                                        location.AddValue("holo", _hcCount);
                                        location.AddValue("pas", Password);
                                        location.AddValue("lat", v.Current.latitude);
                                        location.AddValue("lon", v.Current.longitude);
                                        location.AddValue("alt", v.Current.altitude + 1);
                                        location.AddValue("left", _left);
                                        location.AddValue("pitch", _pitch);

                                        Quaternion or = toSave.rootPart.transform.rotation;
                                        location.AddValue("rot", or.x + "," + or.y + "," + or.z + "," + or.w);

                                        Debug.Log("[OrX Mission] Adding coords ............................. " + HoloCacheName);
                                        ConfigNode craftFile = craftData.AddNode("craft");
                                        ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloCacheName + "</b></color>");
                                        craftConstruct.CopyTo(craftFile);
                                        foreach (ConfigNode.Value cv in craftFile.values)
                                        {
                                            if (cv.name == "ship")
                                            {
                                                cv.value = v.Current.vesselName;
                                            }
                                        }

                                        // ADD ENCRYPTION

                                        foreach (ConfigNode.Value cv in location.values)
                                        {
                                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                            cv.name = cvEncryptedName;
                                            cv.value = cvEncryptedValue;
                                        }

                                        foreach (ConfigNode.Value cv in craftFile.values)
                                        {
                                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                            cv.name = cvEncryptedName;
                                            cv.value = cvEncryptedValue;
                                        }

                                        foreach (ConfigNode cn in craftFile.nodes)
                                        {
                                            foreach (ConfigNode.Value cv in cn.values)
                                            {
                                                string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                                string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                                cv.name = cvEncryptedName;
                                                cv.value = cvEncryptedValue;
                                            }

                                            foreach (ConfigNode cn2 in cn.nodes)
                                            {
                                                foreach (ConfigNode.Value cv2 in cn2.values)
                                                {
                                                    if (cv2.name != "currentRotation")
                                                    {
                                                        string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                                                        string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                                                        cv2.name = cvEncryptedName;
                                                        cv2.value = cvEncryptedValue;
                                                    }
                                                }
                                            }
                                        }

                                        saveShip = false;
                                        Debug.Log("[Module OrX HoloCache] " + v.Current.vesselName + " Saved to " + HoloCacheName);
                                        ScreenMsg("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.Log("[Module OrX HoloCache] EXCEPTION ======================== " + HoloCacheName);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[Module OrX HoloCache] EXCEPTION ======================== " + HoloCacheName);
                    }
                }
                v.Dispose();
                _file.Save(hConfigLoc);


                if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloCacheName))
                {
                    Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloCacheName);
                }

                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            }

            var h = _HoloKron.rootPart.FindModuleImplementing<ModuleOrXMission>();
            h.HoloCacheName = HoloCacheName;

            //var dh = challengeHolo.rootPart.FindModuleImplementing<ModuleOrXMission>();
            //dh.KillHolo();

            if (dakarSetup)
            {
                challengeHolo = _HoloKron;
                h._holoSetup = true;
                h.StartBuild();
            }
            else
            {
                h._holoSetup = false;
                dakarSetup = false;
                GuiEnabledOrXMissions = false;
                OrXHCGUIEnabled = false;
                OrXLog.instance.building = false;
                building = false;
                buildingMission = false;
                addCoords = false;
                PlayOrXMission = false;
                ResetData();
            }
        }

        public void SetupHolo()
        {
            ResetData();
            holoToAdd = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloCache/HoloCache.craft";
            missionType = "GEO-CACHE";
            challengeType = "GEO-CACHE";
            geoCache = true;
            locAdded = false;

            holocacheCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloCache/";
            holocacheFiles = new List<string>(Directory.GetFiles(holocacheCraftLoc, "*.craft", SearchOption.AllDirectories));

            sphLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/SPH/";
            sphFiles = new List<string>(Directory.GetFiles(sphLoc, "*.craft", SearchOption.AllDirectories));

            vabLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB/";
            vabFiles = new List<string>(Directory.GetFiles(vabLoc, "*.craft", SearchOption.AllDirectories));
            showTargetOnScreen = false;
            OrXLog.instance.building = true;
            building = true;
            buildingMission = true;
            showScores = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            craftBrowserOpen = false;
            spawned = true;
            OrXHCGUIEnabled = true;
            _latMission = tpoint.x;
            _lonMission = tpoint.y;
            _altMission = tpoint.z;
        }
        public void OpenHoloCache(string holoName, Vessel v)
        {
            _HoloKron = v;
            HoloCacheName = holoName;
            StartCoroutine(StartMission(holoName));
        }
        public IEnumerator StartMission(string holoName)
        {
            Debug.Log("[OrX Mission] === STARTING MISSION === "); ;

            //ResetData();
            yield return new WaitForFixedUpdate();
            building = false;
            coordCount = 0;
            _scoreboard = new List<string>();
            stageTimes = new List<string>();
            hcCount = 0;
            crafttosave = string.Empty;
            int c = 0;

            if (missionType != "GEO-CACHE")
            {
                OrXLog.instance.mission = true;
                geoCache = false;
                showScores = true;
            }
            else
            {
                geoCache = true;
                showScores = false;
                geoCache = true;
            }

            //geoCache = true;

            if (HoloCacheName != "")
            {
                ec = 0;
                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + holoName + "/" + holoName + ".orx");
                ConfigNode node = _file.GetNode("OrX");

                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                    {
                        Debug.Log("[OrX Mission] === FOUND HOLOCACHE === " + hcCount); ;

                        ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + hcCount);

                        foreach (ConfigNode.Value data in HoloCacheNode.values)
                        {
                            if (data.name == "spawned")
                            {
                                if (data.value == "False")
                                {
                                    Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has not spawned ... "); ;
                                    break;
                                }
                                else
                                {
                                    Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has spawned ... CHECKING FOR EXTRAS"); ;

                                    if (HoloCacheNode.HasValue("extras"))
                                    {
                                        var t = HoloCacheNode.GetValue("extras");
                                        if (t == "False")
                                        {
                                            Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION"); ;
                                            hcCount = int.MaxValue;
                                            break;
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;
                                            hcCount += 1;
                                        }
                                    }
                                }
                            }

                            if (data.name == "missionName")
                            {
                                missionName = data.value;
                            }

                            if (data.name == "missionType")
                            {
                                missionType = data.value;
                            }

                            if (data.name == "challengeType")
                            {
                                challengeType = data.value;
                            }

                            if (data.name == "missionDescription0")
                            {
                                missionDescription0 = data.value;
                            }

                            if (data.name == "missionDescription1")
                            {
                                missionDescription1 = data.value;
                            }

                            if (data.name == "missionDescription2")
                            {
                                missionDescription2 = data.value;
                            }

                            if (data.name == "missionDescription3")
                            {
                                missionDescription3 = data.value;
                            }

                            if (data.name == "missionDescription4")
                            {
                                missionDescription4 = data.value;
                            }
                            if (data.name == "missionDescription5")
                            {
                                missionDescription5 = data.value;
                            }
                            if (data.name == "missionDescription6")
                            {
                                missionDescription6 = data.value;
                            }
                            if (data.name == "missionDescription7")
                            {
                                missionDescription7 = data.value;
                            }

                            if (data.name == "missionDescription8")
                            {
                                missionDescription8 = data.value;
                            }
                            if (data.name == "missionDescription9")
                            {
                                missionDescription9 = data.value;
                            }

                            if (data.name == "Gold")
                            {
                                Gold = data.value;
                            }
                            if (data.name == "Silver")
                            {
                                Silver = data.value;
                            }
                            if (data.name == "Bronze")
                            {
                                Bronze = data.value;
                            }
                            if (data.name == "mCount")
                            {
                                mCount = int.Parse(data.value);
                            }
                        }

                        Debug.Log("[OrX Mission] === DATA PROCESSED ===");
                    }

                    if (spawnCheck.name.Contains("HC" + hcCount + "OrXv0"))
                    {
                        Debug.Log("[OrX Mission] === GRABBING CRAFT FILE FOR " + spawnCheck.name + " ===");

                        foreach (ConfigNode.Value cv in spawnCheck.values)
                        {
                            if (cv.name == "vesselName")
                            {
                                Debug.Log("[OrX Mission] === Blueprints found for '" + cv.value + "' ===");
                                blueprintsAdded = true;
                                crafttosave = cv.value;
                            }
                        }

                        Debug.Log("[OrX Mission] === GRABBING COORDS ===");

                        ConfigNode location = spawnCheck.GetNode("coords");
                        foreach (ConfigNode.Value loc in location.values)
                        {
                            string locEncryptedName = OrXLog.instance.Decrypt(loc.name);
                            if (locEncryptedName == "holo")
                            {
                                string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);

                                if (locEncryptedValue == hcCount.ToString())
                                {
                                    foreach (ConfigNode.Value _loc in location.values)
                                    {
                                        if (locEncryptedName == "pas")
                                        {
                                            pas = OrXLog.instance.Decrypt(_loc.value);
                                        }
                                    }
                                }
                            }
                        }

                        Debug.Log("[OrX Mission] === GRABBING CRAFT FILE DATA ===");

                        ConfigNode craftFile = spawnCheck.GetNode("craft");
                        foreach (ConfigNode.Value cv in craftFile.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        string _type = "";

                        foreach (ConfigNode.Value value in craftFile.values)
                        {
                            if (value.name == "type")
                            {
                                if (value.value == "SPH")
                                {
                                    _type = "SPH/";
                                }
                                if (value.value == "VAB")
                                {
                                    _type = "VAB/";
                                }
                            }
                        }

                        Debug.Log("[OrX Mission] === DECRYPTING CRAFT FILE DATA ===");

                        foreach (ConfigNode cn in craftFile.nodes)
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

                        _blueprints_ = craftFile;
                        blueprintsFile = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder
                            + "/Ships/" + _type + crafttosave + ".craft";

                        Debug.Log("[OrX Mission] === BLUEPRINTS READY ===");

                        ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + hcCount);
                        foreach (ConfigNode.Value cv in HoloCacheNode.values)
                        {
                            string a = OrXLog.instance.Decrypt(cv.name);

                            if (a == "tech")
                            {
                                string b = OrXLog.instance.Decrypt(cv.value);
                                if (b != "")
                                {
                                    techToAdd = b;

                                    if (OrXLog.instance.CheckTechList(techToAdd))
                                    {
                                        Debug.Log("[OrX Mission] " + HoloCacheName + " is adding " + techToAdd + " to the tech list ...");
                                        OrXLog.instance.AddTech(techToAdd);
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Mission] " + techToAdd + " is already in the tech list ...");
                                    }
                                }
                            }
                            if (a == "spawned")
                            {
                                cv.value = OrXLog.instance.Crypt("True");
                            }
                        }

                        _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
                        Debug.Log("[OrX Mission] " + HoloCacheName + " Saved Status - SPAWNED");
                    }
                }

                Debug.Log("[OrX Mission] === BLUEPRINTS PROCESSED ===");

                _mission = _file.GetNode("mission" + hcCount);
                if (_mission != null)
                {
                    Debug.Log("[OrX Mission] === MISSION " + hcCount + " FOUND ===");
                    CoordDatabase = new List<string>();

                    foreach (ConfigNode.Value cv in _mission.values)
                    {
                        if (cv.name.Contains("stage"))
                        {
                            CoordDatabase.Add(cv.value);
                            coordCount += 1;
                        }
                    }

                    Debug.Log("[OrX Mission] === FOUND " + coordCount + " COORDS IN MISSION " + hcCount + " ===");
                    List<Waypoint> waypoints = new List<Waypoint>();

                    if (CoordDatabase.Count >= 0)
                    {
                        System.Random r = new System.Random();
                        waypointColor = (int)(r.NextDouble() * seeds.Count());

                        List<string>.Enumerator coords = CoordDatabase.GetEnumerator();
                        while (coords.MoveNext())
                        {
                            try
                            {
                                string[] data = coords.Current.Split(new char[] { ',' });
                                Vector3d surfacePoint = new Vector3d(double.Parse(data[1]), double.Parse(data[2]), double.Parse(data[3]));
                                Waypoint waypoint = new Waypoint();
                                waypoint.id = "marker";
                                waypoint.seed = seeds[waypointColor];
                                waypoint.name = craftToSpawn;
                                waypoint.celestialName = name;
                                waypoint.longitude = FlightGlobals.ActiveVessel.mainBody.GetLongitude(surfacePoint);
                                waypoint.latitude = FlightGlobals.ActiveVessel.mainBody.GetLatitude(surfacePoint);
                                waypoint.height = TerrainHeight(latMission, lonMission, FlightGlobals.ActiveVessel.mainBody);
                                waypoint.altitude = FlightGlobals.ActiveVessel.mainBody.GetAltitude(surfacePoint) - waypoint.height;
                                waypoints.Add(waypoint);
                                Debug.Log("[OrX HoloCache] ====================================== ADDING WAYPOINT FOR " + craftToSpawn + gpsCount + " ===");
                            }
                            catch
                            {
                            }
                        }
                        coords.Dispose();

                        Debug.Log("[OrX Mission] === GETTING SCOREBOARD ===");
                        _scoreboard_ = _mission.GetNode("scoreboard");

                        // CHECK PODIUM LIST

                        scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                        scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                        scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                        scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                        scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                        scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                        scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                        scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                        scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                        scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                        foreach (ConfigNode.Value cv in scoreboard0.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB0 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB0 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard1.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB1 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB1 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard2.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB2 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB2 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard3.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB3 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB3 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard4.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB4 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB4 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard5.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB5 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB5 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard6.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB6 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB6 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard7.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB7 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB7 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard8.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB8 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB8 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard9.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB9 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB9 = cv.value;
                            }
                        }

                        Debug.Log("[OrX Mission] === SCOREBOARD GENERATED ===");
                    }
                }
            }

            ImportScores();

            Debug.Log("[OrX Mission] === SETTING UP HOLOCACHE FOR MISSION ===");

            var mom = _HoloKron.rootPart.FindModuleImplementing<ModuleOrXMission>();
            mom.completed = false;
            mom.missionName = missionName;
            mom.missionType = missionType;
            mom.challengeType = challengeType;
            mom.tech = tech;
            mom.mCount = mCount;
            mom.Gold = Gold;
            mom.Silver = Silver;
            mom.Bronze = Bronze;
            mom.blueprintsAdded = blueprintsAdded;

            Debug.Log("[OrX Mission] === OPENING " + HoloCacheName + " MISSION WINDOW ===");

            OrXHCGUIEnabled = true;
            showScores = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = true;
            craftBrowserOpen = false;
        }

        IEnumerator StartChallenge()
        {
            if (challengeRunning)
            {
                if (geoCache)
                {
                    challengeRunning = false;
                    PlayOrXMission = false;
                    showScores = false;
                    _blueprints_.Save(blueprintsFile);
                    ScreenMsg("'" + craftToAddMission + "' Blueprints Available");
                    Debug.Log("[OrX Holocache] === '" + craftToAddMission + "' Blueprints Available ===");
                    OrXLog.instance.mission = false;

                    _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
                    ConfigNode node = _file.GetNode("OrX");

                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                        {
                            ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + hcCount);

                            if (HoloCacheNode != null)
                            {
                                Debug.Log("[OrX Mission] === FOUND HOLOCACHE === " + hcCount); ;

                                if (HoloCacheNode.HasValue("completed"))
                                {
                                    var t = HoloCacheNode.GetValue("completed");
                                    if (t == "False")
                                    {
                                        HoloCacheNode.SetValue("completed", "True", true);

                                        Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " was not completed ... CHANGING TO TRUE"); ;
                                        break;
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " is already completed ... "); ;
                                        hcCount += 1;
                                    }
                                }

                                Debug.Log("[OrX Mission] === DATA PROCESSED ===");
                            }
                        }
                    }

                    _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
                }
                else
                {
                    List<Waypoint>.Enumerator _waypoint = waypoints.GetEnumerator();
                    while (_waypoint.MoveNext())
                    {
                        if (_waypoint.Current != null)
                        {
                            ScenarioCustomWaypoints.RemoveWaypoint(_waypoint.Current);
                            yield return new WaitForFixedUpdate();
                            ScenarioCustomWaypoints.AddWaypoint(_waypoint.Current);
                            yield return new WaitForFixedUpdate();
                        }
                    }
                    _waypoint.Dispose();

                    gpsCount = 0;
                    missionTime = HighLogic.CurrentGame.UniversalTime;
                    if (challengeType == "WIND RACING")
                    {
                        // SETUP WIND CONTROLLER
                        //GetNextCoord();

                    }
                    else
                    {
                        if (challengeType == "UNDERWATER")
                        {
                            // START A DEPTH MONITORING MONOBEHAVIOUR/GUI
                            // PRESSURE SENSOR/SLIDER 

                            // TO UNLOCK SCUBA KERB GO TO DIVE INTO MY HOLE LOCATION - ADD REASEARCH OUTPOST AND SPAWNED KERBAL FOR DIALOGUE
                            // BUCKEY BALLS AND LOTION - TAKE A SHOWER, PUT ON LOTION AND JUMP INTO THE HOLE OF MUD THEN START TUTORIAL

                            //GetNextCoord();
                        }
                        else
                        {
                            if (challengeType == "DAKAR RACING")
                            {
                                // START DAKAR RACING GUI
                                // ADD DAKAR, DRAG, SHORT AND LONG TRACK TYPES ???????
                                // INCORPORATE PULL N DRAG ????????
                                // REARVIEW MIRRORS FOR FIRST PERSON VIEW ??????
                                // INCLUDE BILL'S BIG BAD BEAVER ETC ??????

                                GetNextCoord();
                            }
                        }
                    }
                }
            }
        }
        private void SaveScore()
        {
            challengeRunning = false;
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
            _mission = _file.GetNode("mission" + mCount);
            _scoreboard_ = _mission.GetNode("scoreboard");
            Debug.Log("[OrX Mission Scoreboard] === GET CHALLENGER TOTAL TIME AND CREATE STAGE TIME LIST ===");

            // GET CHALLENGER TOTAL TIME AND CREAT STAGE TIME LIST
            int stageCount = 0;

            ConfigNode tempChallengerEntry = new ConfigNode();
            tempChallengerEntry.AddValue("challengersName", challengersName);
            double totalTimeChallenger = 0;
            List<string>.Enumerator st = stageTimes.GetEnumerator();
            while (st.MoveNext())
            {
                stageCount += 1;
                string[] data = st.Current.Split(new char[] { ',' });
                totalTimeChallenger += double.Parse(data[1]);
                tempChallengerEntry.AddValue("stage" + stageCount, double.Parse(data[1]));
            }
            tempChallengerEntry.AddValue("totalTime", totalTimeChallenger);

            Debug.Log("[OrX Mission Scoreboard] === STAGE TIME LIST CREATED ===");

            ConfigNode scores = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".scores");
            if (scores == null)
            {
                Debug.Log("[OrX Mission Scoreboard] === SCORES FILE DOESN'T EXIST ... CREATIING===");

                scores = new ConfigNode();
                scores.AddValue("name", HoloCacheName);

                scores.AddNode("mission" + mCount);
                ConfigNode mis = scores.GetNode("mission" + mCount);

                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                {
                    mis.AddValue(cv.name, cv.value);
                }

                scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".scores");
                Debug.Log("[OrX Mission Scoreboard] === STAGE TIME LIST CREATED ===");

            }
            else
            {
                ConfigNode mis = scores.GetNode("mission" + mCount);

                foreach (ConfigNode.Value cv in mis.values)
                {
                    if (cv.name == "totalTime")
                    {
                        if (Convert.ToDouble(cv.value) >= totalTimeChallenger)
                        {
                            mis.ClearData();
                            foreach (ConfigNode.Value cv2 in tempChallengerEntry.values)
                            {
                                mis.AddValue(cv2.name, cv2.value);
                            }
                        }
                    }
                }
            }

            scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".scores");
            Debug.Log("[OrX Mission Scoreboard] === STAGE TIME LIST SAVED TO SCORES FILE ===");

            // CHECK PODIUM LIST

            scoreboard0 = _scoreboard_.GetNode("scoreboard0");
            scoreboard1 = _scoreboard_.GetNode("scoreboard1");
            scoreboard2 = _scoreboard_.GetNode("scoreboard2");
            scoreboard3 = _scoreboard_.GetNode("scoreboard3");
            scoreboard4 = _scoreboard_.GetNode("scoreboard4");
            scoreboard5 = _scoreboard_.GetNode("scoreboard5");
            scoreboard6 = _scoreboard_.GetNode("scoreboard6");
            scoreboard7 = _scoreboard_.GetNode("scoreboard7");
            scoreboard8 = _scoreboard_.GetNode("scoreboard8");
            scoreboard9 = _scoreboard_.GetNode("scoreboard9");


            bool ammendListscoreboard0 = false;
            string nameToRemovescoreboard0 = string.Empty;
            double totalTimescoreboard0 = 0;
            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard0 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard0 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard0)
                        {
                            ammendListscoreboard0 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard0 = true;
                    }
                }
            }

            bool ammendListscoreboard1 = false;
            string nameToRemovescoreboard1 = string.Empty;
            double totalTimescoreboard1 = 0;
            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard1 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard1 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard1)
                        {
                            ammendListscoreboard1 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard1 = true;
                    }
                }
            }

            bool ammendListscoreboard2 = false;
            string nameToRemovescoreboard2 = string.Empty;
            double totalTimescoreboard2 = 0;
            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard2 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard2 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard2)
                        {
                            ammendListscoreboard2 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard2 = true;
                    }
                }
            }

            bool ammendListscoreboard3 = false;
            string nameToRemovescoreboard3 = string.Empty;
            double totalTimescoreboard3 = 0;
            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard3 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard3 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard3)
                        {
                            ammendListscoreboard3 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard3 = true;
                    }
                }
            }

            bool ammendListscoreboard4 = false;
            string nameToRemovescoreboard4 = string.Empty;
            double totalTimescoreboard4 = 0;
            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard4 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard4 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard4)
                        {
                            ammendListscoreboard4 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard4 = true;
                    }
                }
            }

            bool ammendListscoreboard5 = false;
            string nameToRemovescoreboard5 = string.Empty;
            double totalTimescoreboard5 = 0;
            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard5 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard5 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard5)
                        {
                            ammendListscoreboard5 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard5 = true;
                    }
                }
            }

            bool ammendListscoreboard6 = false;
            string nameToRemovescoreboard6 = string.Empty;
            double totalTimescoreboard6 = 0;
            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard6 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard6 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard6)
                        {
                            ammendListscoreboard6 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard6 = true;
                    }
                }
            }

            bool ammendListscoreboard7 = false;
            string nameToRemovescoreboard7 = string.Empty;
            double totalTimescoreboard7 = 0;
            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard7 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard7 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard7)
                        {
                            ammendListscoreboard7 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard7 = true;
                    }
                }
            }

            bool ammendListscoreboard8 = false;
            string nameToRemovescoreboard8 = string.Empty;
            double totalTimescoreboard8 = 0;
            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard8 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard8 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard8)
                        {
                            ammendListscoreboard8 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard8 = true;
                    }
                }
            }

            bool ammendListscoreboard9 = false;
            string nameToRemovescoreboard9 = string.Empty;
            double totalTimescoreboard9 = 0;
            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard9 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard9 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard9)
                        {
                            ammendListscoreboard9 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard9 = true;
                    }
                }
            }

            Debug.Log("[OrX Mission Scoreboard] === CHECKING SCOREBOARD ===");

            // EDIT PODIUM LIST SCORES IF NEDED

            if (ammendListscoreboard0)
            {
                scoreboard9.ClearData();
                foreach (ConfigNode.Value cv in scoreboard8.values)
                {
                    scoreboard9.AddValue(cv.name, cv.value);
                }

                scoreboard8.ClearData();
                foreach (ConfigNode.Value cv in scoreboard7.values)
                {
                    scoreboard8.AddValue(cv.name, cv.value);
                }

                scoreboard7.ClearData();
                foreach (ConfigNode.Value cv in scoreboard6.values)
                {
                    scoreboard7.AddValue(cv.name, cv.value);
                }

                scoreboard6.ClearData();
                foreach (ConfigNode.Value cv in scoreboard5.values)
                {
                    scoreboard6.AddValue(cv.name, cv.value);
                }

                scoreboard5.ClearData();
                foreach (ConfigNode.Value cv in scoreboard4.values)
                {
                    scoreboard5.AddValue(cv.name, cv.value);
                }

                scoreboard4.ClearData();
                foreach (ConfigNode.Value cv in scoreboard3.values)
                {
                    scoreboard4.AddValue(cv.name, cv.value);
                }

                scoreboard3.ClearData();
                foreach (ConfigNode.Value cv in scoreboard2.values)
                {
                    scoreboard3.AddValue(cv.name, cv.value);
                }

                scoreboard2.ClearData();
                foreach (ConfigNode.Value cv in scoreboard1.values)
                {
                    scoreboard2.AddValue(cv.name, cv.value);
                }

                scoreboard1.ClearData();
                foreach (ConfigNode.Value cv in scoreboard0.values)
                {
                    scoreboard1.AddValue(cv.name, cv.value);
                }

                scoreboard0.ClearData();
                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                {
                    scoreboard0.AddValue(cv.name, cv.value);
                }
                Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard0 ===");

            }
            else
            {
                if (ammendListscoreboard1)
                {
                    scoreboard9.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard8.values)
                    {
                        scoreboard9.AddValue(cv.name, cv.value);
                    }

                    scoreboard8.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard7.values)
                    {
                        scoreboard8.AddValue(cv.name, cv.value);
                    }

                    scoreboard7.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard6.values)
                    {
                        scoreboard7.AddValue(cv.name, cv.value);
                    }

                    scoreboard6.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard5.values)
                    {
                        scoreboard6.AddValue(cv.name, cv.value);
                    }

                    scoreboard5.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard4.values)
                    {
                        scoreboard5.AddValue(cv.name, cv.value);
                    }

                    scoreboard4.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard3.values)
                    {
                        scoreboard4.AddValue(cv.name, cv.value);
                    }

                    scoreboard3.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard2.values)
                    {
                        scoreboard3.AddValue(cv.name, cv.value);
                    }

                    scoreboard2.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard1.values)
                    {
                        scoreboard2.AddValue(cv.name, cv.value);
                    }

                    scoreboard1.ClearData();
                    foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                    {
                        scoreboard1.AddValue(cv.name, cv.value);
                    }
                    Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard1 ===");

                }
                else
                {
                    if (ammendListscoreboard2)
                    {
                        scoreboard9.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard8.values)
                        {
                            scoreboard9.AddValue(cv.name, cv.value);
                        }

                        scoreboard8.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard7.values)
                        {
                            scoreboard8.AddValue(cv.name, cv.value);
                        }

                        scoreboard7.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard6.values)
                        {
                            scoreboard7.AddValue(cv.name, cv.value);
                        }

                        scoreboard6.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard5.values)
                        {
                            scoreboard6.AddValue(cv.name, cv.value);
                        }

                        scoreboard5.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard4.values)
                        {
                            scoreboard5.AddValue(cv.name, cv.value);
                        }

                        scoreboard4.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard3.values)
                        {
                            scoreboard4.AddValue(cv.name, cv.value);
                        }

                        scoreboard3.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard2.values)
                        {
                            scoreboard3.AddValue(cv.name, cv.value);
                        }

                        scoreboard2.ClearData();
                        foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                        {
                            scoreboard2.AddValue(cv.name, cv.value);
                        }
                        Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard2 ===");

                    }
                    else
                    {
                        if (ammendListscoreboard3)
                        {
                            scoreboard9.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard8.values)
                            {
                                scoreboard9.AddValue(cv.name, cv.value);
                            }

                            scoreboard8.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard7.values)
                            {
                                scoreboard8.AddValue(cv.name, cv.value);
                            }

                            scoreboard7.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard6.values)
                            {
                                scoreboard7.AddValue(cv.name, cv.value);
                            }

                            scoreboard6.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard5.values)
                            {
                                scoreboard6.AddValue(cv.name, cv.value);
                            }

                            scoreboard5.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard4.values)
                            {
                                scoreboard5.AddValue(cv.name, cv.value);
                            }

                            scoreboard4.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard3.values)
                            {
                                scoreboard4.AddValue(cv.name, cv.value);
                            }

                            scoreboard3.ClearData();
                            foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                            {
                                scoreboard3.AddValue(cv.name, cv.value);
                            }
                            Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard3 ===");

                        }
                        else
                        {
                            if (ammendListscoreboard4)
                            {
                                scoreboard9.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard8.values)
                                {
                                    scoreboard9.AddValue(cv.name, cv.value);
                                }

                                scoreboard8.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard7.values)
                                {
                                    scoreboard8.AddValue(cv.name, cv.value);
                                }

                                scoreboard7.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard6.values)
                                {
                                    scoreboard7.AddValue(cv.name, cv.value);
                                }

                                scoreboard6.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard5.values)
                                {
                                    scoreboard6.AddValue(cv.name, cv.value);
                                }

                                scoreboard5.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard4.values)
                                {
                                    scoreboard5.AddValue(cv.name, cv.value);
                                }

                                scoreboard4.ClearData();
                                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                {
                                    scoreboard4.AddValue(cv.name, cv.value);
                                }
                                Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard4 ===");

                            }
                            else
                            {
                                if (ammendListscoreboard5)
                                {
                                    scoreboard9.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard8.values)
                                    {
                                        scoreboard9.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard8.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard7.values)
                                    {
                                        scoreboard8.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard7.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard6.values)
                                    {
                                        scoreboard7.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard6.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard5.values)
                                    {
                                        scoreboard6.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard5.ClearData();
                                    foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                    {
                                        scoreboard5.AddValue(cv.name, cv.value);
                                    }
                                    Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard5 ===");

                                }
                                else
                                {
                                    if (ammendListscoreboard6)
                                    {
                                        scoreboard9.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard8.values)
                                        {
                                            scoreboard9.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard8.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard7.values)
                                        {
                                            scoreboard8.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard7.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard6.values)
                                        {
                                            scoreboard7.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard6.ClearData();
                                        foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                        {
                                            scoreboard6.AddValue(cv.name, cv.value);
                                        }
                                        Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard6 ===");

                                    }
                                    else
                                    {
                                        if (ammendListscoreboard7)
                                        {
                                            scoreboard9.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard8.values)
                                            {
                                                scoreboard9.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard8.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard7.values)
                                            {
                                                scoreboard8.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard7.ClearData();
                                            foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                            {
                                                scoreboard7.AddValue(cv.name, cv.value);
                                            }
                                            Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard7 ===");

                                        }
                                        else
                                        {
                                            if (ammendListscoreboard8)
                                            {
                                                scoreboard9.ClearData();
                                                foreach (ConfigNode.Value cv in scoreboard8.values)
                                                {
                                                    scoreboard9.AddValue(cv.name, cv.value);
                                                }

                                                scoreboard8.ClearData();
                                                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                                {
                                                    scoreboard8.AddValue(cv.name, cv.value);
                                                }
                                                Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard8 ===");

                                            }
                                            else
                                            {
                                                if (ammendListscoreboard9)
                                                {
                                                    scoreboard9.ClearData();
                                                    foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                                    {
                                                        scoreboard9.AddValue(cv.name, cv.value);
                                                    }
                                                    Debug.Log("[OrX Mission Scoreboard] === PODIUM CHANGE ... ammendListscoreboard9 ===");

                                                }
                                                else
                                                {
                                                    Debug.Log("[OrX Mission Scoreboard] === NO CHANGES TO PODIUM ===");
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

            Debug.Log("[OrX Mission Scoreboard] === REBUILDING SCOREBOARD ===");

            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameSB0 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB0 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameSB1 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB1 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameSB2 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB2 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameSB3 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB3 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameSB4 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB4 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameSB5 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB5 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameSB6 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB6 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameSB7 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB7 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameSB8 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB8 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameSB9 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB9 = cv.value;
                }
            }

            ConfigNode node = _file.GetNode("OrX");

            foreach (ConfigNode spawnCheck in node.nodes)
            {
                if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                {
                    ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + hcCount);

                    if (HoloCacheNode != null)
                    {
                        Debug.Log("[OrX Mission] === FOUND HOLOCACHE === " + hcCount); ;

                        if (HoloCacheNode.HasValue("completed"))
                        {
                            var t = HoloCacheNode.GetValue("completed");
                            if (t == "False")
                            {
                                HoloCacheNode.SetValue("completed", "True", true);

                                Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " was not completed ... CHANGING TO TRUE"); ;
                                break;
                            }
                            else
                            {
                                Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " was already completed ... "); ;
                                hcCount += 1;
                            }
                        }

                        Debug.Log("[OrX Mission] === DATA PROCESSED ===");
                    }
                }
            }

            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
            Debug.Log("[OrX Mission Scoreboard] === SCOREBOARD SAVED ===");
        }
        private void ImportScores()
        {
            updatingScores = false;

            Debug.Log("[OrX Mission] === CHECKING FOR SCORE IMPORT FILES ===");

            ConfigNode scores = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Import/" + HoloCacheName + ".scores");
            if (scores != null)
            {
                Debug.Log("[OrX Mission] === SCORE IMPORT FILE FOR " + HoloCacheName + " FOUND ===");

                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".scores");
                _mission = _file.GetNode("mission" + mCount);
                _scoreboard_ = _mission.GetNode("scoreboard");

                // GET PODIUM LIST

                scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                foreach (ConfigNode cn in scores.nodes)
                {
                    if (cn.name.Contains("mission"))
                    {
                        string missionNumber = cn.name;
                        ConfigNode temp = null;
                        double t = 0;
                        int sc = 0;
                        List<string> _stageTimes = new List<string>();

                        foreach (ConfigNode.Value cv in cn.values)
                        {
                            _stageTimes.Add(cv.value);
                        }

                        List<string>.Enumerator times = _stageTimes.GetEnumerator();
                        while (times.MoveNext())
                        {
                            sc += 1;
                            string[] data = times.Current.Split(new char[] { ',' });
                            t += double.Parse(data[1]);
                            temp.AddValue("stage" + sc, double.Parse(data[1]));
                        }
                        temp.AddValue("totalTime", t);

                        bool ammendListscoreboard0 = false;
                        string nameToRemovescoreboard0 = string.Empty;
                        double totalTimescoreboard0 = 0;
                        foreach (ConfigNode.Value cv in scoreboard0.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard0 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard0 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard0)
                                    {
                                        ammendListscoreboard0 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard0 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard1 = false;
                        string nameToRemovescoreboard1 = string.Empty;
                        double totalTimescoreboard1 = 0;
                        foreach (ConfigNode.Value cv in scoreboard1.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard1 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard1 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard1)
                                    {
                                        ammendListscoreboard1 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard1 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard2 = false;
                        string nameToRemovescoreboard2 = string.Empty;
                        double totalTimescoreboard2 = 0;
                        foreach (ConfigNode.Value cv in scoreboard2.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard2 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard2 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard2)
                                    {
                                        ammendListscoreboard2 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard2 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard3 = false;
                        string nameToRemovescoreboard3 = string.Empty;
                        double totalTimescoreboard3 = 0;
                        foreach (ConfigNode.Value cv in scoreboard3.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard3 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard3 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard3)
                                    {
                                        ammendListscoreboard3 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard3 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard4 = false;
                        string nameToRemovescoreboard4 = string.Empty;
                        double totalTimescoreboard4 = 0;
                        foreach (ConfigNode.Value cv in scoreboard4.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard4 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard4 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard4)
                                    {
                                        ammendListscoreboard4 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard4 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard5 = false;
                        string nameToRemovescoreboard5 = string.Empty;
                        double totalTimescoreboard5 = 0;
                        foreach (ConfigNode.Value cv in scoreboard5.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard5 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard5 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard5)
                                    {
                                        ammendListscoreboard5 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard5 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard6 = false;
                        string nameToRemovescoreboard6 = string.Empty;
                        double totalTimescoreboard6 = 0;
                        foreach (ConfigNode.Value cv in scoreboard6.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard6 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard6 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard6)
                                    {
                                        ammendListscoreboard6 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard6 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard7 = false;
                        string nameToRemovescoreboard7 = string.Empty;
                        double totalTimescoreboard7 = 0;
                        foreach (ConfigNode.Value cv in scoreboard7.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard7 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard7 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard7)
                                    {
                                        ammendListscoreboard7 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard7 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard8 = false;
                        string nameToRemovescoreboard8 = string.Empty;
                        double totalTimescoreboard8 = 0;
                        foreach (ConfigNode.Value cv in scoreboard8.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard8 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard8 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard8)
                                    {
                                        ammendListscoreboard8 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard8 = true;
                                }
                            }
                        }

                        bool ammendListscoreboard9 = false;
                        string nameToRemovescoreboard9 = string.Empty;
                        double totalTimescoreboard9 = 0;
                        foreach (ConfigNode.Value cv in scoreboard9.values)
                        {
                            if (cv.name == "name")
                            {
                                nameToRemovescoreboard9 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                if (cv.value != "" || cv.value != string.Empty)
                                {
                                    totalTimescoreboard9 = double.Parse(cv.value);
                                    if (t <= totalTimescoreboard9)
                                    {
                                        ammendListscoreboard9 = true;
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard9 = true;
                                }
                            }
                        }

                        // EDIT PODIUM LIST SCORES IF NEDED

                        if (ammendListscoreboard0)
                        {
                            scoreboard9.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard8.values)
                            {
                                scoreboard9.AddValue(cv.name, cv.value);
                            }

                            scoreboard8.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard7.values)
                            {
                                scoreboard8.AddValue(cv.name, cv.value);
                            }

                            scoreboard7.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard6.values)
                            {
                                scoreboard7.AddValue(cv.name, cv.value);
                            }

                            scoreboard6.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard5.values)
                            {
                                scoreboard6.AddValue(cv.name, cv.value);
                            }

                            scoreboard5.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard4.values)
                            {
                                scoreboard5.AddValue(cv.name, cv.value);
                            }

                            scoreboard4.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard3.values)
                            {
                                scoreboard4.AddValue(cv.name, cv.value);
                            }

                            scoreboard3.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard2.values)
                            {
                                scoreboard3.AddValue(cv.name, cv.value);
                            }

                            scoreboard2.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard1.values)
                            {
                                scoreboard2.AddValue(cv.name, cv.value);
                            }

                            scoreboard1.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard0.values)
                            {
                                scoreboard1.AddValue(cv.name, cv.value);
                            }

                            scoreboard0.ClearData();
                            foreach (ConfigNode.Value cv in temp.values)
                            {
                                scoreboard0.AddValue(cv.name, cv.value);
                            }
                        }
                        else
                        {
                            if (ammendListscoreboard1)
                            {
                                scoreboard9.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard8.values)
                                {
                                    scoreboard9.AddValue(cv.name, cv.value);
                                }

                                scoreboard8.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard7.values)
                                {
                                    scoreboard8.AddValue(cv.name, cv.value);
                                }

                                scoreboard7.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard6.values)
                                {
                                    scoreboard7.AddValue(cv.name, cv.value);
                                }

                                scoreboard6.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard5.values)
                                {
                                    scoreboard6.AddValue(cv.name, cv.value);
                                }

                                scoreboard5.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard4.values)
                                {
                                    scoreboard5.AddValue(cv.name, cv.value);
                                }

                                scoreboard4.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard3.values)
                                {
                                    scoreboard4.AddValue(cv.name, cv.value);
                                }

                                scoreboard3.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard2.values)
                                {
                                    scoreboard3.AddValue(cv.name, cv.value);
                                }

                                scoreboard2.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard1.values)
                                {
                                    scoreboard2.AddValue(cv.name, cv.value);
                                }

                                scoreboard1.ClearData();
                                foreach (ConfigNode.Value cv in temp.values)
                                {
                                    scoreboard1.AddValue(cv.name, cv.value);
                                }
                            }
                            else
                            {
                                if (ammendListscoreboard2)
                                {
                                    scoreboard9.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard8.values)
                                    {
                                        scoreboard9.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard8.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard7.values)
                                    {
                                        scoreboard8.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard7.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard6.values)
                                    {
                                        scoreboard7.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard6.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard5.values)
                                    {
                                        scoreboard6.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard5.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard4.values)
                                    {
                                        scoreboard5.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard4.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard3.values)
                                    {
                                        scoreboard4.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard3.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard2.values)
                                    {
                                        scoreboard3.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard2.ClearData();
                                    foreach (ConfigNode.Value cv in temp.values)
                                    {
                                        scoreboard2.AddValue(cv.name, cv.value);
                                    }
                                }
                                else
                                {
                                    if (ammendListscoreboard3)
                                    {
                                        scoreboard9.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard8.values)
                                        {
                                            scoreboard9.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard8.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard7.values)
                                        {
                                            scoreboard8.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard7.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard6.values)
                                        {
                                            scoreboard7.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard6.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard5.values)
                                        {
                                            scoreboard6.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard5.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard4.values)
                                        {
                                            scoreboard5.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard4.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard3.values)
                                        {
                                            scoreboard4.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard3.ClearData();
                                        foreach (ConfigNode.Value cv in temp.values)
                                        {
                                            scoreboard3.AddValue(cv.name, cv.value);
                                        }
                                    }
                                    else
                                    {
                                        if (ammendListscoreboard4)
                                        {
                                            scoreboard9.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard8.values)
                                            {
                                                scoreboard9.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard8.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard7.values)
                                            {
                                                scoreboard8.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard7.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard6.values)
                                            {
                                                scoreboard7.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard6.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard5.values)
                                            {
                                                scoreboard6.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard5.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard4.values)
                                            {
                                                scoreboard5.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard4.ClearData();
                                            foreach (ConfigNode.Value cv in temp.values)
                                            {
                                                scoreboard4.AddValue(cv.name, cv.value);
                                            }
                                        }
                                        else
                                        {
                                            if (ammendListscoreboard5)
                                            {
                                                scoreboard9.ClearData();
                                                foreach (ConfigNode.Value cv in scoreboard8.values)
                                                {
                                                    scoreboard9.AddValue(cv.name, cv.value);
                                                }

                                                scoreboard8.ClearData();
                                                foreach (ConfigNode.Value cv in scoreboard7.values)
                                                {
                                                    scoreboard8.AddValue(cv.name, cv.value);
                                                }

                                                scoreboard7.ClearData();
                                                foreach (ConfigNode.Value cv in scoreboard6.values)
                                                {
                                                    scoreboard7.AddValue(cv.name, cv.value);
                                                }

                                                scoreboard6.ClearData();
                                                foreach (ConfigNode.Value cv in scoreboard5.values)
                                                {
                                                    scoreboard6.AddValue(cv.name, cv.value);
                                                }

                                                scoreboard5.ClearData();
                                                foreach (ConfigNode.Value cv in temp.values)
                                                {
                                                    scoreboard5.AddValue(cv.name, cv.value);
                                                }
                                            }
                                            else
                                            {
                                                if (ammendListscoreboard6)
                                                {
                                                    scoreboard9.ClearData();
                                                    foreach (ConfigNode.Value cv in scoreboard8.values)
                                                    {
                                                        scoreboard9.AddValue(cv.name, cv.value);
                                                    }

                                                    scoreboard8.ClearData();
                                                    foreach (ConfigNode.Value cv in scoreboard7.values)
                                                    {
                                                        scoreboard8.AddValue(cv.name, cv.value);
                                                    }

                                                    scoreboard7.ClearData();
                                                    foreach (ConfigNode.Value cv in scoreboard6.values)
                                                    {
                                                        scoreboard7.AddValue(cv.name, cv.value);
                                                    }

                                                    scoreboard6.ClearData();
                                                    foreach (ConfigNode.Value cv in temp.values)
                                                    {
                                                        scoreboard6.AddValue(cv.name, cv.value);
                                                    }
                                                }
                                                else
                                                {
                                                    if (ammendListscoreboard7)
                                                    {
                                                        scoreboard9.ClearData();
                                                        foreach (ConfigNode.Value cv in scoreboard8.values)
                                                        {
                                                            scoreboard9.AddValue(cv.name, cv.value);
                                                        }

                                                        scoreboard8.ClearData();
                                                        foreach (ConfigNode.Value cv in scoreboard7.values)
                                                        {
                                                            scoreboard8.AddValue(cv.name, cv.value);
                                                        }

                                                        scoreboard7.ClearData();
                                                        foreach (ConfigNode.Value cv in temp.values)
                                                        {
                                                            scoreboard7.AddValue(cv.name, cv.value);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ammendListscoreboard8)
                                                        {
                                                            scoreboard9.ClearData();
                                                            foreach (ConfigNode.Value cv in scoreboard8.values)
                                                            {
                                                                scoreboard9.AddValue(cv.name, cv.value);
                                                            }

                                                            scoreboard8.ClearData();
                                                            foreach (ConfigNode.Value cv in temp.values)
                                                            {
                                                                scoreboard8.AddValue(cv.name, cv.value);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ammendListscoreboard9)
                                                            {
                                                                scoreboard9.ClearData();
                                                                foreach (ConfigNode.Value cv in temp.values)
                                                                {
                                                                    scoreboard9.AddValue(cv.name, cv.value);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // NO CHANGE TO PODIUM
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

                        foreach (ConfigNode.Value cv in scoreboard0.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB0 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB0 = cv.value;
                            }
                        }


                        foreach (ConfigNode.Value cv in scoreboard1.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB1 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB1 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard2.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB2 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB2 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard3.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB3 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB3 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard4.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB4 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB4 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard5.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB5 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB5 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard6.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB6 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB6 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard7.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB7 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB7 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard8.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB8 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB8 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard9.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB9 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB9 = cv.value;
                            }
                        }
                    }
                }
            }

            updatingScores = false;
        }
        private void GetScoreboardData()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
            _mission = _file.GetNode("mission" + mCount);
            _scoreboard_ = _mission.GetNode("scoreboard");
            int sbCount = 0;

            if (_scoreboard_.nodes.Contains("scoreboard0"))
            {
                // DO NOTHING
            }
            else  // ADD NEW PODIUM LIST
            {
                _scoreboard_.AddNode("scoreboard0");
                _scoreboard_.AddNode("scoreboard1");
                _scoreboard_.AddNode("scoreboard2");
                _scoreboard_.AddNode("scoreboard3");
                _scoreboard_.AddNode("scoreboard4");
                _scoreboard_.AddNode("scoreboard5");
                _scoreboard_.AddNode("scoreboard6");
                _scoreboard_.AddNode("scoreboard7");
                _scoreboard_.AddNode("scoreboard8");
                _scoreboard_.AddNode("scoreboard9");

                scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                scoreboard0.AddValue("name", "<empty>");
                scoreboard0.AddValue("time", "");
                scoreboard1.AddValue("name", "<empty>");
                scoreboard1.AddValue("time", "");
                scoreboard2.AddValue("name", "<empty>");
                scoreboard2.AddValue("time", "");
                scoreboard3.AddValue("name", "<empty>");
                scoreboard3.AddValue("time", "");
                scoreboard4.AddValue("name", "<empty>");
                scoreboard4.AddValue("time", "");
                scoreboard5.AddValue("name", "<empty>");
                scoreboard5.AddValue("time", "");
                scoreboard6.AddValue("name", "<empty>");
                scoreboard6.AddValue("time", "");
                scoreboard7.AddValue("name", "<empty>");
                scoreboard7.AddValue("time", "");
                scoreboard8.AddValue("name", "<empty>");
                scoreboard8.AddValue("time", "");
                scoreboard9.AddValue("name", "<empty>");
                scoreboard9.AddValue("time", "");

                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
            }

            // CHECK PODIUM LIST
            scoreboard0 = _scoreboard_.GetNode("scoreboard0");
            scoreboard1 = _scoreboard_.GetNode("scoreboard1");
            scoreboard2 = _scoreboard_.GetNode("scoreboard2");
            scoreboard3 = _scoreboard_.GetNode("scoreboard3");
            scoreboard4 = _scoreboard_.GetNode("scoreboard4");
            scoreboard5 = _scoreboard_.GetNode("scoreboard5");
            scoreboard6 = _scoreboard_.GetNode("scoreboard6");
            scoreboard7 = _scoreboard_.GetNode("scoreboard7");
            scoreboard8 = _scoreboard_.GetNode("scoreboard8");
            scoreboard9 = _scoreboard_.GetNode("scoreboard9");

            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameSB0 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB0 = cv.value;
                }
            }


            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameSB1 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB1 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameSB2 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB2 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameSB3 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB3 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameSB4 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB4 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameSB5 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB5 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameSB6 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB6 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameSB7 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB7 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameSB8 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB8 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameSB9 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB9 = cv.value;
                }
            }
            updatingScores = false;
            showScores = true;
        }

        public void CancelHoloCreation()
        {
            var h = _HoloKron.rootPart.FindModuleImplementing<ModuleOrXMission>();
            h.boid = true;
            h.HideHolo();
            FlightGlobals.ForceSetActiveVessel(triggerVessel);
            ResetData();
        }

        #endregion

        #region Location Targeting

        bool scanningForHolo = false;

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
        public static string FormattedGeoPosShort(Vector3d geoPos, bool getaltitude)
        {
            string finalString = string.Empty;
            //lat
            double lat = geoPos.x;
            double latSign = Math.Sign(lat);
            double latMajor = latSign * Math.Floor(Math.Abs(lat));
            double latMinor = 100 * (Math.Abs(lat) - Math.Abs(latMajor));
            string latString = latMajor.ToString("0") + "." + latMinor.ToString("0");
            finalString += "N:" + latString;


            //longi
            double longi = geoPos.y;
            double longiSign = Math.Sign(longi);
            double longiMajor = longiSign * Math.Floor(Math.Abs(longi));
            double longiMinor = 100 * (Math.Abs(longi) - Math.Abs(longiMajor));
            string longiString = longiMajor.ToString("0") + "." + longiMinor.ToString("0");
            finalString += " E:" + longiString;

            if (getaltitude)
            {
                finalString += " ASL:" + geoPos.z.ToString("0");
            }

            return finalString;
        }
        public static void DrawTextureOnWorldPos(Vector3 loc, Texture texture, Vector2 size)
        {
            Vector3 screenPos = GetMainCamera().WorldToViewportPoint(loc);
            if (screenPos.z < 0) return; //dont draw if point is behind camera
            if (screenPos.x != Mathf.Clamp01(screenPos.x)) return; //dont draw if off screen
            if (screenPos.y != Mathf.Clamp01(screenPos.y)) return;
            float xPos = screenPos.x * Screen.width - (0.5f * size.x);
            float yPos = (1 - screenPos.y) * Screen.height - (0.5f * size.y);
            Rect iconRect = new Rect(xPos, yPos, size.x, size.y);
            GUI.DrawTexture(iconRect, texture);
        }

        bool showTargetOnScreen = false;

        private void TargetDistance()
        {
            checking = true;
            scanning = true;
            reloadWorldPos = true;
            showTargetOnScreen = true;
            building = false;
            buildingMission = false;
            addCoords = false;
            GuiEnabledOrXMissions = false;
            OrXHCGUIEnabled = false;
            PlayOrXMission = false;
            StartCoroutine(CheckTargetDistance(false));
        }
        IEnumerator CheckTargetDistance(bool b)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                yield return new WaitForFixedUpdate();
                if (checking)
                {
                    mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
                    degPerMeter = 1 / mPerDegree;
                    scanDelay = Convert.ToSingle(targetDistance / FlightGlobals.ActiveVessel.srfSpeed) / 10;
                    targetDistance = double.MaxValue;
                    double _latDiff = 0;
                    double _lonDiff = 0;
                    double _altDiff = 0;

                    if (!b)
                    {
                        showTargetOnScreen = false;
                        Vector3 UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
                        Vector3 EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
                        Vector3 NorthVect = Vector3.Cross(EastVect, UpVect).normalized;

                        List<OrXHoloCacheinfo>.Enumerator coordinate = HoloCacheTargets[coords].GetEnumerator();
                        while (coordinate.MoveNext())
                        {

                            if (FlightGlobals.ActiveVessel.altitude <= coordinate.Current.gpsCoordinates.z)
                            {
                                _altDiff = coordinate.Current.gpsCoordinates.z - FlightGlobals.ActiveVessel.altitude;
                            }
                            else
                            {
                                _altDiff = FlightGlobals.ActiveVessel.altitude - coordinate.Current.gpsCoordinates.z;
                            }

                            if (coordinate.Current.gpsCoordinates.x >= 0)
                            {
                                if (FlightGlobals.ActiveVessel.latitude >= coordinate.Current.gpsCoordinates.x)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - coordinate.Current.gpsCoordinates.x;
                                }
                                else
                                {
                                    _latDiff = coordinate.Current.gpsCoordinates.x - FlightGlobals.ActiveVessel.latitude;
                                }
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.latitude >= 0)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - coordinate.Current.gpsCoordinates.x;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.latitude <= coordinate.Current.gpsCoordinates.x)
                                    {
                                        _latDiff = FlightGlobals.ActiveVessel.latitude - coordinate.Current.gpsCoordinates.x;
                                    }
                                    else
                                    {

                                        _latDiff = coordinate.Current.gpsCoordinates.x - FlightGlobals.ActiveVessel.latitude;
                                    }
                                }
                            }

                            if (coordinate.Current.gpsCoordinates.y >= 0)
                            {
                                if (FlightGlobals.ActiveVessel.longitude >= coordinate.Current.gpsCoordinates.y)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - coordinate.Current.gpsCoordinates.y;
                                }
                                else
                                {
                                    _lonDiff = coordinate.Current.gpsCoordinates.y - FlightGlobals.ActiveVessel.latitude;
                                }
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.longitude >= 0)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - coordinate.Current.gpsCoordinates.y;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.longitude <= coordinate.Current.gpsCoordinates.y)
                                    {
                                        _lonDiff = FlightGlobals.ActiveVessel.longitude - coordinate.Current.gpsCoordinates.y;
                                    }
                                    else
                                    {

                                        _lonDiff = coordinate.Current.gpsCoordinates.y - FlightGlobals.ActiveVessel.longitude;
                                    }
                                }
                            }

                            double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                            double _altDiffDeg = _altDiff * degPerMeter;
                            double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                            double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                            if (targetDistance >= _targetDistance)
                            {
                                targetDistance = _targetDistance;
                                Vector3 targetVect = (FlightGlobals.ActiveVessel.transform.position - coordinate.Current.gpsCoordinates).normalized;
                                heading = Vector3.Angle(targetVect, NorthVect);

                                if (Math.Sign(Vector3.Dot(targetVect, EastVect)) < 0)
                                {
                                    heading = 360 - heading; // westward headings become angles greater than 180
                                }
                            }

                            if (scanDelay >= 120)
                            {
                                scanDelay = 120;
                            }

                            if (_targetDistance <= 2000)
                            {
                                targetDistance = _targetDistance;

                                Debug.Log("======================================= TARGET Name: " + coordinate.Current.name);

                                Debug.Log("======================================= TARGET Meters per Degree: " + mPerDegree);
                                Debug.Log("======================================= TARGET Degrees per Meter: " + degPerMeter);
                                Debug.Log("======================================= TARGET Degree offset 2D: " + Math.Sqrt(diffSqr));
                                Debug.Log("======================================= TARGET Degree offset 3D: " + Math.Sqrt(altAdded));
                                Debug.Log("======================================= TARGET Distance in Meters: " + _targetDistance);

                                scanning = false;
                                checking = false;
                                missionHoloSpawned = true;
                                nextLocation = coordinate.Current.gpsCoordinates;
                                StartCoroutine(SpawnHoloCache(coordinate.Current.gpsCoordinates, false, false, coordinate.Current.name));
                                checking = false;
                                break;
                            }
                            else
                            {
                                Debug.Log("======================================= TARGET Name: " + coordinate.Current.name);
                                Debug.Log("======================================= TARGET Meters per Degree: " + mPerDegree);
                                Debug.Log("======================================= TARGET Degrees per Meter: " + degPerMeter);
                                Debug.Log("======================================= TARGET Degree offset 2D: " + Math.Sqrt(diffSqr));
                                Debug.Log("======================================= TARGET Degree offset 3D: " + Math.Sqrt(altAdded));
                                Debug.Log("======================================= TARGET Distance in Meters: " + _targetDistance);
                            }
                        }
                        coordinate.Dispose();
                    }
                    else
                    {
                        showTargetOnScreen = true;

                        if (FlightGlobals.ActiveVessel.altitude <= nextLocation.z)
                        {
                            _altDiff = nextLocation.z - FlightGlobals.ActiveVessel.altitude;
                        }
                        else
                        {
                            _altDiff = FlightGlobals.ActiveVessel.altitude - nextLocation.z;
                        }

                        if (nextLocation.x >= 0)
                        {
                            if (FlightGlobals.ActiveVessel.latitude >= nextLocation.x)
                            {
                                _latDiff = FlightGlobals.ActiveVessel.latitude - nextLocation.x;
                            }
                            else
                            {
                                _latDiff = nextLocation.x - FlightGlobals.ActiveVessel.latitude;
                            }
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.latitude >= 0)
                            {
                                _latDiff = FlightGlobals.ActiveVessel.latitude - nextLocation.x;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.latitude <= nextLocation.x)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - nextLocation.x;
                                }
                                else
                                {

                                    _latDiff = nextLocation.x - FlightGlobals.ActiveVessel.latitude;
                                }
                            }
                        }

                        if (nextLocation.y >= 0)
                        {
                            if (FlightGlobals.ActiveVessel.longitude >= nextLocation.y)
                            {
                                _lonDiff = FlightGlobals.ActiveVessel.longitude - nextLocation.y;
                            }
                            else
                            {
                                _lonDiff = nextLocation.y - FlightGlobals.ActiveVessel.latitude;
                            }
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.longitude >= 0)
                            {
                                _lonDiff = FlightGlobals.ActiveVessel.longitude - nextLocation.y;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.longitude <= nextLocation.y)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - nextLocation.y;
                                }
                                else
                                {

                                    _lonDiff = nextLocation.y - FlightGlobals.ActiveVessel.longitude;
                                }
                            }
                        }

                        double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                        double _altDiffDeg = _altDiff * degPerMeter;
                        double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                        double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                        if (targetDistance >= _targetDistance)
                        {
                            targetDistance = _targetDistance;
                        }

                        if (scanDelay >= 60)
                        {
                            scanDelay = 60;
                        }

                        if (_targetDistance <= 2000)
                        {
                            Debug.Log("======================================= TARGET Name: " + HoloCacheName + gpsCount);

                            Debug.Log("======================================= TARGET Meters per Degree: " + mPerDegree);
                            Debug.Log("======================================= TARGET Degrees per Meter: " + degPerMeter);
                            Debug.Log("======================================= TARGET Degree offset 2D: " + Math.Sqrt(diffSqr));
                            Debug.Log("======================================= TARGET Degree offset 3D: " + Math.Sqrt(altAdded));
                            Debug.Log("======================================= TARGET Distance in Meters: " + _targetDistance);

                            showTargetOnScreen = true;

                            scanning = false;
                            checking = false;
                            missionHoloSpawned = true;
                            StartCoroutine(SpawnHoloCache(nextLocation, true, false, HoloCacheName + gpsCount));
                            checking = false;
                        }
                        else
                        {
                            Debug.Log("======================================= TARGET Meters per Degree: " + mPerDegree);
                            Debug.Log("======================================= TARGET Degrees per Meter: " + degPerMeter);
                            Debug.Log("======================================= TARGET Degree offset 2D: " + Math.Sqrt(diffSqr));
                            Debug.Log("======================================= TARGET Degree offset 3D: " + Math.Sqrt(altAdded));
                            Debug.Log("======================================= TARGET Distance in Meters: " + _targetDistance);
                        }
                    }

                    if (checking)
                    {
                        Debug.Log("======================================= SCAN DELAY: " + scanDelay);
                        yield return new WaitForSeconds(Convert.ToSingle(scanDelay));
                        StartCoroutine(CheckTargetDistance(b));
                    }
                }
            }
        }

        float scanDelay = 0;

        IEnumerator CheckTargetDistanceMission(Vector3d vect)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (checking)
                {
                    showTargetOnScreen = true;
                    targetDistance = double.MaxValue;

                    double _latDiff = 0;
                    double _lonDiff = 0;
                    double _altDiff = 0;

                    if (FlightGlobals.ActiveVessel.altitude <= vect.z)
                    {
                        _altDiff = vect.z - FlightGlobals.ActiveVessel.altitude;
                    }
                    else
                    {
                        _altDiff = FlightGlobals.ActiveVessel.altitude - vect.z;
                    }

                    if (vect.x >= 0)
                    {
                        if (FlightGlobals.ActiveVessel.latitude >= vect.x)
                        {
                            _latDiff = FlightGlobals.ActiveVessel.latitude - vect.x;
                        }
                        else
                        {
                            _latDiff = vect.x - FlightGlobals.ActiveVessel.latitude;
                        }
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.latitude >= 0)
                        {
                            _latDiff = FlightGlobals.ActiveVessel.latitude - vect.x;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.latitude <= vect.x)
                            {
                                _latDiff = FlightGlobals.ActiveVessel.latitude - vect.x;
                            }
                            else
                            {

                                _latDiff = vect.x - FlightGlobals.ActiveVessel.latitude;
                            }
                        }
                    }

                    if (vect.y >= 0)
                    {
                        if (FlightGlobals.ActiveVessel.longitude >= vect.y)
                        {
                            _lonDiff = FlightGlobals.ActiveVessel.longitude - vect.y;
                        }
                        else
                        {
                            _lonDiff = vect.y - FlightGlobals.ActiveVessel.latitude;
                        }
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.longitude >= 0)
                        {
                            _lonDiff = FlightGlobals.ActiveVessel.longitude - vect.y;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.longitude <= vect.y)
                            {
                                _lonDiff = FlightGlobals.ActiveVessel.longitude - vect.y;
                            }
                            else
                            {

                                _lonDiff = vect.y - FlightGlobals.ActiveVessel.longitude;
                            }
                        }
                    }

                    double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                    double _altDiffDeg = _altDiff * degPerMeter;
                    double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                    double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                    if (targetDistance >= _targetDistance)
                    {
                        targetDistance = _targetDistance;
                    }

                    if (_targetDistance <= 25)
                    {
                        Debug.Log("======================================= TARGET Name: " + HoloCacheName + gpsCount);

                        Debug.Log("======================================= TARGET Meters per Degree: " + mPerDegree);
                        Debug.Log("======================================= TARGET Degrees per Meter: " + degPerMeter);
                        Debug.Log("======================================= TARGET Degree offset 2D: " + Math.Sqrt(diffSqr));
                        Debug.Log("======================================= TARGET Degree offset 3D: " + Math.Sqrt(altAdded));
                        Debug.Log("======================================= TARGET Distance in Meters: " + _targetDistance);


                        scanning = false;
                        checking = false;
                        missionHoloSpawned = true;
                        GetNextCoord();
                        checking = false;
                    }
                    else
                    {
                        Debug.Log("======================================= TARGET Name: " + HoloCacheName + gpsCount);

                        Debug.Log("======================================= TARGET Meters per Degree: " + mPerDegree);
                        Debug.Log("======================================= TARGET Degrees per Meter: " + degPerMeter);
                        Debug.Log("======================================= TARGET Degree offset 2D: " + Math.Sqrt(diffSqr));
                        Debug.Log("======================================= TARGET Degree offset 3D: " + Math.Sqrt(altAdded));
                        Debug.Log("======================================= TARGET Distance in Meters: " + _targetDistance);
                        scanDelay = Convert.ToSingle(targetDistance / FlightGlobals.ActiveVessel.srfSpeed) / 10;
                        if (scanDelay >= 120)
                        {
                            scanDelay = 120;
                        }
                        Debug.Log("======================================= SCAN DELAY: " + scanDelay);
                        yield return new WaitForSeconds(delay);
                        StartCoroutine(CheckTargetDistanceMission(vect));
                    }
                }
            }
        }
        public void GetNextCoord()
        {
            GuiEnabledOrXMissions = false;
            challengeRunning = true;
            checkingMission = true;
            if (targetCube != null)
            {
                var mom = targetCube.FindPartModuleImplementing<ModuleOrXMission>();
                if (mom != null)
                {
                    if (mom.boid)
                    {
                        mom.HideHolo();
                    }
                }
            }

            if (coordCount - gpsCount == 0)
            {
                SaveScore();
            }
            else
            {
                stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + (HighLogic.CurrentGame.UniversalTime - missionTime));
                topSurfaceSpeed = 0;
                missionTime = HighLogic.CurrentGame.UniversalTime;
                gpsCount += 1;
                maxDepth = 0;
                bool getCoord = true;
                List<string>.Enumerator coords = CoordDatabase.GetEnumerator();
                while (coords.MoveNext())
                {
                    try
                    {
                        if (getCoord)
                        {
                            string[] data = coords.Current.Split(new char[] { ',' });
                            if (data[0] == gpsCount.ToString())
                            {
                                getCoord = false;
                                NextCoord = coords.Current;
                                latMission = double.Parse(data[1]);
                                lonMission = double.Parse(data[2]);
                                altMission = double.Parse(data[3]);
                                nextLocation = new Vector3d(latMission, lonMission, altMission);
                                Debug.Log("[OrX Mission] NEXT LOCATION ACQUIRED ...... ");
                                /*
                                Vector3d surfacePoint = new Vector3d(latMission, lonMission, altMission);
                                Waypoint waypoint = new Waypoint();
                                System.Random r = new System.Random();
                                int waypointColor = (int)(r.NextDouble() * seeds.Count());
                                waypoint.id = "marker";
                                waypoint.seed = seeds[waypointColor];
                                waypoint.name = craftToSpawn;
                                waypoint.celestialName = name;
                                waypoint.longitude = FlightGlobals.ActiveVessel.mainBody.GetLongitude(surfacePoint);
                                waypoint.latitude = FlightGlobals.ActiveVessel.mainBody.GetLatitude(surfacePoint);
                                waypoint.height = TerrainHeight(latMission, lonMission, FlightGlobals.ActiveVessel.mainBody);
                                waypoint.altitude = alt - waypoint.height;

                                Debug.Log("[OrX HoloCache] ====================================== ADDING WAYPOINT FOR " + craftToSpawn + gpsCount + " ===");
                                ScenarioCustomWaypoints.RemoveWaypoint(waypoint);
                                ScenarioCustomWaypoints.AddWaypoint(waypoint);
                                */
                                break;
                            }
                        }
                        else
                        {
                        }
                    }
                    catch
                    {
                    }
                }
                coords.Dispose();
                scanning = true;
                checking = true;
                spawnHoloCache = false;
                tpoint = nextLocation;
                showTargetOnScreen = true;

                Debug.Log("[Module OrX Mission] === BOID CHECKING DISTANCE ===");
                StartCoroutine(CheckTargetDistance(true));
            }
        }

        #endregion

        #region Spawn HoloCache

        public static Vector3d WorldPositionToGeoCoords(Vector3d worldPosition, CelestialBody body)
        {
            if (!body)
            {
                return Vector3d.zero;
            }
            double lat = body.GetLatitude(worldPosition);
            double longi = body.GetLongitude(worldPosition);
            double alt = body.GetAltitude(worldPosition);
            Debug.Log("[Spawn OrX HoloCache] Lat: " + lat + " - Lon:" + longi + " - Alt: " + alt);
            return new Vector3d(lat, longi, alt);
        }

        public bool spawnByOrx = false;
        public bool spawnByChallenge = false;
        public bool triggerKerbSetup = false;

        public void SpawnByChallengeBuilder(Vessel v)
        {
            spawnByChallenge = true;
            if (challengeType == "DAKAR RACING")
            {
                dakarSetup = true;
            }
            StartCoroutine(SpawnHoloCache(new Vector3d(v.latitude, v.longitude, v.altitude + 2), false, false, HoloCacheName));
        }
        public void SpawnByOrX(Vector3d vect)
        {
            spawnByOrx = true;
            StartCoroutine(SpawnHoloCache(vect, false, true, string.Empty));
        }
        IEnumerator SpawnHoloCache(Vector3d vect, bool b, bool empty, string name)
        {
            mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
            degPerMeter = 1 / mPerDegree;

            spawnByOrx = true;
            craftFile = name;
            string holoFileLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloCache/HoloCache.craft";
            showTargetOnScreen = false;

            yield return new WaitForFixedUpdate();

            if (b)
            {
                holoFileLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/Boids/Boid.craft";
                boid = true;
            }

            emptyholo = empty;
            holo = true;

            _lat = vect.x;
            _lon = vect.y;
            if (empty)
            {
                Debug.Log("[Spawn OrX HoloCache] Spawning Empty HoloCache ...... ");

                _alt = vect.z + 1f;
                tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt) + FlightGlobals.ActiveVessel.transform.forward * 1.3f;
            }
            else
            {
                Debug.Log("[Spawn OrX HoloCache] Spawning " + craftFile);

                if (buildingMission)
                {
                    _alt = vect.z + 1.5f;
                    tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt) + FlightGlobals.ActiveVessel.transform.forward * 1.3f;
                }
                else
                {
                    _alt = vect.z;
                    tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt);
                }
            }

            yield return new WaitForFixedUpdate();

            Vector3d gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);

            HoloCacheData newData = new HoloCacheData();

            newData.craftURL = holoFileLoc;
            newData.latitude = gpsPos.x;
            newData.longitude = gpsPos.y;
            newData.altitude = gpsPos.z;
            newData.body = FlightGlobals.currentMainBody;
            newData.heading = 90;
            newData.pitch = 0;
            newData.orbiting = false;
            newData.flagURL = flagURL;
            newData.owned = true;
            newData.vesselType = VesselType.Unknown;

            Debug.Log("[Spawn OrX HoloCache] Spawning " + newData.name);
            bool landed = false;
            if (!landed)
            {
                landed = true;
                if (newData.altitude == null) // || newData.altitude < 0)
                {
                    newData.altitude = 5;//LocationUtil.TerrainHeight(newData.latitude, newData.longitude, newData.body);
                }
                Debug.Log("[Spawn OrX HoloCache] SpawnVessel Altitude: " + newData.altitude);

                //Vector3d pos = newData.body.GetWorldSurfacePosition(newData.latitude, newData.longitude, newData.altitude.Value);
                Vector3d pos = newData.body.GetRelSurfacePosition(newData.latitude, newData.longitude, newData.altitude.Value);

                newData.orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, newData.body);
                newData.orbit.UpdateFromStateVectors(pos, newData.body.getRFrmVel(pos), newData.body, Planetarium.GetUniversalTime());
            }

            ConfigNode[] partNodes;
            ShipConstruct shipConstruct = null;
            bool hasClamp = false;
            float lcHeight = 0;
            if (!string.IsNullOrEmpty(newData.craftURL))
            {
                // Save the current ShipConstruction ship, otherwise the player will see the spawned ship next time they enter the VAB!
                ConfigNode currentShip = ShipConstruction.ShipConfig;

                shipConstruct = ShipConstruction.LoadShip(newData.craftURL);
                if (shipConstruct == null)
                {
                    Debug.Log("[Spawn OrX HoloCache] ShipConstruct was null when tried to load '" + newData.craftURL +
                      "' (usually this means the file could not be found).");
                    //return;//continue;
                }

                lcHeight = 0;
                // Restore ShipConstruction ship
                ShipConstruction.ShipConfig = currentShip;

                // Set the name
                if (string.IsNullOrEmpty(newData.name))
                {
                    newData.name = name;
                }

                // Set some parameters that need to be at the part level
                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                uint launchID = HighLogic.CurrentGame.launchID++;
                foreach (Part p in shipConstruct.parts)
                {
                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                    p.missionID = missionID;
                    p.launchID = launchID;
                    p.flagURL = flagURL;

                    // Had some issues with this being set to -1 for some ships - can't figure out
                    // why.  End result is the vessel exploding, so let's just set it to a positive
                    // value.
                    p.temperature = 1.0;
                }
                ConfigNode dummyConfig = new ConfigNode();
                ProtoVessel dummyProto = new ProtoVessel(dummyConfig, null);
                Vessel dummyVessel = new Vessel();
                dummyVessel.parts = shipConstruct.parts;
                dummyProto.vesselRef = dummyVessel;

                // Create the ProtoPartSnapshot objects and then initialize them
                foreach (Part p in shipConstruct.parts)
                {
                    dummyProto.protoPartSnapshots.Add(new ProtoPartSnapshot(p, dummyProto));
                }
                foreach (ProtoPartSnapshot p in dummyProto.protoPartSnapshots)
                {
                    p.storePartRefs();
                }

                // Create the ship's parts

                List<ConfigNode> partNodesL = new List<ConfigNode>();
                foreach (ProtoPartSnapshot snapShot in dummyProto.protoPartSnapshots)
                {
                    ConfigNode node = new ConfigNode("PART");
                    snapShot.Save(node);
                    partNodesL.Add(node);
                }
                partNodes = partNodesL.ToArray();
            }
            else
            {
                uint flightId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                partNodes = new ConfigNode[1];
                partNodes[0] = ProtoVessel.CreatePartNode(newData.craftPart.name, flightId, null);

                if (string.IsNullOrEmpty(newData.name))
                {
                    newData.name = newData.craftPart.name;
                }
            }

            Debug.Log("[Spawn OrX HoloCache] CREATING ADDITIONAL NODES FOR " + newData.name);

            ConfigNode[] additionalNodes = new ConfigNode[0];
            ConfigNode protoVesselNode = ProtoVessel.CreateVesselNode(newData.name, newData.vesselType, newData.orbit, 0, partNodes, additionalNodes);

            if (!newData.orbiting)
            {
                Vector3d norm = newData.body.GetRelSurfaceNVector(newData.latitude, newData.longitude);

                double terrainHeight = 0.0;
                if (newData.body.pqsController != null)
                {
                    terrainHeight = newData.body.pqsController.GetSurfaceHeight(norm) - newData.body.pqsController.radius;
                    if (terrainHeight <= newData.body.pqsController.radius)
                    {
                        if (!holo && !spawningMissionCraft && !boid)
                        {
                            var tHeight = newData.body.pqsController.radius - terrainHeight;
                            terrainHeight += tHeight;
                        }
                    }
                }
                bool splashed = false;// = landed && terrainHeight < 0.001;

                // Create the config node representation of the ProtoVessel
                // Note - flying is experimental, and so far doesn't work
                protoVesselNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                  Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                protoVesselNode.SetValue("landed", (landed && !splashed).ToString());
                protoVesselNode.SetValue("splashed", splashed.ToString());
                protoVesselNode.SetValue("lat", newData.latitude.ToString());
                protoVesselNode.SetValue("lon", newData.longitude.ToString());
                protoVesselNode.SetValue("alt", newData.altitude.ToString());
                protoVesselNode.SetValue("landedAt", newData.body.name);

                // Figure out the additional height to subtract
                float lowest = float.MaxValue;
                if (shipConstruct != null)
                {
                    foreach (Part p in shipConstruct.parts)
                    {
                        bool robotic = p.isRobotic();
                        if (!robotic)
                        {
                            foreach (Collider collider in p.GetComponentsInChildren<Collider>())
                            {
                                if (collider.gameObject.layer != 21 && collider.enabled)
                                {
                                    lowest = Mathf.Min(lowest, collider.bounds.min.y);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Part p in shipConstruct.parts)
                    {
                        bool roboticPart = p.isRobotic();
                        if (!roboticPart)
                        {
                            foreach (Collider collider in newData.craftPart.partPrefab.GetComponentsInChildren<Collider>())
                            {
                                if (collider.gameObject.layer != 21 && collider.enabled)
                                {
                                    lowest = Mathf.Min(lowest, collider.bounds.min.y);
                                }
                            }
                        }
                    }
                }

                if (lowest == float.MaxValue)
                {
                    lowest = 0;
                }

                Debug.Log("[Spawn OrX HoloCache] Figure out the surface height and rotation for " + newData.name);

                // Figure out the surface height and rotation
                Quaternion normal = Quaternion.LookRotation(norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
                Quaternion rotation = Quaternion.identity;
                float heading = newData.heading;
                if (shipConstruct == null)
                {
                    rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
                }
                else if (shipConstruct.shipFacility == EditorFacility.SPH)
                {
                    rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                    heading += 180.0f;
                }
                else
                {
                    rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                    rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;
                    newData.heading = 0;
                    newData.pitch = 0;
                }

                rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
                rotation = rotation * Quaternion.AngleAxis(newData.roll, Vector3.down);
                rotation = rotation * Quaternion.AngleAxis(newData.pitch, Vector3.left);

                // Set the height and rotation
                if (landed || splashed)
                {
                    float hgt = (shipConstruct != null ? shipConstruct.parts[0] : newData.craftPart.partPrefab).localRoot.attPos0.y - lowest;
                    hgt += newData.height;

                    foreach (Part p in shipConstruct.Parts)
                    {
                        LaunchClamp lc = p.FindModuleImplementing<LaunchClamp>();
                        if (lc)
                        {
                            hasClamp = true;
                            break;
                        }
                    }

                    if (!hasClamp)
                    {
                        //hgt += 2;
                    }
                    else
                    {
                        hgt += lcHeight;
                    }
                    protoVesselNode.SetValue("hgt", hgt.ToString(), true);
                }

                protoVesselNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);

                Vector3 nrm = (rotation * Vector3.forward);
                protoVesselNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);

                protoVesselNode.SetValue("prst", false.ToString(), true);
            }
            else
            {

            }

            ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
            newData.id = protoVessel.vesselRef.id;
            _HoloKron = new Vessel();
            _HoloKron = protoVessel.vesselRef;

            foreach (Part p in FindObjectsOfType<Part>())
            {
                if (!p.vessel)
                {
                    Destroy(p.gameObject);
                }
            }
            yield return new WaitForFixedUpdate();
            _HoloKron.isPersistent = true;
            _HoloKron.Landed = false;
            _HoloKron.situation = Vessel.Situations.FLYING;
            while (_HoloKron.packed)
            {
                yield return null;
            }
            _HoloKron.SetWorldVelocity(Vector3d.zero);
            yield return null;
            var mom = _HoloKron.FindPartModuleImplementing<ModuleOrXMission>();
            if (mom == null)
            {
                _HoloKron.rootPart.AddModule("ModuleOrXMission", true);
                mom = _HoloKron.FindPartModuleImplementing<ModuleOrXMission>();
                mom.HoloCacheName = name;
                mom.missionType = missionType;
            }
            else
            {
                HoloCacheName = name;
                missionType = mom.missionType;
            }
            mom.latitude = tpoint.x;
            mom.longitude = tpoint.y;
            mom.altitude = tpoint.z;
            mom.pos = new Vector3d(mom.latitude, mom.longitude, mom.altitude);

            _HoloKron.GoOffRails();
            _HoloKron.IgnoreGForces(240);
            StageManager.BeginFlight();

            if (!b)
            {
                if (!empty)
                {
                    if (dakarSetup)
                    {
                        Debug.Log("[Spawn OrX HoloCache] " + newData.name + " IS A DAKAR CHALLENGE .... SWITCHING VESSEL");
                        dakarSetup = false;
                        challengeHolo = _HoloKron;
                        var h = challengeHolo.rootPart.FindModuleImplementing<ModuleOrXMission>();
                        h.StartBuild();
                    }
                    else
                    {
                        Debug.Log("[Spawn OrX HoloCache] " + newData.name + " HAS DATA .... CHECKING FOR VESSELS TO SPAWN");

                        missionHoloSpawned = true;
                        StartCoroutine(SpawnLocalVessels(name));
                    }
                }
                else
                {
                    if (dakarSetup)
                    {
                        Debug.Log("[Spawn OrX HoloCache] " + newData.name + " IS A DAKAR CHALLENGE .... SWITCHING VESSEL");
                        dakarSetup = false;
                        challengeHolo = _HoloKron;
                        var h = challengeHolo.rootPart.FindModuleImplementing<ModuleOrXMission>();
                        h.StartBuild();
                    }
                    else
                    {
                        if (addCoordSetup)
                        {
                            Debug.Log("[Spawn OrX HoloCache] " + newData.name + " IS MISSION HOLO .... SETTING UP COORD MENU");

                            addCoordSetup = false;
                            AddMissionCoordsSetup();
                        }
                        else
                        {
                            Debug.Log("[Spawn OrX HoloCache] " + newData.name + " IS PRIMARY HOLO .... SETTING UP HOLOCACHE");
                            SetupHolo();
                        }
                    }
                }
            }
            else
            {
                mom.boid = true;

                if (!buildingMission)
                {
                    scanning = true;
                    checking = true;
                    targetCube = _HoloKron;
                    StartCoroutine(CheckTargetDistanceMission(gpsPos));
                }
                else
                {
                }
            }
            mom.isLoaded = true;
            holo = false;
            spawnHoloCache = false;
            boid = false;
            spawned = true;
            spawningMissionCraft = false;
            emptyholo = true;
        }
        IEnumerator SpawnLocalVessels(string name)
        {
            HoloCacheName = name;
            Debug.Log("[Spawn OrX Local Vessels] === Spawning Local Vessels === ");
            _file = new ConfigNode();
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + name + "/" + name + ".orx");

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + name))
            {
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + name);
            }
            yield return new WaitForFixedUpdate();

            ConfigNode _toArchive = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + name + "/" + name + ".orx");
            if (_toArchive == null)
            {
                _toArchive = new ConfigNode();
                _toArchive = _file;
                _toArchive.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + name + "/" + name + ".orx");
            }

            if (HoloCacheName != "")
            {
                int _hcCount = 0;
                ConfigNode node = _file.GetNode("OrX");

                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (spawnCheck.name.Contains("OrXHoloCacheCoords" + _hcCount))
                    {
                        Debug.Log("[Spawn OrX Local Vessels] === FOUND HOLOCACHE === " + _hcCount); ;

                        ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + _hcCount);

                        foreach (ConfigNode.Value data in HoloCacheNode.values)
                        {
                            if (data.name == "spawned")
                            {
                                if (data.value == "False")
                                {
                                    Debug.Log("[Spawn OrX Local Vessels] === HOLOCACHE " + _hcCount + " has not spawned ... ");
                                    HoloCacheNode.SetValue("spawned", "True", true);
                                    _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + name + "/" + name + ".orx");
                                    break;
                                }
                                else
                                {
                                    Debug.Log("[Spawn OrX Local Vessels] === HOLOCACHE " + _hcCount + " has spawned ... CHECKING FOR EXTRAS");

                                    if (HoloCacheNode.HasValue("extras"))
                                    {
                                        var t = HoloCacheNode.GetValue("extras");
                                        if (t == "False")
                                        {
                                            Debug.Log("[Spawn OrX Local Vessels] === HOLOCACHE " + _hcCount + " has no extras ... END TRANSMISSION");
                                            _hcCount = int.MaxValue;
                                            break;
                                        }
                                        else
                                        {
                                            Debug.Log("[Spawn OrX Local Vessels] === HOLOCACHE " + _hcCount + " has extras ... SEARCHING");
                                            _hcCount += 1;
                                        }
                                    }
                                }
                            }
                        }

                        Debug.Log("[Spawn OrX Local Vessels] === DATA PROCESSED ===");
                    }

                    if (spawnCheck.name.Contains("HC" + _hcCount + "OrXv"))
                    {
                        if (spawnCheck.name != "HC" + _hcCount + "OrXv0")
                        {
                            Debug.Log("[Spawn OrX Local Vessels] === GRABBING CRAFT FILE FOR " + spawnCheck.name + " ===");
                            holo = false;
                            emptyholo = false;
                            spawningMissionCraft = true;

                            float _left = 0;
                            float _pitch = 0;
                            double _al = 0;
                            double _la = 0;
                            double _lo = 0;
                            int _serial = 0;

                            ConfigNode location = spawnCheck.GetNode("coords");

                            foreach (ConfigNode.Value loc in location.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                loc.name = cvEncryptedName;
                                loc.value = cvEncryptedValue;

                                if (loc.name == "lat")
                                {
                                    _la = double.Parse(loc.value);
                                }
                                if (loc.name == "lon")
                                {
                                   _lo = double.Parse(loc.value);
                                }
                                if (loc.name == "alt")
                                {
                                    _al = double.Parse(loc.value);
                                }
                                if (loc.name == "left")
                                {
                                    _left = float.Parse(loc.value);
                                }
                                if (loc.name == "pitch")
                                {
                                    _pitch = float.Parse(loc.value);
                                }
                                if (loc.name == "pas")
                                {
                                    pas = loc.value;
                                }
                            }

                            Debug.Log("[Spawn OrX Local Vessels] === VESSEL SPAWN COORDS READY ===");

                            Debug.Log("[Spawn OrX Local Vessels] === DECRYPTING CRAFT FILE DATA FOR " + spawnCheck.name + " ===");

                            ConfigNode craftFile = spawnCheck.GetNode("craft");
                            foreach (ConfigNode.Value cv in craftFile.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                cv.name = cvEncryptedName;
                                cv.value = cvEncryptedValue;
                            }


                            foreach (ConfigNode cn in craftFile.nodes)
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
                                        if (cv2.name != "currentRotation")
                                        {
                                            string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                            string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                            cv2.name = cvEncryptedName;
                                            cv2.value = cvEncryptedValue;
                                        }
                                    }
                                }
                            }

                            Debug.Log("[Spawn OrX Local Vessels] === VESSEL DECRYPTED - READY FOR SPAWNING ===");

                            craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                            missionCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp";

                            yield return new WaitForFixedUpdate();

                            HoloCacheData newData = new HoloCacheData();
                            newData.craftURL = missionCraftLoc;
                            newData.latitude = _la;
                            newData.longitude = _lo;
                            newData.altitude = _al + 100;
                            newData.body = FlightGlobals.currentMainBody;
                            newData.heading = 0;
                            newData.pitch = 0;
                            newData.orbiting = false;
                            newData.flagURL = "";
                            newData.owned = false;
                            newData.vesselType = VesselType.Unknown;
                            newData.name = spawnCheck.name;

                            Debug.Log("[Spawn OrX Local Vessels] Spawning " + spawnCheck.name);
                            Debug.Log("[Spawn OrX Local Vessels] Altitude: " + newData.altitude);

                            bool landed = false;
                            if (!landed)
                            {
                                landed = true;
                                Debug.Log("[Spawn OrX HoloCache] SpawnVessel Altitude: " + newData.altitude);

                                Vector3d pos = newData.body.GetRelSurfacePosition(newData.latitude, newData.longitude, newData.altitude.Value);
                                newData.orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, newData.body);
                                newData.orbit.UpdateFromStateVectors(pos, newData.body.getRFrmVel(pos), newData.body, Planetarium.GetUniversalTime());
                            }

                            Debug.Log("[Spawn OrX Local Vessels] Orbit Data Processed");

                            ConfigNode[] partNodes;
                            ShipConstruct shipConstruct = null;

                            ConfigNode currentShip = ShipConstruction.ShipConfig;
                            shipConstruct = ShipConstruction.LoadShip(newData.craftURL);
                            ShipConstruction.ShipConfig = currentShip;
                            uint missionID = (uint)Guid.NewGuid().GetHashCode();
                            uint launchID = HighLogic.CurrentGame.launchID++;

                            Debug.Log("[Spawn OrX Local Vessels] Ship construct created");

                            foreach (Part p in shipConstruct.parts)
                            {
                                p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                                p.missionID = missionID;
                                p.launchID = launchID;
                                p.flagURL = flagURL;
                                p.temperature = 1.0;
                            }

                            Debug.Log("[Spawn OrX Local Vessels] Part flight ID's processed");
                            Debug.Log("[Spawn OrX Local Vessels] Constructing protovessel");

                            ConfigNode empty = new ConfigNode();
                            ProtoVessel dummyProto = new ProtoVessel(empty, null);
                            Vessel dummyVessel = new Vessel();
                            dummyVessel.parts = shipConstruct.parts;
                            dummyProto.vesselRef = dummyVessel;

                            Debug.Log("[Spawn OrX Local Vessels] Checking for crew space");

                            foreach (Part p in shipConstruct.parts)
                            {
                                if (p.Modules.Contains<KerbalEVA>())
                                {
                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                      ? ProtoCrewMember.Gender.Female
                                      : ProtoCrewMember.Gender.Male;
                                    System.Random r = new System.Random();
                                    _serial = r.Next(1000, 9999);
                                    crewMember.KerbalRef.crewMemberName = "TK-" + _serial;
                                    p.AddCrewmember(crewMember);

                                    newData.name = "TK-" + _serial;
                                }

                                dummyProto.protoPartSnapshots.Add(new ProtoPartSnapshot(p, dummyProto));
                            }
                            foreach (ProtoPartSnapshot p in dummyProto.protoPartSnapshots)
                            {
                                p.storePartRefs();
                            }

                            List<ConfigNode> partNodesL = new List<ConfigNode>();
                            foreach (ProtoPartSnapshot snapShot in dummyProto.protoPartSnapshots)
                            {
                                ConfigNode partNode = new ConfigNode("PART");
                                snapShot.Save(partNode);
                                partNodesL.Add(partNode);
                            }
                            partNodes = partNodesL.ToArray();

                            Debug.Log("[Spawn OrX Local Vessels] CREATING ADDITIONAL NODES FOR " + newData.name);
                            ConfigNode[] additionalNodes = new ConfigNode[0];
                            ConfigNode protoVesselNode = ProtoVessel.CreateVesselNode(newData.name, newData.vesselType, newData.orbit, 0, partNodes, additionalNodes);
                            bool splashed = false;

                            Vector3d norm = newData.body.GetRelSurfaceNVector(newData.latitude, newData.longitude);
                            splashed = false;
                            protoVesselNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                              Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                            protoVesselNode.SetValue("landed", (landed && !splashed).ToString());
                            protoVesselNode.SetValue("splashed", splashed.ToString());
                            protoVesselNode.SetValue("lat", newData.latitude.ToString());
                            protoVesselNode.SetValue("lon", newData.longitude.ToString());
                            protoVesselNode.SetValue("alt", newData.altitude.ToString());
                            protoVesselNode.SetValue("landedAt", newData.body.name);

                            Quaternion normal = Quaternion.LookRotation((Vector3)norm);
                            Quaternion rotation = Quaternion.identity;
                            rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                            rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;
                            protoVesselNode.SetValue("hgt", newData.height.ToString(), true);
                            protoVesselNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
                            Vector3 nrm = (rotation * Vector3.forward);
                            protoVesselNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
                            protoVesselNode.SetValue("prst", false.ToString(), true);

                            ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
                            protoVessel.vesselRef.transform.rotation = protoVessel.rotation;
                            newData.id = protoVessel.vesselRef.id;
                            Vessel localVessel = protoVessel.vesselRef;
                            foreach (Part p in FindObjectsOfType<Part>())
                            {
                                if (!p.vessel)
                                {
                                    Destroy(p.gameObject);
                                }
                            }

                            yield return new WaitForFixedUpdate();
                            ConfigNode craft = ConfigNode.Load(missionCraftLoc);
                            craft.ClearData();
                            craft.Save(missionCraftLoc);

                            localVessel.isPersistent = true;
                            localVessel.Landed = false;
                            localVessel.situation = Vessel.Situations.FLYING;
                            while (localVessel.packed)
                            {
                                yield return null;
                            }
                            localVessel.SetWorldVelocity(Vector3d.zero);
                            localVessel.GoOffRails();
                            localVessel.IgnoreGForces(240);
                            localVessel.rootPart.AddModule("ModuleHideVessel", true);
                            emptyholo = true;
                            StageManager.BeginFlight();

                            holo = false;
                            spawningMissionCraft = false;
                            spawnHoloCache = false;
                            Debug.Log("[Spawn OrX Local Vessels] === FIXING ROTATION FOR " + spawnCheck.name + " ===");
                            if (_left >= 360)
                            {
                                _left -= 360;
                            }

                            Vector3 UpVect = (localVessel.transform.position - localVessel.mainBody.position).normalized;
                            Vector3 EastVect = localVessel.mainBody.getRFrmVel(localVessel.CoM).normalized;
                            Vector3 NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
                            Quaternion _fixRot = Quaternion.identity;

                            localVessel.IgnoreGForces(240);
                            localVessel.angularVelocity = Vector3.zero;
                            localVessel.angularMomentum = Vector3.zero;
                            localVessel.SetWorldVelocity(Vector3d.zero);
                            _fixRot = Quaternion.FromToRotation(-localVessel.ReferenceTransform.right, NorthVect) * localVessel.ReferenceTransform.rotation;
                            localVessel.SetRotation(_fixRot, true);
                            _fixRot = Quaternion.AngleAxis(180, localVessel.ReferenceTransform.right) * localVessel.ReferenceTransform.rotation;
                            localVessel.SetRotation(_fixRot, true);
                            localVessel.IgnoreGForces(240);
                            _fixRot = Quaternion.AngleAxis(_pitch - 90, localVessel.ReferenceTransform.right) * localVessel.ReferenceTransform.rotation;
                            localVessel.SetRotation(_fixRot, true);
                            _fixRot = Quaternion.AngleAxis(_left, UpVect) * localVessel.ReferenceTransform.rotation;
                            localVessel.SetRotation(_fixRot, true);
                            float localAlt = Convert.ToSingle(localVessel.radarAltitude);
                            float dropRate = Mathf.Clamp((localAlt * 2), 0.1f, 200);
                            Debug.Log("[Spawn OrX Local Vessels] === PLACING " + spawnCheck.name + " ===");

                            while (dropRate >= 1 && !localVessel.LandedOrSplashed)
                            {
                                localVessel.IgnoreGForces(240);
                                localVessel.angularVelocity = Vector3.zero;
                                localVessel.angularMomentum = Vector3.zero;
                                localVessel.SetWorldVelocity(Vector3.zero);
                                dropRate = Mathf.Clamp((localAlt * 2), 0.1f, 200);

                                if (dropRate > 2)
                                {
                                    localVessel.Translate(dropRate * Time.fixedDeltaTime * -UpVect);
                                }
                                else
                                {
                                    dropRate = 0.1f;
                                    localVessel.SetWorldVelocity(dropRate * -UpVect);
                                }
                                localAlt -= dropRate * Time.fixedDeltaTime;
                                yield return new WaitForFixedUpdate();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("[Spawn OrX Local Vessels] === HOLOCACHE " + _hcCount + " has no vessels ... END TRANSMISSION");
                    }
                }

                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                    {
                        ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + _hcCount);

                        if (HoloCacheNode != null)
                        {
                            Debug.Log("[Spawn OrX Local Vessels] === FOUND HOLOCACHE " + _hcCount + " === UPDATING");


                            Debug.Log("[Spawn OrX Local Vessels] === DATA PROCESSED ===");
                        }
                    }
                }
            }

            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + name + "/" + name + ".orx");
        }

        #endregion

        /// 

        #region Main GUI

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

                if (showTargetOnScreen)
                {
                    DrawTextureOnWorldPos(nextLocation, instance.HoloTargetTexture, new Vector2(8, 8));
                }

                if (OrXHCGUIEnabled)
                {
                    WindowRectToolbar = GUI.Window(265227765, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
                }
            }
            else
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    if (!OrXHCGUIEnabled) return;
                    WindowRectToolbar = GUI.Window(266222695, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
                }
            }
        }
        private void AddToolbarButton()
        {
            string OrXDir = "OrX/Plugin/";

            if (!hasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(OrXDir + "OrX_icon", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB 
                    | ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.TRACKSTATION | ApplicationLauncher.AppScenes.MAPVIEW, buttonTexture);
                hasAddedButton = true;
            }
        }

        public void HideGameUI()
        {
            showScores = false;
            //building = false;
            //buildingMission = false;
            //addCoords = false;
            GuiEnabledOrXMissions = false;
            OrXHCGUIEnabled = false;
            PlayOrXMission = false;
            Debug.Log("[OrX Mission]: Hiding OrXMissions GUI");
        }
        void ShowGameUI()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                vid = FlightGlobals.ActiveVessel.id;
                if (!buildingMission)
                {
                    //ClearDatabase();
                    //CheckSOI();
                    Debug.Log("[OrX GUI] === Operation Dinner Out is a go !!!! ===");
                    ScreenMsg("Operation 'Dinner Out' is a go !!!!");
                }
                else
                {
                    GuiEnabledOrXMissions = true;
                    addCoords = true;
                    building = true;
                }
                OrXHCGUIEnabled = true;
            }
        }
        private void ToggleGUI()
        {
            if (OrXHCGUIEnabled)
            {
                if (!paused)
                {
                    pauseCheck = false;
                }
                else
                {
                    pauseCheck = true;
                }

                if (!addCoords)
                {
                    building = false;
                    buildingMission = false;
                    addCoords = false;
                    GuiEnabledOrXMissions = false;
                    OrXHCGUIEnabled = false;
                    PlayOrXMission = false;
                    showScores = false;

                    if (_HoloKron != null)
                    {
                        var h = _HoloKron.rootPart.FindModuleImplementing<ModuleOrXMission>();
                        if (h.boid)
                        {
                            h.HideHolo();
                        }
                    }

                    if (OrXLog.instance.keysSet)
                    {
                        OrXLog.instance.ResetFocusKeys();
                    }

                    ResetData();
                }
                else
                {
                    GuiEnabledOrXMissions = false;
                    OrXHCGUIEnabled = false;
                    PlayOrXMission = false;
                    showScores = false;
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
        public void ToggleCraftBrowser()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                if (craftBrowserOpen)
                {
                    OrXHCGUIEnabled = false;
                    GuiEnabledOrXMissions = false;
                    craftBrowserOpen = false;
                }
                else
                {
                    OrXHCGUIEnabled = true;
                    GuiEnabledOrXMissions = true;
                    craftBrowserOpen = true;
                }
            }
            else
            {

            }
        }

        private void OrXHCGUI(int OrX_HCGUI)
        {
            float line = 0;
            float leftIndent = 10;
            float contentWidth = toolWindowWidth - leftIndent;
            float contentTop = 10;
            float entryHeight = 20;
            float HCGUILines = 0;

            if (GuiEnabledOrXMissions)
            {
                GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));

                if (craftBrowserOpen)
                {
                    DrawCraftBrowserTitle(line);
                    line++;
                    DrawHangar(line);
                    line++;
                    if (HighLogic.LoadedSceneIsEditor)
                    {
                        if (sph)
                        {
                            int c = 0;
                            List<string>.Enumerator hcFile = sphFiles.GetEnumerator();
                            while (hcFile.MoveNext())
                            {
                                try
                                {
                                    ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                    string vn = "";

                                    foreach (ConfigNode.Value cv in craft.values)
                                    {
                                        if (cv.name == "ship")
                                        {
                                            vn = cv.value;
                                        }
                                    }

                                    //if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.8f, entryHeight), vn, HighLogic.Skin.button))
                                    if (GUILayout.Button(vn))
                                    {
                                        Debug.Log("[OrX Mission] === ADDING BLUPRINTS === SPH");

                                        if (savingToHoloKron)
                                        {
                                            var p = EditorLogic.RootPart.FindModuleImplementing<ModuleOrXHoloKron>();
                                            p.blueprints = hcFile.Current;
                                            p.SaveBlueprints();
                                            ToggleCraftBrowser();
                                        }
                                        else
                                        {
                                            blueprintsFile = hcFile.Current;
                                            craftToAddMission = vn;
                                            craftBrowserOpen = false;
                                            addingBluePrints = false;
                                            blueprintsAdded = true;
                                            blueprintsLabel = craftToAddMission;
                                        }
                                    }
                                    //line++;
                                    c += 1;
                                }
                                catch
                                {

                                }
                            }
                            hcFile.Dispose();
                        }
                        else
                        {
                            int c = 0;
                            List<string>.Enumerator hcFile = vabFiles.GetEnumerator();
                            while (hcFile.MoveNext())
                            {
                                try
                                {
                                    ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                    string vn = "";

                                    foreach (ConfigNode.Value cv in craft.values)
                                    {
                                        if (cv.name == "ship")
                                        {
                                            vn = cv.value;
                                        }
                                    }

                                    if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight), vn, HighLogic.Skin.button))
                                    {
                                        Debug.Log("[OrX Mission] === ADDING BLUPRINTS === VAB");

                                        if (savingToHoloKron)
                                        {

                                        }
                                        else
                                        {
                                            blueprintsFile = hcFile.Current;
                                            craftToAddMission = vn;
                                            craftBrowserOpen = false;
                                            addingBluePrints = false;
                                            blueprintsAdded = true;
                                            blueprintsLabel = craftToAddMission;
                                        }
                                    }
                                    line++;
                                    c += 1;
                                }
                                catch
                                {

                                }
                            }
                            hcFile.Dispose();
                        }
                    }
                    else
                    {
                        if (sph)
                        {
                            int c = 0;
                            List<string>.Enumerator hcFile = sphFiles.GetEnumerator();
                            while (hcFile.MoveNext())
                            {
                                try
                                {
                                    ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                    string vn = "";

                                    foreach (ConfigNode.Value cv in craft.values)
                                    {
                                        if (cv.name == "ship")
                                        {
                                            vn = cv.value;
                                        }
                                    }

                                    if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight), vn, HighLogic.Skin.button))
                                    {
                                        Debug.Log("[OrX Mission] === ADDING BLUPRINTS === VAB");

                                        blueprintsFile = hcFile.Current;
                                        craftToAddMission = vn;
                                        craftBrowserOpen = false;
                                        addingBluePrints = false;
                                        blueprintsAdded = true;
                                        blueprintsLabel = craftToAddMission;
                                    }
                                    line++;
                                    c += 1;
                                }
                                catch
                                {

                                }
                            }
                            hcFile.Dispose();
                        }
                        else
                        {
                            int c = 0;
                            List<string>.Enumerator hcFile = vabFiles.GetEnumerator();
                            while (hcFile.MoveNext())
                            {
                                try
                                {
                                    ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                    string vn = "";

                                    foreach (ConfigNode.Value cv in craft.values)
                                    {
                                        if (cv.name == "ship")
                                        {
                                            vn = cv.value;
                                        }
                                    }

                                    if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight), vn, HighLogic.Skin.button))
                                    {
                                        Debug.Log("[OrX Mission] === ADDING BLUPRINTS === VAB");

                                        blueprintsFile = hcFile.Current;
                                        craftToAddMission = vn;
                                        craftBrowserOpen = false;
                                        addingBluePrints = false;
                                        blueprintsAdded = true;
                                        blueprintsLabel = craftToAddMission;
                                    }
                                    line++;
                                    c += 1;
                                }
                                catch
                                {

                                }
                            }
                            hcFile.Dispose();
                        }
                    }

                    line++;
                    DrawCloseBrowser(line);
                }
                else
                {
                    if (PlayOrXMission)
                    {
                        if (showScores)
                        {
                            if (updatingScores)
                            {
                                DrawScoreboard(line);
                                line++;
                                DrawCloseScoreboard(line);
                            }
                            else
                            {
                                DrawScoreboard(line);
                                line++;
                                line++;
                                DrawScoreboard0(line);
                                line++;
                                DrawScoreboard1(line);
                                line++;
                                DrawScoreboard2(line);
                                line++;
                                DrawScoreboard3(line);
                                line++;
                                DrawScoreboard4(line);
                                line++;
                                DrawScoreboard5(line);
                                line++;
                                DrawScoreboard6(line);
                                line++;
                                DrawScoreboard7(line);
                                line++;
                                DrawScoreboard8(line);
                                line++;
                                DrawScoreboard9(line);
                                line++;
                                line++;
                                DrawUpdateScoreboard(line);
                                line++;
                                DrawCloseScoreboard(line);
                            }
                        }
                        else
                        {
                            DrawPlayHoloCacheName(line);
                            line++;
                            line++;
                            DrawPlayMissionType(line);
                            line++;

                            if (!geoCache)
                            {
                                DrawPlayRaceType(line);
                                line++;
                                line++;
                                DrawChallengerName(line);
                                line++;
                            }
                            if (blueprintsAdded)
                            {
                                line++;
                                DrawPlayBlueprintsAdded(line);
                                line++;
                            }

                            line++;
                            DrawDescription0(line);
                            line++;
                            if (missionDescription1 != "" && missionDescription1 != string.Empty)
                            {
                                DrawDescription1(line);
                                line++;

                                if (missionDescription2 != "" && missionDescription2 != string.Empty)
                                {
                                    DrawDescription2(line);
                                    line++;

                                    if (missionDescription3 != "" && missionDescription3 != string.Empty)
                                    {
                                        DrawDescription3(line);
                                        line++;

                                        if (missionDescription4 != "" && missionDescription4 != string.Empty)
                                        {
                                            DrawDescription4(line);
                                            line++;

                                            if (missionDescription5 != "" && missionDescription5 != string.Empty)
                                            {
                                                DrawDescription5(line);
                                                line++;

                                                if (missionDescription6 != "" && missionDescription6 != string.Empty)
                                                {
                                                    DrawDescription6(line);
                                                    line++;

                                                    if (missionDescription7 != "" && missionDescription7 != string.Empty)
                                                    {
                                                        DrawDescription7(line);
                                                        line++;

                                                        if (missionDescription8 != "" && missionDescription8 != string.Empty)
                                                        {
                                                            DrawDescription8(line);
                                                            line++;

                                                            if (missionDescription9 != "" && missionDescription9 != string.Empty)
                                                            {
                                                                DrawDescription9(line);
                                                                line++;

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            line++;
                            if (!geoCache)
                            {
                                DrawShowScoreboard(line);
                                line++;
                                line++;
                                DrawStart(line);
                            }
                            line++;
                            DrawCancel(line);
                        }

                        _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
                        _windowRect.height = _windowHeight;
                    }
                    else
                    {
                        DrawTitle(line);
                        line++;

                        if (!addCoords)
                        {
                            DrawHoloCacheName2(line);
                            line++;
                            DrawPassword(line);
                            line++;
                            DrawEditDescription(line);
                            line++;
                            DrawEditDescription2(line);
                            line++;
                            DrawDescription0(line);
                            line++;
                            if (missionDescription0 != "" && missionDescription0 != string.Empty)
                            {
                                DrawDescription1(line);
                                line++;

                                if (missionDescription1 != "" && missionDescription1 != string.Empty)
                                {
                                    DrawDescription2(line);
                                    line++;

                                    if (missionDescription2 != "" && missionDescription2 != string.Empty)
                                    {
                                        DrawDescription3(line);
                                        line++;

                                        if (missionDescription3 != "" && missionDescription3 != string.Empty)
                                        {
                                            DrawDescription4(line);
                                            line++;

                                            if (missionDescription4 != "" && missionDescription4 != string.Empty)
                                            {
                                                DrawDescription5(line);
                                                line++;

                                                if (missionDescription5 != "" && missionDescription5 != string.Empty)
                                                {
                                                    DrawDescription6(line);
                                                    line++;

                                                    if (missionDescription6 != "" && missionDescription6 != string.Empty)
                                                    {
                                                        DrawDescription7(line);
                                                        line++;

                                                        if (missionDescription7 != "" && missionDescription7 != string.Empty)
                                                        {
                                                            DrawDescription8(line);
                                                            line++;

                                                            if (missionDescription8 != "" && missionDescription8 != string.Empty)
                                                            {
                                                                DrawDescription9(line);
                                                                line++;

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (missionDescription0 != "" && missionDescription0 != string.Empty)
                            {
                                DrawClearDescription(line);
                                line++;
                            }

                            DrawMissionType(line);
                            line++;
                            if (!geoCache)
                            {
                                DrawRaceType(line);
                                line++;
                            }
                            line += 0.4f;
                            DrawVessel(line);
                            line++;
                            line += 0.4f;
                            DrawAddBlueprints(line);
                            line++;
                            DrawSaveLocal(line);
                            line++;
                            if (saveLocalVessels)
                            {
                                line += 0.4f;
                                DrawLocalSaveRangeText(line);
                                line++;
                                DrawLocalSaveRange(line);
                                line++;
                                line += 0.3f;
                            }

                            line++;
                            DrawSave(line);
                            line++;
                            DrawCancel(line);
                        }
                        else
                        {
                            DrawAddCoords(line);
                            line++;
                            if (locAdded)
                            {
                                line += 0.5f;
                                DrawCoordCount(line);
                                line++;
                                DrawClearLastCoord(line);
                                line++;
                                DrawClearAllCoords(line);
                                line++;
                                line++;
                                DrawSave(line);
                                line++;
                            }
                            DrawCancel(line);
                        }
                    }
                }

                toolWindowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
                WindowRectToolbar.height = toolWindowHeight;
            }
            else
            {
                GUI.DragWindow(new Rect(30, 0, toolWindowWidth - 90, 30));

                if (HighLogic.LoadedSceneIsEditor)
                {
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

                }
                else
                {
                    if (HighLogic.LoadedSceneIsFlight)
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

                        GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum", titleStyle);

                        if (scanningForHolo)
                        {
                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Closest Holo is " + Math.Round((targetDistance / 1000), 2) + " km away", titleStyle);

                            line++;

                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Heading: " + Math.Round(heading, 2) + " degrees", titleStyle);

                            line++;

                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "ETA @current speed: " + Math.Round(scanDelay / 6, 1) + " minutes", titleStyle);

                            line++;

                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Next Scan in " + Math.Round(scanDelay, 2) + " seconds", titleStyle);

                            line += 1.25f;

                            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Cancel Scan", OrXGUISkin.button))
                            {
                                scanningForHolo = false;
                                checking = false;
                            }
                        }
                        else
                        {
                            if (!hide)
                            {
                                line ++;

                                if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Scan For HoloCache", OrXGUISkin.button))
                                {
                                    scanningForHolo = true;
                                    checking = true;
                                    ClearDatabase();
                                }

                                line += 1.25f;

                                if (!spawnHoloCache)
                                {
                                    if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Create HoloCache", OrXGUISkin.button))
                                    {
                                        if (FlightGlobals.ActiveVessel.isEVA)
                                        {
                                            if (FlightGlobals.ActiveVessel.LandedOrSplashed)
                                            {
                                                // SPAWN HOLOCACHE
                                                GuiEnabledOrXMissions = true;
                                                PlayOrXMission = false;
                                                building = true;
                                                buildingMission = true;
                                                geoCache = true;
                                                OrXHCGUIEnabled = false;
                                                boid = false;

                                                //SetupHolo();
                                                Vector3d hl = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude + 1.5);
                                                StartCoroutine(SpawnHoloCache(hl, false, true, string.Empty));
                                            }
                                            else
                                            {
                                                //buildingMission = true;
                                                //OrXMissions.instance.StartMissionBuilder(new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude), new Guid());
                                                //HideGameUI();

                                                ScreenMsg("<color=#cc4500ff><b>You must be Landed or Splashed</b></color>");
                                                ScreenMsg("<color=#cc4500ff><b>to create a HoloCache</b></color>");
                                            }
                                        }
                                        else
                                        {
                                            ScreenMsg("<color=#cc4500ff><b>You must be EVA to create a HoloCache</b></color>");
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
                    }
                }

                toolWindowHeight = Mathf.Lerp(toolWindowHeight, contentTop + (line * entryHeight) + 5, 1);
                WindowRectToolbar.height = toolWindowHeight;

            }
        }
        private void WindowHCGUI()
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
                                    }

                                    if (!reload && !hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name + " : " + label, OrXGUISkin.box))
                                        {
                                            scanning = false;
                                            checking = false;
                                            TargetHCGUI = false;
                                            reload = true;
                                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Description</b></color>");
                                            CheckSOI();
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                        {
                                            scanning = false;
                                            checking = false;
                                            TargetHCGUI = false;
                                            reload = true;
                                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Description</b></color>");
                                            CheckSOI();
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
                                    }

                                    if (!hide)
                                    {
                                        if (!reload)
                                        {
                                            if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                CheckSOI();
                                            }

                                            if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                CheckSOI();
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
                                            _craftFile = coordinate.Current.name;
                                            if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
                                            {

                                            }

                                            // LOAD HOLOCACHE DESCRIPTION WINDOW
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                        {
                                            HoloCacheName = coordinate.Current.name;

                                            TargetHCGUIIndex = index;
                                            TargetHCGUI = true;
                                            resetTargetHCGUI = false;
                                            scanning = true;
                                            _craftFile = coordinate.Current.name;
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
                                        _craftFile = coordinate.Current.name;
                                        // LOAD HOLOCACHE DESCRIPTION WINDOW
                                    }

                                    if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                    {
                                        HoloCacheName = coordinate.Current.name;

                                        TargetHCGUIIndex = index;
                                        TargetHCGUI = true;
                                        resetTargetHCGUI = false;
                                        scanning = true;
                                        _craftFile = coordinate.Current.name;
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

                        if (scanning)
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                    }

                                    if (!reload && !hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, 240, HCGUIEntryHeight), coordinate.Current.name + " " + label, OrXGUISkin.box))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                checking = false;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                CheckSOI();
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
                                    }

                                    if (!hide)
                                    {
                                        if (!reload)
                                        {
                                            if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, 240, HCGUIEntryHeight), coordinate.Current.name + " " + label, OrXGUISkin.box))
                                            {
                                                if (HighLogic.LoadedSceneIsFlight)
                                                {
                                                    HoloCacheName = "";

                                                    scanning = false;
                                                    checking = false;
                                                    TargetHCGUI = false;
                                                    reload = true;
                                                    ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                    CheckSOI();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, 240, HCGUIEntryHeight), coordinate.Current.name + " " + label, OrXGUISkin.box))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (soi == FlightGlobals.currentMainBody.name)
                                                {
                                                    OrXHCGUIEnabled = false;
                                                    HoloCacheName = coordinate.Current.name;

                                                    TargetHCGUIIndex = index;
                                                    TargetHCGUI = true;
                                                    resetTargetHCGUI = false;
                                                    scanning = true;
                                                    _lat = coordinate.Current.gpsCoordinates.x;
                                                    _lon = coordinate.Current.gpsCoordinates.y;
                                                    _alt = coordinate.Current.gpsCoordinates.z;
                                                    tpoint = new Vector3d(_lat, _lon, _alt);
                                                    _craftFile = coordinate.Current.name;
                                                    mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
                                                    degPerMeter = 1 / mPerDegree;

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
                                    if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, 240, HCGUIEntryHeight), coordinate.Current.name + " " + label, OrXGUISkin.button))
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            if (soi == FlightGlobals.currentMainBody.name)
                                            {
                                                OrXHCGUIEnabled = false;

                                                HoloCacheName = coordinate.Current.name;
                                                HoloCacheName = coordinate.Current.name;
                                                craftFile = coordinate.Current.name;
                                                TargetHCGUIIndex = index;
                                                TargetHCGUI = true;
                                                resetTargetHCGUI = false;
                                                scanning = true;
                                                _craftFile = coordinate.Current.name;
                                                _lat = coordinate.Current.gpsCoordinates.x;
                                                _lon = coordinate.Current.gpsCoordinates.y;
                                                _alt = coordinate.Current.gpsCoordinates.z;
                                                tpoint = new Vector3d(_lat, _lon, _alt);

                                                mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
                                                degPerMeter = 1 / mPerDegree;

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

        #region Coords GUI

        public Vessel triggerVessel;
        public bool _triggered = false;
        int waypointColor = 0;
        bool addCoordSetup = false;

        public void AddMissionCoords()
        {
            triggerKerbSetup = true;
            building = true;
            buildingMission = true;
            _triggered = true;
            addCoordSetup = true;
            addCoords = true;
            System.Random r = new System.Random();
            waypointColor = (int)(r.NextDouble() * seeds.Count());
            triggerVessel = FlightGlobals.ActiveVessel;
            challengeHolo = _HoloKron;

            StartCoroutine(AddWaypoint(new Vector3d(latMission, lonMission, altMission), true));
            SpawnByChallengeBuilder(triggerVessel);
        }
        private void AddMissionCoordsSetup()
        {
            var h = challengeHolo.rootPart.FindModuleImplementing<ModuleOrXMission>();
            h._holoSetup = true;
            OrXVesselMove.Instance._moveMode = 0;
            FlightGlobals.ForceSetActiveVessel(challengeHolo);
            OrXVesselMove.Instance.StartMove(challengeHolo, true);
        }

        public void HoloAddCoords()
        {
            if (!locAdded)
            {
                locAdded = true;
            }

            locCount += 1;
            latMission = FlightGlobals.ActiveVessel.latitude;
            lonMission = FlightGlobals.ActiveVessel.longitude;
            altMission = FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude +1;
            //windIntensity = WindGUI.instance.windIntensity;
            //windVariability = WindGUI.instance.windVariability;
            //variationIntensity = WindGUI.instance.variationIntensity;
            //heading = WindGUI.instance.heading;
            //teaseDelay = WindGUI.instance.teaseDelay;
            // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay

            if (windRacing)
            {
                if (FlightGlobals.ActiveVessel.altitude >= 0 && FlightGlobals.ActiveVessel.atmDensity >= 0.007)
                {
                    CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                        + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                }
                else
                {
                    ScreenMsg("Unable to add coordinate to Wind Challenge if vessel is below water or not in an atmosphere");
                }
            }
            else
            {
                if (Scuba)
                {
                    if (FlightGlobals.ActiveVessel.Splashed)
                    {
                        CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                    }
                    else
                    {
                        ScreenMsg("Unable to add coordinate to Scuba Challenge if vessel is not Splashed");
                    }

                }
                else
                {
                    CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                        + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                }
            }

            if (challengeType == "DAKAR RACING")
            {
                StartCoroutine(AddWaypoint(new Vector3d(latMission, lonMission, altMission), false));
            }
            else
            {
                StartCoroutine(SpawnHoloCache(new Vector3d(latMission, lonMission, altMission), true, false, HoloCacheName + "-STAGE " + locCount));
            }
        }
        IEnumerator AddWaypoint(Vector3d vect, bool start)
        {
            Waypoint waypoint = new Waypoint();
            System.Random r = new System.Random();
            int waypointColor = (int)(r.NextDouble() * seeds.Count());
            waypoint.id = "marker";
            waypoint.seed = seeds[waypointColor];
            waypoint.name = craftToSpawn + locCount;
            waypoint.celestialName = FlightGlobals.currentMainBody.name;
            waypoint.longitude = vect.y;
            waypoint.latitude = vect.x;
            waypoint.height = FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude + 1;//TerrainHeight(lat, longi, FlightGlobals.ActiveVessel.mainBody);
            waypoint.altitude = alt - waypoint.height;
            int _iconsize = Convert.ToInt32(waypoint.iconSize / 2);
            waypoint.iconSize = _iconsize;
            Debug.Log("[OrX Target Manager] ====================================== ADDING WAYPOINT FOR " + craftToSpawn + " ===");
            ScenarioCustomWaypoints.RemoveWaypoint(waypoint);
            ScenarioCustomWaypoints.AddWaypoint(waypoint);


            if (!start)
            {
                if (challengeType == "DAKAR RACING")
                {
                    yield return new WaitForFixedUpdate();
                    OrXVesselMove.Instance.EndMove();
                }
            }
            else
            {

            }
        }

        bool dakarSetup = false;

        public void DakarContinue()
        {
            dakarSetup = true;
            int dakarStageCount = hcCount + 1;
            StartCoroutine(SpawnHoloCache(new Vector3d(challengeHolo.latitude, challengeHolo.longitude, challengeHolo.altitude + 2), false, false, HoloCacheName + "-STAGE " + dakarStageCount));
        }

        public void DrawAddCoords(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "ADD LOCATION", HighLogic.Skin.button))
            {
                //OrXVesselMove.Instance._moveMode = OrXVesselMove.MoveModes.Slow;
                HoloAddCoords();
                //OrXVesselMove.Instance.EndMove();
            }
        }
        private void DrawClearLastCoord(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "DELETE LAST", HighLogic.Skin.button))
            {
                CoordDatabase.Remove(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                locCount -= 1;
                if (CoordDatabase.Count == 0)
                {
                    locAdded = false;
                }
                /*
                Waypoint waypoint = new Waypoint();
                System.Random r = new System.Random();
                waypoint.id = "marker";
                waypoint.seed = seeds[waypointColor];
                waypoint.name = craftToSpawn + locCount;
                waypoint.celestialName = FlightGlobals.currentMainBody.name;
                waypoint.longitude = lonMission;
                waypoint.latitude = latMission;
                waypoint.height = FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude + 1;//TerrainHeight(lat, longi, FlightGlobals.ActiveVessel.mainBody);
                waypoint.altitude = alt - waypoint.height;

                Debug.Log("[OrX Target Manager] ====================================== REMOVING WAYPOINT " + craftToSpawn + locCount + " ===");

                ScenarioCustomWaypoints.RemoveWaypoint(waypoint);

                */
            }
        }
        private void DrawClearAllCoords(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "DELETE ALL", HighLogic.Skin.button))
            {
                CoordDatabase.Clear();
                locCount = 0;
                locAdded = false;
            }
        }
        private void DrawCoordCount(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Coord Count = " + locCount, titleStyle);
        }

        #endregion

        #region Scoreboard GUI

        private void DrawScoreboard(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionName + " Scoreboard", titleStyle);
        }
        private void DrawScoreboard0(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB0 + " - " + String.Format("{0:0.00}", timeSB0), titleStyle);
        }
        private void DrawScoreboard1(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB1 + " - " + String.Format("{0:0.00}", timeSB1), titleStyle);
        }
        private void DrawScoreboard2(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB2 + " - " + String.Format("{0:0.00}", timeSB2), titleStyle);
        }
        private void DrawScoreboard3(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB3 + " - " + String.Format("{0:0.00}", timeSB3), titleStyle);
        }
        private void DrawScoreboard4(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB4 + " - " + String.Format("{0:0.00}", timeSB4), titleStyle);
        }
        private void DrawScoreboard5(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB5 + " - " + String.Format("{0:0.00}", timeSB5), titleStyle);
        }
        private void DrawScoreboard6(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB6 + " - " + String.Format("{0:0.00}", timeSB6), titleStyle);
        }
        private void DrawScoreboard7(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB7 + " - " + String.Format("{0:0.00}", timeSB7), titleStyle);
        }
        private void DrawScoreboard8(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB8 + " - " + String.Format("{0:0.00}", timeSB8), titleStyle);
        }
        private void DrawScoreboard9(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), nameSB9 + " - " + String.Format("{0:0.00}", timeSB9), titleStyle);
        }
        private void DrawUpdateScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "IMPORT SCORES", HighLogic.Skin.button))
            {
                ImportScores();
            }
        }
        private void DrawCloseScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CLOSE SCOREBOARD", HighLogic.Skin.button))
            {
                showScores = false;
            }
        }

        #endregion

        #region Play Mission GUI

        private void DrawPlayHoloCacheName(float line)
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

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth, 20), HoloCacheName, titleStyle);
        }
        private void DrawPlayMissionName(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.UpperLeft
            };

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth, 20), missionName, titleStyle);
        }
        private void DrawPlayMissionType(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloCache Type: " + missionType, titleStyle);
        }
        private void DrawPlayRaceType(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Challenge Type: " + challengeType, titleStyle);
        }
        private void DrawPlayBlueprintsAdded(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.UpperLeft
            };

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth, 20), "Bluprints: "+ crafttosave, titleStyle);
        }
        private void DrawChallengerName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenger: ",
                leftLabel);
            float textFieldWidth = 80;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            challengersName = GUI.TextField(fwdFieldRect, challengersName);
        }

        private void DrawPlayPassword(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password: ",
                leftLabel);
            float textFieldWidth = 80;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            Password = GUI.TextField(fwdFieldRect, Password);
        }
        private void DrawUnlock(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "UNLOCK", HighLogic.Skin.button))
            {
                if (Password == pas)
                {
                    Debug.Log("[OrX Mission] === UNLOCKING ===");

                    unlocked = true;
                }
                else
                {
                    Debug.Log("[OrX Mission] === WRONG PASSWORD ===");

                    ScreenMsg("WRONG PASSWORD");
                }
            }
        }
        private void DrawShowScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!updatingScores)
            {
                if (GUI.Button(saveRect, "SHOW SCOREBOARD", HighLogic.Skin.button))
                {
                    updatingScores = true;
                    Debug.Log("[OrX Mission] === SHOW SCOREBOARD ===");
                    GetScoreboardData();
                }
            }
            else
            {
                if (GUI.Button(saveRect, "UPDATING SCOREBOARD", HighLogic.Skin.box))
                {
                }
            }
        }

        private void DrawStart(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!geoCache)
            {
                if (GUI.Button(saveRect, "START CHALLENGE", HighLogic.Skin.button))
                {
                    if (challengersName != "" || challengersName != string.Empty)
                    {
                        Debug.Log("[OrX Mission] === NAME ENTERED - STARTING ===");
                        if (!challengeRunning)
                        {
                            challengeRunning = true;
                            StartCoroutine(StartChallenge());
                        }
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === PLEASE ENTER CHALLENGER NAME ===");

                        ScreenMsg("Please enter a challenger name");
                    }
                }
            }
            else
            {
                if (GUI.Button(saveRect, "CLOSE WINDOW", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === HOLO IS GEO-CACHE - CLOSING WINDOW ===");

                    challengeRunning = true;
                    StartCoroutine(StartChallenge());
                }
            }
        }

        #endregion

        #region Description Window GUI

        private void DrawEditTitle(float line)
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Description Editor", titleStyle);
        }
        private void DrawClearDescription(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CLEAR DESCRIPTION", HighLogic.Skin.button))
            {
                Debug.Log("[OrX Mission] === CLEARING DESCRIPTION ===");

                missionDescription0 = string.Empty;
                missionDescription1 = string.Empty;
                missionDescription2 = string.Empty;
                missionDescription3 = string.Empty;
                missionDescription4 = string.Empty;
                missionDescription5 = string.Empty;
                missionDescription6 = string.Empty;
                missionDescription7 = string.Empty;
                missionDescription8 = string.Empty;
                missionDescription9 = string.Empty;
            }
        }
        private void DrawSaveDescription(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
            {
                Debug.Log("[OrX Mission] === SAVING DESCRIPTION ===");

                editDescription = false;
            }
        }

        private void DrawDescription0(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription0, titleStyle);
            }
            else
            {
                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription0 = GUI.TextField(fwdFieldRect, missionDescription0);
            }
        }
        private void DrawDescription1(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription1, titleStyle);
            }
            else
            {

                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription1 = GUI.TextField(fwdFieldRect, missionDescription1);
            }
        }
        private void DrawDescription2(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription2, titleStyle);
            }
            else
            {

                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription2 = GUI.TextField(fwdFieldRect, missionDescription2);
            }
        }
        private void DrawDescription3(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription3, titleStyle);
            }
            else
            {

                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription3 = GUI.TextField(fwdFieldRect, missionDescription3);
            }
        }
        private void DrawDescription4(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription4, titleStyle);
            }
            else
            {

                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription4 = GUI.TextField(fwdFieldRect, missionDescription4);
            }
        }
        private void DrawDescription5(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription5, titleStyle);
            }
            else
            {
                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription5 = GUI.TextField(fwdFieldRect, missionDescription5);
            }
        }
        private void DrawDescription6(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription6, titleStyle);
            }
            else
            {
                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription6 = GUI.TextField(fwdFieldRect, missionDescription6);
            }
        }
        private void DrawDescription7(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription7, titleStyle);
            }
            else
            {

                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription7 = GUI.TextField(fwdFieldRect, missionDescription7);
            }
        }
        private void DrawDescription8(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription8, titleStyle);
            }
            else
            {

                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription8 = GUI.TextField(fwdFieldRect, missionDescription8);
            }
        }
        private void DrawDescription9(float line)
        {
            if (PlayOrXMission)
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

                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionDescription9, titleStyle);
            }
            else
            {

                var leftLabel = new GUIStyle();
                leftLabel.alignment = TextAnchor.UpperLeft;
                leftLabel.normal.textColor = Color.white;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                    leftLabel);
                float textFieldWidth = 220;
                var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                    ContentTop + line * entryHeight, textFieldWidth, entryHeight);
                missionDescription9 = GUI.TextField(fwdFieldRect, missionDescription9);
            }
        }

        #endregion

        #region Craft Browser GUI

        private void DrawCraftBrowserTitle(float line)
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

            if (holoHangar)
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Bag of Holding", titleStyle);
            }
            else
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Blueprints Browser", titleStyle);
            }
        }
        private void DrawHangar(float line)
        {
            if (holoHangar)
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

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKrons Available", titleStyle);
            }
            else
            {
                var sphButton = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, 90, entryHeight);
                var vabButton = new Rect(((LeftIndent * 1.5f) * 2) + 90, ContentTop + line * entryHeight, 90, entryHeight);

                if (sph)
                {
                    if (GUI.Button(sphButton, "SPH", HighLogic.Skin.box))
                    {
                    }

                    if (GUI.Button(vabButton, "VAB", HighLogic.Skin.button))
                    {
                        sph = false;
                    }
                }
                else
                {
                    if (GUI.Button(sphButton, "SPH", HighLogic.Skin.button))
                    {
                        sph = true;
                    }

                    if (GUI.Button(vabButton, "VAB", HighLogic.Skin.box))
                    {
                    }
                }
            }
        }
        private void DrawCloseBrowser(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CANCEL", HighLogic.Skin.button))
            {
                blueprintsLabel = "Add Blueprints to Holo";
                craftBrowserOpen = false;
            }
        }

        #endregion

        #region Challenge Creator GUI

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

            if (addCoords)
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Co-ordinate Editor", titleStyle);
            }
            else
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX HoloCache Creator", titleStyle);
            }
        }
        private void DrawHoloCacheName2(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Name: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            HoloCacheName = GUI.TextField(fwdFieldRect, HoloCacheName);
        }
        private void DrawMissionType(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "HoloCache Type: ",
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight);

            if (geoCache)
            {
                if (GUI.Button(bfRect, missionType, HighLogic.Skin.button))
                {
                    //locAdded = true;
                    if (!locAdded)
                    {
                        ScreenMsg("HOLOCACHE TYPE CHANGED TO CHALLENGE");
                        Debug.Log("[OrX Mission] === HOLOCACHE TYPE - CHALLENGE ===");

                        geoCache = false;
                        missionType = "CHALLENGE";
                        challengeType = "DAKAR RACING";
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === HOLOCACHE LOCKED AS GEO-CACHE ===");
                        ScreenMsg("HOLOCACHE TYPE LOCKED AS GEO-CACHE");

                    }
                }
            }
            else
            {
                if (GUI.Button(bfRect, missionType, HighLogic.Skin.button))
                {
                    locAdded = false;
                    if (!locAdded)
                    {
                        ScreenMsg("HOLOCACHE TYPE CHANGED TO GEO-CACHE");
                        Debug.Log("[OrX Mission] === HOLOCACHE TYPE - GEO-CACHE ===");
                        challengeType = "GEO_CACHE";
                        missionType = "GEO-CACHE";
                        geoCache = true;
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === HOLOCACHE LOCKED AS CHALLENGE ===");
                        ScreenMsg("HOLOCACHE TYPE LOCKED AS CHALLENGE");

                    }
                }
            }
        }
        private void DrawRaceType(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenge Type: ",
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight);

            if (geoCache)
            {
                if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                {
                    //locAdded = true;
                    if (!locAdded)
                    {
                        Debug.Log("[OrX Mission] === CHALLENGE TYPE - DAKAR RACING ===");
                        //challengeType = "SCUBA";
                        challengeType = "DAKAR RACING";
                        ScreenMsg("CHALLENGE TYPE CHANGED TO " + challengeType);

                        geoCache = false;
                        windRacing = false;
                        Scuba = false;
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS GEO-CACHE ===");
                        ScreenMsg("CHALLENGE TYPE LOCKED AS GEO-CACHE");
                    }
                }
            }
            else
            {
                if (windRacing && !Scuba)
                {
                    if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                    {
                        if (!locAdded)
                        {
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE - WIND RACING ===");
                            challengeType = "SCUBA";

                            windRacing = false;
                            Scuba = true;
                        }
                        else
                        {
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS WIND RACING ===");

                        }
                    }
                }
                else
                {
                    if (Scuba)
                    {
                        if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                        {
                            if (!locAdded)
                            {
                                Debug.Log("[OrX Mission] === CHALLENGE TYPE - SCUBA ===");
                                challengeType = "DAKAR RACING";

                                windRacing = false;
                                Scuba = false;
                            }
                            else
                            {
                                Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS SCUBA ===");

                            }

                        }
                    }
                    else
                    {
                        if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                        {
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS DAKAR RACING ===");
                            ScreenMsg("CHALLENGE TYPE LOCKED AS DAKAR RACING");

                            if (!locAdded)
                            {
                                //Debug.Log("[OrX Mission] === CHALLENGE TYPE - DAKAR RACING ===");
                                // challengeType = "WIND RACING";
                                //challengeType = "GEO_CACHE";
                                //geoCache = true;
                               // windRacing = true;
                               // Scuba = false;
                            }
                            else
                            {
                                //Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS DAKAR RACING ===");
                                //ScreenMsg("CHALLENGE TYPE LOCKED AS DAKAR RACING");

                            }

                        }
                    }
                }

            }
        }
        private void DrawVessel(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Blueprint Data", titleStyle);
        }
        private void DrawAddBlueprints(float line)
        {
            //var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), blueprintsLabel,
                leftLabel);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!blueprintsAdded)
            {
                if (GUI.Button(bfRect, "", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === ADDING BLUEPRINTS ===");
                    addingBluePrints = true;
                    blueprintsFile = "";
                    PlayOrXMission = false;

                    craftBrowserOpen = true;
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", HighLogic.Skin.box))
                {
                    Debug.Log("[OrX Mission] === REMOVING BLUEPRINTS ===");
                    blueprintsLabel = "Add Blueprints to Holo";
                    blueprintsFile = "";
                    blueprintsAdded = false;
                }
            }

        }

        private void DrawPassword(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password:",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            Password = GUI.TextField(fwdFieldRect, Password);
        }
        private void DrawModule(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Tech: ",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            tech = GUI.TextField(fwdFieldRect, tech);
        }

        private void DrawEditDescription(float line)
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
                "Edit the HoloCache description below", titleStyle);
        }
        private void DrawEditDescription2(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "(Press TAB to jump to next line)", titleStyle);
        }

        private void DrawSaveLocal(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), saveLocalLabel,
                leftLabel);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!saveLocalVessels)
            {
                if (GUI.Button(bfRect, "", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === SAVE LOCAL VESSELS = TRUE ===");
                    saveLocalVessels = true;
                    saveLocalLabel = "Saving Local Craft";
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", HighLogic.Skin.box))
                {
                    Debug.Log("[OrX Mission] === SAVE LOCAL VESSELS = FALSE ===");
                    saveLocalVessels = false;
                    saveLocalLabel = "Save Local Craft";
                }
            }
        }
        private void DrawLocalSaveRangeText(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 11,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth, 20), "Local Vessel Save Range", titleStyle);
        }

        private void DrawLocalSaveRange(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 11,
                alignment = TextAnchor.MiddleCenter
            };

            var saveRect = new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth - (LeftIndent * 2), entryHeight);
            GUI.Label(new Rect(0, (ContentTop + line * entryHeight) + line, WindowWidth, 20), 
                String.Format("{0:0}", localSaveRange) + " meters", titleStyle);
            localSaveRange = GUI.HorizontalSlider(saveRect, localSaveRange, 50, 1000);
        }

        private void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!geoCache)
            {
                if (addCoords)
                {
                    if (GUI.Button(saveRect, "SAVE AND EXIT", HighLogic.Skin.button))
                    {
                        checking = false;
                        Debug.Log("[OrX Mission] === SAVING .orx ===");
                        _HoloKron = challengeHolo;
                        dakarSetup = false;
                        var h = _HoloKron.rootPart.FindModuleImplementing<ModuleOrXMission>();
                        h.boid = true;
                        h.KillHolo();
                        FlightGlobals.ForceSetActiveVessel(triggerVessel);
                    }
                }
                else
                {
                    if (GUI.Button(saveRect, "ADD COORDS", HighLogic.Skin.button))
                    {
                        if (HoloCacheName != string.Empty && HoloCacheName != "")
                        {
                            if (missionDescription0 != string.Empty && missionDescription0 != "")
                            {
                                OrXLog.instance.SetFocusKeys();
                                CoordDatabase = new List<string>();

                                _file = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
                                if (_file == null)
                                {
                                    checking = false;
                                    addCoords = true;
                                    AddMissionCoords();
                                }
                                else
                                {
                                    ConfigNode node = _file.GetNode("OrX");

                                    foreach (ConfigNode spawnCheck in node.nodes)
                                    {
                                        if (spawnCheck.name.Contains("HC0OrXv"))
                                        {
                                            if (spawnCheck.name == "HC0OrXv0")
                                            {
                                                ConfigNode location = spawnCheck.GetNode("coords");

                                                foreach (ConfigNode.Value loc in location.values)
                                                {
                                                    string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                                    string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                                    loc.name = cvEncryptedName;
                                                    loc.value = cvEncryptedValue;

                                                    if (loc.name == "pas")
                                                    {
                                                        pas = loc.value;

                                                        if (Password == pas)
                                                        {
                                                            addCoords = true;
                                                            OrXAppendCfg.instance.EnableGui();
                                                        }
                                                        else
                                                        {
                                                            Debug.Log("[OrX Mission] === WRONG PASSWORD ===");

                                                            ScreenMsg("WRONG PASSWORD");
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
                                ScreenMsg("Please add a description");
                            }
                        }
                        else
                        {
                            ScreenMsg("Please enter a name for your HoloCache");
                        }
                    }
                }
            }
            else
            {
                if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
                {
                    if (HoloCacheName != string.Empty && HoloCacheName != "")
                    {
                        if (missionDescription0 != string.Empty && missionDescription0 != "")
                        {
                            _file = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName +  ".orx");
                            if (_file == null)
                            {
                                Debug.Log("[OrX Mission] === SAVING .orx ===");
                                GuiEnabledOrXMissions = false;
                                OrXHCGUIEnabled = false;
                                building = false;
                                buildingMission = false;
                                OrXLog.instance.building = false;
                                SaveConfig();
                            }
                            else
                            {
                                if (Password == pas)
                                {
                                    OrXAppendCfg.instance.EnableGui();
                                }
                                else
                                {
                                    Debug.Log("[OrX Mission] === WRONG PASSWORD ===");

                                    ScreenMsg("WRONG PASSWORD");
                                }
                            }
                        }
                        else
                        {
                            ScreenMsg("Please add a description");
                        }
                    }
                    else
                    {
                        ScreenMsg("Please enter a name for your HoloCache");
                    }
                }
            }
        }
        private void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            string label = string.Empty;

            if (GUI.Button(saveRect, "CANCEL", HighLogic.Skin.button))
            {
                if (PlayOrXMission)
                {
                    Debug.Log("[OrX Mission] === CANCEL CHALLENGE ===");
                    CancelChallenge();
                }
                else
                {
                    Debug.Log("[OrX Mission] === CANCEL HOLOCACHE CREATION ===");

                    var h = _HoloKron.rootPart.FindModuleImplementing<ModuleOrXMission>();
                    h.KillHolo();
                    CancelChallenge();
                }
            }
        }

        public void CancelChallenge()
        {
            Debug.Log("[OrX Mission] === CANCEL CHALLENGE ===");

            if (addCoords && FlightGlobals.ActiveVessel.rootPart.Modules.Contains<ModuleOrXMission>())
            {
                FlightGlobals.ForceSetActiveVessel(triggerVessel);
            }

            locCount = 0;
            locAdded = false;
            building = false;
            buildingMission = false;
            addCoords = false;
            GuiEnabledOrXMissions = false;
            OrXHCGUIEnabled = false;
            PlayOrXMission = false;
            ResetData();
        }

        #endregion

    }
}