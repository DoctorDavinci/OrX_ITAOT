using System;
using UnityEngine;

using System.Collections.Generic;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXEnemy : MonoBehaviour
    {
        public static OrXEnemy instance;
        public List<Vessel> _enemyCraft;
        public int _enemyCount = 0;
        public bool _checking = false;
        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }
        
        private void Start()
        {
            _enemyCraft = new List<Vessel>();
        }

        public void CheckEnemies(bool _finalSpawn)
        {
            if (!_checking)
            {
                _checking = true;
                StartCoroutine(CheckEnemiesRoutine(_finalSpawn));
            }
        }

        IEnumerator CheckEnemiesRoutine(bool _finalSpawn)
        {
            List<Vessel> _enemiesToRemove = new List<Vessel>();
            yield return new WaitForSeconds(15);
            List<Vessel>.Enumerator _enemies = _enemyCraft.GetEnumerator();
            while (_enemies.MoveNext())
            {
                if (_enemies.Current != null)
                {
                    bool destroyed = true;

                    try
                    {
                        List<Vessel>.Enumerator loadedVessels = FlightGlobals.VesselsLoaded.GetEnumerator();
                        while (loadedVessels.MoveNext())
                        {
                            if (loadedVessels.Current != null)
                            {
                                if (loadedVessels.Current == _enemies.Current)
                                {
                                    destroyed = false;
                                }
                            }
                        }
                        loadedVessels.Dispose();

                        if (destroyed)
                        {
                            _enemiesToRemove.Add(_enemies.Current);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            _enemies.Dispose();

            if (_enemiesToRemove.Count != 0)
            {
                List<Vessel>.Enumerator enemiesToRemove = _enemiesToRemove.GetEnumerator();
                while (enemiesToRemove.MoveNext())
                {
                    if (enemiesToRemove.Current != null)
                    {
                        _enemyCraft.Remove(enemiesToRemove.Current);
                    }
                }
                enemiesToRemove.Dispose();
            }

            if (_finalSpawn && OrXHoloKron.instance._HoloKron != null)
            {
                if (_enemyCraft.Count == 0 && !spawn.OrXSpawnHoloKron.instance.spawning)
                {
                    _checking = false;
                    spawn.OrXSpawnHoloKron.instance.SpawnRandomAirSupport(true, OrXHoloKron.instance.HoloKronName, new Vector3d());
                }
                else
                {
                    StartCoroutine(CheckEnemiesRoutine(_finalSpawn));
                }
            }
            else
            {
                StartCoroutine(CheckEnemiesRoutine(_finalSpawn));
            }
        }
    }
}