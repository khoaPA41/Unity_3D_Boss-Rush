using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerLandingState : PlayerBaseState
    {
        private readonly int _landingAnimationHash = Animator.StringToHash("Landing");
        private const string LandingAnimationTag = "Landing";

        private Vector3 momentum;

        public PlayerLandingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(_landingAnimationHash, playerStateMachine.AnimationCrossFade);
            momentum = playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, LandingAnimationTag, 0);
            if (normalizeTime is > .9f and <= 1f)
            {
                playerStateMachine.ReturnLocomotion();
            }
            Move(deltaTime);
            FaceTarget(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {

        }

        public override void Exit()
        {
            // IsFinished = true;
        }
    }
}
