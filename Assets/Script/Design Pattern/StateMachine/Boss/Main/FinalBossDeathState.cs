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
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(
                randomAnimation == 0 ? DeathAnimationOneHash : DeathAnimationTwoHash,
                FinalBossStateMachine.AnimationCrossFade);
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
