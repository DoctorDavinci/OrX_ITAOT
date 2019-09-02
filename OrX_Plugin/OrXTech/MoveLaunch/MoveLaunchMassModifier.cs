using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoveLaunch
{
    public class MoveLaunchMassModifier : PartModule
    {
        public bool modify = true;
        private double defaultMass = 0;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                defaultMass = this.vessel.totalMass;
                this.vessel.totalMass = 0;
            }
            base.OnStart(state);
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (modify)
                {
                    this.vessel.totalMass = 0;
                }
                else
                {
                    StartCoroutine(Drop());
                }
            }
        }

        IEnumerator Drop()
        {
            yield return new WaitForEndOfFrame();
            this.vessel.totalMass = defaultMass / 20;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass / 15;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            this.vessel.totalMass = defaultMass / 10;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            this.vessel.totalMass = defaultMass / 8;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass / 4;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass / 3;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass / 2.5f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            this.vessel.totalMass = defaultMass / 2;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass / 1.7f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass / 1.4f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass / 1.2f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            this.vessel.totalMass = defaultMass;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            Destroy(this);
        }
    }
}
