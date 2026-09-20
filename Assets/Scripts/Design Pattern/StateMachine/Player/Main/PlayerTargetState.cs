using UnityEngine;

namespace Design_Pattern.StateMachine.Player
{
    public class PlayerTargetState : PlayerBaseState
    {
        private readonly int _targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int _movementXParam = Animator.StringToHash("MovementX");
        private readonly int _movementYParam = Animator.StringToHash("MovementY");

        public PlayerTargetState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.TargetPoint.gameObject.SetActive(true);
            playerStateMachine.InputReader.JumpAction += playerStateMachine.HandleJumpState;
            playerStateMachine.InputReader.DodgeAction += playerStateMachine.HandleDodgeState;
            playerStateMachine.InputReader.SkillAction += playerStateMachine.HandleSkillEvent;
            playerStateMachine.InputReader.UsePotionAction += playerStateMachine.HandleUsePotionState;
            playerStateMachine.InputReader.UseSubPotionAction += playerStateMachine.HandleUseSubPotionState;
            playerStateMachine.InputReader.TargetAction += OutTargetState;

            playerStateMachine.Animator.CrossFadeInFixedTime(_targetLookBlendTreeHash,
                playerStateMachine.AnimationCrossFade, 0);

            if (!playerStateMachine.isAttackState)
            {
                playerStateMachine.EnterChangeAction(true);
            }
        }

        public override void Tick(float deltaTime)
        {
            if (playerStateMachine.Targeter.currentTarget == null)
            {
                OutTargetState();
            }

            var _movement = CalculateMovementInTarget();
            var speed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.FreeLookMovementSprintSpeed
                : playerStateMachine.FreeLookMovementSpeed;

            if (_movement != Vector3.zero)
            {
                if (playerStateMachine.InputReader.IsSprint)
                {
                    playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.runReduce);
                }
                else
                {
                    playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.walkReduce);
                }
            }
            else
            {
                playerStateMachine.Stamina.RecoveryStamina();
            }

            if (playerStateMachine.Stamina.CurrentStamina < playerStateMachine.Stamina.walkReduce)
            {
                speed = 0f;
                _movement = Vector3.zero;
            }


            playerStateMachine.HandleAttackState();
            playerStateMachine.HandleHeavyAttackState();
            Move(_movement * speed, deltaTime);
            UpdateAnimation(deltaTime);
            FaceTarget(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            playerStateMachine.TargetPoint.gameObject.SetActive(false);
            playerStateMachine.InputReader.JumpAction -= playerStateMachine.HandleJumpState;
            playerStateMachine.InputReader.DodgeAction -= playerStateMachine.HandleDodgeState;
            playerStateMachine.InputReader.SkillAction -= playerStateMachine.HandleSkillEvent;
            playerStateMachine.InputReader.UsePotionAction -= playerStateMachine.HandleUsePotionState;
            playerStateMachine.InputReader.UseSubPotionAction -= playerStateMachine.HandleUseSubPotionState;
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

            playerStateMachine.Animator.SetFloat(_movementXParam, dirX, playerStateMachine.AnimationCrossFade,
                deltaTime);
            playerStateMachine.Animator.SetFloat(_movementYParam, dirY, playerStateMachine.AnimationCrossFade,
                deltaTime);
        }
    }
}