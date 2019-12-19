using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXWMI : PartModule
    {
        public bool _owned = true;
        public bool _switching = false;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                part.OnJustAboutToDie += OnJustAboutToDie;
            }
            base.OnStart(state);
        }
        public void OnJustAboutToDie()
        {
            if (!_owned)
            {
                OrXVesselLog.instance._enemyCraft.Remove(vessel);
                OrXHoloKron.instance._killCount += 1;
                OrXSounds.instance.sound_OrXFatality.Play();
            }
            else
            {
                OrXVesselLog.instance._playerCraft.Remove(vessel);
                OrXHoloKron.instance._lifeCount += 1;
                OrXSounds.instance.NoSaltSound();
            }
        }
        public void SwitchToEnemy()
        {
            OrXVesselLog.instance.AddToEnemyVesselList(vessel);
            Debug.Log("[OrX Weapon Manager Interface] === SWITCHING " + vessel.vesselName + " TO ENEMY STATUS ===");
            _owned = false;
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (OrXHoloKron.instance.bdaChallenge)
            {
                if (vessel.isActiveVessel && !_owned)
                {
                    if (!spawn.OrXSpawnHoloKron.instance.spawning)
                    {
                        OrXVesselLog.instance.CheckPlayerVesselList();
                    }
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