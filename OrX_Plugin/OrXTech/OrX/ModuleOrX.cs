
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OrX.spawn;

namespace OrX.parts
{
    public class ModuleOrX : PartModule, IPartMassModifier
    {
        #region Fields

        [KSPField(isPersistant = true, guiActiveEditor = true, guiActive = true, guiName = "SPAWN BOIDS"),
             UI_Toggle(controlEnabled = true, scene = UI_Scene.All, disabledText = "Off", enabledText = "On")]
        public bool boids = false;

        public bool narcosis = false;
        public bool bends = false;
        string narcosisText = "Nominal";

        public bool unlockedScuba = true;
        public bool trimUp = false;
        public bool trimDown = false;
        private bool sinking = false;
        private bool rising = false;

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

        private bool toggle = false;
        private float massModifier = 0.0f;

        [KSPField(isPersistant = true)]
        public float trimModifier = 1;
        public float _trimModifier = 1;

        public bool resetTrim = false;
        private bool trimming = false;
        private float trimModCheck = 0.0f;
        public bool outofbreath = false;
        public bool holdDepth = false;
        public bool holdingDepth = false;

        [KSPField(isPersistant = true)]
        public bool infected = false;

        [KSPField(isPersistant = true)]
        public bool orx = false;

        [KSPField(isPersistant = true)]
        public float oxygenMax = 100.0f;
        [KSPField(isPersistant = true)]
        public float oxygen = 100.0f;
        [KSPField(isPersistant = true)]
        public int _scubaLevel = 1;
        public string kerbalName = string.Empty;

        [KSPField(isPersistant = true)]
        public bool helmetRemoved = false;

        ConfigNode file = null;
        private int LockCheck = 0;

        private bool chasing = false;
        private bool guiPauseTrigger = false;
        private bool Nchanged = false;

        Transform snowball;

        #endregion

        /// //////////////////////////////////////////////////////////////
        Guid id;

        private bool setupOrXModule = true;
        public Transform trans;
        private bool throwBall = false;
        public double hoverAlt = 2;

        private bool pilot = false;
        private bool engineer = false;
        private bool scientist = false;
        private bool civilian = false;

        public void AddTransform()
        {
            trans = this.part.gameObject.AddComponent<Transform>();
            trans.parent = this.part.transform;
            trans.forward = this.part.transform.forward;
            trans.up = this.part.transform.up;
            trans.right = this.part.transform.right;
            trans.localPosition = this.part.transform.localPosition + new Vector3(0,0,5);

        }

