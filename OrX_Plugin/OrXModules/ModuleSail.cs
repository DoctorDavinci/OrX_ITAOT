using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrXWind
{
    public class ModuleSail : PartModule
    {
        [KSPField(isPersistant = true)]
        private bool setup = false;

        [KSPField(isPersistant = true)]
        public float dlc = 0;

        Rigidbody rigidBody;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
            }

            base.OnStart(state);
        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!this.vessel.packed)
                {
                    if (!setup)
                    {
                        var ls = this.part.FindModuleImplementing<ModuleLiftingSurface>();
                        if (ls != null)
                        {
                            dlc = ls.deflectionLiftCoeff * 10;
                            Debug.Log("[OrX Wind] SAIL ... " + this.part.name + " - Deflection Lift Coefficient: " + dlc);
                            float speed = WindGUI.instance._wi * ((1 / Vector3.Angle(WindGUI.instance.windDirection, this.part.transform.forward) * dlc));
                            Debug.Log("[OrX Wind] SAIL ... speed: " + speed);
                        }
                        setup = true;
                    }
                    else
                    {
                        if (!WindGUI.instance.enableWind) 
                        {
                            Debug.Log("[OrX Wind] ... Taking the wind from your sails");
                            Destroy(this);
                        }
                        else 
                        {
                            float speed = WindGUI.instance._wi; //* ((1 / Vector3.Angle(WindGUI.instance.windDirection, this.part.transform.forward))); //* dlc)); 

                            rigidBody = this.part.GetComponent<Rigidbody>();
                            rigidBody.AddForce((WindGUI.instance.windDirection - this.part.transform.forward).normalized * speed);
                        }
                    }
                }
            }
        }
    }
}
