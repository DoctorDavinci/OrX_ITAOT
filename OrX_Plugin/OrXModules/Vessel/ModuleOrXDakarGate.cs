using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrX
{
    public class ModuleOrXDakarGate : PartModule
    {
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
            if (!vessel.loaded) return;

            CheckDistance(OrXHoloKron.instance._HoloKron);
        }

        private void CheckDistance(Vessel _player)
        {
            double _latDiff = 0;
            double _lonDiff = 0;
            double _altDiff = 0;

            if (_player.altitude <= vessel.altitude)
            {
                _altDiff = vessel.altitude - _player.altitude;
            }
            else
            {
                _altDiff = _player.altitude - vessel.altitude;
            }

            if (vessel.latitude >= 0)
            {
                if (_player.latitude >= vessel.latitude)
                {
                    _latDiff = _player.latitude - vessel.latitude;
                }
                else
                {
                    _latDiff = vessel.latitude - _player.latitude;
                }
            }
            else
            {
                if (_player.latitude >= 0)
                {
                    _latDiff = _player.latitude - vessel.latitude;
                }
                else
                {
                    if (_player.latitude <= vessel.latitude)
                    {
                        _latDiff = _player.latitude - vessel.latitude;
                    }
                    else
                    {

                        _latDiff = vessel.latitude - _player.latitude;
                    }
                }
            }

            if (vessel.longitude >= 0)
            {
                if (_player.longitude >= vessel.longitude)
                {
                    _lonDiff = _player.longitude - vessel.longitude;
                }
                else
                {
                    _lonDiff = vessel.longitude - _player.latitude;
                }
            }
            else
            {
                if (_player.longitude >= 0)
                {
                    _lonDiff = _player.longitude - vessel.longitude;
                }
                else
                {
                    if (_player.longitude <= vessel.longitude)
                    {
                        _lonDiff = _player.longitude - vessel.longitude;
                    }
                    else
                    {

                        _lonDiff = vessel.longitude - _player.longitude;
                    }
                }
            }

            if (Math.Sqrt(((_altDiff * (((2 * (vessel.mainBody.Radius + vessel.altitude)) * Math.PI) / 360)) 
                * (_altDiff * (((2 * (vessel.mainBody.Radius + vessel.altitude)) * Math.PI) / 360))) 
                + (_latDiff * _latDiff) + (_lonDiff * _lonDiff)) 
                * (1 / (((2 * (vessel.mainBody.Radius + vessel.altitude)) * Math.PI) / 360)) >= 2000)
            {
                vessel.rootPart.AddModule("ModuleOrXJason", true);
            }
        }
    }
}