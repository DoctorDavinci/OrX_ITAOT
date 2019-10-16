using OrX.spawn;
using OrXWind;
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
        public string tech = string.Empty;

        [KSPField(isPersistant = true)]
        public int mCount = 0;

        [KSPField(isPersistant = true)]
        public bool spawned = false;

        [KSPField(isPersistant = true)]
        public string Gold = string.Empty;
        [KSPField(isPersistant = true)]
        public string Silver = string.Empty;
        [KSPField(isPersistant = true)]
        public string Bronze = string.Empty;


        [KSPField(isPersistant = true)]
        public bool blueprintsAdded = false;

        Vessel triggerCraft;

        [KSPField(isPersistant = true)]
        public double altitude = 0;
        [KSPField(isPersistant = true)]
        public double latitude = 0;
        [KSPField(isPersistant = true)]
        public double longitude = 0;

        public bool hideBoid = false;
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
        public bool boid = false;
        public int triggerRange = 10;
        private bool checking = false;
        public bool isLoaded = false;

        #endregion

        [KSPField(unfocusedRange = 10, guiActiveUnfocused = true, isPersistant = true, guiActiveEditor = false, guiActive = true, guiName = "OPEN HOLOKRON"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool deploy = false;

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && isLoaded)
            {
                if (!hideBoid && !this.vessel.isActiveVessel)
                {
                    OrXLog.DrawTextureOnWorldPos(pos, OrXLog.instance.HoloTargetTexture, new Vector2(8, 8));
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
                foreach (Collider c in part.GetComponents<Collider>())
                {
                    c.enabled = false;
                    //Destroy(c);
                }
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (isLoaded)
            {
                if (!this.vessel.isActiveVessel)
                {
                    if (boid)
                    {
                        if (!hideBoid)
                        {
                            OrXHoloKron.instance.showTargets = false;
                            double targetDistance = double.MaxValue;
                            double _latDiff = 0;
                            double _lonDiff = 0;
                            double _altDiff = 0;

                            if (FlightGlobals.ActiveVessel.altitude <= this.vessel.altitude)
                            {
                                _altDiff = this.vessel.altitude - FlightGlobals.ActiveVessel.altitude;
                            }
                            else
                            {
                                _altDiff = FlightGlobals.ActiveVessel.altitude - this.vessel.altitude;
                            }

                            if (this.vessel.latitude >= 0)
                            {
                                if (FlightGlobals.ActiveVessel.latitude >= this.vessel.latitude)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - this.vessel.latitude;
                                }
                                else
                                {
                                    _latDiff = this.vessel.latitude - FlightGlobals.ActiveVessel.latitude;
                                }
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.latitude >= 0)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - this.vessel.latitude;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.latitude <= this.vessel.latitude)
                                    {
                                        _latDiff = FlightGlobals.ActiveVessel.latitude - this.vessel.latitude;
                                    }
                                    else
                                    {

                                        _latDiff = this.vessel.latitude - FlightGlobals.ActiveVessel.latitude;
                                    }
                                }
                            }

                            if (this.vessel.longitude >= 0)
                            {
                                if (FlightGlobals.ActiveVessel.longitude >= this.vessel.longitude)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - this.vessel.longitude;
                                }
                                else
                                {
                                    _lonDiff = this.vessel.longitude - FlightGlobals.ActiveVessel.longitude;
                                }
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.longitude >= 0)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - this.vessel.longitude;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.longitude <= this.vessel.longitude)
                                    {
                                        _lonDiff = FlightGlobals.ActiveVessel.longitude - this.vessel.longitude;
                                    }
                                    else
                                    {

                                        _lonDiff = this.vessel.longitude - FlightGlobals.ActiveVessel.longitude;
                                    }
                                }
                            }

                            double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                            double _altDiffDeg = _altDiff * OrXTargetDistance.instance.degPerMeter;
                            double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                            double _targetDistance = Math.Sqrt(altAdded) * OrXTargetDistance.instance.mPerDegree;
                            Debug.Log("[Module OrX Mission] ==== TARGET DISTANCE: " + targetDistance);

                            if (_targetDistance <= 10)
                            {
                                Debug.Log("[OrX Target Distance - Mission] ==== TARGET DISTANCE: " + targetDistance);
                                hideBoid = true;
                                OrXHoloKron.instance.GetNextCoord();
                            }
                        }
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.isEVA)
                        {
                            if (OrXHoloKron.instance.Scuba)
                            {
                                if (deploy)
                                {
                                    deploy = false;
                                    hideBoid = true;
                                    Debug.Log("[Module OrX Mission] === OPENING '" + HoloKronName + "' === "); ;
                                    OrXHoloKron.instance.OpenHoloKron(HoloKronName, this.vessel, FlightGlobals.ActiveVessel);
                                }
                            }
                            else
                            {
                                if (deploy)
                                {
                                    deploy = false;
                                    ScreenMessages.PostScreenMessage(new ScreenMessage("Get into a vehicle to start the challenge", 4, ScreenMessageStyle.UPPER_CENTER));
                                }
                            }
                        }
                        else
                        {
                            if (deploy)
                            {
                                deploy = false;
                                Debug.Log("[Module OrX Mission] === OPENING '" + HoloKronName + "' === "); ;
                                OrXHoloKron.instance.OpenHoloKron(HoloKronName, this.vessel, FlightGlobals.ActiveVessel);
                                triggerCraft = FlightGlobals.ActiveVessel;
                            }
                        }
                    }

                    if (hideBoid && (tLevel == maxfade) && boid)
                    {
                        this.part.explode();
                    }
                }
            }
        }

        bool hasMoved = false;

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                this.vessel.IgnoreGForces(240);
                this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
                if (isLoaded)
                {
                    if (this.vessel != OrXVesselMove.Instance.MovingVessel)
                    {
                        if (hasMoved)
                        {
                            pos = new Vector3d(latitude, longitude, altitude);
                            hasMoved = false;
                        }
                        this.vessel.SetPosition(pos, true);
                    }
                    else
                    {
                        if (longitude != this.vessel.longitude || latitude != this.vessel.latitude)
                        {
                            hasMoved = true;
                            latitude = this.vessel.latitude;
                            longitude = this.vessel.longitude;
                            altitude = this.vessel.altitude - this.vessel.radarAltitude + 4;
                        }
                    }

                    if (hideBoid && (tLevel > maxfade) || !hideBoid && (tLevel < maxVis) || triggerHide)
                    {
                        float delta = Time.deltaTime * rateOfFade;
                        if (hideBoid && (tLevel > maxfade))
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
                }
            }
            else
            {
                this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
            }
        }

    }
}