using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using KSP.UI.Screens;
using OrX.spawn;

namespace OrX
{
    public class ModuleOrXHoloCache : PartModule
    {

        #region Fields

        private bool pilot = false;
        private bool engineer = false;
        private bool scientist = false;
        private bool civilian = false;

        private string craftLoc = "";

        [KSPField(isPersistant = true)]
        private string craftToAdd = "";
        [KSPField(isPersistant = true)]
        public bool getGPS = true;
        [KSPField(isPersistant = true)]
        public bool setup = false;
        [KSPField(isPersistant = true)]
        public bool isSetup = false;
        [KSPField(isPersistant = true)]
        public bool save = false;

        [KSPField(isPersistant = true)]
        public float infectedCount = 1;
        [KSPField(isPersistant = true)]
        public bool spawnInfected = false;

        [KSPField(isPersistant = true)]
        public bool sth = true;
        [KSPField(isPersistant = true)]
        public string _sth = string.Empty;

        [KSPField(isPersistant = true)]
        public bool unlocked = true;
        [KSPField(isPersistant = true)]
        public string _unlocked = "True";

        [KSPField(isPersistant = true)]
        public bool hostile = false;

        [KSPField(isPersistant = true)]
        double lat = 0f;
        [KSPField(isPersistant = true)]
        double lon = 0f;
        [KSPField(isPersistant = true)]
        double alt = 0f;

        [KSPField(isPersistant = true)]
        public string celestialBody = string.Empty;
        [KSPField(isPersistant = true)]
        private string HoloCacheName =  "";
        [KSPField(isPersistant = true)]
        private string text1 = string.Empty;
        [KSPField(isPersistant = true)]
        private string text2 = string.Empty;
        [KSPField(isPersistant = true)]
        private string text3 = string.Empty;

        [KSPField(isPersistant = true)]
        private string tech = string.Empty;
        [KSPField(isPersistant = true)]
        private bool techIncluded = false;
        private string techToUnlock = string.Empty;

        [KSPField(isPersistant = true)]
        private string Password_ = "OrX";
        private string Password = "OrX";

        [KSPField(isPersistant = true)]
        public bool spawned = false;
        [KSPField(isPersistant = true)]
        public bool spawn = true;
        [KSPField(isPersistant = true)]
        public bool openHolo = false;

        public bool powerDown = false;
        private bool velMatch = false;
        private bool checkingVel = false;

        [KSPField(isPersistant = true)]
        private bool openHoloCache = false;

        private bool opened = false;
        private bool opening = false;
        private bool saveLocalVessels = false;
        private int timer = 2;

        Guid vid;

        #endregion

        /// ////////////////////////////////////////////////////////

        #region Coords

        [KSPField(isPersistant = true)] public string HoloCraft1name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft1 = "HoloCraft1";
        [KSPField(isPersistant = true)] public bool HoloCoord1 = false;
        [KSPField(isPersistant = true)] public double HoloCoord1lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord1lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord1alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft2name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft2 = "HoloCraft2";
        [KSPField(isPersistant = true)] public bool HoloCoord2 = false;
        [KSPField(isPersistant = true)] public double HoloCoord2lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord2lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord2alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft3name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft3 = "HoloCraft3";
        [KSPField(isPersistant = true)] public bool HoloCoord3 = false;
        [KSPField(isPersistant = true)] public double HoloCoord3lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord3lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord3alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft4name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft4 = "HoloCraft4";
        [KSPField(isPersistant = true)] public bool HoloCoord4 = false;
        [KSPField(isPersistant = true)] public double HoloCoord4lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord4lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord4alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft5name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft5 = "HoloCraft5";
        [KSPField(isPersistant = true)] public bool HoloCoord5 = false;
        [KSPField(isPersistant = true)] public double HoloCoord5lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord5lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord5alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft6name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft6 = "HoloCraft6";
        [KSPField(isPersistant = true)] public bool HoloCoord6 = false;
        [KSPField(isPersistant = true)] public double HoloCoord6lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord6lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord6alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft7name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft7 = "HoloCraft7";
        [KSPField(isPersistant = true)] public bool HoloCoord7 = false;
        [KSPField(isPersistant = true)] public double HoloCoord7lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord7lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord7alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft8name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft8 = "HoloCraft8";
        [KSPField(isPersistant = true)] public bool HoloCoord8 = false;
        [KSPField(isPersistant = true)] public double HoloCoord8lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord8lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord8alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft9name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft9 = "HoloCraft9";
        [KSPField(isPersistant = true)] public bool HoloCoord9 = false;
        [KSPField(isPersistant = true)] public double HoloCoord9lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord9lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord9alt = 0;

        [KSPField(isPersistant = true)] public string HoloCraft10name = string.Empty;
        [KSPField(isPersistant = true)] public string HoloCraft10 = "HoloCraft10";
        [KSPField(isPersistant = true)] public bool HoloCoord10 = false;
        [KSPField(isPersistant = true)] public double HoloCoord10lat = 0;
        [KSPField(isPersistant = true)] public double HoloCoord10lon = 0;
        [KSPField(isPersistant = true)] public double HoloCoord10alt = 0;

        #endregion

        public override void OnStart(StartState state)
        {
            part.force_activate();
            this.part.collider.isTrigger = true;
            powerDown = false;
            opened = false;
            openHoloCache = false;
            velMatch = false;
            checkingVel = false;
            vid = FlightGlobals.ActiveVessel.id;
            Debug.Log("[Module OrX HoloCache] this.part.collider.tag = " + this.part.collider.tag);

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (sth)
                {
                    _sth = "True";
                }
                else
                {
                    _sth = "False";
                }
            }

            craftTitleLabel = new GUIStyle();
            craftTitleLabel.normal.textColor = OrXCraftSkin.window.normal.textColor;
            craftTitleLabel.font = OrXCraftSkin.window.font;
            craftTitleLabel.fontSize = OrXCraftSkin.window.fontSize;
            craftTitleLabel.fontStyle = OrXCraftSkin.window.fontStyle;
            craftTitleLabel.alignment = TextAnchor.UpperCenter;

