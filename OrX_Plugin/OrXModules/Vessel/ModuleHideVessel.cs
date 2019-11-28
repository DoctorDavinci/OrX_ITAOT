using OrX.spawn;
using UnityEngine;

namespace OrX
{
    public class ModuleHideVessel : PartModule
    {
        #region Fields

        public bool setup = false;

        private bool currentShadowState = true;
        private float visLev = 0;

        #endregion

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                vessel.IgnoreGForces(500);
                visLev = 0;
                foreach (Part p in vessel.parts)
                {
                    p.SetOpacity(visLev);
                }
                setup = true;
                OrXSpawnHoloKron.instance.spawning = false;
            }
            base.OnStart(state);
        }

        public void Update()
        {
            vessel.IgnoreGForces(500);

            if (visLev == 1)
            {
                Destroy(this);
            }
            else
            {
                if (visLev < 1 && setup)
                {
                    visLev += Time.deltaTime * 0.2f;
                    visLev = Mathf.Clamp(visLev, 0, 1);

                    foreach (Part p in vessel.parts)
                    {
                        p.SetOpacity(visLev);

                        if (p.gameObject != null)
                        {
                            int i;

                            MeshRenderer[] MRs = p.GetComponentsInChildren<MeshRenderer>();
                            for (i = 0; i < MRs.GetLength(0); i++)
                                MRs[i].enabled = visLev > 0;

                            SkinnedMeshRenderer[] SMRs = p.GetComponentsInChildren<SkinnedMeshRenderer>();
                            for (i = 0; i < SMRs.GetLength(0); i++)
                                SMRs[i].enabled = visLev > 0;

                            if (visLev > 0 != currentShadowState)
                            {
                                for (i = 0; i < MRs.GetLength(0); i++)
                                {
                                    if (visLev > 0)
                                        MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                                    else
                                        MRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                                }
                                for (i = 0; i < SMRs.GetLength(0); i++)
                                {
                                    if (visLev > 0)
                                        SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                                    else
                                        SMRs[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                                }
                                currentShadowState = visLev > 0;
                            }
                        }
                    }
                }
            }
        }
    }
}