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
        public string missionDescription0 = string.Empty;
        private bool m0 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription1 = string.Empty;
        private bool m1 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription2 = string.Empty;
        private bool m2 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription3 = string.Empty;
        private bool m3 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription4 = string.Empty;
        private bool m4 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription5 = string.Empty;
        private bool m5 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription6 = string.Empty;
        private bool m6 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription7 = string.Empty;
        private bool m7 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription8 = string.Empty;
        private bool m8 = false;

        [KSPField(isPersistant = true)]
        public string missionDescription9 = string.Empty;
        private bool m9 = false;

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

        private bool guiOpened = false;
        private bool scanning = false;
        private bool unlocked = false;
        private bool guiOpen = false;
        public bool saveShip = false;

        float _time = 0;

        Guid vid;

        private string craftToAdd = "";
        private bool techIncluded = false;
        private bool spawnInfected = false;
        private bool crewCap = false;

        int timer = 2;

        #endregion

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                vid = FlightGlobals.ActiveVessel.id;
                List<Part>.Enumerator p = this.vessel.parts.GetEnumerator();
                while (p.MoveNext())
                {
                    if (p.Current.CrewCapacity >= 0)
                    {
                        crewCap = true;
                        break;
                    }
                }
                p.Dispose();

                blueprintsAdded = OrXMissions.instance.blueprintsAdded;
                tech = OrXMissions.instance.techToAdd;
                mCount = OrXMissions.instance.mCount;
                HoloCacheName = OrXMissions.instance.HoloCacheName;
                missionName = OrXMissions.instance.missionName;
                missionType = OrXMissions.instance.missionType;
                challengeType = OrXMissions.instance.challengeType;
                mCount = OrXMissions.instance.mCount;
                completed = OrXMissions.instance.completed;
                Gold = OrXMissions.instance.Gold;
                Silver = OrXMissions.instance.Silver;
                Bronze = OrXMissions.instance.Bronze;
                missionDescription0 = OrXMissions.instance.missionDescription0;
                missionDescription1 = OrXMissions.instance.missionDescription1;
                missionDescription2 = OrXMissions.instance.missionDescription2;
                missionDescription4 = OrXMissions.instance.missionDescription4;
                missionDescription5 = OrXMissions.instance.missionDescription5;
                missionDescription6 = OrXMissions.instance.missionDescription6;
                missionDescription7 = OrXMissions.instance.missionDescription7;
                missionDescription8 = OrXMissions.instance.missionDescription8;
                missionDescription9 = OrXMissions.instance.missionDescription9;
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && !OrXMissions.instance.building)
            {
                if (unlocked)
                {
                    if (!guiOpen)
                    {
                        if (crewCap)
                        {
                            if (vessel.GetCrewCount() >= 0)
                            {
                                OrXMissions.instance.StartMission(HoloCacheName, mCount, this.vessel);
                            }
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.isEVA)
                            {
                                double targetDistance = Vector3d.Distance(FlightGlobals.ActiveVessel.GetWorldPos3D(), this.vessel.GetTransform().position);

                                if (targetDistance <= 10)
                                {
                                    OrXMissions.instance.StartMission(HoloCacheName, mCount, this.vessel);
                                }

                                if (targetDistance >= 25)
                                {
                                    unlocked = false;
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    else
                    {
                        if (unlocked)
                        {
                            if (crewCap)
                            {
                                if (vessel.GetCrewCount() == 0)
                                {
                                    unlocked = false;
                                }
                            }
                            else
                            {
                                unlocked = false;
                            }
                        }
                    }
                }
                else
                {
                    if (!scanning)
                    {
                        if (_time <= 0)
                        {
                            _time = 5;
                        }
                        else
                        {
                            _time -= Time.fixedDeltaTime;

                            if (FlightGlobals.ActiveVessel.isEVA)
                            {
                                scanning = true;
                                Scan();
                            }
                        }
                    }
                }
            }

            base.OnFixedUpdate();
        }

        private void Scan()
        {
            if (!guiOpened)
            {
                double targetDistance = Vector3d.Distance(this.vessel.GetTransform().position, FlightGlobals.ActiveVessel.GetTransform().position);

                if (targetDistance <= 10)
                {
                    unlocked = true;
                    scanning = false;
                    OrXLog.instance.AddToVesselList(this.vessel);
                }
            }
            else
            {
                scanning = false;
            }
        }
    }
}
