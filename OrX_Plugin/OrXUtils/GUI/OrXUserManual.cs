using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.FlightEditorAndKSC, false)]
    public class OrXUserManual : MonoBehaviour
    {
        public static OrXUserManual instance;

        #region Variables

        float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public bool guiEnabled = false;
        float entryHeight = 20;
        float _contentWidth;
        float _windowHeight = 600;
        public static Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = XKCDColors.BoogerGreen }
        };
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
        static GUIStyle titleStyleMed = new GUIStyle(centerLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        static GUIStyle titleStyleMedLeft = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold
        };

        bool openSubmenu = false;
        bool scubaKerb = false;
        bool scubaMenu = false;
        bool scubaBallast = false;
        bool narcosis = false;
        bool bends = false;

        bool winds = false;
        bool windsMenu = false;
        bool windsControls = false;

        bool holokron = false;
        bool holokronMain = false;
        bool holokronMain2 = false;
        bool holokronMain3 = false;
        bool holokronMain4 = false;
        bool holokronMain5 = false;
        bool holokronMain6 = false;
        bool holokronMain7 = false;
        bool holokronMain8 = false;
        bool holokronMain9 = false;

        bool challengeTypes = false;
        bool cTypes1 = false;
        bool cTypes2 = false;
        bool cTypes3 = false;
        bool cTypes4 = false;
        bool cTypes5 = false;
        bool cTypes6 = false;
        bool cTypes7 = false;
        bool cTypes8 = false;

        bool pre = false;
        bool mrKleen = false;
        bool kontinuum = false;
        string titleLabel = "The Kurgan Manual";

        string _label1 = "";
        string _label2 = "";
        string _label3 = "";
        string _label4 = "";
        string _label5 = "";
        string _label6 = "";
        string _label7 = "";
        string _label8 = "";
        string _label9 = "";
        string _label0 = "";

        #endregion

        #region Core

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
            //if (HighLogic.LoadedScene == GameScenes.LOADING) return;
            if (!HighLogic.LoadedSceneIsEditor)
            {
                //if (PauseMenu.isOpen) return;
            }
            GUI.backgroundColor = XKCDColors.DarkGrey;
            GUI.contentColor = XKCDColors.DarkGrey;
            GUI.color = XKCDColors.DarkGrey;
            if (guiEnabled)
            {
                _windowRect = GUI.Window(902434275, _windowRect, OrXUserManualGUI, "");
            }
        }
        private void OrXUserManualGUI(int OrXManual)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;
            GUI.Label(new Rect(0, 0, WindowWidth, 20), titleLabel, titleStyleOrange);

            line++;

            if (!openSubmenu)
            {
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "The Kontinuum", OrXGUISkin.button))
                {
                    //openSubmenu = true;
                    //kontinuum = true;
                    //WindowWidth = 700;
                    //titleLabel = "About the Kontinuum";
                    OrXHoloKron.instance.OnScrnMsgUC("The Kontinuum is currently unavailable");
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "OrX Scuba Kerb", OrXGUISkin.button))
                {
                    openSubmenu = true;
                    scubaKerb = true;
                    WindowWidth = 700;
                    titleLabel = "OrX Scuba Kerb";
                    ScubaKerbMenu();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "OrX W[ind/S]", OrXGUISkin.button))
                {
                    openSubmenu = true;
                    winds = true;
                    WindowWidth = 700;
                    titleLabel = "OrX W[ind/S]";
                    WindsMenu();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "OrX HoloKron System", OrXGUISkin.button))
                {
                    openSubmenu = true;
                    holokron = true;
                    WindowWidth = 700;
                    titleLabel = "OrX HoloKron System";
                    HolokKronMain();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Challenge Types", OrXGUISkin.button))
                {
                    openSubmenu = true;
                    challengeTypes = true;
                    WindowWidth = 700;
                    titleLabel = "Challenge Types";
                    ChallengeTypesMain();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Physics Range", OrXGUISkin.button))
                {
                    openSubmenu = true;
                    pre = true;
                    WindowWidth = 700;
                    titleLabel = "Physics Range";
                    PreMain();
                }

                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "About Mr. Kleen", OrXGUISkin.button))
                {
                    openSubmenu = true;
                    mrKleen = true;
                    WindowWidth = 700;
                    titleLabel = "About Mr. Kleen";
                    MrKleenMain();
                }

                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Close Manual", OrXGUISkin.button))
                {
                    guiEnabled = false;
                }
            }
            else
            {
                if (kontinuum)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Connect to Kontinuum", OrXGUISkin.button))
                    {

                    }
                    line++;
                    line += 0.2f;
                }

                if (scubaKerb)
                {
                    if (!scubaMenu)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb Menu", OrXGUISkin.button))
                        {
                            ScubaKerbMenu();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb Menu", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!scubaBallast)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb Controls", OrXGUISkin.button))
                        {
                            ScubaKerbBallast();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb Controls", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyleMedLeft);

                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                }

                if (winds)
                {
                    if (!windsMenu)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Menu", OrXGUISkin.button))
                        {
                            WindsMenu();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Menu", OrXGUISkin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!windsControls)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Controls", OrXGUISkin.button))
                        {
                            WindsControls();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Controls", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                }

                if (holokron)
                {
                    if (!holokronMain)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "HoloKron Files", OrXGUISkin.button))
                        {
                            HolokKronMain();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "HoloKron Files", OrXGUISkin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!holokronMain2)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Sharing a HoloKron", OrXGUISkin.button))
                        {
                            HolokKronMain2();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Sharing a HoloKron", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!holokronMain3)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating a HoloKron", OrXGUISkin.button))
                        {
                            HolokKronMain3();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating a HoloKron", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!holokronMain4)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Adding a Challenge", OrXGUISkin.button))
                        {
                            HolokKronMain4();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Adding a Challenge", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!holokronMain5)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Passwords", OrXGUISkin.button))
                        {
                            HolokKronMain5();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Passwords", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!holokronMain6)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "HoloKron Target Info", OrXGUISkin.button))
                        {
                            HolokKronMain6();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "HoloKron Target Info", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!holokronMain7)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Opening HoloKrons", OrXGUISkin.button))
                        {
                            HolokKronMain7();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Opening HoloKrons", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!holokronMain8)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Blueprints and Vessels", OrXGUISkin.button))
                        {
                            HolokKronMain8();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Blueprints and Vessels", OrXGUISkin.box))
                        {

                        }
                    }


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    if (!holokronMain9)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Additional Information", OrXGUISkin.button))
                        {
                            HolokKronMain9();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Additional Information", OrXGUISkin.box))
                        {

                        }
                    }


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                }

                if (challengeTypes)
                {
                    if (!cTypes1)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Challenge Types", OrXGUISkin.button))
                        {
                            ChallengeTypesMain();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Challenge Types", OrXGUISkin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!cTypes8)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "ScoreBoard", OrXGUISkin.button))
                        {
                            ChallengeScores();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "ScoreBoard", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!cTypes2)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Extracting ScoreBoard", OrXGUISkin.button))
                        {
                            ChallengeExtract();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Extracting ScoreBoard", OrXGUISkin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!cTypes3)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Short Track", OrXGUISkin.button))
                        {
                            ChallengeShortTrack();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Short Track", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!cTypes4)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Dakar Racing", OrXGUISkin.button))
                        {
                            ChallengeDakar();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Dakar Racing", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!cTypes5)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S]", OrXGUISkin.button))
                        {
                            ChallengeWinds();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S]", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!cTypes6)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb", OrXGUISkin.button))
                        {
                            ChallengeScuba();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    if (!cTypes7)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "BD Armory", OrXGUISkin.button))
                        {
                            ChallengeBDAc();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "BD Armory", OrXGUISkin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                }

                if (pre)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Physics Range", OrXGUISkin.box))
                    {

                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                }

                if (mrKleen)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "The Magic Eraser", OrXGUISkin.box))
                    {

                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyleMedLeft);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyleMedLeft);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyleMedLeft);
                    line++;
                    line += 0.2f;
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Return to Previous Menu", OrXGUISkin.button))
                {
                    Reset();
                }

            }
            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        #endregion

        #region Kurgan Manual
        public void Reset()
        {
            openSubmenu = false;
            WindowWidth = 250;

            scubaKerb = false;
            scubaMenu = false;
            scubaBallast = false;
            narcosis = false;
            bends = false;

            winds = false;
            windsControls = false;
            windsMenu = false;

            holokron = false;
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = false;

            challengeTypes = false;
            cTypes1 = false;
            cTypes2 = false;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = false;

            pre = false;

            mrKleen = false;

            kontinuum = false;

            titleLabel = "The Kurgan Manual";

            _label1 = "";
            _label2 = "";
            _label3 = "";
            _label4 = "";
            _label5 = "";
            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);
        }

        private void ScubaKerbMenu()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            scubaMenu = true;
            scubaBallast = false;
            narcosis = false;
            bends = false;
            _label1 = "- The Scuba Kerb menu will automagically open while splashed";
            _label2 = "- The Oxygen slider displays your Kerbals' total Oxygen amount";
            _label3 = "- Oxygen will replenish if in an atmosphere that contains Oxygen";
            _label4 = "";
            _label5 = "- Kerbals who have been lost to the sea can be revived if you ";
            _label6 = "  can find a way to get them to the surface";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void ScubaKerbBallast()
        {
            scubaMenu = false;
            scubaBallast = true;
            narcosis = false;
            bends = false;

            _label1 = "- 'Q' to trim up, 'E' to trim down, 'Z' to surface and 'X' to dive";
            _label2 = "- Press the 'Tab' key to hold your current depth";
            _label3 = "- While diving pay attention to your Martini level ... the higher";
            _label4 = "  your Martini level the closer you are to being drunk";
            _label5 = "- Martini level slowly drops over time while holding depth";
            _label6 = "- If your Martini level reaches 6 then your Kerbal is drunk";
            _label7 = "- Your drunk Kerbal may be unable to recover if sinking too fast";
            _label8 = "- Beware the Bends ... could potentially cause your kerbal to go 'POP'";
            _label9 = "  as you rise to the surface ... pro tip, don't rise too quickly";
            _label0 = "";

        }

        private void WindsMenu()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            windsMenu = true;
            windsControls = false;
            _label1 = "- The W[ind/S] menu has various sliders and buttons";
            _label2 = "  the main ones are a button to turn the W[ind/S] on/off";
            _label3 = "  and a direction randomizer/weather simulation";
            _label4 = "";
            _label5 = "  PLEASE NOTE: W[ind/S] is highly experimental";
            _label6 = "  and is considered use at your own risk !!!";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void WindsControls()
        {
            windsMenu = false;
            windsControls = true;

            _label1 = "- Intensity controls how hard the wind blows";
            _label2 = "- Variability controls variations in direction";
            _label3 = "- Ability to manually enter W[ind/S] heading (0 - 360)";
            _label4 = "- Variation Intesity controls the intensity of variation";
            _label5 = "- Tease Timer controls how often things change";
            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }

        private void HolokKronMain()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            holokronMain = true;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = false;

            _label1 = "- Created HoloKrons are saved as .orx files";
            _label2 = "- Created .orx files can be found inside of the creators";
            _label3 = "  directory within GameData/OrX/Export/";
            _label4 = "- The Export directory will contain a folder for each";
            _label5 = "  creator name you have used which will contain a folder";
            _label6 = "  for every HoloKron that has been created using that name";
            _label7 = "- Place all shared files in GameData/OrX/Import/";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void HolokKronMain2()
        {
            holokronMain = false;
            holokronMain2 = true;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = false;

            _label1 = "- Shared .orx files are to be placed into GameData/OrX/Import/";
            _label2 = "- Click on the Creator List to show a list of HoloKron creators";
            _label3 = "- Select a Creator to show a list of their HoloKrons";
            _label4 = "- Select from the list to show the HoloKron details window";
            _label5 = "- If in the flight scene select 'SCAN FOR HOLOKRON' to ";
            _label6 = "  start OrX Kontinuum scanning for the selected HoloKron";
            _label7 = "- Click 'SHOW SCOREBOARD' to open the scoreboard menu ... ";
            _label8 = "  ... available only if the HoloKron is not a Geo-Cache";
            _label9 = "- Click 'SCAN FOR HOLOKRON' to open the HoloKron Target Info window";
            _label0 = "- Click 'CLOSE WINDOW' to return to the previous menu";



        }
        private void HolokKronMain3()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = true;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = false;

            _label1 = "- Select 'CREATE HOLOKRON' to open the HoloKron Creation menu";
            _label2 = "- Click on the HoloKron Type button to change the type";
            _label3 = "- Enter a name for your HoloKron";
            _label4 = "- Enter a Creator name";
            _label5 = "- Enter a password to lock the HoloKron from being added to";
            _label6 = "- Add a description for your HoloKron";
            _label7 = "- Click on the Add Blueprints check box to save blueprints to";
            _label8 = "  your HoloKron which will become available when it is opened";
            _label9 = "- Click on the Save Local Vessel check box to save in range vessels";
            _label0 = "- Click on the 'SAVE HOLOKRON' button to save Geo-Cache HoloKron's";

        }
        private void HolokKronMain4()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = true;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = false;

            _label1 = "- Click on the 'START ADD COORDS' to start adding coordinates to your";
            _label2 = "  challenge ... Focus will switch to a spawned HoloKron which can be";
            _label3 = "  moved around with the WASD keys";
            _label4 = "- Use the throttle up/down keys to adjust height if needed";
            _label5 = "- Click on 'Add Location' to spawn a stage gate at your current location";
            _label6 = "- Use the throttle up/down, translate and rotate keys (Q, E, I, J, K, L)";
            _label7 = "  to orient the stage gate to your liking";
            _label9 = "- Click on 'PLACE GATE' to save to the challenge coordinates list";
            _label0 = "- Click on 'Add Next Stage' to add another stage to the challenge";
            _label8 = "- Click 'SAVE AND EXIT' to save your HoloKron";

        }
        private void HolokKronMain5()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = true;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = false;

            _label1 = "- Changing the password from the default 'OrX' provides a creator the";
            _label2 = "  ability to lock their HoloKron from being added to";
            _label3 = "   ";
            _label4 = "- The password is also used for exporting the master scoreboard for";
            _label5 = "  the challenge ... the purpose of this is to allow for a creator to";
            _label6 = "  maintain the master scoreboard file without having other scoreboard";
            _label7 = "  files out in the wild";
            _label9 = "  ";
            _label0 = "- PLEASE NOTE: If a creator does not change the default password then";
            _label8 = "  their HoloKron is in an 'Unlocked' status";

        }
        private void HolokKronMain6()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = true;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = false;

            _label1 = "- The HoloKron Target Info window tells you the distance to the targeted";
            _label2 = "  HoloKron as well as its altitude";
            _label3 = "- Click the 'Stop HoloKron Scan' button to stop scanning for the currently";
            _label4 = "  selected HoloKron and return to the Main Menu";
            _label5 = "";
            _label6 = "- A green targeting reticle will appear on the screen indicating";
            _label7 = "  where the HoloKron is located";
            _label9 = "- The menu will automatically close when you are within 25 meters";
            _label0 = "";
            _label8 = "- Click on the OrX application button to show or hide the window";

        }
        private void HolokKronMain7()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = true;
            holokronMain8 = false;
            holokronMain9 = false;

            _label1 = "- When you are within 25 meters of a HoloKron, right click on the rotating";
            _label2 = "  cube and select 'OPEN HOLOKRON' in the part action window and the ";
            _label3 = "  HoloKron details window will open";
            _label4 = "- Enter a challenger name for your scoreboard entry";
            _label5 = "- Click 'SPAWN CHALLENGE' to spawn any vessels and stage gates contained";
            _label6 = "  in the HoloKron";
            _label7 = "- Click 'SHOW SCOREBOARD' to open the scoreboard menu if there ";
            _label9 = "  is a challenge";
            _label0 = "- Click on 'CLOSE WINDOW' to close the window";
            _label8 = "- Click on 'START CHALLENGE' to start the challenge";

        }
        private void HolokKronMain8()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = true;
            holokronMain9 = false;

            _label1 = "- Blueprints that are added to a HoloKron will be saved as a .craft file";
            _label2 = "  after a HoloKron has been opened";
            _label3 = "  PLEASE NOTE: If the HoloKron contains a challenge then you must complete";
            _label4 = "  the challenge for the blueprints to be made available";
            _label5 = "- Any local vessels that have been saved into a HoloKron will not be available";
            _label6 = "  as .craft files and are only available in the Flight Scene after they ";
            _label7 = "  have been spawned";
            _label9 = "";
            _label0 = "";
            _label8 = "";

        }
        private void HolokKronMain9()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;
            holokronMain7 = false;
            holokronMain8 = false;
            holokronMain9 = true;

            _label1 = "- If a player does not have the same mods installed as the creator of the";
            _label2 = "  HoloKron a warning menu will appear when you attempt to spawn a challenge";
            _label3 = "  A list of the missing mods will be shown and you will have the option to";
            _label4 = "  attempt the spawn or return to the previous menu";
            _label5 = "";
            _label6 = "";
            _label7 = "";
            _label9 = "";
            _label0 = "";
            _label8 = "";

        }

        private void ChallengeScores()
        {
            cTypes1 = false;
            cTypes2 = false;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = true;

            _label1 = "- OrX Kontinuum contains a scoreboard for each challenge with 10 entries";
            _label2 = "- Click on an entry name to open a window with details about that entry";
            _label3 = "- Click on 'Export Challengers Entry' while viewing entries in the";
            _label4 = "  scoreboard to save a .scores file to the OrX/Export/ directory for";
            _label5 = "  sharing with other players";
            _label6 = "- Place shared .scores files into the OrX/Import/ directory";
            _label7 = "- Click on Import Scores to view a detailed breakdown of each stage";
            _label8 = "  in the scoreboard entry";
            _label9 = "- Click on 'Dicard and Check' for More Scores or 'Confirm Scores Import'";
            _label0 = "  to continue";
        }
        private void ChallengeTypesMain()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            cTypes1 = true;
            cTypes2 = false;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = false;

            _label1 = "- Outlaw Racing contains Short Track and Dakar Racing types";
            _label2 = "- Scuba Kerb covers underwater challenges";
            _label3 = "- W[ind/S] covers W[ind/S] related challenges";
            _label4 = "- BD Armory covers BD Armory challenges";
            _label5 = "";

            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void ChallengeExtract()
        {
            cTypes1 = false;
            cTypes2 = true;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = false;

            _label1 = "- Click on Extract Scoreboard to open the Scoreboard password menu";
            _label2 = "- Enter the password for the HoloKron you are viewing";
            _label3 = "- Click on Extract Scoreboard and a .scoreboard file will be saved";
            _label4 = "  to the OrX/Export/ directory for sharing";
            _label5 = "- Place shared .scoreboard files in the OrX/Import/ directory and";
            _label6 = "  click on Import Scores while viewing the matching HoloKron";
            _label7 = "  to have the scoreboard automagically updated with the new scoreboard";
            _label9 = "- PLEASE NOTE: If the HoloKron creator has changed the default password";
            _label0 = "  then the HoloKron is in a 'Locked' status and the scoreboard can not";
            _label8 = "  be extracted unless the proper password is entered";

        }
        private void ChallengeShortTrack()
        {
            cTypes1 = false;
            cTypes2 = false;
            cTypes3 = true;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = false;

            _label1 = "- Short Track Racing is a multi stage race";
            _label2 = "- When you start the challenge your vessel will be automagically ";
            _label3 = "  placed at the starting line facing the first stage gate as well";
            _label4 = "  as having the brakes engaged ... Follow on screen directions";
            _label5 = "- When you pass through a stage gate your time will be recorded";
            _label6 = "- Green indicator will show where the next stage gate is";
            _label7 = "- Your time will be saved to scoreboard and any blueprints included";
            _label8 = "   will be saved once the challenge has been completed";
            _label9 = "- Short Track racing has a maximum radius of 4 km from the location";
            _label0 = "  the HoloKron was created at";

        }
        private void ChallengeDakar()
        {
            cTypes1 = false;
            cTypes2 = false;
            cTypes3 = false;
            cTypes4 = true;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = false;

            _label1 = "- Dakar Racing is a multi stage race occurring over a long distance";
            _label2 = "";
            _label3 = "- Time will be saved to scoreboard as well as a .scores";
            _label4 = "file will be saved to the OrX/Export/ directory for sharing";
            _label5 = "";
            _label6 = "CURRENTLY UNAVAILABLE";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void ChallengeWinds()
        {
            cTypes1 = false;
            cTypes2 = false;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = true;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = false;

            _label1 = "- W[ind/S] covers W[ind/S] related challenges";
            _label2 = "";
            _label3 = "  CURRENTLY UNAVAILABLE";
            _label4 = "";
            _label5 = "";
            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void ChallengeScuba()
        {
            cTypes1 = false;
            cTypes2 = false;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = true;
            cTypes7 = false;
            cTypes8 = false;

            _label1 = "- Scuba Kerb covers Underwater challenges";
            _label2 = "";
            _label3 = "  CURRENTLY UNAVAILABLE";
            _label4 = "";
            _label5 = "";
            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void ChallengeBDAc()
        {
            cTypes1 = false;
            cTypes2 = false;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = true;
            cTypes8 = false;

            _label1 = "- BD Armory covers BD Armory challenges";
            _label2 = "";
            _label3 = "  CURRENTLY UNAVAILABLE";
            _label4 = "";
            _label5 = "";
            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }

        private void MrKleenMain()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            _label1 = "- Mr Kleen's Magic Eraser gives you the ability to clean";
            _label2 = "  up your mess before, during or after starting a challenge";
            _label3 = "- The Magic Eraser can be found in the Settings menu";
            _label4 = "- Every vessel except for the active vessel is a target";
            _label5 = "  for scrubbing from the game if it is within an 8km range";
            _label6 = "- You may have to use the Magic Eraser a couple times";
            _label7 = "  to erase all vessels and debris lying around";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }

        private void PreMain()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            _label1 = "- OrX Kontinuum manipulates the stock game physics ranges";
            _label2 = "  and requires you to turn off Physics Range Extender if you";
            _label3 = "  have that mod installed";
            _label4 = "- If PRE is installed and enabled OrX Kontinuum will reset";
            _label5 = "  and not allow you to start any challenges as well as";
            _label6 = "  not allow you to create a HoloKron";
            _label7 = "- If PRE is installed OrX Kontinuum will adopt the ranges";
            _label8 = "  that have been set in PRE";
            _label9 = "- OrX Kontinuum has a base landed load range of 10km and ";
            _label0 = "  a flying load range of 40km";

        }

        #endregion

    }
}