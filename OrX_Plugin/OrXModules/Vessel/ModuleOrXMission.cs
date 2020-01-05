using OrX.spawn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXMission : PartModule
    {
        #region Fields

        [KSPField(isPersistant = true)]
        public bool fml = false;

        [KSPField(isPersistant = true)]
        public bool _auto = false;

        [KSPField(isPersistant = true)]
        public bool completed = false;

        [KSPField(isPersistant = true)]
        public string HoloKronName = string.Empty;

        [KSPField(isPersistant = true)]
        public string missionName = string.Empty;

        [KSPField(isPersistant = true)]
        public string missionType = string.Empty;

        [KSPField(isPersistant = true)]
        public string challengeType = string.Empty;
        [KSPField(isPersistant = true)]
        public string raceType = string.Empty;


        [KSPField(isPersistant = true)]
        public string tech = string.Empty;

        [KSPField(isPersistant = true)]
        public int hkCount = 0;

        [KSPField(isPersistant = true)]
        public bool spawned = false;

        [KSPField(isPersistant = true)]
        public string creator = string.Empty;

        [KSPField(isPersistant = true)]
        public string Gold = string.Empty;
        [KSPField(isPersistant = true)]
        public string Silver = string.Empty;
        [KSPField(isPersistant = true)]
        public string Bronze = string.Empty;

        [KSPField(isPersistant = true)]
        public double _asRangeMed = 50000;
        [KSPField(isPersistant = true)]
        public double _asRangeShort = 25000;
        [KSPField(isPersistant = true)]
        public double _asRangeLong = 75000;

        [KSPField(isPersistant = true)]
        public bool asRangeMed = false;
        [KSPField(isPersistant = true)]
        public bool asRangeShort = false;
        [KSPField(isPersistant = true)]
        public bool asRangeLong = false;

        [KSPField(isPersistant = true)]
        public bool blueprintsAdded = false;

        Vessel triggerCraft;

        [KSPField(isPersistant = true)]
        public double altitude = 0;
        [KSPField(isPersistant = true)]
        public double latitude = 0;
        [KSPField(isPersistant = true)]
        public double longitude = 0;

        public bool hideGoal = false;
        public bool setup = false;
        private static float maxfade = 0f;
        private static float maxVis = 1f;
        private static float rLevel = 0.0f;
        private bool currentShadowState = true;
        private float tLevel = 0;
        private float rateOfFade = 5f;
        private float shadowCutoff = 0.0f;
        private bool triggerHide = false;
        public Vector3d pos;
        public Vector3d _pos;

        public Vector3 UpVect;
        public bool Goal = false;
        public bool opened = false;
        public int triggerRange = 10;
        public bool isLoaded = false;
        public int stage = 0;
        public bool triggered = false;

        bool geoCache = true;
        bool gateSpawned = false;
        bool posSet = false;

        Rigidbody _rb;

        #endregion

        [KSPField(unfocusedRange = 15, guiActiveUnfocused = true, isPersistant = false, guiActiveEditor = false, guiActive = true, guiName = "OPEN HOLOKRON"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.Flight, disabledText = "", enabledText = "")]
        public bool deploy = false;

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && isLoaded)
            {
                if (!opened && !OrXHoloKron.instance.buildingMission && !this.vessel.isActiveVessel)
                {
                    OrXLog.DrawRecticle(pos, OrXLog.instance.HoloTargetTexture, new Vector2(16, 16));
                }
            }
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                tLevel = 0;
                part.SetOpacity(tLevel);
                HoloKronName = OrXHoloKron.instance.HoloKronName;
                triggerCraft = FlightGlobals.ActiveVessel;
                part.explosionPotential *= 0.2f;
                UpVect = (vessel.transform.position - vessel.mainBody.transform.position).normalized;
                pos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude);

                foreach (Collider c in part.GetComponents<Collider>())
                {
                    c.enabled = false;
                }
            }
        }
        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                this.vessel.IgnoreGForces(240);
                this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

                if (isLoaded)
                {
                    SetPosition();

                    if (hideGoal && (tLevel > maxfade) || !hideGoal && (tLevel < maxVis) || triggerHide)
                    {
                        Fade();
                    }
                }
            }
            else
            {
                this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
            }
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (_rb == null)
                {
                    _rb = part.GetComponent<Rigidbody>();
                    _rb.isKinematic = true;
                }

                if (isLoaded && !OrXHoloKron.instance.buildingMission)
                {
                    if (!this.vessel.isActiveVessel)
                    {
                        if (Goal)
                        {
                            if (!FlightGlobals.ActiveVessel.rootPart.Modules.Contains<ModuleOrXStage>())
                            {
                                if (!hideGoal)
                                {
                                    CheckDistance();
                                }
                            }
                        }
                    }

                    if (deploy)
                    {
                        CheckStatus();
                    }
                }
            }
        }

        private void SetPosition()
        {
            bool _getCoords = false;

            if (this.vessel != OrXVesselMove.Instance.MovingVessel && posSet)
            {
                if (!OrXHoloKron.instance._killingLastCoord)
                {
                    this.vessel.SetPosition(pos);
                }
                else
                {
                    _getCoords = true;
                }
            }
            else
            {
                _getCoords = true;
            }

            if (_getCoords)
            {
                if (longitude != this.vessel.longitude || latitude != this.vessel.latitude)
                {
                    latitude = this.vessel.latitude;
                    longitude = this.vessel.longitude;
                    altitude = this.vessel.altitude - this.vessel.radarAltitude + 5;
                    pos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude);
                    posSet = true;
                }
            }
        }
        private void Fade()
        {
            float delta = Time.deltaTime * rateOfFade;
            if (hideGoal && (tLevel > maxfade))
                delta = -delta;

            tLevel += delta;
            tLevel = Mathf.Clamp(tLevel, maxfade, maxVis);
            this.part.SetOpacity(tLevel);
            int i;

            MeshRenderer[] MRs = this.part.GetComponentsInChildren<MeshRenderer>();
            for (i = 0; i < MRs.GetLength(0); i++)
                MRs[i].enabled = tLevel > rLevel;

            SkinnedMeshRenderer[] SMRs = this.part.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (i = 0; i < SMRs.GetLength(0); i++)
                SMRs[i].enabled = tLevel > rLevel;

            if (tLevel > shadowCutoff != currentShadowState)
            {
                for (i = 0; i < MRs.GetLength(0); i++)
                {
                    if (tLevel > shadowCutoff)
                        MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    else
                        MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                for (i = 0; i < SMRs.GetLength(0); i++)
                {
                    if (tLevel > shadowCutoff)
                        SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    else
                        SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                currentShadowState = tLevel > shadowCutoff;
            }
        }
        private void CheckStatus()
        {
            if (FlightGlobals.ActiveVessel.isEVA)
            {
                if (challengeType == "LBC")
                {
                    if (!OrXSpawnHoloKron.instance.spawning)
                    {
                        deploy = false;
                        opened = true;
                        hideGoal = true;
                        geoCache = false;
                        OrXLog.instance.DebugLog("[Module OrX Mission - LBC] === CLOSING '" + HoloKronName + "-" + hkCount + "-" + creator + "' BOID " + stage + " === ");
                        OrXHoloKron.instance.GetNextCoord();
                        this.part.explosionPotential *= 0.2f;
                        this.part.explode();
                    }
                    else
                    {
                        ScreenMessages.PostScreenMessage(new ScreenMessage("Unable to close HoloKron while spawning ....", 4, ScreenMessageStyle.UPPER_CENTER));
                        deploy = false;
                        opened = false;
                    }
                }
                else
                {
                    if (!OrXSpawnHoloKron.instance.spawning)
                    {
                        deploy = false;
                        opened = true;
                        //hideGoal = true;
                        if (missionType == "CHALLENGE" && challengeType != "BD ARMORY")
                        {
                            geoCache = false;
                        }
                        latitude = this.vessel.latitude;
                        longitude = this.vessel.longitude;
                        altitude = this.vessel.altitude - this.vessel.radarAltitude + 5;

                        OrXHoloKron.instance._challengeStartLoc = new Vector3d(latitude, longitude, altitude);
                        OrXLog.instance.DebugLog("[Module OrX Mission] === OPENING '" + HoloKronName + "-" + hkCount + "-" + creator + "' === ");
                        OrXHoloKron.instance.holoOpen = true;
                        OrXHoloKron.instance.OpenHoloKron(geoCache, HoloKronName, hkCount, this.vessel, FlightGlobals.ActiveVessel);
                        ScreenMessages.PostScreenMessage(new ScreenMessage("Get into a vehicle to start the challenge", 4, ScreenMessageStyle.UPPER_CENTER));
                    }
                    else
                    {
                        ScreenMessages.PostScreenMessage(new ScreenMessage("Unable to open HoloKron while spawning ....", 4, ScreenMessageStyle.UPPER_CENTER));
                        deploy = false;
                        opened = false;
                    }
                }
            }
            else
            {
                if (!OrXSpawnHoloKron.instance.spawning)
                {
                    opened = true;
                    deploy = false;
                    OrXLog.instance.DebugLog("[Module OrX Mission] === OPENING '" + HoloKronName + "-" + hkCount + "-" + creator + "' === ");
                    OrXHoloKron.instance.holoOpen = true;
                    if (missionType == "CHALLENGE")
                    {
                        geoCache = false;
                    }
                    triggerCraft = FlightGlobals.ActiveVessel;
                    OrXHoloKron.instance.OpenHoloKron(geoCache, HoloKronName, hkCount, this.vessel, FlightGlobals.ActiveVessel);
                }
                else
                {
                    ScreenMessages.PostScreenMessage(new ScreenMessage("Unable to open HoloKron while spawning ....", 4, ScreenMessageStyle.UPPER_CENTER));
                    deploy = false;
                    opened = false;
                }
            }
        }
        private void CheckDistance()
        {
            OrXHoloKron.instance.showTargets = false;
            double _targetDistance = OrXUtilities.instance.GetDistance(FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.latitude, vessel.longitude, vessel.latitude, (FlightGlobals.ActiveVessel.altitude + vessel.altitude) / 2);

            if (!fml)
            {
                if (_auto)
                {
                    if (challengeType == "LBC")
                    {
                        if (_targetDistance <= 5)
                        {
                            Goal = false;
                        }
                    }
                    else
                    {
                        float distance = 25;

                        if (challengeType == "BD ARMORY")
                        {
                            distance = 150;
                        }

                        if (_targetDistance <= distance)
                        {
                            if (challengeType == "BD ARMORY")
                            {
                                if (OrXVesselLog.instance._playerCraft.Contains(FlightGlobals.ActiveVessel))
                                {
                                    OrXHoloKron.instance.SaveBDAcScore();
                                    part.explosionPotential *= 0.2f;
                                    part.explode();
                                }
                            }
                            else
                            {
                                hideGoal = true;
                                deploy = false;
                                opened = true;
                                //hideGoal = true;
                                latitude = this.vessel.latitude;
                                longitude = this.vessel.longitude;
                                altitude = this.vessel.altitude - this.vessel.radarAltitude + 10;
                                Goal = false;
                                if (missionType == "CHALLENGE")
                                {
                                    geoCache = false;
                                }
                                else
                                {
                                    geoCache = true;
                                }
                                OrXHoloKron.instance._challengeStartLoc = new Vector3d(latitude, longitude, altitude);
                                OrXLog.instance.DebugLog("[Module OrX Mission] === OPENING '" + HoloKronName + "' === ");
                                OrXHoloKron.instance.holoOpen = true;
                                OrXHoloKron.instance.OpenHoloKron(geoCache, HoloKronName, hkCount, this.vessel, FlightGlobals.ActiveVessel);
                            }
                        }
                    }
                }
                else
                {
                    if (raceType == "DAKAR RACING" && !gateSpawned)
                    {
                        if (_targetDistance <= 800)
                        {
                            gateSpawned = true;
                            StartCoroutine(GateCheck());
                        }
                    }

                    if (_targetDistance <= 8)
                    {
                        OrXHoloKron.instance._getCenterDist = false;
                        hideGoal = true;
                        Goal = false;
                        OrXLog.instance.DebugLog("== STAGE " + stage + " TARGET DISTANCE: " + _targetDistance);
                        OrXHoloKron.instance.GetNextCoord();
                    }
                }
            }
            else
            {
                if (OrXVesselLog.instance._playerCraft.Contains(FlightGlobals.ActiveVessel))
                {
                    if (_targetDistance <= _asRangeLong)
                    {
                        if (!asRangeLong)
                        {
                            asRangeLong = true;

                        }

                        if (_targetDistance <= _asRangeMed)
                        {
                            if (!asRangeMed)
                            {
                                asRangeMed = true;
                            }

                            if (_targetDistance <= _asRangeShort)
                            {
                                if (!asRangeShort)
                                {
                                    asRangeShort = true;
                                }

                                if (_targetDistance <= 10000)
                                {
                                    _auto = true;
                                    fml = false;
                                    OrXLog.instance.DebugLog("[Module OrX Mission - BDAc Challenge] == TARGET DISTANCE: " + _targetDistance);
                                    OrXSpawnHoloKron.instance.SpawnLocal(true, HoloKronName, new Vector3d(), OrXTargetDistance.instance._wmActivateDelay, stage);
                                }
                                else
                                {

                                    if (_targetDistance <= 12000)
                                    {
                                        double _altitude = 0;
                                        List<Vessel>.Enumerator loggedCraft = OrXVesselLog.instance._playerCraft.GetEnumerator();
                                        while (loggedCraft.MoveNext())
                                        {
                                            if (loggedCraft.Current != null)
                                            {
                                                if (loggedCraft.Current.altitude <= loggedCraft.Current.radarAltitude)
                                                {
                                                    if (_altitude <= loggedCraft.Current.altitude)
                                                    {
                                                        _altitude = loggedCraft.Current.altitude;
                                                    }
                                                }
                                                else
                                                {
                                                    if (_altitude <= loggedCraft.Current.radarAltitude)
                                                    {
                                                        _altitude = loggedCraft.Current.radarAltitude;
                                                    }
                                                }
                                            }
                                        }
                                        loggedCraft.Dispose();

                                        if (_targetDistance <= 10000 + _altitude)
                                        {
                                            _auto = true;
                                            fml = false;
                                            OrXLog.instance.DebugLog("[Module OrX Mission - BDAc Challenge] == TARGET DISTANCE: " + _targetDistance);
                                            OrXSpawnHoloKron.instance.SpawnLocal(true, HoloKronName, new Vector3d(), OrXTargetDistance.instance._wmActivateDelay, stage);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        IEnumerator GateCheck()
        {
            bool _gateFound = false;

            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
            while (v.MoveNext())
            {
                if (v.Current != null && v.Current.loaded && !v.Current.packed)
                {
                    if (v.Current.rootPart.Modules.Contains<ModuleOrXStage>() && v.Current.parts.Count >= 3)
                    {
                        if (OrXUtilities.instance.GetDistance(vessel.longitude, vessel.latitude, v.Current.longitude, v.Current.latitude, (v.Current.altitude + vessel.altitude) / 2) <= 20)
                        {
                            if (hideGoal && !Goal)
                            {
                                v.Current.rootPart.AddModule("ModuleOrXJason", true);
                                OrXLog.instance.DebugLog("[OrX OrX Mission - Gate Check] === Gate found ... calling Jason ===");
                                _gateFound = true;
                                break;
                            }
                            else
                            {
                                OrXLog.instance.DebugLog("[OrX OrX Mission - Gate Check] === Gate found ===");
                                _gateFound = true;
                                break;
                            }
                        }
                    }
                }
            }
            v.Dispose();

            if (!_gateFound)
            {
                if (!hideGoal && Goal)
                {
                    yield return new WaitForFixedUpdate();
                    OrXSpawnHoloKron.instance.SpawnFile(OrXSpawnHoloKron.instance.GoalPostCraft, true, false, false, false, false, stage, 0, 0, new Vector3d(vessel.latitude, vessel.longitude, vessel.altitude));
                }
            }
            else
            {

            }
        }
    }
}