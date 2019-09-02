using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;
using System.Collections;
using OrX.spawn;
using System.IO;
using System.Text;
using System.Linq;
using Wind;

namespace OrX
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class OrXHoloCache : MonoBehaviour
    {

        public static Dictionary<OrXHoloCache.OrXCoords, List<OrXTargetInfo>> TargetDatabase;
        public static Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>> HoloCacheTargets;
        OrXCoords coords;
        public enum OrXCoords
        {
            Kerbol,
            Moho,
            Eve,
            Gilly,
            Kerbin,
            Mun,
            Minmus,
            Duna,
            Ike,
            Dres,
            Jool,
            Laythe,
            Vall,
            Tylo,
            Bop,
            Pol,
            Eeloo,
            All,
            None
        }

        #region GUI Styles

        //gui styles
        GUIStyle centerLabel;
        GUIStyle centerLabelRed;
        GUIStyle centerLabelOrange;
        GUIStyle centerLabelBlue;
        GUIStyle leftLabel;
        GUIStyle leftLabelRed;
        GUIStyle rightLabelRed;
        GUIStyle leftLabelGray;
        GUIStyle rippleSliderStyle;
        GUIStyle rippleThumbStyle;
        GUIStyle kspTitleLabel;
        GUIStyle middleLeftLabel;
        GUIStyle middleLeftLabelOrange;
        GUIStyle targetModeStyle;
        GUIStyle targetModeStyleSelected;
        GUIStyle redErrorStyle;
        GUIStyle redErrorShadowStyle;

        #endregion

        #region Fields

        string _sth = string.Empty;
        bool showScores = false;

        private bool unlocked = false;

        public bool buildingMission = false;
        public static bool showTargets = true;
        public static Rect WindowRectToolbar;
        public static Rect WindowRectHCGUI;
        public static OrXHoloCache instance;
        //public static bool GAME_UI_ENABLED;
        public static GUISkin OrXGUISkin = HighLogic.Skin;
        public static bool hasAddedButton = false;
        public static bool OrXHCGUIEnabled;

        private bool scanning = false;
        public bool spawnHoloCache = false;

        float toolWindowWidth = 250;
        float toolWindowHeight = 100;
        bool showWindowHCGUI = true;
        bool maySavethisInstance = false;

        private int count = 0;

        float HCGUIHeight;
        public bool reload;
        public string HoloCacheName = string.Empty;

        private double lat = 0;
        private double lon = 0;
        private double alt = 0;

        public float minLoadRange = 1000;

        float HCGUIEntryCount;
        float HCGUIEntryHeight = 24;
        float HCGUIBorder = 5;
        public bool TargetHCGUI;
        int TargetHCGUIIndex;
        bool resetTargetHCGUI;
        public OrXHoloCacheinfo designatedHCGUIInfo;
        private string soi = "";
        Guid vid;

        private static Vector2 _displayViewerPosition = Vector2.zero;
        public Vector3d designatedHCGUICoords => designatedHCGUIInfo.gpsCoordinates;

        Vector3 worldPos;
        Vector3d SpawnCoords;

        private Texture2D redDot;
        public Texture2D HoloTargetTexture
        {
            get { return redDot ? redDot : redDot = GameDatabase.Instance.GetTexture("OrX/Plugin/HoloTarget", false); }
        }

        private Vessel holoCube;

        public bool addCoords = false;
        Rigidbody rigidBody;
        private bool passive = false;
        private bool paused = false;
        private int delay = 300;
        public bool sth = true;
        public bool hide = false;
        CelestialBody SOIcurrent;
        DestroyOnSceneSwitch ds;


        private bool reloadWorldPos = false;
        private string targetLabel;

        private float timer = 1;
        public string _craftFile = string.Empty;
        public bool checking = false;
        private double targetDistance = 0;

        public bool challengeRunning = false;
        public bool setup = false;
        public bool completed = false;
        public string missionName = string.Empty;
        public string missionType = string.Empty;
        public string challengeType = string.Empty;
        public string tech = string.Empty;
        public int mCount = 0;
        public bool spawned = false;
        public string Gold = string.Empty;
        public string Silver = string.Empty;
        public string Bronze = string.Empty;

        [KSPField(isPersistant = true)]
        public bool blueprintsAdded = false;

        public bool saveShip = false;

        private string craftToAddMission = string.Empty;

        public double altitude = 0;
        public double latitude = 0;
        public double longitude = 0;

        public double _altitude = 0;
        public double _latitude = 0;
        public double _longitude = 0;

        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public bool GuiEnabledOrXMissions = false;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _windowHeight = 250;
        private Rect _windowRect;
        public double distance = 0;
        private double missionTime = 0;

        public string techToAdd = string.Empty;

        public string missionDescription0 = string.Empty;
        public string missionDescription1 = string.Empty;
        public string missionDescription2 = string.Empty;
        public string missionDescription3 = string.Empty;
        public string missionDescription4 = string.Empty;
        public string missionDescription5 = string.Empty;
        public string missionDescription6 = string.Empty;
        public string missionDescription7 = string.Empty;
        public string missionDescription8 = string.Empty;
        public string missionDescription9 = string.Empty;

        public string textBox = string.Empty;

        public bool outlawRacing = false;
        public bool Scuba = false;
        public bool windRacing = false;
        public bool snowballFight = false;
        private bool PlayOrXMission = false;
        private bool craftBrowserOpen = false;
        public bool building = false;
        private bool pauseCheck = false;
        public bool resetHoloCache = false;
        public bool checkingMission = false;
        private bool geoCache = true;
        private bool addingBluePrints = false;
        private bool locAdded = false;
        private bool holoSelected = false;
        private bool holoSpawned = false;
        private bool editDescription = false;

        public float pitch = 0;
        public float heading = 0;
        public float windIntensity = 10;
        public float teaseDelay = 0;
        public float windVariability = 50;
        public float variationIntensity = 50;

        public static Rect WindowRectBrowser;
        double _latMission = 0;
        double _lonMission = 0;
        double _altMission = 0;
        Vessel missionCraft;

        public string Password = "OrX";
        public string pas = string.Empty;

        public string missionCraftFile = string.Empty;
        public string blueprintsFile = string.Empty;
        public string holoToAdd = string.Empty;

        int gpsCount = 0;
        double latMission = 0;
        double lonMission = 0;
        double altMission = 0;

        Vector3 lastCoord;
        int locCount = 0;

        string NextCoord;
        List<string> CoordDatabase;
        int coordCount = 0;

        Vector3 startLocation;
        Guid id;

        List<string> stageTimes;
        private double maxDepth = 0;

        List<string> _scoreboard;
        private string challengersName = string.Empty;
        private double topSurfaceSpeed = 0;

        private int ec = 0;

        ConfigNode _file;
        ConfigNode _mission;
        ConfigNode _scoreboard_;
        ConfigNode _blueprints_;

        ConfigNode scoreboard0;
        ConfigNode scoreboard1;
        ConfigNode scoreboard2;
        ConfigNode scoreboard3;
        ConfigNode scoreboard4;
        ConfigNode scoreboard5;
        ConfigNode scoreboard6;
        ConfigNode scoreboard7;
        ConfigNode scoreboard8;
        ConfigNode scoreboard9;

        string nameSB0 = string.Empty;
        string timeSB0 = string.Empty;
        string nameSB1 = string.Empty;
        string timeSB1 = string.Empty;
        string nameSB2 = string.Empty;
        string timeSB2 = string.Empty;
        string nameSB3 = string.Empty;
        string timeSB3 = string.Empty;
        string nameSB4 = string.Empty;
        string timeSB4 = string.Empty;
        string nameSB5 = string.Empty;
        string timeSB5 = string.Empty;
        string nameSB6 = string.Empty;
        string timeSB6 = string.Empty;
        string nameSB7 = string.Empty;
        string timeSB7 = string.Empty;
        string nameSB8 = string.Empty;
        string timeSB8 = string.Empty;
        string nameSB9 = string.Empty;
        string timeSB9 = string.Empty;

        Vector3 nextLocation;
        private double targetDistanceMission = 0;
        public string _targetDistance = string.Empty;

        Quaternion rot;
        double la = 0;
        double lo = 0;
        double al = 0;
        public double _lat = 0f;
        public double _lon = 0f;
        public double _alt = 0f;

        public ConfigNode craft = null;
        public string shipDescription = string.Empty;

        private StringBuilder debugString = new StringBuilder();
        public string craftToSpawn = string.Empty;
        public string cfgToLoad = string.Empty;
        string OrXv = "OrXv";

        public Vessel holoCache;

        #endregion

        #region Spawn HoloCache

        ////////////////////////////


        /// <summary>
        /// /////////////////////////
        /// </summary>
        internal static List<ProtoCrewMember> SelectedCrewData;

        public string craftFile = string.Empty;
        private string flagURL = string.Empty;
        private float spawnTimer = 0.0f;
        private bool loadingCraft = false;

        private double _lat_ = 0.0f;
        private double _lon_ = 0.0f;

        public bool holo = true;
        public bool emptyholo = true;

        public Vector3d _SpawnCoords_()
        {
            return FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt);
        }

        private int boidCount = 10;
        private int spawnRadius = 0;
        public bool boid = false;
        float offsetx = 0;
        float offsety = 0;
        bool _timer = false;
        public string missionCraftLoc = string.Empty;

        public void SpawnBoids()
        {
            spawnRadius = new System.Random().Next(25, 50);
            boidCount = new System.Random().Next(3, 10);
            boid = true;

            int random = new System.Random().Next(1, 4);
            if (random == 1)
            {
                offsetx = 0.001f;
                offsety = -0.001f;
            }
            if (random == 21)
            {
                offsetx = -0.001f;
                offsety = -0.001f;
            }
            if (random == 3)
            {
                offsetx = -0.001f;
                offsety = 0.001f;
            }
            if (random == 4)
            {
                offsetx = 0.001f;
                offsety = 0.001f;
            }

            SpawnBoidRoutine();
        }
        public void SpawnBoidRoutine()
        {

            _lat = FlightGlobals.ActiveVessel.latitude + offsetx;
            _lon = FlightGlobals.ActiveVessel.longitude + offsety;
            if (FlightGlobals.ActiveVessel.Splashed)
            {
                _alt = FlightGlobals.ActiveVessel.altitude -= 25;

            }
            else
            {
                _alt = FlightGlobals.ActiveVessel.altitude + 25;
            }

            string craftFileLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/Boids/Boid.craft";
            emptyholo = false;
            Debug.Log("[Spawn OrX HoloCache] === Spawning Boids ===");
            loadingCraft = false;
            _timer = true;
            holo = false;
            boidCount -= 1;
            StartCoroutine(SpawnEmptyHoloRoutine(craftFileLoc));
        }
        public void SpawnEmptyHoloCache()
        {
            boid = false;
            string craftFileLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloCache/HoloCache.craft";
            emptyholo = true;
            Debug.Log("[Spawn OrX HoloCache] Spawning Empty HoloCache ...... " + craftFile);
            loadingCraft = false;
            _timer = true;
            holo = true;
            _lat = FlightGlobals.ActiveVessel.latitude + 0.0002f;
            _lon = FlightGlobals.ActiveVessel.longitude + 0.0002f;
            _alt = FlightGlobals.ActiveVessel.altitude + 2;

            StartCoroutine(SpawnEmptyHoloRoutine(craftFileLoc));
        }

        private IEnumerator SpawnEmptyHoloRoutine(string craftUrl, List<ProtoCrewMember> crewData = null)
        {
            loadingCraft = true;

            Vector3 worldPos = _SpawnCoords_();
            Vector3 gpsPos = WorldPositionToGeoCoords(worldPos, FlightGlobals.currentMainBody);
            yield return new WaitForFixedUpdate();
            SpawnVesselFromCraftFile(craftUrl, gpsPos, 90, 0, crewData);
        }
        public void CheckSpawnTimer()
        {
            boid = false;
            emptyholo = false;
            string craftFileLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + craftFile + ".craft";
            loadingCraft = true;
            _timer = true;
            Debug.Log("[Spawn OrX HoloCache] Spawning: " + craftFile);
            StartCoroutine(SpawnCraftRoutine(craftFileLoc));
        }
        public void SpawnMissionCraft(string hcLoc, Vector3 loc, bool hc, Quaternion r, float h)
        {
            heading = h;
            rot = r;
            missionCraftLoc = hcLoc;
            SpawnCoords = loc;
            holo = hc;
            emptyholo = false;
            loadingCraft = true;
            _timer = true;
            Debug.Log("[Spawn OrX HoloCache] Spawning HoloCache Craft");
            StartCoroutine(SpawnCraftRoutine(missionCraftLoc));
        }
        private IEnumerator SpawnCraftRoutine(string craftUrl, List<ProtoCrewMember> crewData = null)
        {
            Vector3 pos = new Vector3();

            if (holo)
            {
                pos = _SpawnCoords_();
                SpawnCoords = pos;
            }
            else
            {
                pos = SpawnCoords;
            }
            Vector3 gpsPos = WorldPositionToGeoCoords(pos, FlightGlobals.currentMainBody);
            yield return new WaitForFixedUpdate();
            SpawnVesselFromCraftFile(craftUrl, gpsPos, 90, 0, crewData);
        }

        private void SpawnVesselFromCraftFile(string craftURL, Vector3d gpsCoords, float heading, float pitch, List<ProtoCrewMember> crewData = null)
        {
            HoloCacheData newData = new HoloCacheData();

            newData.craftURL = craftURL;
            newData.latitude = gpsCoords.x;
            newData.longitude = gpsCoords.y;
            newData.altitude = gpsCoords.z;

            Debug.Log("[Spawn OrX HoloCache] SpawnVesselFromCraftFile Altitude: " + newData.altitude);

            newData.body = FlightGlobals.currentMainBody;
            newData.heading = heading;
            newData.pitch = pitch;
            newData.orbiting = false;

            newData.flagURL = flagURL;
            newData.owned = true;
            newData.vesselType = VesselType.Unknown;

            SpawnVessel(newData, crewData);
        }
        private void SpawnVessel(HoloCacheData HoloCacheData, List<ProtoCrewMember> crewData = null)
        {
            //      string gameDataDir = KSPUtil.ApplicationRootPath;
            Debug.Log("[Spawn OrX HoloCache] Spawning " + HoloCacheData.name);

            // Set additional info for landed vessels
            bool landed = false;
            if (!landed)
            {
                landed = true;
                if (HoloCacheData.altitude == null) // || HoloCacheData.altitude < 0)
                {
                    HoloCacheData.altitude = 5;//LocationUtil.TerrainHeight(HoloCacheData.latitude, HoloCacheData.longitude, HoloCacheData.body);
                }
                Debug.Log("[Spawn OrX HoloCache] SpawnVessel Altitude: " + HoloCacheData.altitude);

                //Vector3d pos = HoloCacheData.body.GetWorldSurfacePosition(HoloCacheData.latitude, HoloCacheData.longitude, HoloCacheData.altitude.Value);
                Vector3d pos = HoloCacheData.body.GetRelSurfacePosition(HoloCacheData.latitude, HoloCacheData.longitude, HoloCacheData.altitude.Value);

                HoloCacheData.orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, HoloCacheData.body);
                HoloCacheData.orbit.UpdateFromStateVectors(pos, HoloCacheData.body.getRFrmVel(pos), HoloCacheData.body, Planetarium.GetUniversalTime());
            }

            ConfigNode[] partNodes;
            ShipConstruct shipConstruct = null;
            bool hasClamp = false;
            float lcHeight = 0;
            ConfigNode craftNode;
            Quaternion craftRotation = Quaternion.identity;

            if (!string.IsNullOrEmpty(HoloCacheData.craftURL))
            {
                // Save the current ShipConstruction ship, otherwise the player will see the spawned ship next time they enter the VAB!
                ConfigNode currentShip = ShipConstruction.ShipConfig;

                shipConstruct = ShipConstruction.LoadShip(HoloCacheData.craftURL);
                if (shipConstruct == null)
                {
                    Debug.Log("[Spawn OrX HoloCache] ShipConstruct was null when tried to load '" + HoloCacheData.craftURL +
                      "' (usually this means the file could not be found).");
                    return;//continue;
                }

                craftNode = ConfigNode.Load(HoloCacheData.craftURL);
                lcHeight = 0;
                //craftRotation = rot;

                // Restore ShipConstruction ship
                ShipConstruction.ShipConfig = currentShip;

                // Set the name
                if (string.IsNullOrEmpty(HoloCacheData.name))
                {
                    HoloCacheData.name = craftFile;
                }

                // Set some parameters that need to be at the part level
                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                uint launchID = HighLogic.CurrentGame.launchID++;
                foreach (Part p in shipConstruct.parts)
                {
                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                    p.missionID = missionID;
                    p.launchID = launchID;
                    p.flagURL = flagURL;

                    // Had some issues with this being set to -1 for some ships - can't figure out
                    // why.  End result is the vessel exploding, so let's just set it to a positive
                    // value.
                    p.temperature = 1.0;
                }

                // Create a dummy ProtoVessel, we will use this to dump the parts to a config node.
                // We can't use the config nodes from the .craft file, because they are in a
                // slightly different format than those required for a ProtoVessel (seriously
                // Squad?!?).
                ConfigNode empty = new ConfigNode();
                ProtoVessel dummyProto = new ProtoVessel(empty, null);
                Vessel dummyVessel = new Vessel();
                dummyVessel.parts = shipConstruct.parts;
                dummyProto.vesselRef = dummyVessel;

                // Create the ProtoPartSnapshot objects and then initialize them
                foreach (Part p in shipConstruct.parts)
                {
                    dummyProto.protoPartSnapshots.Add(new ProtoPartSnapshot(p, dummyProto));
                }
                foreach (ProtoPartSnapshot p in dummyProto.protoPartSnapshots)
                {
                    p.storePartRefs();
                }

                // Create the ship's parts

                List<ConfigNode> partNodesL = new List<ConfigNode>();
                foreach (ProtoPartSnapshot snapShot in dummyProto.protoPartSnapshots)
                {
                    ConfigNode node = new ConfigNode("PART");
                    snapShot.Save(node);
                    partNodesL.Add(node);
                }
                partNodes = partNodesL.ToArray();
            }
            else
            {

                // Create crew member array
                ProtoCrewMember[] crewArray = new ProtoCrewMember[HoloCacheData.crew.Count];
                /*
                        int i = 0;
                        foreach (CrewData cd in HoloCacheData.crew)
                        {
                /*
                          // Create the ProtoCrewMember
                          ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
                          if (cd.name != null)
                          {
                            crewMember.KerbalRef.name = cd.name;
                          }

                          crewArray[i++] = crewMember;

                        }
                */
                // Create part nodes
                uint flightId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                partNodes = new ConfigNode[1];
                partNodes[0] = ProtoVessel.CreatePartNode(HoloCacheData.craftPart.name, flightId, crewArray);

                // Default the size class
                //sizeClass = UntrackedObjectClass.A;

                // Set the name
                if (string.IsNullOrEmpty(HoloCacheData.name))
                {
                    HoloCacheData.name = HoloCacheData.craftPart.name;
                }
            }

            // Create additional nodes
            ConfigNode[] additionalNodes = new ConfigNode[0];
            //DiscoveryLevels discoveryLevel = HoloCacheData.owned ? DiscoveryLevels.Owned : DiscoveryLevels.Unowned;
            //additionalNodes[0] = ProtoVessel.CreateDiscoveryNode(discoveryLevel, sizeClass, contract.TimeDeadline, contract.TimeDeadline);

            // Create the config node representation of the ProtoVessel
            ConfigNode protoVesselNode = ProtoVessel.CreateVesselNode(HoloCacheData.name, HoloCacheData.vesselType, HoloCacheData.orbit, 0, partNodes, additionalNodes);

            // Additional seetings for a landed vessel
            if (!HoloCacheData.orbiting)
            {
                Vector3d norm = HoloCacheData.body.GetRelSurfaceNVector(HoloCacheData.latitude, HoloCacheData.longitude);

                double terrainHeight = 0.0;
                if (HoloCacheData.body.pqsController != null)
                {
                    terrainHeight = HoloCacheData.body.pqsController.GetSurfaceHeight(norm) - HoloCacheData.body.pqsController.radius;
                }
                bool splashed = false;// = landed && terrainHeight < 0.001;

                // Create the config node representation of the ProtoVessel
                // Note - flying is experimental, and so far doesn't wOrX
                protoVesselNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                  Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                protoVesselNode.SetValue("landed", (landed && !splashed).ToString());
                protoVesselNode.SetValue("splashed", splashed.ToString());
                protoVesselNode.SetValue("lat", HoloCacheData.latitude.ToString());
                protoVesselNode.SetValue("lon", HoloCacheData.longitude.ToString());
                protoVesselNode.SetValue("alt", HoloCacheData.altitude.ToString());
                protoVesselNode.SetValue("landedAt", HoloCacheData.body.name);

                // Figure out the additional height to subtract
                float lowest = float.MaxValue;
                if (shipConstruct != null)
                {
                    foreach (Part p in shipConstruct.parts)
                    {
                        foreach (Collider collider in p.GetComponentsInChildren<Collider>())
                        {
                            if (collider.gameObject.layer != 21 && collider.enabled)
                            {
                                lowest = Mathf.Min(lowest, collider.bounds.min.y);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Collider collider in HoloCacheData.craftPart.partPrefab.GetComponentsInChildren<Collider>())
                    {
                        if (collider.gameObject.layer != 21 && collider.enabled)
                        {
                            lowest = Mathf.Min(lowest, collider.bounds.min.y);
                        }
                    }
                }

                if (lowest == float.MaxValue)
                {
                    lowest = 0;
                }

                // Figure out the surface height and rotation
                Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
                Quaternion rotation = Quaternion.identity;
                float heading = HoloCacheData.heading;

                if (shipConstruct == null)
                {
                    //rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
                    rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                }
                else if (shipConstruct.shipFacility == EditorFacility.SPH)
                {
                    rotation = rotation * Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
                    heading += 180.0f;
                }
                else
                {
                    rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                    rotation = Quaternion.FromToRotation(Vector3.up, -Vector3.up) * rotation;

                    rotation = craftRotation;


                    HoloCacheData.heading = 0;
                    HoloCacheData.pitch = 0;
                }

                rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
                rotation = rotation * Quaternion.AngleAxis(HoloCacheData.roll, Vector3.down);
                rotation = rotation * Quaternion.AngleAxis(HoloCacheData.pitch, Vector3.left);

                // Set the height and rotation
                if (landed || splashed)
                {
                    float hgt = (shipConstruct != null ? shipConstruct.parts[0] : HoloCacheData.craftPart.partPrefab).localRoot.attPos0.y - lowest;
                    hgt += HoloCacheData.height;
                    protoVesselNode.SetValue("hgt", hgt.ToString(), true);
                }

                if (rot != null)
                {
                    protoVesselNode.SetValue("rot", KSPUtil.WriteQuaternion(rot), true);
                }
                else
                {
                    protoVesselNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
                }

                // Set the normal vector relative to the surface
                Vector3 nrm = (rotation * Vector3.forward);
                protoVesselNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);

                protoVesselNode.SetValue("prst", false.ToString(), true);
            }

            // Add vessel to the game
            ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
            //protoVessel.vesselRef.transform.rotation = protoVessel.rotation;


            // Store the id for later use
            HoloCacheData.id = protoVessel.vesselRef.id;

            //protoVessel.vesselRef.currentStage = 0;
            hasClamp = false;

            StartCoroutine(PlaceSpawnedVessel(protoVessel.vesselRef, !hasClamp));

            // Associate it so that it can be used in contract parameters
            //ContractVesselTracker.Instance.AssociateVessel(HoloCacheData.name, protoVessel.vesselRef);


            //destroy prefabs
            foreach (Part p in FindObjectsOfType<Part>())
            {
                if (!p.vessel)
                {
                    Destroy(p.gameObject);
                }
            }
        }
        private IEnumerator PlaceSpawnedVessel(Vessel v, bool moveVessel)
        {
            loadingCraft = true;
            v.isPersistent = true;
            v.Landed = false;
            v.situation = Vessel.Situations.FLYING;
            while (v.packed)
            {
                yield return null;
            }
            v.SetWorldVelocity(Vector3d.zero);
            //      yield return null;
            //      FlightGlobals.ForceSetActiveVessel(v);
            //yield return null;
            //v.Landed = true;
            //v.situation = Vessel.Situations.LANDED;

            if (holo)
            {
                /*
                var mom = v.FindPartModuleImplementing<ModuleOrXMission>();
                if (mom == null)
                {
                    v.rootPart.AddModule("ModuleOrXMission", true);
                }
                */
                if (!emptyholo)
                {
                    var mom = v.FindPartModuleImplementing<ModuleOrXMission>();
                    mom.spawned = true;
                    mom.HoloCacheName = HoloCacheName;
                    mom.latitude = _lat;
                    mom.longitude = _lon;
                    mom.altitude = _alt;
                }
            }
            else
            {
                if (boid)
                {
                    var mom = v.FindPartModuleImplementing<ModuleOrXMission>();
                    if (mom != null)
                    {
                        Destroy(mom);
                    }

                    v.rootPart.AddModule("ModuleOrXBoid", true);
                }
                else
                {
                    ConfigNode craft = ConfigNode.Load(missionCraftLoc);
                    craft.ClearData();
                    craft.Save(missionCraftLoc);
                }
            }

            v.GoOffRails();
            v.IgnoreGForces(120);
            emptyholo = true;
            StageManager.BeginFlight();
            loadingCraft = false;
            holo = false;
            spawnHoloCache = false;
            if (boid && boidCount >= 0)
            {
                //SpawnBoidRoutine();
            }
            else
            {
                boid = false;
            }
        }

        internal class CrewData
        {
            public string name = null;
            public ProtoCrewMember.Gender? gender = null;
            public bool addToRoster = true;

            public CrewData() { }
            public CrewData(CrewData cd)
            {
                name = cd.name;
                gender = cd.gender;
                addToRoster = cd.addToRoster;
            }
        }
        private class HoloCacheData
        {
            public string name = null;
            public Guid? id = null;
            public string craftURL = null;
            public AvailablePart craftPart = null;
            public string flagURL = null;
            public VesselType vesselType = VesselType.Ship;
            public CelestialBody body = null;
            public Orbit orbit = null;
            public double latitude = 0.0;
            public double longitude = 0.0;
            public double? altitude = null;
            public float height = 0.0f;
            public bool orbiting = false;
            public bool owned = false;
            public List<CrewData> crew = new List<CrewData>();
            public PQSCity pqsCity = null;
            public Vector3d pqsOffset;
            public float heading;
            public float pitch;
            public float roll;

            public HoloCacheData() { }
            public HoloCacheData(HoloCacheData vd)
            {
                name = vd.name;
                id = vd.id;
                craftURL = vd.craftURL;
                craftPart = vd.craftPart;
                flagURL = vd.flagURL;
                vesselType = vd.vesselType;
                body = vd.body;
                orbit = vd.orbit;
                latitude = vd.latitude;
                longitude = vd.longitude;
                altitude = vd.altitude;
                height = vd.height;
                orbiting = vd.orbiting;
                owned = vd.owned;
                pqsCity = vd.pqsCity;
                pqsOffset = vd.pqsOffset;
                heading = vd.heading;
                pitch = vd.pitch;
                roll = vd.roll;

                foreach (CrewData cd in vd.crew)
                {
                    crew.Add(new CrewData(cd));
                }
            }
        }

        #endregion

        #region Core

        public void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
            holoCache = null;
            resetHoloCache = false;
        }
        void Start()
        {
            //legacy targetDatabase
            TargetDatabase = new Dictionary<OrXHoloCache.OrXCoords, List<OrXTargetInfo>>();
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Bop, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Dres, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Duna, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Eeloo, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Eve, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Gilly, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Ike, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Jool, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Kerbin, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Kerbol, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Laythe, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Minmus, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Moho, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Mun, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Pol, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Tylo, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.Vall, new List<OrXTargetInfo>());
            TargetDatabase.Add(OrXHoloCache.OrXCoords.All, new List<OrXTargetInfo>());

            if (HoloCacheTargets == null)
            {
                HoloCacheTargets = new Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>>();
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Bop, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Dres, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Duna, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eeloo, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eve, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Gilly, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Ike, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Jool, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbin, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbol, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Laythe, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Minmus, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Moho, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Mun, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Pol, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Tylo, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Vall, new List<OrXHoloCacheinfo>());
                HoloCacheTargets.Add(OrXHoloCache.OrXCoords.All, new List<OrXHoloCacheinfo>());

            }

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/");

            //StartCoroutine(loadHolo());

            OrXHCGUIEnabled = false;
            AddToolbarButton();
            TargetHCGUI = true;
            spawnHoloCache = false;
            scanning = false;

            if (HighLogic.LoadedSceneIsFlight)
                maySavethisInstance = true;     //otherwise later we should NOT save the current window positions!

            // window position settings
            WindowRectToolbar = new Rect((Screen.width / 16) * 2.5f, 140, toolWindowWidth, toolWindowHeight);
            // Default, if not in file.
            WindowRectHCGUI = new Rect(0, 0, WindowRectToolbar.width - 10, 0);

            WindowRectHCGUI.width = WindowRectToolbar.width - 10;

            //setup gui styles
            centerLabel = new GUIStyle();
            centerLabel.alignment = TextAnchor.UpperCenter;
            centerLabel.normal.textColor = Color.white;

            centerLabelRed = new GUIStyle();
            centerLabelRed.alignment = TextAnchor.UpperCenter;
            centerLabelRed.normal.textColor = Color.red;

            centerLabelOrange = new GUIStyle();
            centerLabelOrange.alignment = TextAnchor.UpperCenter;
            centerLabelOrange.normal.textColor = XKCDColors.BloodOrange;

            centerLabelBlue = new GUIStyle();
            centerLabelBlue.alignment = TextAnchor.UpperCenter;
            centerLabelBlue.normal.textColor = XKCDColors.AquaBlue;

            leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            middleLeftLabel = new GUIStyle(leftLabel);
            middleLeftLabel.alignment = TextAnchor.MiddleLeft;

            middleLeftLabelOrange = new GUIStyle(middleLeftLabel);
            middleLeftLabelOrange.normal.textColor = XKCDColors.BloodOrange;

            targetModeStyle = new GUIStyle();
            targetModeStyle.alignment = TextAnchor.MiddleRight;
            targetModeStyle.fontSize = 9;
            targetModeStyle.normal.textColor = Color.white;

            targetModeStyleSelected = new GUIStyle(targetModeStyle);
            targetModeStyleSelected.normal.textColor = XKCDColors.BloodOrange;

            leftLabelRed = new GUIStyle();
            leftLabelRed.alignment = TextAnchor.UpperLeft;
            leftLabelRed.normal.textColor = Color.red;

            rightLabelRed = new GUIStyle();
            rightLabelRed.alignment = TextAnchor.UpperRight;
            rightLabelRed.normal.textColor = Color.red;

            leftLabelGray = new GUIStyle();
            leftLabelGray.alignment = TextAnchor.UpperLeft;
            leftLabelGray.normal.textColor = Color.gray;

            rippleSliderStyle = new GUIStyle(OrXGUISkin.horizontalSlider);
            rippleThumbStyle = new GUIStyle(OrXGUISkin.horizontalSliderThumb);
            rippleSliderStyle.fixedHeight = rippleThumbStyle.fixedHeight = 0;

            kspTitleLabel = new GUIStyle();
            kspTitleLabel.normal.textColor = OrXGUISkin.window.normal.textColor;
            kspTitleLabel.font = OrXGUISkin.window.font;
            kspTitleLabel.fontSize = OrXGUISkin.window.fontSize;
            kspTitleLabel.fontStyle = OrXGUISkin.window.fontStyle;
            kspTitleLabel.alignment = TextAnchor.UpperCenter;

            redErrorStyle = new GUIStyle(OrXGUISkin.label);
            redErrorStyle.normal.textColor = Color.red;
            redErrorStyle.fontStyle = FontStyle.Bold;
            redErrorStyle.fontSize = 22;
            redErrorStyle.alignment = TextAnchor.UpperCenter;

            redErrorShadowStyle = new GUIStyle(redErrorStyle);
            redErrorShadowStyle.normal.textColor = new Color(0, 0, 0, 0.75f);
            GameEvents.onHideUI.Add(HideGameUI);
            GameEvents.onShowUI.Add(ShowGameUI);
        }
        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!passive && !scanning)
                {
                    passive = true;
                    delay = 300;
                    StartCoroutine(PassiveCheck());
                }
            }
        }
        void OnDestroy()
        {
            GameEvents.onHideUI.Remove(HideGameUI);
            GameEvents.onShowUI.Remove(ShowGameUI);

            HoloCacheTargets = new Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>>();
            TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Bop);
            TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Dres);
            TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Duna);
            TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eeloo);
            TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eve);
            TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Gilly);
            TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Ike);
            TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Jool);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbin);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbol);
            TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Laythe);
            TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Minmus);
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Moho);
            TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Mun);
            TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Pol);
            TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Tylo);
            TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Vall);
            TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target == null);
            TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.All);

        }

        IEnumerator PassiveCheck()
        {
            CheckSOI();
            yield return new WaitForSeconds(delay);
            passive = false;
        }
        public void CheckSOI()
        {
            var SOIcheck = FlightGlobals.ActiveVessel.orbitDriver.orbit.referenceBody;
            if (SOIcheck != SOIcurrent)
            {
                SOIcurrent = FlightGlobals.ActiveVessel.orbitDriver.orbit.referenceBody;

                if (FlightGlobals.ActiveVessel.mainBody.name == "Bop")
                {
                    coords = OrXCoords.Bop;
                    soi = "Bop";
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.mainBody.name == "Dres")
                    {
                        coords = OrXCoords.Dres;
                        soi = "Dres";
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.mainBody.name == "Duna")
                        {
                            soi = "Duna";
                            coords = OrXCoords.Duna;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.mainBody.name == "Eeloo")
                            {
                                soi = "Eeloo";
                                coords = OrXCoords.Eeloo;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.mainBody.name == "Eve")
                                {
                                    soi = "Eve";
                                    coords = OrXCoords.Eve;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Gilly")
                                    {
                                        soi = "Gilly";
                                        coords = OrXCoords.Gilly;
                                    }
                                    else
                                    {
                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Ike")
                                        {
                                            soi = "Ike";
                                            coords = OrXCoords.Ike;
                                        }
                                        else
                                        {
                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Jool")
                                            {
                                                soi = "Jool";
                                                coords = OrXCoords.Jool;
                                            }
                                            else
                                            {
                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbin")
                                                {
                                                    soi = "Kerbin";
                                                    coords = OrXCoords.Kerbin;
                                                }
                                                else
                                                {
                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbol")
                                                    {
                                                        soi = "Kerbol";
                                                        coords = OrXCoords.Kerbol;
                                                    }
                                                    else
                                                    {
                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Laythe")
                                                        {
                                                            soi = "Laythe";
                                                            coords = OrXCoords.Laythe;
                                                        }
                                                        else
                                                        {
                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Minmus")
                                                            {
                                                                soi = "Minmus";
                                                                coords = OrXCoords.Minmus;
                                                            }
                                                            else
                                                            {
                                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Moho")
                                                                {
                                                                    soi = "Moho";
                                                                    coords = OrXCoords.Moho;
                                                                }
                                                                else
                                                                {
                                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Mun")
                                                                    {
                                                                        soi = "Mun";
                                                                        coords = OrXCoords.Mun;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Pol")
                                                                        {
                                                                            soi = "Pol";
                                                                            coords = OrXCoords.Pol;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Tylo")
                                                                            {
                                                                                soi = "Tylo";
                                                                                coords = OrXCoords.Tylo;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Vall")
                                                                                {
                                                                                    soi = "Vall";
                                                                                    coords = OrXCoords.Vall;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        IEnumerator loadHolo()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                LoadHoloCacheTargets();
            }
            else
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(loadHolo());
            }
        }
        public void LoadHoloCacheTargets()
        {
            HoloCacheName = "";

            TargetHCGUI = false;
            scanning = false;
            checking = false;
            worldPos = Vector3d.zero;
            reload = true;

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"))
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/");

            if (HoloCacheTargets == null)
            {
                HoloCacheTargets = new Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>>();
            }
            HoloCacheTargets.Clear();
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Bop, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Dres, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Duna, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eeloo, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Eve, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Gilly, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Ike, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Jool, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbin, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Kerbol, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Laythe, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Minmus, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Moho, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Mun, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Pol, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Tylo, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.Vall, new List<OrXHoloCacheinfo>());
            HoloCacheTargets.Add(OrXHoloCache.OrXCoords.All, new List<OrXHoloCacheinfo>());

            string soi = FlightGlobals.currentMainBody.name;
            string holoCacheLoc = UrlDir.ApplicationRootPath + "GameData/";
            var files = new List<string>(Directory.GetFiles(holoCacheLoc, "*.orx", SearchOption.AllDirectories));
            bool spawned = true;
            bool extras = false;
            int hcCount = 0;

            if (files != null)
            {
                List<string>.Enumerator cfgsToAdd = files.GetEnumerator();
                while (cfgsToAdd.MoveNext())
                {
                    try
                    {
                        ConfigNode fileNode = ConfigNode.Load(cfgsToAdd.Current);

                        if (fileNode != null && fileNode.HasNode("OrX"))
                        {
                            ConfigNode node = fileNode.GetNode("OrX");

                            foreach (ConfigNode spawnCheck in node.nodes)
                            {
                                if (spawned)
                                {
                                    if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                                    {
                                        Debug.Log("[OrX Target Manager] === FOUND HOLOCACHE === " + hcCount); ;
                                        foreach (ConfigNode.Value cv in spawnCheck.values)
                                        {
                                            if (cv.name == "spawned")
                                            {
                                                if (cv.value == "False")
                                                {
                                                    Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has not spawned ... "); ;

                                                    spawned = false;
                                                }
                                                else
                                                {
                                                    Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has spawned ... CHECKING FOR EXTRAS"); ;

                                                    if (spawnCheck.HasValue("extras"))
                                                    {
                                                        var t = spawnCheck.GetValue("extras");
                                                        if (t == "False")
                                                        {
                                                            Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION"); ;
                                                            spawned = false;
                                                            extras = false;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;

                                                            extras = true;
                                                            spawned = true;
                                                            hcCount += 1;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            foreach (ConfigNode HoloCacheNode in node.GetNodes("OrXHoloCacheCoords" + hcCount))
                            {
                                if (HoloCacheNode.HasValue("SOI"))
                                {
                                    if (HoloCacheNode.HasValue("Targets"))
                                    {
                                        string targetString = HoloCacheNode.GetValue("Targets");
                                        if (targetString == string.Empty)
                                        {
                                            Debug.Log("[OrX HoloCache] OrX HoloCache Target string was empty!");
                                            return;
                                        }
                                        StringToHoloCacheList(targetString);
                                        Debug.Log("[OrX HoloCache] Loaded OrX HoloCache Targets");
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX HoloCache] No OrX HoloCache Targets value found!");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[OrX Target Manager] HoloCache Targets Out Of Range ...... Continuing");
                    }
                }
                cfgsToAdd.Dispose();
            }
            else
            {
                Debug.Log("[OrX Target Manager] === HoloCache List is empty ===");
            }
            reload = false;
        }

        private void SaveConfig()
        {
            var h = holoCube.rootPart.FindModuleImplementing<ModuleOrXMission>();
            h.altitude = _alt;
            h.latitude = _lat;
            h.longitude = _lon;
            h.spawned = true;

            string hConfigLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/"
                + HoloCacheName + "/" + HoloCacheName + ".orx";

            mCount = 0;
            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName))
            {
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName);
            }
            _file = ConfigNode.Load(hConfigLoc);
            if (_file == null)
            {
                _file = new ConfigNode();
                _file.AddValue("name", HoloCacheName);
                _file.AddNode("OrX");
                _file.Save("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            }
            _file = ConfigNode.Load(hConfigLoc);
            ConfigNode node = null;
            if (_file != null && _file.HasNode("OrX"))
            {
                node = _file.GetNode("OrX");

                int hcCount = 0;
                mCount = 0;
                ConfigNode HoloCacheNode = null;

                foreach (ConfigNode cn in node.nodes)
                {
                    if (cn.name.Contains("OrXHoloCacheCoords"))
                    {
                        Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " FOUND ===");

                        hcCount += 1;
                    }

                    if (cn.name.Contains("mission"))
                    {

                        mCount += 1;
                        Debug.Log("[OrX Mission] === MISSION " + mCount + " FOUND ===");

                    }
                }

                if (node.HasNode("OrXHoloCacheCoords" + hcCount))
                {
                    Debug.Log("[OrX Mission] === ERROR === HOLOCACHE " + hcCount + " FOUND AGAIN ===");

                    foreach (ConfigNode n in node.GetNodes("OrXHoloCacheCoords" + hcCount))
                    {
                        if (n.GetValue("SOI") == FlightGlobals.ActiveVessel.mainBody.name)
                        {
                            HoloCacheNode = n;
                            break;
                        }
                    }

                    if (HoloCacheNode == null)
                    {
                        HoloCacheNode = node.AddNode("OrXHoloCacheCoords" + hcCount);
                        HoloCacheNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                        HoloCacheNode.AddValue("spawned", "False");
                        HoloCacheNode.AddValue("extras", "False");
                        HoloCacheNode.AddValue("unlocked", "False");
                        HoloCacheNode.AddValue("tech", tech);

                        HoloCacheNode.AddValue("missionName", missionName);
                        HoloCacheNode.AddValue("missionType", missionType);
                        HoloCacheNode.AddValue("challengeType", challengeType);

                        HoloCacheNode.AddValue("missionDescription0", missionDescription0);
                        HoloCacheNode.AddValue("missionDescription1", missionDescription1);
                        HoloCacheNode.AddValue("missionDescription2", missionDescription2);
                        HoloCacheNode.AddValue("missionDescription3", missionDescription3);
                        HoloCacheNode.AddValue("missionDescription4", missionDescription4);
                        HoloCacheNode.AddValue("missionDescription5", missionDescription5);
                        HoloCacheNode.AddValue("missionDescription6", missionDescription6);
                        HoloCacheNode.AddValue("missionDescription7", missionDescription7);
                        HoloCacheNode.AddValue("missionDescription8", missionDescription8);
                        HoloCacheNode.AddValue("missionDescription9", missionDescription9);

                        HoloCacheNode.AddValue("gold", Gold);
                        HoloCacheNode.AddValue("silver", Silver);
                        HoloCacheNode.AddValue("bronze", Bronze);

                        HoloCacheNode.AddValue("completed", completed);
                        HoloCacheNode.AddValue("count", mCount);

                        HoloCacheNode.AddValue("lat", _lat);
                        HoloCacheNode.AddValue("lon", _lon);
                        HoloCacheNode.AddValue("alt", _alt);
                    }
                }
                else
                {
                    Debug.Log("[OrX Mission] === CREATING HOLOCACHE " + hcCount + " ===");

                    HoloCacheNode = node.AddNode("OrXHoloCacheCoords" + hcCount);
                    HoloCacheNode.AddValue("SOI", FlightGlobals.ActiveVessel.mainBody.name);
                    HoloCacheNode.AddValue("spawned", "False");
                    HoloCacheNode.AddValue("extras", "False");
                    HoloCacheNode.AddValue("unlocked", "False");
                    HoloCacheNode.AddValue("tech", tech);

                    HoloCacheNode.AddValue("missionName", missionName);
                    HoloCacheNode.AddValue("missionType", missionType);
                    HoloCacheNode.AddValue("challengeType", challengeType);

                    HoloCacheNode.AddValue("missionDescription0", missionDescription0);
                    HoloCacheNode.AddValue("missionDescription1", missionDescription1);
                    HoloCacheNode.AddValue("missionDescription2", missionDescription2);
                    HoloCacheNode.AddValue("missionDescription3", missionDescription3);
                    HoloCacheNode.AddValue("missionDescription4", missionDescription4);
                    HoloCacheNode.AddValue("missionDescription5", missionDescription5);
                    HoloCacheNode.AddValue("missionDescription6", missionDescription6);
                    HoloCacheNode.AddValue("missionDescription7", missionDescription7);
                    HoloCacheNode.AddValue("missionDescription8", missionDescription8);
                    HoloCacheNode.AddValue("missionDescription9", missionDescription9);

                    HoloCacheNode.AddValue("gold", Gold);
                    HoloCacheNode.AddValue("silver", Silver);
                    HoloCacheNode.AddValue("bronze", Bronze);

                    HoloCacheNode.AddValue("completed", completed);
                    HoloCacheNode.AddValue("count", mCount);

                    HoloCacheNode.AddValue("lat", _lat);
                    HoloCacheNode.AddValue("lon", _lon);
                    HoloCacheNode.AddValue("alt", _alt);
                }
                _file.Save(hConfigLoc);
                string targetString = HoloCacheListToString();
                HoloCacheNode.SetValue("Targets", targetString, true);

                _mission = _file.GetNode("mission" + mCount);
                if (_mission == null)
                {
                    Debug.Log("[OrX Mission] === ADDING NODE 'mission" + mCount + "' ===");

                    _file.AddNode("mission" + mCount);
                    _mission = _file.GetNode("mission" + mCount);
                }
                _file.Save(hConfigLoc);

                _file = ConfigNode.Load(hConfigLoc);
                node = _file.GetNode("OrX");
                Debug.Log("[OrX Mission] === ADDING NODE 'HoloCache" + hcCount + "' ===");
                node.AddNode("HoloCache" + hcCount);
                ConfigNode HCnode = node.GetNode("HoloCache" + hcCount);
                ConfigNode holoFileLoc = ConfigNode.Load(holoToAdd);
                holoFileLoc.CopyTo(HCnode);

                // ADD ENCRYPTION

                foreach (ConfigNode.Value cv in HCnode.values)
                {
                    string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                    string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                    cv.name = cvEncryptedName;
                    cv.value = cvEncryptedValue;
                }

                foreach (ConfigNode cn in HCnode.nodes)
                {
                    foreach (ConfigNode.Value cv in cn.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn2 in cn.nodes)
                    {
                        foreach (ConfigNode.Value cv2 in cn2.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                            cv2.name = cvEncryptedName;
                            cv2.value = cvEncryptedValue;
                        }
                    }
                }

                Debug.Log("[OrX Mission] === HOLO CRAFT ENCRYPTED ===");

                _file.Save(hConfigLoc);
                Debug.Log("[OrX Mission] " + HoloCacheName + " Saved");
                _file = ConfigNode.Load(hConfigLoc);
                node = _file.GetNode("OrX");

                int count = 0;

                if (blueprintsAdded)
                {
                    ConfigNode addedCraft = ConfigNode.Load(blueprintsFile);

                    if (addedCraft != null)
                    {
                        ConfigNode craftData = node.AddNode("HC" + hcCount + "OrXv0");
                        craftData.AddValue("vesselName", craftToAddMission);
                        ConfigNode location = craftData.AddNode("coords");
                        location.AddValue("holo", hcCount);
                        location.AddValue("pas", Password);
                        location.AddValue("lat", FlightGlobals.ActiveVessel.latitude);
                        location.AddValue("lon", FlightGlobals.ActiveVessel.longitude);
                        location.AddValue("alt", FlightGlobals.ActiveVessel.altitude);
                        location.AddValue("heading", 0);
                        location.AddValue("pitch", 0);
                        location.AddValue("rot", "null");
                        location.AddValue("pos", "null");

                        foreach (ConfigNode.Value cv in location.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        ConfigNode craftFile = craftData.AddNode("craft");
                        ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloCacheName + "</b></color>");
                        Debug.Log("[OrX Mission] Saving: " + craftToAddMission);
                        addedCraft.CopyTo(craftFile);

                        foreach (ConfigNode.Value cv in craftFile.values)
                        {
                            if (cv.name == "ship")
                            {
                                cv.value = craftToAddMission;
                                break;
                            }
                        }

                        // ADD ENCRYPTION

                        foreach (ConfigNode.Value cv in craftFile.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        foreach (ConfigNode cn in craftFile.nodes)
                        {
                            foreach (ConfigNode.Value cv in cn.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                cv.name = cvEncryptedName;
                                cv.value = cvEncryptedValue;
                            }

                            foreach (ConfigNode cn2 in cn.nodes)
                            {
                                foreach (ConfigNode.Value cv2 in cn2.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                                    string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                                    cv2.name = cvEncryptedName;
                                    cv2.value = cvEncryptedValue;
                                }
                            }
                        }
                        _file.Save(hConfigLoc);
                        Debug.Log("[OrX Mission] " + craftToAddMission + " Saved to " + HoloCacheName);
                        ScreenMsg("<color=#cfc100ff><b>" + craftToAddMission + " Saved</b></color>");
                    }
                }

                _hcCount = hcCount;
            }
            coordCount = 0;

            if (saveLocalVessels)
            {
                Debug.Log("[OrX Mission] === SAVING LOCAL VESELS ===");

                int count = 0;
                _file = ConfigNode.Load(hConfigLoc);
                node = _file.GetNode("OrX");
                Debug.Log("[OrX Mission] === '" + HoloCacheName + "' found ===");

                List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                while (v.MoveNext())
                {
                    try
                    {
                        if (v.Current == null) continue;
                        if (!v.Current.loaded || v.Current.packed) continue;
                        if (!v.Current != holoCube && !v.Current.rootPart.Modules.Contains<KerbalEVA>())
                        {
                            try
                            {
                                Vector3d vLoc = v.Current.GetWorldPos3D();
                                double targetDistance = Vector3d.Distance(FlightGlobals.ActiveVessel.GetWorldPos3D(), vLoc);

                                if (targetDistance <= 1000) // && v.Current.parts.Count != 1)
                                {
                                    count += 1;

                                    Vessel toSave = v.Current;
                                    Debug.Log("[OrX Mission] Vessel " + v.Current.vesselName + " Identified .......................");
                                    string shipDescription = v.Current.vesselName + " blueprints from " + HoloCacheName;

                                    ShipConstruct ConstructToSave = new ShipConstruct(HoloCacheName, shipDescription, v.Current.parts[0]);
                                    ConfigNode craftConstruct = new ConfigNode("Craft");
                                    craftConstruct = ConstructToSave.SaveShip();
                                    craftConstruct.SetValue("persistentState", "STOWED");
                                    craftConstruct.SetValue("generatorIsActive", "False");
                                    craftConstruct.SetValue("EngineIgnited", "False");
                                    craftConstruct.SetValue("sensorActive", "False");
                                    craftConstruct.SetValue("Staged", "False");
                                    craftConstruct.SetValue("currentThrottle", "0");
                                    craftConstruct.SetValue("throttle", "0");

                                    Quaternion or = v.Current.vesselTransform.rotation;
                                    //Vector3 op = v.Current.vesselTransform.position;
                                    //v.Current.SetRotation(new Quaternion(0, 0, 0, 1));
                                    //Vector3 ShipSize = ShipConstruction.CalculateCraftSize(ConstructToSave);
                                    //v.Current.SetPosition(new Vector3(0, ShipSize.y + 2, 0));
                                    //v.Current.SetRotation(or);
                                    //v.Current.SetPosition(op);
                                    Debug.Log("[OrX Mission] Saving: " + v.Current.vesselName);
                                    ScreenMsg("<color=#cfc100ff><b>Saving: " + v.Current.vesselName + "</b></color>");

                                    ConfigNode craftData = node.AddNode("HC" + _hcCount + "OrXv" + count);
                                    craftData.AddValue("vesselName", v.Current.vesselName);
                                    ConfigNode location = craftData.AddNode("coords");
                                    location.AddValue("holo", _hcCount);
                                    location.AddValue("pas", Password);
                                    location.AddValue("lat", v.Current.latitude);
                                    location.AddValue("lon", v.Current.longitude);
                                    location.AddValue("alt", v.Current.altitude);
                                    location.AddValue("heading", GetHeading());
                                    location.AddValue("rot", KSPUtil.WriteQuaternion(or));

                                    foreach (ConfigNode.Value cv in location.values)
                                    {
                                        string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                        cv.name = cvEncryptedName;
                                        cv.value = cvEncryptedValue;
                                    }

                                    Debug.Log("[OrX Mission] Adding coords ............................. " + HoloCacheName);
                                    ConfigNode craftFile = craftData.AddNode("craft");
                                    ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloCacheName + "</b></color>");
                                    craftConstruct.CopyTo(craftFile);
                                    foreach (ConfigNode.Value cv in craftFile.values)
                                    {
                                        if (cv.name == "ship")
                                        {
                                            cv.value = v.Current.vesselName;
                                            break;
                                        }
                                    }
                                    // ADD ENCRYPTION

                                    foreach (ConfigNode.Value cv in craftFile.values)
                                    {
                                        if (cv.name == "rot")
                                        {
                                            cv.value = KSPUtil.WriteQuaternion(or);
                                        }

                                        string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                        string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                        cv.name = cvEncryptedName;
                                        cv.value = cvEncryptedValue;
                                    }

                                    foreach (ConfigNode cn in craftFile.nodes)
                                    {
                                        foreach (ConfigNode.Value cv in cn.values)
                                        {
                                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                            cv.name = cvEncryptedName;
                                            cv.value = cvEncryptedValue;
                                        }

                                        foreach (ConfigNode cn2 in cn.nodes)
                                        {
                                            foreach (ConfigNode.Value cv2 in cn2.values)
                                            {
                                                string cvEncryptedName = OrXLog.instance.Crypt(cv2.name);
                                                string cvEncryptedValue = OrXLog.instance.Crypt(cv2.value);
                                                cv2.name = cvEncryptedName;
                                                cv2.value = cvEncryptedValue;
                                            }
                                        }
                                    }

                                    saveShip = false;
                                    Debug.Log("[Module OrX HoloCache] " + v.Current.vesselName + " Saved to " + HoloCacheName);
                                    ScreenMsg("<color=#cfc100ff><b>" + v.Current.vesselName + " Saved</b></color>");
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.Log("[Module OrX HoloCache] EXCEPTION ======================== " + HoloCacheName);

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[Module OrX HoloCache] EXCEPTION ======================== " + HoloCacheName);
                    }
                }
                v.Dispose();
                _file.Save(hConfigLoc);
            }

            if (!addCoords)
            {
                building = false;
                buildingMission = false;
                addCoords = false;
                GuiEnabledOrXMissions = false;
                OrXHCGUIEnabled = false;
                PlayOrXMission = false;

                ResetData();
            }
        }
        private void SaveCoords()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            _mission = _file.GetNode("mission" + mCount);

            if (CoordDatabase != null)
            {
                if (CoordDatabase.Count >= 0)
                {
                    Debug.Log("[OrX Mission] ========================== CoordDatabase.Count >= 0");

                    int c = 0;

                    string[] coords = CoordDatabase.ToArray();
                    foreach (string s in coords)
                    {
                        c += 1;
                        _mission.AddValue("stage" + c, s);
                        Debug.Log("[OrX Mission] === stage" + c + " added to " + HoloCacheName + ".orx === " + s);
                    }
                }
            }

            GuiEnabledOrXMissions = false;
            OrXHCGUIEnabled = false;
            OrXLog.instance.building = false;
            building = false;
            buildingMission = false;
            addCoords = false;
            PlayOrXMission = false;
            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            ResetData();
        }
        public string GetHeading()
        {
            Vector3 UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            Vector3 EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            Vector3 NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            heading = Vector3.Angle(FlightGlobals.ActiveVessel.transform.forward, NorthVect);
            if (Math.Sign(Vector3.Dot(FlightGlobals.ActiveVessel.transform.forward, EastVect)) < 0)
            {
                heading = 360 - heading; // westward headings become angles greater than 180
            }

            return heading.ToString();
        }
        private void ResetData()
        {
            nameSB0 = string.Empty;
            timeSB0 = string.Empty;
            nameSB1 = string.Empty;
            timeSB1 = string.Empty;
            nameSB2 = string.Empty;
            timeSB2 = string.Empty;
            nameSB3 = string.Empty;
            timeSB3 = string.Empty;
            nameSB4 = string.Empty;
            timeSB4 = string.Empty;
            nameSB5 = string.Empty;
            timeSB5 = string.Empty;
            nameSB6 = string.Empty;
            timeSB6 = string.Empty;
            nameSB7 = string.Empty;
            timeSB7 = string.Empty;
            nameSB8 = string.Empty;
            timeSB8 = string.Empty;
            nameSB9 = string.Empty;
            timeSB9 = string.Empty;

            _file = null;
            _mission = null;
            _scoreboard_ = null;

            scoreboard0 = null;
            scoreboard1 = null;
            scoreboard2 = null;
            scoreboard3 = null;
            scoreboard4 = null;
            scoreboard5 = null;
            scoreboard6 = null;
            scoreboard7 = null;
            scoreboard8 = null;
            scoreboard9 = null;

            coordCount = 0;

            if (_scoreboard != null)
            {
                _scoreboard.Clear();

            }

            if (stageTimes != null)
            {
                stageTimes.Clear();
            }

            if (CoordDatabase != null)
            {
                CoordDatabase.Clear();
            }

            missionDescription0 = string.Empty;
            missionDescription1 = string.Empty;
            missionDescription2 = string.Empty;
            missionDescription3 = string.Empty;
            missionDescription4 = string.Empty;
            missionDescription5 = string.Empty;
            missionDescription6 = string.Empty;
            missionDescription7 = string.Empty;
            missionDescription8 = string.Empty;
            missionDescription9 = string.Empty;
        }

        private string HoloCacheListToString()
        {
            string finalString = string.Empty;
            string aString = string.Empty;
            aString += FlightGlobals.currentMainBody.name;
            aString += ",";
            aString += HoloCacheName;
            aString += ",";
            aString += Password;
            aString += ",";
            aString += _lat;
            aString += ",";
            aString += _lon;
            aString += ",";
            aString += _alt;
            aString += ";";
            aString += missionName;
            aString += ";";
            aString += missionType;
            aString += ";";
            aString += challengeType;
            aString += ";";

            finalString += aString;
            finalString += ":";

            string bString = string.Empty;
            bString += FlightGlobals.currentMainBody.name;
            bString += ",";
            bString += HoloCacheName;
            bString += ",";
            bString += Password;
            bString += ",";
            bString += _lat;
            bString += ",";
            bString += _lon;
            bString += ",";
            bString += _alt;
            bString += ";";
            bString += missionName;
            bString += ";";
            bString += missionType;
            bString += ";";
            bString += challengeType;
            bString += ";";

            finalString += bString;

            return finalString;
        }
        private void StringToHoloCacheList(string listString)
        {
            if (FlightGlobals.ActiveVessel.mainBody.name == "Bop")
            {
                coords = OrXHoloCache.OrXCoords.Bop;
            }
            else
            {
                if (FlightGlobals.ActiveVessel.mainBody.name == "Dres")
                {
                    coords = OrXHoloCache.OrXCoords.Dres;
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.mainBody.name == "Duna")
                    {
                        coords = OrXHoloCache.OrXCoords.Duna;
                    }
                    else
                    {
                        if (FlightGlobals.ActiveVessel.mainBody.name == "Eeloo")
                        {
                            coords = OrXHoloCache.OrXCoords.Eeloo;
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.mainBody.name == "Eve")
                            {
                                coords = OrXHoloCache.OrXCoords.Eve;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.mainBody.name == "Gilly")
                                {
                                    coords = OrXHoloCache.OrXCoords.Gilly;
                                }
                                else
                                {
                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Ike")
                                    {
                                        coords = OrXHoloCache.OrXCoords.Ike;
                                    }
                                    else
                                    {
                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Jool")
                                        {
                                            coords = OrXHoloCache.OrXCoords.Jool;
                                        }
                                        else
                                        {
                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbin")
                                            {
                                                coords = OrXHoloCache.OrXCoords.Kerbin;
                                            }
                                            else
                                            {
                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Kerbol")
                                                {
                                                    coords = OrXHoloCache.OrXCoords.Kerbol;
                                                }
                                                else
                                                {
                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Laythe")
                                                    {
                                                        coords = OrXHoloCache.OrXCoords.Laythe;
                                                    }
                                                    else
                                                    {
                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Minmus")
                                                        {
                                                            coords = OrXHoloCache.OrXCoords.Minmus;
                                                        }
                                                        else
                                                        {
                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Moho")
                                                            {
                                                                coords = OrXHoloCache.OrXCoords.Moho;
                                                            }
                                                            else
                                                            {
                                                                if (FlightGlobals.ActiveVessel.mainBody.name == "Mun")
                                                                {
                                                                    coords = OrXHoloCache.OrXCoords.Mun;
                                                                }
                                                                else
                                                                {
                                                                    if (FlightGlobals.ActiveVessel.mainBody.name == "Pol")
                                                                    {
                                                                        coords = OrXHoloCache.OrXCoords.Pol;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (FlightGlobals.ActiveVessel.mainBody.name == "Tylo")
                                                                        {
                                                                            coords = OrXHoloCache.OrXCoords.Tylo;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FlightGlobals.ActiveVessel.mainBody.name == "Vall")
                                                                            {
                                                                                coords = OrXHoloCache.OrXCoords.Vall;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (listString == null || listString == string.Empty)
            {
                Debug.Log("[OrX Target Manager] === HoloCache List string was empty or null ===");
                return;
            }

            string[] OrbitalBodyNames = listString.Split(new char[] { ':' });

            Debug.Log("[OrX Target Manager] Loading HoloCache Targets ..........");

            try
            {
                if (OrbitalBodyNames[0] != null && OrbitalBodyNames[0].Length > 0 && OrbitalBodyNames[0] != "null")
                {
                    string[] OrbitalBodyNameACoords = OrbitalBodyNames[0].Split(new char[] { ';' });
                    for (int i = 0; i < OrbitalBodyNameACoords.Length; i++)
                    {
                        if (OrbitalBodyNameACoords[i] != null && OrbitalBodyNameACoords[i].Length > 0)
                        {
                            string[] data = OrbitalBodyNameACoords[i].Split(new char[] { ',' });
                            string name = data[0];
                            craftToSpawn = data[1];
                            _sth = data[1];
                            double lat = double.Parse(data[3]);
                            double longi = double.Parse(data[4]);
                            double alt = double.Parse(data[5]);
                            OrXHoloCacheinfo newInfo = new OrXHoloCacheinfo(new Vector3d(lat, longi, alt), craftToSpawn);
                            HoloCacheTargets[coords].Add(newInfo);
                            HoloCacheTargets[OrXHoloCache.OrXCoords.All].Add(newInfo);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log("[OrX Target Manager] HoloCache config file processed ...... ");
            }
        }

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }
        public static void UseMouseEventInRect(Rect rect)
        {
            if (MouseIsInRect(rect) && Event.current.isMouse && (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp))
            {
                Event.current.Use();
            }
        }
        public static bool MouseIsInRect(Rect rect)
        {
            Vector3 inverseMousePos = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0);
            return rect.Contains(inverseMousePos);
        }

        IEnumerator CleanDatabaseRoutine()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(5);

                TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Bop].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Bop);
                TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Dres].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Dres);
                TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Duna].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Duna);
                TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eeloo);
                TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Eve].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Eve);
                TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Gilly].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Gilly);
                TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Ike].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Ike);
                TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Jool].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Jool);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbin);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Kerbol);
                TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Laythe].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Laythe);
                TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Minmus].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Minmus);
                TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Moho].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Moho);
                TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Mun].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Mun);
                TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Pol].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Pol);
                TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Tylo].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Tylo);
                TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.Vall].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.Vall);
                TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target == null);
                TargetDatabase[OrXHoloCache.OrXCoords.All].RemoveAll(target => target.OrbitalBodyName == OrXHoloCache.OrXCoords.All);

            }
        }
        public void RemoveTarget(OrXTargetInfo target, OrXHoloCache.OrXCoords OrbitalBodyName)
        {
            TargetDatabase[OrbitalBodyName].Remove(target);

        }
        public static void AddTarget(OrXTargetInfo target)
        {
            if (FlightGlobals.currentMainBody.name == "Bop")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Bop].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Dres")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Dres].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Duna")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Duna].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Eeloo")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Eve")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Eve].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Gilly")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Gilly].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Ike")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Ike].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Jool")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Jool].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Kerbin")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Kerbol")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Laythe")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Laythe].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Minmus")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Minmus].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Moho")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Moho].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Mun")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Mun].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Pol")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Pol].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Tylo")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Tylo].Add(target);
            }
            if (FlightGlobals.currentMainBody.name == "Vall")
            {
                TargetDatabase[OrXHoloCache.OrXCoords.Vall].Add(target);
            }

            TargetDatabase[OrXHoloCache.OrXCoords.All].Add(target);

        }
        public void ClearDatabase()
        {
            foreach (OrXHoloCache.OrXCoords t in TargetDatabase.Keys)
            {
                foreach (OrXTargetInfo target in TargetDatabase[t])
                {
                    target.detectedTime = 0;
                }
            }

            TargetDatabase[OrXHoloCache.OrXCoords.Bop].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Dres].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Duna].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Eeloo].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Eve].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Gilly].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Ike].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Jool].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbin].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Kerbol].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Laythe].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Minmus].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Moho].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Mun].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Pol].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Tylo].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.Vall].Clear();
            TargetDatabase[OrXHoloCache.OrXCoords.All].Clear();

        }

        public static Vector3d WorldPositionToGeoCoords(Vector3d worldPosition, CelestialBody body)
        {
            if (!body)
            {
                return Vector3d.zero;
            }
            double lat = body.GetLatitude(worldPosition);
            double longi = body.GetLongitude(worldPosition);
            double alt = body.GetAltitude(worldPosition);
            Debug.Log("[Spawn OrX HoloCache] Lat: " + lat + " - Lon:" + longi + " - Alt: " + alt);
            return new Vector3d(lat, longi, alt);
        }

        #endregion

        #region Missions

        public void SetupHolo(Vessel v)
        {
            ResetData();
            holoCube = v;
            holoToAdd = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloCache/HoloCache.craft";
            missionType = "GEO-CACHE";
            challengeType = "OUTLAW RACING";
            geoCache = true;
            holocacheCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloCache/";
            holocacheFiles = new List<string>(Directory.GetFiles(holocacheCraftLoc, "*.craft", SearchOption.AllDirectories));

            sphLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/SPH/";
            sphFiles = new List<string>(Directory.GetFiles(sphLoc, "*.craft", SearchOption.AllDirectories));

            vabLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/VAB/";
            vabFiles = new List<string>(Directory.GetFiles(vabLoc, "*.craft", SearchOption.AllDirectories));
            OrXLog.instance.building = true;
            building = true;
            buildingMission = true;
            _lat = FlightGlobals.ActiveVessel.latitude;
            _lon = FlightGlobals.ActiveVessel.longitude;
            _alt = FlightGlobals.ActiveVessel.altitude;
            id = FlightGlobals.ActiveVessel.id;
            startLocation = new Vector3d(_lat, _lon, _alt);
            lastCoord = startLocation;
            lat = _lat;
            lon = _lon;
            alt = _alt;
            showScores = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = false;
            craftBrowserOpen = false;
            spawned = true;
            OrXHCGUIEnabled = true;
        }
        public void OpenHoloCache(string holoName)
        {
            challengeRunning = true;
            StartCoroutine(StartMission(holoName));
        }
        public IEnumerator StartMission(string holoName)
        {
            Debug.Log("[OrX Mission] === STARTING MISSION === "); ;

            ResetData();
            HoloCacheName = holoName;


            OrXLog.instance.mission = true;
            building = false;
            coordCount = 0;
            _scoreboard = new List<string>();
            stageTimes = new List<string>();
            CoordDatabase = new List<string>();
            int hcCount = 0;

            if (missionType != "GEO-CACHE")
            {
                geoCache = false;
                showScores = true;
            }
            else
            {
                showScores = false;
                geoCache = true;
            }

            if (HoloCacheName != "")
            {
                ec = 0;
                _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                ConfigNode node = _file.GetNode("OrX");

                foreach (ConfigNode spawnCheck in node.nodes)
                {
                    if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                    {
                        Debug.Log("[OrX Mission] === FOUND HOLOCACHE === " + hcCount); ;

                        ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + hcCount);

                        foreach (ConfigNode.Value cv in HoloCacheNode.values)
                        {
                            if (cv.name == "spawned")
                            {
                                if (cv.value == "False")
                                {
                                    Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has not spawned ... "); ;

                                    foreach (ConfigNode.Value data in HoloCacheNode.values)
                                    {
                                        if (data.name == "missionName")
                                        {
                                            missionName = data.value;
                                        }

                                        if (data.name == "missionType")
                                        {
                                            missionType = data.value;
                                        }

                                        if (data.name == "challengeType")
                                        {
                                            challengeType = data.value;
                                        }

                                        if (data.name == "missionDescription0")
                                        {
                                            missionDescription0 = data.value;
                                        }

                                        if (data.name == "missionDescription1")
                                        {
                                            missionDescription1 = data.value;
                                        }

                                        if (data.name == "missionDescription2")
                                        {
                                            missionDescription2 = data.value;
                                        }

                                        if (data.name == "missionDescription3")
                                        {
                                            missionDescription3 = data.value;
                                        }

                                        if (data.name == "missionDescription4")
                                        {
                                            missionDescription4 = data.value;
                                        }
                                        if (data.name == "missionDescription5")
                                        {
                                            missionDescription5 = data.value;
                                        }
                                        if (data.name == "missionDescription6")
                                        {
                                            missionDescription6 = data.value;
                                        }
                                        if (data.name == "missionDescription7")
                                        {
                                            missionDescription7 = data.value;
                                        }

                                        if (data.name == "missionDescription8")
                                        {
                                            missionDescription8 = data.value;
                                        }
                                        if (data.name == "missionDescription9")
                                        {
                                            missionDescription9 = data.value;
                                        }

                                        if (data.name == "Gold")
                                        {
                                            Gold = data.value;
                                        }
                                        if (data.name == "Silver")
                                        {
                                            Silver = data.value;
                                        }
                                        if (data.name == "Bronze")
                                        {
                                            Bronze = data.value;
                                        }
                                        if (data.name == "mCount")
                                        {
                                            mCount = int.Parse(data.value);
                                        }

                                    }
                                }
                                else
                                {
                                    Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has spawned ... CHECKING FOR EXTRAS"); ;

                                    if (HoloCacheNode.HasValue("extras"))
                                    {
                                        var t = HoloCacheNode.GetValue("extras");
                                        if (t == "False")
                                        {
                                            Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION"); ;
                                            break;
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Mission] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;
                                            hcCount += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string crafttosave = string.Empty;
                int c = 0;

                foreach (ConfigNode craftNode in node.nodes)
                {
                    if (craftNode.name.Contains("HC" + hcCount + "OrXv"))
                    {
                        rot = new Quaternion();

                        if (craftNode.name.Contains("HC" + hcCount + "OrXv0"))
                        {
                            ConfigNode local = craftNode.GetNode("HC" + hcCount + "OrXv0");
                            foreach (ConfigNode.Value cv in local.values)
                            {
                                if (cv.name == "vesselName")
                                {
                                    Debug.Log("[OrX Mission] === Blueprints found for '" + cv.value + "' ===");

                                    crafttosave = cv.value;
                                }
                            }

                            ConfigNode location = local.GetNode("coords");
                            foreach (ConfigNode.Value loc in location.values)
                            {
                                string locEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                if (locEncryptedName == "holo")
                                {
                                    string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);

                                    if (locEncryptedValue == hcCount.ToString())
                                    {
                                        foreach (ConfigNode.Value _loc in location.values)
                                        {
                                            if (locEncryptedName == "pas")
                                            {
                                                pas = OrXLog.instance.Decrypt(_loc.value);
                                            }
                                        }
                                    }
                                }
                            }

                            ConfigNode craftFile = local.GetNode("craft");
                            foreach (ConfigNode.Value cv in craftFile.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                cv.name = cvEncryptedName;
                                cv.value = cvEncryptedValue;
                            }
                            foreach (ConfigNode cn in craftFile.nodes)
                            {
                                foreach (ConfigNode.Value cv in cn.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                    cv.name = cvEncryptedName;
                                    cv.value = cvEncryptedValue;
                                }

                                foreach (ConfigNode cn2 in cn.nodes)
                                {
                                    foreach (ConfigNode.Value cv2 in cn2.values)
                                    {
                                        string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                        cv2.name = cvEncryptedName;
                                        cv2.value = cvEncryptedValue;
                                    }
                                }
                            }

                            string _type = "";

                            foreach (ConfigNode.Value value in craftFile.values)
                            {
                                if (value.name == "type")
                                {
                                    if (value.value == "SPH")
                                    {
                                        _type = "SPH/";
                                    }
                                    if (value.value == "VAB")
                                    {
                                        _type = "VAB/";
                                    }
                                }
                            }

                            _blueprints_ = craftFile;
                            blueprintsFile = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder
                                + "/Ships/" + _type + crafttosave + ".craft";

                            ConfigNode HoloCacheNode = node.GetNode("OrXHoloCacheCoords" + hcCount);
                            foreach (ConfigNode.Value cv in HoloCacheNode.values)
                            {
                                string a = OrXLog.instance.Decrypt(cv.name);

                                if (a == "tech")
                                {
                                    string b = OrXLog.instance.Decrypt(cv.value);
                                    if (b != "")
                                    {
                                        techToAdd = b;

                                        if (OrXLog.instance.CheckTechList(techToAdd))
                                        {
                                            Debug.Log("[OrX Mission] " + HoloCacheName + " is adding " + techToAdd + " to the tech list ...");
                                            OrXLog.instance.AddTech(techToAdd);
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Mission] " + techToAdd + " is already in the tech list ...");
                                        }
                                    }
                                }
                                if (a == "spawned")
                                {
                                    cv.value = OrXLog.instance.Crypt("True");
                                }
                            }

                            _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                            Debug.Log("[OrX Mission] " + HoloCacheName + " Saved Status - SPAWNED");
                        }
                        else
                        {
                            ConfigNode location = craftNode.GetNode("coords");
                            foreach (ConfigNode.Value loc in location.values)
                            {
                                string locEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                if (locEncryptedName == "lat")
                                {
                                    string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                    la = double.Parse(locEncryptedValue);
                                }
                                if (locEncryptedName == "lon")
                                {
                                    string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                    lo = double.Parse(locEncryptedValue);
                                }
                                if (locEncryptedName == "alt")
                                {
                                    string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                    al = double.Parse(locEncryptedValue);
                                }
                                if (locEncryptedName == "heading")
                                {
                                    string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                    heading = float.Parse(locEncryptedValue);
                                }

                                if (locEncryptedName == "rot")
                                {
                                    string locEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                    rot = KSPUtil.ParseQuaternion(locEncryptedValue);
                                }
                            }

                            Vector3 sl = new Vector3d(la, lo, al);

                            ConfigNode craftFile = craftNode.GetNode("craft");
                            foreach (ConfigNode.Value cv in craftFile.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                cv.name = cvEncryptedName;
                                cv.value = cvEncryptedValue;
                            }
                            foreach (ConfigNode cn in craftFile.nodes)
                            {
                                foreach (ConfigNode.Value cv in cn.values)
                                {
                                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                    string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                    cv.name = cvEncryptedName;
                                    cv.value = cvEncryptedValue;
                                }

                                foreach (ConfigNode cn2 in cn.nodes)
                                {
                                    foreach (ConfigNode.Value cv2 in cn2.values)
                                    {
                                        string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                        cv2.name = cvEncryptedName;
                                        cv2.value = cvEncryptedValue;
                                    }
                                }
                            }

                            string _type = "";

                            foreach (ConfigNode.Value value in craftFile.values)
                            {
                                if (value.name == "type")
                                {
                                    if (value.value == "SPH")
                                    {
                                        _type = "SPH/";
                                    }
                                    if (value.value == "VAB")
                                    {
                                        _type = "VAB/";
                                    }
                                }
                            }

                            craftFile.SetValue("rot", KSPUtil.WriteQuaternion(rot), true);

                            craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");

                            SpawnMissionCraft(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/"
                                + "spawn.tmp", sl, false, rot, heading);

                            /// WAIT FOR 4 FRAMES FOR SPAWN TO RESET - COULD PROBABLY BE LESS DEPENDING ON PARTS COUNT OF CRAFT
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();
                            yield return new WaitForEndOfFrame();

                        }
                    }
                }

                _mission = _file.GetNode("mission" + mCount);
                if (_mission != null)
                {
                    foreach (ConfigNode.Value cv in _mission.values)
                    {
                        if (cv.name.Contains("stage"))
                        {
                            CoordDatabase.Add(cv.value);
                            coordCount += 1;
                        }
                    }

                    if (CoordDatabase.Count >= 0)
                    {
                        List<string>.Enumerator firstCoords = CoordDatabase.GetEnumerator();
                        while (firstCoords.MoveNext())
                        {
                            try
                            {
                                string[] data = firstCoords.Current.Split(new char[] { ',' });
                                if (data[0] == "1")
                                {
                                    gpsCount = 1;
                                    NextCoord = firstCoords.Current;
                                    latMission = double.Parse(data[1]);
                                    lonMission = double.Parse(data[2]);
                                    altMission = double.Parse(data[3]);
                                    nextLocation = new Vector3d(double.Parse(data[1]), double.Parse(data[1]), double.Parse(data[1]));
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Debug.Log("[OrX Mission] HoloCache config file processed ...... ");
                            }
                        }
                        firstCoords.Dispose();

                        if (_scoreboard_.nodes.Contains("scoreboard0"))
                        {
                            // DO NOTHING
                        }
                        else  // ADD NEW PODIUM LIST
                        {
                            _scoreboard_.AddNode("scoreboard0");
                            _scoreboard_.AddNode("scoreboard1");
                            _scoreboard_.AddNode("scoreboard2");
                            _scoreboard_.AddNode("scoreboard3");
                            _scoreboard_.AddNode("scoreboard4");
                            _scoreboard_.AddNode("scoreboard5");
                            _scoreboard_.AddNode("scoreboard6");
                            _scoreboard_.AddNode("scoreboard7");
                            _scoreboard_.AddNode("scoreboard8");
                            _scoreboard_.AddNode("scoreboard9");

                            scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                            scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                            scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                            scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                            scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                            scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                            scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                            scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                            scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                            scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                            scoreboard0.AddValue("name", "<empty>");
                            scoreboard0.AddValue("time", "");
                            scoreboard1.AddValue("name", "<empty>");
                            scoreboard1.AddValue("time", "");
                            scoreboard2.AddValue("name", "<empty>");
                            scoreboard2.AddValue("time", "");
                            scoreboard3.AddValue("name", "<empty>");
                            scoreboard3.AddValue("time", "");
                            scoreboard4.AddValue("name", "<empty>");
                            scoreboard4.AddValue("time", "");
                            scoreboard5.AddValue("name", "<empty>");
                            scoreboard5.AddValue("time", "");
                            scoreboard6.AddValue("name", "<empty>");
                            scoreboard6.AddValue("time", "");
                            scoreboard7.AddValue("name", "<empty>");
                            scoreboard7.AddValue("time", "");
                            scoreboard8.AddValue("name", "<empty>");
                            scoreboard8.AddValue("time", "");
                            scoreboard9.AddValue("name", "<empty>");
                            scoreboard9.AddValue("time", "");
                        }

                        // CHECK PODIUM LIST

                        scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                        scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                        scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                        scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                        scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                        scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                        scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                        scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                        scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                        scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                        foreach (ConfigNode.Value cv in scoreboard0.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB0 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB0 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard1.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB1 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB1 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard2.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB2 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB2 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard3.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB3 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB3 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard4.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB4 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB4 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard5.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB5 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB5 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard6.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB6 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB6 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard7.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB7 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB7 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard8.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB8 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB8 = cv.value;
                            }
                        }

                        foreach (ConfigNode.Value cv in scoreboard9.values)
                        {
                            if (cv.name == "name")
                            {
                                nameSB9 = cv.value;
                            }

                            if (cv.name == "time")
                            {
                                timeSB9 = cv.value;
                            }
                        }

                    }
                }
            }

            var mom = holoCube.rootPart.FindModuleImplementing<ModuleOrXMission>();
            mom.completed = false;
            mom.missionName = missionName;
            mom.missionType = missionType;
            mom.challengeType = challengeType;
            mom.tech = tech;
            mom.mCount = mCount;
            mom.Gold = Gold;
            mom.Silver = Silver;
            mom.Bronze = Bronze;
            mom.blueprintsAdded = blueprintsAdded;
            OrXHCGUIEnabled = true;
            showScores = false;
            GuiEnabledOrXMissions = true;
            PlayOrXMission = true;
            craftBrowserOpen = false;
        }
        private void StartChallenge()
        {
            if (geoCache)
            {
                PlayOrXMission = false;
                showScores = false;
                _blueprints_.Save(blueprintsFile);
                ScreenMsg("'" + craftToAddMission + "' Blueprints Available");
                Debug.Log("[OrX Holocache] === '" + craftToAddMission + "' Blueprints Available ===");

                // ????????? ADD NAME OF PLAYER FOR POSTERITY ?????????
            }
            else
            {
                missionTime = FlightGlobals.ActiveVessel.missionTime;

                if (challengeType == "WIND RACING")
                {
                    // SETUP WIND CONTROLLER
                    TargetDistanceMission();
                    GuiEnabledOrXMissions = false;
                    PlayOrXMission = false;

                    showScores = false;

                }
                else
                {
                    if (challengeType == "SCUBA CHALLENGE")
                    {
                        // START A DEPTH MONITORING MONOBEHAVIOUR/GIU
                        // PRESSURE SENSOR/SLIDER 

                        // TO UNLOCK SCUBA KERB GO TO DIVE INTO MY HOLE LOCATION - ADD REASEARCH OUTPOST AND SPAWNED KERBAL FOR DIALOGUE
                        // BUCKEY BALLS AND LOTION - TAKE A SHOWER, PUT ON LOTION AND JUMP INTO THE HOLE OF MUD THEN START TUTORIAL

                        TargetDistanceMission();
                        GuiEnabledOrXMissions = false;
                        PlayOrXMission = false;
                        showScores = false;
                    }
                    else
                    {
                        if (challengeType == "OUTLAW RACING")
                        {
                            // START OUTLAW RACING GUI
                            // ADD DAKAR, DRAG, SHORT AND LONG TRACK TYPES
                            // INCORPORATE PULL N DRAG ????????
                            // REARVIEW MIRRORS FOR FIRST PERSON VIEW ??????
                            // INCLUDE BILL'S BIG BAD BEAVER ETC ??????

                            TargetDistanceMission();
                            GuiEnabledOrXMissions = false;
                            PlayOrXMission = false;

                            showScores = false;

                        }
                    }
                }
            }
        }
        public void StartEndGame()
        {
            SaveScore();
        }

        #endregion

        #region Target Location Spawning

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
        public static string FormattedGeoPosShort(Vector3d geoPos, bool altitude)
        {
            string finalString = string.Empty;
            //lat
            double lat = geoPos.x;
            double latSign = Math.Sign(lat);
            double latMajor = latSign * Math.Floor(Math.Abs(lat));
            double latMinor = 100 * (Math.Abs(lat) - Math.Abs(latMajor));
            string latString = latMajor.ToString("0") + " " + latMinor.ToString("0");
            finalString += "N:" + latString;


            //longi
            double longi = geoPos.y;
            double longiSign = Math.Sign(longi);
            double longiMajor = longiSign * Math.Floor(Math.Abs(longi));
            double longiMinor = 100 * (Math.Abs(longi) - Math.Abs(longiMajor));
            string longiString = longiMajor.ToString("0") + " " + longiMinor.ToString("0");
            finalString += " E:" + longiString;

            if (altitude)
            {
                finalString += " ASL:" + geoPos.z.ToString("0");
            }

            return finalString;
        }
        public static void DrawTextureOnWorldPos(Vector3 loc, Texture texture, Vector2 size)
        {
            Vector3 screenPos = GetMainCamera().WorldToViewportPoint(loc);
            if (screenPos.z < 0) return; //dont draw if point is behind camera
            if (screenPos.x != Mathf.Clamp01(screenPos.x)) return; //dont draw if off screen
            if (screenPos.y != Mathf.Clamp01(screenPos.y)) return;
            float xPos = screenPos.x * Screen.width - (0.5f * size.x);
            float yPos = (1 - screenPos.y) * Screen.height - (0.5f * size.y);
            Rect iconRect = new Rect(xPos, yPos, size.x, size.y);
            GUI.DrawTexture(iconRect, texture);
        }
        private void TargetDistance()
        {
            checking = true;
            Debug.Log("[OrX Holocache] === TargetDistance() - checking = true; ===");
            SpawnCoords = new Vector3d(lat, lon, alt);

            if (scanning)
            {
                Debug.Log("[OrX Holocache] === StartCoroutine(CheckTargetDistance()); ===");

                StartCoroutine(CheckTargetDistance());
            }
        }
        IEnumerator CheckTargetDistance()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (checking)
                {
                    reloadWorldPos = true;
                    targetDistance = Vector3d.Distance(new Vector3d(FlightGlobals.ActiveVessel.latitude, 
                        FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude), SpawnCoords);

                    if (targetDistance <= minLoadRange)
                    {
                        if (!holoSpawned)
                        {
                            holoSpawned = true;
                            Debug.Log("[OrX Holocache] === StartCoroutine(HoloSpawn()); ===");

                            StartCoroutine(HoloSpawn());
                        }
                        else
                        {
                            if (targetDistance <= 100)
                            {
                                Debug.Log("[OrX Holocache] === targetDistance <= 500 ===");

                                scanning = false;
                                checking = false;
                                worldPos = Vector3d.zero;
                                HideGameUI();
                            }
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(timer);
                        StartCoroutine(CheckTargetDistance());
                    }
                }
            }
        }
        private void TargetDistanceMission()
        {
            checkingMission = true;
            StartCoroutine(CheckTargetDistanceMission());
        }
        IEnumerator CheckTargetDistanceMission()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (checkingMission)
                {
                    targetDistanceMission = Vector3d.Distance(FlightGlobals.ActiveVessel.GetWorldPos3D(), nextLocation);
                    _targetDistance = String.Format("{0:0.00}", targetDistanceMission);
                    float d = Convert.ToSingle(targetDistanceMission / 100);

                    if (targetDistanceMission <= 10)
                    {
                        checkingMission = false;
                        GetNextCoord();
                    }
                    else
                    {
                        yield return new WaitForSeconds(d);
                        StartCoroutine(CheckTargetDistanceMission());
                    }
                }
            }
        }
        IEnumerator HoloSpawn()
        {
            if (!spawnHoloCache)
            {
                spawnHoloCache = true;
                yield return new WaitForFixedUpdate();
                ConfigNode node = null;
                ConfigNode fileNode = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                Debug.Log("[OrX Holocache] === Data Loaded ===");
                bool spawned = true;
                int hcCount = 0;
                Debug.Log("[OrX Holocache] === Checking if HoloCache #" + hcCount + " has already spawned ===");
                bool empty = false;

                if (fileNode != null && fileNode.HasNode("OrX"))
                {
                    node = fileNode.GetNode("OrX");

                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (spawned)
                        {
                            if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                            {
                                Debug.Log("[OrX Target Manager] === FOUND HOLOCACHE === " + hcCount); ;
                                foreach (ConfigNode.Value cv in spawnCheck.values)
                                {
                                    if (cv.name == "spawned")
                                    {
                                        if (cv.value == "False")
                                        {
                                            Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has not spawned ... SPAWNING"); ;
                                            cv.value = "True";
                                            fileNode.Save("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                                            spawned = false;
                                            break;
                                        }
                                        else
                                        {
                                            Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has spawned ... CHECKING FOR EXTRAS"); ;

                                            if (spawnCheck.HasValue("extras"))
                                            {
                                                var t = spawnCheck.GetValue("extras");
                                                if (t == "False")
                                                {
                                                    Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION"); ;
                                                    spawned = false;
                                                    hcCount += 1;
                                                    break;
                                                }
                                                else
                                                {
                                                    Debug.Log("[OrX Target Manager] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;

                                                    spawned = true;
                                                    hcCount += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("[OrX Holocache] === HoloCache List is empty ===");
                    empty = true;
                }

                Debug.Log("[OrX Holocache] === SEARCHING FOR HOLOCACHE #" + hcCount + " ===");

                fileNode = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                node = fileNode.GetNode("OrX");

                ConfigNode hc = node.GetNode("HoloCache" + hcCount);

                if (hc == null || empty)
                {
                    Debug.Log("[OrX Holocache] === ERROR === HoloCache #" + hcCount + " doesn't exist ===");
                }
                else
                {
                    Debug.Log("[OrX Holocache] === FOUND HOLOCACHE #" + hcCount + " ... LOADING ===");

                    foreach (ConfigNode.Value cv in hc.values)
                    {
                        string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                        cv.name = cvEncryptedName;
                        cv.value = cvEncryptedValue;
                    }

                    foreach (ConfigNode cn in hc.nodes)
                    {
                        foreach (ConfigNode.Value cv in cn.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        foreach (ConfigNode cn2 in cn.nodes)
                        {
                            foreach (ConfigNode.Value cv2 in cn2.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                cv2.name = cvEncryptedName;
                                cv2.value = cvEncryptedValue;
                            }
                        }
                    }

                    hc.Save("GameData/OrX/Holocache/" + HoloCacheName + "/" + HoloCacheName + ".craft");
                    yield return new WaitForFixedUpdate();

                    holo = true;
                    craftFile = HoloCacheName;
                    _lat = lat;
                    _lon = lon;
                    _alt = alt;
                    CheckSpawnTimer();
                    spawnHoloCache = false;
                }
            }
        }

        #endregion

        #region GUI

        void OnGUI()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (PauseMenu.isOpen)
                {
                    paused = true;
                    OrXHCGUIEnabled = false;
                }
                else
                {
                    paused = false;
                }

                if (checkingMission)
                {
                    DrawTextureOnWorldPos(nextLocation, instance.HoloTargetTexture, new Vector2(8, 8));
                }

                if (TargetHCGUI && scanning)
                {
                    DrawTextureOnWorldPos(SpawnCoords, instance.HoloTargetTexture, new Vector2(8, 8));
                }

                if (OrXHCGUIEnabled)
                {
                    WindowRectToolbar = GUI.Window(265227765, WindowRectToolbar, OrXHCGUI, "", OrXGUISkin.window);
                    UseMouseEventInRect(WindowRectToolbar);
                }
            }
            else
            {
                if (!OrXHCGUIEnabled) return;
                WindowRectToolbar = GUI.Window(266222695, WindowRectToolbar, OrXHCGUI, "HoloCache Locations", OrXGUISkin.window);
                UseMouseEventInRect(WindowRectToolbar);
            }
        }
        private void AddToolbarButton()
        {
            string OrXDir = "OrX/Plugin/";

            if (!hasAddedButton)
            {
                Texture buttonTexture = GameDatabase.Instance.GetTexture(OrXDir + "OrX_HoloCache", false); //texture to use for the button
                ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, Dummy, Dummy, Dummy, Dummy,
                    ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.VAB |
                    ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH |
                    ApplicationLauncher.AppScenes.TRACKSTATION, buttonTexture);
                hasAddedButton = true;
            }
        }

        public void HideGameUI()
        {
            OrXHCGUIEnabled = false;
        }
        void ShowGameUI()
        {
            if (!scanning && !buildingMission)
            {
                ClearDatabase();
                LoadHoloCacheTargets();
            }

            if (HighLogic.LoadedSceneIsFlight)
            {
                vid = FlightGlobals.ActiveVessel.id;
            }
            OrXHCGUIEnabled = true;
            //LoadHoloCacheTargets();
        }
        private void EnableGui()
        {
            craftBrowserOpen = true;
            Debug.Log("[OrX Mission]: Showing OrXMissions GUI");
        }
        private void DisableGui()
        {
            ResetData();
            PlayOrXMission = false;
            GuiEnabledOrXMissions = false;
            Debug.Log("[OrX Mission]: Hiding OrXMissions GUI");
        }
        private void ToggleGUI()
        {
            if (OrXHCGUIEnabled)
            {
                if (!paused)
                {
                    pauseCheck = false;
                    HideGameUI();
                }
                else
                {
                    pauseCheck = true;
                    HideGameUI();
                }

            }
            else
            {
                if (!pauseCheck)
                {
                    paused = false;
                    ShowGameUI();
                }
                else
                {
                    pauseCheck = false;
                    paused = false;
                    //HideGameUI();
                }
            }
        }

        private void OrXHCGUI(int OrX_HCGUI)
        {
            float line = 0;
            float leftIndent = 10;
            float contentWidth = toolWindowWidth - leftIndent;
            float contentTop = 10;
            float entryHeight = 20;
            float HCGUILines = 0;

            if (GuiEnabledOrXMissions)
            {
                GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));

                if (PlayOrXMission)
                {
                    DrawPlayHoloCacheName(line);
                    line++;
                    DrawPlayTitle(line);
                    line++;
                    DrawPlayMissionType(line);
                    line++;
                    if (!geoCache)
                    {
                        DrawPlayRaceType(line);
                        line++;
                    }
                    if (blueprintsAdded)
                    {
                        DrawPlayBlueprintsAdded(line);
                        line++;
                    }
                    line++;

                    if (missionDescription0 != "" && missionDescription0 != string.Empty)
                    {
                        DrawDescription0(line);
                        line++;
                        if (missionDescription1 != "" && missionDescription1 != string.Empty)
                        {
                            DrawDescription1(line);
                            line++;

                            if (missionDescription2 != "" && missionDescription2 != string.Empty)
                            {
                                DrawDescription2(line);
                                line++;

                                if (missionDescription3 != "" && missionDescription3 != string.Empty)
                                {
                                    DrawDescription3(line);
                                    line++;

                                    if (missionDescription4 != "" && missionDescription4 != string.Empty)
                                    {
                                        DrawDescription4(line);
                                        line++;

                                        if (missionDescription5 != "" && missionDescription5 != string.Empty)
                                        {
                                            DrawDescription5(line);
                                            line++;

                                            if (missionDescription6 != "" && missionDescription6 != string.Empty)
                                            {
                                                DrawDescription6(line);
                                                line++;

                                                if (missionDescription7 != "" && missionDescription7 != string.Empty)
                                                {
                                                    DrawDescription7(line);
                                                    line++;

                                                    if (missionDescription8 != "" && missionDescription8 != string.Empty)
                                                    {
                                                        DrawDescription8(line);
                                                        line++;

                                                        if (missionDescription9 != "" && missionDescription9 != string.Empty)
                                                        {
                                                            DrawDescription9(line);
                                                            line++;

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!unlocked)
                    {
                        DrawPlayPassword(line++);
                        line++;
                        DrawUnlock(line);
                    }
                    else
                    {
                        if (!geoCache)
                        {
                            DrawShowScoreboard(line);
                            line++;
                            DrawChallengerName(line);
                            line++;
                            DrawStart(line);

                        }
                    }
                    line++;
                    DrawCancel(line);

                    _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
                    _windowRect.height = _windowHeight;

                }
                else
                {
                    if (showScores)
                    {
                        DrawScoreboard(line);
                        line++;
                        line++;
                        DrawScoreboard0(line);
                        line++;
                        DrawScoreboard1(line);
                        line++;
                        DrawScoreboard2(line);
                        line++;
                        DrawScoreboard3(line);
                        line++;
                        DrawScoreboard4(line);
                        line++;
                        DrawScoreboard5(line);
                        line++;
                        DrawScoreboard6(line);
                        line++;
                        DrawScoreboard7(line);
                        line++;
                        DrawScoreboard8(line);
                        line++;
                        DrawScoreboard9(line);
                        line++;
                        line++;
                        DrawCloseScoreboard(line);

                        _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
                        _windowRect.height = _windowHeight;

                    }
                    else
                    {
                        if (craftBrowserOpen)
                        {
                            DrawCraftBrowserTitle(line);
                            line++;
                            DrawHangar(line);
                            line++;
                            if (HighLogic.LoadedSceneIsEditor)
                            {
                                if (sph)
                                {
                                    int c = 0;
                                    List<string>.Enumerator hcFile = sphFiles.GetEnumerator();
                                    while (hcFile.MoveNext())
                                    {
                                        try
                                        {
                                            ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                            string vn = "";

                                            foreach (ConfigNode.Value cv in craft.values)
                                            {
                                                if (cv.name == "ship")
                                                {
                                                    vn = cv.value;
                                                }
                                            }

                                            if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight), vn, HighLogic.Skin.button))
                                            {
                                                Debug.Log("[OrX Mission] === ADDING BLUPRINTS === SPH");

                                                blueprintsFile = hcFile.Current;
                                                craftToAddMission = vn;
                                                craftBrowserOpen = false;
                                                addingBluePrints = false;
                                                blueprintsAdded = true;
                                            }
                                            line++;
                                            c += 1;
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    hcFile.Dispose();
                                }
                                else
                                {
                                    int c = 0;
                                    List<string>.Enumerator hcFile = vabFiles.GetEnumerator();
                                    while (hcFile.MoveNext())
                                    {
                                        try
                                        {
                                            ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                            string vn = "";

                                            foreach (ConfigNode.Value cv in craft.values)
                                            {
                                                if (cv.name == "ship")
                                                {
                                                    vn = cv.value;
                                                }
                                            }

                                            if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight), vn, HighLogic.Skin.button))
                                            {
                                                Debug.Log("[OrX Mission] === ADDING BLUPRINTS === VAB");

                                                blueprintsFile = hcFile.Current;
                                                craftToAddMission = vn;
                                                craftBrowserOpen = false;
                                                addingBluePrints = false;
                                                blueprintsAdded = true;
                                            }
                                            line++;
                                            c += 1;
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    hcFile.Dispose();
                                }
                            }
                            else
                            {
                                if (sph)
                                {
                                    int c = 0;
                                    List<string>.Enumerator hcFile = sphFiles.GetEnumerator();
                                    while (hcFile.MoveNext())
                                    {
                                        try
                                        {
                                            ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                            string vn = "";

                                            foreach (ConfigNode.Value cv in craft.values)
                                            {
                                                if (cv.name == "ship")
                                                {
                                                    vn = cv.value;
                                                }
                                            }

                                            if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight), vn, HighLogic.Skin.button))
                                            {
                                                Debug.Log("[OrX Mission] === ADDING BLUPRINTS === VAB");

                                                blueprintsFile = hcFile.Current;
                                                craftToAddMission = vn;
                                                craftBrowserOpen = false;
                                                addingBluePrints = false;
                                                blueprintsAdded = true;
                                            }
                                            line++;
                                            c += 1;
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    hcFile.Dispose();
                                }
                                else
                                {
                                    int c = 0;
                                    List<string>.Enumerator hcFile = vabFiles.GetEnumerator();
                                    while (hcFile.MoveNext())
                                    {
                                        try
                                        {
                                            ConfigNode craft = ConfigNode.Load(hcFile.Current);
                                            string vn = "";

                                            foreach (ConfigNode.Value cv in craft.values)
                                            {
                                                if (cv.name == "ship")
                                                {
                                                    vn = cv.value;
                                                }
                                            }

                                            if (GUI.Button(new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight), vn, HighLogic.Skin.button))
                                            {
                                                Debug.Log("[OrX Mission] === ADDING BLUPRINTS === VAB");

                                                blueprintsFile = hcFile.Current;
                                                craftToAddMission = vn;
                                                craftBrowserOpen = false;
                                                addingBluePrints = false;
                                                blueprintsAdded = true;
                                            }
                                            line++;
                                            c += 1;
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    hcFile.Dispose();
                                }
                            }

                            line++;
                            DrawCloseBrowser(line);
                        }
                        else
                        {
                            if (editDescription)
                            {
                                DrawEditTitle(line);
                                line++;
                                DrawDescription0(line);
                                line++;
                                if (missionDescription0 != "")
                                {
                                    DrawDescription1(line);
                                    line++;

                                    if (missionDescription1 != "")
                                    {
                                        DrawDescription2(line);
                                        line++;

                                        if (missionDescription2 != "")
                                        {
                                            DrawDescription3(line);
                                            line++;

                                            if (missionDescription3 != "")
                                            {
                                                DrawDescription4(line);
                                                line++;

                                                if (missionDescription4 != "")
                                                {
                                                    DrawDescription5(line);
                                                    line++;

                                                    if (missionDescription5 != "")
                                                    {
                                                        DrawDescription6(line);
                                                        line++;

                                                        if (missionDescription6 != "")
                                                        {
                                                            DrawDescription7(line);
                                                            line++;

                                                            if (missionDescription7 != "")
                                                            {
                                                                DrawDescription8(line);
                                                                line++;

                                                                if (missionDescription8 != "")
                                                                {
                                                                    DrawDescription9(line);
                                                                    line++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                line++;
                                DrawClearDescription(line);
                                line++;
                                DrawSaveDescription(line);
                                line++;
                                DrawCancel(line);
                            }
                            else
                            {
                                DrawTitle(line);
                                line++;

                                if (!addCoords)
                                {
                                    DrawHoloCacheName(line);
                                    line++;
                                    DrawPassword(line);
                                    line++;
                                    DrawEditDescription(line);
                                    line++;
                                    DrawMissionType(line);
                                    line++;
                                    if (!geoCache)
                                    {
                                        DrawRaceType(line);
                                        line++;
                                    }
                                    line++;
                                    DrawAddBlueprints(line);
                                    line++;
                                    line++;
                                    DrawSaveLocal(line);
                                    line++;
                                    line++;

                                    DrawSave(line);
                                    line++;
                                    DrawCancel(line);
                                }
                                else
                                {
                                    DrawAddCoords(line);
                                    line++;
                                    if (locAdded)
                                    {
                                        DrawClearLastCoord(line);
                                        line++;
                                        DrawClearAllCoords(line);
                                        line++;
                                    }

                                    line++;
                                    DrawSave(line);
                                    line++;
                                    DrawCancel(line);
                                }
                            }
                        }
                    }
                }


                toolWindowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
                WindowRectToolbar.height = toolWindowHeight;

            }
            else
            {
                GUI.DragWindow(new Rect(30, 0, toolWindowWidth - 90, 30));

                line += 0.6f;

                if (!reload)
                {
                    GUI.BeginGroup(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth, WindowRectHCGUI.height));
                    WindowHCGUI();
                    GUI.EndGroup();

                    if (!hide)
                    {
                        HCGUILines = WindowRectHCGUI.height / entryHeight;

                        HCGUIHeight = Mathf.Lerp(HCGUIHeight, HCGUILines, 0.15f);
                        line += HCGUIHeight;

                        line += 0.25f;

                        if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Load HoloCache Data", OrXGUISkin.button))
                        {
                            TargetHCGUI = false;
                            reload = true;
                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                            ClearDatabase();
                            LoadHoloCacheTargets();
                        }
                    }
                }
                else
                {
                    if (!hide)
                    {
                        if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "HoloCache Data Loading", OrXGUISkin.box))
                        {
                            // do nothing ... reload will turn false after OrXHoloCache is finished loading targets
                        }
                    }
                }

                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (!hide)
                    {
                        line += 1.25f;

                        if (!spawnHoloCache)
                        {
                            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Create HoloCache", OrXGUISkin.button))
                            {
                                if (FlightGlobals.ActiveVessel.isEVA)
                                {
                                    if (FlightGlobals.ActiveVessel.LandedOrSplashed)
                                    {
                                        // SPAWN HOLOCACHE
                                        GuiEnabledOrXMissions = true;
                                        PlayOrXMission = false;
                                        building = true;
                                        buildingMission = true;
                                        geoCache = true;
                                        OrXHCGUIEnabled = false;
                                        SpawnEmptyHoloCache();
                                    }
                                    else
                                    {
                                        //buildingMission = true;
                                        //OrXMissions.instance.StartMissionBuilder(new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude), new Guid());
                                        //HideGameUI();

                                        ScreenMsg("<color=#cc4500ff><b>You must be Landed or Splashed</b></color>");
                                        ScreenMsg("<color=#cc4500ff><b>to create a HoloCache</b></color>");
                                    }
                                }
                                else
                                {
                                    ScreenMsg("<color=#cc4500ff><b>You must be EVA to create a HoloCache</b></color>");
                                }
                            }
                        }
                        else
                        {
                            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth - 5, 20), "Creating HoloCache", OrXGUISkin.box))
                            {
                                // do nothing ... spawnHoloCache will turn false after OrXHoloCache is finished spawning empty HoloCache
                            }
                        }
                        line += 1F;
                    }
                }
                else
                {
                    GUI.BeginGroup(new Rect(5, contentTop + (line * entryHeight), toolWindowWidth, WindowRectHCGUI.height));
                    WindowHCGUI();
                    GUI.EndGroup();
                }

                toolWindowHeight = Mathf.Lerp(toolWindowHeight, contentTop + (line * entryHeight) + 5, 1);
                WindowRectToolbar.height = toolWindowHeight;

            }
        }
        private void WindowHCGUI()
        {
            GUI.Box(WindowRectHCGUI, GUIContent.none, OrXGUISkin.box);
            HCGUIEntryCount = 0;
            Rect listRect = new Rect(HCGUIBorder, HCGUIBorder, WindowRectHCGUI.width - (2 * HCGUIBorder),
                WindowRectHCGUI.height - (2 * HCGUIBorder));
            GUI.BeginGroup(listRect);

            if (HighLogic.LoadedSceneIsFlight)
            {
                targetLabel = "SOI: " + FlightGlobals.ActiveVessel.orbitDriver.orbit.referenceBody;
            }
            else
            {
                targetLabel = "Holocaches";
            }

            GUI.Label(new Rect(0, 0, listRect.width, HCGUIEntryHeight), targetLabel, kspTitleLabel);

            // Expand/Collapse Target Toggle button
            if (GUI.Button(new Rect(listRect.width - (HCGUIEntryHeight * 2), 0, HCGUIEntryHeight, HCGUIEntryHeight), showTargets ? "-" : "+", OrXGUISkin.button))
                showTargets = !showTargets;

            HCGUIEntryCount += 1.2f;
            int indexToRemove = -1;
            int index = 0;

            if (HighLogic.LoadedSceneIsEditor)
            {
                if (showTargets)
                {
                    Color origWColor = GUI.color;
                    List<OrXHoloCacheinfo>.Enumerator coordinate = HoloCacheTargets[coords].GetEnumerator();
                    while (coordinate.MoveNext())
                    {
                        string label = FormattedGeoPosShort(coordinate.Current.gpsCoordinates, true);
                        float nameWidth = 120;
                        if (scanning)
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.worldPos;
                                    }

                                    if (!reload && !hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                        {
                                            scanning = false;
                                            checking = false;
                                            worldPos = Vector3d.zero;
                                            TargetHCGUI = false;
                                            reload = true;
                                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Description</b></color>");




                                            LoadHoloCacheTargets();
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                        {
                                            scanning = false;
                                            checking = false;
                                            //worldPos = Vector3d.zero;
                                            TargetHCGUI = false;
                                            reload = true;
                                            ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Description</b></color>");
                                            LoadHoloCacheTargets();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.worldPos;
                                    }

                                    if (!hide)
                                    {
                                        if (!reload)
                                        {
                                            if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                worldPos = Vector3d.zero;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                LoadHoloCacheTargets();
                                            }

                                            if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                            {
                                                HoloCacheName = "";

                                                scanning = false;
                                                worldPos = Vector3d.zero;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                LoadHoloCacheTargets();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                        {
                                            HoloCacheName = coordinate.Current.name;

                                            TargetHCGUIIndex = index;
                                            TargetHCGUI = true;
                                            resetTargetHCGUI = false;
                                            scanning = true;
                                            _craftFile = coordinate.Current.name;
                                            // LOAD HOLOCACHE DESCRIPTION WINDOW
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                        {
                                            HoloCacheName = coordinate.Current.name;

                                            TargetHCGUIIndex = index;
                                            TargetHCGUI = true;
                                            resetTargetHCGUI = false;
                                            scanning = true;
                                            _craftFile = coordinate.Current.name;
                                            // LOAD HOLOCACHE DESCRIPTION WINDOW
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!hide)
                                {
                                    if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                    {
                                        HoloCacheName = coordinate.Current.name;

                                        TargetHCGUIIndex = index;
                                        TargetHCGUI = true;
                                        resetTargetHCGUI = false;
                                        scanning = true;
                                        _craftFile = coordinate.Current.name;
                                        // LOAD HOLOCACHE DESCRIPTION WINDOW
                                    }

                                    if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                    {
                                        HoloCacheName = coordinate.Current.name;

                                        TargetHCGUIIndex = index;
                                        TargetHCGUI = true;
                                        resetTargetHCGUI = false;
                                        scanning = true;
                                        _craftFile = coordinate.Current.name;
                                        // LOAD HOLOCACHE DESCRIPTION WINDOW
                                    }
                                }
                            }
                        }

                        HCGUIEntryCount++;
                        index++;
                        GUI.color = origWColor;
                    }
                    coordinate.Dispose();
                }
            }
            else
            {
                if (showTargets)
                {
                    List<OrXHoloCacheinfo>.Enumerator coordinate = HoloCacheTargets[coords].GetEnumerator();
                    while (coordinate.MoveNext())
                    {
                        Color origWColor = GUI.color;

                        string label = FormattedGeoPosShort(coordinate.Current.gpsCoordinates, true);
                        float nameWidth = 120;
                        if (scanning)
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.gpsCoordinates;
                                    }

                                    if (!reload && !hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.DisableGui();
                                                }
                                                HoloCacheName = "";

                                                scanning = false;
                                                checking = false;
                                                worldPos = Vector3d.zero;
                                                TargetHCGUI = false;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                LoadHoloCacheTargets();
                                            }
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.DisableGui();
                                                }
                                                HoloCacheName = "";

                                                TargetHCGUI = false;
                                                scanning = false;
                                                checking = false;
                                                worldPos = Vector3d.zero;
                                                reload = true;
                                                ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                LoadHoloCacheTargets();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (TargetHCGUI)
                            {
                                if (index == TargetHCGUIIndex)
                                {
                                    if (reloadWorldPos)
                                    {
                                        reloadWorldPos = false;
                                        worldPos = coordinate.Current.gpsCoordinates;
                                    }

                                    if (!hide)
                                    {
                                        if (!reload)
                                        {
                                            if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.box))
                                            {
                                                if (HighLogic.LoadedSceneIsFlight)
                                                {
                                                    if (OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.DisableGui();
                                                    }
                                                    HoloCacheName = "";

                                                    scanning = false;
                                                    worldPos = Vector3d.zero;
                                                    TargetHCGUI = false;
                                                    reload = true;
                                                    ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                    LoadHoloCacheTargets();
                                                }
                                            }

                                            if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.box))
                                            {
                                                if (HighLogic.LoadedSceneIsFlight)
                                                {
                                                    if (OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.DisableGui();
                                                    }
                                                    HoloCacheName = "";

                                                    TargetHCGUI = false;
                                                    scanning = false;
                                                    worldPos = Vector3d.zero;
                                                    reload = true;
                                                    ScreenMsg("<color=#cc4500ff><b>Loading HoloCache Targets</b></color>");
                                                    LoadHoloCacheTargets();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!hide)
                                    {
                                        if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (soi == FlightGlobals.currentMainBody.name)
                                                {
                                                    if (!OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.EnableGui();
                                                    }
                                                    HoloCacheName = coordinate.Current.name;

                                                    worldPos = coordinate.Current.gpsCoordinates;

                                                    lat = coordinate.Current.gpsCoordinates.x;
                                                    lon = coordinate.Current.gpsCoordinates.y;
                                                    alt = coordinate.Current.gpsCoordinates.z;
                                                    _lat = coordinate.Current.gpsCoordinates.x;
                                                    _lon = coordinate.Current.gpsCoordinates.y;
                                                    _alt = coordinate.Current.gpsCoordinates.z;

                                                    TargetHCGUIIndex = index;
                                                    TargetHCGUI = true;
                                                    resetTargetHCGUI = false;
                                                    scanning = true;

                                                    _craftFile = coordinate.Current.name;

                                                    if (!checking)
                                                    {
                                                        checking = true;
                                                        TargetDistance();
                                                    }
                                                }
                                                else
                                                {
                                                    ScreenMsg("You must be in the same SOI as this HoloCache");
                                                }
                                            }
                                        }

                                        if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                        {
                                            if (HighLogic.LoadedSceneIsFlight)
                                            {
                                                if (soi == FlightGlobals.currentMainBody.name)
                                                {
                                                    if (!OrXDC.instance.GuiEnabledOrXDC)
                                                    {
                                                        OrXDC.instance.EnableGui();
                                                    }
                                                    HoloCacheName = coordinate.Current.name;

                                                    _lat = coordinate.Current.gpsCoordinates.x;
                                                    _lon = coordinate.Current.gpsCoordinates.y;
                                                    _alt = coordinate.Current.gpsCoordinates.z;

                                                    lat = coordinate.Current.gpsCoordinates.x;
                                                    lon = coordinate.Current.gpsCoordinates.y;
                                                    alt = coordinate.Current.gpsCoordinates.z;
                                                    worldPos = coordinate.Current.gpsCoordinates;
                                                    TargetHCGUIIndex = index;
                                                    TargetHCGUI = true;
                                                    resetTargetHCGUI = false;
                                                    scanning = true;
                                                    _craftFile = coordinate.Current.name;

                                                    if (!checking)
                                                    {
                                                        checking = true;
                                                        TargetDistance();
                                                    }
                                                }
                                                else
                                                {
                                                    ScreenMsg("You must be in the same SOI as this HoloCache");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!hide)
                                {
                                    if (GUI.Button(new Rect(0, HCGUIEntryCount * HCGUIEntryHeight, nameWidth, HCGUIEntryHeight), coordinate.Current.name, OrXGUISkin.button))
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            if (soi == FlightGlobals.currentMainBody.name)
                                            {
                                                if (!OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.EnableGui();
                                                }
                                                HoloCacheName = coordinate.Current.name;

                                                worldPos = coordinate.Current.worldPos;

                                                lat = coordinate.Current.gpsCoordinates.x;
                                                lon = coordinate.Current.gpsCoordinates.y;
                                                alt = coordinate.Current.gpsCoordinates.z;

                                                HoloCacheName = coordinate.Current.name;
                                                craftFile = coordinate.Current.name;
                                                _lat = lat;
                                                _lon = lon;
                                                _alt = alt;

                                                TargetHCGUIIndex = index;
                                                TargetHCGUI = true;
                                                resetTargetHCGUI = false;
                                                scanning = true;
                                                _craftFile = coordinate.Current.name;

                                                if (!checking)
                                                {
                                                    checking = true;
                                                    TargetDistance();
                                                }
                                            }
                                            else
                                            {
                                                ScreenMsg("You must be in the same SOI as this HoloCache");
                                            }
                                        }
                                    }

                                    if (GUI.Button(new Rect(nameWidth, HCGUIEntryCount * HCGUIEntryHeight, listRect.width - HCGUIEntryHeight - nameWidth, HCGUIEntryHeight), label, OrXGUISkin.button))
                                    {
                                        if (HighLogic.LoadedSceneIsFlight)
                                        {
                                            if (soi == FlightGlobals.currentMainBody.name)
                                            {
                                                if (!OrXDC.instance.GuiEnabledOrXDC)
                                                {
                                                    OrXDC.instance.EnableGui();
                                                }
                                                HoloCacheName = coordinate.Current.name;

                                                lat = coordinate.Current.gpsCoordinates.x;
                                                lon = coordinate.Current.gpsCoordinates.y;
                                                alt = coordinate.Current.gpsCoordinates.z;
                                                worldPos = coordinate.Current.worldPos;

                                                HoloCacheName = coordinate.Current.name;
                                                craftFile = coordinate.Current.name;
                                                _lat = lat;
                                                _lon = lon;
                                                _alt = alt;

                                                TargetHCGUIIndex = index;
                                                TargetHCGUI = true;
                                                resetTargetHCGUI = false;
                                                scanning = true;
                                                _craftFile = coordinate.Current.name;

                                                if (!checking)
                                                {
                                                    checking = true;
                                                    TargetDistance();
                                                }
                                            }
                                            else
                                            {
                                                ScreenMsg("You must be in the same SOI as this HoloCache");
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        HCGUIEntryCount++;
                        index++;
                        GUI.color = origWColor;
                    }
                    coordinate.Dispose();
                }
            }

            if (resetTargetHCGUI)
            {
                checking = false;
                scanning = false;
                resetTargetHCGUI = false;
                TargetHCGUIIndex = 0;
            }

            GUI.EndGroup();
            WindowRectHCGUI.height = (2 * HCGUIBorder) + (HCGUIEntryCount * HCGUIEntryHeight);
        }


        #region Coords and Scorboard GUI

        private void GetNextCoord()
        {
            if (coordCount - gpsCount == 0)
            {
                StartEndGame();
            }
            else
            {
                stageTimes.Add(gpsCount + "," + topSurfaceSpeed + "," + maxDepth + "," + (FlightGlobals.ActiveVessel.missionTime - missionTime));
                topSurfaceSpeed = 0;
                missionTime = FlightGlobals.ActiveVessel.missionTime;
                gpsCount += 1;
                maxDepth = 0;
                bool getCoord = true;
                List<string>.Enumerator coords = CoordDatabase.GetEnumerator();
                while (coords.MoveNext())
                {
                    try
                    {
                        if (getCoord)
                        {
                            string[] data = coords.Current.Split(new char[] { ',' });
                            if (data[0] == gpsCount.ToString())
                            {
                                getCoord = false;
                                NextCoord = coords.Current;

                                string[] _coords = NextCoord.Split(new char[] { ',' });
                                if (_coords[0] == gpsCount.ToString())
                                {
                                    latMission = double.Parse(_coords[1]);
                                    lonMission = double.Parse(_coords[2]);
                                    altMission = double.Parse(_coords[3]);
                                    nextLocation = new Vector3d(double.Parse(_coords[1]), double.Parse(_coords[1]), double.Parse(_coords[1]));
                                }

                                break;
                            }
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.Log("[OrX Mission] NEXT LOCATION ACQUIRED ...... ");
                    }
                }
                coords.Dispose();

                TargetDistance();
            }
        }
        private void DrawAddCoords(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "ADD LOCATION", HighLogic.Skin.button))
            {
                if (!locAdded)
                {
                    locAdded = true;
                }

                if (windRacing)
                {
                    if (FlightGlobals.ActiveVessel.altitude >= 0 && FlightGlobals.ActiveVessel.atmDensity >= 0.007)
                    {
                        locCount += 1;
                        lastCoord = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                        latMission = lastCoord.x;
                        lonMission = lastCoord.y;
                        altMission = lastCoord.z;
                        windIntensity = WindGUI.instance.windIntensity;
                        windVariability = WindGUI.instance.windVariability;
                        variationIntensity = WindGUI.instance.variationIntensity;
                        heading = WindGUI.instance.heading;
                        teaseDelay = WindGUI.instance.teaseDelay;
                        // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                        CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                    }
                    else
                    {
                        ScreenMsg("Unable to add coordinate to Wind Challenge if vessel is below water or not in an atmosphere");
                    }
                }
                else
                {
                    if (Scuba)
                    {
                        if (FlightGlobals.ActiveVessel.altitude <= 1)
                        {
                            locCount += 1;
                            lastCoord = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                            latMission = lastCoord.x;
                            lonMission = lastCoord.y;
                            altMission = lastCoord.z;
                            windIntensity = WindGUI.instance.windIntensity;
                            windVariability = WindGUI.instance.windVariability;
                            variationIntensity = WindGUI.instance.variationIntensity;
                            heading = WindGUI.instance.heading;
                            teaseDelay = WindGUI.instance.teaseDelay;
                            // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                            CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                                + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                        }
                        else
                        {
                            ScreenMsg("Unable to add coordinate to Scuba Challenge if vessel is not Splashed");
                        }

                    }
                    else
                    {
                        locCount += 1;
                        lastCoord = new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude);
                        latMission = lastCoord.x;
                        lonMission = lastCoord.y;
                        altMission = lastCoord.z;
                        windIntensity = WindGUI.instance.windIntensity;
                        windVariability = WindGUI.instance.windVariability;
                        variationIntensity = WindGUI.instance.variationIntensity;
                        heading = WindGUI.instance.heading;
                        teaseDelay = WindGUI.instance.teaseDelay;
                        // location count, latitude, longitude, altitude, wind intensity, wind variability, wind variation intensity, heading, tease delay
                        CoordDatabase.Add(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                    }
                }
            }
        }
        private void DrawClearLastCoord(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "DELETE LAST", HighLogic.Skin.button))
            {
                CoordDatabase.Remove(locCount + "," + latMission + "," + lonMission + "," + altMission + ","
                            + windIntensity + "," + windVariability + "," + variationIntensity + "," + heading + "," + teaseDelay);
                locCount -= 1;
                if (CoordDatabase.Count == 0)
                {
                    locAdded = false;
                }
            }
        }
        private void DrawClearAllCoords(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "DELETE ALL", HighLogic.Skin.button))
            {
                CoordDatabase.Clear();
                locCount = 0;
                locAdded = false;
            }
        }
        private void SaveScore()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            _mission = _file.GetNode("mission" + mCount);
            _scoreboard_ = _mission.GetNode("scoreboard");

            if (_scoreboard_ == null)
            {
                _mission.AddNode("scoreboard");
            }
            _scoreboard_ = _mission.GetNode("scoreboard");

            if (_scoreboard_.nodes.Contains("scoreboard0"))
            {
                // DO NOTHING
            }
            else
            {
                // ADD NEW PODIUM LIST
                _scoreboard_.AddNode("scoreboard0");
                _scoreboard_.AddNode("scoreboard1");
                _scoreboard_.AddNode("scoreboard2");
                _scoreboard_.AddNode("scoreboard3");
                _scoreboard_.AddNode("scoreboard4");
                _scoreboard_.AddNode("scoreboard5");
                _scoreboard_.AddNode("scoreboard6");
                _scoreboard_.AddNode("scoreboard7");
                _scoreboard_.AddNode("scoreboard8");
                _scoreboard_.AddNode("scoreboard9");

                scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                scoreboard0.AddValue("name", "<empty>");
                scoreboard0.AddValue("time", "");
                scoreboard1.AddValue("name", "<empty>");
                scoreboard1.AddValue("time", "");
                scoreboard2.AddValue("name", "<empty>");
                scoreboard2.AddValue("time", "");
                scoreboard3.AddValue("name", "<empty>");
                scoreboard3.AddValue("time", "");
                scoreboard4.AddValue("name", "<empty>");
                scoreboard4.AddValue("time", "");
                scoreboard5.AddValue("name", "<empty>");
                scoreboard5.AddValue("time", "");
                scoreboard6.AddValue("name", "<empty>");
                scoreboard6.AddValue("time", "");
                scoreboard7.AddValue("name", "<empty>");
                scoreboard7.AddValue("time", "");
                scoreboard8.AddValue("name", "<empty>");
                scoreboard8.AddValue("time", "");
                scoreboard9.AddValue("name", "<empty>");
                scoreboard9.AddValue("time", "");
            }

            // GET CHALLENGER TOTAL TIME AND CREAT STAGE TIME LIST
            int stageCount = 0;

            ConfigNode tempChallengerEntry = null;
            double totalTimeChallenger = 0;
            List<string>.Enumerator st = stageTimes.GetEnumerator();
            while (st.MoveNext())
            {
                stageCount += 1;
                string[] data = st.Current.Split(new char[] { ',' });
                totalTimeChallenger += double.Parse(data[1]);
                tempChallengerEntry.AddValue("stage" + stageCount, double.Parse(data[1]));
            }
            tempChallengerEntry.AddValue("totalTime", totalTimeChallenger);

            // CHECK PODIUM LIST

            scoreboard0 = _scoreboard_.GetNode("scoreboard0");
            scoreboard1 = _scoreboard_.GetNode("scoreboard1");
            scoreboard2 = _scoreboard_.GetNode("scoreboard2");
            scoreboard3 = _scoreboard_.GetNode("scoreboard3");
            scoreboard4 = _scoreboard_.GetNode("scoreboard4");
            scoreboard5 = _scoreboard_.GetNode("scoreboard5");
            scoreboard6 = _scoreboard_.GetNode("scoreboard6");
            scoreboard7 = _scoreboard_.GetNode("scoreboard7");
            scoreboard8 = _scoreboard_.GetNode("scoreboard8");
            scoreboard9 = _scoreboard_.GetNode("scoreboard9");


            bool ammendListscoreboard0 = false;
            string nameToRemovescoreboard0 = string.Empty;
            double totalTimescoreboard0 = 0;
            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard0 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard0 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard0)
                        {
                            ammendListscoreboard0 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard0 = true;
                    }
                }
            }

            bool ammendListscoreboard1 = false;
            string nameToRemovescoreboard1 = string.Empty;
            double totalTimescoreboard1 = 0;
            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard1 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard1 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard1)
                        {
                            ammendListscoreboard1 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard1 = true;
                    }
                }
            }

            bool ammendListscoreboard2 = false;
            string nameToRemovescoreboard2 = string.Empty;
            double totalTimescoreboard2 = 0;
            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard2 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard2 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard2)
                        {
                            ammendListscoreboard2 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard2 = true;
                    }
                }
            }

            bool ammendListscoreboard3 = false;
            string nameToRemovescoreboard3 = string.Empty;
            double totalTimescoreboard3 = 0;
            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard3 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard3 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard3)
                        {
                            ammendListscoreboard3 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard3 = true;
                    }
                }
            }

            bool ammendListscoreboard4 = false;
            string nameToRemovescoreboard4 = string.Empty;
            double totalTimescoreboard4 = 0;
            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard4 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard4 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard4)
                        {
                            ammendListscoreboard4 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard4 = true;
                    }
                }
            }

            bool ammendListscoreboard5 = false;
            string nameToRemovescoreboard5 = string.Empty;
            double totalTimescoreboard5 = 0;
            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard5 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard5 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard5)
                        {
                            ammendListscoreboard5 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard5 = true;
                    }
                }
            }

            bool ammendListscoreboard6 = false;
            string nameToRemovescoreboard6 = string.Empty;
            double totalTimescoreboard6 = 0;
            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard6 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard6 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard6)
                        {
                            ammendListscoreboard6 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard6 = true;
                    }
                }
            }

            bool ammendListscoreboard7 = false;
            string nameToRemovescoreboard7 = string.Empty;
            double totalTimescoreboard7 = 0;
            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard7 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard7 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard7)
                        {
                            ammendListscoreboard7 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard7 = true;
                    }
                }
            }

            bool ammendListscoreboard8 = false;
            string nameToRemovescoreboard8 = string.Empty;
            double totalTimescoreboard8 = 0;
            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard8 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard8 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard8)
                        {
                            ammendListscoreboard8 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard8 = true;
                    }
                }
            }

            bool ammendListscoreboard9 = false;
            string nameToRemovescoreboard9 = string.Empty;
            double totalTimescoreboard9 = 0;
            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameToRemovescoreboard9 = cv.value;
                }

                if (cv.name == "time")
                {
                    if (cv.value != "" || cv.value != string.Empty)
                    {
                        totalTimescoreboard9 = double.Parse(cv.value);
                        if (totalTimeChallenger <= totalTimescoreboard9)
                        {
                            ammendListscoreboard9 = true;
                        }
                    }
                    else
                    {
                        ammendListscoreboard9 = true;
                    }
                }
            }


            // EDIT PODIUM LIST SCORES IF NEDED

            if (ammendListscoreboard0)
            {
                scoreboard9.ClearData();
                foreach (ConfigNode.Value cv in scoreboard8.values)
                {
                    scoreboard9.AddValue(cv.name, cv.value);
                }

                scoreboard8.ClearData();
                foreach (ConfigNode.Value cv in scoreboard7.values)
                {
                    scoreboard8.AddValue(cv.name, cv.value);
                }

                scoreboard7.ClearData();
                foreach (ConfigNode.Value cv in scoreboard6.values)
                {
                    scoreboard7.AddValue(cv.name, cv.value);
                }

                scoreboard6.ClearData();
                foreach (ConfigNode.Value cv in scoreboard5.values)
                {
                    scoreboard6.AddValue(cv.name, cv.value);
                }

                scoreboard5.ClearData();
                foreach (ConfigNode.Value cv in scoreboard4.values)
                {
                    scoreboard5.AddValue(cv.name, cv.value);
                }

                scoreboard4.ClearData();
                foreach (ConfigNode.Value cv in scoreboard3.values)
                {
                    scoreboard4.AddValue(cv.name, cv.value);
                }

                scoreboard3.ClearData();
                foreach (ConfigNode.Value cv in scoreboard2.values)
                {
                    scoreboard3.AddValue(cv.name, cv.value);
                }

                scoreboard2.ClearData();
                foreach (ConfigNode.Value cv in scoreboard1.values)
                {
                    scoreboard2.AddValue(cv.name, cv.value);
                }

                scoreboard1.ClearData();
                foreach (ConfigNode.Value cv in scoreboard0.values)
                {
                    scoreboard1.AddValue(cv.name, cv.value);
                }

                scoreboard0.ClearData();
                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                {
                    scoreboard0.AddValue(cv.name, cv.value);
                }
            }
            else
            {
                if (ammendListscoreboard1)
                {
                    scoreboard9.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard8.values)
                    {
                        scoreboard9.AddValue(cv.name, cv.value);
                    }

                    scoreboard8.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard7.values)
                    {
                        scoreboard8.AddValue(cv.name, cv.value);
                    }

                    scoreboard7.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard6.values)
                    {
                        scoreboard7.AddValue(cv.name, cv.value);
                    }

                    scoreboard6.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard5.values)
                    {
                        scoreboard6.AddValue(cv.name, cv.value);
                    }

                    scoreboard5.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard4.values)
                    {
                        scoreboard5.AddValue(cv.name, cv.value);
                    }

                    scoreboard4.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard3.values)
                    {
                        scoreboard4.AddValue(cv.name, cv.value);
                    }

                    scoreboard3.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard2.values)
                    {
                        scoreboard3.AddValue(cv.name, cv.value);
                    }

                    scoreboard2.ClearData();
                    foreach (ConfigNode.Value cv in scoreboard1.values)
                    {
                        scoreboard2.AddValue(cv.name, cv.value);
                    }

                    scoreboard1.ClearData();
                    foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                    {
                        scoreboard1.AddValue(cv.name, cv.value);
                    }
                }
                else
                {
                    if (ammendListscoreboard2)
                    {
                        scoreboard9.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard8.values)
                        {
                            scoreboard9.AddValue(cv.name, cv.value);
                        }

                        scoreboard8.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard7.values)
                        {
                            scoreboard8.AddValue(cv.name, cv.value);
                        }

                        scoreboard7.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard6.values)
                        {
                            scoreboard7.AddValue(cv.name, cv.value);
                        }

                        scoreboard6.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard5.values)
                        {
                            scoreboard6.AddValue(cv.name, cv.value);
                        }

                        scoreboard5.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard4.values)
                        {
                            scoreboard5.AddValue(cv.name, cv.value);
                        }

                        scoreboard4.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard3.values)
                        {
                            scoreboard4.AddValue(cv.name, cv.value);
                        }

                        scoreboard3.ClearData();
                        foreach (ConfigNode.Value cv in scoreboard2.values)
                        {
                            scoreboard3.AddValue(cv.name, cv.value);
                        }

                        scoreboard2.ClearData();
                        foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                        {
                            scoreboard2.AddValue(cv.name, cv.value);
                        }
                    }
                    else
                    {
                        if (ammendListscoreboard3)
                        {
                            scoreboard9.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard8.values)
                            {
                                scoreboard9.AddValue(cv.name, cv.value);
                            }

                            scoreboard8.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard7.values)
                            {
                                scoreboard8.AddValue(cv.name, cv.value);
                            }

                            scoreboard7.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard6.values)
                            {
                                scoreboard7.AddValue(cv.name, cv.value);
                            }

                            scoreboard6.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard5.values)
                            {
                                scoreboard6.AddValue(cv.name, cv.value);
                            }

                            scoreboard5.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard4.values)
                            {
                                scoreboard5.AddValue(cv.name, cv.value);
                            }

                            scoreboard4.ClearData();
                            foreach (ConfigNode.Value cv in scoreboard3.values)
                            {
                                scoreboard4.AddValue(cv.name, cv.value);
                            }

                            scoreboard3.ClearData();
                            foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                            {
                                scoreboard3.AddValue(cv.name, cv.value);
                            }
                        }
                        else
                        {
                            if (ammendListscoreboard4)
                            {
                                scoreboard9.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard8.values)
                                {
                                    scoreboard9.AddValue(cv.name, cv.value);
                                }

                                scoreboard8.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard7.values)
                                {
                                    scoreboard8.AddValue(cv.name, cv.value);
                                }

                                scoreboard7.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard6.values)
                                {
                                    scoreboard7.AddValue(cv.name, cv.value);
                                }

                                scoreboard6.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard5.values)
                                {
                                    scoreboard6.AddValue(cv.name, cv.value);
                                }

                                scoreboard5.ClearData();
                                foreach (ConfigNode.Value cv in scoreboard4.values)
                                {
                                    scoreboard5.AddValue(cv.name, cv.value);
                                }

                                scoreboard4.ClearData();
                                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                {
                                    scoreboard4.AddValue(cv.name, cv.value);
                                }
                            }
                            else
                            {
                                if (ammendListscoreboard5)
                                {
                                    scoreboard9.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard8.values)
                                    {
                                        scoreboard9.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard8.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard7.values)
                                    {
                                        scoreboard8.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard7.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard6.values)
                                    {
                                        scoreboard7.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard6.ClearData();
                                    foreach (ConfigNode.Value cv in scoreboard5.values)
                                    {
                                        scoreboard6.AddValue(cv.name, cv.value);
                                    }

                                    scoreboard5.ClearData();
                                    foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                    {
                                        scoreboard5.AddValue(cv.name, cv.value);
                                    }
                                }
                                else
                                {
                                    if (ammendListscoreboard6)
                                    {
                                        scoreboard9.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard8.values)
                                        {
                                            scoreboard9.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard8.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard7.values)
                                        {
                                            scoreboard8.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard7.ClearData();
                                        foreach (ConfigNode.Value cv in scoreboard6.values)
                                        {
                                            scoreboard7.AddValue(cv.name, cv.value);
                                        }

                                        scoreboard6.ClearData();
                                        foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                        {
                                            scoreboard6.AddValue(cv.name, cv.value);
                                        }
                                    }
                                    else
                                    {
                                        if (ammendListscoreboard7)
                                        {
                                            scoreboard9.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard8.values)
                                            {
                                                scoreboard9.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard8.ClearData();
                                            foreach (ConfigNode.Value cv in scoreboard7.values)
                                            {
                                                scoreboard8.AddValue(cv.name, cv.value);
                                            }

                                            scoreboard7.ClearData();
                                            foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                            {
                                                scoreboard7.AddValue(cv.name, cv.value);
                                            }
                                        }
                                        else
                                        {
                                            if (ammendListscoreboard8)
                                            {
                                                scoreboard9.ClearData();
                                                foreach (ConfigNode.Value cv in scoreboard8.values)
                                                {
                                                    scoreboard9.AddValue(cv.name, cv.value);
                                                }

                                                scoreboard8.ClearData();
                                                foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                                {
                                                    scoreboard8.AddValue(cv.name, cv.value);
                                                }
                                            }
                                            else
                                            {
                                                if (ammendListscoreboard9)
                                                {
                                                    scoreboard9.ClearData();
                                                    foreach (ConfigNode.Value cv in tempChallengerEntry.values)
                                                    {
                                                        scoreboard9.AddValue(cv.name, cv.value);
                                                    }
                                                }
                                                else
                                                {
                                                    // NO CHANGE TO PODIUM
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameSB0 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB0 = cv.value;
                }
            }


            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameSB1 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB1 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameSB2 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB2 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameSB3 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB3 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameSB4 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB4 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameSB5 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB5 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameSB6 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB6 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameSB7 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB7 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameSB8 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB8 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameSB9 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB9 = cv.value;
                }
            }
        }
        private void GetScoreboardData()
        {
            _file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            _mission = _file.GetNode("mission" + mCount);
            _scoreboard_ = _mission.GetNode("scoreboard");
            int sbCount = 0;

            if (_scoreboard_.nodes.Contains("scoreboard0"))
            {
                // DO NOTHING
            }
            else  // ADD NEW PODIUM LIST
            {
                _scoreboard_.AddNode("scoreboard0");
                _scoreboard_.AddNode("scoreboard1");
                _scoreboard_.AddNode("scoreboard2");
                _scoreboard_.AddNode("scoreboard3");
                _scoreboard_.AddNode("scoreboard4");
                _scoreboard_.AddNode("scoreboard5");
                _scoreboard_.AddNode("scoreboard6");
                _scoreboard_.AddNode("scoreboard7");
                _scoreboard_.AddNode("scoreboard8");
                _scoreboard_.AddNode("scoreboard9");

                scoreboard0 = _scoreboard_.GetNode("scoreboard0");
                scoreboard1 = _scoreboard_.GetNode("scoreboard1");
                scoreboard2 = _scoreboard_.GetNode("scoreboard2");
                scoreboard3 = _scoreboard_.GetNode("scoreboard3");
                scoreboard4 = _scoreboard_.GetNode("scoreboard4");
                scoreboard5 = _scoreboard_.GetNode("scoreboard5");
                scoreboard6 = _scoreboard_.GetNode("scoreboard6");
                scoreboard7 = _scoreboard_.GetNode("scoreboard7");
                scoreboard8 = _scoreboard_.GetNode("scoreboard8");
                scoreboard9 = _scoreboard_.GetNode("scoreboard9");

                scoreboard0.AddValue("name", "<empty>");
                scoreboard0.AddValue("time", "");
                scoreboard1.AddValue("name", "<empty>");
                scoreboard1.AddValue("time", "");
                scoreboard2.AddValue("name", "<empty>");
                scoreboard2.AddValue("time", "");
                scoreboard3.AddValue("name", "<empty>");
                scoreboard3.AddValue("time", "");
                scoreboard4.AddValue("name", "<empty>");
                scoreboard4.AddValue("time", "");
                scoreboard5.AddValue("name", "<empty>");
                scoreboard5.AddValue("time", "");
                scoreboard6.AddValue("name", "<empty>");
                scoreboard6.AddValue("time", "");
                scoreboard7.AddValue("name", "<empty>");
                scoreboard7.AddValue("time", "");
                scoreboard8.AddValue("name", "<empty>");
                scoreboard8.AddValue("time", "");
                scoreboard9.AddValue("name", "<empty>");
                scoreboard9.AddValue("time", "");

                _file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            }

            // CHECK PODIUM LIST
            scoreboard0 = _scoreboard_.GetNode("scoreboard0");
            scoreboard1 = _scoreboard_.GetNode("scoreboard1");
            scoreboard2 = _scoreboard_.GetNode("scoreboard2");
            scoreboard3 = _scoreboard_.GetNode("scoreboard3");
            scoreboard4 = _scoreboard_.GetNode("scoreboard4");
            scoreboard5 = _scoreboard_.GetNode("scoreboard5");
            scoreboard6 = _scoreboard_.GetNode("scoreboard6");
            scoreboard7 = _scoreboard_.GetNode("scoreboard7");
            scoreboard8 = _scoreboard_.GetNode("scoreboard8");
            scoreboard9 = _scoreboard_.GetNode("scoreboard9");

            foreach (ConfigNode.Value cv in scoreboard0.values)
            {
                if (cv.name == "name")
                {
                    nameSB0 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB0 = cv.value;
                }
            }


            foreach (ConfigNode.Value cv in scoreboard1.values)
            {
                if (cv.name == "name")
                {
                    nameSB1 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB1 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard2.values)
            {
                if (cv.name == "name")
                {
                    nameSB2 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB2 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard3.values)
            {
                if (cv.name == "name")
                {
                    nameSB3 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB3 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard4.values)
            {
                if (cv.name == "name")
                {
                    nameSB4 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB4 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard5.values)
            {
                if (cv.name == "name")
                {
                    nameSB5 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB5 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard6.values)
            {
                if (cv.name == "name")
                {
                    nameSB6 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB6 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard7.values)
            {
                if (cv.name == "name")
                {
                    nameSB7 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB7 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard8.values)
            {
                if (cv.name == "name")
                {
                    nameSB8 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB8 = cv.value;
                }
            }

            foreach (ConfigNode.Value cv in scoreboard9.values)
            {
                if (cv.name == "name")
                {
                    nameSB9 = cv.value;
                }

                if (cv.name == "time")
                {
                    timeSB9 = cv.value;
                }
            }

            showScores = true;
        }

        #endregion

        #region Scoreboard GUI

        private void DrawScoreboard(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), missionName + " Scoreboard", titleStyle);
        }
        private void DrawScoreboard0(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB0 + " - " + String.Format("{0:0.00}", timeSB0), titleStyle);
        }
        private void DrawScoreboard1(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB1 + " - " + String.Format("{0:0.00}", timeSB1), titleStyle);
        }
        private void DrawScoreboard2(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB2 + " - " + String.Format("{0:0.00}", timeSB2), titleStyle);
        }
        private void DrawScoreboard3(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB3 + " - " + String.Format("{0:0.00}", timeSB3), titleStyle);
        }
        private void DrawScoreboard4(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB4 + " - " + String.Format("{0:0.00}", timeSB4), titleStyle);
        }
        private void DrawScoreboard5(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB5 + " - " + String.Format("{0:0.00}", timeSB5), titleStyle);
        }
        private void DrawScoreboard6(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB6 + " - " + String.Format("{0:0.00}", timeSB6), titleStyle);
        }
        private void DrawScoreboard7(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB7 + " - " + String.Format("{0:0.00}", timeSB7), titleStyle);
        }
        private void DrawScoreboard8(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB8 + " - " + String.Format("{0:0.00}", timeSB8), titleStyle);
        }
        private void DrawScoreboard9(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), nameSB9 + " - " + String.Format("{0:0.00}", timeSB9), titleStyle);
        }
        private void DrawCloseScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CLOSE", HighLogic.Skin.button))
            {
                showScores = false;
            }
        }

        #endregion

        #region Play Mission GUI

        private void DrawPlayHoloCacheName(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), HoloCacheName, titleStyle);
        }
        private void DrawPlayTitle(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), missionName, titleStyle);
        }
        private void DrawPlayMissionType(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), missionType, titleStyle);
        }
        private void DrawPlayRaceType(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), challengeType, titleStyle);
        }
        private void DrawPlayBlueprintsAdded(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "", titleStyle);
        }
        private void DrawChallengerName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenger: ",
                leftLabel);
            float textFieldWidth = 80;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            challengersName = GUI.TextField(fwdFieldRect, challengersName);
        }

        private void DrawPlayPassword(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password: ",
                leftLabel);
            float textFieldWidth = 80;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            Password = GUI.TextField(fwdFieldRect, Password);
        }
        private void DrawUnlock(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "UNLOCK", HighLogic.Skin.button))
            {
                if (Password == pas)
                {
                    Debug.Log("[OrX Mission] === UNLOCKING ===");

                    unlocked = true;
                }
                else
                {
                    Debug.Log("[OrX Mission] === WRONG PASSWORD ===");

                    ScreenMsg("WRONG PASSWORD");
                }
            }
        }
        private void DrawShowScoreboard(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "SHOW SCOREBOARD", HighLogic.Skin.button))
            {
                Debug.Log("[OrX Mission] === SHOW SCOREBOARD ===");
                GetScoreboardData();
            }
        }

        private void DrawStart(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!geoCache)
            {
                if (GUI.Button(saveRect, "START CHALLENGE", HighLogic.Skin.button))
                {
                    if (challengersName != "" || challengersName != string.Empty)
                    {
                        Debug.Log("[OrX Mission] === NAME ENTERED - STARTING ===");

                        StartChallenge();
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === PLEASE ENTER CHALLENGER NAME ===");

                        ScreenMsg("Please enter a challenger name");
                    }
                }
            }
            else
            {
                if (GUI.Button(saveRect, "CLOSE WINDOW", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === HOLO IS GEO-CACHE - CLOSING WINDOW ===");

                    StartChallenge();
                }
            }
        }

        #endregion

        #region Edit Description Window GUI

        private void DrawEditTitle(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Description Editor", titleStyle);
        }

        private void DrawClearDescription(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CLEAR DESCRIPTION", HighLogic.Skin.button))
            {
                Debug.Log("[OrX Mission] === CLEARING DESCRIPTION ===");

                missionDescription0 = string.Empty;
                missionDescription1 = string.Empty;
                missionDescription2 = string.Empty;
                missionDescription3 = string.Empty;
                missionDescription4 = string.Empty;
                missionDescription5 = string.Empty;
                missionDescription6 = string.Empty;
                missionDescription7 = string.Empty;
                missionDescription8 = string.Empty;
                missionDescription9 = string.Empty;
            }
        }

        private void DrawSaveDescription(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
            {
                Debug.Log("[OrX Mission] === SAVING DESCRIPTION ===");

                editDescription = false;
            }
        }

        private void DrawDescription0(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription0 = GUI.TextField(fwdFieldRect, missionDescription0);
        }
        private void DrawDescription1(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription1 = GUI.TextField(fwdFieldRect, missionDescription1);
        }
        private void DrawDescription2(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription2 = GUI.TextField(fwdFieldRect, missionDescription2);
        }
        private void DrawDescription3(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription3 = GUI.TextField(fwdFieldRect, missionDescription3);
        }
        private void DrawDescription4(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription4 = GUI.TextField(fwdFieldRect, missionDescription4);
        }
        private void DrawDescription5(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription5 = GUI.TextField(fwdFieldRect, missionDescription5);
        }
        private void DrawDescription6(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription6 = GUI.TextField(fwdFieldRect, missionDescription6);
        }
        private void DrawDescription7(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription7 = GUI.TextField(fwdFieldRect, missionDescription7);
        }
        private void DrawDescription8(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription8 = GUI.TextField(fwdFieldRect, missionDescription8);
        }
        private void DrawDescription9(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "",
                leftLabel);
            float textFieldWidth = 220;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            missionDescription9 = GUI.TextField(fwdFieldRect, missionDescription9);
        }


        #endregion

        #region Craft Browser

        private void DrawCraftBrowserTitle(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Blueprints Browser", titleStyle);
        }
        private void DrawHangar(float line)
        {
            var sphButton = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, 110, entryHeight);
            var vabButton = new Rect((LeftIndent * 1.5f) + contentWidth - 110, ContentTop + line * entryHeight, 100, entryHeight);

            if (sph)
            {
                if (GUI.Button(sphButton, "SPH", HighLogic.Skin.box))
                {
                }

                if (GUI.Button(vabButton, "VAB", HighLogic.Skin.button))
                {
                    sph = false;
                }
            }
            else
            {
                if (GUI.Button(sphButton, "SPH", HighLogic.Skin.button))
                {
                    sph = true;
                }

                if (GUI.Button(vabButton, "VAB", HighLogic.Skin.box))
                {
                }
            }
        }
        private void DrawCloseBrowser(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "CLOSE", HighLogic.Skin.button))
            {
                craftBrowserOpen = false;
            }
        }


        #endregion

        #region Challenge Creator GUI

        string holocacheCraftLoc = string.Empty;
        List<string> holocacheFiles;
        string sphLoc = string.Empty;
        List<string> sphFiles;
        string vabLoc = string.Empty;
        List<string> vabFiles;
        bool sph = false;
        bool holoHangar = true;

        int _hcCount = 0;
        bool saveLocalVessels = false;
        string saveLocalLabel = "Save Local Craft";

        private void DrawTitle(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };

            if (addCoords)
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "Co-ordinate Editor", titleStyle);
            }
            else
            {
                GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX HoloCache Creator", titleStyle);
            }
        }
        private void DrawHoloCacheName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "HoloCache Name: ",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            HoloCacheName = GUI.TextField(fwdFieldRect, HoloCacheName);
        }
        private void DrawMissionType(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "HoloCache Type: ",
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight);

            if (geoCache)
            {
                if (GUI.Button(bfRect, missionType, HighLogic.Skin.button))
                {
                    //Debug.Log("[OrX Mission] === MISSION TYPE - CHALLENGE ===");
                    Debug.Log("[OrX Mission] === MISSION TYPE LOCKED AS GEO-CACHE ===");
                    ScreenMsg("MISSION TYPE LOCKED AS GEO-CACHE");
                    if (!locAdded)
                    {
                        geoCache = false;
                        missionType = "CHALLENGE";
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === MISSION TYPE LOCKED AS GEO-CACHE ===");

                    }
                }
            }
            else
            {
                if (GUI.Button(bfRect, missionType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        missionType = "GEO-CACHE";
                        geoCache = true;
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === MISSION TYPE LOCKED AS CHALLENGE ===");

                    }
                }
            }
        }
        private void DrawRaceType(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Challenge Type: ",
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 120, ContentTop + line * entryHeight, 120, entryHeight);

            if (windRacing && !Scuba)
            {
                if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                {
                    if (!locAdded)
                    {
                        Debug.Log("[OrX Mission] === CHALLENGE TYPE - WIND RACING ===");
                        challengeType = "SCUBA";

                        windRacing = false;
                        Scuba = true;
                    }
                    else
                    {
                        Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS WIND RACING ===");

                    }
                }
            }
            else
            {
                if (Scuba)
                {

                    if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                    {
                        if (!locAdded)
                        {
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE - SCUBA ===");
                            challengeType = "OUTLAW RACING";

                            windRacing = false;
                            Scuba = false;
                        }
                        else
                        {
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS SCUBA ===");

                        }

                    }
                }
                else
                {
                    if (GUI.Button(bfRect, challengeType, HighLogic.Skin.button))
                    {
                        if (!locAdded)
                        {
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE - OUTLAW RACING ===");
                            challengeType = "WIND RACING";

                            windRacing = true;
                            Scuba = false;
                        }
                        else
                        {
                            Debug.Log("[OrX Mission] === CHALLENGE TYPE LOCKED AS OUTLAW RACING ===");

                        }
                    }
                }
            }
        }
        private void DrawAddBlueprints(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!blueprintsAdded)
            {
                if (GUI.Button(saveRect, "ADD BLUEPRINTS", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === ADDING BLUEPRINTS ===");

                    addingBluePrints = true;
                    blueprintsFile = "";
                    PlayOrXMission = false;

                    craftBrowserOpen = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "BLUEPRINTS ADDED", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === REMOVING BLUEPRINTS ===");

                    blueprintsFile = "";
                    blueprintsAdded = false;
                }
            }
        }

        private void DrawPassword(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password:",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            Password = GUI.TextField(fwdFieldRect, Password);
        }
        private void DrawModule(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Tech: ",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            tech = GUI.TextField(fwdFieldRect, tech);
        }

        private void DrawEditDescription(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!editDescription)
            {
                if (GUI.Button(saveRect, "EDIT DESCRIPTION", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === EDITING DESCRIPTION ===");

                    editDescription = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "CLOSE WINDOW", HighLogic.Skin.box))
                {
                    Debug.Log("[OrX Mission] === CLOSING EDIT WINDOW ===");

                    editDescription = false;
                }
            }
        }

        private void DrawSaveLocal(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), saveLocalLabel,
                leftLabel);
            var bfRect = new Rect(LeftIndent + contentWidth - 30, ContentTop + line * entryHeight, 10, entryHeight);

            if (!saveLocalVessels)
            {
                if (GUI.Button(bfRect, "", HighLogic.Skin.button))
                {
                    Debug.Log("[OrX Mission] === SAVE LOCAL VESSELS = TRUE ===");
                    saveLocalVessels = true;
                    saveLocalLabel = "Saving Local Craft";
                }
            }
            else
            {
                if (GUI.Button(bfRect, "X", HighLogic.Skin.box))
                {
                    Debug.Log("[OrX Mission] === SAVE LOCAL VESSELS = FALSE ===");
                    saveLocalVessels = false;
                    saveLocalLabel = "Save Local Craft";
                }
            }
        }
        private void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!geoCache)
            {
                if (addCoords)
                {
                    if (GUI.Button(saveRect, "SAVE AND EXIT", HighLogic.Skin.button))
                    {
                        Debug.Log("[OrX Mission] === SAVING .orx ===");

                        SaveCoords();
                    }
                }
                else
                {
                    if (GUI.Button(saveRect, "ADD COORDS", HighLogic.Skin.button))
                    {
                        if (HoloCacheName != string.Empty && HoloCacheName != "")
                        {
                            if (missionDescription0 != string.Empty && missionDescription0 != "")
                            {
                                CoordDatabase = new List<string>();
                                addCoords = true;
                                SaveConfig();
                            }
                            else
                            {
                                ScreenMsg("Please add a description");
                            }
                        }
                        else
                        {
                            ScreenMsg("Please enter a name for your HoloCache");
                        }
                    }
                }
            }
            else
            {
                if (GUI.Button(saveRect, "SAVE", HighLogic.Skin.button))
                {
                    if (HoloCacheName != string.Empty && HoloCacheName != "")
                    {
                        if (missionDescription0 != string.Empty && missionDescription0 != "")
                        {
                            Debug.Log("[OrX Mission] === SAVING .orx ===");
                            GuiEnabledOrXMissions = false;
                            OrXHCGUIEnabled = false;
                            building = false;
                            buildingMission = false;
                            OrXLog.instance.building = false;
                            SaveConfig();
                        }
                        else
                        {
                            ScreenMsg("Please add a description");
                        }
                    }
                    else
                    {
                        ScreenMsg("Please enter a name for your HoloCache");
                    }
                }
            }
        }
        private void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            string label = string.Empty;

            if (GUI.Button(saveRect, "CANCEL", HighLogic.Skin.button))
            {
                if (PlayOrXMission)
                {
                    Debug.Log("[OrX Mission] === CANCEL CHALLENGE ===");

                    CoordDatabase.Clear();
                    locCount = 0;
                    locAdded = false;
                    ResetData();
                    DisableGui();
                }
                else
                {
                    Debug.Log("[OrX Mission] === CANCEL HOLOCACHE CREATION ===");

                    List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                    while (v.MoveNext())
                    {
                        if (v.Current == null) continue;
                        if (!v.Current.loaded || v.Current.packed) continue;
                        if (v.Current.id == id)
                        {
                            v.Current.DestroyVesselComponents();
                            v.Current.Die();
                        }
                    }
                    v.Dispose();

                    ResetData();
                    locCount = 0;
                    locAdded = false;
                    DisableGui();
                }
            }
        }

        #endregion

        #endregion

        private void Dummy() { }

    }
}