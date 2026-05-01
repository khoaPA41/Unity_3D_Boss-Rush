using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    readonly int AttackAnimationHash = Animator.StringToHash("FreeLookBlendTree");

    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
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
