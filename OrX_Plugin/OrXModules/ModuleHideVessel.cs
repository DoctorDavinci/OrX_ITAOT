using OrX.spawn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OrX
{
    public class ModuleHideVessel : PartModule
    {
        #region Fields

        public bool setup = false;

        private float maxfade = 0f;
        private float surfaceAreaToCloak = 0.0f;

        public bool cloakOn = false;

        private static float UNCLOAKED = 1.0f;
        private static float RENDER_THRESHOLD = 0.0f;
        private float fadePerTime = 0.5f;
        private bool currentShadowState = true;
        private bool recalcCloak = true;
        private float visiblilityLevel = 0;
        private float fadeTime = 10f;
        private float shadowCutoff = 0.0f;
        private bool selfCloak = true;

        #endregion

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                recalcCloak = true;
                recalcSurfaceArea();
                visiblilityLevel = 0;
                part.SetOpacity(visiblilityLevel);
            }
            base.OnStart(state);
        }

        public void Update()
        {
            if (visiblilityLevel == 1)
            {
                Destroy(this);
            }

            if (IsTransitioning())
            {
                recalcCloak = false;
                calcNewCloakLevel();

                foreach (Part p in vessel.parts)
                {
                    if (selfCloak || (p != part))
                    {
                        p.SetOpacity(visiblilityLevel);
                        SetRenderAndShadowStates(p, visiblilityLevel > shadowCutoff, visiblilityLevel > RENDER_THRESHOLD);
                    }
                }
            }
        }

        #region Cloak

        public void engageCloak()
        {
            cloakOn = true;
            UpdateCloakField(null, null);
        }
        public void disengageCloak()
        {
            if (cloakOn)
            {
                cloakOn = false;
                UpdateCloakField(null, null);
            }
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
        private void ReconfigureEvent(Vessel v)
        {
            if (v == null) { return; }

            if (v == vessel)
            {   // This is the cloaking vessel - recalc EC required based on new configuration (unless this is a dock event)
                recalcCloak = true;
                recalcSurfaceArea();
            }
            else
            {   
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