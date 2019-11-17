using OrX.spawn;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OrX
{
    public class ModuleHoldVessel : PartModule
    {
        public bool isLoaded = false;

        [KSPField(isPersistant = true)]
        public double altitude = 0;
        [KSPField(isPersistant = true)]
        public double latitude = 0;
        [KSPField(isPersistant = true)]
        public double longitude = 0;

        float _timer = 3;
        Rigidbody _rb;

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
                if (isLoaded)
                {
                    if (this.vessel != OrXVesselMove.Instance.MovingVessel)
                    {
                        if (vessel.radarAltitude <= altitude)
                        {

                        }
                        else
                        {
                            if (_rb != null)
                            {
                                _rb.isKinematic = false;
                                _rb = null;
                            }

                            vessel.IgnoreGForces(240);
                            vessel.angularVelocity = Vector3.zero;
                            vessel.angularMomentum = Vector3.zero;
                            vessel.SetWorldVelocity(Vector3.zero);
                            this.vessel.SetPosition(vessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude));
                        }
                    }
                }
            }
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (isLoaded)
            {
                if (vessel.radarAltitude <= altitude)
                {
                    if (_rb == null)
                    {
                        _rb = vessel.GetComponent<Rigidbody>();
                    }
                    _rb.isKinematic = true;
                    vessel.IgnoreGForces(240);
                    vessel.SetWorldVelocity(Vector3.zero);
                    vessel.Translate(((float)vessel.radarAltitude / 4) * Time.fixedDeltaTime * (vessel.transform.position - FlightGlobals.currentMainBody.transform.position).normalized);

                    if (_timer <= 0 && !OrXHoloKron.instance.triggerVessel.isActiveVessel)
                    {
                        _timer = float.MaxValue;
                        FlightGlobals.ForceSetActiveVessel(OrXHoloKron.instance.triggerVessel);
                    }
                    else
                    {
                        if (_timer <= 10)
                        {
                            _timer -= Time.fixedDeltaTime;
                        }
                    }
                }
            }
        }
    }
}