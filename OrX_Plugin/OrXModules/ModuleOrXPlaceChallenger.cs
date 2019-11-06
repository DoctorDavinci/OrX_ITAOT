using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXPlaceChallenger : PartModule
    {
        public bool setup = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                vessel.IgnoreGForces(240);
            }
            base.OnStart(state);
        }

        public void PlaceCraft()
        {
            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);
            vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);

            StartCoroutine(Place());
        }
        IEnumerator Place()
        {
            vessel.IgnoreGForces(240);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);

            Vector3 UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            float localAlt = (float)vessel.radarAltitude;
            float mod = 4;

            OrXLog.instance.DebugLog("[OrX Place Challenger] === PLACING " + this.vessel.vesselName + " ===");
            float dropRate = Mathf.Clamp((localAlt / mod), 0.1f, 200);

            while (!vessel.LandedOrSplashed)
            {
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);
                yield return new WaitForFixedUpdate();

                dropRate = Mathf.Clamp((localAlt / mod), 0.1f, 200);

                if (dropRate > 3)
                {
                    vessel.Translate(dropRate * Time.fixedDeltaTime * -UpVect);
                }
                else
                {
                    if (dropRate <= 1)
                    {
                        dropRate = 1;
                    }

                    vessel.SetWorldVelocity(dropRate * -UpVect);
                }

                localAlt -= dropRate * Time.fixedDeltaTime;
            }
            yield return new WaitForSeconds(0.25f);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);

            yield return new WaitForSeconds(0.25f);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);
            yield return new WaitForSeconds(0.25f);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);
            yield return new WaitForSeconds(0.25f);
            vessel.angularVelocity = Vector3.zero;
            vessel.angularMomentum = Vector3.zero;
            vessel.SetWorldVelocity(Vector3.zero);

            yield return new WaitForSeconds(2);
            OrXHoloKron.instance._placingChallenger = false;
            Destroy(this);
        }
    }
}