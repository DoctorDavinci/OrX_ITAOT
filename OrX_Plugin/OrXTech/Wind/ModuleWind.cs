using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wind
{
    public class ModuleWind : PartModule
    {
        public int randomDirection = 0;
        Rigidbody rigidBody;
        double area = 0;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                List<Part>.Enumerator p = this.vessel.parts.GetEnumerator();
                while (p.MoveNext())
                {
                    if (p.Current.Modules.Contains<ModuleLiftingSurface>())
                    {
                        area += p.Current.skinExposedArea / 2;
                    }
                }
            }
            base.OnStart(state);
        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!this.vessel.packed)
                {
                    if (!WindGUI.instance.enableWind) // if Wind is not enabled
                    {
                        Debug.Log("[Wind} ... Killing the weather man");
                        Destroy(this);
                    }
                    else // if Wind is enabled
                    {
                        if (this.vessel.mainBody.atmDensityASL >=0.007)
                        {
                            if (this.vessel.LandedOrSplashed)
                            {
                                if (this.vessel.Splashed)
                                {
                                    rigidBody = this.vessel.GetComponent<Rigidbody>();
                                    rigidBody.AddForce(WindGUI.instance.windDirection * (WindGUI.instance._wi) * Convert.ToInt32(area));
                                }
                                else
                                {
                                    rigidBody = this.vessel.GetComponent<Rigidbody>();
                                    rigidBody.AddForce(WindGUI.instance.windDirection * (WindGUI.instance._wi / 2) * Convert.ToInt32(area));
                                }
                            }
                            else
                            {
                                if (this.vessel.radarAltitude <= 5000)
                                {
                                    if (this.vessel.radarAltitude <= 1000)
                                    {
                                        rigidBody = this.vessel.GetComponent<Rigidbody>();
                                        rigidBody.AddForce(WindGUI.instance.windDirection * (WindGUI.instance._wi) * Convert.ToInt32(area));
                                    }
                                    else
                                    {
                                        var flyingWind = Convert.ToSingle(this.vessel.radarAltitude / 100);
                                        rigidBody = this.vessel.GetComponent<Rigidbody>();
                                        rigidBody.AddForce(WindGUI.instance.windDirection * (WindGUI.instance._wi / flyingWind) * Convert.ToInt32(area));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
