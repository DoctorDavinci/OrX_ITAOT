using System;
using System.Collections.Generic;
using System.Reflection;

namespace OrX
{
    public class ModuleOrXWMI : PartModule
    {

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
            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                //this.vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                if (this.vessel.isActiveVessel)
                {
                }
            }
        }

        public void CheckTeam()
        {
            List<PartModule>.Enumerator _pm = part.Modules.GetEnumerator();
            while (_pm.MoveNext())
            {
                if (_pm.Current != null)
                { 
                    if (_pm.Current.moduleName == "MissileFire")
                    {
                        try
                        {
                            Type fieldsType = typeof(PartModule);
                            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                            for (int i = 0; i < fields.Length; i++)
                            {
                                if (fields[i].Name == "team")
                                {
                                    fields[i].GetValue(_pm.Current);
                                    _pm.Current.SendMessage("NextTeam");
                                }
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            _pm.Dispose();
        }
    }
}