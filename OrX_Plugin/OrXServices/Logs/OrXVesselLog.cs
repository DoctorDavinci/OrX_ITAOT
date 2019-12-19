using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXVesselLog : MonoBehaviour
    {
        public static OrXVesselLog instance;
        public List<Vessel> _enemyCraft;
        public List<Vessel> _playerCraft;

        public bool _checking = false;
        public bool _finalSpawn = false;
        public bool _removing = false;
        public float _delayMod = 300;
        public int _wave = 1;
        public bool _bdacSaved = false;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }
        private void Start()
        {
            _enemyCraft = new List<Vessel>();
            _playerCraft = new List<Vessel>();
            _wave = 1;
        }

        public void ResetLog()
        {
            _bdacSaved = false;
            _enemyCraft = new List<Vessel>();
            _playerCraft = new List<Vessel>();
            _wave = 1;
            _checking = false;
            _removing = false;
            _finalSpawn = false;
        }

        public void SwitchToNextVessel()
        {
            if (_playerCraft.Count == 0) return;

            int Count = 0;
            int switchCount = 0;

            List<Vessel>.Enumerator _ownedCraft = _playerCraft.GetEnumerator();
            while (_ownedCraft.MoveNext())
            {
                if (_ownedCraft.Current != null)
                {
                    Count += 1;
                    if (_ownedCraft.Current == FlightGlobals.ActiveVessel)
                    {
                        if (Count == _playerCraft.Count)
                        {
                            Count = 1;
                        }
                        else
                        {
                            Count += 1;
                        }
                    }
                }
            }
            _ownedCraft.Dispose();

            List<Vessel>.Enumerator _switchCraft = _playerCraft.GetEnumerator();
            while (_switchCraft.MoveNext())
            {
                if (_switchCraft.Current != null)
                {
                    switchCount += 1;
                    if (Count == switchCount)
                    {
                        FlightGlobals.ForceSetActiveVessel(_switchCraft.Current);
                    }
                }
            }
            _switchCraft.Dispose();
        }
        public void SwitchToPreviousVessel()
        {
            if (_playerCraft.Count == 0) return;

            int Count = 0;
            int switchCount = 0;

            List<Vessel>.Enumerator _ownedCraft = _playerCraft.GetEnumerator();
            while (_ownedCraft.MoveNext())
            {
                if (_ownedCraft.Current != null)
                {
                    Count += 1;
                    if (_ownedCraft.Current == FlightGlobals.ActiveVessel)
                    {
                        if (Count == 1)
                        {
                            Count = _playerCraft.Count;
                        }
                        else
                        {
                            Count -= 1;
                        }
                    }
                }
            }
            _ownedCraft.Dispose();

            List<Vessel>.Enumerator _switchCraft = _playerCraft.GetEnumerator();
            while (_switchCraft.MoveNext())
            {
                if (_switchCraft.Current != null)
                {
                    switchCount += 1;
                    if (Count == switchCount)
                    {
                        FlightGlobals.ForceSetActiveVessel(_switchCraft.Current);
                    }
                }
            }
            _switchCraft.Dispose();
        }
        public void GetVesselList()
        {
            try
            {
                _enemyCraft.Clear();
                _playerCraft.Clear();
                List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
                while (v.MoveNext())
                {
                    if (v.Current != null)
                    {
                        List<Part>.Enumerator p = v.Current.parts.GetEnumerator();
                        while (p.MoveNext())
                        {
                            if (p.Current != null)
                            {
                                if (p.Current.Modules.Contains<ModuleOrXWMI>())
                                {
                                    var _wmi = p.Current.FindModuleImplementing<ModuleOrXWMI>();
                                    if (!_wmi._owned)
                                    {
                                        _enemyCraft.Add(v.Current);
                                    }
                                    else
                                    {
                                        _playerCraft.Add(v.Current);
                                    }
                                }
                            }
                        }
                        p.Dispose();
                    }
                }
                v.Dispose();
            }
            catch
            {

            }
        }

        bool _checkingPlayerList = false;

        public void AddToPlayerVesselList(Vessel data)
        {
            if (!_playerCraft.Contains(data))
            {
                OrXHoloKron.instance._playerVesselCount += 1;
                _playerCraft.Add(data);
            }
        }
        public void CheckPlayerVesselList()
        {
            StartCoroutine(GetOwnedVessel());
        }
        IEnumerator GetOwnedVessel()
        {
            if (!_checkingPlayerList && !_bdacSaved)
            {
                _checkingPlayerList = true;
                if (_playerCraft.Count != 0)
                {
                    int r = new System.Random().Next(0, _playerCraft.Count);
                    int _count = 0;
                    bool _remove = false;
                    Vessel _craft = new Vessel();
                    yield return new WaitForFixedUpdate();
                    if (r == 0)
                    {
                        r = 1;
                    }
                    List<Vessel>.Enumerator loggedCraft = _playerCraft.GetEnumerator();
                    while (loggedCraft.MoveNext())
                    {
                        if (loggedCraft.Current != null)
                        {
                            _count += 1;
                            if (_count == r)
                            {
                                if (FlightGlobals.Vessels.Contains(loggedCraft.Current))
                                {
                                    OrXLog.instance.DebugLog("[OrX Vessel Log - Get Owned Vessel] === Switching to " + loggedCraft.Current.vesselName + " ===");
                                    FlightGlobals.ForceSetActiveVessel(loggedCraft.Current);
                                    _checkingPlayerList = false;
                                }
                                else
                                {
                                    _craft = loggedCraft.Current;
                                    _remove = true;
                                }
                            }
                        }
                    }
                    loggedCraft.Dispose();

                    if (_remove)
                    {
                        _playerCraft.Remove(_craft);
                        yield return new WaitForFixedUpdate();
                        _checkingPlayerList = false;
                        StartCoroutine(GetOwnedVessel());
                    }
                }
                else
                {
                    if (OrXHoloKron.instance.bdaChallenge)
                    {
                        _bdacSaved = true;
                        _checkingPlayerList = false;

                        OrXLog.instance.DebugLog("[OrX Vessel Log - Get Owned Vessel] === Player Vessel list is empty ... GAME OVER ===");
                        OrXHoloKron.instance.OnScrnMsgUC("<b>GAME OVER</b>");
                        OrXHoloKron.instance.SaveBDAcScore();
                    }
                }
            }
        }

        public void AddToEnemyVesselList(Vessel data)
        {
            if (!_enemyCraft.Contains(data))
            {
                _enemyCraft.Add(data);
            }
        }
        public void CheckEnemies(bool finalSpawn)
        {
            _finalSpawn = finalSpawn;
            if (OrXHoloKron.instance.IronKerbal && !_checking)
            {
                OrXLog.instance.DebugLog("[OrX Vessel Log - Check Enemies Routine] === STARTING IRON KERBAL ===");
                _checking = true;
                spawn.OrXSpawnHoloKron.instance.SpawnRandomAirSupport(0);
                StartCoroutine(CheckEnemiesRoutine());
            }
        }
        IEnumerator CheckEnemiesRoutine()
        {
            if (_checking)
            {
                yield return new WaitForFixedUpdate();

                if (OrXHoloKron.instance.IronKerbal)
                {
                    float random = 30;

                    if (_enemyCraft.Count <= 2)
                    {
                        if (_wave <= 3)
                        {
                            random += new System.Random().Next(60, 100) / _wave;
                            yield return new WaitForSeconds(random);
                            _wave += 1;
                            spawn.OrXSpawnHoloKron.instance.SpawnRandomAirSupport(0);
                        }
                        else
                        {
                            random += new System.Random().Next(15, 60);
                            yield return new WaitForSeconds(random);
                            _wave += 1;
                            spawn.OrXSpawnHoloKron.instance.SpawnRandomAirSupport(0);
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(random);
                        StartCoroutine(CheckEnemiesRoutine());
                    }
                }
                else
                {
                    _checking = false;
                }
            }
            else
            {
                _checking = false;
            }
        }
    }
}