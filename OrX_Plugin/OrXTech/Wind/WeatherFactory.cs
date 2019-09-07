using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace OrXWind
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class WeatherFactory : MonoBehaviour
    {
        public static WeatherFactory instance;

        private bool setup = false;

        public List<Vector3d> HurricaneFactories;
        public List<Vector3d> TornadoFactories;
        public List<Vector3d> SnowFactories;
        public List<Vector3d> IceFactories;

        public List<Vector3d> Hurricanes;
        public List<Vector3d> Tornadoes;
        public List<Vector3d> SnowStorms;
        public List<Vector3d> IceStorms;

        private void Awake()
        {        
            if (instance)
            {
                Destroy(instance);
            }
            instance = this;
        }
        
        private void Start()
        {
            // INITIALIZE FACTORY LOCATION LISTS 
            HurricaneFactories = new List<Vector3d>();
            TornadoFactories = new List<Vector3d>();
            SnowFactories = new List<Vector3d>();
            IceFactories = new List<Vector3d>();

            // INITIALIZE STORM LOCATION LISTS
            Hurricanes = new List<Vector3d>();
            Tornadoes = new List<Vector3d>();
            SnowStorms = new List<Vector3d>();
            IceStorms = new List<Vector3d>();

            // ADD FACTORY LOCATIONS TO FACTORY LOCATION LISTS ... 
            // NEED LOCATIONS ... PERHAPS LOAD FROM USER EDITABLE CONFIG ????????

        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (WindGUI.instance.enableWind)
                {
                    if (!setup)
                    {
                        setup = true;
                    }
                }
                else
                {
                }
            }
        }

        private void HurricaneFactory()
        {
            List<Vector3d>.Enumerator fact = HurricaneFactories.GetEnumerator();
            while (fact.MoveNext())
            {
                double factRandom = new System.Random().Next(1, 100);
                if (factRandom <= 10 / Hurricanes.Count)
                {
                    Hurricanes.Add(fact.Current);
                }
            }
            fact.Dispose();
        }

        private void HurricaneControl()
        {
            if (Hurricanes.Count >= 0)
            {
                List<Vector3d> temp = new List<Vector3d>();
                List<Vector3d>.Enumerator storm = Hurricanes.GetEnumerator();
                while (storm.MoveNext())
                {
                    // using up and east (assumed direction of planetary rotation) I can calculate a north vector
                    Vector3d UpVect = (storm.Current - new Vector3d(storm.Current.x, storm.Current.y, storm.Current.z - 2000).normalized);
                    Vector3d EastVect = FlightGlobals.currentMainBody.getRFrmVel(storm.Current).normalized;
                    Vector3d NorthVect = Vector3.Cross(EastVect, UpVect).normalized;

                    Vector3d NorthWesterlies = ((NorthVect - EastVect).normalized - EastVect).normalized;
                    Vector3d SouthWesterlies = ((-NorthVect - EastVect).normalized - EastVect).normalized;
                    Vector3d NorthTrades = ((NorthVect - (-EastVect)).normalized - (-EastVect)).normalized;
                    Vector3d SouthTrades = ((-NorthVect - (-EastVect)).normalized - (-EastVect)).normalized;

                    Vector3d TropoSphere = new Vector3d();
                    Vector3d MesoSphere = new Vector3d();

                    // get the current storm position
                    Vector3d currentPos = storm.Current;

                    // declare a virtual position to rotate around the active vessel based off of coords
                    Vector3d virtualPos = new Vector3d();

                    // get how many degrees in latitude difference active vessel is in relation to equator
                    // 0.0055555556f is approx 1 degree
                    double degOffset = currentPos.x / 0.0055555556f;

                    if (currentPos.x >= 0) // if in the northern hemisphere
                    {
                        if (currentPos.x <= 0.6)
                        {
                            TropoSphere = NorthWesterlies;
                        }
                        else
                        {
                            TropoSphere = NorthTrades;
                        }

                        if (currentPos.x <= 1 - (0.0055555556f * 3)) // if more than 3 degree from the north pole
                        {
                            if (currentPos.y >= 0) // if in eastern quadrant
                            {
                                if (currentPos.y <= 1 - (0.0055555556f / 2)) // if not more than half a degree from the eastern most point in coords
                                {
                                    virtualPos = new Vector3d(currentPos.y + 0.01111111111,
                       currentPos.x + 0.02222222222 - (0.0002469136 * degOffset), currentPos.z);

                                }
                            }
                            else
                            {
                                if (currentPos.y >= -1 + (0.0055555556f / 2)) // if more than half a degree from the western most point in coords
                                {
                                    virtualPos = new Vector3d(currentPos.x + 0.01111111111,
                       currentPos.y - 0.02222222222 + (0.0002469136 * degOffset), currentPos.z);

                                }
                            }

                            MesoSphere = (virtualPos - currentPos).normalized;
                        }
                        else
                        {
                            // MesoSphere wind direction should be east while less than 3 degrees from the poles
                            MesoSphere = EastVect;
                        }
                    }
                    else // if in southern hemisphere
                    {
                        if (currentPos.x >= -0.6)
                        {
                            TropoSphere = SouthWesterlies;
                        }
                        else
                        {
                            TropoSphere = SouthTrades;
                        }

                        if (currentPos.x >= -1 + (0.0055555556f * 3)) // if more than 3 degree from the south pole
                        {
                            if (currentPos.y >= 0) // if in eastern quadrant
                            {
                                if (currentPos.y <= 1 - (0.0055555556f / 2)) // if not more than half a degree from the eastern most point in coords
                                {
                                    virtualPos = new Vector3d(currentPos.x - 0.01111111111,
                       currentPos.y + 0.02222222222 - (0.0002469136 * degOffset), currentPos.z);

                                }
                            }
                            else
                            {
                                if (currentPos.y >= -1 + (0.0055555556f / 2)) // if more than half a degree from the western most point in coords
                                {
                                    virtualPos = new Vector3d(currentPos.x - 0.01111111111,
                       currentPos.y - 0.02222222222 + (0.0002469136 * degOffset), currentPos.z);

                                }
                            }
                            MesoSphere = (virtualPos - currentPos).normalized;

                        }
                        else
                        {
                            // MesoSphere wind direction should be east while less than 3 degrees from the poles
                            MesoSphere = EastVect;
                        }
                    }

                    GeneralWindDirection = (TropoSphere - MesoSphere).normalized;
                    windDirection = GeneralWindDirection;



                }
                storm.Dispose();


            }
        }

        private void TornadoFactory()
        {
            List<Vector3d>.Enumerator fact = TornadoFactories.GetEnumerator();
            while (fact.MoveNext())
            {

            }

        }

        private void SnowFactory()
        {
            List<Vector3d>.Enumerator fact = SnowFactories.GetEnumerator();
            while (fact.MoveNext())
            {

            }


        }

        private void IceFactory()
        {
            List<Vector3d>.Enumerator fact = IceFactories.GetEnumerator();
            while (fact.MoveNext())
            {

            }

        }

    }
}