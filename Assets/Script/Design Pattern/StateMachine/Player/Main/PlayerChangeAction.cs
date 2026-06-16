using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerChangeAction : PlayerBaseState
    {
        private readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

        readonly int MovementXParam = Animator.StringToHash("MovementX");
        readonly int MovementYParam = Animator.StringToHash("MovementY");
        readonly int MovementParam = Animator.StringToHash("Movement");


        readonly int SwordEnterAnimationHash = Animator.StringToHash("Sword_Enter");
        readonly int SwordExitAnimationHash = Animator.StringToHash("Sword_Exit");

        readonly string SwordChangeTag = "SwordChange";
        readonly string IdleAnimationName = "Idle_Loop";
        readonly string SwordIdleAnimationName = "Sword_Idle";

        private readonly bool isSwordEnter;
        private Vector3 movement;
        private bool isTarget = false;

        public PlayerChangeAction(PlayerStateMachine playerStateMachine, bool isSwordEnter) : base(playerStateMachine)
        {
            this.isSwordEnter = isSwordEnter;
        }

        public override void Enter()
        {
            if (playerStateMachine.Targeter.currentTarget is not null)
            {
                isTarget = true;
            }
            playerStateMachine.Animator.CrossFadeInFixedTime(
                playerStateMachine.Targeter.currentTarget is not null ? TargetLookBlendTreeHash : freeLookBlendTreeHash,
                playerStateMachine.AnimationCrossFade, 0);


            if (isSwordEnter)
            {
                playerStateMachine.Animator.CrossFadeInFixedTime(SwordEnterAnimationHash,
                    playerStateMachine.AnimationCrossFade, 1);
                playerStateMachine.isAttackState = true;
            }
            else
            {
                playerStateMachine.Animator.CrossFadeInFixedTime(SwordExitAnimationHash,
                    playerStateMachine.AnimationCrossFade, 1);
                playerStateMachine.isAttackState = false;
            }
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, SwordChangeTag, 1);
            var speed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.FreeLookMovementSprintSpeed
                : playerStateMachine.FreeLookMovementSpeed;
            if (normalizeTime is > .9f and <= 1f)
            {
                playerStateMachine.ReturnLocomotion();
            }

            if (playerStateMachine.Targeter.currentTarget is not null)
            {
                movement = CalculateMovementInTarget();
                FaceTarget(deltaTime);
            }
            else
            {
                movement = CalculateMovementInFreeLook();
                FaceDir(movement, deltaTime);
            }

            if (isTarget)
            {
                UpdateTargetAnimation(deltaTime);
            }
            else
            {
                UpdateFreeLookAnimation(deltaTime);
            }

            UpdateFreeLookAnimation(deltaTime);
            Move(movement * speed, deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            if (isSwordEnter)
            {
                ChangeSwordIdle(IdleAnimationName, playerStateMachine.SwordIdleAnimationClip);
            }
            else
            {
                ChangeSwordIdle(SwordIdleAnimationName, playerStateMachine.IdleLoopAnimationClip);
            }
        }

        private void UpdateFreeLookAnimation(float deltaTime)
        {
            if (playerStateMachine.InputReader.InputMovement == Vector2.zero)
            {
                playerStateMachine.Animator.SetFloat(MovementParam, 0f, playerStateMachine.AnimationCrossFade,
                    deltaTime);
                return;
            }

            if (playerStateMachine.InputReader.IsSprint)
            {
                playerStateMachine.Animator.SetFloat(MovementParam, 1f, playerStateMachine.AnimationCrossFade,
                    deltaTime);
                return;
            }

            playerStateMachine.Animator.SetFloat(MovementParam, .5f, playerStateMachine.AnimationCrossFade, deltaTime);
        }

        private void UpdateTargetAnimation(float deltaTime)
        {
            var dirX = 0f;
            var dirY = 0f;
            if (playerStateMachine.InputReader.InputMovement.x != 0)
            {
                dirX = Mathf.Sign(playerStateMachine.InputReader.InputMovement.x);
            }

            if (playerStateMachine.InputReader.InputMovement.y != 0)
            {
                dirY = Mathf.Sign(playerStateMachine.InputReader.InputMovement.y);
            }

            playerStateMachine.Animator.SetFloat(MovementXParam, dirX, playerStateMachine.AnimationCrossFade,
                deltaTime);
            playerStateMachine.Animator.SetFloat(MovementYParam, dirY, playerStateMachine.AnimationCrossFade,
                deltaTime);
        }
    }
}