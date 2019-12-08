using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXPlace : PartModule
    {
        public bool setup = false;
        Rigidbody _rb;
        private float maxfade = 0f;
        private float surfaceAreaToCloak = 0.0f;
        public bool cloakOn = false;

        private static float UNCLOAKED = 1.0f;
        private static float RENDER_THRESHOLD = 0.0f;
        private float fadePerTime = 0.2f;
        private bool currentShadowState = true;
        private float visiblilityLevel = 0;
        private float fadeTime = 10f;
        private float shadowCutoff = 0.0f;
        bool parkingBrake = false;
        float brakeTweakable = 0;
        bool rotationsSet = false;

        float left = 0;
        float pitch = 0;

        double degPerMeter = 0;
        double mPerDegree = 0;
        double targetDistance = 0;
        public bool inRange = true;

        public double altitude = 0;
        public double latitude = 0;
        public double longitude = 0;
        float altToSub = 0;

        Vector3 UpVect;
        Quaternion _fixRot;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                vessel.IgnoreGForces(240);
                UpVect = (vessel.ReferenceTransform.position - vessel.mainBody.position).normalized;
            }
            base.OnStart(state);
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!inRange && vessel.loaded)
                {
                    vessel.IgnoreGForces(240);
                    vessel.SetWorldVelocity(Vector3.zero);
                    this.vessel.SetPosition(vessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude));
                }

                if (visiblilityLevel < UNCLOAKED && !cloakOn || visiblilityLevel > 0 && cloakOn)
                {
                    float delta = Time.deltaTime * fadePerTime;
                    if (cloakOn && (visiblilityLevel > maxfade))
                        delta = -delta;

                    visiblilityLevel += delta;
                    visiblilityLevel = Mathf.Clamp(visiblilityLevel, maxfade, UNCLOAKED);

                    List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                    while (p.MoveNext())
                    {
                        if (p.Current != null)
                        {
                            p.Current.SetOpacity(visiblilityLevel);
                            int i;

                            MeshRenderer[] MRs = p.Current.GetComponentsInChildren<MeshRenderer>();
                            for (i = 0; i < MRs.GetLength(0); i++)
                                MRs[i].enabled = visiblilityLevel > RENDER_THRESHOLD;// || !fullRenderHide;

                            SkinnedMeshRenderer[] SMRs = p.Current.GetComponentsInChildren<SkinnedMeshRenderer>();
                            for (i = 0; i < SMRs.GetLength(0); i++)
                                SMRs[i].enabled = visiblilityLevel > RENDER_THRESHOLD;// || !fullRenderHide;

                            if (visiblilityLevel > shadowCutoff != currentShadowState)
                            {
                                for (i = 0; i < MRs.GetLength(0); i++)
                                {
                                    if (visiblilityLevel > shadowCutoff)
                                        MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                                    else
                                        MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                                }
                                for (i = 0; i < SMRs.GetLength(0); i++)
                                {
                                    if (visiblilityLevel > shadowCutoff)
                                        SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                                    else
                                        SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                                }
                                currentShadowState = visiblilityLevel > shadowCutoff;
                            }
                        }
                    }
                    p.Dispose();
                }
            }
        }

        IEnumerator GetDistance()
        {
            if (!rotationsSet)
            {
                rotationsSet = true;
                SetRotation();
            }

            if (!inRange)
            {
                double _latDiff = 0;
                double _lonDiff = 0;
                double _altDiff = 0;
                if (FlightGlobals.ActiveVessel.altitude <= altitude)
                {
                    _altDiff = vessel.altitude - FlightGlobals.ActiveVessel.altitude;
                }
                else
                {
                    _altDiff = FlightGlobals.ActiveVessel.altitude - altitude;
                }

                if (latitude >= 0)
                {
                    if (FlightGlobals.ActiveVessel.latitude >= latitude)
                    {
                        _latDiff = FlightGlobals.ActiveVessel.latitude - latitude;
                    }
                    else
                    {
                        _latDiff = latitude - FlightGlobals.ActiveVessel.latitude;
                    }
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.latitude >= 0)
                    {
                        _latDiff = FlightGlobals.ActiveVessel.latitude - latitude;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.latitude <= latitude)
                        {
                            _latDiff = FlightGlobals.ActiveVessel.latitude - latitude;
                        }
                        else
                        {

                            _latDiff = latitude - FlightGlobals.ActiveVessel.latitude;
                        }
                    }
                }

                if (longitude >= 0)
                {
                    if (FlightGlobals.ActiveVessel.longitude >= longitude)
                    {
                        _lonDiff = FlightGlobals.ActiveVessel.longitude - longitude;
                    }
                    else
                    {
                        _lonDiff = longitude - FlightGlobals.ActiveVessel.latitude;
                    }
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.longitude >= 0)
                    {
                        _lonDiff = FlightGlobals.ActiveVessel.longitude - longitude;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.longitude <= longitude)
                        {
                            _lonDiff = FlightGlobals.ActiveVessel.longitude - longitude;
                        }
                        else
                        {

                            _lonDiff = longitude - FlightGlobals.ActiveVessel.longitude;
                        }
                    }
                }

                double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                double _altDiffDeg = _altDiff * degPerMeter;
                double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                if (targetDistance <= 4000)
                {
                    inRange = true;
                    yield return new WaitForFixedUpdate();
                    StartCoroutine(PlaceVessel());
                }
                else
                {
                    yield return new WaitForFixedUpdate();
                    StartCoroutine(GetDistance());
                }
            }
            else
            {
            }
        }
        public void SetRotation()
        {
            _fixRot = Quaternion.identity;
            vessel.IgnoreGForces(240);
            vessel.SetWorldVelocity(Vector3d.zero);
            _fixRot = Quaternion.FromToRotation(vessel.ReferenceTransform.up, UpVect) * vessel.ReferenceTransform.rotation;
            vessel.SetRotation(_fixRot, true);
            _fixRot = Quaternion.FromToRotation(-vessel.ReferenceTransform.right, spawn.OrXSpawnHoloKron.instance.NorthVect) * vessel.ReferenceTransform.rotation;
            vessel.SetRotation(_fixRot, true);
            _fixRot = Quaternion.AngleAxis(pitch, vessel.ReferenceTransform.right) * vessel.ReferenceTransform.rotation;
            vessel.SetRotation(_fixRot, true);
            _fixRot = Quaternion.AngleAxis(left, UpVect) * vessel.ReferenceTransform.rotation;
            vessel.SetRotation(_fixRot, true);
            _fixRot = vessel.ReferenceTransform.rotation;
            vessel.IgnoreGForces(240);
        }

        public void PlaceCraft(bool _bda, bool _airborne, bool _splashed, bool _gate, bool _challengeStart, float _altToSubtract, float _left, float _pitch)
        {
            vessel.IgnoreGForces(240);

            _rb = vessel.GetComponent<Rigidbody>();
            _rb.isKinematic = true;

            UpVect = (vessel.ReferenceTransform.position - vessel.mainBody.position).normalized;
            left = _left;
            pitch = _pitch - 90;

            if (left >= 360)
            {
                left -= 360;
            }

            if (vessel.rootPart.Modules.Contains<ModuleOrXStage>())
            {
                left -= 90;
            }

            SetRotation();

            altToSub = _altToSubtract;
            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;

            if (_bda)
            {
                ActivateEngines();

                if (!_airborne && !_splashed)
                {
                    inRange = false;
                    ActivateBDA();
                }
                else
                {
                    if (!_splashed)
                    {
                        inRange = true;
                        ActivateBDA();
                        _rb.isKinematic = false;
                        Destroy(this);
                    }
                    else
                    {
                        inRange = true;
                        StartCoroutine(PlaceVessel());
                    }
                }
            }
            else
            {
                inRange = true;
                StartCoroutine(PlaceVessel());
            }
        }
        IEnumerator PlaceVessel()
        {
            OrXLog.instance.DebugLog("[OrX Place] === PLACING " + vessel.vesselName + " ===");
            _rb = vessel.GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            SetRotation();
            float localAlt = 5;
            float dropRate = Mathf.Clamp((localAlt), 0.1f, 200);

            if (inRange)
            {
                while (!vessel.LandedOrSplashed)
                {
                    if (localAlt <= 0.1f)
                    {
                        localAlt = 0.1f;
                    }
                    vessel.IgnoreGForces(240);
                    vessel.angularVelocity = Vector3.zero;
                    vessel.angularMomentum = Vector3.zero;
                    vessel.SetWorldVelocity(Vector3.zero);
                    dropRate = Mathf.Clamp((localAlt), 0.1f, 200);

                    if (dropRate <= 2f)
                    {
                        dropRate = 2f;
                    }

                    vessel.Translate(dropRate * Time.fixedDeltaTime * -(vessel.ReferenceTransform.position - vessel.mainBody.position).normalized);

                    localAlt -= dropRate * Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }
            _rb.isKinematic = false;
            Destroy(this);
        }

        private void ActivateEngines()
        {
            List<Part>.Enumerator p = vessel.parts.GetEnumerator();
            while (p.MoveNext())
            {
                if (p.Current != null)
                {
                    var engines = p.Current.FindModuleImplementing<ModuleEngines>();
                    var enginesFX = p.Current.FindModuleImplementing<ModuleEnginesFX>();
                    if (engines != null)
                    {
                        OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND ENGINES ON " + vessel.vesselName + " ... ACTIVATING PART " + p.Current.name + " =====");
                        p.Current.force_activate();
                        engines.ActivateAction(new KSPActionParam(KSPActionGroup.None, KSPActionType.Activate));
                        engines.Activate();
                    }

                    if (enginesFX != null)
                    {
                        OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND ENGINEFX ON " + vessel.vesselName + " ... ACTIVATING PART " + p.Current.name + " =====");
                        p.Current.force_activate();
                        enginesFX.ActivateAction(new KSPActionParam(KSPActionGroup.None, KSPActionType.Activate));
                        enginesFX.Activate();
                    }

                    OrXLog.instance.DebugLog("[OrX Craft Setup] ===== ADDING SALT TO " + p.Current.name + " =====");
                    p.Current.AddModule("ModuleAddSalt", true);
                    var _addSalt = p.Current.FindModuleImplementing<ModuleAddSalt>();
                    _addSalt.AddSalt();
                }
            }
            p.Dispose();
        }
        public void ActivateBDA()
        {
            List<Part>.Enumerator _parts = vessel.parts.GetEnumerator();
            while (_parts.MoveNext())
            {
                if (_parts.Current.Modules.Contains("BDModulePilotAI") || _parts.Current.Modules.Contains("BDModuleSurfaceAI"))
                {
                    OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND PILOT AI ON " + vessel.vesselName + " ... ACTIVATING =====");
                    _parts.Current.SendMessage("ActivatePilot");
                }
                else
                {
                    if (_parts.Current.Modules.Contains("MissileFire"))
                    {
                        OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND WEAPON MANAGER ON " + vessel.vesselName + " ... ACTIVATING GUARD MODE =====");
                        _parts.Current.SendMessage("ToggleGuardMode");
                    }
                    else
                    {
                        if (_parts.Current.Modules.Contains("ModuleRadar") || _parts.Current.Modules.Contains("ModuleSpaceRadar"))
                        {
                            OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND RADAR ON " + vessel.vesselName + " ... ACTIVATING =====");
                            _parts.Current.SendMessage("EnableRadar");
                        }
                        if (_parts.Current.Modules.Contains("MissileTurret"))
                        {
                            OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND MISSILE TURRET ON " + vessel.vesselName + " ... ACTIVATING =====");
                            _parts.Current.SendMessage("EnableTurret");
                        }
                    }
                }
            }
            _parts.Dispose();

            if (!inRange)
            {
                StartCoroutine(GetDistance());
            }
        }
    }
}