        public override void OnStart(StartState state)
        {
            Debug.Log(" ========================== setupOrXModule =======================");
            if (!orx)
            {
                OrXLog.instance.AddToVesselList(this.vessel);
            }
            id = FlightGlobals.ActiveVessel.id;
            kerbalName = this.vessel.vesselName;
            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

            _maxJumpForce = kerbal.maxJumpForce;
            _walkSpeed = kerbal.walkSpeed;
            _runSpeed = kerbal.runSpeed;
            _strafeSpeed = kerbal.strafeSpeed;
            _swimSpeed = kerbal.swimSpeed;
            unlockedScuba = true;
            trimModifier = _trimModifier;
            _windowRect = new Rect(Screen.width - 320 - WindowWidth, 140, WindowWidth, _windowHeight);
            _gameUiToggle = true;
            forward = this.part.transform.forward;
            AddTransform();
            base.OnStart(state);
        }
        public void Update()
        {
            if (!FlightGlobals.ready || PauseMenu.isOpen || !vessel.loaded || vessel.HoldPhysics)
                return;

            if (HighLogic.LoadedSceneIsFlight && vessel.isEVA)
            {
                if (!orx)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                        throwBall = true;

                    if (this.vessel.Splashed)
                    {
                        if (vessel.isActiveVessel)
                        {
                            if (!GuiEnabledScuba)
                            {
                                guiopenScuba = true;
                                GuiEnabledScuba = true;
                                controlGUIswitched = true;
                            }

                            if (Input.GetKeyDown(KeyCode.T))
                            {
                                if (!holdDepth)
                                {
                                    holdDepth = true;
                                    ScreenMsg("Holding depth at " + Convert.ToInt32(this.vessel.altitude) + " meters");

                                    if (!holdingDepth)
                                    {
                                        holdingDepth = true;
                                        depth = this.vessel.altitude;
                                        StartCoroutine(DepthCheck());
                                    }
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

                            if (Input.GetKeyDown(KeyCode.Q))
                                trimDown = true;

                            if (Input.GetKeyDown(KeyCode.E))
                                trimUp = true;
                        }
                        else
                        {
                            if (GuiEnabledScuba)
                            {
                                guiopenScuba = false;
                                GuiEnabledScuba = false;

                                if (controlGUIswitched)
                                {
                                    controlGUIswitched = false;
                                }
                            }

                            checkTrim();
                        }

                        if (part.vessel.altitude <= -1)
                        {
                            checkTrim();
                            oxyCheck();

                            if (!bendsCheck)
                            {
                                bendsCheck = true;
                                StartCoroutine(BendsCheck());
                            }
                            if (!narcosisCheck)
                            {
                                narcosisCheck = true;
                                StartCoroutine(NarcosisCheck());
                            }
                        }
                    }
                    else
                    {
                        narcosisCheck = false;
                        bendsCheck = false;
                        massModifier = 0;
                        if (GuiEnabledScuba)
                        {
                            guiopenScuba = false;
                            GuiEnabledScuba = false;
                        }

                        if (controlGUIswitched)
                        {
                            controlGUIswitched = false;
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

                            if (!chasing)
                            {
                                chasing = true;
                                //StartCoroutine(InfectedChase());
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
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (throwBall)
                {
                    throwBall = false;
                    OrXHoloCache.instance.SpawnEmptyHoloCache();
                }
            }
            base.OnFixedUpdate();
        }

        //////////////////////////////////////////////////////////////////////////////

        #region OrX Jet Pack

        Rigidbody rigidBody;
        double hoverHeight = 2;



        #endregion

        //////////////////////////////////////////////////////////////////////////////

        #region Scuba

        bool narcosisCheck = false;
        bool drunk = false;
        bool stumbling = false;
        int martiniLevel = 0;
        double narcosisDepth = -150;
        private bool bendsCheck = false;
        double bendsDepth = -300;
        double bendsTriggerDepth = 0;
        double depthCheck = 0;
        double p1 = 0;
        double p2 = 0;
        private bool bendsTimer = false;
        double depth = 0;

        IEnumerator DepthCheck()
        {
            if (holdDepth)
            {
                if (this.vessel.altitude >= depth * 0.99f)
                {
                    massModifier += 0.4f;
                }

                if (this.vessel.altitude <= depth * 0.99f)
                {
                    massModifier -= 0.4f;
                }

                yield return new WaitForSeconds(1);
                StartCoroutine(DepthCheck());
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

                        if ((-p2 + p1) / 100 >= (1 - (this.vessel.altitude / p2))
                            * ((p2 / this.vessel.altitude)))
                        {
                            bendsTriggerDepth = -p2 + p1;
                            narcosis = true;

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
                        StartCoroutine(BendsCheck());
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
                    kerbal.isRagdoll = false;
                    if (this.vessel.altitude >= bendsTriggerDepth)
                    {
                        yield return new WaitForSeconds(7);
                        kerbal.isRagdoll = true;
                        yield return new WaitForSeconds(0.5f);
                        kerbal.isRagdoll = false;

                        if (this.vessel.altitude >= bendsTriggerDepth)
                        {
                            yield return new WaitForSeconds(5);
                            kerbal.isRagdoll = true;
                            yield return new WaitForSeconds(0.5f);
                            kerbal.isRagdoll = false;
                            yield return new WaitForSeconds(2);
                            bends = true;
                            kerbal.isRagdoll = true;
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
                var kerbal = this.part.FindModuleImplementing<KerbalEVA>();
                martiniLevel = Convert.ToInt32(this.vessel.altitude / (narcosisDepth * _scubaLevel));

                if (!bends)
                {
                    if (martiniLevel >= 1)
                    {
                        narcosis = true;
                        kerbal.swimSpeed = _swimSpeed / martiniLevel * 0.85f;

                        if (martiniLevel >= 3)
                        {
                            if (martiniLevel >=5)
                            {
                                drunk = true;
                                yield return new WaitForSeconds(10);
                                kerbal.isRagdoll = true;
                                yield return new WaitForSeconds(0.5f);
                                kerbal.isRagdoll = false;
                                yield return new WaitForSeconds(7);
                                kerbal.isRagdoll = true;
                                yield return new WaitForSeconds(0.5f);
                                kerbal.isRagdoll = false;
                            }
                            else
                            {
                                drunk = false;
                                yield return new WaitForSeconds(2);
                                kerbal.isRagdoll = true;
                                yield return new WaitForSeconds(0.5f);
                                kerbal.isRagdoll = false;
                                StartCoroutine(NarcosisCheck());
                            }
                        }
                    }
                    else
                    {
                        kerbal.swimSpeed = _swimSpeed;
                        yield return new WaitForSeconds(2);
                        StartCoroutine(NarcosisCheck());
                    }
                }
            }
            else
            {
                narcosisCheck = false;
            }
        }
        private void oxyCheck()
        {
            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

            if (part.vessel.altitude <= -1 && this.vessel.Splashed)
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

            if (oxygen <= 50)
            {
                outofbreath = true;

                if (oxygen <= 25)
                {
                    kerbal.maxJumpForce = _maxJumpForce * 0.50f;
                    kerbal.walkSpeed = _walkSpeed * 0.50f;
                    kerbal.runSpeed = _runSpeed * 0.50f;
                    kerbal.strafeSpeed = _strafeSpeed * 0.50f;
                    kerbal.swimSpeed = _swimSpeed * 0.50f;
                }
                else
                {
                    kerbal.maxJumpForce = _maxJumpForce * 0.75f;
                    kerbal.walkSpeed = _walkSpeed * 0.75f;
                    kerbal.runSpeed = _runSpeed * 0.75f;
                    kerbal.strafeSpeed = _strafeSpeed * 0.75f;
                    kerbal.swimSpeed = _swimSpeed * 0.75f;
                }

                if (oxygen <= 1)
                {
                    kerbal.isRagdoll = true;
                }
            }
            else
            {
                if (outofbreath)
                {
                    outofbreath = false;
                    kerbal.maxJumpForce = _maxJumpForce;
                    kerbal.walkSpeed = _walkSpeed;
                    kerbal.runSpeed = _runSpeed;
                    kerbal.strafeSpeed = _strafeSpeed;
                    kerbal.swimSpeed = _swimSpeed;
                }
            }
        }
        public void checkTrim()
        {
            if (trimUp && !trimming)
            {
                StartCoroutine(TrimUp());
            }

            if (trimDown && !trimming)
            {
                StartCoroutine(TrimDown());
            }
        }
        IEnumerator TrimDown()
        {
            trimming = true;

            var _trimModifier = trimModifier + trimModCheck;
            massModifier = _trimModifier;
            trimModCheck = massModifier;
            yield return new WaitForSeconds(0.25f);
            trimDown = false;
            trimming = false;
        }
        IEnumerator TrimUp()
        {
            trimming = true;

            var _trimModifier = trimModCheck - trimModifier;
            if (_trimModifier >= 0)
            {
                massModifier = _trimModifier;
                trimModCheck = massModifier;
                yield return new WaitForEndOfFrame();
                trimUp = false;
                trimming = false;
            }
            else
            {
                massModifier = 0;
                trimModCheck = massModifier;
                yield return new WaitForEndOfFrame();
                trimUp = false;
                trimming = false;
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

        #region SCUBA KERB GUI

        private const float WindowWidth = 220;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static bool GuiEnabledScuba;
        public static bool HasAddedButton;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        private float _contentWidth;
        private bool _gameUiToggle;
        private float _windowHeight = 250;
        private Rect _windowRect;

        private bool _vesselName = true;
        public string vesselName = string.Empty;
        public float _trim = 1;
        public float _oxygen = 0;
        public bool getOrXScubaTeam = false;

        public bool guiopenScuba = false;

        private bool controlGUIswitched = false;
        private bool pauseCheck = true;

        private void OnGUI()
        {
            if (PauseMenu.isOpen) return;

            if (GuiEnabledScuba)
            {
                _windowRect = GUI.Window(628263315, _windowRect, GuiWindowOrXScuba, "");
            }
        }

        private void TrimUpGUI()
        {
            foreach (Part p in FlightGlobals.ActiveVessel.Parts)
            {
                trimModifier = _trim;
                trimUp = true;
            }
        }

        private void TrimDownGUI()
        {
            foreach (Part p in FlightGlobals.ActiveVessel.Parts)
            {
                trimModifier = _trim;
                trimDown = true;
            }
        }

        #region GUI
        /// <summary>
        /// GUI
        /// </summary>


        private void GuiWindowOrXScuba(int OrXScuba)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;
            _contentWidth = WindowWidth - 2 * LeftIndent;

            DrawTitle();
            DrawOxygenText(line);
            line++;
            DrawOxygen(line);
            if (narcosis)
            {
                line++;
                DrawNarcosis(line);
            }
            //line++;
            //DrawScubaText(line);
            //line++;
            //DrawTrimModifier(line);
            //line++;
            //DrawTrimUp(line);
            //line++;
            //DrawTrimDown(line);


            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
        }

        private void DrawNarcosis(float line)
        {
            var leftLabel = new GUIStyle();
            leftLabel.alignment = TextAnchor.UpperCenter;
            leftLabel.normal.textColor = Color.white;
            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Status: ",
                leftLabel);

            var rightLabel = new GUIStyle();
            rightLabel.alignment = TextAnchor.UpperCenter;
            rightLabel.fontStyle = FontStyle.Bold;

            if (!narcosis)
            {
                rightLabel.normal.textColor = Color.white;
                narcosisText = "Nominal";
            }
            else
            {
                if (!drunk)
                {
                    if (martiniLevel == 1)
                    {
                        rightLabel.normal.textColor = Color.green;
                    }
                    else
                    {
                        if (martiniLevel <= 4)
                        {
                            rightLabel.normal.textColor = Color.yellow;
                        }
                        else
                        {
                            rightLabel.normal.textColor = Color.red;
                        }
                    }
                    narcosisText = martiniLevel + " Martini's";
                }
                else
                {
                    narcosisText = "You're Drunk";
                    rightLabel.normal.textColor = Color.magenta;
                }
            }

            GUI.Label(new Rect(LeftIndent + (contentWidth / 2), ContentTop + line * entryHeight, 140, entryHeight),
                narcosisText, rightLabel);
        }

        private void DrawScubaText(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                "Trim Modifier",
                titleStyle);
        }

        private void DrawTrimModifier(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0");
            GUI.Label(new Rect(100, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|");
            GUI.Label(new Rect(178, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "5");
            trimModifier = GUI.HorizontalSlider(saveRect, _trimModifier, 1, 5);
        }

        private void DrawOxygenText(float line)
        {
            var centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            var titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };

            GUI.Label(new Rect(0, ContentTop + line * entryHeight, WindowWidth, 20),
                "OXYGEN %",
                titleStyle);
        }

        private void DrawOxygen(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            GUI.Label(new Rect(8, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "0");
            GUI.Label(new Rect(90, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "|");
            GUI.Label(new Rect(175, ContentTop + line * entryHeight, contentWidth * 0.9f, 20), "100");
            oxygen = GUI.HorizontalSlider(saveRect, oxygen, 0, 100);
        }

        private void DrawTrimUp(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "Trim Up"))
            {
                TrimUpGUI();
            }
        }

        private void DrawTrimDown(float line)
        {
            var saveRect = new Rect(LeftIndent * 1.5f, ContentTop + line * entryHeight, contentWidth * 0.9f, entryHeight);
            if (GUI.Button(saveRect, "Trim Down"))
            {
                TrimDownGUI();
            }
        }

        private void EnableGuiOrXScuba()
        {
            guiopenScuba = true;
            GuiEnabledScuba = true;
            Debug.Log("[OrXScuba]: Showing OrXScuba GUI");
        }

        private void DisableGuiOrXScuba()
        {
            guiopenScuba = false;
            GuiEnabledScuba = false;
            Debug.Log("[OrXScuba]: Hiding OrXScuba GUI");
        }

        private void DrawTitle()
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
            GUI.Label(new Rect(0, 0, WindowWidth, 20), "OrX Scuba Kerb", titleStyle);
        }

        #endregion

        private void Dummy()
        {
        }
    }


    #endregion

}
