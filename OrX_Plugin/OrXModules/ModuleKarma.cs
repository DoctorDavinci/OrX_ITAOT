using System;
using System.Collections.Generic;

namespace OrX
{
    public class ModuleKarma : PartModule
    {
        public bool kill = false;
        double targetDistance = 250000;
        double _latDiff = 0;
        double _lonDiff = 0;
        double _altDiff = 0;
        bool _checking = true;
        public double mPerDegree = 0;
        public double degPerMeter = 0;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
                mPerDegree = (((2 * (FlightGlobals.ActiveVessel.mainBody.Radius + FlightGlobals.ActiveVessel.altitude)) * Math.PI) / 360);
                degPerMeter = 1 / mPerDegree;
            }
            base.OnStart(state);
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                //this.vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                if (this.vessel.isActiveVessel)
                {
                    List<Vessel>.Enumerator v = FlightGlobals.VesselsLoaded.GetEnumerator();
                    while (v.MoveNext())
                    {
                        if (v.Current != null)
                        {
                            if (v.Current.rootPart.Modules.Contains<KerbalEVA>())
                            {
                                if (vessel.altitude <= v.Current.altitude)
                                {
                                    _altDiff = v.Current.altitude - vessel.altitude;
                                }
                                else
                                {
                                    _altDiff = vessel.altitude - v.Current.altitude;
                                }

                                if (v.Current.latitude >= 0)
                                {
                                    if (vessel.latitude >= v.Current.latitude)
                                    {
                                        _latDiff = vessel.latitude - v.Current.latitude;
                                    }
                                    else
                                    {
                                        _latDiff = v.Current.latitude - vessel.latitude;
                                    }
                                }
                                else
                                {
                                    if (vessel.latitude >= 0)
                                    {
                                        _latDiff = vessel.latitude - v.Current.latitude;
                                    }
                                    else
                                    {
                                        if (vessel.latitude <= v.Current.latitude)
                                        {
                                            _latDiff = vessel.latitude - v.Current.latitude;
                                        }
                                        else
                                        {

                                            _latDiff = v.Current.latitude - vessel.latitude;
                                        }
                                    }
                                }

                                if (v.Current.longitude >= 0)
                                {
                                    if (vessel.longitude >= v.Current.longitude)
                                    {
                                        _lonDiff = vessel.longitude - v.Current.longitude;
                                    }
                                    else
                                    {
                                        _lonDiff = v.Current.longitude - vessel.latitude;
                                    }
                                }
                                else
                                {
                                    if (vessel.longitude >= 0)
                                    {
                                        _lonDiff = vessel.longitude - v.Current.longitude;
                                    }
                                    else
                                    {
                                        if (vessel.longitude <= v.Current.longitude)
                                        {
                                            _lonDiff = vessel.longitude - v.Current.longitude;
                                        }
                                        else
                                        {

                                            _lonDiff = v.Current.longitude - vessel.longitude;
                                        }
                                    }
                                }

                                double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                                double _altDiffDeg = _altDiff * degPerMeter;
                                double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                                double _targetDistance = Math.Sqrt(altAdded) * mPerDegree;

                                if (_targetDistance <= 1)
                                {
                                    v.Current.rootPart.explosionPotential *= 0.2f;
                                    v.Current.rootPart.explode();
                                }
                            }
                        }
                    }
                    v.Dispose();
                }
            }
        }
    }
}