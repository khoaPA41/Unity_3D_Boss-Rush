using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
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
                playerStateMachine.SwitchState(playerStateMachine.landingState);
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
}
