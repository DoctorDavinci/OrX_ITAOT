using System;
using UnityEngine;
using System.Collections;
using OrX.parts;
using System.Collections.Generic;
using System.Linq;
using OrXWind;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXLog : MonoBehaviour
    {
        public static OrXLog instance;

        public static Dictionary<AddedTech, List<string>> UnlockedTech;
        public static Dictionary<AddedTech, List<string>> TechDatabase;

        public static Dictionary<AddedVessels, List<Guid>> OwnedVessels;
        public static Dictionary<AddedVessels, List<Guid>> CraftDatabase;
        public enum AddedVessels
        {
            ownedCraft,
            None
        }

        public enum AddedTech
        {
            addedTech,
            None
        }

        Guid _id = Guid.Empty;

        List<string> owned;


        public bool mission = false;
        public bool story = false;
        public bool building = false;

        public bool unlockTech = false;
        public bool unlockedScuba = true;
        public bool unlockedTractorBeam = false;
        public bool unlockedCloak = false;
        public bool unlockedGrapple = false;
        public bool unlockedBit = false;
        public bool unlockedWind = false;

        // HoloCache Tech
        public bool addBlueprints = true;
        public bool addLocalVessels = true;
        public bool addTech = true;
        public bool addInfected = true;
        public bool addLock = true;
        private int loggedVesselCount = 0;
        private int errorCount = 0;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        
        private void Start()
        {
            owned = new List<string>();
            TechDatabase = new Dictionary<AddedTech, List<string>>();
            TechDatabase.Add(AddedTech.addedTech, new List<string>());

            if (TechDatabase != null)
            {
                UnlockedTech = new Dictionary<AddedTech, List<string>>();
                UnlockedTech.Add(AddedTech.addedTech, new List<string>());
            }

            CraftDatabase = new Dictionary<AddedVessels, List<Guid>>();
            CraftDatabase.Add(AddedVessels.ownedCraft, new List<Guid>());

            if (CraftDatabase != null)
            {
                CraftDatabase = new Dictionary<AddedVessels, List<Guid>>();
                CraftDatabase.Add(AddedVessels.ownedCraft, new List<Guid>());
            }

            GameEvents.OnFlightGlobalsReady.Add(onFlightGlobalsReady);
            GameEvents.onVesselChange.Add(onVesselChange);
            GameEvents.onCrewOnEva.Add(onEVA);
            GameEvents.onCrewBoardVessel.Add(onCrewBoarding);
            CheckUpgrades();
        }

        private void onCrewBoarding(GameEvents.FromToAction<Part, Part> data)
        {
            AddToVesselList(FlightGlobals.ActiveVessel);
        }

        private void onFlightGlobalsReady(bool data)
        {
            ImportVesselList();
        }

        #region Checks

        public void onVesselChange(Vessel data)
        {
            if (building)
            {

            }
            else
            {
                if (!FlightGlobals.ActiveVessel.isEVA)
                {

                }
                else
                {
                    if (FlightGlobals.ActiveVessel.missionTime == 0)
                    {
                        AddToVesselList(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        private void onEVA(GameEvents.FromToAction<Part, Part> data)
        {
            if (!FlightGlobals.ActiveVessel.packed && !FlightGlobals.ActiveVessel.HoldPhysics)
            {
                if (FlightGlobals.ActiveVessel.isEVA)
                {
                    AddToVesselList(FlightGlobals.ActiveVessel);
                }
                else
                {
                }
            }
            else
            {
            }
        }

        private void CheckUpgrades()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");
            if (file == null)
            {
                file = new ConfigNode();
                ConfigNode ul = file.AddNode("OrX");
                ul.AddValue("unlockedScuba", unlockedScuba);
                ul.AddValue("unlockedTractorBeam", unlockedTractorBeam);
                ul.AddValue("unlockedCloak", unlockedCloak);
                ul.AddValue("unlockedGrapple", unlockedGrapple);
                ul.AddValue("unlockedBit", unlockedBit);
                ul.AddValue("unlockedWind", unlockedWind);
                ul.AddValue("addBlueprints", addBlueprints);
                ul.AddValue("addLocalVessels", addLocalVessels);
                ul.AddValue("addTech", addTech);
                ul.AddValue("addInfected", addInfected);
                ul.AddValue("addLock", addLock);

                foreach (ConfigNode.Value cv in ul.values)
                {
                    string cvEncryptedName = Crypt(cv.name);
                    string cvEncryptedValue = Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }

                file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
            }

            if (file.HasNode("OrX"))
            {
                ConfigNode node = file.GetNode("OrX");

                foreach (ConfigNode.Value value in node.nodes)
                {
                    string cvEncryptedName = Decrypt(value.name);
                    if (cvEncryptedName == "unlockedScuba")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue == "True")
                        {
                            unlockedScuba = true;
                        }
                    }

                    if (cvEncryptedName == "tech")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue != "")
                        {
                            UnlockedTech[AddedTech.addedTech].Add(cvEncryptedValue);
                        }
                    }

                    if (cvEncryptedName == "unlockedWind")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue == "True")
                        {
                            UnlockedTech[AddedTech.addedTech].Add(cvEncryptedValue);
                            unlockedWind = true;
                        }
                    }

                    if (cvEncryptedName == "addBlueprints")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue == "True")
                        {
                            UnlockedTech[AddedTech.addedTech].Add(cvEncryptedName);
                            addBlueprints = true;
                        }
                    }

                    if (cvEncryptedName == "addLocalVessels")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue == "True")
                        {
                            UnlockedTech[AddedTech.addedTech].Add(cvEncryptedName);
                            addLocalVessels = true;
                        }
                    }

                    if (cvEncryptedName == "addTech")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue == "True")
                        {
                            UnlockedTech[AddedTech.addedTech].Add(cvEncryptedName);
                            addTech = true;
                        }
                    }

                    if (cvEncryptedName == "addInfected")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue == "True")
                        {
                            UnlockedTech[AddedTech.addedTech].Add(cvEncryptedName);
                            addInfected = true;
                        }
                    }

                    if (cvEncryptedName == "addLock")
                    {
                        string cvEncryptedValue = Decrypt(value.value);

                        if (cvEncryptedValue == "True")
                        {
                            UnlockedTech[AddedTech.addedTech].Add(cvEncryptedName);
                            addLock = true;
                        }
                    }
                }
            }
        }

        public void AddToVesselList(Vessel data)
        {
            bool added = false;

            if(owned == null)
            {
                owned = new List<string>();
            }

            List<string>.Enumerator craft = owned.GetEnumerator(); //CraftDatabase[AddedVessels.ownedCraft].GetEnumerator();
            while (craft.MoveNext())
            {
                if (craft.Current == data.id.ToString())
                {
                    added = true;
                }
            }
            craft.Dispose();

            if (!added)
            {
                Debug.Log("[OrX Log] === Adding OrX to owned vessel list ===");

                owned.Add(data.id.ToString());
                ExportVesselList();
            }
        }
        public void RemoveFromVesselList(Vessel data)
        {
            bool added = false;

            List<string>.Enumerator craft = owned.GetEnumerator(); 
            while (craft.MoveNext())
            {
                if (craft.Current == data.id.ToString())
                {
                    added = true;
                }
            }
            craft.Dispose();

            if (added)
            {
                owned.Remove(data.id.ToString());
                ExportVesselList();
            }
        }

        public void CheckVesselList(Vessel data)
        {
            if (owned != null)
            {
                if (data.rootPart.Modules.Contains<ModuleOrXMission>())
                {
                    RemoveFromVesselList(data);
                }
                else
                {
                    bool added = false;
                    bool forcedVesselSwitch = false;
                    int count = 0;
                    string guid = data.id.ToString();

                    List<string>.Enumerator craft = owned.GetEnumerator();
                    while (craft.MoveNext())
                    {
                        count += 1;
                        if (craft.Current == guid)
                        {
                            Debug.Log("[OrX Log] === Vessel is in owned vessels list  ===");
                            added = true;
                        }
                    }
                    craft.Dispose();

                    loggedVesselCount = count;

                    if (!added)
                    {
                        int r = new System.Random().Next(1, loggedVesselCount);
                        count = 0;

                        List<string>.Enumerator loggedCraft = owned.GetEnumerator();
                        while (loggedCraft.MoveNext())
                        {
                            count += 1;
                            if (count == r)
                            {
                                guid = loggedCraft.Current;
                                break;
                            }
                        }
                        loggedCraft.Dispose();

                        List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                        try
                        {
                            while (v.MoveNext())
                            {
                                if (v.Current == null) continue;
                                if (!v.Current.loaded || v.Current.packed) continue;
                                if (v.Current.id.ToString() == guid)
                                {
                                    Debug.Log("[OrX Log] === Vessel is not in owned vessels list ... Switching to " + v.Current.vesselName + " ===");

                                    FlightGlobals.ForceSetActiveVessel(v.Current);
                                    forcedVesselSwitch = true;
                                }
                            }
                            v.Dispose();
                        }
                        catch
                        {

                        }
                    }
                }
            }
            else
            {
                owned = new List<string>();
                ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrXVesselList.data");
                if (file == null)
                {
                    file = new ConfigNode();
                }

                ConfigNode ul = file.GetNode(HighLogic.CurrentGame.Title);
                if (ul == null)
                {
                    ul = file.AddNode(HighLogic.CurrentGame.Title);
                }

                foreach (ConfigNode.Value cv in ul.values)
                {
                    owned.Add(cv.value);
                }

                CheckVesselList(data);
            }
        }
        private void ExportVesselList()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrXVesselList.data");
            if (file == null)
            {
                file = new ConfigNode();
            }
            else
            {
                file.ClearData();
            }

            ConfigNode ul = file.AddNode(HighLogic.CurrentGame.Title);
            List<string>.Enumerator craft = owned.GetEnumerator();
            while (craft.MoveNext())
            {
                ul.AddValue("guid", craft.Current);
            }
            craft.Dispose();
            file.Save("GameData/OrX/Plugin/PluginData/OrXVesselList.data");
        }
        private void ImportVesselList()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrXVesselList.data");
            if (file != null)
            {
                ConfigNode ul = file.GetNode(HighLogic.CurrentGame.Title);

                if (ul != null)
                {
                    foreach (ConfigNode.Value cv in ul.values)
                    {
                        owned.Add(cv.value);
                    }
                }
            }
        }

        public void AddTech(string t)
        {
            if (!CheckTechList(t))
            {
                ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");
                ConfigNode node = file.GetNode("OrX");
                string _tech = Crypt("tech");
                string _techName = Crypt(t);
                node.AddValue(_tech, _techName);
                file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
                TechDatabase[AddedTech.addedTech].Add(t);
            }
        }

        public bool CheckTechList(string t)
        {
            bool added = false;
            List<string>.Enumerator technologies = UnlockedTech[AddedTech.addedTech].GetEnumerator();
            while (technologies.MoveNext())
            {
                string tt = technologies.Current.ToString();
                if (tt == t)
                {
                    added = true;
                }
            }

            return added;
        }

        #endregion

        #region Tech Locks

        public void UnlockScuba()
        {
            if (FlightGlobals.ActiveVessel.isEVA)
            {
                unlockedScuba = true;
                ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");

                if (file != null && file.HasNode("OrX"))
                {
                    ConfigNode node = file.GetNode("OrX");

                    foreach (ConfigNode.Value value in node.nodes)
                    {
                        string cvEncryptedName = Decrypt(value.name);
                        if (cvEncryptedName == "unlockedScuba")
                        {
                            value.value = Crypt("True");
                            file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
                        }
                    }
                }

                ModuleOrX sk = null;
                sk = FlightGlobals.ActiveVessel.rootPart.FindModuleImplementing<ModuleOrX>();
                sk.unlockedScuba = true;
            }
        }

        public void UnlockWind()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");
            unlockedWind = true;

            if (file != null && file.HasNode("OrX"))
            {
                ConfigNode node = file.GetNode("OrX");

                foreach (ConfigNode.Value value in node.nodes)
                {
                    string cvEncryptedName = Decrypt(value.name);
                    if (cvEncryptedName == "unlockedWind")
                    {
                        value.value = Crypt("True");
                        file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
                    }
                }
            }
        }

        public void UnlockTractorBeam()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");
            unlockedTractorBeam = true;

            if (file != null && file.HasNode("OrX"))
            {
                ConfigNode node = file.GetNode("OrX");

                foreach (ConfigNode.Value value in node.nodes)
                {
                    string cvEncryptedName = Decrypt(value.name);
                    if (cvEncryptedName == "unlockedTractorBeam")
                    {
                        value.value = Crypt("True");
                        file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
                    }
                }
            }
        }

        public void UnlockCloak()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");
            unlockedCloak = true;

            if (file != null && file.HasNode("OrX"))
            {
                ConfigNode node = file.GetNode("OrX");

                foreach (ConfigNode.Value value in node.nodes)
                {
                    string cvEncryptedName = Decrypt(value.name);
                    if (cvEncryptedName == "unlockedCloak")
                    {
                        value.value = Crypt("True");
                        file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
                    }
                }
            }
        }

        public void UnlockGrapple()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");
            unlockedGrapple = true;

            if (file != null && file.HasNode("OrX"))
            {
                ConfigNode node = file.GetNode("OrX");

                foreach (ConfigNode.Value value in node.nodes)
                {
                    string cvEncryptedName = Decrypt(value.name);
                    if (cvEncryptedName == "unlockedGrapple")
                    {
                        value.value = Crypt("True");
                        file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
                    }
                }
            }
        }

        public void UnlockBit()
        {
            ConfigNode file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");
            unlockedBit = true;

            if (file != null && file.HasNode("OrX"))
            {
                ConfigNode node = file.GetNode("OrX");

                foreach (ConfigNode.Value value in node.nodes)
                {
                    string cvEncryptedName = Decrypt(value.name);
                    if (cvEncryptedName == "unlockedBit")
                    {
                        value.value = Crypt("True");
                        file.Save("GameData/OrX/Plugin/PluginData/OrX.data");
                    }
                }
            }
        }

        #endregion

        #region Crypt

        public string Crypt(string toCrypt)
        {
            char[] chars = toCrypt.ToArray();
            System.Random r = new System.Random(259);
            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = r.Next(0, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }
            return new string(chars);
        }

        public string Decrypt(string scrambled)
        {
            char[] sc = scrambled.ToArray();
            System.Random r = new System.Random(259);
            List<int> swaps = new List<int>();
            for (int i = 0; i < sc.Length; i++)
            {
                swaps.Add(r.Next(0, sc.Length));
            }
            for (int i = sc.Length - 1; i >= 0; i--)
            {
                char temp = sc[swaps[i]];
                sc[swaps[i]] = sc[i];
                sc[i] = temp;
            }
            return new string(sc);
        }
        #endregion

        public static void DrawTextureOnWorldPos(Vector3 worldPos, Texture texture, Vector2 size)
        {
            Vector3 screenPos = GetMainCamera().WorldToViewportPoint(worldPos);
            if (screenPos.z < 0) return; //dont draw if point is behind camera
            if (screenPos.x != Mathf.Clamp01(screenPos.x)) return; //dont draw if off screen
            if (screenPos.y != Mathf.Clamp01(screenPos.y)) return;
            float xPos = screenPos.x * Screen.width - (0.5f * size.x);
            float yPos = (1 - screenPos.y) * Screen.height - (0.5f * size.y);
            Rect iconRect = new Rect(xPos, yPos, size.x, size.y);
            GUI.DrawTexture(iconRect, texture);
        }
        public static Camera GetMainCamera()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                return FlightCamera.fetch.mainCamera;
            }
            else
            {
                return Camera.main;
            }
        }

    }
}