
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OrX;

namespace OrX.parts
{
    public class ModuleOrX : PartModule, IPartMassModifier
    {
        #region Fields

        public bool narcosis = false;
        public bool bends = false;

        public bool unlockedScuba = false;
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
        public float trimModifier = 2;

        public bool resetTrim = false;
        private bool trimming = false;
        private float trimModCheck = 0.0f;
        public bool outofbreath = false;

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


        #endregion

        /// //////////////////////////////////////////////////////////////
        Guid id;

        private bool setupOrXModule = true;

        public override void OnStart(StartState state)
        {
            setupOrXModule = true;
            base.OnStart(state);
        }

        public void Update()
        {
            if (!FlightGlobals.ready || PauseMenu.isOpen || !vessel.loaded || vessel.HoldPhysics)
                return;

            if (HighLogic.LoadedSceneIsFlight && vessel.isEVA)
            {
                if (setupOrXModule)
                {
                    setupOrXModule = false;

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

                    file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");

                    if (file != null && file.HasNode("OrX"))
                    {
                        ConfigNode node = file.GetNode("OrX");

                        foreach (ConfigNode.Value value in node.nodes)
                        {
                            string cvEncryptedName = OrXLog.instance.Decrypt(value.name);
                            if (cvEncryptedName == "unlockedScuba")
                            {
                                string cvEncryptedValue = OrXLog.instance.Decrypt(value.value);

                                if (cvEncryptedValue == "True")
                                {
                                    unlockedScuba = true;
                                }
                            }
                        }
                    }
                    _windowRect = new Rect(Screen.width - 320 - WindowWidth, 140, WindowWidth, _windowHeight);
                    GameEvents.onHideUI.Add(GameUiDisableOrXScuba);
                    GameEvents.onShowUI.Add(GameUiEnableOrXScuba);
                    _gameUiToggle = true;
                }

                if (!orx)
                {
                    if (narcosis)
                    {
                        if (!Nchanged)
                        {
                            Debug.Log(" ========================== NARCOSIS =======================");
                            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();
                            Nchanged = true;
                            kerbal.walkSpeed = _walkSpeed / 1.3f;
                            kerbal.runSpeed = _runSpeed / 2;
                            kerbal.swimSpeed = _swimSpeed / 2;
                            kerbal.maxJumpForce = _maxJumpForce / 2;
                        }
                    }
                    else
                    {
                        if (Nchanged)
                        {
                            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();
                            Nchanged = false;
                            kerbal.walkSpeed = _walkSpeed;
                            kerbal.runSpeed = _runSpeed;
                            kerbal.swimSpeed = _swimSpeed;
                            kerbal.maxJumpForce = _maxJumpForce;
                        }
                    }

                    if (unlockedScuba)
                    {
                        if (bends)
                        {
                            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

                            if (GuiEnabledScuba)
                            {
                                guiopenScuba = false;
                                GuiEnabledScuba = false;
                            }

                            if (!kerbal.isRagdoll)
                            {
                                kerbal.isRagdoll = true;
                            }
                        }
                        else
                        {
                            if (pauseCheck)
                            {
                                if (PauseMenu.isOpen)
                                {
                                    pauseCheck = false;
                                    if (GuiEnabledScuba)
                                    {
                                        guiPauseTrigger = true;
                                        DisableGuiOrXScuba();
                                    }
                                }
                            }
                            else
                            {
                                if (!PauseMenu.isOpen)
                                {
                                    if (!pauseCheck)
                                    {
                                        pauseCheck = true;
                                        if (guiPauseTrigger)
                                        {
                                            guiPauseTrigger = false;
                                            EnableGuiOrXScuba();
                                        }
                                    }
                                }
                                else
                                {
                                    if (GuiEnabledScuba)
                                    {
                                        DisableGuiOrXScuba();
                                    }
                                }
                            }

                            if (GuiEnabledScuba)
                            {
                                ScubaCheck();
                            }

                            if (vessel.isActiveVessel)
                            {
                                if (vessel.Splashed)
                                {
                                    if (!GuiEnabledScuba)
                                    {
                                        guiopenScuba = true;
                                        GuiEnabledScuba = true;

                                        if (!controlGUIswitched)
                                        {
                                            controlGUIswitched = true;
                                        }
                                    }

                                    checkTrim();

                                    if (part.vessel.altitude <= -1)
                                    {
                                        oxyCheck();
                                    }
                                }
                                else
                                {
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
                                if (vessel.Splashed)
                                {
                                    checkTrim();

                                    if (part.vessel.altitude <= -1)
                                    {
                                        oxyCheck();

                                        if (part.vessel.altitude <= (bendsDepth * _scubaLevel))
                                        {
                                            if (!bendsCheck)
                                            {
                                                bendsCheck = true;
                                                StartCoroutine(BendsCheck());
                                            }
                                        }
                                        else
                                        {
                                            bendsCheck = true;
                                        }
                                    }
                                    else
                                    {
                                        bendsCheck = true;
                                    }

                                    if (Input.GetKeyDown(KeyCode.Z))
                                        massModifier = 0;

                                    if (Input.GetKeyDown(KeyCode.X))
                                        massModifier = 50;

                                    if (Input.GetKeyDown(KeyCode.Q))
                                        trimDown = true;

                                    if (Input.GetKeyDown(KeyCode.E))
                                        trimUp = true;
                                }
                                else
                                {
                                    massModifier = 0;
                                }
                            }

                            if (vessel.Splashed)
                            {
                            }

                            if (part.vessel.altitude <= -1)
                            {
                                oxyCheck();
                            }
                        }
                    }
                }
                else
                {
                    if (this.vessel.isActiveVessel)
                    {
                        foreach (Vessel v in FlightGlobals.Vessels)
                        {
                            if (v.id == id)
                            {
                                FlightGlobals.ForceSetActiveVessel(v);
                            }
                        }
                    }
                    else
                    {
                        if (!FlightGlobals.ActiveVessel.isEVA)
                        {
                            this.vessel.DestroyVesselComponents();
                            this.vessel.Die();
                        }
                    }

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

        private bool pilot = false;
        private bool engineer = false;
        private bool scientist = false;
        private bool civilian = false;


        private void pilotStory()
        {

        }

        //////////////////////////////////////////////////////////////////////////////

        #region Scuba

        private bool bendsCheck = false;
        double bendsDepth = -300;
        double bendsTriggerDepth = 0;
        double depthCheck = 0;
        double p1 = 0;
        double p2 = 0;
        private bool narcosisTimer = false;

        IEnumerator BendsCheck()
        {
            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

            if (!bends)
            {
                p1 = this.vessel.altitude;
                yield return new WaitForSeconds(2);
                p2 = this.vessel.altitude;
                depthCheck = this.vessel.altitude;

                if (p2 <= p1)
                {
                    // Going down
                }
                else
                {
                    // Going up

                    if ((-p2 + p1) / 100 >= (1 - (this.vessel.altitude / depthCheck)) 
                        * ((depthCheck / this.vessel.altitude)))
                    {
                        bendsTriggerDepth = -p2 + p1;
                        narcosis = true;

                        if (this.vessel.altitude >= bendsTriggerDepth)
                        {
                            if (!narcosisTimer)
                            {
                                narcosisTimer = true;
                                StartCoroutine(NarcosisTimer());
                            }
                        }
                    }
                }

                if (!narcosis)
                {
                    yield return new WaitForSeconds(5);

                    if (this.vessel.altitude <= (bendsDepth * _scubaLevel))
                    {
                        StartCoroutine(BendsCheck());
                    }
                    else
                    {
                        kerbal.swimSpeed = _swimSpeed;
                        bendsCheck = false;
                    }
                }
            }
            else
            {
                kerbal.isRagdoll = true;
            }
        }

        IEnumerator NarcosisTimer()
        {
            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

            if (narcosisTimer)
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
                    }
                    else
                    {
                        narcosis = false;
                        narcosisTimer = false;
                    }
                }
                else
                {
                    narcosis = false;
                    narcosisTimer = false;
                }
            }
        }

        public void CheckLock()
        {
            file = ConfigNode.Load("GameData/OrX/Plugin/PluginData/OrX.data");

            if (file != null)
            {
                ConfigNode node = file.GetNode("OrX");

                foreach (ConfigNode.Value cv in node.values)
                {
                    string cvEncryptedName = OrXLog.instance.Decrypt(cv.name);
                    if (cvEncryptedName == "unlockedScuba")
                    {
                        string cvEncryptedValue = OrXLog.instance.Decrypt(cv.value);

                        if (cvEncryptedValue == "True")
                        {
                            unlockedScuba = true;
                        }
                    }
                }
            }
        }

        private void oxyCheck()
        {
            var kerbal = this.part.FindModuleImplementing<KerbalEVA>();

            if (part.vessel.altitude <= -1 && this.vessel.Splashed)
            {
                oxygen -= 0.015f;
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

            if (GuiEnabledScuba && _gameUiToggle)
            {
                _windowRect = GUI.Window(628263315, _windowRect, GuiWindowOrXScuba, "");
            }
        }

        private void ScubaCheck()
        {
            foreach (Part p in FlightGlobals.ActiveVessel.Parts)
            {
                if (p.vessel.isEVA)
                {
                    var scuba = p.vessel.FindPartModuleImplementing<ModuleOrX>();
                    _oxygen = (scuba.oxygen / scuba.oxygenMax);
                }
                else
                {
                    GuiEnabledScuba = false;
                }
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
            line++;
            DrawScubaText(line);
            line++;
            DrawTrimModifier(line);
            line++;
            DrawTrimUp(line);
            line++;
            DrawTrimDown(line);


            _windowHeight = ContentTop + line * entryHeight + entryHeight + (entryHeight / 2);
            _windowRect.height = _windowHeight;
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
            _trim = GUI.HorizontalSlider(saveRect, _trim, 0, 5);
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
            _oxygen = GUI.HorizontalSlider(saveRect, _oxygen, 0, 100);
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


        private void GameUiEnableOrXScuba()
        {
            _gameUiToggle = true;
            GuiEnabledScuba = true;
        }

        private void GameUiDisableOrXScuba()
        {
            _gameUiToggle = false;
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
