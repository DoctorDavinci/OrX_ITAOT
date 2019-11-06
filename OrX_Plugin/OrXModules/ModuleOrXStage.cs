using System.Collections.Generic;

namespace OrX
{
    public class ModuleOrXStage : PartModule
    {
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
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight && !OrXHoloKron.instance.buildingMission)
            {
                if (this.vessel.Landed)
                {
                    //this.part.explosionPotential *= 0.2f;
                    //this.part.explode();
                }
            }
        }
    }
}