using System;
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

        public FinalBossAttackState(FinalBossStateMachine finalBossStateMachine, int normalComboIndex, int index) : base(finalBossStateMachine)
        {
            FinalBossStateMachine.currentAttackData = FinalBossStateMachine.NormalCombo[normalComboIndex].AttackData;
            _attackData = FinalBossStateMachine.currentAttackData[index];
            this.normalComboIndex = normalComboIndex;
        }

        public override void Enter()
        {
            FinalBossStateMachine.WeaponDealDamage.SetDamage(10);
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName,
                _attackData.AnimationTransition);
        }

        public override void Tick(float deltaTime)
        {
            float normalizeTime = GetNormalizeTime(FinalBossStateMachine.Animator, "Attack", 0);

            if (normalizeTime >= _previousTime && normalizeTime < 1f)
            {
                glowCountTime += deltaTime;
                var t = Mathf.Clamp01(glowCountTime / _attackData.AttackAnimationTime);
                GlowingWeapon(t);
                TrySlowAnimation(t);
                
                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
                }

                if (FinalBossStateMachine.IsAttack)
                {
                    TryCombo();
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
            FinalBossStateMachine.IsAttack = false;
            FinalBossStateMachine.Animator.speed = 1;
            FinalBossStateMachine.WeaponMaterial.SetColor("_EmissionColor", FinalBossStateMachine.WeaponEmissionColor);
        }


        private void TrySlowAnimation(float time)
        {
            if (time < _attackData.AttackAnimationTime)
            {
                FinalBossStateMachine.Animator.speed = time < _attackData.AnimationSlowStartThreshold ? 1f : 0f;
                return;
            }
       
            FinalBossStateMachine.Animator.speed = 1;
        }

        private void GlowingWeapon(float time)
        {
            var currentIntensity = FinalBossStateMachine.AnimationWeaponEmissionCurve.Evaluate(time);
            var finalColor = FinalBossStateMachine.WeaponEmissionColor * Mathf.Pow(2f, 10f);
            FinalBossStateMachine.WeaponMaterial.SetColor("_EmissionColor", finalColor * currentIntensity);
        }

        private void TryCombo()
        {
            if (_attackData.NextAttackDataIndex == -1)
            {
                return;
            }

            if (_previousTime < _attackData.AttackAnimationTime)
            {
                return;
            }

            FinalBossStateMachine.LastAttackTime = Time.time;
            FinalBossStateMachine.CurrentComboIndex = _attackData.NextAttackDataIndex;
            
            FinalBossStateMachine.SwitchState(new FinalBossAttackState(
                FinalBossStateMachine,
                normalComboIndex,
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