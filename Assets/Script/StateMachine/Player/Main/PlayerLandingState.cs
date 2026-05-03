using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    readonly int LandingAnimationHash = Animator.StringToHash("Landing");

    public PlayerLandingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {

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
