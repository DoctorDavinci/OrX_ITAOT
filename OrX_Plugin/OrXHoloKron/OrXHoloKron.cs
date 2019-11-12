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
        #region Variables

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
        public static bool hasAddedButton = false;
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
        int locCount = 0;

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
        public static int[] seeds = new int[] {
            269, 316, 876, 9, 569, 159, 262, 822, 412, 972, 105, 665, 255, 358, 1375, 51,
            98, 1115, 248, 808, 398, 501, 91, 651, 241, 344, 904, 37, 597, 187, 747, 337,
            384, 487, 77, 180, 1197, 330, 890, 23, 583, 173, 276, 1293, 426, 16, 119, 679,
        };

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

        string statsName = "";
        string statsTime = "";
        double statsMaxSpeed = 0;
        string statsTotalAirTime = "";
        double statsMaxDepth = 0;
        bool _modePassword = false;

        static GUIStyle leftLabel = new GUIStyle()
        {
            alignment = TextAnchor.UpperLeft,
            normal = { textColor = Color.white }
        };
        static GUIStyle leftLabelGreen = new GUIStyle()
        {
            alignment = TextAnchor.UpperLeft,
            normal = { textColor = Color.green }
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
        static GUIStyle titleStyleMedYellowL = new GUIStyle(leftLabelYellow)
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
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.green }
        };
        static GUIStyle titleStyle = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter
        };
        static GUIStyle titleStyleMedNoItal = new GUIStyle(centerLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        static GUIStyle titleStyleMed = new GUIStyle(centerLabel)
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
        static GUIStyle titleStyleLarge = new GUIStyle(centerLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        public List<string> scoreboardStats;
        bool scoreboardStatsWindow = false;

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

        #endregion
        
        private readonly List<AvailablePart> parts = new List<AvailablePart>();

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
            KontinuumDirectoryList = new List<string>();
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
            AddToolbarButton();
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
                }
            }

            if (HighLogic.LoadedSceneIsFlight)
            {
                GameEvents.onVesselSOIChanged.Add(checkSOI);
                GameEvents.onGameStateLoad.Add(resetHoloKronSystem);
            }
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (holoOpen)
                {
                    //triggerVessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
                }
            }
        }

        #endregion

        #region Kontinuum Connect

        // FTP
        private void CreateConnectionFTP(string hostIP, string userName, string password)
        {
            KontinuumClient = new Kontinuum.OrXKontinuum(@hostIP, userName, password);
        }
        private void UploadFileFTP(string remoteFile, string localFile)
        {
            KontinuumClient.Upload(remoteFile, @localFile);

        }
        private void DownloadFileFTP(string remoteFile, string localFile)
        {
            KontinuumClient.Download(remoteFile, @localFile);
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
            string loc = UrlDir.ApplicationRootPath + "GameData/OrX/Kontinuum/" + _webLoc;
            Uri webLoc = new Uri(_webLoc);

            try
            {
                fileDownloader = new WebClient();
                fileDownloader.DownloadFileAsync(webLoc, _localSaveLoc);
                fileDownloader.Dispose();
                connectToKontinuum = false;
                _downloading = false;
                ScreenMsg("Welcome to the Kontinuum " + challengersName + " ....");

            }
            catch
            {
                Debug.Log("[OrX Download File - WEB] ==================================================");
                Debug.Log("[OrX Download File - WEB] ===== Unable to establish a connection ....  =====");
                Debug.Log("[OrX Download File - WEB] ===== FILE LINK: " + _webLoc + " =====");
                Debug.Log("[OrX Download File - WEB] ==================================================");
                ScreenMsg("Unable to establish a connection ....");
                ScreenMsg("Check if the file link is valid ....");
            }
            _downloading = false;

        }

        #endregion

        #region Utilities

        IEnumerator CheckInstalledMods()
        {
            getNextCoord = false;
            movingCraft = true;
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
                        count2 += 1;
                        _orxFilePartModules.Add(cv.name);
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

                bool _continue = true;
                if (count != 0 || count2 != 0)
                {
                    _continue = false;
                }

                if (!_continue)
                {
                    Debug.Log("[OrX Check Installed Mods] === COUNT: " + count + " ===");

                    List<string>.Enumerator _leftovers = _orxFileMods.GetEnumerator();
                    while (_leftovers.MoveNext())
                    {
                        if (_leftovers.Current != null)
                        {
                            modCheckFail = true;
                            movingCraft = false;

                            Debug.Log("[OrX Check Installed Mods] === " + _leftovers.Current + " NOT INSTALLED ===");
                        }
                    }
                    _leftovers.Dispose();

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
                }
                else
                {
                    Debug.Log("[OrX Check Installed Mods] === MODS CHECKED OUT ===");
                    movingCraft = false;
                    modCheckFail = false;
                    OrXSpawnHoloKron.instance.SpawnLocal(true, HoloKronName, new Vector3d());
                }
            }
        }

        public void checkSOI(GameEvents.HostedFromToAction<Vessel, CelestialBody> data)
        {
            OrXLog.instance.DebugLog("[OrX Check SOI] === CHECKING ===");

            if (soi != FlightGlobals.ActiveVessel.mainBody.name)
            {
                OrXLog.instance.DebugLog("[OrX Check SOI] === '" + soi + "' doesn't match '" + FlightGlobals.ActiveVessel.mainBody.name + "' ===");
            }
        }
        private void resetHoloKronSystem(ConfigNode data)
        {
            challengeRunning = false;
            ScreenMsg("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
            locCount = 0;
            locAdded = false;
            building = false;
            buildingMission = false;
            addCoords = false;
            OrXHCGUIEnabled = false;
            MainMenu();
            ResetData();
        }
        public void Dummy() { }
        public void ScreenMsg(string _text)
        {
            if (!_blink)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage(_text, 4, ScreenMessageStyle.UPPER_CENTER));
            }
            else
            {
                _blink = false;
                _blinking = true;
                StartCoroutine(ScreenMsgBlink(_text));
            }
        }
        public IEnumerator ScreenMsgBlink(string _text)
        {
            if (_blinking)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage(_text + " " + OrXSpawnHoloKron.instance._vesselName, 0.9f, ScreenMessageStyle.UPPER_CENTER));
                yield return new WaitForSeconds(1.1f);
                StartCoroutine(ScreenMsgBlink(_text));
            }
        }
        public void AddToolbarButton()
        {
            string OrXDir = "OrX/Plugin/";

            if (!hasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(OrXDir + "OrX_icon", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.SPH 
                    | ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPACECENTER, buttonTexture);
                hasAddedButton = true;
            }
        }

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
                    ScreenMsg("No HoloKrons detected .....");
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
                    ScreenMsg("No HoloKrons detected .....");
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
                ScreenMsg("Found " + files.Count + " files .....");

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
                                ScreenMsg("UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD");

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
        }

        IEnumerator SetDebug()
        {
            OrXLog.instance._mode = true;
            _settings0 = true;
            OrXLog.instance._debugLog = true;
            yield return new WaitForSeconds(5);
            //OrXLog.instance._mode = false;
        }
        public void LoadResetDelay()
        {
            StartCoroutine(LoadDelay());
        }
        IEnumerator LoadDelay()
        {
            while (HighLogic.LoadedScene == GameScenes.LOADING)
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

                    try
                    {
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
                                                string _count = cn.GetValue("count");
                                                string _newDir = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creator + "/" + _name + "/";
                                                if (!Directory.Exists(_newDir))
                                                    Directory.CreateDirectory(_newDir);
                                                string _moveToLoc = _newDir + "/" + _name + "-" + _count + "-" + _creator + ".orx";
                                                string _nodeValue = _name;
                                                string _missionType = cn.GetValue("missionType");

                                                try
                                                {
                                                    if (!File.Exists(_moveToLoc))
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Check Imports] === " + _name + " PROCESSED ===");

                                                        File.Move(cfgsToMove.Current, _moveToLoc);
                                                    }
                                                    else
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Check Imports] === " + _name + " AREADY EXISTS ===");
                                                    }

                                                }
                                                catch (Exception e)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Check Imports - MOVING FILE ERROR] === COPYING ===");
                                                    orxFile.Save(_moveToLoc);
                                                    OrXLog.instance.DebugLog("[OrX Check Imports - MOVING FILE ERROR] === " + e + " ===");

                                                }

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
                    catch (Exception e)
                    {
                        Debug.Log("[OrX Check Imports - CRITICAL ERROR] === " + e + " ===");
                    }
                }
            }

            #region STUFF

            
            yield return new WaitForFixedUpdate();

            holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            files = new List<string>(Directory.GetFiles(holoKronLoc, "*.orx", SearchOption.AllDirectories));

            if (files != null)
            {
                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === Found " + files.Count + " files ===");
                ScreenMsg("Found " + files.Count + " files .....");

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
                                    ScreenMsg("UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD");

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
                    ScreenMsg("There are no HoloKrons in " + FlightGlobals.ActiveVessel.mainBody.name + "'s SOI that have not been spawned .....");
                    ScreenMsg("Dinner Out is Cancelled .....");
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
                    ScreenMsg("Operation 'Dinner Out' is a go .....");
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
                ScreenMsg("There are no HoloKrons in the current SOI");
                ScreenMsg("Dinner Out is Cancelled .....");
                */
            }
            

                            #endregion

        }

        private void GetCenterShortTrack()
        {
            double lat1 = 0;
            double lat2 = double.MaxValue;
            double lon1 = 0;
            double lon2 = double.MaxValue;
            double centerLat = 0;
            double centerLon = 0;
            double latDiff = 0;
            double lonDiff = 0;
            List<string>.Enumerator _centerCheck = CoordDatabase.GetEnumerator();
            while (_centerCheck.MoveNext())
            {
                try
                {
                    if (_centerCheck.Current != null)
                    {
                        string[] data = _centerCheck.Current.Split(new char[] { ',' });

                        double _lat1 = Math.Max(lat1, double.Parse(data[0]));
                        lat1 = _lat1;
                        double _lat2 = Math.Min(lat2, double.Parse(data[0]));
                        lat2 = _lat2;

                        double _lon1 = Math.Max(lon1, double.Parse(data[1]));
                        lon1 = _lon1;
                        double _lon2 = Math.Min(lon2, double.Parse(data[1]));
                        lon2 = _lon2;
                    }
                }
                catch
                {

                }
            }

            if (lat1 <= 0)
            {
                if (lat2 <= lat1)
                {
                    latDiff = (lat2 - lat1) / 2;

                    centerLat = lat1 + latDiff;
                }
                else
                {
                    latDiff = (lat1 - lat2) / 2;

                    if (lat2 >= 0)
                    {
                        centerLat = lat2 + latDiff;
                    }
                    else
                    {
                        centerLat = lat2 + latDiff;
                    }
                }
            }
            else
            {
                if (lat2 <= lat1)
                {
                    latDiff = (lat1 - lat2) / 2;

                    if (lat2 >= 0)
                    {
                        centerLat = lat1 - latDiff;
                    }
                    else
                    {
                        centerLat = lat2 + latDiff;
                    }
                }
                else
                {
                    latDiff = (lat2 - lat1) / 2;

                    centerLat = lat1 + latDiff;
                }
            }

            if (lon1 <= 0)
            {
                if (lon2 <= lon1)
                {
                    lonDiff = (lon2 - lon1) / 2;

                    centerLon = lon1 + lonDiff;
                }
                else
                {
                    lonDiff = (lon1 - lon2) / 2;

                    if (lon2 >= 0)
                    {
                        centerLon = lon2 + lonDiff;
                    }
                    else
                    {
                        centerLon = lon2 + lonDiff;
                    }
                }
            }
            else
            {
                if (lon2 <= lon1)
                {
                    lonDiff = (lon1 - lon2) / 2;

                    if (lon2 >= 0)
                    {
                        centerLon = lon1 - lonDiff;
                    }
                    else
                    {
                        centerLon = lon2 + lonDiff;
                    }
                }
                else
                {
                    lonDiff = (lon2 - lon1) / 2;

                    centerLon = lon1 + lonDiff;
                }
            }

            boidPos = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
            Vector3d _center = new Vector3d(centerLat, centerLon, FlightGlobals.ActiveVessel.altitude + 50);
            FlightGlobals.ActiveVessel.SetPosition(_center, true);
            //OrXSpawnHoloKron.instance.SpawnGates(true, HoloKronName, _center);
        }
        public void GateSpawnComplete()
        {
            //FlightGlobals.ForceSetActiveVessel(triggerVessel);
            OrXLog.instance.ResetFocusKeys();
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
                ScreenMsg("Operation 'Dinner Out' was cancelled .....");
                ResetData();
            }
        }

        public void PlaceCraft()
        {
            movingCraft = false;
            //FlightGlobals.ForceSetActiveVessel(_HoloKron);
            OrXHCGUIEnabled = true;
        }
        IEnumerator MrKleen()
        {
            killingChallenge = true;
            ScreenMsg("The Kontinuum is calling a maid .....");
            getNextCoord = false;
            movingCraft = true;
            GuiEnabledOrXMissions = true;

            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
            while (v.MoveNext())
            {
                if (v.Current == null) continue;
                if (v.Current.packed || !v.Current.loaded) continue;
                if (!v.Current.isActiveVessel)
                {
                    v.Current.rootPart.AddModule("ModuleOrXDestroyVessel");
                    yield return new WaitForFixedUpdate();
                }
            }
            v.Dispose();
            getNextCoord = false;
            movingCraft = false;
            GuiEnabledOrXMissions = false;
            killingChallenge = false;
            ScreenMsg("The slate is being swept clean .....");
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
        public void ResetData()
        {
            OrXSpawnHoloKron.instance.spawning = false;
            startGate = null;
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

            raceType = "";
            loginName = "";
            connectToKontinuum = false;
            pasKontinuum = "";
            _labelConnect = "Connect to Kontinuum";
            urlKontinumm = "";

            _KontinuumConnect = false;

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

        #endregion


        #region Build Challenge

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
            //FlightGlobals.ForceSetActiveVessel(_HoloKron);
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

            ScreenMsg("Adding Current Location ..... ");

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
                        ScreenMsg("Unable to add coordinate to Wind Challenge if vessel is below water or not in an atmosphere");
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
                        ScreenMsg("Unable to add coordinate to Scuba Challenge if vessel is not Splashed");
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
                    mods.SetValue(_installedMods.Current, "Mod", true);
                }
            }
            _installedMods.Dispose();
            /*
            List<string>.Enumerator _pm = ModuleDatabase.GetEnumerator();
            while (_pm.MoveNext())
            {
                if (_pm.Current != null)
                {
                    mods.SetValue(_pm.Current, "PartModule", true);
                }
            }
            _pm.Dispose();
            */
            if (!_continue)
            {

            }
            else
            {
                if (geoCache)
                {
                    //localSaveRange = 1000;
                }
                else
                {
                    localSaveRange = 8000;
                }

                ScreenMsg("Saving " + HoloKronName + " " + hkCount + " ....");
                ConfigNode node = _file.GetNode("OrX");

                if (addingMission)
                {
                    _mission = _file.GetNode("mission" + hkCount);
                    if (_mission == null)
                    {
                        _file.AddNode("mission" + hkCount);
                        _mission = _file.GetNode("mission" + hkCount);

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
                            location.AddValue("lat", _HoloKron.latitude);
                            location.AddValue("lon", _HoloKron.longitude);
                            location.AddValue("alt", _HoloKron.altitude);
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

                            ConfigNode craftFile = craftData.AddNode("craft");
                            ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloKronName + "</b></color>");
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
                            ScreenMsg("<color=#cfc100ff><b>" + craftToAddMission + " Saved</b></color>");
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

                                        if (v.Current.altitude <= _HoloKron.altitude)
                                        {
                                            _altDiff = _HoloKron.altitude - v.Current.altitude;
                                        }
                                        else
                                        {
                                            _altDiff = v.Current.altitude - _HoloKron.altitude;
                                        }

                                        if (_HoloKron.latitude >= 0)
                                        {
                                            if (v.Current.latitude >= _HoloKron.latitude)
                                            {
                                                _latDiff = v.Current.latitude - _HoloKron.latitude;
                                            }
                                            else
                                            {
                                                _latDiff = _HoloKron.latitude - v.Current.latitude;
                                            }
                                        }
                                        else
                                        {
                                            if (v.Current.latitude >= 0)
                                            {
                                                _latDiff = v.Current.latitude - _HoloKron.latitude;
                                            }
                                            else
                                            {
                                                if (v.Current.latitude <= _HoloKron.latitude)
                                                {
                                                    _latDiff = v.Current.latitude - _HoloKron.latitude;
                                                }
                                                else
                                                {

                                                    _latDiff = _HoloKron.latitude - v.Current.latitude;
                                                }
                                            }
                                        }

                                        if (_HoloKron.longitude >= 0)
                                        {
                                            if (v.Current.longitude >= _HoloKron.longitude)
                                            {
                                                _lonDiff = v.Current.longitude - _HoloKron.longitude;
                                            }
                                            else
                                            {
                                                _lonDiff = _HoloKron.longitude - v.Current.latitude;
                                            }
                                        }
                                        else
                                        {
                                            if (v.Current.longitude >= 0)
                                            {
                                                _lonDiff = v.Current.longitude - _HoloKron.longitude;
                                            }
                                            else
                                            {
                                                if (v.Current.longitude <= _HoloKron.longitude)
                                                {
                                                    _lonDiff = v.Current.longitude - _HoloKron.longitude;
                                                }
                                                else
                                                {

                                                    _lonDiff = _HoloKron.longitude - v.Current.longitude;
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
                                            ScreenMsg("<color=#cfc100ff><b>Saving: " + v.Current.vesselName + "</b></color>");

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
                                            ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloKronName + " " + hkCount + "</b></color>");
                                            craftConstruct.CopyTo(craftFile);
                                            craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + v.Current.vesselName + "-" + count + ".craft");
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

                                            OrXLog.instance.DebugLog("[OrX Add Mission] === " + v.Current.vesselName + " ENCRYPTED ===");
                                            saveShip = false;
                                            OrXLog.instance.DebugLog("[OrX Add Mission] " + v.Current.vesselName + " Saved to " + HoloKronName);
                                            ScreenMsg("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
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
                        _HoloKron.rootPart.explosionPotential *= 0.2f;
                        _HoloKron.rootPart.explode();
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
                    hkCount = 0;
                    ConfigNode HoloKronNode = null;

                    foreach (ConfigNode cn in node.nodes)
                    {
                        if (cn.name.Contains("OrXHoloKronCoords" + hkCount))
                        {
                            OrXLog.instance.DebugLog("[OrX Save Config] === HoloKron " + hkCount + " FOUND ===");
                            cn.SetValue("extras", "True");
                            hkCount += 1;
                        }
                    }

                    if (node.HasNode("OrXHoloKronCoords" + hkCount))
                    {
                        OrXLog.instance.DebugLog("[OrX Save Config] === ERROR === HoloKron " + hkCount + " FOUND AGAIN ===");

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
                    toSave.vesselName = HoloKronName + " " + hkCount;
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
                    ScreenMsg("<color=#cfc100ff><b>Saving: " + toSave.vesselName + "</b></color>");

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

                    OrXLog.instance.DebugLog("[OrX Save Config] === " + toSave.vesselName + " ENCRYPTED ===");
                    saveShip = false;
                    ScreenMsg("<color=#cfc100ff><b>" + toSave.vesselName + " Saved</b></color>");
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
                            location.AddValue("lat", _HoloKron.latitude);
                            location.AddValue("lon", _HoloKron.longitude);
                            location.AddValue("alt", _HoloKron.altitude);
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

                            ConfigNode craftFile = craftData.AddNode("craft");
                            ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloKronName + "</b></color>");
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
                            ScreenMsg("<color=#cfc100ff><b>" + craftToAddMission + " Saved</b></color>");
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

                                    if (v.Current.altitude <= _HoloKron.altitude)
                                    {
                                        _altDiff = _HoloKron.altitude - v.Current.altitude;
                                    }
                                    else
                                    {
                                        _altDiff = v.Current.altitude - _HoloKron.altitude;
                                    }

                                    if (_HoloKron.latitude >= 0)
                                    {
                                        if (v.Current.latitude >= _HoloKron.latitude)
                                        {
                                            _latDiff = v.Current.latitude - _HoloKron.latitude;
                                        }
                                        else
                                        {
                                            _latDiff = _HoloKron.latitude - v.Current.latitude;
                                        }
                                    }
                                    else
                                    {
                                        if (v.Current.latitude >= 0)
                                        {
                                            _latDiff = v.Current.latitude - _HoloKron.latitude;
                                        }
                                        else
                                        {
                                            if (v.Current.latitude <= _HoloKron.latitude)
                                            {
                                                _latDiff = v.Current.latitude - _HoloKron.latitude;
                                            }
                                            else
                                            {

                                                _latDiff = _HoloKron.latitude - v.Current.latitude;
                                            }
                                        }
                                    }

                                    if (_HoloKron.longitude >= 0)
                                    {
                                        if (v.Current.longitude >= _HoloKron.longitude)
                                        {
                                            _lonDiff = v.Current.longitude - _HoloKron.longitude;
                                        }
                                        else
                                        {
                                            _lonDiff = _HoloKron.longitude - v.Current.latitude;
                                        }
                                    }
                                    else
                                    {
                                        if (v.Current.longitude >= 0)
                                        {
                                            _lonDiff = v.Current.longitude - _HoloKron.longitude;
                                        }
                                        else
                                        {
                                            if (v.Current.longitude <= _HoloKron.longitude)
                                            {
                                                _lonDiff = v.Current.longitude - _HoloKron.longitude;
                                            }
                                            else
                                            {

                                                _lonDiff = _HoloKron.longitude - v.Current.longitude;
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
                                        ScreenMsg("<color=#cfc100ff><b>Saving: " + v.Current.vesselName + "</b></color>");

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
                                        ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloKronName + " " + hkCount + "</b></color>");
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

                                        OrXLog.instance.DebugLog("[OrX Save Config] === " + v.Current.vesselName + " ENCRYPTED ===");
                                        saveShip = false;
                                        OrXLog.instance.DebugLog("[OrX Save Config] " + v.Current.vesselName + " Saved to " + HoloKronName);
                                        ScreenMsg("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
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
                        ResetData();
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
                            spawningStartGate = false;
                            Vector3d vect = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude + 10);
                            OrXSpawnHoloKron.instance.StartSpawn(vect, vect, false, true, true, HoloKronName, missionType);
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
        string _currentOrXFile = "";

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
                                                            if (locEncryptedName == "pas")
                                                            {
                                                                pas = OrXLog.instance.Decrypt(_loc.value);
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
                                                + "/Ships/" + _type + crafttosave + ".craft";

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
                                                    }
                                                }
                                                catch (IndexOutOfRangeException e)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Open HoloKron] HoloKron config file processed ...... ");
                                                }
                                            }
                                            firstCoords.Dispose();
                                        }

                                        //ImportScores();

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
            double ta = triggerVessel.altitude;
            triggerVessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
            triggerVessel.IgnoreGForces(240);
            _placingChallenger = true;
            ScreenMsg("PLACING " + triggerVessel.vesselName + " AT START POSITION");
            Debug.Log("[OrX Start Challenge] === PLACING " + triggerVessel.vesselName + " AT START POSITION ===");
            Vector3 _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)triggerVessel.latitude, (double)triggerVessel.longitude, (double)_HoloKron.altitude + 5);

            triggerVessel.SetPosition(_startPos);

            triggerVessel.angularVelocity = Vector3.zero;
            triggerVessel.angularMomentum = Vector3.zero;
            triggerVessel.SetWorldVelocity(Vector3d.zero);
            Vector3 _holoPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_HoloKron.latitude, (double)_HoloKron.longitude, (double)_HoloKron.altitude);
            triggerVessel.SetPosition(_holoPos);

            Vector3 _goalPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission);
            _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)triggerVessel.latitude, (double)triggerVessel.longitude, (double)altMission);
            Vector3 startPosDirection = (_goalPos - _startPos).normalized;

            Quaternion _fixRot = Quaternion.identity;
            _fixRot = Quaternion.FromToRotation(triggerVessel.ReferenceTransform.up, startPosDirection) * triggerVessel.ReferenceTransform.rotation;
            triggerVessel.SetRotation(_fixRot, true);
            yield return new WaitForFixedUpdate();
            Vector3 UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            float localAlt = (float)triggerVessel.radarAltitude;
            float mod = 2;
            ScreenMessages.PostScreenMessage(new ScreenMessage("APPLYING BRAKES", 3, ScreenMessageStyle.UPPER_CENTER));

            OrXLog.instance.DebugLog("[OrX Place Challenger] === PLACING " + triggerVessel.vesselName + " ===");
            float dropRate = Mathf.Clamp((localAlt / mod), 0.1f, 200);
            triggerVessel.Landed = false;
            triggerVessel.Splashed = false;

            while (!triggerVessel.LandedOrSplashed)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("PLACING .....", 0.5f, ScreenMessageStyle.UPPER_CENTER));
                yield return new WaitForFixedUpdate();

                dropRate = Mathf.Clamp((localAlt / mod), 0.1f, 200);

                if (dropRate > 3)
                {
                    triggerVessel.Translate(dropRate * Time.fixedDeltaTime * -UpVect);
                }
                else
                {
                    if (dropRate <= 1)
                    {
                        dropRate = 1;
                    }

                    triggerVessel.SetWorldVelocity(dropRate * -UpVect);
                }

                localAlt -= dropRate * Time.fixedDeltaTime;

                triggerVessel.checkLanded();
                triggerVessel.checkSplashed();
            }
            FlightGlobals.ForceSetActiveVessel(triggerVessel);
            OrXLog.instance.ResetFocusKeys();
            OrXLog.instance.DebugLog("[OrX Start Mission Delay] === Starting Delay ===");
            GuiEnabledOrXMissions = false;
            challengeRunning = true;
            OrXHCGUIEnabled = false;
            stageStart = true;
            holoOpen = false;
            ScreenMessages.PostScreenMessage(new ScreenMessage("WAITING FOR VESSEL TO SETTLE", 0.7f, ScreenMessageStyle.UPPER_CENTER));
            yield return new WaitForSeconds(1);
            ScreenMessages.PostScreenMessage(new ScreenMessage("WAITING FOR VESSEL TO SETTLE", 0.7f, ScreenMessageStyle.UPPER_CENTER));
            yield return new WaitForSeconds(1);
            ScreenMessages.PostScreenMessage(new ScreenMessage("WAITING FOR VESSEL TO SETTLE", 0.7f, ScreenMessageStyle.UPPER_CENTER));

            while (triggerVessel.srfSpeed <= 0.1f)
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("TIMER STARTS WHEN YOU START MOVING", 0.5f, ScreenMessageStyle.UPPER_CENTER));
                yield return null;
            }
            OrXLog.instance.DebugLog("[OrX Start Mission Delay] === Player Vessel Speed = " + FlightGlobals.ActiveVessel.srfSpeed + " ===");
            OrXLog.instance.DebugLog("[OrX Start Mission Delay] === MISSION TIME START: " + TimeSet((float)FlightGlobals.ActiveVessel.missionTime) + " ===");
            OrXLog.instance.DebugLog("[OrX Start Mission Delay] === Starting Challenge ===");
            StartChallenge();
        }
        public void StartChallenge()
        {
            holoOpen = false;
            if (challengeRunning)
            {
                if (geoCache)
                {
                    challengeRunning = false;
                    PlayOrXMission = false;
                    showScores = false;
                    string creatorName = _file.GetValue("creator");

                    _blueprints_.Save(blueprintsFile);
                    ScreenMsg("Blueprints Available .....");
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

                    _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName+ ".orx");
                    OrXHCGUIEnabled = false;
                    checking = false;
                    GuiEnabledOrXMissions = false;
                    ResetData();
                }
                else
                {
                    ScreenMsg("Starting challenge ...........");
                    OrXLog.instance.DebugLog("[OrX Start Mission] === Starting Challenge ===");
                    gpsCount = 0;
                    GetNextCoord();

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

                                //GetNextCoord();
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
        }
        public void GetNextCoord()
        {
            GuiEnabledOrXMissions = false;
            challengeRunning = true;
            checkingMission = true;
            showTargets = false;

            if (targetCoord != null)
            {
                targetCoord.rootPart.explode();
                targetCoord = null;
            }
            double _time = FlightGlobals.ActiveVessel.missionTime - missionTime;
            missionTime = FlightGlobals.ActiveVessel.missionTime;

            if (CoordDatabase.Count - gpsCount <= 0)
            {
                OrXLog.instance.mission = false;

                if (CoordDatabase.Count >= 0)
                {
                    stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + airTime + "," 
                        + _time + "," + CheatOptions.NoCrashDamage + ","
                        + CheatOptions.UnbreakableJoints + "," + CheatOptions.IgnoreMaxTemperature + "," 
                        + CheatOptions.InfinitePropellant + "," + CheatOptions.InfiniteElectricity);
                    StartCoroutine(SaveScore());
                }
                OrXHCGUIEnabled = true;
                GuiEnabledOrXMissions = true;
                challengeRunning = false;
                PlayOrXMission = true;
                showScores = true;

                if (blueprintsFile != "" && blueprintsAdded)
                {
                    _blueprints_.Save(blueprintsFile);
                    ScreenMsg("'" + craftToAddMission + "' Blueprints Available");
                    OrXLog.instance.DebugLog("[OrX Target Manager] === '" + craftToAddMission + "' Blueprints Available ===");
                }
            }
            else
            {
                if (stageStart)
                {
                    stageStart = false;
                }
                else
                {
                    stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + airTime + ","
                        + _time + "," + CheatOptions.NoCrashDamage + ","
                        + CheatOptions.UnbreakableJoints + "," + CheatOptions.IgnoreMaxTemperature + ","
                        + CheatOptions.InfinitePropellant + "," + CheatOptions.InfiniteElectricity);
                }

                OrXLog.instance.DebugLog("[OrX Get Next Coord] === stage" + gpsCount + " = " + gpsCount + ", " + topSurfaceSpeed + ", " + maxDepth + ", " + airTime + ", " + _time + " ===");
                OrXLog.instance.DebugLog("STAGE TIME: " + TimeSet((float)_time));
                OrXLog.instance.DebugLog("COORD COUNT: " + TimeSet((float)_time));

                ScreenMsg("STAGE TIME: " + TimeSet((float)_time));
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

        #endregion


        #region Scoreboard

        public void OpenStatsWindow()
        {
            scoreboardStatsWindow = true;
            showScores = true;
            PlayOrXMission = true;
            movingCraft = false;
            GuiEnabledOrXMissions = true;
            connectToKontinuum = false;
            _showSettings = false;
        }
        public void OpenScoreboardMenu()
        {
            _importingScores = false;
            scoreboardStatsWindow = false;
            showScores = true;
            PlayOrXMission = true;
            movingCraft = false;
            GuiEnabledOrXMissions = true;
            connectToKontinuum = false;
            _showSettings = false;
        }

        public void ExtractScoreboard(string creatorName, string holoName, int _hkCount_)
        {
            ConfigNode _sbFile = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + holoName + "/" + holoName + "-" + _hkCount_ + "-" + creatorName + ".orx");
            if (_sbFile != null)
            {
                foreach (ConfigNode cn in _sbFile.nodes)
                {
                    if (cn.name.Contains("mission"))
                    {
                        ConfigNode scoreboardFile = cn;
                        scoreboardFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + creatorName + "/" + holoName + "/" + holoName + "-" + _hkCount_ + "-" + creatorName + ".scoreboard");
                    }
                }
            }
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
            OrXScoreboardStats.instance.OpenStatsWindow(statsName, statsTime, statsTotalAirTime, statsMaxSpeed, statsMaxDepth, scoreboardStats);

            scoreboardStatsWindow = true;
        }
        public void GetScoreboardData()
        {
            //_file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName +  ".orx");
            Debug.Log("[OrX Get Scoreboard Data] === mission count " + hkCount + " ===");
            string fileToRemove = "";
            List<string> scoreBoardFiles = new List<string>(Directory.GetFiles(UrlDir.ApplicationRootPath + "GameData/OrX/Import/", "*.scoreboard", SearchOption.AllDirectories));
            if (scoreBoardFiles != null)
            {
                OrXLog.instance.DebugLog("[OrX Import Score Board] === SCORE BOARD IMPORT FILES FOUND ===");
                List<string>.Enumerator scoreBoardFile = scoreBoardFiles.GetEnumerator();
                while (scoreBoardFile.MoveNext())
                {
                    OrXLog.instance.DebugLog("[OrX Import Score Board] === CHECKING FILE === " + scoreBoardFile);

                    if (scoreBoardFile.Current != null)
                    {
                        if (scoreBoardFile.Current.Contains(HoloKronName + "-" + hkCount))
                        {
                            OrXLog.instance.DebugLog("[OrX Import Score Board] === '" + scoreBoardFile.Current + "' MATCHES '" + HoloKronName + "-" + hkCount + "' ===");

                            ConfigNode _scoreBoard = ConfigNode.Load(scoreBoardFile.Current);
                            if (_scoreBoard != null)
                            {
                                OrXLog.instance.DebugLog("[OrX Import Score Board] === FILE LOADED === " + scoreBoardFile.Current + " ===");
                                fileToRemove = scoreBoardFile.Current;
                                _mission.ClearData();
                                _mission = _scoreBoard;
                                _file.Save(_currentOrXFile);
                            }
                        }
                    }
                }
                scoreBoardFile.Dispose();
            }

            if (fileToRemove != "")
            {
                try
                {
                    File.Delete(fileToRemove);
                    _file.ClearData();
                    _file = ConfigNode.Load(_currentOrXFile);
                }
                catch (Exception e)
                {
                    OrXLog.instance.DebugLog("[OrX Import Score Board] === ERROR: " + e);

                }
            }

            _mission = _file.GetNode("mission" + hkCount);
            if (_mission == null)
            {
                _file.AddNode("mission" + hkCount);
            }
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
                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".orx");
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

                            if (file.Current.Contains(HoloKronName + "-" + hkCount))
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

                                        OrXScoreboardStats.instance.OpenStatsWindow(statsName, statsTime, statsTotalAirTime, statsMaxSpeed, statsMaxDepth, scoreboardStats);
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
                    _air = missionNode.GetValue("airTime");

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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
                                    nameAlreadyPresent = true;

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

                            if (maxDepthChallenger <= _maxDepth)
                            {
                                maxDepthChallenger = _maxDepth;
                            }

                            double _maxSpeed = double.Parse(data[1]);

                            if (maxSpeedChallenger >= _maxSpeed)
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

                OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === STAGE TIME LIST CREATED ===");
                ScreenMsg("TOTAL TIME: " + TimeSet((float)totalTimeChallenger));
                yield return new WaitForFixedUpdate();

                ConfigNode scores = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "-" + hkCount + "-" + challengersName + ".scores");
                if (scores == null)
                {
                    OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === SCORES FILE DOESN'T EXIST ... CREATIING===");

                    scores = new ConfigNode();
                    scores.AddValue("name", HoloKronName);
                    scores.AddValue("count", hkCount);
                    scores.AddValue("creator", creatorName);
                    scores.AddValue("challengersName", challengersName);
                    scores.AddNode("mission" + hkCount);
                    ConfigNode mis = scores.GetNode("mission" + hkCount);

                    foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                    {
                        mis.AddValue(cv.name, cv.value);
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

                    scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "-" + hkCount + "-" + challengersName + ".scores");
                    scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Import/" + HoloKronName + "-" + hkCount + "-" + challengersName + ".scores");

                    OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === STAGE TIME LIST CREATED ===");
                }
                else
                {
                    foreach (ConfigNode.Value cv in scores.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in scores.nodes)
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

                    ConfigNode miss = scores.GetNode("mission" + hkCount);
                    foreach (ConfigNode.Value cv in miss.values)
                    {
                        if (cv.name == "totalTime")
                        {
                            if (Convert.ToDouble(cv.value) >= totalTimeChallenger)
                            {
                                miss.ClearData();
                                yield return new WaitForFixedUpdate();
                                foreach (ConfigNode.Value cv2 in tempChallengerEntry.values)
                                {
                                    miss.AddValue(cv2.name, cv2.value);
                                }
                            }
                        }
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

                    scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "-" + hkCount + "-" + challengersName + ".scores");
                    OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === STAGE TIME LIST SAVED TO SCORES FILE ===");
                }

                //tempChallengerEntry.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "-" + hkCount + "-" + challengersName + ".exportedscores");

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
                        if (missionNode.name.Contains("mission"))
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
                                if (cn.name.Contains("mission"))
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

                                    if (cn.HasValue("totalTime"))
                                    {
                                        _time = cn.GetValue("totalTime");
                                        t = double.Parse(_time);
                                    }

                                    if (cn.HasValue("maxSpeed"))
                                    {
                                        _speed = cn.GetValue("maxSpeed");
                                    }

                                    if (cn.HasValue("maxDepth"))
                                    {
                                        _depth = cn.GetValue("maxDepth");
                                    }

                                    if (cn.HasValue("airTime"))
                                    {
                                        _air = cn.GetValue("airTime");
                                    }
                                }
                                else
                                {
                                    OrXLog.instance.DebugLog("[OrX Import Scores] === NO DATA CONTAINED IN .SCORES FILE FOR CHALLENGE IN '" + HoloKronName + " " + hkCount + "' ===");

                                }
                            }

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                                            nameAlreadyPresent = true;

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
                            /*
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

                            scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "-" + hkCount + "-" + challengersName + ".scores");
                            */
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
                OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === FOUND " + HoloKronName + " " + hkCount + " ==="); ;

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

            movingCraft = false;
            showScores = true;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = true;
            updatingScores = false;
            OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === DATA PROCESSED AND ENCRYPTED ===");
            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + creatorName + "/" + HoloKronName + "/" + HoloKronName + "-" + hkCount + "-" + creatorName + ".orx");
        }

        #endregion

        public bool _showPartModules = false;

        #region Main GUI

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!PauseMenu.isOpen)
                {
                    if (showTargets)
                    {
                        OrXLog.DrawRecticle(worldPos, OrXLog.instance.HoloTargetTexture, new Vector2(32, 32));
                    }

                    if (OrXHCGUIEnabled && !OrXScoreboardStats.instance.GuiEnabledStats)
                    {
                        WindowRectToolbar = GUI.Window(291827362, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
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
                            ScreenMsg("UNSUPPORTED VERSION OF KSP");
                            ScreenMsg("Dinner Out is Cancelled .....");
                            ScreenMsg("OrX Kontinuum shutting down .....");
                            OrXHCGUIEnabled = false;
                            StopAllCoroutines();
                        }
                        else
                        {
                            if (!OrXMode.instance._guiEnabled)
                            {
                                WindowRectToolbar = new Rect(40, 50, toolWindowWidth, toolWindowHeight);

                                PREsettings = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/PhysicsRangeExtender/settings.cfg");
                                if (PREsettings != null)
                                {
                                    _preInstalled = true;
                                    OrXLog.instance.GetPRERanges();
                                    ConfigNode PREnode = PREsettings.GetNode("PreSettings");
                                    if (PREnode.GetValue("ModEnabled") != "False")
                                    {
                                        ScreenMsg("Please remember to disable");
                                        ScreenMsg("Physics Range Extender");
                                    }
                                }

                                OrXHCGUIEnabled = true;

                                if (!OrXSpawnHoloKron.instance.spawning)
                                {
                                    if (HighLogic.LoadedSceneIsFlight)
                                    {
                                        SetRanges(8000);
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
                                        ScreenMsg("Unable to scan while creating .......");
                                        reset = true;
                                        connectToKontinuum = true;
                                        _showSettings = false;
                                    }
                                }
                            }
                            else
                            {
                                ScreenMsg("Close the mode menu to re-open the main menu");
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
                    if (!OrXEditorChallengeList.instance.guiEnabled)
                    {
                        OrXEditorChallengeList.instance.guiEnabled = true;
                    }
                    else
                    {
                        OrXEditorChallengeList.instance.guiEnabled = false;
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
                GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum Settings", titleStyleLarge);

                if (!devKitInstalled)
                {
                    // GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "OrX Dev Kit is not installed", titleStyleMedRed);
                }
                else
                {
                    line += 0.5f;
                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "OrX Dev Kit is installed", titleStyleMedYellow);
                    line++;
                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Welcome to the abyss ......", titleStyleMedYellow);
                    line += 0.5f;
                }
                line++;

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Debug Logging", titleStyleMedYellowL);

                if (!OrXLog.instance._debugLog)
                {
                    if (GUI.Button(new Rect(WindowWidth - 30, ContentTop + (line * entryHeight), 20, 20), "X", HighLogic.Skin.button))
                    {
                        StartCoroutine(SetDebug());
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(WindowWidth - 30, ContentTop + (line * entryHeight), 20, 30), "X", HighLogic.Skin.box))
                    {
                        OrXLog.instance._mode = false;
                        _settings0 = false;
                        OrXLog.instance._debugLog = false;
                    }
                }

                line++;
                line++;

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
                    ScreenMsg("Resetting the Kontinuum .....");
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

                line++;
                line += 0.5f;
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Kergan's Manual", OrXGUISkin.button))
                {
                    OrXUserManual.instance.guiEnabled = true;
                }

                line += 0.2f;
                line++;
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
                {
                    _showSettings = false;
                }
                
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (OrXLog.instance._debugLog)
                    {
                        line += 0.2f;
                        line++;
                        DrawPasswordKarma(line);
                        line++;
                        DrawPlayKarma(line);
                        line += 0.3f;
                    }
                }
                
                //toolWindowHeight = Mathf.Lerp(toolWindowHeight, ContentTop + (line * entryHeight) + 5, 1);
                //WindowRectToolbar.height = toolWindowHeight;
            }
            else
            {
                if (connectToKontinuum)
                {
                    if (reset)
                    {
                        GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum Reset", titleStyleLarge);
                        line++;

                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Reset OrX Kontinuum", OrXGUISkin.button))
                        {
                            connectToKontinuum = false;
                            ScreenMsg("Resetting the Kontinuum .....");
                            reset = false;
                            OrXHCGUIEnabled = false;
                            ResetData();
                        }
                    }
                    else
                    {
                        GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum Login", titleStyleLarge);
                        line += 0.5f;
                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Enter a username below to start", titleStyleMedYellow);
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
                                            ScreenMsg("Unable to connect ....");
                                        }
                                    }
                                    else
                                    {
                                        ScreenMsg("Please enter a shared file web link ....");
                                    }

                                }
                                else
                                {
                                    ScreenMsg("Please enter a file name ....");
                                }
                            }
                            else
                            {
                                ScreenMsg("Please enter a name ....");
                            }
                        }
                        //line += 0.5f;
                        //line++;
                        
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Disconnect from Kontinuum", OrXGUISkin.button))
                        {
                            //Disconnect();
                            connectToKontinuum = false;
                            ScreenMsg("The Kontinuum is currently unavailable .....");
                        }
                        line += 0.5f;
                        line++;

                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
                        {
                            connectToKontinuum = false;
                        }
                        line++;
                    }

                    line++;

                    toolWindowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
                    WindowRectToolbar.height = toolWindowHeight;
                }
                else
                {
                    if (GuiEnabledOrXMissions)
                    {
                        if (movingCraft)
                        {
                            GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum", titleStyleLarge);
                            line++;

                            if (getNextCoord)
                            {
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Coord Count = " + locCount, titleStyle);
                                line++;

                                if (spawningStartGate)
                                {
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "PLACE GATE", OrXGUISkin.button))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Place Gate] ===== PLACING GATE FOR " + HoloKronName + " STAGE " + hkCount + " =====");
                                        spawningStartGate = false;
                                        getNextCoord = false;
                                        FlightGlobals.ActiveVessel.vesselName = HoloKronName + " " + hkCount + " STAGE " + hkCount;
                                        OrXVesselMove.Instance.EndMove(true, false, true);
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
                                            addingMission = true;
                                            getNextCoord = true;
                                            showTargets = false;
                                            movingCraft = false;
                                            OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 0, false);
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
                                    line++;
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "SAVE AND EXIT", OrXGUISkin.button))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Save and Exit] === SAVING HOLOKRON ===");
                                        addCoords = false;
                                        addingMission = true;
                                        //saveLocalVessels = true;
                                        getNextCoord = false;
                                        //blueprintsAdded = true;
                                        _lastStage.vesselName = HoloKronName + " " + hkCount + " FINSH LINE";
                                        _HoloKron.vesselName = HoloKronName + " " + hkCount;
                                        SaveConfig(HoloKronName, false);
                                    }
                                }
                            }
                            else
                            {
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Reaching into the Outer Limits ...", titleStyle);
                                line++;
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "... Please stand By ...", titleStyle);

                                if (!OrXMode.instance._guiEnabled)
                                {
                                    line++;
                                    line += 0.2f;

                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Reset Kontinuum", OrXGUISkin.button))
                                    {
                                        challengeRunning = false;
                                        ScreenMsg("Resetting the Kontinuum .....");
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
                                }
                            }
                        }
                        else
                        {
                            if (PlayOrXMission)
                            {
                                if (showScores)
                                {
                                    if (!scoreboardStatsWindow)
                                    {
                                        DrawScoreboard(line);
                                        line++;
                                        line += 0.2f;
                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Challenger", titleStyleMed);
                                        GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), "Total Time", titleStyleMed);
                                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth / 2, 20), "____________", titleStyleMed);
                                        GUI.Label(new Rect(WindowWidth / 2, ContentTop + line * entryHeight, WindowWidth / 2, 20), "____________", titleStyleMed);
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
                                        DrawCloseScoreboard(line);
                                        line++;
                                        line += 0.1f;
                                        DrawUpdateScoreboard(line);
                                        line += 0.1f;
                                        line++;
                                        DrawExtractScoreboard(line);
                                    }
                                    else
                                    {
                                        GUI.Label(new Rect(0, 1, WindowWidth, 20), statsName, titleStyleLarge);
                                        line++;
                                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), "TOTAL TIME", HighLogic.Skin.button)){}
                                        if (GUI.Button(new Rect((WindowWidth / 2) + 3, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), statsTime, HighLogic.Skin.box)){}
                                        line++;
                                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), "AIRTIME", HighLogic.Skin.button)) { }
                                        if (GUI.Button(new Rect((WindowWidth / 2) + 3, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), statsTotalAirTime, HighLogic.Skin.box)) { }
                                        line++;
                                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), "TOP SPEED", HighLogic.Skin.button)) { }
                                        if (GUI.Button(new Rect((WindowWidth / 2) + 3, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), "" + statsMaxSpeed, HighLogic.Skin.box)) { }
                                        line++;
                                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), "MAX DEPTH", HighLogic.Skin.button)) { }
                                        if (GUI.Button(new Rect((WindowWidth / 2) + 3, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), "" + statsMaxDepth, HighLogic.Skin.box)) { }
                                        line += 0.5f;
                                        line++;
                                        line++;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "Show Stage Details", OrXGUISkin.button))
                                        {
                                            OrXScoreboardStats.instance.OpenStatsWindow(statsName, statsTime, statsTotalAirTime, statsMaxSpeed, statsMaxDepth, scoreboardStats);
                                        }
                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
                                        {
                                            scoreboardStatsWindow = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (modCheckFail)
                                    {
                                        int scrollIndex = 0;
                                        int pmScrollIndex = 0;

                                        GUI.Label(new Rect(0, 1, WindowWidth, 20), "Missing Mods List", titleStyleLarge);
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
                                        GUI.Label(new Rect(0, ContentTop + (line * entryHeight), WindowWidth, 20), "Spawning without all the mods", titleStyle);
                                        line++;

                                        GUI.Label(new Rect(0, ContentTop + (line * entryHeight), WindowWidth, 20), "listed may break your game", titleStyle);

                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Attempt Spawn", OrXGUISkin.button))
                                        {
                                            modCheckFail = false;
                                            OrXSpawnHoloKron.instance.SpawnLocal(true, HoloKronName, new Vector3d());
                                        }

                                        line++;
                                        line += 0.2f;

                                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", OrXGUISkin.button))
                                        {
                                            modCheckFail = false;
                                        }
                                    }
                                    else
                                    {
                                        DrawPlayHoloKronName(line);
                                        line += 0.2f;
                                        DrawPlayMissionType(line);
                                        line++;
                                        line += 0.2f;

                                        if (!geoCache)
                                        {
                                            DrawPlayRaceType(line);
                                            line++;
                                            line += 0.2f;

                                            if (challengeType == "OUTLAW RACING")
                                            {
                                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "STAGE GATES: " + CoordDatabase.Count, titleStyle);
                                                line++;
                                                line += 0.2f;
                                            }
                                        }

                                        if (blueprintsAdded)
                                        {
                                            DrawPlayBlueprintsAdded(line);
                                            line++;
                                            line += 0.2f;

                                        }

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
                                            line++;
                                            line += 0.2f;

                                            if (!_editor)
                                            {
                                                DrawChallengerName(line);
                                                line++;
                                                line += 0.2f;
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

                                //_windowHeight = ContentTop + line * entryHeight + (entryHeight / 2);
                                //_windowRect.height = _windowHeight;
                            }
                            else
                            {
                                DrawTitle(line);
                                line++;

                                if (!addCoords)
                                {
                                    line++;
                                    DrawHoloKronName(line);
                                    line++;
                                    DrawHoloKronName2(line);
                                    line++;
                                    DrawCreatorName(line);
                                    line++;
                                    DrawPassword(line);
                                    line++;
                                    DrawMissionType(line);
                                    line++;
                                    if (!geoCache)
                                    {
                                        DrawChallengeType(line);
                                        line++;
                                        if (outlawRacing)
                                        {
                                            DrawRaceType(line);
                                            line++;
                                        }

                                        if (bdaChallenge)
                                        {
                                            //DrawKontinuumLogin1(line);
                                            //line++;
                                        }
                                    }

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

                                    DrawVessel(line);
                                    line++;
                                    DrawAddBlueprints(line);
                                    line++;

                                    if (geoCache)
                                    {
                                        DrawSaveLocal(line);
                                        line++;
                                        DrawLocalSaveRange(line);
                                        line++;
                                    }

                                    line++;
                                    DrawSave(line);
                                    line++;
                                    DrawCancel(line);
                                }
                                else
                                {
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Coord Count = " + locCount, titleStyle);
                                    line++;

                                    DrawAddCoords(line);
                                    if (locAdded)
                                    {
                                    }
                                    //DrawSave(line);
                                    //line++;
                                    //DrawCancel(line);
                                }
                            }
                            line++;
                            line++;
                            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Kergan's Manual", OrXGUISkin.button))
                            {
                                OrXUserManual.instance.guiEnabled = true;
                            }
                        }

                        //toolWindowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
                        //WindowRectToolbar.height = toolWindowHeight;
                    }
                    else
                    {
                        GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum", titleStyleLarge);
                        line += 0.5f;

                        if (!challengeRunning)
                        {
                            if (checking)
                            {
                                line += 0.5f;

                                if (targetDistance <= 100000)
                                {
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron is " + Math.Round((targetDistance / 1000), 2) + " km away", titleStyleMedNoItal);
                                    line++;
                                    line += 0.2f;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Altitude: " + Math.Round(_altitude, 1) + " meters", titleStyleMedNoItal);
                                }
                                else
                                {
                                    scanDelay = OrXTargetDistance.instance.scanDelay;
                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "No HoloKron in range ......", titleStyle);
                                    line++;
                                    line += 0.2f;

                                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Rescan in " + Math.Round(scanDelay, 0) + " seconds", titleStyleMedNoItal);
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
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), creatorName + "'s HoloKrons", titleStyleMed);
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
                                                                            //challengeRunning = true;
                                                                            showGeoCacheList = false;
                                                                            showCreatedHolokrons = false;
                                                                            showChallengelist = false;
                                                                            //checking = true;
                                                                            //challengeRunning = true;
                                                                            HoloKronName = data2[1];
                                                                            creatorName = data2[2];
                                                                            _editor = true;
                                                                            latMission = double.Parse(data2[3]);
                                                                            lonMission = double.Parse(data2[4]);
                                                                            altMission = double.Parse(data2[5]);
                                                                            
                                                                            if (data2[7] != "GEO-CACHE")
                                                                            {
                                                                                geoCache = false;
                                                                            }
                                                                            StartCoroutine(OpenHoloKronRoutine(geoCache, HoloKronName, int.Parse(data2[6]), null, null));
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
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Local System HoloKrons", titleStyleMed);
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
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Local HoloKron Creators", titleStyleMed);
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
                                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron Creators", titleStyleMed);
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
                                                            bool enabled = true;
                                                            ConfigNode PREsettings = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/PhysicsRangeExtender/settings.cfg");
                                                            if (PREsettings != null)
                                                            {
                                                                _preInstalled = true;

                                                                ConfigNode PREnode = PREsettings.GetNode("PreSettings");
                                                                if (PREnode.GetValue("ModEnabled") != "False")
                                                                {
                                                                    ScreenMsg("Physics Range Extender must be disabled");
                                                                    ScreenMsg("for OrX Kontinuum to function properly");
                                                                    ScreenMsg("... Shutting down ...");
                                                                    enabled = false;
                                                                    ResetData();
                                                                    MainMenu();
                                                                    OrXHCGUIEnabled = false;
                                                                }
                                                            }

                                                            if (enabled)
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
                                        bool enabled = true;
                                        ConfigNode PREsettings = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/PhysicsRangeExtender/settings.cfg");
                                        if (PREsettings != null)
                                        {
                                            _preInstalled = true;

                                            ConfigNode PREnode = PREsettings.GetNode("PreSettings");
                                            if (PREnode.GetValue("ModEnabled") != "False")
                                            {
                                                ScreenMsg("Physics Range Extender must be disabled");
                                                ScreenMsg("for OrX Kontinuum to function properly");
                                                ScreenMsg("... Shutting down ...");
                                                enabled = false;
                                                ResetData();
                                                MainMenu();
                                                OrXHCGUIEnabled = false;
                                            }
                                        }

                                        if (enabled)
                                        {
                                            if (OrXLog.instance._debugLog && _pKarma == Karma)
                                            {
                                                connectToKontinuum = true;
                                                ScreenMsg("The matrix has you " + challengersName + " ....");
                                            }
                                            else
                                            {
                                                ScreenMsg("The Kontinuum is currently unavailable ....");
                                            }
                                        }
                                    }
                                    line++;
                                    line += 0.2f;

                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Creator List", OrXGUISkin.button))
                                    {
                                        bool _enable = true;
                                        ConfigNode PREsettings = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/PhysicsRangeExtender/settings.cfg");
                                        if (PREsettings != null)
                                        {
                                            _preInstalled = true;

                                            ConfigNode PREnode = PREsettings.GetNode("PreSettings");
                                            if (PREnode.GetValue("ModEnabled") != "False")
                                            {
                                                ScreenMsg("Physics Range Extender must be disabled");
                                                ScreenMsg("for OrX Kontinuum to function properly");
                                                ScreenMsg("... Shutting down ...");
                                                _enable = false;
                                                ResetData();
                                                MainMenu();
                                                OrXHCGUIEnabled = false;
                                            }
                                        }

                                        if (_enable)
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
                                            bool _enable = true;
                                            ConfigNode PREsettings = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/PhysicsRangeExtender/settings.cfg");
                                            if (PREsettings != null)
                                            {
                                                _preInstalled = true;

                                                ConfigNode PREnode = PREsettings.GetNode("PreSettings");
                                                if (PREnode.GetValue("ModEnabled") != "False")
                                                {
                                                    ScreenMsg("Physics Range Extender must be disabled");
                                                    ScreenMsg("for OrX Kontinuum to function properly");
                                                    ScreenMsg("... Shutting down ...");
                                                    _enable = false;
                                                    ResetData();
                                                    MainMenu();
                                                    OrXHCGUIEnabled = false;
                                                }
                                            }

                                            if (_enable)
                                            {
                                                SetupHolo(FlightGlobals.ActiveVessel, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                                            }
                                        }

                                        line++;
                                        line += 0.2f;
                                    }

                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Settings", OrXGUISkin.button))
                                    {
                                        _showSettings = true;
                                    }

                                    line++;
                                    line += 0.2f;

                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Kergan's Manual", OrXGUISkin.button))
                                    {
                                        OrXUserManual.instance.guiEnabled = true;
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
                                        ScreenMsg("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
                                        locCount = 0;
                                        locAdded = false;
                                        building = false;
                                        buildingMission = false;
                                        addCoords = false;
                                        MainMenu();
                                        ResetData();
                                    }
                                }
                                line++;
                                line += 0.5f;
                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Kergan's Manual", OrXGUISkin.button))
                                {
                                    OrXUserManual.instance.guiEnabled = true;
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
                                    ScreenMsg("Exiting " + HoloKronName + " " + hkCount + " challenge .....");
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
                                line++;
                                line += 0.5f;
                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Kergan's Manual", OrXGUISkin.button))
                                {
                                    OrXUserManual.instance.guiEnabled = true;
                                }
                            }
                        }

                        //toolWindowHeight = Mathf.Lerp(toolWindowHeight, ContentTop + (line * entryHeight) + 5, 1);
                        //WindowRectToolbar.height = toolWindowHeight;

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
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "ADD LOCATION", HighLogic.Skin.button))
            {
                movingCraft = true;
                OrXVesselMove.Instance.EndMove(true, false, false);
            }
        }

        public void DrawClearLastCoord(float line)
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
            }
        }
        public void DrawClearAllCoords(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "DELETE ALL", HighLogic.Skin.button))
            {
                CoordDatabase.Clear();
                locCount = 0;
                locAdded = false;
            }
        }
        public void DrawCoordCount(float line)
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

        public void DrawScoreboard(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), HoloKronName, titleStyle);
        }
        public void DrawScoreboard0(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), nameSB0, HighLogic.Skin.button))
            {
                if (nameSB0 != "<empty>")
                {
                    _importingScores = false;

                    GetStats(nameSB0, 0);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 3, ContentTop + line * entryHeight, (WindowWidth / 2) - 12, 20), timeSB0, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB1, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB1 != "<empty>")
                {
                    GetStats(nameSB1, 1);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB1, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB2, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB2 != "<empty>")
                {
                    GetStats(nameSB2, 2);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB2, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB3, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB3 != "<empty>")
                {
                    GetStats(nameSB3, 3);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB3, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB4, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB4 != "<empty>")
                {
                    GetStats(nameSB4, 4);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB4, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB5, HighLogic.Skin.button))
            {
                if (nameSB5 != "<empty>")
                {
                    _importingScores = false;

                    GetStats(nameSB5, 5);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB5, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB6, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB6 != "<empty>")
                {
                    GetStats(nameSB6, 6);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB6, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB7, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB7 != "<empty>")
                {
                    GetStats(nameSB7, 7);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB7, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB8, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB8 != "<empty>")
                {
                    GetStats(nameSB8, 8);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB8, HighLogic.Skin.box))
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
            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), nameSB9, HighLogic.Skin.button))
            {
                _importingScores = false;

                if (nameSB9 != "<empty>")
                {
                    GetStats(nameSB9, 9);
                }
            }

            if (GUI.Button(new Rect((WindowWidth / 2) + 5, ContentTop + line * entryHeight, (WindowWidth / 2) - 15, 20), timeSB9, HighLogic.Skin.box))
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
            if (GUI.Button(saveRect, "EXTRACT SCOREBOARD", HighLogic.Skin.button))
            {
                ExtractScoreboard(creatorName, HoloKronName, hkCount);
                ScreenMsg("Scoreboard saved to the OrX/Export/" + creatorName + "/" + HoloKronName + "/" + " directory");
            }
        }

        public void DrawUpdateScoreboard(float line)
        {
            var saveRect = new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20);
            if (GUI.Button(saveRect, "IMPORT SCORES", HighLogic.Skin.button))
            {
                _importingScores = true;
                StartImporting();
            }
        }
        public void DrawCloseScoreboard(float line)
        {
            var saveRect = new Rect(10, ContentTop + (line * entryHeight), toolWindowWidth - 20, 20);
            if (GUI.Button(saveRect, "CLOSE SCOREBOARD", HighLogic.Skin.button))
            {
                showScores = false;
                if (!OrXLog.instance.mission && !PlayOrXMission)
                {
                    OrXHCGUIEnabled = false;
                }
            }
        }

        #endregion


        #region Play Mission GUI

        public void DrawPlayHoloKronName(float line)
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

            GUI.Label(new Rect(0, 1, WindowWidth, 20), HoloKronName, titleStyle);
        }
        public void DrawPlayMissionName(float line)
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
        public void DrawPlayMissionType(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "HoloKron Type: " + missionType, titleStyle);
        }
        public void DrawPlayRaceType(float line)
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
        public void DrawPlayBlueprintsAdded(float line)
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
        public void DrawChallengerName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenger: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            challengersName = GUI.TextField(fwdFieldRect, challengersName);
        }

        public void DrawPlayPassword(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            Password = GUI.TextField(fwdFieldRect, Password);
        }
        public void DrawPlayKarma(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Enter the void", HighLogic.Skin.button))
            {
                if (_pKarma == Karma)
                {
                    Debug.Log("[OrX Karma] === OPENING MODE GUI ===");
                    Reach();
                    OrXMode.instance.SetMode();
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Karma] === WRONG PASSWORD ===");
                    ScreenMsg("WRONG PASSWORD");
                }

            }
        }

        public void DrawUnlock(float line)
        {
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "UNLOCK", HighLogic.Skin.button))
            {
                if (Password == pas)
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === UNLOCKING ===");

                    unlocked = true;
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === WRONG PASSWORD ===");

                    ScreenMsg("WRONG PASSWORD");
                }
            }
        }
        public void DrawSpawnChallenge(float line)
        {
            if (!showScores)
            {
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SPAWN CHALLENGE", HighLogic.Skin.button))
                {
                    StartCoroutine(CheckInstalledMods());
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
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SHOW SCOREBOARD", HighLogic.Skin.button))
                {
                    getNextCoord = false;
                    movingCraft = true;
                    OrXLog.instance.DebugLog("[OrX Mission] === SHOW SCOREBOARD ===");
                    GetScoreboardData();
                }
            }
            else
            {
            }
        }

        public void DrawStart(float line)
        {
            if (!geoCache)
            {
                if (!_editor)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "START CHALLENGE", HighLogic.Skin.button))
                    {
                        if (challengersName != "" || challengersName != string.Empty)
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === NAME ENTERED - STARTING ===");

                            if (challengeType == "SCUBA KERB")
                            {
                                if (!challengeRunning)
                                {
                                    if (disablePRE)
                                    {
                                        //OrXPRExtension.PreOff("OrX Kontinuum");
                                    }

                                    ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
                                    if (playerData == null)
                                    {
                                        playerData = new ConfigNode();
                                    }
                                    playerData.SetValue("name", challengersName, true);
                                    playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");

                                    GuiEnabledOrXMissions = false;
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

                                        ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
                                        if (playerData == null)
                                        {
                                            playerData = new ConfigNode();
                                        }
                                        playerData.SetValue("name", challengersName, true);
                                        playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");

                                        GuiEnabledOrXMissions = false;
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

                            ScreenMsg("Please enter a challenger name");
                        }
                    }
                }
                else
                {
                    if (!HighLogic.LoadedSceneIsFlight)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", HighLogic.Skin.button))
                        {
                            GetCreatorList(true);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SCAN FOR HOLOKRON", HighLogic.Skin.button))
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
            else
            {
                if (!_editor)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", HighLogic.Skin.button))
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
                        StartChallenge();
                    }
                }
                else
                {
                    if (!HighLogic.LoadedSceneIsFlight)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", HighLogic.Skin.button))
                        {
                            GetCreatorList(true);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SCAN FOR HOLOKRON", HighLogic.Skin.button))
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
            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLEAR DESCRIPTION", HighLogic.Skin.button))
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
        public void DrawDescription1(float line)
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
        public void DrawDescription2(float line)
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
        public void DrawDescription3(float line)
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
        public void DrawDescription4(float line)
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
        public void DrawDescription5(float line)
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
        public void DrawDescription6(float line)
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
        public void DrawDescription7(float line)
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
        public void DrawDescription8(float line)
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
        public void DrawDescription9(float line)
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

        public void DrawCraftBrowserTitle(float line)
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
        public void DrawHangar(float line)
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

                GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth, 20), "HoloCubes Available", titleStyle);
            }
            else
            {
                var sphButton = new Rect(10, ContentTop + line * entryHeight, 120, entryHeight);
                var vabButton = new Rect(130, ContentTop + line * entryHeight, 120, entryHeight);

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

        #endregion

        #region Challenge Creator GUI

        public void DrawTitle(float line)
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
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Co-ordinate Editor", titleStyle);
            }
            else
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX HoloKron Creator", titleStyle);
            }
        }
        public void DrawHoloKronName(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Name your HoloKron below", titleStyle);
        }
        public void DrawHoloKronName2(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Name: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            HoloKronName = GUI.TextField(fwdFieldRect, HoloKronName);
        }
        public void DrawCreatorName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Creator: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            creatorName = GUI.TextField(fwdFieldRect, creatorName);
        }

        public void DrawMissionType(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "HoloKron Type: ",
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 130, ContentTop + line * entryHeight, 130, entryHeight);

            if (geoCache)
            {
                if (GUI.Button(bfRect, missionType, HighLogic.Skin.button))
                {
                    //locAdded = true;
                    if (!locAdded)
                    {
                        if (_OrXV == _ate)
                        {
                            ScreenMsg("UNSUPPORTED VERSION OF KSP");
                            ScreenMsg("Dinner Out is Cancelled .....");
                            ScreenMsg("OrX Kontinuum shutting down .....");
                            OrXHCGUIEnabled = false;
                            StopAllCoroutines();
                        }
                        else
                        {
                            ScreenMsg("HOLOKRON TYPE CHANGED TO CHALLENGE");
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
                            ScreenMsg("UNSUPPORTED VERSION OF KSP");
                            ScreenMsg("Dinner Out is Cancelled .....");
                            ScreenMsg("OrX Kontinuum shutting down .....");
                            OrXHCGUIEnabled = false;
                            StopAllCoroutines();
                        }
                        else
                        {
                            OrXLog.instance.DebugLog("[OrX Mission] === HOLOKRON LOCKED AS GEO-CACHE ===");
                            ScreenMsg("HOLOKRON TYPE LOCKED AS GEO-CACHE");
                            geoCache = true;
                        }
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
                        ScreenMsg("HOLOKRON TYPE CHANGED TO GEO-CACHE");
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
                        ScreenMsg("HOLOKRON TYPE LOCKED AS CHALLENGE");
                    }
                }
            }
        }
        public void DrawChallengeType(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenge Type: ",
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 130, ContentTop + line * entryHeight, 130, entryHeight);

            if (windRacing)
            {
                if (GUI.Button(bfRect, "W[ind/S]", HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE - SCUBA KERB ===");
                        //challengeType = "SCUBA KERB";
                        ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                        //bdaChallenge = false;
                        //windRacing = false;
                        //Scuba = true;
                        //outlawRacing = false;

                    }
                    else
                    {
                        ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                        OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

                        //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS W[ind/S] ===");

                    }
                }
            }
            else
            {
                if (Scuba)
                {
                    if (GUI.Button(bfRect, "SCUBA KERB", HighLogic.Skin.button))
                    {
                        if (!locAdded)
                        {
                            //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE - BD ARMORY ===");
                            //challengeType = "BD ARMORY";
                            ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                            //bdaChallenge = true;
                            //windRacing = false;
                            //Scuba = false;
                            //outlawRacing = false;
                        }
                        else
                        {
                            //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS SCUBA ===");
                            ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

                        }
                    }
                }
                else
                {
                    if (bdaChallenge)
                    {
                        if (GUI.Button(bfRect, "BD ARMORY", HighLogic.Skin.button))
                        {
                            if (!locAdded)
                            {
                                //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE - DAKAR RACING ===");
                                //challengeType = "OUTLAW RACING";
                                //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                //raceType = "DAKAR RACING";
                                ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                                //bdaChallenge = false;
                                //windRacing = false;
                                //Scuba = false;
                                //dakarRacing = true;
                                //outlawRacing = true;
                            }
                            else
                            {
                                //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS BD ARMORY ===");
                                OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            }
                        }

                    }
                    else
                    {
                        if (outlawRacing)
                        {
                            if (GUI.Button(bfRect, "OUTLAW RACING", HighLogic.Skin.button))
                            {
                                if (!locAdded)
                                {
                                    //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE - W[ind/S] ===");
                                    //challengeType = "W[ind/S]";
                                    //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                    ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                                    //bdaChallenge = false;
                                    //windRacing = true;
                                    //Scuba = false;
                                    //outlawRacing = false;
                                }
                                else
                                {
                                    //OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                    OrXLog.instance.DebugLog("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                    ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
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
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Race Type: ",
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 130, ContentTop + line * entryHeight, 130, entryHeight);

            if (dakarRacing)
            {
                if (GUI.Button(bfRect, raceType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        raceType = "SHORT TRACK";
                        OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE CHANGED TO SHORT TRACK ===");
                        ScreenMsg("RACE TYPE CHANGED TO SHORT TRACK");
                        dakarRacing = false;
                        shortTrackRacing = true;
                    }
                    else
                    {
                        raceType = "DAKAR RACING";
                        OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE LOCKED AS DAKAR RACING ===");
                        ScreenMsg("RACE TYPE LOCKED AS DAKAR RACING");
                    }
                }
            }
            else
            {
                if (GUI.Button(bfRect, raceType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        //raceType = "DAKAR RACING";
                        //dakarRacing = true;
                        //OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE CHANGED TO DAKAR RACING ===");
                        //ScreenMsg("RACE TYPE CHANGED TO DAKAR RACING");
                        //shortTrackRacing = false;

                        raceType = "SHORT TRACK";
                        dakarRacing = false;
                        shortTrackRacing = true;
                        OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE LOCKED AS SHORT TRACK ===");
                        ScreenMsg("RACE TYPE LOCKED AS SHORT TRACK");

                    }
                    else
                    {
                        raceType = "SHORT TRACK";
                        dakarRacing = false;
                        OrXLog.instance.DebugLog("[OrX Mission] === RACE TYPE LOCKED AS SHORT TRACK ===");
                        ScreenMsg("RACE TYPE LOCKED AS SHORT TRACK");
                    }
                }

            }
        }

        public void DrawKontinuumLogin1(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), _labelConnect, leftLabel);
            var bfRect = new Rect(WindowWidth - 20, ContentTop + line * entryHeight, 10, 20);
            if (!_KontinuumConnect)
            {
                if (GUI.Button(bfRect, "", HighLogic.Skin.button))
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
                if (GUI.Button(bfRect, "X", HighLogic.Skin.box))
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
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Login Name:",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            loginName = GUI.TextField(fwdFieldRect, loginName);
        }
        public void DrawKontinuumLogin3(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Save File: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            pasKontinuum = GUI.TextField(fwdFieldRect, pasKontinuum);
        }
        public void DrawKontinuumLogin4(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Web Link: ", leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            urlKontinumm = GUI.TextField(fwdFieldRect, urlKontinumm);
        }

        public void DrawVessel(float line)
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
        public void DrawAddBlueprints(float line)
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
                    OrXLog.instance.DebugLog("[OrX Mission] === ADDING BLUEPRINTS ===");
                    addingBluePrints = true;
                    blueprintsFile = "";
                    PlayOrXMission = false;
                    movingCraft = true;
                    spawningGoal = false;
                    OrXHCGUIEnabled = false;
                    openingCraftBrowser = true;
                    OrXSpawnHoloKron.instance.CraftSelect();
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", HighLogic.Skin.box))
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === REMOVING BLUEPRINTS ===");
                    blueprintsLabel = "Add Blueprints to Holo";
                    blueprintsFile = "";
                    blueprintsAdded = false;
                }
            }

        }

        public void DrawPasswordKarma(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password:",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            _pKarma = GUI.TextField(fwdFieldRect, _pKarma);
        }

        public void DrawPassword(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password:",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            Password = GUI.TextField(fwdFieldRect, Password);
        }
        public void DrawModule(float line)
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

        public void DrawEditDescription(float line)
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
                "Edit the HoloKron description below", titleStyle);
        }
        public void DrawEditDescription2(float line)
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

        public void DrawSaveLocal(float line)
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
                    OrXLog.instance.DebugLog("[OrX Mission] === SAVE LOCAL VESSELS = TRUE ===");
                    saveLocalVessels = true;
                    saveLocalLabel = "Saving Local Craft";
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", HighLogic.Skin.box))
                {
                    OrXLog.instance.DebugLog("[OrX Mission] === SAVE LOCAL VESSELS = FALSE ===");
                    saveLocalVessels = false;
                    saveLocalLabel = "Save Local Craft";
                }
            }
        }
        public void DrawLocalSaveRangeText(float line)
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

        public void DrawLocalSaveRange(float line)
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
            localSaveRange = GUI.HorizontalSlider(saveRect, localSaveRange, 50, 1000, HighLogic.Skin.horizontalSlider, HighLogic.Skin.horizontalSliderThumb);
        }
        public void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (HoloKronName != string.Empty && HoloKronName != "")
            {
                if (creatorName != "" || creatorName != string.Empty)
                {
                    if (missionDescription0 != string.Empty && missionDescription0 != "")
                    {
                        if (!geoCache)
                        {
                            if (addCoords)
                            {
                                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SAVE AND EXIT", HighLogic.Skin.button))
                                {
                                    OrXLog.instance.DebugLog("[OrX Mission] === CREATOR NAME ENTERED  ===");
                                    OrXLog.instance.DebugLog("[OrX Mission] === SAVING HOLOKRON ===");
                                    addCoords = false;
                                    addingMission = true;
                                    //saveLocalVessels = true;
                                    getNextCoord = false;
                                    //blueprintsAdded = true;
                                    _lastStage.vesselName = HoloKronName + " " + hkCount + " FINSH LINE";
                                    _HoloKron.vesselName = HoloKronName + " " + hkCount;
                                    SaveConfig(HoloKronName, false);
                                }
                            }
                            else
                            {
                                if (!spawningStartGate)
                                {
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "START ADD COORDS", HighLogic.Skin.button))
                                    {
                                        if (HoloKronName != string.Empty && HoloKronName != "")
                                        {
                                            if (missionDescription0 != string.Empty && missionDescription0 != "")
                                            {
                                                //OrXPRExtension.PreOff("OrX Kontinuum");
                                                hkSpawned = false;
                                                buildingMission = true;
                                                CoordDatabase = new List<string>();
                                                addCoords = true;
                                                addingMission = false;
                                                saveLocalVessels = false;
                                                OrXSpawnHoloKron.instance.stageCount = 0;
                                                startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
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
                                                        ScreenMsg(HoloKronName + " already exists ....");

                                                        ScreenMsg("Please enter a new name ....");

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
                                            ScreenMsg("Please enter a name for your HoloKron");
                                        }
                                    }
                                }
                                else
                                {
                                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "START GATE SPAWNED", HighLogic.Skin.box))
                                    {
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "SAVE", HighLogic.Skin.button))
                            {
                                if (HoloKronName != string.Empty && HoloKronName != "")
                                {
                                    if (missionDescription0 != string.Empty && missionDescription0 != "")
                                    {
                                        movingCraft = true;
                                        spawningStartGate = false;
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
                                                ScreenMsg(HoloKronName + " already exists ....");

                                                ScreenMsg("Please enter a new name ....");

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
                                    ScreenMsg("Please enter a name for your HoloKron");
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
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Please enter a creator name", 1, ScreenMessageStyle.UPPER_CENTER));
                }

            }
            else
            {
                ScreenMessages.PostScreenMessage(new ScreenMessage("Please enter a name for your HoloKron", 1, ScreenMessageStyle.UPPER_CENTER));
            }

        }
        public void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            string label = string.Empty;
            if (PlayOrXMission)
            {
                label = "CANCEL CHALLENGE";
            }
            else
            {
                label = "CANCEL";
            }

            if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "CLOSE WINDOW", HighLogic.Skin.button))
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