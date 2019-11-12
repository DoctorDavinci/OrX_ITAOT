using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXLoadedVesselPlace : PartModule
    {
        public bool setup = false;
        Vector3d _currentPos;
        List<string> _parts;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                vessel.IgnoreGForces(240);
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (vessel.loaded && !setup)
            {
                setup = true;
                if (!spawn.OrXSpawnHoloKron.instance.spawning)
                {
                    OrXLog.instance.SetRange(vessel, 8000);
                    _parts = new List<string>();

                    if (!vessel.isActiveVessel)
                    {
                        _currentPos = new Vector3d(vessel.latitude, vessel.longitude, vessel.altitude + 15);
                        StartCoroutine(Place());
                    }
                }
                else
                {
                    Destroy(this);
                }
            }
        }

        IEnumerator Place()
        {
            if (!spawn.OrXSpawnHoloKron.instance.spawning)
            {
                this.vessel.SetPosition(FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_currentPos.x, (double)_currentPos.y, (double)_currentPos.z));
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);

                Vector3 UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
                float localAlt = (float)vessel.radarAltitude;
                float mod = 2;

                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === PLACING " + vessel.vesselName + " ===");
                float dropRate = Mathf.Clamp((localAlt * mod), 0.1f, 200);

                while (!vessel.LandedOrSplashed)
                {
                    vessel.IgnoreGForces(240);
                    vessel.angularVelocity = Vector3.zero;
                    vessel.angularMomentum = Vector3.zero;
                    vessel.SetWorldVelocity(Vector3.zero);

                    dropRate = Mathf.Clamp((localAlt * mod), 0.1f, 200);

                    if (dropRate > 3)
                    {
                        vessel.Translate(dropRate * Time.fixedDeltaTime * -UpVect);
                    }
                    else
                    {
                        if (dropRate <= 1.5f)
                        {
                            dropRate = 1.5f;
                        }

                        vessel.SetWorldVelocity(dropRate * -UpVect);
                    }

                    localAlt -= dropRate * Time.fixedDeltaTime;

                    yield return new WaitForFixedUpdate();
                }

                vessel.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(this);
            }
            else
            {
                vessel.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(this);
            }
        }
    }
}