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
               
                List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                while (p.MoveNext())
                {
                    if (p.Current != null && p.Current != part)
                    {
                        p.Current.AddModule("ModuleOrXStage");
                    }
                }
                p.Dispose();
            }
            base.OnStart(state);
        }
    }
}