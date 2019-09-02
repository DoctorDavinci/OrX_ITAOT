using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wind
{
    public class ModuleSail : PartModule
    {
        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "Activate Sails"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "OFF", enabledText = "ON")]
        public bool sailOn = false;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "Rotate Left"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool rotateLeft = false;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "Rotate Right"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "", enabledText = "")]
        public bool rotateRight = false;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "Deg of Rot"),
         UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, minValue = 0f, maxValue = 90, stepIncrement = 1f)]
        public float angle = 5;

        private bool setup = true;
        public int randomDirection = 0;
        private bool directionRandomized = false;
        Rigidbody rigidBody;

        public Vector3 windDirection;
        public Vector3 sailPosition;

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (setup)
                {
                    setup = false;
                    sailPosition = this.part.transform.forward;
                }
                else
                {
                    if (rotateLeft || rotateRight)
                    {
                        // RotateSail();
                    }
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
                            //Debug.Log("[Wind} ... Taking the wind from your sails");
                            //Destroy(this);
                        }
                        else // if Wind is enabled
                        {
                            if (sailOn)
                            {
                                BlowSails();
                            }
                        }
                    }
                }
            }
        }

        private void RotateSail()
        {
            if (rotateLeft)
            {
                rotateLeft = false;
                rotateRight = false;
                this.part.transform.Rotate(this.part.transform.up, -angle);
                sailPosition = this.part.transform.forward;
            }

            if (rotateRight)
            {
                rotateRight = false;
                rotateLeft = false;
                this.part.transform.Rotate(this.part.transform.up, angle);
                sailPosition = this.part.transform.forward;
            }
        }

        private void BlowSails()
        {
            var srfArea = this.part.skinExposedArea / 2;

            rigidBody = this.part.GetComponent<Rigidbody>();
            rigidBody.AddForce(WindGUI.instance.windDirection * WindGUI.instance._wi);
        }
    }
}
