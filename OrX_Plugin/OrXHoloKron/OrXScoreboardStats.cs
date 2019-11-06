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
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 600;
        private Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;

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
                _windowRect = GUI.Window(826564275, _windowRect, OrXChallengeScoreboardStats, "");
            }
        }

        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        };
        static GUIStyle titleStyle = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter
        };
        static GUIStyle titleStyleMed = new GUIStyle(centerLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.BoldAndItalic
        };

        public List<string> scoreboardStats;
        string scoreName = "";
        double maxSpeed = 0;
        string totalAirTime = "";
        double maxDepth = 0;
        string totalTime = "";
        bool cheats = false;

        public void OpenStatsWindow(string _name, string _totalTime, string _totalAirTime, double _maxSpeed, double _maxDepth, List<string> _data)
        {
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
            _contentWidth = WindowWidth - 2 * LeftIndent;

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Detailed results for " + scoreName + "'s scoreboard entry for " + OrXHoloKron.instance.HoloKronName, titleStyle);
            line++;

            GUI.Label(new Rect(475, ContentTop + line * entryHeight, 235, 20), "Enabled Cheats", titleStyleMed);
            line++;

            GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, 20), "Stage", titleStyleMed);
            GUI.Label(new Rect(65, ContentTop + line * entryHeight, 100, 20), "Time", titleStyleMed);
            GUI.Label(new Rect(165, ContentTop + line * entryHeight, 100, 20), "Air Time", titleStyleMed);
            GUI.Label(new Rect(265, ContentTop + line * entryHeight, 100, 20), "Top Speed", titleStyleMed);
            GUI.Label(new Rect(365, ContentTop + line * entryHeight, 100, 20), "Max Depth", titleStyleMed);

            GUI.Label(new Rect(465, ContentTop + line * entryHeight, 50, 20), "NCD", titleStyleMed);
            GUI.Label(new Rect(520, ContentTop + line * entryHeight, 50, 20), "UJ", titleStyleMed);
            GUI.Label(new Rect(575, ContentTop + line * entryHeight, 50, 20), "IMT", titleStyleMed);
            GUI.Label(new Rect(630, ContentTop + line * entryHeight, 50, 20), "IP", titleStyleMed);
            GUI.Label(new Rect(685, ContentTop + line * entryHeight, 50, 20), "IEC", titleStyleMed);



            GUI.Label(new Rect(10, ContentTop + line * entryHeight, 60, 20), "_______", titleStyleMed);
            GUI.Label(new Rect(70, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
            GUI.Label(new Rect(170, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
            GUI.Label(new Rect(270, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
            GUI.Label(new Rect(370, ContentTop + line * entryHeight, 90, 20), "____________", titleStyleMed);
            GUI.Label(new Rect(465, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMed);
            GUI.Label(new Rect(520, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMed);
            GUI.Label(new Rect(575, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMed);
            GUI.Label(new Rect(630, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMed);
            GUI.Label(new Rect(685, ContentTop + line * entryHeight, 50, 20), "_____", titleStyleMed);


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
                        GUI.Label(new Rect(65, ContentTop + line * entryHeight, 100, 20), OrXHoloKron.instance.TimeSet(float.Parse(data[4])), titleStyle);
                        GUI.Label(new Rect(165, ContentTop + line * entryHeight, 100, 20), OrXHoloKron.instance.TimeSet(float.Parse(data[3])), titleStyle);
                        GUI.Label(new Rect(265, ContentTop + line * entryHeight, 100, 20), Math.Round(float.Parse(data[1]), 1) + " m/s", titleStyle);
                        GUI.Label(new Rect(365, ContentTop + line * entryHeight, 100, 20), Math.Round(float.Parse(data[2]), 1) + "", titleStyle);

                        GUI.Label(new Rect(465, ContentTop + line * entryHeight, 50, 20), data[5], titleStyle);
                        GUI.Label(new Rect(520, ContentTop + line * entryHeight, 50, 20), data[6], titleStyle);
                        GUI.Label(new Rect(575, ContentTop + line * entryHeight, 50, 20), data[7], titleStyle);
                        GUI.Label(new Rect(630, ContentTop + line * entryHeight, 50, 20), data[8], titleStyle);
                        GUI.Label(new Rect(685, ContentTop + line * entryHeight, 50, 20), data[9], titleStyle);
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
            line++;
            if (GUI.Button(new Rect(70, ContentTop + line * entryHeight, 90, 20), totalTime, HighLogic.Skin.box)){}
            if (GUI.Button(new Rect(170, ContentTop + line * entryHeight, 90, 20), totalAirTime, HighLogic.Skin.box)) { }
            if (GUI.Button(new Rect(270, ContentTop + line * entryHeight, 90, 20), maxSpeed.ToString(), HighLogic.Skin.box)) { }
            if (GUI.Button(new Rect(370, ContentTop + line * entryHeight, 90, 20), maxDepth.ToString(), HighLogic.Skin.box)) { }
            line++;
            line++;

            if (OrXHoloKron.instance._importingScores)
            {
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Discard and Check for More Scores", HighLogic.Skin.button))
                {
                    GuiEnabledStats = false;
                    OrXHoloKron.instance.OrXHCGUIEnabled = true;
                    OrXHoloKron.instance.GetNextScoresFile(false, scoreboardStats);
                }

                if (GUI.Button(new Rect((WindowWidth / 2) + 15, ContentTop + (line * entryHeight), (WindowWidth / 2) - 25, 20), "Confirm Scores Import", HighLogic.Skin.button))
                {
                    GuiEnabledStats = false;
                    OrXHoloKron.instance.OrXHCGUIEnabled = true;
                    OrXHoloKron.instance.GetNextScoresFile(true, scoreboardStats);
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Return To Previous Menu", HighLogic.Skin.button))
                {
                    GuiEnabledStats = false;
                    OrXHoloKron.instance.OpenScoreboardMenu();
                }
            }

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
    }
}