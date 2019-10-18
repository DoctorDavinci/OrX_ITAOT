
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

        [KSPField(isPersistant = false, guiActiveEditor = false, guiActive = true, guiName = "HOLOKRONSPAWN"),
             UI_Toggle(controlEnabled = true, scene = UI_Scene.Flight, disabledText = "", enabledText = "")]
        public bool spawnHoloKron = false;

        [KSPField(unfocusedRange = 1, isPersistant = false, guiActiveEditor = false, guiActive = false, guiName = "REVIVE"),
             UI_Toggle(controlEnabled = true, scene = UI_Scene.Flight, disabledText = "", enabledText = "")]
        public bool revive = false;

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
        public double _bendsDepth = 0;
        public double hoverAlt = 2;

        float localScale = 0;

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
            localScale = this.part.transform.localScale.x;
            base.OnStart(state);
        }
        public void Update()
        {
            if (!FlightGlobals.ready || PauseMenu.isOpen || !vessel.loaded || vessel.HoldPhysics)
                return;

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!bends)
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
                                    if (!drunk)
                                    {
                                        if (vessel.isActiveVessel)
                                        {
                                            if (Input.GetKeyDown(KeyCode.Tab))
                                            {
                                                if (!holdDepth)
                                                {
                                                    massModifier = 0.4f;
                                                    holdDepth = true;
                                                    ScreenMsg("Holding depth at " + Convert.ToInt32(this.vessel.altitude) + " meters");
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
                                        warningLight = true;
                                        _wlFlash = false;
                                    }

                                    if (warningLight)
                                    {
                                        if (!_wlFlash)
                                        {
                                            this.vessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
                                        }
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
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (HighLogic.LoadedSceneIsFlight && this.vessel.loaded)
            {
                if (revive)
                {
                    if (!bends)
                    {
                        if (drunk && this.vessel.Splashed)
                        {
                            _scubaLevel += 0.01f;
                            drunk = false;
                        }
                    }
                    else
                    {
                        int r = new System.Random().Next(1, 10);
                        if (r >= 2)
                        {
                            if (r >= 8)
                            {
                                this.vessel.Translate((this.vessel.ReferenceTransform.position - this.vessel.mainBody.transform.position).normalized * 2);
                            }
                            else
                            {
                                StartCoroutine(PopKornRevival());
                            }
                        }
                        else
                        {
                            this.part.explosionPotential *= 0.2f;
                            this.part.explode();
                        }
                    }
                }

                if (massModifier <= 0)
                {
                    massModifier = 0;
                }
                else
                {
                    if (massModifier >= 20)
                    {
                        massModifier = 20;
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

                    if (this.vessel.altitude <= narcosisDepth * _scubaLevel)
                    {
                        _scubaLevel += 0.0001f;

                        if (_bendsDepth >= this.vessel.altitude)
                        {
                            _bendsDepth = this.vessel.altitude;
                        }
                    }

                    if (_scubaLevel >= 1.00002f)
                    {
                        if (this.vessel.altitude >= (_bendsDepth * 0.667f) * _scubaLevel)
                        {
                            bends = true;
                        }
                        else
                        {
                            if (this.vessel.altitude >= _bendsDepth * 0.9f)
                            {
                                _bendsDepth += 0.1f;
                                _scubaLevel -= 0.00001f;
                            }
                        }
                    }

                    if (!bends)
                    {
                        if (!holdingDepth && holdDepth)
                        {
                            holdingDepth = true;
                            depth = this.vessel.altitude;
                            StartCoroutine(DepthCheck());
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
                                MiniMe();
                            }

                            if (!this.vessel.Landed)
                            {
                                this.part.transform.Rotate(new Vector3(42, 42, 42) * Time.fixedDeltaTime);
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
                        if (this.vessel.rootPart.Modules.Contains<KerbalEVA>() && _scubaLevel >= 1.0002f)
                        {
                            if (!poppingKorn)
                            {
                                poppingKorn = true;
                                PopKorn();
                            }
                        }
                    }
                }
                else
                {
                    _scubaLevel = 1;
                    martiniLevel = 0;
                    drunk = false;
                    revive = false;
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

        bool poppingKorn = false;

        private void PopKorn()
        {
            bends = true;
            holdingDepth = false;
            holdDepth = false;
            massModifier = 0;
            kerbal = kerbalControl();
            kerbal.canRecover = false;
            kerbal.isRagdoll = true;
            this.vessel.UpdateCaches();
            localScale = this.part.transform.localScale.x * 2;
            StartCoroutine(PopGoesTheKerbal());
        }

        Vector3 _localScale;
        IEnumerator PopGoesTheKerbal()
        {
            if (this.part.transform.localScale.x <= localScale)
            {
                yield return new WaitForSeconds(1);
                this.part.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                StartCoroutine(PopGoesTheKerbal());
            }
            else
            {
                int r = new System.Random().Next(1, 10);
                if (r >= 2)
                {
                    this.part.explosionPotential *= 0.2f;
                    this.part.explode();
                }
            }
        }

        IEnumerator PopKornRevival()
        {
            if (this.part.transform.localScale.x >= localScale)
            {
                yield return new WaitForSeconds(1);
                this.part.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                StartCoroutine(PopKornRevival());
            }
        }

        private void MiniMe()
        {
            bends = false;
            //holdingDepth = false;
            //holdDepth = false;
            //massModifier = 0;
            kerbal = kerbalControl();
            kerbal.canRecover = false;
            kerbal.isRagdoll = true;
            this.vessel.UpdateCaches();
            //massModifier = 5;
            localScale = this.part.transform.localScale.x / 2;
            StartCoroutine(MiniMeKerbal());
        }

        IEnumerator MiniMeKerbal()
        {
            if (this.part.transform.localScale.x >= localScale)
            {
                yield return new WaitForSeconds(1);
                this.part.transform.localScale += new Vector3(-0.05f, -0.05f, -0.05f);
                StartCoroutine(MiniMeKerbal());
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
        double depthCheck = 0;
        double p1 = 0;
        double p2 = 0;
        private bool bendsTimer = false;
        double depth = 0;
        double timeAtDepth = 0;
        bool bendsCheck = false;
        bool narcosisHoldDepth = false;
        bool warningLight = false;
        bool _wlFlash = false;

        IEnumerator DepthCheck()
        {
            if (holdDepth)
            {
                if (this.vessel.altitude >= depth)
                {
                    if (_bendsDepth - (narcosisDepth * 2.5f) >= this.vessel.altitude)
                    {
                        if (massModifier <= 1)
                        {
                            massModifier += 0.1f;
                        }
                    }
                }
                else
                {
                    if (this.vessel.altitude <= depth)
                    {
                        if (this.vessel.altitude >= depth + narcosisDepth)
                        {
                            if (massModifier >= 0)
                            {
                                massModifier -= 0.1f;
                            }
                        }
                        else
                        {
                            holdDepth = false;
                            massModifier = 0.5f;
                        }
                    }
                }

                yield return new WaitForSeconds(0.5f);

                StartCoroutine(DepthCheck());
            }
            else
            {
                holdingDepth = false;
                ScreenMsg("Releasing hold on depth");
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
                        if (martiniLevel >= 2.7)
                        {
                            if (martiniLevel >= 4.5)
                            {
                                warningLight = true;
                                _wlFlash = false;

                                if (martiniLevel >= 6)
                                {
                                    drunk = true;
                                    kerbal.canRecover = false;
                                    kerbal.isRagdoll = true;
                                }
                                else
                                {
                                    if (martiniLevel >= 5.3)
                                    {
                                        depth += 0.01f;
                                        if (!holdDepth)
                                        {
                                            holdDepth = true;
                                            holdingDepth = false;
                                        }
                                    }
                                    kerbal.canRecover = true;
                                    yield return new WaitForSeconds(1);
                                }
                            }
                            else
                            {
                                warningLight = true;
                                _wlFlash = false;
                                yield return new WaitForSeconds(1);
                                _wlFlash = true;
                                drunk = false;
                                kerbal.isRagdoll = false;
                                kerbal.canRecover = true;
                                yield return new WaitForSeconds(1);
                            }
                        }
                        else
                        {
                            warningLight = true;
                            _wlFlash = false;
                            yield return new WaitForSeconds(1);
                            _wlFlash = true;
                            kerbal.canRecover = true;
                            drunk = false;
                            yield return new WaitForSeconds(1);
                        }
                    }
                    else
                    {
                        warningLight = false;
                        kerbal.canRecover = true;
                        drunk = false;
                        yield return new WaitForSeconds(1);
                    }
                }
            }
            else
            {
                warningLight = false;
                drunk = false;
            }
            _wlFlash = false;
            narcosisCheck = false;
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
