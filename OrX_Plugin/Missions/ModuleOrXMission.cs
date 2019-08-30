using OrX.spawn;
using OrX.wind;
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


        #region Part Fields

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

        #endregion


        [KSPField(unfocusedRange = 5, guiActiveUnfocused = true, isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "OPEN HOLOCACHE"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool deploy = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                Debug.Log("[Module OrX Mission] === OnStart(StartState state) ===");

                triggerCraft = FlightGlobals.ActiveVessel;

                if (!spawned)
                {
                    spawned = true;
                    altitude = SpawnOrX_HoloCache.instance._alt;
                    latitude = SpawnOrX_HoloCache.instance._lat;
                    longitude = SpawnOrX_HoloCache.instance._lon;
                    OrXHoloCache.instance.SetupHolo(this.vessel);
                }
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

                    if (HoloCacheName != "" && HoloCacheName != string.Empty)
                    {
                        OrXHoloCache.instance.OpenHoloCache(HoloCacheName);
                        FlightGlobals.ForceSetActiveVessel(this.vessel);
                    }
                    else
                    {
                        deploy = false;
                        setup = false;
                        FlightGlobals.ForceSetActiveVessel(this.vessel);
                    }
                }

                if (this.vessel.isActiveVessel)
                {
                    FlightGlobals.ForceSetActiveVessel(triggerCraft);
                }
                else
                {
                    if (FlightGlobals.ActiveVessel != triggerCraft && triggerCraft != this.vessel)
                    {
                        triggerCraft = FlightGlobals.ActiveVessel;
                    }
                }
            }
            base.OnFixedUpdate();
        }

        void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (this.vessel.parts.Count == 1)
                {
                    this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
                    this.vessel.IgnoreGForces(240);
                    this.vessel.geeForce = 0;
                    this.vessel.geeForce_immediate = 0;
                    this.vessel.SetPosition(this.vessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude), true);
                    //this.vessel.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    //this.vessel.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
                else
                {
                    //Destroy(this);
                }
            }
            else
            {
                this.part.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
            }
        }
    }
}