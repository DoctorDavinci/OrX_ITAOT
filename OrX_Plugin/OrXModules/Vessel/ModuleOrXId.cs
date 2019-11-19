using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXId : PartModule
    {
        [KSPField(isPersistant = true)]
        public int _partCount = 0;

        public bool kill = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                 part.force_activate();
                
                 List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                 while (p.MoveNext())
                 {
                     if (p.Current != null)
                     {
                        _partCount += 1;
                    }
                 }
                 p.Dispose();
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
    }
}