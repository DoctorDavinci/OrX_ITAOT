using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrXWind;
using FinePrint;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXLog : MonoBehaviour
    {
        public static OrXLog instance;

        public List<Waypoint> waypoints;

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

        // HoloKron Tech
        public bool addBlueprints = true;
        public bool addLocalVessels = true;
        public bool addTech = true;
        public bool addInfected = true;
        public bool addLock = true;
        private int loggedVesselCount = 0;
        private int errorCount = 0;

        // Move Launch 
        public bool islandRunway = false;
        public bool TrunkPeninsula = false;
        public bool KerbiniIsland = false;
        public bool MidwayIsland = false;
        public bool NorthPole = false;
        public bool SouthPole = false;
        public bool kscIsandChannel = false;
        public bool kscHarborEast = false;
        public bool MissileRange200Island = false;
        public bool kscIslandNewHarbor = false;
        public bool TirpitzBay = false;
        public bool KerbiniAtol = false;
        public bool kscIslandBeach = false;
        public bool baikerbanur = false;
        public bool pyramids = false;
        public bool runway = false;
        public bool beach = false;


        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        private void Start()
        {
            owned = new List<string>();
            waypoints = new List<Waypoint>();
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
            AddToVesselList(data.to.vessel);
        }

        private void onFlightGlobalsReady(bool data)
        {
            ImportVesselList();
        }

        Waypoint currentWaypoint;
        public static int[] _rColors = new int[]{669, 212, 1212, 359, 5, 1287};
        int waypointColor = 0;

        public void AddWaypoint(string HoloKronName, Vector3 nextLocation)
        {
            System.Random r = new System.Random();
            waypointColor = (int)(r.NextDouble() * _rColors.Count());
            currentWaypoint = new Waypoint();
            currentWaypoint.id = "marker";
            currentWaypoint.seed = _rColors[waypointColor];
            currentWaypoint.name = HoloKronName;
            currentWaypoint.celestialName = FlightGlobals.currentMainBody.name;
            currentWaypoint.longitude = nextLocation.y;
            currentWaypoint.latitude = nextLocation.x;
            currentWaypoint.height = FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude + 1;//TerrainHeight(lat, longi, FlightGlobals.ActiveVessel.mainBody);
            currentWaypoint.altitude = nextLocation.z;
            //currentWaypoint.iconSize = 1;
            Debug.Log("[OrX Target Manager] === ADDING WAYPOINT FOR " + name + " ===");
            waypoints.Add(currentWaypoint);
            ScenarioCustomWaypoints.AddWaypoint(currentWaypoint);
        }

        public void RemoveWaypoint(string HoloKronName, Vector3 nextLocation)
        {
            currentWaypoint = new Waypoint();
            currentWaypoint.id = "marker";
            currentWaypoint.seed = _rColors[waypointColor];
            currentWaypoint.name = HoloKronName;
            currentWaypoint.celestialName = FlightGlobals.currentMainBody.name;
            currentWaypoint.longitude = nextLocation.y;
            currentWaypoint.latitude = nextLocation.x;
            currentWaypoint.height = FlightGlobals.ActiveVessel.altitude - FlightGlobals.ActiveVessel.radarAltitude + 1;//TerrainHeight(lat, longi, FlightGlobals.ActiveVessel.mainBody);
            currentWaypoint.altitude = nextLocation.z;
            //currentWaypoint.iconSize = 1;
            waypoints.Remove(currentWaypoint);
            ScenarioCustomWaypoints.RemoveWaypoint(currentWaypoint);
        }

        #region Checks

        public void onVesselChange(Vessel data)
        {
            if (OrXHoloKron.instance.buildingMission)
            {

            }
            else
            {
                EVAUnlockWS();

                if (!FlightGlobals.ActiveVessel.isEVA)
                {
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.missionTime == 0)
                    {
                        AddToVesselList(FlightGlobals.ActiveVessel);
                    }

                    if (FlightGlobals.ActiveVessel.Splashed)
                    {

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

        #region Keyboard Locks

        private string lockID = "";
        private bool setFocusKeys = true;
        private KeyCode next;
        private KeyCode prev;
        private KeyCode next2;
        private KeyCode prev2;

        [KSPField(isPersistant = true)]
        public bool keysSet = false;

        public void SetFocusKeys()
        {
            if (!keysSet)
            {
                keysSet = true;
                next = GameSettings.FOCUS_NEXT_VESSEL.primary.code;
                prev = GameSettings.FOCUS_PREV_VESSEL.primary.code;

                next2 = GameSettings.FOCUS_NEXT_VESSEL.secondary.code;
                prev2 = GameSettings.FOCUS_PREV_VESSEL.secondary.code;
                Debug.Log("[OrX LOG]: Setting Vessel Focus Hotkeys");
                Debug.Log("[OrX LOG]: " + GameSettings.FOCUS_PREV_VESSEL.primary.code + " changing to NONE");
                Debug.Log("[OrX LOG]: " + GameSettings.FOCUS_PREV_VESSEL.secondary.code + " changing to NONE");

                GameSettings.FOCUS_NEXT_VESSEL.primary.code = KeyCode.None;
                GameSettings.FOCUS_PREV_VESSEL.primary.code = KeyCode.None;
                GameSettings.FOCUS_NEXT_VESSEL.secondary.code = KeyCode.None;
                GameSettings.FOCUS_PREV_VESSEL.secondary.code = KeyCode.None;
            }
        }
        public void ResetFocusKeys()
        {
            if (keysSet)
            {
                keysSet = false;

                Debug.Log("[OrX LOG]: Resetting Vessel Focus Hotkeys");
                GameSettings.FOCUS_NEXT_VESSEL.primary.code = next;
                GameSettings.FOCUS_PREV_VESSEL.primary.code = prev;
                GameSettings.FOCUS_NEXT_VESSEL.secondary.code = next2;
                GameSettings.FOCUS_PREV_VESSEL.secondary.code = prev2;
                Debug.Log("[OrX LOG]: " + next2 + " re-enabled ............................");
                Debug.Log("[OrX LOG]: " + next + " re-enabled ............................");
            }
        }

        public bool _EVALockWS = false;
        public bool _EVALockAD = false;

        KeyCode w;
        KeyCode a;
        KeyCode s;
        KeyCode d;
        KeyCode w2;
        KeyCode a2;
        KeyCode s2;
        KeyCode d2;

        public void EVALockWS()
        {
            if (!_EVALockWS)
            {
                _EVALockWS = true;
                w = GameSettings.EVA_forward.primary.code;
                s = GameSettings.EVA_back.primary.code;
                w2 = GameSettings.EVA_forward.secondary.code;
                s2 = GameSettings.EVA_back.secondary.code;

                Debug.Log("[OrX LOG]: Setting EVA Control Lock");
                Debug.Log("[OrX LOG]: " + w + " changing to NONE");
                Debug.Log("[OrX LOG]: " + s + " changing to NONE");
                Debug.Log("[OrX LOG]: " + w2 + " changing to NONE");
                Debug.Log("[OrX LOG]: " + s2 + " changing to NONE");

                GameSettings.EVA_forward.primary.code = KeyCode.None;
                GameSettings.EVA_back.primary.code = KeyCode.None;
                GameSettings.EVA_forward.secondary.code = KeyCode.None;
                GameSettings.EVA_back.secondary.code = KeyCode.None;
            }
            EVALockAD();
        }
        public void EVALockAD()
        {
            if (!_EVALockAD)
            {
                _EVALockAD = true;
                a = GameSettings.EVA_left.primary.code;
                d = GameSettings.EVA_right.primary.code;
                a2 = GameSettings.EVA_left.secondary.code;
                d2 = GameSettings.EVA_right.secondary.code;

                Debug.Log("[OrX LOG]: Setting EVA Control Lock");
                Debug.Log("[OrX LOG]: " + a + " changing to NONE");
                Debug.Log("[OrX LOG]: " + d + " changing to NONE");
                Debug.Log("[OrX LOG]: " + a2 + " changing to NONE");
                Debug.Log("[OrX LOG]: " + d2 + " changing to NONE");

                GameSettings.EVA_left.primary.code = KeyCode.None;
                GameSettings.EVA_right.primary.code = KeyCode.None;
                GameSettings.EVA_left.secondary.code = KeyCode.None;
                GameSettings.EVA_right.secondary.code = KeyCode.None;
            }
        }
        public void EVAUnlockWS()
        {
            if (_EVALockWS)
            {
                _EVALockWS = false;

                Debug.Log("[OrX LOG]: Resetting EVA Control Lock");
                Debug.Log("[OrX LOG]: " + w + " re-enabled ............................");
                Debug.Log("[OrX LOG]: " + s + " re-enabled ............................");
                Debug.Log("[OrX LOG]: " + w2 + " re-enabled ............................");
                Debug.Log("[OrX LOG]: " + s2 + " re-enabled ............................");

                GameSettings.EVA_forward.primary.code = w;
                GameSettings.EVA_back.primary.code = s;
                GameSettings.EVA_forward.secondary.code = w2;
                GameSettings.EVA_back.secondary.code = s2;
            }
            EVAUnlockAD();
        }
        public void EVAUnlockAD()
        {
            if (_EVALockAD)
            {
                _EVALockAD = false;

                Debug.Log("[OrX LOG]: Resetting EVA Control Lock");
                Debug.Log("[OrX LOG]: " + a + " re-enabled ............................");
                Debug.Log("[OrX LOG]: " + d + " re-enabled ............................");
                Debug.Log("[OrX LOG]: " + a2 + " re-enabled ............................");
                Debug.Log("[OrX LOG]: " + d2 + " re-enabled ............................");

                GameSettings.EVA_left.primary.code = a;
                GameSettings.EVA_right.primary.code = d;
                GameSettings.EVA_left.secondary.code = a2;
                GameSettings.EVA_right.secondary.code = d2;
            }
        }


        public void LockKeyboard()
        {
            Debug.Log("[OrX LOG]: Locking Keyboard");
            InputLockManager.SetControlLock(ControlTypes.ACTIONS_ALL, lockID);
        }

        public void UnlockKeyboard()
        {
            Debug.Log("[OrX LOG]: Unlocking Keyboard");
            InputLockManager.RemoveControlLock(lockID);
        }



        #endregion

    }
}