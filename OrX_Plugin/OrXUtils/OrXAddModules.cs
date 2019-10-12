using UnityEngine;

namespace OrX
{
    /// <summary>
    /// Add the module to all kerbals available. 
    /// </summary>
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    class OrXAddModules : MonoBehaviour
    {
        public static OrXAddModules instance;

        public void Awake()
        {
            
            if (instance) Destroy(instance);
            instance = this;

            Debug.Log("[ORX] === ADDING OrX MODULE ===");
            ConfigNode EVA = new ConfigNode("MODULE");
            EVA.AddValue("name", "ModuleOrX");

            try
            {
                PartLoader.getPartInfoByName("kerbalEVA").partPrefab.AddModule(EVA);
                Debug.Log("[ORX] === ADDED OrX MODULE to kerbalEVA ===");
            }
            catch
            {
                //Debug.Log("[ORX] === ADDED OrX MODULE to kerbalEVA ===");
            }

            try
            {
				PartLoader.getPartInfoByName("kerbalEVAfemale").partPrefab.AddModule(EVA);
                Debug.Log("[ORX] === ADDED OrX MODULE to kerbalEVAfemale ===");

            }
            catch
            {
               // Debug.Log("[ORX] === ADDED OrX MODULE to kerbalEVAfemale ===");
            }
        }
    }
}
