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

        Rigidbody _rb;
        bool holdPos = false;

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
                if (holdPos)
                {
                    vessel.IgnoreGForces(240);
                    vessel.angularVelocity = Vector3.zero;
                    vessel.angularMomentum = Vector3.zero;
                    vessel.SetWorldVelocity(Vector3.zero);

                    this.vessel.SetPosition(vessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude));
                }
            }
        }

        public void SetAltitude(double _latitude, double _longitude, double _altitude)
        {
            _rb = vessel.GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            altitude = _altitude;
            latitude = _latitude;
            longitude = _longitude;
            StartCoroutine(SetAltitudeRoutine(_altitude));
        }

        IEnumerator SetAltitudeRoutine(double _altitude)
        {
            if (vessel.altitude <= _altitude)
            {
                vessel.IgnoreGForces(240);
                vessel.SetWorldVelocity(Vector3.zero);
                vessel.Translate((((float)vessel.radarAltitude / 4) * Time.fixedDeltaTime * (vessel.transform.position - FlightGlobals.currentMainBody.transform.position).normalized));
                yield return new WaitForFixedUpdate();
                StartCoroutine(SetAltitudeRoutine(_altitude));
            }
            else
            {
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);
                _rb = vessel.GetComponent<Rigidbody>();
                _rb.isKinematic = false;
                holdPos = true;
            }
        }
    }
}