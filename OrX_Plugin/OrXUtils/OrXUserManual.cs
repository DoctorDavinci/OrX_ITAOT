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
        float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static OrXUserManual instance;
        public bool guiEnabled = false;
        float entryHeight = 20;
        float _contentWidth;
        float _windowHeight = 600;
        private Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.UpperCenter,
            normal = { textColor = Color.white }
        };
        static GUIStyle titleStyle = new GUIStyle(centerLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.UpperLeft
        };
        static GUIStyle titleStyleMed = new GUIStyle(centerLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
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

        string titleLabel = "OrX User Manual";

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

            if (guiEnabled)
            {
                _windowRect = GUI.Window(902434275, _windowRect, OrXUserManualGUI, "");
            }
        }

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

            titleLabel = "OrX User Manual";

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
            _label1 = "- The Scuba Kerb menu will automatically open while splashed";
            _label2 = "";
            _label3 = "- The Oxygen slider displays your Kerbals' total Oxygen amount";
            _label4 = "";
            _label5 = "- Oxygen will replenish if in an atmosphere that contains Oxygen";
            _label6 = "";
            _label7 = "- Kerbals who have been lost to the sea can be revived if you ";
            _label8 = "can find a way to get them to the surface";
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
            _label2 = "";
            _label3 = "- You can also hold your current depth by pressing the 'Tab' key";
            _label4 = "";
            _label5 = "- Oxygen will replenish if in an atmosphere that contains Oxygen";
            _label6 = "";
            _label7 = "- While diving pay attention to your Martini level ... ";
            _label8 = "the higher the Martini level the closer you are to being drunk";
            _label9 = "";
            _label0 = "";

        }
        private void ScubaKerbNarcosis()
        {
            scubaMenu = false;
            scubaBallast = false;
            narcosis = true;
            bends = false;

            _label1 = "- Martini level slowly drops over time while holding depth";
            _label2 = "";
            _label3 = "- If your Martini level reaches 6 then your Kerbal is drunk";
            _label4 = "and it will automatically attempt to hold its depth";
            _label5 = "";
            _label6 = "- Your drunk Kerbal may be unable to recover if sinking too fast";
            _label7 = "and may dissapear into the depths in search of the Kraken";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void ScubaKerbBends()
        {
            scubaMenu = false;
            scubaBallast = false;
            narcosis = false;
            bends = true;

            _label1 = "- Beware the Bends ... You will experience decompression as you ";
            _label2 = "rise to the surface and gasses that have built up inside your ";
            _label3 = "Kerbal due to the pressure will be released as you ascend which ";
            _label4 = "could potentially cause your kerbal to go 'POP'";
            _label5 = "";
            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }

        private void WindsMenu()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            windsMenu = true;
            windsControls = false;
            _label1 = "- The W[ind/S] menu has various sliders and buttons";
            _label2 = "the main ones are a button to turn the W[ind/S] on/off";
            _label3 = "and a direction randomizer/weather simulation";
            _label4 = "";
            _label5 = "PLEASE NOTE: W[ind/S] is highly experimental";
            _label6 = "and is considered use at your own risk !!!";
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
            _label2 = "";
            _label3 = "- Variability controls variations in direction";
            _label4 = "";
            _label5 = "- Ability to manually enter W[ind/S] heading (0 - 360)";
            _label6 = "";
            _label7 = "- Variation Intesity controls the intensity of variation";
            _label8 = "";
            _label9 = "- Tease Timer controls how often things change";
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

            _label1 = "- Created HoloKrons are saved as .orx files";
            _label2 = "";
            _label3 = "- Created .orx files are found within GameData/OrX/Export/";
            _label4 = "";
            _label5 = "- The Export directory will contain a folder for each";
            _label6 = "creator name you have used and each creator folder will ";
            _label7 = "contain a folder for every HoloKron that has been created";
            _label8 = "using that creator name";
            _label9 = "";
            _label0 = "- Place all shared files in GameData/OrX/Import/";

        }
        private void HolokKronMain2()
        {
            holokronMain = false;
            holokronMain2 = true;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;

            _label1 = "- Shared .orx files are to be placed into GameData/OrX/Import/";
            _label2 = "";
            _label3 = "- Click on Creator List to show a list of HoloKron creators";
            _label4 = "";
            _label5 = "- Select a creator to show a list of their HoloKrons";
            _label6 = "";
            _label7 = "- Select from the list to show details about a HoloKron";
            _label8 = "";
            _label9 = "- While in the flight scene select 'SCAN FOR HOLOKRON' to start";
            _label0 = "";

        }
        private void HolokKronMain3()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = true;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = false;

            _label1 = "- Select Create HoloKron to open the HoloKron Creation menu";
            _label2 = "";
            _label3 = "- Click on the HoloKron Type button to change the type";
            _label4 = "";
            _label5 = "- Enter a Name, Creator and Password to lock the HoloKron";
            _label6 = "";
            _label7 = "- Add a description for your HoloKron";
            _label8 = "press 'TAB' to jump to the new text entry field";
            _label9 = "";
            _label0 = "";

        }
        private void HolokKronMain4()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = true;
            holokronMain5 = false;
            holokronMain6 = false;

            _label1 = "- Click on the Add Blueprints check box to open the Craft Browser";
            _label2 = "and select a craft you wish to save as blueprints in the HoloKron";
            _label3 = "";
            _label4 = "- Click on the Save Local Vessel check box to save vessels in the";
            _label5 = "local area as well all information required for later spawning";
            _label6 = "";
            _label7 = "- Click on the Save button to save the HoloKron";
            _label8 = "";
            _label9 = "";
            _label0 = "";

        }
        private void HolokKronMain5()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = true;
            holokronMain6 = false;

            _label1 = "- Click on the START ADD COORDS to start adding coordinates";
            _label2 = "";

            _label3 = "- Click on Add Location to spawn a stage gate";
            _label4 = "";

            _label5 = "- Use the translate and rotate keys (Q, E,  I, J, K, L) to orient";
            _label6 = "the stage gate to your liking";
            _label7 = "";

            _label8 = "- Click on 'PLACE GATE' to place it and have that location saved";
            _label9 = "to the challenge coordinates list";
            _label0 = "";

        }
        private void HolokKronMain6()
        {
            holokronMain = false;
            holokronMain2 = false;
            holokronMain3 = false;
            holokronMain4 = false;
            holokronMain5 = false;
            holokronMain6 = true;

            _label1 = "- Click on Add Next Stage to add another stage to the challenge";
            _label2 = "";
            _label3 = "- When done adding stages click SAVE AND EXIT";
            _label4 = "Focus will switch back to the Kerbal whom created the HoloKron";
            _label5 = "and vessel switching will be re-enabled as well as";
            _label6 = "your challenge will be saved and ready for sharing";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

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

            _label1 = "- OrX Kontinuum contains a scoreboard for each challenge";
            _label2 = "with 10 entries as well as the ability to view the challenge";
            _label3 = "entry in detail (stage times, cheats eneabled, air time etc)";
            _label4 = "";
            _label5 = "- Place shared .scores files into the OrX/Import/ directory";
            _label6 = "";
            _label7 = "- Click on import scores to view file contents and a detailed";
            _label8 = "breakdown of each stage in the challenge including Top Speed,";
            _label9 = "Total Air Time, Max Depth, which cheats were enabled as well as";
            _label0 = "the total time for each stage";

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

            _label1 = "- Outlaw Racing covers rover racing challenges";
            _label2 = "Outlaw Racing Types - Drag, Short Track and Dakar Racing";
            _label3 = "";
            _label4 = "- W[ind/S] covers W[ind/S] related challenges";
            _label5 = "";
            _label6 = "- Scuba Kerb covers underwater challenges";
            _label7 = "";
            _label8 = "- BD Armory covers BD Armory challenges";
            _label9 = "";
            _label0 = "";

        }
        private void ChallengeDrag()
        {
            cTypes1 = false;
            cTypes2 = true;
            cTypes3 = false;
            cTypes4 = false;
            cTypes5 = false;
            cTypes6 = false;
            cTypes7 = false;
            cTypes8 = false;

            _label1 = "- Drag Racing is a straight point A to point B race";
            _label2 = "- Time will be saved to scoreboard as well as a .scores";
            _label3 = "file will be saved to the OrX/Export/ directory for sharing";
            _label4 = "";
            _label5 = "CURRENTLY UNAVAILABLE";
            _label6 = "";
            _label7 = "";
            _label8 = "";
            _label9 = "";
            _label0 = "";

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
            _label2 = "";
            _label3 = "- When you pass through a stage gate your time will be recorded";
            _label4 = "";
            _label5 = "- Green indicator will show where the next stage gate is";
            _label6 = "";
            _label7 = "- Time will be saved to scoreboard as well as a .scores";
            _label8 = "file will be saved to the OrX/Export/ directory for sharing";
            _label9 = "once the challenge has been completed as well as ";
            _label0 = "any blueprints included will be available in the editor";

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
            _label8 = "CURRENTLY UNAVAILABLE";
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
            _label3 = "CURRENTLY UNAVAILABLE";
            _label4 = "";
            _label5 = "CURRENTLY UNAVAILABLE";
            _label6 = "";
            _label7 = "CURRENTLY UNAVAILABLE";
            _label8 = "";
            _label9 = "CURRENTLY UNAVAILABLE";
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
            _label3 = "CURRENTLY UNAVAILABLE";
            _label4 = "";
            _label5 = "CURRENTLY UNAVAILABLE";
            _label6 = "";
            _label7 = "CURRENTLY UNAVAILABLE";
            _label8 = "";
            _label9 = "CURRENTLY UNAVAILABLE";
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
            _label3 = "CURRENTLY UNAVAILABLE";
            _label4 = "";
            _label5 = "CURRENTLY UNAVAILABLE";
            _label6 = "";
            _label7 = "CURRENTLY UNAVAILABLE";
            _label8 = "";
            _label9 = "CURRENTLY UNAVAILABLE";
            _label0 = "";

        }

        private void MrKleenMain()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            _label1 = "- Mr Kleen's Magic Eraser gives you the ability to clean";
            _label2 = "up your mess before, during or after starting a challenge";
            _label3 = "";
            _label4 = "- The Magic Eraser can be found in the Settings menu";
            _label5 = "";
            _label6 = "- Every vessel except for the active vessel is a target";
            _label7 = "for scrubbing from the game if it is within range";
            _label8 = "";
            _label9 = "- You may have to use the Magic Eraser a couple times";
            _label0 = "to erase all vessels and debris lying around";

        }

        private void PreMain()
        {
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);

            _label1 = "- OrX Kontinuum manipulates the stock game physics ranges";
            _label2 = "and requires you to turn off Physics Range Extender if you";
            _label3 = "have that mod installed in order for OrX Kontinuum to work";
            _label4 = "";
            _label5 = "- If PRE is installed and enabled OrX Kontinuum will reset";
            _label6 = "and not allow you to start any challenges as well as";
            _label7 = "not allow you to open or create a HoloKron";
            _label8 = "";
            _label9 = "- If PRE is installed OrX Kontinuum will adopt the ranges";
            _label0 = "that have been set in PRE";

        }

        private void OrXUserManualGUI(int OrXManual)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;
            GUI.Label(new Rect(0, 0, WindowWidth, 20), titleLabel, titleStyleMed);
            line++;
            line += 0.2f;

            if (!openSubmenu)
            {
                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "The Kontinuum", HighLogic.Skin.button))
                {
                    //openSubmenu = true;
                    //kontinuum = true;
                    //WindowWidth = 600;
                    //titleLabel = "About the Kontinuum";
                    OrXHoloKron.instance.ScreenMsg("The Kontinuum is currently unavailable");
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "OrX Scuba Kerb", HighLogic.Skin.button))
                {
                    openSubmenu = true;
                    scubaKerb = true;
                    WindowWidth = 600;
                    titleLabel = "OrX Scuba Kerb";
                    ScubaKerbMenu();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "OrX W[ind/S]", HighLogic.Skin.button))
                {
                    openSubmenu = true;
                    winds = true;
                    WindowWidth = 600;
                    titleLabel = "OrX W[ind/S]";
                    WindsMenu();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "OrX HoloKron System", HighLogic.Skin.button))
                {
                    openSubmenu = true;
                    holokron = true;
                    WindowWidth = 600;
                    titleLabel = "OrX HoloKron System";
                    HolokKronMain();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Challenge Types", HighLogic.Skin.button))
                {
                    openSubmenu = true;
                    challengeTypes = true;
                    WindowWidth = 600;
                    titleLabel = "Challenge Types";
                    ChallengeTypesMain();
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Physics Range", HighLogic.Skin.button))
                {
                    openSubmenu = true;
                    pre = true;
                    WindowWidth = 600;
                    titleLabel = "Physics Range";
                    PreMain();
                }

                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "About Mr. Kleen", HighLogic.Skin.button))
                {
                    openSubmenu = true;
                    mrKleen = true;
                    WindowWidth = 600;
                    titleLabel = "About Mr. Kleen";
                    MrKleenMain();
                }

                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), WindowWidth - 20, 20), "Close Kergan's Manual", HighLogic.Skin.button))
                {
                    guiEnabled = false;
                }
            }
            else
            {
                if (kontinuum)
                {

                }

                if (scubaKerb)
                {
                    if (!scubaMenu)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb Menu", HighLogic.Skin.button))
                        {
                            ScubaKerbMenu();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb Menu", HighLogic.Skin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!scubaBallast)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Ballast Controls", HighLogic.Skin.button))
                        {
                            ScubaKerbBallast();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Ballast Controls", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!narcosis)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Nitrogen Narcosis", HighLogic.Skin.button))
                        {
                            ScubaKerbNarcosis();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Nitrogen Narcosis", HighLogic.Skin.box))
                        {

                        }

                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!bends)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "The Bends", HighLogic.Skin.button))
                        {
                            ScubaKerbBends();
                        }

                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "The Bends", HighLogic.Skin.box))
                        {

                        }

                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyle);

                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyle);
                    line++;
                    line += 0.2f;

                }

                if (winds)
                {
                    if (!windsMenu)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Menu", HighLogic.Skin.button))
                        {
                            WindsMenu();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Menu", HighLogic.Skin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!windsControls)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Controls", HighLogic.Skin.button))
                        {
                            WindsControls();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S] Controls", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyle);
                    line++;
                    line += 0.2f;

                }

                if (holokron)
                {
                    if (!holokronMain)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "HoloKron Files", HighLogic.Skin.button))
                        {
                            HolokKronMain();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "HoloKron Files", HighLogic.Skin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!holokronMain2)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Sharing", HighLogic.Skin.button))
                        {
                            HolokKronMain2();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Sharing", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!holokronMain3)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating", HighLogic.Skin.button))
                        {
                            HolokKronMain3();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!holokronMain4)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating 2", HighLogic.Skin.button))
                        {
                            HolokKronMain4();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating 2", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!holokronMain5)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating 3", HighLogic.Skin.button))
                        {
                            HolokKronMain5();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating 3", HighLogic.Skin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!holokronMain6)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating 4", HighLogic.Skin.button))
                        {
                            HolokKronMain6();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Creating 4", HighLogic.Skin.box))
                        {

                        }
                    }
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyle);
                    line++;
                    line += 0.2f;

                }

                if (challengeTypes)
                {
                    if (!cTypes1)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Challenge Types", HighLogic.Skin.button))
                        {
                            ChallengeTypesMain();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Challenge Types", HighLogic.Skin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!cTypes8)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Score Board", HighLogic.Skin.button))
                        {
                            ChallengeScores();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Score Board", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!cTypes2)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Drag Racing", HighLogic.Skin.button))
                        {
                            ChallengeDrag();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Drag Racing", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!cTypes3)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Short Track", HighLogic.Skin.button))
                        {
                            ChallengeShortTrack();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Short Track", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!cTypes4)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Dakar Racing", HighLogic.Skin.button))
                        {
                            ChallengeDakar();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Dakar Racing", HighLogic.Skin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!cTypes5)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S]", HighLogic.Skin.button))
                        {
                            ChallengeWinds();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "W[ind/S]", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!cTypes6)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb", HighLogic.Skin.button))
                        {
                            ChallengeScuba();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Scuba Kerb", HighLogic.Skin.box))
                        {

                        }
                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyle);
                    line++;
                    line += 0.2f;

                    if (!cTypes7)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "BD Armory", HighLogic.Skin.button))
                        {
                            ChallengeBDAc();
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "BD Armory", HighLogic.Skin.box))
                        {

                        }
                    }

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyle);
                    line++;
                    line += 0.2f;

                }

                if (pre)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Physics Range", HighLogic.Skin.box))
                    {

                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyle);
                    line++;
                    line += 0.2f;

                }

                if (mrKleen)
                {
                    if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "The Magic Eraser", HighLogic.Skin.box))
                    {

                    }
                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label1, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label2, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label3, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label4, titleStyle);
                    line++;
                    line += 0.2f;


                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label5, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label6, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label7, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label8, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label9, titleStyle);
                    line++;
                    line += 0.2f;

                    GUI.Label(new Rect(250, ContentTop + (line * entryHeight), WindowWidth, 20), _label0, titleStyle);
                    line++;
                    line += 0.2f;
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + (line * entryHeight), 230, 20), "Return to Previous Menu", HighLogic.Skin.button))
                {
                    Reset();
                }

            }
            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }
    }
}