using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrXWind
{
    public class ModuleSail : PartModule
    {
        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "Activate Sails"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "OFF", enabledText = "ON")]
        public bool sailOn = false;
        /*
        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "Rotate Left"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool rotateLeft = false;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "Rotate Right"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool rotateRight = false;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Deg of Rot"),
         UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, minValue = 0f, maxValue = 90, stepIncrement = 1f)]
        public float angle = 5;
        */
        private bool setup = true;
        public int randomDirection = 0;
        Rigidbody rigidBody;
        public double surfaceArea = 0;

        public Vector3 sailForward;

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
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (setup)
                {
                    setup = false;
                    sailForward = this.part.transform.forward;
                    surfaceArea = this.part.skinExposedArea / 2;
                }
            }
        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!this.vessel.packed)
                {
                    if (!setup)
                    {
                        if (!WindGUI.instance.enableWind) // if Wind is not enabled
                        {
                            if (sailOn)
                            {
                                Debug.Log("[OrX Wind] ... Taking the wind from your sails");
                            }
                            sailOn = false;

                            if (this.part.Modules.Contains<ModuleLiftingSurface>())
                            {
                                Destroy(this);
                            }
                        }
                        else // if Wind is enabled
                        {
                            if (sailOn)
                            {
                                BlowSails();
                            }
                            else
                            {
                                if (!this.part.Modules.Contains<ModuleLiftingSurface>())
                                {
                                    sailOn = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BlowSails()
        {
            sailForward = this.part.transform.up;
            float vOffset = 1 / Vector3.Angle(WindGUI.instance.windDirection, sailForward);
            float speed = WindGUI.instance._wi * ((1 / Vector3.Angle(WindGUI.instance.windDirection, sailForward)) * Convert.ToInt32(surfaceArea));
            rigidBody = this.part.GetComponent<Rigidbody>();
            rigidBody.AddForce((WindGUI.instance.windDirection - sailForward).normalized * speed);
        }
    }
}
