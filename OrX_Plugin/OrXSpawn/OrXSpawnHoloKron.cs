using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KSP.UI.Screens;
using System.IO;

namespace OrX.spawn
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class OrXSpawnHoloKron : MonoBehaviour
    {
        public static OrXSpawnHoloKron instance;
        public Vector3 UpVect;
        public Vector3 EastVect;
        public Vector3 NorthVect;
        public double _lat = 0;
        public double _lon = 0;
        public double _alt = 0;
        public Vessel startGate;
        bool dakarRacing = false;
        public bool openingCraftBrowser = false;
        public CraftBrowserDialog craftBrowser;
        public bool spawningGoal = false;
        public int stageCount = 0;
        public bool spawning = false;
        public string _vesselName = "";

        private void Awake()
        {
            if (instance) Destroy(instance);
            instance = this;
        }

        public Vector3d WorldPositionToGeoCoords(Vector3d worldPosition, CelestialBody body)
        {
            if (!body)
            {
                return Vector3d.zero;
            }
            double lat = body.GetLatitude(worldPosition);
            double longi = body.GetLongitude(worldPosition);
            double alt = body.GetAltitude(worldPosition);
            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Lat: " + lat + " - Lon:" + longi + " - Alt: " + alt);
            return new Vector3d(lat, longi, alt);
        }

        public void StartSpawn(Vector3d stageStartCoords, Vector3d vect, bool Goal, bool empty, bool primary, string HoloKronName, string challengeType)
        {
            OrXHoloKron.instance.Reach();

            if (!OrXLog.instance.PREnabled())
            {
                spawning = true;
                StartCoroutine(SpawnHoloKron(stageStartCoords, vect, Goal, empty, primary, HoloKronName, challengeType));
            }
        }

        IEnumerator SpawnHoloKron(Vector3d stageStartCoords, Vector3d vect, bool b, bool empty, bool primary, string HoloKronName, string challengeType)
        {
            int count = OrXHoloKron.instance.locCount + 1;
            yield return new WaitForFixedUpdate();

            string holoFileLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/HoloKron/HoloKron.craft";
            _lat = vect.x;
            _lon = vect.y;
            _alt = vect.z;
            bool spawnGate = false;
            //yield return new WaitForFixedUpdate();

            if (primary)
            {
                _alt += 5;

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Spawning " + HoloKronName + " " + OrXHoloKron.instance.hkCount);
            }
            else
            {
                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Spawning Boid for " + HoloKronName + " " + OrXHoloKron.instance.hkCount);

                if (OrXHoloKron.instance.buildingMission)
                {
                    holoFileLoc = GoalPostCraft;
                    empty = false;
                    spawnGate = true;
                    b = true;
                    _alt += 15;
                }
                else
                {
                    empty = false;
                }
                stageCount += 1;
            }

            Vector3d tpoint;

            if (empty)
            {
                //_alt += 4;
                tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt)
                    + FlightGlobals.ActiveVessel.transform.forward * 3f;
            }
            else
            {
                if (spawnGate)
                {
                    //_alt += 4;
                }
                //_alt += 5;
                tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt);
            }

            Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Altitude: " + gpsPos.z);

            bool landed = false;
            Orbit orbit = null;

            if (!landed)
            {
                landed = true;
                Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Calculating Orbit ================== ");

                orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
            }
            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Orbit Calculated ================== ");

            ConfigNode[] pNodes;
            ShipConstruct shipConstruct = null;
            if (!string.IsNullOrEmpty(holoFileLoc))
            {
                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Loading Ship ================== ");

                ConfigNode currentShip = ShipConstruction.ShipConfig;
                shipConstruct = ShipConstruction.LoadShip(holoFileLoc);
                if (shipConstruct == null)
                {
                    OrXLog.instance.DebugLog("[Spawn OrX HoloKron] ShipConstruct was null when tried to load '" + holoFileLoc +
                      "' (usually this means the file could not be found).");
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Loaded ================== ");

                ShipConstruction.ShipConfig = currentShip;
                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                uint launchID = HighLogic.CurrentGame.launchID++;
                foreach (Part p in shipConstruct.parts)
                {
                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                    p.missionID = missionID;
                    p.launchID = launchID;
                    p.flagURL = "";
                    p.temperature = 1.0;
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct Parts Configured ================== ");

                if (spawnGate)
                {
                    List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                    while (part.MoveNext())
                    {
                        if (part.Current != null)
                        {
                            if (part.Current.Modules.Contains<KerbalSeat>())
                            {
                                ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                  ? ProtoCrewMember.Gender.Female
                                  : ProtoCrewMember.Gender.Male;
                                crewMember.trait = "Tourist";
                                part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                            }
                        }
                    }
                    part.Dispose();
                }

                ConfigNode _tempC = new ConfigNode();
                ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                Vessel _tempV = new Vessel();
                _tempV.parts = shipConstruct.parts;
                _tempP.vesselRef = _tempV;

                foreach (Part p in shipConstruct.parts)
                {
                    _tempV.loaded = false;
                    p.vessel = _tempV;
                    _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct protoPartSnapshots Added ================== ");

                foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                {
                    p.storePartRefs();
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct storePartRefs Stored ================== ");


                List<ConfigNode> pNodesL = new List<ConfigNode>();
                foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                {
                    ConfigNode node = new ConfigNode("PART");
                    snapShot.Save(node);
                    pNodesL.Add(node);
                }

                OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Ship Construct storePartRefs Stored ================== ");

                pNodes = pNodesL.ToArray();
            }
            else
            {
                uint flightId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                pNodes = new ConfigNode[1];
                pNodes[0] = ProtoVessel.CreatePartNode("EMPTY", flightId, null);

                if (string.IsNullOrEmpty(HoloKronName) && !b)
                {
                    HoloKronName = "EMPTY HOLOKRON";
                }
            }

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] CREATING ADDITIONAL NODES FOR " + HoloKronName + " " + OrXHoloKron.instance.hkCount);

            ConfigNode[] additionalNodes = new ConfigNode[0];
            ConfigNode protoNode = ProtoVessel.CreateVesselNode(HoloKronName, VesselType.Base, orbit, 0, pNodes, additionalNodes);

            Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);

            double terrainHeight = 0.0;
            if (FlightGlobals.currentMainBody.pqsController != null)
            {
                terrainHeight = FlightGlobals.currentMainBody.pqsController.GetSurfaceHeight(norm) - FlightGlobals.currentMainBody.pqsController.radius;
                if (terrainHeight <= FlightGlobals.currentMainBody.pqsController.radius)
                {
                    var tHeight = FlightGlobals.currentMainBody.pqsController.radius - terrainHeight;
                    terrainHeight += tHeight;
                }
            }
            bool splashed = false;

            protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
              Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
            protoNode.SetValue("landed", (landed && !splashed).ToString());
            protoNode.SetValue("splashed", splashed.ToString());
            protoNode.SetValue("lat", gpsPos.x.ToString());
            protoNode.SetValue("lon", gpsPos.y.ToString());
            protoNode.SetValue("alt", gpsPos.z.ToString());
            protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Figure out the surface height and rotation for " + HoloKronName + " " + OrXHoloKron.instance.hkCount);
            Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
            Quaternion rotation = Quaternion.identity;
            float heading = 0;
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
            }

            rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

            protoNode.SetValue("hgt", "0", true);
            protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
            Vector3 nrm = (rotation * Vector3.forward);
            protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
            protoNode.SetValue("prst", false.ToString(), true);
            ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

            Vessel holoCube = protoCraft.vesselRef;
            OrXLog.instance.SetRange(holoCube, 10000);
            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] VESSEL RANGES SET FOR " + HoloKronName);

            foreach (Part p in FindObjectsOfType<Part>())
            {
                if (!p.vessel)
                {
                    Destroy(p.gameObject);
                }
            }

            if (!OrXHoloKron.instance.buildingMission)
            {
                foreach (Part p in holoCube.parts)
                {
                    p.SetOpacity(0);
                }
            }


            //yield return new WaitForFixedUpdate();
            holoCube.IgnoreGForces(240);

            holoCube.isPersistent = true;
            holoCube.Landed = false;
            holoCube.situation = Vessel.Situations.FLYING;
            while (holoCube.packed)
            {
                yield return null;
            }
            holoCube.SetWorldVelocity(Vector3d.zero);
            yield return null;
            holoCube.IgnoreGForces(240);
            var mom = holoCube.rootPart.FindModuleImplementing<ModuleOrXMission>();

            if (holoCube.parts.Count == 1 && !spawnGate)
            {
                if (mom == null)
                {
                    holoCube.rootPart.AddModule("ModuleOrXMission", true);
                    mom = holoCube.rootPart.FindModuleImplementing<ModuleOrXMission>();
                }
                mom.HoloKronName = HoloKronName;
                mom.creator = OrXHoloKron.instance.creatorName;
                mom.missionType = OrXHoloKron.instance.missionType;
                mom.challengeType = OrXHoloKron.instance.challengeType;
                mom.latitude = _lat;
                mom.longitude = _lon;
                mom.altitude = _alt;
                mom.pos = tpoint;
            }
            else
            {
                if (mom != null)
                {
                    Destroy(mom);
                }

                if (spawnGate)
                {
                    if (!holoCube.rootPart.Modules.Contains<ModuleOrXStage>())
                    {
                        holoCube.rootPart.AddModule("ModuleOrXStage", true);
                    }
                    var _orxStage = holoCube.rootPart.FindModuleImplementing<ModuleOrXStage>();
                    _orxStage._stageCount = count;
                }
            }


            holoCube.GoOffRails();
            holoCube.IgnoreGForces(240);
            StageManager.BeginFlight();

            if (!b)
            {
                holoCube.IgnoreGForces(240);

                if (!empty)
                {
                    spawning = false;
                    OrXHoloKron.instance.checking = true;
                    if (!OrXHoloKron.instance._showTimer)
                    {
                        OrXHoloKron.instance.movingCraft = false;
                        OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                    }

                    if (!primary)
                    {
                        mom.Goal = true;
                        mom.stage = count;
                        OrXHoloKron.instance.targetCoord = holoCube;
                    }
                    else
                    {
                        if (OrXHoloKron.instance.missionType == "GEO-CACHE")
                        {
                            OrXHoloKron.instance.challengeRunning = false;
                            StartCoroutine(SpawnLocalVessels(false, HoloKronName, vect));
                        }
                        else
                        {
                            if (OrXHoloKron.instance.challengeType == "BD ARMORY")
                            {
                                mom.fml = true;
                                mom.Goal = true;
                                OrXHoloKron.instance.SetBDAc();
                                StartCoroutine(SpawnLocalVessels(true, HoloKronName, vect));
                            }
                        }
                    }
                }
                else
                {
                    spawning = false;

                    OrXHoloKron.instance.GetShortTrackCenter(OrXHoloKron.instance._challengeStartLoc);
                    OrXHoloKron.instance._HoloKron = holoCube;
                    OrXHoloKron.instance.PlayOrXMission = false;
                    OrXHoloKron.instance.movingCraft = false;
                    OrXVesselMove.Instance.StartMove(holoCube, false, 0, false, false);
                }
            }
            else
            {
                if (holoCube.rootPart.Modules.Contains<ModuleOrXMission>())
                {
                    mom.Goal = true;
                    mom.stage = stageCount;
                    mom.isLoaded = true;
                    OrXHoloKron.instance.targetCoord = holoCube;
                }

                if (OrXHoloKron.instance.buildingMission && !holoCube.rootPart.Modules.Contains<ModuleOrXMission>())
                {
                    holoCube.vesselName = OrXHoloKron.instance.HoloKronName + " " + OrXHoloKron.instance.hkCount + " - STAGE " + count;
                    OrXLog.instance.DebugLog("[OrX Spawn OrX HoloKron] === FIXING ROTATION FOR " + holoCube.vesselName + " ..................");
                    Quaternion _fixRot = Quaternion.identity;
                    holoCube.IgnoreGForces(240);
                    holoCube.angularVelocity = Vector3.zero;
                    holoCube.angularMomentum = Vector3.zero;
                    holoCube.SetWorldVelocity(Vector3d.zero);
                    Vector3 _startPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)OrXHoloKron.instance.lastCoord.x, (double)OrXHoloKron.instance.lastCoord.y, (double)holoCube.altitude);
                    Vector3 _goalPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)holoCube.latitude, (double)holoCube.longitude, (double)holoCube.altitude);
                    Vector3 startPosDirection = (_goalPos - _startPos).normalized;
                    _fixRot = Quaternion.FromToRotation(holoCube.transform.up, startPosDirection) * holoCube.ReferenceTransform.rotation;
                    holoCube.SetRotation(_fixRot, true);
                    OrXHoloKron.instance.getNextCoord = true;
                    OrXHoloKron.instance.spawningStartGate = true;
                    OrXHoloKron.instance._lastStage = holoCube;
                    OrXVesselMove.Instance.StartMove(holoCube, false, 0, true, false);
                    spawning = false;
                }
                else
                {
                    OrXHoloKron.instance.checking = true;
                    if (!OrXHoloKron.instance._showTimer)
                    {
                        OrXHoloKron.instance.movingCraft = false;
                        OrXHoloKron.instance.GuiEnabledOrXMissions = false;
                    }
                }
            }

            if (holoCube.rootPart.Modules.Contains<ModuleOrXMission>())
            {
                mom.isLoaded = true;
            }

            spawning = false;
            //OrXHoloKron.instance.challengeRunning = false;
            //OrXHoloKron.instance.GuiEnabledOrXMissions = true;

        }

        public void SpawnOrx()
        {
            string _orxCraft = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/OrX/FOrX.craft";
        }
        public void SpawnLocal(bool _bda, string HoloKronName, Vector3d vect)
        {
            if (!OrXLog.instance.PREnabled())
            {
                spawning = true;
                OrXHoloKron.instance.Reach();
                StartCoroutine(SpawnLocalVessels(_bda, HoloKronName, vect));
            }
        }
        IEnumerator SpawnLocalVessels(bool _bda, string HoloKronName, Vector3d vect)
        {
            string missionCraftLoc = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp";
            string pas = "";
            string _orxFileLoc = "";
            float _altToSubtract = 0;
            UpVect = (FlightGlobals.ActiveVessel.ReferenceTransform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            ConfigNode _file = new ConfigNode();
            string _orxFile = "";

            string importLoc = UrlDir.ApplicationRootPath + "GameData/OrX/HoloKron/";
            List<string> files = new List<string>(Directory.GetFiles(importLoc, "*.orx", SearchOption.AllDirectories));
            if (files != null)
            {
                List<string>.Enumerator orxFile = files.GetEnumerator();
                while(orxFile.MoveNext())
                {
                    if (orxFile.Current != null)
                    {
                        if (orxFile.Current.Contains(HoloKronName + "-" + OrXHoloKron.instance.hkCount + "-" + OrXHoloKron.instance.creatorName))
                        {
                            _orxFileLoc = orxFile.Current;
                            _file = ConfigNode.Load(orxFile.Current);
                            break;
                        }
                    }
                }
                orxFile.Dispose();
            }

            if (_file != null)
            {
                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === Spawning Local Vessels === ");

                if (HoloKronName != "")
                {
                    int _vesselCount = 1;
                    int _hkCount = 0;
                    ConfigNode node = _file.GetNode("OrX");


                    foreach (ConfigNode spawnCheck in node.nodes)
                    {
                        if (spawnCheck.name.Contains("OrXHoloKronCoords" + _hkCount))
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === FOUND " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " ... DECRYPTING ===");

                            foreach (ConfigNode.Value data in spawnCheck.values)
                            {
                                if (data.name == "challengeType")
                                {
                                    if (data.value == "OUTLAW RACING")
                                    {
                                        OrXHoloKron.instance.outlawRacing = true;
                                    }

                                    if (data.value == "BD ARMORY")
                                    {
                                        OrXHoloKron.instance.bdaChallenge = true;
                                        OrXHoloKron.instance.geoCache = false;
                                        OrXHoloKron.instance.outlawRacing = false;

                                    }

                                    if (data.value == "GEO-CACHE")
                                    {
                                        OrXHoloKron.instance.geoCache = true;
                                        OrXHoloKron.instance.bdaChallenge = false;
                                        OrXHoloKron.instance.outlawRacing = false;

                                    }
                                }

                                if (data.name == "spawned")
                                {
                                    if (data.value == "False")
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] ===  " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has not spawned ===");
                                        spawnCheck.SetValue("spawned", "True", true);
                                        _file.Save(_orxFileLoc);
                                        break;
                                    }
                                    else
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has spawned ... CHECKING FOR EXTRAS");

                                        if (spawnCheck.HasValue("extras"))
                                        {
                                            if (spawnCheck.GetValue("extras") == "False")
                                            {
                                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has no extras ... END TRANSMISSION");
                                                break;
                                            }
                                            else
                                            {
                                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + HoloKronName + " " + _hkCount + "-" + OrXHoloKron.instance.creatorName + " has extras ... SEARCHING");
                                                _hkCount += 1;
                                            }
                                        }
                                    }
                                }
                            }

                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === DATA PROCESSED ===");
                        }
                    }

                    //OrXHoloKron.instance._blink = true;
                    //OrXHoloKron.instance.OnScrnMsgUC("Placing");

                    foreach (ConfigNode _vts in node.nodes)
                    {
                        if (_vts.name.Contains("HC" + _hkCount + "OrXv" + _vesselCount))
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === GRABBING CRAFT FILE FOR " + _vts.name + " ===");
                            _vesselCount += 1;
                            float _left = 0;
                            float _pitch = 0;
                            double _al = 0;
                            double _la = 0;
                            double _lo = 0;
                            int _serial = 1138;

                            _vesselName = _vts.GetValue("vesselName");

                            ConfigNode location = _vts.GetNode("coords");

                            foreach (ConfigNode.Value loc in location.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(loc.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(loc.value);
                                loc.name = cvEncryptedName;
                                loc.value = cvEncryptedValue;
                                if (loc.name == "vesselName")
                                {
                                    _vesselName = loc.value;
                                }
                                if (loc.name == "lat")
                                {
                                    _la = double.Parse(loc.value);
                                }
                                if (loc.name == "lon")
                                {
                                    _lo = double.Parse(loc.value);
                                }
                                if (loc.name == "alt")
                                {
                                    _al = double.Parse(loc.value);
                                    _al += 5;
                                }
                                if (loc.name == "left")
                                {
                                    _left = float.Parse(loc.value);
                                }
                                if (loc.name == "pitch")
                                {
                                    _pitch = float.Parse(loc.value);
                                }
                                if (loc.name == "pas")
                                {
                                    pas = loc.value;
                                }
                            }

                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === VESSEL SPAWN COORDS READY ===");

                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === DECRYPTING CRAFT FILE DATA FOR " + _vts.name + " ===");
                            ConfigNode craftFile = _vts.GetNode("craft");

                            foreach (ConfigNode.Value cv in craftFile.values)
                            {
                                string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                                string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);
                                cv.name = cvEncryptedName;
                                cv.value = cvEncryptedValue;

                                if (cv.name == "ship")
                                {
                                    cv.value = _vesselName;
                                }

                                if (cv.name == "size")
                                {
                                    string[] _sizeString = cv.value.Split(new char[] { ',' });

                                    try
                                    {
                                        if (_sizeString[0] != null && _sizeString[0].Length > 0 && _sizeString[0] != "null")
                                        {
                                            for (int i = 0; i < _sizeString.Length; i++)
                                            {
                                                if (_sizeString[i] != null && _sizeString[i].Length > 0)
                                                {
                                                    float _maxValue = Math.Max(float.Parse(_sizeString[0]), Math.Max(float.Parse(_sizeString[1]), float.Parse(_sizeString[2])));
                                                    _altToSubtract = _maxValue / 2;
                                                }
                                            }
                                        }
                                    }
                                    catch (IndexOutOfRangeException e)
                                    {
                                        OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Altitude calculated ...... ");
                                    }
                                }
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
                                        if (cv2.name != "currentRotation")
                                        {
                                            string cvEncryptedName = OrXLog.instance.Decrypt(cv2.name);
                                            string cvEncryptedValue = OrXLog.instance.Decrypt(cv2.value);
                                            cv2.name = cvEncryptedName;
                                            cv2.value = cvEncryptedValue;
                                        }
                                    }
                                }
                            }
                            yield return new WaitForFixedUpdate();

                            craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                            yield return new WaitForFixedUpdate();

                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === VESSEL DECRYPTED - CHECKING MODULES ===");
                            List<string> partsLoaded = new List<string>();
                            int partCount = 0;
                            bool okToLoad = true;
                            bool hasKerbal = false;
                            spawningGoal = false;

                            foreach (ConfigNode partCheck in craftFile.nodes)
                            {
                                if (node.name == "PART")
                                {
                                    foreach (ConfigNode.Value partCheck2 in partCheck.values)
                                    {
                                        if (partCheck2.name == "name")
                                        {
                                            if (partCheck2.value.Contains("kerbalEVA"))
                                            {
                                                hasKerbal = true;
                                            }

                                            if (partCheck2.value.Contains("ModuleOrXStage"))
                                            {
                                                spawningGoal = true;
                                            }
                                            /*
                                            if (!partsLoaded.Contains(partCheck2.value))
                                            {
                                                partCount += 1;
                                                partsLoaded.Add(partCheck2.value);
                                            }
                                            */
                                        }
                                    }
                                }
                            }

                            /*
                            List<AvailablePart>.Enumerator availablePart = PartLoader.LoadedPartsList.GetEnumerator();
                            while (availablePart.MoveNext())
                            {
                                try
                                {
                                    if (availablePart.Current != null)
                                    {
                                        if (partsLoaded.Contains(availablePart.Current.name))
                                        {
                                            partCount -= 1;
                                            partsLoaded.Remove(availablePart.Current.name);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + e + " ===");
                                }
                            }
                            availablePart.Dispose();

                            if (partCount >= 0)
                            {
                                foreach (string s in partsLoaded)
                                {
                                    okToLoad = false;
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + _vts.name + " CONTAINS UNRECOGNIZED PARTS ... SKIPPING ===");
                                    OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + s + " UNRECOGNIZED ===");
                                }
                            }

                            */

                            if (okToLoad && !hasKerbal || spawningGoal)
                            {
                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + _vesselName + " READY FOR SPAWNING ===");
                                //yield return new WaitForFixedUpdate();

                                VesselType vt;

                                if (spawningGoal)
                                {
                                    vt = VesselType.Debris;
                                }
                                else
                                {
                                    vt = VesselType.Ship;
                                }

                                //craftFile.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/spawn.tmp");
                                yield return new WaitForFixedUpdate();

                                Vector3d tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_la, (double)_lo, (double)_al + (_altToSubtract * 3));
                                Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);
                                Orbit orbit = null;

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Altitude: " + gpsPos.z);

                                bool landed = false;
                                if (!landed)
                                {
                                    landed = true;

                                    Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                                    orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                                    orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
                                }

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Orbit Data Processed");
                                yield return new WaitForFixedUpdate();

                                ConfigNode[] partNodes;
                                ShipConstruct shipConstruct = null;

                                ConfigNode currentShip = ShipConstruction.ShipConfig;
                                shipConstruct = ShipConstruction.LoadShip("GameData/OrX/Plugin/PluginData/spawn.tmp");
                                ShipConstruction.ShipConfig = currentShip;
                                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                                uint launchID = HighLogic.CurrentGame.launchID++;

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Ship construct created");

                                foreach (Part p in shipConstruct.parts)
                                {
                                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                                    p.missionID = missionID;
                                    p.launchID = launchID;
                                    p.flagURL = "";
                                    p.temperature = 1.0;
                                }
                                //yield return new WaitForFixedUpdate();

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Part flight ID's processed");
                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Constructing protoCraft");

                                bool hasSeat = false;

                                List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                                while (part.MoveNext())
                                {
                                    if (part.Current != null)
                                    {
                                        part.Current.SetOpacity(0);

                                        if (part.Current.Modules.Contains<KerbalSeat>())
                                        {
                                            hasSeat = true;
                                            /*
                                            ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                            crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                              ? ProtoCrewMember.Gender.Female
                                              : ProtoCrewMember.Gender.Male;
                                            _serial = new System.Random().Next(1000, 9999);
                                            crewMember.trait = "Pilot";
                                            //crewMember.KerbalRef.crewMemberName = "TK - " + _serial;
                                            part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                            //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                            //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
                                            */
                                        }
                                        else
                                        {
                                            if (part.Current.Modules.Contains<KerbalEVA>())
                                            {
                                                ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                  ? ProtoCrewMember.Gender.Female
                                                  : ProtoCrewMember.Gender.Male;
                                                _serial = new System.Random().Next(1000, 9999);
                                                crewMember.trait = "Pilot";
                                                //crewMember.KerbalRef.crewMemberName = "TK - " + _serial;
                                                part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));

                                                if (!part.Current.Modules.Contains<ModuleOrX>())
                                                {
                                                    part.Current.AddModule("ModuleOrX");
                                                }
                                            }
                                            else
                                            {
                                                if (part.Current.Modules.Contains<ModuleCommand>() && part.Current.protoModuleCrew.Count <= 1)
                                                {
                                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                                      ? ProtoCrewMember.Gender.Female
                                                      : ProtoCrewMember.Gender.Male;
                                                    _serial = new System.Random().Next(1000, 9999);
                                                    crewMember.trait = "Pilot";
                                                    //crewMember.KerbalRef.crewMemberName = "TK - " + _serial;
                                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                                    //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                    //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
                                                }
                                            }
                                        }
                                    }
                                }
                                part.Dispose();

                                ConfigNode _tempC = new ConfigNode();
                                ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                                Vessel _tempV = new Vessel();
                                _tempV.parts = shipConstruct.parts;
                                _tempP.vesselRef = _tempV;

                                foreach (Part p in shipConstruct.parts)
                                {
                                    _tempV.loaded = false;
                                    p.vessel = _tempV;
                                    _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                                }

                                foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                                {
                                    p.storePartRefs();
                                }

                                List<ConfigNode> partNodesL = new List<ConfigNode>();
                                foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                                {
                                    ConfigNode partNode = new ConfigNode("PART");
                                    snapShot.Save(partNode);
                                    partNodesL.Add(partNode);
                                }
                                partNodes = partNodesL.ToArray();

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] CREATING ADDITIONAL NODES FOR " + HoloKronName);
                                ConfigNode[] additionalNodes = new ConfigNode[0];
                                ConfigNode protoNode = ProtoVessel.CreateVesselNode(HoloKronName, vt, orbit, 0, partNodes, additionalNodes);
                                bool splashed = false;
                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] FINDING NORTH FOR " + HoloKronName);

                                Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);
                                splashed = false;
                                protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
                                  Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
                                protoNode.SetValue("landed", (landed && !splashed).ToString());
                                protoNode.SetValue("splashed", splashed.ToString());
                                protoNode.SetValue("lat", gpsPos.x.ToString());
                                protoNode.SetValue("lon", gpsPos.y.ToString());
                                protoNode.SetValue("alt", gpsPos.z.ToString());
                                protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] protoCraft SET VALUES FOR " + HoloKronName);

                                Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
                                Quaternion rotation = Quaternion.identity;
                                float heading = 0;
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
                                }

                                rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
                                rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
                                rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] INITIALIZING ROTATIONS FOR " + HoloKronName);

                                protoNode.SetValue("hgt", "0", true);
                                protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
                                Vector3 nrm = (rotation * Vector3.forward);
                                protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
                                protoNode.SetValue("prst", false.ToString(), true);

                                //protoNode.Save(UrlDir.ApplicationRootPath + "GameData/OrX/Export/" + HoloKronName + "Vessel" + _vesselCount + ".proto");
                                /*
                                foreach (ConfigNode cn in protoNode.nodes)
                                {
                                    if (cn.name == "PART")
                                    {
                                        foreach (ConfigNode.Value cv in cn.values)
                                        {
                                            if (cv.name == "part")
                                            {
                                                if (cv.value.Contains("kerbalEVA"))
                                                {
                                                    string tkName = "";
                                                    string currentName = "";

                                                    foreach (ConfigNode cn2 in cn.nodes)
                                                    {
                                                        if (cn2.name == "MODULE")
                                                        {
                                                            foreach (ConfigNode.Value cv2 in cn2.values)
                                                            {
                                                                if (cv2.name == "name")
                                                                {
                                                                    if (cv2.value == "KerbalEVA")
                                                                    {
                                                                        //ConfigNode vInfo = cn2.GetNode("vInfo");
                                                                        //_serial = new System.Random().Next(1000, 9999);
                                                                        //tkName = "TK - " + _serial;
                                                                        //currentName = vInfo.GetValue("vesselName");
                                                                       // vInfo.SetValue("vesselName", "TK - " + _serial, true);

                                                                        //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                                                        //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
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
                                */
                                ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

                                foreach (Part p in protoCraft.vesselRef.parts)
                                {
                                    //p.SetOpacity(0);
                                }
                                protoCraft.vesselRef.transform.rotation = protoCraft.rotation;

                                Vessel localVessel = protoCraft.vesselRef;
                                OrXLog.instance.SetRange(localVessel, 10000);

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] VESSEL RANGES SET FOR " + localVessel.vesselName);

                                foreach (Part p in FindObjectsOfType<Part>())
                                {
                                    if (!p.vessel)
                                    {
                                        Destroy(p.gameObject);
                                    }
                                }

                                localVessel.isPersistent = true;
                                localVessel.Landed = false;
                                localVessel.situation = Vessel.Situations.FLYING;
                                while (localVessel.packed)
                                {
                                    yield return null;
                                }

                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] " + HoloKronName + " IS GOING OFF RAILS");
                                ScreenMessages.PostScreenMessage(new ScreenMessage("Placing " + _vesselName, 1, ScreenMessageStyle.UPPER_CENTER));

                                localVessel.SetWorldVelocity(Vector3d.zero);
                                localVessel.GoOffRails();
                                localVessel.IgnoreGForces(240);

                                StageManager.BeginFlight();
                                localVessel.rootPart.AddModule("ModuleOrXPlace", true);
                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === FIXING ROTATION FOR " + _vts.name + " ===");

                                if (_bda)
                                {

                                }

                                var _place = localVessel.rootPart.FindModuleImplementing<ModuleOrXPlace>();
                                _place.PlaceCraft(localVessel.rootPart.Modules.Contains<ModuleOrXStage>(),false, _altToSubtract, _left, _pitch);
                                yield return new WaitForFixedUpdate();

                                ConfigNode craft = ConfigNode.Load(missionCraftLoc);
                                craft.ClearData();
                                craft.Save(missionCraftLoc);
                                OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] === " + _vts.name + " Spawned ===");
                            }
                        }
                    }
                    OrXHoloKron.instance.movingCraft = false;
                    spawning = false;
                }
            }
        }

        string GoalPostCraft = UrlDir.ApplicationRootPath + "GameData/OrX/Plugin/PluginData/VesselData/Goal/Goal.craft";

        public void CraftSelect(bool _spawningBarrier)
        {
            StartCoroutine(StartVesselSelectRoutine(_spawningBarrier));
        }
        public IEnumerator StartVesselSelectRoutine(bool _spawningBarrier)
        {
            string _dir = "";
            if (_spawningBarrier)
            {
                OrXLog.instance.DebugLog("[OrX Craft Select] CRAFT FOLDER: " + UrlDir.ApplicationRootPath + "GameData/OrX/Parts/Base/");

                _dir = UrlDir.ApplicationRootPath + "GameData/OrX/Parts/Base/";
            }
            else
            {
                OrXLog.instance.DebugLog("[OrX Craft Select] CRAFT FOLDER: " + HighLogic.SaveFolder);

                _dir = HighLogic.SaveFolder;
            }
            OrXLog.instance.DebugLog("[OrX Craft Select] Start craft selection");
            OrXHoloKron.instance.OrXHCGUIEnabled = false;
            OrXHoloKron.instance.movingCraft = true;
            OrXHoloKron.instance.openingCraftBrowser = true;
            yield return null;
            craftBrowser = CraftBrowserDialog.Spawn(EditorFacility.SPH, _dir, OnSelected, OnCancelled, false);
        }
        public void OnSelected(string _selectedCraftFile, CraftBrowserDialog.LoadType loadType)
        {
            craftBrowser = null;
            openingCraftBrowser = false;
            OrXLog.instance.DebugLog("[OrX Craft Select] Selected Craft: " + _selectedCraftFile);

            if (spawningGoal)
            {
                OrXLog.instance.DebugLog("[OrX Craft Select] Start goal spawn");

                //Vector3d gpsPos = WorldPositionToGeoCoords(new Vector3d(_HoloKron.latitude, _HoloKron.longitude, _HoloKron.altitude), FlightGlobals.currentMainBody);
                //OrXSpawnHoloKron.instance.SpawnStartingGate();
            }
            else
            {
                if (!_spawnCraftFile)
                {
                    if (OrXHoloKron.instance.addingBluePrints)
                    {
                        ConfigNode _craftFile = ConfigNode.Load(_selectedCraftFile);
                        OrXLog.instance.DebugLog("[OrX Craft Select] SAVING BLUEPRINTS TO HOLOCACHE ............");

                        foreach (ConfigNode.Value cv in _craftFile.values)
                        {
                            if (cv.name == "ship")
                            {
                                OrXHoloKron.instance.craftToAddMission = cv.value;
                                break;
                            }

                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        foreach (ConfigNode cn in _craftFile.nodes)
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
                        OrXHoloKron.instance.addingBluePrints = false;
                        OrXHoloKron.instance.blueprintsAdded = true;
                        OrXHoloKron.instance.blueprintsLabel = OrXHoloKron.instance.craftToAddMission;
                        OrXHoloKron.instance.spawningGoal = false;
                        OrXHoloKron.instance.blueprintsFile = _selectedCraftFile;
                        OrXHoloKron.instance.addingBluePrints = false;
                        OrXHoloKron.instance.movingCraft = false;
                        OrXHoloKron.instance.OrXHCGUIEnabled = true;
                        OrXHoloKron.instance.OnScrnMsgUC(OrXHoloKron.instance.craftToAddMission + " Saved to HoloKron ....");
                        OrXHoloKron.instance.GuiEnabledOrXMissions = true;
                    }
                }
                else
                {
                    _spawnCraftFile = false;
                    StartCoroutine(SpawnFromCraftFile(_selectedCraftFile, OrXHoloKron.instance._addCrew));
                }
            }
        }
        public void OnCancelled()
        {
            OrXLog.instance.DebugLog("[OrX Craft Select] Cancelling Select Craft ............");

            OrXHoloKron.instance.GuiEnabledOrXMissions = true;
            OrXHoloKron.instance.movingCraft = false;
            OrXHoloKron.instance.spawningGoal = false;
            OrXHoloKron.instance.craftBrowser = null;
            OrXHoloKron.instance.openingCraftBrowser = false;
            OrXHoloKron.instance.OrXHCGUIEnabled = true;
        }

        public bool _spawnCraftFile = false;

        IEnumerator SpawnFromCraftFile(string _craftFile, bool _addCrew)
        {
            OrXHoloKron.instance.Reach();
            OrXHoloKron.instance.OrXHCGUIEnabled = true;
            OrXHoloKron.instance.GetShortTrackCenter(OrXHoloKron.instance._challengeStartLoc);
            string _name = "";
            float _altToAdd = 15;

            ConfigNode vesselToLoad = ConfigNode.Load(_craftFile);
            if (vesselToLoad != null)
            {
                _name = vesselToLoad.GetValue("ship");
                foreach (ConfigNode.Value cv in vesselToLoad.values)
                {
                    if (cv.name == "ship")
                    {
                        _name = cv.value;
                    }

                    if (cv.name == "size")
                    {
                        string[] _sizeString = cv.value.Split(new char[] { ',' });

                        try
                        {
                            if (_sizeString[0] != null && _sizeString[0].Length > 0 && _sizeString[0] != "null")
                            {
                                for (int i = 0; i < _sizeString.Length; i++)
                                {
                                    if (_sizeString[i] != null && _sizeString[i].Length > 0)
                                    {
                                        float _maxValue = Math.Max(float.Parse(_sizeString[0]), Math.Max(float.Parse(_sizeString[1]), float.Parse(_sizeString[2])));
                                        _altToAdd += _maxValue / 2;
                                    }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            OrXLog.instance.DebugLog("[OrX Spawn Local Vessels] Altitude calculated ...... ");
                        }
                    }
                }
            }

            _lat = FlightGlobals.ActiveVessel.latitude;
            _lon = FlightGlobals.ActiveVessel.longitude;
            _alt = FlightGlobals.ActiveVessel.altitude + _altToAdd;

            Vector3d tpoint = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition((double)_lat, (double)_lon, (double)_alt) + FlightGlobals.ActiveVessel.transform.forward * 5f;
            Vector3 gpsPos = WorldPositionToGeoCoords(tpoint, FlightGlobals.currentMainBody);

            OrXLog.instance.DebugLog("[Spawn OrX Craft File] Altitude: " + gpsPos.z);

            bool landed = false;
            Orbit orbit = null;

            if (!landed)
            {
                landed = true;
                Vector3d pos = FlightGlobals.currentMainBody.GetRelSurfacePosition(gpsPos.x, gpsPos.y, gpsPos.z);
                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Calculating Orbit ================== ");

                orbit = new Orbit(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, FlightGlobals.currentMainBody);
                orbit.UpdateFromStateVectors(pos, FlightGlobals.currentMainBody.getRFrmVel(pos), FlightGlobals.currentMainBody, Planetarium.GetUniversalTime());
            }
            OrXLog.instance.DebugLog("[Spawn OrX Craft File] Orbit Calculated ================== ");

            ConfigNode[] pNodes;
            ShipConstruct shipConstruct = null;
            if (!string.IsNullOrEmpty(_craftFile))
            {
                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Loading Ship ================== ");

                ConfigNode currentShip = ShipConstruction.ShipConfig;
                shipConstruct = ShipConstruction.LoadShip(_craftFile);
                if (shipConstruct == null)
                {
                    OrXLog.instance.DebugLog("[Spawn OrX HoloKron] ShipConstruct was null when tried to load '" + _craftFile +
                      "' (usually this means the file could not be found).");
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Loaded ================== ");

                ShipConstruction.ShipConfig = currentShip;
                uint missionID = (uint)Guid.NewGuid().GetHashCode();
                uint launchID = HighLogic.CurrentGame.launchID++;
                foreach (Part p in shipConstruct.parts)
                {
                    p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                    p.missionID = missionID;
                    p.launchID = launchID;
                    p.flagURL = "";
                    p.temperature = 1.0;
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct Parts Configured ================== ");

                if (_addCrew)
                {
                    List<Part>.Enumerator part = shipConstruct.parts.GetEnumerator();
                    while (part.MoveNext())
                    {
                        if (part.Current != null)
                        {
                            if (part.Current.Modules.Contains<KerbalSeat>())
                            {
                                ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                  ? ProtoCrewMember.Gender.Female
                                  : ProtoCrewMember.Gender.Male;
                                crewMember.trait = "Tourist";
                                part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                            }
                            else
                            {
                                if (part.Current.Modules.Contains<ModuleCommand>() && part.Current.protoModuleCrew.Count <= 1)
                                {
                                    ProtoCrewMember crewMember = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                                    crewMember.gender = UnityEngine.Random.Range(0, 100) > 50
                                      ? ProtoCrewMember.Gender.Female
                                      : ProtoCrewMember.Gender.Male;
                                    crewMember.trait = "Pilot";
                                    //crewMember.KerbalRef.crewMemberName = "TK - " + _serial;
                                    part.Current.AddCrewmemberAt(crewMember, part.Current.protoModuleCrew.Count);
                                    //OrXLog.instance.DebugLog("[OrX Klone Activation] === OrX Klone " + "TK - " + _serial + " coming online ===");
                                    //ScreenMessages.PostScreenMessage(new ScreenMessage("OrX Klone " + "TK - " + _serial + " coming online", 1, ScreenMessageStyle.UPPER_CENTER));
                                }
                            }
                        }
                    }
                    part.Dispose();
                }

                ConfigNode _tempC = new ConfigNode();
                ProtoVessel _tempP = new ProtoVessel(_tempC, null);
                Vessel _tempV = new Vessel();
                _tempV.parts = shipConstruct.parts;
                _tempP.vesselRef = _tempV;

                foreach (Part p in shipConstruct.parts)
                {
                    _tempV.loaded = false;
                    p.vessel = _tempV;
                    _tempP.protoPartSnapshots.Add(new ProtoPartSnapshot(p, _tempP));
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct protoPartSnapshots Added ================== ");

                foreach (ProtoPartSnapshot p in _tempP.protoPartSnapshots)
                {
                    p.storePartRefs();
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct storePartRefs Stored ================== ");


                List<ConfigNode> pNodesL = new List<ConfigNode>();
                foreach (ProtoPartSnapshot snapShot in _tempP.protoPartSnapshots)
                {
                    ConfigNode node = new ConfigNode("PART");
                    snapShot.Save(node);
                    pNodesL.Add(node);
                }

                OrXLog.instance.DebugLog("[Spawn OrX Craft File] Ship Construct storePartRefs Stored ================== ");

                pNodes = pNodesL.ToArray();
            }
            else
            {
                uint flightId = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                pNodes = new ConfigNode[1];
                pNodes[0] = ProtoVessel.CreatePartNode("EMPTY", flightId, null);

                if (string.IsNullOrEmpty(_name))
                {
                    _name = "THX " + new System.Random().Next(1000, 9999);
                }
            }

            OrXLog.instance.DebugLog("[Spawn OrX Craft File] CREATING ADDITIONAL NODES FOR " + _name);

            ConfigNode[] additionalNodes = new ConfigNode[0];
            ConfigNode protoNode = ProtoVessel.CreateVesselNode(_name, VesselType.Ship, orbit, 0, pNodes, additionalNodes);

            Vector3d norm = FlightGlobals.currentMainBody.GetRelSurfaceNVector(gpsPos.x, gpsPos.y);

            double terrainHeight = 0.0;
            if (FlightGlobals.currentMainBody.pqsController != null)
            {
                terrainHeight = FlightGlobals.currentMainBody.pqsController.GetSurfaceHeight(norm) - FlightGlobals.currentMainBody.pqsController.radius;
                if (terrainHeight <= FlightGlobals.currentMainBody.pqsController.radius)
                {
                    var tHeight = FlightGlobals.currentMainBody.pqsController.radius - terrainHeight;
                    terrainHeight += tHeight;
                }
            }
            bool splashed = false;

            protoNode.SetValue("sit", (splashed ? Vessel.Situations.SPLASHED : landed ?
              Vessel.Situations.LANDED : Vessel.Situations.FLYING).ToString());
            protoNode.SetValue("landed", (landed && !splashed).ToString());
            protoNode.SetValue("splashed", splashed.ToString());
            protoNode.SetValue("lat", gpsPos.x.ToString());
            protoNode.SetValue("lon", gpsPos.y.ToString());
            protoNode.SetValue("alt", gpsPos.z.ToString());
            protoNode.SetValue("landedAt", FlightGlobals.currentMainBody.name);

            OrXLog.instance.DebugLog("[Spawn OrX HoloKron] Figure out the surface height and rotation for " + _name);
            Quaternion normal = Quaternion.LookRotation((Vector3)norm);// new Vector3((float)norm.x, (float)norm.y, (float)norm.z));
            Quaternion rotation = Quaternion.identity;
            float heading = 0;
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
            }

            rotation = rotation * Quaternion.AngleAxis(heading, Vector3.back);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.down);
            rotation = rotation * Quaternion.AngleAxis(0, Vector3.left);

            protoNode.SetValue("hgt", "0", true);
            protoNode.SetValue("rot", KSPUtil.WriteQuaternion(normal * rotation), true);
            Vector3 nrm = (rotation * Vector3.forward);
            protoNode.SetValue("nrm", nrm.x + "," + nrm.y + "," + nrm.z, true);
            protoNode.SetValue("prst", false.ToString(), true);
            ProtoVessel protoCraft = HighLogic.CurrentGame.AddVessel(protoNode);

            Vessel craftFromFile = protoCraft.vesselRef;
            OrXLog.instance.SetRange(craftFromFile, 10000);

            OrXLog.instance.DebugLog("[Spawn OrX Craft File] VESSEL RANGES SET FOR " + _name);

            foreach (Part p in FindObjectsOfType<Part>())
            {
                if (!p.vessel)
                {
                    Destroy(p.gameObject);
                }
            }

            if (!OrXHoloKron.instance.buildingMission)
            {
                foreach (Part p in craftFromFile.parts)
                {
                    p.SetOpacity(0);
                }
            }

            //yield return new WaitForFixedUpdate();
            craftFromFile.IgnoreGForces(240);
            craftFromFile.isPersistent = true;
            craftFromFile.Landed = false;
            craftFromFile.situation = Vessel.Situations.FLYING;
            while (craftFromFile.packed)
            {
                yield return null;
            }
            craftFromFile.SetWorldVelocity(Vector3d.zero);
            yield return null;
            craftFromFile.IgnoreGForces(240);

            craftFromFile.GoOffRails();
            craftFromFile.IgnoreGForces(240);
            StageManager.BeginFlight();
            spawning = false;
            OrXVesselMove.Instance.StartMove(craftFromFile, false, _altToAdd, false, false);
            OrXHoloKron.instance.getNextCoord = true;
            spawning = false;
        }
    }
}