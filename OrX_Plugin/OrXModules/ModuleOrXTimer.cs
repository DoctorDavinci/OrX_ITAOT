using OrX.spawn;
using OrXWind;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXTimer : PartModule
    {

        public bool triggered = false;
        double _latDiff = 0;
        double _lonDiff = 0;
        double _altDiff = 0;

        public override void OnStart(StartState state)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                part.force_activate();
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!this.vessel.isActiveVessel)
                {
                    if (!triggered)
                    {
                        if (FlightGlobals.ActiveVessel.altitude <= this.vessel.altitude)
                        {
                            _altDiff = this.vessel.altitude - FlightGlobals.ActiveVessel.altitude;
                        }
                        else
                        {
                            _altDiff = FlightGlobals.ActiveVessel.altitude - this.vessel.altitude;
                        }

                        if (this.vessel.latitude >= 0)
                        {
                            if (FlightGlobals.ActiveVessel.latitude >= this.vessel.latitude)
                            {
                                _latDiff = FlightGlobals.ActiveVessel.latitude - this.vessel.latitude;
                            }
                            else
                            {
                                _latDiff = this.vessel.latitude - FlightGlobals.ActiveVessel.latitude;
                            }
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.latitude >= 0)
                            {
                                _latDiff = FlightGlobals.ActiveVessel.latitude - this.vessel.latitude;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.latitude <= this.vessel.latitude)
                                {
                                    _latDiff = FlightGlobals.ActiveVessel.latitude - this.vessel.latitude;
                                }
                                else
                                {

                                    _latDiff = this.vessel.latitude - FlightGlobals.ActiveVessel.latitude;
                                }
                            }
                        }

                        if (this.vessel.longitude >= 0)
                        {
                            if (FlightGlobals.ActiveVessel.longitude >= this.vessel.longitude)
                            {
                                _lonDiff = FlightGlobals.ActiveVessel.longitude - this.vessel.longitude;
                            }
                            else
                            {
                                _lonDiff = this.vessel.longitude - FlightGlobals.ActiveVessel.longitude;
                            }
                        }
                        else
                        {
                            if (FlightGlobals.ActiveVessel.longitude >= 0)
                            {
                                _lonDiff = FlightGlobals.ActiveVessel.longitude - this.vessel.longitude;
                            }
                            else
                            {
                                if (FlightGlobals.ActiveVessel.longitude <= this.vessel.longitude)
                                {
                                    _lonDiff = FlightGlobals.ActiveVessel.longitude - this.vessel.longitude;
                                }
                                else
                                {

                                    _lonDiff = this.vessel.longitude - FlightGlobals.ActiveVessel.longitude;
                                }
                            }
                        }

                        double diffSqr = (_latDiff * _latDiff) + (_lonDiff * _lonDiff);
                        double _altDiffDeg = _altDiff * OrXHoloKron.instance.degPerMeter;
                        double altAdded = (_altDiffDeg * _altDiffDeg) + diffSqr;
                        double _targetDistance = Math.Sqrt(altAdded) * OrXHoloKron.instance.mPerDegree;

                        if (_targetDistance <= 10)
                        {
                            triggered = true;
                            Debug.Log("======================================= TARGET DISTANCE: " + _targetDistance);
                            List<Part>.Enumerator p = this.vessel.parts.GetEnumerator();
                            while (p.MoveNext())
                            {
                                if (p.Current != null)
                                {
                                    var light = p.Current.FindModuleImplementing<ModuleLight>();
                                    if (light != null)
                                    {
                                        light.lightR = 1;
                                        light.lightG = 0;
                                        light.lightB = 0;
                                    }
                                }
                            }
                            p.Dispose();
                            OrXHoloKron.instance.missionTime = FlightGlobals.ActiveVessel.missionTime;
                            OrXHoloKron.instance.GetNextCoord();
                        }
                    }
                }
                else
                {
                }
            }
        }
    }
}