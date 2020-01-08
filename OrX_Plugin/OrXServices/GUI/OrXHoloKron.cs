using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;
using System.IO;
using System.Text;
using System.Linq;
using FinePrint;
using OrX.spawn;
using System.Reflection;
using System.Net;
using UnityEngine.SceneManagement;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXHoloKron : MonoBehaviour
    {
        public static OrXHoloKron instance;

        #region Variables

        public CraftBrowserDialog craftBrowser;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        public static bool TBBadded = false;
        public bool OrXHCGUIEnabled;
        public static Rect WindowRectToolbar;
        public const float WindowWidth = 250;
        public const float DraggableHeight = 40;
        public const float LeftIndent = 12;
        public const float ContentTop = 20;
        public bool GuiEnabledOrXMissions = false;
        public readonly float _incrButtonWidth = 26;
        public readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        public readonly float entryHeight = 20;
        public float _windowHeight = 250;
        public Rect _windowRect;

        public StringBuilder debugString = new StringBuilder();

        public bool _timing = false;
        public bool _getCenterDist = false;
        public bool _editor = false;
        public double airTime = 0;
        public bool reset = false;
        public bool mrKleen = false;
        public bool devKitInstalled = false;
        public string fileDateTime = "";
        public string fileSize = "";
        public string _ate = "9";
        public bool _spawningOrX = false;
        public bool _bdacSet = false;

        public Vector3 UpVect;
        public Vector3 EastVect;
        public Vector3 NorthVect;
        public Vector3 worldPos;

        public Vector3 targetPos = Vector3.zero;
        public Vessel triggerVessel;

        public List<string> OrXCoordsList;
        public List<string> OrXChallengeList;
        public List<string> OrXChallengeNameList;
        public List<string> OrXGeoCacheNameList;
        public List<string> OrXLoadedFileList;
        public List<string> OrXGeoCacheCreatorList;
        public List<string> OrXChallengeCreatorList;

        public Vessel startGate;
        public bool dakarRacing = false;
        public bool openingCraftBrowser = false;
        public bool spawningGoal = false;

        public bool addNextCoord = false;

        public string _sth = string.Empty;
        public bool showScores = false;

        public bool unlocked = false;

        public bool buildingMission = false;
        public bool showTargets = false;

        public bool scanning = false;
        public bool spawnHoloKron = false;

        float toolWindowWidth = 250;
        float toolWindowHeight = 100;

        public int count = 0;

        public bool reload;
        public string HoloKronName = string.Empty;

        public float minLoadRange = 1000;
        public bool TargetHCGUI = false;
        public string soi = "";

        public Vector3d tpoint;

        public bool addCoords = false;
        public bool passive = false;
        public bool paused = false;
        public int delay = 300;
        public bool sth = true;
        public bool hide = false;

        public bool reloadWorldPos = false;
        public string targetLabel;

        public float timer = 1;
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
        public bool spawned = false;
        public string Gold = string.Empty;
        public string Silver = string.Empty;
        public string Bronze = string.Empty;

        [KSPField(isPersistant = true)]
        public bool blueprintsAdded = false;
        public string crafttosave = string.Empty;
        public string blueprintsLabel = "Add Blueprints to Holo";

        public bool saveShip = false;
        public string craftToAddMission = string.Empty;

        public double _altitude = 0;

        public double distance = 0;
        public float missionTime = 0;

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

        public bool _missionDescription9_ = false;
        public bool _missionDescription8_ = false;
        public bool _missionDescription7_ = false;
        public bool _missionDescription6_ = false;
        public bool _missionDescription5_ = false;
        public bool _missionDescription4_ = false;
        public bool _missionDescription3_ = false;
        public bool _missionDescription2_ = false;
        public bool _missionDescription1_ = false;

        public string textBox = string.Empty;

        public bool bdaChallenge = false;
        public bool outlawRacing = false;
        public bool LBC = false;
        public bool Scuba = false;
        public bool windRacing = false;
        public bool snowballFight = false;
        public bool PlayOrXMission = false;
        public bool movingCraft = false;
        public bool building = false;
        public bool pauseCheck = false;
        public bool resetHoloKron = false;
        public bool checkingMission = false;
        public bool geoCache = true;
        public bool addingBluePrints = false;
        public bool locAdded = false;
        public bool holoSelected = false;
        public bool holoSpawned = false;
        public bool editDescription = false;

        public float pitch = 0;
        public float heading = 0;
        public float windIntensity = 10;
        public float teaseDelay = 0;
        public float windVariability = 50;
        public float variationIntensity = 50;

        public static Rect WindowRectBrowser;

        public string Password = "OrX";
        public string pas = string.Empty;

        public string missionCraftFile = string.Empty;
        public string blueprintsFile = string.Empty;
        public string holoToAdd = string.Empty;

        public int gpsCount = 0;
        public double latMission = 0;
        public double lonMission = 0;
        public double altMission = 0;

        public Vector3d lastCoord;
        public int locCount = 0;

        public List<string> CoordDatabase;
        public int coordCount = 0;
        public List<string> ModuleDatabase;

        public Vector3d startLocation;

        public List<string> stageTimes;
        public double maxDepth = 0;

        public List<string> _scoreboard;
        public string challengersName = string.Empty;
        public double topSurfaceSpeed = 0;

        public int ec = 0;

        public ConfigNode _file;
        public ConfigNode _mission;
        public ConfigNode _scoreboard_;
        public ConfigNode _blueprints_;
        public ConfigNode _stageGates;

        public ConfigNode scoreboard0;
        public ConfigNode scoreboard1;
        public ConfigNode scoreboard2;
        public ConfigNode scoreboard3;
        public ConfigNode scoreboard4;
        public ConfigNode scoreboard5;
        public ConfigNode scoreboard6;
        public ConfigNode scoreboard7;
        public ConfigNode scoreboard8;
        public ConfigNode scoreboard9;

        public string nameSB0 = string.Empty;
        public string timeSB0 = string.Empty;
        public string saltSB0 = string.Empty;

        public string nameSB1 = string.Empty;
        public string timeSB1 = string.Empty;
        public string saltSB1 = string.Empty;

        public string nameSB2 = string.Empty;
        public string timeSB2 = string.Empty;
        public string saltSB2 = string.Empty;

        public string nameSB3 = string.Empty;
        public string timeSB3 = string.Empty;
        public string saltSB3 = string.Empty;

        public string nameSB4 = string.Empty;
        public string timeSB4 = string.Empty;
        public string saltSB4 = string.Empty;

        public string nameSB5 = string.Empty;
        public string timeSB5 = string.Empty;
        public string saltSB5 = string.Empty;

        public string nameSB6 = string.Empty;
        public string timeSB6 = string.Empty;
        public string saltSB6 = string.Empty;

        public string nameSB7 = string.Empty;
        public string timeSB7 = string.Empty;
        public string saltSB7 = string.Empty;

        public string nameSB8 = string.Empty;
        public string timeSB8 = string.Empty;
        public string saltSB8 = string.Empty;

        public string nameSB9 = string.Empty;
        public string timeSB9 = string.Empty;
        public string saltSB9 = string.Empty;

        public bool updatingScores = false;

        public Vector3 nextLocation;
        public double targetDistanceMission = 0;
        public string _targetDistance = string.Empty;
        public Vessel targetCoord;

        public Quaternion rot;
        public double _lat = 0f;
        public double _lon = 0f;
        public double _alt = 0f;

        public ConfigNode craft = null;
        public string shipDescription = string.Empty;

        public string craftToSpawn = string.Empty;
        public string cfgToLoad = string.Empty;
        string OrXv = "OrXv";
        string _OrXV = "";
        public Vessel _lastStage;
        public Vessel _HoloKron;
        public string holoKronCraftLoc = string.Empty;
        public List<string> holoKronFiles;
        public string sphLoc = string.Empty;
        public List<string> sphFiles;
        public string vabLoc = string.Empty;
        public List<string> vabFiles;
        public bool sph = true;
        public bool holoHangar = false;

        public bool saveLocalVessels = false;
        public string saveLocalLabel = "Save Local Craft";
        public float localSaveRange = 1000;

        internal static List<ProtoCrewMember> SelectedCrewData;

        public string craftFile = string.Empty;
        public string flagURL = string.Empty;
        public float spawnTimer = 0.0f;
        public bool loadingCraft = false;

        public double _lat_ = 0.0f;
        public double _lon_ = 0.0f;

        public bool holo = true;
        public bool emptyholo = true;
        public int hkCount = 0;
        public bool showHoloTargets = false;
        public int GoalCount = 10;
        public int spawnRadius = 0;
        public bool Goal = false;
        public string missionCraftLoc = string.Empty;
        public bool coordSpawn = false;

        public bool holoOpen = false;
        public Waypoint currentWaypoint;
        public double mPerDegree = 0;
        public double degPerMeter = 0;
        public float scanDelay = 0;
        public float holoHeading = 0;

        public bool addingMission = false;
        public bool addingGoalPosts = false;
        public bool startGateSpawned = false;
        public bool spawningStartGate = false;
        public bool stageStart = false;
        public bool shortTrackRacing = false;

        public string raceType = "DAKAR RACING";
        public string loginName = "";
        public bool connectToKontinuum = false;
        string pasKontinuum = "";
        string _labelConnect = "Connect to Kontinuum";
        string urlKontinuum = "";
        bool _a2 = false;
        bool _s = false;

        Vector3d playerPositon;
        public bool _KontinuumConnect = false;
        public string Karma = "ssadab";
        public bool _blink = false;
        public bool _blinking = false;

        public bool showChallengelist = false;
        public bool showGeoCacheList = false;
        public bool _showSettings = false;
        public bool _settings0 = false;
        public bool _settings1 = false;
        public bool _settings2 = false;
        public string gpsString = "";
        public string groupName = "";

        public bool killingChallenge = false;
        public bool showCreatedHolokrons = false;
        public bool IronKerbal = false;

        public string statsMods = "";
        public string statsName = "";
        public string statsTime = "";
        public double statsMaxSpeed = 0;
        public string statsTotalAirTime = "";
        public double statsMaxDepth = 0;
        bool _showMode = false;
        bool _b = false;
        bool _a = false;


        public List<string> scoreboardStats;
        public bool _extractScoreboard = false;
        bool _d = false;
        bool _s2 = false;
        bool _modeEnabled = false;

        public Vector3d boidPos;

        public bool _importingScores = false;
        public string currentScoresFile = "";
        public Type partModules;

        public bool getNextCoord = false;
        public bool modCheckFail = false;
        public Vector2 scrollPosition = Vector2.zero;
        public Vector2 scrollPosition2 = Vector2.zero;
        public bool _placingChallenger = false;
        public bool hkSpawned = false;
        public List<string> _orxFileMods;
        public List<string> _orxFilePartModules;
        public bool disablePRE = false;
        public ConfigNode PREsettings;
        public string _pKarma = string.Empty;
        public List<string> installedMods;
        public bool _preInstalled = false;

        private readonly List<AvailablePart> parts = new List<AvailablePart>();
        public bool _showPartModules = false;
        public bool _scoreSaved = false;
        public bool _gateKillDelay = false;
        public bool _killPlace = false;

        public bool _showTimer = false;
        public double _timer = 0;
        public double _missionStartTime = 0;
        public float _time = 0;
        public string _currentOrXFile = "";
        public Rigidbody _rb;
        public Vector3d _challengeStartLoc;

        public static List<string> ModWhitelist = new List<string>()
        {
        "DCKinc",
        "PysicsRangeExtender",
        "Squad",
        "VesselMover",
        "OrX"
        };
        public static List<string> colliderWhitelist = new List<string>()
        {
        "Kerbin",
        "Mun",
        "Laythe",
        "Minmus",
        "Vall"
        };

        public string _timerStageTime = "";
        public string _timerTotalTime = "";

        public bool _killingLastCoord = false;
        public Vector3 targetLoc;

        int _removalAttempt = 0;
        public bool _spawningVessel = false;
        public bool _addCrew = true;
        public bool _holdVesselPos = false;

        public bool _airSuperiority = false;
        public bool _navalBattle = false;
        public bool _groundAssault = false;
        public float _saveAltitude = 2000;
        public bool _settingAltitude = false;

        public bool _timerOn = false;
        public bool _refuel = false;
        public float salt = 0;
        public Vessel _spawnedCraft;

        public bool _spawnBoss = false;
        public bool _spawnBrute = false;
        public int _killCount = 0;
        public int _lifeCount = 0;
        public int _playerVesselCount = 0;
        public int _opForCount = 0;

        bool _bdacSaved = false;
        public bool _savingAirSup = false;
        public bool _savingAirSup1 = false;
        public bool _savingAirSup2 = false;
        public bool _savingAirSup3 = false;
        public string _airSupName1 = "<empty>";
        public string _airSupName2 = "<empty>";
        public string _airSupName3 = "<empty>";
        public string _airSupFile1 = "";
        public string _airSupFile2 = "";
        public string _airSupFile3 = "";
        public float _spawnChance = 0;

        public bool _deleteWarning = false;
        int _entryCount = 10;

        #endregion

        bool stopwatch = false;
        bool pauseStopWatch = false;



        #region GUI Styles

        static GUIStyle leftLabel = new GUIStyle()
        {
            alignment = TextAnchor.UpperLeft,
            normal = { textColor = Color.white }
        };
        static GUIStyle leftLabelBooger = new GUIStyle()
        {
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = XKCDColors.BoogerGreen },
            fontStyle = FontStyle.Bold
        };
        static GUIStyle leftLabelYellow = new GUIStyle()
        {
            alignment = TextAnchor.UpperLeft,
            normal = { textColor = Color.yellow }
        };
        static GUIStyle leftLabelRed = new GUIStyle()
        {
            alignment = TextAnchor.UpperLeft,
            normal = { textColor = Color.red }
        };
        static GUIStyle titleStyleMedBooger = new GUIStyle(leftLabelBooger)
        {
            fontSize = 13,
            alignment = TextAnchor.UpperLeft,
            fontStyle = FontStyle.BoldAndItalic
        };
        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        };
        static GUIStyle centerLabelOrange = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = XKCDColors.OrangeRed }
        };

        static GUIStyle centerLabelRed = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.red }
        };
        static GUIStyle centerLabelYellow = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.yellow }
        };
        static GUIStyle centerLabelGreen = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = XKCDColors.BoogerGreen }
        };
        static GUIStyle titleStyle = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold

        };
        static GUIStyle titleStyleYellow = new GUIStyle(centerLabelYellow)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold

        };
        static GUIStyle titleStyleRed = new GUIStyle(centerLabelRed)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold

        };
        static GUIStyle titleStyleMedNoItal = new GUIStyle(centerLabelOrange)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        static GUIStyle titleStyleMed = new GUIStyle(centerLabelYellow)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic
        };
        static GUIStyle titleStyleMedRed = new GUIStyle(centerLabelRed)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic
        };
        static GUIStyle titleStyleMedYellow = new GUIStyle(centerLabelYellow)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic
        };
        static GUIStyle titleStyleMedGreen = new GUIStyle(centerLabelGreen)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic
        };
        static GUIStyle titleStyleLarge = new GUIStyle(centerLabelOrange)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        GUIStyle rangeColor = titleStyle;

        #endregion

        #region Core

        public void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
            _HoloKron = null;
            resetHoloKron = false;
        }
        void Start()
        {
            OrXCoordsList = new List<string>();
            OrXChallengeList = new List<string>();
            OrXGeoCacheNameList = new List<string>();
            OrXGeoCacheCreatorList = new List<string>();
            OrXChallengeCreatorList = new List<string>();
            ModuleDatabase = new List<string>();

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Import/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Import/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Export/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Kontinuum/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Kontinuum/");

            OrXHCGUIEnabled = false;
            TBBAdd();
            TargetHCGUI = false;
            spawnHoloKron = false;
            scanning = false;

            string path = UrlDir.ApplicationRootPath + "GameData/";
            installedMods = new List<string>();

            foreach (string s in Directory.GetDirectories(path))
            {
                installedMods.Add(s.Remove(0, path.Length));
                Debug.Log("[OrX HoloKron - Start] === ADDING " + s.Remove(0, path.Length) + " TO INSTALLED MOD DATABASE ===");
            }
            /*
            parts.Clear();
            int count = PartLoader.LoadedPartsList.Count;
            for (int i = 0; i < count; ++i)
            {
                var avPart = PartLoader.LoadedPartsList[i];
                if (!avPart.partPrefab) continue;

                parts.Add(avPart);

                foreach (AvailablePart.ModuleInfo mi in avPart.moduleInfos)
                {
                    if (!ModuleDatabase.Contains(mi.moduleName))
                    {
                        Debug.Log("[OrX HoloKron - Start] === moduleDisplayName: " + mi.moduleDisplayName + " TO PART MODULE DATABASE ===");
                        Debug.Log("[OrX HoloKron - Start] === info: " + mi.info + " ===");
                        Debug.Log("[OrX HoloKron - Start] === Primary Info: " + mi.primaryInfo + " ===");
                        Debug.Log("[OrX HoloKron - Start] === moduleName " + mi.moduleName + " TO PART MODULE DATABASE ===");

                        ModuleDatabase.Add(mi.moduleName);
                    }
                }
            }
            */
            WindowRectToolbar = new Rect(Screen.width - (WindowWidth + 50), 50, toolWindowWidth, toolWindowHeight);
            string[] _v = Application.version.Split(new char[] { '.' });
            _OrXV = _v[1];

            ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            if (playerData != null)
            {
                challengersName = playerData.GetValue("name");
                if (challengersName != "")
                {
                    //creatorName = challengersName;
                    //loginName = creatorName;
                }
            }

            GameEvents.onVesselSOIChanged.Add(checkSOI);
            GameEvents.onGameStateLoad.Add(resetHoloKronSystem);
            //GameEvents.OnFlightGlobalsReady.Add(onFlightGlobalsReady);
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (OrXLog.instance._debugLog && _showSettings)
                {
                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        _showMode = true;
                    }
                }
                /*
                if (!buildingMission && challengeRunning)
                {
                    if (_showTimer && bdaChallenge)
                    {
                        if (Input.GetKeyDown(KeyCode.LeftBracket))
                        {
                            OrXVesselLog.instance.SwitchToPreviousVessel();
                        }
                        if (Input.GetKeyDown(KeyCode.RightBracket))
                        {
                            OrXVesselLog.instance.SwitchToNextVessel();
                        }
                        if (Input.GetKeyDown(KeyCode.PageDown))
                        {
                            OrXVesselLog.instance.SwitchToPreviousVessel();
                        }
                        if (Input.GetKeyDown(KeyCode.PageUp))
                        {
                            OrXVesselLog.instance.SwitchToNextVessel();
                        }
                    }
                }
                */
            }
        }

        private void onFlightGlobalsReady(bool data)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                /*
                if (challengeRunning)
                {
                    salt = 0;
                    challengeRunning = false;
                    OnScrnMsgUC("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
                    locCount = 0;
                    locAdded = false;
                    building = false;
                    buildingMission = false;
                    addCoords = false;
                    OrXHCGUIEnabled = false;
                    ResetData();
                }
                */
            }
        }
        public void TBBAdd()
        {
            string OrXDir = "OrX/Plugin/";

            if (!TBBadded)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(OrXDir + "OrX_icon", false);
                ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, Blank, Blank, Blank, Blank,
                    ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.SPH
                    | ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPACECENTER, buttonTexture);
                TBBadded = true;
            }
        }
        public void checkSOI(GameEvents.HostedFromToAction<Vessel, CelestialBody> data)
        {
            OrXLog.instance.DebugLog("[OrX Check SOI] === CHECKING ===");

            if (soi != FlightGlobals.ActiveVessel.mainBody.name)
            {
                OrXLog.instance.DebugLog("[OrX Check SOI] === '" + soi + "' doesn't match '" + FlightGlobals.ActiveVessel.mainBody.name + "' ===");
                soi = FlightGlobals.ActiveVessel.mainBody.name;
            }
        }
        private void resetHoloKronSystem(ConfigNode data)
        {
            if (challengeRunning)
            {
                salt = 0;
                challengeRunning = false;
                OnScrnMsgUC("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
                locCount = 0;
                locAdded = false;
                building = false;
                buildingMission = false;
                addCoords = false;
                OrXHCGUIEnabled = false;
                ResetData();
            }
        }
        public void OnScrnMsgUC(string _text)
        {
            if (!_blink)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage(_text, 4, ScreenMessageStyle.UPPER_CENTER));
            }
            else
            {
                _blink = false;
                _blinking = true;
                StartCoroutine(OnScrnMsgUCBlink(_text));
            }
        }
        public IEnumerator OnScrnMsgUCBlink(string _text)
        {
            if (_blinking)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage(_text + " " + OrXSpawnHoloKron.instance._vesselName, 0.9f, ScreenMessageStyle.UPPER_CENTER));
                yield return new WaitForSeconds(1.1f);
                StartCoroutine(OnScrnMsgUCBlink(_text));
            }
        }
        public void ResetData()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                OrXSpawnHoloKron.instance.spawning = false;
            }
            groupName = "";
            _timerOn = false;
            _timing = false;
            missionTime = 0;
            _missionStartTime = 0;
            _time = 0;
            _getCenterDist = false;
            _showTimer = false;
            _lifeCount = 0;
            _killCount = 0;
            _addCrew = false;
            startGate = null;
            _scoreSaved = false;
            _gateKillDelay = false;
            dakarRacing = false;
            openingCraftBrowser = false;
            spawningGoal = false;
            addNextCoord = false;
            _sth = string.Empty;
            showScores = false;
            unlocked = false;
            buildingMission = false;
            showTargets = false;
            scanning = false;
            spawnHoloKron = false;
            count = 0;
            HoloKronName = string.Empty;
            minLoadRange = 1000;
            TargetHCGUI = false;
            addCoords = false;
            passive = false;
            paused = false;
            delay = 300;
            sth = true;
            hide = false;
            reloadWorldPos = false;
            timer = 1;
            _craftFile = string.Empty;
            checking = false;
            targetDistance = 0;
            challengeRunning = false;
            setup = false;
            completed = false;
            missionName = string.Empty;
            missionType = string.Empty;
            challengeType = string.Empty;
            tech = string.Empty;
            spawned = false;
            Gold = string.Empty;
            Silver = string.Empty;
            Bronze = string.Empty;
            blueprintsAdded = false;
            crafttosave = string.Empty;
            blueprintsLabel = "Add Blueprints to Holo";
            saveShip = false;
            craftToAddMission = string.Empty;
            _altitude = 0;
            distance = 0;
            missionTime = 0;
            techToAdd = string.Empty;

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

            _missionDescription9_ = false;
            _missionDescription8_ = false;
            _missionDescription7_ = false;
            _missionDescription6_ = false;
            _missionDescription5_ = false;
            _missionDescription4_ = false;
            _missionDescription3_ = false;
            _missionDescription2_ = false;
            _missionDescription1_ = false;

            textBox = string.Empty;

            bdaChallenge = false;
            outlawRacing = false;
            Scuba = false;
            windRacing = false;
            snowballFight = false;
            PlayOrXMission = false;
            movingCraft = false;
            building = false;
            pauseCheck = false;
            resetHoloKron = false;
            checkingMission = false;
            geoCache = true;
            addingBluePrints = false;
            locAdded = false;
            holoSelected = false;
            holoSpawned = false;
            editDescription = false;

            pitch = 0;
            heading = 0;
            windIntensity = 10;
            teaseDelay = 0;
            windVariability = 50;
            variationIntensity = 50;

            Password = "OrX";
            pas = string.Empty;

            missionCraftFile = string.Empty;
            blueprintsFile = string.Empty;
            holoToAdd = string.Empty;

            gpsCount = 0;
            latMission = 0;
            lonMission = 0;
            altMission = 0;
            locCount = 0;
            coordCount = 0;
            maxDepth = 0;
            topSurfaceSpeed = 0;
            ec = 0;

            nameSB0 = string.Empty;
            timeSB0 = string.Empty;
            saltSB0 = string.Empty;

            nameSB1 = string.Empty;
            timeSB1 = string.Empty;
            saltSB1 = string.Empty;

            nameSB2 = string.Empty;
            timeSB2 = string.Empty;
            saltSB2 = string.Empty;

            nameSB3 = string.Empty;
            timeSB3 = string.Empty;
            saltSB3 = string.Empty;

            nameSB4 = string.Empty;
            timeSB4 = string.Empty;
            saltSB4 = string.Empty;

            nameSB5 = string.Empty;
            timeSB5 = string.Empty;
            saltSB5 = string.Empty;

            nameSB6 = string.Empty;
            timeSB6 = string.Empty;
            saltSB6 = string.Empty;

            nameSB7 = string.Empty;
            timeSB7 = string.Empty;
            saltSB7 = string.Empty;

            nameSB8 = string.Empty;
            timeSB8 = string.Empty;
            saltSB8 = string.Empty;

            nameSB9 = string.Empty;
            timeSB9 = string.Empty;
            saltSB9 = string.Empty;

            updatingScores = false;
            targetDistanceMission = 0;
            _targetDistance = string.Empty;

            _lat = 0f;
            _lon = 0f;
            _alt = 0f;

            craft = null;
            shipDescription = string.Empty;

            craftToSpawn = string.Empty;
            cfgToLoad = string.Empty;
            OrXv = "OrXv";
            holoKronCraftLoc = string.Empty;
            sphLoc = string.Empty;
            vabLoc = string.Empty;
            sph = true;
            holoHangar = false;

            saveLocalVessels = false;
            saveLocalLabel = "Save Local Craft";

            craftFile = string.Empty;
            flagURL = string.Empty;
            spawnTimer = 0.0f;
            loadingCraft = false;

            _lat_ = 0.0f;
            _lon_ = 0.0f;

            emptyholo = true;
            hkCount = 0;
            showHoloTargets = false;
            GoalCount = 10;
            spawnRadius = 0;
            Goal = false;
            missionCraftLoc = string.Empty;
            coordSpawn = false;

            holoOpen = false;
            mPerDegree = 0;
            degPerMeter = 0;
            scanDelay = 0;
            holoHeading = 0;

            addingMission = false;
            addingGoalPosts = false;
            startGateSpawned = false;
            spawningStartGate = false;
            stageStart = false;
            shortTrackRacing = false;

            //raceType = "";
            //loginName = "";
            connectToKontinuum = false;
            //pasKontinuum = "";
            _labelConnect = "Connect to Kontinuum";
            urlKontinuum = "";
            _timerStageTime = "00:00:00.00";
            _timerTotalTime = "00:00:00.00";

            _KontinuumConnect = false;
            _file = null;

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

            MainMenu();
        }
        public void Blank() { }

        #endregion

        /// <summary>
        /// //////////////////////////////
        /// </summary>

        #region Build Challenge

        public bool _spawnByOrX = false;

        public void SpawnByOrX()
        {
            if (FlightGlobals.ActiveVessel.isEVA)
            {
                _spawnByOrX = true;
                triggerVessel = FlightGlobals.ActiveVessel;
                OrXSpawnHoloKron.instance.CraftSelect(false, true, false);
            }
            else
            {
                OnScrnMsgUC("Please get out of your vehicle");
            }
        }
        public void SetupHolo(Vessel v, Vector3d holoPosition)
        {
            mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
            degPerMeter = 1 / mPerDegree;

            HoloKronName = "";
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            building = true;
            buildingMission = true;
            geoCache = true;
            OrXHCGUIEnabled = true;
            Goal = false;
            triggerVessel = FlightGlobals.ActiveVessel;

            _lat = holoPosition.x;
            _lon = holoPosition.y;
            _alt = holoPosition.z;
            _HoloKron = v;
            holoToAdd = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloKron/HoloKron.craft";
            missionType = "GEO-CACHE";
            challengeType = "GEO-CACHE";
            raceType = "";
            locAdded = false;
            OrXLog.instance.building = true;
            startLocation = holoPosition;
            lastCoord = startLocation;
            showScores = false;
            movingCraft = false;
            spawned = true;
            _getCenterDist = true;
            GetShortTrackCenter(holoPosition);
        }

        public void ClearLastCoord()
        {
            OrXLog.instance.DebugLog("[OrX Clear Last Coord] === CLEARING LAST COORD FROM DATABASE ===");

            string coordsToRemove = "";
            try
            {
                List<Vessel>.Enumerator _gates = FlightGlobals.VesselsLoaded.GetEnumerator();
                while (_gates.MoveNext())
                {
                    if (_gates.Current != null)
                    {
                        if (_gates.Current.rootPart.Modules.Contains<ModuleOrXStage>())
                        {
                            OrXLog.instance.DebugLog("[OrX Clear Last Coord] === GATE FOUND ===");

                            var g = _gates.Current.rootPart.FindModuleImplementing<ModuleOrXStage>();
                            if (g != null)
                            {
                                if (g._stageCount == locCount)
                                {
                                    OrXLog.instance.DebugLog("[OrX Clear Last Coord] === " + _gates.Current.vesselName + " matches count ===");
                                    if (!_gates.Current.packed)
                                    {
                                        _gates.Current.rootPart.AddModule("ModuleOrXJason", true);
                                    }
                                    else
                                    {
                                        _gates.Current.Die();
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                _gates.Dispose();

                List<string>.Enumerator coords = CoordDatabase.GetEnumerator();
                while (coords.MoveNext())
                {
                    if (coords.Current != null)
                    {
                        string[] _locCount = coords.Current.Split(new char[] { ',' });
                        if (_locCount[0] == locCount.ToString())
                        {
                            OrXLog.instance.DebugLog("[OrX Clear Last Coord] === " + coords.Current + " found in coordinate list ===");

                            coordsToRemove = coords.Current;
                            break;
                        }
                    }
                }
                coords.Dispose();

                if (coordsToRemove != "")
                {
                    OrXLog.instance.DebugLog("[OrX Clear Last Coord] === Removing " + coords.Current + " from the coordinate list ===");

                    CoordDatabase.Remove(coordsToRemove);
                }

                locCount -= 1;

                if (locCount == 0)
                {
                    locAdded = false;
                }
                else
                {
                    List<string>.Enumerator _lastCoords = CoordDatabase.GetEnumerator();
                    while (_lastCoords.MoveNext())
                    {
                        if (_lastCoords.Current != null)
                        {
                            string[] _locCount = _lastCoords.Current.Split(new char[] { ',' });
                            if (_locCount[0] == locCount.ToString())
                            {
                                latMission = double.Parse(_locCount[1]);
                                lonMission = double.Parse(_locCount[2]);
                                altMission = double.Parse(_locCount[3]);
                                lastCoord = new Vector3d(latMission, lonMission, altMission);
                                break;
                            }
                        }
                    }
                    coords.Dispose();
                }
            }
            catch (Exception e)
            {
                if (_removalAttempt <= 2)
                {
                    _removalAttempt += 1;
                    OrXLog.instance.DebugLog("[OrX Clear Last Coord] === ERROR ... Attempt: " + _removalAttempt + " === " + e);
                    ClearLastCoord();
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Clear Last Coord] === ERROR ... Still having issues === " + e);
                }
            }
        }
        public void ChallengeAddNextCoord()
        {
            FlightGlobals.ActiveVessel.SetWorldVelocity(Vector3.zero);
            FlightGlobals.ActiveVessel.angularVelocity = Vector3.zero;
            FlightGlobals.ActiveVessel.angularMomentum = Vector3.zero;

            OnScrnMsgUC("Adding Current GPS Location ..... ");

            if (!locAdded)
            {
                locAdded = true;
            }

            if (dakarRacing)
            {
                saveLocalVessels = true;
                spawningStartGate = false;
                getNextCoord = true;
                StartCoroutine(AddCoordsToList(FlightGlobals.ActiveVessel));
            }
            else
            {
                if (shortTrackRacing)
                {
                    StartCoroutine(AddCoordsToList(null));
                    dakarRacing = false;
                    spawningStartGate = false;
                    getNextCoord = true;
                }
                else
                {
                    if (LBC)
                    {
                        StartCoroutine(AddCoordsToList(null));
                        saveLocalVessels = true;
                        spawningStartGate = false;
                        getNextCoord = true;
                        addingMission = true;
                        getNextCoord = true;
                        showTargets = false;
                        movingCraft = false;
                        OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 0, false, false, new Vector3d());
                    }
                    else
                    {
                        if (Scuba)
                        {
                            if (FlightGlobals.ActiveVessel.Splashed)
                            {
                                locCount += 1;
                                lastCoord = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude + 5);
                                latMission = lastCoord.x;
                                lonMission = lastCoord.y;
                                altMission = lastCoord.z;
                                windIntensity = WindGUI.instance.windIntensity;
                                windVariability = WindGUI.instance.windVariability;
                                variationIntensity = WindGUI.instance.variationIntensity;
                                heading = WindGUI.instance.heading;
                                teaseDelay = WindGUI.instance.teaseDelay;
                                startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);

                                // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                                CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                                    + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                            }
                            else
                            {
                                OnScrnMsgUC("Unable to add coordinate to Scuba Challenge if vessel is not Splashed");
                            }
                        }
                    }
                }
            }

            OrXLog.instance.DebugLog("[OrX Challenge Add Next Coord - Coords After] === Lat: " + FlightGlobals.ActiveVessel.latitude + " = Lon: " + FlightGlobals.ActiveVessel.longitude + " = Alt: " + FlightGlobals.ActiveVessel.latitude + " === ");
        }
        IEnumerator AddCoordsToList(Vessel toSave)
        {
            movingCraft = true;
            locCount += 1;
            lastCoord = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude + 4);
            latMission = lastCoord.x;
            lonMission = lastCoord.y;
            altMission = lastCoord.z;
            windIntensity = WindGUI.instance.windIntensity;
            windVariability = WindGUI.instance.windVariability;
            variationIntensity = WindGUI.instance.variationIntensity;
            heading = WindGUI.instance.heading;
            teaseDelay = WindGUI.instance.teaseDelay;

            // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
            CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
            OrXLog.instance.DebugLog("[OrX Challenge Add Next Coord - Coords Before] === Lat: " + FlightGlobals.ActiveVessel.latitude + " = Lon: " + FlightGlobals.ActiveVessel.longitude + " = Alt: " + FlightGlobals.ActiveVessel.latitude + " === ");

            startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
            GuiEnabledOrXMissions = true;
            OrXHCGUIEnabled = true;
            addCoords = true;
            movingCraft = true;
            addingMission = true;
            if (dakarRacing)
            {
                _getCenterDist = false;
                _killPlace = false;

                /*
                OrXLog.instance.DebugLog("[OrX Save Stage] === SAVING STAGE GATE ===");
                string hConfigLoc2 = UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + creatorName + ".tmp";
                toSave = FlightGlobals.ActiveVessel;
                string shipDescription = HoloKronName + " STAGE GATE #" + locCount;
                OrXLog.instance.DebugLog("[OrX Save Stage] Saving " + FlightGlobals.ActiveVessel.vesselName + "'s orientation .......................");

                UpVect = (toSave.transform.position - toSave.mainBody.position).normalized;
                EastVect = toSave.mainBody.getRFrmVel(toSave.CoM).normalized;
                NorthVect = Vector3.Cross(EastVect, UpVect).normalized;

                float _pitch = Vector3.Angle(toSave.ReferenceTransform.forward, UpVect);
                float _left = Vector3.Angle(-toSave.ReferenceTransform.right, NorthVect); // left is 90 degrees behind vessel heading

                if (Math.Sign(Vector3.Dot(-toSave.ReferenceTransform.right, EastVect)) < 0)
                {
                    _left = 360 - _left; // westward headings become angles greater than 180
                }

                // Be sure to subtract 90 degrees from pitch and left as the vessel reference transform is offset 90 degrees
                // from the respective vectors due to reasons

                ShipConstruct ConstructToSave = new ShipConstruct(FlightGlobals.ActiveVessel.vesselName, shipDescription, FlightGlobals.ActiveVessel.parts[0]);
                ConfigNode craftConstruct = new ConfigNode("craft");
                craftConstruct = ConstructToSave.SaveShip();

                //craftConstruct.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "Vessel" + count + ".proto");

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

                OrXLog.instance.DebugLog("[OrX Save Stage] Saving: " + FlightGlobals.ActiveVessel.vesselName);
                OnScrnMsgUC("<color=#cfc100ff><b>Saving: " + FlightGlobals.ActiveVessel.vesselName + "</b></color>");
                ConfigNode _holoNode = _file.GetNode(hkCount.ToString());
                ConfigNode _orxNode = _holoNode.GetNode("OrX");
                
                if (_orxNode.HasNode("STAGEGATE" + locCount))
                {
                    _orxNode.RemoveNode("STAGEGATE" + locCount);
                }

                ConfigNode craftData = _orxNode.AddNode("STAGEGATE" + locCount);
                craftData.AddValue("vesselName", FlightGlobals.ActiveVessel.vesselName);
                ConfigNode location = craftData.AddNode("coords");
                location.AddValue("holo", hkCount);
                location.AddValue("pas", Password);
                location.AddValue("lat", FlightGlobals.ActiveVessel.latitude);
                location.AddValue("lon", FlightGlobals.ActiveVessel.longitude);
                location.AddValue("alt", FlightGlobals.ActiveVessel.altitude + 1);
                location.AddValue("left", _left);
                location.AddValue("pitch", _pitch);

                Quaternion or = toSave.rootPart.transform.rotation;
                location.AddValue("rot", or.x + "," + or.y + "," + or.z + "," + or.w);

                foreach (ConfigNode.Value cv in location.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }
                saveShip = false;
                
                ConfigNode craftFile = craftData.AddNode("craft");
                OnScrnMsgUC("<color=#cfc100ff><b>Saving to " + HoloKronName + " " + hkCount + "</b></color>");
                craftConstruct.CopyTo(craftFile);
                //craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + v.Current.vesselName + "-" + count + ".craft");

                // ADD ENCRYPTION

                foreach (ConfigNode.Value cv in craftFile.values)
                {
                    if (cv.name == "ship")
                    {
                        cv.value = FlightGlobals.ActiveVessel.vesselName;
                    }

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
                            if (!cv2.value.Contains("(") && !cv2.value.Contains(")"))
                            {
                                string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                                string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                                cv2.name = cvEncryptedName;
                                cv2.value = cvEncryptedValue;
                            }
                        }
                    }
                }

                OrXLog.instance.DebugLog("[OrX Save Stage] === " + FlightGlobals.ActiveVessel.vesselName + " ENCRYPTED ===");
                OrXLog.instance.DebugLog("[OrX Save Stage] " + FlightGlobals.ActiveVessel.vesselName + " Saved to " + HoloKronName);
                OnScrnMsgUC("<color=#cfc100ff><b>" + FlightGlobals.ActiveVessel.vesselName + " Saved</b></color>");
                
                _file.Save(hConfigLoc2);
                toSave.rootPart.AddModule("ModuleOrXDakarGate", true);
                yield return new WaitForFixedUpdate();
                */
            }
            yield return new WaitForFixedUpdate();

            if (FlightGlobals.ActiveVessel != _HoloKron)
            {
                FlightGlobals.ForceSetActiveVessel(_HoloKron);
            }
        }

        public void GetShortTrackCenter(Vector3d _boidPos)
        {
            rangeColor = titleStyle;
            _getCenterDist = true;
            checking = true;
            StartCoroutine(GetCenterShortTrackRoutine(_boidPos));
        }
        IEnumerator GetCenterShortTrackRoutine(Vector3d _boidPos)
        {
            yield return new WaitForFixedUpdate();

            if (_getCenterDist)
            {
                targetDistance = OrXUtilities.instance.GetDistance(_boidPos.y, _boidPos.x, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.latitude, (FlightGlobals.ActiveVessel.altitude + _boidPos.z) / 2);

                if (dakarRacing)
                {
                    if (buildingMission)
                    {
                        if (targetDistance <= 25)
                        {
                            _killPlace = true;
                            rangeColor = titleStyleMedRed;
                        }
                        else
                        {
                            if (targetDistance >= 50000)
                            {
                                if (targetDistance >= 100000)
                                {
                                    _killPlace = true;
                                    rangeColor = titleStyleMedRed;
                                }
                                else
                                {
                                    _killPlace = false;
                                    rangeColor = titleStyleMedYellow;
                                }
                            }
                            else
                            {
                                _killPlace = false;
                                rangeColor = titleStyleMedGreen;
                            }
                        }
                    }
                }
                else
                {
                    if (targetDistance <= 25)
                    {
                        _killPlace = true;
                        rangeColor = titleStyleMedRed;
                    }
                    else
                    {
                        if (targetDistance >= 3000)
                        {
                            if (targetDistance >= 4000)
                            {
                                _killPlace = true;
                                rangeColor = titleStyleMedRed;
                            }
                            else
                            {
                                _killPlace = false;
                                rangeColor = titleStyleMedYellow;
                            }
                        }
                        else
                        {
                            _killPlace = false;
                            rangeColor = titleStyleMedGreen;
                        }
                    }
                }
                yield return new WaitForFixedUpdate();

                StartCoroutine(GetCenterShortTrackRoutine(_boidPos));
            }
        }
        IEnumerator GateKillDelayRoutine()
        {
            _gateKillDelay = true;

            if (OrXUtilities.instance.GetDistance(lonMission, latMission, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.latitude, (FlightGlobals.ActiveVessel.altitude + altMission) / 2) >= 15)
            {
                targetLoc = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission);
                OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 50, false, true, new Vector3d(latMission, lonMission, altMission));
            }
            else
            {
                _gateKillDelay = false;
            }
            while (_gateKillDelay)
            {
                yield return null;
            }

            ClearLastCoord();
            yield return new WaitForSeconds(2.5f);
            _spawningVessel = false;
            spawningStartGate = false;
            getNextCoord = true;
        }

        public void SaveConfig(string holoName, bool append)
        {
            StartCoroutine(SaveConfigRoutine(holoName, append));
        }
        IEnumerator SaveConfigRoutine(string holoName, bool append)
        {
            OrXLog.instance.SetRange(triggerVessel, 75000);
            string hConfigLoc2 = UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp";
            hkCount = 0;

            if (challengersName != "")
            {
                //creatorName = challengersName;
            }
            if (groupName != "")
            {
                //challengersName = creatorName;
            }
            ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            if (playerData == null)
            {
                playerData = new ConfigNode();
            }
            playerData.SetValue("name", challengersName, true);
            playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");

            building = true;
            buildingMission = true;
            bool _continue = true;
            if (holoName != "" && holoName != string.Empty)
            {
                HoloKronName = holoName;
            }
            ConfigNode _holoKron = new ConfigNode();
            OrXLog.instance.DebugLog("[OrX Save Config] === SAVING ===");


            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/" ))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/" );

            if (File.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".orx"))
            {
                OrXLog.instance.DebugLog("[OrX Save Config] === " + HoloKronName + "-" + groupName + " Exists ... Checking for tmp file ===");

                if (!File.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp"))
                {
                    OrXLog.instance.DebugLog("[OrX Save Config] === tmp file not found ... generating " + HoloKronName + "-" + groupName + ".tmp ===");

                    File.Copy(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".orx",
                        UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp");
                    yield return new WaitForFixedUpdate();

                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Save Config] === tmp file found ... deleting " + HoloKronName + "-" + groupName + ".tmp ===");

                    File.Delete(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp");
                    yield return new WaitForFixedUpdate();

                    OrXLog.instance.DebugLog("[OrX Save Config] === generating new tmp file " + HoloKronName + "-" + groupName + ".tmp ===");
                    File.Copy(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".orx",
                        UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp");
                    yield return new WaitForFixedUpdate();

                }
            }
            else
            {
                OrXLog.instance.DebugLog("[OrX Save Config] === " + HoloKronName + "-" + groupName + " does not exist ... Checking for tmp file ===");

                if (!File.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp"))
                {
                    OrXLog.instance.DebugLog("[OrX Save Config] === tmp file not found ... generating " + HoloKronName + "-" + groupName + ".tmp ===");
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Save Config] === tmp file found ... deleting " + HoloKronName + "-" + groupName + ".tmp ===");
                    File.Delete(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp");
                    yield return new WaitForFixedUpdate();
                }

            }
            yield return new WaitForFixedUpdate();

            _file = ConfigNode.Load(hConfigLoc2);
            if (_file == null)
            {
                OrXLog.instance.DebugLog("[OrX Save Config] === generating new tmp file " + HoloKronName + "-" + groupName + ".tmp ===");

                _file = new ConfigNode();
                _file.AddValue("name", HoloKronName);
                _file.AddValue("group", groupName);
                _file.AddNode(hkCount.ToString());
                _holoKron = _file.GetNode(hkCount.ToString());
                _holoKron.AddValue("name", HoloKronName);
                _holoKron.AddValue("group", groupName);
                _holoKron.AddValue("count", hkCount);
                _holoKron.AddValue("Kontinuum", connectToKontinuum);
                _holoKron.AddNode("Mods");
                _holoKron.AddNode("OrX");
                _holoKron.AddNode("boids");
                _holoKron.AddNode("data");

                _file.Save(hConfigLoc2);
            }
            else
            {
                if (append)
                {
                    foreach (ConfigNode cn in _file.nodes)
                    {
                        hkCount += 1;
                    }
                    /*
                    int _hkCount = hkCount - 1;
                    ConfigNode _holoNode = _file.GetNode("-" + _hkCount + "-");
                    ConfigNode _orxNode = _holoNode.GetNode("OrX");

                    foreach (ConfigNode cn in _orxNode.nodes)
                    {
                        if (cn.name.Contains("OrXHoloKronCoords"))
                        {
                            cn.SetValue("extras", "True");
                            _file.Save(hConfigLoc2);
                        }
                    }
                    */
                    _holoKron = _file.GetNode(hkCount.ToString());
                    if (_holoKron == null)
                    {
                        _holoKron = _file.AddNode(hkCount.ToString());
                        _holoKron.AddValue("name", HoloKronName);
                        _holoKron.AddValue("group", groupName);
                        _holoKron.AddValue("count", hkCount);
                        _holoKron.AddValue("Kontinuum", connectToKontinuum);
                        _holoKron.AddNode("Mods");
                        _holoKron.AddNode("OrX");
                        _holoKron.AddNode("boids");
                        _holoKron.AddNode("data");
                        _file.Save(hConfigLoc2);
                    }
                    else
                    {
                        OrXLog.instance.DebugLog("[OrX Save Config] === ERROR === HoloKron " + hkCount + " FOUND IN ORX FILE ... CANCELLING CREATION ===");
                        _continue = false;
                    }
                }
            }

            if (_continue)
            {
                _file = ConfigNode.Load(hConfigLoc2);
                _holoKron = _file.GetNode(hkCount.ToString());

                ConfigNode mods = _holoKron.GetNode("Mods");
                List<string>.Enumerator _installedMods = installedMods.GetEnumerator();
                while (_installedMods.MoveNext())
                {
                    if (_installedMods.Current != null)
                    {
                        if (!ModWhitelist.Contains(_installedMods.Current))
                        {
                            mods.SetValue(_installedMods.Current, "Mod", true);
                        }
                    }
                }
                _installedMods.Dispose();

                if (geoCache)
                {
                    if (localSaveRange >= 4000)
                    {
                        localSaveRange = 4000;
                    }
                }
                else
                {
                    localSaveRange = 10000;
                }

                OnScrnMsgUC("Saving " + HoloKronName + " " + hkCount + " ....");
                ConfigNode node = _holoKron.GetNode("OrX");
                ConfigNode HoloKronNode = null;

                if (node.HasNode("OrXHoloKronCoords" + hkCount))
                {
                    OrXLog.instance.DebugLog("[OrX Save Config] === NODE 'OrXHoloKronCoords" + hkCount + "' FOUND ... LOADING ===");
                    HoloKronNode = node.GetNode("OrXHoloKronCoords" + hkCount);

                    if (HoloKronNode == null)
                    {
                        HoloKronNode = node.AddNode("OrXHoloKronCoords" + hkCount);
                        HoloKronNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                        HoloKronNode.AddValue("spawned", "False");
                        HoloKronNode.AddValue("extras", "False");
                        HoloKronNode.AddValue("unlocked", "False");
                        HoloKronNode.AddValue("tech", tech);

                        HoloKronNode.AddValue("missionName", HoloKronName);
                        HoloKronNode.AddValue("missionType", missionType);
                        HoloKronNode.AddValue("challengeType", challengeType);
                        HoloKronNode.AddValue("raceType", raceType);

                        HoloKronNode.AddValue("gold", Gold);
                        HoloKronNode.AddValue("silver", Silver);
                        HoloKronNode.AddValue("bronze", Bronze);

                        HoloKronNode.AddValue("completed", completed);
                        HoloKronNode.AddValue("count", hkCount);

                        HoloKronNode.AddValue("lat", _lat);
                        HoloKronNode.AddValue("lon", _lon);
                        HoloKronNode.AddValue("alt", _alt);

                        HoloKronNode.AddValue("chance", _spawnChance);

                        HoloKronNode.AddValue("missionDescription0", missionDescription0);
                        HoloKronNode.AddValue("missionDescription1", missionDescription1);
                        HoloKronNode.AddValue("missionDescription2", missionDescription2);
                        HoloKronNode.AddValue("missionDescription3", missionDescription3);
                        HoloKronNode.AddValue("missionDescription4", missionDescription4);
                        HoloKronNode.AddValue("missionDescription5", missionDescription5);
                        HoloKronNode.AddValue("missionDescription6", missionDescription6);
                        HoloKronNode.AddValue("missionDescription7", missionDescription7);
                        HoloKronNode.AddValue("missionDescription8", missionDescription8);
                        HoloKronNode.AddValue("missionDescription9", missionDescription9);
                        HoloKronNode.SetValue("Targets", FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + groupName + "," + _lat + "," + _lon + "," + _alt + ","
+ hkCount + "," + missionType + "," + challengeType + ":" + FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + groupName + "," + _lat + "," + _lon + "," + _alt + ","
+ hkCount + "," + missionType + "," + challengeType, true);

                    }
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Save Config] === CREATING " + HoloKronName + " " + hkCount + " ===");

                    HoloKronNode = node.AddNode("OrXHoloKronCoords" + hkCount);
                    HoloKronNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                    HoloKronNode.AddValue("spawned", "False");
                    HoloKronNode.AddValue("extras", "False");
                    HoloKronNode.AddValue("unlocked", "False");
                    HoloKronNode.AddValue("tech", tech);

                    HoloKronNode.AddValue("missionName", HoloKronName);
                    HoloKronNode.AddValue("missionType", missionType);
                    HoloKronNode.AddValue("challengeType", challengeType);
                    HoloKronNode.AddValue("raceType", raceType);

                    HoloKronNode.AddValue("gold", Gold);
                    HoloKronNode.AddValue("silver", Silver);
                    HoloKronNode.AddValue("bronze", Bronze);

                    HoloKronNode.AddValue("completed", completed);
                    HoloKronNode.AddValue("count", hkCount);

                    HoloKronNode.AddValue("lat", _lat);
                    HoloKronNode.AddValue("lon", _lon);
                    HoloKronNode.AddValue("alt", _alt);

                    HoloKronNode.AddValue("chance", _spawnChance);

                    HoloKronNode.AddValue("missionDescription0", missionDescription0);
                    HoloKronNode.AddValue("missionDescription1", missionDescription1);
                    HoloKronNode.AddValue("missionDescription2", missionDescription2);
                    HoloKronNode.AddValue("missionDescription3", missionDescription3);
                    HoloKronNode.AddValue("missionDescription4", missionDescription4);
                    HoloKronNode.AddValue("missionDescription5", missionDescription5);
                    HoloKronNode.AddValue("missionDescription6", missionDescription6);
                    HoloKronNode.AddValue("missionDescription7", missionDescription7);
                    HoloKronNode.AddValue("missionDescription8", missionDescription8);
                    HoloKronNode.AddValue("missionDescription9", missionDescription9);
                    HoloKronNode.SetValue("Targets", FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + groupName + "," + _lat + "," + _lon + "," + _alt + ","
+ hkCount + "," + missionType + "," + challengeType + ":" + FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + groupName + "," + _lat + "," + _lon + "," + _alt + ","
+ hkCount + "," + missionType + "," + challengeType, true);

                }

                if (addingMission)
                {
                    _mission = _holoKron.AddNode("mission" + hkCount);
                    _mission.AddValue(OrXLog.instance.Crypt("pas"), OrXLog.instance.Crypt(Password));
                    OrXLog.instance.DebugLog("[OrX Add Mission] === ADDING NODE 'mission" + hkCount + "' ===");
                    if (CoordDatabase != null)
                    {
                        if (CoordDatabase.Count >= 0)
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === Coord Database Count = " + CoordDatabase.Count);

                            int c = 0;

                            List<string>.Enumerator coordList = CoordDatabase.GetEnumerator();
                            while (coordList.MoveNext())
                            {
                                try
                                {
                                    if (coordList.Current != null)
                                    {
                                        c += 1;

                                        _mission.AddValue("stage" + c, coordList.Current);
                                        OrXLog.instance.DebugLog("[OrX Mission] === stage " + c + " added to " + HoloKronName + " " + hkCount + " ===");
                                    }
                                }
                                catch
                                {

                                }
                            }
                            coordList.Dispose();
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
                    scoreboard0.AddValue("totalTime", "");
                    scoreboard0.AddValue("maxSpeed", "");

                    scoreboard1.AddValue("name", "<empty>");
                    scoreboard1.AddValue("totalTime", "");
                    scoreboard1.AddValue("maxSpeed", "");

                    scoreboard2.AddValue("name", "<empty>");
                    scoreboard2.AddValue("totalTime", "");
                    scoreboard2.AddValue("maxSpeed", "");

                    scoreboard3.AddValue("name", "<empty>");
                    scoreboard3.AddValue("totalTime", "");
                    scoreboard3.AddValue("maxSpeed", "");

                    scoreboard4.AddValue("name", "<empty>");
                    scoreboard4.AddValue("totalTime", "");
                    scoreboard4.AddValue("maxSpeed", "");

                    scoreboard5.AddValue("name", "<empty>");
                    scoreboard5.AddValue("totalTime", "");
                    scoreboard5.AddValue("maxSpeed", "");

                    scoreboard6.AddValue("name", "<empty>");
                    scoreboard6.AddValue("totalTime", "");
                    scoreboard6.AddValue("maxSpeed", "");

                    scoreboard7.AddValue("name", "<empty>");
                    scoreboard7.AddValue("totalTime", "");
                    scoreboard7.AddValue("maxSpeed", "");

                    scoreboard8.AddValue("name", "<empty>");
                    scoreboard8.AddValue("totalTime", "");
                    scoreboard8.AddValue("maxSpeed", "");

                    scoreboard9.AddValue("name", "<empty>");
                    scoreboard9.AddValue("totalTime", "");
                    scoreboard9.AddValue("maxSpeed", "");

                    // ENCRYPTION
                    foreach (ConfigNode.Value cv in _scoreboard_.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in _scoreboard_.nodes)
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

                    if (blueprintsAdded)
                    {
                        ConfigNode addedCraft = ConfigNode.Load(blueprintsFile);

                        if (addedCraft != null)
                        {
                            ConfigNode craftData = node.AddNode("HC" + hkCount + "OrXv0");
                            craftData.AddValue("vesselName", craftToAddMission);
                            ConfigNode location = craftData.AddNode("coords");
                            location.AddValue("holo", hkCount);
                            location.AddValue("pas", Password);
                            //location.AddValue("lat", _HoloKron.latitude);
                            //location.AddValue("lon", _HoloKron.longitude);
                            //location.AddValue("alt", _HoloKron.altitude);
                            //location.AddValue("heading", 0);
                            //location.AddValue("pitch", 0);
                            //location.AddValue("rot", "null");
                            //location.AddValue("pos", "null");

                            foreach (ConfigNode.Value cv in location.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                cv.name = cvEncryptedName;
                                cv.value = cvEncryptedValue;
                            }

                            ConfigNode craftFile = craftData.AddNode("craft");
                            OnScrnMsgUC("<color=#cfc100ff><b>Saving to " + HoloKronName + "</b></color>");
                            OrXLog.instance.DebugLog("[OrX Save Config] Saving: " + craftToAddMission);
                            addedCraft.CopyTo(craftFile);

                            ConfigNode _modules = _holoKron.GetNode("Mods");
                            if (_modules == null)
                            {
                                _modules = new ConfigNode();
                                _modules = _holoKron.AddNode("Mods");
                            }
                            _modules = _holoKron.GetNode("Mods");
                            /*
                            foreach (ConfigNode partCheck in craftFile.nodes)
                            {
                                if (partCheck.name == "PART")
                                {
                                    foreach (ConfigNode partCheck2 in partCheck.nodes)
                                    {
                                        if (partCheck2.name == "MODULE")
                                        {
                                            foreach (ConfigNode.Value partCheck3 in partCheck2.values)
                                            {
                                                if (partCheck3.name == "name")
                                                {
                                                    _modules.SetValue(partCheck3.value, "PartModule", true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            */
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
                            _file.Save(hConfigLoc2);
                            OrXLog.instance.DebugLog("[OrX Save Config] " + craftToAddMission + " Saved to " + HoloKronName);
                            OnScrnMsgUC("<color=#cfc100ff><b>" + craftToAddMission + " Saved</b></color>");
                            blueprintsFile = "";
                            craftToAddMission = "";
                            blueprintsAdded = false;
                        }
                    }

                    if (!dakarRacing)
                    {
                        if (!addCoords && !LBC)
                        {
                            OrXLog.instance.DebugLog("[OrX Add Mission] === SAVING LOCAL VESELS ===");

                            int count = 0;
                            bool isKerb = false;

                            List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
                            while (v.MoveNext())
                            {
                                if (v.Current == null) continue;
                                if (v.Current.packed || !v.Current.loaded) continue;
                                if (v.Current == triggerVessel) continue;

                                if (v.Current.rootPart.Modules.Contains<KerbalEVA>())
                                {
                                    isKerb = true;
                                }

                                if (!v.Current.rootPart.Modules.Contains<ModuleOrXMission>() && !v.Current.isActiveVessel && !isKerb)
                                {
                                    CheckAndSave(node, ref count, ref v);
                                }
                            }
                            v.Dispose();
                        }
                    }

                    _file.Save(hConfigLoc2);
                    OrXLog.instance.DebugLog("[OrX Add Mission] === " + HoloKronName + " Saved ===");

                    if (!addCoords)
                    {
                        OrXLog.instance.building = false;
                        OrXLog.instance.ResetFocusKeys();
                        OrXSpawnHoloKron.instance.stageCount = 0;
                        addingMission = false;
                        GuiEnabledOrXMissions = false;
                        OrXHCGUIEnabled = false;
                        saveLocalVessels = false;
                        building = false;
                        buildingMission = false;
                        addCoords = false;
                        PlayOrXMission = false;
                        challengeRunning = false;
                        StartCoroutine(EndSave());
                    }
                    else
                    {
                        _lat = FlightGlobals.ActiveVessel.latitude;
                        _lon = FlightGlobals.ActiveVessel.longitude;
                        _alt = FlightGlobals.ActiveVessel.altitude + 2;
                        if (dakarRacing)
                        {
                            CoordDatabase = new List<string>();
                        }
                        getNextCoord = true;
                        showTargets = false;
                        movingCraft = true;
                        addCoords = true;
                        saveLocalVessels = false;
                        PlayOrXMission = false;
                        GuiEnabledOrXMissions = true;
                        OrXHCGUIEnabled = true;
                        addingMission = false;
                    }
                }
                else
                {
                    /*
                    hkCount = 0;

                    foreach (ConfigNode cn in node.nodes)
                    {
                        if (cn.name.Contains("OrXHoloKronCoords" + hkCount))
                        {
                            OrXLog.instance.DebugLog("[OrX Save Config] === HoloKron " + hkCount + " FOUND ===");
                            cn.SetValue("extras", "True");
                            hkCount += 1;
                        }
                    }
                    */

                    OrXLog.instance.DebugLog("[OrX Save Config] === ADDING NODE 'HoloKron" + hkCount + "' ===");
                    node.AddNode("HoloKron" + hkCount);
                    ConfigNode HCnode = node.GetNode("HoloKron" + hkCount);

                    Vessel toSave = FlightGlobals.ActiveVessel;
                    ShipConstruct ConstructToSave = new ShipConstruct(toSave.vesselName, toSave.vesselName, toSave.parts[0]);
                    ConfigNode craftConstruct = new ConfigNode("craft");
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

                    OrXLog.instance.DebugLog("[OrX Save Config] Saving: " + toSave.vesselName);
                    OnScrnMsgUC("<color=#cfc100ff><b>Saving: " + toSave.vesselName + "</b></color>");

                    // ADD ENCRYPTION

                    foreach (ConfigNode.Value cv in craftConstruct.values)
                    {
                        if (cv.name == "ship")
                        {
                            cv.value = toSave.vesselName;
                        }

                        string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in craftConstruct.nodes)
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
                                if (cv2.name != "(" && cv2.name != ")")
                                {
                                    string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                                    string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                                    cv2.name = cvEncryptedName;
                                    cv2.value = cvEncryptedValue;
                                }
                            }
                        }
                    }

                    OrXLog.instance.DebugLog("[OrX Save Config] === " + toSave.vesselName + " ENCRYPTED ===");
                    saveShip = false;
                    OnScrnMsgUC("<color=#cfc100ff><b>" + toSave.vesselName + " Saved</b></color>");
                    craftConstruct.CopyTo(HCnode);

                    ConfigNode holoFileLoc = ConfigNode.Load(holoToAdd);
                    ConfigNode addedCraft = new ConfigNode();
                    OrXLog.instance.DebugLog("[OrX Save Config] === HOLO CRAFT ENCRYPTED ===");

                    if (blueprintsAdded)
                    {
                        addedCraft = ConfigNode.Load(blueprintsFile);

                        if (addedCraft != null)
                        {
                            EncryptCraft(hConfigLoc2, craftToAddMission, node, addedCraft, "OrXv0", null);
                            OrXLog.instance.DebugLog("[OrX Save Config] " + craftToAddMission + " Saved to " + HoloKronName);
                            OnScrnMsgUC("<color=#cfc100ff><b>" + craftToAddMission + " Saved</b></color>");
                            blueprintsFile = "";
                            craftToAddMission = "";
                            blueprintsAdded = false;
                        }
                        blueprintsAdded = false;
                    }

                    if (saveLocalVessels)
                    {
                        OrXLog.instance.DebugLog("[OrX Save Config] === SAVING LOCAL VESELS ===");

                        int count = 0;

                        double _latDiff = 0;
                        double _lonDiff = 0;
                        double _altDiff = 0;

                        List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
                        while (v.MoveNext())
                        {
                            if (v.Current == null) continue;
                            if (v.Current.packed || !v.Current.loaded) continue;
                            if (v.Current == triggerVessel) continue;

                            OrXLog.instance.DebugLog("[OrX Save Config] === CHECKING FOR EVA KERBAL ===");

                            bool isKerb = false;
                            if (v.Current.rootPart.Modules.Contains<KerbalEVA>())
                            {
                                isKerb = true;
                            }

                            if (!v.Current.rootPart.Modules.Contains<ModuleOrXMission>() && !v.Current.isActiveVessel && !isKerb)
                            {
                                CheckAndSave(node, ref count, ref v);
                            }
                        }
                        v.Dispose();
                    }

                    if (_savingAirSup1)
                    {
                        addedCraft = ConfigNode.Load(_airSupFile1);

                        if (addedCraft != null)
                        {
                            EncryptCraft(hConfigLoc2, _airSupName1, node, addedCraft, "ASv1", null);
                            OrXLog.instance.DebugLog("[OrX Save Config] " + _airSupName1 + " Saved to " + HoloKronName);
                            OnScrnMsgUC("<color=#cfc100ff><b>" + _airSupName1 + " Saved</b></color>");
                        }
                        _savingAirSup1 = false;
                    }

                    if (_savingAirSup2)
                    {
                        addedCraft = ConfigNode.Load(_airSupFile2);

                        if (addedCraft != null)
                        {
                            EncryptCraft(hConfigLoc2, _airSupName2, node, addedCraft, "ASv2", null);
                            OrXLog.instance.DebugLog("[OrX Save Config] " + _airSupName2 + " Saved to " + HoloKronName);
                            OnScrnMsgUC("<color=#cfc100ff><b>" + _airSupName2 + " Saved</b></color>");
                        }
                        _savingAirSup2 = false;
                    }

                    if (_savingAirSup3)
                    {
                        addedCraft = ConfigNode.Load(_airSupFile3);

                        if (addedCraft != null)
                        {
                            EncryptCraft(hConfigLoc2, _airSupName3, node, addedCraft, "ASv3", null);
                            OrXLog.instance.DebugLog("[OrX Save Config] " + _airSupName3 + " Saved to " + HoloKronName);
                            OnScrnMsgUC("<color=#cfc100ff><b>" + _airSupName3 + " Saved</b></color>");
                        }
                        _savingAirSup3 = false;
                    }

                    saveLocalVessels = false;

                    _file.Save(hConfigLoc2);
                    OrXLog.instance.DebugLog("[OrX Save Config] " + HoloKronName + " " + hkCount + " Saved");

                    if (!addCoords)
                    {
                        OrXLog.instance.DebugLog("[OrX Save Config] " + HoloKronName + " building complete ... Saving");
                        coordCount = 0;
                        challengeRunning = false;
                        OrXLog.instance.building = false;
                        OrXLog.instance.ResetFocusKeys();
                        addingMission = false;
                        GuiEnabledOrXMissions = true;
                        OrXHCGUIEnabled = false;
                        building = false;
                        buildingMission = false;
                        addCoords = false;
                        PlayOrXMission = false;
                        StartCoroutine(EndSave());
                    }
                    else
                    {
                        OrXLog.instance.DebugLog("[OrX Save Config] Adding to " + HoloKronName + " ..... Current count: " + hkCount);
                        Reach();

                        if (!hkSpawned)
                        {
                            hkSpawned = true;
                            if (bdaChallenge)
                            {
                                
                            }
                            else
                            {
                                spawningStartGate = false;
                                Vector3d vect = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude + 10);
                                OrXSpawnHoloKron.instance.StartSpawn(vect, vect, false, true, true, HoloKronName, missionType);
                            }
                        }
                    }
                }
            }
        }
        private void CheckAndSave(ConfigNode node, ref int count, ref List<Vessel>.Enumerator v)
        {
            OrXLog.instance.DebugLog("[OrX Add Mission - Range Check] Vessel " + v.Current.vesselName + " Identified .......................");

            double _targetDistance = OrXUtilities.instance.GetDistance(triggerVessel.longitude, triggerVessel.latitude, v.Current.longitude, v.Current.latitude, (v.Current.altitude + triggerVessel.altitude) / 2);
            bool _inRange = false;

            OrXLog.instance.DebugLog("[OrX Add Mission] === RANGE: " + _targetDistance);

            if (v.Current.LandedOrSplashed)
            {
                if (bdaChallenge)
                {
                    if (_targetDistance <= 8000)
                    {
                        _inRange = true;
                    }
                }
                else
                {
                    if (_targetDistance <= localSaveRange)
                    {
                        _inRange = true;
                    }

                }
            }
            else
            {
                if (bdaChallenge)
                {
                    if (_targetDistance <= 15000)
                    {
                        _inRange = true;
                    }
                }
                else
                {
                    if (_targetDistance <= 4000)
                    {
                        _inRange = true;
                    }
                }
            }

            if (_inRange)
            {
                count += 1;

                List<Part>.Enumerator parts = v.Current.parts.GetEnumerator();
                while (parts.MoveNext())
                {
                    if (parts.Current != null)
                    {

                        if (parts.Current.Modules.Contains<KerbalEVA>())
                        {
                            parts.Current.Die();
                            //yield return new WaitForFixedUpdate();
                        }
                    }
                }
                parts.Dispose();
                //yield return new WaitForFixedUpdate();
                Vessel toSave = v.Current;
                string shipDescription = "";
                OrXLog.instance.DebugLog("[OrX Add Mission] Saving " + v.Current.vesselName + "'s orientation .......................");

                UpVect = (toSave.transform.position - toSave.mainBody.position).normalized;
                EastVect = toSave.mainBody.getRFrmVel(toSave.CoM).normalized;
                NorthVect = Vector3.Cross(EastVect, UpVect).normalized;

                float _pitch = Vector3.Angle(toSave.ReferenceTransform.forward, UpVect);
                float _left = Vector3.Angle(-toSave.ReferenceTransform.right, NorthVect); // left is 90 degrees behind vessel heading

                if (Math.Sign(Vector3.Dot(-toSave.ReferenceTransform.right, EastVect)) < 0)
                {
                    _left = 360 - _left; // westward headings become angles greater than 180
                }

                // Be sure to subtract 90 degrees from pitch and left as the vessel reference transform is offset 90 degrees
                // from the respective vectors due to reasons

                ShipConstruct ConstructToSave = new ShipConstruct(HoloKronName, shipDescription, v.Current.parts[0]);
                ConfigNode craftConstruct = new ConfigNode("craft");
                craftConstruct = ConstructToSave.SaveShip();

                //craftConstruct.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "Vessel" + count + ".proto");

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

                OrXLog.instance.DebugLog("[OrX Add Mission] Saving: " + v.Current.vesselName);
                OnScrnMsgUC("<color=#cfc100ff><b>Saving: " + v.Current.vesselName + "</b></color>");

                ConfigNode craftData = node.AddNode("HC" + hkCount + "OrXv" + count);
                craftData.AddValue("vesselName", v.Current.vesselName);
                ConfigNode location = craftData.AddNode("coords");
                location.AddValue("holo", hkCount);
                location.AddValue("pas", Password);
                location.AddValue("lat", v.Current.latitude);
                location.AddValue("lon", v.Current.longitude);
                location.AddValue("alt", v.Current.altitude + 1);
                location.AddValue("left", _left);
                location.AddValue("pitch", _pitch);
                if (!v.Current.LandedOrSplashed)
                {
                    location.AddValue("airborne", "True");
                }
                if (v.Current.Splashed)
                {
                    location.AddValue("splashed", "True");
                }

                Quaternion or = toSave.rootPart.transform.rotation;
                location.AddValue("rot", or.x + "," + or.y + "," + or.z + "," + or.w);

                foreach (ConfigNode.Value cv in location.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }


                ConfigNode craftFile = craftData.AddNode("craft");
                OnScrnMsgUC("<color=#cfc100ff><b>Saving to " + HoloKronName + " " + hkCount + "</b></color>");
                craftConstruct.CopyTo(craftFile);
                //craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + v.Current.vesselName + "-" + count + ".craft");
                /*
                ConfigNode _modules = _file.GetNode("Modules");
                if (_modules == null)
                {
                    _modules = new ConfigNode();
                    _modules = _file.AddNode("Modules");
                }
                _modules = _file.GetNode("Modules");

                foreach (ConfigNode partCheck in craftConstruct.nodes)
                {
                    if (partCheck.name == "PART")
                    {
                        foreach (ConfigNode partCheck2 in partCheck.nodes)
                        {
                            if (partCheck2.name == "MODULE")
                            {
                                foreach (ConfigNode.Value partCheck3 in partCheck2.values)
                                {
                                    if (partCheck3.name == "name")
                                    {
                                        _modules.SetValue(partCheck3.value, "PartModule", true);
                                    }
                                }
                            }
                        }
                    }
                }
                */
                // ADD ENCRYPTION

                foreach (ConfigNode.Value cv in craftFile.values)
                {
                    if (cv.name == "ship")
                    {
                        cv.value = v.Current.vesselName;
                    }

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
                            if (!cv2.value.Contains("(") && !cv2.value.Contains(")"))
                            {
                                string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                                string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                                cv2.name = cvEncryptedName;
                                cv2.value = cvEncryptedValue;
                            }
                        }
                    }
                }

                OrXLog.instance.DebugLog("[OrX Add Mission] === " + v.Current.vesselName + " ENCRYPTED ===");
                saveShip = false;
                OrXLog.instance.DebugLog("[OrX Add Mission] " + v.Current.vesselName + " Saved to " + HoloKronName);
                OnScrnMsgUC("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
                if (bdaChallenge || outlawRacing)
                {
                    if (toSave != triggerVessel)
                    {
                        toSave.rootPart.AddModule("ModuleOrXJason", true);
                    }
                }
            }
        }
        IEnumerator EndSave()
        {
            if (_HoloKron != null)
            {
                if (_HoloKron.parts.Count == 1)
                {
                    _HoloKron.rootPart.explosionPotential *= 0.2f;
                    _HoloKron.rootPart.explode();

                }
            }
            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".orx");
            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Import/" + HoloKronName + "-" + groupName + ".orx");
            File.Delete(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".tmp");

            yield return new WaitForSeconds(2f);
            FlightGlobals.ForceSetActiveVessel(triggerVessel);
            ResetData();
            challengeRunning = false;
            building = false;
            buildingMission = false;
            OrXHCGUIEnabled = false;
            localSaveRange = 1000;
            MainMenu();
        }

        private void EncryptCraft(string _holoFile, string _name, ConfigNode node, ConfigNode addedCraft, string _suffix, Vessel _vessel)
        {
            ConfigNode craftData = node.AddNode("HC" + hkCount + _suffix);
            craftData.AddValue("vesselName", _name);

            ConfigNode location = craftData.AddNode("coords");
            location.AddValue("holo", hkCount);
            location.AddValue("pas", Password);
            if (_vessel != null)
            {
                location.AddValue("lat", _vessel.latitude);
                location.AddValue("lon", _vessel.longitude);
                location.AddValue("alt", _vessel.altitude + 1);
                location.AddValue("left", Vector3.Angle(_vessel.ReferenceTransform.forward, UpVect));
                location.AddValue("pitch", Vector3.Angle(-_vessel.ReferenceTransform.right, NorthVect));
                if (!_vessel.LandedOrSplashed)
                {
                    location.AddValue("airborne", "True");
                }
                else
                {
                    if (_vessel.Splashed)
                    {
                        location.AddValue("splashed", "True");
                    }

                }
            }

            foreach (ConfigNode.Value cv in location.values)
            {
                string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                cv.name = cvEncryptedName;
                cv.value = cvEncryptedValue;
            }

            ConfigNode craftFile = craftData.AddNode("craft");
            OnScrnMsgUC("<color=#cfc100ff><b>Saving to " + HoloKronName + "</b></color>");
            OrXLog.instance.DebugLog("[OrX Save Config] Saving: " + _name);
            addedCraft.CopyTo(craftFile);

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
            _file.Save(_holoFile);
        }
        IEnumerator FocusSwitchDelay(Vessel v)
        {
            yield return new WaitForSeconds(3);
            FlightGlobals.ForceSetActiveVessel(v);
            OrXLog.instance.ResetFocusKeys();
            SpawnMenu();
        }

        #endregion

        #region Play Challenge

        public void ScanForHoloKron()
        {
            Reach();
            triggerVessel = FlightGlobals.ActiveVessel;
            ScanMenu();

            OrXTargetDistance.instance.TargetDistance(true, true, false, true, HoloKronName, _challengeStartLoc);
        }

        public void OpenHoloKron(bool _geoCache, string holoName, int _hkCount_, Vessel holoKron, Vessel player)
        {
            StartCoroutine(OpenHoloKronRoutine(_geoCache, holoName, _hkCount_, holoKron, player));
        }
        IEnumerator OpenHoloKronRoutine(bool _geoCache, string holoName, int _hkCount_, Vessel holoKron, Vessel player)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                triggerVessel = player;
                _HoloKron = holoKron;
            }
            challengeRunning = false;
            hkCount = _hkCount_;
            geoCache = _geoCache;
            OrXLog.instance.DebugLog("[OrX Open HoloKron] === OPENING HOLOKRON === ");
            yield return new WaitForFixedUpdate();
            building = false;
            coordCount = 0;
            _scoreboard = new List<string>();
            stageTimes = new List<string>();
            crafttosave = string.Empty;

            OrXLoadedFileList = new List<string>();

            int c = 0;

            string _extras = "extras";
            string _completed = "completed";
            string _missionName = "missionName";
            string _missionType = "missionType";
            string _challengeType = "challengeType";
            string _raceType = "raceType";

            string _tech = "tech";
            string _hkCount = "hkCount";
            string _spawned = "spawned";
            string _Gold = "Gold";
            string _Silver = "Silver";
            string _Bronze = "Bronze";

            string _false = "False";
            string _true = "True";

            string _missionDescription0 = "missionDescription0";
            string _missionDescription1 = "missionDescription1";
            string _missionDescription2 = "missionDescription2";
            string _missionDescription3 = "missionDescription3";
            string _missionDescription4 = "missionDescription4";
            string _missionDescription5 = "missionDescription5";
            string _missionDescription6 = "missionDescription6";
            string _missionDescription7 = "missionDescription7";
            string _missionDescription8 = "missionDescription8";
            string _missionDescription9 = "missionDescription9";

            yield return new WaitForFixedUpdate();

            if (holoName != "")
            {
                Debug.Log("[OrX Open HoloKron] === SEEKING " + holoName);

                string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
                List<string> files = new List<string>(Directory.GetFiles(importLoc, "*.holo", SearchOption.AllDirectories));
                if (files != null)
                {
                    List<string>.Enumerator orxFile = files.GetEnumerator();
                    while (orxFile.MoveNext())
                    {
                        if (orxFile.Current != null)
                        {
                            yield return new WaitForFixedUpdate();

                            if (orxFile.Current.Contains(holoName + "-"))
                            {
                                _file = ConfigNode.Load(orxFile.Current);

                                OrXLoadedFileList.Add(orxFile.Current);
                                if (_file != null)
                                {
                                    Debug.Log("[OrX Open HoloKron] === LOADING: " + orxFile.Current);

                                    ConfigNode node = _file.GetNode("OrX");
                                    _currentOrXFile = orxFile.Current;

                                    if (_file.GetValue("Kontinuum") == "True")
                                    {
                                        _KontinuumConnect = true;
                                    }

                                    HoloKronName = _file.GetValue("name");
                                    groupName = _file.GetValue("group");
                                    int _vesselCount = 1;
                                    OrXSpawnHoloKron.instance._airSupportList = new List<string>();
                                    OrXSpawnHoloKron.instance._groundSupportList = new List<string>();
                                    OrXSpawnHoloKron.instance._seaSupportList = new List<string>();
                                    OrXSpawnHoloKron.instance._interceptorList = new List<string>();
                                    OrXSpawnHoloKron.instance._interceptorNameList = new List<string>();
                                    _opForCount = 0;
                                    OrXSpawnHoloKron.instance._enemyCount = 0;
                                    OrXSpawnHoloKron.instance._airSupportCount = 0;
                                    OrXSpawnHoloKron.instance._groundSupportCount = 0;
                                    OrXSpawnHoloKron.instance._seaSupportCount = 0;
                                    OrXSpawnHoloKron.instance._interceptorCount = 0;

                                    foreach (ConfigNode spawnCheck in node.nodes)
                                    {
                                        if (spawnCheck.name.Contains("OrXHoloKronCoords"))
                                        {
                                            bool loadData = true;

                                            foreach (ConfigNode.Value data in spawnCheck.values)
                                            {
                                                string dataName = data.name;

                                                if (data.name == _completed)
                                                {
                                                    if (data.value == _false)
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Open HoloKron] === HoloKron " + holoName + " has not been completed ... ");
                                                        loadData = true;
                                                    }
                                                    else
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Open HoloKron] === HoloKron " + holoName + " has been completed ... CHECKING FOR EXTRAS");

                                                        if (spawnCheck.HasValue(_extras))
                                                        {
                                                            var t = spawnCheck.GetValue(_extras);
                                                            if (t == _false)
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Open HoloKron] === HoloKron " + holoName + " has no extras ... END TRANSMISSION");
                                                                loadData = true;
                                                                //hkCount += 1;
                                                            }
                                                            else
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Open HoloKron] === HoloKron " + holoName + " has extras ... SEARCHING");
                                                                //hkCount += 1;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (loadData)
                                                {
                                                    if (data.name == _missionName)
                                                    {
                                                        missionName = data.value;
                                                    }

                                                    if (data.name == _missionType)
                                                    {
                                                        missionType = data.value;

                                                        if (missionType != "GEO-CACHE")
                                                        {
                                                            Debug.Log("[OrX Open HoloKron] === CHALLENGE DETECTED IN " + holoName + " ===");

                                                            geoCache = false;
                                                            showScores = true;
                                                        }
                                                        else
                                                        {
                                                            Debug.Log("[OrX Open HoloKron] === " + holoName + " IS A GEO-CACHE ===");

                                                            geoCache = true;
                                                            showScores = false;
                                                        }
                                                    }

                                                    if (data.name == _challengeType)
                                                    {
                                                        challengeType = data.value;
                                                        if (challengeType == "BD ARMORY")
                                                        {
                                                            Debug.Log("[OrX Open HoloKron] === BD ARMORY CHALLENGE DETECTED IN " + holoName + " ===");
                                                            bdaChallenge = true;
                                                        }
                                                        else
                                                        {
                                                            if (challengeType == "W[ind/S]")
                                                            {
                                                                Debug.Log("[OrX Open HoloKron] === SHORT TRACK CHALLENGE DETECTED IN " + holoName + " ===");
                                                                windRacing = true;
                                                            }
                                                            else
                                                            {
                                                                if (challengeType == "OUTLAW RACING")
                                                                {
                                                                    Debug.Log("[OrX Open HoloKron] === OUTLAW RACING CHALLENGE DETECTED IN " + holoName + " ===");
                                                                    outlawRacing = true;
                                                                }
                                                                else
                                                                {
                                                                    if (challengeType == "LBC")
                                                                    {
                                                                        Debug.Log("[OrX Open HoloKron] === LBC CHALLENGE DETECTED IN " + holoName + " ===");
                                                                        LBC = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (challengeType == "SCUBA KERB")
                                                                        {
                                                                            Debug.Log("[OrX Open HoloKron] === SCUBA CHALLENGE DETECTED IN " + holoName + " ===");
                                                                            Scuba = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (data.name == _raceType)
                                                    {
                                                        raceType = data.value;
                                                        if (raceType == "SHORT TRACK")
                                                        {
                                                            shortTrackRacing = true;
                                                        }
                                                        if (raceType == "DAKAR")
                                                        {
                                                            dakarRacing = true;
                                                        }
                                                    }

                                                    if (data.name == _missionDescription0)
                                                    {
                                                        missionDescription0 = data.value;
                                                    }

                                                    if (data.name == _missionDescription1)
                                                    {
                                                        missionDescription1 = data.value;
                                                    }

                                                    if (data.name == _missionDescription2)
                                                    {
                                                        missionDescription2 = data.value;
                                                    }

                                                    if (data.name == _missionDescription3)
                                                    {
                                                        missionDescription3 = data.value;
                                                    }

                                                    if (data.name == _missionDescription4)
                                                    {
                                                        missionDescription4 = data.value;
                                                    }
                                                    if (data.name == _missionDescription5)
                                                    {
                                                        missionDescription5 = data.value;
                                                    }
                                                    if (data.name == _missionDescription6)
                                                    {
                                                        missionDescription6 = data.value;
                                                    }
                                                    if (data.name == _missionDescription7)
                                                    {
                                                        missionDescription7 = data.value;
                                                    }

                                                    if (data.name == _missionDescription8)
                                                    {
                                                        missionDescription8 = data.value;
                                                    }
                                                    if (data.name == _missionDescription9)
                                                    {
                                                        missionDescription9 = data.value;
                                                    }

                                                    if (data.name == _Gold)
                                                    {
                                                        Gold = data.value;
                                                    }
                                                    if (data.name == _Silver)
                                                    {
                                                        Silver = data.value;
                                                    }
                                                    if (data.name == _Bronze)
                                                    {
                                                        Bronze = data.value;
                                                    }
                                                }
                                                /*
                                                if (spawnCheck.GetValue("missionType") != "GEO-CACHE")
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Open HoloKron] === HOLOKRON TYPE IS CHALLENGE ===");

                                                    OrXLog.instance.mission = true;
                                                    geoCache = false;
                                                    showScores = true;
                                                }
                                                else
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Open HoloKron] === HOLOKRON TYPE IS GEO-CACHE ===");

                                                    geoCache = true;
                                                    showScores = false;
                                                }
                                                */
                                            }
                                        }

                                        if (spawnCheck.name.Contains("HC" + hkCount + "OrXv0"))
                                        {
                                            OrXLog.instance.DebugLog("[OrX Open HoloKron] === GRABBING CRAFT FILE FOR " + spawnCheck.name + " ===");

                                            foreach (ConfigNode.Value cv in spawnCheck.values)
                                            {
                                                if (cv.name == "vesselName")
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Open HoloKron] === Blueprints found for '" + cv.value + "' ===");
                                                    blueprintsAdded = true;
                                                    crafttosave = cv.value;
                                                }
                                            }

                                            
                                            OrXLog.instance.DebugLog("[OrX Open HoloKron] === GRABBING COORDS ===");

                                            ConfigNode location = spawnCheck.GetNode("coords");
                                            foreach (ConfigNode.Value loc in location.values)
                                            {
                                                string locEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                                if (locEncryptedName == "holo")
                                                {
                                                    string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);

                                                    if (locEncryptedValue == hkCount.ToString())
                                                    {
                                                        foreach (ConfigNode.Value _loc in location.values)
                                                        {
                                                            if (_loc.name == "psa")
                                                            {
                                                                pas = _loc.value;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            
                                            OrXLog.instance.DebugLog("[OrX Open HoloKron] === GRABBING CRAFT FILE DATA ===");

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

                                            OrXLog.instance.DebugLog("[OrX Open HoloKron] === DECRYPTING CRAFT FILE DATA ===");

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
                                                + "/Ships/" + _type + "/" + crafttosave + ".craft";

                                            OrXLog.instance.DebugLog("[OrX Open HoloKron] === BLUEPRINTS READY ===");

                                            ConfigNode HoloKronNode = node.GetNode("OrXHoloKronCoords" + hkCount);
                                            foreach (ConfigNode.Value cv in HoloKronNode.values)
                                            {
                                                if (cv.name == _tech)
                                                {
                                                    if (cv.value != "")
                                                    {
                                                        techToAdd = cv.value;

                                                        if (OrXLog.instance.CheckTechList(techToAdd))
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Open HoloKron] " + holoName + " is adding " + techToAdd + " to the tech list ...");
                                                            OrXLog.instance.AddTech(techToAdd);
                                                        }
                                                        else
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Open HoloKron] " + techToAdd + " is already in the tech list ...");
                                                        }
                                                    }
                                                }
                                                if (cv.name == _spawned)
                                                {
                                                    cv.value = _true;
                                                }
                                            }
                                        }

                                        if (spawnCheck.name.Contains("HC" + _hkCount + "ASv"))
                                        {
                                            OrXTargetDistance.instance._randomSpawned = false;
                                            OrXLog.instance.DebugLog("[OrX Process HoloKron] === GRABBING CRAFT FILE FOR " + spawnCheck.name + " ===");
                                            OrXLog.instance.DebugLog("[OrX Process HoloKron] === DECRYPTING CRAFT FILE DATA FOR " + spawnCheck.name + " ===");
                                            ConfigNode craftFile = spawnCheck.GetNode("craft");
                                            OrXSpawnHoloKron.instance._interceptorNameList.Add(craftFile.GetValue("ship"));
                                            OrXLog.instance.DebugLog("[OrX Process HoloKron] === VESSEL IS INTERCEPTOR ===");
                                            OrXSpawnHoloKron.instance._interceptorCount += 1;
                                            OrXSpawnHoloKron.instance._interceptorList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/interceptor" + OrXSpawnHoloKron.instance._interceptorCount + ".tmp");
                                            craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/interceptor" + OrXSpawnHoloKron.instance._interceptorCount + ".tmp");
                                        }

                                        if (spawnCheck.name.Contains("HC" + _hkCount + "OrXv" + _vesselCount))
                                        {
                                            OrXLog.instance.DebugLog("[OrX Process HoloKron] === GRABBING CRAFT FILE FOR " + spawnCheck.name + " ===");
                                            OrXSpawnHoloKron.instance._enemyCount += 1;
                                            _opForCount += 1;
                                            _vesselCount += 1;
                                            bool _airborne = false;
                                            bool _splashed = false;

                                            ConfigNode location = spawnCheck.GetNode("coords");

                                            foreach (ConfigNode.Value loc in location.values)
                                            {
                                                string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                                string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                                loc.name = cvEncryptedName;
                                                loc.value = cvEncryptedValue;
                                                if (loc.name == "airborne")
                                                {
                                                    if (loc.value == "True")
                                                    {
                                                        _airborne = true;
                                                    }
                                                }
                                                if (loc.name == "splashed")
                                                {
                                                    if (loc.value == "True")
                                                    {
                                                        _splashed = true;
                                                    }
                                                }
                                                if (loc.name == "pas")
                                                {
                                                    pas = loc.value;
                                                }
                                            }

                                            OrXLog.instance.DebugLog("[OrX Process HoloKron] === DECRYPTING CRAFT FILE DATA FOR " + spawnCheck.name + " ===");
                                            ConfigNode craftFile = spawnCheck.GetNode("craft");

                                            if (_airborne)
                                            {
                                                OrXLog.instance.DebugLog("[OrX Process HoloKron] === VESSEL IS AIRBORNE ===");

                                                OrXSpawnHoloKron.instance._airSupportCount += 1;
                                                OrXSpawnHoloKron.instance._airSupportList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/airSupport" + OrXSpawnHoloKron.instance._airSupportCount + ".tmp");
                                                craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/airSupport" + OrXSpawnHoloKron.instance._airSupportCount + ".tmp");
                                            }
                                            else
                                            {
                                                if (_splashed)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Process HoloKron] === VESSEL IS SPLASHED ===");

                                                    OrXSpawnHoloKron.instance._seaSupportCount += 1;
                                                    OrXSpawnHoloKron.instance._seaSupportList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/seaSupport" + OrXSpawnHoloKron.instance._seaSupportCount + ".tmp");
                                                    craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/seaSupport" + OrXSpawnHoloKron.instance._seaSupportCount + ".tmp");

                                                }
                                                else
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Process HoloKron] === VESSEL IS LANDED ===");

                                                    OrXSpawnHoloKron.instance._groundSupportCount += 1;
                                                    OrXSpawnHoloKron.instance._groundSupportList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/groundSupport" + OrXSpawnHoloKron.instance._groundSupportCount + ".tmp");
                                                    craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/groundSupport" + OrXSpawnHoloKron.instance._groundSupportCount + ".tmp");

                                                }
                                            }
                                        }

                                    }

                                    OrXLog.instance.DebugLog("[OrX Open HoloKron] === BLUEPRINTS PROCESSED ===");

                                    _mission = _file.GetNode("mission" + hkCount);
                                    if (_mission != null)
                                    {
                                        OrXLog.instance.DebugLog("[OrX Open HoloKron] === CHALLENGE FOUND ===");
                                        CoordDatabase = new List<string>();

                                        foreach (ConfigNode.Value cv in _mission.values)
                                        {
                                            if (cv.name.Contains("stage"))
                                            {
                                                CoordDatabase.Add(cv.value);
                                                coordCount += 1;
                                            }

                                            if (cv.name.Contains("psa"))
                                            {
                                                pas = cv.value;
                                            }
                                        }

                                        OrXLog.instance.DebugLog("[OrX Open HoloKron] === FOUND " + coordCount + " COORDS IN CHALLENGE ===");

                                        if (CoordDatabase.Count >= 0 && holoOpen)
                                        {
                                            List<string>.Enumerator firstCoords = CoordDatabase.GetEnumerator();
                                            while (firstCoords.MoveNext())
                                            {
                                                try
                                                {
                                                    string[] data = firstCoords.Current.Split(new char[] { ',' });
                                                    if (data[0] == "1")
                                                    {
                                                        gpsCount = 0;
                                                        latMission = double.Parse(data[1]);
                                                        lonMission = double.Parse(data[2]);
                                                        altMission = double.Parse(data[3]);
                                                        nextLocation = new Vector3d(latMission, lonMission, altMission);
                                                        _challengeStartLoc = new Vector3d(latMission, lonMission, altMission);
                                                    }
                                                }
                                                catch (IndexOutOfRangeException e)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Open HoloKron] HoloKron config file processed ...... ");
                                                }
                                            }
                                            firstCoords.Dispose();
                                        }

                                        OrXLog.instance.DebugLog("[OrX Open HoloKron] === SETTING UP " + holoName + " FOR CHALLENGE ===");
                                    }
                                }
                            }
                        }
                    }
                    orxFile.Dispose();
                }
            }

            if (missionDescription9 != "" && missionDescription9 != string.Empty)
            {
                _missionDescription9_ = true;
                _missionDescription8_ = true;
                _missionDescription7_ = true;
                _missionDescription6_ = true;
                _missionDescription5_ = true;
                _missionDescription4_ = true;
                _missionDescription3_ = true;
                _missionDescription2_ = true;
                _missionDescription1_ = true;
            }
            else
            {
                if (missionDescription8 != "" && missionDescription8 != string.Empty)
                {
                    _missionDescription9_ = false;
                    _missionDescription8_ = true;
                    _missionDescription7_ = true;
                    _missionDescription6_ = true;
                    _missionDescription5_ = true;
                    _missionDescription4_ = true;
                    _missionDescription3_ = true;
                    _missionDescription2_ = true;
                    _missionDescription1_ = true;
                }
                else
                {
                    if (missionDescription7 != "" && missionDescription7 != string.Empty)
                    {
                        _missionDescription9_ = false;
                        _missionDescription8_ = false;
                        _missionDescription7_ = true;
                        _missionDescription6_ = true;
                        _missionDescription5_ = true;
                        _missionDescription4_ = true;
                        _missionDescription3_ = true;
                        _missionDescription2_ = true;
                        _missionDescription1_ = true;
                    }
                    else
                    {
                        if (missionDescription6 != "" && missionDescription6 != string.Empty)
                        {
                            _missionDescription9_ = false;
                            _missionDescription8_ = false;
                            _missionDescription7_ = false;
                            _missionDescription6_ = true;
                            _missionDescription5_ = true;
                            _missionDescription4_ = true;
                            _missionDescription3_ = true;
                            _missionDescription2_ = true;
                            _missionDescription1_ = true;
                        }
                        else
                        {
                            if (missionDescription5 != "" && missionDescription5 != string.Empty)
                            {
                                _missionDescription9_ = false;
                                _missionDescription8_ = false;
                                _missionDescription7_ = false;
                                _missionDescription6_ = false;
                                _missionDescription5_ = true;
                                _missionDescription4_ = true;
                                _missionDescription3_ = true;
                                _missionDescription2_ = true;
                                _missionDescription1_ = true;
                            }
                            else
                            {
                                if (missionDescription4 != "" && missionDescription4 != string.Empty)
                                {
                                    _missionDescription9_ = false;
                                    _missionDescription8_ = false;
                                    _missionDescription7_ = false;
                                    _missionDescription6_ = false;
                                    _missionDescription5_ = false;
                                    _missionDescription4_ = true;
                                    _missionDescription3_ = true;
                                    _missionDescription2_ = true;
                                    _missionDescription1_ = true;
                                }
                                else
                                {
                                    if (missionDescription3 != "" && missionDescription3 != string.Empty)
                                    {
                                        _missionDescription9_ = false;
                                        _missionDescription8_ = false;
                                        _missionDescription7_ = false;
                                        _missionDescription6_ = false;
                                        _missionDescription5_ = false;
                                        _missionDescription4_ = false;
                                        _missionDescription3_ = true;
                                        _missionDescription2_ = true;
                                        _missionDescription1_ = true;
                                    }
                                    else
                                    {
                                        if (missionDescription2 != "" && missionDescription2 != string.Empty)
                                        {
                                            _missionDescription9_ = false;
                                            _missionDescription8_ = false;
                                            _missionDescription7_ = false;
                                            _missionDescription6_ = false;
                                            _missionDescription5_ = false;
                                            _missionDescription4_ = false;
                                            _missionDescription3_ = false;
                                            _missionDescription2_ = true;
                                            _missionDescription1_ = true;
                                        }
                                        else
                                        {
                                            if (missionDescription1 != "" && missionDescription1 != string.Empty)
                                            {
                                                _missionDescription9_ = false;
                                                _missionDescription8_ = false;
                                                _missionDescription7_ = false;
                                                _missionDescription6_ = false;
                                                _missionDescription5_ = false;
                                                _missionDescription4_ = false;
                                                _missionDescription3_ = false;
                                                _missionDescription2_ = false;
                                                _missionDescription1_ = true;
                                            }
                                            else
                                            {
                                                _missionDescription9_ = false;
                                                _missionDescription8_ = false;
                                                _missionDescription7_ = false;
                                                _missionDescription6_ = false;
                                                _missionDescription5_ = false;
                                                _missionDescription4_ = false;
                                                _missionDescription3_ = false;
                                                _missionDescription2_ = false;
                                                _missionDescription1_ = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (_HoloKron != null)
            {
                var mom = _HoloKron.rootPart.FindModuleImplementing<ModuleOrXMission>();
                mom.completed = false;
                mom.missionName = missionName;
                mom.missionType = missionType;
                mom.challengeType = challengeType;
                mom.tech = tech;
                mom.hkCount = hkCount;
                mom.Gold = Gold;
                mom.Silver = Silver;
                mom.Bronze = Bronze;
                mom.blueprintsAdded = blueprintsAdded;
            }

            Debug.Log("[OrX Open HoloKron] === OPENING " + holoName + " GUI WINDOW ===");

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (OrXUtilities.instance.GetDistance(lonMission, latMission, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.latitude, (altMission + FlightGlobals.ActiveVessel.altitude) / 2) <= 25)
                {
                    ScanForHoloKron();
                }
                else
                {
                    OrXHCGUIEnabled = true;
                    showScores = false;
                    GuiEnabledOrXMissions = true;
                    PlayOrXMission = true;
                    movingCraft = false;
                }
            }
            else
            {
                OrXHCGUIEnabled = true;
                showScores = false;
                GuiEnabledOrXMissions = true;
                PlayOrXMission = true;
                movingCraft = false;
            }
        }

        public void BeginChallenge()
        {
            ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            if (playerData == null)
            {
                playerData = new ConfigNode();
            }
            playerData.SetValue("name", challengersName, true);
            playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");

            if (challengeType == "BD ARMORY")
            {
                bdaChallenge = true;
                geoCache = false;
            }

            _returnToSender = false;
            triggerVessel = FlightGlobals.ActiveVessel;
            _scoreSaved = false;

            if (challengeType == "LBC")
            {
                if (!challengeRunning)
                {
                    if (disablePRE)
                    {
                        //OrXPRExtension.PreOff("OrX Kontinuum");
                    }

                    challengeRunning = true;
                    geoCache = false;
                    StartCoroutine(GetNextCoordRoutine());
                }
            }
            else
            {
                if (!FlightGlobals.ActiveVessel.isEVA)
                {
                    if (!challengeRunning)
                    {
                        if (disablePRE)
                        {
                            //OrXPRExtension.PreOff("OrX Kontinuum");
                        }

                        challengeRunning = true;
                        geoCache = false;
                        StartCoroutine(ChallengeStartDelay());
                    }
                }
                else
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Get into a vehicle to start the challenge", 4, ScreenMessageStyle.UPPER_CENTER));
                }
            }
        }
        public void ChallengeStart()
        {
            StartCoroutine(ChallengeStartDelay());
        }
        IEnumerator ChallengeStartDelay()
        {
            Reach();
            _getCenterDist = false;
            Game _thisGame = HighLogic.CurrentGame.Updated();
            _thisGame.startScene = GameScenes.FLIGHT;
            GamePersistence.SaveGame(_thisGame, HoloKronName + " START", HighLogic.SaveFolder, SaveMode.OVERWRITE);
            string screenshot = UrlDir.ApplicationRootPath + "Screenshots/" + HoloKronName + " START.png";
            if (File.Exists(screenshot))
            {
                File.Delete(screenshot);
                yield return new WaitForFixedUpdate();
            }
            worldPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission);
            showTargets = true;
            OrXTargetDistance.instance.TargetDistance(false, true, true, true, HoloKronName, nextLocation);
            gpsCount = 0;
            _time = 0;
            missionTime = 0;
            _timer = 0;
            _timerStageTime = "00:00:00.00";
            _timerTotalTime = "00:00:00.00";
            stageTimes = new List<string>();
            _showTimer = true;
            _rb = triggerVessel.GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            double ta = triggerVessel.altitude;
            triggerVessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
            triggerVessel.IgnoreGForces(240);
            _placingChallenger = true;
            OnScrnMsgUC("PLACING " + triggerVessel.vesselName + " AT START POSITION");
            Debug.Log("[OrX Start Challenge] === PLACING " + triggerVessel.vesselName + " AT START POSITION ===");
            Vector3 _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)triggerVessel.latitude, (double)triggerVessel.longitude, (double)_HoloKron.altitude - (_HoloKron.radarAltitude * 0.25));
            Vector3 UpVect = (triggerVessel.ReferenceTransform.position - triggerVessel.mainBody.position).normalized;
            
            float localAlt = (float)_HoloKron.altitude;
            float dropRate = Mathf.Clamp((localAlt * 2), 0.1f, 200);

            while (triggerVessel.altitude <= _HoloKron.altitude + 5)
            {
                triggerVessel.IgnoreGForces(240);
                triggerVessel.angularVelocity = Vector3.zero;
                triggerVessel.angularMomentum = Vector3.zero;
                triggerVessel.SetWorldVelocity(Vector3.zero);
                triggerVessel.Translate(7 * Time.fixedDeltaTime * UpVect);
                yield return new WaitForFixedUpdate();
            }

            triggerVessel.IgnoreGForces(240);
            triggerVessel.angularVelocity = Vector3.zero;
            triggerVessel.angularMomentum = Vector3.zero;
            triggerVessel.SetWorldVelocity(Vector3.zero);
            Vector3 _holoPos = _HoloKron.mainBody.GetWorldSurfacePosition((double)_HoloKron.latitude, (double)_HoloKron.longitude, (double)_HoloKron.altitude + 5);
            targetLoc = _holoPos;
            triggerVessel.SetPosition(_holoPos);
            yield return new WaitForFixedUpdate();
            if (_HoloKron != null)
            {
                _HoloKron.rootPart.explode();
            }
            Vector3 _goalPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_challengeStartLoc.x, (double)_challengeStartLoc.y, (double)_challengeStartLoc.z);
            _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)triggerVessel.latitude, (double)triggerVessel.longitude, (double)_challengeStartLoc.z);
            Vector3 startPosDirection = (_goalPos - _startPos).normalized;
            Quaternion _fixRot = Quaternion.identity;
            _fixRot = Quaternion.FromToRotation(triggerVessel.ReferenceTransform.up, startPosDirection) * triggerVessel.ReferenceTransform.rotation;
            triggerVessel.SetRotation(_fixRot, true);
            
            float _left = Vector3.Angle(-triggerVessel.ReferenceTransform.right, UpVect) - 90;
           
            _fixRot = Quaternion.AngleAxis(-_left, triggerVessel.ReferenceTransform.up) * triggerVessel.ReferenceTransform.rotation;
            triggerVessel.SetRotation(_fixRot, true);
            triggerVessel.IgnoreGForces(240);
            triggerVessel.angularVelocity = Vector3.zero;
            triggerVessel.angularMomentum = Vector3.zero;
            triggerVessel.SetWorldVelocity(Vector3.zero);

            yield return new WaitForFixedUpdate();
            
            if (Vector3.Angle(-triggerVessel.ReferenceTransform.right, UpVect) >= 90)
            {
                _left = Vector3.Angle(-triggerVessel.ReferenceTransform.right, UpVect) - 90;
                _fixRot = Quaternion.AngleAxis(-_left, triggerVessel.ReferenceTransform.up) * triggerVessel.ReferenceTransform.rotation;
                triggerVessel.SetRotation(_fixRot, true);
                triggerVessel.IgnoreGForces(240);
                triggerVessel.angularVelocity = Vector3.zero;
                triggerVessel.angularMomentum = Vector3.zero;
                triggerVessel.SetWorldVelocity(Vector3.zero);
            }
            else
            {
                if (Vector3.Angle(-triggerVessel.ReferenceTransform.right, UpVect) <= 90)
                {
                    _left = 90 - Vector3.Angle(-triggerVessel.ReferenceTransform.right, UpVect);
                    _fixRot = Quaternion.AngleAxis(_left, triggerVessel.ReferenceTransform.up) * triggerVessel.ReferenceTransform.rotation;
                    triggerVessel.SetRotation(_fixRot, true);
                    triggerVessel.IgnoreGForces(240);
                    triggerVessel.angularVelocity = Vector3.zero;
                    triggerVessel.angularMomentum = Vector3.zero;
                    triggerVessel.SetWorldVelocity(Vector3.zero);
                }
            }
            yield return new WaitForFixedUpdate();
            
            ScreenMessages.PostScreenMessage(new ScreenMessage("APPLYING BRAKES", 3, ScreenMessageStyle.UPPER_CENTER));
            OrXLog.instance.DebugLog("[OrX Place Challenger] === PLACING " + triggerVessel.vesselName + " ===");
            triggerVessel.Landed = false;
            triggerVessel.Splashed = false;
            if (!triggerVessel.isActiveVessel)
            {
                FlightGlobals.ForceSetActiveVessel(triggerVessel);
            }

            while (!triggerVessel.LandedOrSplashed)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("PLACING .....", 0.5f, ScreenMessageStyle.UPPER_CENTER));
                yield return new WaitForFixedUpdate();
                triggerVessel.IgnoreGForces(240);
                triggerVessel.angularVelocity = Vector3.zero;
                triggerVessel.angularMomentum = Vector3.zero;
                triggerVessel.SetWorldVelocity(Vector3.zero);
                triggerVessel.Translate((float)triggerVessel.radarAltitude * Time.fixedDeltaTime * -UpVect);

            }
            OrXLog.instance.ResetFocusKeys();
            OrXLog.instance.DebugLog("[OrX Start Mission Delay] === Starting Delay ===");
            _rb.isKinematic = false;
            StartCoroutine(GetNextCoordRoutine());
        }

        public void CloseGeoCache()
        {
            holoOpen = false;
            challengeRunning = false;
            PlayOrXMission = false;
            showScores = false;
            OrXLog.instance.mission = false;

            if (blueprintsFile != "" || blueprintsFile != string.Empty)
            {
                OnScrnMsgUC("Blueprints Available .....");
                Debug.Log("[OrX Close Geo-Cache] === '" + blueprintsFile + "' Available ===");
                _blueprints_.Save(blueprintsFile);
            }

            _file = ConfigNode.Load(_currentOrXFile);
            ConfigNode node = _file.GetNode("OrX");

            foreach (ConfigNode spawnCheck in node.nodes)
            {
                if (spawnCheck.name.Contains("OrXHoloKronCoords"))
                {
                    ConfigNode HoloKronNode = node.GetNode("OrXHoloKronCoords" + hkCount);

                    if (HoloKronNode != null)
                    {
                        OrXLog.instance.DebugLog("[OrX Close Geo-Cache] === FOUND HoloKron === " + hkCount);

                        if (HoloKronNode.HasValue("completed"))
                        {
                            var t = HoloKronNode.GetValue("completed");
                            if (t == "False")
                            {
                                HoloKronNode.SetValue("completed", "True", true);

                                OrXLog.instance.DebugLog("[OrX Close Geo-Cache] === HoloKron " + hkCount + " was not completed ... CHANGING TO TRUE");
                                break;
                            }
                            else
                            {
                                OrXLog.instance.DebugLog("[OrX Close Geo-Cache] === HoloKron " + hkCount + " is already completed ... ");
                            }
                        }

                        OrXLog.instance.DebugLog("[OrX Start Close Geo-Cache] === DATA PROCESSED ===");
                    }
                }
            }

            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + groupName + "/" + HoloKronName + "-" + hkCount + "-" + groupName + ".holo");
            OrXHCGUIEnabled = false;
            checking = false;
            GuiEnabledOrXMissions = false;
            ResetData();
        }
        public void GetNextCoord()
        {
            StartCoroutine(GetNextCoordRoutine());
        }
        IEnumerator GetNextCoordRoutine()
        {
            if (_placingChallenger)
            {
                _placingChallenger = false;
                challengeRunning = true;
                holoOpen = false;
                ScreenMessages.PostScreenMessage(new ScreenMessage("WAITING FOR VESSEL TO SETTLE", 0.7f, ScreenMessageStyle.UPPER_CENTER));
                yield return new WaitForSeconds(1);
                ScreenMessages.PostScreenMessage(new ScreenMessage("WAITING FOR VESSEL TO SETTLE", 0.7f, ScreenMessageStyle.UPPER_CENTER));
                yield return new WaitForSeconds(1);
                bool _continue = true;
                while (FlightGlobals.ActiveVessel.horizontalSrfSpeed <= 0.15f)
                {
                    if (challengeRunning)
                    {
                        ScreenMessages.PostScreenMessage(new ScreenMessage("TIMER STARTS WHEN YOU START MOVING", 0.5f, ScreenMessageStyle.UPPER_CENTER));
                        yield return new WaitForFixedUpdate();
                        triggerVessel.angularVelocity = Vector3.zero;
                        triggerVessel.angularMomentum = Vector3.zero;
                    }
                    else
                    {
                        _continue = false;
                        yield return new WaitForFixedUpdate();
                    }
                }

                if (_continue)
                {
                    OnScrnMsgUC("Starting challenge ...........");
                    ScreenCapture.CaptureScreenshot(UrlDir.ApplicationRootPath + "Screenshots/" + HoloKronName + " START.png");
                    gpsCount = 0;
                    stageStart = true;
                    _timerStageTime = "";
                    _timerTotalTime = "";
                }
            }

            checkingMission = true;

            if (gpsCount != 0)
            {
                showTargets = false;
            }
            _getCenterDist = false;
            float _stageTime = missionTime - _time;
            if (targetCoord != null)
            {
                targetCoord.rootPart.explode();
                targetCoord = null;
            }

            if (CoordDatabase.Count - gpsCount <= 0)
            {
                if (raceType == "DAKAR RACING")
                {
                    StartCoroutine(FinishWhenStopped(_stageTime));
                }
                else
                {
                    ChallengeFinish(_stageTime);
                }
            }
            else
            {
                if (stageStart)
                {
                    stageStart = false;
                    StartTimer();
                }
                else
                {
                    //missionTime = (float)FlightGlobals.ActiveVessel.missionTime;
                    stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + airTime + ","
                        + _stageTime + "," + CheatOptions.NoCrashDamage + ","
                        + CheatOptions.UnbreakableJoints + "," + CheatOptions.IgnoreMaxTemperature + ","
                        + CheatOptions.InfinitePropellant + "," + CheatOptions.InfiniteElectricity);
                    _timerStageTime = OrXUtilities.instance.TimeSet(_stageTime);
                    _time = missionTime;
                    OnScrnMsgUC("STAGE TIME: " + _timerStageTime);
                    OrXLog.instance.DebugLog("[OrX Get Next Coord] === stage" + gpsCount + " = " + gpsCount + ", " + topSurfaceSpeed + ", " + maxDepth + ", " + airTime + ", " + missionTime + " ===");
                }

                gpsCount += 1;
                maxDepth = 0;
                topSurfaceSpeed = 0;

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
                                latMission = double.Parse(data[1]);
                                lonMission = double.Parse(data[2]);
                                altMission = double.Parse(data[3]);
                                nextLocation = new Vector3d(latMission, lonMission, altMission);
                                worldPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission);
                                showTargets = true;
                                OrXLog.instance.DebugLog("[OrX Get Next Coord] === NEXT LOCATION ACQUIRED ===");
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
                coordSpawn = false;
                scanning = true;
                checking = true;
                spawnHoloKron = false;
                tpoint = nextLocation;

                if (LBC)
                {
                    OrXSpawnHoloKron.instance.SpawnLBC(gpsCount, HoloKronName);
                }

                OrXLog.instance.DebugLog("[OrX Get Next Coord] === CHECKING DISTANCE ===");

                OrXTargetDistance.instance.TargetDistance(false, true, true, true, HoloKronName, nextLocation);
            }
        }

        IEnumerator FinishWhenStopped(float _stageTime)
        {
            yield return new WaitForFixedUpdate();

            if (FlightGlobals.ActiveVessel.horizontalSrfSpeed <= 0.2f)
            {
                ChallengeFinish(_stageTime);
            }
            else
            {
                StartCoroutine(FinishWhenStopped(missionTime - _time));
            }
        }
        private void ChallengeFinish(float _stageTime)
        {
            _timerOn = false;
            OrXLog.instance.mission = false;
            _showTimer = false;

            if (CoordDatabase.Count >= 0)
            {
                stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + airTime + ","
                    + _stageTime + "," + CheatOptions.NoCrashDamage + ","
                    + CheatOptions.UnbreakableJoints + "," + CheatOptions.IgnoreMaxTemperature + ","
                    + CheatOptions.InfinitePropellant + "," + CheatOptions.InfiniteElectricity);
                StartCoroutine(SaveScore());
            }

            _timerStageTime = OrXUtilities.instance.TimeSet(missionTime - _time);
            OnScrnMsgUC("FINAL STAGE TIME: " + _timerStageTime);

            OrXHCGUIEnabled = true;
            GuiEnabledOrXMissions = true;
            challengeRunning = false;
            PlayOrXMission = true;
            showScores = true;

            if (blueprintsFile != "" && blueprintsAdded)
            {
                _blueprints_.Save(blueprintsFile);
                OnScrnMsgUC("Blueprints Available for '" + crafttosave + "'");
                OrXLog.instance.DebugLog("[OrX Target Manager] === '" + crafttosave + "' Blueprints Available ===");
            }
        }

        public void StartTimer()
        {
            _missionStartTime = HighLogic.CurrentGame.UniversalTime;//triggerVessel.missionTime;
            _time = 0;
            missionTime = 0;
            _timerStageTime = "00:00:00.00";
            _timerTotalTime = "00:00:00.00";
            _timerOn = true;
            _timing = true;
            StartCoroutine(StageTimer());
            OrXLog.instance.DebugLog("[OrX Start Timer] === START TIME: " + _missionStartTime + " ===");
        }
        public void StopTimer()
        {
            _timerOn = false;
            _timing = false;
        }
        IEnumerator StageTimer()
        {
            if (_timerOn)
            {
                if (bdaChallenge && FlightGlobals.ActiveVessel == null)
                {
                    OrXVesselLog.instance.CheckPlayerVesselList();
                }
                yield return new WaitForFixedUpdate();

                missionTime += Time.fixedDeltaTime * TimeWarp.CurrentRate;
                _timerTotalTime = OrXUtilities.instance.TimeSet(missionTime);
                StartCoroutine(StageTimer());
            }
            else
            {
                if (bdaChallenge)
                {
                    OrXLog.instance.DebugLog("[OrX Start Timer] === FINISH TIME: " + missionTime + " ===");
                }
            }
        }

        public bool _returnToSender = false;

        public void CancelChallenge()
        {
            OrXLog.instance.DebugLog("[OrX Cancel Challenge] === CANCEL ===");
            OrXLog.instance.mission = false;
            OrXLog.instance.building = false;
            locCount = 0;
            locAdded = false;
            building = false;
            buildingMission = false;
            addCoords = false;
            GuiEnabledOrXMissions = false;
            OrXHCGUIEnabled = false;
            PlayOrXMission = false;
            HoloKronName = "";
            ResetData();
            StopTimer();

            if (FlightGlobals.ActiveVessel != triggerVessel && triggerVessel != null)
            {
                FlightGlobals.ForceSetActiveVessel(triggerVessel);
            }
        }
        public void ReturnToSender(Vector3d _loc)
        {
            Reach();
            _returnToSender = true;
            _timerStageTime = "";
            targetLoc = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_loc.x, (double)_loc.y, (double)_loc.z);
            OrXVesselMove.Instance.StartMove(triggerVessel, false, 20, false, true, _loc); //new Vector3d(_HoloKron.latitude, _HoloKron.longitude, _HoloKron.altitude));
        }
        public void Refuel()
        {

        }

        #endregion

        #region Scoreboard Functions

        public void ExtractScoreboard(string groupName, string holoName, int _hkCount_)
        {
            ConfigNode _sbFile = new ConfigNode();
            ConfigNode _missionNode = _sbFile.AddNode("mission" + _hkCount_);
            ConfigNode _scoreboardNode = _missionNode.AddNode("scoreboard");
            ConfigNode _scoreboard0_ = _scoreboardNode.AddNode("scoreboard0");
            ConfigNode _scoreboard1_ = _scoreboardNode.AddNode("scoreboard1");
            ConfigNode _scoreboard2_ = _scoreboardNode.AddNode("scoreboard2");
            ConfigNode _scoreboard3_ = _scoreboardNode.AddNode("scoreboard3");
            ConfigNode _scoreboard4_ = _scoreboardNode.AddNode("scoreboard4");
            ConfigNode _scoreboard5_ = _scoreboardNode.AddNode("scoreboard5");
            ConfigNode _scoreboard6_ = _scoreboardNode.AddNode("scoreboard6");
            ConfigNode _scoreboard7_ = _scoreboardNode.AddNode("scoreboard7");
            ConfigNode _scoreboard8_ = _scoreboardNode.AddNode("scoreboard8");
            ConfigNode _scoreboard9_ = _scoreboardNode.AddNode("scoreboard9");

            foreach (ConfigNode.Value cv in _mission.values)
            {
                _missionNode.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                _scoreboard0_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                _scoreboard1_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                _scoreboard2_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                _scoreboard3_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                _scoreboard4_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                _scoreboard5_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                _scoreboard6_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                _scoreboard7_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                _scoreboard8_.AddValue(cv.name, cv.value);
            }

            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                _scoreboard9_.AddValue(cv.name, cv.value);
            }

            _sbFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + holoName + "-" + _hkCount_ + "-" + groupName + ".scoreboard");
            OrXLog.instance.DebugLog("[OrX Extract Score Board] === MISSION NODE FOUND ... SAVING === ");
            OnScrnMsgUC("Scoreboard saved to GameData/Orx/Export");
        }
        private void GetStats(string _name, int _entryCount)
        {
            bool bda = false;
            if (challengeType == "BD ARMORY")
            {
                bda = true;
                bdaChallenge = true;
            }
            scoreboardStats = new List<string>();
            _mission = _file.GetNode("mission" + hkCount);

            _scoreboard_ = _mission.GetNode("scoreboard");
            ConfigNode entryCount = _scoreboard_.GetNode("scoreboard" + _entryCount);
            statsName = _name;
            statsTime = "";
            statsMaxSpeed = 0;
            statsTotalAirTime = "";
            statsMaxDepth = 0;

            foreach (ConfigNode.Value cv in entryCount.values)
            {
                string decryptedName = OrXLog.instance.Decrypt(cv.name);
                string decryptedValue = OrXLog.instance.Decrypt(cv.value);

                if (decryptedName == "maxSpeed")
                {
                    double ms = double.Parse(decryptedValue);
                    if (ms >= statsMaxSpeed)
                    {
                        statsMaxSpeed = ms;
                    }
                }
                if (decryptedName == "maxDepth")
                {
                    double md = double.Parse(decryptedValue);
                    if (md <= statsMaxDepth)
                    {
                        statsMaxDepth = md;
                    }

                    statsMaxDepth = Math.Round(double.Parse(decryptedValue), 1);
                }
                if (decryptedName == "totalAirTime")
                {
                    if (bdaChallenge)
                    {
                        statsTotalAirTime = decryptedValue;
                    }
                    else
                    {
                        statsTotalAirTime = OrXUtilities.instance.TimeSet(float.Parse(decryptedValue));
                    }
                }
                if (decryptedName == "totalTime")
                {
                    statsTime = OrXUtilities.instance.TimeSet(float.Parse(decryptedValue));
                }

                if (decryptedName == "mods")
                {
                    statsMods = decryptedValue;
                }

                if (decryptedName.Contains("stage"))
                {
                    scoreboardStats.Add(decryptedValue);
                }
            }
            OrXScoreboardStats.instance.OpenStatsWindow(bda, HoloKronName, groupName, statsName, statsTime, statsTotalAirTime, hkCount, statsMaxSpeed, statsMaxDepth, scoreboardStats, statsMods);

            _extractScoreboard = true;
        }

        public void GetNextScoresFile(bool _addToScoreboard, List<string> _scoresData)
        {
            Reach();
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/scores/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/scores/");

            if (!_addToScoreboard)
            {
                File.Move(currentScoresFile, UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/scores/" + HoloKronName + "-" + hkCount + "-" + statsName + ".scores");
                StartImporting();
            }
            else
            {
                StartCoroutine(Import(currentScoresFile, _scoresData));
            }
        }
        IEnumerator GetScorboardFile()
        {
            yield return new WaitForFixedUpdate();
            _file = ConfigNode.Load(_currentOrXFile);
            if (_file != null)
            {
                _mission = _file.GetNode("mission" + hkCount);
                if (_mission != null)
                {
                    _scoreboard_ = _mission.GetNode("scoreboard");

                    if (_scoreboard_ != null)
                    {
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
                        yield return new WaitForFixedUpdate();
                        string fileToRemove = "";
                        List<string> scoreBoardFiles = new List<string>(Directory.GetFiles(UrlDir.ApplicationRootPath + "GameData/OrX/Import/", "*.scoreboard", SearchOption.AllDirectories));
                        if (scoreBoardFiles != null)
                        {
                            List<string>.Enumerator scoreBoardFile = scoreBoardFiles.GetEnumerator();
                            while (scoreBoardFile.MoveNext())
                            {
                                if (scoreBoardFile.Current != null)
                                {
                                    Debug.Log("[OrX Import Score Board] === SCORE BOARD IMPORT FILES FOUND ===");
                                    string _scoreFileFix = scoreBoardFile.Current.Replace('.', ' ');
                                    if (_scoreFileFix.Contains(HoloKronName + "-" + hkCount + "-" + groupName))
                                    {
                                        Debug.Log("[OrX Import Score Board] === '" + scoreBoardFile.Current + "' MATCHES '" + HoloKronName + "' ===");

                                        ConfigNode _scoreBoard = ConfigNode.Load(scoreBoardFile.Current);
                                        if (_scoreBoard != null)
                                        {
                                            Debug.Log("[OrX Import Score Board] === FILE LOADED === " + scoreBoardFile.Current + " ===");

                                            fileToRemove = scoreBoardFile.Current;
                                            _mission.ClearValues();
                                            scoreboard0.ClearValues();
                                            scoreboard1.ClearValues();
                                            scoreboard2.ClearValues();
                                            scoreboard3.ClearValues();
                                            scoreboard4.ClearValues();
                                            scoreboard5.ClearValues();
                                            scoreboard6.ClearValues();
                                            scoreboard7.ClearValues();
                                            scoreboard8.ClearValues();
                                            scoreboard9.ClearValues();

                                            yield return new WaitForFixedUpdate();
                                            ConfigNode _missionNode = _scoreBoard.GetNode("mission" + hkCount);
                                            ConfigNode _scoreboardNode = _missionNode.GetNode("scoreboard");
                                            ConfigNode _scoreboard0_ = _scoreboardNode.GetNode("scoreboard0");
                                            ConfigNode _scoreboard1_ = _scoreboardNode.GetNode("scoreboard1");
                                            ConfigNode _scoreboard2_ = _scoreboardNode.GetNode("scoreboard2");
                                            ConfigNode _scoreboard3_ = _scoreboardNode.GetNode("scoreboard3");
                                            ConfigNode _scoreboard4_ = _scoreboardNode.GetNode("scoreboard4");
                                            ConfigNode _scoreboard5_ = _scoreboardNode.GetNode("scoreboard5");
                                            ConfigNode _scoreboard6_ = _scoreboardNode.GetNode("scoreboard6");
                                            ConfigNode _scoreboard7_ = _scoreboardNode.GetNode("scoreboard7");
                                            ConfigNode _scoreboard8_ = _scoreboardNode.GetNode("scoreboard8");
                                            ConfigNode _scoreboard9_ = _scoreboardNode.GetNode("scoreboard9");
                                            yield return new WaitForFixedUpdate();

                                            foreach (ConfigNode.Value cv in _missionNode.values)
                                            {
                                                _mission.AddValue(cv.name, cv.value);
                                            }
                                            foreach (ConfigNode.Value cv0 in _scoreboard0_.values)
                                            {
                                                scoreboard0.AddValue(cv0.name, cv0.value);
                                            }
                                            foreach (ConfigNode.Value cv1 in _scoreboard1_.values)
                                            {
                                                scoreboard1.AddValue(cv1.name, cv1.value);
                                            }
                                            foreach (ConfigNode.Value cv2 in _scoreboard2_.values)
                                            {
                                                scoreboard2.AddValue(cv2.name, cv2.value);
                                            }
                                            foreach (ConfigNode.Value cv3 in _scoreboard3_.values)
                                            {
                                                scoreboard3.AddValue(cv3.name, cv3.value);
                                            }
                                            foreach (ConfigNode.Value cv4 in _scoreboard4_.values)
                                            {
                                                scoreboard4.AddValue(cv4.name, cv4.value);
                                            }
                                            foreach (ConfigNode.Value cv5 in _scoreboard5_.values)
                                            {
                                                scoreboard5.AddValue(cv5.name, cv5.value);
                                            }
                                            foreach (ConfigNode.Value cv6 in _scoreboard6_.values)
                                            {
                                                scoreboard6.AddValue(cv6.name, cv6.value);
                                            }
                                            foreach (ConfigNode.Value cv7 in _scoreboard7_.values)
                                            {
                                                scoreboard7.AddValue(cv7.name, cv7.value);
                                            }
                                            foreach (ConfigNode.Value cv8 in _scoreboard8_.values)
                                            {
                                                scoreboard8.AddValue(cv8.name, cv8.value);
                                            }
                                            foreach (ConfigNode.Value cv9 in _scoreboard9_.values)
                                            {
                                                scoreboard9.AddValue(cv9.name, cv9.value);
                                            }
                                            Debug.Log("[OrX Import Score Board] === Score Board Updated ... Saving ===");

                                            yield return new WaitForFixedUpdate();
                                            _file.Save(_currentOrXFile);
                                            if (fileToRemove != "")
                                            {
                                                Debug.Log("[OrX Import Score Board] === Deleting Score Board File ===");
                                                File.Delete(fileToRemove);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Import Score Board] === '" + scoreBoardFile.Current + "' DOES NOT MATCH '" + HoloKronName + "-" + hkCount + "-" + groupName + "' ===");
                                    }
                                }
                            }
                            scoreBoardFile.Dispose();
                        }
                        else
                        {
                            Debug.Log("[OrX Import Score Board] === No Score Board Files Found ===");
                        }
                        yield return new WaitForFixedUpdate();
                        StartCoroutine(GetScoreboardData());
                    }
                }
            }
        }
        IEnumerator GetScoreboardData()
        {
            nameSB0 = string.Empty;
            timeSB0 = string.Empty;
            saltSB0 = string.Empty;

            nameSB1 = string.Empty;
            timeSB1 = string.Empty;
            saltSB1 = string.Empty;

            nameSB2 = string.Empty;
            timeSB2 = string.Empty;
            saltSB2 = string.Empty;

            nameSB3 = string.Empty;
            timeSB3 = string.Empty;
            saltSB3 = string.Empty;

            nameSB4 = string.Empty;
            timeSB4 = string.Empty;
            saltSB4 = string.Empty;

            nameSB5 = string.Empty;
            timeSB5 = string.Empty;
            saltSB5 = string.Empty;

            nameSB6 = string.Empty;
            timeSB6 = string.Empty;
            saltSB6 = string.Empty;

            nameSB7 = string.Empty;
            timeSB7 = string.Empty;
            saltSB7 = string.Empty;

            nameSB8 = string.Empty;
            timeSB8 = string.Empty;
            saltSB8 = string.Empty;

            nameSB9 = string.Empty;
            timeSB9 = string.Empty;
            saltSB9 = string.Empty;

            if (challengeType == "BD ARMORY")
            {
                bdaChallenge = true;
            }

            _file = ConfigNode.Load(_currentOrXFile);
            yield return new WaitForFixedUpdate();
            _mission = _file.GetNode("mission" + hkCount);
            if (_mission == null)
            {
                _file.AddNode("mission" + hkCount);
                _mission = _file.GetNode("mission" + hkCount);
                _mission.AddValue("psa", OrXLog.instance.Crypt(Password));
            }
            pas = _mission.GetValue("psa");
            _scoreboard_ = _mission.GetNode("scoreboard");
            bool _save = false;

            if (_scoreboard_ == null)  // ADD NEW PODIUM LIST
            {
                OrXLog.instance.DebugLog("[OrX Get Scoreboard Data] === SCOREBOARD NOT FOUND ... GENERATING ===");

                _scoreboard_ = _mission.AddNode("scoreboard");

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
                scoreboard0.AddValue("totalTime", "");
                scoreboard0.AddValue("maxSpeed", "");

                scoreboard1.AddValue("name", "<empty>");
                scoreboard1.AddValue("totalTime", "");
                scoreboard1.AddValue("maxSpeed", "");

                scoreboard2.AddValue("name", "<empty>");
                scoreboard2.AddValue("totalTime", "");
                scoreboard2.AddValue("maxSpeed", "");

                scoreboard3.AddValue("name", "<empty>");
                scoreboard3.AddValue("totalTime", "");
                scoreboard3.AddValue("maxSpeed", "");

                scoreboard4.AddValue("name", "<empty>");
                scoreboard4.AddValue("totalTime", "");
                scoreboard4.AddValue("maxSpeed", "");

                scoreboard5.AddValue("name", "<empty>");
                scoreboard5.AddValue("totalTime", "");
                scoreboard5.AddValue("maxSpeed", "");

                scoreboard6.AddValue("name", "<empty>");
                scoreboard6.AddValue("totalTime", "");
                scoreboard6.AddValue("maxSpeed", "");

                scoreboard7.AddValue("name", "<empty>");
                scoreboard7.AddValue("totalTime", "");
                scoreboard7.AddValue("maxSpeed", "");

                scoreboard8.AddValue("name", "<empty>");
                scoreboard8.AddValue("totalTime", "");
                scoreboard8.AddValue("maxSpeed", "");

                scoreboard9.AddValue("name", "<empty>");
                scoreboard9.AddValue("totalTime", "");
                scoreboard9.AddValue("maxSpeed", "");

                _save = true;
            }
            else
            {
                foreach (ConfigNode.Value cv in _scoreboard_.values)
                {
                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                    cv.name = cvEncryptedName;

                    if (cv.value != null)
                    {
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                        cv.value = cvEncryptedValue;
                    }
                }

                foreach (ConfigNode cn in _scoreboard_.nodes)
                {
                    foreach (ConfigNode.Value cv in cn.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                        cv.name = cvEncryptedName;
                        if (cv.value != null)
                        {
                            string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                            cv.value = cvEncryptedValue;
                        }
                    }

                    foreach (ConfigNode cn2 in cn.nodes)
                    {
                        foreach (ConfigNode.Value cv2 in cn2.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                            cv2.name = cvEncryptedName;
                            if (cv2.value != null)
                            {
                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                cv2.value = cvEncryptedValue;
                            }
                        }
                    }
                }
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

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB0 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }

                if (cv.name == "maxSpeed")
                {
                    saltSB0 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameSB1 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB1 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }

                if (cv.name == "maxSpeed")
                {
                    saltSB1 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameSB2 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB2 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB2 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameSB3 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB3 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB3 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameSB4 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB4 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB4 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameSB5 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB5 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB5 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameSB6 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB6 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB6 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameSB7 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB7 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }

                if (cv.name == "maxSpeed")
                {
                    saltSB7 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameSB8 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB8 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB8 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameSB9 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB9 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB9 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in _scoreboard_.values)
            {
                string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                cv.name = cvEncryptedName;
                if (cv.value != null)
                {
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.value = cvEncryptedValue;
                }
            }

            foreach (ConfigNode cn in _scoreboard_.nodes)
            {
                foreach (ConfigNode.Value cv in cn.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    cv.name = cvEncryptedName;

                    if (cv.value != null)
                    {
                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                        cv.value = cvEncryptedValue;
                    }

                }

                foreach (ConfigNode cn2 in cn.nodes)
                {
                    foreach (ConfigNode.Value cv2 in cn2.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                        cv2.name = cvEncryptedName;

                        if (cv2.value != null)
                        {
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                            cv2.value = cvEncryptedValue;
                        }
                    }
                }
            }

            if (_save)
            {
                _file.Save(_currentOrXFile);
            }
            updatingScores = false;
            showScores = true;
            movingCraft = false;
        }
        public void StartImporting()
        {
            Reach();
            bool bda = false;
            if (challengeType == "BD ARMORY")
            {
                bda = true;
                bdaChallenge = true;
            }

            statsMaxSpeed = 0;
            statsMaxDepth = 0;
            currentScoresFile = "";
            bool _continue = true;
            OrXLog.instance.DebugLog("[OrX Import Scores] === CHECKING FOR SCORE IMPORT FILES ===");
            string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Import/";
            string _challengeName = "";

            List<string> scoreFiles = new List<string>(Directory.GetFiles(importLoc, "*.scores", SearchOption.AllDirectories));
            if (scoreFiles != null)
            {
                OrXLog.instance.DebugLog("[OrX Import Scores] === SCORE IMPORT FILES FOUND ===");
                _continue = true;
                List<string>.Enumerator file = scoreFiles.GetEnumerator();
                while (file.MoveNext())
                {
                    OrXLog.instance.DebugLog("[OrX Import Scores] === CHECKING FILE === " + file);

                    if (file.Current != null && _continue)
                    {
                        OrXLog.instance.DebugLog("[OrX Import Scores] === SCORE IMPORT FILES FOUND === " + file.Current + " ===");

                        ConfigNode _scores = ConfigNode.Load(file.Current);
                        if (_scores != null)
                        {
                            OrXLog.instance.DebugLog("[OrX Import Scores] === FILE LOADED === " + file.Current + " ===");

                            foreach (ConfigNode.Value cv in _scores.values)
                            {
                                string dn = OrXLog.instance.Decrypt(cv.name);
                                if (dn == "name")
                                {
                                    _challengeName = OrXLog.instance.Decrypt(cv.value);
                                    //_challengeName = _challengeName.Replace(' ', '_');
                                    OrXLog.instance.DebugLog("[OrX Import Scores] === CHALLENGE NAME === " + _challengeName + " ===");
                                }
                            }
                            string _scoreFileFix = file.Current.Replace('.', ' ');

                            if (_scoreFileFix.Contains(HoloKronName + "-" + hkCount))
                            {
                                OrXLog.instance.DebugLog("[OrX Import Scores] === '" + file.Current + "' MATCHES '" + HoloKronName + "-" + hkCount + "' ===");
                                currentScoresFile = file.Current;
                                foreach (ConfigNode cn in _scores.nodes)
                                {
                                    if (cn.name.Contains("mission" + hkCount))
                                    {
                                        scoreboardStats = new List<string>();
                                        _continue = false;
                                        int _count = 0;

                                        foreach (ConfigNode.Value cv in cn.values)
                                        {
                                            string decryptedName = OrXLog.instance.Decrypt(cv.name);
                                            string decryptedValue = OrXLog.instance.Decrypt(cv.value);



                                            if (decryptedName == "name")
                                            {
                                                statsName = decryptedValue;
                                            }

                                            if (decryptedName == "maxSpeed")
                                            {
                                                double ms = double.Parse(decryptedValue);
                                                if (ms >= statsMaxSpeed)
                                                {
                                                    statsMaxSpeed = ms;
                                                    OrXLog.instance.DebugLog("[OrX Import Scores] === MAX SPEED === " + statsMaxSpeed + " ===");
                                                }
                                            }
                                            if (decryptedName == "maxDepth")
                                            {
                                                double md = double.Parse(decryptedValue);
                                                if (md <= statsMaxDepth)
                                                {
                                                    statsMaxDepth = md;
                                                    OrXLog.instance.DebugLog("[OrX Import Scores] === MAX DEPTH === " + statsMaxSpeed + " ===");

                                                }
                                            }
                                            if (decryptedName == "totalAirTime")
                                            {
                                                statsTotalAirTime = OrXUtilities.instance.TimeSet(float.Parse(decryptedValue));
                                                OrXLog.instance.DebugLog("[OrX Import Scores] === TOTAL AIR TIME === " + statsTotalAirTime + " ===");

                                            }
                                            if (decryptedName == "totalTime")
                                            {
                                                statsTime = OrXUtilities.instance.TimeSet(float.Parse(decryptedValue));
                                                OrXLog.instance.DebugLog("[OrX Import Scores] === TOTAL TIME === " + statsTime + " ===");

                                            }
                                            
                                            if (decryptedName.Contains("stage"))
                                            {
                                                _count += 1;
                                                scoreboardStats.Add(decryptedValue);
                                                OrXLog.instance.DebugLog("[OrX Import Scores] === STAGE " + _count + " === " + decryptedValue + " ===");

                                            }

                                            if (decryptedName == "mods")
                                            {
                                                statsMods = decryptedValue;
                                            }
                                        }

                                        OrXScoreboardStats.instance.OpenStatsWindow(bda, HoloKronName, groupName, statsName, statsTime, statsTotalAirTime, hkCount, statsMaxSpeed, statsMaxDepth, scoreboardStats, statsMods);
                                    }
                                }
                            }
                        }
                    }
                }
                file.Dispose();

                if (_continue)
                {
                    OpenScoreboardMenu();
                }
            }
            else
            {
                OpenScoreboardMenu();
            }
        }
        IEnumerator Import(string _scoresFile, List<string> _scoresData)
        {
            updatingScores = true;
            if (challengeType == "BD ARMORY")
            {
                bdaChallenge = true;
            }
            yield return new WaitForFixedUpdate();
            string challengerNameImport = "null";
            string missionNumber = "null";
            string _time = "null";
            string _speed = "null";
            string _depth = "null";
            string _air = "null";

            OrXLog.instance.DebugLog("[OrX Import Scores] === SCORE IMPORT FILE FOR " + HoloKronName + " FOUND ===");
            _mission = _file.GetNode("mission" + hkCount);
            _scoreboard_ = _mission.GetNode("scoreboard");
            OrXLog.instance.DebugLog("[OrX Import Scores] === IMPORTING: " + _scoresFile + " ===");
            bool _continue = false;
            scoreboardStats = _scoresData;

            if (_scoreboard_ == null)
            {
                OrXLog.instance.DebugLog("[OrX Import Scores] === " + HoloKronName + "-" + hkCount + "-" + groupName + " SCOREBOARD NOT FOUND ... CREATING NEW ===");

                _scoreboard_ = _mission.AddNode("scoreboard");
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
                scoreboard0.AddValue("totalTime", "");
                scoreboard0.AddValue("maxSpeed", "");

                scoreboard1.AddValue("name", "<empty>");
                scoreboard1.AddValue("totalTime", "");
                scoreboard1.AddValue("maxSpeed", "");

                scoreboard2.AddValue("name", "<empty>");
                scoreboard2.AddValue("totalTime", "");
                scoreboard2.AddValue("maxSpeed", "");

                scoreboard3.AddValue("name", "<empty>");
                scoreboard3.AddValue("totalTime", "");
                scoreboard3.AddValue("maxSpeed", "");

                scoreboard4.AddValue("name", "<empty>");
                scoreboard4.AddValue("totalTime", "");
                scoreboard4.AddValue("maxSpeed", "");

                scoreboard5.AddValue("name", "<empty>");
                scoreboard5.AddValue("totalTime", "");
                scoreboard5.AddValue("maxSpeed", "");

                scoreboard6.AddValue("name", "<empty>");
                scoreboard6.AddValue("totalTime", "");
                scoreboard6.AddValue("maxSpeed", "");

                scoreboard7.AddValue("name", "<empty>");
                scoreboard7.AddValue("totalTime", "");
                scoreboard7.AddValue("maxSpeed", "");

                scoreboard8.AddValue("name", "<empty>");
                scoreboard8.AddValue("totalTime", "");
                scoreboard8.AddValue("maxSpeed", "");

                scoreboard9.AddValue("name", "<empty>");
                scoreboard9.AddValue("totalTime", "");
                scoreboard9.AddValue("maxSpeed", "");
            }
            else
            {
                OrXLog.instance.DebugLog("[OrX Import Scores] === SCOREBOARD FOUND ... DECRYPTING ===");

                foreach (ConfigNode.Value cv in _scoreboard_.values)
                {
                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }

                foreach (ConfigNode cn in _scoreboard_.nodes)
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
            }

            ConfigNode scores = ConfigNode.Load(_scoresFile);
            foreach (ConfigNode missionNode in scores.nodes)
            {
                if (missionNode.name.Contains("mission" + hkCount))
                {
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

                    OrXLog.instance.DebugLog("[OrX Import Scores] === DECRYPTING SCORES FILE FOR " + HoloKronName + " CHALLENGE " + hkCount + " ===");

                    foreach (ConfigNode.Value cv in missionNode.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in missionNode.nodes)
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

                    bool nameAlreadyPresent = false;
                    double t = 0;

                    _continue = true;
                    challengerNameImport = missionNode.GetValue("name");
                    missionNumber = missionNode.name;
                    _time = missionNode.GetValue("totalTime");
                    t = double.Parse(_time);
                    _speed = missionNode.GetValue("maxSpeed");
                    _depth = missionNode.GetValue("maxDepth");
                    _air = missionNode.GetValue("totalAirTime");

                    Debug.Log("[OrX Import Scores] === " + HoloKronName + "-" + challengerNameImport + " .SCORES CONTAINS DATA ===");
                    if (bdaChallenge)
                    {
                        Debug.Log("[OrX Import Scores] === " + OrXUtilities.instance.TimeSet(float.Parse(_time)) + " TOTAL TIME ===");
                        Debug.Log("[OrX Import Scores] === " + _depth + " WAVES ===");
                        Debug.Log("[OrX Import Scores] === " + _air + " SALT ===");
                        Debug.Log("[OrX Import Scores] === " + _speed + " KILLS ===");
                        t = double.Parse(_air);
                    }
                    else
                    {
                        Debug.Log("[OrX Import Scores] === " + OrXUtilities.instance.TimeSet(float.Parse(_time)) + " TOTAL TIME ===");
                        Debug.Log("[OrX Import Scores] === " + _depth + " MAX DEPTH ACHIEVED ===");
                        Debug.Log("[OrX Import Scores] === " + OrXUtilities.instance.TimeSet(float.Parse(_air)) + " TOTAL AIR TIME ===");
                        Debug.Log("[OrX Import Scores] === " + _speed + " TOP SPEED ===");
                        t = double.Parse(_time);
                    }


                    bool ammendListscoreboard0 = false;
                    string nameToRemovescoreboard0 = string.Empty;
                    double totalTimescoreboard0 = 0;
                    foreach (ConfigNode.Value cv in scoreboard0.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard0 = cv.value;

                            if (bdaChallenge)
                            {
                                string _t = scoreboard0.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard0 = double.Parse(_t);

                                    if (nameToRemovescoreboard0 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard0 != t)
                                        {
                                            if (t >= totalTimescoreboard0)
                                            {
                                                ammendListscoreboard0 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard0)
                                        {
                                            ammendListscoreboard0 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard0 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard0.GetValue("totalTime");

                                if (_t != "")
                                {
                                    if (nameToRemovescoreboard0 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard0 = double.Parse(_t);
                                            if (t <= totalTimescoreboard0)
                                            {
                                                ammendListscoreboard0 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard0 = double.Parse(_t);
                                        if (t <= totalTimescoreboard0)
                                        {
                                            ammendListscoreboard0 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard0 = true;
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard1.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard1 = double.Parse(_t);

                                    if (nameToRemovescoreboard1 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard1 != t)
                                        {
                                            if (t >= totalTimescoreboard1)
                                            {
                                                ammendListscoreboard1 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard1)
                                        {
                                            ammendListscoreboard1 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard1 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard1.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard1 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard1 = _t_;
                                            if (t <= totalTimescoreboard1)
                                            {
                                                ammendListscoreboard1 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard1 = _t_;
                                        if (t <= totalTimescoreboard1)
                                        {
                                            ammendListscoreboard1 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard1 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard2.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard2 = double.Parse(_t);

                                    if (nameToRemovescoreboard2 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard2 != t)
                                        {
                                            if (t >= totalTimescoreboard2)
                                            {
                                                ammendListscoreboard2 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard2)
                                        {
                                            ammendListscoreboard2 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard2 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard2.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard2 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard2 = _t_;
                                            if (t <= totalTimescoreboard2)
                                            {
                                                ammendListscoreboard2 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard2 = _t_;
                                        if (t <= totalTimescoreboard2)
                                        {
                                            ammendListscoreboard2 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard2 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard3.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard3 = double.Parse(_t);

                                    if (nameToRemovescoreboard3 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard3 != t)
                                        {
                                            if (t >= totalTimescoreboard3)
                                            {
                                                ammendListscoreboard3 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard3)
                                        {
                                            ammendListscoreboard3 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard3 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard3.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard3 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard3 = _t_;
                                            if (t <= totalTimescoreboard3)
                                            {
                                                ammendListscoreboard3 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard3 = _t_;
                                        if (t <= totalTimescoreboard3)
                                        {
                                            ammendListscoreboard3 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard3 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard4.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard4 = double.Parse(_t);

                                    if (nameToRemovescoreboard4 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard4 != t)
                                        {
                                            if (t >= totalTimescoreboard4)
                                            {
                                                ammendListscoreboard4 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard4)
                                        {
                                            ammendListscoreboard4 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard4 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard4.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard4 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard4 = _t_;
                                            if (t <= totalTimescoreboard4)
                                            {
                                                ammendListscoreboard4 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard4 = _t_;
                                        if (t <= totalTimescoreboard4)
                                        {
                                            ammendListscoreboard4 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard4 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard5.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard5 = double.Parse(_t);

                                    if (nameToRemovescoreboard5 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard5 != t)
                                        {
                                            if (t >= totalTimescoreboard5)
                                            {
                                                ammendListscoreboard5 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard5)
                                        {
                                            ammendListscoreboard5 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard5 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard5.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard5 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard5 = _t_;
                                            if (t <= totalTimescoreboard5)
                                            {
                                                ammendListscoreboard5 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard5 = _t_;
                                        if (t <= totalTimescoreboard5)
                                        {
                                            ammendListscoreboard5 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard5 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard6.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard6 = double.Parse(_t);

                                    if (nameToRemovescoreboard6 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard6 != t)
                                        {
                                            if (t >= totalTimescoreboard6)
                                            {
                                                ammendListscoreboard6 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard6)
                                        {
                                            ammendListscoreboard6 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard6 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard6.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard6 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard6 = _t_;
                                            if (t <= totalTimescoreboard6)
                                            {
                                                ammendListscoreboard6 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard6 = _t_;
                                        if (t <= totalTimescoreboard6)
                                        {
                                            ammendListscoreboard6 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard6 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard7.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard7 = double.Parse(_t);

                                    if (nameToRemovescoreboard7 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard7 != t)
                                        {
                                            if (t >= totalTimescoreboard7)
                                            {
                                                ammendListscoreboard7 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard7)
                                        {
                                            ammendListscoreboard7 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard7 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard7.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard7 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard7 = _t_;
                                            if (t <= totalTimescoreboard7)
                                            {
                                                ammendListscoreboard7 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard7 = _t_;
                                        if (t <= totalTimescoreboard7)
                                        {
                                            ammendListscoreboard7 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {

                                        ammendListscoreboard7 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard8.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard8 = double.Parse(_t);

                                    if (nameToRemovescoreboard8 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard8 != t)
                                        {
                                            if (t >= totalTimescoreboard8)
                                            {
                                                ammendListscoreboard8 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard8)
                                        {
                                            ammendListscoreboard8 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard8 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard9.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard8 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard8 = _t_;
                                            if (t <= totalTimescoreboard8)
                                            {
                                                ammendListscoreboard8 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard8 = _t_;
                                        if (t <= totalTimescoreboard8)
                                        {
                                            ammendListscoreboard8 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard8 = true;
                                    }
                                }
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
                            if (bdaChallenge)
                            {
                                string _t = scoreboard9.GetValue("totalAirTime");

                                if (_t != "")
                                {
                                    totalTimescoreboard9 = double.Parse(_t);

                                    if (nameToRemovescoreboard9 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (totalTimescoreboard9 != t)
                                        {
                                            if (t >= totalTimescoreboard9)
                                            {
                                                ammendListscoreboard9 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (t >= totalTimescoreboard9)
                                        {
                                            ammendListscoreboard9 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    ammendListscoreboard9 = true;
                                }
                            }
                            else
                            {
                                string _t = scoreboard9.GetValue("totalTime");

                                if (_t != "")
                                {
                                    double _t_ = double.Parse(_t);

                                    if (nameToRemovescoreboard9 == challengerNameImport)
                                    {
                                        //nameAlreadyPresent = true;

                                        if (_t != _time)
                                        {
                                            totalTimescoreboard9 = _t_;
                                            if (t <= totalTimescoreboard9)
                                            {
                                                ammendListscoreboard9 = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        totalTimescoreboard9 = _t_;
                                        if (t <= totalTimescoreboard9)
                                        {
                                            ammendListscoreboard9 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!nameAlreadyPresent)
                                    {
                                        ammendListscoreboard9 = true;
                                    }
                                }
                            }
                        }
                    }

                    // EDIT PODIUM LIST SCORES IF NEDED
                    OrXLog.instance.DebugLog("[OrX Import Scores] === RETICULATING SPLINES FOR " + HoloKronName + "-" + hkCount + "-" + challengerNameImport + "' ===");

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
                        foreach (ConfigNode.Value cv in missionNode.values)
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
                            foreach (ConfigNode.Value cv in missionNode.values)
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
                                foreach (ConfigNode.Value cv in missionNode.values)
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
                                    foreach (ConfigNode.Value cv in missionNode.values)
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
                                        foreach (ConfigNode.Value cv in missionNode.values)
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
                                            foreach (ConfigNode.Value cv in missionNode.values)
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
                                                foreach (ConfigNode.Value cv in missionNode.values)
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
                                                    foreach (ConfigNode.Value cv in missionNode.values)
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
                                                        foreach (ConfigNode.Value cv in missionNode.values)
                                                        {
                                                            scoreboard8.AddValue(cv.name, cv.value);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ammendListscoreboard9)
                                                        {
                                                            scoreboard9.ClearData();
                                                            foreach (ConfigNode.Value cv in missionNode.values)
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

                    OrXLog.instance.DebugLog("[OrX Import Scores] === UPDATING SCOREBOARD FOR CHALLENGE " + hkCount + " IN '" + HoloKronName + " " + hkCount + "' ===");

                    foreach (ConfigNode.Value cv in scoreboard0.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB0 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB0 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB0 = cv.value;
                        }
                    }
                    foreach (ConfigNode.Value cv in scoreboard1.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB1 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB1 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB1 = cv.value;
                        }

                    }
                    foreach (ConfigNode.Value cv in scoreboard2.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB2 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB2 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB2 = cv.value;
                        }

                    }
                    foreach (ConfigNode.Value cv in scoreboard3.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB3 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB3 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB3 = cv.value;
                        }

                    }
                    foreach (ConfigNode.Value cv in scoreboard4.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB4 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB4 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                            if (cv.name == "maxSpeed")
                            {
                                saltSB4 = cv.value;
                            }

                        }
                    }
                    foreach (ConfigNode.Value cv in scoreboard5.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB5 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB5 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB5 = cv.value;
                        }

                    }
                    foreach (ConfigNode.Value cv in scoreboard6.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB6 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB6 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB6 = cv.value;
                        }

                    }
                    foreach (ConfigNode.Value cv in scoreboard7.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB7 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB7 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB7 = cv.value;
                        }

                    }
                    foreach (ConfigNode.Value cv in scoreboard8.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB8 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB8 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB8 = cv.value;
                        }

                    }
                    foreach (ConfigNode.Value cv in scoreboard9.values)
                    {
                        if (cv.name == "name")
                        {
                            nameSB9 = cv.value;
                        }

                        if (cv.name == "totalTime")
                        {
                            if (cv.value != "" && cv.value != "0")
                            {
                                timeSB9 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            saltSB9 = cv.value;
                        }

                    }

                    OrXLog.instance.DebugLog("[OrX Import Scores] === SCOREBOARD UPDATED FOR CHALLENGE IN '" + HoloKronName + " " + hkCount + "' ===");

                    foreach (ConfigNode.Value cv in _scoreboard_.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in _scoreboard_.nodes)
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
                    _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + groupName + "/"  + HoloKronName + "-" + hkCount + "-" + groupName + ".holo");
                }
            }

            updatingScores = false;
            movingCraft = false;
        }

        public void SaveBDAcScore()
        {
            if (!_bdacSaved)
            {
                _bdacSaved = true;
                OrXSounds.instance.WaldoSound();
                StopTimer();
                IronKerbal = false;
                _HoloKron = null;
                stageTimes = new List<string>();

                stageTimes.Add(gpsCount + "," + Math.Round(salt, 3) + "," + OrXVesselLog.instance._wave + "," + _killCount + ","
                    + missionTime + "," + CheatOptions.NoCrashDamage + "," + CheatOptions.UnbreakableJoints + "," + CheatOptions.IgnoreMaxTemperature + ","
                    + CheatOptions.InfinitePropellant + "," + CheatOptions.InfiniteElectricity + "," + _lifeCount + "," + _playerVesselCount + "," + _opForCount);
                StartCoroutine(SaveScore());
            }
        }
        IEnumerator SaveScore()
        {
            StopTimer();
            string path = UrlDir.ApplicationRootPath + "GameData/";
            installedMods = new List<string>();
            yield return new WaitForFixedUpdate();
            foreach (string s in Directory.GetDirectories(path))
            {
                installedMods.Add(s.Remove(0, path.Length));
                Debug.Log("[OrX HoloKron - Save Score] === ADDING " + s.Remove(0, path.Length) + " TO INSTALLED MOD DATABASE ===");
            }
            yield return new WaitForFixedUpdate();
            string mods = "";
            List<string>.Enumerator _installedMods = installedMods.GetEnumerator();
            while (_installedMods.MoveNext())
            {
                if (_installedMods.Current != null)
                {
                    if (mods != "")
                    {
                        mods += ",";
                    }
                    mods += _installedMods.Current;
                    Debug.Log("[OrX HoloKron - Save Score] === ADDING " + _installedMods.Current + " TO INSTALLED MOD LIST ===");
                }
            }
            _installedMods.Dispose();
            yield return new WaitForFixedUpdate();

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "Screenshots/" + HoloKronName + "/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "Screenshots/" + HoloKronName + "/");

            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/";
            List<string> files = new List<string>(Directory.GetFiles(holoKronLoc, "*.tmp", SearchOption.AllDirectories));
            if (files != null)
            {
                try
                {
                    List<string>.Enumerator toDelete = files.GetEnumerator();
                    while (toDelete.MoveNext())
                    {
                        if (toDelete.Current != null)
                        {
                            if (File.Exists(toDelete.Current))
                            {
                                File.Delete(toDelete.Current);
                                Debug.Log("[OrX HoloKron - Delete Temp Files] === DELETING " + toDelete.Current + " ===");
                            }
                        }
                    }
                    toDelete.Dispose();
                }
                catch { }
            }
            yield return new WaitForFixedUpdate();
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + groupName + "/"  + HoloKronName + "-" + hkCount + "-" + groupName + ".holo");

            if (_file != null)
            {
                _mission = _file.GetNode("mission" + hkCount);
                if (_mission == null)
                {
                    Debug.Log("[OrX HoloKron - Save Score] === Adding Mission Node ===");
                    _mission = _file.AddNode("mission" + hkCount);
                }

                _scoreboard_ = _mission.GetNode("scoreboard");
                if (_scoreboard_ == null)
                {
                    Debug.Log("[OrX HoloKron - Save Score] === Adding Scoreboard Node ===");

                    _scoreboard_ = _mission.AddNode("scoreboard");
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
                    scoreboard0.AddValue("totalTime", "");
                    scoreboard0.AddValue("maxSpeed", "");

                    scoreboard1.AddValue("name", "<empty>");
                    scoreboard1.AddValue("totalTime", "");
                    scoreboard1.AddValue("maxSpeed", "");

                    scoreboard2.AddValue("name", "<empty>");
                    scoreboard2.AddValue("totalTime", "");
                    scoreboard2.AddValue("maxSpeed", "");

                    scoreboard3.AddValue("name", "<empty>");
                    scoreboard3.AddValue("totalTime", "");
                    scoreboard3.AddValue("maxSpeed", "");

                    scoreboard4.AddValue("name", "<empty>");
                    scoreboard4.AddValue("totalTime", "");
                    scoreboard4.AddValue("maxSpeed", "");

                    scoreboard5.AddValue("name", "<empty>");
                    scoreboard5.AddValue("totalTime", "");
                    scoreboard5.AddValue("maxSpeed", "");

                    scoreboard6.AddValue("name", "<empty>");
                    scoreboard6.AddValue("totalTime", "");
                    scoreboard6.AddValue("maxSpeed", "");

                    scoreboard7.AddValue("name", "<empty>");
                    scoreboard7.AddValue("totalTime", "");
                    scoreboard7.AddValue("maxSpeed", "");

                    scoreboard8.AddValue("name", "<empty>");
                    scoreboard8.AddValue("totalTime", "");
                    scoreboard8.AddValue("maxSpeed", "");

                    scoreboard9.AddValue("name", "<empty>");
                    scoreboard9.AddValue("totalTime", "");
                    scoreboard9.AddValue("maxSpeed", "");
                }
                else
                {
                    foreach (ConfigNode.Value cv in _scoreboard_.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in _scoreboard_.nodes)
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
                }

                OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === GET CHALLENGER TOTAL TIME AND CREATE STAGE TIME LIST ===");
                int stageCount = 0;

                ConfigNode tempChallengerEntry = new ConfigNode();
                tempChallengerEntry.AddValue("name", challengersName);
                double totalTimeChallenger = 0;
                double totalAirTimeChallenger = 0;
                double maxDepthChallenger = 0;
                double maxSpeedChallenger = 0;
                yield return new WaitForFixedUpdate();

                OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === STAGE TIME COUNT = " + stageTimes.Count + " ===");

                List<string>.Enumerator st = stageTimes.GetEnumerator();
                while (st.MoveNext())
                {
                    if (st.Current != null)
                    {
                        try
                        {
                            string[] data = st.Current.Split(new char[] { ',' });
                            stageCount += 1;

                            totalTimeChallenger += double.Parse(data[4]);
                            totalAirTimeChallenger += double.Parse(data[3]);

                            double _maxDepth = double.Parse(data[2]);

                            if (bdaChallenge)
                            {
                                if (maxDepthChallenger <= _maxDepth)
                                {
                                    maxDepthChallenger = _maxDepth;
                                }
                            }
                            else
                            {
                                if (maxDepthChallenger >= _maxDepth)
                                {
                                    maxDepthChallenger = _maxDepth;
                                }
                            }

                            double _maxSpeed = double.Parse(data[1]);

                            if (maxSpeedChallenger <= _maxSpeed)
                            {
                                maxSpeedChallenger = _maxSpeed;
                            }

                            tempChallengerEntry.AddValue("stage" + stageCount, st.Current);
                        }
                        catch
                        {

                        }

                        yield return new WaitForFixedUpdate();
                    }
                }
                st.Dispose();

                tempChallengerEntry.AddValue("totalTime", totalTimeChallenger);
                tempChallengerEntry.AddValue("totalAirTime", totalAirTimeChallenger);
                tempChallengerEntry.AddValue("maxDepth", maxDepthChallenger);
                tempChallengerEntry.AddValue("maxSpeed", maxSpeedChallenger);
                tempChallengerEntry.AddValue("mods", mods);

                yield return new WaitForFixedUpdate();

                OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === TEMPORARY STAGE TIME LIST CREATED ===");
                OnScrnMsgUC("TOTAL TIME: " + _timerTotalTime);

                ConfigNode scores = new ConfigNode();
                scores.AddValue("name", HoloKronName);
                scores.AddValue("count", hkCount);
                scores.AddValue("group", groupName);
                scores.AddValue("challengersName", challengersName);
                scores.AddNode("mission" + hkCount);
                ConfigNode miss = scores.GetNode("mission" + hkCount);
                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                {
                    miss.AddValue(cv.name, cv.value);
                }

                foreach (ConfigNode.Value cv in scores.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }

                foreach (ConfigNode cn in scores.nodes)
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

                //scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Import/" + HoloKronName + "-" + hkCount + "-" + challengersName + ".scores");
                string challengerNameImport = "null";
                string missionNumber = "null";
                string _time_ = "null";
                string _speed = "null";
                string _depth = "null";
                string _air = "null";

                OrXLog.instance.DebugLog("[OrX Import Scores] === SCORE IMPORT FILE FOR " + HoloKronName + " FOUND ===");
                OrXLog.instance.DebugLog("[OrX Import Scores] === GETTING PODIUM LIST FOR " + HoloKronName + " CHALLENGE " + hkCount + " ===");

                foreach (ConfigNode missionNode in scores.nodes)
                {
                    if (missionNode.name.Contains("mission" + hkCount))
                    {
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

                        OrXLog.instance.DebugLog("[OrX Import Scores] === DECRYPTING SCORES FILE FOR " + HoloKronName + " CHALLENGE " + hkCount + " ===");

                        foreach (ConfigNode.Value cv in missionNode.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }
                        foreach (ConfigNode cn in missionNode.nodes)
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

                        bool nameAlreadyPresent = false;
                        double t = 0;
                        ConfigNode scoresNode = new ConfigNode();
                        bool _continue = true;

                        foreach (ConfigNode cn in scores.nodes)
                        {
                            if (cn.name.Contains("mission" + hkCount))
                            {
                                if (_continue)
                                {
                                    _continue = false;
                                    foreach (ConfigNode.Value cv in cn.values)
                                    {
                                        scoresNode.AddValue(cv.name, cv.value);
                                    }
                                }
                                challengerNameImport = cn.GetValue("name");
                                missionNumber = cn.name;
                                _time_ = cn.GetValue("totalTime");
                                _speed = cn.GetValue("maxSpeed");
                                _depth = cn.GetValue("maxDepth");
                                _air = cn.GetValue("totalAirTime");
                            }
                            else
                            {
                                OrXLog.instance.DebugLog("[OrX Import Scores] === NO DATA CONTAINED IN SCORES FOR CHALLENGE IN '" + HoloKronName + " " + hkCount + "' ===");
                            }
                        }

                        if (bdaChallenge)
                        {
                            Debug.Log("[OrX Import Scores] === " + OrXUtilities.instance.TimeSet((float)t) + " TOTAL TIME ===");
                            Debug.Log("[OrX Import Scores] === " + _speed + " SALT ===");
                            Debug.Log("[OrX Import Scores] === " + _air + " KILLS ===");
                            Debug.Log("[OrX Import Scores] === " + _depth + " WAVES ===");

                            t = double.Parse(_speed);

                        }
                        else
                        {
                            Debug.Log("[OrX Import Scores] === " + OrXUtilities.instance.TimeSet((float)t) + " TOTAL TIME ===");
                            Debug.Log("[OrX Import Scores] === " + _depth + " MAX DEPTH ACHIEVED ===");
                            Debug.Log("[OrX Import Scores] === " + OrXUtilities.instance.TimeSet(float.Parse(_air)) + " TOTAL AIR TIME ===");
                            Debug.Log("[OrX Import Scores] === " + _speed + " TOP SPEED ===");
                            t = double.Parse(_time_);

                        }

                        AmendScoreboard(challengerNameImport, _time_, nameAlreadyPresent, t, scoresNode);
                        UpdateScoreboard();
                    }
                }
                ConfigNode node = _file.GetNode("OrX");
                ConfigNode HoloKronNode = node.GetNode("OrXHoloKronCoords" + hkCount);

                if (HoloKronNode != null)
                {
                    Debug.Log("[OrX Mission Scoreboard] === FOUND " + HoloKronName + " " + hkCount + " ==="); ;

                    if (HoloKronNode.HasValue("completed"))
                    {
                        var t = HoloKronNode.GetValue("completed");
                        if (t == "False")
                        {
                            HoloKronNode.SetValue("completed", "True", true);

                            OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === " + HoloKronName + " " + hkCount + " was not completed ... CHANGING TO TRUE ==="); ;
                        }
                        else
                        {
                            OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === " + HoloKronName + " " + hkCount + " is already completed ==="); ;
                        }
                    }
                }

                foreach (ConfigNode.Value cv in _scoreboard_.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }
                foreach (ConfigNode cn in _scoreboard_.nodes)
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

                _showTimer = false;
                movingCraft = false;
                showScores = true;
                GuiEnabledOrXMissions = true;
                PlayOrXMission = true;
                updatingScores = false;

                OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === DATA PROCESSED AND ENCRYPTED ===");
                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + groupName + "/"  + HoloKronName + "-" + hkCount + "-" + groupName + ".holo");
                _scoreSaved = true;
                if (_entryCount != 10)
                {
                    GetStats(challengersName, _entryCount);
                    StartCoroutine(ScreenshotFinishDelay());
                }
            }
        }
        IEnumerator ScreenshotFinishDelay()
        {
            string screenshot = UrlDir.ApplicationRootPath + "Screenshots/" + HoloKronName + " FINISH.png";
            if (File.Exists(screenshot))
            {
                File.Delete(screenshot);
            }
            yield return new WaitForSeconds(2);
            ScreenCapture.CaptureScreenshot(screenshot);
        }

        private void AmendScoreboard(string challengerNameImport, string _time_, bool nameAlreadyPresent, double t, ConfigNode scoresNode)
        {
            _entryCount = 10;

            bool ammendListscoreboard0 = false;
            string nameToRemovescoreboard0 = string.Empty;
            double totalTimescoreboard0 = 0;
            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard0 = cv.value;

                    if (bdaChallenge)
                    {
                        string _t = scoreboard0.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard0 = double.Parse(_t);

                            if (nameToRemovescoreboard0 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard0 != t)
                                {
                                    if (t >= totalTimescoreboard0)
                                    {
                                        ammendListscoreboard0 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard0)
                                {
                                    ammendListscoreboard0 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard0 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard0.GetValue("totalTime");

                        if (_t != "")
                        {
                            if (nameToRemovescoreboard0 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard0 = double.Parse(_t);
                                    if (t <= totalTimescoreboard0)
                                    {
                                        ammendListscoreboard0 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard0 = double.Parse(_t);
                                if (t <= totalTimescoreboard0)
                                {
                                    ammendListscoreboard0 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard0 = true;
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard1.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard1 = double.Parse(_t);

                            if (nameToRemovescoreboard1 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard1 != t)
                                {
                                    if (t >= totalTimescoreboard1)
                                    {
                                        ammendListscoreboard1 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard1)
                                {
                                    ammendListscoreboard1 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard1 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard1.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard1 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard1 = _t_;
                                    if (t <= totalTimescoreboard1)
                                    {
                                        ammendListscoreboard1 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard1 = _t_;
                                if (t <= totalTimescoreboard1)
                                {
                                    ammendListscoreboard1 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard1 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard2.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard2 = double.Parse(_t);

                            if (nameToRemovescoreboard2 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard2 != t)
                                {
                                    if (t >= totalTimescoreboard2)
                                    {
                                        ammendListscoreboard2 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard2)
                                {
                                    ammendListscoreboard2 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard2 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard2.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard2 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard2 = _t_;
                                    if (t <= totalTimescoreboard2)
                                    {
                                        ammendListscoreboard2 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard2 = _t_;
                                if (t <= totalTimescoreboard2)
                                {
                                    ammendListscoreboard2 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard2 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard3.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard3 = double.Parse(_t);

                            if (nameToRemovescoreboard3 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard3 != t)
                                {
                                    if (t >= totalTimescoreboard3)
                                    {
                                        ammendListscoreboard3 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard3)
                                {
                                    ammendListscoreboard3 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard3 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard3.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard3 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard3 = _t_;
                                    if (t <= totalTimescoreboard3)
                                    {
                                        ammendListscoreboard3 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard3 = _t_;
                                if (t <= totalTimescoreboard3)
                                {
                                    ammendListscoreboard3 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard3 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard4.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard4 = double.Parse(_t);

                            if (nameToRemovescoreboard4 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard4 != t)
                                {
                                    if (t >= totalTimescoreboard4)
                                    {
                                        ammendListscoreboard4 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard4)
                                {
                                    ammendListscoreboard4 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard4 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard4.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard4 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard4 = _t_;
                                    if (t <= totalTimescoreboard4)
                                    {
                                        ammendListscoreboard4 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard4 = _t_;
                                if (t <= totalTimescoreboard4)
                                {
                                    ammendListscoreboard4 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard4 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard5.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard5 = double.Parse(_t);

                            if (nameToRemovescoreboard5 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard5 != t)
                                {
                                    if (t >= totalTimescoreboard5)
                                    {
                                        ammendListscoreboard5 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard5)
                                {
                                    ammendListscoreboard5 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard5 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard5.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard5 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard5 = _t_;
                                    if (t <= totalTimescoreboard5)
                                    {
                                        ammendListscoreboard5 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard5 = _t_;
                                if (t <= totalTimescoreboard5)
                                {
                                    ammendListscoreboard5 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard5 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard6.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard6 = double.Parse(_t);

                            if (nameToRemovescoreboard6 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard6 != t)
                                {
                                    if (t >= totalTimescoreboard6)
                                    {
                                        ammendListscoreboard6 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard6)
                                {
                                    ammendListscoreboard6 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard6 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard6.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard6 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard6 = _t_;
                                    if (t <= totalTimescoreboard6)
                                    {
                                        ammendListscoreboard6 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard6 = _t_;
                                if (t <= totalTimescoreboard6)
                                {
                                    ammendListscoreboard6 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard6 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard7.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard7 = double.Parse(_t);

                            if (nameToRemovescoreboard7 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard7 != t)
                                {
                                    if (t >= totalTimescoreboard7)
                                    {
                                        ammendListscoreboard7 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard7)
                                {
                                    ammendListscoreboard7 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard7 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard7.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard7 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard7 = _t_;
                                    if (t <= totalTimescoreboard7)
                                    {
                                        ammendListscoreboard7 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard7 = _t_;
                                if (t <= totalTimescoreboard7)
                                {
                                    ammendListscoreboard7 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {

                                ammendListscoreboard7 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard8.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard8 = double.Parse(_t);

                            if (nameToRemovescoreboard8 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard8 != t)
                                {
                                    if (t >= totalTimescoreboard8)
                                    {
                                        ammendListscoreboard8 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard8)
                                {
                                    ammendListscoreboard8 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard8 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard9.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard8 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard8 = _t_;
                                    if (t <= totalTimescoreboard8)
                                    {
                                        ammendListscoreboard8 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard8 = _t_;
                                if (t <= totalTimescoreboard8)
                                {
                                    ammendListscoreboard8 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard8 = true;
                            }
                        }
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
                    if (bdaChallenge)
                    {
                        string _t = scoreboard9.GetValue("maxSpeed");

                        if (_t != "")
                        {
                            totalTimescoreboard9 = double.Parse(_t);

                            if (nameToRemovescoreboard9 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (totalTimescoreboard9 != t)
                                {
                                    if (t >= totalTimescoreboard9)
                                    {
                                        ammendListscoreboard9 = true;
                                    }
                                }
                            }
                            else
                            {
                                if (t >= totalTimescoreboard9)
                                {
                                    ammendListscoreboard9 = true;
                                }
                            }
                        }
                        else
                        {
                            ammendListscoreboard9 = true;
                        }
                    }
                    else
                    {
                        string _t = scoreboard9.GetValue("totalTime");

                        if (_t != "")
                        {
                            double _t_ = double.Parse(_t);

                            if (nameToRemovescoreboard9 == challengerNameImport)
                            {
                                //nameAlreadyPresent = true;

                                if (_t != _time_)
                                {
                                    totalTimescoreboard9 = _t_;
                                    if (t <= totalTimescoreboard9)
                                    {
                                        ammendListscoreboard9 = true;
                                    }
                                }
                            }
                            else
                            {
                                totalTimescoreboard9 = _t_;
                                if (t <= totalTimescoreboard9)
                                {
                                    ammendListscoreboard9 = true;
                                }
                            }
                        }
                        else
                        {
                            if (!nameAlreadyPresent)
                            {
                                ammendListscoreboard9 = true;
                            }
                        }
                    }
                }
            }

            // EDIT PODIUM LIST SCORES IF NEDED
            OrXLog.instance.DebugLog("[OrX Import Scores] === RETICULATING SPLINES FOR " + HoloKronName + "-" + hkCount + "-" + challengerNameImport + "' ===");

            if (ammendListscoreboard0)
            {
                _entryCount = 0;
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
                foreach (ConfigNode.Value cv in scoresNode.values)
                {
                    scoreboard0.AddValue(cv.name, cv.value);
                }
            }
            else
            {
                if (ammendListscoreboard1)
                {
                    _entryCount = 1;
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
                    foreach (ConfigNode.Value cv in scoresNode.values)
                    {
                        scoreboard1.AddValue(cv.name, cv.value);
                    }
                }
                else
                {
                    if (ammendListscoreboard2)
                    {
                        _entryCount = 2;
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
                        foreach (ConfigNode.Value cv in scoresNode.values)
                        {
                            scoreboard2.AddValue(cv.name, cv.value);
                        }
                    }
                    else
                    {
                        if (ammendListscoreboard3)
                        {
                            _entryCount = 3;

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
                            foreach (ConfigNode.Value cv in scoresNode.values)
                            {
                                scoreboard3.AddValue(cv.name, cv.value);
                            }
                        }
                        else
                        {
                            if (ammendListscoreboard4)
                            {
                                _entryCount = 4;

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
                                foreach (ConfigNode.Value cv in scoresNode.values)
                                {
                                    scoreboard4.AddValue(cv.name, cv.value);
                                }
                            }
                            else
                            {
                                if (ammendListscoreboard5)
                                {
                                    _entryCount = 5;

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
                                    foreach (ConfigNode.Value cv in scoresNode.values)
                                    {
                                        scoreboard5.AddValue(cv.name, cv.value);
                                    }
                                }
                                else
                                {
                                    if (ammendListscoreboard6)
                                    {
                                        _entryCount = 6;

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
                                        foreach (ConfigNode.Value cv in scoresNode.values)
                                        {
                                            scoreboard6.AddValue(cv.name, cv.value);
                                        }
                                    }
                                    else
                                    {
                                        if (ammendListscoreboard7)
                                        {
                                            _entryCount = 7;

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
                                            foreach (ConfigNode.Value cv in scoresNode.values)
                                            {
                                                scoreboard7.AddValue(cv.name, cv.value);
                                            }
                                        }
                                        else
                                        {
                                            if (ammendListscoreboard8)
                                            {
                                                _entryCount = 8;

                                                scoreboard9.ClearData();
                                                foreach (ConfigNode.Value cv in scoreboard8.values)
                                                {
                                                    scoreboard9.AddValue(cv.name, cv.value);
                                                }

                                                scoreboard8.ClearData();
                                                foreach (ConfigNode.Value cv in scoresNode.values)
                                                {
                                                    scoreboard8.AddValue(cv.name, cv.value);
                                                }
                                            }
                                            else
                                            {
                                                if (ammendListscoreboard9)
                                                {
                                                    _entryCount = 9;

                                                    scoreboard9.ClearData();
                                                    foreach (ConfigNode.Value cv in scoresNode.values)
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
        }
        private void UpdateScoreboard()
        {
            Debug.Log("[OrX Import Scores] === UPDATING SCOREBOARD FOR CHALLENGE " + hkCount + " IN '" + HoloKronName + " " + hkCount + "' ===");

            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameSB0 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB0 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB0 = cv.value;
                }
            }
            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameSB1 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB1 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB1 = cv.value;
                }

            }
            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameSB2 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB2 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB2 = cv.value;
                }

            }
            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameSB3 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB3 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB3 = cv.value;
                }

            }
            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameSB4 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB4 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                    if (cv.name == "maxSpeed")
                    {
                        saltSB4 = cv.value;
                    }

                }
            }
            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameSB5 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB5 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB5 = cv.value;
                }

            }
            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameSB6 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB6 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB6 = cv.value;
                }

            }
            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameSB7 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB7 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB7 = cv.value;
                }

            }
            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameSB8 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB8 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB8 = cv.value;
                }

            }
            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameSB9 = cv.value;
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB9 = OrXUtilities.instance.TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB9 = cv.value;
                }

            }

            OrXLog.instance.DebugLog("[OrX Import Scores] === SCOREBOARD UPDATED FOR CHALLENGE IN '" + HoloKronName + " " + hkCount + "' ===");
        }
        public void ResetScoreboard()
        {
            ConfigNode missionNode = _mission.GetNode("scoreboard");
            foreach (ConfigNode cn in missionNode.nodes)
            {
                if (cn.name.Contains("scoreboard"))
                {
                    foreach (ConfigNode.Value cv in cn.nodes)
                    {
                        if (cv.name == "neam")
                        {
                            cv.value = "tyem<>p";
                        }

                        if (cv.name == "iloameTtt")
                        {
                            cv.value = "";
                        }
                        if (cv.name == "mpaSedex ")
                        {
                            cv.value = "";
                        }
                    }
                }
            }
            _file.Save(_currentOrXFile);

            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameSB0 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB0 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB0 = "";
                }
            }
            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameSB1 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB1 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB1 = "";
                }

            }
            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameSB2 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB2 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB2 = "";
                }

            }
            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameSB3 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB3 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB3 = "<empty>";
                }

            }
            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameSB4 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB4 = "";
                    }
                    if (cv.name == "maxSpeed")
                    {
                        saltSB4 = "";
                    }

                }
            }
            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameSB5 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB5 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB5 = "";
                }

            }
            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameSB6 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB6 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB6 = "";
                }

            }
            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameSB7 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB7 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB7 = "";
                }

            }
            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameSB8 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB8 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB8 = "";
                }

            }
            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameSB9 = "<empty>";
                }

                if (cv.name == "totalTime")
                {
                    if (cv.value != "" && cv.value != "0")
                    {
                        timeSB9 = "";
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    saltSB9 = "";
                }

            }
        }

        #endregion

        #region Menu Shortcuts

        public void Reach()
        {
            //_getCenterDist = false;
            getNextCoord = false;
            movingCraft = true;
            GuiEnabledOrXMissions = true;
            _showSettings = false;
            connectToKontinuum = false;
        }
        public void MainMenu()
        {
            OrXLog.instance.ResetFocusKeys();
            PlayOrXMission = false;
            movingCraft = false;
            getNextCoord = false;
            GuiEnabledOrXMissions = false;
            _showSettings = false;
            connectToKontinuum = false;
            showGeoCacheList = false;
            showCreatedHolokrons = false;
            showChallengelist = false;
            checking = false;
            challengeRunning = false;
            _spawningVessel = false;
        }
        public void SetModFail()
        {
            modCheckFail = true;
            _showSettings = false;
            PlayOrXMission = true;
            showScores = false;
            movingCraft = false;
        }
        public void PlaceMenu()
        {
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            _spawningVessel = false;
            addCoords = true;
            getNextCoord = true;
            movingCraft = true;
        }

        public void SpawnMenu()
        {
            _holdVesselPos = false;
            _settingAltitude = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            _spawningVessel = true;
            addCoords = false;
            movingCraft = false;
        }
        public void PlaceCraft()
        {
            _settingAltitude = false;
            _holdVesselPos = false;
            PlayOrXMission = false;
            addCoords = false;
            movingCraft = false;
            _spawningVessel = true;
        }
        public void SetBDAc()
        {
            if (!_bdacSet)
            {
                _bdacSet = true;
                _killCount = 0;
                _lifeCount = 0;
                gpsCount = 1;
                GuiEnabledOrXMissions = true;
                OrXHCGUIEnabled = true;
                connectToKontinuum = false;
                _showSettings = false;
                getNextCoord = false;
                movingCraft = true;
                _timerOn = false;
                salt = 0;
                challengeRunning = true;
                bdaChallenge = true;
                outlawRacing = false;
                geoCache = false;
                windRacing = false;
                Scuba = false;
                _showTimer = true;
                triggerVessel = FlightGlobals.ActiveVessel;
                StartTimer();
            }
        }
        public void ShowTimer()
        {
            salt = 0;
            challengeRunning = true;
            bdaChallenge = true;
            outlawRacing = false;
            geoCache = false;
            windRacing = false;
            Scuba = false;
        }
        public void OpenScoreboardMenu()
        {
            GuiEnabledOrXMissions = true;
            _importingScores = false;
            _extractScoreboard = false;
            showScores = true;
            PlayOrXMission = true;
            movingCraft = false;
            connectToKontinuum = false;
            _showSettings = false;
        }
        public void ScanMenu()
        {
            checking = true;
            GuiEnabledOrXMissions = false;
            _editor = false;
            challengeRunning = false;
            showGeoCacheList = false;
            showCreatedHolokrons = false;
            showChallengelist = false;
            showScores = false;
            mrKleen = false;
            movingCraft = false;
        }

        #endregion

        /// <summary>
        /// //////////////////////////////
        /// </summary>

        void OnGUI()
        {
            GUI.backgroundColor = XKCDColors.DarkGrey;
            GUI.contentColor = XKCDColors.DarkGrey;
            GUI.color = XKCDColors.DarkGrey;

            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (showTargets)
                {
                    OrXLog.DrawRecticle(FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission), OrXLog.instance.HoloTargetTexture, new Vector2(16, 16));
                }

                if (OrXHCGUIEnabled && !OrXScoreboardStats.instance.GuiEnabledStats)
                {
                    WindowRectToolbar = GUI.Window(558917362, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
                }
            }
            else
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    if (OrXHCGUIEnabled && !OrXScoreboardStats.instance.GuiEnabledStats)
                    {
                        WindowRectToolbar = GUI.Window(558917362, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
                    }
                }
            }
        }
        public void ToggleGUI()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                _editor = false;
                if (!reset)
                {
                    if (OrXHCGUIEnabled)
                    {
                        if (buildingMission)
                        {
                            reset = true;
                            connectToKontinuum = true;
                            _showSettings = false;
                        }
                        else
                        {
                            if (challengeRunning)
                            {
                                if (!_showTimer)
                                {
                                    OrXLog.instance.DebugLog("[OrX Toggle GUI]: Hiding OrX HoloKron GUI");
                                    OrXHCGUIEnabled = false;
                                }
                            }
                            else
                            {
                                OrXLog.instance.DebugLog("[OrX Toggle GUI]: Hiding OrX HoloKron GUI");
                                OrXHCGUIEnabled = false;
                            }
                        }
                    }
                    else
                    {
                        if (_OrXV == _ate)
                        {
                            OnScrnMsgUC("UNSUPPORTED VERSION OF KSP");
                            OnScrnMsgUC("Dinner Out is Cancelled .....");
                            OnScrnMsgUC("OrX Kontinuum shutting down .....");
                            OrXHCGUIEnabled = false;
                            StopAllCoroutines();
                        }
                        else
                        {
                            if (!OrXMode.instance._guiEnabled)
                            {
                                WindowRectToolbar = new Rect(Screen.width - (WindowWidth + 50), 50, toolWindowWidth, toolWindowHeight);

                                if (OrXLog.instance.PREnabled())
                                {
                                    OrXLog.instance.GetPRERanges();
                                    OnScrnMsgUC("Please remember to disable");
                                    OnScrnMsgUC("Physics Range Extender");
                                }

                                OrXHCGUIEnabled = true;
                                Debug.Log("[OrX Toggle GUI - Flag Url] === " + HighLogic.CurrentGame.flagURL + " === ");
                                Debug.Log("[OrX Toggle GUI - Save Folder] === " + HighLogic.SaveFolder + " === ");

                                if (!challengeRunning)
                                {
                                    if (FlightGlobals.currentMainBody.pqsController.horizonDistance != OrXLog.instance._preLoadRange * 1000)
                                    {
                                        FlightGlobals.currentMainBody.pqsController.horizonDistance = OrXLog.instance._preLoadRange * 1000;
                                        FlightGlobals.currentMainBody.pqsController.maxDetailDistance = OrXLog.instance._preLoadRange * 1000;
                                        FlightGlobals.currentMainBody.pqsController.minDetailDistance = OrXLog.instance._preLoadRange * 1000;
                                        FlightGlobals.currentMainBody.pqsController.visRadSeaLevelValue = 200;
                                        FlightGlobals.currentMainBody.pqsController.collapseSeaLevelValue = 200;
                                    }

                                    if (!OrXSpawnHoloKron.instance.spawning)
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            OrXUtilities.instance.SetRanges(65000);
                                        }
                                        if (!buildingMission)
                                        {
                                            ResetData();
                                            movingCraft = false;
                                            getNextCoord = false;
                                            MainMenu();

                                            Debug.Log("[OrX Toggle GUI]: Not building ... Checking if challenge is running ....");
                                            if (!challengeRunning)
                                            {
                                                Debug.Log("[OrX Toggle GUI]: Challenge not running ....");
                                                //OrXUtilities.instance.LoadData();
                                            }
                                            else
                                            {
                                                Debug.Log("[OrX Toggle GUI]: Challenge is running ... Opening OrX HoloKron Challenge GUI");
                                                GuiEnabledOrXMissions = false;

                                                checking = true;
                                                _showSettings = false;
                                                connectToKontinuum = false;
                                            }
                                        }
                                        else
                                        {
                                            OnScrnMsgUC("Unable to scan while creating .......");
                                            reset = true;
                                            connectToKontinuum = true;
                                            _showSettings = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                OnScrnMsgUC("Close the mode menu to re-open the main menu");
                            }
                        }
                    }
                }
            }
            else
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
                    _editor = true;
                    if (OrXHCGUIEnabled)
                    {
                        OrXHCGUIEnabled = false;
                        MainMenu();
                    }
                    else
                    {
                        if (!OrXEditor.instance._guiEnabled)
                        {
                            OrXEditor.instance._guiEnabled = true;
                        }
                        else
                        {
                            OrXEditor.instance._guiEnabled = false;
                        }
                    }
                }
            }
        }
        public void OrXHCGUI(int OrX_HCGUI)
        {
            float line = 0;
            float leftIndent = 10;
            float contentWidth = toolWindowWidth - leftIndent;
            float ContentTop = 10;
            float entryHeight = 20;
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));

            if (_showSettings)
            {
                if (!stopwatch)
                {
                    GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum Settings", titleStyleLarge);
                }
                else
                {
                    GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Stop Watch", titleStyleLarge);
                }
                line++;

                if (!_showMode)
                {
                    if (HighLogic.LoadedSceneIsFlight)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Mr Kleen's Magic Eraser", OrXGUISkin.button))
                        {
                            OrXUtilities.instance.StartMrKleen();
                        }
                        line++;
                        line += 0.2f;
                    }

                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Reset Kontinuum", OrXGUISkin.button))
                    {
                        challengeRunning = false;
                        OnScrnMsgUC("Resetting the Kontinuum .....");
                        locCount = 0;
                        locAdded = false;
                        building = false;
                        buildingMission = false;
                        addCoords = false;
                        GuiEnabledOrXMissions = false;
                        OrXHCGUIEnabled = false;
                        PlayOrXMission = false;
                        _showSettings = false;
                        checking = false;
                        ResetData();
                    }

                    line += 0.2f;
                    line++;
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                    {
                        _showSettings = false;
                    }
                }
                else
                {
                    _pKarma = GUI.TextField(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), _pKarma);
                    line++;
                    line += 0.2f;
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Enter the void", OrXGUISkin.button))
                    {
                        if (_pKarma == Karma)
                        {
                            Debug.Log("[OrX Karma] === OPENING MODE GUI ===");
                            Reach();
                            _showMode = false;
                            OrXMode.instance.SetMode();
                        }
                        else
                        {
                            _showMode = false;
                            MainMenu();
                            OrXLog.instance.DebugLog("[OrX Karma] === WRONG PASSWORD ===");
                            OnScrnMsgUC("<b><i><color#b01b00ff>YOU SHALL NOT PASS</color></i></b>");
                        }
                    }
                    line += 0.2f;
                    line++;
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                    {
                        _showMode = false;
                    }
                }
            }
            else
            {
                if (connectToKontinuum)
                {
                    if (reset)
                    {
                        GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum Reset", titleStyleLarge);
                        line++;

                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Reset OrX Kontinuum", OrXGUISkin.button))
                        {
                            connectToKontinuum = false;
                            OnScrnMsgUC("Resetting the Kontinuum .....");
                            reset = false;
                            OrXHCGUIEnabled = false;
                            ResetData();
                        }
                    }
                    else
                    {
                        GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum Login", titleStyleLarge);
                        line += 0.5f;
                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Enter a username below", titleStyleMedBooger);
                        line += 0.2f;
                        line++;
                        DrawKontinuumLogin2(line);
                        line += 0.2f;
                        line++;
                        DrawKontinuumLogin4(line);
                        line++;
                        line += 0.2f;
                        DrawKontinuumLogin3(line);
                        line++;
                        line++;

                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Download from Kontinuum", OrXGUISkin.button))
                        {
                            if (loginName != "")
                            {
                                if (pasKontinuum != "")
                                {
                                    if (urlKontinuum != "")
                                    {
                                        challengersName = loginName;
                                        ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
                                        if (playerData == null)
                                        {
                                            playerData = new ConfigNode();
                                        }
                                        playerData.SetValue("name", challengersName, true);
                                        playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
                                        try
                                        {
                                            Kontinuum.OrXKontinuum.instance.DownloadFile(urlKontinuum, pasKontinuum);
                                        }
                                        catch
                                        {
                                            OnScrnMsgUC("Unable to connect ....");
                                        }
                                    }
                                    else
                                    {
                                        OnScrnMsgUC("Please enter a shared file web link ....");
                                    }

                                }
                                else
                                {
                                    OnScrnMsgUC("Please enter a file name ....");
                                }
                            }
                            else
                            {
                                OnScrnMsgUC("Please enter a name ....");
                            }
                        }
                        //line += 0.5f;
                        //line++;
                        
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Disconnect from Kontinuum", OrXGUISkin.button))
                        {
                            //Disconnect();
                            connectToKontinuum = false;
                            OnScrnMsgUC("The Kontinuum is currently unavailable .....");
                        }
                        line += 0.5f;
                        line++;

                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                        {
                            connectToKontinuum = false;
                        }
                        line++;
                    }
                }
                else
                {
                    if (GuiEnabledOrXMissions)
                    {
                        if (movingCraft)
                        {
                            if (getNextCoord)
                            {
                                if (!_spawningVessel)
                                {
                                    GUI.Label(new Rect(0, 0, WindowWidth, 20), "Co-ordinate Editor", titleStyleLarge);
                                    line++;
                                    line += 0.1f;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Start Location Distance: " + Math.Round((targetDistance / 1000), 2) + " km", rangeColor);
                                    line++;
                                    line += 0.1f;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Stage Count: " + locCount, titleStyleMedGreen);
                                    line++;
                                    line += 0.1f;

                                    if (spawningStartGate)
                                    {
                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "PLACE GATE", OrXGUISkin.button))
                                        {
                                            if (!_killPlace)
                                            {
                                                OrXLog.instance.UpdateRangesOnFGReady();
                                                
                                                int _count = (locCount + 1);
                                                OrXLog.instance.DebugLog("[OrX Place Gate] ===== PLACING GATE FOR " + HoloKronName + " STAGE " + _count + " =====");
                                                spawningStartGate = false;
                                                getNextCoord = false;
                                                OrXVesselMove.Instance.MovingVessel.vesselName = HoloKronName + " STAGE " + _count;
                                                OrXVesselMove.Instance.KillMove(true, true);
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!dakarRacing)
                                        {
                                            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "ADD NEXT STAGE", OrXGUISkin.button))
                                            {
                                                OrXLog.instance.DebugLog("[OrX Add Stage] ===== ADDING STAGE " + OrXSpawnHoloKron.instance.stageCount + " =====");
                                                addCoords = true;
                                                startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                                                //OrXLog.instance.UpdateRangesOnFGReady();

                                                addingMission = true;
                                                getNextCoord = true;
                                                showTargets = false;
                                                movingCraft = false;
                                                saveLocalVessels = true;
                                                OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 0, false, false, new Vector3d());
                                            }

                                        }
                                        else
                                        {
                                            if (locCount == 0)
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "ADD NEXT STAGE", OrXGUISkin.button))
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Add Stage] ===== ADDING STAGE " + OrXSpawnHoloKron.instance.stageCount + " =====");
                                                    addCoords = true;
                                                    startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                                                    //OrXLog.instance.UpdateRangesOnFGReady();

                                                    addingMission = true;
                                                    getNextCoord = true;
                                                    showTargets = false;
                                                    movingCraft = false;
                                                    saveLocalVessels = true;
                                                    OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 0, false, false, new Vector3d());
                                                }
                                            }
                                        }

                                        if (CoordDatabase.Count != 0 && !_gateKillDelay)
                                        {
                                            line++;
                                            line += 0.2f;
                                            DrawClearLastCoord(line);
                                            line++;
                                            line += 0.2f;
                                            DrawMoveToLastCoord(line);
                                        }
                                        line++;
                                        line++;
                                        line += 0.2f;
                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "SAVE AND EXIT", OrXGUISkin.button))
                                        {
                                            //OrXLog.instance.UpdateRangesOnFGReady();

                                            OrXLog.instance.DebugLog("[OrX Save and Exit] === SAVING HOLOKRON ===");
                                            addCoords = false;
                                            addingMission = true;
                                            getNextCoord = false;
                                            _lastStage.vesselName = HoloKronName + " " + hkCount + " FINSH";

                                            SaveConfig(HoloKronName, false);
                                        }

                                        line++;
                                        line += 0.2f;
                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CANCEL AND EXIT", OrXGUISkin.button))
                                        {
                                            OrXLog.instance.DebugLog("[OrX Mission] === CANCEL HOLOKRON CREATION ===");
                                            if (_HoloKron != null)
                                            {
                                                if (_HoloKron.rootPart.Modules.Contains<ModuleOrXMission>())
                                                {
                                                    _HoloKron.rootPart.explode();
                                                }
                                            }
                                            CancelChallenge();
                                        }
                                    }
                                }
                                else
                                {
                                    GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Spawn Menu", titleStyleLarge);
                                    line++;
                                    line += 0.2f;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Start Location Distance: " + Math.Round((targetDistance / 1000), 2) + " km", rangeColor);
                                    line++;
                                    line += 0.2f;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Current Altitude: " + Math.Round(_spawnedCraft.altitude, 2) + " meters", rangeColor);

                                    if (!_settingAltitude)
                                    {
                                        line++;
                                        line += 0.2f;
                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "PLACE", OrXGUISkin.button))
                                        {
                                            if (!_killPlace)
                                            {
                                                Reach();
                                                OrXLog.instance.UpdateRangesOnFGReady();
                                                _holdVesselPos = true;
                                                _spawningVessel = true;

                                                OrXLog.instance.DebugLog("[OrX Spawn Craft] ===== PLACING " + FlightGlobals.ActiveVessel.vesselName + " =====");
                                                OrXVesselMove.Instance.KillMove(true, LBC);
                                            }
                                            else
                                            {
                                                OnScrnMsgUC("You are too far from the start location");
                                                OnScrnMsgUC("Check your distance");
                                            }
                                        }
                                    }

                                    if (!_holdVesselPos && !FlightGlobals.ActiveVessel.rootPart.Modules.Contains<KerbalEVA>())
                                    {

                                        if (OrXHoloKron.instance.buildingMission)
                                        {
                                            line++;
                                            line += 0.4f;
                                            _saveAltitude = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), _saveAltitude, 2000, 8000, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalSliderThumb);
                                            line += 0.4f;
                                            GUI.Label(new Rect(0, (ContentTop + line * entryHeight) + line, WindowWidth, 20), "Radar Alt: " + String.Format("{0:0}", _saveAltitude) + " meters", centerLabelGreen);

                                            line++;
                                            line++;

                                            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "SET RADAR ALTITUDE", OrXGUISkin.button))
                                            {
                                                if (!_killPlace)
                                                {
                                                    Reach();
                                                    FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.Gear, false);
                                                    _settingAltitude = true;
                                                    OrXLog.instance.DebugLog("[OrX Spawn Craft] ===== SETTING ALTITUDE FOR " + FlightGlobals.ActiveVessel.vesselName + " =====");
                                                    bool _raise = false;
                                                    if (FlightGlobals.ActiveVessel.radarAltitude <= _saveAltitude)
                                                    {
                                                        _raise = true;
                                                    }
                                                    OrXVesselMove.Instance.SetAltitude(_raise, _saveAltitude);
                                                    _holdVesselPos = false;
                                                }
                                                else
                                                {
                                                    OnScrnMsgUC("You are too far from the start location");
                                                    OnScrnMsgUC("Check your distance");
                                                }
                                            }
                                        }

                                        if (_settingAltitude)
                                        {
                                            line++;
                                            line += 0.2f;

                                            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "HOLD POSITION", OrXGUISkin.button))
                                            {
                                                if (!_killPlace)
                                                {
                                                    Reach();
                                                    _holdVesselPos = true;
                                                    OrXLog.instance.DebugLog("[OrX Spawn Craft] ===== HOLDING ALTITUDE FOR " + FlightGlobals.ActiveVessel.vesselName + " =====");
                                                    OrXVesselMove.Instance.KillMove(false, false);
                                                }
                                                else
                                                {
                                                    OnScrnMsgUC("You are too far from the start location");
                                                    OnScrnMsgUC("Check your distance and altitude");
                                                }
                                            }
                                        }
                                    }

                                    if (!triggerVessel.isActiveVessel)
                                    {
                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "KILL CRAFT", OrXGUISkin.button))
                                        {
                                            Reach();
                                            FlightGlobals.ActiveVessel.rootPart.AddModule("ModuleOrXJason", true);
                                            StartCoroutine(FocusSwitchDelay(triggerVessel));
                                        }
                                    }
                                    /*
                                    if (!_holdVesselPos)
                                    {
                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "SPAWN MENU", OrXGUISkin.button))
                                        {
                                            _settingAltitude = false;
                                            _holdVesselPos = false;
                                            PlayOrXMission = false;
                                            addCoords = false;
                                            movingCraft = false;
                                            _spawningVessel = true;
                                        }
                                    }
                                    */
                                }
                                line += 0.5f;
                            }
                            else
                            {
                                if (_showTimer)
                                {
                                    if (bdaChallenge)
                                    {
                                        GUI.Label(new Rect(20, 1, WindowWidth / 2, 20), "Time: ", titleStyleMedNoItal);
                                        GUI.Label(new Rect(WindowWidth / 2 + 10, 1, WindowWidth / 2, 20), _timerTotalTime, titleStyleMedGreen);
                                        line += 0.4f;
                                        if (targetDistance >= 20000)
                                        {
                                            GUI.Label(new Rect(20, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Distance: ", titleStyleMedNoItal);
                                            GUI.Label(new Rect(WindowWidth / 2 + 10, ContentTop + line * entryHeight, WindowWidth / 2, 20), Math.Round((targetDistance / 1000), 0) + " km", titleStyleMedGreen);
                                            line++;
                                        }

                                        GUI.Label(new Rect(20, (ContentTop + line * entryHeight), WindowWidth / 2, 20), "Salt: ", titleStyleMedNoItal);
                                        GUI.Label(new Rect(WindowWidth / 2 + 10, (ContentTop + line * entryHeight), WindowWidth / 2, 20), Math.Round(salt, 3).ToString(), titleStyleMedGreen);
                                        
                                        if (_killCount != 0)
                                        {
                                            line++;
                                            GUI.Label(new Rect(20, (ContentTop + line * entryHeight), WindowWidth / 2, 20), "Kills: ", titleStyleMedNoItal);
                                            GUI.Label(new Rect(WindowWidth / 2 + 10, (ContentTop + line * entryHeight), WindowWidth / 2, 20), _killCount.ToString(), titleStyleMedGreen);
                                        }

                                        if (_lifeCount != 0)
                                        {
                                            line++;
                                            GUI.Label(new Rect(20, (ContentTop + line * entryHeight), WindowWidth / 2, 20), "MIA: ", titleStyleMedNoItal);
                                            GUI.Label(new Rect(WindowWidth / 2 + 10, (ContentTop + line * entryHeight), WindowWidth / 2, 20), _lifeCount.ToString(), titleStyleMedGreen);
                                        }
                                        if (!IronKerbal)
                                        {
                                            if (_killCount >= _opForCount - 1)
                                            {
                                                line++;
                                                line += 0.3f;
                                                DrawIronKerbal(line);
                                            }
                                        }
                                        else
                                        {
                                            line++;
                                            line += 0.3f;
                                            DrawIronKerbal(line);
                                        }

                                        if (salt >= 1)
                                        {
                                            line++;
                                            line += 0.7f;
                                            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, toolWindowWidth - 20, 20), "SPORTING GOODS", OrXGUISkin.button))
                                            {
                                                OrXSounds.instance.sound_ShopSmart.Play();
                                                OrXSpawnHoloKron.instance.CraftSelect(false, true, true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (outlawRacing)
                                        {
                                            if (challengeRunning)
                                            {
                                                GUI.Label(new Rect(20, 1, WindowWidth / 2, 20), "Challenge Total Time: ", titleStyleMedNoItal);
                                                GUI.Label(new Rect(WindowWidth / 2 + 10, 1, WindowWidth / 2, 20), _timerTotalTime, titleStyleMedGreen);
                                                if (shortTrackRacing)
                                                {
                                                    GUI.Label(new Rect(20, (ContentTop + (line + 0.4f) * entryHeight), WindowWidth / 2, 20), "Previous Stage Time: ", titleStyleMedNoItal);
                                                    GUI.Label(new Rect(WindowWidth / 2 + 10, (ContentTop + (line + 0.4f) * entryHeight) + 0.5f, WindowWidth / 2, 20), _timerStageTime, titleStyleMedGreen);
                                                    line++;
                                                }
                                                GUI.Label(new Rect(23, ContentTop + (line + 0.4f) * entryHeight, WindowWidth / 2, 20), "Next Stage Distance: ", titleStyleMedNoItal);
                                                GUI.Label(new Rect(WindowWidth / 2, ContentTop + (line + 0.4f) * entryHeight, WindowWidth / 2, 20), "" + Math.Round((targetDistance * distanceDisplayMod), 1), titleStyleMedGreen);

                                                if (GUI.Button(new Rect(WindowWidth - 50, ContentTop + (line + 0.5f) * entryHeight, 35, 18), distanceDisplayLabel, OrXGUISkin.box))
                                                {
                                                    if (!mph)
                                                    {
                                                        speedDisplayLabel = "mph";
                                                        speedDisplayMod = 2.23694f;
                                                        distanceDisplayLabel = "mi";
                                                        distanceDisplayMod = 0.000621f;
                                                        mph = true;
                                                    }
                                                    else
                                                    {
                                                        speedDisplayLabel = "kph";
                                                        speedDisplayMod = 3.6f;
                                                        distanceDisplayLabel = "km";
                                                        distanceDisplayMod = 0.001f;
                                                        mph = false;
                                                    }
                                                }

                                                line++;

                                                //if (GUI.Button(new Rect(12, ContentTop + (line + 0.3f) * entryHeight, 40, 18), "set", OrXGUISkin.box)) { };

                                                GUI.Label(new Rect(39, ContentTop + (line + 0.3f) * entryHeight, WindowWidth / 2, 20), "Current Speed: ", titleStyleMedNoItal);
                                                GUI.Label(new Rect(WindowWidth / 2, ContentTop + (line + 0.3f) * entryHeight, WindowWidth / 2, 20), "" + Math.Round((FlightGlobals.ActiveVessel.horizontalSrfSpeed * speedDisplayMod), 0), titleStyleMedGreen);

                                                if (GUI.Button(new Rect(WindowWidth - 50, ContentTop + (line + 0.3f) * entryHeight, 35, 18), speedDisplayLabel, OrXGUISkin.box))
                                                {
                                                    if (!mph)
                                                    {
                                                        speedDisplayLabel = "mph";
                                                        speedDisplayMod = 2.23694f;
                                                        distanceDisplayLabel = "mi";
                                                        distanceDisplayMod = 0.000621f;
                                                        mph = true;
                                                    }
                                                    else
                                                    {
                                                        speedDisplayLabel = "kph";
                                                        speedDisplayMod = 3.6f;
                                                        distanceDisplayLabel = "km";
                                                        distanceDisplayMod = 0.001f;
                                                        mph = false;
                                                    }
                                                }


                                                /*
                                                if (OrXLog.instance._debugLog)
                                                {
                                                    line++;
                                                    line += 0.2f;

                                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "RESET FLOATING POINT", OrXGUISkin.button))
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX RESET FLOATING POINT] === RESET FLOATING POINT ===");
                                                        FloatingOrigin.SetOffset(new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                                                    }
                                                }
                                                */
                                            }
                                            else
                                            {
                                                GUI.Label(new Rect(30, 1, WindowWidth / 2, 20), "HoloKron Distance: ", titleStyleMedNoItal);
                                                GUI.Label(new Rect(WindowWidth / 2, 1, WindowWidth / 2, 20), "" + Math.Round((targetDistance * distanceDisplayMod), 1), titleStyleMedGreen);
                                                if (GUI.Button(new Rect(WindowWidth - 45, 2, 35, 18), distanceDisplayLabel, OrXGUISkin.box))
                                                {
                                                }

                                                line++;
                                                line += 0.2f;
                                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "STOP SCANNING", OrXGUISkin.button))
                                                {
                                                    if (OrXLog.instance.mission)
                                                    {
                                                        mrKleen = true;
                                                    }
                                                    else
                                                    {
                                                        challengeRunning = false;
                                                        OnScrnMsgUC("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
                                                        locCount = 0;
                                                        locAdded = false;
                                                        building = false;
                                                        buildingMission = false;
                                                        addCoords = false;
                                                        MainMenu();
                                                        ResetData();
                                                    }
                                                }
                                            }
                                        }

                                        if (stopwatch)
                                        {
                                            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Stop Watch", titleStyleMedNoItal);
                                            line += 0.3f;
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), _timerTotalTime, titleStyleMedGreen);
                                            line++;
                                            line += 0.2f;
                                            if (!_timing)
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Start Timer", OrXGUISkin.button))
                                                {
                                                    _timing = true;
                                                    _timerOn = true;
                                                    StartCoroutine(StageTimer());
                                                    OrXLog.instance.DebugLog("[OrX Stop Watch] === START ===");
                                                }
                                            }
                                            else
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Stop Timer", OrXGUISkin.box))
                                                {
                                                    _timing = false;
                                                    _timerOn = false;
                                                    OrXLog.instance.DebugLog("[OrX Stop Watch] === STOP ===");
                                                }
                                            }

                                            line++;
                                            line += 0.2f;

                                            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), "Reset", OrXGUISkin.button))
                                            {
                                                _timing = false;
                                                _timerOn = false;
                                                _time = 0;
                                                missionTime = 0;
                                                _timerStageTime = "00:00:00.00";
                                                _timerTotalTime = "00:00:00.00";
                                                OrXLog.instance.DebugLog("[OrX Stop Watch] === RESET ===");
                                            }

                                            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), "Close", OrXGUISkin.button))
                                            {
                                                _timing = false;
                                                _timerOn = false;
                                                _time = 0;
                                                missionTime = 0;
                                                _timerStageTime = "00:00:00.00";
                                                _timerTotalTime = "00:00:00.00";
                                                OrXLog.instance.DebugLog("[OrX Stop Watch] === EXIT ===");
                                                stopwatch = false;
                                                MainMenu();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Reaching into the Outer Limits ...", titleStyleMedGreen);
                                    line++;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "... Please stand By ...", titleStyleMedGreen);
                                }
                            }
                        }
                        else
                        {
                            if (PlayOrXMission)
                            {
                                if (showScores)
                                {
                                    GUI.Label(new Rect(0, 0, WindowWidth, 20), HoloKronName, titleStyleLarge);
                                    line++;
                                    line += 0.2f;

                                    if (!_extractScoreboard)
                                    {
                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Challenger", titleStyleMedGreen);
                                        if (challengeType == "BD ARMORY")
                                        {
                                            GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Salt", titleStyleMedGreen);
                                        }
                                        else
                                        {
                                            GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Total Time", titleStyleMedGreen);
                                        }
                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth / 2, 20), "____________", titleStyleMedGreen);
                                        GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), "____________", titleStyleMedGreen);
                                        line++;
                                        line += 0.3f;
                                        DrawScoreboard0(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard1(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard2(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard3(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard4(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard5(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard6(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard7(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard8(line);
                                        line++;
                                        line += 0.1f;
                                        DrawScoreboard9(line);
                                        line++;
                                        line++;
                                        DrawImportScores(line);
                                        line += 0.2f;
                                        line++;
                                        DrawImportScoreboard(line);
                                        line += 0.2f;
                                        line++;
                                        DrawExtractScoreboard(line);
                                        //line++;
                                        line += 0.4f;
                                        //if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLEAR SCOREBOARD", OrXGUISkin.button))
                                        //{
                                        //    ResetScoreboard();
                                        //}

                                        if (outlawRacing && HighLogic.LoadedSceneIsFlight)
                                        {
                                            line++;
                                            line += 0.2f;

                                            if (!_scoreSaved)
                                            {
                                                if (_returnToSender)
                                                {
                                                    DrawStart(line);
                                                }
                                            }
                                            else
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "RETURN TO START", OrXGUISkin.button))
                                                {
                                                    ReturnToSender(new Vector3d(_HoloKron.latitude, _HoloKron.longitude, _HoloKron.altitude));
                                                }
                                            }
                                        }
                                        line += 0.2f;
                                        line++;
                                        DrawCloseScoreboard(line);
                                        line += 0.2f;
                                    }
                                    else
                                    {
                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Enter the HoloKron password below", titleStyleMedYellow);
                                        line++;
                                        line += 0.2f;
                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "to extract the scoreboard", titleStyleMedYellow);
                                        line++;
                                        line++;

                                        DrawPassword(line);

                                        line++;
                                        line += 0.2f;

                                        DrawExtractScoreboard(line);
                                        line++;
                                        line += 0.5f;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                                        {
                                            _extractScoreboard = false;
                                        }
                                        line += 0.2f;
                                    }
                                }
                                else
                                {
                                    if (modCheckFail)
                                    {
                                        int scrollIndex = 0;
                                        //int pmScrollIndex = 0;
                                        GUI.Label(new Rect(0, 0, WindowWidth, 20), "Missing Mods List", titleStyleLarge);
                                        line++;
                                        scrollPosition = GUI.BeginScrollView(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 140), scrollPosition, new Rect(15, 0, WindowWidth - 30, _orxFileMods.Count * 20));
                                        List<string>.Enumerator _modList = _orxFileMods.GetEnumerator();
                                        while (_modList.MoveNext())
                                        {
                                            if (_modList.Current != null)
                                            {

                                                GUI.Label(new Rect(0, scrollIndex * 20, WindowWidth, 20), _modList.Current, titleStyleMedYellow);
                                                scrollIndex += 1;
                                            }
                                        }
                                        _modList.Dispose();
                                        GUI.EndScrollView();
                                        if (scrollIndex >= 7)
                                        {
                                            line += 7;
                                        }
                                        else
                                        {
                                            line += scrollIndex;
                                        }
                                        
                                        line += 0.5f;


                                        /*
                                         * int scrollIndex2 = 0;
                                        GUI.Label(new Rect(0, ContentTop + (line * entryHeight), WindowWidth, 20), "Missing Part Modules", titleStyle);
                                        line++;
                                        scrollPosition2 = GUI.BeginScrollView(new Rect(5, ContentTop + (line * entryHeight), WindowWidth - 10, 100), scrollPosition2, new Rect(10, 0, WindowWidth - 20, 200));

                                        List<string>.Enumerator _leftovers = _orxFilePartModules.GetEnumerator();
                                        while (_leftovers.MoveNext())
                                        {
                                            if (_leftovers.Current != null)
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), _leftovers.Current, OrXGUISkin.box))
                                                {
                                                }
                                                line++;
                                                line += 0.2f;
                                            }
                                        }
                                        _leftovers.Dispose();
                                        GUI.EndScrollView();
                                        line++;
                                        line += 0.2f;

                                        */
                                        /*
                                        GUI.Label(new Rect(0, ContentTop + (line * entryHeight), WindowWidth, 20), "Part Modules in HoloKron", titleStyleMedNoItal);
                                        line++;
                                        line += 0.2f;

                                        scrollPosition2 = GUI.BeginScrollView(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 140), scrollPosition2, new Rect(15, 0, WindowWidth - 30, 200));

                                        List<string>.Enumerator _leftovers = _orxFilePartModules.GetEnumerator();
                                        while (_leftovers.MoveNext())
                                        {
                                            if (_leftovers.Current != null)
                                            {
                                                GUI.Label(new Rect(0, pmScrollIndex * 20, WindowWidth, 20), _leftovers.Current, titleStyleMedYellow);
                                                pmScrollIndex += 1;

                                            }
                                        }
                                        _leftovers.Dispose();
                                        GUI.EndScrollView();
                                        if (pmScrollIndex >= 7)
                                        {
                                            line += 7;
                                        }
                                        else
                                        {
                                            line += pmScrollIndex;
                                        }
                                        */
                                        line += 0.2f;
                                        GUI.Label(new Rect(0, ContentTop + (line * entryHeight), WindowWidth, 20), "If you Continue without all the", titleStyle);
                                        line++;

                                        GUI.Label(new Rect(0, ContentTop + (line * entryHeight), WindowWidth, 20), "mods listed it may break your game", titleStyle);

                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "It's Fine, Continue", OrXGUISkin.button))
                                        {
                                            Reach();
                                            modCheckFail = false;
                                            if (showCreatedHolokrons)
                                            {
                                                StartCoroutine(OpenHoloKronRoutine(geoCache, HoloKronName, hkCount, null, null));
                                            }
                                            else
                                            {
                                                if (geoCache || shortTrackRacing)
                                                {
                                                    OrXSpawnHoloKron.instance.SpawnLocal(false, HoloKronName, new Vector3d(), OrXTargetDistance.instance._wmActivateDelay, 0);
                                                }
                                            }
                                        }

                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                                        {
                                            if (showCreatedHolokrons)
                                            {
                                                GuiEnabledOrXMissions = false;
                                            }
                                            modCheckFail = false;
                                        }
                                    }
                                    else
                                    {
                                        if (_deleteWarning)
                                        {
                                            GUI.Label(new Rect(0, 1, WindowWidth, 20), "- WARNING -", titleStyleLarge);
                                            line += 0.4f;
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "This will remove the OrX file", titleStyleMedGreen);
                                            line++;
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "and all the data it contains", titleStyleMedGreen);
                                            line++;
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "from OrX Kontinuum ", titleStyleMedGreen);
                                            line++;
                                            line += 0.2f;

                                            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "DELETE HOLOKRON", OrXGUISkin.button))
                                            {
                                                Reach();
                                                _deleteWarning = false;
                                                OrXUtilities.instance.DeleteHoloKron(groupName, HoloKronName);
                                            }
                                            line += 0.2f;
                                            line++;

                                            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                                            {
                                                _deleteWarning = false;
                                            }
                                        }
                                        else
                                        {
                                            GUI.Label(new Rect(0, 0, WindowWidth, 20), HoloKronName, titleStyleLarge);
                                            line += 0.2f;
                                            line++;
                                            GUI.Label(new Rect(20, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron Type: ", titleStyleMedBooger);
                                            GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), missionType, titleStyle);

                                            line++;

                                            if (!geoCache)
                                            {
                                                GUI.Label(new Rect(20, ContentTop + line * entryHeight, WindowWidth, 20), "Challenge Type: ", titleStyleMedBooger);
                                                GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), challengeType, titleStyle);

                                                line++;
                                                if (challengeType == "OUTLAW RACING")
                                                {
                                                    line += 0.2f;

                                                    GUI.Label(new Rect(20, ContentTop + line * entryHeight, WindowWidth / 2 - 10, 20), "Race Type: ", titleStyleMedBooger);
                                                    GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), raceType, titleStyle);

                                                    line++;
                                                    line += 0.2f;
                                                    GUI.Label(new Rect(20, ContentTop + line * entryHeight, WindowWidth / 2 - 10, 20), "Stage Gates: ", titleStyleMedBooger);
                                                    GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), CoordDatabase.Count.ToString(), titleStyle);

                                                    line++;
                                                    line += 0.3f;

                                                }
                                            }

                                            if (blueprintsAdded)
                                            {
                                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Blueprints Available", titleStyleMedGreen);
                                                line++;
                                                line += 0.2f;

                                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), crafttosave, titleStyle);
                                                line++;
                                                line += 0.2f;
                                            }

                                            if (OrXSpawnHoloKron.instance._interceptorCount != 0)
                                            {
                                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Interceptors: " + OrXSpawnHoloKron.instance._interceptorCount, titleStyleYellow);
                                                line++;
                                                line += 0.2f;
                                                List<string>.Enumerator _interceptorList = OrXSpawnHoloKron.instance._interceptorNameList.GetEnumerator();
                                                while (_interceptorList.MoveNext())
                                                {
                                                    if (_interceptorList.Current != null)
                                                    {
                                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), _interceptorList.Current, titleStyle);
                                                        line++;
                                                    }
                                                }
                                                _interceptorList.Dispose();
                                            }
                                            line++;
                                            line += 0.5f;
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron Details", titleStyleMedGreen);
                                            line += 0.5f;
                                            DrawDescription0(line);
                                            line++;
                                            if (_missionDescription1_)
                                            {
                                                DrawDescription1(line);
                                                line++;

                                                if (_missionDescription2_)
                                                {
                                                    DrawDescription2(line);
                                                    line++;

                                                    if (_missionDescription3_)
                                                    {
                                                        DrawDescription3(line);
                                                        line++;

                                                        if (_missionDescription4_)
                                                        {
                                                            DrawDescription4(line);
                                                            line++;

                                                            if (_missionDescription5_)
                                                            {
                                                                DrawDescription5(line);
                                                                line++;

                                                                if (_missionDescription6_)
                                                                {
                                                                    DrawDescription6(line);
                                                                    line++;

                                                                    if (_missionDescription7_)
                                                                    {
                                                                        DrawDescription7(line);
                                                                        line++;

                                                                        if (_missionDescription8_)
                                                                        {
                                                                            DrawDescription8(line);
                                                                            line++;

                                                                            if (_missionDescription9_)
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
                                            if (!geoCache)
                                            {
                                                if (HighLogic.LoadedSceneIsFlight)
                                                {
                                                    line++;
                                                    line += 0.2f;
                                                    DrawChallengerName(line);
                                                    line++;
                                                    line++;

                                                    if (!_editor)
                                                    {
                                                        if (bdaChallenge)
                                                        {
                                                            //DrawKontinuumLogin1(line);
                                                            //line++;
                                                            //line += 0.2f;
                                                        }

                                                        if (holoOpen)
                                                        {
                                                            if (raceType == "SHORT TRACK")
                                                            {
                                                                DrawSpawnChallenge(line);
                                                                line++;
                                                                line += 0.7f;
                                                                if (_scoreSaved)
                                                                {
                                                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "GET NEW CRAFT", OrXGUISkin.button))
                                                                    {
                                                                        SpawnByOrX();
                                                                    }
                                                                }
                                                                line++;
                                                                line += 0.2f;
                                                            }
                                                        }
                                                    }
                                                }

                                                DrawShowScoreboard(line);

                                                if (!_editor)
                                                {
                                                    line++;
                                                    line += 0.2f;

                                                    DrawCancel(line);
                                                }
                                            }
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                line += 0.2f;
                                                line++;
                                                DrawStart(line);
                                            }
                                            /*
                                            if (outlawRacing)
                                            {
                                                line += 0.2f;
                                                line++;
                                                if (GUI.Button(new Rect(10, ContentTop + (line + 0.4f) * entryHeight, WindowWidth - 20, 20), "TAKE ME THERE", OrXGUISkin.button))
                                                {
                                                    
                                                    triggerVessel = FlightGlobals.ActiveVessel;
                                                    ReturnToSender(new Vector3d(latMission, lonMission, altMission));
                                                }
                                            }
                                            */
                                            if (_editor)
                                            {
                                                line++;
                                                line += 0.2f;
                                                DrawCancel(line);
                                            }
                                            line += 0.8f;
                                            line++;
                                            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "DELETE HOLOKRON", OrXGUISkin.button))
                                            {
                                                _deleteWarning = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (addCoords)
                                {
                                    GUI.Label(new Rect(0, 0, WindowWidth, 20), "Co-ordinate Editor", titleStyleLarge);
                                }
                                else
                                {
                                    if (!_spawningVessel)
                                    {
                                        GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX HoloKron Creator", titleStyleLarge);
                                    }
                                    else
                                    {
                                        GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Spawn Menu", titleStyleLarge);
                                    }
                                }
                                //line++;

                                if (!addCoords)
                                {
                                    if (!_spawningVessel)
                                    {
                                        DrawCreationName(line);
                                        line++;
                                        DrawCreatorName(line);
                                        line++;
                                        DrawHoloKronName(line);
                                        line++;
                                        DrawHoloKronName2(line);
                                        line++;
                                        DrawPassword(line);
                                        line++;
                                        line += 0.2f;
                                        DrawMissionType(line);
                                        line++;
                                        line += 0.2f;

                                        if (!geoCache)
                                        {
                                            DrawChallengeType(line);
                                            line++;
                                            line += 0.2f;
                                            if (!bdaChallenge)
                                            {
                                                DrawRaceType(line);
                                                line++;
                                                line += 0.2f;
                                            }
                                        }
                                        line++;

                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Edit the HoloKron description below", titleStyleYellow);
                                        line++;
                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),  "(Press TAB to jump to next line)", titleStyleYellow);
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
                                            line += 0.2f;

                                            DrawClearDescription(line);
                                            line++;
                                        }
                                        line += 0.6f;

                                        DrawVessel(line);

                                        line++;

                                        DrawAddBlueprints(line);
                                        line++;
                                        line += 0.2f;
                                        if (bdaChallenge || LBC)
                                        {
                                            DrawSpawnVessel(line);
                                            line++;
                                            line++;
                                            /*
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Air Interceptors", titleStyleYellow);
                                            line++;

                                            DrawAddAS1(line);
                                            line += 0.2f;
                                            line++;
                                            if (_savingAirSup1)
                                            {
                                                DrawAddAS2(line);
                                                line += 0.2f;
                                                line++;
                                                if (_savingAirSup2)
                                                {
                                                    DrawAddAS3(line);
                                                    line += 0.2f;
                                                    line++;
                                                }
                                            }
                                            */
                                        }

                                        if (geoCache)
                                        {
                                            DrawSaveLocal(line);
                                            line += 0.2f;
                                            line++;
                                            DrawLocalSaveRange(line);
                                            line++;
                                            line += 0.2f;
                                        }

                                        line += 0.5f;
                                        DrawSave(line);
                                        line += 0.2f;
                                        line++;
                                        DrawCancel(line);
                                    }
                                    else
                                    {
                                        /*
                                        GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, entryHeight), "Add Crew to Spawned Craft", titleStyleMedBooger);

                                        if (!_addCrew)
                                        {
                                            if (GUI.Button(new Rect(WindowWidth - 30, ContentTop + line * entryHeight, 20, 20), "X", OrXGUISkin.button))
                                            {
                                                _addCrew = true;
                                            }
                                        }
                                        else
                                        {
                                            if (GUI.Button(new Rect(WindowWidth - 30, ContentTop + line * entryHeight, 20, 20), "X", OrXGUISkin.box))
                                            {
                                                _addCrew = false;
                                            }
                                        }
                                        line += 0.2f;
                                        line++;
                                        */
                                        //line++;

                                        DrawSpawnVessel(line);
                                        line += 0.2f;
                                        line++;
                                        if (LBC)
                                        {
                                            DrawSpawnOrX(line);
                                            line++;
                                            line += 0.2f;
                                        }

                                        DrawSpawnBarrier(line);

                                        if (!triggerVessel.isActiveVessel)
                                        {
                                            line++;
                                            line++;
                                            if (OrXVesselMove.Instance.MovingVessel == FlightGlobals.ActiveVessel)
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "PLACE FOCUSED", OrXGUISkin.button))
                                                {
                                                    OrXVesselMove.Instance.KillMove(true, false);
                                                }
                                            }
                                            else
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "MOVE FOCUSED", OrXGUISkin.button))
                                                {
                                                    if (!FlightGlobals.ActiveVessel.rootPart.Modules.Contains<ModuleOrXStage>() && !triggerVessel.isActiveVessel)
                                                    {
                                                        OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 10, false, false, new Vector3d());
                                                    }
                                                }
                                            }
                                            line++;
                                            line++;

                                            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "KILL CRAFT", OrXGUISkin.button))
                                            {
                                                FlightGlobals.ActiveVessel.rootPart.AddModule("ModuleOrXJason", true);
                                                _holdVesselPos = false;
                                                _settingAltitude = false;
                                                PlayOrXMission = false;
                                                addCoords = false;
                                                movingCraft = false;
                                                _spawningVessel = true;
                                                FlightGlobals.ForceSetActiveVessel(OrXHoloKron.instance.triggerVessel);

                                            }
                                        }
                                        line++;
                                        line++;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                                        {
                                            if (LBC && CoordDatabase.Count >= 0)
                                            {
                                                Reach();
                                                if (!_HoloKron.isActiveVessel)
                                                {
                                                    FlightGlobals.ForceSetActiveVessel(_HoloKron);
                                                }

                                                spawningStartGate = false;
                                                _spawningVessel = false;
                                                getNextCoord = true;
                                            }
                                            else
                                            {
                                                _spawningVessel = false;
                                                _getCenterDist = false;
                                                _addCrew = false;
                                                _holdVesselPos = false;
                                                if (!triggerVessel.isActiveVessel)
                                                {
                                                    FlightGlobals.ForceSetActiveVessel(triggerVessel);
                                                }
                                            }
                                        }
                                        line++;
                                    }
                                }
                                else
                                {
                                    line += 0.1f;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Start Location Distance: " + Math.Round((targetDistance / 1000), 2) + " km", rangeColor);
                                    line++;
                                    line += 0.1f;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Stage Count: " + locCount, titleStyleMedGreen);
                                    line++;
                                    line += 0.1f;
                                    DrawAddCoords(line);
                                    line++;
                                    line += 0.6f;
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CANCEL AND EXIT", OrXGUISkin.button))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Mission] === CANCEL HOLOKRON CREATION ===");
                                        if (_HoloKron != null)
                                        {
                                            if (_HoloKron.rootPart.Modules.Contains<ModuleOrXMission>())
                                            {
                                                _HoloKron.rootPart.explode();
                                            }
                                        }
                                        CancelChallenge();
                                    }
                                    line++;
                                    line += 0.2f;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!challengeRunning)
                        {
                            if (checking)
                            {
                                if (targetDistance <= 100000)
                                {
                                    GUI.Label(new Rect(30, 1, WindowWidth / 2, 20), "HoloKron Distance: ", titleStyleMedNoItal);
                                    GUI.Label(new Rect(WindowWidth / 2, 1, WindowWidth / 2, 20), "" + Math.Round((targetDistance * distanceDisplayMod), 1), titleStyleMedGreen);
                                    if (GUI.Button(new Rect(WindowWidth - 45, 2, 35, 18), distanceDisplayLabel, OrXGUISkin.box))
                                    {
                                    }

                                    if (bdaChallenge)
                                    {
                                        line++;
                                        line += 0.2f;
                                        if (targetDistance <= 60000)
                                        {
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Detection Altitude: " + Math.Round((targetDistance / 100), 1) + " meters", titleStyleMedYellow);
                                        }
                                    }
                                }
                                else
                                {
                                    scanDelay = OrXTargetDistance.instance.scanDelay;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "No HoloKron in range ......", titleStyle);
                                    line++;
                                    line += 0.2f;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Rescan in " + Math.Round(scanDelay, 0) + " seconds", titleStyleMedYellow);
                                }
                                line++;
                                line += 0.2f;

                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "Stop HoloKron Scan", OrXGUISkin.button))
                                {
                                    OrXUtilities.instance.StopScan(true);
                                }
                            }
                            else
                            {
                                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum", titleStyleLarge);
                                line++;

                                if (showChallengelist)
                                {
                                    if (showCreatedHolokrons)
                                    {
                                        if (!showGeoCacheList)
                                        {
                                            float scrollIndex = 0;
                                            float _buttonAdjust = 20;
                                            if (OrXChallengeNameList.Count >= 7)
                                            {
                                                _buttonAdjust += 25;
                                            }

                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), groupName + " List", titleStyleMedGreen);
                                            line++;
                                            line += 0.2f;
                                            scrollPosition = GUI.BeginScrollView(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 140), scrollPosition, new Rect(15, 0, WindowWidth - 30, OrXChallengeNameList.Count * 24));
                                            List<string>.Enumerator holoNames = OrXChallengeNameList.GetEnumerator();
                                            while (holoNames.MoveNext())
                                            {
                                                if (holoNames.Current != null)
                                                {
                                                    if (GUI.Button(new Rect(15, scrollIndex * 24, WindowWidth - _buttonAdjust, 20), holoNames.Current, OrXGUISkin.button))
                                                    {
                                                        hkCount = 0;
                                                        HoloKronName = holoNames.Current;
                                                        /*
                                                        if (holoNames.Current.Contains("-"))
                                                        {
                                                            string[] data = holoNames.Current.Split(new char[] { '-' });
                                                            hkCount = int.Parse(data[1]);
                                                            HoloKronName = data[0];
                                                        }
                                                        else
                                                        {
                                                            HoloKronName = holoNames.Current;
                                                        }
                                                        */
                                                        Debug.Log("[OrX Local System Challenges] Opening OrX HoloKron for " + holoNames.Current);

                                                        List<string>.Enumerator orxChallengeList = OrXChallengeList.GetEnumerator();
                                                        while (orxChallengeList.MoveNext())
                                                        {
                                                            if (orxChallengeList.Current != null)
                                                            {
                                                                string[] data2 = orxChallengeList.Current.Split(new char[] { ',' });
                                                                if (HoloKronName == data2[1])
                                                                {
                                                                    Debug.Log("[OrX Local System Challenges] Found orx file for " + HoloKronName);

                                                                    if (data2[6] == hkCount.ToString())
                                                                    {
                                                                        Debug.Log("[OrX Local System Challenges] orx file HoloKron count matches ... Loading " + HoloKronName + " " + hkCount);
                                                                        if (disablePRE)
                                                                        {
                                                                            //OrXPRExtension.PreOff("OrX Kontinuum");
                                                                        }
                                                                        Reach();
                                                                        IronKerbal = false;
                                                                        salt = 0;
                                                                        missionType = data2[7];
                                                                        challengeType = data2[8];
                                                                        showGeoCacheList = false;
                                                                        showCreatedHolokrons = false;
                                                                        showChallengelist = false;
                                                                        HoloKronName = data2[1];
                                                                        groupName = data2[2];
                                                                        _editor = true;
                                                                        latMission = double.Parse(data2[3]);
                                                                        lonMission = double.Parse(data2[4]);
                                                                        altMission = double.Parse(data2[5]);
                                                                        Debug.Log("[OrX Local System Challenges] Lat: " + latMission + " - Lon: " + lonMission + " - Alt: " + altMission);
                                                                        if (HighLogic.LoadedSceneIsFlight)
                                                                        {
                                                                            worldPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission);
                                                                            OrXVesselLog.instance._enemyCraft = new List<Vessel>();
                                                                            OrXVesselLog.instance._playerCraft = new List<Vessel>();
                                                                            triggerVessel = FlightGlobals.ActiveVessel;
                                                                            _challengeStartLoc = new Vector3d(latMission, lonMission, altMission);
                                                                            OrXLog.instance.SetRange(FlightGlobals.ActiveVessel, 75000);

                                                                        }
                                                                        if (data2[7] != "GEO-CACHE")
                                                                        {
                                                                            geoCache = false;
                                                                        }
                                                                        if (challengeType == "OUTLAW RACING")
                                                                        { outlawRacing = true; }
                                                                        if (challengeType == "BD ARMORY")
                                                                        { bdaChallenge = true; }
                                                                        if (challengeType == "LBC")
                                                                        { LBC = true; }

                                                                        OrXUtilities.instance.GetInstalledMods(false);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        orxChallengeList.Dispose();
                                                    }
                                                    scrollIndex += 1;
                                                }
                                            }
                                            holoNames.Dispose();
                                            GUI.EndScrollView();
                                            line += 0.2f;

                                            if (scrollIndex >= 7)
                                            {
                                                line += 7;
                                            }
                                            else
                                            {
                                                line += scrollIndex;
                                            }
                                        }
                                        else
                                        {
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Local System HoloKrons", titleStyleMedYellow);
                                            line++;

                                            List<string>.Enumerator holoNames = OrXGeoCacheNameList.GetEnumerator();
                                            while (holoNames.MoveNext())
                                            {
                                                if (holoNames.Current != null)
                                                {
                                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), holoNames.Current, OrXGUISkin.button))
                                                    {
                                                        if (HighLogic.LoadedSceneIsFlight)
                                                        {
                                                            hkCount = 0;

                                                            HoloKronName = holoNames.Current;
                                                            Debug.Log("[OrX Local System Challenges] Opening OrX HoloKron Challenge for " + holoNames.Current);

                                                            List<string>.Enumerator orxCoordsList = OrXCoordsList.GetEnumerator();
                                                            while (orxCoordsList.MoveNext())
                                                            {
                                                                if (orxCoordsList.Current != null)
                                                                {
                                                                    string[] data2 = orxCoordsList.Current.Split(new char[] { ',' });
                                                                    if (HoloKronName == data2[1])
                                                                    {
                                                                        Debug.Log("[OrX Local System Challenges] Found orx file for " + HoloKronName);

                                                                        if (data2[6] == hkCount.ToString())
                                                                        {
                                                                            Debug.Log("[OrX Local System Challenges] orx file HoloKron count matches ... Loading " + HoloKronName + " " + hkCount);
                                                                            if (disablePRE)
                                                                            {
                                                                            }
                                                                            Reach();
                                                                            coordCount = 0;
                                                                            showGeoCacheList = false;
                                                                            showCreatedHolokrons = false;
                                                                            showChallengelist = false;
                                                                            checking = true;
                                                                            challengeRunning = true;
                                                                            HoloKronName = data2[1];
                                                                            groupName = data2[2];

                                                                            //OrXTargetDistance.instance.TargetDistance(true, true, false, true, HoloKronName, new Vector3d(double.Parse(data2[3]), double.Parse(data2[4]), double.Parse(data2[5])));
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            orxCoordsList.Dispose();
                                                        }
                                                        else
                                                        {

                                                        }
                                                    }
                                                    line++;
                                                    line += 0.1f;
                                                }
                                            }
                                            holoNames.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        if (showGeoCacheList)
                                        {
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Geo-Cache Groups", titleStyleMedBooger);
                                            line++;

                                            List<string>.Enumerator creatorNames = OrXGeoCacheCreatorList.GetEnumerator();
                                            while (creatorNames.MoveNext())
                                            {
                                                if (creatorNames.Current != null)
                                                {
                                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), creatorNames.Current, OrXGUISkin.button))
                                                    {
                                                        if (HighLogic.LoadedSceneIsFlight)
                                                        {
                                                            Reach();
                                                            groupName = creatorNames.Current;
                                                            OrXUtilities.instance.GrabCreations(creatorNames.Current, false);
                                                        }
                                                    }
                                                    line++;
                                                    line += 0.2f;
                                                }
                                            }
                                            creatorNames.Dispose();
                                            // GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Kontinuum HoloKrons", titleStyleMed);
                                            //line++;
                                            //GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "<currently unavailable>", titleStyle);

                                        }
                                        else
                                        {
                                            line += 0.5f;
                                            float scrollIndex = 0;
                                            float _buttonAdjust = 20;
                                            if (OrXChallengeCreatorList.Count >= 7)
                                            {
                                                _buttonAdjust += 25;
                                            }

                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron Group List", titleStyleMedGreen);
                                            line++;
                                            line += 0.2f;
                                            scrollPosition = GUI.BeginScrollView(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 140), scrollPosition, new Rect(15, 0, WindowWidth - 30, OrXChallengeCreatorList.Count * 24));
                                            List<string>.Enumerator _creatorList = OrXChallengeCreatorList.GetEnumerator();
                                            while (_creatorList.MoveNext())
                                            {
                                                if (_creatorList.Current != null)
                                                {
                                                    if (GUI.Button(new Rect(15, scrollIndex * 24, WindowWidth - _buttonAdjust, 20), _creatorList.Current, OrXGUISkin.button))
                                                    {
                                                        if (OrXLog.instance._preInstalled)
                                                        {
                                                            if (!OrXLog.instance.PREnabled())
                                                            {
                                                                groupName = _creatorList.Current;
                                                                OrXUtilities.instance.GrabCreations(_creatorList.Current, true);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            groupName = _creatorList.Current;
                                                            OrXUtilities.instance.GrabCreations(_creatorList.Current, true);
                                                        }
                                                        OrXSounds.instance.KnowMore();
                                                    }
                                                    scrollIndex += 1f;
                                                }
                                            }
                                            _creatorList.Dispose();
                                            GUI.EndScrollView();
                                            if (scrollIndex >= 7)
                                            {
                                                line += 7;
                                            }
                                            else
                                            {
                                                line += scrollIndex;
                                            }
                                        }
                                    }
                                    line++;

                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Previous Menu", OrXGUISkin.button))
                                    {
                                        if (showCreatedHolokrons)
                                        {
                                            showCreatedHolokrons = false;
                                        }
                                        else
                                        {
                                            showGeoCacheList = false;
                                            showChallengelist = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Kontinuum Konnect", OrXGUISkin.button))
                                    {
                                        if (OrXLog.instance._preInstalled)
                                        {
                                            if (!OrXLog.instance.PREnabled())
                                            {
                                                if (OrXLog.instance._debugLog && _pKarma == Karma)
                                                {
                                                    connectToKontinuum = true;
                                                    OnScrnMsgUC("The matrix has you " + challengersName + " ....");
                                                }
                                                else
                                                {
                                                    OnScrnMsgUC("The Kontinuum is currently unavailable ....");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (OrXLog.instance._debugLog && _pKarma == Karma)
                                            {
                                                connectToKontinuum = true;
                                                OnScrnMsgUC("The matrix has you " + challengersName + " ....");
                                            }
                                            else
                                            {
                                                OnScrnMsgUC("The Kontinuum is currently unavailable ....");
                                            }
                                        }

                                    }
                                    line++;
                                    line += 0.2f;
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Scan for Geo-Cache", OrXGUISkin.button))
                                    {
                                        Reach();
                                        OrXUtilities.instance.ProcessHoloFiles();
                                    }
                                    line++;
                                    line += 0.2f;

                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "HoloKron Group List", OrXGUISkin.button))
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            if (OrXLog.instance._preInstalled)
                                            {
                                                if (!OrXLog.instance.PREnabled())
                                                {
                                                    OrXUtilities.instance.LoadData();
                                                }
                                            }
                                            else
                                            {
                                                OrXUtilities.instance.LoadData();
                                            }
                                        }
                                        else
                                        {
                                            OrXUtilities.instance.LoadData();
                                        }
                                    }
                                    line++;
                                    line += 0.2f;
                                    if (HighLogic.LoadedSceneIsFlight)
                                    {
                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Create HoloKron", OrXGUISkin.button))
                                        {
                                            if (OrXLog.instance._preInstalled)
                                            {
                                                if (!OrXLog.instance.PREnabled())
                                                {
                                                    ResetData();
                                                    triggerVessel = FlightGlobals.ActiveVessel;
                                                    OrXLog.instance.SetRange(FlightGlobals.ActiveVessel, 10000);
                                                    _challengeStartLoc = new Vector3d(triggerVessel.latitude, triggerVessel.longitude, triggerVessel.altitude);
                                                    SetupHolo(null, _challengeStartLoc);
                                                }
                                            }
                                            else
                                            {
                                                ResetData();
                                                triggerVessel = FlightGlobals.ActiveVessel;
                                                OrXLog.instance.SetRange(FlightGlobals.ActiveVessel, 10000);
                                                _challengeStartLoc = new Vector3d(triggerVessel.latitude, triggerVessel.longitude, triggerVessel.altitude);
                                                SetupHolo(null, _challengeStartLoc);
                                            }
                                        }

                                        line++;
                                        line += 0.2f;
                                    }
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Stop Watch", OrXGUISkin.button))
                                    {
                                        ShowStopWatch();
                                    }
                                    line++;
                                    line += 0.2f;
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Settings", OrXGUISkin.button))
                                    {
                                        _showSettings = true;
                                    }
                                    line++;
                                    line += 0.2f;

                                    if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Close Menu", OrXGUISkin.button))
                                    {
                                        OrXHCGUIEnabled = false;
                                        ResetData();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!mrKleen)
                            {
                                GUI.Label(new Rect(30, 1, WindowWidth / 2, 20), "HoloKron Distance: ", titleStyleMedNoItal);
                                GUI.Label(new Rect(WindowWidth / 2, 1, WindowWidth / 2, 20), "" + Math.Round((targetDistance * distanceDisplayMod), 1), titleStyleMedGreen);
                                if (GUI.Button(new Rect(WindowWidth - 45, 2, 35, 18), distanceDisplayLabel, OrXGUISkin.box))
                                {
                                }
                                line += 0.5f;
                                if (bdaChallenge)
                                {
                                    if (targetDistance <= 60000)
                                    {
                                        GUI.Label(new Rect(20, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Detection Altitude: ", titleStyleMedNoItal);
                                        GUI.Label(new Rect(WindowWidth / 2 + 10, ContentTop + line * entryHeight, WindowWidth / 2, 20), Math.Round((targetDistance / 100), 0) + " m", titleStyleMedGreen);
                                    }
                                    line++;
                                    line += 0.3f;
                                }
                                line += 0.4f;

                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "STOP SCANNING", OrXGUISkin.button))
                                {
                                    if (OrXLog.instance.mission)
                                    {
                                        mrKleen = true;
                                    }
                                    else
                                    {
                                        challengeRunning = false;
                                        OnScrnMsgUC("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
                                        locCount = 0;
                                        locAdded = false;
                                        building = false;
                                        buildingMission = false;
                                        addCoords = false;
                                        MainMenu();
                                        ResetData();
                                    }
                                }
                            }
                            else
                            {
                                line++;
                                line += 0.2f;

                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Clean up your mess", OrXGUISkin.button))
                                {
                                    OrXUtilities.instance.StartMrKleen();
                                }
                                line++;
                                line += 0.2f;

                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Exit Challenge", OrXGUISkin.button))
                                {
                                    challengeRunning = false;
                                    OnScrnMsgUC("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
                                    locCount = 0;
                                    locAdded = false;
                                    building = false;
                                    buildingMission = false;
                                    addCoords = false;
                                    GuiEnabledOrXMissions = false;
                                    OrXHCGUIEnabled = false;
                                    PlayOrXMission = false;
                                    checking = false;
                                    ResetData();
                                }
                            }
                        }
                    }
                }
            }

            if (!_showMode)
            {
                if (_showSettings)
                {
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(50, ContentTop + line * entryHeight, 200, entryHeight), "Debug Logging", leftLabelBooger);

                    if (!OrXLog.instance._debugLog)
                    {
                        if (GUI.Button(new Rect(WindowWidth - 30, ContentTop + (line * entryHeight), 20, 20), "X", OrXGUISkin.button))
                        {
                            OrXLog.instance._mode = true;
                            _settings0 = true;
                            OrXLog.instance._debugLog = true;
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(WindowWidth - 30, ContentTop + (line * entryHeight), 20, 20), "X", OrXGUISkin.box))
                        {
                            OrXLog.instance._mode = false;
                            _settings0 = false;
                            OrXLog.instance._debugLog = false;
                        }
                    }
                }

                if (!_showTimer && !showChallengelist)
                {
                    if (!_spawningVessel && !challengeRunning)
                    {
                        line++;
                        line += 0.7f;
                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "The Kurgan Manual", OrXGUISkin.button))
                        {
                            OrXUserManual.instance.guiEnabled = true;
                        }
                    }
                }
            }

            toolWindowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            WindowRectToolbar.height = toolWindowHeight;
        }

        private void ShowStopWatch()
        {
            _time = 0;
            missionTime = 0;
            _timerStageTime = "00:00:00.00";
            _timerTotalTime = "00:00:00.00";

            GuiEnabledOrXMissions = true;
            _showSettings = false;
            connectToKontinuum = false;
            getNextCoord = false;
            movingCraft = true;
            stopwatch = true;
            outlawRacing = false;
            bdaChallenge = false;
            _showTimer = true;
        }

        bool mph = false;
        float speedDisplayMod = 3.6f;
        string speedDisplayLabel = " kph";
        float distanceDisplayMod = 0.001f;
        string distanceDisplayLabel = "km";

        #region Coords GUI

        public void DrawAddCoords(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "ADD LOCATION", OrXGUISkin.button))
            {
                if (!_killPlace)
                {
                    Reach();
                    movingCraft = true;
                    if (outlawRacing)
                    {
                        OrXVesselMove.Instance.EndMove(true, false, true);
                    }
                    else
                    {
                        if (!LBC)
                        {
                            OrXVesselMove.Instance.KillMove(false, true);
                        }
                        else
                        {
                            OrXVesselMove.Instance.EndMove(true, false, true);
                        }
                    }
                }
                else
                {
                    OnScrnMsgUC("You are too far from the start location");
                    OnScrnMsgUC("Check your distance");
                }
            }
        }
        public void DrawClearLastCoord(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "DELETE LAST", OrXGUISkin.button))
            {
                _gateKillDelay = true;
                Reach();
                StartCoroutine(GateKillDelayRoutine());
            }
        }
        public void DrawMoveToLastCoord(float line)
        {
            var saveRect = new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight);

            if (GUI.Button(saveRect, "MOVE TO GATE " + locCount, OrXGUISkin.button))
            {
                if (CoordDatabase.Count != 0)
                {
                    Reach();
                    List<string>.Enumerator _lastCoords = CoordDatabase.GetEnumerator();
                    while (_lastCoords.MoveNext())
                    {
                        if (_lastCoords.Current != null)
                        {
                            string[] _locCount = _lastCoords.Current.Split(new char[] { ',' });
                            if (_locCount[0] == locCount.ToString())
                            {
                                targetLoc = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)double.Parse(_locCount[1]), (double)double.Parse(_locCount[2]), (double)double.Parse(_locCount[3]));
                                OrXVesselMove.Instance.StartMove(_HoloKron, false, 50, false, true, new Vector3d(double.Parse(_locCount[1]), double.Parse(_locCount[2]), double.Parse(_locCount[3])));
                            }
                        }
                    }
                    _lastCoords.Dispose();
                }
            }
        }

        #endregion

        #region Scoreboard GUI

        public void DrawScoreboard0(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB0;
            }
            else
            {
                _label = timeSB0;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), nameSB0, OrXGUISkin.button))
            {
                if (nameSB0 != "<empty>")
                {
                    _importingScores = false;

                    GetStats(nameSB0, 0);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 3, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), _label, OrXGUISkin.box))
            {
                if (nameSB0 != "<empty>")
                {
                    _importingScores = false;
                    GetStats(nameSB0, 0);
                }
            }
        }
        public void DrawScoreboard1(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB1;
            }
            else
            {
                _label = timeSB1;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB1, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB1 != "<empty>")
                {
                    GetStats(nameSB1, 1);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB1 != "<empty>")
                {
                    GetStats(nameSB1, 1);
                }
            }
        }
        public void DrawScoreboard2(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB2;
            }
            else
            {
                _label = timeSB2;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB2, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB2 != "<empty>")
                {
                    GetStats(nameSB2, 2);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB2 != "<empty>")
                {
                    GetStats(nameSB2, 2);
                }
            }
        }
        public void DrawScoreboard3(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB3;
            }
            else
            {
                _label = timeSB3;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB3, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB3 != "<empty>")
                {
                    GetStats(nameSB3, 3);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB3 != "<empty>")
                {
                    GetStats(nameSB3, 3);
                }
            }
        }
        public void DrawScoreboard4(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB4;
            }
            else
            {
                _label = timeSB4;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB4, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB4 != "<empty>")
                {
                    GetStats(nameSB4, 4);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB4 != "<empty>")
                {
                    GetStats(nameSB4, 4);
                }
            }
        }
        public void DrawScoreboard5(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB5;
            }
            else
            {
                _label = timeSB5;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB5, OrXGUISkin.button))
            {
                if (nameSB5 != "<empty>")
                {
                    _importingScores = false;

                    GetStats(nameSB5, 5);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB5 != "<empty>")
                {
                    GetStats(nameSB5, 5);
                }
            }
        }
        public void DrawScoreboard6(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB6;
            }
            else
            {
                _label = timeSB6;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB6, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB6 != "<empty>")
                {
                    GetStats(nameSB6, 6);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB6 != "<empty>")
                {
                    GetStats(nameSB6, 6);
                }
            }
        }
        public void DrawScoreboard7(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB7;
            }
            else
            {
                _label = timeSB7;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB7, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB7 != "<empty>")
                {
                    GetStats(nameSB7, 7);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB7 != "<empty>")
                {
                    GetStats(nameSB7, 7);
                }
            }
        }
        public void DrawScoreboard8(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB8;
            }
            else
            {
                _label = timeSB8;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB8, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB8 != "<empty>")
                {
                    GetStats(nameSB8, 8);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB8 != "<empty>")
                {
                    GetStats(nameSB8, 8);
                }
            }
        }
        public void DrawScoreboard9(float line)
        {
            string _label = string.Empty;
            if (challengeType == "BD ARMORY")
            {
                _label = saltSB9;
            }
            else
            {
                _label = timeSB9;
            }

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB9, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB9 != "<empty>")
                {
                    GetStats(nameSB9, 9);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), _label, OrXGUISkin.box))
            {
                _importingScores = false;

                if (nameSB9 != "<empty>")
                {
                    GetStats(nameSB9, 9);
                }
            }
        }
        public void DrawExtractScoreboard(float line)
        {
            var saveRect = new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20);
            if (GUI.Button(saveRect, "EXTRACT SCOREBOARD", OrXGUISkin.button))
            {
                if (_extractScoreboard)
                {
                    if (Password == OrXLog.instance.Decrypt(pas))
                    {
                        _extractScoreboard = false;
                        Password = "OrX";
                        ExtractScoreboard(groupName, HoloKronName, hkCount);
                    }
                    else
                    {
                        OnScrnMsgUC("WRONG PASSWORD");
                    }
                }
                else
                {
                    _extractScoreboard = true;
                }
            }
        }
        public void DrawImportScores(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "IMPORT SCORES", OrXGUISkin.button))
            {
                _importingScores = true;
                StartImporting();
            }
        }
        public void DrawImportScoreboard(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "IMPORT SCOREBOARD", OrXGUISkin.button))
            {
                Reach();
                _importingScores = true;
                StartCoroutine(GetScorboardFile());
            }
        }
        public void DrawCloseScoreboard(float line)
        {
            var saveRect = new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20);
            if (GUI.Button(saveRect, "CLOSE SCOREBOARD", OrXGUISkin.button))
            {
                showScores = false;
                /*
                if (_scoreSaved)
                {
                    OrXHCGUIEnabled = false;
                    ResetData();
                    MainMenu();
                }
                */
            }
        }

        #endregion

        #region Play Mission GUI

        public void DrawChallengerName(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenger: ", leftLabelBooger);
            challengersName = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, ((WindowWidth / 3) * 2) - 10, entryHeight), challengersName);
        }
        public void DrawPlayPassword(float line)
        {
            GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, entryHeight), "Password: ", leftLabelBooger);
            Password = GUI.TextField(new Rect(10 + contentWidth - ((WindowWidth / 3) * 2) - 10, ContentTop + line * entryHeight, ((WindowWidth / 3) * 2) - 10, entryHeight), Password);
        }
        public void DrawUnlock(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "UNLOCK", OrXGUISkin.button))
            {
                if (Password == OrXLog.instance.Decrypt(pas))
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === UNLOCKING ===");

                    unlocked = true;
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === WRONG PASSWORD ===");

                    OnScrnMsgUC("WRONG PASSWORD");
                }
            }
        }
        public void DrawSpawnChallenge(float line)
        {
            if (!showScores)
            {
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SPAWN CHALLENGE", OrXGUISkin.button))
                {
                    OrXUtilities.instance.GetInstalledMods(true);
                }
            }
            else
            {
            }
        }

        public void DrawShowScoreboard(float line)
        {
            if (!showScores)
            {
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SHOW SCOREBOARD", OrXGUISkin.button))
                {
                    getNextCoord = false;
                    movingCraft = true;
                    OrXLog.instance.DebugLog("[OrX Mission] === SHOW SCOREBOARD ===");
                    StartCoroutine(GetScoreboardData());

                    if (outlawRacing)
                    {
                    }
                    else
                    {
                        if (bdaChallenge)
                        {
                        }
                        else
                        {
                            if (windRacing)
                            {
                            }
                            else
                            {
                                if (Scuba)
                                {
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
        public void DrawStart(float line)
        {
            if (!geoCache)
            {
                if (!_editor)
                {
                    if (challengersName != "" || challengersName != string.Empty)
                    {
                        string _label = "START CHALLENGE";
                        if (_scoreSaved)
                        {
                            _label = "RETRY CHALLENGE";
                        }

                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), _label, OrXGUISkin.button))
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === NAME ENTERED - STARTING ===");
                            BeginChallenge();
                        }
                    }
                    else
                    {
                        OnScrnMsgUC("Please Enter a Challenger Name");
                    }
                }
                else
                {
                    if (!HighLogic.LoadedSceneIsFlight)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", OrXGUISkin.button))
                        {
                            OrXUtilities.instance.GetCreatorList(true);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SCAN FOR HOLOKRON", OrXGUISkin.button))
                        {
                            ScanForHoloKron();
                        }
                    }
                }
            }
            else
            {
                if (!_editor)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", OrXGUISkin.button))
                    {
                        OrXLog.instance.DebugLog("[OrX Mission] === HOLO IS GEO-CACHE - CLOSING WINDOW ===");
                        MainMenu();
                        OrXLog.instance.ResetFocusKeys();
                        if (FlightGlobals.ActiveVessel != triggerVessel && triggerVessel != null)
                        {
                            FlightGlobals.ForceSetActiveVessel(triggerVessel);
                        }
                        CloseGeoCache();
                    }
                }
                else
                {
                    if (!HighLogic.LoadedSceneIsFlight)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", OrXGUISkin.button))
                        {
                            OrXUtilities.instance.GetCreatorList(true);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SCAN FOR HOLOKRON", OrXGUISkin.button))
                        {
                            ScanForHoloKron();
                        }
                    }
                }
            }
        }


        #endregion

        #region Description Window GUI

        public void DrawClearDescription(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLEAR DESCRIPTION", OrXGUISkin.button))
            {
                OrXLog.instance.DebugLog("[OrX Mission] === CLEARING DESCRIPTION ===");

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
        public void DrawDescription0(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription1(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription2(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription3(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription4(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription5(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription6(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription7(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription8(float line)
        {
            if (PlayOrXMission)
            {
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
        public void DrawDescription9(float line)
        {
            if (PlayOrXMission)
            {
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

        public void DrawCraftBrowserTitle(float line)
        {
            if (holoHangar)
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Bag of Holding", titleStyleLarge);
            }
            else
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Blueprints Browser", titleStyleLarge);
            }
        }
        public void DrawHangar(float line)
        {
            if (holoHangar)
            {
                GUI.Label(new Rect(10, ContentTop + line * entryHeight, WindowWidth, 20), "HoloCubes Available", titleStyleMedBooger);
            }
            else
            {
                var sphButton = new Rect(10, ContentTop + line * entryHeight, 120, entryHeight);
                var vabButton = new Rect(130, ContentTop + line * entryHeight, 120, entryHeight);

                if (sph)
                {
                    if (GUI.Button(sphButton, "SPH", OrXGUISkin.box))
                    {
                    }

                    if (GUI.Button(vabButton, "VAB", OrXGUISkin.button))
                    {
                        sph = false;
                    }
                }
                else
                {
                    if (GUI.Button(sphButton, "SPH", OrXGUISkin.button))
                    {
                        sph = true;
                    }

                    if (GUI.Button(vabButton, "VAB", OrXGUISkin.box))
                    {
                    }
                }
            }
        }

        #endregion

        #region Challenge Creator GUI

        public void DrawCreationName(float line)
        {
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Name your Group below", titleStyleYellow);
        }

        public void DrawCreatorName(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Group: ", leftLabelBooger);
            groupName = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, ((WindowWidth / 3) * 2) - 10, entryHeight), groupName);
        }


        public void DrawHoloKronName(float line)
        {
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Name your HoloKron below", titleStyleYellow);
        }
        public void DrawHoloKronName2(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Holo Name: ", leftLabelBooger);
            HoloKronName = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, ((WindowWidth / 3) * 2) - 10, entryHeight), HoloKronName);
        }

        public void DrawMissionType(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "HoloKron Type: ", leftLabelBooger);

            if (geoCache)
            {
                if (GUI.Button(new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight), missionType, OrXGUISkin.button))
                {
                    //locAdded = true;
                    if (!locAdded)
                    {
                        if (_OrXV == _ate)
                        {
                            OnScrnMsgUC("UNSUPPORTED VERSION OF KSP");
                            OnScrnMsgUC("Dinner Out is Cancelled .....");
                            OnScrnMsgUC("OrX Kontinuum shutting down .....");
                            OrXHCGUIEnabled = false;
                            StopAllCoroutines();
                        }
                        else
                        {
                            OnScrnMsgUC("HOLOKRON TYPE CHANGED TO CHALLENGE");
                            OrXLog.instance.DebugLog("[OrX Mission] === HOLOKRON TYPE - CHALLENGE ===");
                            challengeType = "OUTLAW RACING";
                            outlawRacing = true;
                            dakarRacing = false;
                            shortTrackRacing = true;
                            geoCache = false;
                            windRacing = false;
                            Scuba = false;
                            bdaChallenge = false;
                            missionType = "CHALLENGE";
                            raceType = "SHORT TRACK";
                        }
                    }
                    else
                    {
                        if (_OrXV == _ate)
                        {
                            OnScrnMsgUC("UNSUPPORTED VERSION OF KSP");
                            OnScrnMsgUC("Dinner Out is Cancelled .....");
                            OnScrnMsgUC("OrX Kontinuum shutting down .....");
                            OrXHCGUIEnabled = false;
                            StopAllCoroutines();
                        }
                        else
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === HOLOKRON LOCKED AS GEO-CACHE ===");
                            OnScrnMsgUC("HOLOKRON TYPE LOCKED AS GEO-CACHE");
                            geoCache = true;
                        }
                    }
                }
            }
            else
            {
                if (GUI.Button(new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight), missionType, OrXGUISkin.button))
                {
                    locAdded = false;
                    if (!locAdded)
                    {
                        OnScrnMsgUC("HOLOKRON TYPE CHANGED TO GEO-CACHE");
                        OrXLog.instance.DebugLog("[OrX Mission] === HOLOKRON TYPE - GEO-CACHE ===");
                        missionType = "GEO-CACHE";
                        challengeType = missionType;
                        shortTrackRacing = false;
                        dakarRacing = false;
                        windRacing = false;
                        Scuba = false;
                        bdaChallenge = false;
                        geoCache = true;
                        raceType = "";
                        outlawRacing = false;
                    }
                    else
                    {
                        OrXLog.instance.DebugLog("[OrX Mission] === HOLOKRON LOCKED AS CHALLENGE ===");
                        OnScrnMsgUC("HOLOKRON TYPE LOCKED AS CHALLENGE");
                    }
                }
            }
        }
        public void DrawChallengeType(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenge Type: ", leftLabelBooger);
            var bfRect = new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight);

            if (windRacing)
            {
                if (GUI.Button(bfRect, "W[ind/S]", OrXGUISkin.button))
                {
                    if (!locAdded)
                    {
                        //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE - SCUBA KERB ===");
                        //challengeType = "SCUBA KERB";
                        OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                        //bdaChallenge = false;
                        //windRacing = false;
                        //Scuba = true;
                        //outlawRacing = false;

                    }
                    else
                    {
                        OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                        OrXLog.instance.DebugLog("[OrX Challenge Type] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

                        //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS W[ind/S] ===");

                    }
                }
            }
            else
            {
                if (LBC)
                {
                    if (GUI.Button(bfRect, "LBC", OrXGUISkin.button))
                    {
                        if (!locAdded)
                        {
                            shortTrackRacing = true;
                            challengeType = "OUTLAW RACING";
                            raceType = "SHORT TRACK";
                            OnScrnMsgUC("CHALLENGE TYPE CHANGED TO OUTLAW RACING");
                            bdaChallenge = false;
                            windRacing = false;
                            Scuba = false;
                            dakarRacing = false;
                            outlawRacing = true;
                            _airSuperiority = false;
                            _groundAssault = true;
                            _navalBattle = false;
                        }
                        else
                        {
                            //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS SCUBA ===");
                            ///OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            //OrXLog.instance.DebugLog("[OrX Challenge Type] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                        }
                    }
                }
                else
                {
                    if (bdaChallenge)
                    {
                        if (GUI.Button(bfRect, "BD ARMORY", OrXGUISkin.button))
                        {
                            if (!locAdded)
                            {
                                if (_pKarma == Karma)
                                {
                                    shortTrackRacing = false;
                                    challengeType = "LBC";
                                    raceType = "LBC";
                                    OnScrnMsgUC("CHALLENGE TYPE CHANGED TO LOOT BOX CONTROVERSY");
                                    bdaChallenge = false;
                                    windRacing = false;
                                    Scuba = false;
                                    dakarRacing = false;
                                    outlawRacing = false;
                                    _airSuperiority = false;
                                    _groundAssault = true;
                                    _navalBattle = false;
                                    LBC = true;
                                }
                                else
                                {
                                    shortTrackRacing = true;
                                    challengeType = "OUTLAW RACING";
                                    raceType = "SHORT TRACK";
                                    OnScrnMsgUC("CHALLENGE TYPE CHANGED TO OUTLAW RACING");
                                    bdaChallenge = false;
                                    windRacing = false;
                                    Scuba = false;
                                    dakarRacing = false;
                                    outlawRacing = true;
                                    _airSuperiority = false;
                                    _groundAssault = true;
                                    _navalBattle = false;
                                }
                            }
                            else
                            {
                                //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS BD ARMORY ===");
                                OrXLog.instance.DebugLog("[OrX Challenge Type] === CHALLENGE TYPE LOCKED AS BD ARMORY ===");
                                OnScrnMsgUC("CHALLENGE TYPE LOCKED AS BD ARMORY");
                            }
                        }
                    }
                    else
                    {
                        if (outlawRacing)
                        {
                            if (GUI.Button(bfRect, "OUTLAW RACING", OrXGUISkin.button))
                            {
                                if (!locAdded)
                                {
                                    if (OrXBDAcExtension.BDArmoryIsInstalled())
                                    {
                                        OrXLog.instance.DebugLog("[OrX Challenge Type] === CHALLENGE TYPE - BD ARMORY ===");
                                        challengeType = "BD ARMORY";
                                        raceType = "LAND";
                                        OnScrnMsgUC("CHALLENGE TYPE CHANGED TO BD ARMORY");
                                        bdaChallenge = true;
                                        windRacing = false;
                                        Scuba = false;
                                        outlawRacing = false;
                                        dakarRacing = false;
                                        _airSuperiority = false;
                                        _groundAssault = true;
                                        _navalBattle = false;
                                    }
                                    else
                                    {
                                        OrXLog.instance.DebugLog("[OrX Challenge Type] === BDAc NOT INSTALLED ... CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                        OnScrnMsgUC("BD ARMORY IS NOT INSTALLED");
                                        OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                                    }
                                }
                                else
                                {
                                    OrXLog.instance.DebugLog("[OrX Challenge Type] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                    OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                                }
                            }
                        }
                        else
                        {
                        }

                    }
                }
            }
        }
        public void DrawRaceType(float line)
        {
            if (!bdaChallenge)
            {
                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Race Type: ", leftLabelBooger);
                var bfRect = new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight);

                if (dakarRacing)
                {
                    if (GUI.Button(bfRect, raceType, OrXGUISkin.button))
                    {
                        if (!locAdded)
                        {
                            raceType = "SHORT TRACK";
                            OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE CHANGED TO SHORT TRACK ===");
                            OnScrnMsgUC("RACE TYPE CHANGED TO SHORT TRACK");
                            dakarRacing = false;
                            shortTrackRacing = true;
                        }
                        else
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE LOCKED AS DAKAR RACING ===");
                            OnScrnMsgUC("RACE TYPE LOCKED AS DAKAR RACING");
                        }
                    }
                }
                else
                {
                    if (shortTrackRacing)
                    {
                        if (GUI.Button(bfRect, raceType, OrXGUISkin.button))
                        {
                            if (!locAdded)
                            {
                                raceType = "DAKAR RACING";
                                dakarRacing = true;
                                OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE CHANGED TO DAKAR RACING ===");
                                OnScrnMsgUC("RACE TYPE CHANGED TO DAKAR RACING");
                                shortTrackRacing = false;
                            }
                            else
                            {
                                OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE LOCKED AS SHORT TRACK ===");
                                OnScrnMsgUC("RACE TYPE LOCKED AS SHORT TRACK");
                            }
                        }
                    }
                }
            }
            else
            {
                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "BDA Type: ", leftLabelBooger);
                var bfRect = new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight);

                if (_airSuperiority)
                {
                    if (GUI.Button(bfRect, raceType, OrXGUISkin.button))
                    {
                        raceType = "LAND";
                        OrXLog.instance.DebugLog("[OrX Mission] === BD ARMORY CHALLENGE TYPE CHANGED TO LAND ===");
                        OnScrnMsgUC("BD ARMORY CHALLENGE TYPE CHANGED TO LAND");
                        _airSuperiority = false;
                        _groundAssault = true;
                        _navalBattle = false;
                    }
                }
                else
                {
                    if (_groundAssault)
                    {
                        if (GUI.Button(bfRect, raceType, OrXGUISkin.button))
                        {
                            raceType = "SEA";
                            OrXLog.instance.DebugLog("[OrX Mission] === BD ARMORY CHALLENGE TYPE CHANGED TO SEA ===");
                            OnScrnMsgUC("BD ARMORY CHALLENGE TYPE CHANGED TO SEA");
                            _airSuperiority = false;
                            _groundAssault = false;
                            _navalBattle = true;
                        }
                    }
                    else
                    {
                        if (_navalBattle)
                        {
                            if (GUI.Button(bfRect, raceType, OrXGUISkin.button))
                            {
                                raceType = "AIR";
                                OrXLog.instance.DebugLog("[OrX Mission] === BD ARMORY CHALLENGE TYPE CHANGED TO AIR ===");
                                OnScrnMsgUC("BD ARMORY CHALLENGE TYPE CHANGED TO AIR");
                                _airSuperiority = true;
                                _groundAssault = false;
                                _navalBattle = false;
                            }
                        }
                    }
                }
            }
        }
        public void DrawIronKerbal(float line)
        {
            if (!IronKerbal)
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "IRON KERBAL", OrXGUISkin.button))
                {
                    IronKerbal = true;

                    OrXUtilities.instance.StartMrKleen();
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "IRON KERBAL WAVE: " + OrXVesselLog.instance._wave, OrXGUISkin.box))
                {
                    //IronKerbal = false;
                }
            }
        }

        public void DrawSpawnVessel(float line)
        {
            if (!_spawningVessel)
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "SPAWN MENU", OrXGUISkin.button))
                {
                    Reach();
                    buildingMission = true;
                    _holdVesselPos = false;
                    _spawningVessel = true;
                    _settingAltitude = false;
                    SpawnMenu();
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "SPAWN CRAFT", OrXGUISkin.button))
                {
                    Reach();
                    buildingMission = true;
                    _holdVesselPos = false;
                    _spawningVessel = true;
                    _settingAltitude = false;
                    OrXSpawnHoloKron.instance.CraftSelect(false, true, false);
                }
            }
        }
        public void DrawSpawnOrX(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "SPAWN INFECTED", OrXGUISkin.button))
            {
                Reach();
                buildingMission = true;
                _holdVesselPos = false;
                _spawningOrX = true;
                _spawningVessel = true;
                _settingAltitude = false;
                OrXSpawnHoloKron.instance._spawnCraftFile = true;
                OrXSpawnHoloKron.instance.CraftSelect(false, true, false);
            }
        }
        public void DrawSpawnBarrier(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "CONSTRUCT BASE", OrXGUISkin.button))
            {
            }
        }

        public void DrawKontinuumLogin1(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), _labelConnect, leftLabelBooger);
            var bfRect = new Rect(WindowWidth - 20, ContentTop + line * entryHeight, 10, 20);
            if (!_KontinuumConnect)
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.button))
                {
                    if (!PlayOrXMission)
                    {
                        _KontinuumConnect = true;
                        _labelConnect = "Kontinuum Connect";
                    }
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.box))
                {
                    if (!PlayOrXMission)
                    {
                        _KontinuumConnect = false;
                        _labelConnect = "Connect to Kontinuum";
                    }
                }
            }
        }
        public void DrawKontinuumLogin2(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Login Name:", leftLabelBooger);
            loginName = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, (WindowWidth * 0.66f) - 10, entryHeight), loginName);
        }
        public void DrawKontinuumLogin3(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Save File: ",
                leftLabelBooger);
            pasKontinuum = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, (WindowWidth * 0.66f) - 10, entryHeight), pasKontinuum);
        }
        public void DrawKontinuumLogin4(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 80, entryHeight), "Web Link: ", leftLabelBooger);
            urlKontinuum = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, (WindowWidth * 0.66f) - 10, entryHeight), urlKontinuum);
        }

        public void DrawVessel(float line)
        {
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron Craft Data", titleStyleYellow);
        }
        public void DrawAddBlueprints(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), blueprintsLabel, leftLabelBooger);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!blueprintsAdded)
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.button))
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === ADDING BLUEPRINTS ===");
                    addingBluePrints = true;
                    blueprintsFile = "";
                    PlayOrXMission = false;
                    movingCraft = true;
                    spawningGoal = false;
                    OrXHCGUIEnabled = false;
                    openingCraftBrowser = true;
                    OrXSpawnHoloKron.instance.CraftSelect(false, false, false);
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.box))
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === REMOVING BLUEPRINTS ===");
                    blueprintsLabel = "Add Blueprints to Holo";
                    blueprintsFile = "";
                    blueprintsAdded = false;
                }
            }

        }

        public void DrawAddAS1(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 200, entryHeight), _airSupName1, leftLabelBooger);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!_savingAirSup1)
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.button))
                {
                    addingBluePrints = false;
                    OrXLog.instance.DebugLog("[OrX Mission] === ADDING AIR SUPPORT 1 ===");
                    openingCraftBrowser = true;
                    OrXSpawnHoloKron.instance.CraftSelect(false, false, false);
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.box))
                {
                    _savingAirSup1 = false;
                    saveLocalVessels = false;
                    _airSupName1 = "<empty>";
                }
            }
        }
        public void DrawAddAS2(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 200, entryHeight), _airSupName2, leftLabelBooger);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!_savingAirSup2)
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.button))
                {
                    addingBluePrints = false;

                    OrXLog.instance.DebugLog("[OrX Mission] === ADDING AIR SUPPORT 2 ===");
                    openingCraftBrowser = true;
                    OrXSpawnHoloKron.instance.CraftSelect(false, false, false);
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.box))
                {
                    _savingAirSup2 = false;
                    _airSupName2 = "<empty>";
                }
            }
        }
        public void DrawAddAS3(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 200, entryHeight), _airSupName3, leftLabelBooger);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!_savingAirSup3)
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.button))
                {
                    addingBluePrints = false;

                    OrXLog.instance.DebugLog("[OrX Mission] === ADDING AIR SUPPORT 3 ===");
                    openingCraftBrowser = true;
                    OrXSpawnHoloKron.instance.CraftSelect(false, false, false);
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.box))
                {
                    _savingAirSup3 = false;
                    _airSupName3 = "<empty>";
                }
            }
        }
        public void DrawSpawnChanceRange(float line)
        {
            GUI.Label(new Rect(0, (ContentTop + line * entryHeight) + line, WindowWidth, 20), "Encounter Chance: " + String.Format("{0:0}", localSaveRange) + " %", centerLabelGreen);
            _spawnChance = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), _spawnChance * 100, 0, 1, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalSliderThumb);
        }

        public void DrawPassword(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 80, entryHeight), "Password:", leftLabelBooger);
            Password = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, (WindowWidth * 0.66f) - 10, entryHeight), Password);
        }
        public void DrawModule(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Tech: ", leftLabelBooger);
            tech = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, (WindowWidth * 0.66f) - 10, entryHeight), tech);
        }

        public void DrawSaveLocal(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 200, entryHeight), saveLocalLabel, leftLabelBooger);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!saveLocalVessels)
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.button))
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === SAVE LOCAL VESSELS = TRUE ===");
                    saveLocalVessels = true;
                    saveLocalLabel = "Saving Local Craft";
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", OrXGUISkin.box))
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === SAVE LOCAL VESSELS = FALSE ===");
                    saveLocalVessels = false;
                    saveLocalLabel = "Save Local Craft";
                }
            }
        }
        public void DrawLocalSaveRange(float line)
        {
            GUI.Label(new Rect(0, (ContentTop + line * entryHeight) + line, WindowWidth, 20), String.Format("{0:0}", localSaveRange) + " meters", centerLabelGreen);
            localSaveRange = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), localSaveRange, 50, 1000, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalSliderThumb);
        }
        public void DrawSave(float line)
        {
            var saveRect = new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight);
            if (HoloKronName != string.Empty && HoloKronName != "")
            {
                if (!HoloKronName.Contains(".") && !HoloKronName.Contains("-"))
                {
                    if (groupName != "" || groupName != string.Empty)
                    {
                        if (!groupName.Contains("."))
                        {
                            if (missionDescription0 != string.Empty && missionDescription0 != "")
                            {
                                if (!geoCache)
                                {
                                    if (addCoords || bdaChallenge)
                                    {
                                        if (GUI.Button(saveRect, "SAVE AND EXIT", OrXGUISkin.button))
                                        {
                                            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/" + HoloKronName + "-" + groupName + ".orx");
                                            if (_file == null)
                                            {
                                                OrXLog.instance.DebugLog("[OrX Mission] === GROUP NAME ENTERED  ===");
                                                OrXLog.instance.DebugLog("[OrX Mission] === SAVING HOLOKRON ===");
                                                addCoords = false;
                                                _getCenterDist = false;
                                                if (bdaChallenge)
                                                {
                                                    addingMission = false;
                                                    saveLocalVessels = true;
                                                }
                                                else
                                                {
                                                    addingMission = true;
                                                    if (_lastStage != null)
                                                    {
                                                        _lastStage.vesselName = HoloKronName + " " + hkCount + " FINSH LINE";
                                                        _lastStage = null;
                                                    }
                                                }
                                                getNextCoord = false;
                                                SaveConfig(HoloKronName, false);
                                            }
                                            else
                                            {
                                                OnScrnMsgUC(HoloKronName + " already exists ......");

                                                OnScrnMsgUC("Please choose another name");

                                                //OrXAppendCfg.instance.EnableGui(HoloKronName);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!spawningStartGate)
                                        {
                                            if (GUI.Button(saveRect, "START ADD COORDS", OrXGUISkin.button))
                                            {
                                                if (HoloKronName != string.Empty && HoloKronName != "")
                                                {
                                                    if (missionDescription0 != string.Empty && missionDescription0 != "")
                                                    {
                                                        _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".orx");
                                                        if (_file == null)
                                                        {
                                                            _lastStage = null;
                                                            triggerVessel = FlightGlobals.ActiveVessel;
                                                            hkSpawned = false;
                                                            buildingMission = true;
                                                            CoordDatabase = new List<string>();
                                                            _stageGates = new ConfigNode();
                                                            addCoords = true;
                                                            addingMission = false;
                                                            saveLocalVessels = false;
                                                            OrXSpawnHoloKron.instance.stageCount = 0;
                                                            startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                                                            boidPos = startLocation;
                                                            GetShortTrackCenter(startLocation);
                                                            SaveConfig(HoloKronName, false);
                                                        }
                                                        else
                                                        {
                                                            OnScrnMsgUC(HoloKronName + " already exists ......");

                                                            OnScrnMsgUC("Please choose another name");

                                                            //OrXAppendCfg.instance.EnableGui(HoloKronName);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        OnScrnMsgUC("Please add a description");
                                                    }
                                                }
                                                else
                                                {
                                                    OnScrnMsgUC("Please enter a name for your HoloKron");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "START GATE SPAWNED", OrXGUISkin.box))
                                            {
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (GUI.Button(saveRect, "SAVE HOLOKRON", OrXGUISkin.button))
                                    {
                                        if (HoloKronName != string.Empty && HoloKronName != "")
                                        {
                                            if (missionDescription0 != string.Empty && missionDescription0 != "")
                                            {
                                                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + groupName + "/"  + HoloKronName + "-" + groupName + ".orx");
                                                if (_file == null)
                                                {
                                                    triggerVessel = FlightGlobals.ActiveVessel;
                                                    movingCraft = true;
                                                    spawningStartGate = false;
                                                    _getCenterDist = false;

                                                    SaveConfig(HoloKronName, false);
                                                }
                                                else
                                                {
                                                    OnScrnMsgUC(HoloKronName + " already exists ......");

                                                    OnScrnMsgUC("Please choose another name");

                                                    //OrXAppendCfg.instance.EnableGui(HoloKronName);
                                                }
                                            }
                                            else
                                            {
                                                OnScrnMsgUC("Please add a description");
                                            }
                                        }
                                        else
                                        {
                                            OnScrnMsgUC("Please enter a name for your HoloKron");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ScreenMessages.PostScreenMessage(new ScreenMessage("Please add a description", 1, ScreenMessageStyle.UPPER_CENTER));
                            }
                        }
                        else
                        {
                            ScreenMessages.PostScreenMessage(new ScreenMessage("Creator name cannot contain a '.'", 1, ScreenMessageStyle.UPPER_CENTER));
                            ScreenMessages.PostScreenMessage(new ScreenMessage("Please enter another creator name", 1, ScreenMessageStyle.UPPER_CENTER));
                        }
                    }
                    else
                    {
                        ScreenMessages.PostScreenMessage(new ScreenMessage("Please enter a creator name", 1, ScreenMessageStyle.UPPER_CENTER));
                    }
                }
                else
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("HoloKron names cannot contain a '.' or a '-'", 1, ScreenMessageStyle.UPPER_CENTER));
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Please rename your HoloKron ....", 1, ScreenMessageStyle.UPPER_CENTER));
                }
            }
            else
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Please enter a name for your HoloKron", 1, ScreenMessageStyle.UPPER_CENTER));
            }

        }
        public void DrawCancel(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", OrXGUISkin.button))
            {
                if (PlayOrXMission)
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === CLOSE WINDOW ===");
                    if (_editor)
                    {
                        OrXUtilities.instance.GrabCreations(groupName, true);
                    }
                    else
                    {
                        CancelChallenge();
                    }
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === CANCEL HOLOKRON CREATION ===");
                    if (_HoloKron != null)
                    {
                        if (_HoloKron.rootPart.Modules.Contains<ModuleOrXMission>())
                        {
                            _HoloKron.rootPart.explode();
                        }
                    }
                    CancelChallenge();
                }
            }
        }

        #endregion

    }
}