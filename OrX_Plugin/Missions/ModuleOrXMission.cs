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
        public string HoloCacheName = string.Empty;

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

        public bool setup = false;

        private float maxfade = 0f;
        private float surfaceAreaToCloak = 0.0f;

        public bool cloakOn = false;

        private static float UNCLOAKED = 1.0f;
        private static float RENDER_THRESHOLD = 0.0f;
        private float fadePerTime = 0.5f;
        private bool currentShadowState = true;
        private bool recalcCloak = true;
        private float visiblilityLevel = UNCLOAKED;
        private float fadeTime = 5f;
        private float shadowCutoff = 0.0f;
        private bool selfCloak = true;

        #endregion

        [KSPField(unfocusedRange = 5, guiActiveUnfocused = true, isPersistant = true, guiActiveEditor = false, guiActive = true, guiName = "OPEN HOLOCACHE"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool deploy = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                Debug.Log("[Module OrX Mission] === OnStart(StartState state) ===");
                recalcCloak = true;
                recalcSurfaceArea();
                visiblilityLevel = 0;
                part.SetOpacity(visiblilityLevel);
                triggerCraft = FlightGlobals.ActiveVessel;
                altitude = OrXHoloCache.instance._alt;
                latitude = OrXHoloCache.instance._lat;
                longitude = OrXHoloCache.instance._lon;
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (deploy && !setup)
                {
                    setup = true;
                    triggerCraft = FlightGlobals.ActiveVessel;
                    deploy = false;
                    part.PartActionWindow.enabled = false;

                    if (HoloCacheName != "" && HoloCacheName != string.Empty)
                    {
                        Debug.Log("[Module OrX Mission] === OPENING '" + HoloCacheName + "' === "); ;

                        OrXHoloCache.instance.OpenHoloCache(HoloCacheName, this.vessel);
                    }
                }
            }
            base.OnFixedUpdate();
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (visiblilityLevel == maxfade && setup)
                {
                    this.vessel.DestroyVesselComponents();
                    this.vessel.Die();
                }

                if (IsTransitioning())
                {
                    recalcCloak = false;
                    calcNewCloakLevel();

                    foreach (Part p in vessel.parts)
                        if (selfCloak || (p != part))
                        {
                            p.SetOpacity(visiblilityLevel);
                            SetRenderAndShadowStates(p, visiblilityLevel > shadowCutoff, visiblilityLevel > RENDER_THRESHOLD);
                        }
                }

                if (this.vessel.parts.Count == 1)
                {
                    this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
                    this.vessel.IgnoreGForces(240);
                    this.vessel.geeForce = 0;
                    this.vessel.geeForce_immediate = 0;
                    this.vessel.SetPosition(this.vessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude), true);
                }
            }
            else
            {
                this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
            }
        }

        #region Cloak

        public void engageCloak()
        {
            cloakOn = true;
            UpdateCloakField(null, null);
        }
        public void UpdateCloakField(BaseField field, object oldValueObj)
        {
            // Update in case its been changed
            calcFadeTime();
            recalcSurfaceArea();
            recalcCloak = true;
        }
        private void calcFadeTime()
        {
            // In case fadeTime == 0
            try
            { fadePerTime = (1 - maxfade) / fadeTime; }
            catch (Exception)
            { fadePerTime = 10.0f; }
        }
        private void recalcSurfaceArea()
        {
            Part p;

            if (vessel != null)
            {
                surfaceAreaToCloak = 0.0f;
                for (int i = 0; i < vessel.parts.Count; i++)
                {
                    p = vessel.parts[i];
                    if (p != null)
                        if (selfCloak || (p != part))
                            surfaceAreaToCloak = (float)(surfaceAreaToCloak + p.skinExposedArea);
                }
            }
        }
        private void SetRenderAndShadowStates(Part p, bool shadowsState, bool renderState)
        {
            if (p.gameObject != null)
            {
                int i;

                MeshRenderer[] MRs = p.GetComponentsInChildren<MeshRenderer>();
                for (i = 0; i < MRs.GetLength(0); i++)
                    MRs[i].enabled = renderState;// || !fullRenderHide;

                SkinnedMeshRenderer[] SMRs = p.GetComponentsInChildren<SkinnedMeshRenderer>();
                for (i = 0; i < SMRs.GetLength(0); i++)
                    SMRs[i].enabled = renderState;// || !fullRenderHide;

                if (shadowsState != currentShadowState)
                {
                    for (i = 0; i < MRs.GetLength(0); i++)
                    {
                        if (shadowsState)
                            MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        else
                            MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                    for (i = 0; i < SMRs.GetLength(0); i++)
                    {
                        if (shadowsState)
                            SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        else
                            SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                    currentShadowState = shadowsState;
                }
            }
        }
        protected void calcNewCloakLevel()
        {
            calcFadeTime();
            float delta = Time.deltaTime * fadePerTime;
            if (cloakOn && (visiblilityLevel > maxfade))
                delta = -delta;

            visiblilityLevel += delta;
            visiblilityLevel = Mathf.Clamp(visiblilityLevel, maxfade, UNCLOAKED);
        }
        protected bool IsTransitioning()
        {
            return (cloakOn && (visiblilityLevel > maxfade)) ||     // Cloaking in progress
                   (!cloakOn && (visiblilityLevel < UNCLOAKED)) ||  // Uncloaking in progress
                   recalcCloak;                                     // A forced refresh 
        }

        #endregion
    }
}