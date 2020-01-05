
namespace OrX
{
    public class ModuleOrXBFC : PartModule
    {
        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "BOOST FLAP"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "Disabled", enabledText = "Enabled")]
        public bool boostFlap = false;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "DEPLOY CONTROL"),
         UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "Normal", enabledText = "Inverted")]
        public bool toggleDeploy = false;

        [KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "DEPLOY SPEED"),
         UI_FloatRange(controlEnabled = true, scene = UI_Scene.All, minValue = 0.0f, maxValue = 100f, stepIncrement = 1f)]
        public float actuatorSpeed = 100f;
        
        [KSPAction("Toggle Boost Flaps")]
        public void actionToggleBF(KSPActionParam param)
        {
            manualDeploy = !manualDeploy;
        }

        private bool manualDeploy = false;
        private bool bfCheck = false;
        public bool deployed = false;

        private ModuleControlSurface bfPart;
        private ModuleControlSurface ControlSurface()
        {
            ModuleControlSurface Control = part.FindModuleImplementing<ModuleControlSurface>();
            return Control;
        }

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
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
            {
                if (boostFlap)
                {
                    if (!bfCheck)
                    {
                        bfCheck = true;
                        bfPart = ControlSurface();
                        bfPart.ignorePitch = true;
                        bfPart.ignoreRoll = true;
                        bfPart.ignoreYaw = true;
                    }

                    if (!manualDeploy)
                    {
                        if (!this.vessel.Landed)
                        {
                            if (!deployed)
                            {
                                deployed = true;
                                bfPart.actuatorSpeed = actuatorSpeed;

                                if (toggleDeploy)
                                {
                                    bfPart.deploy = false;
                                }
                                else
                                {
                                    bfPart.deploy = true;
                                }
                            }
                        }
                        else
                        {
                            deployed = false;

                            if (toggleDeploy)
                            {
                                bfPart.deploy = true;
                            }
                            else
                            {
                                bfPart.deploy = false;
                            }
                        }
                    }
                    else
                    {
                        if (!deployed)
                        {
                            deployed = true;

                            if (toggleDeploy)
                            {
                                bfPart.deploy = false;
                            }
                            else
                            {
                                bfPart.deploy = true;
                            }
                        }
                    }
                }
                else
                {
                    if (bfCheck)
                    {
                        bfCheck = false;
                    }
                }
            }
        }
    }
}
