using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerDodgeState : PlayerBaseState
    {
        private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
        private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
        private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");

        private Vector2 dodgeDirection;

        private float remainingTime;
        public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            dodgeDirection = playerStateMachine.InputReader.InputMovement;
            IsFinished = false;
            playerStateMachine.Animator.SetFloat(DodgeRightHash, dodgeDirection.x);
            playerStateMachine.Animator.SetFloat(DodgeForwardHash, dodgeDirection.y);
            playerStateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);
            remainingTime = playerStateMachine.DodgeDuration;
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            Move(movement, deltaTime);
            FaceTarget(deltaTime);

            remainingTime -= deltaTime;

            if (remainingTime <= 0f)
            {
                IsFinished = true;
            }
        }

        public override void PhysicTick(float fixedDeltaTime)
        {

        }

        public override void Exit()
        {
            IsFinished = true;
        }

        private Vector3 CalculateMovement()
        {
            var movement = new Vector3();

            movement += playerStateMachine.transform.right * (dodgeDirection.x * playerStateMachine.DodgeLength) / playerStateMachine.DodgeDuration;
            movement += playerStateMachine.transform.forward * (dodgeDirection.y * playerStateMachine.DodgeLength) / playerStateMachine.DodgeDuration;

            return movement;
        }
    }
}
