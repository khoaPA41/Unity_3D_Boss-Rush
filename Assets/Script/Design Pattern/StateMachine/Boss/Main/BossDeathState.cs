using UnityEngine;


namespace Design_Pattern.StateMachine.Boss
{
    public class BossDeathState : BossBaseState
    {
        readonly int DeathAnimationOneHash = Animator.StringToHash("Death_1");
        readonly int DeathAnimationTwoHash = Animator.StringToHash("Death_2");
        readonly string DeathAnimationTag = "Death";

        private float previousTime;
        int randomAnimation;
        bool isDead;
        public BossDeathState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {

            randomAnimation = Random.Range(0, 2);
            bossStateMachine.Animator.CrossFadeInFixedTime(
                randomAnimation == 0 ? DeathAnimationOneHash : DeathAnimationTwoHash,
                bossStateMachine.AnimationCrossFade);
            // bossStateMachine.GetComponent<BossStateMachine>().enabled = false;


        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, DeathAnimationTag, 0);
            if (normalizeTime > previousTime && normalizeTime >= .9f)
            {
                if (!isDead)
                {
                    bossStateMachine.BossBehaviorBrain.IsDead = true;
                    isDead = true;
                }
            }
            previousTime = normalizeTime;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}