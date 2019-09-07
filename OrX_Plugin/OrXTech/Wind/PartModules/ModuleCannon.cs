using System;
using System.Linq;
using System.Collections.Generic;
using KSP.UI.Screens;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace OrXWind
{
    public class ModuleCannon : PartModule
    {
        [KSPField]
        public AnimationState[] deployStates;
        [KSPField(isPersistant = false)]
        public bool hasDeployAnimation = true;
        [KSPField(isPersistant = true)]
        public string deployAnimName = "17CDeploy";

        private bool deploy = false;
        private bool retract = false;

        public override void OnStart(PartModule.StartState state)
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                try
                {
                    RightVect = this.vessel.transform.right;
                    ForwardVect = this.vessel.transform.forward;
                    facing = Vector3.Angle(this.part.transform.forward, ForwardVect);
                    if (Math.Sign(Vector3.Dot(this.part.transform.forward, RightVect)) < 0)
                    {
                        facing = 360 - facing;
                    }

                    if (facing <= 45 || facing >= 315)
                    {
                        forwardFacing = true;
                    }
                    else
                    {
                        if (facing <= 135 || facing >= 45)
                        {
                            rightFacing = true;
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }

            if (HighLogic.LoadedSceneIsFlight)
            {
                this.enabled = true;
                this.part.force_activate();
                fireTransform = part.FindModelTransform(fireTransformName);
                GetGun();
            }
            if (deployAnimName != "")
            {
                deployStates = SetUpAnimation(deployAnimName, this.part);
            }
            if (hasDeployAnimation)
            {
                deployStates = SetUpAnimation(deployAnimName, this.part);
                foreach (AnimationState anim in deployStates)
                {
                    anim.enabled = false;
                }
            }
            base.OnStart(state);
        }

        public override void OnFixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight) return;

            if (deploy)
            {
                deploy = false;
                retract = false;

                foreach (AnimationState anim in deployStates)
                {
                    //animation clamping
                    if (anim.normalizedTime > 1)
                    {
                        anim.speed = 0;
                        anim.normalizedTime = 1;
                    }
                    if (anim.normalizedTime < 0)
                    {
                        anim.speed = 0;
                        anim.normalizedTime = 0;
                    }
                    anim.enabled = true;
                    if (anim.normalizedTime < 1 && anim.speed < 1)
                    {
                        anim.speed = 1;
                    }
                    if (anim.normalizedTime == 1)
                    {
                        anim.enabled = false;
                    }
                }
            }

            if (retract)
            {
                deploy = false;
                retract = false;

                foreach (AnimationState anim in deployStates)
                {
                    //animation clamping
                    if (anim.normalizedTime > 1)
                    {
                        anim.speed = 0;
                        anim.normalizedTime = 1;
                    }
                    if (anim.normalizedTime < 0)
                    {
                        anim.speed = 0;
                        anim.normalizedTime = 0;
                    }
                    anim.enabled = true;
                    if (anim.normalizedTime > 0 && anim.speed > -1)
                    {
                        anim.speed = -1;
                    }
                    if (anim.normalizedTime == 0)
                    {
                        anim.enabled = false;
                    }
                }
            }
        }

        public static AnimationState[] SetUpAnimation(string animationName, Part part)  //Thanks Majiir!(From BDA)
        {
            var states = new List<AnimationState>();
            foreach (var animation in part.FindModelAnimators(animationName))
            {
                var animationState = animation[animationName];
                animationState.speed = 0;
                animationState.enabled = true;
                animationState.wrapMode = WrapMode.ClampForever;
                animation.Blend(animationName);
                states.Add(animationState);
            }
            return states.ToArray();
        }

        [KSPAction("Fire Cannon")]
        public void actionFire(KSPActionParam param)
        {
            Fire();
        }

        [KSPAction("Fire Barrage")]
        public void actionBarrage(KSPActionParam param)
        {
            FireBarrage();
        }

        public Transform fireTransform;

        private bool barrage = false;
        public bool rightFacing = false;
        public bool leftFacing = false;
        public bool rearFacing = false;
        public bool forwardFacing = false;
        private bool loading = false;
        private bool firing = false;
        public string fireTransformName = "CannonTransform";

        float facing = 0;
        Vector3 UpVect;
        Vector3 RightVect;
        Vector3 ForwardVect;

        private void CheckDirection()
        {
            UpVect = (this.vessel.transform.position - this.vessel.mainBody.position).normalized;
            RightVect = this.vessel.transform.right;
            ForwardVect = this.vessel.transform.forward;
            facing = Vector3.Angle(this.part.transform.forward, ForwardVect);
            if (Math.Sign(Vector3.Dot(this.part.transform.forward, RightVect)) < 0)
            {
                facing = 360 - facing;
            }

            if (facing <= 45 && facing >= 315)
            {
                forwardFacing = true;
                leftFacing = false;
                rightFacing = false;
                rearFacing = false;

            }
            else
            {
                if (facing <= 135 && facing >= 45)
                {
                    rightFacing = true;
                    leftFacing = false;
                    forwardFacing = false;
                    rearFacing = false;

                }
                else
                {
                    if (facing >= 135 && facing <= 225)
                    {
                        rearFacing = true;
                        leftFacing = false;
                        rightFacing = false;
                        forwardFacing = false;

                    }
                    else
                    {
                        if (facing >= 225 && facing <= 315)
                        {
                            leftFacing = true;
                            forwardFacing = false;
                            rightFacing = false;
                            rearFacing = false;
                        }
                    }
                }
            }
        }

        private Transform AmmoTransform = null;

        public int ammoCount = 0;

        [KSPEvent(name = "Fire Cannon", guiName = "Fire Cannon", active = true, guiActive = true)]
        public void Fire()
        {
            facing = Vector3.Angle(this.part.transform.forward, ForwardVect);
            if (Math.Sign(Vector3.Dot(this.part.transform.forward, RightVect)) < 0)
            {
                facing = 360 - facing;
            }

            if (facing <= 45 && facing >= 315)
            {
                forwardFacing = true;
                leftFacing = false;
                rightFacing = false;
                rearFacing = false;

            }
            else
            {
                if (facing <= 135 && facing >= 45)
                {
                    rightFacing = true;
                    leftFacing = false;
                    forwardFacing = false;
                    rearFacing = false;

                }
                else
                {
                    if (facing >= 135 && facing <= 225)
                    {
                        rearFacing = true;
                        leftFacing = false;
                        rightFacing = false;
                        forwardFacing = false;

                    }
                    else
                    {
                        if (facing >= 225 && facing <= 315)
                        {
                            leftFacing = true;
                            forwardFacing = false;
                            rightFacing = false;
                            rearFacing = false;
                        }
                    }
                }
            }

            if (ammoCount > 0 && !loading)
            {
                loading = true;
                StartCoroutine(FireCannon());
            }
            else
            {
                if (!fired)
                {
                    fired = true;
                    StartCoroutine(FireDelay());
                }
            }
        }

        private bool fired = false;

        IEnumerator FireDelay()
        {
            yield return new WaitForSeconds(2);
            GetAmmo();
        }


        IEnumerator FireCannon()
        {
            deploy = true;
            yield return new WaitForSeconds(2);
            SpawnCannonBall.instance.SpawnCoords = fireTransform.position;
            SpawnCannonBall.instance.dir = this.part.transform.forward;
            SpawnCannonBall.instance.CheckSpawnTimer();
            part.RequestResource("CannonBalls", 1);
            GetAmmo();
        }

        [KSPEvent(name = "Fire Barrage", guiName = "Fire Barrage", active = true, guiActive = true)]
        public void FireBarrage()
        {
            List<Part>.Enumerator p = this.vessel.Parts.GetEnumerator();
            while (p.MoveNext())
            {
                if (p.Current == null) continue;
                var cannon = p.Current.FindModuleImplementing<ModuleCannon>();
                if (cannon != null)
                {
                    if (forwardFacing)
                    {
                        if (cannon.forwardFacing)
                        {
                            cannon.Fire();
                        }
                    }
                    else
                    {
                        if (rightFacing)
                        {
                            if (cannon.rightFacing)
                            {
                                cannon.Fire();
                            }
                        }
                        else
                        {
                            if (rearFacing)
                            {
                                if (cannon.rearFacing)
                                {
                                    cannon.Fire();
                                }
                            }
                            else
                            {
                                if (leftFacing)
                                {
                                    if (cannon.leftFacing)
                                    {
                                        cannon.Fire();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            p.Dispose();
        }

        private bool Cannon = false;
        private bool Cannon17thc = false;
        private bool Cannon12lb = false;

        private void GetGun()
        {
            CheckDirection();

            if (this.part.partName == "12lbCannon")
            {
                Cannon12lb = true;
            }
            else
            {
                if (this.part.partName == "17thcCannon")
                {
                    Cannon17thc = true;
                }
                else
                {
                    if (this.part.partName == "Cannon")
                    {
                        Cannon = true;
                    }
                }
            }
            GetAmmo();
        }

        private void GetAmmo()
        {
            double totalAmount = 0;

            foreach (var p in vessel.parts)
            {
                PartResource r = p.Resources.Where(pr => pr.resourceName == "CannonBalls").FirstOrDefault();
                if (r != null)
                {
                    totalAmount += r.amount;
                }
            }

            if (totalAmount > 0)
            {
                ammoCount = Convert.ToInt32(totalAmount);
            }

            StartCoroutine(LoadDelay());
        }

        IEnumerator LoadDelay()
        {
            yield return new WaitForSeconds(1.25f);
            retract = true;
            firing = false;
            loading = false;
            barrage = false;
            fired = false;
        }
    }
}

