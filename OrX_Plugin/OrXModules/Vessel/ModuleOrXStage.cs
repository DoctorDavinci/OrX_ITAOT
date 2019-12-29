using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXStage : PartModule
    {
        [KSPField(isPersistant = true)]
        public int _stageCount = 0;

        public bool kill = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (OrXHoloKron.instance.buildingMission && OrXHoloKron.instance.dakarRacing)
                {
                    if (this.vessel.LandedOrSplashed && !OrXHoloKron.instance.movingCraft)
                    {
                        part.AddModule("ModuleOrXJason", true);
                    }
                }
            }
        }
    }
}