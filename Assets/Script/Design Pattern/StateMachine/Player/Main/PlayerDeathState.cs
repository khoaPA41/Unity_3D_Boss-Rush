using Script.Design_Pattern.StateMachine.Player.Base;
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
        IsFinished = false;
        randomAnimation = Random.Range(0, 2);
        playerStateMachine.Animator.CrossFadeInFixedTime(
            randomAnimation == 0 ? DeathAnimationOneHash : DeathAnimationTwoHash,
            playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {
        IsFinished = false;
    }
}
