using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleAddSalt : PartModule
    {
        float _partSalt = 0;

        public override void OnStart(StartState state)
        {
            part.force_activate();
            base.OnStart(state);
        }

        public void AddSalt()
        {
            _partSalt = part.mass;
            //OrXLog.instance.DebugLog("[OrX Add Salt - " + part.name + "] ===== " + _partSalt + " Salt Total =====");
            part.OnJustAboutToBeDestroyed += OnPartAboutToDie;
        }

        public void OnPartAboutToDie()
        {
            if (part.Modules.Contains("MissileFire"))
            {
                if (OrXVesselLog.instance._enemyCraft.Contains(vessel))
                {
                    OrXVesselLog.instance._enemyCraft.Remove(vessel);
                }
            }
            OrXHoloKron.instance.salt += part.mass;
            OrX_KC.instance.salt += part.mass;
            //OrXLog.instance.DebugLog("[OrX Add Salt - " + part.name + "] ===== " + part.mass + " Salt Added =====");
        }
    }
}