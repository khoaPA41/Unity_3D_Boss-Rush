using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossEnterPhaseState : FinalBossBaseState
    {
        private readonly UltimateCombo ultimate;
        private float _previousTime;
        private bool _alreadyApplyForce;
        private readonly AttackData _attackData;
        private readonly int comboIndex;
        
        public FinalBossEnterPhaseState(FinalBossStateMachine finalBossStateMachine, int comboIndex, int attackIndex) : base(finalBossStateMachine)
        {
            ultimate = FinalBossStateMachine.UltimateCombo[comboIndex];
            FinalBossStateMachine.currentAttackData = ultimate.AttackData;
            _attackData = FinalBossStateMachine.currentAttackData[attackIndex];
            this.comboIndex = comboIndex;
        }

        public override void Enter()
        {
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName, _attackData.AnimationTransition);
            UseSkill(_attackData.SkillType);
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

                if (_previousTime > _attackData.AttackAnimationTime)
                {
                    TryCombo();
                }
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
            FinalBossStateMachine.IsActiveUltimate = false;
            if (_attackData.NextAttackDataIndex != -1)
            {
                FinalBossStateMachine.IsChangePhase = false;
            }
        }
        
        private void TryCombo()
        {
            if (_attackData.NextAttackDataIndex == -1)
            {
                FinalBossStateMachine.NextPhase++;
                FinalBossStateMachine.ReturnLocomotion();
                return;
            }
            
            FinalBossStateMachine.SwitchState(new FinalBossEnterPhaseState(
                FinalBossStateMachine,
                comboIndex,
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
