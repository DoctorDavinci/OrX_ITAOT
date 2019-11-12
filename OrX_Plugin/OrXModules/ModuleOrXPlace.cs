using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXPlace : PartModule
    {
        public bool setup = false;

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
        bool tweaked = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                vessel.IgnoreGForces(240);
                if (!vessel.isActiveVessel)
                {
                    List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                    while (p.MoveNext())
                    {
                        if (p.Current != null)
                        {
                            if (p.Current != part && p.Current.Modules.Contains<KerbalEVA>())
                            {
                                //parkingBrake = true;
                            }

                            if (p.Current.Modules.Contains<ModuleWheels.ModuleWheelBrakes>())
                            {
                                //var wheel = p.Current.FindModuleImplementing<ModuleWheels.ModuleWheelBrakes>();
                                //brakeTweakable = wheel.brakeTweakable;
                            }
                        }
                    }
                    p.Dispose();
                }
            }
            base.OnStart(state);
        }

        public void Update()
        {
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
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (parkingBrake)
            {
                //this.vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);

                if (this.vessel.srfSpeed >= 1)
                {
                    List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                    while (p.MoveNext())
                    {
                        if (p.Current != null)
                        {
                            if (p.Current.Modules.Contains<ModuleWheels.ModuleWheelBrakes>())
                            {
                                //var wheel = p.Current.FindModuleImplementing<ModuleWheels.ModuleWheelBrakes>();
                                //wheel.brakeTweakable = 1000;
                                //tweaked = true;
                            }
                        }
                    }
                    p.Dispose();
                }
            }
            else
            {
                if (tweaked)
                {
                    tweaked = false;

                    List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                    while (p.MoveNext())
                    {
                        if (p.Current != null)
                        {
                            if (p.Current.Modules.Contains<ModuleWheels.ModuleWheelBrakes>())
                            {
                                //var wheel = p.Current.FindModuleImplementing<ModuleWheels.ModuleWheelBrakes>();
                                //wheel.brakeTweakable = brakeTweakable;
                                //tweaked = false;
                            }
                        }
                    }
                    p.Dispose();
                }
            }
        }

        public void Die()
        {
            StartCoroutine(DieRoutine());
        }
        IEnumerator DieRoutine()
        {
            cloakOn = true;
            while (visiblilityLevel > 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1);
            this.vessel.Die();
        }
        public void PlaceCraft(bool _challengeStart, float _altToSubtract, float _left, float _pitch)
        {
            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);
            //vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);

            StartCoroutine(Place(_challengeStart, _altToSubtract, _left, _pitch));
        }
        IEnumerator Place(bool _challengeStart, float _altToSubtract, float _left, float _pitch)
        {
            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);

            if (!_challengeStart)
            {
                if (_left >= 360)
                {
                    _left -= 360;
                }

                Quaternion _fixRot = Quaternion.identity;

                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3d.zero);
                _fixRot = Quaternion.FromToRotation(vessel.ReferenceTransform.up, spawn.OrXSpawnHoloKron.instance.UpVect) * vessel.ReferenceTransform.rotation;
                vessel.SetRotation(_fixRot, true);
                _fixRot = Quaternion.FromToRotation(-vessel.ReferenceTransform.right, spawn.OrXSpawnHoloKron.instance.NorthVect) * vessel.ReferenceTransform.rotation;
                vessel.SetRotation(_fixRot, true);
                _fixRot = Quaternion.AngleAxis(_pitch - 90, vessel.ReferenceTransform.right) * vessel.ReferenceTransform.rotation;
                vessel.SetRotation(_fixRot, true);
                _fixRot = Quaternion.AngleAxis(_left, spawn.OrXSpawnHoloKron.instance.UpVect) * vessel.ReferenceTransform.rotation;
                vessel.SetRotation(_fixRot, true);

                if (vessel.rootPart.partInfo.name == "MassiveBooster")
                {
                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ADDING OrX STAGE MODULE TO " + vessel.vesselName);

                    vessel.rootPart.AddModule("ModuleOrXStage", true);
                }
                else
                {
                    //OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ADDING OrX Parking Brake MODULE TO " + this.vessel.vesselName);

                    //this.vessel.rootPart.AddModule("ModuleParkingBrake", true);
                }
                vessel.IgnoreGForces(240);
            }

            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);

            Vector3 UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            float localAlt = (float)vessel.radarAltitude - _altToSubtract;
            float mod = 2;

            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === PLACING " + vessel.vesselName + " ===");
            float dropRate = Mathf.Clamp((localAlt * mod), 0.1f, 200);

            while (!vessel.LandedOrSplashed)
            {
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);
                dropRate = Mathf.Clamp((localAlt * mod), 0.1f, 200);

                if (dropRate > 3)
                {
                    vessel.Translate(dropRate * Time.fixedDeltaTime * -UpVect);
                }
                else
                {
                    if (dropRate <= 1.5f)
                    {
                        dropRate = 1.5f;
                    }

                    vessel.SetWorldVelocity(dropRate * -UpVect);
                }

                localAlt -= dropRate * Time.fixedDeltaTime;
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);

                yield return new WaitForFixedUpdate();
            }
            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);

            if (_challengeStart)
            {
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);
                yield return new WaitForSeconds(0.25f);
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);

                yield return new WaitForSeconds(0.25f);
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);
                yield return new WaitForSeconds(0.25f);
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);
                yield return new WaitForSeconds(0.25f);
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);

                yield return new WaitForSeconds(1.5f);
                OrXHoloKron.instance._placingChallenger = false;
                Destroy(this);
            }
        }
    }
}