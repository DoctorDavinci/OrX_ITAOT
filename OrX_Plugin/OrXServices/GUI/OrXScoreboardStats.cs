using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.FlightEditorAndKSC, false)]
    public class OrXScoreboardStats : MonoBehaviour
    {
        private const float WindowWidth = 740;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXScoreboardStats instance;
        public bool GuiEnabledStats = false;
        public static bool TBBadded;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 600;
        public static Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        public List<string> scoreboardStats;
        string scoreName = "";
        double maxSpeed = 0;
        string totalAirTime = "";
        double maxDepth = 0;
        string totalTime = "";
        bool cheats = false;
        public float _airTimeMod = 0.15f;
        string creatorName = "";
        string HoloKronName = "";
        int hkCount = 0;
        bool bda = false;
        List<string> _modList;
        bool _showModList = false;
        public Vector2 scrollPosition = Vector2.zero;

        string _mia = "";
        string _playerVesselCount = "";
        string _opForCount = "";
        string _waveCount = "";

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
        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        };
        static GUIStyle titleStyle = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        static GUIStyle titleStyleMed = new GUIStyle(centerLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic,
            normal = { textColor = XKCDColors.BoogerGreen }
        };
        static GUIStyle titleStyleMedYellow = new GUIStyle(centerLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic,
            normal = { textColor = XKCDColors.BoogerGreen }
        };
        static GUIStyle titleStyleGreen = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic,
            normal = { textColor = Color.green }
        };
        static GUIStyle titleStyleWarning = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic,
            normal = { textColor = Color.yellow }
        };

        public GUIStyle _switchCheatStyle(string _text)
        {
            if (_text != "False")
            {
                return titleStyleWarning;
            }
            else
            {
                return titleStyleGreen;
            }
        }
        public GUIStyle _switchAirTimeStyle(string _airTime, string _stageTime)
        {
            float _air = float.Parse(_airTime);
            float time = float.Parse(_stageTime);

            if (_air >= time * _airTimeMod)
            {
                return titleStyleWarning;
            }
            else
            {
                return titleStyle;
            }
        }


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
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);
        }
        private void OnGUI()
        {
            if (GuiEnabledStats)
            {
                GUI.backgroundColor = XKCDColors.DarkGrey;
                GUI.contentColor = XKCDColors.DarkGrey;
                GUI.color = XKCDColors.DarkGrey;

                _windowRect = GUI.Window(826564275, _windowRect, OrXChallengeScoreboardStats, "");
            }
        }

        public void ExportScoresFile()
        {
            int stageCount = 0;
            ConfigNode tempChallengerEntry = new ConfigNode();
            tempChallengerEntry.AddValue("name", scoreName);
            double totalTimeChallenger = 0;
            double totalAirTimeChallenger = 0;
            double maxDepthChallenger = 0;
            double maxSpeedChallenger = 0;

            List<string>.Enumerator st = scoreboardStats.GetEnumerator();
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

                }
            }
            st.Dispose();

            tempChallengerEntry.AddValue("totalTime", totalTimeChallenger);
            tempChallengerEntry.AddValue("totalAirTime", totalAirTimeChallenger);
            tempChallengerEntry.AddValue("maxDepth", maxDepthChallenger);
            tempChallengerEntry.AddValue("maxSpeed", maxSpeedChallenger);


            ConfigNode scores = new ConfigNode();
            scores.AddValue("name", HoloKronName);
            scores.AddValue("count", hkCount);
            scores.AddValue("creator", creatorName);
            scores.AddValue("challengersName", scoreName);
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

            scores.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "-" + hkCount + "-" + scoreName + "-" + totalTimeChallenger + ".scores");
            OrXLog.instance.DebugLog("[OrX Mission Scoreboard] === SCORES FILE EXPORTED ===");
            GuiEnabledStats = false;
            OrXHoloKron.instance.OpenScoreboardMenu();
        }
        public void OpenStatsWindow(bool _bdac, string _holoName, string _creator, string _name, string _totalTime, string _totalAirTime, int _hkCount, double _maxSpeed, double _maxDepth, List<string> _data, string _mods)
        {
            _modList = new List<string>();
            string[] data = _mods.Split(new char[] { ',' });

            if (data[0] != null && data[0].Length > 0 && data[0] != "null")
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != null && data[i].Length > 0)
                    {
                        _modList.Add(data[i]);
                    }
                }
            }

            bda = _bdac;
            HoloKronName = _holoName;
            creatorName = _creator;
            hkCount = _hkCount;
            scoreboardStats = new List<string>();
            maxSpeed = Math.Round(_maxSpeed, 1);
            maxDepth = Math.Round(_maxDepth, 1);
            totalAirTime = _totalAirTime;
            totalTime = _totalTime;
            scoreName = _name;
            scoreboardStats = _data;
            GuiEnabledStats = true;
        }
        private void OrXChallengeScoreboardStats(int Scoreboard)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            if (!_showModList)
            {
                _contentWidth = WindowWidth - 2 * LeftIndent;

                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Detailed results for " + scoreName + "'s scoreboard entry", titleStyleOrange);

                if (GUI.Button(new Rect(475, ContentTop + line * entryHeight, 235, 20), "Enabled Cheats", OrXGUISkin.box))
                {
                    GuiEnabledStats = false;
                    OrXHoloKron.instance.OrXHCGUIEnabled = true;
                    OrXHoloKron.instance.GetNextScoresFile(false, scoreboardStats);
                }

                line++;

                GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, 20), "Stage", titleStyleMedYellow);
                GUI.Label(new Rect(65, ContentTop + line * entryHeight, 100, 20), "Time", titleStyleMedYellow);
                if (bda)
                {
                    GUI.Label(new Rect(165, ContentTop + line * entryHeight, 100, 20), "Salt", titleStyleMedYellow);
                    GUI.Label(new Rect(265, ContentTop + line * entryHeight, 100, 20), "Waves", titleStyleMedYellow);
                    GUI.Label(new Rect(365, ContentTop + line * entryHeight, 100, 20), "Kills", titleStyleMedYellow);
                }
                else
                {
                    GUI.Label(new Rect(165, ContentTop + line * entryHeight, 100, 20), "Air Time", titleStyleMedYellow);
                    GUI.Label(new Rect(265, ContentTop + line * entryHeight, 100, 20), "Top Speed", titleStyleMedYellow);
                    GUI.Label(new Rect(365, ContentTop + line * entryHeight, 100, 20), "Max Depth", titleStyleMedYellow);
                }

                GUI.Label(new Rect(465, ContentTop + line * entryHeight, 50, 20), "NCD", titleStyleMedYellow);
                GUI.Label(new Rect(520, ContentTop + line * entryHeight, 50, 20), "UJ", titleStyleMedYellow);
                GUI.Label(new Rect(575, ContentTop + line * entryHeight, 50, 20), "IMT", titleStyleMedYellow);
                GUI.Label(new Rect(630, ContentTop + line * entryHeight, 50, 20), "IP", titleStyleMedYellow);
                GUI.Label(new Rect(685, ContentTop + line * entryHeight, 50, 20), "IEC", titleStyleMedYellow);



                GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, 20), "_______", titleStyleMed);
                GUI.Label(new Rect(70, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                GUI.Label(new Rect(170, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                GUI.Label(new Rect(270, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                GUI.Label(new Rect(370, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                GUI.Label(new Rect(465, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMedYellow);
                GUI.Label(new Rect(520, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMedYellow);
                GUI.Label(new Rect(575, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMedYellow);
                GUI.Label(new Rect(630, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMedYellow);
                GUI.Label(new Rect(685, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMedYellow);


                line++;
                List<string>.Enumerator scoreStats = scoreboardStats.GetEnumerator();
                while (scoreStats.MoveNext())
                {
                    if (scoreStats.Current != null)
                    {
                        try
                        {
                            string[] data = scoreStats.Current.Split(new char[] { ',' });
                            GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, 20), data[0], titleStyle);
                            GUI.Label(new Rect(65, ContentTop + line * entryHeight, 100, 20), OrXUtilities.instance.TimeSet(float.Parse(data[4])), titleStyle);
                            if (bda)
                            {
                                totalAirTime = data[3];
                                _playerVesselCount = data[11];
                                _mia = data[10];
                                _opForCount = data[12];
                                GUI.Label(new Rect(165, ContentTop + line * entryHeight, 100, 20), data[1], titleStyle);
                                GUI.Label(new Rect(265, ContentTop + line * entryHeight, 100, 20), data[2], titleStyle);
                                GUI.Label(new Rect(365, ContentTop + line * entryHeight, 100, 20), data[3], titleStyle);
                            }
                            else
                            {
                                GUI.Label(new Rect(165, ContentTop + line * entryHeight, 100, 20), OrXUtilities.instance.TimeSet(float.Parse(data[3])), _switchAirTimeStyle(data[3], data[4]));
                                GUI.Label(new Rect(265, ContentTop + line * entryHeight, 100, 20), Math.Round(float.Parse(data[1]), 1) + " m/s", titleStyle);
                                GUI.Label(new Rect(365, ContentTop + line * entryHeight, 100, 20), Math.Round(float.Parse(data[2]), 1).ToString(), titleStyle);
                            }

                            GUI.Label(new Rect(465, ContentTop + line * entryHeight, 50, 20), data[5], _switchCheatStyle(data[5]));
                            GUI.Label(new Rect(520, ContentTop + line * entryHeight, 50, 20), data[6], _switchCheatStyle(data[6]));
                            GUI.Label(new Rect(575, ContentTop + line * entryHeight, 50, 20), data[7], _switchCheatStyle(data[7]));
                            GUI.Label(new Rect(630, ContentTop + line * entryHeight, 50, 20), data[8], _switchCheatStyle(data[8]));
                            GUI.Label(new Rect(685, ContentTop + line * entryHeight, 50, 20), data[9], _switchCheatStyle(data[9]));
                        }
                        catch
                        {

                        }

                        line++;
                    }
                }
                scoreStats.Dispose();
                GUI.Label(new Rect(70, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                GUI.Label(new Rect(170, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                GUI.Label(new Rect(270, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                GUI.Label(new Rect(370, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
                if (OrXHoloKron.instance.bdaChallenge)
                {
                    line++;
                    GUI.Label(new Rect(65, ContentTop + line * entryHeight, 130, 20), "Total Player Craft:", titleStyleOrange);
                    GUI.Label(new Rect(180, ContentTop + line * entryHeight, 50, 20), _playerVesselCount, titleStyleMedYellow);
                    GUI.Label(new Rect(385, ContentTop + line * entryHeight, 130, 20), "Total Enemy Craft:", titleStyleOrange);
                    GUI.Label(new Rect(515, ContentTop + line * entryHeight, 50, 20), _opForCount, titleStyleMedYellow);
                    line++;
                    GUI.Label(new Rect(65, ContentTop + line * entryHeight, 50, 20), "MIA:", titleStyleOrange);
                    GUI.Label(new Rect(180, ContentTop + line * entryHeight, 50, 20), _mia, titleStyleMedYellow);
                    GUI.Label(new Rect(385, ContentTop + line * entryHeight, 100, 20), "MIA:", titleStyleOrange);
                    GUI.Label(new Rect(515, ContentTop + line * entryHeight, 50, 20), totalAirTime, titleStyleMedYellow);
                }
                else
                {
                    line++;
                    if (GUI.Button(new Rect(70, ContentTop + line * entryHeight, 90, 20), totalTime, OrXGUISkin.box)) { }
                    if (GUI.Button(new Rect(170, ContentTop + line * entryHeight, 90, 20), totalAirTime, OrXGUISkin.box)) { }
                    if (GUI.Button(new Rect(270, ContentTop + line * entryHeight, 90, 20), maxSpeed.ToString(), OrXGUISkin.box)) { }
                    if (GUI.Button(new Rect(370, ContentTop + line * entryHeight, 90, 20), maxDepth.ToString(), OrXGUISkin.box)) { }
                }
                line++;
                line++;
                if (GUI.Button(new Rect((WindowWidth / 2) + 15, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Show " + scoreName + "'s Mod list", OrXGUISkin.button))
                {
                    _showModList = true;
                }
                line++;
                line += 0.2f;

                if (OrXHoloKron.instance._importingScores)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Discard and Check for More Scores", OrXGUISkin.button))
                    {
                        GuiEnabledStats = false;
                        OrXHoloKron.instance.OrXHCGUIEnabled = true;
                        OrXHoloKron.instance.GetNextScoresFile(false, scoreboardStats);
                    }

                    if (GUI.Button(new Rect((WindowWidth / 2) + 15, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Confirm Scores Import", OrXGUISkin.button))
                    {
                        GuiEnabledStats = false;
                        OrXHoloKron.instance.OrXHCGUIEnabled = true;
                        OrXHoloKron.instance.GetNextScoresFile(true, scoreboardStats);
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Return To Previous Menu", OrXGUISkin.button))
                    {
                        GuiEnabledStats = false;
                        OrXHoloKron.instance.OpenScoreboardMenu();
                    }

                    if (GUI.Button(new Rect((WindowWidth / 2) + 15, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Export " + scoreName + "'s entry", OrXGUISkin.button))
                    {
                        ExportScoresFile();
                    }
                }
            }
            else
            {
                _contentWidth = WindowWidth - 2 * LeftIndent;
                int scrollIndex = 0;
                GUI.Label(new Rect(0, 0, WindowWidth, 20), scoreName + "'s installed Mods", titleStyleOrange);
                line++;
                scrollPosition = GUI.BeginScrollView(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 200), scrollPosition, new Rect(15, 0, WindowWidth - 30, _modList.Count * 20));
                List<string>.Enumerator modList = _modList.GetEnumerator();
                while (modList.MoveNext())
                {
                    if (modList.Current != null)
                    {
                        GUI.Label(new Rect(10, scrollIndex * 20, WindowWidth - 40, 20), modList.Current, titleStyleMedYellow);
                        scrollIndex += 1;
                    }
                }
                modList.Dispose();
                GUI.EndScrollView();
                if (scrollIndex >= 10)
                {
                    line += 10;
                }
                else
                {
                    line += scrollIndex;
                }

                line += 0.5f;

                line++;

                if (GUI.Button(new Rect((WindowWidth / 2) + 15, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Return to previous menu", OrXGUISkin.button))
                {
                    _showModList = false;
                }
            }

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
    }
}