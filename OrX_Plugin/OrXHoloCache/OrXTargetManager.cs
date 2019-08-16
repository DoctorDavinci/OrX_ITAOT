using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;

namespace OrX
{
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class OrXTargetManager : MonoBehaviour
    {
        public static Dictionary<OrXHoloCache.OrXCoords, List<OrXTargetInfo>> TargetDatabase;
		public static Dictionary<OrXHoloCache.OrXCoords, List<OrXHoloCacheinfo>> HoloCacheTargets;

		public static OrXTargetManager instance;
        OrXHoloCache.OrXCoords coords;

        public bool resetHoloCache = false;
        public ConfigNode craft = null;
        public string shipDescription = string.Empty;

        private StringBuilder debugString = new StringBuilder();
        public string craftFile = string.Empty;
        public string craftToSpawn = string.Empty;
        public string sth = string.Empty;
        public string cfgToLoad = string.Empty;
        public string HoloCacheName = string.Empty;
        string OrXv = "OrXv";

        private float updateTimer = 0;

        public Vessel holoCache;

        public double _lat = 0f;
        public double _lon = 0f;
        public double _alt = 0f;

		void Awake()
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
        }

        void OnDestroy()
        {
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

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 5, ScreenMessageStyle.UPPER_CENTER));
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
            OrXHoloCache.instance.reload = false;
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

			string[] OrbitalBodyNames = listString.Split(new char[]{ ':' });

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
                            sth = data[1];
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

		IEnumerator CleanDatabaseRoutine()
		{
			while(enabled)
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

        public void Vreport(Vessel v)
        {
            ReportVessel(v);
        }

        public static void ReportVessel(Vessel v)
        {
            if (!v) return;

            OrXTargetInfo info = v.gameObject.GetComponent<OrXTargetInfo>();
            if (!info)
            {
                List<ModuleOrXHoloCache>.Enumerator jdi = v.FindPartModulesImplementing<ModuleOrXHoloCache>().GetEnumerator();
                while (jdi.MoveNext())
                {
                    if (jdi.Current == null) continue;
                    if (jdi.Current.getGPS)
                    {
                        info = v.gameObject.AddComponent<OrXTargetInfo>();
                        break;
                    }

                }
                jdi.Dispose();
            }

            // add target to database
            if (info)
            {
                AddTarget(info);
                info.detectedTime = Time.time;
            }
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
			foreach(OrXHoloCache.OrXCoords t in TargetDatabase.Keys)
			{
				foreach(OrXTargetInfo target in TargetDatabase[t])
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
    }
}

