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

        private float countPerfectFrame = 0;
        public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            
        }

        public override void Enter()
        {
            if (playerStateMachine.CheckLowStamina())
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }
            playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.dodgeReduce);
            playerStateMachine.Health.isPerfectDodge = true;
            dodgeDirection = playerStateMachine.InputReader.InputMovement;
            playerStateMachine.Animator.SetFloat(DodgeRightHash, dodgeDirection.x);
            playerStateMachine.Animator.SetFloat(DodgeForwardHash, dodgeDirection.y);
            playerStateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);
            remainingTime = playerStateMachine.DodgeDuration;

            playerStateMachine.Health.DodgeAwardAction += playerStateMachine.DodgeAward.DodgeAwardActive;
        }

        public override void Tick(float deltaTime)
        {
            CheckCounterattack();
            
            countPerfectFrame += deltaTime;
            var movement = CalculateMovement();

            Move(movement, deltaTime);
            FaceTarget(deltaTime);

            remainingTime -= deltaTime;

            if (remainingTime <= .3f)
            {
                playerStateMachine.Health.isPerfectDodge = false;
            }
            if (remainingTime <= 0f)
            {
                playerStateMachine.ReturnLocomotion();
            }

            if (remainingTime < playerStateMachine.DodgeDuration * 0.2f)
            {
                            
                if (playerStateMachine.InputBuffering.TryConsume(ActionType.Attack))
                {
                    playerStateMachine.SwitchState(new PlayerAttackState(playerStateMachine, 0));
                }
            }
        }

        public override void PhysicTick(float fixedDeltaTime)
        {

        }

        public override void Exit()
        {
            playerStateMachine.Health.isPerfectDodge = false;
            playerStateMachine.Health.noDamage = false;
            playerStateMachine.Health.DodgeAwardAction -= playerStateMachine.DodgeAward.DodgeAwardActive;
        }

        private Vector3 CalculateMovement()
        {
            var movement = new Vector3();

            movement += playerStateMachine.transform.right * (dodgeDirection.x * playerStateMachine.DodgeLength) / playerStateMachine.DodgeDuration;
            movement += playerStateMachine.transform.forward * (dodgeDirection.y * playerStateMachine.DodgeLength) / playerStateMachine.DodgeDuration;

            return movement;
        }

        private void CheckCounterattack()
        {
            if (playerStateMachine.IsCounterAttack && playerStateMachine.InputReader.IsAttack)
            {
                playerStateMachine.SwitchState(new PlayerCounterAttackState(playerStateMachine));
            }
        }
    }
}
