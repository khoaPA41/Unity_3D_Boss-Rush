using UnityEngine;


namespace Script.Design_Pattern.StateMachine
{
    public class BossDeathState : BossBaseState
    {
        readonly int DeathAnimationOneHash = Animator.StringToHash("Death_1");
        readonly int DeathAnimationTwoHash = Animator.StringToHash("Death_2");
        int randomAnimation;
        public BossDeathState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {
            randomAnimation = Random.Range(0, 2);
            bossStateMachine.Animator.CrossFadeInFixedTime(
                randomAnimation == 0 ? DeathAnimationOneHash : DeathAnimationTwoHash,
                bossStateMachine.AnimationCrossFade);
            bossStateMachine.GetComponent<BossStateMachine>().enabled = false;
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}