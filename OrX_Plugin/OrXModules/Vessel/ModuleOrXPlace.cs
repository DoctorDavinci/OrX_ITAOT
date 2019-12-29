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
        float _dropSpeed = 0;

        Vector3 UpVect;
        Quaternion _fixRot;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                vessel.IgnoreGForces(240);
                UpVect = (vessel.ReferenceTransform.position - vessel.mainBody.position).normalized;
                mPerDegree = (((2 * (vessel.mainBody.Radius + vessel.altitude)) * Math.PI) / 360);
                degPerMeter = 1 / mPerDegree;
            }
            base.OnStart(state);
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!inRange && vessel.loaded)
                {
                    vessel.Landed = true;
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
                if (CheckDistance(FlightGlobals.ActiveVessel) <= 8000)
                {
                    if (OrXHoloKron.instance.bdaChallenge)
                    {
                        if (OrXVesselLog.instance._playerCraft.Contains(FlightGlobals.ActiveVessel))
                        {
                            inRange = true;
                        }
                    }
                    else
                    {
                        inRange = true;
                    }
                }

                yield return new WaitForSeconds(5);

                if (!inRange)
                {
                    StartCoroutine(GetDistance());
                }
                else
                {
                    vessel.Landed = false;
                    StartCoroutine(PlaceVessel());
                }
            }
        }

        private double CheckDistance(Vessel _player)
        {
            double _latDiff = 0;
            double _lonDiff = 0;
            double _altDiff = 0;

            if (_player.altitude <= vessel.altitude)
            {
                _altDiff = vessel.altitude - _player.altitude;
            }
            else
            {
                _altDiff = _player.altitude - vessel.altitude;
            }

            if (vessel.latitude >= 0)
            {
                if (_player.latitude >= vessel.latitude)
                {
                    _latDiff = _player.latitude - vessel.latitude;
                }
                else
                {
                    _latDiff = vessel.latitude - _player.latitude;
                }
            }
            else
            {
                if (_player.latitude >= 0)
                {
                    _latDiff = _player.latitude - vessel.latitude;
                }
                else
                {
                    if (_player.latitude <= vessel.latitude)
                    {
                        _latDiff = _player.latitude - vessel.latitude;
                    }
                    else
                    {

                        _latDiff = vessel.latitude - _player.latitude;
                    }
                }
            }

            if (vessel.longitude >= 0)
            {
                if (_player.longitude >= vessel.longitude)
                {
                    _lonDiff = _player.longitude - vessel.longitude;
                }
                else
                {
                    _lonDiff = vessel.longitude - _player.latitude;
                }
            }
            else
            {
                if (_player.longitude >= 0)
                {
                    _lonDiff = _player.longitude - vessel.longitude;
                }
                else
                {
                    if (_player.longitude <= vessel.longitude)
                    {
                        _lonDiff = _player.longitude - vessel.longitude;
                    }
                    else
                    {

                        _lonDiff = vessel.longitude - _player.longitude;
                    }
                }
            }

            double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
            double _altDiffDeg = _altDiff * degPerMeter;
            double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
            targetDistance = Math.Sqrt(altAdded) * mPerDegree;
            return targetDistance;
        }

        public void SetRotation()
        {
            if (left == 0 && pitch == -90) return;

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

        public void PlaceCraft(bool _bda, bool _airborne, bool _splashed, bool _gate, bool _owned, float _altToSubtract, float _left, float _pitch, float _delay)
        {
            vessel.IgnoreGForces(240);
            bool _flip = false;
            _rb = vessel.GetComponent<Rigidbody>();
            _rb.isKinematic = true;

            UpVect = (vessel.ReferenceTransform.position - vessel.mainBody.position).normalized;

            if (_pitch == 180)
            {
                _flip = true;
            }

            left = _left;
            pitch = _pitch - 90;

            if (left >= 360)
            {
                left -= 360;
            }
            if (left <= -360)
            {
                left += 360;
            }

            if (vessel.rootPart.Modules.Contains<ModuleOrXStage>() && OrXHoloKron.instance.buildingMission)
            {
                left -= 90;
            }

            if (!_gate)
            {
                SetRotation();
            }
            _dropSpeed = ((float)vessel.radarAltitude);

            if (_flip)
            {
                _fixRot = Quaternion.identity;
                vessel.IgnoreGForces(240);
                vessel.SetWorldVelocity(Vector3d.zero);
                _fixRot = Quaternion.AngleAxis(180, vessel.ReferenceTransform.right) * vessel.ReferenceTransform.rotation;
                vessel.SetRotation(_fixRot, true);
                _fixRot = Quaternion.AngleAxis(90, UpVect) * vessel.ReferenceTransform.rotation;
                vessel.SetRotation(_fixRot, true);
            }
            
            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;

            if (_bda)
            {
                ActivateEngines(_owned);
                ActivateBDA(_delay, _owned);

                if (!_airborne && !_splashed)
                {
                    inRange = false;
                    StartCoroutine(GetDistance());
                }
                else
                {
                    if (!_splashed)
                    {
                        inRange = true;
                        _rb.isKinematic = false;
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
            vessel.Landed = false;
            OrXLog.instance.DebugLog("[OrX Place] === PLACING " + vessel.vesselName + " ===");
            _rb = vessel.GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            SetRotation();
            float dropRate = Mathf.Clamp(_dropSpeed / 2, 0.1f, 200);
            bool _continue = true;
            if (inRange)
            {
                while (!vessel.LandedOrSplashed)
                {
                    if (_continue)
                    {
                        vessel.IgnoreGForces(240);
                        vessel.angularVelocity = Vector3.zero;
                        vessel.angularMomentum = Vector3.zero;
                        vessel.SetWorldVelocity(Vector3.zero);
                        dropRate = Mathf.Clamp(_dropSpeed / 2, 0.2f, 200);

                        if (dropRate <= 0.2f)
                        {
                            dropRate = 0.2f;
                        }
                        if (_dropSpeed <= 2)
                        {
                            _dropSpeed = 2;
                        }

                        vessel.Translate(dropRate * Time.fixedDeltaTime * -(vessel.ReferenceTransform.position - vessel.mainBody.position).normalized);
                        _dropSpeed -= dropRate * Time.fixedDeltaTime;
                        yield return new WaitForFixedUpdate();

                        if (vessel.radarAltitude <= 0.5f)
                        {
                            _continue = false;
                            vessel.Landed = true;
                        }
                    }
                }
            }
            _rb.isKinematic = false;
        }

        private void ActivateEngines(bool _owned)
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

                    p.Current.AddModule("ModuleAddSalt", true);
                    var _addSalt = p.Current.FindModuleImplementing<ModuleAddSalt>();
                    _addSalt.AddSalt(_owned);
                }
            }
            p.Dispose();
        }
        public void ActivateBDA(float _delay, bool _owned)
        {
            List<Part>.Enumerator _parts = vessel.parts.GetEnumerator();
            while (_parts.MoveNext())
            {
                if (_parts.Current.Modules.Contains("BDModulePilotAI") || _parts.Current.Modules.Contains("BDModuleSurfaceAI"))
                {
                    OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND PILOT AI ON " + vessel.vesselName + " ... ACTIVATING =====");
                    _parts.Current.SendMessage("ActivatePilot");
                }

                if (_parts.Current.Modules.Contains("MissileFire") && !_owned)
                {
                    OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND WEAPON MANAGER ON " + vessel.vesselName + " ... SETTING TEAM =====");
                    _parts.Current.SendMessage("NextTeam");
                }
            }
            _parts.Dispose();
            StartCoroutine(WeaponManagerDelay(_delay));
        }
        IEnumerator WeaponManagerDelay(float _delay)
        {
            float _d = _delay / 1000;
            OrXLog.instance.DebugLog("[OrX Craft Setup] ===== " + vessel.vesselName + " WEAPON MANAGER ENGAGEMENT DELAY: " + _d + " SECONDS =====");
            yield return new WaitForSeconds(_d);
            CheckPlayerLocations();
        }
        private void CheckPlayerLocations()
        {
            bool _continue = false;
            float _altitude = 0;
            List<Vessel>.Enumerator _playerVessels = OrXVesselLog.instance._playerCraft.GetEnumerator();
            while (_playerVessels.MoveNext())
            {
                if (_playerVessels.Current != null)
                {
                    if (_playerVessels.Current.altitude >= _playerVessels.Current.radarAltitude)
                    {
                        _altitude = (float)_playerVessels.Current.radarAltitude;

                        if (_playerVessels.Current.radarAltitude >= OrXTargetDistance.instance.targetDistance / 200)
                        {
                            _continue = true;
                        }
                    }
                    else
                    {
                        _altitude = (float)_playerVessels.Current.altitude;

                        if (_playerVessels.Current.altitude >= OrXTargetDistance.instance.targetDistance / 200)
                        {
                            _continue = true;
                        }
                    }

                    if (CheckDistance(_playerVessels.Current) <= 8000)
                    {
                        _continue = true;
                    }
                }
            }
            _playerVessels.Dispose();

            if (_continue)
            {
                EngageWM();
            }
            else
            {
                StartCoroutine(WeaponManagerDelay(5 - _altitude / 100));
            }
        }
        private void EngageWM()
        {
            List<Part>.Enumerator _parts = vessel.parts.GetEnumerator();
            while (_parts.MoveNext())
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
            _parts.Dispose();
        }
    }
}