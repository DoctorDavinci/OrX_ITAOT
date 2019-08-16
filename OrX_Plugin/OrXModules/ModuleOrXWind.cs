using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX.wind
{
    public class ModuleOrXWind : PartModule
    {
        Rigidbody rigidBody;
        public float deflectionLiftCoeff = 0;
        private float _modifier = 0;
        private float modifier = 0;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                var mls = this.part.FindModuleImplementing<ModuleLiftingSurface>();
                if (mls != null)
                {
                    deflectionLiftCoeff = mls.deflectionLiftCoeff;
                    if (deflectionLiftCoeff >= 1)
                    {
                        modifier = (deflectionLiftCoeff / deflectionLiftCoeff) / (deflectionLiftCoeff / deflectionLiftCoeff);
                    }
                    else
                    {
                        modifier = deflectionLiftCoeff;
                    }
                }
            }
            base.OnStart(state);
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
            }
        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!this.vessel.packed)
                {
                    if (!OrXWeatherSim.instance.enableWind) // if Wind is not enabled
                    {
                        Debug.Log("[Module OrX Wind] " + this.part.name + " is killing the weather man .....");
                        Destroy(this);
                    }
                    else // if Wind is enabled
                    {
                        if (this.vessel.mainBody.atmosphere)
                        {
                            if (this.vessel.mainBody.atmDensityASL >= 0.007)
                            {
                                Blow();
                            }
                        }
                    }
                }
            }
        }

        private void Blow()
        {
            _modifier = modifier * (Vector3.Angle(this.part.transform.up, OrXWeatherSim.instance.windDirection) / 100);
            Vector3 direction = Vector3.Slerp(OrXWeatherSim.instance.windDirection, this.part.transform.up, 0.5f);
            rigidBody = this.part.GetComponent<Rigidbody>();
            rigidBody.AddForce(direction * (OrXWeatherSim.instance._wi * _modifier));
        }
    }
}
