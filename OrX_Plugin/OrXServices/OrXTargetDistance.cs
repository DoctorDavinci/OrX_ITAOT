using System;
using UnityEngine;
using System.Collections;
using FinePrint;
using System.Collections.Generic;
using System.Linq;
using OrX.spawn;
using System.Device.Location;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXTargetDistance : MonoBehaviour
    {
        public static OrXTargetDistance instance;
        public float scanDelay = 0;
        public double _radius = 0;
        public double targetDistance = 0;
        double _latMission = 0;
        double _lonMission = 0;
        double _altMission = 0;
        bool _checking = true;
        public bool _airsupportSpawned = false;
        public float _wmActivateDelay = 0;
        public bool _randomSpawned = false;
        bool _continue = false;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }

        public void TargetDistance(bool primary, bool b, bool Goal, bool checking, string HoloKronName, Vector3d missionCoords)
        {
            _continue = true;
            OrXVesselLog.instance._playerCraft = new List<Vessel>();
            OrXLog.instance.DebugLog("[OrX Spawn Air Support] === Player vessel list initialized === ");
            if (OrXHoloKron.instance.bdaChallenge)
            {
                OrXVesselLog.instance.ResetLog();
            }

            try
            {
                List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
                while (v.MoveNext())
                {
                    if (v.Current != null)
                    {
                        bool _owned = false;
                        List<Part>.Enumerator _parts = v.Current.parts.GetEnumerator();
                        while (_parts.MoveNext())
                        {
                            if (_parts.Current != null)
                            {
                                if (_parts.Current.Modules.Contains<ModuleOrXWMI>())
                                {
                                    var _wmi = _parts.Current.FindModuleImplementing<ModuleOrXWMI>();
                                    if (_wmi != null)
                                    {
                                        if (_wmi._owned)
                                        {
                                            _owned = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            _parts.Dispose();
                        }

                        if (_owned && !v.Current.vesselName.Contains("Debris"))
                        {
                            OrXLog.instance.DebugLog("[OrX Target Distance] === Adding " + v.Current.vesselName + " to player vessel list === ");
                            OrXVesselLog.instance.AddToPlayerVesselList(v.Current);
                        }
                    }
                }
                v.Dispose();
            }
            catch
            {
                _continue = false;
                TargetDistance(primary, b, Goal, checking, HoloKronName, missionCoords);
            }

            if (_continue)
            {
                _radius = FlightGlobals.ActiveVessel.mainBody.Radius / 1000;
                _randomSpawned = false;
                _airsupportSpawned = false;

                _randomSpawned = false;
                _airsupportSpawned = false;
                _checking = checking;
                OrXHoloKron.instance.airTime = 0;
                _wmActivateDelay = 0;
                OrXHoloKron.instance._getCenterDist = false;
                if (!OrXHoloKron.instance.buildingMission)
                {
                    if (OrXLog.instance._preInstalled)
                    {
                        if (!OrXLog.instance.PREnabled())
                        {
                            if (b)
                            {
                                OrXHoloKron.instance.showTargets = true;

                                _latMission = missionCoords.x;
                                _lonMission = missionCoords.y;
                                _altMission = missionCoords.z;
                            }
                            else
                            {
                                OrXHoloKron.instance.showTargets = false;
                                OrXHoloKron.instance.challengeRunning = false;
                                OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                                OrXHoloKron.instance.checking = true;
                                OrXHoloKron.instance.movingCraft = false;
                            }
                            StartCoroutine(CheckTargetDistance(primary, b, Goal, checking, HoloKronName, missionCoords));
                        }
                    }
                    else
                    {

                        if (b)
                        {
                            OrXHoloKron.instance.showTargets = true;

                            _latMission = missionCoords.x;
                            _lonMission = missionCoords.y;
                            _altMission = missionCoords.z;
                        }
                        else
                        {
                            OrXHoloKron.instance.showTargets = false;
                            OrXHoloKron.instance.challengeRunning = false;
                            OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                            OrXHoloKron.instance.checking = true;
                            OrXHoloKron.instance.movingCraft = false;
                        }
                        StartCoroutine(CheckTargetDistance(primary, b, Goal, checking, HoloKronName, missionCoords));
                    }
                }
            }
        }
        IEnumerator CheckTargetDistance(bool primary, bool b, bool Goal, bool checking, string HoloKronName, Vector3d missionCoords)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                yield return new WaitForFixedUpdate();
                scanDelay = 5;
                int coordCount = 0;

                if (!b)
                {
                    OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] Loading HoloKron Targets ..........");

                    if (OrXHoloKron.instance.OrXCoordsList.Count >= 0)
                    {
                        OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] OrX Coords List Count = " + OrXHoloKron.instance.OrXCoordsList.Count + " ..........");
                        OrXHoloKron.instance.checking = true;
                        coordCount = OrXHoloKron.instance.OrXCoordsList.Count;
                        string _holoName = "";

                        List<string>.Enumerator coordinate = OrXHoloKron.instance.OrXCoordsList.GetEnumerator();
                        while (coordinate.MoveNext())
                        {
                            try
                            {
                                OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] Checking: " + coordinate.Current + " ..........");

                                string[] targetHoloKrons = coordinate.Current.Split(new char[] { ':' });

                                if (targetHoloKrons[0] != null && targetHoloKrons[0].Length > 0 && targetHoloKrons[0] != "null")
                                {
                                    string[] TargetCoords = targetHoloKrons[0].Split(new char[] { ';' });
                                    for (int i = 0; i < TargetCoords.Length; i++)
                                    {
                                        if (TargetCoords[i] != null && TargetCoords[i].Length > 0)
                                        {
                                            string[] data = TargetCoords[i].Split(new char[] { ',' });
                                            if (data[7] != "CHALLENGE")
                                            {
                                                HoloKronName = data[1];
                                                _latMission = double.Parse(data[3]);
                                                _lonMission = double.Parse(data[4]);
                                                _altMission = double.Parse(data[5]);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] " + coordinate.Current + " was empty ..........");
                                }
                            }
                            catch
                            {
                                //OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] HoloKron data processed ...... ");
                            }

                            yield return new WaitForFixedUpdate();

                            double _targetDistance = OrXUtilities.instance.GetDistance(FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.latitude, _lonMission, _latMission, (FlightGlobals.ActiveVessel.altitude + _altMission) / 2);

                            if (targetDistance >= _targetDistance)
                            {
                                targetDistance = _targetDistance;
                                OrXHoloKron.instance.targetDistance = _targetDistance;
                                _holoName = HoloKronName;
                            }
                        }
                        coordinate.Dispose();

                        OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] === HOLOKRONS FOUND: " + coordCount);

                        List<string>.Enumerator getClosestCoord = OrXHoloKron.instance.OrXCoordsList.GetEnumerator();
                        while (getClosestCoord.MoveNext())
                        {
                            string[] targetHoloKrons = getClosestCoord.Current.Split(new char[] { ':' });
                            try
                            {
                                if (targetHoloKrons[0] != null && targetHoloKrons[0].Length > 0 && targetHoloKrons[0] != "null")
                                {
                                    string[] TargetCoords = targetHoloKrons[0].Split(new char[] { ';' });
                                    for (int i = 0; i < TargetCoords.Length; i++)
                                    {
                                        if (TargetCoords[i] != null && TargetCoords[i].Length > 0)
                                        {
                                            string[] data = TargetCoords[i].Split(new char[] { ',' });
                                            if (data[1] == _holoName)
                                            {
                                                _latMission = double.Parse(data[3]);
                                                _lonMission = double.Parse(data[4]);
                                                _altMission = double.Parse(data[5]);

                                                if (targetDistance <= 5000)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] === " + _holoName + " Distance in Meters: " + targetDistance);
                                                    missionCoords = new Vector3d(_latMission, _lonMission, _altMission);
                                                    checking = true;
                                                    _continue = false;
                                                    b = true;
                                                    OrXHoloKron.instance.checking = false;
                                                    OrXHoloKron.instance.Reach();
                                                    CheckIfHoloSpawned(_holoName, missionCoords, missionCoords, true, false);
                                                }
                                                else
                                                {
                                                    //OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] === NO Geo-Cache IN RANGE ===");
                                                    scanDelay = 5;
                                                    b = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                //OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] HoloKron data processed ...... ");
                            }

                            yield return new WaitForFixedUpdate();
                        }
                        getClosestCoord.Dispose();
                    }
                    else
                    {
                        OrXLog.instance.DebugLog("[OrX Geo-Cache Distance] === NO Geo-Cache FOUND ===");
                        OrXHoloKron.instance.targetDistance = 1138.1138;
                        checking = false;
                        OrXHoloKron.instance.checking = false;
                        OrXHoloKron.instance.MainMenu();
                        ScreenMessages.PostScreenMessage(new ScreenMessage("There are no Geo-Cache's within " + FlightGlobals.currentMainBody.name + "'s SOI", 4, ScreenMessageStyle.UPPER_CENTER));
                    }
                }
                else
                {
                    coordCount += 1;
                    targetDistance = OrXUtilities.instance.GetDistance(FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.latitude, _lonMission, _latMission, (FlightGlobals.ActiveVessel.altitude + _altMission) / 2); 
                    OrXHoloKron.instance.targetDistance = targetDistance;

                    if (OrXHoloKron.instance.challengeRunning)
                    {
                        if (FlightGlobals.ActiveVessel.srfSpeed >= OrXHoloKron.instance.topSurfaceSpeed)
                        {
                            OrXHoloKron.instance.topSurfaceSpeed = FlightGlobals.ActiveVessel.srfSpeed;
                        }

                        if (!FlightGlobals.ActiveVessel.LandedOrSplashed)
                        {
                            OrXHoloKron.instance.airTime += Time.fixedDeltaTime;
                        }
                    }

                    if (targetDistance <= 20000)
                    {
                        if (checking)
                        {
                            if (OrXHoloKron.instance.bdaChallenge)
                            {
                                if (OrXVesselLog.instance._playerCraft.Contains(FlightGlobals.ActiveVessel))
                                {
                                    Vector3d stageStartCoords = OrXUtilities.instance.WorldPositionToGeoCoords(new Vector3d(_latMission, _lonMission, _altMission), FlightGlobals.currentMainBody);
                                    OrXHoloKron.instance.checking = false;
                                    OrXLog.instance.DebugLog("[OrX Target Distance - Goal] === TARGET Name: " + HoloKronName);
                                    OrXLog.instance.DebugLog("[OrX Target Distance - Goal] === TARGET Distance in Meters: " + targetDistance);
                                    checking = false;
                                    CheckIfHoloSpawned(HoloKronName, stageStartCoords, missionCoords, primary, Goal);
                                }
                            }
                            else
                            {
                                if (_continue)
                                {
                                    if (targetDistance <= 4000)
                                    {
                                        _continue = false;
                                        Vector3d stageStartCoords = OrXUtilities.instance.WorldPositionToGeoCoords(new Vector3d(_latMission, _lonMission, _altMission), FlightGlobals.currentMainBody);
                                        OrXLog.instance.DebugLog("[OrX HoloKron Distance] === " + HoloKronName + " Distance in Meters: " + targetDistance);
                                        CheckIfHoloSpawned(HoloKronName, stageStartCoords, missionCoords, primary, Goal);
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {
                            if (targetDistance <= 50 && OrXHoloKron.instance.checking)
                            {
                                OrXHoloKron.instance.checking = false;

                                if (!OrXHoloKron.instance.bdaChallenge)
                                {
                                    OrXHoloKron.instance.OrXHCGUIEnabled = false;
                                    OrXHoloKron.instance.MainMenu();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (OrXHoloKron.instance.bdaChallenge && !_airsupportSpawned)
                        {
                            try
                            {
                                bool _continue = false;

                                List<Vessel>.Enumerator _playerVessels = OrXVesselLog.instance._playerCraft.GetEnumerator();
                                while (_playerVessels.MoveNext())
                                {
                                    if (_playerVessels.Current != null)
                                    {
                                        if (targetDistance <= 60000)
                                        {
                                            if (_playerVessels.Current.altitude >= _playerVessels.Current.radarAltitude)
                                            {
                                                if (_playerVessels.Current.radarAltitude >= targetDistance / 100)
                                                {
                                                    _randomSpawned = true;
                                                    _continue = true;
                                                }
                                            }
                                            else
                                            {
                                                if (_playerVessels.Current.altitude >= targetDistance / 100)
                                                {
                                                    _randomSpawned = true;
                                                    _continue = true;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (OrXSpawnHoloKron.instance._interceptorCount != 0 && !_randomSpawned)
                                            {
                                                if (targetDistance <= 100000)
                                                {
                                                    double spawnChance;

                                                    if (_playerVessels.Current.altitude >= _playerVessels.Current.radarAltitude)
                                                    {
                                                        spawnChance = (((100000 - targetDistance) / 10000) * OrXSpawnHoloKron.instance._interceptorCount) * 5;
                                                        if (_playerVessels.Current.radarAltitude <= spawnChance)
                                                        {
                                                            _continue = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        spawnChance = (((100000 - targetDistance) / 10000) * OrXSpawnHoloKron.instance._interceptorCount) * 5;
                                                        if (_playerVessels.Current.altitude <= spawnChance)
                                                        {
                                                            _continue = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                _playerVessels.Dispose();

                                if (_continue)
                                {
                                    if (!_randomSpawned)
                                    {
                                        _randomSpawned = true;
                                        OrXLog.instance.DebugLog("[OrX Target Distance - Spawn Random Air Support] === TARGET Distance in Meters: " + targetDistance);
                                        _wmActivateDelay = (100000 - ((float)targetDistance)) * 2;
                                        OrXSpawnHoloKron.instance.SpawnRandomAirSupport(_wmActivateDelay);
                                    }
                                    else
                                    {
                                        OrXLog.instance.DebugLog("[OrX Target Distance - Spawn Air Support] === TARGET Distance in Meters: " + targetDistance);
                                        _airsupportSpawned = true;
                                        _wmActivateDelay = 60000 - ((float)targetDistance);
                                        OrXSpawnHoloKron.instance.SpawnAirSupport(false, true, HoloKronName, new Vector3d(), _wmActivateDelay);
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }

                if (coordCount == 0)
                {
                    OrXLog.instance.DebugLog("[OrX Holo Distance Check] === NO HOLOKRON IN RANGE ===");
                    OrXHoloKron.instance.targetDistance = 1138.1138;
                    //checking = false;
                    OrXHoloKron.instance.OnScrnMsgUC("No HoloKrons in range ......");
                    scanDelay = 5;
                    OrXHoloKron.instance.checking = false;
                    OrXHoloKron.instance.MainMenu();
                }
                yield return new WaitForFixedUpdate();

                if (OrXHoloKron.instance.checking)
                {
                    if (!b)
                    {
                        scanDelay = Convert.ToSingle(targetDistance / FlightGlobals.ActiveVessel.srfSpeed) / 10;
                        if (scanDelay >= 10)
                        {
                            scanDelay = 10;
                        }

                        while (scanDelay >= 0)
                        {
                            yield return new WaitForSeconds(1);
                            scanDelay -= 1;
                            OrXHoloKron.instance.scanDelay = scanDelay;
                        }
                        StartCoroutine(CheckTargetDistance(primary, b, Goal, checking, HoloKronName, missionCoords));
                    }
                    else
                    {
                        if (targetDistance >= 25)
                        {
                            yield return new WaitForFixedUpdate();
                            StartCoroutine(CheckTargetDistance(primary, true, Goal, checking, HoloKronName, missionCoords));
                        }
                        else
                        {
                            OrXHoloKron.instance.checking = false;
                        }
                    }
                }
                else
                {
                }
            }
        }

        public void CheckIfHoloSpawned(string HoloKronName, Vector3d stageStartCoords, Vector3d vect, bool primary, bool Goal)
        {
            bool s = false;
            bool rescan = false;
            List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
            while (v.MoveNext())
            {
                try
                {
                    if (v.Current != null && v.Current.loaded && !v.Current.packed)
                    {
                        if (v.Current.vesselName == HoloKronName)
                        {
                            if (OrXUtilities.instance.GetDistance(FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.latitude, _lonMission, _latMission, (FlightGlobals.ActiveVessel.altitude + _altMission) / 2) <= 10)
                            {
                                OrXLog.instance.DebugLog("[OrX Target Distance - Spawn Check] === Vessel found ... unable to spawn ===");
                                s = true;
                                break;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    rescan = true;
                    OrXLog.instance.DebugLog("[OrX Holo Spawn Check] ERROR -" + e + " ...... ");
                }
            }
            v.Dispose();

            if (!s)
            {
                OrXHoloKron.instance.showTargets = false;
                OrXLog.instance.DebugLog("[OrX Holo Spawn Check] " + HoloKronName + " " + OrXHoloKron.instance.hkCount + " has not been spawned ...... SPAWNING");
                
                if (Goal)
                {
                    //OrXHoloKron.instance.GetShortTrackCenter(new Vector3d(vect.x, vect.y, vect.z));
                }

                if (primary)
                {
                    OrXSpawnHoloKron.instance.StartSpawn(stageStartCoords, vect, false, false, true, HoloKronName, OrXHoloKron.instance.challengeType);
                }
                else
                {
                    OrXSpawnHoloKron.instance.StartSpawn(stageStartCoords, vect, true, false, false, HoloKronName, OrXHoloKron.instance.challengeType);
                }
            }
            else
            {
                if (rescan)
                {
                    OrXLog.instance.DebugLog("[OrX Holo Spawn Check] ERROR - RETRYING SPAWN CHECK ...... ");
                    CheckIfHoloSpawned(HoloKronName, stageStartCoords, vect, primary, Goal);
                }
                else
                {
                    OrXLog.instance.DebugLog("[OrX Holo Spawn Check] " + HoloKronName + " " + OrXHoloKron.instance.hkCount + " has already been spawned ...... ");
                }
            }
        }
    }
}