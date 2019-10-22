using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrX.spawn
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXVesselMove : MonoBehaviour
    {
        #region Declarations

        public static OrXVesselMove Instance;

        public enum MoveModes { Slow = 0, Normal = 1, Ludicrous = 2 }

        public MoveModes _moveMode = MoveModes.Slow;
        private bool _moving = false;
        public List<Vessel> _placingVessels = new List<Vessel>();
        private bool _hoverChanged;

        public float MoveHeight = 0;
        private float _hoverAdjust = 0f;

        public float MoveSpeed = 25;
        public float MoveAccel = 15;

        private readonly float[] _rotationSpeeds = new float[] {5, 25, 50 };

        private float RotationSpeed
        {
            get
            {
                return _rotationSpeeds[(int)_moveMode] * Time.fixedDeltaTime;
            }
        }

        public bool IsMovingVessel = false;
        public Vessel MovingVessel;
        private Quaternion _startRotation;
        private Quaternion _currRotation;
        private float _currMoveSpeed = 0;
        private Vector3 _currMoveVelocity;
        private VesselBounds _vBounds;
        private LineRenderer _debugLr;
        private Vector3 _up;
        private Vector3 _startingUp;
        private readonly float maxPlacementSpeed = 1050;
        private bool _hasRotated = false;
        private float _timeBoundsUpdated = 0;
        private ScreenMessage _moveMessage;
        public bool placingFinishGate = false;
        #endregion

        #region KSP Events

        private void Awake()
        {
            if (Instance)
            {
                Destroy(Instance);
            }
            Instance = this;
        }

        private void Start()
        {
            _debugLr = new GameObject().AddComponent<LineRenderer>();
            _debugLr.material = new Material(Shader.Find("KSP/Emissive/Diffuse"));
            _debugLr.material.SetColor("_EmissiveColor", Color.green);
            _debugLr.startWidth = 0.05f;
            _debugLr.endWidth = 0.05f;
            _debugLr.enabled = false;
        }

        private void Update()
        {
            if (_moving && MovingVessel == OrXHoloKron.instance._HoloKron)
            {
                int circleRes = 24;

                Vector3[] positions = new Vector3[circleRes + 3];
                for (int i = 0; i < circleRes; i++)
                {
                    positions[i] = GetBoundPoint(i, circleRes, 1);
                }
                positions[circleRes] = GetBoundPoint(0, circleRes, 1);
                positions[circleRes + 1] = MovingVessel.CoM;
                positions[circleRes + 2] = MovingVessel.CoM + (MoveHeight * -_up);

                _debugLr.positionCount = circleRes + 3;
                _debugLr.SetPositions(positions);

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (MoveHeight <= 11)
                    {
                        _debugLr.material.SetColor("_EmissiveColor", XKCDColors.AquaGreen);
                        MoveHeight = 35;
                        MoveSpeed = 25;
                        MoveAccel = 15;
                    }
                    else
                    {
                        if (MoveHeight <= 36)
                        {
                            _debugLr.material.SetColor("_EmissiveColor", XKCDColors.BluePurple);
                            MoveHeight = 3000;
                            MoveSpeed = 750;
                            MoveAccel = 500;
                        }
                        else
                        {
                            if (MoveHeight >= 36)
                            {
                                _debugLr.material.SetColor("_EmissiveColor", XKCDColors.AcidGreen);
                                MoveHeight = 10;
                                MoveSpeed = 5;
                                MoveAccel = 5;
                            }
                        }
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_moving) return;
            if (MovingVessel == null)
            {
                _moving = false;
                MoveHeight = 0;
                _hoverAdjust = 0f;
                MovingVessel = new Vessel();
            }
            MovingVessel.IgnoreGForces(240);
            UpdateMove();

            if (_hasRotated && Time.time - _timeBoundsUpdated > 0.2f)
            {
                //UpdateBounds();
            }
        }

        #endregion

        float _mhSub = 0;

        private void UpdateMove()
        {
            MovingVessel.IgnoreGForces(240);

            // Lerp is animating move
            if (!_hoverChanged)
            {
                //MoveHeight = Mathf.Lerp(MoveHeight, Convert.ToSingle(MovingVessel.radarAltitude) - _alt, 10 * Time.fixedDeltaTime);// + HoverHeight, 10 * Time.fixedDeltaTime);
            }
            else
            {
                _alt = MoveHeight;

                if (MoveHeight < _alt) MoveHeight = (float)_alt;
                if (MoveHeight > _alt) MoveHeight = (float)_alt;
                MoveHeight = Mathf.Lerp(MoveHeight, (float)_alt + _hoverAdjust, 10 * Time.fixedDeltaTime);
            }
            MovingVessel.ActionGroups.SetGroup(KSPActionGroup.RCS, false);

            _hoverAdjust = 0;

            _up = (MovingVessel.transform.position - FlightGlobals.currentMainBody.transform.position).normalized;

            Vector3 forward;
            if (MapView.MapIsEnabled)
            {
                forward = North();
            }
            else
            {
                forward = Vector3.ProjectOnPlane(MovingVessel.CoM - FlightCamera.fetch.mainCamera.transform.position, _up).normalized;
                if (Vector3.Dot(-_up, FlightCamera.fetch.mainCamera.transform.up) > 0)
                {
                    forward = Vector3.ProjectOnPlane(FlightCamera.fetch.mainCamera.transform.up, _up).normalized;
                }
            }

            Vector3 right = Vector3.Cross(_up, forward);

            Vector3 offsetDirection = Vector3.zero;
            bool inputting = false;
            float _moveSpeed = 0;

            //Altitude Adjustment

            if (!OrXHoloKron.instance.spawningStartGate)
            {
                _moveSpeed = MoveSpeed;

                if (GameSettings.PITCH_DOWN.GetKey())
                {
                    offsetDirection += (forward * _moveSpeed * Time.fixedDeltaTime);
                    inputting = true;
                }
                if (GameSettings.PITCH_UP.GetKey())
                {
                    offsetDirection += (-forward * _moveSpeed * Time.fixedDeltaTime);
                    inputting = true;
                }

                if (GameSettings.YAW_RIGHT.GetKey())
                {
                    offsetDirection += (right * _moveSpeed * Time.fixedDeltaTime);
                    inputting = true;
                }
                if (GameSettings.YAW_LEFT.GetKey())
                {
                    offsetDirection += (-right * _moveSpeed * Time.fixedDeltaTime);
                    inputting = true;
                }


                /*
                 * 

                if (GameSettings.THROTTLE_UP.GetKey())
                {
                    _hoverAdjust += _moveSpeed * Time.fixedDeltaTime;
                    inputting = true;
                    _hoverChanged = true;
                }

                if (GameSettings.THROTTLE_DOWN.GetKey())
                {
                    _hoverAdjust += -(_moveSpeed * Time.fixedDeltaTime);
                    inputting = true;
                    _hoverChanged = true;
                }
                */
            }

            if (GameSettings.THROTTLE_UP.GetKey())
            {
                if (_mhSub == 0 && MovingVessel.radarAltitude <= 11)
                {
                    if (placingFinishGate)
                    {
                        _hoverAdjust += 0.1f;
                    }
                }
            }

            if (GameSettings.THROTTLE_DOWN.GetKey())
            {
                if (_mhSub == 0 && MovingVessel.radarAltitude <= 11)
                {
                    if (placingFinishGate)
                    {
                        _hoverAdjust -= 0.1f;
                    }
                }
            }

            if (GameSettings.TRANSLATE_RIGHT.GetKey())
            {
                _startRotation = Quaternion.AngleAxis(-RotationSpeed, MovingVessel.ReferenceTransform.forward) * _startRotation;
                _hasRotated = true;
            }
            else if (GameSettings.TRANSLATE_LEFT.GetKey())
            {
                _startRotation = Quaternion.AngleAxis(RotationSpeed, MovingVessel.ReferenceTransform.forward) * _startRotation;
                _hasRotated = true;
            }

            if (GameSettings.TRANSLATE_DOWN.GetKey())
            {
                _startRotation = Quaternion.AngleAxis(RotationSpeed, MovingVessel.ReferenceTransform.right) * _startRotation;
                _hasRotated = true;
            }
            else if (GameSettings.TRANSLATE_UP.GetKey())
            {
                _startRotation = Quaternion.AngleAxis(-RotationSpeed, MovingVessel.ReferenceTransform.right) * _startRotation;
                _hasRotated = true;
            }

            if (GameSettings.ROLL_LEFT.GetKey())
            {
                _startRotation = Quaternion.AngleAxis(RotationSpeed, MovingVessel.ReferenceTransform.up) * _startRotation;
                _hasRotated = true;
            }
            else if (GameSettings.ROLL_RIGHT.GetKey())
            {
                _startRotation = Quaternion.AngleAxis(-RotationSpeed, MovingVessel.ReferenceTransform.up) * _startRotation;
                _hasRotated = true;
            }

            if (spawningLocal)
            {
            }

            if (inputting)
            {
                _currMoveSpeed = Mathf.Clamp(Mathf.MoveTowards(_currMoveSpeed, _moveSpeed, MoveAccel * Time.fixedDeltaTime), 0, MoveSpeed);
            }
            else
            {
                _currMoveSpeed = 0;
            }

            Vector3 offset = offsetDirection.normalized * _currMoveSpeed;
            _currMoveVelocity = offset / Time.fixedDeltaTime;
            Vector3 vSrfPt = MovingVessel.CoM - (MoveHeight * _up);
            RaycastHit ringHit;

            bool surfaceDetected = CapsuleCast(out ringHit);
            Vector3 finalOffset = Vector3.zero;

            Vector3 rOffset = Vector3.Project(ringHit.point - vSrfPt, _up);
            Vector3 mOffset = (vSrfPt + offset) - MovingVessel.CoM;
            finalOffset = rOffset + mOffset + (MoveHeight * _up);
            MovingVessel.Translate(finalOffset);

            PQS bodyPQS = MovingVessel.mainBody.pqsController;

            Vector3d geoCoords = WorldPositionToGeoCoords(MovingVessel.GetWorldPos3D() + (_currMoveVelocity * Time.fixedDeltaTime), MovingVessel.mainBody);
            double lat = geoCoords.x;
            double lng = geoCoords.y;

            Vector3d bodyUpVector = new Vector3d(1, 0, 0);
            bodyUpVector = QuaternionD.AngleAxis(lat, Vector3d.forward/*around Z axis*/) * bodyUpVector;
            bodyUpVector = QuaternionD.AngleAxis(lng, Vector3d.down/*around -Y axis*/) * bodyUpVector;

            double srfHeight = bodyPQS.GetSurfaceHeight(bodyUpVector);

            if (!surfaceDetected)
            {
                Vector3 terrainPos = MovingVessel.mainBody.position + (float)srfHeight * _up;

                if (!surfaceDetected)
                {
                    MovingVessel.SetPosition(terrainPos + (MoveHeight * _up) + offset);
                }
            }

            if (MovingVessel.parts.Count == 1)
            {
                //fix surface rotation
                Quaternion srfRotFix = Quaternion.FromToRotation(_startingUp, _up);
                _currRotation = srfRotFix * _startRotation;
                MovingVessel.SetRotation(_currRotation);

                if (Vector3.Angle(_startingUp, _up) > 5)
                {
                    _startRotation = _currRotation;
                    _startingUp = _up;
                }
            }
            MovingVessel.SetWorldVelocity(Vector3d.zero);
        }

        private Vector3d WorldPositionToGeoCoords(Vector3d worldPosition, CelestialBody body)
        {
            if (!body)
            {
                Debug.LogWarning("WorldPositionToGeoCoords body is null");
                return Vector3d.zero;
            }

            double lat = body.GetLatitude(worldPosition);
            double longi = body.GetLongitude(worldPosition);
            double alt = body.GetAltitude(worldPosition);
            return new Vector3d(lat, longi, alt);
        }

        bool spawningLocal = false;
        double degPerMeter = 0;
        double mPerDegree = 0;
        float _alt = 0;

        public void StartMove(Vessel v, bool _spawningLocal, float _altitude, bool _placingFinishGate)
        {
            MovingVessel = new Vessel();
            placingFinishGate = _placingFinishGate;
            _alt = _altitude;
            spawningLocal = _spawningLocal;
            _moveMode = MoveModes.Normal;
            MovingVessel = v;
            IsMovingVessel = true;
            if (spawningLocal)
            {
                MoveHeight = (float)MovingVessel.altitude + _alt;
            }
            else
            {
                MoveHeight = 12;
            }
            mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
            degPerMeter = 1 / mPerDegree;

            _up = (v.transform.position - v.mainBody.transform.position).normalized;
            _startRotation = MovingVessel.transform.rotation;
            _currRotation = _startRotation;

            _moving = true;
            _debugLr.enabled = true;

            FlightGlobals.ForceSetActiveVessel(MovingVessel);
        }

        public void EndMove(bool addingCoords, bool _spawningLocal, bool drop)
        {
            spawningLocal = _spawningLocal;
            Debug.Log("[OrX Vessel Move] ===== ENDING MOVE =====");
            StartCoroutine(EndMoveRoutine(addingCoords, _spawningLocal));
        }

        private IEnumerator EndMoveRoutine(bool addingCoords, bool _spawningLocal)
        {
            _debugLr.enabled = false;
            _placingVessels.Add(MovingVessel);
            float altitude = _alt;
            IsMovingVessel = false;
            _moving = false;
            _up = (MovingVessel.transform.position - FlightGlobals.currentMainBody.transform.position).normalized;
            float localAlt = Convert.ToSingle(MovingVessel.radarAltitude);


            Debug.Log("[OrX Spawn Local Vessels] === PLACING " + MovingVessel.name + " ===");
            float dropRate = Mathf.Clamp((localAlt * 1.5f), 0.1f, 200);

            if (MovingVessel != OrXHoloKron.instance._HoloKron)
            {
                while (!MovingVessel.LandedOrSplashed)
                {
                    MovingVessel.IgnoreGForces(240);
                    MovingVessel.SetWorldVelocity(Vector3.zero);
                    dropRate = Mathf.Clamp((localAlt / 5), 0.1f, 200);

                    if (dropRate > 3)
                    {
                        MovingVessel.Translate(dropRate * Time.fixedDeltaTime * -_up);
                    }
                    else
                    {
                        MovingVessel.SetWorldVelocity(dropRate * -_up);
                    }

                    if (localAlt <= 0.3f)
                    {
                        localAlt = 0.3f;
                    }

                    localAlt -= dropRate * Time.fixedDeltaTime;

                    yield return new WaitForFixedUpdate();
                }
            }

            MovingVessel.SetWorldVelocity(Vector3.zero);
            MovingVessel.IgnoreGForces(240);
            _placingVessels.Remove(MovingVessel);
            MoveHeight = 0;
            _hoverAdjust = 0f;
            MovingVessel = new Vessel();

            if (_spawningLocal)
            {
                spawningLocal = false;
                MovingVessel.rootPart.AddModule("ModuleParkingBrake", true);
                OrXHoloKron.instance.PlaceCraft();
            }
            else
            {
                yield return new WaitForFixedUpdate();

                if (addingCoords)
                {
                    if (placingFinishGate)
                    {
                        placingFinishGate = false;
                        OrXHoloKron.instance.ChallengeAddNextCoord();
                    }
                    else
                    {
                        OrXHoloKron.instance.SpawnGoal();
                    }
                }
                else
                {
                    OrXHoloKron.instance.addingMission = true;
                    OrXHoloKron.instance.SaveConfig("");
                }
            }
        }

        private Vector3 GetBoundPoint(int index, int totalPoints, float radiusFactor)
        {
            float angleIncrement = 360 / (float)totalPoints;

            float angle = index * angleIncrement;

            Vector3 forward = North();//Vector3.ProjectOnPlane((movingVessel.CoM)-FlightCamera.fetch.mainCamera.transform.position, up).normalized;

            float radius = _alt;

            Vector3 offsetVector = (radius * radiusFactor * forward);
            offsetVector = Quaternion.AngleAxis(angle, _up) * offsetVector;

            Vector3 point = MovingVessel.CoM + offsetVector;

            return point;
        }

        private bool CapsuleCast(out RaycastHit rayHit)
        {
            //float radius = (Mathf.Max (Mathf.Max(vesselBounds.size.x, vesselBounds.size.y), vesselBounds.size.z)) + (currMoveSpeed*2);
            float radius = _alt + Mathf.Clamp(_currMoveSpeed, 0, 200);

            return Physics.CapsuleCast(MovingVessel.CoM + (250 * _up), MovingVessel.CoM + (249 * _up), radius, -_up, out rayHit, 2000, 1 << 15);
        }

        private Vector3 North()
        {
            Vector3 n = MovingVessel.mainBody.GetWorldSurfacePosition(MovingVessel.latitude + 1, MovingVessel.longitude, MovingVessel.altitude) - MovingVessel.GetWorldPos3D();
            n = Vector3.ProjectOnPlane(n, _up);
            return n.normalized;
        }

        public struct VesselBounds
        {
            public Vessel vessel;
            public float BottomLength;
            public float Radius;

            private Vector3 _localBottomPoint;
            public Vector3 BottomPoint
            {
                get
                {
                    return vessel.transform.TransformPoint(_localBottomPoint);
                }
            }

            public VesselBounds(Vessel v)
            {
                vessel = v;
                BottomLength = 0;
                Radius = 0;
                _localBottomPoint = Vector3.zero;
                UpdateBounds();
            }

            public void UpdateBounds()
            {
                Vector3 up = (vessel.CoM - vessel.mainBody.transform.position).normalized;
                Vector3 forward = Vector3.ProjectOnPlane(vessel.CoM - FlightCamera.fetch.mainCamera.transform.position, up).normalized;
                Vector3 right = Vector3.Cross(up, forward);

                float maxSqrDist = 0;
                Part furthestPart = null;

                //bottom check
                Vector3 downPoint = vessel.CoM - (2000 * up);
                Vector3 closestVert = vessel.CoM;
                float closestSqrDist = Mathf.Infinity;

                //radius check
                Vector3 furthestVert = vessel.CoM;
                float furthestSqrDist = -1;

                foreach (Part p in vessel.parts)
                {
                    if (p.Modules.Contains("ModuleRobotArmScanner")) return;
                    if (p.Modules.Contains("ModuleScienceExperiment")) return;
                    if (p.Modules.Contains("Tailhook")) return;
                    if (p.Modules.Contains("Arrestwire")) return;
                    if (p.Modules.Contains("Catapult")) return;
                    if (p.Modules.Contains("CLLS")) return;
                    if (p.Modules.Contains("OLS")) return;

                    float sqrDist = Vector3.ProjectOnPlane((p.transform.position - vessel.CoM), up).sqrMagnitude;
                    if (sqrDist > maxSqrDist)
                    {
                        maxSqrDist = sqrDist;
                        furthestPart = p;
                    }

                    //if(Vector3.Dot(up, p.transform.position-vessel.CoM) < 0)
                    //{

                    foreach (MeshFilter mf in p.GetComponentsInChildren<MeshFilter>())
                    {
                        //Mesh mesh = mf.mesh;
                        foreach (Vector3 vert in mf.mesh.vertices)
                        {
                            //bottom check
                            Vector3 worldVertPoint = mf.transform.TransformPoint(vert);
                            float bSqrDist = (downPoint - worldVertPoint).sqrMagnitude;
                            if (bSqrDist < closestSqrDist)
                            {
                                closestSqrDist = bSqrDist;
                                closestVert = worldVertPoint;
                            }

                            //radius check
                            //float sqrDist = (vessel.CoM-worldVertPoint).sqrMagnitude;
                            float hSqrDist = Vector3.ProjectOnPlane(vessel.CoM - worldVertPoint, up).sqrMagnitude;
                            if (!(hSqrDist > furthestSqrDist)) continue;
                            furthestSqrDist = hSqrDist;
                            furthestVert = worldVertPoint;
                        }
                    }

                    //}
                }

                Vector3 radVector = Vector3.ProjectOnPlane(furthestVert - vessel.CoM, up);
                Radius = radVector.magnitude;

                BottomLength = Vector3.Project(closestVert - vessel.CoM, up).magnitude;
                _localBottomPoint = vessel.transform.InverseTransformPoint(closestVert);

                //Debug.Log ("Vessel bottom length: "+bottomLength);
                /*
                        if(furthestPart!=null)
                        {
                            //Debug.Log ("Furthest Part: "+furthestPart.partInfo.title);

                            Vector3 furthestVert = vessel.CoM;
                            float furthestSqrDist = -1;

                            foreach(var mf in furthestPart.GetComponentsInChildren<MeshFilter>())
                            {
                                Mesh mesh = mf.mesh;
                                foreach(var vert in mesh.vertices)
                                {
                                    Vector3 worldVertPoint = mf.transform.TransformPoint(vert);
                                    float sqrDist = (vessel.CoM-worldVertPoint).sqrMagnitude;
                                    if(sqrDist > furthestSqrDist)
                                    {
                                        furthestSqrDist = sqrDist;
                                        furthestVert = worldVertPoint;
                                    }
                                }
                            }

                            Vector3 radVector = Vector3.ProjectOnPlane(furthestVert-vessel.CoM, up);
                            radius = radVector.magnitude;
                            //Debug.Log ("Vert test found radius to be "+radius);
                        }
                        */
                //radius *= 1.75f;
                //radius += 5;//15;
                Radius += Mathf.Clamp(Radius, 2, 10);
            }


        }

        public static List<string> partIgnoreModules = new List<string>(9)
        {
        "ModuleRobotArmScanner",
        "ModuleScienceExperiment",
            "Tailhook",
            "Arrestwire",
            "Catapult",
            "CLLS",
            "OLS"
        };

        private static bool IsPartModuleIgnored(string ModuleName)
        {
            return true;
        }

    }
}

