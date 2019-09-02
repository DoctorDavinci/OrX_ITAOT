using System;
using UnityEngine;

namespace Wind
{
    public class ModuleCannonBall : PartModule
    {
        private Rigidbody rigidbody;
        private Vector3 dir;
        private bool loaded = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                dir = SpawnCannonBall.instance.dir;
            }
            base.OnStart(state);
        }

        private void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                try
                {
                    rigidbody = this.part.GetComponent<Rigidbody>();
                    rigidbody.AddForce(dir * 100);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}

