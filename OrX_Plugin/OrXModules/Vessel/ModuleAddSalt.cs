using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleAddSalt : PartModule
    {
        public override void OnStart(StartState state)
        {
            part.force_activate();
            base.OnStart(state);
        }
        public void AddSalt(bool _owned)
        {
            if (!_owned)
            {
                part.OnJustAboutToBeDestroyed += OnJustAboutToBeDestroyed;
            }
        }
        public void OnJustAboutToBeDestroyed()
        {
            OrXHoloKron.instance.salt += part.mass * 4000;
            OrX_KC.instance.salt += part.mass * 4000;
        }
        IEnumerator DebrisCheck()
        {
            yield return new WaitForSeconds(30);

            if (part == vessel.rootPart)
            {
                if (vessel.vesselName.Contains("Debris") || vessel.vesselType == VesselType.Debris)
                {
                    if (vessel.LandedOrSplashed)
                    {
                        bool _continue = true;

                        try
                        {
                            List<Part>.Enumerator _parts = vessel.parts.GetEnumerator();
                            while (_parts.MoveNext())
                            {
                                if (_parts.Current != null)
                                {
                                    if (_parts.Current.Modules.Contains("MissileFire"))
                                    {
                                        _continue = false;
                                        break;
                                    }
                                }
                            }
                            _parts.Dispose();
                        }
                        catch
                        {
                            _continue = false;
                        }

                        if (_continue)
                        {
                            vessel.rootPart.AddModule("ModuleOrXJason", true);
                        }
                    }
                }
            }
            StartCoroutine(DebrisCheck());
        }
    }
}