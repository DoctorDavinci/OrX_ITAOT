using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXId : PartModule
    {
        [KSPField(isPersistant = true)]
        public int _partCount = 0;
        float salt = 0;
        public bool kill = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                 part.force_activate();

                float _salt = 0;
                List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                 while (p.MoveNext())
                 {
                     if (p.Current != null)
                     {
                        _partCount += 1;
                        _salt += p.Current.mass;
                     }
                 }
                 p.Dispose();

                salt = _salt / vessel.parts.Count;
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            try
            {
                if (vessel.parts.Count <= _partCount)
                {
                    OrX_KC.instance.salt += salt * (_partCount - vessel.parts.Count);



                    float _salt = 0;
                    List<Part>.Enumerator p = vessel.parts.GetEnumerator();
                    while (p.MoveNext())
                    {
                        if (p.Current != null)
                        {
                            _partCount += 1;
                            _salt += p.Current.mass;
                        }
                    }
                    p.Dispose();

                    salt = _salt / vessel.parts.Count;
                }
            }
            catch
            {

            }
        }
    }
}