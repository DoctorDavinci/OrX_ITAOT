using System;
using UnityEngine;
using System.Collections;
using OrX.parts;
using System.Collections.Generic;
using System.Linq;
using OrX.wind;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXLog : MonoBehaviour
    {
        public static OrXLog instance;

        public static Dictionary<AddedTech, List<string>> UnlockedTech;
        public static Dictionary<AddedTech, List<string>> TechDatabase;

        public enum AddedTech
        {
            addedTech,
            None
        }

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

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        
        private void Start()
        {
            TechDatabase = new Dictionary<AddedTech, List<string>>();
            TechDatabase.Add(AddedTech.addedTech, new List<string>());

            if (TechDatabase != null)
            {
                UnlockedTech = new Dictionary<AddedTech, List<string>>();
                UnlockedTech.Add(AddedTech.addedTech, new List<string>());
            }

            GameEvents.onCrewOnEva.Add(onEVA);
            CheckUpgrades();
        }

        private void onEVA(GameEvents.FromToAction<Part, Part> data)
        {
            StartCoroutine(CheckForOrXModules());
        }

        IEnumerator CheckForOrXModules()
        {
            if (!FlightGlobals.ActiveVessel.packed && !FlightGlobals.ActiveVessel.HoldPhysics)
            {
                if (FlightGlobals.ActiveVessel.isEVA)
                {
                    yield return new WaitForEndOfFrame();

                    Debug.Log("[OrX Log] === Adding OrX Module ===");

                    var OrXmodule = FlightGlobals.ActiveVessel.rootPart.FindModuleImplementing<ModuleOrX>();
                    if (OrXmodule == null)
                    {
                        FlightGlobals.ActiveVessel.rootPart.AddModule("ModuleOrX", true);
                        OrXmodule = FlightGlobals.ActiveVessel.rootPart.FindModuleImplementing<ModuleOrX>();

                        if (OrXmodule != null)
                        {
                            OrXmodule.infected = false;
                            OrXmodule.orx = false;
                        }
                    }
                    else
                    {
                        OrXmodule.infected = false;
                        OrXmodule.orx = false;
                    }
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                    StartCoroutine(CheckForOrXModules());
                }
            }
            else
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(CheckForOrXModules());
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

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (!unlockedScuba)
                {
                    if (FlightGlobals.ActiveVessel.isEVA)
                    {
                        if (FlightGlobals.ActiveVessel.Splashed && FlightGlobals.ActiveVessel.altitude <= -100)
                        {
                            unlockedScuba = true;
                            UnlockScuba();
                        }
                    }
                }

                if (unlockedWind && !OrXWindGUI.instance.HasAddedButton)
                {
                    OrXWindGUI.instance.AddToolbarButton();
                }
            }
        }

        /////////////////////////////////////////////////
        ///

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
    }
}