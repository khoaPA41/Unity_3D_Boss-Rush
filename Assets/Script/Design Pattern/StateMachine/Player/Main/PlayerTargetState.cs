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
            playerStateMachine.InputReader.JumpAction += EnterJumpState;
            playerStateMachine.InputReader.TargetAction += OutTargetState;
            playerStateMachine.InputReader.DodgeAction += EnterDodgeState;
            playerStateMachine.InputReader.SkillAction += UseSkill;

            playerStateMachine.Health.HitAction += playerStateMachine.EnterHitState;

            playerStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);

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

            if (playerStateMachine.InputReader.IsAttack)
            {
                playerStateMachine.EnterAttackState(0);
            }

            Vector3 movement = CalculateMovementInTarget();
            Move(movement * playerStateMachine.FreeLookMovementSpeed, deltaTime);
            UpdateAnimation(deltaTime);
            FaceTarget(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {

        }

        public override void Exit()
        {
            playerStateMachine.InputReader.TargetAction -= OutTargetState;
            playerStateMachine.InputReader.JumpAction -= EnterJumpState;
            playerStateMachine.InputReader.DodgeAction -= EnterDodgeState;
            playerStateMachine.InputReader.SkillAction -= UseSkill;
            playerStateMachine.Health.HitAction -= playerStateMachine.EnterHitState;
        }

        void OutTargetState()
        {
            playerStateMachine.Targeter.CancelTarget();
            playerStateMachine.EnterChangeAction(false);
            return;
        }

        void EnterDodgeState()
        {
            playerStateMachine.SwitchState(new PlayerDodgeState(playerStateMachine, playerStateMachine.InputReader.InputMovement));
        }

        void UpdateAnimation(float deltaTime)
        {
            float dirX = 0f;
            float dirY = 0f;
            if (playerStateMachine.InputReader.InputMovement.x != 0)
            {
                dirX = Mathf.Sign(playerStateMachine.InputReader.InputMovement.x);

            }
            if (playerStateMachine.InputReader.InputMovement.y != 0)
            {
                dirY = Mathf.Sign(playerStateMachine.InputReader.InputMovement.y);
            }

            playerStateMachine.Animator.SetFloat(movementXParam, dirX, playerStateMachine.AnimationCrossFade, deltaTime);
            playerStateMachine.Animator.SetFloat(movementYParam, dirY, playerStateMachine.AnimationCrossFade, deltaTime);
        }

    }
}
