using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerHeavyAttackState : PlayerBaseState
    {
        private readonly int _heavyAttackAnimationHash = Animator.StringToHash("HeavyAttack");
        private const string HeavyAttackAnimationTag = "HeavyAttack";
        
        private const float HoldTimeLimit = 1f;
        private float _holdTime;
        private float _holdDamage;
        
        private float _previousTime;
        
        public PlayerHeavyAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            _holdTime = 0f;
            if (playerStateMachine.CheckLowStamina())
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }

            playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.heavyAttackReduce);
            playerStateMachine.Animator.CrossFadeInFixedTime(_heavyAttackAnimationHash, playerStateMachine.AnimationCrossFade,
                0);
            
        }

        public override void Tick(float deltaTime)
        {

            CountHoldTime(deltaTime); // Calculate hold time
            // CalculateDamage();
            
            
            var normalizedTime = GetNormalizeTime(playerStateMachine.Animator, HeavyAttackAnimationTag, 0);
            if (normalizedTime >= _previousTime && normalizedTime > .8f)
            {
                playerStateMachine.ReturnLocomotion();
            }

            _previousTime = normalizedTime;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.IsHeavyAttack = false;
            playerStateMachine.Animator.speed = 1f;
            Debug.Log("Out");
        }

        private void CountHoldTime(float deltaTime)
        {
            if (playerStateMachine.InputReader.isCharging)
            {
                playerStateMachine.Animator.speed = .3f;
                _holdTime += deltaTime;
                _holdTime = Mathf.Clamp(_holdTime, 0f, HoldTimeLimit);
            }

            if (_holdTime >= HoldTimeLimit || !playerStateMachine.InputReader.isCharging)
            {
                playerStateMachine.InputReader.isCharging = false;
                playerStateMachine.Animator.speed = 1f;
                CalculateDamage();
            }
        }

        private void CalculateDamage()
        {
            _holdDamage = playerStateMachine.AttackData[0].AttackDamage * _holdTime * 3f;
            playerStateMachine.DealDamage.SetDamage(_holdDamage);
        }
    }
}
