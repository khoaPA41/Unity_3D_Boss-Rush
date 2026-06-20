using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class FreeLookState : PlayerBaseState
    {
        private static readonly int Movement = Animator.StringToHash("Movement");
        private readonly int freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private Vector3 _movement;

        public FreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.InputReader.JumpAction += playerStateMachine.HandleJumpState;
            playerStateMachine.InputReader.DodgeAction += playerStateMachine.HandleDodgeState;
            playerStateMachine.InputReader.SkillAction += playerStateMachine.HandleSkillEvent;
            playerStateMachine.InputReader.TargetAction += playerStateMachine.HandleTargetState;
            playerStateMachine.InputReader.UsePotionAction += playerStateMachine.HandleUsePotionState;
            playerStateMachine.InputReader.UseSubPotionAction += playerStateMachine.HandleUseSubPotionState;


            playerStateMachine.Animator.CrossFadeInFixedTime(freeLookBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            _movement = CalculateMovementInFreeLook();
            float speed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.FreeLookMovementSprintSpeed
                : playerStateMachine.FreeLookMovementSpeed;

            if (_movement != Vector3.zero)
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
            }
            
            playerStateMachine.HandleAttackState();
            playerStateMachine.HandleHeavyAttackState();
            // if (InputBuffering.TryConsume(ActionType.Dodge, out_))
            // {
            //     
            // }
            
            
            Move(_movement * speed, deltaTime);
            UpdateAnimation(deltaTime);
            FaceDir(_movement, deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.SkillAction -= playerStateMachine.HandleSkillEvent;
            playerStateMachine.InputReader.JumpAction -= playerStateMachine.HandleJumpState;
            playerStateMachine.InputReader.DodgeAction -= playerStateMachine.HandleDodgeState;
            playerStateMachine.InputReader.TargetAction -= playerStateMachine.HandleTargetState;
            playerStateMachine.InputReader.UsePotionAction -= playerStateMachine.HandleUsePotionState;
            playerStateMachine.InputReader.UseSubPotionAction -= playerStateMachine.HandleUseSubPotionState;
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
    }
}