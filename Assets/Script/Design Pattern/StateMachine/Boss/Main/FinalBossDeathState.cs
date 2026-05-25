using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossDeathState : FinalBossBaseState
    {
        readonly int DeathAnimationOneHash = Animator.StringToHash("Death_1");
        readonly int DeathAnimationTwoHash = Animator.StringToHash("Death_2");
        int randomAnimation;
        public FinalBossDeathState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
        {
        }

        public override void Enter()
        {
            randomAnimation = Random.Range(0, 2);
            if (randomAnimation == 0)
            {
                FinalBossStateMachine.Animator.CrossFadeInFixedTime(DeathAnimationOneHash, FinalBossStateMachine.AnimationCrossFade);
            }
            else
            {
                FinalBossStateMachine.Animator.CrossFadeInFixedTime(DeathAnimationTwoHash, FinalBossStateMachine.AnimationCrossFade);
            }
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
