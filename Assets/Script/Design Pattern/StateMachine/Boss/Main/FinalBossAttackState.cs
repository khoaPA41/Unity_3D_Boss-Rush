using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossAttackState : FinalBossBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;
        private int normalComboIndex;

        public FinalBossAttackState(FinalBossStateMachine finalBossStateMachine, int normalComboIndex, int index) : base(finalBossStateMachine)
        {
            FinalBossStateMachine.currentAttackData = FinalBossStateMachine.NormalCombo[normalComboIndex].AttackData;
            _attackData = FinalBossStateMachine.currentAttackData[index];
            this.normalComboIndex = normalComboIndex;
            // _attackData = finalBossStateMachine.AttackData[index];
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
                TrySlowAnimation(normalizeTime);
                
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
            FaceTarget(FinalBossStateMachine.GetDirToPlayer());
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            FinalBossStateMachine.IsAttack = false;
        }


        private void TrySlowAnimation(float normalizedTime)
        {
            if (normalizedTime >= _attackData.AnimationSlowStartThreshold)
            {
                FinalBossStateMachine.Animator.speed = _attackData.AnimationSpeed;
            }

            if (normalizedTime >= _attackData.AnimationSlowEndThreshold)
            {
                FinalBossStateMachine.Animator.speed = 1;
            }
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