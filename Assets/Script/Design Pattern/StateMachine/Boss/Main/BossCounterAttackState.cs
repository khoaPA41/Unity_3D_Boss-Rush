using UnityEngine;

namespace Design_Pattern.StateMachine.Boss
{
    public class BossCounterAttackState : BossBaseState
    {
        readonly int CounterAttackAnimationHash = Animator.StringToHash("Counterattack");
        readonly string CounterAttackAnimationTag = "Counterattack";
        private float previousTime;

        public BossCounterAttackState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {
            bossStateMachine.Health.noDamage = true;
            bossStateMachine.IsFinishedAttack = false;
            previousTime = 0f;
            bossStateMachine.Animator.CrossFadeInFixedTime(CounterAttackAnimationHash, bossStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, CounterAttackAnimationTag, 0);
            if (normalizeTime > previousTime && normalizeTime >= .9f)
            {
                bossStateMachine.IsFinishedAttack = true;
                bossStateMachine.ReturnLocomotion();
            }
            previousTime = normalizeTime;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            bossStateMachine.Health.noDamage = false;
            bossStateMachine.IsCounterAttack = false;
        }
    }
}