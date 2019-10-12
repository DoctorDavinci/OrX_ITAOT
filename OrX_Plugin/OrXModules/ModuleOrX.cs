
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OrX
{
    public class ModuleOrX : PartModule, IPartMassModifier
    {
        #region Fields

        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "HoloKronSpawn"),
             UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "Off", enabledText = "On")]
        public bool spawnHoloKron = false;

        [KSPField(isPersistant = true)]
        public bool infected = false;
        [KSPField(isPersistant = true)]
        public bool orx = false;
        [KSPField(isPersistant = true)]
        public bool helmetRemoved = false;

        public bool _narcosisCheck = false;
        public bool holdDepth = false;
        public bool holdingDepth = false;
        public bool narcosis = false;
        public bool bends = false;
        public bool unlockedScuba = true;
        public bool trimUp = false;
        public bool trimDown = false;

        private bool pilot = false;
        private bool engineer = false;
        private bool scientist = false;
        private bool civilian = false;

        [KSPField(isPersistant = true)]
        public float _walkSpeed = 0.0f;
        [KSPField(isPersistant = true)]
        public float _runSpeed = 0.0f;
        [KSPField(isPersistant = true)]
        public float _strafeSpeed = 0.0f;
        [KSPField(isPersistant = true)]
        public float _maxJumpForce = 0.0f;
        [KSPField(isPersistant = true)]
        public float _swimSpeed = 0.0f;
        [KSPField(isPersistant = true)]
        public float oxygenMax = 100.0f;
        [KSPField(isPersistant = true)]
        public float oxygen = 100.0f;
        [KSPField(isPersistant = true)]
        public float trimModifier = 1;
        public float _trimModifier = 1;
        private float massModifier = 0.0f;

        [KSPField(isPersistant = true)]
        public double _scubaLevel = 1;
        [KSPField(isPersistant = true)]
        public double _bendsLevel = 1;
        public double hoverAlt = 2;

        #endregion

        private KerbalEVA kerbal;
        private KerbalEVA kerbalControl()
        {
            KerbalEVA kControl = part.FindModuleImplementing<KerbalEVA>();
            return kControl;
        }

        public override void OnStart(StartState state)
        {
            this.part.force_activate();
            if (!orx)
            {
                OrXLog.instance.AddToVesselList(this.vessel);
            }
            kerbal = kerbalControl();

            _maxJumpForce = kerbal.maxJumpForce;
            _walkSpeed = kerbal.walkSpeed;
            _runSpeed = kerbal.runSpeed;
            _strafeSpeed = kerbal.strafeSpeed;
            _swimSpeed = kerbal.swimSpeed;
            unlockedScuba = true;
            trimModifier = _trimModifier;
            forward = this.part.transform.forward;

            base.OnStart(state);
        }
        public void Update()
        {
            if (!FlightGlobals.ready || PauseMenu.isOpen || !vessel.loaded || vessel.HoldPhysics)
                return;

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!orx)
                {
                    if (FlightGlobals.currentMainBody.atmosphereContainsOxygen)
                    {
                        if (this.vessel.Splashed)
                        {
                            if (!this.vessel.isEVA)
                            {
                                massModifier = 0;
                            }
                            else
                            {
                                if (this.vessel.altitude <= narcosisDepth)
                                {
                                    _scubaLevel += 0.0005f;
                                }

                                if (!drunk)
                                {
                                    if (vessel.isActiveVessel)
                                    {
                                        if (Input.GetKeyDown(KeyCode.Tab))
                                        {
                                            if (!holdDepth)
                                            {
                                                massModifier = 0;
                                                holdDepth = true;
                                            }
                                            else
                                            {
                                                holdDepth = false;
                                                holdingDepth = false;
                                            }
                                        }

                                        if (Input.GetKeyDown(KeyCode.Z))
                                            massModifier = 0;

                                        if (Input.GetKeyDown(KeyCode.X))
                                            massModifier += 10;

                                        if (Input.GetKeyDown(KeyCode.E))
                                            massModifier += 0.4f;

                                        if (Input.GetKeyDown(KeyCode.Q))
                                            massModifier -= 0.4f;
                                    }
                                }
                                else
                                {
                                    this.vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
                                }
                            }

                            if (this.vessel.altitude <= -1)
                            {
                                oxygen -= 0.005f;
                            }
                            else
                            {
                                if (oxygen <= 99.9f)
                                {
                                    oxygen += 0.1f;
                                }
                                else
                                {
                                    oxygen = 100;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (this.vessel.isActiveVessel)
                    {
                        OrXLog.instance.CheckVesselList(this.vessel);
                    }
                    else
                    {
                        if (infected)
                        {
                            if (!orxSetup)
                            {
                                orxSetup = true;
                                //SetupOrXStats();
                            }
                        }
                        else
                        {
                            // SETUP DIALOGUE MENU ACCESS HERE
                            if (pilot)
                            {
                                // ACCESS PILOT STORY
                            }
                            if (engineer)
                            {
                                // ACCESS ENGINEER STORY
                            }
                            if (scientist)
                            {
                                // YADA
                            }
                            if (civilian)
                            {
                                // YADA
                            }
                        }
                    }
                }
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                if (massModifier <= 0)
                {
                    massModifier = 0;
                }
                else
                {
                    if (massModifier >= 15)
                    {
                        massModifier = 15;
                    }
                }

                if (this.vessel.Splashed)
                {
                    if (this.vessel.isActiveVessel)
                    {
                        OrXScubaKerbGUI.instance.drunk = drunk;
                        OrXScubaKerbGUI.instance.oxygen = oxygen;
                        OrXScubaKerbGUI.instance.martiniLevel = martiniLevel;
                    }

                    if (!holdingDepth && holdDepth)
                    {
                        holdingDepth = true;
                        depth = this.vessel.altitude;
                        StartCoroutine(DepthCheck());
                        ScreenMsg("Holding depth at " + Convert.ToInt32(this.vessel.altitude) + " meters");
                    }

                    martiniLevel = this.vessel.altitude / (narcosisDepth * _scubaLevel);
                    if (!narcosisCheck)
                    {
                        narcosisCheck = true;
                        StartCoroutine(NarcosisCheck());
                    }

                    if (drunk)
                    {
                        if (!drunkTank)
                        {
                            drunkTank = true;
                        }

                        if (this.vessel.isActiveVessel && !OrXLog.instance._EVALockWS)
                        {
                            OrXLog.instance.EVALockWS();
                            kerbal = kerbalControl();
                            kerbal.canRecover = false;
                            kerbal.isRagdoll = true;
                        }

                        if (!this.vessel.Landed)
                        {
                            this.part.transform.Rotate(new Vector3(45, 60, 45) * Time.fixedDeltaTime);
                        }
                    }
                    else
                    {
                        if (drunkTank)
                        {
                            drunkTank = false;

                            if (this.vessel.isActiveVessel && OrXLog.instance._EVALockWS)
                            {
                                OrXLog.instance.EVAUnlockWS();
                            }
                            kerbal = kerbalControl();
                            kerbal.canRecover = true;
                            kerbal.isRagdoll = false;
                        }
                    }
                }
                else
                {
                    _scubaLevel = 1;
                    martiniLevel = 0;
                    drunk = false;
                    narcosisCheck = false;
                }

                if (spawnHoloKron)
                {
                    spawnHoloKron = false;
                    if (this.part != this.vessel.rootPart)
                    {
                        double height = 0;
                        bool craftFound = false;
                        List<string> craftFiles = new List<string>(Directory.GetFiles(UrlDir.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/", "*.craft", SearchOption.AllDirectories));
                        if (craftFiles != null)
                        {
                            List<string>.Enumerator craft = craftFiles.GetEnumerator();
                            while (craft.MoveNext())
                            {
                                ConfigNode craftFile = ConfigNode.Load(craft.Current);
                                if (craftFile.GetValue("ship") == this.vessel.vesselName)
                                {
                                    string size = craftFile.GetValue("size");
                                    string[] measurements = size.Split(new char[] { ',' });
                                    height = double.Parse(measurements[1]);
                                    craftFound = true;
                                    break;
                                }
                            }
                            craft.Dispose();

                            if (craftFound)
                            {
                                OrXHoloKron.instance.SpawnByOrX(new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude)
                                    + FlightGlobals.ActiveVessel.transform.up * (((float)height / 2) + 1));
                            }
                            else
                            {
                                ScreenMsg("Unable to spawn HoloKron ... Please get out of your chair");
                                Debug.Log("[Module OrX] === ACTIVE CRAFT FILE NOT FOUND IN SAVE FOLDER ... UNABLE TO SPAWN HOLOKRON ===");
                            }
                        }
                    }
                    else
                    {
                        OrXHoloKron.instance.SpawnByOrX(new Vector3d(FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude, FlightGlobals.ActiveVessel.altitude));
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////

        #region OrX Jet Pack

        Rigidbody rigidBody;
        double hoverHeight = 2;
        bool hover = false;


        #endregion

        //////////////////////////////////////////////////////////////////////////////

        #region Scuba

        bool narcosisCheck = false;

        [KSPField(isPersistant = true)]
        public bool drunk = false;

        [KSPField(isPersistant = true)]
        public bool drunkTank = false;

        [KSPField(isPersistant = true)]
        public double martiniLevel = 0;

        double narcosisDepth = -20;
        double bendsTriggerDepth = 0;
        double depthCheck = 0;
        double p1 = 0;
        double p2 = 0;
        private bool bendsTimer = false;
        double depth = 0;
        double timeAtDepth = 0;
        bool bendsCheck = false;
        bool narcosisHoldDepth = false;

        IEnumerator DepthCheck()
        {
            if (holdDepth)
            {
                if (this.vessel.altitude >= depth * 0.99f)
                {
                    if (massModifier <= 1)
                    {
                        massModifier += 0.2f;
                    }
                }
                else
                {
                    if (this.vessel.altitude <= depth * 0.99f)
                    {
                        if (massModifier >= 0)
                        {
                            massModifier -= 0.2f;
                        }
                    }
                }

                yield return new WaitForFixedUpdate();
                StartCoroutine(DepthCheck());
            }
            else
            {
                holdingDepth = false;
                ScreenMsg("Releasing hold on depth");
            }
        }
        IEnumerator BendsCheck()
        {
            if (this.vessel.Splashed)
            {
                var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

                if (!bends)
                {
                    p1 = this.vessel.altitude;
                    yield return new WaitForSeconds(1);
                    p2 = this.vessel.altitude;

                    if (p2 <= p1)
                    {
                        // Going down
                    }
                    else
                    {
                        // Going up

                        if (martiniLevel >= _scubaLevel)
                        {
                            bendsTriggerDepth = -p2 + p1;

                            if (this.vessel.altitude >= bendsTriggerDepth)
                            {
                                if (!bendsTimer)
                                {
                                    bendsTimer = true;
                                    StartCoroutine(BendsTimer());
                                }
                            }
                        }
                    }

                    if (!bendsTimer)
                    {
                        yield return new WaitForSeconds(5);
                        bendsCheck = false;
                    }
                }
                else
                {
                    kerbal.isRagdoll = true;
                }
            }
            else
            {
                bendsCheck = false;
            }
        }
        IEnumerator BendsTimer()
        {
            if (this.vessel.Splashed)
            {
                var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

                if (bendsTimer)
                {
                    yield return new WaitForSeconds(10);
                    kerbal.isRagdoll = true;
                    yield return new WaitForSeconds(0.5f);
                    massModifier += 0.4f;
                    kerbal.isRagdoll = false;
                    if (this.vessel.altitude >= bendsTriggerDepth)
                    {
                        yield return new WaitForSeconds(7);
                        kerbal.isRagdoll = true;
                        massModifier += 0.4f;
                        yield return new WaitForSeconds(0.5f);
                        kerbal.isRagdoll = false;

                        if (this.vessel.altitude >= bendsTriggerDepth)
                        {
                            yield return new WaitForSeconds(5);
                            kerbal.isRagdoll = true;
                            massModifier += 0.4f;
                            yield return new WaitForSeconds(0.5f);
                            kerbal.isRagdoll = false;
                            yield return new WaitForSeconds(2);
                            bends = true;
                            massModifier += 0.4f;
                            kerbal.isRagdoll = true;
                            bendsCheck = false;
                        }
                        else
                        {
                            bends = false;
                            bendsTimer = false;
                        }
                    }
                    else
                    {
                        narcosis = false;
                        bendsTimer = false;
                    }
                }
            }
        }
        IEnumerator NarcosisCheck()
        {
            if (this.vessel.Splashed)
            {
                kerbal = kerbalControl();

                if (oxygen <= 0)
                {
                    oxygen = 0;
                    if (this.vessel.isActiveVessel)
                    {
                        OrXLog.instance.EVALockWS();
                    }
                    kerbal.canRecover = false;
                    kerbal.isRagdoll = true;
                }
                else
                {
                    if (martiniLevel >= 1)
                    {
                        if (martiniLevel >= 2.5)
                        {
                            if (martiniLevel >= 4.5)
                            {
                                if (martiniLevel >= 6)
                                {
                                    drunk = true;
                                    kerbal.canRecover = false;

                                    yield return new WaitForSeconds(2);
                                    kerbal.isRagdoll = true;
                                    massModifier += 0.4f;
                                    yield return new WaitForSeconds(0.5f);

                                    massModifier += 0.8f;
                                    yield return new WaitForSeconds(2);

                                    massModifier += 1.6f;
                                    yield return new WaitForSeconds(0.5f);
                                    massModifier += 4f;
                                    kerbal.isRagdoll = true;

                                    yield return new WaitForSeconds(0.5f);
                                    massModifier += 8f;
                                    yield return new WaitForSeconds(0.5f);
                                    massModifier += 10f;
                                }
                                else
                                {
                                    if (martiniLevel >= 5.5)
                                    {
                                        if (!holdDepth)
                                        {
                                            holdDepth = true;
                                            StartCoroutine(DepthCheck());
                                        }
                                    }
                                    kerbal.canRecover = true;
                                    yield return new WaitForSeconds(2);
                                    narcosisCheck = false;
                                }
                            }
                            else
                            {
                                drunk = false;
                                kerbal.isRagdoll = false;
                                kerbal.canRecover = true;
                                yield return new WaitForSeconds(2);
                                narcosisCheck = false;
                            }
                        }
                        else
                        {
                            kerbal.canRecover = true;
                            drunk = false;
                            yield return new WaitForSeconds(2);
                            narcosisCheck = false;
                        }
                    }
                    else
                    {
                        kerbal.canRecover = true;
                        drunk = false;
                        yield return new WaitForSeconds(2);
                        narcosisCheck = false;
                    }
                }
            }
            else
            {
                drunk = false;
                narcosisCheck = false;
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////

        #region Kerbal

        //////////////////////////////////////////////////////////////////////////////
        // KERBAL UPDATES
        //////////////////////////////////////////////////////////////////////////////

        private bool orxSetup = false;

        private void SetupOrXStats()
        {
            Debug.Log("[Module OrX] SETUP ORX STATS ========================================");

            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();
            kerbal.maxJumpForce = _maxJumpForce * 0.5f;
            kerbal.walkSpeed = _walkSpeed * 0.7f;
            kerbal.runSpeed = _runSpeed * 0.7f;
            kerbal.strafeSpeed = _strafeSpeed;
            kerbal.swimSpeed = _swimSpeed * 0.7f;
            kerbal.lampOn = false;
            helmetRemoved = true;
        }

        public void ResetOrXStats()
        {
            Debug.Log("[Module OrX] RESET ORX STATS ========================================");

            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();
            kerbal.maxJumpForce = _maxJumpForce;
            kerbal.walkSpeed = _walkSpeed;
            kerbal.runSpeed = _runSpeed;
            kerbal.strafeSpeed = _strafeSpeed;
            kerbal.swimSpeed = _swimSpeed;
        }

        //////////////////////////////////////////////////////////////////////////////

        #endregion

        //////////////////////////////////////////////////////////////////////////////

        #region HELMET
        //////////////////////////////////////////////////////////////////////////////
        // HELMET
        //////////////////////////////////////////////////////////////////////////////

        private void RemoveHelmet()
        {
            Debug.Log("[Module OrX] REMOVING HELMET ========================================");
            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();
            kerbal.lampOn = false;
            helmetRemoved = true;
        }

        private void ShowHelmet()
        {
            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();
            kerbal.lampOn = false;
            helmetRemoved = false;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////

        #region Snowball

        Vector3 forward;


        private void SpawnSnowball()
        {

        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////

        #region Core

        public void setMassModifier(float massModifier)
        {
            this.massModifier = massModifier;
        }

        public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
        {
            return defaultMass * massModifier;
        }

        public ModifierChangeWhen GetModuleMassChangeWhen()
        {
            return ModifierChangeWhen.CONSTANTLY;
        }

        private void ScreenMsg(string msg)
        {
            ScreenMessages.PostScreenMessage(new ScreenMessage(msg, 3f, ScreenMessageStyle.UPPER_CENTER));
        }

        #endregion

    }

}
