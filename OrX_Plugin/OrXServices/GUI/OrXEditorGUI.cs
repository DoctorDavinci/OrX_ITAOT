using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, true)]
    public class OrXEditor : MonoBehaviour
    {
        public static OrXEditor instance;

        #region Variables

        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public bool _guiEnabled = false;

        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private float _windowHeight = 250;
        public static Rect _windowRect;
        public static GUISkin OrXGUISkin = HighLogic.Skin;

        public bool _craftSelected = false;
        public bool _tuneCraft = false;
        public bool _tuning = false;
        public string _craftBeingTuned = "";
        public string crafttosave = "";

        public Vessel tunedCraft;


        static GUIStyle centerLabelW = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = XKCDColors.White }
        };

        static GUIStyle centerLabel = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = XKCDColors.BoogerGreen }
        };
        static GUIStyle titleStyleL = new GUIStyle(centerLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            normal = { textColor = XKCDColors.OrangeRed }
        };


        #endregion

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && _craftSelected)
            {
                if (!_guiEnabled)
                {
                    _guiEnabled = true;
                }
                _tuning = true;
                if (crafttosave == "" || tunedCraft == null)
                {
                    tunedCraft = FlightGlobals.ActiveVessel;
                    crafttosave = tunedCraft.vesselName;
                }
                else
                {

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
            _windowRect = new Rect((Screen.width / 2) - (WindowWidth / 2), (Screen.height / 2) - (_windowHeight / 2), WindowWidth, _windowHeight);
        }
        private void OnGUI()
        {
            GUI.backgroundColor = XKCDColors.DarkGrey;
            GUI.contentColor = XKCDColors.DarkGrey;
            GUI.color = XKCDColors.DarkGrey;

            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (PauseMenu.isOpen) return;
            }

            if (_guiEnabled)
            {
                _windowRect = GUI.Window(529344475, _windowRect, OrXEditorGUI, "");
            }
        }

        private void OrXEditorGUI(int EditorGUI)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;


            if (_tuneCraft)
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Kontinuum Craft Tuning", titleStyleL);
                line++;
                line += 0.2f;

                if (_tuning)
                {
                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Craft Being Tuned", centerLabel);
                    line += 0.2f;
                    line++;
                    GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), crafttosave, centerLabelW);
                    line++;
                    line += 0.2f;

                    if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Save Craft Variant", OrXGUISkin.button))
                    {
                        if (HighLogic.LoadedSceneIsFlight)
                        {
                            SaveCraftVariant(tunedCraft);
                        }
                    }
                }
                else
                {
                    if (!_craftSelected)
                    {
                        if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Select Craft", OrXGUISkin.button))
                        {
                            _guiEnabled = false;
                            spawn.OrXSpawnHoloKron.instance.CraftSelect(false, true, false);
                        }
                    }
                    else
                    {
                        if (HighLogic.LoadedSceneIsEditor)
                        {
                            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "LAUNCH CRAFT", OrXGUISkin.button))
                            {
                                EditorLogic.fetch.launchVessel();
                            }
                        }
                    }
                }
            }
            else
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Kontinuum", titleStyleL);
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Craft Tuning", OrXGUISkin.button))
                {
                    _tuneCraft = true;
                }
                line++;
                line += 0.2f;

                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "HoloKrons", OrXGUISkin.button))
                {
                    
                }
                line++;
                line += 0.2f;
                if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Missions", OrXGUISkin.button))
                {
                }
            }
            line++;
            line += 0.2f;

            if (GUI.Button(new Rect(10, ContentTop + line * entryHeight, WindowWidth - 20, 20), "Close Menu", OrXGUISkin.button))
            {
                _craftSelected = false;
                _tuning = false;
                _tuneCraft = false;
                _guiEnabled = false;
                _count = 0;
            }

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        int _count = 0;

        IEnumerator SaveCraftVariant(Vessel toSave)
        {
            _count += 1;
            int partCount = 0;
            string shipDescription = toSave.vesselName + " Variant " + _count;
            OrXLog.instance.DebugLog("[OrX Save Craft Variant] Saving " + toSave.vesselName + " .......................");
            ShipConstruct ConstructToSave = new ShipConstruct(toSave.vesselName + " Variant " + _count, shipDescription, toSave.parts[0]);
            ConfigNode craftConstruct = new ConfigNode("craft");
            craftConstruct = ConstructToSave.SaveShip();
            yield return new WaitForFixedUpdate();
            //craftConstruct.RemoveValue("persistentId");
            craftConstruct.RemoveValue("steamPublishedFileId");
            craftConstruct.RemoveValue("rot");
            craftConstruct.RemoveValue("missionFlag");
            craftConstruct.RemoveValue("vesselType");
            craftConstruct.RemoveValue("OverrideDefault");
            craftConstruct.RemoveValue("OverrideActionControl");
            craftConstruct.RemoveValue("OverrideAxisControl");
            craftConstruct.RemoveValue("OverrideGroupNames");
            string _craftFileToSave = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/SPH/" + toSave.vesselName + "-Variant-" + _count + ".craft";
            craftConstruct.Save(_craftFileToSave);

            OrXLog.instance.DebugLog("[OrX Save Craft Variant] Saved " + toSave.vesselName + " to the hangar .......................");
            OrXHoloKron.instance.OnScrnMsgUC("<color=#cfc100ff><b>" + toSave.vesselName + " Variant " + _count + " Saved</b></color>");
        }
    }
}