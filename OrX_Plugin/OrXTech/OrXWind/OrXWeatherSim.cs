using KSP.UI.Screens;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OrX.wind
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, true)]
    public class OrXWeatherSim : MonoBehaviour
    {
        public static OrXWeatherSim instance;

        public bool setDirection = false;
        public bool manual = false;
        public bool enableWind = false;
        public bool _blowing = false;
        public bool blowing = false;
        public bool random360 = false;

        private float updateTimer = 0;
        public float teaseDelay = 0;
        public float heading = 0;
        public float fivedegree = 0.03125f;
        public float windIntensity = 10;
        public float _wi = 0;
        public float windVariability = 50;
        public float variationIntensity = 50;

        public int variationCount = 0;
        public int count = 8;

        public string _degrees = "0";

        double N = 1;
        double NW = 2;
        double W = 3;
        double SW = 4;
        double S = 5;
        double SE = 6;
        double E = 7;
        double NE = 8;

        public Vector3 windDirection;
        public Vector3 originalWindDirection;

        public Vector3 EastVect;
        public Vector3 NorthVect;
        public Vector3 UpVect;

        public Vector3d vesselLoc;
        List<Vector3d> vectorList;
        List<Vector3d> negativeAltitude;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        private void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                negativeAltitude = new List<Vector3d>();
                vectorList = new List<Vector3d>();
            }
        }

        private void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (enableWind && !_blowing)
                {
                    _blowing = true;
                    Setup();
                }

                if (blowing)
                {
                    if (windIntensity <= 1)
                    {
                        windIntensity = 1;
                    }

                    if (windVariability <= 1)
                    {
                        windVariability = 1;
                    }

                    if (variationIntensity <= 1)
                    {
                        variationIntensity = 1;
                    }

                    if (teaseDelay <= 1)
                    {
                        teaseDelay = 1;
                    }

                    updateTimer -= Time.fixedDeltaTime;

                    if (updateTimer < 0)
                    {
                        int windSpeedMod = new System.Random().Next(1, 50); // generate random wind speed modifier for adjusting intensity of wind to simulate lulls and gusts
                        int random = new System.Random().Next(1, 100); // for deciding if wind speed is to increae or decrease using the wind speed modifier

                        // The below if statement allows for wind intensity changes over time within + or - 10% of the wind intensity setting
                        if (random >= 50) // if random is above 50
                        {
                            //Debug.Log("[Wind} ... Changing wind speed");
                            _wi = windIntensity + (windSpeedMod / 50); // increase wind speed by adding the wind speed modifier divided by 50 to wind speed
                        }
                        else // if random is below 51
                        {
                            //Debug.Log("[Wind} ... Changing wind speed");
                            _wi = windIntensity - (windSpeedMod / 50);// decrese wind speed by subtracting the wind speed modifier divided by 50 to wind speed
                        }

                        int r = new System.Random().Next(5, 30);
                        updateTimer = r;
                    }
                }
            }
            else
            {
                blowing = false;
                _blowing = false;
            }
        }

        private void Setup()
        {
            Debug.Log("[OrX Wind] ... Waking Weather Man ...");
            _degrees = "0";
            List<Vessel>.Enumerator v = FlightGlobals.Vessels.GetEnumerator(); // creat a list of vessels in game and scrutinize each one
            while (v.MoveNext()) // while scrutinizing a vessel
            {
                if (v.Current == null) continue; // if vessel is null/non existant then move to the next vessel
                if (!v.Current.loaded || v.Current.packed) continue; // if current vessel is not loaded or vessel is packed move on to the next vessel
                List<Part>.Enumerator p = FlightGlobals.ActiveVessel.parts.GetEnumerator(); // creat a list of parts in the vessel and scrutinize each one
                while (p.MoveNext()) // while scrutinizing a part
                {
                    if (p.Current == null) continue;
                    if (p.Current.Modules.Contains("ModuleLiftingSurface"))
                    {
                        p.Current.AddModule("ModuleOrXWind");
                        var ms = p.Current.FindModuleImplementing<ModuleOrXWind>();
                        var mls = p.Current.FindModuleImplementing<ModuleLiftingSurface>();
                        ms.deflectionLiftCoeff = mls.deflectionLiftCoeff;
                    }
                }
                p.Dispose(); // dispose of parts list ... remove list from RAM and KSP without it going to the garbage heap/collector

            }
            v.Dispose(); // dispose of vessel list ... remove list from RAM and KSP without it going to the garbage heap/collector

            vectorList.Clear();

            if (manual)
            {
                setDirection = true;
            }

            StartCoroutine(CalcSlope());
        }

        public void ToggleWind()
        {
            if (enableWind) // if Wind is enabled
            {
                blowing = false;
                enableWind = false;
            }
            else // if Wind is not enabled
            {
                enableWind = true;
            }
        }

        IEnumerator CalcSlope()
        {
            // create list for height of terrain at each point (8 cardinal directions)

            if (count >= 0)
            {
                Vector3d th;
                Vector3d a = new Vector3();

                if (count == 1)
                {
                    a.x = vesselLoc.x + fivedegree;
                    a.y = vesselLoc.y;
                }

                if (count == 2)
                {
                    a.x = vesselLoc.x + (fivedegree * 0.7f);
                    a.y = vesselLoc.y + (fivedegree * 0.7f);
                }

                if (count == 3)
                {
                    a.x = vesselLoc.x;
                    a.y = vesselLoc.y + fivedegree;
                }

                if (count == 4)
                {
                    a.x = vesselLoc.x - (fivedegree * 0.7f);
                    a.y = vesselLoc.y + (fivedegree * 0.7f);
                }

                if (count == 5)
                {
                    a.x = vesselLoc.x - fivedegree;
                    a.y = vesselLoc.y;
                }

                if (count == 6)
                {
                    a.x = vesselLoc.x - (fivedegree * 0.7f);
                    a.y = vesselLoc.y - (fivedegree * 0.7f);
                }

                if (count == 7)
                {
                    a.x = vesselLoc.x;
                    a.y = vesselLoc.y - fivedegree;
                }

                if (count == 8)
                {
                    a.x = vesselLoc.x + (fivedegree * 0.7f);
                    a.y = vesselLoc.y - (fivedegree * 0.7f);
                }

                th = FlightGlobals.ActiveVessel.mainBody.GetRelSurfaceNVector(a.x, a.y);

                if (FlightGlobals.ActiveVessel.mainBody.pqsController != null)
                {
                    double t = FlightGlobals.ActiveVessel.mainBody.pqsController.GetSurfaceHeight(th)
                        - FlightGlobals.ActiveVessel.mainBody.pqsController.radius;

                    a.z = t;
                    vectorList.Add(a);

                    if (count == 1)
                    {
                        N = t;
                    }

                    if (count == 2)
                    {
                        NW = t;
                    }

                    if (count == 3)
                    {
                        W = t;
                    }

                    if (count == 4)
                    {
                        SW = t;
                    }

                    if (count == 5)
                    {
                        S = t;
                    }

                    if (count == 6)
                    {
                        SE = t;
                    }

                    if (count == 7)
                    {
                        E = t;
                    }

                    if (count == 8)
                    {
                        NE = t;
                    }

                    count -= 1;
                    StartCoroutine(CalcSlope());
                }
            }
            else
            {
                // if vessel terrain altitude is negative 
                // - find the highest value and use the location as the wind vector start point
                // - use the vessel location as the end point of the wind vector
                // - scan again in 5 minutes or so

                Vector3d startPoint = new Vector3d();
                Vector3d endPoint = new Vector3d();

                if (FlightGlobals.ActiveVessel.terrainAltitude <=0)
                {
                    var h = Math.Max(N, Math.Max(NW, Math.Max(W, Math.Max(SW, Math.Max(S, Math.Max(SE, Math.Max(E, NE)))))));

                    List<Vector3d>.Enumerator loc = vectorList.GetEnumerator();
                    while (loc.MoveNext())
                    {
                        if (loc.Current.z == h)
                        {
                            startPoint = new Vector3d(loc.Current.x, loc.Current.y, 0);
                            endPoint = vesselLoc;
                            endPoint.z = 0;
                        }
                    }
                    loc.Dispose();
                }
                else
                {
                    // if vessel terrain altitude is positive see if list contains negative values

                    var h = Math.Min(N, Math.Min(NW, Math.Min(W, Math.Min(SW, Math.Min(S, Math.Min(SE, Math.Min(E, NE)))))));

                    if (h <= 0)
                    {
                        // - if negative values found, use the highest negative value as the end point of the vector
                        negativeAltitude.Clear();

                        List<Vector3d>.Enumerator loc = vectorList.GetEnumerator();
                        while (loc.MoveNext())
                        {
                            if (loc.Current.z <= 0)
                            {
                                negativeAltitude.Add(loc.Current);
                            }
                        }
                        loc.Dispose();

                        double a = -(FlightGlobals.ActiveVessel.mainBody.pqsController.radius);

                        List<Vector3d>.Enumerator _negativeAltitude = negativeAltitude.GetEnumerator();
                        while (_negativeAltitude.MoveNext())
                        {
                            if (_negativeAltitude.Current.z >= a)
                            {
                                a = _negativeAltitude.Current.z;
                                endPoint = _negativeAltitude.Current;
                            }
                        }
                        _negativeAltitude.Dispose();

                        // get start point - lowest positive value in list

                        negativeAltitude.Clear();
                        double b = 10000;

                        List<Vector3d>.Enumerator _loc = vectorList.GetEnumerator();
                        while (_loc.MoveNext())
                        {
                            if (_loc.Current.z >= 0)
                            {
                                var bb = Math.Min(b, _loc.Current.z);
                                b = bb;
                            }
                        }
                        _loc.Dispose();

                        List<Vector3d>.Enumerator _start = vectorList.GetEnumerator();
                        while (_start.MoveNext())
                        {
                            if (_start.Current.z == b)
                            {
                                startPoint = _start.Current;
                            }
                        }
                        _start.Dispose();
                    }
                    else
                    {
                        /// if no negative values
                        /// 

                        negativeAltitude.Clear();
                        double b = 10000;

                        List<Vector3d>.Enumerator loc = vectorList.GetEnumerator();
                        while (loc.MoveNext())
                        {
                            if (loc.Current.z <= b)
                            {
                                var bb = Math.Min(b, loc.Current.z);
                                b = bb;
                            }
                        }
                        loc.Dispose();

                        List<Vector3d>.Enumerator _loc = vectorList.GetEnumerator();
                        while (_loc.MoveNext())
                        {
                            if (_loc.Current.z == b)
                            {
                                startPoint = new Vector3d(_loc.Current.x, _loc.Current.y, 0);
                            }
                        }
                        _loc.Dispose();

                        // get end point - highest point in ist

                        var h2 = Math.Max(N, Math.Max(NW, Math.Max(W, Math.Max(SW, Math.Max(S, Math.Max(SE, Math.Max(E, NE)))))));

                        List<Vector3d>.Enumerator _loc_ = vectorList.GetEnumerator();
                        while (_loc_.MoveNext())
                        {
                            if (_loc_.Current.z == h2)
                            {
                                endPoint = new Vector3d(_loc_.Current.x, _loc_.Current.y, 0);
                            }
                        }
                        _loc_.Dispose();

                        negativeAltitude.Clear();
                    }
                }

                windDirection = (startPoint - endPoint).normalized;
                originalWindDirection = windDirection;
                count = 8;
                
            }

            if (setDirection)
            {
                _degrees = GetHeading();
                SetDirection();
            }
            else
            {
                yield return new WaitForSeconds(300);

                if (enableWind)
                {
                    vectorList.Clear();
                    blowing = true;

                    if (setDirection)
                    {
                        _degrees = GetHeading();
                        SetDirection();
                    }
                    else
                    {
                        StartCoroutine(CalcSlope());
                    }
                }
                else
                {
                    _blowing = false;
                    blowing = false;
                }
            }
        }

        public string GetHeading()
        {
            UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            heading = Vector3.Angle(windDirection, NorthVect);
            if (Math.Sign(Vector3.Dot(windDirection, EastVect)) < 0)
            {
                heading = 360 - heading; // westward headings become angles greater than 180
            }

            return heading.ToString();
        }

        private void BlowDirectionRandom()
        {
            blowing = true;

            int randomDirection = new System.Random().Next(1, 10); // randomizer for variable wind direction ... for determining if wind direction should change and in which direction
            int randomYaw = new System.Random().Next(1, 100); // amount of wind direction change, if any

            // the following code determines any wind direction changes over time
            if (randomDirection <= 6) // if random direction is below 6
            {
                if (randomDirection >= 2) // if random direction is above 2
                {
                    float angle = Vector3.Angle(windDirection, originalWindDirection);

                    if (angle <= windVariability / 100)
                    {
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by subtracting the randomized yaw divided by 1000 from the wind direction Y vector
                    }
                    else
                    {
                        variationCount += 1;
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector

                        if (variationCount >= 3) // && random360)
                        {
                            originalWindDirection = windDirection;
                            variationCount = 0;
                        }
                    }
                }
            }
            else// if random direction is above 5
            {
                if (randomDirection <= 9) // if random direction is below 9
                {
                    float angle = Vector3.Angle(windDirection, originalWindDirection);

                    if (angle <= windVariability / 100)
                    {
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by adding the randomized yaw divided by 1000 from the wind direction Y vector
                    }
                    else
                    {
                        variationCount += 1;
                        Debug.Log("[Wind} ... Changing direction");
                        windDirection = Quaternion.Euler(0, -randomYaw / (variationIntensity * 10), 0) * windDirection; // Change direction by subtracting the randomized yaw divided by 1000 from the wind direction Y vector

                        if (variationCount >= 3) // && random360)
                        {
                            originalWindDirection = windDirection;
                            variationCount = 0;
                        }
                    }
                }
            }

            StartCoroutine(Tease());
        }

        IEnumerator Tease()
        {
            if (blowing)
            {
                if (windIntensity <= 1)
                {
                    windIntensity = 1;
                }
                if (windVariability <= 1)
                {
                    windVariability = 1;
                }

                if (variationIntensity <= 1)
                {
                    variationIntensity = 1;
                }

                if (teaseDelay <= 1)
                {
                    teaseDelay = 1;
                }

                int windSpeedMod = new System.Random().Next(1, 50); // generate random wind speed modifier for adjusting intensity of wind to simulate lulls and gusts
                int random = new System.Random().Next(1, 100); // for deciding if wind speed is to increae or decrease using the wind speed modifier

                // The below if statement allows for wind intensity changes over time within + or - 10% of the wind intensity setting
                if (random >= 50) // if random is above 50
                {
                    //Debug.Log("[Wind} ... Changing wind speed");
                    _wi = windIntensity + (windSpeedMod / 50); // increase wind speed by adding the wind speed modifier divided by 50 to wind speed
                }
                else // if random is below 51
                {
                    //Debug.Log("[Wind} ... Changing wind speed");
                    _wi = windIntensity - (windSpeedMod / 50);// decrese wind speed by subtracting the wind speed modifier divided by 50 to wind speed
                }
                yield return new WaitForSeconds(teaseDelay);
                BlowDirectionRandom();
            }
        }

        private void SetDirection()
        {
            // using up and east (assumed direction of planetary rotation) I can calculate a north vector
            UpVect = (FlightGlobals.ActiveVessel.transform.position - FlightGlobals.ActiveVessel.mainBody.position).normalized;
            EastVect = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(FlightGlobals.ActiveVessel.CoM).normalized;
            NorthVect = Vector3.Cross(EastVect, UpVect).normalized;
            heading = Vector3.Angle(windDirection, NorthVect);
            if (Math.Sign(Vector3.Dot(windDirection, EastVect)) < 0)
            {
                heading = 360 - heading; // westward headings become angles greater than 180
            }

            if (setDirection)
            {
                setDirection = false;
                var _heading = float.Parse(_degrees);
                windDirection = NorthVect;
                originalWindDirection = windDirection;

                if (_heading >= 360 || _heading <= 0)
                {
                    _heading = 0;
                    _degrees = "0";
                }
                else
                {
                    windDirection = Quaternion.AngleAxis(_heading, UpVect) * NorthVect;
                    originalWindDirection = windDirection;
                }
            }
        }

        public void KillWeatherman()
        {
        }
    }
}