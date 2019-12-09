using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXGameobjectTrash : MonoBehaviour
    {
        public static OrXGameobjectTrash instance;
        GameObject _toDestroy;
        public List<GameObject> _objectsToDestroy;
        bool _destroying = false;

        public void Awake()
        {
            if (instance)
                Destroy(instance);
            instance = this;
        }

        void Start()
        {
            _objectsToDestroy = new List<GameObject>();
        }

        public void EmptyTrashBin()
        {
            OrXLog.instance.DebugLog("[OrX Gameobject Trash] Game Objects To Destroy = " + _objectsToDestroy.Count);
            StartCoroutine(DestroyGameobjectsRoutine());
        }

        IEnumerator DestroyGameobjectsRoutine()
        {
            if (_objectsToDestroy.Count != 0)
            {

                if (!spawn.OrXSpawnHoloKron.instance.spawning)
                {
                    try
                    {
                        for (int i = 0; i < _objectsToDestroy.Count; i++)
                        {
                            if (_objectsToDestroy[i] != null)
                            {
                                _toDestroy = _objectsToDestroy[i];
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        OrXLog.instance.DebugLog("[OrX Gameobject Trash] ERROR - " + e);
                    }

                    if (_toDestroy != null)
                    {
                        _objectsToDestroy.Remove(_toDestroy);
                        yield return new WaitForFixedUpdate();
                       // OrXLog.instance.DebugLog("[OrX Gameobject Trash] Destroying " + _toDestroy.name);
                        Destroy(_toDestroy);
                        yield return new WaitForSeconds(1);
                       // OrXLog.instance.DebugLog("[OrX Gameobject Trash] Rinse and Repeat ......");
                        StartCoroutine(DestroyGameobjectsRoutine());
                    }
                    else
                    {
                        yield return new WaitForSeconds(1);
                        StartCoroutine(DestroyGameobjectsRoutine());
                    }
                }
                else
                {
                    yield return new WaitForSeconds(1);
                    StartCoroutine(DestroyGameobjectsRoutine());
                }
            }
            else
            {
                OrXLog.instance.DebugLog("[OrX Gameobject Trash] TRASH IS EMPTY ..... ");
                _destroying = false;
            }
        }
    }
}