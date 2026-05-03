using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    readonly int FallingAnimationHash = Animator.StringToHash("Falling");

    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
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
