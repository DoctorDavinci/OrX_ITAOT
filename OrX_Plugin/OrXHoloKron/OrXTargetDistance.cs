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
        Vector3 startCoords;
        bool showTargetOnScreen = false;
        public double mPerDegree = 0;
        public double degPerMeter = 0;
        float scanDelay = 0;
        float holoHeading = 0;

        Vector3 UpVect;
        Vector3 EastVect;
        Vector3 NorthVect;
        Vector3 targetVect;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }

        void DrawLine(Vector3 start, Vector3 end, float duration)
        {
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("KSP/Emissive/Diffuse"));
            lr.material.SetColor("_EmissiveColor", Color.blue);
            lr.startWidth = 0.05f;
            lr.endWidth = 0.000001f;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            GameObject.Destroy(myLine, duration);
        }

        public void TargetDistance(bool b, bool checking, Vector3d missionCoords)
        {
            StartCoroutine(CheckTargetDistance(b, checking, "", missionCoords));
        }
        IEnumerator CheckTargetDistance(bool b, bool checking, string HoloKronName, Vector3d missionCoords)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                yield return new WaitForFixedUpdate();
                if (checking)
                {
                    mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
                    degPerMeter = 1 / mPerDegree;
                    scanDelay = 5;
                    double targetDistance = 250000;
                    double _latDiff = 0;
                    double _lonDiff = 0;
                    double _altDiff = 0;
                    double _latMission = 0;
                    double _lonMission = 0;
                    double _altMission = 0;
                    _latMission = missionCoords.x;
                    _lonMission = missionCoords.y;
                    _altMission = missionCoords.z;
                    string hcn = "";

                    UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
                    EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
                    NorthVect = Vector3.Cross(EastVect, UpVect).normalized;

                    if (!b)
                    {
                        Debug.Log("[OrX Holo Distance Check] Loading HoloKron Targets ..........");

                        if (OrXHoloKron.instance.OrXCoordsList.Count >= 0)
                        {
                            Debug.Log("[OrX Holo Distance Check] OrX Coords List Count = " + OrXHoloKron.instance.OrXCoordsList.Count + " ..........");

                            List<string>.Enumerator coordinate = OrXHoloKron.instance.OrXCoordsList.GetEnumerator();
                            while (coordinate.MoveNext())
                            {
                                string[] targetHoloKrons = coordinate.Current.Split(new char[] { ':' });
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
                                                HoloKronName = data[1];
                                                _latMission = double.Parse(data[3]);
                                                _lonMission = double.Parse(data[4]);
                                                _altMission = double.Parse(data[5]);
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    Debug.Log("[OrX Load HoloKron Targets] HoloKron data processed ...... ");
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
                                    hcn = HoloKronName;
                                }
                            }
                            coordinate.Dispose();

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
                                                if (data[1] == hcn)
                                                {
                                                    HoloKronName = data[1];
                                                    _latMission = double.Parse(data[3]);
                                                    _lonMission = double.Parse(data[4]);
                                                    _altMission = double.Parse(data[5]);

                                                    if (targetDistance <= 100000)
                                                    {
                                                        startCoords = OrXSpawnHoloKron.instance.WorldPositionToGeoCoords(new Vector3d(_latMission, _lonMission, _altMission), FlightGlobals.currentMainBody);
                                                        OrXLog.instance.RemoveWaypoint(HoloKronName, new Vector3d(_latMission, _lonMission, _altMission));
                                                        OrXLog.instance.AddWaypoint(HoloKronName, new Vector3d(_latMission, _lonMission, _altMission));
                                                        b = true;
                                                        Debug.Log("[OrX Holo Distance Check]  HoloKron Name: " + HoloKronName);
                                                        Debug.Log("[OrX Holo Distance Check]   lat .......... " + _latMission);
                                                        Debug.Log("[OrX Holo Distance Check]   lon .......... " + _lonMission);
                                                        Debug.Log("[OrX Holo Distance Check]   alt .......... " + _altMission);
                                                        Debug.Log("[OrX Holo Distance Check]  Distance: " + targetDistance + " meters");
                                                        Debug.Log("[OrX Holo Distance Check]  Navball Heading: " + holoHeading + " degrees");
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Debug.Log("[OrX Holo Distance Check] === NO HOLOKRON IN RANGE ===");
                                                        OrXHoloKron.instance.targetDistance = 11381138;
                                                        checking = false;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    Debug.Log("[OrX Load HoloKron Targets] HoloKron data processed ...... ");
                                }

                                yield return new WaitForFixedUpdate();
                            }
                            getClosestCoord.Dispose();

                        }
                        else
                        {
                            Debug.Log("[OrX Holo Distance Check] === NO HOLOKRON FOUND ===");
                            OrXHoloKron.instance.targetDistance = 11381138;
                            checking = false;
                        }
                    }
                    else
                    {
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

                        if (_targetDistance <= 2000)
                        {
                            startCoords = OrXSpawnHoloKron.instance.WorldPositionToGeoCoords(new Vector3d(_latMission, _lonMission, _altMission), FlightGlobals.currentMainBody);

                            Debug.Log("[OrX Target Distance - Boid] === TARGET Name: " + HoloKronName);
                            Debug.Log("[OrX Target Distance - Boid] === TARGET Meters per Degree: " + mPerDegree);
                            Debug.Log("[OrX Target Distance - Boid] === TARGET Degrees per Meter: " + degPerMeter);
                            Debug.Log("[OrX Target Distance - Boid] === TARGET Degree offset 2D: " + Math.Sqrt(diffSqr));
                            Debug.Log("[OrX Target Distance - Boid] === TARGET Degree offset 3D: " + Math.Sqrt(altAdded));
                            Debug.Log("[OrX Target Distance - Boid] === TARGET Distance in Meters: " + _targetDistance);

                            OrXLog.instance.RemoveWaypoint(HoloKronName, missionCoords);

                            checking = false;
                            CheckIfHoloSpawned(HoloKronName, startCoords, missionCoords, false, true);
                        }
                    }

                    if (checking)
                    {
                        OrXHoloKron.instance.movingCraft = false;

                        Vector3 targetVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - new Vector3((float)_latMission, (float)_lonMission, (float)_altMission)).normalized;
                        holoHeading = Vector3.Angle(targetVect, FlightGlobals.ActiveVessel.transform.forward);
                        if (Math.Sign(Vector3.Dot(targetVect, FlightGlobals.ActiveVessel.transform.forward)) < 0)
                        {
                            holoHeading = -holoHeading;
                        }

                        OrXHoloKron.instance.holoHeading = holoHeading;
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
                        }
                        else
                        {
                            missionCoords = new Vector3d(_latMission, _lonMission, _altMission);
                            yield return new WaitForFixedUpdate();
                        }
                        StartCoroutine(CheckTargetDistance(b, checking, HoloKronName, missionCoords));
                    }
                    else
                    {

                    }
                }
            }
        }
        public void CheckIfHoloSpawned(string name, Vector3 startCoords, Vector3d vect, bool primary, bool boid)
        {
            bool s = false;
            bool rescan = false;
            double _latDiff = 0;
            double _lonDiff = 0;
            double _altDiff = 0;

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
                    Debug.Log("[OrX Holo Spawn Check] ERROR -" + e + " ...... ");
                }
            }
            v.Dispose();

            if (!s)
            {
                Debug.Log("[OrX Holo Spawn Check] " + name + " has not been spawned ...... SPAWNING");
                if (primary)
                {
                    OrXSpawnHoloKron.instance.StartSpawn(startCoords, vect, boid, false, primary, name, "");
                }
            }
            else
            {
                if (rescan)
                {
                    Debug.Log("[OrX Holo Spawn Check] ERROR - RETRYING SPAWN CHECK ...... ");
                    CheckIfHoloSpawned(name, startCoords, vect, primary, boid);
                }
                else
                {
                    Debug.Log("[OrX Holo Spawn Check] " + name + " has already been spawned ...... ");
                }
            }

            if (!primary && !rescan && boid)
            {
                StartCoroutine(CheckTargetDistanceMission(vect));
            }
        }

        public void StartMissionScan()
        {

        }
        IEnumerator CheckTargetDistanceMission(Vector3d missionCoords)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                double targetDistance = double.MaxValue;
                double _latDiff = 0;
                double _lonDiff = 0;
                double _altDiff = 0;

                if (FlightGlobals.ActiveVessel.altitude <= missionCoords.z)
                {
                    _altDiff = missionCoords.z - FlightGlobals.ActiveVessel.altitude;
                }
                else
                {
                    _altDiff = FlightGlobals.ActiveVessel.altitude - missionCoords.z;
                }

                if (missionCoords.x >= 0)
                {
                    if (FlightGlobals.ActiveVessel.latitude >= missionCoords.x)
                    {
                        _latDiff = FlightGlobals.ActiveVessel.latitude - missionCoords.x;
                    }
                    else
                    {
                        _latDiff = missionCoords.x - FlightGlobals.ActiveVessel.latitude;
                    }
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.latitude >= 0)
                    {
                        _latDiff = FlightGlobals.ActiveVessel.latitude - missionCoords.x;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.latitude <= missionCoords.x)
                        {
                            _latDiff = FlightGlobals.ActiveVessel.latitude - missionCoords.x;
                        }
                        else
                        {

                            _latDiff = missionCoords.x - FlightGlobals.ActiveVessel.latitude;
                        }
                    }
                }

                if (missionCoords.y >= 0)
                {
                    if (FlightGlobals.ActiveVessel.longitude >= missionCoords.y)
                    {
                        _lonDiff = FlightGlobals.ActiveVessel.longitude - missionCoords.y;
                    }
                    else
                    {
                        _lonDiff = missionCoords.y - FlightGlobals.ActiveVessel.longitude;
                    }
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.longitude >= 0)
                    {
                        _lonDiff = FlightGlobals.ActiveVessel.longitude - missionCoords.y;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.longitude <= missionCoords.y)
                        {
                            _lonDiff = FlightGlobals.ActiveVessel.longitude - missionCoords.y;
                        }
                        else
                        {

                            _lonDiff = missionCoords.y - FlightGlobals.ActiveVessel.longitude;
                        }
                    }
                }

                double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                double _altDiffDeg = _altDiff * degPerMeter;
                double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                OrXHoloKron.instance.targetDistance = _targetDistance;

                Vector3 targetVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - new Vector3(Convert.ToSingle(missionCoords.x), Convert.ToSingle(missionCoords.y), Convert.ToSingle(FlightGlobals.ActiveVessel.altitude)).normalized);
                holoHeading = Vector3.Angle(targetVect, NorthVect);
                if (Math.Sign(Vector3.Dot(targetVect, EastVect)) < 0)
                {
                    //holoHeading = 360 - holoHeading;
                }

                if (_targetDistance <= 10)
                {
                    Debug.Log("[OrX Target Distance - Mission] ==== TARGET DISTANCE: " + targetDistance);

                    OrXHoloKron.instance.GetNextCoord();
                }
                else
                {
                    yield return new WaitForFixedUpdate();
                    StartCoroutine(CheckTargetDistanceMission(missionCoords));
                }
            }
        }
    }
}