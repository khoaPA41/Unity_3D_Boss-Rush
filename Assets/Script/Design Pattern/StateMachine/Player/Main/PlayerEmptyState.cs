using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class PlayerEmptyState : PlayerBaseState
{
    public PlayerEmptyState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.InputReader.ActiveCheckPointAction += playerStateMachine.ReturnLocomotion;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.ActiveCheckPointAction -= playerStateMachine.ReturnLocomotion;
    }
}
