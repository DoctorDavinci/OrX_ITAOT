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
                vessel.IgnoreGForces(240);
                vessel.angularVelocity = Vector3.zero;
                vessel.angularMomentum = Vector3.zero;
                vessel.SetWorldVelocity(Vector3.zero);
                this.vessel.SetPosition(vessel.mainBody.GetWorldSurfacePosition((double)latitude, (double)longitude, (double)altitude));
            }
        }
    }
}