using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class FreeLookState : PlayerBaseState
    {
        private static readonly int Movement = Animator.StringToHash("Movement");
        private readonly int freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private Vector3 movement;
        public float CountTimeToChangeIdleLoop { get; } = 0;

        public FreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }
        

        public override void Enter()
        {
            IsFinished = false;
            Debug.Log(IsFinished);
            playerStateMachine.Animator.CrossFadeInFixedTime(freeLookBlendTreeHash,
                playerStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            movement = CalculateMovementInFreeLook();
            float speed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.FreeLookMovementSprintSpeed
                : playerStateMachine.FreeLookMovementSpeed;

            // playerStateMachine.CountSkillTime -= deltaTime;

            Move(movement * speed, deltaTime);
            UpdateAnimation(deltaTime);
            FaceDir(movement, deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            IsFinished = true;
        }

        private void UpdateAnimation(float deltaTime)
        {
            if (playerStateMachine.InputReader.InputMovement == Vector2.zero)
            {
                playerStateMachine.Animator.SetFloat(Movement, 0f, playerStateMachine.AnimationCrossFade, deltaTime);
                return;
            }

            if (playerStateMachine.InputReader.IsSprint)
            {
                playerStateMachine.Animator.SetFloat(Movement, 1f, playerStateMachine.AnimationCrossFade, deltaTime);
                return;
            }

            playerStateMachine.Animator.SetFloat(Movement, .5f, playerStateMachine.AnimationCrossFade, deltaTime);
        }

        private void EnterTargetState()
        {
            if (!playerStateMachine.Targeter.SelectedTarget())
            {
                return;
            }

            playerStateMachine.SwitchState(new PlayerTargetState(playerStateMachine));
            return;
        }

        private void EnterDodgeState()
        {
            playerStateMachine.SwitchState(new PlayerDodgeState(playerStateMachine,
                playerStateMachine.InputReader.InputMovement));
        }
    }
}