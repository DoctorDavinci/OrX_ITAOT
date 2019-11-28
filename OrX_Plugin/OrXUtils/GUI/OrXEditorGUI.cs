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
                            spawn.OrXSpawnHoloKron.instance.CraftSelect(false, true);
                        }
                    }
                    else
                    {
                        if (HighLogic.LoadedSceneIsEditor)
                        {
                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "Launch your Craft using", centerLabel);
                            line += 0.2f;
                            line++;
                            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20), "the stock launch button", centerLabel);
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
                    _tuneCraft = true;
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
            //craftConstruct.RemoveValue("persistentId");
            //craftConstruct.RemoveValue("steamPublishedFileId");
            craftConstruct.RemoveValue("rot");
            //craftConstruct.RemoveValue("missionFlag");
            craftConstruct.RemoveValue("vesselType");
            craftConstruct.RemoveValue("OverrideDefault");
            craftConstruct.RemoveValue("OverrideActionControl");
            craftConstruct.RemoveValue("OverrideAxisControl");
            craftConstruct.RemoveValue("OverrideGroupNames");
            string _craftFileToSave = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/SPH/" + toSave.vesselName + "-Variant-" + _count + ".craft";

            ConfigNode.ConfigNodeList _parts = new ConfigNode.ConfigNodeList();
            ConfigNode.ConfigNodeList _partsToEdit = new ConfigNode.ConfigNodeList();
            ConfigNode _tempCraftFile = new ConfigNode();

            ConfigNode _craftBeingTuned_ = ConfigNode.Load(_craftBeingTuned);
            foreach (ConfigNode.Value cv in _craftBeingTuned_.values)
            {
                if (cv.name == "ship")
                {
                    cv.value = toSave.vesselName + " Variant " + _count;
                }

                _tempCraftFile.AddValue(cv.name, cv.value);
            }
            foreach (ConfigNode _partNodeToEdit in _craftBeingTuned_.nodes)
            {
                if (_partNodeToEdit.name == "PART")
                {
                    partCount += 1;
                    _partsToEdit.Add(_partNodeToEdit);
                }
            }

            foreach (ConfigNode _partNode in craftConstruct.nodes)
            {
                if (_partNode.name == "PART")
                {
                    bool _continue = true;
                    foreach (ConfigNode.Value _val in _partNode.values)
                    {
                        if (_val.name == "part" && _val.value.Contains("kerbalEVA"))
                        {
                            _continue = false;
                        }
                    }

                    if (_continue)
                    {
                        _parts.Add(_partNode);
                    }
                }
            }

            yield return new WaitForFixedUpdate();

            /*

            while (partCount != 0)
            {
                ConfigNode _partBeingEdited = new ConfigNode();





                for (int i = 0; i < _partsToEdit.Count; i++)
                {
                    if (_partsToEdit[i] != null)
                    {
                        _partBeingEdited = _partsToEdit[partCount];
                        break;
                    }
                }

                ConfigNode _fsPart = new ConfigNode();

                for (int i = 0; i < _parts.Count; i++)
                {
                    if (_parts[i] != null)
                    {
                        _fsPart = _parts[i];
                        break;
                    }
                }

                ConfigNode _nodeToAdd = _tempCraftFile.AddNode(_partBeingEdited.name);
                foreach (ConfigNode.Value cv in _partBeingEdited.values)
                {
                    _nodeToAdd.AddValue(cv.name, cv.value);
                }

            }

            */


            foreach (ConfigNode cn in craftConstruct.nodes)
            {
                if (cn.name == "PART")
                {
                    foreach (ConfigNode.Value cv in cn.values)
                    {
                        if (cv.name == "part")
                        {
                            if (!cv.value.Contains("kerbalEVA"))
                            {
                                foreach (ConfigNode _tempNode in _craftBeingTuned_.nodes)
                                {
                                    if (_tempNode.name == "PART")
                                    {
                                        foreach (ConfigNode.Value _value in _tempNode.values)
                                        {
                                            if (_value.name == "part")
                                            {
                                                if (_value.value == cv.value)
                                                {
                                                    // CONTINUE WITH EDITING CRAFT FILE
                                                    foreach (ConfigNode _tempCNnode in cn.nodes)
                                                    {
                                                        foreach (ConfigNode _tempNode2 in _tempNode.nodes)
                                                        {
                                                            if (_tempCNnode.name == _tempNode2.name)
                                                            {
                                                                // FIRST LAYER MATCHES

                                                                foreach (ConfigNode.Value _nodeValues in _tempCNnode.values)
                                                                {
                                                                    _tempNode2.SetValue(_nodeValues.name, _nodeValues.value);
                                                                }

                                                                foreach (ConfigNode _tempCNnode2 in _tempCNnode.nodes)
                                                                {
                                                                    foreach (ConfigNode _tempNode3 in _tempNode2.nodes)
                                                                    {
                                                                        if (_tempNode3.name == _tempCNnode2.name)
                                                                        {
                                                                            // SECOND LAYER MATCHES

                                                                            foreach (ConfigNode.Value _nodeValues2 in _tempCNnode2.values)
                                                                            {
                                                                                _tempNode3.SetValue(_nodeValues2.name, _nodeValues2.value);

                                                                            }

                                                                            foreach (ConfigNode _tempCNnode3 in _tempCNnode2.nodes)
                                                                            {
                                                                                foreach (ConfigNode _tempNode4 in _tempNode3.nodes)
                                                                                {
                                                                                    if (_tempNode4.name == _tempCNnode3.name)
                                                                                    {
                                                                                        // THIRD LAYER MATCHES

                                                                                        foreach (ConfigNode.Value _nodeValues3 in _tempCNnode3.values)
                                                                                        {
                                                                                            _tempNode3.SetValue(_nodeValues3.name, _nodeValues3.value);

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
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
           
            _craftBeingTuned_.Save(_craftFileToSave);
            /*
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(_craftFileToSave))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Contains("link") && !line.Contains("kerbalEVA"))
                    {
                        sw.WriteLine(line);
                    }
                }
            }

            File.Delete(_craftFileToSave);
            File.Move(tempFile, _craftFileToSave);
            */
            OrXLog.instance.DebugLog("[OrX Save Craft Variant] Saved " + toSave.vesselName + " to the hangar .......................");

            OrXHoloKron.instance.OnScrnMsgUC("<color=#cfc100ff><b>" + toSave.vesselName + " Variant " + _count + " Saved</b></color>");
        }
    }
}