using DesignPattern.StateMachine.Player.Base;

using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerFallState : PlayerBaseState
    {
        readonly int FallingAnimationHash = Animator.StringToHash("Falling");
        private Vector3 momentum;

        public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(FallingAnimationHash, playerStateMachine.AnimationCrossFade);
            momentum = playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }

        public override void Tick(float deltaTime)
        {
            if (playerStateMachine.CharacterController.isGrounded && playerStateMachine.CharacterController.velocity.y <= 0f)
            {
                playerStateMachine.SwitchState(playerStateMachine.LandingState);
                return;
            }

            Move(momentum, deltaTime);
            FaceTarget(deltaTime);
        }

        public override void Exit()
        {

        }
    }
}
