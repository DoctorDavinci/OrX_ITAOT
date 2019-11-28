using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXJason : PartModule
    {

        public bool hideGoal = false;
        public bool setup = false;
        private static float maxfade = 0f;
        private static float maxVis = 1f;
        private static float rLevel = 0.0f;
        private bool currentShadowState = true;
        private float tLevel = 1;
        private float rateOfFade = 0.5f;
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


        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
            }
            base.OnStart(state);
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                float delta = Time.deltaTime * rateOfFade;
                if (tLevel > maxfade)
                    delta = -delta;

                tLevel += delta;
                tLevel = Mathf.Clamp(tLevel, maxfade, maxVis);

                List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                while (p.MoveNext())
                {
                    if (p.Current != null)
                    {
                        p.Current.SetOpacity(tLevel);
                        int i;

                        MeshRenderer[] MRs = p.Current.GetComponentsInChildren<MeshRenderer>();
                        for (i = 0; i < MRs.GetLength(0); i++)
                            MRs[i].enabled = tLevel > rLevel;// || !fullRenderHide;

                        SkinnedMeshRenderer[] SMRs = p.Current.GetComponentsInChildren<SkinnedMeshRenderer>();
                        for (i = 0; i < SMRs.GetLength(0); i++)
                            SMRs[i].enabled = tLevel > rLevel;// || !fullRenderHide;

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
                p.Dispose();

                if (tLevel <= 0.001f)
                {
                    this.vessel.Die();
                }
            }
        }
    }
}
