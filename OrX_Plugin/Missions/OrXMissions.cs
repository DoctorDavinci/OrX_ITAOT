using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using OrX.spawn;
using OrX.wind;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXMissions : MonoBehaviour
    {

        #region Fields

        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXMissions instance;
        public bool GuiEnabledOrXMissions = false;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;
        private Rect _scoreRect_;
        public double distance = 0;

        private bool scubaKerb = false;
        private bool tractorBeam = false;
        private bool jetPack = false;

        public bool completed = false;
        public string HoloCacheName = string.Empty;
        public string missionName = string.Empty;
        public string missionType = string.Empty;
        public string challengeType = string.Empty;
        public string techToAdd = string.Empty;

        public string missionDescription0 = string.Empty;
        private bool m0 = false;
        public string missionDescription1 = string.Empty;
        private bool m1 = false;
        public string missionDescription2 = string.Empty;
        private bool m2 = false;
        public string missionDescription3 = string.Empty;
        private bool m3 = false;
        public string missionDescription4 = string.Empty;
        private bool m4 = false;
        public string missionDescription5 = string.Empty;
        private bool m5 = false;
        public string missionDescription6 = string.Empty;
        private bool m6 = false;
        public string missionDescription7 = string.Empty;
        private bool m7 = false;
        public string missionDescription8 = string.Empty;
        private bool m8 = false;
        public string missionDescription9 = string.Empty;
        private bool m9 = false;

        public string tech = string.Empty;
        public int mCount = 0;
        public bool spawned = false;

        public string Gold = string.Empty;
        public string Silver = string.Empty;
        public string Bronze = string.Empty;

        public string textBox = string.Empty;

        public bool racing = false;


        public bool Scuba = false;
        private int depth = 0;

        public bool windRacing = false;
        public float heading = 0;
        public float windIntensity = 10;
        public float teaseDelay = 0;
        public float windVariability = 50;
        public float variationIntensity = 50;

        private bool PlayOrXMission = false;
        private bool craftBrowserOpen = false;
        private bool holoCraftSelected = false;
        public bool blueprintsAdded = false;

        public static Rect WindowRectBrowser;
        public static Rect WindowRectCS;
        float listHeight;
        float browserWindowWidth = 250;
        float browserWindowHeight = 24;
        public static GUISkin OrXBrowserSkin = HighLogic.Skin;
        float entryCount;
        GUIStyle craftTitleLabel;
        public static GUISkin OrXCraftSkin = HighLogic.Skin;
        private int craftIndex;
        float craftEntryHeight = 24;

        double _lat = 0;
        double _lon = 0;
        double _alt = 0;
        Vessel missionCraft;

        public string Password = "OrX";
        public string pas = string.Empty;

        private bool unlocked = false;

        private bool holoSpawned = false;
        private bool editDescription = false;
        private string description = string.Empty;
        public string craftFile = string.Empty;
        public string blueprintsFile = string.Empty;
        public string craftToAdd = string.Empty;
        public string holoToAdd = string.Empty;

        int gpsCount = 0;
        double lat = 0;
        double lon = 0;
        double alt = 0;
        private bool locAdded = false;
        Vector3 lastCoord;
        int locCount = 0;

        string NextCoord;
        List<string> CoordDatabase;
        int coordCount = 0;

        private bool geoCache = true;
        private bool addingBluePrints = false;
        Vector3 _location;
        Guid id;

        List<string> stageTimes;
        private double maxDepth = 0;

        List<string> _scoreboard;
        private string challengersName = string.Empty;
        private double topSurfaceSpeed = 0;


        #endregion

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && (OrXLog.instance.mission || OrXLog.instance.story))
            {
                if (FlightGlobals.ActiveVessel.altitude <= maxDepth)
                {
                    maxDepth = FlightGlobals.ActiveVessel.altitude;
                }
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        private void Start()
        {
            ResetData();
            _scoreRect_ = new Rect((Screen.width / 2) - (WindowWidth /2), 50, WindowWidth, _windowHeight);
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth * 3) + 10, 50, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisableOrXMissions);
            GameEvents.onShowUI.Add(GameUiEnableOrXMissions);
            _gameUiToggle = true;
            distance = 0;
        }

        public void StartMissionBuilder(Vector3 data, Guid _id)
        {
            ResetData();
            OrXLog.instance.building = true;
            building = true;
            id = _id;
            _location = data;
            _lat = data.x;
            _lon = data.y;
            _alt = data.z;
            showScores = false;
            GuiEnabledOrXMissions = false;
            PlayOrXMission = false;
            craftBrowserOpen = true;
        }

        private void ResetData()
        {
            m0 = false;
            m1 = false;
            m2 = false;
            m3 = false;
            m4 = false;
            m5 = false;
            m6 = false;
            m7 = false;
            m8 = false;
            m9 = false;

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
            _scoreboard.Clear();
            stageTimes.Clear();
            CoordDatabase.Clear();

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

        public void WrapText(string s)
        {
            m0 = false;
            m1 = false;
            m2 = false;
            m3 = false;
            m4 = false;
            m5 = false;
            m6 = false;
            m7 = false;
            m8 = false;
            m9 = false;

            int _count = 0;
            string formatted = "";
            int lengthAvailable = 25;

            string[] wordArray = s.Trim().Split(' ');

            foreach (string w in wordArray)
            {
                string word = w;
                if (word == "")
                {
                    continue;
                }

                int length = word.Length;

                if (length >= 25)
                {
                    if (lengthAvailable > 0)
                    {
                        formatted += word.Substring(0, lengthAvailable) + "\n";
                        word = word.Substring(lengthAvailable);
                    }
                    else
                    {
                        formatted += "\n";
                    }
                    word = word + " ";
                    lengthAvailable = 25;
                    for (var count = 0; count < word.Length; count++)
                    {
                        char ch = word.ElementAt(count);

                        if (lengthAvailable == 0)
                        {
                            formatted += "\n";
                            lengthAvailable = 25;
                        }

                        formatted += ch;
                        lengthAvailable--;
                    }
                    continue;
                }

                if ((length + 1) <= lengthAvailable)
                {
                    formatted += word + " ";
                    lengthAvailable -= (length + 1);

                    if (_count == 0)
                    {
                        missionDescription0 = formatted;
                        m0 = true;
                    }

                    if (_count == 1)
                    {
                        missionDescription1 = formatted;
                        m1 = true;
                    }

                    if (_count == 2)
                    {
                        missionDescription2 = formatted;
                        m2 = true;
                    }

                    if (_count == 3)
                    {
                        missionDescription3 = formatted;
                        m3 = true;
                    }

                    if (_count == 4)
                    {
                        missionDescription4 = formatted;
                        m4 = true;
                    }

                    if (_count == 5)
                    {
                        missionDescription5 = formatted;
                        m5 = true;
                    }

                    if (_count == 6)
                    {
                        missionDescription6 = formatted;
                        m6 = true;
                    }

                    if (_count == 7)
                    {
                        missionDescription7 = formatted;
                        m7 = true;
                    }

                    if (_count == 8)
                    {
                        missionDescription8 = formatted;
                        m8 = true;
                    }

                    if (_count == 9)
                    {
                        missionDescription9 = formatted;
                        m9 = true;
                    }

                    _count += 1;

                    continue;
                }
                else
                {
                    lengthAvailable = 25;
                    formatted += "\n" + word + " ";
                    lengthAvailable -= (length + 1);

                    if (_count == 0)
                    {
                        missionDescription0 = formatted;
                        m0 = true;
                    }

                    if (_count == 1)
                    {
                        missionDescription1 = formatted;
                        m1 = true;
                    }

                    if (_count == 2)
                    {
                        missionDescription2 = formatted;
                        m2 = true;
                    }

                    if (_count == 3)
                    {
                        missionDescription3 = formatted;
                        m3 = true;
                    }

                    if (_count == 4)
                    {
                        missionDescription4 = formatted;
                        m4 = true;
                    }

                    if (_count == 5)
                    {
                        missionDescription5 = formatted;
                        m5 = true;
                    }

                    if (_count == 6)
                    {
                        missionDescription6 = formatted;
                        m6 = true;
                    }

                    if (_count == 7)
                    {
                        missionDescription7 = formatted;
                        m7 = true;
                    }

                    if (_count == 8)
                    {
                        missionDescription8 = formatted;
                        m8 = true;
                    }

                    if (_count == 9)
                    {
                        missionDescription9 = formatted;
                        m9 = true;
                    }
                    _count += 1;

                    continue;
                }
            }
        }

        public void StartMission(string hcn, int mc, Vessel v) /// LOAD .orx
        {
            ResetData();
            OrXLog.instance.mission = true;
            building = false;
            missionCraft = v;
            coordCount = 0;
            _scoreboard.Clear();
            stageTimes.Clear();
            CoordDatabase.Clear();
            HoloCacheName = hcn;
            mCount = mc;
            int hcCount = 0;
            int vn = 0;
            bool hasSpawned = true;


            if (HoloCacheName != "")
            {
                ec = 0;
                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                _mission = _file.GetNode("mission" + mCount);
                _scoreboard_ = _mission.GetNode("scoreboard");

                ConfigNode node = _file.GetNode("OrX");
                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (hasSpawned)
                    {
                        if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                        {
                            Debug.Log("[OrX Missions] === FOUND HOLOCACHE === " + hcCount); ;

                            ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + hcCount);

                            foreach (ConfigNode.Value cv in HoloCacheNode.values)
                            {
                                if (cv.name == "spawned")
                                {
                                    if (cv.value == "False")
                                    {
                                        Debug.Log("[OrX Missions] === HOLOCACHE " + hcCount + " has not spawned ... "); ;

                                        foreach (ConfigNode.Value data in HoloCacheNode.values)
                                        {
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

                                        hasSpawned = false;
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Missions] === HOLOCACHE " + hcCount + " has spawned ... CHECKING FOR EXTRAS"); ;

                                        if (HoloCacheNode.HasValue("extras"))
                                        {
                                            var t = HoloCacheNode.GetValue("extras");
                                            if (t == "False")
                                            {
                                                Debug.Log("[OrX Missions] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION"); ;
                                                hasSpawned = false;
                                                break;
                                            }
                                            else
                                            {
                                                Debug.Log("[OrX Missions] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;
                                                hasSpawned = true;
                                                hcCount += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string crafttosave = string.Empty;

                foreach (ConfigNode cnode in node.nodes)
                {
                    if (cnode.name.Contains("HC" + hcCount + "OrXv"))
                    {
                        ConfigNode local = cnode.GetNode("HC" + hcCount + "OrXv" + vn);
                        foreach (ConfigNode.Value cv in local.values)
                        {
                            if (cv.name == "vesselName")
                            {
                                Debug.Log("[OrX Missions] === Blueprints found for '" + cv.value + "' ===");

                                crafttosave = cv.value;
                            }
                        }

                        ConfigNode location = local.GetNode("coords");
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

                        ConfigNode craftFile = local.GetNode("craft");
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
                                    string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                    cv2.name = cvEncryptedName;
                                    cv2.value = cvEncryptedValue;
                                }
                            }
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

                        blueprintsFile = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder
                            + "/Ships/" + _type + crafttosave + ".craft";

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
                                        Debug.Log("[OrX Missions] " + HoloCacheName + " is adding " + techToAdd + " to the tech list ...");
                                        OrXLog.instance.AddTech(techToAdd);
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Missions] " + techToAdd + " is already in the tech list ...");
                                    }
                                }
                            }
                            if (a == "spawned")
                            {
                                cv.value = OrXLog.instance.Crypt("True");
                            }
                        }

                        _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                        Debug.Log("[OrX Missions] " + HoloCacheName + " Saved Status - SPAWNED");
                    }
                }

                ConfigNode mission = _file.GetNode("mission" + mCount);
                foreach (ConfigNode.Value cv in mission.values)
                {
                    if (cv.name.Contains("missionCoord"))
                    {
                        coordCount += 1;
                        CoordDatabase.Add(cv.value);
                    }
                }

                List<string>.Enumerator firstCoords = CoordDatabase.GetEnumerator();
                while (firstCoords.MoveNext())
                {
                    try
                    {
                        string[] data = firstCoords.Current.Split(new char[] { ',' });
                        if (data[0] == "1")
                        {
                            gpsCount = 2;
                            NextCoord = firstCoords.Current;
                            break;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.Log("[OrX Missions] HoloCache config file processed ...... ");
                    }
                }
                firstCoords.Dispose();

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

                building = false;
                
                showScores = true;
                PlayOrXMission = true; /// PUT AT END OF METHOD
                _gameUiToggle = true;
                if (missionType != "GEO-CACHE")
                {
                    showScores = true;
                }
            }
        }

        ConfigNode _file;
        ConfigNode _mission;
        ConfigNode _scoreboard_;

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

        private void GetScoreboardData()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            _mission = _file.GetNode("mission" + mCount);
            _scoreboard_ = _mission.GetNode("scoreboard");

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
        }

        private void SaveScore()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            _mission = _file.GetNode("mission" + mCount);
            _scoreboard_ = _mission.GetNode("scoreboard");

            if (_scoreboard_.nodes.Contains("scoreboard0"))
            {
                // DO NOTHING
            }
            else
            {
                // ADD NEW PODIUM LIST
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

            // GET CHALLENGER TOTAL TIME AND CREAT STAGE TIME LIST
            int stageCount = 0;

            ConfigNode tempChallengerEntry = null;
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
            string nameToRemovescoreboard2= string.Empty;
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

            bool ammendListscoreboard5= false;
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

            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
        }





        public void StartEndGame()
        {

        }

        private void GetNextCoord()
        {
            bool getCoord = true;
            List<string>.Enumerator coords = CoordDatabase.GetEnumerator();
            while (coords.MoveNext())
            {
                try
                {
                    if (coordCount - gpsCount == 0)
                    {
                        StartEndGame();
                    }
                    else
                    {
                        if (getCoord)
                        {
                            string[] data = coords.Current.Split(new char[] { ',' });
                            if (data[0] == gpsCount.ToString())
                            {
                                stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth
                                    + "," + FlightGlobals.ActiveVessel.missionTime.ToString());

                                gpsCount += 1;
                                maxDepth = 0;
                                getCoord = false;
                                NextCoord = coords.Current;
                                break;
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.Log("[OrX Missions] NEXT LOCATION ACQUIRED ...... ");
                }
            }
            coords.Dispose();
        }

        private void SaveConfig()
        {
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName))
            {
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName);
            }
            ConfigNode file = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            if (file == null)
            {
                file = new ConfigNode();
                file.AddValue("name", HoloCacheName);
                file.AddNode("OrX");
                file.Save("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            }

            ConfigNode node = null;
            if (file != null && file.HasNode("OrX"))
            {
                int hcCount = 0;
                mCount = 0;
                node = file.GetNode("OrX");
                ConfigNode HoloCacheNode = null;

                foreach (ConfigNode cn in node.nodes)
                {
                    if (cn.name.Contains("OrXHoloCacheCoords"))
                    {
                        hcCount += 1;
                    }

                    if (cn.name.Contains("mission"))
                    {
                        mCount += 1;
                    }
                }

                ConfigNode mission = file.GetNode("mission" + mCount);
                if (mission == null)
                {
                    file.AddNode("mission" + mCount);

                }

                mission = file.GetNode("mission" + mCount);
                ConfigNode scoreboard = mission.AddNode("scoreboard");

                List<string>.Enumerator missionCoords = CoordDatabase.GetEnumerator();
                while (missionCoords.MoveNext())
                {
                    try
                    {
                        if (missionCoords.Current == null) continue;
                        coordCount += 1;
                        mission.AddValue("missionCoord" + coordCount, missionCoords.Current);
                    }
                    catch
                    {

                    }
                }
                missionCoords.Dispose();

                if (node.HasNode("OrXHoloCacheCoords" + hcCount))
                {
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
                    }
                }
                else
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
                }

                string targetString = HoloCacheListToString();
                HoloCacheNode.SetValue("Targets", targetString, true);
                if (!node.HasNode("HoloCache" + hcCount))
                {
                    node.AddNode("HoloCache" + hcCount);
                }

                ConfigNode craftFileLoc = ConfigNode.Load(craftFile);
                ConfigNode HCnode = node.GetNode("HoloCache" + hcCount);
                craftFileLoc.CopyTo(HCnode);
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

                file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                Debug.Log("[OrX Missions] " + HoloCacheName + " Saved");

                int count = 0;

                if (blueprintsAdded)
                {
                    ConfigNode addedCraft = ConfigNode.Load(blueprintsFile);

                    if (addedCraft != null)
                    {
                        foreach (ConfigNode n in node.nodes)
                        {
                            if (n.name.Contains("HC" + hcCount + "OrXv"))
                            {
                                count += 1;
                            }
                        }

                        ConfigNode craftData = node.AddNode("HC" + hcCount + "OrXv" + count);
                        craftData.AddValue("vesselName", craftToAdd);
                        ConfigNode location = craftData.AddNode("coords");
                        location.AddValue("holo", hcCount);
                        location.AddValue("pas", Password);
                        location.AddValue("lat", FlightGlobals.ActiveVessel.latitude);
                        location.AddValue("lon", FlightGlobals.ActiveVessel.longitude);
                        location.AddValue("alt", FlightGlobals.ActiveVessel.altitude);

                        foreach (ConfigNode.Value cv in location.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        ConfigNode craftFile = craftData.AddNode("craft");
                        ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloCacheName + "</b></color>");
                        Debug.Log("[OrX Missions] Saving: " + craftToAdd);
                        addedCraft.CopyTo(craftFile);

                        foreach (ConfigNode.Value cv in craftFile.values)
                        {
                            if (cv.name == "ship")
                            {
                                cv.value = craftToAdd;
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

                        file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                        Debug.Log("[OrX Missions] " + craftToAdd + " Saved to " + HoloCacheName);
                        ScreenMsg("<color=#cfc100ff><b>" + craftToAdd + " Saved</b></color>");
                    }
                }
            }

            coordCount = 0;
            file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            SpawnOrX_HoloCache.instance.SpawnMissionHolo(holoToAdd, _location, false);
        }

        private int ec = 0;
        public bool building = false;


        private void StartChallenge()
        {
            if (missionType == "GEO-CACHE")
            {

            }
            else
            {
                if (challengeType == "WIND RACING")
                {
                    // SETUP WIND CONTROLLER
                }
                else
                {
                    if (challengeType == "SCUBA CHALLENGE")
                    {
                        // START A DEPTH MONITORING MONOBEHAVIOUR
                    }
                    else
                    {
                        if (challengeType == "ANYTHING GOES")
                        {

                        }
                    }
                }
            }


            // START A SCOREBOARD



        }




        #region GUI

        private void OnGUI()
        {
            if (GuiEnabledOrXMissions && _gameUiToggle)
            {
                building = true;
                _windowRect = GUI.Window(492265212, _windowRect, GuiWindowOrXMissions, "");
            }

            if (editDescription)
            {
                _scoreRect_ = GUI.Window(269255922, _scoreRect_, GuiOrXEditDescription, "");
            }

            if (craftBrowserOpen && _gameUiToggle)
            {
                building = true;
                GuiEnabledOrXMissions = false;
                PlayOrXMission = false;
                _windowRect = GUI.Window(30162892, _windowRect, GuiWindowOrX_MissionCraftBrowser, "");
            }

            if (PlayOrXMission && _gameUiToggle)
            {
                building = false;
                GuiEnabledOrXMissions = false;
                _windowRect = GUI.Window(260075212, _windowRect, GuiWindowPlayOrXMission, "");
            }
            
            if (showScores && PlayOrXMission)
            {
                _scoreRect_ = GUI.Window(269271212, _scoreRect_, GuiWindowOrXScoreboard, "");
            }
        }
        private void EnableGui()
        {
            craftBrowserOpen = true;
            Debug.Log("[OrX Missions]: Showing OrXMissions GUI");
        }
        private void DisableGui()
        {
            PlayOrXMission = false;
            GuiEnabledOrXMissions = false;
            Debug.Log("[OrX Missions]: Hiding OrXMissions GUI");
        }
        private void GameUiEnableOrXMissions()
        {
            _gameUiToggle = true;
        }
        private void GameUiDisableOrXMissions()
        {
            _gameUiToggle = false;
        }


        #region Scoreboard

        bool showScores = false;

        private void GuiWindowOrXScoreboard(int OrX_Scoreboard) 
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

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
            DrawCloseScoreboard(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), missionName + " Scoreboard", titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB0 + " - " + String.Format("{0:0.00}", timeSB0), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB1 + " - " + String.Format("{0:0.00}", timeSB1), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB2 + " - " + String.Format("{0:0.00}", timeSB2), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB3 + " - " + String.Format("{0:0.00}", timeSB3), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB4 + " - " + String.Format("{0:0.00}", timeSB4), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB5 + " - " + String.Format("{0:0.00}", timeSB5), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB6 + " - " + String.Format("{0:0.00}", timeSB6), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB7 + " - " + String.Format("{0:0.00}", timeSB7), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB8 + " - " + String.Format("{0:0.00}", timeSB8), titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB9 + " - " + String.Format("{0:0.00}", timeSB9), titleStyle);
        }

        private void DrawCloseScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CLOSE", HighLogic.Skin.button))
            {
                showScores = false;
            }
        }

        #endregion

        #region Craft Browser

        void GuiWindowOrX_MissionCraftBrowser(int OrX_MissionCraftBrowser)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));

            float line = 0;
            float leftIndent = 10;
            float contentWidth = WindowWidth - leftIndent;
            float contentTop = 10;
            float entryHeight = 20;
            float gpsLines = 0;

            line += 0.6f;

            GUI.BeginGroup(new Rect(5, contentTop + (line * entryHeight), WindowWidth, WindowRectCS.height));
            WindowCraftBrowser();
            GUI.EndGroup();
            gpsLines = WindowRectCS.height / entryHeight;

            listHeight = Mathf.Lerp(listHeight, gpsLines, 0.15f);
            line += listHeight;
            line += 0.25f;
            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), 250 - 5, 20), "CANCEL", OrXCraftSkin.button))
            {
                if (!addingBluePrints)
                {
                    if (!holoCraftSelected)
                    {
                        Debug.Log("[OrX MISSIONS] === NO HOLOCACHE SELECTED ... CANCELLING ===");
                    }
                    else
                    {
                        Debug.Log("[OrX MISSIONS] === REMOVING HOLOCACHE ===");
                    }

                    craftBrowserOpen = false;
                    craftFile = string.Empty;
                    holoCraftSelected = false;
                }
                else
                {
                    if (blueprintsAdded)
                    {
                        Debug.Log("[OrX MISSIONS] === REMOVING BLUEPRINTS ===");
                    }
                    else
                    {
                        Debug.Log("[OrX MISSIONS] === NO BLUEPRINTS SELECTED ... CANCELLING ===");
                    }

                    addingBluePrints = false;
                    blueprintsFile = string.Empty;
                    craftToAdd = string.Empty;
                    addingBluePrints = false;
                    blueprintsAdded = false;
                }

                PlayOrXMission = false;
                craftBrowserOpen = false;
                GuiEnabledOrXMissions = true;
            }
            line += 1.25f;
            browserWindowHeight = Mathf.Lerp(browserWindowHeight, contentTop + (line * entryHeight) + 5, 1);
            WindowRectBrowser.height = browserWindowHeight;
        }
        public void WindowCraftBrowser()
        {
            GUI.Box(WindowRectCS, GUIContent.none, OrXBrowserSkin.button);
            entryCount = 0;
            Rect listRect = new Rect(5, 5, 240 - (2 * 5),
                WindowRectCS.height - (2 * 5));
            GUI.BeginGroup(listRect);
            GUI.Label(new Rect(0, 0, listRect.width, 20), "Craft Files", craftTitleLabel);
            entryCount += 1.2f;
            int index = 0;

            string craftLoc = string.Empty;
            List<string> files = new List<string>();

            if (missionType == "GEO-CACHE")
            {
                if (!addingBluePrints)
                {
                    geoCache = true;
                    craftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloCache/";
                    files = new List<string>(Directory.GetFiles(craftLoc, "*.craft", SearchOption.AllDirectories));
                }
                else
                {
                    craftLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/";
                    files = new List<string>(Directory.GetFiles(craftLoc, "*.craft", SearchOption.AllDirectories));
                }
            }
            else
            {
                craftLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/";
                files = new List<string>(Directory.GetFiles(craftLoc, "*.craft", SearchOption.AllDirectories));
                geoCache = false;
            }

            if (files != null)
            {
                List<string>.Enumerator craftFilesToAdd = files.GetEnumerator();
                while (craftFilesToAdd.MoveNext())
                {
                    Color origWColor = GUI.color;
                    ConfigNode craft = ConfigNode.Load(craftFilesToAdd.Current);
                    string vn = "";

                    try
                    {
                        foreach (ConfigNode.Value cv in craft.values)
                        {
                            if (cv.name == "ship")
                            {
                                vn = cv.value;
                            }
                        }

                        if (GUI.Button(new Rect(0, entryCount * craftEntryHeight, 240, craftEntryHeight), vn, OrXCraftSkin.button))
                        {
                            if (addingBluePrints)
                            {
                                blueprintsFile = craftFilesToAdd.Current;
                                craftToAdd = vn;
                                craftBrowserOpen = false;
                                addingBluePrints = false;
                                blueprintsAdded = true;
                            }
                            else
                            {
                                if (!holoCraftSelected)
                                {
                                    Debug.Log("[OrX MISSIONS] === HOLOCACHE SELECTED ===");
                                    //craftLoc = craftFilesToAdd.Current;
                                    craftBrowserOpen = false;
                                    craftFile = vn;
                                    holoCraftSelected = true;
                                    holoToAdd = craftFilesToAdd.Current;
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[OrX MISSIONS] Exception Thrown While Listing Craft ...... Continuing");
                    }

                    entryCount++;
                    index++;
                    GUI.color = origWColor;
                }
                craftFilesToAdd.Dispose();
            }
            else
            {
                Debug.Log("[OrX MISSIONS] === Craft Files Not Found ===");
            }

            GUI.EndGroup();
            WindowRectCS.height = (2 * 5) + (entryCount * craftEntryHeight);
        }

        #endregion

        #region Play Mission

        private void GuiWindowPlayOrXMission(int Play_OrXMission) // USE COORDS SENT FROM ModuleOrXMission
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawPlayHoloCacheName(line);
            line++;
            DrawPlayTitle(line);
            line++;
            DrawPlayMissionType(line);
            line++;
            if (missionType != "GEO-CACHE")
            {
                DrawPlayRaceType(line);
                line++;
            }
            if (blueprintsAdded)
            {
                DrawPlayBlueprintsAdded(line);
                line++;
            }
            line++;

            if (m0)
            {
                DrawDescription0(line);
                line++;
                if (m1)
                {
                    DrawDescription1(line);
                    line++;

                    if (m2)
                    {
                        DrawDescription2(line);
                        line++;

                        if (m3)
                        {
                            DrawDescription3(line);
                            line++;

                            if (m4)
                            {
                                DrawDescription4(line);
                                line++;

                                if (m5)
                                {
                                    DrawDescription5(line);
                                    line++;

                                    if (m6)
                                    {
                                        DrawDescription6(line);
                                        line++;

                                        if (m7)
                                        {
                                            DrawDescription7(line);
                                            line++;

                                            if (m8)
                                            {
                                                DrawDescription8(line);
                                                line++;

                                                if (m9)
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
            }

            if (!unlocked)
            {
                DrawPlayPassword(line++);
                line++;
                DrawUnlock(line);
            }
            else
            {
                DrawChallengerName(line);
                line++;
                DrawStart(line);
            }
            line++;
            DrawCancel(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void DrawPlayHoloCacheName(float line)
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), HoloCacheName, titleStyle);
        }


        private void DrawPlayTitle(float line)
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), missionName, titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), missionType, titleStyle);
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), challengeType, titleStyle);
        }
        private void DrawPlayBlueprintsAdded(float line)
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), challengeType, titleStyle);
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
                    unlocked = true;
                }
                else
                {
                    ScreenMsg("WRONG PASSWORD");
                }
            }
        }

        private void DrawStart(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "START CHALLENGE", HighLogic.Skin.button))
            {
                if (challengersName != "" || challengersName != string.Empty)
                {
                    StartChallenge();
                }
                else
                {
                    ScreenMsg("Please enter a challenger name");
                }
            }
        }

        #endregion

        #region Edit Description Window


        private void GuiOrXEditDescription(int OrX_EditDescription)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawEditTitle(line);
            line++;
            line++;
            DrawDescription0(line);
            line++;
            DrawDescription1(line);
            line++;
            DrawDescription2(line);
            line++;
            DrawDescription3(line);
            line++;
            DrawDescription4(line);
            line++;
            DrawDescription5(line);
            line++;
            DrawDescription6(line);
            line++;
            DrawDescription7(line);
            line++;
            DrawDescription8(line);
            line++;
            DrawDescription9(line);
            line++;
            line++;
            DrawClearDescription(line);
            line++;
            DrawSaveDescription(line);
            

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

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
                description = string.Empty;
            }
        }

        private void DrawSaveDescription(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
            {
                description = string.Empty;
            }
        }

        private void DrawDescription0(float line)
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
        private void DrawDescription1(float line)
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
        private void DrawDescription2(float line)
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
        private void DrawDescription3(float line)
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
        private void DrawDescription4(float line)
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
        private void DrawDescription5(float line)
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
        private void DrawDescription6(float line)
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
        private void DrawDescription7(float line)
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
        private void DrawDescription8(float line)
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
        private void DrawDescription9(float line)
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


        #endregion

        #region GuiWindowOrXMissions

        private void GuiWindowOrXMissions(int OrXMissions)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            line++;
            if (!holoSpawned)
            {
                DrawHoloCacheName(line);
                line++;
                //DrawModule(line);
                //line++;
                DrawMissionType(line);
                line++;
                if (!geoCache)
                {
                    DrawRaceType(line);
                    line++;
                }
                DrawAddBlueprints(line);
                line++;
                DrawPassword(line);
                line++;
                DrawEditDescription(line);
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
                    DrawClearLastCoord(line);
                    line++;
                    DrawClearAllCoords(line);
                    line++;
                }

                line++;
                DrawSave(line);
                line++;
                DrawCancel(line);

            }


            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX HoloCache Creator", titleStyle);
        }
        private void DrawHoloCacheName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "HoloCache: ",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            HoloCacheName = GUI.TextField(fwdFieldRect, HoloCacheName);
        }
        private void DrawMissionType(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (geoCache)
            {
                missionType = "GEO-CACHE";

                if (GUI.Button(saveRect, missionType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        geoCache = false;
                    }
                }
            }
            else
            {
                missionType = "CHALLENGE";

                if (GUI.Button(saveRect, missionType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        geoCache = true;
                    }
                }
            }
        }
        private void DrawRaceType(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, challengeType, HighLogic.Skin.box))
            {
                if (windRacing && !Scuba)
                {
                    challengeType = "WIND RACING";

                    if (GUI.Button(saveRect, challengeType, HighLogic.Skin.button))
                    {
                        if (!locAdded)
                        {
                            windRacing = false;
                            Scuba = true;
                        }
                    }
                }
                else
                {
                    if (Scuba)
                    {
                        challengeType = "SCUBA CHALLENGE";

                        if (GUI.Button(saveRect, challengeType, HighLogic.Skin.button))
                        {
                            if (!locAdded)
                            {
                                windRacing = false;
                                Scuba = false;
                            }
                        }
                    }
                    else
                    {
                        challengeType = "ANYTHING GOES";

                        if (GUI.Button(saveRect, challengeType, HighLogic.Skin.button))
                        {
                            if (!locAdded)
                            {
                                windRacing = true;
                                Scuba = false;
                            }
                        }
                    }
                }
            }
        }
        private void DrawAddBlueprints(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!blueprintsAdded)
            {
                if (GUI.Button(saveRect, "ADD BLUEPRINTS", HighLogic.Skin.button))
                {
                    addingBluePrints = true;
                    blueprintsFile = "";
                    PlayOrXMission = false;
                    GuiEnabledOrXMissions = false;
                    craftBrowserOpen = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "BLUEPRINTS ADDED", HighLogic.Skin.button))
                {
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
            float textFieldWidth = 80;
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

        private void DrawDescription(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            description = GUI.TextField(fwdFieldRect, description);
            WrapText(description);
        }
        private void DrawEditDescription(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!editDescription)
            {
                if (GUI.Button(saveRect, "EDIT DESCRIPTION", HighLogic.Skin.button))
                {
                    editDescription = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "EDITING", HighLogic.Skin.box))
                {
                    editDescription = false;
                }
            }
        }

        private void DrawAddCoords(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "ADD STAGE", HighLogic.Skin.button))
            {
                if (!locAdded)
                {
                    locAdded = true;
                }

                if (windRacing)
                {
                    if (FlightGlobals.ActiveVessel.altitude >= 0 && FlightGlobals.ActiveVessel.atmDensity >= 0.007)
                    {
                        locCount += 1;
                        lastCoord = FlightGlobals.ActiveVessel.GetTransform().position;
                        lat = lastCoord.x;
                        lon = lastCoord.y;
                        alt = lastCoord.z;
                        windIntensity = OrXWeatherSim.instance.windIntensity;
                        windVariability = OrXWeatherSim.instance.windVariability;
                        variationIntensity = OrXWeatherSim.instance.variationIntensity;
                        heading = OrXWeatherSim.instance.heading;
                        teaseDelay = OrXWeatherSim.instance.teaseDelay;
                        // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                        CoordDatabase.Add(locCount + "," + lat + "," + lon + "," + alt + ","
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
                        if (FlightGlobals.ActiveVessel.altitude <= 1)
                        {
                            locCount += 1;
                            lastCoord = FlightGlobals.ActiveVessel.GetTransform().position;
                            lat = lastCoord.x;
                            lon = lastCoord.y;
                            alt = lastCoord.z;
                            windIntensity = OrXWeatherSim.instance.windIntensity;
                            windVariability = OrXWeatherSim.instance.windVariability;
                            variationIntensity = OrXWeatherSim.instance.variationIntensity;
                            heading = OrXWeatherSim.instance.heading;
                            teaseDelay = OrXWeatherSim.instance.teaseDelay;
                            // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                            CoordDatabase.Add(locCount + "," + lat + "," + lon + "," + alt + ","
                                + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                        }
                        else
                        {
                            ScreenMsg("Unable to add coordinate to Scuba Challenge if vessel is not Splashed");
                        }

                    }
                    else
                    {
                        locCount += 1;
                        lastCoord = FlightGlobals.ActiveVessel.GetTransform().position;
                        lat = lastCoord.x;
                        lon = lastCoord.y;
                        alt = lastCoord.z;
                        windIntensity = OrXWeatherSim.instance.windIntensity;
                        windVariability = OrXWeatherSim.instance.windVariability;
                        variationIntensity = OrXWeatherSim.instance.variationIntensity;
                        heading = OrXWeatherSim.instance.heading;
                        teaseDelay = OrXWeatherSim.instance.teaseDelay;
                        // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                        CoordDatabase.Add(locCount + "," + lat + "," + lon + "," + alt + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                    }
                }
            }
        }
        private void DrawClearLastCoord(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "DELETE LAST", HighLogic.Skin.button))
            {
                CoordDatabase.Remove(locCount + "," + lat + "," + lon + "," + alt + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                locCount -= 1;
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

        private void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (holoSpawned)
            {
                if (GUI.Button(saveRect, "FINISHED", HighLogic.Skin.button))
                {
                    SaveConfig();
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
                            if (holoCraftSelected)
                            {
                                SaveConfig();
                            }
                            else
                            {
                                ScreenMsg("Please add a craft file");
                            }
                        }
                        else
                        {
                            ScreenMsg("Please add a description");
                        }
                    }
                    else
                    {
                        ScreenMsg("Please enter a name for your Challenge");
                    }
                }
            }
        }
        private void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "CANCEL", HighLogic.Skin.button))
            {
                if (PlayOrXMission)
                {
                    CoordDatabase.Clear();
                    locCount = 0;
                    locAdded = false;

                    DisableGui();
                }
                else
                {
                    List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                    while (v.MoveNext())
                    {
                        if (v.Current == null) continue;
                        if (!v.Current.loaded || v.Current.packed) continue;
                        if (v.Current.id == id)
                        {
                            v.Current.DestroyVesselComponents();
                            v.Current.Die();
                        }
                    }
                    v.Dispose();

                    CoordDatabase.Clear();
                    locCount = 0;
                    locAdded = false;

                    DisableGui();
                }
            }
        }

        #endregion

        #endregion

        private string HoloCacheListToString()
        {
            string finalString = string.Empty;
            string aString = string.Empty;
            aString += FlightGlobals.currentMainBody.name;
            aString += ",";
            aString += HoloCacheName;
            aString += ",";
            aString += Password;
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
            bString += Password;
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

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }

        private void Dummy()
        {
        }
    }
}