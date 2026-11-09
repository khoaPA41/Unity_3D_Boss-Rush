using DesignPattern.StateMachine.Player.Base;
using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerChangeActionState : PlayerBaseState
    {
        private readonly int _targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

        readonly int _movementXParam = Animator.StringToHash("MovementX");
        readonly int _movementYParam = Animator.StringToHash("MovementY");
        readonly int _movementParam = Animator.StringToHash("Movement");


        readonly int _swordEnterAnimationHash = Animator.StringToHash("Sword_Enter");
        readonly int _swordExitAnimationHash = Animator.StringToHash("Sword_Exit");

        readonly string _swordChangeTag = "SwordChange";
        readonly string _idleAnimationName = "Idle_Loop";
        readonly string _swordIdleAnimationName = "Sword_Idle";

        private readonly bool _isSwordEnter;
        private Vector3 _movement;
        private bool _isTarget = false;

        public PlayerChangeActionState(PlayerStateMachine playerStateMachine, bool isSwordEnter) : base(playerStateMachine)
        {
            _isSwordEnter = isSwordEnter;
        }

        public override void Enter()
        {
            if (playerStateMachine.Targeter.CurrentTarget != null)
            {
                _isTarget = true;
            }
            playerStateMachine.Animator.CrossFadeInFixedTime(
                playerStateMachine.Targeter.CurrentTarget != null ? _targetLookBlendTreeHash : _freeLookBlendTreeHash,
                playerStateMachine.AnimationCrossFade, 0);


            if (_isSwordEnter)
            {
                playerStateMachine.Animator.CrossFadeInFixedTime(_swordEnterAnimationHash,
                    playerStateMachine.AnimationCrossFade, 1);
                playerStateMachine.IsAttackState = true;
            }
            else
            {
                playerStateMachine.Animator.CrossFadeInFixedTime(_swordExitAnimationHash,
                    playerStateMachine.AnimationCrossFade, 1);
                playerStateMachine.IsAttackState = false;
            }
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _swordChangeTag, 1);
            var speed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.FreeLookMovementSprintSpeed
                : playerStateMachine.FreeLookMovementSpeed;
            if (normalizeTime is > .9f and <= 1f)
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }

            if (playerStateMachine.Targeter.CurrentTarget != null)
            {
                _movement = CalculateMovementInTarget();
                FaceTarget(deltaTime);
            }
            else
            {
                _movement = CalculateMovementInFreeLook();
                FaceDir(_movement, deltaTime);
            }

            if (_isTarget)
            {
                UpdateTargetAnimation(deltaTime);
            }
            else
            {
                UpdateFreeLookAnimation(deltaTime);
            }

            Move(_movement * speed, deltaTime);
        }

        public override void Exit()
        {
            if (_isSwordEnter)
            {
                ChangeSwordIdle(_idleAnimationName, playerStateMachine.SwordIdleAnimationClip);
            }
            else
            {
                ChangeSwordIdle(_swordIdleAnimationName, playerStateMachine.IdleLoopAnimationClip);
            }
        }

        private void UpdateFreeLookAnimation(float deltaTime)
        {
            if (playerStateMachine.InputReader.InputMovement == Vector2.zero)
            {
                playerStateMachine.Animator.SetFloat(_movementParam, 0f, playerStateMachine.AnimationCrossFade,
                    deltaTime);
                return;
            }

            if (playerStateMachine.InputReader.IsSprint)
            {
                playerStateMachine.Animator.SetFloat(_movementParam, 1f, playerStateMachine.AnimationCrossFade,
                    deltaTime);
                return;
            }

            playerStateMachine.Animator.SetFloat(_movementParam, .5f, playerStateMachine.AnimationCrossFade, deltaTime);
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

            playerStateMachine.Animator.SetFloat(_movementXParam, dirX, playerStateMachine.AnimationCrossFade,
                deltaTime);
            playerStateMachine.Animator.SetFloat(_movementYParam, dirY, playerStateMachine.AnimationCrossFade,
                deltaTime);
        }
    }
}