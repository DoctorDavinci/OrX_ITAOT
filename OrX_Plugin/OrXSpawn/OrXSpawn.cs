using System;
using System.Collections;
using System.Collections.Generic;
using OrX.parts;
using UnityEngine;
using KSP.UI.Screens;
using System.IO;
using System.Reflection;

namespace OrX.spawn
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXSpawn : MonoBehaviour
    {
        public static OrXSpawn instance;
        //internal static List<ProtoCrewMember> SelectedCrewData;
        private bool loadingCraft = false;
        private string flagURL = string.Empty;
        private bool delay = true;
        private double _altitude = 0.0f;
        private bool detecting = false;
        private bool timer = false;
        public Vector3d SpawnCoords;
        UntrackedObjectClass sizeClass;
        private float average = 1;
        
        public bool orx = false;
        public bool waldo = false;
        public bool brute = false;
        public bool stayPunkd = false;
        public bool player = false;
        public float infectedCount = 0;
        public bool infected = false;

        private string orxCraft = string.Empty;
        private string waldoCraft = string.Empty;
        private string bruteCraft = string.Empty;
        private string stayPunkdCraft = string.Empty;
        private string infectedCraft = string.Empty;

        private string craftToSpawn = string.Empty;

        private string OrXname = "OrX";
        private string Waldoname = "Waldo";
        private string Brutename = "Brute";
        private string StayPunkdname = "Stay Punkd";
        private string infectedName = "Infected";

        private string Spawnedname = string.Empty;

        private int orxSalt = 15; // + (//KerbinMissions.instance.level * 2);
        private int bruteSalt = 40; // + (//KerbinMissions.instance.level * 5);
        private int stayPunkdSalt = 75; // + (//KerbinMissions.instance.level * 10);
        private int waldoSalt = 100; // + (//KerbinMissions.instance.level * 20);

        public double _lat = 0;
        public double _lon = 0;
        public double _alt = 0;
        private double _lat_ = 2f;
        private double _lon_ = 2f;

        public bool holocacheSpawn = false;
        private int modAlt = 2;
        private int randomizeLoc = 0;
        public bool survival = false;
        ConfigNode toLoad;

        private void Awake()
        {
            var _OrXdir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            flagURL = _OrXdir + "\\OrX_icon";
            orxCraft = _OrXdir + "\\PluginData" + "\\OrXKerbal.craft";
            if (instance) Destroy(instance);
            instance = this;
            toLoad = ConfigNode.Load(orxCraft);
            if (toLoad == null)
            {
                toLoad = new ConfigNode();
                toLoad.Save(orxCraft);
            }
        }

        public void SpawnInfected()
        {
            randomizeLoc = new System.Random().Next(1, 100);

            if (randomizeLoc <= 26)
            {
                _lat_ = 0.0002f;
                _lon_ = 0.0002f;
            }
            else
            {
                if (randomizeLoc <= 51)
                {
                    _lat_ = -0.0002f;
                    _lon_ = 0.0002f;
                }
                else
                {
                    if (randomizeLoc <= 76)
                    {
                        _lat_ = 0.0002f;
                        _lon_ = -0.0002f;
                    }
                    else
                    {
                        if (randomizeLoc <= 101)
                        {
                            _lat_ = -0.0002f;
                            _lon_ = -0.0002f;
                        }
                    }
                }
            }

            Debug.Log("[OrX Spawn] SpawnInfected ................. ");
            loadingCraft = true;
            modAlt = 2;
            infected = false;
            waldo = false;
            player = false;
            orx = true;
            brute = false;
            stayPunkd = false;
            craftToSpawn = orxCraft;
            Spawnedname = infectedName;
            detecting = false;
            timer = true;
            //StartCoroutine(SpawnCraftRoutine(craftToSpawn));
            ////KerbinMissions.instance.saltTotal += orxSalt;
        }

        public void SpawnNPC()
        {
            randomizeLoc = new System.Random().Next(-100, 100);

            _lat_ = randomizeLoc * 0.00001f;
            _lon_ = randomizeLoc * 0.00001f;

            Debug.Log("[OrX Spawn] SpawnOrX ................. ");
            loadingCraft = true;
            modAlt = 2;
            waldo = false;
            player = false;
            orx = true;
            brute = false;
            stayPunkd = false;
            infected = false;
            craftToSpawn = orxCraft;
            Spawnedname = "NPC";
            detecting = false;
            timer = true;
            //StartCoroutine(SpawnCraftRoutine(craftToSpawn));
            ////KerbinMissions.instance.saltTotal += orxSalt;
        }

        public void SpawnWaldo()
        {
            Debug.Log("[OrX Spawn] SpawnWaldo ................. ");
            loadingCraft = true;
            modAlt = 5;
            waldo = true;
            player = false;
            orx = false;
            brute = false;
            stayPunkd = false;
            craftToSpawn = waldoCraft;
            Spawnedname = Waldoname;
            detecting = false;
            timer = true;
            StartCoroutine(SpawnCraftRoutine(craftToSpawn));
            ////KerbinMissions.instance.saltTotal += waldoSalt;
        }

        public void SpawnBrute()
        {
            randomizeLoc = new System.Random().Next(1, 100);
            loadingCraft = true;
            modAlt = 3;
            brute = true;
            player = false;
            orx = false;
            waldo = false;
            stayPunkd = false;
            craftToSpawn = bruteCraft;
            Spawnedname = Brutename;
            detecting = false;
            timer = true;

            if (randomizeLoc <= 26)
            {
                _lat_ = 0.00001f;
                _lon_ = 0.00001f;
            }
            else
            {
                if (randomizeLoc <= 51)
                {
                    _lat_ = -0.00001f;
                    _lon_ = 0.00001f;
                }
                else
                {
                    if (randomizeLoc <= 76)
                    {
                        _lat_ = 0.00001f;
                        _lon_ = -0.00001f;
                    }
                    else
                    {
                        if (randomizeLoc <= 101)
                        {
                            _lat_ = -0.00001f;
                            _lon_ = -0.00001f;
                        }
                    }
                }
            }

            _lat += _lat_;
            _lon += _lon_;

            Debug.Log("[OrX Spawn] SpawnBrute ................. ");
            StartCoroutine(SpawnCraftRoutine(craftToSpawn));
            ////KerbinMissions.instance.saltTotal += bruteSalt;
        }

        public void SpawnStayPunkd()
        {
            randomizeLoc = new System.Random().Next(1, 100);
            loadingCraft = true;
            modAlt = 3;
            stayPunkd = true;
            player = false;
            orx = false;
            waldo = false;
            brute = false;
            craftToSpawn = stayPunkdCraft;
            Spawnedname = StayPunkdname;
            detecting = false;
            timer = true;

            if (randomizeLoc <= 26)
            {
                _lat_ = 0.00001f;
                _lon_ = 0.00001f;
            }
            else
            {
                if (randomizeLoc <= 51)
                {
                    _lat_ = -0.00001f;
                    _lon_ = 0.00001f;
                }
                else
                {
                    if (randomizeLoc <= 76)
                    {
                        _lat_ = 0.00001f;
                        _lon_ = -0.00001f;
                    }
                    else
                    {
                        if (randomizeLoc <= 101)
                        {
                            _lat_ = -0.00001f;
                            _lon_ = -0.00001f;
                        }
                    }
                }
            }

            _lat += _lat_;
            _lon += _lon_;

            Debug.Log("[OrX Spawn] SpawnStayPunkd ................. ");
            StartCoroutine(SpawnCraftRoutine(craftToSpawn));
            ////KerbinMissions.instance.saltTotal += stayPunkdSalt;
        }

        public void SurvivalSpawn()
        {
            randomizeLoc = new System.Random().Next(1, 100);
            loadingCraft = true;
            modAlt = 2;
            waldo = false;
            player = false;
            orx = true;
            brute = false;
            stayPunkd = false;
            Spawnedname = OrXname;
            detecting = false;
            timer = true;

            if (randomizeLoc <= 26)
            {
                craftToSpawn = orxCraft;
                _lat_ = 0.002f;
                _lon_ = 0.002f;
            }
            else
            {
                if (randomizeLoc <= 51)
                {
                    //KerbinMissions.instance.saltTotal += bruteSalt;

                    craftToSpawn = bruteCraft;
                    _lat_ = -0.002f;
                    _lon_ = 0.002f;
                }
                else
                {
                    if (randomizeLoc <= 76)
                    {
                        /*
                        if (KerbinMissions.instance.level >= 5)
                        {
                            //KerbinMissions.instance.saltTotal += stayPunkdSalt;

                            craftToSpawn = stayPunkdCraft;
                        }
                        else
                        {
                            //KerbinMissions.instance.saltTotal += orxSalt;

                            craftToSpawn = orxCraft;

                        }*/

                        _lat_ = 0.002f;
                        _lon_ = -0.002f;
                    }
                    else
                    {
                        if (randomizeLoc <= 101)
                        {
                            //KerbinMissions.instance.saltTotal += orxSalt;

                            craftToSpawn = orxCraft;
                            _lat_ = -0.002f;
                            _lon_ = -0.002f;
                        }
                    }
                }
            }

            Debug.Log("[OrX Spawn] SpawnOrX ................. ");

            _lat = FlightGlobals.ActiveVessel.latitude + _lat_;
            _lon = FlightGlobals.ActiveVessel.longitude + _lon_;
            _alt = FlightGlobals.ActiveVessel.altitude;

            StartCoroutine(SpawnCraftRoutine(craftToSpawn));
        }

        public bool waldoAttack = false;

        private Vector3 worldPos;

        private IEnumerator SpawnCraftRoutine(string craftUrl, List<ProtoCrewMember> crewData = null)
        {
            yield return new WaitForFixedUpdate();

            if (waldoAttack || holocacheSpawn || orx)
            {
                waldoAttack = false;
                holocacheSpawn = false;
                worldPos = SpawnCoords;
            }
            else
            {
                worldPos = _SpawnCoords();
            }

            Vector3 gpsPos = WorldPositionToGeoCoords(worldPos, FlightGlobals.currentMainBody);
            SpawnVesselFromCraftFile(craftUrl, gpsPos, 90, 0, crewData);

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
            return new Vector3d(lat, longi, alt);
        }

        private void SpawnVesselFromCraftFile(string craftURL, Vector3d gpsCoords, float heading, float pitch, List<ProtoCrewMember> crewData = null)
        {
            VesselData newData = new VesselData();

            newData.craftURL = craftURL;
            newData.latitude = gpsCoords.x + _lat_;
            newData.longitude = gpsCoords.y + _lon_;
            newData.altitude = gpsCoords.z + modAlt;

            newData.body = FlightGlobals.currentMainBody;
            newData.heading = 0;
            newData.pitch = 0;
            newData.orbiting = false;
            newData.flagURL = HighLogic.CurrentGame.flagURL;
            newData.owned = false;
            newData.vesselType = VesselType.Unknown;

            newData.crew = new List<CrewData>();

            SpawnVessel(newData, crewData);
        }

        private void SpawnVessel(VesselData vesselData, List<ProtoCrewMember> crewData = null)
        {
            Debug.Log("[OrX Spawn] Spawning a vessel named '" + Spawnedname + "'");

            // Set additional info for landed vessels
            bool landed = false;
            if (!vesselData.orbiting)
            {
                landed = true;
                if (vesselData.altitude == null)
                {
                    vesselData.altitude = 35;//LocationUtil.TerrainHeight(vesselData.latitude, vesselData.longitude, vesselData.body);
                }

                //Vector3d pos = vesselData.body.GetWorldSurfacePosition(vesselData.latitude, vesselData.longitude, vesselData.altitude.Value);
                Vector3d pos = vesselData.body.GetRelSurfacePosition(vesselData.latitude, vesselData.longitude, vesselData.altitude.Value);

                vesselData.orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, vesselData.body);
                vesselData.orbit.UpdateFromStateVectors(pos, vesselData.body.getRFrmVel(pos), vesselData.body, Planetarium.GetUniversalTime());
            }
            else
            {
                vesselData.orbit.referenceBody = vesselData.body;
            }

            ConfigNode[] partNodes;
            //UntrackedObjectClass sizeClass;
            ShipConstruct shipConstruct = new ShipConstruct();
            bool hasClamp = false;
            float lcHeight = 0;
            ConfigNode craftNode;
            Quaternion craftRotation = Quaternion.identity;
            if (!string.IsNullOrEmpty(vesselData.craftURL))
            {
                // Save the current ShipConstruction ship, otherwise the player will see the spawned ship next time they enter the VAB!
                ConfigNode currentShip = ShipConstruction.ShipConfig;

                shipConstruct = ShipConstruction.LoadShip(vesselData.craftURL);
                if (shipConstruct == null)
                {
                    Debug.Log("ShipConstruct was null when tried to load '" + vesselData.craftURL +
                      "' (usually this means the file could not be found).");
                    return;//continue;
                }
                craftNode = ConfigNode.Load(vesselData.craftURL);
                lcHeight = ConfigNode.ParseVector3(craftNode.GetNode("PART").GetValue("pos")).y;
                craftRotation = ConfigNode.ParseQuaternion(craftNode.GetNode("PART").GetValue("rot"));

                // Restore ShipConstruction ship
                ShipConstruction.ShipConfig = currentShip;

                // Set the name
                if (string.IsNullOrEmpty(vesselData.name))
                {
                    vesselData.name = Spawnedname;
                }

                // Set some parameters that need to be at the part level
                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                uint launchID = HighLogic.CurrentGame.launchID++;
                foreach (Part p in shipConstruct.parts)
                {
                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                    p.missionID = missionID;
                    p.launchID = launchID;
                    p.flagURL = vesselData.flagURL ?? HighLogic.CurrentGame.flagURL;
                    p.temperature = 1.0;
                }

                Part part = shipConstruct.parts.Find(p => p.protoModuleCrew.Count < p.CrewCapacity);

                if (part != null)
                {
                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                      ? ProtoCrewMember.Gender.Female
                      : ProtoCrewMember.Gender.Male;
                    System.Random r = new System.Random();
                    int trait_ = r.Next(0, 100);
                    if (trait_ <= 33)
                    {
                        crewMember.trait = "Pilot";
                    }
                    else
                    {
                        if (trait_ <= 66)
                        {
                            crewMember.trait = "Engineer";
                        }
                        else
                        {
                            crewMember.trait = "Scientist";
                        }
                    }
                    part.AddCrewmemberAt(crewMember, part.protoModuleCrew.Count);
                }

                ConfigNode empty = new ConfigNode();
                ProtoVessel dummyProto = new ProtoVessel(empty, null);
                Vessel dummyVessel = new Vessel();
                dummyVessel.parts = shipConstruct.parts;
                dummyProto.vesselRef = dummyVessel;

                foreach (Part p in shipConstruct.parts)
                {
                    dummyProto.protoPartSnapshots.Add(new ProtoPartSnapshot(p, dummyProto));
                }
                foreach (ProtoPartSnapshot p in dummyProto.protoPartSnapshots)
                {
                    p.storePartRefs();
                }

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
                ProtoCrewMember[] crewArray = new ProtoCrewMember[vesselData.crew.Count];
                int i = 0;
                foreach (CrewData cd in vesselData.crew)
                {
                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
                    if (cd.name != null)
                    {
                        crewMember.KerbalRef.name = Spawnedname;
                    }

                    crewArray[i++] = crewMember;
                }

                uint flightId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                partNodes = new ConfigNode[1];
                partNodes[0] = ProtoVessel.CreatePartNode(vesselData.craftPart.name, flightId, crewArray);
                if (string.IsNullOrEmpty(vesselData.name))
                {
                    vesselData.name = Spawnedname;
                }
            }

            ConfigNode[] additionalNodes = new ConfigNode[0];
            //DiscoveryLevels discoveryLevel = vesselData.owned ? DiscoveryLevels.Owned : DiscoveryLevels.Unowned;
            //additionalNodes[0] = ProtoVessel.CreateDiscoveryNode(discoveryLevel, sizeClass, contract.TimeDeadline, contract.TimeDeadline);

            // Create the config node representation of the ProtoVessel
            ConfigNode protoVesselNode = ProtoVessel.CreateVesselNode(vesselData.name, vesselData.vesselType, vesselData.orbit, 0, partNodes, additionalNodes);

            // Additional seetings for a landed vessel
            if (!vesselData.orbiting)
            {
                Vector3d norm = vesselData.body.GetRelSurfaceNVector(vesselData.latitude, vesselData.longitude);

                double terrainHeight = 0.0;
                if (vesselData.body.pqsController != null)
                {
                    terrainHeight = vesselData.body.pqsController.GetSurfaceHeight(norm) - vesselData.body.pqsController.radius;
                }
                bool splashed = false;// = landed && terrainHeight < 0.001;

                // Create the config node representation of the ProtoVessel
                // Note - flying is experimental, and so far doesn't worx
                protoVesselNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                  Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                protoVesselNode.SetValue("landed", (landed && !splashed).ToString());
                protoVesselNode.SetValue("splashed", splashed.ToString());
                protoVesselNode.SetValue("lat", vesselData.latitude.ToString());
                protoVesselNode.SetValue("lon", vesselData.longitude.ToString());
                protoVesselNode.SetValue("alt", vesselData.altitude.ToString());
                protoVesselNode.SetValue("landedAt", vesselData.body.name);

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
                    foreach (Collider collider in vesselData.craftPart.partPrefab.GetComponentsInChildren<Collider>())
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
                float heading = vesselData.heading;
                if (shipConstruct == null)
                {
                    rotation = rotation * Quaternion.FromToRotation(Vector3.up, Vector3.back);
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

                    //rotation = craftRotation;


                    vesselData.heading = 0;
                    vesselData.pitch = 0;
                }

                rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
                rotation = rotation * Quaternion.AngleAxis(vesselData.roll, Vector3.down);
                rotation = rotation * Quaternion.AngleAxis(vesselData.pitch, Vector3.left);

                // Set the height and rotation
                if (landed || splashed)
                {
                    float hgt = (shipConstruct != null ? shipConstruct.parts[0] : vesselData.craftPart.partPrefab).localRoot.attPos0.y - lowest;
                    hgt += vesselData.height;

                    if (!hasClamp)
                    {
                        hgt += 35;
                    }
                    else
                    {
                        hgt += lcHeight;
                    }
                    protoVesselNode.SetValue("hgt", hgt.ToString(), true);
                }
                protoVesselNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);

                // Set the normal vector relative to the surface
                Vector3 nrm = (rotation * Vector3.forward);
                protoVesselNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);

                protoVesselNode.SetValue("prst", false.ToString(), true);
            }

            Debug.Log("[OrX Spawn] // Add vessel to the game");

            // Add vessel to the game
            ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
            //protoVessel.vesselRef.transform.rotation = protoVessel.rotation;


            // Store the id for later use
            vesselData.id = protoVessel.vesselRef.id;

            //protoVessel.vesselRef.currentStage = 0;

            StartCoroutine(PlaceSpawnedVessel(protoVessel.vesselRef, !hasClamp));

            // Associate it so that it can be used in contract parameters
            //ContractVesselTracker.Instance.AssociateVessel(vesselData.name, protoVessel.vesselRef);

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
            Debug.Log("[SPAWN OrX] PLACING SPAWNED OrX ...................");
            
            loadingCraft = true;
            v.isPersistent = true;
            v.Landed = true;
            v.situation = Vessel.Situations.LANDED;
            while (v.packed)
            {
                yield return null;
            }
            v.IgnoreGForces(60);
            v.SetWorldVelocity(Vector3d.zero);
            yield return null;

            if (infectedCount <= 1)
            {
                ConfigNode temp = new ConfigNode();
                temp.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/OrX.tmp");
            }
            var orx = v.rootPart.FindModuleImplementing<ModuleOrX>();
            if (orx == null)
            {
                v.rootPart.AddModule("ModuleOrX", true);
            }
            orx = v.rootPart.FindModuleImplementing<ModuleOrX>();
            orx.orx = true;

            v.GoOffRails();
            StageManager.BeginFlight();

            loadingCraft = false;
        }

        public Vector3d _SpawnCoords()
        {
            return FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt);
        }

        public static class LocationUtil
        {
            public static float TerrainHeight(double lat, double lon, CelestialBody body)
            {
                return 0;
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

        private class VesselData
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

            public VesselData() { }
            public VesselData(VesselData vd)
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
    }
}

