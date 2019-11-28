using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleAddSalt : PartModule
    {
        
        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                GameEvents.onPartDie.Add(AddSalt);
            }
            base.OnStart(state);
        }

        private void AddSalt(Part data)
        {
            OrXHoloKron.instance.salt += part.mass;
            OrX_KC.instance.salt += part.mass;
        }
    }
}