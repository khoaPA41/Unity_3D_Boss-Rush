using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.StateMachine.Player.Main;
using UnityEngine;

public class PlayerStartJumpState : PlayerBaseState
{
    private readonly int _jumpAnimationHash = Animator.StringToHash("Jump");
    Vector3 momentum;
    public PlayerStartJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.CheckStamina();

        playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.jumpReduce);
        playerStateMachine.Animator.CrossFadeInFixedTime(_jumpAnimationHash, playerStateMachine.AnimationCrossFade);
        playerStateMachine.ForceReceiver.Jump(playerStateMachine.JumpForce);
        momentum = playerStateMachine.CharacterController.velocity;
        momentum.y = 0;
    }

    public override void Tick(float deltaTime)
    {
        if (!playerStateMachine.CharacterController.isGrounded && playerStateMachine.CharacterController.velocity.y <= 0f)
        {
            playerStateMachine.SwitchState(new PlayerFallState(playerStateMachine));
            return;
        }


        Move(momentum, deltaTime);
        FaceTarget(deltaTime);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }

}
