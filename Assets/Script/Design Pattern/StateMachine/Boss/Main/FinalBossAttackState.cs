using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossAttackState : FinalBossBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;

        public FinalBossAttackState(FinalBossStateMachine finalBossStateMachine, int index) : base(finalBossStateMachine)
        {
            _attackData = finalBossStateMachine.AttackDatas[index];
        }

        public override void Enter()
        {
            // FinalBossStateMachine.IsAttack = false;
            FinalBossStateMachine.WeaponDealDamage.SetDamage(10);
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName,
                _attackData.AnimationTransition);
            
        }

        public override void Tick(float deltaTime)
        {
            float normalizeTime = GetNormalizeTime(FinalBossStateMachine.Animator, "Attack", 0);
            if (normalizeTime >= _previousTime && normalizeTime < 1f)
            {
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