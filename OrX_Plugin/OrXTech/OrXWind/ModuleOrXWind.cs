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
        private LineRenderer lrWind;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                var mls = this.part.FindModuleImplementing<ModuleLiftingSurface>();
                if (mls != null)
                {
                    deflectionLiftCoeff = mls.deflectionLiftCoeff;
                    modifier = deflectionLiftCoeff;
                }
                else
                {
                    Destroy(this);
                }
            }
            base.OnStart(state);
        }

        private void Start()
        {
            lrWind = new GameObject().AddComponent<LineRenderer>();
            lrWind.material = new Material(Shader.Find("KSP/Emissive/Diffuse"));
            lrWind.material.SetColor("_EmissiveColor", Color.blue);
            lrWind.startWidth = 0.15f;
            lrWind.endWidth = 0.15f;
            lrWind.enabled = false;
            
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
            _modifier = modifier * (Vector3.Angle(this.part.transform.forward, OrXWeatherSim.instance.windDirection) / 100);
            Vector3 direction = Vector3.Slerp(OrXWeatherSim.instance.windDirection, this.part.transform.forward, 0.5f);
            rigidBody = this.part.GetComponent<Rigidbody>();
            rigidBody.AddForce(direction * (OrXWeatherSim.instance._wi * _modifier));
        }
    }
}
