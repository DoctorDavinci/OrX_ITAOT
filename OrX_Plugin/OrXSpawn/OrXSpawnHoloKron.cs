using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KSP.UI.Screens;
using System.IO;

namespace OrX.spawn
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
    public class OrXSpawnHoloKron : MonoBehaviour
    {
        public static OrXSpawnHoloKron instance;
        public Vector3 UpVect;
        public Vector3 EastVect;
        public Vector3 NorthVect;
        public double _lat = 0;
        public double _lon = 0;
        public double _alt = 0;
        public Vessel startGate;
        bool dakarRacing = false;
        public bool openingCraftBrowser = false;
        public CraftBrowserDialog craftBrowser;
        public bool spawningGoal = false;
        public int stageCount = 0;
        public bool spawning = false;
        public string _vesselName = "";
        public string flagURL = "OrX/Plugin/OrX_icon";
        public string orxCraft = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/OrX/FOrX.craft";
        public string GoalPostCraft = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/Goal/Goal.craft";
        public bool _spawnCraftFile = false;
        private bool wingman = false;
        public double _holoAlt = 0;
        Vessel trigger;
        public List<string> _airSupportList;
        public List<string> _groundSupportList;
        public List<string> _seaSupportList;
        public List<string> _interceptorList;
        public List<string> _interceptorNameList;

        public bool _doubleMint = false;
        public int _airSupportCount = 0;
        public int _enemyCount = 0;
        public int _groundSupportCount = 0;
        public int _seaSupportCount = 0;
        public int _interceptorCount = 0;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }

        public void ProcessHoloKronFile(string _holoName, string _holoFile)
        {
            ConfigNode _file = ConfigNode.Load(_holoFile);

            if (_file != null)
            {
                OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === Process HoloKron File: " + _holoFile + " === ");
                _airSupportList = new List<string>();
                _groundSupportList = new List<string>();
                _seaSupportList = new List<string>();
                _interceptorList = new List<string>();
                _interceptorNameList = new List<string>();
                OrXHoloKron.instance._opForCount = 0;
                _enemyCount = 0;
                _airSupportCount = 0;
                _groundSupportCount = 0;
                _seaSupportCount = 0;
                _interceptorCount = 0;

                string pas = "";
                UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
                EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
                NorthVect = Vector3.Cross(EastVect, UpVect).normalized;


                int _vesselCount = 1;
                int _hkCount = 0;
                ConfigNode node = _file.GetNode("OrX");

                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (spawnCheck.name.Contains("OrXHoloKronCoords" + _hkCount))
                    {
                        OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === FOUND " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " ... DECRYPTING ===");

                        foreach (ConfigNode.Value data in spawnCheck.values)
                        {
                            if (data.name == "challengeType")
                            {
                                if (data.value.Contains("OUTLAW RACING"))
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] ===  " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is OUTLAW RACING ===");

                                    OrXHoloKron.instance.outlawRacing = true;
                                }

                                if (data.value.Contains("BD ARMORY"))
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] ===  " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is BD ARMORY ===");

                                    OrXHoloKron.instance.bdaChallenge = true;
                                    OrXHoloKron.instance.outlawRacing = false;

                                }

                                if (data.value.Contains("GEO-CACHE"))
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] ===  " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is GEO-CACHE ===");

                                    OrXHoloKron.instance.geoCache = true;
                                }
                            }

                            if (data.name == "spawned")
                            {
                                if (data.value == "False")
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] ===  " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has not spawned ===");
                                    spawnCheck.SetValue("spawned", "True", true);
                                    _file.Save(_holoFile);
                                    //break;
                                }
                                else
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has spawned ... CHECKING FOR EXTRAS");

                                    if (spawnCheck.HasValue("extras"))
                                    {
                                        if (spawnCheck.GetValue("extras") == "False")
                                        {
                                            OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has no extras ... END TRANSMISSION");
                                            //break;
                                        }
                                        else
                                        {
                                            OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === " + _holoName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has extras ... SEARCHING");
                                            _hkCount += 1;
                                        }
                                    }
                                }
                            }
                        }

                        OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === DATA PROCESSED ===");
                    }
                }

                int ASvCount = 0;

                foreach (ConfigNode _vts in node.nodes)
                {
                    if (_vts.name.Contains("HC" + _hkCount + "ASv"))
                    {
                        OrXTargetDistance.instance._randomSpawned = false;
                        OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === GRABBING CRAFT FILE FOR " + _vts.name + " ===");

                        ASvCount += 1;

                        OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === DECRYPTING CRAFT FILE DATA FOR " + _vts.name + " ===");
                        ConfigNode craftFile = _vts.GetNode("craft");
                        _interceptorNameList.Add(craftFile.GetValue("ship"));
                        OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === VESSEL IS INTERCEPTOR ===");
                        _interceptorCount += 1;
                        _interceptorList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/interceptor" + _interceptorCount + ".tmp");
                        craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/interceptor" + _interceptorCount + ".tmp");
                    }

                    if (_vts.name.Contains("HC" + _hkCount + "OrXv" + _vesselCount))
                    {
                        OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === GRABBING CRAFT FILE FOR " + _vts.name + " ===");
                        _enemyCount += 1;
                        OrXHoloKron.instance._opForCount += 1;
                        _vesselCount += 1;
                        bool _airborne = false;
                        bool _splashed = false;

                        ConfigNode location = _vts.GetNode("coords");

                        foreach (ConfigNode.Value loc in location.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                            string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                            loc.name = cvEncryptedName;
                            loc.value = cvEncryptedValue;
                            if (loc.name == "airborne")
                            {
                                if (loc.value == "True")
                                {
                                    _airborne = true;
                                }
                            }
                            if (loc.name == "splashed")
                            {
                                if (loc.value == "True")
                                {
                                    _splashed = true;
                                }
                            }
                            if (loc.name == "pas")
                            {
                                pas = loc.value;
                            }
                        }

                        OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === DECRYPTING CRAFT FILE DATA FOR " + _vts.name + " ===");
                        ConfigNode craftFile = _vts.GetNode("craft");

                        if (_airborne)
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === VESSEL IS AIRBORNE ===");

                            _airSupportCount += 1;
                            _airSupportList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/airSupport" + _airSupportCount + ".tmp");
                            craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/airSupport" + _airSupportCount + ".tmp");
                        }
                        else
                        {
                            if (_splashed)
                            {
                                OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === VESSEL IS SPLASHED ===");

                                _seaSupportCount += 1;
                                _seaSupportList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/seaSupport" + _seaSupportCount + ".tmp");
                                craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/seaSupport" + _seaSupportCount + ".tmp");

                            }
                            else
                            {
                                OrXLog.instance.DebugLog("[OrX Spawn Process HoloKron] === VESSEL IS LANDED ===");

                                _groundSupportCount += 1;
                                _groundSupportList.Add(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/groundSupport" + _groundSupportCount + ".tmp");
                                craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/groundSupport" + _groundSupportCount + ".tmp");

                            }
                        }
                    }
                }
            }
        }
        private void AddBlueprints(string _selectedCraftFile)
        {
            craftBrowser = null;
            openingCraftBrowser = false;
            string _craftName = "";
            ConfigNode _craftFile = ConfigNode.Load(_selectedCraftFile);
            OrXLog.instance.DebugLog("[OrX Craft Select] SAVING BLUEPRINTS TO HOLOKRON ............");
            OrXSounds.instance.PlayCraftSelected();

            foreach (ConfigNode.Value cv in _craftFile.values)
            {
                if (cv.name == "ship")
                {
                    _craftName = cv.value;
                    break;
                }

                string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                cv.name = cvEncryptedName;
                cv.value = cvEncryptedValue;
            }

            foreach (ConfigNode cn in _craftFile.nodes)
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

            if (OrXHoloKron.instance.addingBluePrints)
            {
                OrXHoloKron.instance.craftToAddMission = _craftName;
                OrXHoloKron.instance.addingBluePrints = false;
                OrXHoloKron.instance.blueprintsAdded = true;
                OrXHoloKron.instance.blueprintsLabel = OrXHoloKron.instance.craftToAddMission;
                OrXHoloKron.instance.spawningGoal = false;
                OrXHoloKron.instance.blueprintsFile = _selectedCraftFile;
                OrXHoloKron.instance.addingBluePrints = false;
                OrXHoloKron.instance.movingCraft = false;
                OrXHoloKron.instance.OrXHCGUIEnabled = true;
                OrXHoloKron.instance.OnScrnMsgUC(OrXHoloKron.instance.craftToAddMission + " Saved to HoloKron ....");
                OrXHoloKron.instance.GuiEnabledOrXMissions = true;
            }
            else
            {
                if (!OrXHoloKron.instance._savingAirSup1)
                {
                    OrXHoloKron.instance._savingAirSup1 = true;
                    OrXHoloKron.instance._airSupName1 = _craftName;
                    OrXHoloKron.instance._airSupFile1 = _selectedCraftFile;
                }
                else
                {
                    if (!OrXHoloKron.instance._savingAirSup2)
                    {
                        OrXHoloKron.instance._savingAirSup2 = true;
                        OrXHoloKron.instance._airSupName2 = _craftName;
                        OrXHoloKron.instance._airSupFile2 = _selectedCraftFile;
                    }
                    else
                    {
                        if (!OrXHoloKron.instance._savingAirSup3)
                        {
                            OrXHoloKron.instance._savingAirSup3 = true;
                            OrXHoloKron.instance._airSupName3 = _craftName;
                            OrXHoloKron.instance._airSupFile3 = _selectedCraftFile;
                        }
                    }
                }
                OrXHoloKron.instance.OrXHCGUIEnabled = true;
            }
        }
        public void StartSpawn(Vector3d stageStartCoords, Vector3d vect, bool Goal, bool empty, bool primary, string HoloKronName, string challengeType)
        {
            OrXHoloKron.instance.Reach();
            OrXVesselLog.instance._enemyCraft = new List<Vessel>();
            if (OrXLog.instance._preInstalled)
            {
                if (!OrXLog.instance.PREnabled())
                {
                    spawning = true;
                    StartCoroutine(SpawnHoloKron(stageStartCoords, vect, Goal, empty, primary, HoloKronName, challengeType));
                }
            }
            else
            {
                spawning = true;
                StartCoroutine(SpawnHoloKron(stageStartCoords, vect, Goal, empty, primary, HoloKronName, challengeType));
            }
        }
        public void SpawnLBC(int _stage, string HoloKronName)
        {
            if (OrXLog.instance._preInstalled)
            {
                if (!OrXLog.instance.PREnabled())
                {
                    spawning = true;
                    StartCoroutine(SpawnLBCRoutine(_stage, HoloKronName));
                }
            }
            else
            {
                spawning = true;
                StartCoroutine(SpawnLBCRoutine(_stage, HoloKronName));
            }
        }
        public void SpawnLocal(bool _bda, string HoloKronName, Vector3d vect, float _delay, int _count)
        {
            trigger = FlightGlobals.ActiveVessel;
            OrXHoloKron.instance.triggerVessel = FlightGlobals.ActiveVessel;

            if (OrXLog.instance._preInstalled)
            {
                if (!OrXLog.instance.PREnabled())
                {
                    spawning = true;
                    if (!_bda)
                    {
                        OrXHoloKron.instance.Reach();
                    }
                    StartCoroutine(SpawnLocalVessels(_bda, HoloKronName, vect, _delay, _count));
                }
            }
            else
            {
                spawning = true;
                if (!_bda)
                {
                    OrXHoloKron.instance.Reach();
                }

                StartCoroutine(SpawnLocalVessels(_bda, HoloKronName, vect, _delay, _count));
            }
        }

        public void SpawnFile(string _craftFile, bool _addCrew, bool _eva, bool _cavalry, bool _enemyAirSupport, bool _crypt, float _delay, float _left, float _pitch, Vector3d _coords)
        {
            spawning = true;
            trigger = FlightGlobals.ActiveVessel;
            if (_eva)
            {
                _craftFile = orxCraft;
            }
            StartCoroutine(SpawnFromCraftFile(_craftFile, _addCrew, _eva, _cavalry, _enemyAirSupport, _crypt, _delay, _left, _pitch, _coords));
        }
        private void SpawnSelected(string _selectedCraftFile)
        {
            _spawnCraftFile = false;

            if (HighLogic.LoadedSceneIsEditor)
            {
                craftBrowser = null;
                openingCraftBrowser = false;

                OrXEditor.instance._tuneCraft = true;
                OrXEditor.instance._craftSelected = true;
                OrXEditor.instance._craftBeingTuned = _selectedCraftFile;
                EditorLogic.LoadShipFromFile(_selectedCraftFile);
            }
            else
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    spawning = true;

                    if (!OrXHoloKron.instance.buildingMission)
                    {
                        int _charToSubract = _selectedCraftFile.Length - HighLogic.SaveFolder.Length - 19;
                        string _metaFile = _selectedCraftFile.Remove(0, _charToSubract);
                        string[] _ext = _metaFile.Split(new char[] { '.' });
                        string _toLoad = "";

                        for (int i = 0; i < _ext.Length - 1; ++i)
                        {
                            _toLoad += _ext[i];
                        }

                        OrXLog.instance.DebugLog("[Spawn OrX Spawn Selected] File Meta: " + _toLoad);

                        ConfigNode _meta = null;

                        List<string> files = new List<string>(Directory.GetFiles(UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder, "*.loadmeta", SearchOption.AllDirectories));
                        if (files != null)
                        {
                            List<string>.Enumerator _craftMeta = files.GetEnumerator();
                            while (_craftMeta.MoveNext())
                            {
                                if (_craftMeta.Current != null)
                                {
                                    if (_craftMeta.Current.Contains(_toLoad))
                                    {
                                        _meta = new ConfigNode();
                                        _meta = ConfigNode.Load(_craftMeta.Current);
                                        break;
                                    }
                                }
                            }
                            _craftMeta.Dispose();
                        }

                        if (_meta == null)
                        {
                            List<string> files2 = new List<string>(Directory.GetFiles(UrlDir.ApplicationRootPath + "Ships/", "*.loadmeta", SearchOption.AllDirectories));
                            if (files2 != null)
                            {
                                List<string>.Enumerator _craftMeta = files2.GetEnumerator();
                                while (_craftMeta.MoveNext())
                                {
                                    if (_craftMeta.Current != null)
                                    {
                                        if (_craftMeta.Current.Contains(_toLoad))
                                        {
                                            _meta = new ConfigNode();
                                            _meta = ConfigNode.Load(_craftMeta.Current);
                                            break;
                                        }
                                    }
                                }
                                _craftMeta.Dispose();
                            }
                        }

                        if (_meta != null)
                        {
                            OrXSounds.instance.PlaySpawnBySalt();

                            float _saltCost = float.Parse(_meta.GetValue("totalCost"));

                            if (_saltCost >= OrXHoloKron.instance.salt)
                            {
                                OrXSounds.instance.sound_MissedItByThatMuch.Play();
                                OrXHoloKron.instance.OnScrnMsgUC((_saltCost - OrXHoloKron.instance.salt) + " more Salt required to purchase that vessel");
                                spawning = false;
                            }
                            else
                            {
                                craftBrowser = null;
                                openingCraftBrowser = false;

                                OrXHoloKron.instance.OnScrnMsgUC("That just cost you " + _saltCost + " Salt");
                                OrXHoloKron.instance.salt -= _saltCost;
                                SpawnFile(_selectedCraftFile, true, OrXHoloKron.instance._spawningOrX, wingman, false, false, 0, 0, 0, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                            }
                        }
                        else
                        {
                            OrXSounds.instance.sound_SpawnOrXNeeds.Play();
                            OrXHoloKron.instance.OnScrnMsgUC(_toLoad + " is Priceless !!!");
                            spawning = false;
                        }
                    }
                    else
                    {
                        craftBrowser = null;
                        openingCraftBrowser = false;
                        SpawnFile(_selectedCraftFile, true, OrXHoloKron.instance._spawningOrX, wingman, false, true, 0, 0, 0, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                    }
                }
            }
        }

        public void SpawnAirSupport(bool _interceptor, bool _bda, string HoloKronName, Vector3d vect, float _delay)
        {
            trigger = FlightGlobals.ActiveVessel;
            OrXHoloKron.instance.triggerVessel = FlightGlobals.ActiveVessel;

            if (OrXLog.instance._preInstalled)
            {
                if (!OrXLog.instance.PREnabled())
                {
                    try
                    {
                        if (!_bda)
                        {
                            OrXHoloKron.instance.Reach();
                        }
                        else
                        {

                            //_airSupportList = new List<string>();
                            //_groundSupportList = new List<string>();
                            //_seaSupportList = new List<string>();
                            OrXHoloKron.instance.SetBDAc();
                            StartCoroutine(SpawnAirSupportRoutine(_interceptor, _bda, HoloKronName, vect, _delay));
                            spawning = true;
                        }
                    }
                    catch
                    {
                        SpawnAirSupport(_interceptor, _bda, HoloKronName, vect, _delay);
                    }
                }
            }
            else
            {
                spawning = true;
                OrXHoloKron.instance.Reach();
                StartCoroutine(SpawnAirSupportRoutine(_interceptor, _bda, HoloKronName, vect, _delay));
            }
        }
        public void SpawnRandomAirSupport(float _delay)
        {
            if (!OrXLog.instance.PREnabled())
            {
                if (OrXHoloKron.instance.IronKerbal)
                {
                    SpawnAirSupportFromList(_delay);
                }
                else
                {
                    SpawnInterceptorFromList(_delay);
                }
            }
        }

        public void SpawnAirSupportFromList(float _delay)
        {
            if (_airSupportList.Count != 0)
            {
                spawning = true;
                OrXSounds.instance.sound_OrXSpinachChin.Play();
                int r = new System.Random().Next(1, _airSupportList.Count);
                int _count = 0;
                List<string>.Enumerator _airSupport = _airSupportList.GetEnumerator();
                while (_airSupport.MoveNext())
                {
                    if (_airSupport.Current != null)
                    {
                        _count += 1;
                        if (r == _count)
                        {
                            OrXHoloKron.instance.SetBDAc();
                            SpawnFile(_airSupport.Current, true, false, true, true, true, _delay, 0, 0, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                        }
                    }
                }
                _airSupport.Dispose();
            }
        }
        public void SpawnInterceptorFromList(float _delay)
        {
            if (_interceptorList.Count != 0)
            {
                spawning = true;
                OrXSounds.instance.sound_OrXSpinachChin.Play();
                int r = new System.Random().Next(1, _interceptorList.Count);
                int _count = 0;
                List<string>.Enumerator _interceptor = _interceptorList.GetEnumerator();
                while (_interceptor.MoveNext())
                {
                    if (_interceptor.Current != null)
                    {
                        _count += 1;
                        if (r == _count)
                        {
                            OrXHoloKron.instance.SetBDAc();
                            SpawnFile(_interceptor.Current, true, false, true, true, true, _delay, 0, 0, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                        }
                    }
                }
                _interceptor.Dispose();
            }
        }
        public void SpawnSeaSupportFromList(float _delay)
        {
            if (_seaSupportList.Count != 0)
            {
                spawning = true;
                OrXSounds.instance.sound_OrXSpinachChin.Play();
                int r = new System.Random().Next(1, _seaSupportList.Count);
                int _count = 0;
                List<string>.Enumerator _seaSupport = _seaSupportList.GetEnumerator();
                while (_seaSupport.MoveNext())
                {
                    if (_seaSupport.Current != null)
                    {
                        _count += 1;
                        if (r == _count)
                        {
                            OrXHoloKron.instance.SetBDAc();
                            SpawnFile(_seaSupport.Current, true, false, true, true, true, _delay, 0, 0, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                        }
                    }
                }
                _seaSupport.Dispose();
            }
        }
        public void SpawnGroundSupportFromList(float _delay)
        {
            if (_groundSupportList.Count != 0)
            {
                spawning = true;
                OrXSounds.instance.sound_OrXSpinachChin.Play();
                int r = new System.Random().Next(1, _groundSupportList.Count);
                int _count = 0;
                List<string>.Enumerator _groundSupport = _groundSupportList.GetEnumerator();
                while (_groundSupport.MoveNext())
                {
                    if (_groundSupport.Current != null)
                    {
                        _count += 1;
                        if (r == _count)
                        {
                            OrXHoloKron.instance.SetBDAc();
                            SpawnFile(_groundSupport.Current, true, false, true, true, true, _delay, 0, 0, new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                        }
                    }
                }
                _groundSupport.Dispose();
            }
        }

        IEnumerator SpawnFromCraftFile(string _craftFile, bool _addCrew, bool _eva, bool _cavalry, bool _enemyAirSupport, bool _crypt, float _delay, float _left, float _pitch, Vector3d _coords)
        {
            string _name = "";
            double _altToAdd = 30;
            string _craftFileToSpawn = _craftFile;

            if (_crypt)
            {
                ConfigNode _enemyFile = ConfigNode.Load(_craftFile);
                if (_enemyFile != null)
                {
                    OrXLog.instance.DebugLog("[Spawn OrX Craft File] _enemyFile: " + _craftFile);

                    foreach (ConfigNode.Value cv in _enemyFile.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;

                        if (cv.name == "ship")
                        {
                            _name = cv.value;
                        }
                    }

                    foreach (ConfigNode cn in _enemyFile.nodes)
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
                                if (!cv2.value.Contains("(") && !cv2.value.Contains(")"))
                                {
                                    string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                    cv2.name = cvEncryptedName;
                                    cv2.value = cvEncryptedValue;
                                }
                            }
                        }
                    }
                }
                _enemyFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/craftFromFile.tmp");
                _craftFileToSpawn = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/craftFromFile.tmp";
            }
            yield return new WaitForFixedUpdate();
            if (_cavalry)
            {
                int r = new System.Random().Next(1000, 20000);
                double dist = new System.Random().Next(8000, 15000);

                if (_enemyAirSupport)
                {
                    OrXLog.instance.SetFocusKeys();

                    OrXHoloKron.instance._opForCount += 1;
                    OrXSounds.instance.sound_SpawnOrXHole.Play();

                    if (FlightGlobals.ActiveVessel.radarAltitude <= 3000)
                    {
                        _altToAdd = 5000 + (dist / 4);
                    }
                    else
                    {
                        _altToAdd = r / 4;
                    }

                    if (!OrXTargetDistance.instance._randomSpawned)
                    {
                        if (_doubleMint)
                        {
                            _doubleMint = false;
                        }
                        else
                        {
                            _doubleMint = true;
                        }
                    }
                }
                else
                {
                    OrXSounds.instance.sound_OrXSheBitch.Play();

                    dist *= 0.15;
                    _altToAdd = r / 10;
                }

                double offset = dist * (1 / (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360));
                yield return new WaitForFixedUpdate();

                if (r <= 5000)
                {
                    _lat = _coords.x + offset;
                    _lon = _coords.y + offset;
                }
                else
                {
                    if (r <= 10000)
                    {
                        _lat = _coords.x - offset;
                        _lon = _coords.y - offset;
                    }
                    else
                    {
                        if (r <= 15000)
                        {
                            _lat = _coords.x + offset;
                            _lon = _coords.y - offset;
                        }
                        else
                        {
                            _lat = _coords.x - offset;
                            _lon = _coords.y + offset;
                        }
                    }
                }
            }
            else
            {
                _lat = _coords.x;
                _lon = _coords.y;
            }
            _alt = _coords.z + _altToAdd;

            ConfigNode vesselToLoad = ConfigNode.Load(_craftFileToSpawn);
            if (vesselToLoad != null)
            {
                _name = vesselToLoad.GetValue("ship");
                foreach (ConfigNode.Value cv in vesselToLoad.values)
                {
                    if (cv.name == "ship")
                    {
                        _name = cv.value;
                    }

                    if (cv.name == "size")
                    {
                        string[] _sizeString = cv.value.Split(new char[] { ',' });

                        try
                        {
                            if (_sizeString[0] != null && _sizeString[0].Length > 0 && _sizeString[0] != "null")
                            {
                                for (int i = 0; i < _sizeString.Length; i++)
                                {
                                    if (_sizeString[i] != null && _sizeString[i].Length > 0)
                                    {
                                        float _maxValue = Math.Max(float.Parse(_sizeString[0]), Math.Max(float.Parse(_sizeString[1]), float.Parse(_sizeString[2])));
                                        _altToAdd += _maxValue / 2;
                                    }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Altitude calculated ...... ");
                        }
                    }
                }
            }

            Vector3d tpoint = FlightGlobals.currentMainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt);
            Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);

            OrXLog.instance.DebugLog("[Spawn OrX Craft File] Altitude: " + gpsPos.z);

            bool landed = false;
            Orbit orbit = null;

            if (!landed)
            {
                landed = true;
                Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Calculating Orbit ================== ");

                orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
            }
            OrXLog.instance.DebugLog("[Spawn OrX Craft File] Orbit Calculated ================== ");

            ConfigNode[] pNodes;
            ShipConstruct shipConstruct = null;
            if (!string.IsNullOrEmpty(_craftFileToSpawn))
            {
                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Loading Ship ================== ");

                ConfigNode currentShip = ShipConstruction.ShipConfig;
                shipConstruct = ShipConstruction.LoadShip(_craftFileToSpawn);
                if (shipConstruct == null)
                {
                    OrXLog.instance.DebugLog("[Spawn OrX HoloKron] ShipConstruct was null when tried to load '" + _craftFileToSpawn +
                      "' (usually this means the file could not be found).");
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Loaded ================== ");

                ShipConstruction.ShipConfig = currentShip;
                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                uint launchID = HighLogic.CurrentGame.launchID++;
                foreach (Part p in shipConstruct.parts)
                {
                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                    p.missionID = missionID;
                    p.launchID = launchID;
                    if (_cavalry && !_enemyAirSupport)
                    {
                        p.flagURL = HighLogic.CurrentGame.flagURL;
                    }
                    else
                    {
                        p.flagURL = flagURL;
                    }
                    p.temperature = 1.0;
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct Parts Configured ================== ");

                if (_addCrew)
                {
                    List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                    while (part.MoveNext())
                    {
                        if (part.Current != null)
                        {
                            if (_eva)
                            {
                                if (part.Current.Modules.Contains<KerbalEVA>())
                                {
                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                      ? ProtoCrewMember.Gender.Female
                                      : ProtoCrewMember.Gender.Male;
                                    crewMember.trait = "Pilot";
                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                }
                            }

                            if (part.Current.Modules.Contains<KerbalSeat>())
                            {
                                if (_eva)
                                {
                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                      ? ProtoCrewMember.Gender.Female
                                      : ProtoCrewMember.Gender.Male;
                                    crewMember.trait = "Pilot";
                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                }
                            }
                            else
                            {
                                if (part.Current.Modules.Contains<ModuleCommand>() && part.Current.protoModuleCrew.Capacity >= 0)
                                {
                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                      ? ProtoCrewMember.Gender.Female
                                      : ProtoCrewMember.Gender.Male;
                                    crewMember.trait = "Pilot";
                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                }
                            }
                        }
                    }
                    part.Dispose();
                }

                ConfigNode _tempC = new ConfigNode();
                ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                Vessel _tempV = new Vessel();
                _tempV.parts = shipConstruct.parts;
                _tempP.vesselRef = _tempV;

                foreach (Part p in shipConstruct.parts)
                {
                    _tempV.loaded = false;
                    p.vessel = _tempV;
                    _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct protoPartSnapshots Added ================== ");

                foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                {
                    p.storePartRefs();
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct storePartRefs Stored ================== ");


                List<ConfigNode> pNodesL = new List<ConfigNode>();
                foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                {
                    ConfigNode node = new ConfigNode("PART");
                    snapShot.Save(node);
                    pNodesL.Add(node);
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct storePartRefs Stored ================== ");

                pNodes = pNodesL.ToArray();
            }
            else
            {
                uint flightId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                pNodes = new ConfigNode[1];
                pNodes[0] = ProtoVessel.CreatePartNode("EMPTY", flightId, null);
            }

            if (string.IsNullOrEmpty(_name) || _eva)
            {
                _name = "THX " + new System.Random().Next(1000, 9999);
            }

            OrXLog.instance.DebugLog("[Spawn OrX Craft File] CREATING ADDITIONAL NODES FOR " + _name);

            ConfigNode[] additionalNodes = new ConfigNode[0];
            ConfigNode protoNode = ProtoVessel.CreateVesselNode(_name, VesselType.Ship, orbit, 0, pNodes, additionalNodes);

            Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);

            double terrainHeight = 0.0;
            if (FlightGlobals.currentMainBody.pqsController != null)
            {
                terrainHeight = FlightGlobals.currentMainBody.pqsController.GetSurfaceHeight(norm) - FlightGlobals.currentMainBody.pqsController.radius;
                if (terrainHeight <= FlightGlobals.currentMainBody.pqsController.radius)
                {
                    var tHeight = FlightGlobals.currentMainBody.pqsController.radius - terrainHeight;
                    terrainHeight += tHeight;
                }
            }
            bool splashed = false;

            protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
              Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
            protoNode.SetValue("landed", (landed && !splashed).ToString());
            protoNode.SetValue("splashed", splashed.ToString());
            protoNode.SetValue("lat", gpsPos.x.ToString());
            protoNode.SetValue("lon", gpsPos.y.ToString());
            protoNode.SetValue("alt", gpsPos.z.ToString());
            protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Figure out the surface height and rotation for " + _name);
            Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
            Quaternion rotation = Quaternion.identity;
            float heading = 0;
            if (shipConstruct == null)
            {
                rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
            }
            else if (shipConstruct.shipFacility == EditorFacility.SPH)
            {
                rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                heading += 180.0f;
            }
            else
            {
                rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;
            }

            rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

            protoNode.SetValue("hgt", "0", true);
            protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
            Vector3 nrm = (rotation * Vector3.forward);
            protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
            protoNode.SetValue("prst", false.ToString(), true);
            ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

            Vessel craftFromFile = protoCraft.vesselRef;
            OrXLog.instance.SetRange(craftFromFile, 10000);

            OrXLog.instance.DebugLog("[Spawn OrX Craft File] VESSEL RANGES SET FOR " + _name);

            foreach (Part p in FindObjectsOfType<Part>())
            {
                if (!p.vessel)
                {
                    if (_eva)
                    {
                        Destroy(p.gameObject);
                    }
                    else
                    {
                        p.gameObject.SetActive(false);
                        OrXGameobjectTrash.instance._objectsToDestroy.Add(p.gameObject);
                        //OrXLog.instance.DebugLog("[Spawn OrX Craft File] THROWING " + p.gameObject.name + " IN THE TRASH .......");
                    }
                }
            }
            OrXGameobjectTrash.instance.EmptyTrashBin();

            if (_enemyAirSupport)
            {
                ConfigNode _enemyFile = new ConfigNode();
                _enemyFile.Save(_craftFileToSpawn);
            }

            craftFromFile.IgnoreGForces(240);
            craftFromFile.isPersistent = true;
            craftFromFile.Landed = false;
            craftFromFile.situation = Vessel.Situations.FLYING;
            while (craftFromFile.packed)
            {
                yield return null;
            }
            craftFromFile.SetWorldVelocity(Vector3d.zero);
            craftFromFile.IgnoreGForces(240);
            craftFromFile.GoOffRails();
            OrXHoloKron.instance._spawnedCraft = craftFromFile;
            StageManager.BeginFlight();
            craftFromFile.IgnoreGForces(240);
            craftFromFile.angularVelocity = Vector3.zero;
            craftFromFile.angularMomentum = Vector3.zero;
            craftFromFile.SetWorldVelocity(Vector3d.zero);

            if (!_eva)
            {
                UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
                EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
                NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
                spawning = false;

                if (_cavalry)
                {
                    _pitch = Vector3.Angle(FlightGlobals.ActiveVessel.ReferenceTransform.forward, UpVect);
                    _left = Vector3.Angle(-FlightGlobals.ActiveVessel.ReferenceTransform.right, NorthVect); // left is 90 degrees behind vessel heading

                    if (_enemyAirSupport)
                    {
                        craftFromFile.ActionGroups.SetGroup(KSPActionGroup.Gear, true);
                        yield return new WaitForFixedUpdate();
                        craftFromFile.ActionGroups.SetGroup(KSPActionGroup.Gear, false);
                        List<Part>.Enumerator parts = craftFromFile.parts.GetEnumerator();
                        while (parts.MoveNext())
                        {
                            if (parts.Current != null)
                            {
                                if (parts.Current.Modules.Contains<ModuleOrXWMI>())
                                {
                                    parts.Current.SendMessage("SwitchToEnemy");
                                }
                            }
                        }
                        parts.Dispose();
                        yield return new WaitForFixedUpdate();
                        _pitch = 90;
                        Vector3 _playerDir = (FlightGlobals.ActiveVessel.ReferenceTransform.position - craftFromFile.ReferenceTransform.position).normalized;
                        _left = Vector3.Angle(-craftFromFile.ReferenceTransform.right, _playerDir); // left is 90 degrees behind vessel heading
                        OrXHoloKron.instance.OnScrnMsgUC(craftFromFile.vesselName + " incoming");
                        OrXVesselLog.instance.AddToEnemyVesselList(craftFromFile);
                        craftFromFile.rootPart.AddModule("ModuleOrXPlace", true);
                        var _place = craftFromFile.rootPart.FindModuleImplementing<ModuleOrXPlace>();
                        _place.PlaceCraft(true, true, false, false, false, 0, _left, _pitch, OrXTargetDistance.instance._wmActivateDelay);
                        yield return new WaitForFixedUpdate();
                        yield return new WaitForFixedUpdate();
                        FlightGlobals.ForceSetActiveVessel(craftFromFile);
                        yield return new WaitForSecondsRealtime(1.5f);

                        if (_doubleMint)
                        {
                            SpawnRandomAirSupport(_delay);
                        }
                        else
                        {
                            spawning = false;
                            yield return new WaitForSecondsRealtime(2);
                            FlightGlobals.ForceSetActiveVessel(trigger);
                            yield return new WaitForFixedUpdate();
                            OrXLog.instance.ResetFocusKeys();
                            OrXVesselLog.instance.CheckEnemies(true);
                        }
                    }
                    else
                    {
                        craftFromFile.ActionGroups.SetGroup(KSPActionGroup.Gear, true);
                        OrXVesselLog.instance.AddToPlayerVesselList(craftFromFile);
                        OrXLog.instance.DebugLog("[OrX Spawn Air Support] === Adding " + craftFromFile.vesselName + " to player vessel list === ");

                        yield return new WaitForFixedUpdate();
                        craftFromFile.rootPart.AddModule("ModuleOrXPlace", true);
                        var _place = craftFromFile.rootPart.FindModuleImplementing<ModuleOrXPlace>();
                        _place.PlaceCraft(true, true, false, false, true, 0, _left, _pitch, 0);
                        craftFromFile.ActionGroups.SetGroup(KSPActionGroup.Gear, false);
                        spawning = false;
                    }
                }
                else
                {
                    craftFromFile.IgnoreGForces(500);
                    craftFromFile.ActionGroups.SetGroup(KSPActionGroup.Gear, false);
                    if (_craftFileToSpawn == GoalPostCraft)
                    {
                        Quaternion _fixRot = Quaternion.identity;
                        craftFromFile.IgnoreGForces(240);
                        craftFromFile.angularVelocity = Vector3.zero;
                        craftFromFile.angularMomentum = Vector3.zero;
                        craftFromFile.SetWorldVelocity(Vector3d.zero);
                        Vector3 _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)FlightGlobals.ActiveVessel.latitude, (double)FlightGlobals.ActiveVessel.longitude, (double)craftFromFile.altitude);
                        Vector3 _goalPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)craftFromFile.latitude, (double)craftFromFile.longitude, (double)craftFromFile.altitude);
                        Vector3 startPosDirection = (_goalPos - _startPos).normalized;
                        _fixRot = Quaternion.FromToRotation(craftFromFile.transform.up, startPosDirection) * craftFromFile.ReferenceTransform.rotation;
                        craftFromFile.SetRotation(_fixRot, true);

                        if (!craftFromFile.rootPart.Modules.Contains<ModuleOrXStage>())
                        {
                            craftFromFile.rootPart.AddModule("ModuleOrXStage", true);
                        }
                        var _orxStage = craftFromFile.rootPart.FindModuleImplementing<ModuleOrXStage>();
                        _orxStage._stageCount = (int)_delay;
                        yield return new WaitForFixedUpdate();
                        craftFromFile.IgnoreGForces(240);
                        craftFromFile.angularVelocity = Vector3.zero;
                        craftFromFile.angularMomentum = Vector3.zero;
                        craftFromFile.SetWorldVelocity(Vector3d.zero);

                        if (OrXHoloKron.instance.challengeRunning)
                        {
                            craftFromFile.rootPart.AddModule("ModuleOrXPlace", true);
                            var _place = craftFromFile.rootPart.FindModuleImplementing<ModuleOrXPlace>();
                            _place.PlaceCraft(false, false, false, true, false, 0, 0, 0, 0);
                        }
                    }
                    else
                    {
                        OrXVesselMove.Instance.StartMove(craftFromFile, false, (float)_altToAdd, false, false, new Vector3d());
                        spawning = false;
                    }
                }
            }
            else
            {
                var kerb = craftFromFile.rootPart.FindModuleImplementing<KerbalEVA>();
                if (kerb != null)
                {
                    //kerb.vessel.checkLanded();
                    //kerb.Awake();
                    //kerb.OnStart(kerb.part.GetModuleStartState());
                }
                craftFromFile.GetComponent<Rigidbody>().velocity = UnityEngine.Random.onUnitSphere * 25;
                OrXVesselLog.instance.AddToEnemyVesselList(craftFromFile);
                spawning = false;

            }
        }
        IEnumerator SpawnHoloKron(Vector3d stageStartCoords, Vector3d vect, bool b, bool empty, bool primary, string HoloKronName, string challengeType)
        {
            int count = OrXHoloKron.instance.locCount + 1;
            yield return new WaitForFixedUpdate();
            OrXSounds.instance.sound_SpawnOrXWhatsThat.Play();
            if (OrXHoloKron.instance.bdaChallenge && !OrXHoloKron.instance.buildingMission)
            {
                OrXHoloKron.instance.SetBDAc();
            }
            string holoFileLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloKron/HoloKron.craft";
            _lat = vect.x;
            _lon = vect.y;
            _alt = vect.z;
            bool spawnGate = false;
            //yield return new WaitForFixedUpdate();

            if (primary)
            {
                _alt += 10;

                if (OrXHoloKron.instance.bdaChallenge)
                {
                    _alt += 60;
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Spawning " + HoloKronName + " " + OrXHoloKron.instance.hkCount);
            }
            else
            {
                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Spawning Boid for " + HoloKronName + " " + OrXHoloKron.instance.hkCount);

                if (OrXHoloKron.instance.buildingMission)
                {
                    holoFileLoc = GoalPostCraft;
                    empty = false;
                    spawnGate = true;
                    b = true;
                    _alt += 15;
                }
                else
                {
                    empty = false;
                }
                stageCount += 1;
            }

            Vector3d tpoint;

            if (empty)
            {
                //_alt += 4;
                tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt)
                    + FlightGlobals.ActiveVessel.transform.forward * 3f;
            }
            else
            {
                if (spawnGate)
                {
                    //_alt += 4;
                }
                //_alt += 5;
                tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt);
            }

            Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Altitude: " + gpsPos.z);

            bool landed = false;
            Orbit orbit = null;

            if (!landed)
            {
                landed = true;
                Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Calculating Orbit ================== ");

                orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
            }
            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Orbit Calculated ================== ");

            ConfigNode[] pNodes;
            ShipConstruct shipConstruct = null;
            if (!string.IsNullOrEmpty(holoFileLoc))
            {
                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Loading Ship ================== ");

                ConfigNode currentShip = ShipConstruction.ShipConfig;
                shipConstruct = ShipConstruction.LoadShip(holoFileLoc);
                if (shipConstruct == null)
                {
                    OrXLog.instance.DebugLog("[Spawn OrX HoloKron] ShipConstruct was null when tried to load '" + holoFileLoc +
                      "' (usually this means the file could not be found).");
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Loaded ================== ");

                ShipConstruction.ShipConfig = currentShip;
                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                uint launchID = HighLogic.CurrentGame.launchID++;
                foreach (Part p in shipConstruct.parts)
                {
                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                    p.missionID = missionID;
                    p.launchID = launchID;
                    p.flagURL = flagURL;
                    p.temperature = 1.0;
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct Parts Configured ================== ");

                if (spawnGate)
                {
                    List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                    while (part.MoveNext())
                    {
                        if (part.Current != null)
                        {
                            if (part.Current.Modules.Contains<KerbalSeat>())
                            {
                                ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                  ? ProtoCrewMember.Gender.Female
                                  : ProtoCrewMember.Gender.Male;
                                crewMember.trait = "Tourist";
                                part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                            }
                        }
                    }
                    part.Dispose();
                }

                ConfigNode _tempC = new ConfigNode();
                ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                Vessel _tempV = new Vessel();
                _tempV.parts = shipConstruct.parts;
                _tempP.vesselRef = _tempV;

                foreach (Part p in shipConstruct.parts)
                {
                    _tempV.loaded = false;
                    p.vessel = _tempV;
                    _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct protoPartSnapshots Added ================== ");

                foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                {
                    p.storePartRefs();
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct storePartRefs Stored ================== ");


                List<ConfigNode> pNodesL = new List<ConfigNode>();
                foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                {
                    ConfigNode node = new ConfigNode("PART");
                    snapShot.Save(node);
                    pNodesL.Add(node);
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct storePartRefs Stored ================== ");

                pNodes = pNodesL.ToArray();
            }
            else
            {
                uint flightId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                pNodes = new ConfigNode[1];
                pNodes[0] = ProtoVessel.CreatePartNode("EMPTY", flightId, null);

                if (string.IsNullOrEmpty(HoloKronName) && !b)
                {
                    HoloKronName = "EMPTY HOLOKRON";
                }
            }

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] CREATING ADDITIONAL NODES FOR " + HoloKronName + " " + OrXHoloKron.instance.hkCount);

            ConfigNode[] additionalNodes = new ConfigNode[0];
            ConfigNode protoNode = ProtoVessel.CreateVesselNode(HoloKronName, VesselType.Base, orbit, 0, pNodes, additionalNodes);

            Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);

            double terrainHeight = 0.0;
            if (FlightGlobals.currentMainBody.pqsController != null)
            {
                terrainHeight = FlightGlobals.currentMainBody.pqsController.GetSurfaceHeight(norm) - FlightGlobals.currentMainBody.pqsController.radius;
                if (terrainHeight <= FlightGlobals.currentMainBody.pqsController.radius)
                {
                    var tHeight = FlightGlobals.currentMainBody.pqsController.radius - terrainHeight;
                    terrainHeight += tHeight;
                }
            }
            bool splashed = false;

            protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
              Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
            protoNode.SetValue("landed", (landed && !splashed).ToString());
            protoNode.SetValue("splashed", splashed.ToString());
            protoNode.SetValue("lat", gpsPos.x.ToString());
            protoNode.SetValue("lon", gpsPos.y.ToString());
            protoNode.SetValue("alt", gpsPos.z.ToString());
            protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Figure out the surface height and rotation for " + HoloKronName + " " + OrXHoloKron.instance.hkCount);
            Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
            Quaternion rotation = Quaternion.identity;
            float heading = 0;
            if (shipConstruct == null)
            {
                rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
            }
            else if (shipConstruct.shipFacility == EditorFacility.SPH)
            {
                rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                heading += 180.0f;
            }
            else
            {
                rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;
            }

            rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

            protoNode.SetValue("hgt", "0", true);
            protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
            Vector3 nrm = (rotation * Vector3.forward);
            protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
            protoNode.SetValue("prst", false.ToString(), true);
            ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

            Vessel holoCube = protoCraft.vesselRef;
            OrXLog.instance.SetRange(holoCube, 10000);
            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] VESSEL RANGES SET FOR " + HoloKronName);

            foreach (Part p in FindObjectsOfType<Part>())
            {
                if (!p.vessel)
                {
                    p.gameObject.SetActive(false);
                    OrXGameobjectTrash.instance._objectsToDestroy.Add(p.gameObject);
                    OrXLog.instance.DebugLog("[Spawn OrX Craft File] THROWING " + p.gameObject.name + " IN THE TRASH .......");

                    // Destroy(p.gameObject);
                }
            }
            yield return new WaitForFixedUpdate();
            OrXGameobjectTrash.instance.EmptyTrashBin();

            holoCube.IgnoreGForces(240);
            holoCube.isPersistent = true;
            holoCube.Landed = false;
            holoCube.situation = Vessel.Situations.FLYING;
            while (holoCube.packed)
            {
                yield return null;
            }
            holoCube.SetWorldVelocity(Vector3d.zero);
            //yield return null;
            holoCube.IgnoreGForces(240);
            var mom = holoCube.rootPart.FindModuleImplementing<ModuleOrXMission>();

            if (holoCube.parts.Count == 1 && !spawnGate)
            {
                if (mom == null)
                {
                    holoCube.rootPart.AddModule("ModuleOrXMission", true);
                    mom = holoCube.rootPart.FindModuleImplementing<ModuleOrXMission>();
                }
                mom.HoloKronName = HoloKronName;
                mom.creator = OrXHoloKron.instance.creatorName;
                mom.missionType = OrXHoloKron.instance.missionType;
                mom.challengeType = OrXHoloKron.instance.challengeType;
                mom.raceType = OrXHoloKron.instance.raceType;
                mom.latitude = _lat;
                mom.longitude = _lon;
                mom.altitude = _alt;
                mom.pos = tpoint;
                _holoAlt = _alt;
            }
            else
            {
                if (mom != null)
                {
                    Destroy(mom);
                }

                if (spawnGate)
                {
                    if (!holoCube.rootPart.Modules.Contains<ModuleOrXStage>())
                    {
                        holoCube.rootPart.AddModule("ModuleOrXStage", true);
                    }
                    var _orxStage = holoCube.rootPart.FindModuleImplementing<ModuleOrXStage>();
                    _orxStage._stageCount = count;
                }
            }

            holoCube.GoOffRails();
            holoCube.IgnoreGForces(240);
            StageManager.BeginFlight();

            if (!b)
            {
                holoCube.IgnoreGForces(240);

                if (!empty)
                {
                    spawning = false;
                    OrXHoloKron.instance.checking = true;
                    if (!OrXHoloKron.instance._showTimer)
                    {
                        OrXHoloKron.instance.movingCraft = false;
                        OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                    }

                    if (!primary)
                    {
                        mom.Goal = true;
                        mom.stage = count;
                        OrXHoloKron.instance.targetCoord = holoCube;
                    }
                    else
                    {
                        if (OrXHoloKron.instance.missionType == "GEO-CACHE")
                        {
                            OrXHoloKron.instance.challengeRunning = false;
                            StartCoroutine(SpawnLocalVessels(false, HoloKronName, vect, 0, 0));
                        }
                        else
                        {
                            if (OrXHoloKron.instance.challengeType == "BD ARMORY")
                            {
                                mom.fml = true;
                                mom.Goal = true;
                                OrXHoloKron.instance.GetShortTrackCenter(OrXHoloKron.instance._challengeStartLoc);
                                OrXHoloKron.instance._HoloKron = holoCube;

                                //StartCoroutine(SpawnLocalVessels(true, HoloKronName, vect));
                            }
                            else
                            {
                                mom._auto = true;
                                mom.fml = false;
                                mom.Goal = true;
                                OrXHoloKron.instance.GetShortTrackCenter(OrXHoloKron.instance._challengeStartLoc);
                                OrXHoloKron.instance._HoloKron = holoCube;
                            }
                        }
                    }
                }
                else
                {
                    spawning = false;
                    OrXHoloKron.instance.GetShortTrackCenter(OrXHoloKron.instance._challengeStartLoc);
                    OrXHoloKron.instance._HoloKron = holoCube;
                    OrXHoloKron.instance.PlayOrXMission = false;
                    OrXHoloKron.instance.movingCraft = false;
                    OrXVesselMove.Instance.StartMove(holoCube, false, 0, false, false, new Vector3d());
                }
            }
            else
            {
                if (holoCube.rootPart.Modules.Contains<ModuleOrXMission>())
                {
                    mom.Goal = true;
                    mom.stage = stageCount;
                    mom.isLoaded = true;
                    OrXHoloKron.instance.targetCoord = holoCube;
                }

                if (OrXHoloKron.instance.buildingMission && !holoCube.rootPart.Modules.Contains<ModuleOrXMission>())
                {
                    holoCube.vesselName = OrXHoloKron.instance.HoloKronName + " " + OrXHoloKron.instance.hkCount + " - STAGE " + (OrXHoloKron.instance.locCount + 1);
                    OrXLog.instance.DebugLog("[OrX Spawn OrX HoloKron] === FIXING ROTATION FOR " + holoCube.vesselName + " ..................");
                    Quaternion _fixRot = Quaternion.identity;
                    holoCube.IgnoreGForces(240);
                    holoCube.angularVelocity = Vector3.zero;
                    holoCube.angularMomentum = Vector3.zero;
                    holoCube.SetWorldVelocity(Vector3d.zero);
                    Vector3 _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)OrXHoloKron.instance.lastCoord.x, (double)OrXHoloKron.instance.lastCoord.y, (double)holoCube.altitude);
                    Vector3 _goalPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)holoCube.latitude, (double)holoCube.longitude, (double)holoCube.altitude);
                    Vector3 startPosDirection = (_goalPos - _startPos).normalized;
                    _fixRot = Quaternion.FromToRotation(holoCube.transform.up, startPosDirection) * holoCube.ReferenceTransform.rotation;
                    holoCube.SetRotation(_fixRot, true);
                    OrXHoloKron.instance.getNextCoord = true;
                    OrXHoloKron.instance.spawningStartGate = true;
                    OrXHoloKron.instance._lastStage = holoCube;
                    if (OrXHoloKron.instance.dakarRacing) // && OrXHoloKron.instance.locCount == 0)
                    {
                        OrXHoloKron.instance.GetShortTrackCenter(new Vector3d(holoCube.latitude, holoCube.longitude, holoCube.altitude));
                    }

                    OrXVesselMove.Instance.StartMove(holoCube, false, 0, true, false, new Vector3d());
                    spawning = false;
                }
                else
                {
                    OrXHoloKron.instance.checking = true;
                    if (!OrXHoloKron.instance._showTimer)
                    {
                        OrXHoloKron.instance.movingCraft = false;
                        OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                    }
                }
            }

            if (holoCube.rootPart.Modules.Contains<ModuleOrXMission>())
            {
                mom.isLoaded = true;
            }

            spawning = false;
            //OrXHoloKron.instance.challengeRunning = false;
            //OrXHoloKron.instance.GuiEnabledOrXMissions = true;

        }
        IEnumerator SpawnLBCRoutine(int _stage, string HoloKronName)
        {
            string missionCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp";
            string pas = "";
            string _orxFileLoc = "";
            float _altToSubtract = 0;
            UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            ConfigNode _file = new ConfigNode();
            string _orxFile = "";

            string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            List<string> files = new List<string>(Directory.GetFiles(importLoc, "*.orx", SearchOption.AllDirectories));
            if (files != null)
            {
                List<string>.Enumerator orxFile = files.GetEnumerator();
                while (orxFile.MoveNext())
                {
                    if (orxFile.Current != null)
                    {
                        if (orxFile.Current.Contains(HoloKronName + "-" + OrXHoloKron.instance.hkCount + "-" + OrXHoloKron.instance.creatorName))
                        {
                            _orxFileLoc = orxFile.Current;
                            _file = ConfigNode.Load(orxFile.Current);
                            break;
                        }
                    }
                }
                orxFile.Dispose();
            }

            if (_file != null)
            {
                OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === Spawning LBC Vessels === ");

                if (HoloKronName != "")
                {
                    int _vesselCount = 1;
                    int _hkCount = 0;
                    ConfigNode node = _file.GetNode("boids");

                    foreach (ConfigNode _vts in node.nodes)
                    {
                        if (_vts.name.Contains("STAGE" + _stage + "-OrXv"))
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === GRABBING CRAFT FILE FOR " + _vts.name + " ===");
                            _vesselCount += 1;
                            float _left = 0;
                            float _pitch = 0;
                            double _al = 0;
                            double _la = 0;
                            double _lo = 0;
                            int _serial = 1138;
                            bool _airborne = false;
                            bool _splashed = false;

                            _vesselName = _vts.GetValue("vesselName");

                            ConfigNode location = _vts.GetNode("coords");

                            foreach (ConfigNode.Value loc in location.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                loc.name = cvEncryptedName;
                                loc.value = cvEncryptedValue;
                                if (loc.name == "airborne")
                                {
                                    if (loc.value == "True")
                                    {
                                        //_airborne = true;
                                    }
                                }
                                if (loc.name == "splashed")
                                {
                                    if (loc.value == "True")
                                    {
                                        //_splashed = true;
                                    }
                                }

                                if (loc.name == "vesselName")
                                {
                                    _vesselName = loc.value;
                                }
                                if (loc.name == "lat")
                                {
                                    _la = double.Parse(loc.value);
                                }
                                if (loc.name == "lon")
                                {
                                    _lo = double.Parse(loc.value);
                                }
                                if (loc.name == "alt")
                                {
                                    _al = double.Parse(loc.value);
                                    _al += 50;
                                }
                                if (loc.name == "left")
                                {
                                    _left = float.Parse(loc.value);
                                }
                                if (loc.name == "pitch")
                                {
                                    _pitch = float.Parse(loc.value);
                                }
                                if (loc.name == "pas")
                                {
                                    pas = loc.value;
                                }
                            }

                            if (!_airborne)
                            {
                                OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === VESSEL SPAWN COORDS READY ===");

                                OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === DECRYPTING CRAFT FILE DATA FOR " + _vts.name + " ===");
                                ConfigNode craftFile = _vts.GetNode("craft");

                                foreach (ConfigNode.Value cv in craftFile.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                    cv.name = cvEncryptedName;
                                    cv.value = cvEncryptedValue;

                                    if (cv.name == "ship")
                                    {
                                        cv.value = _vesselName;
                                    }

                                    if (cv.name == "size")
                                    {
                                        string[] _sizeString = cv.value.Split(new char[] { ',' });

                                        try
                                        {
                                            if (_sizeString[0] != null && _sizeString[0].Length > 0 && _sizeString[0] != "null")
                                            {
                                                for (int i = 0; i < _sizeString.Length; i++)
                                                {
                                                    if (_sizeString[i] != null && _sizeString[i].Length > 0)
                                                    {
                                                        float _maxValue = Math.Max(float.Parse(_sizeString[0]), Math.Max(float.Parse(_sizeString[1]), float.Parse(_sizeString[2])));
                                                        _altToSubtract = _maxValue / 2;
                                                    }
                                                }
                                            }
                                        }
                                        catch (IndexOutOfRangeException e)
                                        {
                                            OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] Altitude calculated ...... ");
                                        }
                                    }
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
                                            if (!cv2.name.Contains("(") && !cv2.name.Contains(")"))
                                            {
                                                string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                                cv2.name = cvEncryptedName;
                                                cv2.value = cvEncryptedValue;
                                            }
                                        }
                                    }
                                }
                                yield return new WaitForFixedUpdate();

                                craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                                yield return new WaitForFixedUpdate();

                                OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === VESSEL DECRYPTED - CHECKING MODULES ===");
                                List<string> partsLoaded = new List<string>();
                                int partCount = 0;
                                bool okToLoad = true;
                                bool hasKerbal = false;
                                spawningGoal = false;

                                foreach (ConfigNode partCheck in craftFile.nodes)
                                {
                                    if (node.name == "PART")
                                    {
                                        foreach (ConfigNode.Value partCheck2 in partCheck.values)
                                        {
                                            if (partCheck2.name == "name")
                                            {
                                                if (partCheck2.value.Contains("kerbalEVA"))
                                                {
                                                    hasKerbal = true;
                                                }

                                                if (partCheck2.value.Contains("ModuleOrXStage"))
                                                {
                                                    spawningGoal = true;
                                                }
                                                /*
                                                if (!partsLoaded.Contains(partCheck2.value))
                                                {
                                                    partCount += 1;
                                                    partsLoaded.Add(partCheck2.value);
                                                }
                                                */
                                            }
                                        }
                                    }
                                }

                                /*
                                List<AvailablePart>.Enumerator availablePart = PartLoader.LoadedPartsList.GetEnumerator();
                                while (availablePart.MoveNext())
                                {
                                    try
                                    {
                                        if (availablePart.Current != null)
                                        {
                                            if (partsLoaded.Contains(availablePart.Current.name))
                                            {
                                                partCount -= 1;
                                                partsLoaded.Remove(availablePart.Current.name);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + e + " ===");
                                    }
                                }
                                availablePart.Dispose();

                                if (partCount >= 0)
                                {
                                    foreach (string s in partsLoaded)
                                    {
                                        okToLoad = false;
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + _vts.name + " CONTAINS UNRECOGNIZED PARTS ... SKIPPING ===");
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + s + " UNRECOGNIZED ===");
                                    }
                                }

                                */

                                if (okToLoad)
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === " + _vesselName + " READY FOR SPAWNING ===");
                                    //yield return new WaitForFixedUpdate();

                                    VesselType vt;

                                    if (spawningGoal)
                                    {
                                        vt = VesselType.Debris;
                                    }
                                    else
                                    {
                                        vt = VesselType.Ship;
                                    }

                                    //craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                                    yield return new WaitForFixedUpdate();

                                    Vector3d tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_la, (double)_lo, (double)_al + (_altToSubtract * 3));
                                    Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);
                                    Orbit orbit = null;

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] Altitude: " + gpsPos.z);

                                    bool landed = false;
                                    if (!landed)
                                    {
                                        landed = true;

                                        Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                                        orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                                        orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
                                    }

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] Orbit Data Processed");
                                    yield return new WaitForFixedUpdate();

                                    ConfigNode[] partNodes;
                                    ShipConstruct shipConstruct = null;

                                    ConfigNode currentShip = ShipConstruction.ShipConfig;
                                    shipConstruct = ShipConstruction.LoadShip("GameData/OrX/Plugin/PluginData/spawn.tmp");
                                    ShipConstruction.ShipConfig = currentShip;
                                    uint missionID = (uint)Guid.NewGuid().GetHashCode();
                                    uint launchID = HighLogic.CurrentGame.launchID++;

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] Ship construct created");

                                    foreach (Part p in shipConstruct.parts)
                                    {
                                        p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                                        p.missionID = missionID;
                                        p.launchID = launchID;
                                        p.flagURL = flagURL;
                                        p.temperature = 1.0;
                                    }
                                    //yield return new WaitForFixedUpdate();

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] Part flight ID's processed");
                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] Constructing protoCraft");

                                    bool hasSeat = false;

                                    List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                                    while (part.MoveNext())
                                    {
                                        if (part.Current != null)
                                        {
                                            if (part.Current.Modules.Contains<KerbalSeat>())
                                            {
                                                hasSeat = true;
                                                /*
                                                ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                  ? ProtoCrewMember.Gender.Female
                                                  : ProtoCrewMember.Gender.Male;
                                                _serial = new System.Random().Next(1000, 9999);
                                                crewMember.trait = "Pilot";
                                                //crewMember.KerbalRef.crewMemberName = "TK - " + _serial;
                                                part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
                                                */
                                            }
                                            else
                                            {
                                                if (part.Current.Modules.Contains<KerbalEVA>())
                                                {
                                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                      ? ProtoCrewMember.Gender.Female
                                                      : ProtoCrewMember.Gender.Male;
                                                    _serial = new System.Random().Next(1000, 9999);
                                                    crewMember.trait = "Tourist";
                                                    //HighLogic.CurrentGame.CrewRoster.ChangeName(crewMember.KerbalRef.crewMemberName, "TK - " + _serial);
                                                    //crewMember.KerbalRef.crewMemberName = "TK - " + _serial;
                                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                    //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                    //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));

                                                    if (!part.Current.Modules.Contains<ModuleOrX>())
                                                    {
                                                        part.Current.AddModule("ModuleOrX");
                                                    }
                                                }
                                                else
                                                {
                                                    if (part.Current.Modules.Contains<ModuleCommand>() && part.Current.protoModuleCrew.Count <= 1)
                                                    {
                                                        ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                        crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                          ? ProtoCrewMember.Gender.Female
                                                          : ProtoCrewMember.Gender.Male;
                                                        _serial = new System.Random().Next(1000, 9999);
                                                        crewMember.trait = "Pilot";
                                                        //HighLogic.CurrentGame.CrewRoster.ChangeName(crewMember.KerbalRef.crewMemberName, "TK - " + _serial);

                                                        //crewMember.KerbalRef.crewMemberName = "TK - " + _serial;
                                                        part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                        //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                        //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    part.Dispose();

                                    ConfigNode _tempC = new ConfigNode();
                                    ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                                    Vessel _tempV = new Vessel();
                                    _tempV.parts = shipConstruct.parts;
                                    _tempP.vesselRef = _tempV;

                                    foreach (Part p in shipConstruct.parts)
                                    {
                                        _tempV.loaded = false;
                                        p.vessel = _tempV;
                                        _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                                    }

                                    foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                                    {
                                        p.storePartRefs();
                                    }

                                    List<ConfigNode> partNodesL = new List<ConfigNode>();
                                    foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                                    {
                                        ConfigNode partNode = new ConfigNode("PART");
                                        snapShot.Save(partNode);
                                        partNodesL.Add(partNode);
                                    }
                                    partNodes = partNodesL.ToArray();

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] CREATING ADDITIONAL NODES FOR " + _vesselName);
                                    ConfigNode[] additionalNodes = new ConfigNode[0];
                                    ConfigNode protoNode = ProtoVessel.CreateVesselNode(_vesselName, vt, orbit, 0, partNodes, additionalNodes);
                                    bool splashed = false;
                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] FINDING NORTH FOR " + _vesselName);

                                    Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);
                                    splashed = false;
                                    protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                                      Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                                    protoNode.SetValue("landed", (landed && !splashed).ToString());
                                    protoNode.SetValue("splashed", splashed.ToString());
                                    protoNode.SetValue("lat", gpsPos.x.ToString());
                                    protoNode.SetValue("lon", gpsPos.y.ToString());
                                    protoNode.SetValue("alt", gpsPos.z.ToString());
                                    protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] protoCraft SET VALUES FOR " + _vesselName);

                                    Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
                                    Quaternion rotation = Quaternion.identity;
                                    float heading = 0;
                                    if (shipConstruct == null)
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
                                    }
                                    else if (shipConstruct.shipFacility == EditorFacility.SPH)
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                                        heading += 180.0f;
                                    }
                                    else
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                                        rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;
                                    }

                                    rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
                                    rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
                                    rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] INITIALIZING ROTATIONS FOR " + _vesselName);

                                    protoNode.SetValue("hgt", "0", true);
                                    protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
                                    Vector3 nrm = (rotation * Vector3.forward);
                                    protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
                                    protoNode.SetValue("prst", false.ToString(), true);

                                    //protoNode.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "Vessel" + _vesselCount + ".proto");
                                    /*
                                    foreach (ConfigNode cn in protoNode.nodes)
                                    {
                                        if (cn.name == "PART")
                                        {
                                            foreach (ConfigNode.Value cv in cn.values)
                                            {
                                                if (cv.name == "part")
                                                {
                                                    if (cv.value.Contains("kerbalEVA"))
                                                    {
                                                        string tkName = "";
                                                        string currentName = "";

                                                        foreach (ConfigNode cn2 in cn.nodes)
                                                        {
                                                            if (cn2.name == "MODULE")
                                                            {
                                                                foreach (ConfigNode.Value cv2 in cn2.values)
                                                                {
                                                                    if (cv2.name == "name")
                                                                    {
                                                                        if (cv2.value == "KerbalEVA")
                                                                        {
                                                                            //ConfigNode vInfo = cn2.GetNode("vInfo");
                                                                            //_serial = new System.Random().Next(1000, 9999);
                                                                            //tkName = "TK - " + _serial;
                                                                            //currentName = vInfo.GetValue("vesselName");
                                                                           // vInfo.SetValue("vesselName", "TK - " + _serial, true);

                                                                            //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                                            //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
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
                                    */
                                    ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

                                    foreach (Part p in protoCraft.vesselRef.parts)
                                    {
                                        //p.SetOpacity(0);
                                    }
                                    protoCraft.vesselRef.transform.rotation = protoCraft.rotation;

                                    Vessel localVessel = protoCraft.vesselRef;
                                    OrXLog.instance.SetRange(localVessel, 10000);

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] VESSEL RANGES SET FOR " + localVessel.vesselName);

                                    foreach (Part p in FindObjectsOfType<Part>())
                                    {
                                        if (!p.vessel)
                                        {
                                            /*
                                            foreach (PartModule pm in p.Modules)
                                            {
                                                //Destroy(pm);
                                            }
                                            //p.gameObject.SetActive(false);
                                            //OrXGameobjectTrash.instance._objectsToDestroy.Add(p.gameObject);
                                            //OrXLog.instance.DebugLog("[Spawn OrX Craft File] THROWING " + p.gameObject.name + " IN THE TRASH .......");
                                            */
                                            Destroy(p.gameObject);
                                        }
                                    }

                                    localVessel.isPersistent = true;
                                    localVessel.Landed = false;
                                    localVessel.situation = Vessel.Situations.PRELAUNCH;
                                    while (localVessel.packed)
                                    {
                                        yield return null;
                                    }

                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] " + _vesselName + " IS GOING OFF RAILS");
                                    ScreenMessages.PostScreenMessage(new ScreenMessage("Placing " + _vesselName, 1, ScreenMessageStyle.UPPER_CENTER));

                                    localVessel.SetWorldVelocity(Vector3d.zero);
                                    localVessel.GoOffRails();
                                    localVessel.IgnoreGForces(240);
                                    localVessel.angularVelocity = Vector3.zero;
                                    localVessel.angularMomentum = Vector3.zero;

                                    StageManager.BeginFlight();
                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === BEGINNING FLIGHT FOR " + _vesselName + " ===");
                                    localVessel.IgnoreGForces(240);
                                    localVessel.angularVelocity = Vector3.zero;
                                    localVessel.angularMomentum = Vector3.zero;

                                    localVessel.rootPart.AddModule("ModuleOrXPlace", true);
                                    var _place = localVessel.rootPart.FindModuleImplementing<ModuleOrXPlace>();
                                    _place.PlaceCraft(true, false, false, localVessel.rootPart.Modules.Contains<ModuleOrXStage>(), false, _altToSubtract, _left, _pitch, 0);
                                    OrXVesselLog.instance.AddToEnemyVesselList(localVessel);
                                    ConfigNode craft = ConfigNode.Load(missionCraftLoc);
                                    craft.ClearData();
                                    craft.Save(missionCraftLoc);
                                    OrXLog.instance.DebugLog("[OrX Spawn LBC Vessels] === " + _vesselName + " Spawned ===");
                                    yield return new WaitForFixedUpdate();

                                    List<Part>.Enumerator parts = localVessel.parts.GetEnumerator();
                                    while (parts.MoveNext())
                                    {
                                        if (parts.Current != null)
                                        {
                                            if (parts.Current.Modules.Contains<ModuleOrXWMI>())
                                            {
                                                parts.Current.SendMessage("SwitchToEnemy");
                                            }
                                        }
                                    }
                                    parts.Dispose();

                                }
                            }
                        }
                    }

                    spawning = false;
                    //OrXGameobjectTrash.instance.EmptyTrashBin();
                }
            }
        }
        IEnumerator SpawnLocalVessels(bool _bda, string HoloKronName, Vector3d vect, float _delay, int _count)
        {
            OrXHoloKron.instance.bdaChallenge = _bda;
            if (_bda)
            {
                OrXSounds.instance.BobDougSound();
                OrXLog.instance.SetFocusKeys();
            }

            string missionCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp";
            string pas = "";
            string _orxFileLoc = "";
            float _altToSubtract = 0;
            UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            ConfigNode _file = new ConfigNode();
            string _orxFile = "";

            string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            List<string> files = new List<string>(Directory.GetFiles(importLoc, "*.holo", SearchOption.AllDirectories));
            if (files != null)
            {
                List<string>.Enumerator orxFile = files.GetEnumerator();
                while(orxFile.MoveNext())
                {
                    if (orxFile.Current != null)
                    {
                        if (orxFile.Current.Contains(HoloKronName + "-" + OrXHoloKron.instance.hkCount + "-" + OrXHoloKron.instance.creatorName))
                        {
                            _orxFileLoc = orxFile.Current;
                            _file = ConfigNode.Load(orxFile.Current);
                            break;
                        }
                    }
                }
                orxFile.Dispose();
            }

            if (_file != null)
            {
                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === Spawning Local Vessels === ");

                if (HoloKronName != "")
                {
                    int _vesselCount = 1;
                    int _hkCount = 0;
                    ConfigNode node = _file.GetNode("OrX");

                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (spawnCheck.name.Contains("OrXHoloKronCoords" + _hkCount))
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === FOUND " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " ... DECRYPTING ===");

                            foreach (ConfigNode.Value data in spawnCheck.values)
                            {
                                if (data.name == "challengeType")
                                {
                                    if (data.value.Contains("OUTLAW RACING"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is OUTLAW RACING ===");

                                        OrXHoloKron.instance.outlawRacing = true;
                                    }
                                    
                                    if (data.value.Contains("BD ARMORY"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is BD ARMORY ===");

                                        OrXHoloKron.instance.bdaChallenge = true;
                                        OrXHoloKron.instance.outlawRacing = false;

                                    }
                                    
                                    if (data.value.Contains("GEO-CACHE"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ===  " + HoloKronName + " " + _hkCount + " " + OrXHoloKron.instance.creatorName + " is GEO-CACHE ===");

                                        OrXHoloKron.instance.geoCache = true;
                                    }
                                }

                                if (data.name == "raceType")
                                {
                                    if (data.value.Contains("SHORT TRACK"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is SHORT TRACK ===");

                                        OrXHoloKron.instance.shortTrackRacing = true;
                                    }

                                    if (data.value.Contains("DAKAR"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is DAKAR ===");
                                        OrXHoloKron.instance.dakarRacing = true;
                                    }
                                }

                                if (data.name == "spawned")
                                {
                                    if (data.value == "False")
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ===  " + HoloKronName + " " + _hkCount + " " + OrXHoloKron.instance.creatorName + " has not spawned ===");
                                        spawnCheck.SetValue("spawned", "True", true);
                                        _file.Save(_orxFileLoc);
                                        //break;
                                    }
                                    else
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + HoloKronName + " " + _hkCount + " " + OrXHoloKron.instance.creatorName + " has spawned ... CHECKING FOR EXTRAS");

                                        if (spawnCheck.HasValue("extras"))
                                        {
                                            if (spawnCheck.GetValue("extras") == "False")
                                            {
                                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + HoloKronName + " " + _hkCount + " " + OrXHoloKron.instance.creatorName + " has no extras ... END TRANSMISSION");
                                                //break;
                                            }
                                            else
                                            {
                                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + HoloKronName + " " + _hkCount + " " + OrXHoloKron.instance.creatorName + " has extras ... SEARCHING");
                                                _hkCount += 1;
                                            }
                                        }
                                    }
                                }
                            }

                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === DATA PROCESSED ===");
                        }
                    }

                    string _nodeName;

                    if (OrXHoloKron.instance.dakarRacing)
                    {
                        _nodeName = "STAGEGATE" + _count;
                    }
                    else
                    {
                        _nodeName = "HC" + _hkCount + "OrXv" + _vesselCount;
                    }

                    foreach (ConfigNode _vts in node.nodes)
                    {
                        if (_vts.name.Contains(_nodeName))
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === GRABBING CRAFT FILE FOR " + _vts.name + " ===");
                            _vesselCount += 1;
                            float _left = 0;
                            float _pitch = 0;
                            double _al = 0;
                            double _la = 0;
                            double _lo = 0;
                            int _serial = 1138;
                            bool _airborne = false;
                            bool _splashed = false;
                            _vesselName = _vts.GetValue("vesselName");

                            ConfigNode location = _vts.GetNode("coords");

                            foreach (ConfigNode.Value loc in location.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                loc.name = cvEncryptedName;
                                loc.value = cvEncryptedValue;
                                if (loc.name == "airborne")
                                {
                                    if (loc.value == "True")
                                    {
                                        _airborne = true;
                                    }
                                }
                                if (loc.name == "splashed")
                                {
                                    if (loc.value == "True")
                                    {
                                        _splashed = true;
                                    }
                                }

                                if (loc.name == "vesselName")
                                {
                                    _vesselName = loc.value;
                                }
                                if (loc.name == "lat")
                                {
                                    _la = double.Parse(loc.value);
                                }
                                if (loc.name == "lon")
                                {
                                    _lo = double.Parse(loc.value);
                                }
                                if (loc.name == "alt")
                                {
                                    _al = double.Parse(loc.value);
                                    _al += 5;
                                }
                                if (loc.name == "left")
                                {
                                    _left = float.Parse(loc.value);
                                }
                                if (loc.name == "pitch")
                                {
                                    _pitch = float.Parse(loc.value);
                                }
                                if (loc.name == "pas")
                                {
                                    pas = loc.value;
                                }
                            }

                            if (!_airborne && !_splashed)
                            {
                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === VESSEL SPAWN COORDS READY ===");

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === DECRYPTING CRAFT FILE DATA FOR " + _vts.name + " ===");
                                ConfigNode craftFile = _vts.GetNode("craft");

                                foreach (ConfigNode.Value cv in craftFile.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                    cv.name = cvEncryptedName;
                                    cv.value = cvEncryptedValue;

                                    if (cv.name == "ship")
                                    {
                                        cv.value = _vesselName;
                                    }

                                    if (cv.name == "size")
                                    {
                                        string[] _sizeString = cv.value.Split(new char[] { ',' });

                                        try
                                        {
                                            if (_sizeString[0] != null && _sizeString[0].Length > 0 && _sizeString[0] != "null")
                                            {
                                                for (int i = 0; i < _sizeString.Length; i++)
                                                {
                                                    if (_sizeString[i] != null && _sizeString[i].Length > 0)
                                                    {
                                                        float _maxValue = Math.Max(float.Parse(_sizeString[0]), Math.Max(float.Parse(_sizeString[1]), float.Parse(_sizeString[2])));
                                                        _altToSubtract = _maxValue / 2;
                                                    }
                                                }
                                            }
                                        }
                                        catch (IndexOutOfRangeException e)
                                        {
                                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Altitude calculated ...... ");
                                        }
                                    }
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
                                            if (!cv2.name.Contains("(") && !cv2.name.Contains(")"))
                                            {
                                                string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                                cv2.name = cvEncryptedName;
                                                cv2.value = cvEncryptedValue;
                                            }
                                        }
                                    }
                                }
                                yield return new WaitForFixedUpdate();

                                craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                                yield return new WaitForFixedUpdate();

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === VESSEL DECRYPTED - CHECKING MODULES ===");
                                List<string> partsLoaded = new List<string>();
                                int partCount = 0;
                                bool okToLoad = true;
                                bool hasKerbal = false;
                                spawningGoal = false;

                                foreach (ConfigNode partCheck in craftFile.nodes)
                                {
                                    if (node.name == "PART")
                                    {
                                        foreach (ConfigNode.Value partCheck2 in partCheck.values)
                                        {
                                            if (partCheck2.name == "name")
                                            {
                                                if (partCheck2.value.Contains("kerbalEVA"))
                                                {
                                                    hasKerbal = true;
                                                }

                                                if (partCheck2.value.Contains("ModuleOrXStage"))
                                                {
                                                    spawningGoal = true;
                                                }
                                                /*
                                                if (!partsLoaded.Contains(partCheck2.value))
                                                {
                                                    partCount += 1;
                                                    partsLoaded.Add(partCheck2.value);
                                                }
                                                */
                                            }
                                        }
                                    }
                                }

                                /*
                                List<AvailablePart>.Enumerator availablePart = PartLoader.LoadedPartsList.GetEnumerator();
                                while (availablePart.MoveNext())
                                {
                                    try
                                    {
                                        if (availablePart.Current != null)
                                        {
                                            if (partsLoaded.Contains(availablePart.Current.name))
                                            {
                                                partCount -= 1;
                                                partsLoaded.Remove(availablePart.Current.name);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + e + " ===");
                                    }
                                }
                                availablePart.Dispose();

                                if (partCount >= 0)
                                {
                                    foreach (string s in partsLoaded)
                                    {
                                        okToLoad = false;
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + _vts.name + " CONTAINS UNRECOGNIZED PARTS ... SKIPPING ===");
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + s + " UNRECOGNIZED ===");
                                    }
                                }

                                */

                                if (okToLoad || spawningGoal)
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + _vesselName + " READY FOR SPAWNING ===");
                                    //yield return new WaitForFixedUpdate();

                                    VesselType vt;

                                    if (spawningGoal)
                                    {
                                        vt = VesselType.Debris;
                                    }
                                    else
                                    {
                                        vt = VesselType.Ship;
                                    }

                                    //craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                                    yield return new WaitForFixedUpdate();

                                    Vector3d tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_la, (double)_lo, (double)_al + (_altToSubtract * 3));
                                    Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);
                                    Orbit orbit = null;

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Altitude: " + gpsPos.z);

                                    bool landed = false;
                                    if (!landed)
                                    {
                                        landed = true;

                                        Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                                        orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                                        orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
                                    }

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Orbit Data Processed");
                                    yield return new WaitForFixedUpdate();

                                    ConfigNode[] partNodes;
                                    ShipConstruct shipConstruct = null;

                                    ConfigNode currentShip = ShipConstruction.ShipConfig;
                                    shipConstruct = ShipConstruction.LoadShip("GameData/OrX/Plugin/PluginData/spawn.tmp");
                                    ShipConstruction.ShipConfig = currentShip;
                                    uint missionID = (uint)Guid.NewGuid().GetHashCode();
                                    uint launchID = HighLogic.CurrentGame.launchID++;

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Ship construct created");

                                    foreach (Part p in shipConstruct.parts)
                                    {
                                        p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                                        p.missionID = missionID;
                                        p.launchID = launchID;
                                        p.flagURL = flagURL;
                                        p.temperature = 1.0;
                                    }
                                    //yield return new WaitForFixedUpdate();

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Part flight ID's processed");
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Constructing protoCraft");

                                    bool hasSeat = false;

                                    List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                                    while (part.MoveNext())
                                    {
                                        if (part.Current != null)
                                        {
                                            if (part.Current.Modules.Contains<KerbalSeat>())
                                            {
                                                hasSeat = true;
                                                if (part.Current.children.Count == 0)
                                                {
                                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                      ? ProtoCrewMember.Gender.Female
                                                      : ProtoCrewMember.Gender.Male;
                                                    _serial = new System.Random().Next(1000, 9999);
                                                    crewMember.trait = "Pilot";
                                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                }
                                            }
                                            else
                                            {
                                                if (part.Current.Modules.Contains<KerbalEVA>())
                                                {
                                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                      ? ProtoCrewMember.Gender.Female
                                                      : ProtoCrewMember.Gender.Male;
                                                    _serial = new System.Random().Next(1000, 9999);
                                                    if (_bda)
                                                    {
                                                        crewMember.trait = "Pilot";
                                                    }
                                                    else
                                                    {
                                                        crewMember.trait = "Tourist";
                                                    }
                                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                }
                                                else
                                                {
                                                    if (part.Current.Modules.Contains<ModuleCommand>() && part.Current.protoModuleCrew.Count <= 1)
                                                    {
                                                        ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                        crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                          ? ProtoCrewMember.Gender.Female
                                                          : ProtoCrewMember.Gender.Male;
                                                        _serial = new System.Random().Next(1000, 9999);
                                                        if (_bda)
                                                        {
                                                            crewMember.trait = "Pilot";
                                                        }
                                                        else
                                                        {
                                                            crewMember.trait = "Tourist";
                                                        }
                                                        part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    part.Dispose();

                                    ConfigNode _tempC = new ConfigNode();
                                    ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                                    Vessel _tempV = new Vessel();
                                    _tempV.parts = shipConstruct.parts;
                                    _tempP.vesselRef = _tempV;

                                    foreach (Part p in shipConstruct.parts)
                                    {
                                        _tempV.loaded = false;
                                        p.vessel = _tempV;
                                        _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                                    }

                                    foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                                    {
                                        p.storePartRefs();
                                    }

                                    List<ConfigNode> partNodesL = new List<ConfigNode>();
                                    foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                                    {
                                        ConfigNode partNode = new ConfigNode("PART");
                                        snapShot.Save(partNode);
                                        partNodesL.Add(partNode);
                                    }
                                    partNodes = partNodesL.ToArray();

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] CREATING ADDITIONAL NODES FOR " + _vesselName);
                                    ConfigNode[] additionalNodes = new ConfigNode[0];
                                    ConfigNode protoNode = ProtoVessel.CreateVesselNode(_vesselName, vt, orbit, 0, partNodes, additionalNodes);
                                    bool splashed = false;
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] FINDING NORTH FOR " + _vesselName);

                                    Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);
                                    splashed = false;
                                    protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                                      Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                                    protoNode.SetValue("landed", (landed && !splashed).ToString());
                                    protoNode.SetValue("splashed", splashed.ToString());
                                    protoNode.SetValue("lat", gpsPos.x.ToString());
                                    protoNode.SetValue("lon", gpsPos.y.ToString());
                                    protoNode.SetValue("alt", gpsPos.z.ToString());
                                    protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] protoCraft SET VALUES FOR " + _vesselName);

                                    Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
                                    Quaternion rotation = Quaternion.identity;
                                    float heading = 0;
                                    if (shipConstruct == null)
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
                                    }
                                    else if (shipConstruct.shipFacility == EditorFacility.SPH)
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                                        heading += 180.0f;
                                    }
                                    else
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                                        rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;
                                    }

                                    rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
                                    rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
                                    rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] INITIALIZING ROTATIONS FOR " + _vesselName);

                                    protoNode.SetValue("hgt", "0", true);
                                    protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
                                    Vector3 nrm = (rotation * Vector3.forward);
                                    protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
                                    protoNode.SetValue("prst", false.ToString(), true);

                                    //protoNode.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "Vessel" + _vesselCount + ".proto");
                                    /*
                                    foreach (ConfigNode cn in protoNode.nodes)
                                    {
                                        if (cn.name == "PART")
                                        {
                                            foreach (ConfigNode.Value cv in cn.values)
                                            {
                                                if (cv.name == "part")
                                                {
                                                    if (cv.value.Contains("kerbalEVA"))
                                                    {
                                                        string tkName = "";
                                                        string currentName = "";

                                                        foreach (ConfigNode cn2 in cn.nodes)
                                                        {
                                                            if (cn2.name == "MODULE")
                                                            {
                                                                foreach (ConfigNode.Value cv2 in cn2.values)
                                                                {
                                                                    if (cv2.name == "name")
                                                                    {
                                                                        if (cv2.value == "KerbalEVA")
                                                                        {
                                                                            //ConfigNode vInfo = cn2.GetNode("vInfo");
                                                                            //_serial = new System.Random().Next(1000, 9999);
                                                                            //tkName = "TK - " + _serial;
                                                                            //currentName = vInfo.GetValue("vesselName");
                                                                           // vInfo.SetValue("vesselName", "TK - " + _serial, true);

                                                                            //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                                            //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
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
                                    */
                                    ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

                                    foreach (Part p in protoCraft.vesselRef.parts)
                                    {
                                        //p.SetOpacity(0);
                                    }
                                    protoCraft.vesselRef.transform.rotation = protoCraft.rotation;

                                    Vessel localVessel = protoCraft.vesselRef;
                                    OrXLog.instance.SetRange(localVessel, 10000);

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] VESSEL RANGES SET FOR " + localVessel.vesselName);

                                    foreach (Part p in FindObjectsOfType<Part>())
                                    {
                                        if (!p.vessel)
                                        {
                                            p.gameObject.SetActive(false);
                                            OrXGameobjectTrash.instance._objectsToDestroy.Add(p.gameObject);
                                            //OrXLog.instance.DebugLog("[Spawn OrX Craft File] THROWING " + p.gameObject.name + " IN THE TRASH .......");
                                            
                                            //Destroy(p.gameObject);
                                        }
                                    }

                                    localVessel.isPersistent = true;
                                    localVessel.Landed = false;
                                    localVessel.situation = Vessel.Situations.FLYING;
                                    while (localVessel.packed)
                                    {
                                        yield return null;
                                    }

                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] " + _vesselName + " IS GOING OFF RAILS");
                                    ScreenMessages.PostScreenMessage(new ScreenMessage("Placing " + _vesselName, 1, ScreenMessageStyle.UPPER_CENTER));

                                    localVessel.SetWorldVelocity(Vector3d.zero);
                                    localVessel.GoOffRails();
                                    localVessel.IgnoreGForces(240);
                                    localVessel.angularVelocity = Vector3.zero;
                                    localVessel.angularMomentum = Vector3.zero;

                                    StageManager.BeginFlight();
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === BEGINNING FLIGHT FOR " + _vesselName + " ===");
                                    localVessel.IgnoreGForces(240);
                                    localVessel.angularVelocity = Vector3.zero;
                                    localVessel.angularMomentum = Vector3.zero;
                                    localVessel.rootPart.AddModule("ModuleOrXPlace", true);
                                    var _place = localVessel.rootPart.FindModuleImplementing<ModuleOrXPlace>();
                                    _place.latitude = localVessel.latitude;
                                    _place.longitude = localVessel.longitude;
                                    _place.altitude = localVessel.altitude + 5;
                                    _place.PlaceCraft(_bda, _airborne, _splashed, localVessel.rootPart.Modules.Contains<ModuleOrXStage>(), false, _altToSubtract, _left, _pitch, _delay);

                                    ConfigNode craft = ConfigNode.Load(missionCraftLoc);
                                    craft.ClearData();
                                    craft.Save(missionCraftLoc);
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + _vesselName + " Spawned ===");
                                    if (_bda)
                                    {
                                        List<Part>.Enumerator parts = localVessel.parts.GetEnumerator();
                                        while (parts.MoveNext())
                                        {
                                            if (parts.Current != null)
                                            {
                                                if (parts.Current.Modules.Contains<ModuleOrXWMI>())
                                                {
                                                    parts.Current.SendMessage("SwitchToEnemy");
                                                }
                                            }
                                        }
                                        parts.Dispose();
                                        FlightGlobals.ForceSetActiveVessel(localVessel);
                                        yield return new WaitForSecondsRealtime(1.5f);
                                    }
                                }
                            }
                        }
                    }

                    if (_bda)
                    {
                        yield return new WaitForSecondsRealtime(1);
                        FlightGlobals.ForceSetActiveVessel(trigger);
                        yield return new WaitForFixedUpdate();
                        OrXLog.instance.ResetFocusKeys();
                        OrXVesselLog.instance.CheckEnemies(true);
                    }
                    else
                    {
                        yield return new WaitForFixedUpdate();
                        OrXHoloKron.instance.OrXHCGUIEnabled = false;
                        OrXHoloKron.instance.MainMenu();
                    }
                    spawning = false;
                    OrXGameobjectTrash.instance.EmptyTrashBin();
                }
            }
        }
        IEnumerator SpawnAirSupportRoutine(bool _interceptor, bool _bda, string HoloKronName, Vector3d vect, float _delay)
        {
            OrXSounds.instance.PlayOrders();
            OrXHoloKron.instance.bdaChallenge = _bda;
            string pas = "";
            string _orxFileLoc = "";
            float _altToSubtract = 0;
            UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            ConfigNode _file = new ConfigNode();
            string _orxFile = "";

            string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            List<string> files = new List<string>(Directory.GetFiles(importLoc, "*.holo", SearchOption.AllDirectories));
            if (files != null)
            {
                List<string>.Enumerator orxFile = files.GetEnumerator();
                while (orxFile.MoveNext())
                {
                    if (orxFile.Current != null)
                    {
                        if (orxFile.Current.Contains(HoloKronName + "-" + OrXHoloKron.instance.hkCount + "-" + OrXHoloKron.instance.creatorName))
                        {
                            _orxFileLoc = orxFile.Current;
                            _file = ConfigNode.Load(orxFile.Current);
                            break;
                        }
                    }
                }
                orxFile.Dispose();
            }

            if (_file != null)
            {
                OrXLog.instance.DebugLog("[OrX Spawn Air Support] === Spawning Air Support === ");

                if (HoloKronName != "")
                {
                    OrXHoloKron.instance._opForCount = 0;
                    _enemyCount = 0;
                    _airSupportCount = 0;
                    int _vesselCount = 1;
                    int _hkCount = 0;
                    ConfigNode node = _file.GetNode("OrX");

                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (spawnCheck.name.Contains("OrXHoloKronCoords" + _hkCount))
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Air Support] === FOUND " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " ... DECRYPTING ===");

                            foreach (ConfigNode.Value data in spawnCheck.values)
                            {
                                if (data.name == "challengeType")
                                {
                                    if (data.value.Contains("OUTLAW RACING"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Air Support] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is OUTLAW RACING ===");

                                        OrXHoloKron.instance.outlawRacing = true;
                                    }

                                    if (data.value.Contains("BD ARMORY"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Air Support] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is BD ARMORY ===");

                                        OrXHoloKron.instance.bdaChallenge = true;
                                        OrXHoloKron.instance.outlawRacing = false;

                                    }

                                    if (data.value.Contains("GEO-CACHE"))
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Air Support] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " is GEO-CACHE ===");

                                        OrXHoloKron.instance.geoCache = true;
                                    }
                                }

                                if (data.name == "spawned")
                                {
                                    if (data.value == "False")
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Air Support] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has not spawned ===");
                                        spawnCheck.SetValue("spawned", "True", true);
                                        _file.Save(_orxFileLoc);
                                        //break;
                                    }
                                    else
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Air Support] === " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has spawned ... CHECKING FOR EXTRAS");

                                        if (spawnCheck.HasValue("extras"))
                                        {
                                            if (spawnCheck.GetValue("extras") == "False")
                                            {
                                                OrXLog.instance.DebugLog("[OrX Spawn Air Support] === " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has no extras ... END TRANSMISSION");
                                                //break;
                                            }
                                            else
                                            {
                                                OrXLog.instance.DebugLog("[OrX Spawn Air Support] === " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has extras ... SEARCHING");
                                                _hkCount += 1;
                                            }
                                        }
                                    }
                                }
                            }

                            OrXLog.instance.DebugLog("[OrX Spawn Air Support] === DATA PROCESSED ===");
                        }
                    }

                    foreach (ConfigNode _vts in node.nodes)
                    {
                        string vtsName;
                        if (_interceptor)
                        {
                            vtsName = "HC" + _hkCount + "ASv";
                        }
                        else
                        {
                            vtsName = "HC" + _hkCount + "OrXv" + _vesselCount;
                        }

                        if (_vts.name.Contains(vtsName))
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Air Support] === GRABBING CRAFT FILE FOR " + _vts.name + " ===");
                            _enemyCount += 1;
                            OrXHoloKron.instance._opForCount += 1;
                            _vesselCount += 1;
                            float _left = 0;
                            float _pitch = 0;
                            double _al = 0;
                            double _la = 0;
                            double _lo = 0;
                            int _serial = 1138;
                            bool _airborne = false;
                            bool _splashed = false;

                            _vesselName = _vts.GetValue("vesselName");

                            ConfigNode location = _vts.GetNode("coords");

                            foreach (ConfigNode.Value loc in location.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                loc.name = cvEncryptedName;
                                loc.value = cvEncryptedValue;
                                if (loc.name == "airborne")
                                {
                                    if (loc.value == "True")
                                    {
                                        _airborne = true;
                                    }
                                }
                                if (loc.name == "splashed")
                                {
                                    if (loc.value == "True")
                                    {
                                        _splashed = true;
                                    }
                                }
                                if (loc.name == "vesselName")
                                {
                                    _vesselName = loc.value;
                                }
                                if (loc.name == "lat")
                                {
                                    _la = double.Parse(loc.value);
                                }
                                if (loc.name == "lon")
                                {
                                    _lo = double.Parse(loc.value);
                                }
                                if (loc.name == "alt")
                                {
                                    _al = double.Parse(loc.value);
                                    _al += 5;
                                }
                                if (loc.name == "left")
                                {
                                    _left = float.Parse(loc.value);
                                }
                                if (loc.name == "pitch")
                                {
                                    _pitch = float.Parse(loc.value);
                                }
                                if (loc.name == "pas")
                                {
                                    pas = loc.value;
                                }
                            }
                            OrXLog.instance.DebugLog("[OrX Spawn Air Support] === VESSEL SPAWN COORDS READY ===");

                            if (_airborne || _splashed)
                            {
                                OrXLog.instance.DebugLog("[OrX Spawn Air Support] === DECRYPTING CRAFT FILE DATA FOR " + _vts.name + " ===");
                                ConfigNode craftFile = _vts.GetNode("craft");
                                string tempFile = "";

                                if (!_splashed)
                                {
                                    if (_interceptor)
                                    {
                                        tempFile = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/interceptor" + _interceptorCount + ".tmp";
                                        //_interceptorList.Add(tempFile);
                                        //_interceptorCount += 1;
                                    }
                                    else
                                    {
                                        tempFile = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/airSupport" + _airSupportCount + ".tmp";
                                        //_airSupportList.Add(tempFile);
                                        //_airSupportCount += 1;
                                    }
                                }
                                else
                                {
                                    tempFile = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/seaSupport" + _seaSupportCount + ".tmp";
                                    //_seaSupportList.Add(tempFile);
                                    //_seaSupportCount += 1;
                                }

                                foreach (ConfigNode.Value cv in craftFile.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                    cv.name = cvEncryptedName;
                                    cv.value = cvEncryptedValue;

                                    if (cv.name == "ship")
                                    {
                                        cv.value = _vesselName;
                                    }

                                    if (cv.name == "size")
                                    {
                                        string[] _sizeString = cv.value.Split(new char[] { ',' });

                                        try
                                        {
                                            if (_sizeString[0] != null && _sizeString[0].Length > 0 && _sizeString[0] != "null")
                                            {
                                                for (int i = 0; i < _sizeString.Length; i++)
                                                {
                                                    if (_sizeString[i] != null && _sizeString[i].Length > 0)
                                                    {
                                                        float _maxValue = Math.Max(float.Parse(_sizeString[0]), Math.Max(float.Parse(_sizeString[1]), float.Parse(_sizeString[2])));
                                                        _altToSubtract = _maxValue / 2;
                                                    }
                                                }
                                            }
                                        }
                                        catch (IndexOutOfRangeException e)
                                        {
                                            OrXLog.instance.DebugLog("[OrX Spawn Air Support] Altitude calculated ...... ");
                                        }
                                    }
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
                                            if (!cv2.name.Contains("(") && !cv2.name.Contains(")"))
                                            {
                                                string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                                cv2.name = cvEncryptedName;
                                                cv2.value = cvEncryptedValue;
                                            }
                                        }
                                    }
                                }

                                ConfigNode _craftFile = new ConfigNode();
                                craftFile.CopyTo(_craftFile);
                                craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawnTemp.tmp");

                                yield return new WaitForFixedUpdate();

                                foreach (ConfigNode.Value cv in _craftFile.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                    cv.name = cvEncryptedName;
                                    cv.value = cvEncryptedValue;
                                }

                                foreach (ConfigNode cn in _craftFile.nodes)
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

                                _craftFile.Save(tempFile);

                                OrXLog.instance.DebugLog("[OrX Spawn Air Support] === VESSEL DECRYPTED - CHECKING MODULES ===");
                                List<string> partsLoaded = new List<string>();
                                int partCount = 0;
                                bool okToLoad = true;
                                bool hasKerbal = false;
                                spawningGoal = false;

                                foreach (ConfigNode partCheck in craftFile.nodes)
                                {
                                    if (node.name == "PART")
                                    {
                                        foreach (ConfigNode.Value partCheck2 in partCheck.values)
                                        {
                                            if (partCheck2.name == "name")
                                            {
                                                if (partCheck2.value.Contains("kerbalEVA"))
                                                {
                                                    hasKerbal = true;
                                                }
                                                /*
                                                if (!partsLoaded.Contains(partCheck2.value))
                                                {
                                                    partCount += 1;
                                                    partsLoaded.Add(partCheck2.value);
                                                }
                                                */
                                            }
                                        }
                                    }
                                }

                                if (okToLoad || spawningGoal)
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] === " + _vesselName + " READY FOR SPAWNING ===");
                                    //yield return new WaitForFixedUpdate();

                                    VesselType vt;

                                    vt = VesselType.Ship;

                                    //craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                                    yield return new WaitForFixedUpdate();

                                    Vector3d tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_la, (double)_lo, (double)_al + (_altToSubtract * 3));
                                    Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);
                                    Orbit orbit = null;

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] Altitude: " + gpsPos.z);

                                    bool landed = false;
                                    if (!landed)
                                    {
                                        landed = true;

                                        Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                                        orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                                        orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
                                    }

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] Orbit Data Processed");
                                    yield return new WaitForFixedUpdate();

                                    ConfigNode[] partNodes;
                                    ShipConstruct shipConstruct = null;

                                    ConfigNode currentShip = ShipConstruction.ShipConfig;
                                    shipConstruct = ShipConstruction.LoadShip(tempFile);
                                    ShipConstruction.ShipConfig = currentShip;
                                    uint missionID = (uint)Guid.NewGuid().GetHashCode();
                                    uint launchID = HighLogic.CurrentGame.launchID++;

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] Ship construct created");

                                    foreach (Part p in shipConstruct.parts)
                                    {
                                        p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                                        p.missionID = missionID;
                                        p.launchID = launchID;
                                        p.flagURL = flagURL;
                                        p.temperature = 1.0;
                                    }
                                    //yield return new WaitForFixedUpdate();

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] Part flight ID's processed");
                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] Constructing protoCraft");

                                    List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                                    while (part.MoveNext())
                                    {
                                        if (part.Current != null)
                                        {
                                            if (part.Current.Modules.Contains<KerbalSeat>())
                                            {
                                                if (part.Current.children.Count == 0)
                                                {
                                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                      ? ProtoCrewMember.Gender.Female
                                                      : ProtoCrewMember.Gender.Male;
                                                    _serial = new System.Random().Next(1000, 9999);
                                                    crewMember.trait = "Pilot";
                                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                }
                                            }
                                            else
                                            {
                                                if (part.Current.Modules.Contains<KerbalEVA>())
                                                {
                                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                      ? ProtoCrewMember.Gender.Female
                                                      : ProtoCrewMember.Gender.Male;
                                                    _serial = new System.Random().Next(1000, 9999);
                                                    if (_bda)
                                                    {
                                                        crewMember.trait = "Pilot";
                                                    }
                                                    else
                                                    {
                                                        crewMember.trait = "Tourist";
                                                    }
                                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);

                                                    if (!part.Current.Modules.Contains<ModuleOrX>())
                                                    {
                                                        part.Current.AddModule("ModuleOrX");
                                                    }
                                                }
                                                else
                                                {
                                                    if (part.Current.Modules.Contains<ModuleCommand>() && part.Current.protoModuleCrew.Count <= 1)
                                                    {
                                                        ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                        crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                          ? ProtoCrewMember.Gender.Female
                                                          : ProtoCrewMember.Gender.Male;
                                                        _serial = new System.Random().Next(1000, 9999);
                                                        if (_bda)
                                                        {
                                                            crewMember.trait = "Pilot";
                                                        }
                                                        else
                                                        {
                                                            crewMember.trait = "Tourist";
                                                        }
                                                        part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    part.Dispose();

                                    ConfigNode _tempC = new ConfigNode();
                                    ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                                    Vessel _tempV = new Vessel();
                                    _tempV.parts = shipConstruct.parts;
                                    _tempP.vesselRef = _tempV;

                                    foreach (Part p in shipConstruct.parts)
                                    {
                                        _tempV.loaded = false;
                                        p.vessel = _tempV;
                                        _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                                    }

                                    foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                                    {
                                        p.storePartRefs();
                                    }

                                    List<ConfigNode> partNodesL = new List<ConfigNode>();
                                    foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                                    {
                                        ConfigNode partNode = new ConfigNode("PART");
                                        snapShot.Save(partNode);
                                        partNodesL.Add(partNode);
                                    }
                                    partNodes = partNodesL.ToArray();

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] CREATING ADDITIONAL NODES FOR " + _vesselName);
                                    ConfigNode[] additionalNodes = new ConfigNode[0];
                                    ConfigNode protoNode = ProtoVessel.CreateVesselNode(_vesselName, vt, orbit, 0, partNodes, additionalNodes);
                                    bool splashed = false;
                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] FINDING NORTH FOR " + _vesselName);

                                    Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);
                                    splashed = false;
                                    protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                                      Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                                    protoNode.SetValue("landed", (landed && !splashed).ToString());
                                    protoNode.SetValue("splashed", splashed.ToString());
                                    protoNode.SetValue("lat", gpsPos.x.ToString());
                                    protoNode.SetValue("lon", gpsPos.y.ToString());
                                    protoNode.SetValue("alt", gpsPos.z.ToString());
                                    protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] protoCraft SET VALUES FOR " + _vesselName);

                                    Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
                                    Quaternion rotation = Quaternion.identity;
                                    float heading = 0;
                                    if (shipConstruct == null)
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
                                    }
                                    else if (shipConstruct.shipFacility == EditorFacility.SPH)
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                                        heading += 180.0f;
                                    }
                                    else
                                    {
                                        rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                                        rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;
                                    }

                                    rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
                                    rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
                                    rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] INITIALIZING ROTATIONS FOR " + _vesselName);

                                    protoNode.SetValue("hgt", "0", true);
                                    protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
                                    Vector3 nrm = (rotation * Vector3.forward);
                                    protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
                                    protoNode.SetValue("prst", false.ToString(), true);

                                    ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

                                    protoCraft.vesselRef.transform.rotation = protoCraft.rotation;

                                    Vessel localVessel = protoCraft.vesselRef;
                                    OrXLog.instance.SetRange(localVessel, 10000);

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] VESSEL RANGES SET FOR " + localVessel.vesselName);

                                    foreach (Part p in FindObjectsOfType<Part>())
                                    {
                                        if (!p.vessel)
                                        {
                                            p.gameObject.SetActive(false);
                                            OrXGameobjectTrash.instance._objectsToDestroy.Add(p.gameObject);
                                            //OrXLog.instance.DebugLog("[Spawn OrX Craft File] THROWING " + p.gameObject.name + " IN THE TRASH .......");

                                            //Destroy(p.gameObject);
                                        }
                                    }

                                    localVessel.isPersistent = true;
                                    localVessel.Landed = false;
                                    localVessel.situation = Vessel.Situations.FLYING;
                                    while (localVessel.packed)
                                    {
                                        yield return null;
                                    }

                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] " + _vesselName + " IS GOING OFF RAILS");

                                    localVessel.SetWorldVelocity(Vector3d.zero);
                                    localVessel.GoOffRails();
                                    localVessel.IgnoreGForces(240);
                                    localVessel.angularVelocity = Vector3.zero;
                                    localVessel.angularMomentum = Vector3.zero;

                                    StageManager.BeginFlight();
                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] === BEGINNING FLIGHT FOR " + _vts.name + " ===");
                                    localVessel.IgnoreGForces(240);
                                    localVessel.angularVelocity = Vector3.zero;
                                    localVessel.angularMomentum = Vector3.zero;

                                    localVessel.rootPart.AddModule("ModuleOrXPlace", true);
                                    var _place = localVessel.rootPart.FindModuleImplementing<ModuleOrXPlace>();
                                    _place.latitude = localVessel.latitude;
                                    _place.longitude = localVessel.longitude;
                                    _place.altitude = localVessel.altitude;
                                    _place.PlaceCraft(_bda, _airborne, _splashed, localVessel.rootPart.Modules.Contains<ModuleOrXStage>(), false, _altToSubtract, _left, _pitch, _delay);

                                    if (_bda)
                                    {
                                        yield return new WaitForFixedUpdate();
                                        List<Part>.Enumerator parts = localVessel.parts.GetEnumerator();
                                        while (parts.MoveNext())
                                        {
                                            if (parts.Current != null)
                                            {
                                                if (parts.Current.Modules.Contains<ModuleOrXWMI>())
                                                {
                                                    parts.Current.SendMessage("SwitchToEnemy");
                                                }
                                            }
                                        }
                                        parts.Dispose();
                                        FlightGlobals.ForceSetActiveVessel(localVessel);
                                        yield return new WaitForSecondsRealtime(1.5f);
                                    }
                                    OrXLog.instance.DebugLog("[OrX Spawn Air Support] === " + _vts.name + " Spawned ===");
                                }
                            }
                        }
                    }

                    if (_bda)
                    {
                        yield return new WaitForSecondsRealtime(1);
                        FlightGlobals.ForceSetActiveVessel(trigger);
                        yield return new WaitForFixedUpdate();
                        OrXLog.instance.ResetFocusKeys();
                        OrXVesselLog.instance.CheckEnemies(true);
                    }
                    spawning = false;
                    OrXGameobjectTrash.instance.EmptyTrashBin();
                    try
                    {
                        OrXBDAcExtension.SetBDAcVSGUI();
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[OrX Spawn Air Support] === ERROR === " + e + " ===");
                    }
                }
            }
        }

        public void CraftSelect(bool _spawningBarrier, bool scf, bool _wingman)
        {
            _spawnCraftFile = scf;
            wingman = _wingman;
            if (!scf)
            {
                OrXHoloKron.instance.OrXHCGUIEnabled = false;
            }

            StartCoroutine(StartVesselSelectRoutine(_spawningBarrier));
        }
        public IEnumerator StartVesselSelectRoutine(bool _spawningBarrier)
        {
            string _dir = "";
            if (_spawningBarrier)
            {
                OrXLog.instance.DebugLog("[OrX Craft Select] CRAFT FOLDER: " + UrlDir.ApplicationRootPath + "GameData/OrX/Parts/Base/");

                _dir = UrlDir.ApplicationRootPath + "GameData/OrX/Parts/Base/";
            }
            else
            {
                if (OrXHoloKron.instance.LBC)
                {
                    OrXLog.instance.DebugLog("[OrX Craft Select] CRAFT FOLDER: " + UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/OrX/");
                    _dir = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/OrX/";
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Craft Select] CRAFT FOLDER: " + HighLogic.SaveFolder);
                    _dir = HighLogic.SaveFolder;
                }
            }
            OrXLog.instance.DebugLog("[OrX Craft Select] Start craft selection");
            yield return null;
            craftBrowser = CraftBrowserDialog.Spawn(EditorFacility.SPH, _dir, OnSelected, OnCancelled, false);
        }
        public void OnSelected(string _selectedCraftFile, CraftBrowserDialog.LoadType loadType)
        {
            OrXLog.instance.DebugLog("[OrX Craft Select] Selected Craft: " + _selectedCraftFile);

            if (spawningGoal)
            {
                OrXLog.instance.DebugLog("[OrX Craft Select] Start goal spawn");
                craftBrowser = null;
                openingCraftBrowser = false;

                //Vector3d gpsPos = WorldPositionToGeoCoords(new Vector3d(_HoloKron.latitude, _HoloKron.longitude, _HoloKron.altitude), FlightGlobals.currentMainBody);
                //OrXSpawnHoloKron.instance.SpawnStartingGate();
            }
            else
            {
                if (!_spawnCraftFile)
                {
                    AddBlueprints(_selectedCraftFile);
                }
                else
                {
                    SpawnSelected(_selectedCraftFile);
                }
            }
        }
        public void OnCancelled()
        {
            OrXLog.instance.DebugLog("[OrX Craft Select] Cancelling Select Craft ............");
            if (wingman)
            {

            }
            else
            {
                OrXHoloKron.instance.SpawnMenu();
            }
            spawningGoal = false;
            spawning = false;
        }

        public Vector3d WorldPositionToGeoCoords(Vector3d worldPosition, CelestialBody body)
        {
            if (!body)
            {
                return Vector3d.zero;
            }
            double lat = body.GetLatitude(worldPosition);
            double longi = body.GetLongitude(worldPosition);
            double alt = body.GetAltitude(worldPosition);
            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Lat: " + lat + " - Lon:" + longi + " - Alt: " + alt);
            return new Vector3d(lat, longi, alt);
        }
    }
}