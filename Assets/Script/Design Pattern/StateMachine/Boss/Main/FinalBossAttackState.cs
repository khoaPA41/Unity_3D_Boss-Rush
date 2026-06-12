using System;
using System.Linq;
using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;
using Math = Unity.Mathematics.Geometry.Math;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossAttackState : FinalBossBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;
        private int normalComboIndex;
        private float glowCountTime;
        private int indexCombo;
        
        float enterStateTime;

        private bool isGlowing;
        private bool isSlowAnimation;

        public FinalBossAttackState(FinalBossStateMachine finalBossStateMachine, int normalComboIndex, int indexCombo, int index) : base(finalBossStateMachine)
        {
            _attackData = FinalBossStateMachine.NormalCombo[normalComboIndex].Combo[indexCombo].AttackData[index];
            this.normalComboIndex = normalComboIndex;
            this.indexCombo = indexCombo;
        }

        public override void Enter()
        {
            enterStateTime = Time.time;
            FinalBossStateMachine.ManageAnimationSkillEvent.NextActionEvent += TryCombo;
            FinalBossStateMachine.ManageAnimationSkillEvent.SlashWeaponEvent += ActiveEasyEffect;
            UseSkill(_attackData.SkillType);
            
            // FinalBossStateMachine.DealDamage.SetDamage(_attackData.AttackDamage);
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName,
                _attackData.AnimationTransition);
        }

        public override void Tick(float deltaTime)
        {
            FinalBossStateMachine.DealsDamage.Where(dealDamage => dealDamage.gameObject.activeInHierarchy).ToList().ForEach(dealDamage => dealDamage.SetDamage(_attackData.AttackDamage));
            if (FinalBossStateMachine.IsCanMove)
            {
                var input = FinalBossStateMachine.GetDirToPlayer(FinalBossStateMachine.Target);
                FinalBossStateMachine.InputMovement = new Vector2(input.x, input.z);
                var dir = new Vector3(FinalBossStateMachine.InputMovement.x, 0, FinalBossStateMachine.InputMovement.y);
                Move(dir * FinalBossStateMachine.DashSpeed, deltaTime);
            }
            else
            {
                Move(deltaTime);
            }
            var normalizeTime = GetNormalizeTime(FinalBossStateMachine.Animator, "Attack", 0);

            if (normalizeTime >= _previousTime && normalizeTime < 1f)
            {
                glowCountTime += deltaTime;
                var t = Mathf.Clamp01(glowCountTime / _attackData.AttackAnimationTime);
                
                if (isGlowing)
                {
                    GlowingWeapon(t);
                }

                if (isSlowAnimation)
                {
                    TrySlowAnimation(t);
                }
                
                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
                }
            }
            else
            {
                FinalBossStateMachine.ReturnLocomotion();
            }
            
            _previousTime = normalizeTime;
            Move(deltaTime);
            FaceTarget(FinalBossStateMachine.GetDirToPlayer(FinalBossStateMachine.Target), FinalBossStateMachine.Target);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            FinalBossStateMachine.ManageAnimationSkillEvent.NextActionEvent -= TryCombo;
            FinalBossStateMachine.ManageAnimationSkillEvent.SlashWeaponEvent -= ActiveEasyEffect;
            FinalBossStateMachine.IsAttack = false;
            FinalBossStateMachine.IsCanMove = false;
            FinalBossStateMachine.Animator.speed = 1;

            if (!FinalBossStateMachine.isBothWeaponVFX)
            {
                if (FinalBossStateMachine.isRightWeaponVFX)
                {
                    if (FinalBossStateMachine.Weapon.activeInHierarchy)
                    {
                        FinalBossStateMachine.WeaponMaterial.SetColor("_EmissionColor", FinalBossStateMachine.WeaponEmissionColor);
                    }

                    if (FinalBossStateMachine.isRightWeaponVFX)
                    {
                        FinalBossStateMachine.WeaponRightMaterial.SetColor("_EmissionColor", FinalBossStateMachine.WeaponRightEmissionColor);
                    }
                }
                else
                {
                    FinalBossStateMachine.WeaponLeftMaterial.SetColor("_EmissionColor", FinalBossStateMachine.WeaponLeftEmissionColor);
                }
            }

            if (FinalBossStateMachine.isBothWeaponVFX)
            {
                FinalBossStateMachine.WeaponLeftMaterial.SetColor("_EmissionColor", FinalBossStateMachine.WeaponLeftEmissionColor);
                FinalBossStateMachine.WeaponRightMaterial.SetColor("_EmissionColor", FinalBossStateMachine.WeaponRightEmissionColor);
            }
            
 
        }
        
        private void TrySlowAnimation(float time)
        {
            if (time < _attackData.AttackAnimationTime)
            {
                FinalBossStateMachine.Animator.speed = time < _attackData.AnimationStartSlowThreshold ? 1f : 0f;
                return;
            }
        
            FinalBossStateMachine.Animator.speed = 1;
        }
        
        private void GlowingWeapon(float time)
        {
            if (!FinalBossStateMachine.isBothWeaponVFX)
            {
                if (FinalBossStateMachine.isRightWeaponVFX)
                {
                    if (FinalBossStateMachine.Weapon.activeInHierarchy)
                    {
                        var currentIntensity = FinalBossStateMachine.AnimationWeaponEmissionCurve.Evaluate(time);
                        var finalColor = FinalBossStateMachine.WeaponEmissionColor * Mathf.Pow(2f, 10f);
                        FinalBossStateMachine.WeaponMaterial.SetColor("_EmissionColor", finalColor * currentIntensity);
                    }

                    if (FinalBossStateMachine.WeaponRight.activeInHierarchy)
                    {
                        var currentIntensity = FinalBossStateMachine.AnimationWeaponEmissionCurve.Evaluate(time);
                        var finalColor = FinalBossStateMachine.WeaponRightEmissionColor * Mathf.Pow(2f, 10f);
                        FinalBossStateMachine.WeaponRightMaterial.SetColor("_EmissionColor", finalColor * currentIntensity);
                    }
                }
                else
                {
                    var currentIntensity = FinalBossStateMachine.AnimationWeaponEmissionCurve.Evaluate(time);
                    var finalColor = FinalBossStateMachine.WeaponLeftEmissionColor * Mathf.Pow(2f, 10f);
                    FinalBossStateMachine.WeaponLeftMaterial.SetColor("_EmissionColor", finalColor * currentIntensity);
                }
            }
            
            if (FinalBossStateMachine.isBothWeaponVFX)
            {
                var currentIntensityLeft = FinalBossStateMachine.AnimationWeaponEmissionCurve.Evaluate(time);
                var finalColorLeft = FinalBossStateMachine.WeaponLeftEmissionColor * Mathf.Pow(2f, 10f);
                FinalBossStateMachine.WeaponLeftMaterial.SetColor("_EmissionColor", finalColorLeft * currentIntensityLeft);
                
                var currentIntensityRight = FinalBossStateMachine.AnimationWeaponEmissionCurve.Evaluate(time);
                var finalColorRight = FinalBossStateMachine.WeaponRightEmissionColor * Mathf.Pow(2f, 10f);
                FinalBossStateMachine.WeaponRightMaterial.SetColor("_EmissionColor", finalColorRight * currentIntensityRight);
            }
        }

        private void ActiveEasyEffect()
        {
            isGlowing = true;
            isSlowAnimation = true;
        }

        private void TryCombo()
        {
            if (_attackData.NextAttackDataIndex == -1)
            {
                FinalBossStateMachine.IsFinishedAttack = true;
                FinalBossStateMachine.NextAttackIndex = -1;
                return;
            }
            
            if (Time.time - enterStateTime < 0.2f)
            {
                return;
            }

            FinalBossStateMachine.LastAttackTime = Time.time;
            FinalBossStateMachine.NextAttackIndex = _attackData.NextAttackDataIndex;
            
            FinalBossStateMachine.SwitchState(new FinalBossAttackState(
                FinalBossStateMachine,
                normalComboIndex,
                indexCombo,
                _attackData.NextAttackDataIndex
            ));
        }

        private void TryApplyForce()
        {
            if (_alreadyApplyForce)
            {
                return;
            }

            FinalBossStateMachine.ForceReceiver.AddForce(FinalBossStateMachine.transform.forward * _attackData.Force);
            _alreadyApplyForce = true;
        }
    }
}