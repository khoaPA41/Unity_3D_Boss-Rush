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

        readonly int SwordEnterAnimationHash = Animator.StringToHash("Sword_Enter");
        readonly int SwordExitAnimationHash = Animator.StringToHash("Sword_Exit");

        readonly string SwordChangeTag = "SwordChange";
        readonly string IdleAnimationName = "Idle_Loop";
        readonly string SwordIdleAnimationName = "Sword_Idle";

        private readonly bool isSwordEnter;
        private Vector3 movement;

        public PlayerChangeAction(PlayerStateMachine playerStateMachine, bool isSwordEnter) : base(playerStateMachine)
        {
            this.isSwordEnter = isSwordEnter;
        }

        public override void Enter()
        {
            IsFinished = false;
            if (playerStateMachine.Targeter.currentTarget is not null)
            {
                playerStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash,
                    playerStateMachine.AnimationCrossFade, 0);
            }
            else
            {
                playerStateMachine.Animator.CrossFadeInFixedTime(freeLookBlendTreeHash,
                    playerStateMachine.AnimationCrossFade, 0);
            }

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
            float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, SwordChangeTag, 1);

            if (normalizeTime is > .9f and <= 1f)
            {
                IsFinished = true;
            }

            if (playerStateMachine.Targeter.currentTarget != null)
            {
                movement = CalculateMovementInTarget();
                FaceTarget(deltaTime);
            }
            else
            {
                movement = CalculateMovementInFreeLook();
                FaceDir(movement, deltaTime);
            }

            UpdateAnimation(deltaTime);
            Move(movement * playerStateMachine.FreeLookMovementSpeed, deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            IsFinished = false;
            if (isSwordEnter)
            {
                ChangeSwordIdle(IdleAnimationName, playerStateMachine.SwordIdleAnimationClip);
            }
            else
            {
                ChangeSwordIdle(SwordIdleAnimationName, playerStateMachine.IdleLoopAnimationClip);
            }
        }

        private void UpdateAnimation(float deltaTime)
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