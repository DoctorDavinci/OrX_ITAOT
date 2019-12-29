using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXUtilities : MonoBehaviour
    {
        public static OrXUtilities instance;
        bool _scanningFile = false;

        public void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        #region Utilities

        public double GetDegreesPerMeter(CelestialBody _body, double _altitude)
        {
            double mPerDegree = (((2 * (_body.Radius + _altitude)) * Math.PI) / 360);
            return 1 / mPerDegree;
        }

        public void GetCreatorList(bool _challenge)
        {
            OrXHoloKron.instance.Reach();

            OrXHoloKron.instance.OrXGeoCacheCreatorList = new List<string>();
            OrXHoloKron.instance.OrXChallengeCreatorList = new List<string>();

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
                            string _group = "";

                            foreach (ConfigNode.Value cv in dataFile.values)
                            {
                                if (cv.name == "group")
                                {
                                    _group = cv.value;
                                }
                                else
                                {
                                    /*
                                    if (!OrXHoloKron.instance.OrXChallengeCreatorList.Contains(_group))
                                    {
                                        Debug.Log("[OrX Get Creator List] === Adding " + _group + " to challenge creator list ===");
                                        OrXHoloKron.instance.OrXChallengeCreatorList.Add(_group);
                                    }
                                    */
                                    if (cv.value == "CHALLENGE")
                                    {
                                        if (!OrXHoloKron.instance.OrXChallengeCreatorList.Contains(_group))
                                        {
                                            Debug.Log("[OrX Get Creator List] === Adding " + _group + " to challenge creator list ===");
                                            OrXHoloKron.instance.OrXChallengeCreatorList.Add(_group);
                                        }
                                    }
                                    else
                                    {
                                        if (!OrXHoloKron.instance.OrXGeoCacheCreatorList.Contains(_group))
                                        {
                                            Debug.Log("[OrX Get Creator List] === Adding " + _group + " to geo-cache creator list ===");
                                            OrXHoloKron.instance.OrXGeoCacheCreatorList.Add(_group);
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
                if (OrXHoloKron.instance.OrXGeoCacheCreatorList.Count != 0)
                {
                    OrXHoloKron.instance.checking = true;
                    OrXTargetDistance.instance.TargetDistance(true, false, false, true, "", new Vector3d());
                }
                else
                {
                    OrXHoloKron.instance.MainMenu();
                    OrXHoloKron.instance.OnScrnMsgUC("No Geo-Cache found .....");
                }

            }
            else
            {
                if (OrXHoloKron.instance.OrXChallengeCreatorList.Count != 0)
                {
                    OrXHoloKron.instance.MainMenu();
                    OrXHoloKron.instance.showChallengelist = true;
                    OrXHoloKron.instance.movingCraft = false;
                    OrXHoloKron.instance.getNextCoord = false;
                    OrXHoloKron.instance.challengeRunning = false;
                    OrXHoloKron.instance.showCreatedHolokrons = false;
                    OrXHoloKron.instance.showGeoCacheList = false;
                }
                else
                {
                    OrXHoloKron.instance.MainMenu();
                    OrXHoloKron.instance.OnScrnMsgUC("No HoloKrons detected .....");
                }
            }
        }
        public void GrabCreations(string _groupName, bool challenge)
        {
            StartCoroutine(GetCreations(_groupName, challenge));
        }
        IEnumerator GetCreations(string _groupName, bool challenge)
        {
            OrXHoloKron.instance.Reach();
            OrXHoloKron.instance.OrXChallengeList = new List<string>();
            OrXHoloKron.instance.OrXCoordsList = new List<string>();
            OrXHoloKron.instance.OrXChallengeNameList = new List<string>();
            OrXHoloKron.instance.OrXGeoCacheNameList = new List<string>();

            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _groupName + "/";
            List<string> files = new List<string>(Directory.GetFiles(holoKronLoc, "*.holo", SearchOption.AllDirectories));

            if (files != null)
            {
                OrXLog.instance.DebugLog("[OrX Get Creations] === Found " + files.Count + " files ===");
                OrXHoloKron.instance.OnScrnMsgUC("Found " + files.Count + " files .....");

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
                                OnScrnMsgUC("UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD");

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
                                            if (_soi == OrXHoloKron.instance.soi)
                                            {
                                                OrXLog.instance.DebugLog("[OrX Get Creations] " + _HoloKronName + "'s current SOI '" + OrXHoloKron.instance.soi + "' matches HoloKron SOI '" + _soi + "'");

                                                bool _challenge = false;
                                                string missionType = spawnCheck.GetValue("missionType");
                                                string _count = spawnCheck.GetValue("count");

                                                if (_count == "0")
                                                {
                                                    _count = "0";
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
                                                    if (!OrXHoloKron.instance.OrXChallengeList.Contains(targetCoords))
                                                    {
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] === ADDING COORDS TO LIST ===");
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                        OrXLog.instance.DebugLog("[OrX Get Creations] " + targetCoords);
                                                        OrXHoloKron.instance.OrXChallengeList.Add(targetCoords);
                                                        string nameToAdd = data[1]; // + "-" + _count;

                                                        if (!OrXHoloKron.instance.OrXChallengeNameList.Contains(nameToAdd))
                                                        {
                                                            OrXHoloKron.instance.OrXChallengeNameList.Add(nameToAdd);
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
            yield return new WaitForFixedUpdate();

            OrXHoloKron.instance.showCreatedHolokrons = true;
            OrXHoloKron.instance.showGeoCacheList = false;
            OrXHoloKron.instance.getNextCoord = false;
            OrXHoloKron.instance.GuiEnabledOrXMissions = false;
            OrXHoloKron.instance._showSettings = false;
            OrXHoloKron.instance.connectToKontinuum = false;
            OrXHoloKron.instance.checking = false;
            OrXHoloKron.instance.movingCraft = false;
        }
        public void DeleteHoloKron(string _groupName, string _holoName)
        {
            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _groupName + "/" + _holoName + "/";
            List<string> files = new List<string>(Directory.GetFiles(holoKronLoc, "*.*", SearchOption.AllDirectories));
            if (files != null)
            {
                List<string>.Enumerator _file = files.GetEnumerator();
                while (_file.MoveNext())
                {
                    if (_file.Current != null)
                    {
                        File.Delete(_file.Current);
                    }
                }
                _file.Dispose();

                Directory.Delete(holoKronLoc);
            }
            OrXHoloKron.instance.MainMenu();
        }

        public void GetInstalledMods(bool _spawn)
        {
            StartCoroutine(CheckInstalledMods(_spawn));
        }
        IEnumerator CheckInstalledMods(bool _spawn)
        {
            OrXHoloKron.instance.getNextCoord = false;
            OrXHoloKron.instance.movingCraft = true;
            OrXHoloKron.instance._file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + OrXHoloKron.instance.groupName + "/"  + OrXHoloKron.instance.HoloKronName + "-" + OrXHoloKron.instance.hkCount + "-" + OrXHoloKron.instance.groupName + ".holo");

            if (OrXHoloKron.instance._file != null)
            {
                ConfigNode mods = OrXHoloKron.instance._file.GetNode("Mods");
                OrXHoloKron.instance._orxFilePartModules = new List<string>();
                OrXHoloKron.instance._orxFileMods = new List<string>();
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
                            //count2 += 1;
                            //_orxFilePartModules.Add(cv.name);
                        }

                        if (cv.value == "Mod")
                        {
                            count += 1;
                            OrXHoloKron.instance._orxFileMods.Add(cv.name);
                        }
                    }
                    yield return new WaitForFixedUpdate();

                    List<string>.Enumerator _installedMods = OrXHoloKron.instance.installedMods.GetEnumerator();
                    while (_installedMods.MoveNext())
                    {
                        if (_installedMods.Current != null)
                        {

                            if (OrXHoloKron.instance._orxFileMods.Contains(_installedMods.Current))
                            {
                                OrXHoloKron.instance._orxFileMods.Remove(_installedMods.Current);
                                count -= 1;
                            }
                        }
                    }
                    _installedMods.Dispose();
                    yield return new WaitForFixedUpdate();
                    /*
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
                    */
                    bool _continue = true;
                    if (count != 0) // || count2 != 0)
                    {
                        _continue = false;
                    }

                    if (HighLogic.LoadedSceneIsFlight)
                    {
                        if (!_continue)
                        {
                            Debug.Log("[OrX Check Installed Mods] === MISSING MOD COUNT: " + count + " ===");

                            List<string>.Enumerator _leftovers = OrXHoloKron.instance._orxFileMods.GetEnumerator();
                            while (_leftovers.MoveNext())
                            {
                                if (_leftovers.Current != null)
                                {
                                    Debug.Log("[OrX Check Installed Mods] === " + _leftovers.Current + " NOT INSTALLED ===");
                                }
                            }
                            _leftovers.Dispose();
                            OrXHoloKron.instance.SetModFail();

                            /*
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
                            */
                        }
                        else
                        {
                            if (_spawn)
                            {
                                Debug.Log("[OrX Check Installed Mods] === MODS CHECKED OUT ... SPAWNING LOCAL===");
                                OrXHoloKron.instance.movingCraft = false;
                                OrXHoloKron.instance.modCheckFail = false;
                                spawn.OrXSpawnHoloKron.instance.SpawnLocal(false, OrXHoloKron.instance.HoloKronName, new Vector3d(), OrXTargetDistance.instance._wmActivateDelay, 0);
                            }
                            else
                            {
                                Debug.Log("[OrX Check Installed Mods] === MODS CHECKED OUT ... OPENING HOLOKRON ===");
                                OrXHoloKron.instance.OpenHoloKron(OrXHoloKron.instance.geoCache, OrXHoloKron.instance.HoloKronName, OrXHoloKron.instance.hkCount, null, null);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("[OrX Check Installed Mods] === IN EDITOR SCENE ... OPENING HOLOKRON ===");
                        OrXHoloKron.instance.OpenHoloKron(OrXHoloKron.instance.geoCache, OrXHoloKron.instance.HoloKronName, OrXHoloKron.instance.hkCount, null, null);
                    }
                    OrXSounds.instance.KnowMore();
                }
            }
            else
            {
                Debug.Log("[OrX Check Installed Mods - ERROR] === FILE WAS NULL ===");
            }
        }

        public void StartMrKleen()
        {
            StartCoroutine(MrKleen());
        }
        IEnumerator MrKleen()
        {
            OrXHoloKron.instance.killingChallenge = true;
            OrXHoloKron.instance.OnScrnMsgUC("The Kontinuum is calling a maid .....");
            OrXHoloKron.instance.getNextCoord = false;
            OrXHoloKron.instance.movingCraft = true;
            OrXHoloKron.instance.GuiEnabledOrXMissions = true;
            Debug.Log("[OrX Mr Kleen] === CALLING JASON ===");

            List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
            while (v.MoveNext())
            {
                if (v.Current != null)
                {
                    if (!v.Current.isActiveVessel)
                    {
                        bool _kill = false;

                        if (OrXHoloKron.instance.bdaChallenge && OrXHoloKron.instance.challengeRunning)
                        {
                            if (v.Current.rootPart.Modules.Contains<ModuleOrXMission>())
                            {
                                _kill = false;
                                break;
                            }

                            List<Part>.Enumerator _parts = v.Current.parts.GetEnumerator();
                            while (_parts.MoveNext())
                            {
                                if (_parts.Current != null)
                                {
                                    if (_parts.Current.Modules.Contains<ModuleOrXWMI>())
                                    {
                                        _kill = false;
                                        break;
                                    }
                                }
                            }
                            _parts.Dispose();
                        }
                        else
                        {
                            _kill = true;
                        }

                        if (_kill)
                        {
                            Debug.Log("[OrX Mr Kleen] === Jason is killing " + v.Current.vesselName + "  ===");
                            v.Current.rootPart.AddModule("ModuleOrXJason", true);
                            yield return new WaitForFixedUpdate();
                        }
                    }
                }
            }
            v.Dispose();

            if (!OrXHoloKron.instance.challengeRunning)
            {
                OrXHoloKron.instance.getNextCoord = false;
                OrXHoloKron.instance.movingCraft = false;
                OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                OrXHoloKron.instance.killingChallenge = false;
                OrXHoloKron.instance.OnScrnMsgUC("The slate is being swept clean .....");
            }
            else
            {
                if (OrXHoloKron.instance.bdaChallenge)
                {
                    OrXHoloKron.instance.OnScrnMsgUC("Jason is cleaning up your mess .....");
                    OrXVesselLog.instance.CheckEnemies(false);
                }
            }
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
            OrXHoloKron.instance.MainMenu();

            OrXHoloKron.instance.checking = false;
            OrXHoloKron.instance.building = false;
            OrXHoloKron.instance.buildingMission = false;
            OrXHoloKron.instance.addCoords = false;
            OrXHoloKron.instance.PlayOrXMission = false;

            if (playerCancel)
            {
                OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                //OrXHCGUIEnabled = false;
                OrXHoloKron.instance.locAdded = false;
                OrXHoloKron.instance.locCount = 0;
                OrXHoloKron.instance.movingCraft = false;
                OrXHoloKron.instance.challengeRunning = false;
                OrXHoloKron.instance.OnScrnMsgUC("Operation 'Dinner Out' was cancelled .....");
                OrXHoloKron.instance.ResetData();
            }
        }

        public void LoadResetDelay()
        {
            StartCoroutine(LoadDelay());
        }
        IEnumerator LoadDelay()
        {
            while (!FlightGlobals.ready)
            {
                yield return null;
            }

            if (OrXHoloKron.instance.building || OrXHoloKron.instance.buildingMission)
            {
                OrXHoloKron.instance.ResetData();
            }
            else
            {
                if (OrXHoloKron.instance.challengeRunning)
                {
                    OrXHoloKron.instance.challengeRunning = false;
                    StopScan(true);
                }
            }
        }
        public void LoadData()
        {
            StartCoroutine(ProcessOrXFiles());
        }

        IEnumerator ProcessOrXFiles()
        {
            OrXHoloKron.instance.OrXLoadedFileList = new List<string>();
            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Import/";
            var files = new List<string>(Directory.GetFiles(holoKronLoc, "*.orx", SearchOption.AllDirectories));
            if (files != null)
            {
                List<string>.Enumerator filesToParse = files.GetEnumerator();
                while (filesToParse.MoveNext())
                {
                    yield return new WaitForFixedUpdate();

                    if (!OrXHoloKron.instance.OrXLoadedFileList.Contains(filesToParse.Current))
                    {
                        ConfigNode holoFile = ConfigNode.Load(filesToParse.Current);
                        if (holoFile != null)
                        {
                            OrXHoloKron.instance.OrXLoadedFileList.Add(filesToParse.Current);
                            Debug.Log("[OrX Check Imports - Processing OrX File] === " + filesToParse.Current  + " ===");
                            bool _save = true;
                            string _name = holoFile.GetValue("name");
                            string _group = holoFile.GetValue("group");
                            string _processed = "";
                            string _count = "";
                            string _fileNameToSave = "";
                            string missionType = "";
                            ConfigNode _orxNode = new ConfigNode();

                            foreach (ConfigNode holoNode in holoFile.nodes)
                            {
                                _fileNameToSave = _name + "-" + holoNode.name + "-" + _group;
                                Debug.Log("[OrX Check Imports - Extracting HoloKron] === " + _name + holoNode.name + _group + " ===");
                                string _newDir = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _group + "/";
                                if (!Directory.Exists(_newDir))
                                {
                                    Directory.CreateDirectory(_newDir);
                                }

                                if (File.Exists(_newDir + _fileNameToSave + ".holo"))
                                {
                                    Debug.Log("[OrX Check Imports - Processing HoloKron] === HOLOKRON " + _fileNameToSave + " EXISTS ===");

                                    ConfigNode _file = ConfigNode.Load(_newDir + _fileNameToSave + ".holo");
                                    if (_file != null)
                                    {
                                        _orxNode = _file.GetNode("OrX");
                                        foreach (ConfigNode cn2 in _orxNode.nodes)
                                        {
                                            if (cn2.name.Contains("OrXHoloKronCoords" + holoNode.name))
                                            {
                                                Debug.Log("[OrX Check Imports] === UPDATING EXTRAS IN " + _fileNameToSave + " ===");

                                                cn2.SetValue("extras", "True");
                                                _file.Save(_newDir + _fileNameToSave + ".holo");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Check Imports - FILE ERROR] === " + _fileNameToSave + " DOESNT EXIST WHEN IT SHOULD ... CREATING NEW ===");
                                        holoNode.Save(_newDir + _fileNameToSave + ".holo");
                                    }
                                }
                                else
                                {
                                    Debug.Log("[OrX Check Imports - Saving HoloKron] === HOLOKRON " + _fileNameToSave + " SAVED ===");
                                    holoNode.Save(_newDir + _fileNameToSave + ".holo");
                                }

                                _orxNode = holoNode.GetNode("OrX");
                                foreach (ConfigNode cn2 in _orxNode.nodes)
                                {
                                    if (cn2.name.Contains("OrXHoloKronCoords"))
                                    {
                                        missionType = cn2.GetValue("missionType");
                                    }
                                }
                                Debug.Log("[OrX Check Imports - Get Mission Type] === " + _fileNameToSave + " MISSION TYPE IS " + missionType + " ===");

                                yield return new WaitForFixedUpdate();
                                ConfigNode dataFile = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _group + "/" + _group + ".data");
                                if (dataFile == null)
                                {
                                    dataFile = new ConfigNode();
                                    dataFile.SetValue("group", _group, true);
                                }
                                dataFile.SetValue(_name + "-" + holoNode.name, missionType, true);
                                dataFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _group + "/" + _group + ".data");
                            }

                            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + _group + "/"))
                            {
                                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + _group + "/");
                            }

                            holoFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloArchive/" + _group + "/" + _name + "-" + _group + ".orx");
                        }
                    }
                }
                filesToParse.Dispose();

                List<string>.Enumerator _fileToDelete = OrXHoloKron.instance.OrXLoadedFileList.GetEnumerator();
                while (_fileToDelete.MoveNext())
                {
                    if (_fileToDelete.Current != null)
                    {
                        File.Delete(_fileToDelete.Current);
                    }
                }
                _fileToDelete.Dispose();
            }

            yield return new WaitForFixedUpdate();
            StartCoroutine(ProcessHoloKronFiles(true));
        }
        public void ProcessHoloFiles()
        {
            StartCoroutine(ProcessHoloKronFiles(false));
        }
        IEnumerator ProcessHoloKronFiles(bool _challenge)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                OrXHoloKron.instance.soi = FlightGlobals.ActiveVessel.mainBody.name;
            }
            OrXHoloKron.instance.OrXCoordsList = new List<string>();
            OrXHoloKron.instance.OrXChallengeList = new List<string>();
            OrXHoloKron.instance.OrXGeoCacheNameList = new List<string>();
            OrXHoloKron.instance.OrXChallengeNameList = new List<string>();

            ConfigNode playerData = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            if (playerData == null)
            {
                playerData = new ConfigNode();
                playerData.SetValue("name", OrXHoloKron.instance.challengersName, true);
                playerData.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/userData.data");
            }
            else
            {
                OrXHoloKron.instance.challengersName = playerData.GetValue("name");
            }

            #region ToAdd

            /*
            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Import/";
            var files = new List<string>(Directory.GetFiles(holoKronLoc, "*.holo", SearchOption.AllDirectories));
            if (files != null)
            {
                OrXLog.instance.DebugLog("[OrX Check Imports] === " + files.Count + " HOLOKRON FILES FOUND ===");
                OrXHoloKron.instance.OrXLoadedFileList = new List<string>();

                List<string>.Enumerator cfgsToMove = files.GetEnumerator();
                while (cfgsToMove.MoveNext())
                {
                    yield return new WaitForFixedUpdate();

                    if (!OrXHoloKron.instance.OrXLoadedFileList.Contains(cfgsToMove.Current))
                    {
                        ConfigNode orxFile = ConfigNode.Load(cfgsToMove.Current);
                        if (orxFile != null)
                        {
                            OrXHoloKron.instance.OrXLoadedFileList.Add(cfgsToMove.Current);
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
                                            int _hkCount = 0;
                                            string _count = cn.GetValue("count");
                                            int _count_ = int.Parse(_count);
                                            string _newDir = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creator + "/" + _name + "/";
                                            if (!Directory.Exists(_newDir))
                                            {
                                                Directory.CreateDirectory(_newDir);
                                            }
                                            else
                                            {
                                                while (_hkCount <= _count_)
                                                {
                                                    OrXHoloKron.instance._file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creator + "/" + _name + "/" + _name + "-" + _hkCount + "-" + _creator + ".holo");
                                                    if (OrXHoloKron.instance._file != null)
                                                    {
                                                        ConfigNode _orxNode = OrXHoloKron.instance._file.GetNode("OrX");
                                                        foreach (ConfigNode cn2 in _orxNode.nodes)
                                                        {
                                                            if (cn2.name.Contains("OrXHoloKronCoords" + _hkCount))
                                                            {
                                                                Debug.Log("[OrX Check Imports] === UPDATING EXTRAS IN " + _name + " number " + _hkCount + " CREATED BY " + _creator + " ===");

                                                                cn2.SetValue("extras", "True");
                                                                OrXHoloKron.instance._file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _creator + "/" + _name + "/" + _name + "-" + _hkCount + "-" + _creator + ".holo");
                                                                _hkCount += 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            string _moveToLoc = _newDir + "/" + _name + "-" + _count + "-" + _creator + ".holo";
                                            string _nodeValue = _name;
                                            string _missionType = cn.GetValue("missionType");
                                            OrXLog.instance.DebugLog("[OrX Check Imports] === " + _name + " PROCESSED ===");
                                            orxFile.Save(_moveToLoc);

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
                cfgsToMove.Dispose();

                List<string>.Enumerator _fileToDelete = OrXHoloKron.instance.OrXLoadedFileList.GetEnumerator();
                while (_fileToDelete.MoveNext())
                {
                    if (_fileToDelete.Current != null)
                    {
                        File.Delete(_fileToDelete.Current);
                    }
                }
                _fileToDelete.Dispose();
            }
            */
            #endregion

            yield return new WaitForFixedUpdate();

            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            List<string> files = new List<string>(Directory.GetFiles(holoKronLoc, "*.holo", SearchOption.AllDirectories));
            if (files != null)
            {
                OrXLog.instance.DebugLog("[OrX Process HoloKron Files] === Found " + files.Count + " files ===");
                OrXHoloKron.instance.OnScrnMsgUC("Found " + files.Count + " files .....");

                List<string>.Enumerator cfgsToAdd = files.GetEnumerator();
                while (cfgsToAdd.MoveNext())
                {
                    if (cfgsToAdd.Current != null)
                    {
                        if (cfgsToAdd.Current.Contains("-0-"))
                        {
                            LoadHoloKronFile(cfgsToAdd.Current, "", 0, "");
                            OrXLog.instance.DebugLog("[OrX Process HoloKron Files] === LOAD AND PROCESS " + cfgsToAdd.Current + " ===");
                            while(_scanningFile)
                            { 
                                yield return null;
                            }
                        }
                    }
                }
                cfgsToAdd.Dispose();

                GetCreatorList(_challenge);
            }
            else
            {
            }
        }
        public void LoadHoloKronFile(string _file, string _HoloKronName, int _hkCount, string _groupName)
        {
            _scanningFile = true;
            bool _continue = false;
            ConfigNode fileNode = new ConfigNode();
            if (_HoloKronName == "")
            {
                fileNode = ConfigNode.Load(_file);
                OrXLog.instance.DebugLog("[OrX Load HoloKron File - Load File] === " + _file + " loaded ===");
            }
            else
            {
                fileNode = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + _groupName + "/" + _HoloKronName + "-" + _hkCount + "-" + _groupName + ".holo");
            }
            string holoKronLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            bool ableToLoad = true;
            bool _spawned = true;
            string _soi = "";
            if (fileNode != null && fileNode.HasNode("OrX"))
            {
                #region To Implement
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
                #endregion

                ConfigNode node = fileNode.GetNode("OrX");
                _HoloKronName = fileNode.GetValue("name");
                _groupName = fileNode.GetValue("group");

                /*
                if (moduleCount >= 0)
                {
                    OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] === UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD ===");
                    ableToLoad = false;
                    OnScrnMsgUC("UNRECOGNIZED PART MODULES FOUND IN " + _HoloKronName + " ... UNABLE TO LOAD");

                }

                */

                if (fileNode.GetValue("count") == _hkCount.ToString() && ableToLoad)
                {
                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (_spawned)
                        {
                            if (spawnCheck.name.Contains("OrXHoloKronCoords" + _hkCount))
                            {
                                if (HighLogic.LoadedSceneIsFlight)
                                {
                                    OrXLog.instance.DebugLog("[OrX Load HoloKron File] === FOUND HOLOKRON ... CHECKING SOI ===");
                                    _soi = spawnCheck.GetValue("SOI");
                                    if (_soi == OrXHoloKron.instance.soi)
                                    {
                                        _continue = true;
                                        OrXLog.instance.DebugLog("[OrX Load HoloKron File] " + _HoloKronName + "'s current SOI '" + OrXHoloKron.instance.soi + "' matches HoloKron SOI '" + _soi + "'");
                                    }
                                }
                                else
                                {
                                    _continue = true;
                                }

                                if (_continue)
                                {
                                    string missionType = spawnCheck.GetValue("missionType");

                                    string targetCoords = spawnCheck.GetValue("Targets");
                                    if (targetCoords == string.Empty)
                                    {
                                        OrXLog.instance.DebugLog("[OrX Load HoloKron File] " + _HoloKronName + " " + _hkCount + " Target string was empty!");
                                    }
                                    else
                                    {
                                        string[] data = targetCoords.Split(new char[] { ',' });

                                        if (missionType == "CHALLENGE")
                                        {
                                            if (!OrXHoloKron.instance.OrXChallengeList.Contains(targetCoords))
                                            {
                                                OrXLog.instance.DebugLog("[OrX Load HoloKron File - CHALLENGE] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                OrXLog.instance.DebugLog("[OrX Load HoloKron File - CHALLENGE] " + targetCoords);
                                                OrXHoloKron.instance.OrXChallengeList.Add(targetCoords);

                                                if (!OrXHoloKron.instance.OrXChallengeNameList.Contains(data[1]))
                                                {
                                                    OrXHoloKron.instance.OrXChallengeNameList.Add(data[1]);
                                                }
                                            }
                                            else
                                            {
                                                OrXLog.instance.DebugLog("[OrX Load HoloKron File - CHALLENGE] === CHALLENGE LIST ALREADY CONTAINS " + targetCoords + " ===");
                                            }
                                        }
                                        else
                                        {
                                            if (!OrXHoloKron.instance.OrXCoordsList.Contains(targetCoords))
                                            {
                                                OrXLog.instance.DebugLog("[OrX Load HoloKron File - GEO-CACHE] Loaded " + _HoloKronName + " " + _hkCount + " Targets");
                                                OrXLog.instance.DebugLog("[OrX Load HoloKron File - GEO-CACHE] " + targetCoords);
                                                OrXHoloKron.instance.OrXCoordsList.Add(targetCoords);

                                                if (!OrXHoloKron.instance.OrXGeoCacheNameList.Contains(data[1]))
                                                {
                                                    OrXHoloKron.instance.OrXGeoCacheNameList.Add(data[1]);
                                                }
                                            }
                                            else
                                            {
                                                OrXLog.instance.DebugLog("[OrX Load HoloKron File - GEO-CACHE] === GEO-CACHE LIST ALREADY CONTAINS " + targetCoords + " ===");
                                            }
                                        }
                                        //yield return new WaitForFixedUpdate();
                                    }

                                    string _spawnedString = spawnCheck.GetValue("spawned");
                                    if (_spawnedString == "False")
                                    {
                                        OrXLog.instance.DebugLog("[OrX Load HoloKron File] === " + _HoloKronName + " " + _hkCount + " has not spawned ... ");
                                        _spawned = false;
                                        _scanningFile = false;
                                    }
                                    else
                                    {
                                        OrXLog.instance.DebugLog("[OrX Load HoloKron File] === " + _HoloKronName + " " + _hkCount + " has spawned ... ");
                                        var complete = spawnCheck.GetValue("completed");
                                        if (complete == "False")
                                        {
                                            OrXLog.instance.DebugLog("[OrX Load HoloKron File] === " + _HoloKronName + " " + _hkCount + " has not been completed ... END TRANSMISSION"); ;
                                            _spawned = false;
                                            _scanningFile = false;
                                        }
                                        else
                                        {
                                            OrXLog.instance.DebugLog("[OrX Load HoloKron File] === " + _HoloKronName + " " + _hkCount + " has been completed ... CHECKING FOR EXTRAS"); ;
                                            if (spawnCheck.HasValue("extras"))
                                            {
                                                var t = spawnCheck.GetValue("extras");
                                                if (t == "False")
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Load HoloKron File] === " + _HoloKronName + " " + _hkCount + " has no extras ... END TRANSMISSION");
                                                    _spawned = false;
                                                    _scanningFile = false;
                                                }
                                                else
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Load HoloKron File] === " + _HoloKronName + " " + _hkCount + " has extras ... CONTINUING");
                                                    _hkCount += 1;
                                                    //_spawned = false;
                                                    LoadHoloKronFile("", _HoloKronName, _hkCount, _groupName);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    OrXLog.instance.DebugLog("[OrX Load HoloKron File] " + _HoloKronName + " is not in the current SOI");
                                }
                            }
                        }
                    }
                }
            }

            _scanningFile = false;
        }

        public bool CheckExports(string holoName)
        {
            OrXLog.instance.DebugLog("[OrX Check Exports] === CHECKING  FOR " + holoName + " ===");
            string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/" + OrXHoloKron.instance.groupName + "/" + holoName + "/";
            List<string> scoreFiles = new List<string>(Directory.GetFiles(importLoc, "*.holo", SearchOption.AllDirectories));
            ConfigNode exportCheck = ConfigNode.Load(importLoc + holoName + "-0-" + OrXHoloKron.instance.groupName + ".holo");

            if (scoreFiles != null)
            {
                OrXHoloKron.instance.HoloKronName = holoName;
                OrXLog.instance.DebugLog("[OrX Check Exports] === FOUND " + scoreFiles.Count + " HOLOKRONS IN " + OrXHoloKron.instance.HoloKronName + " ===");
                return true;
            }
            else
            {
                OrXLog.instance.DebugLog("[OrX Check Exports] === " + holoName + " NOT FOUND ===");
                return false;
            }
        }
        public void SetRanges(float _range)
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

    }
}