            WindowRectBrowser = new Rect((Screen.width / 16) * 2.5f, 140, browserWindowWidth, browserWindowHeight);
            WindowRectCS = new Rect(0, 0, WindowRectBrowser.width - 10, 0);
            WindowRectCS.width = WindowRectBrowser.width - 10;


            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (this.vessel.isActiveVessel)
                {
                    foreach (Vessel v in FlightGlobals.Vessels)
                    {
                        if (v.id == vid)
                        {
                            FlightGlobals.ForceSetActiveVessel(v);
                            break;
                        }
                    }
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.id != vid)
                    {
                        vid = FlightGlobals.ActiveVessel.id;
                    }

                    if (OrXAppendCfg.instance.GuiEnabledOrXAppendCfg)
                    {
                        if (OrXAppendCfg.instance.cancel)
                        {
                            OrXAppendCfg.instance.DisableGui();
                            GameUiEnableOrXHoloCache();
                        }
                        else
                        {
                            if (OrXAppendCfg.instance.save || OrXAppendCfg.instance.append)
                            {
                                HoloCacheName = OrXAppendCfg.instance.hcName;
                                OrXAppendCfg.instance.DisableGui();
                                StartCoroutine(SaveHoloCache());
                            }
                        }
                    }

                    if (!powerDown && !this.vessel.HoldPhysics)
                    {
                        if (!isSetup)
                        {
                            if (!setup)
                            {
                                setup = true;
                                Debug.Log("[Module OrX HoloCache]: setup = true;");
                                spawned = false;
                                CountDownRoutine();
                            }
                        }
                        else
                        {
                            if (!opened && !opening)
                            {
                                opening = true;

                                if (FlightGlobals.ActiveVessel.isEVA)
                                {
                                    Debug.Log("[Module OrX HoloCache]: StartCoroutine(CheckTargetDistance());");
                                    CountDownRoutine();
                                }
                                else
                                {
                                    opening = false;
                                    opened = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CountDownRoutine()
        {
            Debug.Log("[Module OrX HoloCache]: CountDownRoutine");
            timer = 2;

            if (spawned)
            {
                if (!checkingVel)
                {
                    checkingVel = true;
                    StartCoroutine(HeadingCheck());
                }
                else
                {
                    if (FlightGlobals.ActiveVessel.isEVA && velMatch)
                    {
                        if (!openHoloCache && opening)
                        {
                            double targetDistance = Vector3d.Distance(FlightGlobals.ActiveVessel.GetWorldPos3D(), this.vessel.GetTransform().position);

                            if (targetDistance <= 2 && this.vessel.LandedOrSplashed)
                            {
                                EnableOrXHCGui();
                            }
                            else
                            {
                                if (targetDistance <= 10 && this.vessel.LandedOrSplashed)
                                {
                                    timer = 2;
                                }
                                else
                                {
                                    if (targetDistance <= 25 && this.vessel.LandedOrSplashed)
                                    {
                                        timer = 5;
                                    }
                                    else
                                    {
                                        if (targetDistance <= 50 && this.vessel.LandedOrSplashed)
                                        {
                                            timer = 10;
                                        }
                                        else
                                        {
                                            timer = 15;
                                        }
                                    }
                                }

                                StartCoroutine(PauseTimer());
                            }
                        }
                    }
                }
            }
            else
            {
                if (!checkingVel)
                {
                    checkingVel = true;
                    StartCoroutine(HeadingCheck());
                }
                else
                {
                    if (velMatch)
                    {
                        EnableOrXHCGui();
                    }
                }
            }
        }

        IEnumerator PauseTimer()
        {
            yield return new WaitForSeconds(timer);
            CountDownRoutine();
        }

        IEnumerator HeadingCheck()
        {
            if (this.vessel.LandedOrSplashed)
            {
                Debug.Log("[Module OrX HoloCache]: " + HoloCacheName + " is Landed or Splased .............");
                velMatch = true;
                CountDownRoutine();
            }
            else
            {
                if (this.vessel.atmDensity <= 0.007)
                {
                    Debug.Log("[ModuleOrX HoloCache] TARGET SPEED: " + FlightGlobals.ActiveVessel.speed);
                    double diff = FlightGlobals.ActiveVessel.speed - this.vessel.speed;
                    int _diff = Convert.ToInt32(diff);
                    Debug.Log("[ModuleOrX HoloCache] HOLOCACHE SPEED: " + this.vessel.speed);
                    Debug.Log("[ModuleOrX HoloCache] SPEED DIFFERENCE: " + _diff);

                    Vector3 position1 = FlightGlobals.ActiveVessel.GetTransform().position;
                    yield return new WaitForSeconds(0.5f);
                    Vector3 position2 = FlightGlobals.ActiveVessel.GetTransform().position;

                    bool vmf = false;

                    try
                    {
                        Debug.Log("[Module OrX HoloCache]: " + HoloCacheName + " matching orbit");
                        this.vessel.GetComponent<Rigidbody>().velocity = (position1 - position2).normalized * _diff;
                        Debug.Log("[ModuleOrX HoloCache] VELOCITY ADDED: " + _diff);
                    }
                    catch (Exception e)
                    {
                        vmf = true;
                        Debug.Log("[ModuleOrX HoloCache] === ORBIT MATCH FAILED ===");
                    }

                    if (!vmf)
                    {
                        diff = FlightGlobals.ActiveVessel.speed - this.vessel.speed;
                        if (diff <= -10 || diff >= 10)
                        {
                            StartCoroutine(HeadingCheck());
                        }
                        else
                        {
                            Debug.Log("[Module OrX HoloCache]: " + HoloCacheName + " has matched orbit");
                            velMatch = true;
                            CountDownRoutine();
                        }
                    }
                }
                else
                {
                    Debug.Log("[Module OrX HoloCache]: " + HoloCacheName + " is in atmosphere ... waiting until touchdown");

                    yield return new WaitForSeconds(2);
                    StartCoroutine(HeadingCheck());
                }
            }
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

            aString += this.vessel.latitude;
            aString += ",";
            aString += this.vessel.longitude;
            aString += ",";
            aString += this.vessel.altitude;
            aString += ";";

            aString += text1;
            aString += ";";
            aString += text2;
            aString += ";";
            aString += text3;
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

            bString += FlightGlobals.ActiveVessel.orbitDriver.pos.x;
            bString += ",";
            bString += FlightGlobals.ActiveVessel.orbitDriver.pos.y;
            bString += ",";
            bString += FlightGlobals.ActiveVessel.orbitDriver.pos.z;
            bString += ";";

            bString += text1;
            bString += ";";
            bString += text2;
            bString += ";";
            bString += text3;
            bString += ";";

            finalString += bString;

            return finalString;
        }

        IEnumerator SaveHoloCache()
        {
            DisableGui();

            Password_ = Password;
            HoloCoord1 = false;
            HoloCoord2 = false;
            HoloCoord3 = false;
            HoloCoord4 = false;
            HoloCoord5 = false;
            HoloCoord6 = false;
            HoloCoord7 = false;
            HoloCoord8 = false;
            HoloCoord9 = false;
            HoloCoord10 = false;
            spawn = true;
            sth = true;
            openHolo = true;
            this.vessel.vesselName = HoloCacheName;
            lat = this.vessel.latitude;
            lon = this.vessel.longitude;
            alt = this.vessel.altitude + 10;
            string soi = FlightGlobals.currentMainBody.name;
            celestialBody = soi;

            _sth = "True";
            getGPS = false;
            isSetup = true;
            setup = false;

            ScreenMsg("<color=#cfc100ff><b>Creating HoloCache: " + HoloCacheName + "</b></color>");
            Debug.Log("[OrX HoloCache] Creating HoloCache: " + HoloCacheName);
            Debug.Log("[OrX HoloCache] CelestialBody: " + celestialBody);
            Debug.Log("[OrX HoloCache] GPS Latitude: " + lat);
            Debug.Log("[OrX HoloCache] GPS Longitude: " + lon);
            Debug.Log("[OrX HoloCache] GPS Altitude: " + alt);
            Debug.Log("[OrX HoloCache] STH: " + sth);
            Debug.Log("[OrX HoloCache] Unlocked: " + _unlocked);

            OrXHoloCache.instance.HoloCacheName = HoloCacheName;
            OrXHoloCache.instance.resetHoloCache = true;
            OrXHoloCache.instance.holoCache = this.vessel;
            OrXHoloCache.instance.craftToSpawn = HoloCacheName;
            OrXHoloCache.instance._lat = lat;
            OrXHoloCache.instance._lon = lon;
            OrXHoloCache.instance._alt = alt;
            OrXHoloCache.instance.sth = sth;

            if (!Directory.Exists(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName))
            {
                Directory.CreateDirectory(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName);
            }
            ConfigNode file = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            if (file == null)
            {
                file = new ConfigNode();
                file.AddNode("OrX");
                file.AddValue("name", HoloCacheName);
                file.Save("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            }
            ConfigNode node = null;
            if (file != null && file.HasNode("OrX"))
            {
                int hcCount = 0;

                node = file.GetNode("OrX");
                ConfigNode HoloCacheNode = null;

                foreach (ConfigNode cn in node.nodes)
                {
                    if (cn.name.Contains("OrXHoloCacheCoords"))
                    {
                        hcCount += 1;
                    }
                }

                if (node.HasNode("OrXHoloCacheCoords" + hcCount))
                {
                    foreach (ConfigNode n in node.GetNodes("OrXHoloCacheCoords" + hcCount))
                    {
                        if (n.GetValue("SOI") == this.vessel.mainBody.name)
                        {
                            HoloCacheNode = n;
                            break;
                        }
                    }

                    if (HoloCacheNode == null)
                    {
                        HoloCacheNode = node.AddNode("OrXHoloCacheCoords" + hcCount);
                        HoloCacheNode.AddValue("SOI", this.vessel.mainBody.name);
                        HoloCacheNode.AddValue("spawned", "False");
                        HoloCacheNode.AddValue("extras", "False");
                        HoloCacheNode.AddValue("unlocked", unlocked);
                        if (techIncluded)
                        {
                            HoloCacheNode.AddValue("tech", tech);
                        }
                    }
                }
                else
                {
                    HoloCacheNode = node.AddNode("OrXHoloCacheCoords" + hcCount);
                    HoloCacheNode.AddValue("SOI", this.vessel.mainBody.name);
                    HoloCacheNode.AddValue("spawned", "False");
                    HoloCacheNode.AddValue("extras", "False");
                    HoloCacheNode.AddValue("unlocked", unlocked);
                    if (techIncluded)
                    {
                        HoloCacheNode.AddValue("tech", tech);
                    }
                }

                string targetString = HoloCacheListToString();
                HoloCacheNode.SetValue("Targets", targetString, true);
                if (!node.HasNode("HoloCache" + hcCount))
                {
                    node.AddNode("HoloCache" + hcCount);
                }
                Debug.Log("[Module OrX HoloCache] Saving OrX HoloCache Targets");
                VesselToSave = this.vessel;
                Debug.Log("[Module OrX HoloCache] HoloCache Identified .......................");
                shipDescription = HoloCacheName + hcCount;
                ShipConstruct ConstructToSave = new ShipConstruct(HoloCacheName, shipDescription, this.vessel.parts[0]);
                Debug.Log("[Module OrX HoloCache] Saving HoloCache: " + HoloCacheName + " ............");
                ScreenMsg("<color=#cfc100ff><b>Saving HoloCache: " + HoloCacheName + "</b></color>");
                Quaternion or = this.vessel.vesselTransform.rotation;
                Vector3 op = this.vessel.vesselTransform.position;
                this.vessel.SetRotation(new Quaternion(0, 0, 0, 1));
                Vector3 ShipSize = ShipConstruction.CalculateCraftSize(ConstructToSave);
                this.vessel.SetPosition(new Vector3(0, ShipSize.y + 2, 0));
                ConfigNode craftConstruct = new ConfigNode("Craft");
                craftConstruct = ConstructToSave.SaveShip();
                ConfigNodeClean(craftConstruct); // Thanx Claw!!!!!
                this.vessel.SetRotation(or);
                this.vessel.SetPosition(op);
                ConfigNode HCnode = node.GetNode("HoloCache" + hcCount);
                craftConstruct.CopyTo(HCnode);
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

                file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                Debug.Log("[Module OrX HoloCache] " + HoloCacheName + " Saved");

                yield return new WaitForFixedUpdate();
                int count = 0;

                if (blueprintsAdded)
                {
                    ConfigNode addedCraft = ConfigNode.Load(craftLoc);

                    if (addedCraft != null)
                    {
                        foreach (ConfigNode n in node.nodes)
                        {
                            if (n.name.Contains("HC" + hcCount + "OrXv"))
                            {
                                count += 1;
                            }
                        }

                        ConfigNode craftData = node.AddNode("HC" + hcCount + "OrXv" + count);
                        craftData.AddValue("vesselName", craftToAdd);
                        ConfigNode location = craftData.AddNode("coords");
                        location.AddValue("holo", hcCount);
                        location.AddValue("pas", Password_);
                        location.AddValue("lat", FlightGlobals.ActiveVessel.latitude);
                        location.AddValue("lon", FlightGlobals.ActiveVessel.longitude);
                        location.AddValue("alt", FlightGlobals.ActiveVessel.altitude);

                        foreach (ConfigNode.Value cv in location.values)
                        {
                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                            cv.name = cvEncryptedName;
                            cv.value = cvEncryptedValue;
                        }

                        ConfigNode craftFile = craftData.AddNode("craft");
                        ScreenMsg("<color=#cfc100ff><b>Saving to " + HoloCacheName + "</b></color>");
                        Debug.Log("[Module OrX HoloCache] Saving: " + craftToAdd);
                        addedCraft.CopyTo(craftFile);

                        foreach (ConfigNode.Value cv in craftFile.values)
                        {
                            if (cv.name == "ship")
                            {
                                cv.value = craftToAdd;
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

                        file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                        saveShip = false;
                        holo = false;
                        Debug.Log("[Module OrX HoloCache] " + craftToAdd + " Saved to " + HoloCacheName);
                        ScreenMsg("<color=#cfc100ff><b>" + craftToAdd + " Saved</b></color>");

                        var files = new List<string>(Directory.GetFiles(craftLoc, craftToAdd + ".craft", SearchOption.AllDirectories));
                        if (files != null)
                        {
                            List<string>.Enumerator craftFileToAdd = files.GetEnumerator();
                            while (craftFileToAdd.MoveNext())
                            {
                            }
                            craftFileToAdd.Dispose();
                        }
                        else
                        {
                            Debug.Log("[Module OrX HoloCache] === Craft Files Not Found ===");
                        }
                    }
                }

                craftLoc = "";

                if (saveLocalVessels)
                {
                    List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator();
                    while (v.MoveNext())
                    {
                        yield return new WaitForFixedUpdate();

                        try
                        {
                            if (v.Current == null) continue;
                            if (!v.Current.loaded || v.Current.packed) continue;
                            if (v.Current != this.vessel)
                            {
                                try
                                {
                                    Vector3d vLoc = v.Current.GetWorldPos3D();
                                    double targetDistance = Vector3d.Distance(this.vessel.GetTransform().position, vLoc);

                                    if (targetDistance <= 5000 && v.Current.parts.Count != 1)
                                    {
                                        count += 1;

                                        file = ConfigNode.Load(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                                        node = file.GetNode("OrX");
                                        Vessel toSave = v.Current;
                                        Debug.Log("[Module OrX HoloCache] Vessel " + v.Current.vesselName + " Identified .......................");
                                        shipDescription = v.Current.vesselName + " blueprints from " + HoloCacheName;
                                        ConstructToSave = new ShipConstruct(HoloCacheName, shipDescription, v.Current.parts[0]);
                                        ScreenMsg("<color=#cfc100ff><b>Saving: " + v.Current.vesselName + "</b></color>");
                                        or = v.Current.vesselTransform.rotation;
                                        op = v.Current.vesselTransform.position;
                                        v.Current.SetRotation(new Quaternion(0, 0, 0, 1));
                                        ShipSize = ShipConstruction.CalculateCraftSize(ConstructToSave);
                                        v.Current.SetPosition(new Vector3(0, ShipSize.y + 2, 0));
                                        craftConstruct = new ConfigNode("Craft");
                                        craftConstruct = ConstructToSave.SaveShip();
                                        ConfigNodeClean(craftConstruct); // Thanx Claw!!!!!
                                        v.Current.SetRotation(or);
                                        v.Current.SetPosition(op);
                                        Debug.Log("[Module OrX HoloCache] Saving: " + v.Current.vesselName);
                                        ConfigNode craftData = node.AddNode("HC" + hcCount + "OrXv" + count);
                                        craftData.AddValue("vesselName", v.Current.vesselName);
                                        ConfigNode location = craftData.AddNode("coords");
                                        location.AddValue("holo", hcCount);
                                        location.AddValue("pas", Password_);
                                        location.AddValue("lat", v.Current.latitude);
                                        location.AddValue("lon", v.Current.longitude);
                                        location.AddValue("alt", v.Current.altitude);

                                        foreach (ConfigNode.Value cv in location.values)
                                        {
                                            string cvEncryptedName = OrXLog.instance.Crypt(cv.name);
                                            string cvEncryptedValue = OrXLog.instance.Crypt(cv.value);
                                            cv.name = cvEncryptedName;
                                            cv.value = cvEncryptedValue;
                                        }

                                        Debug.Log("[Module OrX HoloCache] Adding coords ............................. " + HoloCacheName);
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

                                        file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                                        saveShip = false;
                                        holo = false;
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
                }

                saveShip = false;
                holo = false;
                craftLoc = "";
                this.vessel.DestroyVesselComponents();
                this.vessel.Die();
            }
        }

        private void ExtractCraftFiles()
        {
            Debug.Log("[OrX Holocache] === Loading Craft Files ===");

            ConfigNode file = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            ConfigNode craft = null;
            ConfigNode craftFile = null;
            ConfigNode location = null;
            double craftlat = 0;
            double craftlon = 0;
            double craftalt = 0;
            int hcCount = 0;
            string techToAdd = "";

            if (file.HasNode("OrX"))
            {
                Debug.Log("[OrX Holocache] === Data Loaded ===");
                ConfigNode Vnode = file.GetNode("OrX");
                int vn = 0;
                bool hasSpawned = true;

                foreach (ConfigNode spawnCheck in Vnode.nodes)
                {
                    if (hasSpawned)
                    {
                        if (spawnCheck.name.Contains("OrXHoloCacheCoords"))
                        {
                            Debug.Log("[OrX Holocache] === FOUND HOLOCACHE === " + hcCount); ;

                            ConfigNode sc = Vnode.GetNode("OrXHoloCacheCoords" + hcCount);

                            foreach (ConfigNode.Value cv in sc.values)
                            {
                                if (cv.name == "spawned")
                                {
                                    if (cv.value == "False")
                                    {
                                        Debug.Log("[OrX Holocache] === HOLOCACHE " + hcCount + " has not spawned ... "); ;

                                        hasSpawned = false;
                                    }
                                    else
                                    {
                                        Debug.Log("[OrX Holocache] === HOLOCACHE " + hcCount + " has spawned ... CHECKING FOR EXTRAS"); ;

                                        if (sc.HasValue("extras"))
                                        {
                                            var t = sc.GetValue("extras");
                                            if (t == "False")
                                            {
                                                Debug.Log("[OrX Holocache] === HOLOCACHE " + hcCount + " has no extras ... END TRANSMISSION"); ;
                                                hasSpawned = false;
                                                break;
                                            }
                                            else
                                            {
                                                Debug.Log("[OrX Holocache] === HOLOCACHE " + hcCount + " has extras ... SEARCHING"); ;
                                                hasSpawned = true;
                                                hcCount += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string crafttosave = string.Empty;

                foreach (ConfigNode cnode in Vnode.nodes)
                {
                    if (cnode.name.Contains("HC" + hcCount + "OrXv"))
                    {
                        ConfigNode local = cnode.GetNode("HC" + hcCount + "OrXv" + vn);
                        foreach (ConfigNode.Value cv in local.values)
                        {
                            if (cv.name == "vesselName")
                            {
                                Debug.Log("[OrX Holocache] === Blueprints found - '" + cv.value + "' ===");

                                crafttosave = cv.value;
                            }
                        }

                        location = local.GetNode("coords");

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
                                            locEncryptedValue = OrXLog.instance.Decrypt(_loc.value);

                                            if (locEncryptedValue == Password)
                                            {
                                                unlocked = true;
                                            }
                                            else
                                            {
                                                unlocked = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (unlocked)
                        {
                            craftFile = local.GetNode("craft");

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

                            foreach (ConfigNode.Value value in location.values)
                            {
                                if (value.name == "lat")
                                {
                                    craftlat = double.Parse(value.value);
                                }

                                if (value.name == "lon")
                                {
                                    craftlon = double.Parse(value.value);
                                }

                                if (value.name == "alt")
                                {
                                    craftalt = double.Parse(value.value);
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

                            Debug.Log("[OrX Holocache] === EXTRACTING '" + crafttosave + "' ===");
                            Debug.Log("[OrX Holocache] === Current Game = " + HighLogic.SaveFolder + " ===");
                            craftFile.Save(UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder
                                + "/Ships/" + _type + crafttosave + ".craft");
                            ScreenMsg("<color=#cfc100ff><b>" + crafttosave + " blueprints available</b></color>");
                            craftFile.Save("GameData/OrX/HoloCache/" + HoloCacheName + "/" + crafttosave + ".craft");

                            ConfigNode HoloCacheNode = Vnode.GetNode("OrXHoloCacheCoords" + hcCount);

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
                                            Debug.Log("[Module OrX HoloCache] " + HoloCacheName + " is adding " + techToAdd + " to the tech list ...");
                                            OrXLog.instance.AddTech(techToAdd);
                                        }
                                        else
                                        {
                                            Debug.Log("[Module OrX HoloCache] " + techToAdd + " is already in the tech list ...");
                                        }
                                    }
                                }
                                if (a == "spawned")
                                {
                                    cv.value = OrXLog.instance.Crypt("True");
                                }
                            }

                            Debug.Log("[Module OrX HoloCache] " + HoloCacheName + " Saved Status - SPAWNED");
                        }

                        vn += 1;
                    }
                }
            }

            file.Save(UrlDir.ApplicationRootPath + "GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
            OrXHoloCache.instance.HideGameUI();

            file.ClearData();
            VesselToSave = FlightGlobals.ActiveVessel;
            Debug.Log("[Module OrX HoloCache] Kerbal Identified .......................");
            shipDescription = "";
            ShipConstruct ConstructToSave = new ShipConstruct(HoloCacheName, shipDescription, FlightGlobals.ActiveVessel.parts[0]);
            Debug.Log("[Module OrX HoloCache] Saving Kerbal ............");
            Quaternion or = FlightGlobals.ActiveVessel.vesselTransform.rotation;
            Vector3 op = FlightGlobals.ActiveVessel.vesselTransform.position;
            FlightGlobals.ActiveVessel.SetRotation(new Quaternion(0, 0, 0, 1));
            Vector3 ShipSize = ShipConstruction.CalculateCraftSize(ConstructToSave);
            FlightGlobals.ActiveVessel.SetPosition(new Vector3(0, ShipSize.y + 2, 0));
            ConfigNode craftConstruct = new ConfigNode("Craft");
            craftConstruct = ConstructToSave.SaveShip();
            ConfigNodeClean(craftConstruct); // Thanx Claw!!!!!
            FlightGlobals.ActiveVessel.SetRotation(or);
            FlightGlobals.ActiveVessel.SetPosition(op);
            craftConstruct.CopyTo(file);
            //file.Save("GameData/OrX/Plugin/PluginData/OrX.tmp");
            file.Save("GameData/OrX/Plugin/PluginData/OrXKerbal.cr");

            Debug.Log("[Module OrX HoloCache]: ===== UNLOCKING =====");
            openHoloCache = true;
            opened = true;

            if (spawnInfected)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    //StartCoroutine(SpawnNPC());
                }
            }
        }

        IEnumerator SpawnNPC()
        {
            OrXSpawn.instance.orx = true;
            OrXSpawn.instance.infected = false;
            OrXSpawn.instance.infectedCount = 1;
            OrXSpawn.instance.SpawnCoords = this.vessel.GetTransform().position;
            OrXSpawn.instance.SpawnInfected();
            yield return new WaitForEndOfFrame();
        }

        private void HoloHack()
        {
            // open new GUI holo hack if Byte has been put together (takes 8 bits to make a byte)
        }

        private CraftBrowserDialog craftBrowser;
        public bool craftBrowserOpen = false;

        public void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 4, ScreenMessageStyle.UPPER_CENTER));
        }

        /// <summary>
        /// //////////////////////////////
        /// </summary>

        public bool saveShip = false;
        public string ShipName = string.Empty;
        public Vessel VesselToSave;
        public bool holo = false;
        public string shipDescription = string.Empty;
        //public string HoloCacheName = string.Empty;
        public bool sthTargets = false;

        private void ConfigNodeClean(ConfigNode craft)
        {
            craft.SetValue("EngineIgnited", "False");
            craft.SetValue("currentThrottle", "0");
            craft.SetValue("Staged", "False");
            craft.SetValue("sensorActive", "False");
            craft.SetValue("throttle", "0");
            craft.SetValue("generatorIsActive", "False");
            craft.SetValue("persistentState", "STOWED");

            string ModuleName = craft.GetValue("name");

            if ("ModuleScienceExperiment" == ModuleName)
            {
                craft.RemoveNodes("ScienceData");
            }
            else if ("ModuleScienceExperiment" == ModuleName)
            {
                craft.SetValue("Inoperable", "False");
                craft.RemoveNodes("ScienceData");
            }
            else if ("Log" == ModuleName)
            {
                craft.ClearValues();
            }


            for (int IndexNodes = 0; IndexNodes < craft.nodes.Count; IndexNodes++)
            {
                ConfigNodeClean(craft.nodes[IndexNodes]);
            }
        }

        /*
        private void ScrubConfigNodes(ConfigNode craft)
        {
            if (null == craft) { return; }

            if ("PART" == craft.name)
            {
                string PartName = ((craft.GetValue("part")).Split('_'))[0];

                Debug.Log("PART: " + PartName);

                Part NewPart = PartLoader.getPartInfoByName(PartName).partPrefab;
                ConfigNode craftConstructNewPart = new ConfigNode();
                Debug.Log("[OrX Ship Save] New Part: " + NewPart.name);

                NewPart.InitializeModules();

                craft.ClearNodes();

                // EVENTS, ACTIONS, PARTDATA, MODULE, RESOURCE

                Debug.Log("EVENTS");
                NewPart.Events.OnSave(craft.AddNode("EVENTS"));
                Debug.Log("ACTIONS");
                NewPart.Actions.OnSave(craft.AddNode("ACTIONS"));
                Debug.Log("PARTDATA");
                NewPart.OnSave(craft.AddNode("PARTDATA"));
                Debug.Log("MODULE");
                for (int IndexModules = 0; IndexModules < NewPart.Modules.Count; IndexModules++)
                {
                    NewPart.Modules[IndexModules].Save(craft.AddNode("MODULE"));
                }
                Debug.Log("RESOURCE");
                for (int IndexResources = 0; IndexResources < NewPart.Resources.Count; IndexResources++)
                {
                    NewPart.Resources[IndexResources].Save(craft.AddNode("RESOURCE"));
                }

                return;
            }
            for (int IndexNodes = 0; IndexNodes < craft.nodes.Count; IndexNodes++)
            {
                ScrubConfigNodes(craft.nodes[IndexNodes]);
            }
        }

    */

        /// <summary>
        /// ////////////////////////////////////////////////////////
        /// </summary>

        #region HoloCache GUI
        /// <summary>
        /// GUI
        /// </summary>

        private void Start()
        {
            _windowRect = new Rect(Screen.width - 320 - WindowWidth, 200, WindowWidth, _windowHeight);
            GameEvents.onHideUI.Add(GameUiDisableOrXHoloCache);
            GameEvents.onShowUI.Add(GameUiEnableOrXHoloCache);
            _gameUiToggle = true;
        }

        private void OnGUI()
        {
            if (GuiEnabledOrX_HoloCache && _gameUiToggle)
            {
                _windowRect = GUI.Window(384766702, _windowRect, GuiWindowOrX_HoloCache, "");
            }

            if (GuiEnabledOrX_HoloCache2 && isSetup)
            {
                GuiEnabledOrX_HoloCache = false;
                craftBrowserOpen = false;
                _windowRect = GUI.Window(38572892, _windowRect, GuiWindowOrX_HoloCache2, "");
            }

            if (craftBrowserOpen && _gameUiToggle)
            {
                GuiEnabledOrX_HoloCache2 = false;
                GuiEnabledOrX_HoloCache = false;
                _windowRect = GUI.Window(38922892, _windowRect, GuiWindowOrX_CraftBrowser, "");
            }
        }

        private const float WindowWidthAppend = 250;

        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public bool GuiEnabledOrX_HoloCache = false;
        public bool GuiEnabledOrX_HoloCache2 = false;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;

        public float _hp = 0;
        private float _oxygen = 0.0f;

        private bool blueprintsAdded = false;

        public static Rect WindowRectBrowser;
        public static Rect WindowRectCS;
        float listHeight;
        float browserWindowWidth = 250;
        float browserWindowHeight = 24;
        public static GUISkin OrXBrowserSkin = HighLogic.Skin;
        float entryCount;
        GUIStyle craftTitleLabel;
        public static GUISkin OrXCraftSkin = HighLogic.Skin;
        private int craftIndex;
        float craftEntryHeight = 24;

        void GuiWindowOrX_CraftBrowser(int OrX_CraftBrowser)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));

            float line = 0;
            float leftIndent = 10;
            float contentWidth = WindowWidth - leftIndent;
            float contentTop = 10;
            float entryHeight = 20;
            float gpsLines = 0;

            line += 0.6f;

            GUI.BeginGroup(new Rect(5, contentTop + (line * entryHeight), WindowWidth, WindowRectCS.height));
            WindowCraftBrowser();
            GUI.EndGroup();
            gpsLines = WindowRectCS.height / entryHeight;

            listHeight = Mathf.Lerp(listHeight, gpsLines, 0.15f);
            line += listHeight;
            line += 0.25f;
            if (GUI.Button(new Rect(5, contentTop + (line * entryHeight), 250 - 5, 20), "Cancel", OrXCraftSkin.button))
            {
                HoloCacheName = "";
                craftBrowserOpen = false;
                GuiEnabledOrX_HoloCache2 = false;
                GuiEnabledOrX_HoloCache = true;
            }
            line += 1.25f;
            browserWindowHeight = Mathf.Lerp(browserWindowHeight, contentTop + (line * entryHeight) + 5, 1);
            WindowRectBrowser.height = browserWindowHeight;
        }

        public void WindowCraftBrowser()
        {
            GUI.Box(WindowRectCS, GUIContent.none, OrXBrowserSkin.button);
            entryCount = 0;
            Rect listRect = new Rect(5, 5, 240 - (2 * 5),
                WindowRectCS.height - (2 * 5));
            GUI.BeginGroup(listRect);
            GUI.Label(new Rect(0, 0, listRect.width, 20), "Craft Files", craftTitleLabel);
            entryCount += 1.2f;
            int index = 0;

            string craftLoc = UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/";
            var files = new List<string>(Directory.GetFiles(craftLoc, "*.craft", SearchOption.AllDirectories));

            if (files != null)
            {
                List<string>.Enumerator craftFilesToAdd = files.GetEnumerator();
                while (craftFilesToAdd.MoveNext())
                {
                    Color origWColor = GUI.color;
                    ConfigNode craft = ConfigNode.Load(craftFilesToAdd.Current);
                    string vn = "";

                    try
                    {
                        foreach (ConfigNode.Value cv in craft.values)
                        {
                            if (cv.name == "ship")
                            {
                                vn = cv.value;
                            }
                        }

                        if (GUI.Button(new Rect(0, entryCount * craftEntryHeight, 240, craftEntryHeight), vn, OrXCraftSkin.button))
                        {
                            Debug.Log("[Module OrX HoloCache] === CRAFT SELECTED ===");
                            craftLoc = craftFilesToAdd.Current;
                            craftBrowserOpen = false;
                            GuiEnabledOrX_HoloCache = true;
                            craftToAdd = vn;
                            blueprintsAdded = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("[Module OrX HoloCache] HoloCache Threw Exception While Listing Craft ...... Continuing");
                    }

                    entryCount++;
                    index++;
                    GUI.color = origWColor;
                }
                craftFilesToAdd.Dispose();
            }
            else
            {
                Debug.Log("[Module OrX HoloCache] === Craft Files Not Found ===");
            }

            GUI.EndGroup();
            WindowRectCS.height = (2 * 5) + (entryCount * craftEntryHeight);
        }

        private void GuiWindowOrX_HoloCache(int OrX_HoloCache)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;

            if (OrXLog.instance.addBlueprints)
            {
                DrawAddBlueprints(line);
                line++;

                if (OrXLog.instance.addLocalVessels)
                {
                    //DrawLocalBlueprints(line);
                    //line++;
                    //line++;
                }
            }

            if (OrXLog.instance.addTech)
            {
                DrawTech(line);
                line++;
                if (techIncluded)
                {
                    DrawModule(line);
                    line++;
                }
            }

            if (OrXLog.instance.addInfected)
            {
                //DrawInfected(line);
                //line++;
                if (spawnInfected)
                {
                    //DrawInfectedCount(line);
                    //line++;
                    //line += 0.5f;
                }
            }

            if (OrXLog.instance.addLock)
            {
                DrawLock(line);
                line++;
                if (!unlocked)
                {
                    DrawPassword(line);
                    line++;
                }
            }
            line++;
            line++;
            DrawTitle4(line);
            line++;
            DrawHoloCacheName(line);
            line++;
            DrawSave(line);
            line++;
            DrawCancel(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void GuiWindowOrX_HoloCache2(int OrX_HoloCache2)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle(line);
            line++;
            line++;
            DrawHoloCacheName1(line);
            line++;

            if (!unlocked)
            {
                DrawPassword(line);
                line++;
                DrawUnLock(line);
                line++;
            }
            else
            {
                if (blueprintsAdded)
                {
                    DrawcraftToAdd(line);
                    line++;
                }

                if (techIncluded)
                {
                    DrawTechIncluded(line);
                    line++;
                }

                if (spawnInfected)
                {
                   // DrawInfected(line);
                   // line++;
                }
            }

            DrawCloseGui(line);
            line++;

            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void EnableOrXHCGui()
        {
            if (!isSetup)
            {
                GuiEnabledOrX_HoloCache = true;
                GuiEnabledOrX_HoloCache2 = false;
                Debug.Log("[OrX]: Showing HoloCache JDI GUI");
            }
            else
            {
                GuiEnabledOrX_HoloCache2 = true;
                GuiEnabledOrX_HoloCache = false;
                Debug.Log("[OrX]: Showing HoloCache JDI GUI 2");
            }
        }

        private void DisableGui()
        {
            if (GuiEnabledOrX_HoloCache)
            {
                Debug.Log("[Module OrX HoloCache]: Hiding HoloCache JDI GUI");
            }

            if (GuiEnabledOrX_HoloCache2)
            {
                Debug.Log("[Module OrX HoloCache]: Hiding HoloCache JDI GUI 2");
            }

            if (craftBrowserOpen)
            {
                Debug.Log("[Module OrX HoloCache]: Hiding Craft Browser");
            }

            GuiEnabledOrX_HoloCache2 = false;
            GuiEnabledOrX_HoloCache = false;
            craftBrowserOpen = false;
            OrXHoloCache.instance.HideGameUI();
        }

        private void GameUiEnableOrXHoloCache()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisableOrXHoloCache()
        {
            _gameUiToggle = false;
        }

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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                "OrX Holo Cache",
                titleStyle);
        }

        private void DrawTitle4(float line)
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

            GUI.Label(new Rect(0, 0, WindowWidth, 20),
                "Name your HoloCache below",
                titleStyle);
        }

        private void DrawcraftToAdd(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                "Bleuprints: "+ craftToAdd,
              titleStyle);
        }

        private void DrawPassword(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Password:",
                leftLabel);
            float textFieldWidth = 80;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            Password = GUI.TextField(fwdFieldRect, Password);
        }

        private void DrawTech(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!techIncluded)
            {
                if (GuiEnabledOrX_HoloCache)
                {
                    if (GUI.Button(saveRect, "ADD TECH", OrXCraftSkin.button))
                    {
                        techIncluded = true;
                    }
                }
                else
                {
                    if (GUI.Button(saveRect, "NO TECH", OrXCraftSkin.button))
                    {
                    }
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Remove Tech", OrXCraftSkin.box))
                {
                    if (GuiEnabledOrX_HoloCache)
                    {
                        tech = "";
                        techIncluded = false;
                    }
                }
            }
        }

        private void DrawModule(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Module: ",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            tech = GUI.TextField(fwdFieldRect, tech);
        }
        
        private void DrawInfected(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!GuiEnabledOrX_HoloCache2)
            {
                if (!spawnInfected)
                {
                    if (GUI.Button(saveRect, "INFECT", OrXCraftSkin.button))
                    {
                        spawnInfected = true;
                    }
                }
                else
                {
                    if (GUI.Button(saveRect, "INFECTED", OrXCraftSkin.box))
                    {
                        spawnInfected = false;
                    }
                }
            }
            else
            {
                if (!spawnInfected)
                {
                }
                else
                {
                    if (GUI.Button(saveRect, "INFECTED", OrXCraftSkin.box))
                    {
                    }
                }

            }

        }

        private void DrawInfectedCount(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(50, (ContentTop + line * entryHeight) + 7, contentWidth * 0.9f, 20), "INFECTION INTENSITY");
            infectedCount = GUI.HorizontalSlider(saveRect, infectedCount, 1, 10);
        }

        private void DrawHoloCacheName(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperLeft;
            leftLabel.normal.textColor = Color.white;

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Holo Name: ",
                leftLabel);
            float textFieldWidth = 100;
            var fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            HoloCacheName = GUI.TextField(fwdFieldRect, HoloCacheName);
        }

        private void DrawHoloCacheName1(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                HoloCacheName,
              titleStyle);

        }

        private void DrawTechIncluded(float line)
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

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                "Tech: " + tech,
              titleStyle);

        }

        private void DrawAddBlueprints(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!blueprintsAdded)
            {
                if (GUI.Button(saveRect, "ADD BLUEPRINTS", OrXCraftSkin.button))
                {
                    craftBrowserOpen = true;
                    GuiEnabledOrX_HoloCache = false;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "REMOVE " + craftToAdd, OrXCraftSkin.box))
                {
                    craftLoc = "";
                    blueprintsAdded = false;
                }
            }
        }

        private void DrawLocalBlueprints(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (!saveLocalVessels)
            {
                if (GUI.Button(saveRect, "SAVE LOCAL", OrXCraftSkin.button))
                {
                    saveLocalVessels = true;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "SAVING LOCAL", OrXCraftSkin.box))
                {
                    saveLocalVessels = false;
                }
            }
        }

        private void DrawLock(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (unlocked)
            {
                if (GUI.Button(saveRect, "LOCK", OrXCraftSkin.button))
                {
                    unlocked = false;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "UNLOCK", OrXCraftSkin.box))
                {
                    unlocked = true;
                    Password = "OrX";
                    Password_ = "OrX";
                }
            }
        }

        private void DrawUnLock(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (unlocked)
            {
                if (GUI.Button(saveRect, "UNLOCKED", OrXCraftSkin.button))
                {
                    //unlocked = false;
                }
            }
            else
            {
                if (GUI.Button(saveRect, "UNLOCK", OrXCraftSkin.button))
                {
                    if (Password == Password_)
                    {
                        unlocked = true;
                    }
                    else
                    {
                        ScreenMsg("<color=#cfc100ff><b>WRONG PASSWORD</b></color>");
                    }
                }
            }
        }

        private void DrawSave(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (!save)
            {
                if (GUI.Button(saveRect, "SAVE HOLOCACHE", OrXCraftSkin.button))
                {
                    if (HoloCacheName == "")
                    {
                        ScreenMsg("Unable to save HoloCache with no name");
                    }
                    else
                    {
                        ConfigNode file = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                        if (file == null)
                        {
                            StartCoroutine(SaveHoloCache());
                        }
                        else
                        {
                            DisableGui();
                            OrXAppendCfg.instance.hcName = HoloCacheName;
                            OrXAppendCfg.instance.EnableGui();
                        }
                    }
                }
            }
        }

        private void DrawCloseGui(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "CLOSE WINDOW", OrXCraftSkin.button))
            {
                DisableGui();

                if (unlocked)
                {
                    ConfigNode file = ConfigNode.Load("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                    foreach (ConfigNode.Value cv in file.values)
                    {
                        if (cv.name == "spawned")
                        {
                            cv.value = "True";
                        }
                    }

                    file.Save("GameData/OrX/HoloCache/" + HoloCacheName + "/" + HoloCacheName + ".orx");
                    ExtractCraftFiles();
                }
                else
                {
                    // Start Code Breaker console
                    Debug.Log("[Module OrX HoloCache] HoloCache is LOCKED ... Start Code Breaker Console");
                    ScreenMsg("HoloCache is LOCKED");
                }
            }
        }

        private void DrawCancel(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);

            if (GUI.Button(saveRect, "CLOSE WINDOW", OrXCraftSkin.button))
            {
                DisableGui();
                this.vessel.DestroyVesselComponents();
                this.vessel.Die();
            }
        }

        #endregion

    }
}
