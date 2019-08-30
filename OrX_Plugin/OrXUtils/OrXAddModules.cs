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
                Debug.Log("[ORX] === ERROR ADDING OrX MODULE ===");
            }

			try
            {
				PartLoader.getPartInfoByName("kerbalEVAfemale").partPrefab.AddModule(EVA);
                Debug.Log("[ORX] === ADDED OrX MODULE to kerbalEVAfemale ===");

            }
            catch
            {
                Debug.Log("[ORX] === ERROR ADDING OrX MODULE ===");
            }

            try
            {
                if (PartLoader.getPartInfoByName("kerbalEVA").partPrefab.Modules.Contains("ModuleOrX"))
                {
                    Debug.Log("[ORX] === ADDED OrX MODULE to kerbalEVA ===");
                }
            }
            catch
            {

            }
        }
    }
}
