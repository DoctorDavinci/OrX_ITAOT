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

        public int _enemyCount = 0;
        public bool _checking = false;
        public bool _finalSpawn = false;
        public bool _removing = false;

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }
        private void Start()
        {
            _enemyCraft = new List<Vessel>();
            _playerCraft = new List<Vessel>();
            //GameEvents.onves.Add(onVesselDestroy);
        }

        public void onVesselDestroy(Vessel data)
        {
            //CheckPlayerVesselList(data, true);
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

            List<Vessel>.Enumerator _switchCraft = _playerCraft.GetEnumerator());
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

        public void AddToPlayerVesselList(Vessel data)
        {
            bool added = false;
            List<Vessel>.Enumerator craft = _playerCraft.GetEnumerator(); //CraftDatabase[AddedVessels.ownedCraft].GetEnumerator();
            while (craft.MoveNext())
            {
                if (craft.Current != null)
                {
                    if (craft.Current == data)
                    {
                        OrXLog.instance.DebugLog("[OrX Vessel Log - Add To Player Vessel List] === " + data.vesselName + " is already in the vessel list ===");

                        added = true;
                    }
                }
            }
            craft.Dispose();

            if (!added)
            {
                OrXLog.instance.DebugLog("[OrX Vessel Log - Add To Player Vessel List] === Adding " + data.vesselName + " to owned vessel list ===");
                RemoveFromEnemyVesselList(data);
                _playerCraft.Add(data);
            }
        }
        public void RemoveFromPlayerVesselList(Vessel data, bool _destroyed)
        {
            bool added = false;
            _removing = true;
            List<Vessel>.Enumerator craft = _playerCraft.GetEnumerator();
            while (craft.MoveNext())
            {
                if (craft.Current == data)
                {
                    added = true;
                }
            }
            craft.Dispose();

            if (added)
            {
                _playerCraft.Remove(data);
            }

            if (_destroyed)
            {
                List<Vessel>.Enumerator _craft = _playerCraft.GetEnumerator();
                while (_craft.MoveNext())
                {
                    if (_craft.Current != null)
                    {
                        if (FlightGlobals.Vessels.Contains(_craft.Current))
                        {
                            FlightGlobals.ForceSetActiveVessel(_craft.Current);
                        }
                        else
                        {
                            RemoveFromPlayerVesselList(_craft.Current, true);
                        }
                    }
                }
                craft.Dispose();

                CheckEnemyVesselList(data, true);
            }
        }
        public void CheckPlayerVesselList(Vessel data, bool _destroyed)
        {
            if (data.rootPart.Modules.Contains<ModuleOrXMission>())
            {
                RemoveFromPlayerVesselList(data, _destroyed);
            }
            else
            {
                bool added = false;

                if (_playerCraft.Contains(data))
                {
                    OrXLog.instance.DebugLog("[OrX Vessel Log - Check Player Vessel List] === " + data.vesselName + " is in owned vessels list  ===");
                    added = true;
                }
                else
                {
                    List<Vessel>.Enumerator craft = _playerCraft.GetEnumerator();
                    while (craft.MoveNext())
                    {
                        if (craft.Current != null)
                        {
                            FlightGlobals.ForceSetActiveVessel(craft.Current);
                            break;
                        }
                    }
                    craft.Dispose();
                }

                if (_destroyed)
                {
                    RemoveFromPlayerVesselList(data, _destroyed);
                }
            }
        }

        IEnumerator GetOwnedVessel()
        {
            int r = new System.Random().Next(0, _playerCraft.Count);
            int _count = 0;

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
                            FlightGlobals.ForceSetActiveVessel(loggedCraft.Current);
                        }
                        else
                        {
                            RemoveFromPlayerVesselList(loggedCraft.Current, true);
                            yield return new WaitForFixedUpdate();
                            StartCoroutine(GetOwnedVessel());
                        }
                    }
                }
            }
            loggedCraft.Dispose();
        }

        public void AddToEnemyVesselList(Vessel data)
        {
            bool added = false;

            if (_enemyCraft == null)
            {
                _enemyCraft = new List<Vessel>();
            }

            List<Vessel>.Enumerator craft = _enemyCraft.GetEnumerator(); //CraftDatabase[AddedVessels.ownedCraft].GetEnumerator();
            while (craft.MoveNext())
            {
                if (craft.Current != null)
                {
                    if (craft.Current.id == data.id)
                    {
                        added = true;
                    }
                }
            }
            craft.Dispose();

            if (!added)
            {
                OrXLog.instance.DebugLog("[OrX Vessel Log] === Adding " + data.vesselName + " to owned vessel list ===");

                _enemyCraft.Add(data);
            }
        }
        public void RemoveFromEnemyVesselList(Vessel data)
        {
            bool added = false;
            if (_enemyCraft == null)
            {
                _enemyCraft = new List<Vessel>();
            }

            List<Vessel>.Enumerator craft = _enemyCraft.GetEnumerator();
            while (craft.MoveNext())
            {
                if (craft.Current == data)
                {
                    added = true;
                }
            }
            craft.Dispose();

            if (added)
            {
                _enemyCraft.Remove(data);
            }
        }
        public void CheckEnemyVesselList(Vessel data, bool _destroyed)
        {
            if (_destroyed)
            {
                RemoveFromEnemyVesselList(data);
            }
            else
            {
                bool added = false;
                int count = 0;

                List<Vessel>.Enumerator craft = _enemyCraft.GetEnumerator();
                while (craft.MoveNext())
                {
                    count += 1;
                    if (craft.Current == data)
                    {
                        OrXLog.instance.DebugLog("[OrX Vessel Log] === Vessel is in enemy vessels list  ===");
                        added = true;
                    }
                }
                craft.Dispose();

                if (!added)
                {
                    AddToEnemyVesselList(data);
                }
            }
        }

        public void CheckEnemies(bool finalSpawn)
        {
            _finalSpawn = finalSpawn;
            if (!_checking)
            {
                _checking = true;
                StartCoroutine(CheckEnemiesRoutine());
            }
        }
        IEnumerator CheckEnemiesRoutine()
        {
            //OrXVesselSwitcher.instance.CheckVSWindow();
            yield return new WaitForSeconds(30);
            if (OrXHoloKron.instance._HoloKron != null)
            {
                if (_finalSpawn)
                {
                    if (_enemyCraft.Count <= 2 && !spawn.OrXSpawnHoloKron.instance.spawning)
                    {
                        _checking = false;
                        spawn.OrXSpawnHoloKron.instance.SpawnRandomAirSupport(true, OrXHoloKron.instance.HoloKronName, new Vector3d());
                    }
                    else
                    {
                        StartCoroutine(CheckEnemiesRoutine());
                    }
                }
                else
                {
                    if (_enemyCraft.Count <= 1 && !spawn.OrXSpawnHoloKron.instance.spawning)
                    {
                        _checking = false;
                        spawn.OrXSpawnHoloKron.instance.SpawnRandomAirSupport(true, OrXHoloKron.instance.HoloKronName, new Vector3d());
                    }
                    else
                    {
                        StartCoroutine(CheckEnemiesRoutine());
                    }
                }
            }
        }
    }
}