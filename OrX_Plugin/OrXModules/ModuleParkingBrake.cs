using System.Collections.Generic;

namespace OrX
{
    public class ModuleParkingBrake : PartModule
    {
        public bool kill = false;

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
            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                //this.vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                if (this.vessel.isActiveVessel)
                {
                    List<Part>.Enumerator p = this.vessel.parts.GetEnumerator();
                    while (p.MoveNext())
                    {
                        //this.vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, false);
                    }
                    p.Dispose();
                    Destroy(this);
                }
            }
        }
    }
}