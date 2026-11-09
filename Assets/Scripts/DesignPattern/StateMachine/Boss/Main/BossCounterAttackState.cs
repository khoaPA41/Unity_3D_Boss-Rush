using DesignPattern.StateMachine.Boss.Base;
using UnityEngine;

namespace DesignPattern.StateMachine.Boss.Main
{
    public class BossCounterAttackState : BossBaseState
    {
        readonly int _counterAttackAnimationHash = Animator.StringToHash("Counterattack");
        readonly string _counterAttackAnimationTag = "Counterattack";
        private float _previousTime;

        public BossCounterAttackState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {
            bossStateMachine.Health.noDamage = true;
            bossStateMachine.IsFinishedAttack = false;
            _previousTime = 0f;
            bossStateMachine.Animator.CrossFadeInFixedTime(_counterAttackAnimationHash, bossStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, _counterAttackAnimationTag, 0);
            if (normalizeTime > _previousTime && normalizeTime >= .9f)
            {
                bossStateMachine.IsFinishedAttack = true;
                bossStateMachine.ReturnLocomotion();
                return;
            }
            _previousTime = normalizeTime;
        }

        public override void Exit()
        {
            bossStateMachine.Health.noDamage = false;
            bossStateMachine.IsCounterAttack = false;
        }
    }
}