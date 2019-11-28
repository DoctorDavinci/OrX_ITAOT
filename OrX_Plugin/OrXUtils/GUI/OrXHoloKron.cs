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

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXHoloKron : MonoBehaviour
    {
        #region Core

        #region Variables

        bool _getCenterDist = false;
        bool _editor = false;
        public double airTime = 0;
        bool reset = false;
        bool mrKleen = false;
        public bool devKitInstalled = false;
        Kontinuum.OrXKontinuum KontinuumClient;
        string fileDateTime = "";
        string fileSize = "";
        public List<string> KontinuumDirectoryList;
        string _ate = "9";

        public Vector3 UpVect;
        public Vector3 EastVect;
        public Vector3 NorthVect;
        Vector3 worldPos;

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
        public CraftBrowserDialog craftBrowser;
        public bool spawningGoal = false;

        public bool addNextCoord = false;

        string _sth = string.Empty;
        bool showScores = false;

        public bool unlocked = false;

        public bool buildingMission = false;
        public bool showTargets = false;
        public static Rect WindowRectToolbar;
        public static OrXHoloKron instance;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        public static bool TBBadded = false;
        public bool OrXHCGUIEnabled;

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
        public double distance = 0;
        public double missionTime = 0;

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

        int gpsCount = 0;
        public double latMission = 0;
        public double lonMission = 0;
        public double altMission = 0;

        public Vector3d lastCoord;
        public int locCount = 0;

        List<string> CoordDatabase;
        int coordCount = 0;
        List<string> ModuleDatabase;

        Vector3d startLocation;

        List<string> stageTimes;
        public double maxDepth = 0;

        List<string> _scoreboard;
        public string challengersName = string.Empty;
        public double topSurfaceSpeed = 0;

        public int ec = 0;

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
        string maxSpeedSB0 = string.Empty;

        string nameSB1 = string.Empty;
        string timeSB1 = string.Empty;
        string maxSpeedSB1 = string.Empty;

        string nameSB2 = string.Empty;
        string timeSB2 = string.Empty;
        string maxSpeedSB2 = string.Empty;

        string nameSB3 = string.Empty;
        string timeSB3 = string.Empty;
        string maxSpeedSB3 = string.Empty;

        string nameSB4 = string.Empty;
        string timeSB4 = string.Empty;
        string maxSpeedSB4 = string.Empty;

        string nameSB5 = string.Empty;
        string timeSB5 = string.Empty;
        string maxSpeedSB5 = string.Empty;

        string nameSB6 = string.Empty;
        string timeSB6 = string.Empty;
        string maxSpeedSB6 = string.Empty;

        string nameSB7 = string.Empty;
        string timeSB7 = string.Empty;
        string maxSpeedSB7 = string.Empty;

        string nameSB8 = string.Empty;
        string timeSB8 = string.Empty;
        string maxSpeedSB8 = string.Empty;

        string nameSB9 = string.Empty;
        string timeSB9 = string.Empty;
        string maxSpeedSB9 = string.Empty;

        bool updatingScores = false;

        Vector3 nextLocation;
        public double targetDistanceMission = 0;
        public string _targetDistance = string.Empty;
        public Vessel targetCoord;

        Quaternion rot;
        public double _lat = 0f;
        public double _lon = 0f;
        public double _alt = 0f;

        public ConfigNode craft = null;
        public string shipDescription = string.Empty;

        public StringBuilder debugString = new StringBuilder();
        public string craftToSpawn = string.Empty;
        public string cfgToLoad = string.Empty;
        string OrXv = "OrXv";
        string _OrXV = "";
        public Vessel _lastStage;
        public Vessel _HoloKron;
        string holoKronCraftLoc = string.Empty;
        List<string> holoKronFiles;
        string sphLoc = string.Empty;
        List<string> sphFiles;
        string vabLoc = string.Empty;
        List<string> vabFiles;
        bool sph = true;
        bool holoHangar = false;

        public bool saveLocalVessels = false;
        string saveLocalLabel = "Save Local Craft";
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
        Waypoint currentWaypoint;
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
        string loginName = "";
        bool connectToKontinuum = false;
        string pasKontinuum = "";
        string _labelConnect = "Connect to Kontinuum";
        string urlKontinumm = "";
        bool _a2 = false;
        bool _s = false;

        Vector3d playerPositon;
        bool _KontinuumConnect = false;
        public string Karma = "ssadab";
        public bool _blink = false;
        public bool _blinking = false;

        bool showChallengelist = false;
        bool showGeoCacheList = false;
        public bool _showSettings = false;
        bool _settings0 = false;
        bool _settings1 = false;
        bool _settings2 = false;
        string gpsString = "";
        public string creatorName = "";

        public bool killingChallenge = false;
        bool showCreatedHolokrons = false;

        public string statsName = "";
        public string statsTime = "";
        public double statsMaxSpeed = 0;
        public string statsTotalAirTime = "";
        public double statsMaxDepth = 0;
        bool _showMode = false;
        bool _b = false;
        bool _a = false;

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

        public List<string> scoreboardStats;
        bool _extractScoreboard = false;
        bool _d = false;
        bool _s2 = false;
        bool _modeEnabled = false;

        Vector3d boidPos;

        public bool _importingScores = false;
        string currentScoresFile = "";
        Type partModules;

        public bool getNextCoord = false;
        bool modCheckFail = false;
        public Vector2 scrollPosition = Vector2.zero;
        public Vector2 scrollPosition2 = Vector2.zero;
        public bool _placingChallenger = false;
        bool hkSpawned = false;
        List<string> _orxFileMods;
        List<string> _orxFilePartModules;
        bool disablePRE = false;
        ConfigNode PREsettings;
        public string _pKarma = string.Empty;
        List<string> installedMods;
        public bool _preInstalled = false;

        private readonly List<AvailablePart> parts = new List<AvailablePart>();
        public bool _showPartModules = false;
        bool _scoreSaved = false;
        bool _gateKillDelay = false;
        bool _killPlace = false;

        public bool _showTimer = false;
        public double _timer = 0;
        public double _missionStartTime = 0;
        public double _time = 0;
        string _currentOrXFile = "";
        Rigidbody _rb;
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
        public float _saveAltitude = 1000;
        public bool _settingAltitude = false;

        bool _timerOn = false;
        public bool _refuel = false;
        public float salt = 0;
        public Vessel _spawnedCraft;

        public List<string> _enemyCraft;

        #endregion


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
            KontinuumDirectoryList = new List<string>();
            OrXGeoCacheCreatorList = new List<string>();
            OrXChallengeCreatorList = new List<string>();
            ModuleDatabase = new List<string>();
            _enemyCraft = new List<string>();

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
            WindowRectToolbar = new Rect(40, 50, toolWindowWidth, toolWindowHeight);
            string[] _v = Application.version.Split(new char[] { '.' });
            _OrXV = _v[1];

            ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            if (playerData != null)
            {
                challengersName = playerData.GetValue("name");
                if (challengersName != "")
                {
                    creatorName = challengersName;
                    loginName = creatorName;
                }
            }

            GameEvents.onVesselSOIChanged.Add(checkSOI);
            GameEvents.onGameStateLoad.Add(resetHoloKronSystem);
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
            }
        }

        public void TBBAdd()
        {
            string OrXDir = "OrX/Plugin/";

            if (!TBBadded)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(OrXDir + "OrX_icon", false); //texture to use for the button
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

        public void Blank() { }

        #region Kontinuum Connect

        // FTP
        private void CreateConnectionFTP(string hostIP, string userName, string password)
        {
            KontinuumClient = new Kontinuum.OrXKontinuum(hostIP, userName, password);
        }
        private void UploadFileFTP(string remoteFile, string localFile)
        {
            KontinuumClient.Upload(remoteFile, localFile);

        }
        private void DownloadFileFTP(string remoteFile, string localFile)
        {
            KontinuumClient.Download(remoteFile, localFile);
        }
        private void DeleteFileFTP(string remoteFile)
        {
            KontinuumClient.Delete(remoteFile);
        }
        private void RenameFileFTP(string remoteFile, string newName)
        {
            KontinuumClient.Rename(remoteFile, newName);
        }
        private void CreateDirectoryFTP(string remoteDirectory)
        {
            KontinuumClient.CreateDirectory(remoteDirectory);
        }
        private void GetFileCreatedDateTimeFTP(string remoteFile)
        {
            fileDateTime = KontinuumClient.GetFileCreatedDateTime(remoteFile);
        }
        private void GetFileSizeFTP(string remoteFile)
        {
            fileSize = KontinuumClient.GetFileSize(remoteFile);
        }
        private void DirectoryListSimpleFTP(string remoteDirectory)
        {
            KontinuumDirectoryList = new List<string>();
            string[] KontinuumDirectoryListing = KontinuumClient.DirectoryListDetailed(remoteDirectory);
            for (int i = 0; i < KontinuumDirectoryListing.Count(); i++)
            {
                KontinuumDirectoryList.Add(KontinuumDirectoryListing[i]);
            }
        }
        private void DirectoryListDetailedFTP(string remoteDirectory)
        {
            KontinuumDirectoryList = new List<string>();
            string[] KontinuumDirectoryListing = KontinuumClient.DirectoryListDetailed(remoteDirectory);
            for (int i = 0; i < KontinuumDirectoryListing.Count(); i++)
            {
                KontinuumDirectoryList.Add(KontinuumDirectoryListing[i]);
            }
        }
        private void DisconnectFTP()
        {
            KontinuumClient = null;
        }

        //WEB
        WebClient fileDownloader;
        bool _downloading = false;
        private void DownloadFile(string _webLoc, string _localSaveLoc)
        {
            _downloading = true;
            string loc = UrlDir.ApplicationRootPath + "GameData/OrX/Kontinuum/" + _localSaveLoc;
            Uri webLoc = new Uri(_webLoc);

            try
            {
                fileDownloader = new WebClient();
                fileDownloader.DownloadFileAsync(webLoc, loc);
                fileDownloader.Dispose();
                connectToKontinuum = false;
                _downloading = false;
                OnScrnMsgUC("Welcome to the Kontinuum " + loginName + " ....");

            }
            catch
            {
                Debug.Log("[OrX Download File - WEB] ==================================================");
                Debug.Log("[OrX Download File - WEB] ===== Unable to establish a connection ....  =====");
                Debug.Log("[OrX Download File - WEB] ===== FILE LINK: " + _webLoc + " =====");
                Debug.Log("[OrX Download File - WEB] ==================================================");
                OnScrnMsgUC("Unable to establish a connection ....");
                OnScrnMsgUC("Check if the file link is valid ....");
            }
            _downloading = false;
        }

        #endregion

        #endregion

        #region Utilities

        private void GetCreatorList(bool _challenge)
        {
            Reach();

            OrXGeoCacheCreatorList = new List<string>();
            OrXChallengeCreatorList = new List<string>();

            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            var files = new List<string>(Directory.GetFiles(holoKronLoc, "*.data", SearchOption.AllDirectories));
            if (files != null)
            {
                Debug.Log("[OrX Get Creator List] === Found " + files.Count + " files ===");

                List<string>.Enumerator dataFiles = files.GetEnumerator();
                while (dataFiles.MoveNext())
                {
                    if (dataFiles.Current != null)
                    {
                        ConfigNode dataFile = ConfigNode.Load(dataFiles.Current);
                        if (dataFile != null)
                        {
                            string _creator = "";

                            foreach (ConfigNode.Value cv in dataFile.values)
                            {
                                if (cv.name == "creator")
                                {
                                    _creator = cv.value;
                                }
                                else
                                {
                                    if (cv.value == "CHALLENGE")
                                    {
                                        if (!OrXChallengeCreatorList.Contains(_creator))
                                        {
                                            Debug.Log("[OrX Get Creator List] === Adding " + _creator + " to challenge creator list ===");
                                            OrXChallengeCreatorList.Add(_creator);
                                        }
                                    }
                                    else
                                    {
                                        if (!OrXGeoCacheCreatorList.Contains(_creator))
                                        {
                                            Debug.Log("[OrX Get Creator List] === Adding " + _creator + " to geo-cache creator list ===");
                                            OrXGeoCacheCreatorList.Add(_creator);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                dataFiles.Dispose();
            }

            if (!_challenge)
            {
                if (OrXGeoCacheCreatorList.Count >= 1)
                {
                    //Reach();
                    //OrXTargetDistance.instance.TargetDistance(true, false, false, true, "", new Vector3d());

                    showChallengelist = true;
                    showGeoCacheList = true;
                }
                else
                {
                    MainMenu();
                    OnScrnMsgUC("No HoloKrons detected .....");
                }

            }
            else
            {
                if (OrXChallengeCreatorList.Count >= 1)
                {
                    MainMenu();
                    showChallengelist = true;
                    movingCraft = false;
                    getNextCoord = false;
                    challengeRunning = false;
                    showCreatedHolokrons = false;
                    showGeoCacheList = false;
                }
                else
                {
                    MainMenu();
                    OnScrnMsgUC("No HoloKrons detected .....");
                }
            }
        }
        IEnumerator GetCreations(string _creatorName, bool challenge)
        {
            Reach();
            OrXChallengeList = new List<string>();
            OrXCoordsList = new List<string>();
            OrXChallengeNameList = new List<string>();
            OrXGeoCacheNameList = new List<string>();

            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creatorName + "/";
            List<string> files = new List<string>(Directory.GetFiles(holoKronLoc, "*.orx", SearchOption.AllDirectories));

            if (files != null)
            {
                OrXLog.instance.DebugLog("[OrX Get Creations] === Found " + files.Count + " files ===");
                OnScrnMsgUC("Found " + files.Count + " files .....");

                List<string>.Enumerator cfgsToAdd = files.GetEnumerator();
                while (cfgsToAdd.MoveNext())
                {
                    if (cfgsToAdd.Current != null)
                    {
                        ConfigNode fileNode = ConfigNode.Load(cfgsToAdd.Current);

                        if (fileNode != null && fileNode.HasNode("OrX"))
                        {
                            #region To Iplement
                            /*
                            ModuleDatabase = new List<string>();
                            int moduleCount = 0;
                            ConfigNode _modules = _file.GetNode("modules");

                            if (_modules != null)
                            {
                                foreach (ConfigNode.Value moduleCheck in _modules.values)
                                {
                                    moduleCount += 1;
                                    ModuleDatabase.Add(moduleCheck.value);
                                }

                                try
                                {
                                    foreach (AvailablePart part in PartLoader.LoadedPartsList)
                                    {
                                        foreach (ConfigNode cn in part.partConfig.nodes)
                                        {
                                            if (cn.name == "MODULE")
                                            {
                                                foreach (ConfigNode.Value cv in cn.values)
                                                {
                                                    if (cv.name == "name")
                                                    {
                                                        List<string>.Enumerator moduleName = ModuleDatabase.GetEnumerator();
                                                        while (moduleName.MoveNext())
                                                        {
                                                            try
                                                            {
                                                                if (moduleName.Current != null)
                                                                {
                                                                    if (moduleName.Current == cv.value)
                                                                    {
                                                                        moduleCount -= 1;
                                                                        //ModuleDatabase.Remove(cv.value);
                                                                    }
                                                                }
                                                            }
                                                            catch
                                                            {

                                                            }
                                                        }
                                                        moduleName.Dispose();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {

                                }

                            }
                            */

                            #endregion

                            ConfigNode node = fileNode.GetNode("OrX");
                            string _HoloKronName = fileNode.GetValue("name");
                            bool ableToLoad = true;
                            bool _spawned = true;
                            int _hkCount = 0;

                            /*
                            if (moduleCount >= 0)
                            {
                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD ===");
                                ableToLoad = false;
                                OnScrnMsgUC("UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD");

                            }
                            */

                            ableToLoad = true;
                            if (ableToLoad)
                            {
                                foreach (ConfigNode spawnCheck in node.nodes)
                                {
                                    yield return new WaitForFixedUpdate();

                                    if (_spawned)
                                    {
                                        if (spawnCheck.name.Contains("OrXHoloKronCoords" + _hkCount))
                                        {
                                            OrXLog.instance.DebugLog("[OrX Get Creations] === FOUND HOLOKRON ... CHECKING SOI ===");

                                            string _soi = spawnCheck.GetValue("SOI");
                                            if (_soi == soi)
                                            {
                                                OrXLog.instance.DebugLog("[OrX Get Creations] " + _HoloKronName + "'s current SOI '" + soi + "' matches HoloKron SOI '" + _soi + "'");

                                                bool _challenge = false;
                                                string missionType = spawnCheck.GetValue("missionType");
                                                string _count = spawnCheck.GetValue("count");

                                                if (_count == "0")
                                                {
                                                    _count = "";
                                                }
                                                else
                                                {
                                                    string tempCount = _count;
                                                    _count = "-" + tempCount;
                                                }
                                                if (missionType == "CHALLENGE")
                                                {
                                                    _challenge = true;
                                                }
                                                yield return new WaitForFixedUpdate();
                                                string targetCoords = spawnCheck.GetValue("Targets");
                                                if (targetCoords == string.Empty)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Get Creations] " + _HoloKronName + " " + _hkCount + " Target string was empty!");
                                                }
                                                else
                                                {
                                                    string[] data = targetCoords.Split(new char[] { ',' });
                                                    if (!OrXChallengeList.Contains(targetCoords))
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] === ADDING COORDS TO LIST ===");
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] " + targetCoords);
                                                        OrXChallengeList.Add(targetCoords);
                                                        string nameToAdd = data[1] + _count;

                                                        if (!OrXChallengeNameList.Contains(nameToAdd))
                                                        {
                                                            OrXChallengeNameList.Add(nameToAdd);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] === GEO-CACHE LIST ALREADY CONTAINS THESE COORDS ===");
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] " + targetCoords);
                                                    }

                                                    /*
                                                    if (_challenge)
                                                    {
                                                        if (!OrXChallengeList.Contains(targetCoords))
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] === ADDING COORDS TO CHALLENGE LIST ===");
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] " + targetCoords);
                                                            OrXChallengeList.Add(targetCoords);
                                                            string nameToAdd = data[1] + _count;

                                                            if (!OrXChallengeNameList.Contains(nameToAdd))
                                                            {
                                                                OrXChallengeNameList.Add(nameToAdd);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] === GEO-CACHE LIST ALREADY CONTAINS THESE COORDS ===");
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] " + targetCoords);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!OrXCoordsList.Contains(targetCoords))
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] === ADDING COORDS TO GEO-CACHE LIST ===");
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] " + targetCoords);
                                                            OrXCoordsList.Add(targetCoords);
                                                            string nameToAdd = data[1] + _count;

                                                            if (!OrXGeoCacheNameList.Contains(nameToAdd))
                                                            {
                                                                OrXGeoCacheNameList.Add(nameToAdd);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] === GEO-CACHE LIST ALREADY CONTAINS THESE COORDS ===");
                                                            OrXLog.instance.DebugLog("[OrX Get Creations] " + targetCoords);
                                                        }
                                                    }
                                                    */
                                                    yield return new WaitForFixedUpdate();
                                                }

                                                string _spawnedString = spawnCheck.GetValue("spawned");
                                                if (_spawnedString == "False")
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Get Creations] === " + _HoloKronName + " " + _hkCount + " has not spawned ... ");
                                                    _spawned = false;
                                                }
                                                else
                                                {
                                                    var complete = spawnCheck.GetValue("completed");
                                                    if (complete == "False")
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] === " + _HoloKronName + " " + _hkCount + " has not been completed ... END TRANSMISSION"); ;
                                                        _spawned = false;
                                                    }
                                                    else
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] === " + _HoloKronName + " " + _hkCount + " has been completed ... CHECKING FOR EXTRAS"); ;
                                                        if (spawnCheck.HasValue("extras"))
                                                        {
                                                            var t = spawnCheck.GetValue("extras");
                                                            if (t == "False")
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Get Creations] === " + _HoloKronName + " " + _hkCount + " has no extras ... END TRANSMISSION");
                                                                _spawned = false;
                                                            }
                                                            else
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Get Creations] === " + _HoloKronName + " " + _hkCount + " has extras ... CONTINUING");
                                                                _hkCount += 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OrXLog.instance.DebugLog("[OrX Get Creations] " + _HoloKronName + " is not in the current SOI");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                cfgsToAdd.Dispose();
            }

            if (challenge)
            {
                showGeoCacheList = false;
            }
            else
            {
                showGeoCacheList = false;

                // showGeoCacheList = true;
            }
            showCreatedHolokrons = true;
            //PlayOrXMission = true;
            movingCraft = false;
            getNextCoord = false;
            GuiEnabledOrXMissions = false;
            _showSettings = false;
            connectToKontinuum = false;
            checking = false;
        }
        IEnumerator CheckInstalledMods(bool _spawn)
        {

            getNextCoord = false;
            movingCraft = true;
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx");
            
            if (_file != null)
            {
                ConfigNode mods = _file.GetNode("Mods");
                _orxFilePartModules = new List<string>();
                _orxFileMods = new List<string>();
                Debug.Log("[OrX Check Installed Mods] === CHECKING INSTALLED MODS ===");
                int count = 0;
                int count2 = 0;

                if (mods != null)
                {
                    Debug.Log("[OrX Check Installed Mods] === MODS FOUND IN FILE ===");

                    foreach (ConfigNode.Value cv in mods.values)
                    {
                        if (cv.value == "PartModule")
                        {
                            //count2 += 1;
                            //_orxFilePartModules.Add(cv.name);
                        }

                        if (cv.value == "Mod")
                        {
                            count += 1;
                            _orxFileMods.Add(cv.name);
                        }
                    }
                    yield return new WaitForFixedUpdate();

                    List<string>.Enumerator _installedMods = installedMods.GetEnumerator();
                    while (_installedMods.MoveNext())
                    {
                        if (_installedMods.Current != null)
                        {

                            if (_orxFileMods.Contains(_installedMods.Current))
                            {
                                _orxFileMods.Remove(_installedMods.Current);
                                count -= 1;
                            }
                        }
                    }
                    _installedMods.Dispose();
                    yield return new WaitForFixedUpdate();
                    /*
                    List<string>.Enumerator _pModules = _orxFilePartModules.GetEnumerator();
                    while (_pModules.MoveNext())
                    {
                        if (_pModules.Current != null)
                        {
                            if (ModuleDatabase.Contains(_pModules.Current))
                            {
                                _orxFilePartModules.Remove(_pModules.Current);
                                count2 -= 1;
                            }
                        }
                    }
                    _pModules.Dispose();
                    yield return new WaitForFixedUpdate();
                    */
                    bool _continue = true;
                    if (count != 0) // || count2 != 0)
                    {
                        _continue = false;
                    }

                    if (!_continue)
                    {
                        Debug.Log("[OrX Check Installed Mods] === MISSING MOD COUNT: " + count + " ===");

                        List<string>.Enumerator _leftovers = _orxFileMods.GetEnumerator();
                        while (_leftovers.MoveNext())
                        {
                            if (_leftovers.Current != null)
                            {
                                Debug.Log("[OrX Check Installed Mods] === " + _leftovers.Current + " NOT INSTALLED ===");
                            }
                        }
                        _leftovers.Dispose();
                        SetModFail();

                        /*
                        Debug.Log("[OrX Check Installed Mods - Missing Part Module] === COUNT: " + count2 + " ===");

                        List<string>.Enumerator _leftovers_ = _orxFilePartModules.GetEnumerator();
                        while (_leftovers_.MoveNext())
                        {
                            if (_leftovers_.Current != null)
                            {
                                modCheckFail = true;
                                movingCraft = false;

                                Debug.Log("[OrX Check Installed Mods - Missing Part Module] === " + _leftovers_.Current + " NOT INSTALLED ===");
                            }
                        }
                        _leftovers_.Dispose();
                        */
                    }
                    else
                    {
                        if (_spawn)
                        {
                            Debug.Log("[OrX Check Installed Mods] === MODS CHECKED OUT ===");
                            movingCraft = false;
                            modCheckFail = false;
                            OrXSpawnHoloKron.instance.SpawnLocal(false, HoloKronName, new Vector3d());
                        }
                        else
                        {
                            StartCoroutine(OpenHoloKronRoutine(geoCache, HoloKronName, hkCount, null, null));
                        }
                    }
                }
            }
        }
        IEnumerator MrKleen()
        {
            killingChallenge = true;
            OnScrnMsgUC("The Kontinuum is calling a maid .....");
            getNextCoord = false;
            movingCraft = true;
            GuiEnabledOrXMissions = true;
            Debug.Log("[OrX Mr Kleen] === CALLING JASON ===");

            List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
            while (v.MoveNext())
            {
                if (v.Current != null)
                {
                    if (!v.Current.isActiveVessel)
                    {
                        Debug.Log("[OrX Mr Kleen] === Jason is killing " + v.Current.vesselName + "  ===");
                        v.Current.rootPart.AddModule("ModuleOrXJason", true);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
            v.Dispose();
            getNextCoord = false;
            movingCraft = false;
            GuiEnabledOrXMissions = false;
            killingChallenge = false;
            OnScrnMsgUC("The slate is being swept clean .....");
        }

        public void HijackAsteroidSpawnTimer(bool hijack)
        {
            /*
            OrXLog.instance.DebugLog("[OrX Hijack Asteroid Spawn] ===== HIJACKING =====");

            var AsteroidSpawn = GetComponent<ScenarioDiscoverableObjects>();
            OrXLog.instance.DebugLog("[OrX Hijack Asteroid Spawn] ===== Get Component =====");

            AsteroidSpawn.enabled = false;
            OrXLog.instance.DebugLog("[OrX Hijack Asteroid Spawn] ===== enabled = false =====");

            AsteroidSpawn.spawnInterval = float.MaxValue;
            OrXLog.instance.DebugLog("[OrX Hijack Asteroid Spawn] ===== spawnInterval = float.MaxValue =====");

            
            if (!hijack)
            {
                _asteroidSpawnTimer = asteroidSpawnTimer.spawnInterval;
                asteroidSpawnTimer.spawnInterval = float.MaxValue;
                asteroidSpawnTimer.enabled = false;
                OrXLog.instance.DebugLog("[OrX Hijack Asteroid Spawn] ===== HIJACKING =====");

            }
            else
            {
                asteroidSpawnTimer.spawnInterval = _asteroidSpawnTimer;
                asteroidSpawnTimer.enabled = true;
                OrXLog.instance.DebugLog("[OrX Hijack Asteroid Spawn] ===== RECOVERED =====");
            }
            */
        }
        public void StopScan(bool playerCancel)
        {
            MainMenu();

            checking = false;
            building = false;
            buildingMission = false;
            addCoords = false;
            PlayOrXMission = false;

            if (playerCancel)
            {
                GuiEnabledOrXMissions = false;
                //OrXHCGUIEnabled = false;
                locAdded = false;
                locCount = 0;
                movingCraft = false;
                challengeRunning = false;
                OnScrnMsgUC("Operation 'Dinner Out' was cancelled .....");
                ResetData();
            }
        }

        public void LoadResetDelay()
        {
            StartCoroutine(LoadDelay());
        }
        IEnumerator LoadDelay()
        {
            while (!FlightGlobals.ready)
            {
                yield return null;
            }

            if (building || buildingMission)
            {
                ResetData();
            }
            else
            {
                if (challengeRunning)
                {
                   challengeRunning = false;
                   StopScan(true);
                }
            }
        }
        IEnumerator LoadDataFiles()
        {
            soi = FlightGlobals.ActiveVessel.mainBody.name;
            OrXCoordsList = new List<string>();
            OrXChallengeList = new List<string>();
            OrXGeoCacheNameList = new List<string>();
            OrXChallengeNameList = new List<string>();
            int coordCount = 0;

            ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            if (playerData == null)
            {
                playerData = new ConfigNode();
                playerData.SetValue("name", challengersName, true);
                playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            }
            else
            {
                challengersName = playerData.GetValue("name");
            }

            Debug.Log("[OrX Check Imports] === CHECKING IMPORTS FOLDER FOR ORX FILES ===");
            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Import/";
            var files = new List<string>(Directory.GetFiles(holoKronLoc, "*.orx", SearchOption.AllDirectories));
            if (files != null)
            {
                OrXLog.instance.DebugLog("[OrX Check Imports] === " + files.Count + " ORX FILES FOUND ===");
                OrXLoadedFileList = new List<string>();

                List<string>.Enumerator cfgsToMove = files.GetEnumerator();
                while (cfgsToMove.MoveNext())
                {
                    yield return new WaitForFixedUpdate();

                    if (!OrXLoadedFileList.Contains(cfgsToMove.Current))
                    {
                        ConfigNode orxFile = ConfigNode.Load(cfgsToMove.Current);
                        if (orxFile != null)
                        {
                            OrXLoadedFileList.Add(cfgsToMove.Current);
                            string _processed = "";
                            string _name = orxFile.GetValue("name");
                            string _creator = orxFile.GetValue("creator");
                            ConfigNode orxNode = orxFile.GetNode("OrX");

                            if (orxFile.HasValue("processed"))
                            {
                                _processed = orxFile.GetValue("processed");
                            }
                            else
                            {
                                orxFile.SetValue("processed", "False", true);
                                _processed = "False";
                            }

                            if (_processed == "False")
                            {
                                Debug.Log("[OrX Check Imports] === PROCESSING " + _name + " CREATED BY " + _creator + " ===");

                                if (orxNode != null)
                                {
                                    foreach (ConfigNode cn in orxNode.nodes)
                                    {
                                        if (cn.name.Contains("OrXHoloKronCoords"))
                                        {
                                            int _hkCount = 0;
                                            string _count = cn.GetValue("count");
                                            int _count_ = int.Parse(_count);
                                            string _newDir = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creator + "/" + _name + "/";
                                            if (!Directory.Exists(_newDir))
                                            {
                                                Directory.CreateDirectory(_newDir);
                                            }
                                            else
                                            {
                                                while (_hkCount <= _count_)
                                                {
                                                    _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + _hkCount + "-" + creatorName + ".orx");
                                                    if (_file != null)
                                                    {
                                                        ConfigNode _orxNode = _file.GetNode("OrX");
                                                        foreach (ConfigNode cn2 in _orxNode.nodes)
                                                        {
                                                            if (cn2.name.Contains("OrXHoloKronCoords" + _hkCount))
                                                            {
                                                                Debug.Log("[OrX Check Imports] === UPDATING EXTRAS IN " + _name + " number " + _hkCount + " CREATED BY " + _creator + " ===");

                                                                cn2.SetValue("extras", "True");
                                                                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + _hkCount + "-" + creatorName + ".orx");
                                                                _hkCount += 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            string _moveToLoc = _newDir + "/" + _name + "-" + _count + "-" + _creator + ".orx";
                                            string _nodeValue = _name;
                                            string _missionType = cn.GetValue("missionType");
                                            orxFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + _name + "-" + _count + "-" + _creator + ".orx");
                                            OrXLog.instance.DebugLog("[OrX Check Imports] === " + _name + " PROCESSED ===");
                                            orxFile.Save(_moveToLoc);

                                            ConfigNode dataFile = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creator + "/" + _creator + ".data");
                                            if (dataFile == null)
                                            {
                                                dataFile = new ConfigNode();
                                                dataFile.SetValue("creator", _creator, true);
                                            }
                                            dataFile.SetValue(_nodeValue, _missionType, true);
                                            dataFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creator + "/" + _creator + ".data");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log("[OrX Check Imports] === FILE ALREADY PROCESSED ===");

                            }
                        }
                    }
                }
            }

            #region STUFF

            List<string>.Enumerator _fileToDelete = OrXLoadedFileList.GetEnumerator();
            while(_fileToDelete.MoveNext())
            {
                if (_fileToDelete.Current != null)
                {
                    File.Delete(_fileToDelete.Current);
                }
            }
            _fileToDelete.Dispose();

            yield return new WaitForFixedUpdate();

            holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            files = new List<string>(Directory.GetFiles(holoKronLoc, "*.orx", SearchOption.AllDirectories));

            if (files != null)
            {
                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === Found " + files.Count + " files ===");
                OnScrnMsgUC("Found " + files.Count + " files .....");

                List<string>.Enumerator cfgsToAdd = files.GetEnumerator();
                while (cfgsToAdd.MoveNext())
                {
                    try
                    {
                        if (cfgsToAdd.Current != null)
                        {
                            ConfigNode fileNode = ConfigNode.Load(cfgsToAdd.Current);

                            if (fileNode != null && fileNode.HasNode("OrX"))
                            {
                                /*
                                ModuleDatabase = new List<string>();
                                int moduleCount = 0;
                                ConfigNode _modules = _file.GetNode("Mods");

                                if (_modules != null)
                                {
                                    foreach (ConfigNode.Value moduleCheck in _modules.values)
                                    {
                                        if ()
                                        moduleCount += 1;
                                        ModuleDatabase.Add(moduleCheck.value);
                                    }

                                    try
                                    {
                                        foreach (AvailablePart part in PartLoader.LoadedPartsList)
                                        {
                                            foreach (ConfigNode cn in part.partConfig.nodes)
                                            {
                                                if (cn.name == "MODULE")
                                                {
                                                    foreach (ConfigNode.Value cv in cn.values)
                                                    {
                                                        if (cv.name == "name")
                                                        {
                                                            List<string>.Enumerator moduleName = ModuleDatabase.GetEnumerator();
                                                            while (moduleName.MoveNext())
                                                            {
                                                                try
                                                                {
                                                                    if (moduleName.Current != null)
                                                                    {
                                                                        if (moduleName.Current == cv.value)
                                                                        {
                                                                            moduleCount -= 1;
                                                                            //ModuleDatabase.Remove(cv.value);
                                                                        }
                                                                    }
                                                                }
                                                                catch
                                                                {

                                                                }
                                                            }
                                                            moduleName.Dispose();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }

                                }

                                */

                                ConfigNode node = fileNode.GetNode("OrX");
                                string _HoloKronName = _file.GetValue("name");
                                bool ableToLoad = true;
                                bool _spawned = true;
                                string _soi = "";
                                int _hkCount = 0;

                                /*
                                if (moduleCount >= 0)
                                {
                                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD ===");
                                    ableToLoad = false;
                                    OnScrnMsgUC("UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD");

                                }

                                */
                                ableToLoad = true;
                                if (ableToLoad)
                                {
                                    foreach (ConfigNode spawnCheck in node.nodes)
                                    {
                                        //yield return new WaitForFixedUpdate();

                                        if (_spawned)
                                        {
                                            if (spawnCheck.name.Contains("OrXHoloKronCoords" + _hkCount))
                                            {
                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === FOUND HOLOKRON ... CHECKING SOI ===");

                                                _soi = spawnCheck.GetValue("SOI");
                                                if (_soi == soi)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] " + _HoloKronName + "'s current SOI '" + soi + "' matches HoloKron SOI '" + _soi + "'");

                                                    bool _challenge = false;
                                                    string missionType = spawnCheck.GetValue("missionType");

                                                    if (missionType == "CHALLENGE")
                                                    {
                                                        _challenge = true;
                                                    }

                                                    string targetCoords = spawnCheck.GetValue("Targets");
                                                    if (targetCoords == string.Empty)
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] " + _HoloKronName + " " + _hkCount + " Target string was empty!");
                                                    }
                                                    else
                                                    {
                                                        string[] data = targetCoords.Split(new char[] { ',' });

                                                        if (_challenge)
                                                        {
                                                            if (!OrXChallengeList.Contains(targetCoords))
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === ADDING COORDS TO CHALLENGE LIST ===");
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] " + targetCoords);
                                                                coordCount += 1;
                                                                OrXChallengeList.Add(targetCoords);

                                                                if (!OrXChallengeNameList.Contains(data[1]))
                                                                {
                                                                    OrXChallengeNameList.Add(data[1]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === GEO-CACHE LIST ALREADY CONTAINS THESE COORDS ===");
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] " + targetCoords);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!OrXCoordsList.Contains(targetCoords))
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === ADDING COORDS TO GEO-CACHE LIST ===");
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] " + targetCoords);
                                                                coordCount += 1;
                                                                OrXCoordsList.Add(targetCoords);

                                                                if (!OrXGeoCacheNameList.Contains(data[1]))
                                                                {
                                                                    OrXGeoCacheNameList.Add(data[1]);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === GEO-CACHE LIST ALREADY CONTAINS THESE COORDS ===");
                                                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] " + targetCoords);
                                                            }
                                                        }
                                                        //yield return new WaitForFixedUpdate();
                                                    }

                                                    string _spawnedString = spawnCheck.GetValue("spawned");
                                                    if (_spawnedString == "False")
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has not spawned ... ");
                                                        _spawned = false;
                                                    }
                                                    else
                                                    {
                                                        var complete = spawnCheck.GetValue("completed");
                                                        if (complete == "False")
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has not been completed ... END TRANSMISSION"); ;
                                                            _spawned = false;
                                                        }
                                                        else
                                                        {
                                                            OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has been completed ... CHECKING FOR EXTRAS"); ;
                                                            if (spawnCheck.HasValue("extras"))
                                                            {
                                                                var t = spawnCheck.GetValue("extras");
                                                                if (t == "False")
                                                                {
                                                                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has no extras ... END TRANSMISSION");
                                                                    _spawned = false;
                                                                }
                                                                else
                                                                {
                                                                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has extras ... CONTINUING");
                                                                    _hkCount += 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] " + _HoloKronName + " is not in the current SOI");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch
                    {

                    }
                }
                cfgsToAdd.Dispose();
                /*
                if (coordCount == 0)
                {
                    movingCraft = false;
                    PlayOrXMission = false;
                    checking = false;
                    OrXHCGUIEnabled = true;
                    GuiEnabledOrXMissions = false;
                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === Dinner Out is Cancelled ===");
                    OnScrnMsgUC("There are no HoloKrons in " + FlightGlobals.ActiveVessel.mainBody.name + "'s SOI that have not been spawned .....");
                    OnScrnMsgUC("Dinner Out is Cancelled .....");
                }
                else
                {
                    GuiEnabledOrXMissions = false;
                    movingCraft = false;
                    PlayOrXMission = false;
                    showTargets = false;
                    checking = false;
                    OrXHCGUIEnabled = true;
                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === HoloKrons found ===");
                    OnScrnMsgUC("Operation 'Dinner Out' is a go .....");
                }
                */
            }
            else
            {
                /*
                                GuiEnabledOrXMissions = false;
                                movingCraft = false;
                                PlayOrXMission = false;
                                showTargets = false;
                                checking = false;
                                OrXHCGUIEnabled = false;
                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === HoloKron List is empty ===");
                                OnScrnMsgUC("There are no HoloKrons in the current SOI");
                                OnScrnMsgUC("Dinner Out is Cancelled .....");
                                */
            }


            #endregion

        }

        public bool CheckExports(string holoName)
        {
            OrXLog.instance.DebugLog("[OrX Check Exports] === CHECKING  FOR " + holoName + " ===");
            creatorName = challengersName;
            string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + holoName + "/";
            List<string> scoreFiles = new List<string>(Directory.GetFiles(importLoc, "*.orx", SearchOption.AllDirectories));
            ConfigNode exportCheck = ConfigNode.Load(importLoc + holoName + "-0-" + creatorName + ".orx");

            if (scoreFiles != null)
            {
                OrXAppendCfg.instance.hkCount = scoreFiles.Count;
                HoloKronName = holoName;
                OrXLog.instance.DebugLog("[OrX Check Exports] === FOUND " + scoreFiles.Count + " HOLOKRONS IN " + HoloKronName + " ===");
                return true;
            }
            else
            {
                OrXLog.instance.DebugLog("[OrX Check Exports] === " + holoName + " NOT FOUND ===");
                return false;
            }
        }
        private void SetRanges(float _range)
        {
            bool _error = false;

            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
            while (v.MoveNext())
            {
                try
                {
                    if (v.Current != null)
                    {
                        OrXLog.instance.SetRange(v.Current, _range);
                    }
                }
                catch (Exception e)
                {
                    _error = true;
                    OrXLog.instance.DebugLog("[OrX Set Ranges] === RETRYING ===");
                    OrXLog.instance.DebugLog("[OrX Set Ranges] === Error: " + e);
                }
            }
            v.Dispose();

            if (_error)
            {
                SetRanges(_range);
            }
        }
        public string TimeSet(float num)
        {
            int h = (int)(num / 3600);
            int m = (int)((num - (3600 * h)) / 60);
            int s = (int)(num - ((h * 3600) + (m * 60)));
            return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00") + "." + num.ToString(".00").Split('.')[1];

            ///return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00") + "." + num.ToString(".00").Split('.')[1];
        }
        public void RefreshPaw(Part part)
        {
            IEnumerator<UIPartActionWindow> paw = FindObjectsOfType(typeof(UIPartActionWindow)).Cast<UIPartActionWindow>().GetEnumerator();
            while (paw.MoveNext())
            {
                if (paw.Current == null) continue;
                if (paw.Current.part == part)
                {
                    paw.Current.displayDirty = true;
                }
            }
            paw.Dispose();
        }
        public void ResetData()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                OrXSpawnHoloKron.instance.spawning = false;
            }

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
            maxSpeedSB0 = string.Empty;

            nameSB1 = string.Empty;
            timeSB1 = string.Empty;
            maxSpeedSB1 = string.Empty;

            nameSB2 = string.Empty;
            timeSB2 = string.Empty;
            maxSpeedSB2 = string.Empty;

            nameSB3 = string.Empty;
            timeSB3 = string.Empty;
            maxSpeedSB3 = string.Empty;

            nameSB4 = string.Empty;
            timeSB4 = string.Empty;
            maxSpeedSB4 = string.Empty;

            nameSB5 = string.Empty;
            timeSB5 = string.Empty;
            maxSpeedSB5 = string.Empty;

            nameSB6 = string.Empty;
            timeSB6 = string.Empty;
            maxSpeedSB6 = string.Empty;

            nameSB7 = string.Empty;
            timeSB7 = string.Empty;
            maxSpeedSB7 = string.Empty;

            nameSB8 = string.Empty;
            timeSB8 = string.Empty;
            maxSpeedSB8 = string.Empty;

            nameSB9 = string.Empty;
            timeSB9 = string.Empty;
            maxSpeedSB9 = string.Empty;

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
            urlKontinumm = "";
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

        public double GetDistance(Vector3d _currentLoc, Vector3d _targetLoc)
        {
            double _latDiff = 0;
            double _lonDiff = 0;
            double _altDiff = 0;

            if (_currentLoc.z <= _targetLoc.z)
            {
                _altDiff = _targetLoc.z - _currentLoc.z;
            }
            else
            {
                _altDiff = _currentLoc.z - _targetLoc.z;
            }

            if (_targetLoc.x >= 0)
            {
                if (_currentLoc.x >= _targetLoc.x)
                {
                    _latDiff = _currentLoc.x - _targetLoc.x;
                }
                else
                {
                    _latDiff = _targetLoc.x - _currentLoc.x;
                }
            }
            else
            {
                if (_currentLoc.x >= 0)
                {
                    _latDiff = _currentLoc.x - _targetLoc.x;
                }
                else
                {
                    if (_currentLoc.x <= _targetLoc.x)
                    {
                        _latDiff = _currentLoc.x - _targetLoc.x;
                    }
                    else
                    {

                        _latDiff = _targetLoc.x - _currentLoc.x;
                    }
                }
            }

            if (_targetLoc.y >= 0)
            {
                if (_currentLoc.y >= _targetLoc.y)
                {
                    _lonDiff = _currentLoc.y - _targetLoc.y;
                }
                else
                {
                    _lonDiff = _targetLoc.y - _currentLoc.y;
                }
            }
            else
            {
                if (_currentLoc.y >= 0)
                {
                    _lonDiff = _currentLoc.y - _targetLoc.y;
                }
                else
                {
                    if (_currentLoc.y <= _targetLoc.y)
                    {
                        _lonDiff = _currentLoc.y - _targetLoc.y;
                    }
                    else
                    {

                        _lonDiff = _targetLoc.y - _currentLoc.y;
                    }
                }
            }

            double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
            double _altDiffDeg = _altDiff * degPerMeter;
            double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
            double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;
            Debug.Log("[OrX Get Distance] === TARGET DISTANCE: " + _targetDistance + " meters ===");

            return _targetDistance;
        }

        #endregion

        #region Build Challenge

        IEnumerator FocusSwitchDelay(Vessel v)
        {
            yield return new WaitForSeconds(3);
            FlightGlobals.ForceSetActiveVessel(v);
            OrXLog.instance.ResetFocusKeys();
            SpawnMenu();
        }

        public void SpawnByOrX(Vector3d vect)
        {
            HoloKronName = "";
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            building = true;
            buildingMission = true;
            geoCache = true;
            OrXHCGUIEnabled = true;
            Goal = false;
            triggerVessel = FlightGlobals.ActiveVessel;
            //OrXSpawnHoloKron.instance.StartSpawn(vect, vect, false, true, true, HoloKronName, missionType);

            SetupHolo(FlightGlobals.ActiveVessel, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));

        }
        public void SetupHolo(Vessel v, Vector3d holoPosition)
        {
            //ResetData();
            //OrXLog.instance.SetFocusKeys();
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

            holoKronCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloKron/";
            holoKronFiles = new List<string>(Directory.GetFiles(holoKronCraftLoc, "*.craft", SearchOption.AllDirectories));

            sphLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/SPH/";
            sphFiles = new List<string>(Directory.GetFiles(sphLoc, "*.craft", SearchOption.AllDirectories));

            vabLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB/";
            vabFiles = new List<string>(Directory.GetFiles(vabLoc, "*.craft", SearchOption.AllDirectories));
            OrXLog.instance.building = true;
            startLocation = holoPosition;
            lastCoord = startLocation;
            showScores = false;
            movingCraft = false;
            spawned = true;
            GetShortTrackCenter(holoPosition);
        }

        public void ClearLastCoord()
        {
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
                            var g = _gates.Current.rootPart.FindModuleImplementing<ModuleOrXStage>();
                            if (g != null)
                            {
                                if (g._stageCount == locCount)
                                {
                                    OrXLog.instance.DebugLog("[OrX Clear Last Coord] === " + _gates.Current.vesselName + " found ===");
                                    _gates.Current.rootPart.AddModule("ModuleOrXJason", true);
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
                                lastCoord = new Vector3d(double.Parse(_locCount[1]), double.Parse(_locCount[2]), double.Parse(_locCount[3]));
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
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Clear Last Coord] === ERROR ... Still having issues === " + e);
                }
            }
        }
        public void GetShortTrackCenter(Vector3d _boidPos)
        {
            rangeColor = titleStyle;
            _getCenterDist = true;
            StartCoroutine(GetCenterShortTrackRoutine(_boidPos));
        }
        IEnumerator GetCenterShortTrackRoutine(Vector3d _boidPos)
        {
            if (_getCenterDist)
            {
                yield return new WaitForFixedUpdate();
                double _latDiff = 0;
                double _lonDiff = 0;
                double _altDiff = 0;

                if (FlightGlobals.ActiveVessel.altitude <= _boidPos.z)
                {
                    _altDiff = _boidPos.z - FlightGlobals.ActiveVessel.altitude;
                }
                else
                {
                    _altDiff = FlightGlobals.ActiveVessel.altitude - _boidPos.z;
                }

                if (_boidPos.x >= 0)
                {
                    if (FlightGlobals.ActiveVessel.latitude >= _boidPos.x)
                    {
                        _latDiff = FlightGlobals.ActiveVessel.latitude - _boidPos.x;
                    }
                    else
                    {
                        _latDiff = _boidPos.x - FlightGlobals.ActiveVessel.latitude;
                    }
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.latitude >= 0)
                    {
                        _latDiff = FlightGlobals.ActiveVessel.latitude - _boidPos.x;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.latitude <= _boidPos.x)
                        {
                            _latDiff = FlightGlobals.ActiveVessel.latitude - _boidPos.x;
                        }
                        else
                        {

                            _latDiff = _boidPos.x - FlightGlobals.ActiveVessel.latitude;
                        }
                    }
                }

                if (_boidPos.y >= 0)
                {
                    if (FlightGlobals.ActiveVessel.longitude >= _boidPos.y)
                    {
                        _lonDiff = FlightGlobals.ActiveVessel.longitude - _boidPos.y;
                    }
                    else
                    {
                        _lonDiff = _boidPos.y - FlightGlobals.ActiveVessel.latitude;
                    }
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.longitude >= 0)
                    {
                        _lonDiff = FlightGlobals.ActiveVessel.longitude - _boidPos.y;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.longitude <= _boidPos.y)
                        {
                            _lonDiff = FlightGlobals.ActiveVessel.longitude - _boidPos.y;
                        }
                        else
                        {

                            _lonDiff = _boidPos.y - FlightGlobals.ActiveVessel.longitude;
                        }
                    }
                }

                double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                double _altDiffDeg = _altDiff * degPerMeter;
                double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                targetDistance = _targetDistance;

                if (!_spawningVessel)
                {
                    if (targetDistance >= 3000)
                    {
                        if (targetDistance >= 4000)
                        {
                            _killPlace = true;
                            rangeColor = titleStyleRed;
                        }
                        else
                        {
                            _killPlace = false;
                            rangeColor = titleStyleYellow;
                        }
                    }
                    else
                    {
                        _killPlace = false;
                        rangeColor = titleStyle;
                    }

                }
                else
                {
                    if (targetDistance >= 6000)
                    {
                        if (targetDistance >= 8000)
                        {
                            _killPlace = true;
                            rangeColor = titleStyleRed;
                        }
                        else
                        {
                            _killPlace = false;
                            rangeColor = titleStyleYellow;
                        }
                    }
                    else
                    {
                        _killPlace = false;
                        rangeColor = titleStyle;
                    }

                }
                yield return new WaitForFixedUpdate();
                StartCoroutine(GetCenterShortTrackRoutine(_boidPos));
            }
        }
        IEnumerator GateKillDelayRoutine()
        {
            ClearLastCoord();
            yield return new WaitForSeconds(2.5f);
            _gateKillDelay = false;
            getNextCoord = true;
        }
        public void ChallengeAddNextCoord()
        {
            FlightGlobals.ActiveVessel.SetWorldVelocity(Vector3.zero);
            FlightGlobals.ActiveVessel.angularVelocity = Vector3.zero;
            FlightGlobals.ActiveVessel.angularMomentum = Vector3.zero;

            if (FlightGlobals.ActiveVessel != _HoloKron)
            {
                FlightGlobals.ForceSetActiveVessel(_HoloKron);
            }

            OnScrnMsgUC("Adding Current Location ..... ");

            if (!locAdded)
            {
                locAdded = true;
            }

            if (dakarRacing || shortTrackRacing)
            {
                movingCraft = true;
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
                // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                    + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                //OrXVesselMove.Instance.EndMove(true, false, true);
            }
            else
            {
                if (windRacing)
                {
                    if (FlightGlobals.ActiveVessel.altitude >= 0 && FlightGlobals.ActiveVessel.atmDensity >= 0.007)
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
                        startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);

                        teaseDelay = WindGUI.instance.teaseDelay;
                        // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                        CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                    }
                    else
                    {
                        OnScrnMsgUC("Unable to add coordinate to Wind Challenge if vessel is below water or not in an atmosphere");
                    }
                }

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

            GuiEnabledOrXMissions = true;
            OrXHCGUIEnabled = true;
            addCoords = true;
            movingCraft = true;
            addingMission = true;

            if (shortTrackRacing)
            {
                dakarRacing = false;
                spawningStartGate = false;
                getNextCoord = true;
            }

            if (dakarRacing)
            {
                saveLocalVessels = true;
                getNextCoord = false;
                SaveConfig(HoloKronName, false);
            }
        }

        public void SaveConfig(string holoName, bool append)
        {
            OrXLog.instance.SetRange(triggerVessel, 10000);
            if (challengersName != "")
            {
                creatorName = challengersName;
            }

            building = true;
            buildingMission = true;
            bool _continue = true;
            if (holoName != "" && holoName != string.Empty)
            {
                HoloKronName = holoName;
            }

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/");

            if (append)
            {
                int _hkCount = hkCount - 1;
                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + _hkCount + "-" + creatorName + ".orx");
                if (_file != null)
                {
                    ConfigNode _orxNode = _file.GetNode("OrX");
                    foreach (ConfigNode cn in _orxNode.nodes)
                    {
                        if (cn.name.Contains("OrXHoloKronCoords" + _hkCount))
                        {
                            cn.SetValue("extras", "True");
                            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + _hkCount + "-" + creatorName + ".orx");
                        }
                    }
                }
            }

            string hConfigLoc2 = UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx";
            OrXLog.instance.DebugLog("[OrX Save Config] === SAVING ===");

            _file = ConfigNode.Load(hConfigLoc2);
            if (_file == null)
            {
                _file = new ConfigNode();
                _file.AddValue("name", HoloKronName);
                _file.AddValue("creator", creatorName);
                _file.AddValue("count", hkCount);
                _file.AddValue("Kontinuum", connectToKontinuum);
                _file.AddNode("Mods");
                _file.AddNode("OrX");
                _file.Save(hConfigLoc2);
            }
            else
            {
                if (append)
                {
                    hkCount += 1;
                    hConfigLoc2 = UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx";
                    _file = ConfigNode.Load(hConfigLoc2);
                    if (_file == null)
                    {
                        _file = new ConfigNode();
                        _file.AddValue("name", HoloKronName);
                        _file.AddValue("creator", creatorName);
                        _file.AddValue("count", hkCount);
                        _file.AddValue("Kontinuum", connectToKontinuum);
                        _file.AddNode("Mods");
                        _file.AddNode("OrX");
                        _file.Save(hConfigLoc2);
                    }
                    else
                    {
                        _continue = false;
                    }
                }
            }

            ConfigNode mods = _file.GetNode("Mods");
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

            if (!_continue)
            {

            }
            else
            {
                if (geoCache)
                {
                    if (localSaveRange >= 1000)
                    {
                        localSaveRange = 1000;
                    }
                }
                else
                {
                    localSaveRange = 10000;
                }

                OnScrnMsgUC("Saving " + HoloKronName + " " + hkCount + " ....");
                ConfigNode node = _file.GetNode("OrX");

                if (addingMission)
                {
                    _mission = _file.GetNode("mission" + hkCount);
                    if (_mission == null)
                    {
                        _file.AddNode("mission" + hkCount);
                        _mission = _file.GetNode("mission" + hkCount);
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

                            ConfigNode _modules = _file.GetNode("Mods");
                            if (_modules == null)
                            {
                                _modules = new ConfigNode();
                                _modules = _file.AddNode("Mods");
                            }
                            _modules = _file.GetNode("Mods");

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
                        if (!addCoords)
                        {
                            OrXLog.instance.DebugLog("[OrX Add Mission] === SAVING LOCAL VESELS ===");

                            int count = 0;

                            double _latDiff = 0;
                            double _lonDiff = 0;
                            double _altDiff = 0;

                            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                            while (v.MoveNext())
                            {
                                try
                                {
                                    if (v.Current == null) continue;
                                    //if (v.Current.packed || !v.Current.loaded) continue;

                                    bool isKerb = false;
                                    if (v.Current.rootPart.Modules.Contains<KerbalEVA>())
                                    {
                                        isKerb = true;
                                    }

                                    bool hasKerb = false;
                                    List<Part>.Enumerator parts = v.Current.parts.GetEnumerator();
                                    while (parts.MoveNext())
                                    {
                                        if (parts.Current != null)
                                        {

                                            if (parts.Current.Modules.Contains<KerbalEVA>())
                                            {
                                                hasKerb = true;
                                            }
                                        }
                                    }
                                    parts.Dispose();

                                    if (!v.Current.rootPart.Modules.Contains<ModuleOrXMission>() && v.Current != triggerVessel && !isKerb)
                                    {
                                        OrXLog.instance.DebugLog("[OrX Add Mission] === RANGE CHECK ===");

                                        if (v.Current.altitude <= triggerVessel.altitude)
                                        {
                                            _altDiff = triggerVessel.altitude - v.Current.altitude;
                                        }
                                        else
                                        {
                                            _altDiff = v.Current.altitude - triggerVessel.altitude;
                                        }

                                        if (triggerVessel.latitude >= 0)
                                        {
                                            if (v.Current.latitude >= triggerVessel.latitude)
                                            {
                                                _latDiff = v.Current.latitude - triggerVessel.latitude;
                                            }
                                            else
                                            {
                                                _latDiff = triggerVessel.latitude - v.Current.latitude;
                                            }
                                        }
                                        else
                                        {
                                            if (v.Current.latitude >= 0)
                                            {
                                                _latDiff = v.Current.latitude - triggerVessel.latitude;
                                            }
                                            else
                                            {
                                                if (v.Current.latitude <= triggerVessel.latitude)
                                                {
                                                    _latDiff = v.Current.latitude - triggerVessel.latitude;
                                                }
                                                else
                                                {

                                                    _latDiff = triggerVessel.latitude - v.Current.latitude;
                                                }
                                            }
                                        }

                                        if (triggerVessel.longitude >= 0)
                                        {
                                            if (v.Current.longitude >= triggerVessel.longitude)
                                            {
                                                _lonDiff = v.Current.longitude - triggerVessel.longitude;
                                            }
                                            else
                                            {
                                                _lonDiff = triggerVessel.longitude - v.Current.latitude;
                                            }
                                        }
                                        else
                                        {
                                            if (v.Current.longitude >= 0)
                                            {
                                                _lonDiff = v.Current.longitude - triggerVessel.longitude;
                                            }
                                            else
                                            {
                                                if (v.Current.longitude <= triggerVessel.longitude)
                                                {
                                                    _lonDiff = v.Current.longitude - triggerVessel.longitude;
                                                }
                                                else
                                                {

                                                    _lonDiff = triggerVessel.longitude - v.Current.longitude;
                                                }
                                            }
                                        }

                                        OrXLog.instance.DebugLog("[OrX Add Mission] Vessel " + v.Current.vesselName + " Identified .......................");

                                        double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                                        double _altDiffDeg = _altDiff * degPerMeter;
                                        double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                                        double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;
                                        bool _inRange = false;

                                        OrXLog.instance.DebugLog("[OrX Add Mission] === RANGE: " + _targetDistance);

                                        if (v.Current.LandedOrSplashed)
                                        {
                                            if (_targetDistance <= localSaveRange)
                                            {
                                                _inRange = true;
                                            }
                                        }
                                        else
                                        {
                                            if (_targetDistance <= localSaveRange * 4)
                                            {
                                                _inRange = true;
                                            }
                                        }

                                        if (_inRange)
                                        {
                                            count += 1;

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
                                            ConfigNode _modules = _file.GetNode("Mods");
                                            if (_modules == null)
                                            {
                                                _modules = new ConfigNode();
                                                _modules = _file.AddNode("Mods");
                                            }
                                            _modules = _file.GetNode("Mods");

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
                                                toSave.rootPart.AddModule("ModuleOrXJason", true);
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
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
                        if (_HoloKron != null)
                        {
                            if (_HoloKron.parts.Count == 1)
                            {
                                _HoloKron.rootPart.explosionPotential *= 0.2f;
                                _HoloKron.rootPart.explode();

                            }
                        }
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
                    ConfigNode HoloKronNode = null;
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
                    if (node.HasNode("OrXHoloKronCoords" + hkCount))
                    {
                        OrXLog.instance.DebugLog("[OrX Save Config] === ERROR === HoloKron " + hkCount + " FOUND ===");

                        foreach (ConfigNode n in node.GetNodes("OrXHoloKronCoords" + hkCount))
                        {
                            if (n.GetValue("SOI") == FlightGlobals.ActiveVessel.mainBody.name)
                            {
                                HoloKronNode = n;
                                break;
                            }
                        }

                        if (HoloKronNode == null)
                        {
                            HoloKronNode = node.AddNode("OrXHoloKronCoords" + hkCount);
                            HoloKronNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                            HoloKronNode.AddValue("spawned", "False");
                            HoloKronNode.AddValue("extras", "False");
                            HoloKronNode.AddValue("unlocked", "False");
                            HoloKronNode.AddValue("tech", tech);

                            HoloKronNode.AddValue("missionName", HoloKronName + " " + hkCount);
                            HoloKronNode.AddValue("missionType", missionType);
                            HoloKronNode.AddValue("challengeType", challengeType);
                            HoloKronNode.AddValue("raceType", raceType);

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

                            HoloKronNode.AddValue("gold", Gold);
                            HoloKronNode.AddValue("silver", Silver);
                            HoloKronNode.AddValue("bronze", Bronze);

                            HoloKronNode.AddValue("completed", completed);
                            HoloKronNode.AddValue("count", hkCount);

                            HoloKronNode.AddValue("lat", _lat);
                            HoloKronNode.AddValue("lon", _lon);
                            HoloKronNode.AddValue("alt", _alt);
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

                        HoloKronNode.AddValue("missionName", HoloKronName + " " + hkCount);
                        HoloKronNode.AddValue("missionType", missionType);
                        HoloKronNode.AddValue("challengeType", challengeType);
                        HoloKronNode.AddValue("raceType", raceType);

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

                        HoloKronNode.AddValue("gold", Gold);
                        HoloKronNode.AddValue("silver", Silver);
                        HoloKronNode.AddValue("bronze", Bronze);

                        HoloKronNode.AddValue("completed", completed);
                        HoloKronNode.AddValue("count", hkCount);

                        HoloKronNode.AddValue("lat", _lat);
                        HoloKronNode.AddValue("lon", _lon);
                        HoloKronNode.AddValue("alt", _alt);
                    }

                    HoloKronNode.SetValue("Targets", FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + creatorName + "," + _lat + "," + _lon + "," + _alt + ","
                    + hkCount + "," + missionType + "," + challengeType + ":" + FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + creatorName + "," + _lat + "," + _lon + "," + _alt + ","
                    + hkCount + "," + missionType + "," + challengeType, true);

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

                    OrXLog.instance.DebugLog("[OrX Save Config] === HOLO CRAFT ENCRYPTED ===");

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

                            ConfigNode _modules = _file.GetNode("Mods");
                            if (_modules == null)
                            {
                                _modules = new ConfigNode();
                                _modules = _file.AddNode("Mods");
                            }
                            _modules = _file.GetNode("Mods");

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

                    if (saveLocalVessels)
                    {
                        OrXLog.instance.DebugLog("[OrX Save Config] === SAVING LOCAL VESELS ===");

                        int count = 0;

                        double _latDiff = 0;
                        double _lonDiff = 0;
                        double _altDiff = 0;

                        List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                        while (v.MoveNext())
                        {
                            try
                            {
                                if (v.Current == null) continue;
                                if (v.Current.packed || !v.Current.loaded) continue;
                                OrXLog.instance.DebugLog("[OrX Save Config] === CHECKING FOR EVA KERBAL ===");

                                bool isKerb = false;
                                if (v.Current.rootPart.Modules.Contains<KerbalEVA>())
                                {
                                    isKerb = true;
                                }


                                bool hasKerb = false;

                                List<Part>.Enumerator parts = v.Current.parts.GetEnumerator();
                                while (parts.MoveNext())
                                {
                                    if (parts.Current != null)
                                    {
                                        if (parts.Current.Modules.Contains<KerbalEVA>())
                                        {
                                            hasKerb = true;
                                        }
                                    }
                                }
                                parts.Dispose();

                                if (!v.Current.rootPart.Modules.Contains<ModuleOrXMission>() && v.Current != triggerVessel && !isKerb)
                                {
                                    OrXLog.instance.DebugLog("[OrX Save Config] === RANGE CHECK ===");

                                    if (v.Current.altitude <= triggerVessel.altitude)
                                    {
                                        _altDiff = triggerVessel.altitude - v.Current.altitude;
                                    }
                                    else
                                    {
                                        _altDiff = v.Current.altitude - triggerVessel.altitude;
                                    }

                                    if (triggerVessel.latitude >= 0)
                                    {
                                        if (v.Current.latitude >= triggerVessel.latitude)
                                        {
                                            _latDiff = v.Current.latitude - triggerVessel.latitude;
                                        }
                                        else
                                        {
                                            _latDiff = triggerVessel.latitude - v.Current.latitude;
                                        }
                                    }
                                    else
                                    {
                                        if (v.Current.latitude >= 0)
                                        {
                                            _latDiff = v.Current.latitude - triggerVessel.latitude;
                                        }
                                        else
                                        {
                                            if (v.Current.latitude <= triggerVessel.latitude)
                                            {
                                                _latDiff = v.Current.latitude - triggerVessel.latitude;
                                            }
                                            else
                                            {

                                                _latDiff = triggerVessel.latitude - v.Current.latitude;
                                            }
                                        }
                                    }

                                    if (triggerVessel.longitude >= 0)
                                    {
                                        if (v.Current.longitude >= triggerVessel.longitude)
                                        {
                                            _lonDiff = v.Current.longitude - triggerVessel.longitude;
                                        }
                                        else
                                        {
                                            _lonDiff = triggerVessel.longitude - v.Current.latitude;
                                        }
                                    }
                                    else
                                    {
                                        if (v.Current.longitude >= 0)
                                        {
                                            _lonDiff = v.Current.longitude - triggerVessel.longitude;
                                        }
                                        else
                                        {
                                            if (v.Current.longitude <= triggerVessel.longitude)
                                            {
                                                _lonDiff = v.Current.longitude - triggerVessel.longitude;
                                            }
                                            else
                                            {

                                                _lonDiff = triggerVessel.longitude - v.Current.longitude;
                                            }
                                        }
                                    }

                                    OrXLog.instance.DebugLog("[OrX Save Config] Vessel " + v.Current.vesselName + " Identified .......................");

                                    double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                                    double _altDiffDeg = _altDiff * degPerMeter;
                                    double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                                    double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;
                                    bool _inRange = false;

                                    OrXLog.instance.DebugLog("[OrX Save Config] === RANGE: " + _targetDistance);

                                    if (v.Current.LandedOrSplashed)
                                    {
                                        if (_targetDistance <= localSaveRange)
                                        {
                                            _inRange = true;
                                        }
                                    }
                                    else
                                    {
                                        if (_targetDistance <= localSaveRange * 4)
                                        {
                                            _inRange = true;
                                        }
                                    }

                                    if (_inRange)
                                    {
                                        count += 1;

                                        toSave = v.Current;
                                        string shipDescription = v.Current.vesselName + " local vessel from " + HoloKronName + " " + hkCount;
                                        OrXLog.instance.DebugLog("[OrX Save Config] Saving " + v.Current.vesselName + "'s orientation .......................");

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

                                        ConstructToSave = new ShipConstruct(HoloKronName, shipDescription, v.Current.parts[0]);
                                        craftConstruct = new ConfigNode("craft");
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

                                        OrXLog.instance.DebugLog("[OrX Save Config] Saving: " + v.Current.vesselName);
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

                                        ConfigNode _modules = _file.GetNode("Mods");
                                        if (_modules == null)
                                        {
                                            _modules = new ConfigNode();
                                            _modules = _file.AddNode("Mods");
                                        }
                                        _modules = _file.GetNode("Mods");

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

                                        OrXLog.instance.DebugLog("[OrX Save Config] === " + v.Current.vesselName + " ENCRYPTED ===");
                                        saveShip = false;
                                        OrXLog.instance.DebugLog("[OrX Save Config] " + v.Current.vesselName + " Saved to " + HoloKronName);
                                        OnScrnMsgUC("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
                                        if (bdaChallenge || outlawRacing)
                                        {
                                            toSave.rootPart.AddModule("ModuleOrXJason", true);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                OrXLog.instance.DebugLog("[OrX Mission] === EXCEPTION: " + e + " in " + HoloKronName + " ===");
                            }
                        }
                        v.Dispose();
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
                        //_HoloKron.rootPart.explosionPotential *= 0.2f;
                        //_HoloKron.rootPart.explode();
                        StartCoroutine(EndSave());
                    }
                    else
                    {
                        if (dakarRacing)
                        {
                            coordCount = 0;
                        }
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
        IEnumerator EndSave()
        {
            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Import/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx");
            yield return new WaitForSeconds(2f);
            FlightGlobals.ForceSetActiveVessel(triggerVessel);
            ResetData();
            challengeRunning = false;
            building = false;
            buildingMission = false;
            OrXHCGUIEnabled = false;
            localSaveRange = 1000;
            OrXPRExtension.PreOn("OrX Kontinuum");
            MainMenu();
        }

        #endregion

        #region Play Challenge

        public void OpenHoloKron(bool _geoCache, string holoName, int _hkCount_, Vessel holoKron, Vessel player)
        {
            StartCoroutine(OpenHoloKronRoutine(_geoCache, holoName, _hkCount_, holoKron, player));
        }
        IEnumerator OpenHoloKronRoutine(bool _geoCache, string holoName, int _hkCount_, Vessel holoKron, Vessel player)
        {
            challengeRunning = false;
            triggerVessel = player;
            _HoloKron = holoKron;
            hkCount = _hkCount_;
            geoCache = _geoCache;
            OrXLog.instance.DebugLog("[OrX Open HoloKron] === OPENING HOLOKRON === ");
            //OrXLog.instance.SetFocusKeys();
            yield return new WaitForFixedUpdate();
            //FlightGlobals.ForceSetActiveVessel(_HoloKron);
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
            string currentFile = "";

            yield return new WaitForFixedUpdate();

            if (holoName != "")
            {
                string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
                List<string> files = new List<string>(Directory.GetFiles(importLoc, "*.orx", SearchOption.AllDirectories));
                if (files != null)
                {
                    List<string>.Enumerator orxFile = files.GetEnumerator();
                    while (orxFile.MoveNext())
                    {
                        if (orxFile.Current != null)
                        {
                            if (orxFile.Current.Contains(holoName))
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
                                    creatorName = _file.GetValue("creator");

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
                                                    }

                                                    if (data.name == _raceType)
                                                    {
                                                        raceType = data.value;
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

            OrXHCGUIEnabled = true;
            showScores = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = true;
            movingCraft = false;
        }
        IEnumerator ChallengeStartDelay()
        {
            _timer = 0;
            Reach();
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
            Vector3 _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)triggerVessel.latitude, (double)triggerVessel.longitude, (double)_HoloKron.altitude + 5);
            Vector3 UpVect = (triggerVessel.ReferenceTransform.position - triggerVessel.mainBody.position).normalized;

            float localAlt = 5;
            float dropRate = Mathf.Clamp((localAlt * 2), 0.1f, 200);

            while (triggerVessel.altitude <= _HoloKron.altitude)
            {
                triggerVessel.IgnoreGForces(240);
                triggerVessel.SetWorldVelocity(Vector3.zero);
                dropRate = Mathf.Clamp((localAlt * 2), 0.1f, 200);

                if (dropRate <= 2f)
                {
                    dropRate = 2f;
                }

                if (dropRate > 3)
                {
                    triggerVessel.Translate(dropRate * Time.fixedDeltaTime * UpVect);
                }
                else
                {
                    triggerVessel.SetWorldVelocity(dropRate * UpVect);
                }

                localAlt -= dropRate * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            triggerVessel.IgnoreGForces(240);
            triggerVessel.SetWorldVelocity(Vector3.zero);
            Vector3 _holoPos = _HoloKron.mainBody.GetWorldSurfacePosition((double)_HoloKron.latitude, (double)_HoloKron.longitude, (double)_HoloKron.altitude);
            targetLoc = _holoPos;
            triggerVessel.SetPosition(_holoPos);
            yield return new WaitForFixedUpdate();

            Vector3 _goalPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_challengeStartLoc.x, (double)_challengeStartLoc.y, (double)_challengeStartLoc.z);
            yield return new WaitForFixedUpdate();

            _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)triggerVessel.latitude, (double)triggerVessel.longitude, (double)_challengeStartLoc.z);
            Vector3 startPosDirection = (_goalPos - _startPos).normalized;
            Quaternion _fixRot = Quaternion.identity;
            _fixRot = Quaternion.FromToRotation(triggerVessel.ReferenceTransform.up, startPosDirection) * triggerVessel.ReferenceTransform.rotation;
            triggerVessel.SetRotation(_fixRot, true);
            yield return new WaitForFixedUpdate();
            ScreenMessages.PostScreenMessage(new ScreenMessage("APPLYING BRAKES", 3, ScreenMessageStyle.UPPER_CENTER));
            OrXLog.instance.DebugLog("[OrX Place Challenger] === PLACING " + triggerVessel.vesselName + " ===");
            triggerVessel.Landed = false;
            triggerVessel.Splashed = false;

            while (!triggerVessel.LandedOrSplashed)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("PLACING .....", 0.5f, ScreenMessageStyle.UPPER_CENTER));
                yield return new WaitForFixedUpdate();
                triggerVessel.Translate((float)triggerVessel.radarAltitude * Time.fixedDeltaTime * -UpVect);
                triggerVessel.checkLanded();
                triggerVessel.checkSplashed();
            }
            if (!triggerVessel.isActiveVessel)
            {
                FlightGlobals.ForceSetActiveVessel(triggerVessel);
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
            string creatorName = _file.GetValue("creator");

            _blueprints_.Save(blueprintsFile);
            OnScrnMsgUC("Blueprints Available .....");
            OrXLog.instance.DebugLog("[OrX Start Mission] === '" + blueprintsFile + "' Available ===");
            OrXLog.instance.mission = false;

            ConfigNode node = _file.GetNode("OrX");

            foreach (ConfigNode spawnCheck in node.nodes)
            {
                if (spawnCheck.name.Contains("OrXHoloKronCoords"))
                {
                    ConfigNode HoloKronNode = node.GetNode("OrXHoloKronCoords" + hkCount);

                    if (HoloKronNode != null)
                    {
                        OrXLog.instance.DebugLog("[OrX Mission] === FOUND HoloKron === " + hkCount);

                        if (HoloKronNode.HasValue("completed"))
                        {
                            var t = HoloKronNode.GetValue("completed");
                            if (t == "False")
                            {
                                HoloKronNode.SetValue("completed", "True", true);

                                OrXLog.instance.DebugLog("[OrX Mission] === HoloKron " + hkCount + " was not completed ... CHANGING TO TRUE");
                                break;
                            }
                            else
                            {
                                OrXLog.instance.DebugLog("[OrX Mission] === HoloKron " + hkCount + " is already completed ... ");
                            }
                        }

                        OrXLog.instance.DebugLog("[OrX Start Mission] === DATA PROCESSED ===");
                    }
                }
            }

            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx");
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
                while (triggerVessel.srfSpeed <= 0.2f)
                {
                    if (challengeRunning)
                    {
                        ScreenMessages.PostScreenMessage(new ScreenMessage("TIMER STARTS WHEN YOU START MOVING", 0.5f, ScreenMessageStyle.UPPER_CENTER));
                        yield return null;
                    }
                    else
                    {
                        _continue = false;
                        yield return new WaitForFixedUpdate();
                    }
                }

                if (_continue)
                {
                    OrXLog.instance.DebugLog("[OrX Start Mission Delay] === Player Vessel Speed = " + FlightGlobals.ActiveVessel.srfSpeed + " ===");
                    OrXLog.instance.DebugLog("[OrX Start Mission Delay] === MISSION TIME START: " + TimeSet((float)FlightGlobals.ActiveVessel.missionTime) + " ===");
                    OrXLog.instance.DebugLog("[OrX Start Mission Delay] === Starting Challenge ===");
                    OnScrnMsgUC("Starting challenge ...........");
                    OrXLog.instance.DebugLog("[OrX Start Mission] === Starting Challenge ===");
                    gpsCount = 0;
                    stageStart = true;
                    _timerStageTime = "";
                    _timerTotalTime = "";
                }
            }
            checkingMission = true;
            showTargets = false;

            if (targetCoord != null)
            {
                targetCoord.rootPart.explode();
                targetCoord = null;
            }

            if (CoordDatabase.Count - gpsCount <= 0)
            {
                _timerOn = false;
                OrXLog.instance.mission = false;
                _showTimer = false;

                if (CoordDatabase.Count >= 0)
                {
                    stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + airTime + "," 
                        + _time + "," + CheatOptions.NoCrashDamage + ","
                        + CheatOptions.UnbreakableJoints + "," + CheatOptions.IgnoreMaxTemperature + "," 
                        + CheatOptions.InfinitePropellant + "," + CheatOptions.InfiniteElectricity);
                    StartCoroutine(SaveScore());
                }
                _timerStageTime = "00:00:00.00";
                _timerTotalTime = "00:00:00.00";

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
            else
            {
                if (stageStart)
                {
                    stageStart = false;
                    _time = 0;
                    _missionStartTime = FlightGlobals.ActiveVessel.missionTime;
                    missionTime = _missionStartTime;
                    StartTimer();
                }
                else
                {
                    _time = FlightGlobals.ActiveVessel.missionTime - missionTime;
                    missionTime = FlightGlobals.ActiveVessel.missionTime;
                    stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + airTime + ","
                        + _time + "," + CheatOptions.NoCrashDamage + ","
                        + CheatOptions.UnbreakableJoints + "," + CheatOptions.IgnoreMaxTemperature + ","
                        + CheatOptions.InfinitePropellant + "," + CheatOptions.InfiniteElectricity);
                    _timerStageTime = TimeSet((float)_time);
                    OnScrnMsgUC("STAGE TIME: " + _timerStageTime);
                    OrXLog.instance.DebugLog("[OrX Get Next Coord] === stage" + gpsCount + " = " + gpsCount + ", " + topSurfaceSpeed + ", " + maxDepth + ", " + airTime + ", " + _time + " ===");
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
                OrXLog.instance.DebugLog("[OrX Get Next Coord] === CHECKING DISTANCE ===");
                OrXTargetDistance.instance.TargetDistance(false, true, true, true, HoloKronName, nextLocation);
            }
        }

        public void StartTimer()
        {
            _missionStartTime = FlightGlobals.ActiveVessel.missionTime;
            _timerStageTime = "00:00:00.00";
            _timerTotalTime = "00:00:00.00";
            _timerOn = true;
            StartCoroutine(StageTimer());
        }
        public void StopTimer()
        {
            _timerOn = false;
            _timerStageTime = "00:00:00.00";
            _timerTotalTime = "00:00:00.00";
        }
        IEnumerator StageTimer()
        {
            if (_timerOn)
            {
                float _time = ((float)FlightGlobals.ActiveVessel.missionTime - (float)_missionStartTime);
                int _hours = (int)(_time / 3600);
                int _minutes = (int)((_time - (3600 * _hours)) / 60);
                int _seconds = (int)(_time - ((_hours * 3600) + (_minutes * 60)));
                _timerTotalTime = _hours.ToString("00") + ":" + _minutes.ToString("00") + ":" + _seconds.ToString("00") + "." + _time.ToString(".00").Split('.')[1];
                yield return new WaitForFixedUpdate();
                StartCoroutine(StageTimer());
            }
            else
            {
                StopTimer();
            }
        }
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
            if (FlightGlobals.ActiveVessel != triggerVessel && triggerVessel != null)
            {
                FlightGlobals.ForceSetActiveVessel(triggerVessel);
            }
        }
        public void ReturnToSender()
        {
            Reach();
            _timerStageTime = "";
            targetLoc = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_HoloKron.latitude, 
                (double)_HoloKron.longitude, (double)_HoloKron.altitude);
            OrXVesselMove.Instance.StartMove(triggerVessel, false, 15, false, true);
        }
        public void Refuel()
        {

        }

        #endregion

        #region Scoreboard Functions

        public void ExtractScoreboard(string creatorName, string holoName, int _hkCount_)
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

            _sbFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + holoName + "-" + _hkCount_ + "-" + creatorName + ".scoreboard");
            OrXLog.instance.DebugLog("[OrX Extract Score Board] === MISSION NODE FOUND ... SAVING === ");
            OnScrnMsgUC("Scoreboard saved to GameData/Orx/Export");
        }
        private void GetStats(string _name, int _entryCount)
        {
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
                    statsTotalAirTime = TimeSet(float.Parse(decryptedValue));
                }
                if (decryptedName == "totalTime")
                {
                    statsTime = TimeSet(float.Parse(decryptedValue));
                }

                if (decryptedName.Contains("stage"))
                {
                    scoreboardStats.Add(decryptedValue);
                }
            }
            OrXScoreboardStats.instance.OpenStatsWindow(HoloKronName, creatorName, statsName, statsTime, statsTotalAirTime, hkCount, statsMaxSpeed, statsMaxDepth, scoreboardStats);

            _extractScoreboard = true;
        }
        public void GetNextScoresFile(bool _addToScoreboard, List<string> _scoresData)
        {
            Reach();
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/scores/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/scores/");

            if (!_addToScoreboard)
            {
                File.Move(currentScoresFile, UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + creatorName + "/" + HoloKronName + "/scores/" + HoloKronName + "-" + hkCount + "-" + statsName + ".scores");
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
                                    if (_scoreFileFix.Contains(HoloKronName + "-" + hkCount + "-" + creatorName))
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
                                        Debug.Log("[OrX Import Score Board] === '" + scoreBoardFile.Current + "' DOES NOT MATCH '" + HoloKronName + "-" + hkCount + "-" + creatorName + "' ===");
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
            maxSpeedSB0 = string.Empty;

            nameSB1 = string.Empty;
            timeSB1 = string.Empty;
            maxSpeedSB1 = string.Empty;

            nameSB2 = string.Empty;
            timeSB2 = string.Empty;
            maxSpeedSB2 = string.Empty;

            nameSB3 = string.Empty;
            timeSB3 = string.Empty;
            maxSpeedSB3 = string.Empty;

            nameSB4 = string.Empty;
            timeSB4 = string.Empty;
            maxSpeedSB4 = string.Empty;

            nameSB5 = string.Empty;
            timeSB5 = string.Empty;
            maxSpeedSB5 = string.Empty;

            nameSB6 = string.Empty;
            timeSB6 = string.Empty;
            maxSpeedSB6 = string.Empty;

            nameSB7 = string.Empty;
            timeSB7 = string.Empty;
            maxSpeedSB7 = string.Empty;

            nameSB8 = string.Empty;
            timeSB8 = string.Empty;
            maxSpeedSB8 = string.Empty;

            nameSB9 = string.Empty;
            timeSB9 = string.Empty;
            maxSpeedSB9 = string.Empty;

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
                        timeSB0 = TimeSet(float.Parse(cv.value));
                    }
                }

                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB0 = cv.value;
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
                        timeSB1 = TimeSet(float.Parse(cv.value));
                    }
                }

                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB1 = cv.value;
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
                        timeSB2 = TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB2 = cv.value;
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
                        timeSB3 = TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB3 = cv.value;
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
                        timeSB4 = TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB4 = cv.value;
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
                        timeSB5 = TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB5 = cv.value;
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
                        timeSB6 = TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB6 = cv.value;
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
                        timeSB7 = TimeSet(float.Parse(cv.value));
                    }
                }

                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB7 = cv.value;
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
                        timeSB8 = TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB8 = cv.value;
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
                        timeSB9 = TimeSet(float.Parse(cv.value));
                    }
                }
                if (cv.name == "maxSpeed")
                {
                    maxSpeedSB9 = cv.value;
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
                                                statsTotalAirTime = TimeSet(float.Parse(decryptedValue));
                                                OrXLog.instance.DebugLog("[OrX Import Scores] === TOTAL AIR TIME === " + statsTotalAirTime + " ===");

                                            }
                                            if (decryptedName == "totalTime")
                                            {
                                                statsTime = TimeSet(float.Parse(decryptedValue));
                                                OrXLog.instance.DebugLog("[OrX Import Scores] === TOTAL TIME === " + statsTime + " ===");

                                            }
                                            
                                            if (decryptedName.Contains("stage"))
                                            {
                                                _count += 1;
                                                scoreboardStats.Add(decryptedValue);
                                                OrXLog.instance.DebugLog("[OrX Import Scores] === STAGE " + _count + " === " + decryptedValue + " ===");

                                            }
                                        }

                                        OrXScoreboardStats.instance.OpenStatsWindow(HoloKronName, creatorName, statsName, statsTime, statsTotalAirTime, hkCount, statsMaxSpeed, statsMaxDepth, scoreboardStats);
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
            string _creator = creatorName;
            bool _continue = false;
            scoreboardStats = _scoresData;

            if (_scoreboard_ == null)
            {
                OrXLog.instance.DebugLog("[OrX Import Scores] === " + HoloKronName + "-" + hkCount + "-" + creatorName + " SCOREBOARD NOT FOUND ... CREATING NEW ===");

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
                    Debug.Log("[OrX Import Scores] === " + _time + " TOTAL TIME ===");
                    Debug.Log("[OrX Import Scores] === " + _depth + " MAX DEPTH ACHIEVED ===");
                    Debug.Log("[OrX Import Scores] === " + _air + " TOTAL AIR TIME ===");
                    Debug.Log("[OrX Import Scores] === " + _speed + " TOP SPEED ===");


                    bool ammendListscoreboard0 = false;
                    string nameToRemovescoreboard0 = string.Empty;
                    double totalTimescoreboard0 = 0;
                    foreach (ConfigNode.Value cv in scoreboard0.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard0 = cv.value;
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

                    bool ammendListscoreboard1 = false;
                    string nameToRemovescoreboard1 = string.Empty;
                    double totalTimescoreboard1 = 0;
                    foreach (ConfigNode.Value cv in scoreboard1.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard1 = cv.value;
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

                    bool ammendListscoreboard2 = false;
                    string nameToRemovescoreboard2 = string.Empty;
                    double totalTimescoreboard2 = 0;
                    foreach (ConfigNode.Value cv in scoreboard2.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard2 = cv.value;
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

                    bool ammendListscoreboard3 = false;
                    string nameToRemovescoreboard3 = string.Empty;
                    double totalTimescoreboard3 = 0;
                    foreach (ConfigNode.Value cv in scoreboard3.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard3 = cv.value;
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

                    bool ammendListscoreboard4 = false;
                    string nameToRemovescoreboard4 = string.Empty;
                    double totalTimescoreboard4 = 0;
                    foreach (ConfigNode.Value cv in scoreboard4.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard4 = cv.value;
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

                    bool ammendListscoreboard5 = false;
                    string nameToRemovescoreboard5 = string.Empty;
                    double totalTimescoreboard5 = 0;
                    foreach (ConfigNode.Value cv in scoreboard5.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard5 = cv.value;
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

                    bool ammendListscoreboard6 = false;
                    string nameToRemovescoreboard6 = string.Empty;
                    double totalTimescoreboard6 = 0;
                    foreach (ConfigNode.Value cv in scoreboard6.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard6 = cv.value;
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

                    bool ammendListscoreboard7 = false;
                    string nameToRemovescoreboard7 = string.Empty;
                    double totalTimescoreboard7 = 0;
                    foreach (ConfigNode.Value cv in scoreboard7.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard7 = cv.value;
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

                    bool ammendListscoreboard8 = false;
                    string nameToRemovescoreboard8 = string.Empty;
                    double totalTimescoreboard8 = 0;
                    foreach (ConfigNode.Value cv in scoreboard8.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard8 = cv.value;
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

                    bool ammendListscoreboard9 = false;
                    string nameToRemovescoreboard9 = string.Empty;
                    double totalTimescoreboard9 = 0;
                    foreach (ConfigNode.Value cv in scoreboard9.values)
                    {
                        if (cv.name == "name")
                        {
                            nameToRemovescoreboard9 = cv.value;
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
                                timeSB0 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB0 = cv.value;
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
                                timeSB1 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB1 = cv.value;
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
                                timeSB2 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB2 = cv.value;
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
                                timeSB3 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB3 = cv.value;
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
                                timeSB4 = TimeSet(float.Parse(cv.value));
                            }
                            if (cv.name == "maxSpeed")
                            {
                                maxSpeedSB4 = cv.value;
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
                                timeSB5 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB5 = cv.value;
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
                                timeSB6 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB6 = cv.value;
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
                                timeSB7 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB7 = cv.value;
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
                                timeSB8 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB8 = cv.value;
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
                                timeSB9 = TimeSet(float.Parse(cv.value));
                            }
                        }
                        if (cv.name == "maxSpeed")
                        {
                            maxSpeedSB9 = cv.value;
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
                    _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx");
                  
                }
            }

            updatingScores = false;
            movingCraft = false;
        }
        IEnumerator SaveScore()
        {
            _timerOn = false;

            _mission = _file.GetNode("mission" + hkCount);
            _scoreboard_ = _mission.GetNode("scoreboard");

            if (_scoreboard_ == null)
            {
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

            if (stageTimes.Count >= 0)
            {
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

                            if (maxDepthChallenger >= _maxDepth)
                            {
                                maxDepthChallenger = _maxDepth;
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
                yield return new WaitForFixedUpdate();

                //OrXScoreboardStats.instance.OpenStatsWindow(HoloKronName, creatorName, challengersName, TimeSet((float)totalTimeChallenger),
                //    TimeSet((float)totalAirTimeChallenger), hkCount, maxSpeedChallenger, maxDepthChallenger, stageTimes);

                OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === TEMPORARY STAGE TIME LIST CREATED ===");
                OnScrnMsgUC("TOTAL TIME: " + TimeSet((float)totalTimeChallenger));

                ConfigNode scores = new ConfigNode();
                scores.AddValue("name", HoloKronName);
                scores.AddValue("count", hkCount);
                scores.AddValue("creator", creatorName);
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
                string _time = "null";
                string _speed = "null";
                string _depth = "null";
                string _air = "null";

                OrXLog.instance.DebugLog("[OrX Import Scores] === SCORE IMPORT FILE FOR " + HoloKronName + " FOUND ===");

                if (_file != null)
                {
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
                                    _time = cn.GetValue("totalTime");
                                    t = double.Parse(_time);
                                    _speed = cn.GetValue("maxSpeed");
                                    _depth = cn.GetValue("maxDepth");
                                    _air = cn.GetValue("totalAirTime");
                                }
                                else
                                {
                                    OrXLog.instance.DebugLog("[OrX Import Scores] === NO DATA CONTAINED IN SCORES FOR CHALLENGE IN '" + HoloKronName + " " + hkCount + "' ===");
                                }
                            }

                            Debug.Log("[OrX Import Scores] === " + HoloKronName + "-" + challengerNameImport + " SCORES CONTAINS DATA ===");
                            Debug.Log("[OrX Import Scores] === " + _time + " TOTAL TIME ===");
                            Debug.Log("[OrX Import Scores] === " + _depth + " MAX DEPTH ACHIEVED ===");
                            Debug.Log("[OrX Import Scores] === " + _air + " TOTAL AIR TIME ===");
                            Debug.Log("[OrX Import Scores] === " + _speed + " TOP SPEED ===");

                            bool ammendListscoreboard0 = false;
                            string nameToRemovescoreboard0 = string.Empty;
                            double totalTimescoreboard0 = 0;
                            foreach (ConfigNode.Value cv in scoreboard0.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard0 = cv.value;
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

                            bool ammendListscoreboard1 = false;
                            string nameToRemovescoreboard1 = string.Empty;
                            double totalTimescoreboard1 = 0;
                            foreach (ConfigNode.Value cv in scoreboard1.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard1 = cv.value;
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

                            bool ammendListscoreboard2 = false;
                            string nameToRemovescoreboard2 = string.Empty;
                            double totalTimescoreboard2 = 0;
                            foreach (ConfigNode.Value cv in scoreboard2.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard2 = cv.value;
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

                            bool ammendListscoreboard3 = false;
                            string nameToRemovescoreboard3 = string.Empty;
                            double totalTimescoreboard3 = 0;
                            foreach (ConfigNode.Value cv in scoreboard3.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard3 = cv.value;
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

                            bool ammendListscoreboard4 = false;
                            string nameToRemovescoreboard4 = string.Empty;
                            double totalTimescoreboard4 = 0;
                            foreach (ConfigNode.Value cv in scoreboard4.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard4 = cv.value;
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

                            bool ammendListscoreboard5 = false;
                            string nameToRemovescoreboard5 = string.Empty;
                            double totalTimescoreboard5 = 0;
                            foreach (ConfigNode.Value cv in scoreboard5.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard5 = cv.value;
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

                            bool ammendListscoreboard6 = false;
                            string nameToRemovescoreboard6 = string.Empty;
                            double totalTimescoreboard6 = 0;
                            foreach (ConfigNode.Value cv in scoreboard6.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard6 = cv.value;
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

                            bool ammendListscoreboard7 = false;
                            string nameToRemovescoreboard7 = string.Empty;
                            double totalTimescoreboard7 = 0;
                            foreach (ConfigNode.Value cv in scoreboard7.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard7 = cv.value;
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

                            bool ammendListscoreboard8 = false;
                            string nameToRemovescoreboard8 = string.Empty;
                            double totalTimescoreboard8 = 0;
                            foreach (ConfigNode.Value cv in scoreboard8.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard8 = cv.value;
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

                            bool ammendListscoreboard9 = false;
                            string nameToRemovescoreboard9 = string.Empty;
                            double totalTimescoreboard9 = 0;
                            foreach (ConfigNode.Value cv in scoreboard9.values)
                            {
                                if (cv.name == "name")
                                {
                                    nameToRemovescoreboard9 = cv.value;
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
                                foreach (ConfigNode.Value cv in scoresNode.values)
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
                                    foreach (ConfigNode.Value cv in scoresNode.values)
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
                                        foreach (ConfigNode.Value cv in scoresNode.values)
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
                                            foreach (ConfigNode.Value cv in scoresNode.values)
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
                                                foreach (ConfigNode.Value cv in scoresNode.values)
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
                                                    foreach (ConfigNode.Value cv in scoresNode.values)
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
                                                        foreach (ConfigNode.Value cv in scoresNode.values)
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
                                                            foreach (ConfigNode.Value cv in scoresNode.values)
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
                                                                foreach (ConfigNode.Value cv in scoresNode.values)
                                                                {
                                                                    scoreboard8.AddValue(cv.name, cv.value);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ammendListscoreboard9)
                                                                {
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
                                        timeSB0 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB0 = cv.value;
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
                                        timeSB1 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB1 = cv.value;
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
                                        timeSB2 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB2 = cv.value;
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
                                        timeSB3 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB3 = cv.value;
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
                                        timeSB4 = TimeSet(float.Parse(cv.value));
                                    }
                                    if (cv.name == "maxSpeed")
                                    {
                                        maxSpeedSB4 = cv.value;
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
                                        timeSB5 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB5 = cv.value;
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
                                        timeSB6 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB6 = cv.value;
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
                                        timeSB7 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB7 = cv.value;
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
                                        timeSB8 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB8 = cv.value;
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
                                        timeSB9 = TimeSet(float.Parse(cv.value));
                                    }
                                }
                                if (cv.name == "maxSpeed")
                                {
                                    maxSpeedSB9 = cv.value;
                                }

                            }

                            OrXLog.instance.DebugLog("[OrX Import Scores] === SCOREBOARD UPDATED FOR CHALLENGE IN '" + HoloKronName + " " + hkCount + "' ===");
                        }
                    }
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Import Scores] === " + HoloKronName + " NOT FOUND ... UNABLE TO LOAD SCORES ===");
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
            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx");
            _scoreSaved = true;
        }

        #endregion

        #region Menu Shortcuts

        public void Reach()
        {
            movingCraft = true;
            getNextCoord = false;
            GuiEnabledOrXMissions = true;
            _showSettings = false;
            connectToKontinuum = false;
        }
        public void MainMenu()
        {
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
            GuiEnabledOrXMissions = true;
            OrXHCGUIEnabled = true;
            connectToKontinuum = false;
            _showSettings = false;
            getNextCoord = false;
            movingCraft = true;

            salt = 0;
            challengeRunning = true;
            bdaChallenge = true;
            outlawRacing = false;
            geoCache = false;
            windRacing = false;
            Scuba = false;
            _showTimer = true;
            StartTimer();
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
            _importingScores = false;
            _extractScoreboard = false;
            showScores = true;
            PlayOrXMission = true;
            movingCraft = false;
            GuiEnabledOrXMissions = true;
            connectToKontinuum = false;
            _showSettings = false;
        }

        IEnumerator CheckEnemiesRoutine()
        {
            yield return new WaitForFixedUpdate();
            try
            {
                List<Vessel>.Enumerator _enemies = FlightGlobals.VesselsLoaded.GetEnumerator();
                while (_enemies.MoveNext())
                {
                    if (_enemies.Current != null)
                    {
                    }
                }
                _enemies.Dispose();
            }
            catch
            {
                StartCoroutine(CheckEnemiesRoutine());
            }
        }

        #endregion

        /// <summary>
        /// //////////////////////////////
        /// </summary>

        #region Main GUI

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!PauseMenu.isOpen)
                {
                    if (showTargets)
                    {
                        OrXLog.DrawRecticle(FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission), OrXLog.instance.HoloTargetTexture, new Vector2(32, 32));
                    }
                    GUI.backgroundColor = XKCDColors.DarkGrey;
                    GUI.contentColor = XKCDColors.DarkGrey;
                    GUI.color = XKCDColors.DarkGrey;

                    if (OrXHCGUIEnabled && !OrXScoreboardStats.instance.GuiEnabledStats)
                    {
                        WindowRectToolbar = GUI.Window(558917362, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
                    }
                }
            }
            else
            {
                if (HighLogic.LoadedSceneIsEditor)
                {
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
                            OrXLog.instance.DebugLog("[OrX Toggle GUI]: Hiding OrX HoloKron GUI");
                            OrXHCGUIEnabled = false;
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
                                WindowRectToolbar = new Rect(40, 50, toolWindowWidth, toolWindowHeight);

                                if (OrXLog.instance.PREnabled())
                                {
                                    OrXLog.instance.GetPRERanges();
                                    OnScrnMsgUC("Please remember to disable");
                                    OnScrnMsgUC("Physics Range Extender");
                                }

                                OrXHCGUIEnabled = true;

                                if (!_showTimer)
                                {
                                    if (!OrXSpawnHoloKron.instance.spawning)
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            SetRanges(8000);
                                            OrXLog.instance.SetTerrainLoad();
                                        }

                                        if (!buildingMission)
                                        {

                                            movingCraft = false;
                                            getNextCoord = false;
                                            MainMenu();

                                            Debug.Log("[OrX Toggle GUI]: Not building ... Checking if challenge is running ....");
                                            if (!challengeRunning)
                                            {
                                                Debug.Log("[OrX Toggle GUI]: Challenge not running ....");
                                                StartCoroutine(LoadDataFiles());
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
                    if (_pKarma == Karma)
                    {
                        _editor = true;
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
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum Settings", titleStyleLarge);
                line++;

                if (!_showMode)
                {

                    if (HighLogic.LoadedSceneIsFlight)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Mr Kleen's Magic Eraser", OrXGUISkin.button))
                        {
                            StartCoroutine(MrKleen());
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
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
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
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
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
                                    if (urlKontinumm != "")
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
                                            DownloadFile(urlKontinumm, pasKontinuum);
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

                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
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
                                    line += 0.2f;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Start Location Distance: " + Math.Round((targetDistance / 1000), 2) + " km", rangeColor);
                                    line++;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Stage Gate Count: " + locCount, titleStyle);
                                    line++;

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
                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "ADD NEXT STAGE", OrXGUISkin.button))
                                        {
                                            OrXLog.instance.DebugLog("[OrX Add Stage] ===== ADDING STAGE " + OrXSpawnHoloKron.instance.stageCount + " =====");
                                            addCoords = true;
                                            startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                                            if (shortTrackRacing)
                                            {
                                                OrXLog.instance.UpdateRangesOnFGReady();

                                                addingMission = true;
                                                getNextCoord = true;
                                                showTargets = false;
                                                movingCraft = false;
                                                OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 0, false, false);
                                            }
                                            else
                                            {
                                                if (dakarRacing)
                                                {
                                                    getNextCoord = false;
                                                    addingMission = false;
                                                    saveLocalVessels = true;
                                                    SaveConfig(HoloKronName, false);
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
                                            OrXLog.instance.UpdateRangesOnFGReady();

                                            OrXLog.instance.DebugLog("[OrX Save and Exit] === SAVING HOLOKRON ===");
                                            addCoords = false;
                                            addingMission = true;
                                            getNextCoord = false;
                                            _lastStage.vesselName = HoloKronName + " " + hkCount + " FINSH LINE";
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
                                    GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Spawn Craft", titleStyleLarge);
                                    line++;
                                    line += 0.2f;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Start Location Distance: " + Math.Round((targetDistance / 1000), 2) + " km", rangeColor);
                                    line++;
                                    line += 0.2f;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Current Altitude: " + Math.Round(_spawnedCraft.altitude, 2) + " meters ASL", rangeColor);

                                    if (!_settingAltitude)
                                    {
                                        line++;
                                        line += 0.2f;
                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "PLACE CRAFT", OrXGUISkin.button))
                                        {
                                            if (!_killPlace)
                                            {
                                                Reach();
                                                OrXLog.instance.UpdateRangesOnFGReady();
                                                _holdVesselPos = true;
                                                _spawningVessel = true;

                                                OrXLog.instance.DebugLog("[OrX Spawn Craft] ===== PLACING " + FlightGlobals.ActiveVessel.vesselName + " =====");
                                                OrXVesselMove.Instance.KillMove(true, false);
                                            }
                                            else
                                            {
                                                OnScrnMsgUC("You are too far from the start location");
                                                OnScrnMsgUC("Check your distance");
                                            }
                                        }
                                    }

                                    line++;
                                    line += 0.4f;
                                    _saveAltitude = GUI.HorizontalSlider(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), _saveAltitude, 2000, 4000, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalSliderThumb);
                                    line += 0.4f;
                                    GUI.Label(new Rect(0, (ContentTop + line * entryHeight) + line, WindowWidth, 20), "Radar Alt: " + String.Format("{0:0}", _saveAltitude) + " meters", centerLabelGreen);

                                    line++;
                                    line++;

                                    if (!_holdVesselPos)
                                    {
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
                                    if (outlawRacing)
                                    {
                                    }

                                    if (bdaChallenge)
                                    {
                                        GUI.Label(new Rect(20, 1, WindowWidth / 2, 20), "Challenge Total Time: ", titleStyleMedNoItal);
                                        GUI.Label(new Rect(WindowWidth / 2 + 10, 1, WindowWidth / 2, 20), _timerTotalTime, titleStyleMedGreen);
                                        GUI.Label(new Rect(20, (ContentTop + (line + 0.4f) * entryHeight), WindowWidth / 2, 20), "Salt: ", titleStyleMedNoItal);
                                        GUI.Label(new Rect(WindowWidth / 2 + 10, (ContentTop + (line + 0.4f) * entryHeight) + 0.5f, WindowWidth / 2, 20), Math.Round(salt, 3).ToString(), titleStyleMedGreen);
                                    }
                                    else
                                    {
                                        GUI.Label(new Rect(20, 1, WindowWidth / 2, 20), "Challenge Total Time: ", titleStyleMedNoItal);
                                        GUI.Label(new Rect(WindowWidth / 2 + 10, 1, WindowWidth / 2, 20), _timerTotalTime, titleStyleMedGreen);
                                        GUI.Label(new Rect(20, (ContentTop + (line + 0.4f) * entryHeight), WindowWidth / 2, 20), "Previous Stage Time: ", titleStyleMedNoItal);
                                        GUI.Label(new Rect(WindowWidth / 2 + 10, (ContentTop + (line + 0.4f) * entryHeight) + 0.5f, WindowWidth / 2, 20), _timerStageTime, titleStyleMedGreen);
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
                                        GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Total Time", titleStyleMedGreen);
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
                                        line++;
                                        line++;
                                        
                                        if (_timerStageTime != "")
                                        {
                                            if (outlawRacing)
                                            {
                                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "RETURN TO START", OrXGUISkin.button))
                                                {
                                                    ReturnToSender();
                                                }
                                            }
                                            else
                                            {
                                                //DrawStart(line);
                                            }
                                        }
                                        else
                                        {
                                            DrawStart(line);
                                        }

                                        line += 0.2f;
                                        line++;
                                        DrawCloseScoreboard(line);
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

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
                                        {
                                            _extractScoreboard = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (modCheckFail)
                                    {
                                        int scrollIndex = 0;
                                        int pmScrollIndex = 0;

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
                                                OrXSpawnHoloKron.instance.SpawnLocal(false, HoloKronName, new Vector3d());
                                            }
                                        }

                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
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
                                            line += 0.7f;

                                            if (!_editor)
                                            {
                                                line += 0.2f;

                                                DrawChallengerName(line);
                                                line++;
                                                line++;
                                                if (bdaChallenge)
                                                {
                                                    DrawKontinuumLogin1(line);
                                                    line++;
                                                    line += 0.2f;

                                                }

                                                if (holoOpen)
                                                {
                                                    DrawSpawnChallenge(line);
                                                    line++;
                                                    line += 0.2f;
                                                    if (_refuel)
                                                    {
                                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "GET NEW CRAFT", OrXGUISkin.button))
                                                        {

                                                        }
                                                        line++;
                                                        line += 0.2f;
                                                    }
                                                    else
                                                    {
                                                    }
                                                }
                                            }

                                            if (outlawRacing)
                                            {
                                                DrawShowScoreboard(line);
                                            }

                                            if (!_editor)
                                            {
                                                line++;
                                                line += 0.2f;

                                                DrawCancel(line);
                                            }
                                        }
                                        line += 0.2f;
                                        line++;
                                        DrawStart(line);

                                        if (_editor)
                                        {
                                            line++;
                                            line += 0.2f;

                                            DrawCancel(line);
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
                                line++;

                                if (!addCoords)
                                {
                                    if (!_spawningVessel)
                                    {
                                        DrawHoloKronName(line);
                                        line++;
                                        DrawHoloKronName2(line);
                                        line++;
                                        DrawCreatorName(line);
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
                                        if (bdaChallenge)
                                        {
                                            DrawSpawnVessel(line);
                                            line++;
                                            line += 0.2f;
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
                                                        OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 10, false, false);
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
                                            _spawningVessel = false;
                                            _getCenterDist = false;
                                            _addCrew = false;
                                            _holdVesselPos = false;
                                            if (!triggerVessel.isActiveVessel)
                                            {
                                                FlightGlobals.ForceSetActiveVessel(triggerVessel);
                                            }
                                        }
                                        line++;
                                    }
                                }
                                else
                                {
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Start Location Distance: " + Math.Round((targetDistance / 1000), 2) + " km", rangeColor);
                                    line++;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Stage Gate Count: " + locCount, titleStyle);
                                    line++;

                                    DrawAddCoords(line);
                                    line++;
                                    line += 0.5f;
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
                        }
                    }
                    else
                    {
                        if (!checking)
                        {
                            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum", titleStyleLarge);
                        }
                        else
                        {
                            GUI.Label(new Rect(0, 0, WindowWidth, 20), "HoloKron Target Info", titleStyleLarge);
                        }
                        line += 0.5f;

                        if (!challengeRunning)
                        {
                            if (checking)
                            {
                                line += 0.5f;

                                if (targetDistance <= 100000)
                                {
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron is " + Math.Round((targetDistance / 1000), 2) + " km away", titleStyleMedYellow);
                                    line++;
                                    line += 0.2f;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Altitude: " + Math.Round(_altitude, 1) + " meters", titleStyleMedYellow);
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
                                line += 0.5f;

                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "Stop HoloKron Scan", OrXGUISkin.button))
                                {
                                    StopScan(true);
                                }
                            }
                            else
                            {
                                if (showChallengelist)
                                {
                                    if (showCreatedHolokrons)
                                    {
                                        if (!showGeoCacheList)
                                        {
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "" + creatorName + "'s HoloKrons", titleStyleMedGreen);
                                            line++;
                                            line += 0.5f;

                                            List<string>.Enumerator holoNames = OrXChallengeNameList.GetEnumerator();
                                            while (holoNames.MoveNext())
                                            {
                                                if (holoNames.Current != null)
                                                {
                                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), holoNames.Current, OrXGUISkin.button))
                                                    {
                                                        if (HighLogic.LoadedSceneIsFlight)
                                                        {
                                                            hkCount = 0;

                                                            if (holoNames.Current.Contains("-"))
                                                            {
                                                                string[] data = holoNames.Current.Split(new char[] { '-' });
                                                                hkCount = int.Parse(data[1]);
                                                                HoloKronName = data[0];
                                                                missionType = data[7];
                                                                challengeType = data[8];
                                                                if (data[8] == "OUTLAW RACING")
                                                                { outlawRacing = true; }
                                                                if (data[8] == "BD ARMORY")
                                                                { bdaChallenge = true; }

                                                            }
                                                            else
                                                            {
                                                                HoloKronName = holoNames.Current;
                                                            }
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
                                                                            hkCount = int.Parse(data2[6]);
                                                                            showGeoCacheList = false;
                                                                            showCreatedHolokrons = false;
                                                                            showChallengelist = false;
                                                                            HoloKronName = data2[1];
                                                                            creatorName = data2[2];
                                                                            _editor = true;
                                                                            latMission = double.Parse(data2[3]);
                                                                            lonMission = double.Parse(data2[4]);
                                                                            altMission = double.Parse(data2[5]);
                                                                            worldPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission);
                                                                            if (data2[7] != "GEO-CACHE")
                                                                            {
                                                                                geoCache = false;
                                                                            }
                                                                            _challengeStartLoc = new Vector3d(latMission, lonMission, altMission);
                                                                            OrXLog.instance.SetRange(FlightGlobals.ActiveVessel, 10000);
                                                                            StartCoroutine(CheckInstalledMods(false));
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            orxChallengeList.Dispose();
                                                        }
                                                    }
                                                    line++;
                                                    line += 0.3f;
                                                }
                                            }
                                            holoNames.Dispose();
                                        }
                                        else
                                        {
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Local System HoloKrons", titleStyleMedYellow);
                                            line++;
                                            line += 0.6f;

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
                                                                                //OrXPRExtension.PreOff("OrX Kontinuum");
                                                                            }
                                                                            Reach();
                                                                            coordCount = 0;
                                                                            showGeoCacheList = false;
                                                                            showCreatedHolokrons = false;
                                                                            showChallengelist = false;
                                                                            checking = true;
                                                                            challengeRunning = true;
                                                                            HoloKronName = data2[1];
                                                                            creatorName = data2[2];

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
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Local HoloKron Creators", titleStyleMedBooger);
                                            line++;
                                            line += 0.6f;

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
                                                            creatorName = creatorNames.Current;
                                                            StartCoroutine(GetCreations(creatorNames.Current, false));
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
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron Creators", titleStyleMedGreen);
                                            line++;
                                            line += 0.6f;
                                            List<string>.Enumerator creatorNames = OrXChallengeCreatorList.GetEnumerator();
                                            while (creatorNames.MoveNext())
                                            {
                                                if (creatorNames.Current != null)
                                                {
                                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), creatorNames.Current, OrXGUISkin.button))
                                                    {
                                                        if (HighLogic.LoadedSceneIsFlight)
                                                        {
                                                            if (OrXLog.instance._preInstalled)
                                                            {
                                                                if (!OrXLog.instance.PREnabled())
                                                                {
                                                                    creatorName = creatorNames.Current;
                                                                    StartCoroutine(GetCreations(creatorNames.Current, true));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                creatorName = creatorNames.Current;
                                                                StartCoroutine(GetCreations(creatorNames.Current, true));
                                                            }
                                                        }
                                                    }
                                                    line++;
                                                    line += 0.2f;
                                                }
                                            }
                                            creatorNames.Dispose();
                                            //GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Kontinuum Challenges", titleStyleMed);
                                            //line++;
                                            //GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "<currently unavailable>", titleStyle);
                                        }
                                    }
                                    line += 0.5f;

                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
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
                                    line += 0.5f;
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Connect to Kontinuum", OrXGUISkin.button))
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
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Creator List", OrXGUISkin.button))
                                    {
                                        if (OrXLog.instance._preInstalled)
                                        {
                                            if (!OrXLog.instance.PREnabled())
                                            {
                                                GetCreatorList(true);
                                            }
                                        }
                                        else
                                        {
                                            GetCreatorList(true);
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
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Settings", OrXGUISkin.button))
                                    {
                                        _showSettings = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!mrKleen)
                            {
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Coordinates are " + Math.Round((targetDistance / 1000), 2) + " km away", titleStyle);
                                line++;
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Altitude: " + Math.Round(_altitude, 1) + " meters", titleStyle);
                                line++;
                                line += 0.5f;
                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE", OrXGUISkin.button))
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
                                    StartCoroutine(MrKleen());
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

                if (!_showTimer)
                {
                    if (!_spawningVessel)
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

        #endregion

        #region Coords GUI

        public void DrawAddCoords(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "ADD LOCATION", OrXGUISkin.button))
            {
                if (!_killPlace)
                {
                    movingCraft = true;
                    OrXVesselMove.Instance.EndMove(true, false, false);
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

            if (GUI.Button(saveRect, "MOVE TO STAGE " + locCount, OrXGUISkin.button))
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
                                OrXVesselMove.Instance.StartMove(_HoloKron, false, 10, false, true);
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), nameSB0, OrXGUISkin.button))
            {
                if (nameSB0 != "<empty>")
                {
                    _importingScores = false;

                    GetStats(nameSB0, 0);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 3, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), timeSB0, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB1, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB1 != "<empty>")
                {
                    GetStats(nameSB1, 1);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB1, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB2, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB2 != "<empty>")
                {
                    GetStats(nameSB2, 2);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB2, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB3, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB3 != "<empty>")
                {
                    GetStats(nameSB3, 3);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB3, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB4, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB4 != "<empty>")
                {
                    GetStats(nameSB4, 4);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB4, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB5, OrXGUISkin.button))
            {
                if (nameSB5 != "<empty>")
                {
                    _importingScores = false;

                    GetStats(nameSB5, 5);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB5, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB6, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB6 != "<empty>")
                {
                    GetStats(nameSB6, 6);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB6, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB7, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB7 != "<empty>")
                {
                    GetStats(nameSB7, 7);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB7, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB8, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB8 != "<empty>")
                {
                    GetStats(nameSB8, 8);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB8, OrXGUISkin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB9, OrXGUISkin.button))
            {
                _importingScores = false;

                if (nameSB9 != "<empty>")
                {
                    GetStats(nameSB9, 9);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB9, OrXGUISkin.box))
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
                        ExtractScoreboard(creatorName, HoloKronName, hkCount);
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
                if (_scoreSaved)
                {
                    OrXHCGUIEnabled = false;
                    ResetData();
                    MainMenu();
                }
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
                    StartCoroutine(CheckInstalledMods(true));
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
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "START CHALLENGE", OrXGUISkin.button))
                    {
                        if (challengersName != "" || challengersName != string.Empty)
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === NAME ENTERED - STARTING ===");
                            ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
                            if (playerData == null)
                            {
                                playerData = new ConfigNode();
                            }
                            playerData.SetValue("name", challengersName, true);
                            playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");

                            if (challengeType == "SCUBA KERB")
                            {
                                if (!challengeRunning)
                                {
                                    if (disablePRE)
                                    {
                                        //OrXPRExtension.PreOff("OrX Kontinuum");
                                    }


                                    //GuiEnabledOrXMissions = false;
                                    challengeRunning = true;
                                    geoCache = false;
                                    // OrXLog.instance.ResetFocusKeys();
                                    //FlightGlobals.ForceSetActiveVessel(triggerVessel);
                                    triggerVessel = FlightGlobals.ActiveVessel;
                                    StartCoroutine(ChallengeStartDelay());
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

                                        //GuiEnabledOrXMissions = false;
                                        challengeRunning = true;
                                        geoCache = false;
                                        // OrXLog.instance.ResetFocusKeys();
                                        //FlightGlobals.ForceSetActiveVessel(triggerVessel);
                                        triggerVessel = FlightGlobals.ActiveVessel;
                                        StartCoroutine(ChallengeStartDelay());
                                    }
                                }
                                else
                                {
                                    ScreenMessages.PostScreenMessage(new ScreenMessage("Get into a vehicle to start the challenge", 4, ScreenMessageStyle.UPPER_CENTER));
                                }
                            }
                        }
                        else
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === PLEASE ENTER CHALLENGER NAME ===");

                            OnScrnMsgUC("Please enter a challenger name");
                        }
                    }
                }
                else
                {
                    if (!HighLogic.LoadedSceneIsFlight)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", OrXGUISkin.button))
                        {
                            GetCreatorList(true);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SCAN FOR HOLOKRON", OrXGUISkin.button))
                        {
                            _editor = false;
                            Reach();
                            challengeRunning = true;
                            showGeoCacheList = false;
                            showCreatedHolokrons = false;
                            showChallengelist = false;
                            checking = true;
                            PlayOrXMission = false;
                            GuiEnabledOrXMissions = false;
                            movingCraft = false;
                            showScores = false;
                            OrXTargetDistance.instance.TargetDistance(true, true, false, true, HoloKronName, _challengeStartLoc);
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

                        GuiEnabledOrXMissions = false;
                        challengeRunning = true;
                        geoCache = true;
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
                            GetCreatorList(true);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SCAN FOR HOLOKRON", OrXGUISkin.button))
                        {
                            _editor = false;
                            Reach();
                            challengeRunning = true;
                            showGeoCacheList = false;
                            showCreatedHolokrons = false;
                            showChallengelist = false;
                            checking = true;
                            PlayOrXMission = false;
                            GuiEnabledOrXMissions = false;
                            movingCraft = false;
                            showScores = false;
                            OrXTargetDistance.instance.TargetDistance(true, true, false, true, HoloKronName, new Vector3d(latMission, lonMission, altMission));
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

        public void DrawHoloKronName(float line)
        {
            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Name your HoloKron below", titleStyleYellow);
        }
        public void DrawHoloKronName2(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Name: ", leftLabelBooger);
            HoloKronName = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, ((WindowWidth / 3) * 2) - 10, entryHeight), HoloKronName);
        }
        public void DrawCreatorName(float line)
        {
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Creator: ", leftLabelBooger);
            creatorName = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, ((WindowWidth / 3) * 2) - 10, entryHeight), creatorName);
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
                        OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

                        //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS W[ind/S] ===");

                    }
                }
            }
            else
            {
                if (Scuba)
                {
                    if (GUI.Button(bfRect, "SCUBA KERB", OrXGUISkin.button))
                    {
                        if (!locAdded)
                        {
                            //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE - BD ARMORY ===");
                            //challengeType = "BD ARMORY";
                            OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                            //bdaChallenge = true;
                            //windRacing = false;
                            //Scuba = false;
                            //outlawRacing = false;
                        }
                        else
                        {
                            //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS SCUBA ===");
                            OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

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
                                //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS BD ARMORY ===");
                                OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
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
                                    OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE - BD ARMORY ===");
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

                                    if (_pKarma == Karma)
                                    {
                                    }
                                    else
                                    {
                                        //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                        //OnScrnMsgUC("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                                    }
                                }
                                else
                                {
                                    OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
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
                                if (_pKarma == Karma)
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

        public void DrawSpawnVessel(float line)
        {
            if (!_spawningVessel)
            {
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, entryHeight), "SPAWN MENU", OrXGUISkin.button))
                {
                    _holdVesselPos = false;
                    _spawningVessel = true;
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
                    OrXSpawnHoloKron.instance._spawnCraftFile = true;
                    OrXSpawnHoloKron.instance.CraftSelect(false, true);
                }
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
                if (GUI.Button(bfRect, "", OrXGUISkin.button))
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
            urlKontinumm = GUI.TextField(new Rect((WindowWidth / 3), ContentTop + line * entryHeight, (WindowWidth * 0.66f) - 10, entryHeight), urlKontinumm);
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
                    OrXSpawnHoloKron.instance.CraftSelect(false, false);
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
                if (!HoloKronName.Contains("."))
                {
                    if (creatorName != "" || creatorName != string.Empty)
                    {
                        if (!creatorName.Contains("."))
                        {
                            if (missionDescription0 != string.Empty && missionDescription0 != "")
                            {
                                if (!geoCache)
                                {
                                    if (addCoords || bdaChallenge)
                                    {
                                        if (GUI.Button(saveRect, "SAVE AND EXIT", OrXGUISkin.button))
                                        {
                                            OrXLog.instance.DebugLog("[OrX Mission] === CREATOR NAME ENTERED  ===");
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
                                                        _lastStage = null;
                                                        triggerVessel = FlightGlobals.ActiveVessel;
                                                        hkSpawned = false;
                                                        buildingMission = true;
                                                        CoordDatabase = new List<string>();
                                                        addCoords = true;
                                                        addingMission = false;
                                                        saveLocalVessels = false;
                                                        OrXSpawnHoloKron.instance.stageCount = 0;
                                                        startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                                                        boidPos = startLocation;
                                                        _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx");
                                                        if (_file == null)
                                                        {
                                                            SaveConfig(HoloKronName, false);
                                                        }
                                                        else
                                                        {
                                                            if (OrXLog.instance._debugLog && _pKarma == Karma)
                                                            {
                                                                string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + challengersName + "/" + HoloKronName + "/";
                                                                List<string> _orxFiles = new List<string>(Directory.GetFiles(importLoc, "*.orx", SearchOption.AllDirectories));
                                                                OrXLog.instance.DebugLog("[OrX Append Cfg] === FOUND " + _orxFiles.Count + " HOLOKRONS ===");
                                                                OrXAppendCfg.instance.EnableGui(_orxFiles.Count, HoloKronName);
                                                            }
                                                            else
                                                            {
                                                                OnScrnMsgUC(HoloKronName + " already exists ....");

                                                                OnScrnMsgUC("Please enter a new name ....");

                                                            }
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
                                                triggerVessel = FlightGlobals.ActiveVessel;
                                                movingCraft = true;
                                                spawningStartGate = false;
                                                _getCenterDist = false;
                                                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + challengersName + "/" + HoloKronName + "/" + HoloKronName + "-0-" + challengersName + ".orx");
                                                if (_file == null)
                                                {
                                                    addCoords = false;
                                                    SaveConfig(HoloKronName, false);
                                                }
                                                else
                                                {
                                                    if (OrXLog.instance._debugLog && _pKarma == Karma)
                                                    {
                                                        string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + challengersName + "/" + HoloKronName + "/";
                                                        List<string> _orxFiles = new List<string>(Directory.GetFiles(importLoc, "*.orx", SearchOption.AllDirectories));
                                                        OrXLog.instance.DebugLog("[OrX Append Cfg] === FOUND " + _orxFiles.Count + " HOLOKRONS ===");
                                                        OrXAppendCfg.instance.EnableGui(_orxFiles.Count, HoloKronName);
                                                    }
                                                    else
                                                    {
                                                        OnScrnMsgUC(HoloKronName + " already exists ....");

                                                        OnScrnMsgUC("Please enter a new name ....");

                                                    }
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
                    ScreenMessages.PostScreenMessage(new ScreenMessage("HoloKron names cannot contain a '.'", 1, ScreenMessageStyle.UPPER_CENTER));
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
                        StartCoroutine(GetCreations(creatorName, true));
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