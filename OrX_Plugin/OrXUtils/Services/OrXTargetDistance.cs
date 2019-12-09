using System;
using UnityEngine;
using System.Collections;
using FinePrint;
using System.Collections.Generic;
using System.Linq;
using OrX.spawn;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXTargetDistance : MonoBehaviour
    {
        public static OrXTargetDistance instance;
        public double mPerDegree = 0;
        public double degPerMeter = 0;
        public float scanDelay = 0;

        double targetDistance = 250000;
        double _latDiff = 0;
        double _lonDiff = 0;
        double _altDiff = 0;
        double _latMission = 0;
        double _lonMission = 0;
        double _altMission = 0;
        bool _checking = true;
        bool _airsupportSpawned = false;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }

        public void TargetDistance(bool primary, bool b, bool Goal, bool checking, string HoloKronName, Vector3d missionCoords)
        {
            _airsupportSpawned = false;
            _checking = checking;
            OrXHoloKron.instance.airTime = 0;

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
                    }
                    StartCoroutine(CheckTargetDistance(primary, b, Goal, checking, HoloKronName, missionCoords));
                }

            }
        }
        IEnumerator CheckTargetDistance(bool primary, bool b, bool Goal, bool checking, string HoloKronName, Vector3d missionCoords)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                yield return new WaitForFixedUpdate();
                mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
                degPerMeter = 1 / mPerDegree;
                scanDelay = 5;
                //string hcn = "";
                int coordCount = 0;

                if (!b)
                {
                    OrXLog.instance.DebugLog("[OrX Holo Distance Check] Loading HoloKron Targets ..........");

                    if (OrXHoloKron.instance.OrXCoordsList.Count >= 0)
                    {
                        OrXLog.instance.DebugLog("[OrX Holo Distance Check] OrX Coords List Count = " + OrXHoloKron.instance.OrXCoordsList.Count + " ..........");

                        List<string>.Enumerator coordinate = OrXHoloKron.instance.OrXCoordsList.GetEnumerator();
                        while (coordinate.MoveNext())
                        {
                            try
                            {
                                OrXLog.instance.DebugLog("[OrX Holo Distance Check] Checking: " + coordinate.Current + " ..........");

                                string[] targetHoloKrons = coordinate.Current.Split(new char[] { ':' });

                                if (targetHoloKrons[0] != null && targetHoloKrons[0].Length > 0 && targetHoloKrons[0] != "null")
                                {
                                    coordCount += 1;

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
                                    OrXLog.instance.DebugLog("[OrX Holo Distance Check] " + coordinate.Current + " was empty ..........");

                                }
                            }
                            catch
                            {
                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] HoloKron data processed ...... ");
                            }

                            yield return new WaitForFixedUpdate();

                            if (FlightGlobals.ActiveVessel.altitude <= _altMission)
                            {
                                _altDiff = _altMission - FlightGlobals.ActiveVessel.altitude;
                            }
                            else
                            {
                                _altDiff = FlightGlobals.ActiveVessel.altitude - _altMission;
                            }

                            if (_latMission >= 0)
                            {
                                if (FlightGlobals.ActiveVessel.latitude >= _latMission)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - _latMission;
                                }
                                else
                                {
                                    _latDiff = _latMission - FlightGlobals.ActiveVessel.latitude;
                                }
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.latitude >= 0)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - _latMission;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.latitude <= _latMission)
                                    {
                                        _latDiff = FlightGlobals.ActiveVessel.latitude - _latMission;
                                    }
                                    else
                                    {

                                        _latDiff = _latMission - FlightGlobals.ActiveVessel.latitude;
                                    }
                                }
                            }

                            if (_lonMission >= 0)
                            {
                                if (FlightGlobals.ActiveVessel.longitude >= _lonMission)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - _lonMission;
                                }
                                else
                                {
                                    _lonDiff = _lonMission - FlightGlobals.ActiveVessel.latitude;
                                }
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.longitude >= 0)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - _lonMission;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.longitude <= _lonMission)
                                    {
                                        _lonDiff = FlightGlobals.ActiveVessel.longitude - _lonMission;
                                    }
                                    else
                                    {

                                        _lonDiff = _lonMission - FlightGlobals.ActiveVessel.longitude;
                                    }
                                }
                            }

                            double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                            double _altDiffDeg = _altDiff * degPerMeter;
                            double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                            double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                            if (targetDistance >= _targetDistance)
                            {
                                targetDistance = _targetDistance;
                                //hcn = HoloKronName;
                            }
                        }
                        coordinate.Dispose();

                        OrXLog.instance.DebugLog("[OrX Target Distance] === HOLOKRONS FOUND: " + coordCount);


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
                                            if (data[1] == HoloKronName)
                                            {
                                                //HoloKronName = data[1];
                                                _latMission = double.Parse(data[3]);
                                                _lonMission = double.Parse(data[4]);
                                                _altMission = double.Parse(data[5]);

                                                if (targetDistance <= 100000)
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Target Distance] === TARGET Name: " + HoloKronName);
                                                    OrXLog.instance.DebugLog("[OrX Target Distance] === _latMission: " + _latMission);
                                                    OrXLog.instance.DebugLog("[OrX Target Distance] === _lonMission: " + _lonMission);
                                                    OrXLog.instance.DebugLog("[OrX Target Distance] === _altMission: " + _altMission);
                                                    OrXLog.instance.DebugLog("[OrX Target Distance] === TARGET Distance in Meters: " + targetDistance);
                                                    b = true;
                                                }
                                                else
                                                {
                                                    OrXLog.instance.DebugLog("[OrX Holo Distance Check] === NO HOLOKRONS IN RANGE ===");
                                                    OrXHoloKron.instance.targetDistance = 1138.8311;
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
                                OrXLog.instance.DebugLog("[OrX Load HoloKron Targets] HoloKron data processed ...... ");
                            }

                            yield return new WaitForFixedUpdate();
                        }
                        getClosestCoord.Dispose();
                    }
                    else
                    {
                        OrXLog.instance.DebugLog("[OrX Holo Distance Check] === NO HOLOKRON FOUND ===");
                        OrXHoloKron.instance.targetDistance = 1138.1138;
                        checking = false;
                        OrXHoloKron.instance.checking = false;
                        OrXHoloKron.instance.MainMenu();
                        ScreenMessages.PostScreenMessage(new ScreenMessage("There are no HoloKrons within " + FlightGlobals.currentMainBody.name + "'s SOI", 4, ScreenMessageStyle.UPPER_CENTER));
                    }
                }
                else
                {
                    coordCount += 1;

                    if (FlightGlobals.ActiveVessel.altitude <= _altMission)
                    {
                        _altDiff = _altMission - FlightGlobals.ActiveVessel.altitude;
                    }
                    else
                    {
                        _altDiff = FlightGlobals.ActiveVessel.altitude - _altMission;
                    }

                    if (_latMission >= 0)
                    {
                        if (FlightGlobals.ActiveVessel.latitude >= _latMission)
                        {
                            _latDiff = FlightGlobals.ActiveVessel.latitude - _latMission;
                        }
                        else
                        {
                            _latDiff = _latMission - FlightGlobals.ActiveVessel.latitude;
                        }
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.latitude >= 0)
                        {
                            _latDiff = FlightGlobals.ActiveVessel.latitude - _latMission;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.latitude <= _latMission)
                            {
                                _latDiff = FlightGlobals.ActiveVessel.latitude - _latMission;
                            }
                            else
                            {

                                _latDiff = _latMission - FlightGlobals.ActiveVessel.latitude;
                            }
                        }
                    }

                    if (_lonMission >= 0)
                    {
                        if (FlightGlobals.ActiveVessel.longitude >= _lonMission)
                        {
                            _lonDiff = FlightGlobals.ActiveVessel.longitude - _lonMission;
                        }
                        else
                        {
                            _lonDiff = _lonMission - FlightGlobals.ActiveVessel.latitude;
                        }
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.longitude >= 0)
                        {
                            _lonDiff = FlightGlobals.ActiveVessel.longitude - _lonMission;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.longitude <= _lonMission)
                            {
                                _lonDiff = FlightGlobals.ActiveVessel.longitude - _lonMission;
                            }
                            else
                            {

                                _lonDiff = _lonMission - FlightGlobals.ActiveVessel.longitude;
                            }
                        }
                    }

                    double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                    double _altDiffDeg = _altDiff * degPerMeter;
                    double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                    double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                    targetDistance = _targetDistance;

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

                    if (_targetDistance <= 20000)
                    {
                        if (checking)
                        {
                            Vector3d stageStartCoords = OrXSpawnHoloKron.instance.WorldPositionToGeoCoords(new Vector3d(_latMission, _lonMission, _altMission), FlightGlobals.currentMainBody);

                            OrXLog.instance.DebugLog("[OrX Target Distance - Goal] === TARGET Name: " + HoloKronName);
                            OrXLog.instance.DebugLog("[OrX Target Distance - Goal] === TARGET Distance in Meters: " + _targetDistance);
                            checking = false;
                            CheckIfHoloSpawned(HoloKronName, stageStartCoords, missionCoords, primary, Goal);
                        }
                        else
                        {
                            if (_targetDistance <= 20)
                            {
                                OrXHoloKron.instance.checking = false;

                                if (!OrXHoloKron.instance._showTimer)
                                {
                                    OrXHoloKron.instance.OrXHCGUIEnabled = false;
                                    OrXHoloKron.instance.MainMenu();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_targetDistance <= 60000 && !_airsupportSpawned)
                        {
                            _airsupportSpawned = true;
                            OrXSpawnHoloKron.instance.SpawnAirSupport(true, HoloKronName, new Vector3d());
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

                if (OrXHoloKron.instance.checking)
                {
                    //OrXHoloKron.instance.movingCraft = false;
                    OrXHoloKron.instance.targetDistance = targetDistance;
                    OrXHoloKron.instance._altitude = _altMission;

                    if (!b)
                    {
                        scanDelay = Convert.ToSingle(targetDistance / FlightGlobals.ActiveVessel.srfSpeed) / 10;
                        if (scanDelay >= 5)
                        {
                            scanDelay = 5;
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
                        missionCoords = new Vector3d(_latMission, _lonMission, _altMission);
                        yield return new WaitForFixedUpdate();
                        StartCoroutine(CheckTargetDistance(primary, b, Goal, checking, HoloKronName, missionCoords));
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
            double _latDiff = 0;
            double _lonDiff = 0;
            double _altDiff = 0;
            OrXLog.instance.DebugLog("[OrX Target Distance - Spawn Check] === Checking if spawned ===");

            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
            while (v.MoveNext())
            {
                try
                {
                    if (v.Current != null && v.Current.loaded && !v.Current.packed)
                    {
                        if (v.Current.name == name)
                        {
                            if (vect.z <= v.Current.altitude)
                            {
                                _altDiff = v.Current.altitude - vect.z;
                            }
                            else
                            {
                                _altDiff = vect.z - v.Current.altitude;
                            }

                            if (v.Current.altitude >= 0)
                            {
                                if (vect.x >= v.Current.latitude)
                                {
                                    _latDiff = vect.x - v.Current.latitude;
                                }
                                else
                                {
                                    _latDiff = v.Current.latitude - vect.x;
                                }
                            }
                            else
                            {
                                if (vect.x >= 0)
                                {
                                    _latDiff = vect.x - v.Current.latitude;
                                }
                                else
                                {
                                    if (vect.x <= v.Current.latitude)
                                    {
                                        _latDiff = vect.x - v.Current.latitude;
                                    }
                                    else
                                    {

                                        _latDiff = v.Current.latitude - vect.x;
                                    }
                                }
                            }

                            if (v.Current.longitude >= 0)
                            {
                                if (vect.y >= v.Current.longitude)
                                {
                                    _lonDiff = vect.y - v.Current.longitude;
                                }
                                else
                                {
                                    _lonDiff = v.Current.longitude - vect.y;
                                }
                            }
                            else
                            {
                                if (vect.y >= 0)
                                {
                                    _lonDiff = vect.y - v.Current.longitude;
                                }
                                else
                                {
                                    if (vect.y <= v.Current.longitude)
                                    {
                                        _lonDiff = vect.y - v.Current.longitude;
                                    }
                                    else
                                    {

                                        _lonDiff = v.Current.latitude - vect.y;
                                    }
                                }
                            }

                            double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                            double _altDiffDeg = _altDiff * degPerMeter;
                            double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                            double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                            if (_targetDistance <= 5)
                            {
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
                if (primary)
                {
                    OrXSpawnHoloKron.instance.StartSpawn(stageStartCoords, vect, false, false, primary, HoloKronName, OrXHoloKron.instance.challengeType);
                }
                else
                {
                    if (OrXHoloKron.instance.LBC)
                    {
                        OrXSpawnHoloKron.instance.StartSpawn(stageStartCoords, vect, true, false, false, HoloKronName, OrXHoloKron.instance.challengeType);
                    }
                    else
                    {
                        OrXSpawnHoloKron.instance.StartSpawn(stageStartCoords, vect, true, false, false, HoloKronName, OrXHoloKron.instance.challengeType);
                    }
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