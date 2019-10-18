using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;
using System.IO;
using System.Text;
using System.Linq;
using OrXWind;
using FinePrint;
using OrX.spawn;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
    public class OrXHoloKron : MonoBehaviour
    {
        #region Fields

        public Vector3 UpVect;
        public Vector3 EastVect;
        public Vector3 NorthVect;

        public Vector3 targetPos = Vector3.zero;
        public Vessel triggerVessel;

        public List<string> OrXCoordsList;

        public Vessel startGate;
        public bool dakarRacing = false;
        public bool openingCraftBrowser = false;
        public CraftBrowserDialog craftBrowser;
        public bool spawningGoal = false;

        public bool addNextCoord = false;


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
        double latMission = 0;
        double lonMission = 0;
        double altMission = 0;

        Vector3 lastCoord;
        int locCount = 0;

        List<string> CoordDatabase;
        int coordCount = 0;

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

        bool holoOpen = false;
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
        string pasKontinumm = "";
        string _labelConnect = "Connect to Kontinuum";
        string urlKontinumm = "";

        #endregion

        #region Core

        public void Awake()
        {
            if (instance)
                Destroy(instance);
            instance = this;
            _HoloKron = null;
            resetHoloKron = false;
        }
        void Start()
        {
            OrXCoordsList = new List<string>();
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Import/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Import/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/Export/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/Export/");
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/");

            OrXHCGUIEnabled = false;
            AddToolbarButton();
            TargetHCGUI = false;
            spawnHoloKron = false;
            scanning = false;

            WindowRectToolbar = new Rect(40, 50, toolWindowWidth, toolWindowHeight);

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
                    triggerVessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
                }
            }
        }

        #endregion

        #region Utilities

        public void checkSOI(GameEvents.HostedFromToAction<Vessel, CelestialBody> data)
        {
            Debug.Log("[OrX Check SOI] === CHECKING ===");

            if (soi != FlightGlobals.ActiveVessel.mainBody.name)
            {
                Debug.Log("[OrX Check SOI] === '" + soi + "' doesn't match '" + FlightGlobals.ActiveVessel.mainBody.name + "' === RELOADING HOLOKRON TARGETS ===");

                LoadHoloKronTargets();
            }
        }
        private void resetHoloKronSystem(ConfigNode data)
        {
            
            //LoadResetDelay();
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
        public void PlaceCraft()
        {
            movingCraft = false;
            //FlightGlobals.ForceSetActiveVessel(_HoloKron);
            OrXHCGUIEnabled = true;
        }

        void LoadHoloKronTargets()
        {
            soi = FlightGlobals.ActiveVessel.mainBody.name;

            OrXCoordsList = new List<string>();

            string _soi = "";
            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            var files = new List<string>(Directory.GetFiles(holoKronLoc, "*.orx", SearchOption.AllDirectories));
            bool _spawned = true;
            int _hkCount = 0;
            int coordCount = 0;
            int _completed = 0;

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
                            string _HoloKronName = fileNode.GetValue("name");
                            foreach (ConfigNode spawnCheck in node.nodes)
                            {
                                if (_spawned)
                                {

                                    if (spawnCheck.name.Contains("OrXHoloKronCoords"))
                                    {
                                        Debug.Log("[OrX Load HoloKron Targets] === FOUND HOLOKRON ... DECRYPTING ===");

                                        foreach (ConfigNode.Value cv in spawnCheck.values)
                                        {
                                            if (cv.name == "spawned")
                                            {
                                                if (cv.value == "False")
                                                {
                                                    Debug.Log("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has not spawned ... ");

                                                    _spawned = false;

                                                    Debug.Log("[OrX Load HoloKron Targets] Checking SOI ................................");
                                                    _soi = spawnCheck.GetValue("SOI");
                                                    if (_soi == soi)
                                                    {
                                                        Debug.Log("[OrX Load HoloKron Targets] " + _HoloKronName + "'s current SOI '" + soi + "' matches HoloKron SOI '" + _soi + "'");

                                                        if (spawnCheck.HasValue("Targets"))
                                                        {
                                                            string targetCoords = spawnCheck.GetValue("Targets");
                                                            if (targetCoords == string.Empty)
                                                            {
                                                                Debug.Log("[OrX Load HoloKron Targets] " + _HoloKronName + " " + _hkCount + " Target string was empty!");
                                                            }
                                                            else
                                                            {
                                                                OrXCoordsList.Add(targetCoords);
                                                                Debug.Log("[OrX Load HoloKron Targets] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                                Debug.Log("[OrX Load HoloKron Targets] " + targetCoords);
                                                                coordCount += 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Debug.Log("[OrX Load HoloKron Targets] No OrX HoloKron coordinates found!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("[OrX Load HoloKron Targets] " + _HoloKronName + " is not in the current SOI");
                                                    }

                                                }
                                                else
                                                {
                                                    _soi = spawnCheck.GetValue("SOI");

                                                    if (soi == _soi)
                                                    {
                                                        _completed += 1;
                                                    }

                                                    var complete = spawnCheck.GetValue("completed");
                                                    if (complete == "False")
                                                    {
                                                        Debug.Log("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has not been completed ... END TRANSMISSION"); ;
                                                        _spawned = false;
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has been completed ... CHECKING FOR EXTRAS"); ;
                                                        if (spawnCheck.HasValue("extras"))
                                                        {
                                                            var t = spawnCheck.GetValue("extras");
                                                            if (t == "False")
                                                            {
                                                                Debug.Log("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has no extras ... END TRANSMISSION");
                                                                _spawned = false;
                                                            }
                                                            else
                                                            {
                                                                Debug.Log("[OrX Load HoloKron Targets] === " + _HoloKronName + " " + _hkCount + " has extras ... CONTINUING");
                                                                _hkCount += 1;
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
                    catch (Exception e)
                    {
                        Debug.Log("[OrX Load HoloKron Targets] HoloKron Targets Out Of Range ...... Continuing");
                    }
                }
                cfgsToAdd.Dispose();

                if (coordCount == 0)
                {
                    movingCraft = false;
                    PlayOrXMission = false;
                    checking = false;
                    OrXHCGUIEnabled = true;
                    GuiEnabledOrXMissions = false;
                    Debug.Log("[OrX Load HoloKron Targets] === Dinner Out is Cancelled ===");
                    ScreenMsg("There are no HoloKrons in " + FlightGlobals.ActiveVessel.mainBody.name + " SOI that have not been spawned .....");
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
                    Debug.Log("[OrX Load HoloKron Targets] === HoloKrons found ===");
                    ScreenMsg("Operation 'Dinner Out' is a go .....");
                }
            }
            else
            {
                Debug.Log("[OrX Load HoloKron Targets] === HoloKron List is empty ===");
                ScreenMsg("There are no HoloKrons in the current SOI");
                ScreenMsg("Dinner Out is Cancelled .....");
            }
        }
        public void StopScan(bool playerCancel)
        {
            checking = false;
            building = false;
            buildingMission = false;
            addCoords = false;
            PlayOrXMission = false;

            if (playerCancel)
            {
                GuiEnabledOrXMissions = false;
                OrXHCGUIEnabled = false;
                locAdded = false;
                locCount = 0;
                movingCraft = false;
                challengeRunning = false;
                ScreenMsg("Operation 'Dinner Out' was cancelled .....");
                OrXTargetDistance.instance.StopAllCoroutines();
                ResetData();
            }
        }

        public bool CheckExports(string holoName)
        {
            Debug.Log("[OrX Check Exports] === CHECKING EXPORTS FOLDER FOR " + holoName + " ===");

            ConfigNode exportCheck = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + holoName + ".orx");

            if (exportCheck != null)
            {
                HoloKronName = holoName;

                ConfigNode node = exportCheck.GetNode("OrX");
                hkCount = 0;
                foreach (ConfigNode cn in node.nodes)
                {
                    if (cn.name.Contains("OrXHoloKronCoords"))
                    {
                        hkCount += 1;
                    }
                }
                Debug.Log("[OrX Check Exports] === FOUND " + hkCount + " HOLOKRONS IN " + HoloKronName + " ===");
                return true;
            }
            else
            {
                Debug.Log("[OrX Check Exports] === " + holoName + " NOT FOUND ===");

                return false;
            }
        }
        public void SaveConfig(string holoName)
        {
            if (holoName != "" && holoName != string.Empty)
            {
                HoloKronName = holoName;
            }
            string hConfigLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + ".orx";
            Debug.Log("[OrX Save Config] === SAVING ===");

            //addingMission = false;
            //addCoords = false;


            if (addingMission)
            {
                //_file = ConfigNode.Load(hConfigLoc);
                _mission = _file.GetNode("mission" + hkCount);
                if (_mission == null)
                {
                    Debug.Log("[OrX Add Mission] === ADDING NODE 'mission" + hkCount + "' ===");

                    _file.AddNode("mission" + hkCount);
                    _mission = _file.GetNode("mission" + hkCount);

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
                                Debug.Log("[OrX Mission] === stage" + c + " added to " + HoloKronName + " " + hkCount + " === " + s);
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
                else
                {

                }


                if (saveLocalVessels)
                {
                    Debug.Log("[OrX Add Mission] === SAVING LOCAL VESELS ===");

                    int count = 0;

                    double _latDiff = 0;
                    double _lonDiff = 0;
                    double _altDiff = 0;

                    ConfigNode node = _file.GetNode("OrX");

                    List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                    while (v.MoveNext())
                    {
                        try
                        {
                            if (v.Current == null) continue;
                            if (v.Current.packed || !v.Current.loaded) continue;
                            bool _saveVessel = true;
                            Debug.Log("[OrX Add Mission] === CHECKING FOR EVA KERBAL ===");

                            List<Part>.Enumerator part = v.Current.parts.GetEnumerator();
                            while (part.MoveNext())
                            {
                                if (part.Current.Modules.Contains<KerbalEVA>())
                                {
                                    Debug.Log("[OrX Add Mission] === FOUND KERBAL ... SKIPPING ===");

                                    _saveVessel = false;
                                    break;
                                }
                            }
                            part.Dispose();

                            if (!v.Current.rootPart.Modules.Contains<KerbalEVA>() && _saveVessel)
                            {
                                Debug.Log("[OrX Add Mission] === RANGE CHECK ===");

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

                                Debug.Log("[OrX Add Mission] Vessel " + v.Current.vesselName + " Identified .......................");

                                double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                                double _altDiffDeg = _altDiff * degPerMeter;
                                double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                                double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;
                                bool _inRange = false;

                                Debug.Log("[OrX Add Mission] === RANGE: " + _targetDistance);

                                if (v.Current.LandedOrSplashed)
                                {
                                    if (_targetDistance <= localSaveRange)
                                    {
                                        _inRange = true;
                                    }
                                }
                                else
                                {
                                    if (_targetDistance <= 8000)
                                    {
                                        _inRange = true;
                                    }
                                }

                                if (_inRange)
                                {
                                    count += 1;

                                    Vessel toSave = v.Current;
                                    string shipDescription = v.Current.vesselName + " blueprints from " + HoloKronName + " " + hkCount;
                                    Debug.Log("[OrX Add Mission] Saving " + v.Current.vesselName + "'s orientation .......................");

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

                                    Debug.Log("[OrX Add Mission] Saving: " + v.Current.vesselName);
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

                                    Debug.Log("[OrX Add Mission] === " + v.Current.vesselName + " ENCRYPTED ===");
                                    saveShip = false;
                                    Debug.Log("[OrX Add Mission] " + v.Current.vesselName + " Saved to " + HoloKronName);
                                    ScreenMsg("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("[OrX Add Mission] === EXCEPTION: " + e + " in " + HoloKronName + " ===");
                        }
                    }
                    v.Dispose();
                }

                saveLocalVessels = false;

                _file.Save(hConfigLoc);
                Debug.Log("[OrX Add Mission] === " + HoloKronName + " Saved ===");

                if (!addCoords)
                {
                    OrXLog.instance.building = false;
                    OrXLog.instance.ResetFocusKeys();
                    addingMission = false;
                    GuiEnabledOrXMissions = false;
                    OrXHCGUIEnabled = false;
                    building = false;
                    buildingMission = false;
                    addCoords = false;
                    PlayOrXMission = false;
                    ResetData();
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
                    PlayOrXMission = false;
                    GuiEnabledOrXMissions = true;
                    OrXHCGUIEnabled = true;
                    addingMission = false;
                }
            }
            else
            {
                _file = ConfigNode.Load(hConfigLoc);
                if (_file == null)
                {
                    _file = new ConfigNode();
                    _file.AddValue("name", HoloKronName);
                    _file.AddNode("OrX");
                    _file.Save(hConfigLoc);
                }

                ConfigNode node = new ConfigNode();
                if (_file != null && _file.HasNode("OrX"))
                {
                    node = _file.GetNode("OrX");

                    hkCount = 0;
                    ConfigNode HoloKronNode = null;

                    foreach (ConfigNode cn in node.nodes)
                    {
                        if (cn.name.Contains("OrXHoloKronCoords" + hkCount))
                        {
                            Debug.Log("[OrX Save Config] === HoloKron " + hkCount + " FOUND ===");
                            cn.SetValue("extras", "True");
                            hkCount += 1;
                        }
                    }

                    if (node.HasNode("OrXHoloKronCoords" + hkCount))
                    {
                        Debug.Log("[OrX OrX Save Config] === ERROR === HOloKron " + hkCount + " FOUND AGAIN ===");

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
                        Debug.Log("[OrX OrX Save Config] === CREATING HoloKron " + hkCount + " ===");

                        HoloKronNode = node.AddNode("OrXHoloKronCoords" + hkCount);
                        HoloKronNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                        HoloKronNode.AddValue("spawned", "False");
                        HoloKronNode.AddValue("extras", "False");
                        HoloKronNode.AddValue("unlocked", "False");
                        HoloKronNode.AddValue("tech", tech);

                        HoloKronNode.AddValue("missionName", missionName);
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

                    //_file.Save(hConfigLoc);

                    HoloKronNode.SetValue("Targets", FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + "Password" + "," + _lat + "," + _lon + "," + _alt + ","
                    + missionName + "," + missionType + "," + challengeType + ":" + FlightGlobals.currentMainBody.name + "," + HoloKronName + "," + "Password" + "," + _lat + "," + _lon + "," + _alt + ","
                    + missionName + "," + missionType + "," + challengeType, true);

                    Debug.Log("[OrX OrX Save Config] === ADDING NODE 'HoloKron" + hkCount + "' ===");
                    node.AddNode("HoloKron" + hkCount);
                    ConfigNode HCnode = node.GetNode("HoloKron" + hkCount);
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

                    Debug.Log("[OrX OrX Save Config] === HOLO CRAFT ENCRYPTED ===");

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
                            Debug.Log("[OrX OrX Save Config] " + craftToAddMission + " Saved to " + HoloKronName);
                            ScreenMsg("<color=#cfc100ff><b>" + craftToAddMission + " Saved</b></color>");
                            blueprintsFile = "";
                            craftToAddMission = "";
                            blueprintsAdded = false;
                        }
                    }
                }
                coordCount = 0;

                if (saveLocalVessels)
                {
                    Debug.Log("[OrX Mission] === SAVING LOCAL VESELS ===");

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
                            bool _saveVessel = true;
                            Debug.Log("[OrX Mission] === CHECKING FOR EVA KERBAL ===");

                            List<Part>.Enumerator part = v.Current.parts.GetEnumerator();
                            while (part.MoveNext())
                            {
                                if (part.Current.Modules.Contains<KerbalEVA>())
                                {
                                    Debug.Log("[OrX Mission] === FOUND KERBAL ... SKIPPING ===");

                                    _saveVessel = false;
                                    break;
                                }
                            }
                            part.Dispose();

                            if (!v.Current.rootPart.Modules.Contains<ModuleOrXMission>() && !v.Current.rootPart.Modules.Contains<KerbalEVA>() && _saveVessel)
                            {
                                Debug.Log("[OrX Mission] === RANGE CHECK ===");

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

                                Debug.Log("[OrX Mission] Vessel " + v.Current.vesselName + " Identified .......................");

                                double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                                double _altDiffDeg = _altDiff * degPerMeter;
                                double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                                double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;
                                bool _inRange = false;

                                Debug.Log("[OrX Mission] === RANGE: " + _targetDistance);

                                if (v.Current.LandedOrSplashed)
                                {
                                    if (_targetDistance <= localSaveRange)
                                    {
                                        _inRange = true;
                                    }
                                }
                                else
                                {
                                    if (_targetDistance <= 8000)
                                    {
                                        _inRange = true;
                                    }
                                }

                                if (_inRange)
                                {
                                    count += 1;

                                    Vessel toSave = v.Current;
                                    string shipDescription = v.Current.vesselName + " blueprints from " + HoloKronName + " " + hkCount;
                                    Debug.Log("[OrX Mission] Saving " + v.Current.vesselName + "'s orientation .......................");

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

                                    Debug.Log("[OrX Mission] === " + v.Current.vesselName + " ENCRYPTED ===");
                                    saveShip = false;
                                    Debug.Log("[OrX Mission] " + v.Current.vesselName + " Saved to " + HoloKronName);
                                    ScreenMsg("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("[OrX Mission] === EXCEPTION: " + e + " in " + HoloKronName + " ===");
                        }
                    }
                    v.Dispose();
                }

                saveLocalVessels = false;

                //_file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + ".arch");
                _file.Save(hConfigLoc);
                Debug.Log("[OrX OrX Save Config] " + HoloKronName + " " + hkCount + " Saved");

                if (!addCoords)
                {
                    Debug.Log("[OrX OrX Save Config] " + HoloKronName + " building complete ... Saving");

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
                    _HoloKron.rootPart.explosionPotential *= 0.2f;
                    _HoloKron.rootPart.explode();
                    StartCoroutine(EndSave());
                }
                else
                {
                    Debug.Log("[OrX OrX Save Config] Adding to " + HoloKronName + " ..... Current count: " + hkCount);
                    getNextCoord = true;
                    showTargets = false;
                    movingCraft = false;
                    OrXVesselMove.Instance.StartMove(FlightGlobals.ActiveVessel, false, 0, false);
                }
            }
        }
        IEnumerator EndSave()
        {
            yield return new WaitForSeconds(2f);
            FlightGlobals.ForceSetActiveVessel(triggerVessel);
            challengeRunning = false;
        }

        public void Dummy() { }
        public void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }
        public void AddToolbarButton()
        {
            string OrXDir = "OrX/Plugin/";

            if (!hasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(OrXDir + "OrX_icon", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT, buttonTexture);
                hasAddedButton = true;
            }
        }

        #endregion

        /// 

        #region Missions

        public void SpawnByOrX(Vector3d vect)
        {
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            building = true;
            buildingMission = true;
            geoCache = true;
            OrXHCGUIEnabled = true;
            Goal = false;
            triggerVessel = FlightGlobals.ActiveVessel;
            OrXSpawnHoloKron.instance.StartSpawn(new Vector3d(), vect, false, true, true, HoloKronName, missionType);
        }
        public void SetupHolo(Vessel v, Vector3d holoPosition)
        {
            ResetData();
            OrXLog.instance.SetFocusKeys();
            _lat = holoPosition.x;
            _lon = holoPosition.y;
            _alt = holoPosition.z;
            _HoloKron = v;
            holoToAdd = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloKron/HoloKron.craft";
            missionType = "GEO-CACHE";
            challengeType = "GEO-CACHE";
            raceType = "";
            geoCache = true;
            locAdded = false;

            holoKronCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloKron/";
            holoKronFiles = new List<string>(Directory.GetFiles(holoKronCraftLoc, "*.craft", SearchOption.AllDirectories));

            sphLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/SPH/";
            sphFiles = new List<string>(Directory.GetFiles(sphLoc, "*.craft", SearchOption.AllDirectories));

            vabLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB/";
            vabFiles = new List<string>(Directory.GetFiles(vabLoc, "*.craft", SearchOption.AllDirectories));
            OrXLog.instance.building = true;
            building = true;
            buildingMission = true;
            startLocation = holoPosition;
            lastCoord = startLocation;
            showScores = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            movingCraft = false;
            spawned = true;
            OrXHCGUIEnabled = true;
            triggerVessel = FlightGlobals.ActiveVessel;
            FlightGlobals.ForceSetActiveVessel(_HoloKron);
        }
        public void OpenHoloKron(string holoName, Vessel holoKron, Vessel player)
        {
            challengeRunning = false;
            triggerVessel = player;
            _HoloKron = holoKron;
            HoloKronName = holoName;
            Debug.Log("[OrX Open HoloKron] === OPENING HOLOKRON === "); ;
            holoOpen = true;
            OrXLog.instance.SetFocusKeys();
            FlightGlobals.ForceSetActiveVessel(_HoloKron);
            building = false;
            coordCount = 0;
            _scoreboard = new List<string>();
            stageTimes = new List<string>();
            crafttosave = string.Empty;
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

            string _missionDescription0= "missionDescription0";
            string _missionDescription1 = "missionDescription1";
            string _missionDescription2 = "missionDescription2";
            string _missionDescription3 = "missionDescription3";
            string _missionDescription4 = "missionDescription4";
            string _missionDescription5 = "missionDescription5";
            string _missionDescription6 = "missionDescription6";
            string _missionDescription7 = "missionDescription7";
            string _missionDescription8 = "missionDescription8";
            string _missionDescription9 = "missionDescription9";


            if (HoloKronName != "")
            {
                ec = 0;
                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".orx");
                ConfigNode node = _file.GetNode("OrX");

                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (spawnCheck.name.Contains("OrXHoloKronCoords" + hkCount))
                    {
                        foreach (ConfigNode.Value data in spawnCheck.values)
                        {
                            string dataName = data.name;

                            if (data.name == _completed)
                            {
                                if (data.value == _false)
                                {
                                    Debug.Log("[OrX Open HoloKron] === HoloKron " + hkCount + " has not been completed ... ");
                                }
                                else
                                {
                                    Debug.Log("[OrX Open HoloKron] === HoloKron " + hkCount + " has been completed ... CHECKING FOR EXTRAS");

                                    if (spawnCheck.HasValue(_extras))
                                    {
                                        var t = spawnCheck.GetValue(_extras);
                                        if (t == _false)
                                        {
                                            Debug.Log("[OrX Open HoloKron] === HoloKron " + hkCount + " has no extras ... END TRANSMISSION");
                                            hkCount += 1;
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Open HoloKron] === HoloKron " + hkCount + " has extras ... SEARCHING");
                                            hkCount += 1;
                                        }
                                    }
                                }
                            }

                            if (data.name == _missionName)
                            {
                                missionName = data.value;
                            }

                            if (data.name == _missionType)
                            {
                                missionType = data.value;
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

                        if (missionType != "GEO-CACHE")
                        {
                            Debug.Log("[OrX Open HoloKron] === HOLOKRON TYPE IS CHALLENGE ===");

                            OrXLog.instance.mission = true;
                            geoCache = false;
                            showScores = true;
                        }
                        else
                        {
                            Debug.Log("[OrX Open HoloKron] === HOLOKRON TYPE IS GEO-CACHE ===");

                            geoCache = true;
                            showScores = false;
                            geoCache = true;
                        }

                        Debug.Log("[OrX Open HoloKron] === HOLOKRON DATA PROCESSED ===");
                    }

                    if (spawnCheck.name.Contains("HC" + hkCount + "OrXv"))
                    {
                        if (spawnCheck.name.Contains("HC" + hkCount + "OrXv0"))
                        {
                            Debug.Log("[OrX Open HoloKron] === GRABBING CRAFT FILE FOR " + spawnCheck.name + " ===");

                            foreach (ConfigNode.Value cv in spawnCheck.values)
                            {
                                if (cv.name == "vesselName")
                                {
                                    Debug.Log("[OrX Open HoloKron] === Blueprints found for '" + cv.value + "' ===");
                                    blueprintsAdded = true;
                                    crafttosave = cv.value;
                                }
                            }

                            Debug.Log("[OrX Open HoloKron] === GRABBING COORDS ===");

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

                            Debug.Log("[OrX Open HoloKron] === GRABBING CRAFT FILE DATA ===");

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

                            Debug.Log("[OrX Open HoloKron] === DECRYPTING CRAFT FILE DATA ===");

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

                            Debug.Log("[OrX Open HoloKron] === BLUEPRINTS READY ===");

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
                                            Debug.Log("[OrX Open HoloKron] " + HoloKronName + " is adding " + techToAdd + " to the tech list ...");
                                            OrXLog.instance.AddTech(techToAdd);
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Open HoloKron] " + techToAdd + " is already in the tech list ...");
                                        }
                                    }
                                }
                                if (cv.name == _spawned)
                                {
                                    cv.value = _true;
                                }
                            }

                            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".orx");
                            Debug.Log("[OrX Open HoloKron] " + HoloKronName + " Saved Status - SPAWNED");
                            break;
                        }
                    }
                }

                Debug.Log("[OrX Mission] === BLUEPRINTS PROCESSED ===");

                _mission = _file.GetNode("mission" + hkCount);
                if (_mission != null)
                {
                    Debug.Log("[OrX Open HoloKron] === MISSION " + hkCount + " FOUND ===");
                    CoordDatabase = new List<string>();

                    foreach (ConfigNode.Value cv in _mission.values)
                    {
                        if (cv.name.Contains("stage"))
                        {
                            CoordDatabase.Add(cv.value);
                            coordCount += 1;
                        }
                    }

                    Debug.Log("[OrX Open HoloKron] === FOUND " + coordCount + " COORDS IN MISSION " + hkCount + " ===");

                    if (CoordDatabase.Count >= 0)
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
                                    latMission = double.Parse(data[2]);
                                    altMission = double.Parse(data[3]);
                                    nextLocation = new Vector3d(latMission, latMission, altMission);




                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Debug.Log("[OrX Open HoloKron] HoloKron config file processed ...... ");
                            }
                        }
                        firstCoords.Dispose();

                        Debug.Log("[OrX Open HoloKron] === GETTING SCOREBOARD ===");
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

                        Debug.Log("[OrX Open HoloKron] === SCOREBOARD GENERATED ===");
                    }

                    ImportScores();

                    Debug.Log("[OrX Open HoloKron] === SETTING UP " + HoloKronName + " FOR CHALLENGE ===");
                }
            }

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

            Debug.Log("[OrX Open HoloKron] === OPENING " + HoloKronName + " GUI WINDOW ===");

            OrXHCGUIEnabled = true;
            showScores = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = true;
            movingCraft = false;
        }

        IEnumerator ChallengeStartDelay()
        {
            Debug.Log("[OrX Start Mission Delay] === Starting Delay ===");
            GuiEnabledOrXMissions = false;
            challengeRunning = true;
            OrXHCGUIEnabled = false;
            stageStart = true;
            holoOpen = false;
            while (FlightGlobals.ActiveVessel.srfSpeed <= 0.5f)
            {
                yield return null;
            }
            Debug.Log("[OrX Start Mission Delay] === Player Vessel Speed = " + FlightGlobals.ActiveVessel.srfSpeed + " ===");

            Debug.Log("[OrX Start Mission Delay] === Starting Challenge ===");
            StartChallenge();
        }
        public void StartChallenge()
        {
            if (challengeRunning)
            {

                if (geoCache)
                {
                    challengeRunning = false;
                    PlayOrXMission = false;
                    showScores = false;
                    _blueprints_.Save(blueprintsFile);
                    ScreenMsg("Blueprints Available .....");
                    Debug.Log("[OrX Start Mission] === '" + blueprintsFile + "' Available ===");
                    OrXLog.instance.mission = false;

                    _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName +  ".orx");
                    ConfigNode node = _file.GetNode("OrX");

                    node = _file.GetNode("OrX");

                    foreach (ConfigNode cn in node.nodes)
                    {
                        if (cn.name.Contains("OrXHoloKronCoords"))
                        {
                            Debug.Log("[OrX Mission] === HoloKron " + hkCount + " FOUND ===");

                            foreach (ConfigNode n in node.GetNodes("OrXHoloKronCoords" + hkCount))
                            {
                                n.SetValue("extras", "True");
                            }
                        }
                    }

                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (spawnCheck.name.Contains("OrXHoloKronCoords"))
                        {
                            ConfigNode HoloKronNode = node.GetNode("OrXHoloKronCoords" + hkCount);

                            if (HoloKronNode != null)
                            {
                                Debug.Log("[OrX Mission] === FOUND HOloKron === " + hkCount); ;

                                if (HoloKronNode.HasValue("completed"))
                                {
                                    var t = HoloKronNode.GetValue("completed");
                                    if (t == "False")
                                    {
                                        HoloKronNode.SetValue("completed", "True", true);

                                        Debug.Log("[OrX Mission] === HoloKron " + hkCount + " was not completed ... CHANGING TO TRUE"); ;
                                        break;
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Mission] === HOloKron " + hkCount + " is already completed ... "); ;
                                        hkCount += 1;
                                    }
                                }

                                Debug.Log("[OrX Start Mission] === DATA PROCESSED ===");
                            }
                        }
                    }

                    _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName +  ".orx");
                    OrXHCGUIEnabled = false;
                    checking = false;
                    GuiEnabledOrXMissions = false;
                    ResetData();
                }
                else
                {
                    ScreenMsg("Starting challenge ...........");
                    Debug.Log("[OrX Start Mission] === Starting Challenge ===");
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
                        }
                    }
                }
            }
        }
        public void SaveScore()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName +  ".orx");
            _mission = _file.GetNode("mission" + hkCount);
            _scoreboard_ = _mission.GetNode("scoreboard");

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

            if (coordCount >= 0)
            {
                Debug.Log("[OrX Mission Scoreboard] === GET CHALLENGER TOTAL TIME AND CREATE STAGE TIME LIST ===");
                int stageCount = 0;

                ConfigNode tempChallengerEntry = new ConfigNode();
                tempChallengerEntry.AddValue("challengersName", challengersName);
                double totalTimeChallenger = 0;
                if (stageTimes != null)
                {
                    List<string>.Enumerator st = stageTimes.GetEnumerator();
                    while (st.MoveNext())
                    {
                        try
                        {
                            if (st.Current != null)
                            {
                                string[] data = st.Current.Split(new char[] { ',' });
                                if (double.Parse(data[3]) != 0)
                                {
                                    stageCount += 1;

                                    totalTimeChallenger += double.Parse(data[3]);
                                    tempChallengerEntry.AddValue("stage" + stageCount, double.Parse(data[1]));
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    tempChallengerEntry.AddValue("totalTime", totalTimeChallenger);
                    Debug.Log("[OrX Mission Scoreboard] === STAGE TIME LIST CREATED ===");
                }

                ConfigNode scores = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".scores");
                if (scores == null)
                {
                    Debug.Log("[OrX Mission Scoreboard] === SCORES FILE DOESN'T EXIST ... CREATIING===");

                    scores = new ConfigNode();
                    scores.AddValue("name", HoloKronName);

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

                    scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".scores");
                    Debug.Log("[OrX Mission Scoreboard] === STAGE TIME LIST CREATED ===");

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

                    ConfigNode mis = scores.GetNode("mission" + hkCount);

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

                    scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".scores");
                }

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

                Debug.Log("[OrX Mission Scoreboard] === SCOREBOARD SAVED ===");
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

                        Debug.Log("[OrX Mission] === " + HoloKronName + " " + hkCount + " was not completed ... CHANGING TO TRUE ==="); ;
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === " + HoloKronName + " " + hkCount + " is already completed ==="); ;
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

            Debug.Log("[OrX Mission Scoreboard] === DATA PROCESSED AND ENCRYPTED ===");
            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName +  ".orx");
        }
        public void ImportScores()
        {
            updatingScores = true;

            Debug.Log("[OrX Import Scores] === CHECKING FOR SCORE IMPORT FILES ===");

            ConfigNode scores = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Import/" + HoloKronName + ".scores");
            if (scores != null)
            {
                Debug.Log("[OrX Import Scores] === SCORE IMPORT FILE FOR " + HoloKronName + " FOUND ===");

                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName +  ".scores");
                _mission = _file.GetNode("mission" + hkCount);
                _scoreboard_ = _mission.GetNode("scoreboard");

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

                foreach (ConfigNode cn in scores.nodes)
                {
                    if (cn.name.Contains("mission" + hkCount))
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
                                    ammendListscoreboard0 = false;
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
                                    ammendListscoreboard1 = false;
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
                                    ammendListscoreboard2 = false;
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
                                    ammendListscoreboard3 = false;
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
                                    ammendListscoreboard4 = false;
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
                                    ammendListscoreboard5 = false;
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
                                    ammendListscoreboard6 = false;
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
                                    ammendListscoreboard7 = false;
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
                                    ammendListscoreboard8 = false;
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
                                    ammendListscoreboard9 = false;
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

                Debug.Log("[OrX Import Scores] === DATA PROCESSED AND ENCRYPTED ===");
                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".orx");


            }

            updatingScores = false;
        }
        public void GetScoreboardData()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName +  ".orx");
            _mission = _file.GetNode("mission" + hkCount);
            _scoreboard_ = _mission.GetNode("scoreboard");
            int sbCount = 0;

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

            bool _save = false;

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
                _save = true;
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

            if (_save)
            {
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

                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".orx");

            }
            updatingScores = false;
            showScores = true;
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

            if (coordCount - gpsCount == 0)
            {
                if (coordCount >= 0)
                {
                    stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + (FlightGlobals.ActiveVessel.missionTime - missionTime));
                    SaveScore();
                }
                challengeRunning = false;
                PlayOrXMission = false;
                showScores = false;
                _blueprints_.Save(blueprintsFile);
                ScreenMsg("'" + craftToAddMission + "' Blueprints Available");
                Debug.Log("[OrX Target Manager] === '" + craftToAddMission + "' Blueprints Available ===");
                OrXLog.instance.mission = false;

                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".orx");
                ConfigNode node = _file.GetNode("OrX");

                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (spawnCheck.name.Contains("OrXHoloKronCoords"))
                    {
                        ConfigNode HoloKronNode = node.GetNode("OrXHoloKronCoords" + hkCount);

                        if (HoloKronNode != null)
                        {
                            Debug.Log("[OrX Mission] === FOUND HOloKron === " + hkCount); ;

                            if (HoloKronNode.HasValue("completed"))
                            {
                                var t = HoloKronNode.GetValue("completed");
                                if (t == "False")
                                {
                                    HoloKronNode.SetValue("completed", "True", true);

                                    Debug.Log("[OrX Target Manager] === HoloKron " + hkCount + " was not completed ... CHANGING TO TRUE"); ;
                                    break;
                                }
                                else
                                {
                                    Debug.Log("[OrX Target Manager] === HoloKron " + hkCount + " is already completed ... "); ;
                                    hkCount += 1;
                                }
                            }

                            Debug.Log("[OrX Target Manager] === DATA PROCESSED ===");
                        }
                    }
                }

                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + HoloKronName + ".orx");
            }
            else
            {
                if (stageStart)
                {
                    missionTime = FlightGlobals.ActiveVessel.missionTime;
                    stageStart = false;
                }
                double _time = FlightGlobals.ActiveVessel.missionTime - missionTime;
                missionTime = FlightGlobals.ActiveVessel.missionTime;
                stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + _time);
                topSurfaceSpeed = 0;

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
                                latMission = double.Parse(data[1]);
                                lonMission = double.Parse(data[2]);
                                altMission = double.Parse(data[3]);
                                nextLocation = new Vector3d(latMission, lonMission, altMission);
                                worldPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latMission, (double)lonMission, (double)altMission);
                                showTargets = true;
                                Debug.Log("[OrX Target Manager] === NEXT LOCATION ACQUIRED ===");
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
                Debug.Log("[OrX Target Manager] === CHECKING DISTANCE ===");
                OrXTargetDistance.instance.TargetDistance(false, true, true, true, nextLocation);
            }
        }
        public void ResetData()
        {
            OrXLog.instance.ResetFocusKeys();
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
        public void CancelChallenge()
        {
            Debug.Log("[OrX Mission] === CANCEL ===");
            locCount = 0;
            locAdded = false;
            building = false;
            buildingMission = false;
            addCoords = false;
            GuiEnabledOrXMissions = false;
            OrXHCGUIEnabled = false;
            PlayOrXMission = false;
            ResetData();
            FlightGlobals.ForceSetActiveVessel(triggerVessel);
        }

        public void SpawnGoal()
        {
            ScreenMsg("Spawning Stage Finish Line ... ");
            OrXSpawnHoloKron.instance.StartSpawn(startLocation, lastCoord, true, false, false, HoloKronName, missionType);
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

            if (dakarRacing)
            {
                saveLocalVessels = true;
                getNextCoord = false;
                addingMission = true;
                GuiEnabledOrXMissions = true;
                OrXHCGUIEnabled = true;
                movingCraft = true;
                dakarRacing = true;
                addCoords = true;
            }
            else
            {
                if (shortTrackRacing)
                {
                    saveLocalVessels = false;
                    getNextCoord = false;
                    addingMission = true;
                    GuiEnabledOrXMissions = true;
                    OrXHCGUIEnabled = true;
                    movingCraft = true;
                    dakarRacing = false;
                    addCoords = true;
                }
            }
            SaveConfig("");
        }

        #endregion


        #region Main GUI

        Vector3 worldPos;

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && !PauseMenu.isOpen)
            {
                if (showTargets)
                {
                    OrXLog.DrawTextureOnWorldPos(worldPos, OrXLog.instance.HoloTargetTexture, new Vector2(8, 8));
                }

                if (OrXHCGUIEnabled)
                {
                    WindowRectToolbar = GUI.Window(291827362, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
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
            if (OrXHCGUIEnabled)
            {
                Debug.Log("[OrX Mission]: Hiding OrX HoloKron GUI");
                OrXHCGUIEnabled = false;
            }
            else
            {
                if (!buildingMission)
                {
                    Debug.Log("[OrX Mission]: Opening OrX HoloKron GUI");
                    OrXHCGUIEnabled = true;
                    if (!challengeRunning)
                    {
                        GuiEnabledOrXMissions = false;
                        movingCraft = true;
                        getNextCoord = false;
                        GuiEnabledOrXMissions = true;
                        LoadHoloKronTargets();
                    }
                }
                else
                {
                    ScreenMsg("Unable to scan while creating .......");
                    ScreenMsg("Dinner Out is Cancelled .....");
                }
            }
        }
        public void OrXHCGUI(int OrX_HCGUI)
        {
            float line = 0;
            float leftIndent = 10;
            float contentWidth = toolWindowWidth - leftIndent;
            float contentTop = 10;
            float entryHeight = 20;
            float HCGUILines = 0;
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
            var centerLabelLarge = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }

            };
            var titleStyleLarge = new GUIStyle(centerLabelLarge)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };

            if (connectToKontinuum)
            {
                GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum", titleStyleLarge);
                line++;
                DrawBDAcOptions2(line);
                line++;
                DrawBDAcOptions3(line);
                line++;
                DrawBDAcOptions4(line);
                line++;

                if (GUI.Button(new Rect(LeftIndent * 1.5f, contentTop + (line * entryHeight), contentWidth * 0.9f, 20), "Connect", OrXGUISkin.button))
                {
                    connectToKontinuum = false;
                    ScreenMsg("The Kontinuum is currently unavailable .....");
                }
                line++;
            }
            else
            {
                if (GuiEnabledOrXMissions)
                {
                    GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));

                    if (movingCraft)
                    {
                        GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum", titleStyleLarge);
                        line++;

                        if (getNextCoord)
                        {
                            if (spawningStartGate)
                            {
                                if (GUI.Button(new Rect(LeftIndent * 1.5f, contentTop + (line * entryHeight), contentWidth * 0.9f, 20), "PLACE FINISH GATE", OrXGUISkin.button))
                                {
                                    Debug.Log("[OrX Place Gate] ===== PLACING GATE FOR " + HoloKronName + " STAGE " + hkCount + " =====");
                                    spawningStartGate = false;
                                    getNextCoord = false;
                                    OrXVesselMove.Instance.placingFinishGate = true;
                                    OrXVesselMove.Instance.EndMove(true, false, true);
                                }
                            }
                            else
                            {
                                if (dakarRacing)
                                {
                                    if (GUI.Button(new Rect(LeftIndent * 1.5f, contentTop + (line * entryHeight), contentWidth * 0.9f, 20), "ADD NEXT STAGE", OrXGUISkin.button))
                                    {
                                        Debug.Log("[OrX Place Gate] ===== ADDING STAGE " + (hkCount + 1) + " =====");
                                        CoordDatabase = new List<string>();
                                        addCoords = true;
                                        getNextCoord = false;
                                        startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);

                                        if (dakarRacing)
                                        {
                                            addingMission = false;
                                            SaveConfig(HoloKronName);
                                        }
                                        else
                                        {
                                            if (shortTrackRacing)
                                            {
                                                addingMission = true;
                                                SaveConfig(HoloKronName);
                                            }
                                        }
                                    }
                                    line++;
                                    DrawSave(line);
                                }
                            }
                        }
                        else
                        {

                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Scanning the Kontinuum .....", titleStyle);
                            line++;
                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "....... Please stand By", titleStyle);
                        }
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
                                DrawPlayHoloKronName(line);
                                line++;
                                DrawPlayMissionType(line);
                                line++;

                                if (!geoCache)
                                {
                                    DrawPlayRaceType(line);
                                    line++;
                                    DrawChallengerName(line);
                                    line++;
                                    if (bdaChallenge)
                                    {
                                        DrawBDAcOptions1(line);
                                        line++;
                                    }
                                }
                                if (blueprintsAdded)
                                {
                                    DrawPlayBlueprintsAdded(line);
                                    line++;
                                }

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
                                DrawStart(line);

                                if (!geoCache)
                                {
                                    line++;
                                    DrawShowScoreboard(line);
                                    line++;
                                    DrawCancel(line);
                                }
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
                                        DrawBDAcOptions1(line);
                                        line++;
                                    }
                                }

                                DrawHoloKronName(line);
                                line++;
                                DrawHoloKronName2(line);
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

                                line++;
                                DrawVessel(line);
                                line++;
                                DrawAddBlueprints(line);
                                line++;
                                DrawSaveLocal(line);
                                line++;
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

                                    if (!dakarRacing)
                                    {
                                        line++;
                                        DrawClearLastCoord(line);
                                        line++;
                                        DrawClearAllCoords(line);
                                        line++;
                                        line++;
                                        DrawSave(line);
                                        line++;
                                    }
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

                    line += 0.6f;
                    GUI.Label(new Rect(0, 1, WindowWidth, 20), "OrX Kontinuum", titleStyleLarge);

                    if (!challengeRunning)
                    {
                        if (checking)
                        {
                            scanDelay = OrXTargetDistance.instance.scanDelay;

                            if (targetDistance <= 100000)
                            {
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Closest HoloKron is " + Math.Round((targetDistance / 1000), 2) + " km away", titleStyle);
                                line++;
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Altitude: " + Math.Round(_altitude, 1) + " meters", titleStyle);
                                line++;
                            }
                            else
                            {
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "No HoloKron in range ......", titleStyle);
                                line++;
                                GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Rescan in " + Math.Round(scanDelay, 0) + " seconds", titleStyle);
                                line++;
                            }
                            line++;

                            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Stop HoloKron Scan", OrXGUISkin.button))
                            {
                                StopScan(true);
                            }
                        }
                        else
                        {
                            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Start HoloKron Scan", OrXGUISkin.button))
                            {
                                challengeRunning = false;
                                checking = true;
                                OrXTargetDistance.instance.TargetDistance(true, false, false, true, new Vector3());
                            }
                            line++;
                            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Connect to Kontinuum", OrXGUISkin.button))
                            {
                                connectToKontinuum = true;
                            }
                        }
                        line++;
                    }
                    else
                    {
                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Coordinates are " + Math.Round((targetDistance / 1000), 2) + " km away", titleStyle);
                        line++;
                        GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Altitude: " + Math.Round(_altitude, 1) + " meters", titleStyle);
                        line++;
                        line++;

                        if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Cancel Challenge", OrXGUISkin.button))
                        {
                            challengeRunning = false;
                            ScreenMsg("Cancelling " + HoloKronName + " " + hkCount + " Challenge .....");
                            locCount = 0;
                            locAdded = false;
                            building = false;
                            buildingMission = false;
                            addCoords = false;
                            GuiEnabledOrXMissions = false;
                            OrXHCGUIEnabled = false;
                            PlayOrXMission = false;
                            checking = false;
                            OrXTargetDistance.instance.StopAllCoroutines();
                            ResetData();
                        }
                        line++;
                    }

                    toolWindowHeight = Mathf.Lerp(toolWindowHeight, contentTop + (line * entryHeight) + 5, 1);
                    WindowRectToolbar.height = toolWindowHeight;

                }

            }

        }

        #endregion

        #region Coords GUI

        public void DrawAddCoords(float line)
        {
            dakarRacing = true;
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "ADD LOCATION", HighLogic.Skin.button))
            {
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
                    OrXVesselMove.Instance.EndMove(true, false, true);
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

        public bool getNextCoord = false;

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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), missionName + " Scoreboard", titleStyle);
        }
        public void DrawScoreboard0(float line)
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
        public void DrawScoreboard1(float line)
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
        public void DrawScoreboard2(float line)
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
        public void DrawScoreboard3(float line)
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
        public void DrawScoreboard4(float line)
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
        public void DrawScoreboard5(float line)
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
        public void DrawScoreboard6(float line)
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
        public void DrawScoreboard7(float line)
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
        public void DrawScoreboard8(float line)
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
        public void DrawScoreboard9(float line)
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
        public void DrawUpdateScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "IMPORT SCORES", HighLogic.Skin.button))
            {
                ImportScores();
            }
        }
        public void DrawCloseScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CLOSE SCOREBOARD", HighLogic.Skin.button))
            {
                showScores = false;
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

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, WindowWidth, 20), HoloKronName, titleStyle);
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
        public void DrawUnlock(float line)
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
        public void DrawShowScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!showScores)
            {
                if (!updatingScores)
                {
                    if (GUI.Button(saveRect, "SHOW SCOREBOARD", HighLogic.Skin.button))
                    {
                        updatingScores = true;
                        showScores = true;
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
            else
            {
            }
        }

        public void DrawStart(float line)
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
                            FlightGlobals.ForceSetActiveVessel(triggerVessel);
                            OrXLog.instance.ResetFocusKeys();
                            StartCoroutine(ChallengeStartDelay());
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

                    GuiEnabledOrXMissions = false;
                    challengeRunning = true;
                    geoCache = true;
                    OrXLog.instance.ResetFocusKeys();
                    FlightGlobals.ForceSetActiveVessel(triggerVessel);
                    StartChallenge();
                }
            }
        }

        #endregion

        #region Description Window GUI

        public void DrawClearDescription(float line)
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
                        ScreenMsg("HOLOKRON TYPE CHANGED TO CHALLENGE");
                        Debug.Log("[OrX Mission] === HOLOKRON TYPE - CHALLENGE ===");
                        challengeType = "OUTLAW RACING";
                        outlawRacing = true;
                        dakarRacing = true;
                        geoCache = false;
                        windRacing = false;
                        Scuba = false;
                        bdaChallenge = false;
                        missionType = "CHALLENGE";
                        raceType = "DAKAR RACING";
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === HOLOKRON LOCKED AS GEO-CACHE ===");
                        ScreenMsg("HOLOKRON TYPE LOCKED AS GEO-CACHE");
                        geoCache = true;
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
                        Debug.Log("[OrX Mission] === HOLOKRON TYPE - GEO-CACHE ===");
                        missionType = "GEO-CACHE";
                        challengeType = missionType;
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
                        Debug.Log("[OrX Mission] === HOLOKRON LOCKED AS CHALLENGE ===");
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
                if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        //Debug.Log("[OrX Mission] === CHALLENGE TYPE - SCUBA KERB ===");
                        challengeType = "SCUBA KERB";
                        ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                        bdaChallenge = false;
                        windRacing = false;
                        Scuba = true;
                        outlawRacing = false;

                    }
                    else
                    {
                        ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                        Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

                        //Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS W[ind/S] ===");

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
                            //Debug.Log("[OrX Mission] === CHALLENGE TYPE - BD ARMORY ===");
                            challengeType = "BD ARMORY";
                            ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                            bdaChallenge = true;
                            windRacing = false;
                            Scuba = false;
                            outlawRacing = false;
                        }
                        else
                        {
                            //Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS SCUBA ===");
                            ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

                        }
                    }
                }
                else
                {
                    if (bdaChallenge)
                    {
                        if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                        {
                            if (!locAdded)
                            {
                                //Debug.Log("[OrX Mission] === CHALLENGE TYPE - DAKAR RACING ===");
                                challengeType = "OUTLAW RACING";
                                Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                raceType = "DAKAR RACING";
                                ScreenMsg("CHALLENGE TYPE LOCKED AS DAKAR RACING");
                                bdaChallenge = false;
                                windRacing = false;
                                Scuba = false;
                                dakarRacing = true;
                                outlawRacing = true;
                            }
                            else
                            {
                                //Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS BD ARMORY ===");
                                Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                            }
                        }

                    }
                    else
                    {
                        if (outlawRacing)
                        {
                            if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                            {
                                if (!locAdded)
                                {
                                    //Debug.Log("[OrX Mission] === CHALLENGE TYPE - W[ind/S] ===");
                                    challengeType = "W[ind/S]";
                                    Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                    ScreenMsg("CHALLENGE TYPE LOCKED AS OUTLAW RACING");
                                    bdaChallenge = false;
                                    windRacing = true;
                                    Scuba = false;
                                    outlawRacing = false;
                                }
                                else
                                {
                                    //Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
                                    Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");
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
            var bfRect = new Rect(LeftIndent + contentWidth - 130, ContentTop + line * entryHeight, 120, entryHeight);

            if (dakarRacing)
            {
                if (GUI.Button(bfRect, raceType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        raceType = "SHORT TRACK";
                        Debug.Log("[OrX Mission] === RACE TYPE CHANGED TO SHORT TRACK ===");
                        ScreenMsg("RACE TYPE CHANGED TO SHORT TRACK");
                        dakarRacing = false;
                        shortTrackRacing = true;
                    }
                    else
                    {
                        raceType = "DAKAR RACING";
                        Debug.Log("[OrX Mission] === RACE TYPE LOCKED AS DAKAR RACING ===");
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
                        raceType = "DAKAR RACING";
                        dakarRacing = true;
                        Debug.Log("[OrX Mission] === RACE TYPE CHANGED TO DAKAR RACING ===");
                        ScreenMsg("RACE TYPE CHANGED TO DAKAR RACING");
                        shortTrackRacing = false;

                    }
                    else
                    {
                        raceType = "SHORT TRACK";
                        dakarRacing = true;
                        Debug.Log("[OrX Mission] === RACE TYPE LOCKED AS SHORT TRACK ===");
                        ScreenMsg("RACE TYPE LOCKED AS SHORT TRACK");
                    }
                }

            }
        }

        public void DrawBDAcOptions1(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), _labelConnect, leftLabel);
            var bfRect = new Rect(WindowWidth - LeftIndent - 10, ContentTop + line * entryHeight, 10, entryHeight);

            if (!connectToKontinuum)
            {
                if (GUI.Button(bfRect, "", HighLogic.Skin.button))
                {
                    connectToKontinuum = true;
                    _labelConnect = "Kontinuum Connect";
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", HighLogic.Skin.box))
                {
                    connectToKontinuum = false;
                    _labelConnect = "Connect to Kontinuum";
                }
            }
        }
        public void DrawBDAcOptions2(float line)
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
        public void DrawBDAcOptions3(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password: ",
                leftLabel);
            float textFieldWidth = ((WindowWidth / 3) * 2) - LeftIndent;
            var fwdFieldRect = new Rect((WindowWidth / 3), ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            pasKontinumm = GUI.TextField(fwdFieldRect, pasKontinumm);
        }
        public void DrawBDAcOptions4(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "URL: ",
                leftLabel);
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
                    Debug.Log("[OrX Mission] === ADDING BLUEPRINTS ===");
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
                    Debug.Log("[OrX Mission] === REMOVING BLUEPRINTS ===");
                    blueprintsLabel = "Add Blueprints to Holo";
                    blueprintsFile = "";
                    blueprintsAdded = false;
                }
            }

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

        bool saveToHoloKronDir = false;

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
            localSaveRange = GUI.HorizontalSlider(saveRect, localSaveRange, 50, 1000);
        }
        public void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!geoCache)
            {
                if (addCoords)
                {
                    if (GUI.Button(saveRect, "SAVE AND EXIT", HighLogic.Skin.button))
                    {
                        Debug.Log("[OrX Mission] === SAVING HOLOKRON ===");
                        addCoords = false;
                        addingMission = true;
                        saveLocalVessels = true;
                        getNextCoord = false;
                        SaveConfig("");
                    }
                }
                else
                {
                    if (!spawningStartGate)
                    {
                        if (GUI.Button(saveRect, "START ADD COORDS", HighLogic.Skin.button))
                        {
                            if (HoloKronName != string.Empty && HoloKronName != "")
                            {
                                if (missionDescription0 != string.Empty && missionDescription0 != "")
                                {
                                    CoordDatabase = new List<string>();
                                    addCoords = true;
                                    addingMission = false;
                                    startLocation = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                                    _file = ConfigNode.Load("GameData/OrX/HoloKron/" + HoloKronName + ".orx");
                                    if (_file == null)
                                    {
                                        SaveConfig(HoloKronName);
                                    }
                                    else
                                    {
                                        int _hkCount = 0;
                                        ConfigNode holoCounter = _file.GetNode("OrX");
                                        foreach (ConfigNode cn in holoCounter.nodes)
                                        {
                                            if (cn.name.Contains("OrXHoloKronCoords"))
                                            {
                                                _hkCount += 1;
                                            }
                                        }
                                        Debug.Log("[OrX Append Cfg] === FOUND " + _hkCount + " HOLOKRONS ===");
                                        OrXAppendCfg.instance.EnableGui(_hkCount, HoloKronName);
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
                        if (GUI.Button(saveRect, "START GATE SPAWNED", HighLogic.Skin.box))
                        {
                        }
                    }
                }
            }
            else
            {
                if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
                {
                    if (HoloKronName != string.Empty && HoloKronName != "")
                    {
                        if (missionDescription0 != string.Empty && missionDescription0 != "")
                        {
                            movingCraft = true;
                            spawningStartGate = false;

                            _file = ConfigNode.Load("GameData/OrX/Export/" + HoloKronName +  ".orx");
                            if (_file == null)
                            {
                                addCoords = false;

                                SaveConfig(HoloKronName);
                            }
                            else
                            {
                                int _hkCount = 0;
                                ConfigNode holoCounter = _file.GetNode("OrX");
                                foreach (ConfigNode cn in holoCounter.nodes)
                                {
                                    if (cn.name.Contains("OrXHoloKronCoords"))
                                    {
                                        _hkCount += 1;
                                    }
                                }
                                Debug.Log("[OrX Append Cfg] === FOUND " + _hkCount + " HOLOKRONS ===");
                                OrXAppendCfg.instance.EnableGui(_hkCount, HoloKronName);
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
        public void DrawCancel(float line)
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
                    Debug.Log("[OrX Mission] === CANCEL HOLOKRON CREATION ===");
                    _HoloKron.rootPart.explode();
                    CancelChallenge();
                }
            }
        }

        #endregion

    }
}