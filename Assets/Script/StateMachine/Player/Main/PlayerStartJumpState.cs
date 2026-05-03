using UnityEngine;

public class PlayerStartJumpState : PlayerBaseState
{
    readonly int JumpAnimationHash = Animator.StringToHash("Jump");

    public PlayerStartJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        //playerStateMachine.Animator.CrossFadeInFixedTime(JumpAnimationHash, playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        playerStateMachine.ForceReceiver.AddForce(playerStateMachine.transform.up * playerStateMachine.JumpForce);
        Move(deltaTime);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }

}
