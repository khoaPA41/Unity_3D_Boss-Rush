using UnityEngine;

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
            finalBossStateMachine.Animator.CrossFadeInFixedTime(DeathAnimationOneHash, finalBossStateMachine.AnimationCrossFade);
        }
        else
        {
            finalBossStateMachine.Animator.CrossFadeInFixedTime(DeathAnimationTwoHash, finalBossStateMachine.AnimationCrossFade);
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
