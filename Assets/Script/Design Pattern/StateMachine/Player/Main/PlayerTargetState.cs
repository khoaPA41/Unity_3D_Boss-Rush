using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerTargetState : PlayerBaseState
    {
        private readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int movementXParam = Animator.StringToHash("MovementX");
        private readonly int movementYParam = Animator.StringToHash("MovementY");

        public PlayerTargetState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.InputReader.JumpAction += playerStateMachine.HandleJumpState;
            playerStateMachine.InputReader.DodgeAction += playerStateMachine.HandleDodgeState;
            playerStateMachine.InputReader.SkillAction += playerStateMachine.HandleSkillEvent;
            playerStateMachine.InputReader.TargetAction += OutTargetState;

            playerStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash,
                playerStateMachine.AnimationCrossFade, 0);

            if (!playerStateMachine.isAttackState)
            {
                playerStateMachine.EnterChangeAction(true);
            }
        }

        public override void Tick(float deltaTime)
        {
            if (playerStateMachine.Targeter.currentTarget is null)
            {
                OutTargetState();
            }

            playerStateMachine.HandleAttackState();
            playerStateMachine.HandleHeavyAttackState();
            var movement = CalculateMovementInTarget();
            var speed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.FreeLookMovementSprintSpeed
                : playerStateMachine.FreeLookMovementSpeed;
            
            if (movement != Vector3.zero)
            {
                playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.movementReduce);
            }
            else
            {
                playerStateMachine.Stamina.RecoveryStamina();
            }
            
            if (playerStateMachine.Stamina.currentStamina <= 0f)
            {
                speed = 0f;
                movement = Vector3.zero;
            }
            
            Move(movement * speed, deltaTime);
            UpdateAnimation(deltaTime);
            FaceTarget(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.JumpAction -= playerStateMachine.HandleJumpState;
            playerStateMachine.InputReader.DodgeAction -= playerStateMachine.HandleDodgeState;
            playerStateMachine.InputReader.SkillAction -= playerStateMachine.HandleSkillEvent;
            playerStateMachine.InputReader.TargetAction -= OutTargetState;
        }

        private void OutTargetState()
        {
            playerStateMachine.Targeter.CancelTarget();
            playerStateMachine.EnterChangeAction(false);
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

            playerStateMachine.Animator.SetFloat(movementXParam, dirX, playerStateMachine.AnimationCrossFade,
                deltaTime);
            playerStateMachine.Animator.SetFloat(movementYParam, dirY, playerStateMachine.AnimationCrossFade,
                deltaTime);
        }
    }
}