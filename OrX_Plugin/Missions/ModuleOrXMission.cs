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
        Vector3d pos;
        public bool boid = false;
        public int triggerRange = 10;
        private bool checking = false;
        public bool isLoaded = false;

        #endregion

        [KSPField(unfocusedRange = 5, guiActiveUnfocused = true, isPersistant = true, guiActiveEditor = false, guiActive = true, guiName = "OPEN HOLOCACHE"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool deploy = false;

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && isLoaded)
            {
                if (boid && !hideBoid)
                {
                    DrawTextureOnWorldPos(pos, HoloTargetTexture, new Vector2(8, 8));
                }
            }
        }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                pos = OrXHoloCache.instance.tpoint;
                tLevel = 0;
                part.SetOpacity(tLevel);
                triggerCraft = FlightGlobals.ActiveVessel;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (!this.vessel.isActiveVessel)
            {
                if (deploy && !setup)
                {
                    setup = true;
                    deploy = false;
                    if (HoloCacheName != "" && HoloCacheName != string.Empty)
                    {
                        Debug.Log("[Module OrX Mission] === OPENING '" + HoloCacheName + "' === "); ;
                        OrXHoloCache.instance.OpenHoloCache(HoloCacheName, this.vessel);
                    }
                }
            }
        }

        public void Update()
        {
            if (!FlightGlobals.ready || PauseMenu.isOpen || !vessel.loaded || vessel.HoldPhysics)
                return;

            if (HighLogic.LoadedSceneIsFlight && isLoaded)
            {
                if (this.vessel.parts.Count == 1)
                {
                    this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
                    this.vessel.SetPosition(pos, true);
                }

                if (tLevel == maxfade && hideBoid)
                {
                    if (boid)
                    {
                        part.explosionPotential *= 0.2f;
                        this.part.explode();
                    }
                    else
                    {
                        this.vessel.Die();
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
            else
            {
                this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
            }
        }

        public void HideHolo()
        {
            if (boid)
            {
                part.explosionPotential *= 0.2f;
                this.part.explode();
            }
            else
            {
                hideBoid = true;
                triggerHide = true;
            }
        }

        #region GUI

        private Texture2D redDot;
        public Texture2D HoloTargetTexture
        {
            get { return redDot ? redDot : redDot = GameDatabase.Instance.GetTexture("OrX/Plugin/HoloTarget", false); }
        }
        public static Camera GetMainCamera()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                return FlightCamera.fetch.mainCamera;
            }
            else
            {
                return Camera.main;
            }
        }
        public static void DrawTextureOnWorldPos(Vector3 loc, Texture texture, Vector2 size)
        {
            Vector3 screenPos = GetMainCamera().WorldToViewportPoint(loc);
            if (screenPos.z < 0) return; //dont draw if point is behind camera
            if (screenPos.x != Mathf.Clamp01(screenPos.x)) return; //dont draw if off screen
            if (screenPos.y != Mathf.Clamp01(screenPos.y)) return;
            float xPos = screenPos.x * Screen.width - (0.5f * size.x);
            float yPos = (1 - screenPos.y) * Screen.height - (0.5f * size.y);
            Rect iconRect = new Rect(xPos, yPos, size.x, size.y);
            GUI.DrawTexture(iconRect, texture);
        }

        #endregion
    }
}