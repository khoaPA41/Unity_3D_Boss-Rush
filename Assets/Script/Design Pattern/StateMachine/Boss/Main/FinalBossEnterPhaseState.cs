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
        private readonly int _comboIndex;
        
        public FinalBossEnterPhaseState(FinalBossStateMachine finalBossStateMachine, int comboIndex, int attackIndex) : base(finalBossStateMachine)
        {
            ultimate = FinalBossStateMachine.UltimateCombo[comboIndex];
            FinalBossStateMachine.currentAttackData = ultimate.AttackData;
            _attackData = FinalBossStateMachine.currentAttackData[attackIndex];
            _comboIndex = comboIndex;
        }
        
        public override void Enter()
        {
            SkillSituationEvent.Instance.NextActionEvent += TryCombo;
            FinalBossStateMachine.Health.noDamage = true;
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName, _attackData.AnimationTransition);
            
            UseSkill(_attackData.SkillType);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(FinalBossStateMachine.Animator, "Attack", 0);
            if (normalizeTime >= _previousTime && normalizeTime < 1f)
            {
                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
                }
            }
            
            _previousTime = normalizeTime;

            if (FinalBossStateMachine.IsCanMove)
            {
                var input = FinalBossStateMachine.GetDirToPlayer();
                FinalBossStateMachine.InputMovement = new Vector2(input.x, input.z);
                var dir = new Vector3(FinalBossStateMachine.InputMovement.x, 0, FinalBossStateMachine.InputMovement.y);
                Move(dir * FinalBossStateMachine.DashSpeed, deltaTime);
            }
            else
            {
                Move(deltaTime);
            }
            
            FaceTarget(FinalBossStateMachine.GetDirToPlayer());
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            FinalBossStateMachine.IsActiveUltimate = _attackData.NextAttackDataIndex != -1;
            FinalBossStateMachine.Health.noDamage = _attackData.NextAttackDataIndex != -1;
            FinalBossStateMachine.IsChangePhase = _attackData.NextAttackDataIndex != -1;
            SkillSituationEvent.Instance.NextActionEvent -= TryCombo;
            FinalBossStateMachine.IsCanMove = false;
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
                _comboIndex,
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
