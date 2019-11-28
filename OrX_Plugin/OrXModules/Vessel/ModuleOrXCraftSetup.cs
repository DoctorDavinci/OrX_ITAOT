using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXCraftSetup : PartModule
    {
        private bool startup = true;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                List<Part>.Enumerator _parts = vessel.parts.GetEnumerator();
                while (_parts.MoveNext())
                {
                    if (_parts.Current!=null)
                    {
                        _parts.Current.AddModule("ModuleAddSalt", true);
                    }
                }
                _parts.Dispose();
                List<Part>.Enumerator _vesselParts = vessel.parts.GetEnumerator();
                while (_vesselParts.MoveNext())
                {
                    if (_vesselParts.Current != null)
                    {
                        var engines = _vesselParts.Current.FindModuleImplementing<ModuleEngines>();
                        var enginesFX = _vesselParts.Current.FindModuleImplementing<ModuleEnginesFX>();

                        if (engines != null)
                        {
                            _vesselParts.Current.force_activate();
                            engines.ActivateAction(new KSPActionParam(KSPActionGroup.None, KSPActionType.Activate));
                            engines.Activate();
                        }

                        if (enginesFX != null)
                        {
                            _vesselParts.Current.force_activate();
                            enginesFX.ActivateAction(new KSPActionParam(KSPActionGroup.None, KSPActionType.Activate));
                            enginesFX.Activate();
                        }

                        if (_vesselParts.Current.Modules.Contains("BDModulePilotAI") || _vesselParts.Current.Modules.Contains("BDModuleSurfaceAI"))
                        {
                            OrXHoloKron.instance.OnScrnMsgUC("ACTIVATING PILOT");

                            OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND PILOT AI ON " + FlightGlobals.ActiveVessel.vesselName + " ... ACTIVATING =====");
                            _vesselParts.Current.SendMessage("ActivatePilot");
                        }

                        if (_vesselParts.Current.Modules.Contains("MissileFire"))
                        {
                            OrXHoloKron.instance.OnScrnMsgUC("ACTIVATING GUARD MODE");

                            OrXLog.instance.DebugLog("[OrX Craft Setup] ===== FOUND WEAPON MANAGER ON " + FlightGlobals.ActiveVessel.vesselName + " ... ACTIVATING GUARD MODE =====");
                            _vesselParts.Current.SendMessage("ToggleGuardMode");
                        }
                    }
                }
                _vesselParts.Dispose();
                Destroy(this);
            }
            base.OnStart(state);
        }
    }
}
