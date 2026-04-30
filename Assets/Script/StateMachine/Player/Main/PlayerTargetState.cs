using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");

    public PlayerTargetState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.InputReader.TargetAction += OutTargetState;
        playerStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash, playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        if (playerStateMachine.Targeter.currentTarget == null)
        {
            OutTargetState();
        }
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {
        playerStateMachine.InputReader.TargetAction -= OutTargetState;

    }

    void OutTargetState()
    {
        playerStateMachine.Targeter.CancelTarget();
        playerStateMachine.SwitchState(new FreeLookState(playerStateMachine));
        return;
    }
}
