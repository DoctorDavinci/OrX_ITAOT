using UnityEngine;

namespace Terrain_Check
{
    public class OrXTerrainCheck : MonoBehaviour
    {
        private bool terrainChecked = false;

        private void Start()
        {
            Debug.LogError("OrX Terrain Check: Start");

            if (HighLogic.LoadedSceneIsFlight)
            {
                Debug.LogError("OrX Terrain Check: Start - Flight Scene");

                if (!terrainChecked)
                {
                    CheckTerrain();
                }
            }
        }

        void Awake()
        {
            Debug.LogError("OrX Terrain Check: Awake");

            if (HighLogic.LoadedSceneIsFlight)
            {
                Debug.LogError("OrX Terrain Check: Awake - Flight Scene");

                if (!terrainChecked)
                {
                    CheckTerrain();
                }
            }
        }

        private void CheckTerrain()
        {
            terrainChecked = true;

            Debug.LogError("OrX Terrain Check: Checking for Landed OrX");

            foreach (Vessel v in FlightGlobals.Vessels)
            {
                if (v.isEVA && v.Landed)
                {
                    Debug.LogError("OrX Terrain Check: Found " + v.vesselName);

                    v.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    v.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    v.geeForce = 0;
                    v.geeForce_immediate = 0;
                    v.GetComponent<Rigidbody>().isKinematic = true;
                    v.altitude += 1;
                    v.GetComponent<Rigidbody>().isKinematic = false;
                    v.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    v.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
            }

            Debug.LogError("OrX Terrain Check: Check Completed");
        }
    }
}