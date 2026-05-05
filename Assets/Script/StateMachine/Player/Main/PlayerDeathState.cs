using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    readonly int DeathAnimationOneHash = Animator.StringToHash("Death_1");
    readonly int DeathAnimationTwoHash = Animator.StringToHash("Death_2");

    int randomAnimation;
    public PlayerDeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        randomAnimation = Random.Range(0, 2);
        if (randomAnimation == 0)
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(DeathAnimationOneHash, playerStateMachine.AnimationCrossFade);
        }
        else
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(DeathAnimationTwoHash, playerStateMachine.AnimationCrossFade);
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
