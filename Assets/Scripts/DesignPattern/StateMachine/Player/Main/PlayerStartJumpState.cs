using DesignPattern.StateMachine.Player.Base;
using UnityEngine;
namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerStartJumpState : PlayerBaseState
    {
        private readonly int _jumpAnimationHash = Animator.StringToHash("Jump");
        Vector3 momentum;
        public PlayerStartJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            if (playerStateMachine.CheckLowStamina())
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }

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
            playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.jumpReduce);

            Move(momentum, deltaTime);
            FaceTarget(deltaTime);
        }

        public override void Exit()
        {

        }

    }
}