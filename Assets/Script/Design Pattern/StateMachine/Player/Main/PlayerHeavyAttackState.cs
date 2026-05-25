using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerHeavyAttackState : PlayerBaseState
    {
        private readonly int _heavyAttackAnimationHash = Animator.StringToHash("HeavyAttack");
        private const string HeavyAttackAnimationTag = "HeavyAttack";
        private float previousTime;

        public PlayerHeavyAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(_heavyAttackAnimationHash, playerStateMachine.AnimationCrossFade,
                0);
            playerStateMachine.WeaponDealDamage.SetDamage(30);
        }

        public override void Tick(float deltaTime)
        {
            var normalizedTime = GetNormalizeTime(playerStateMachine.Animator, HeavyAttackAnimationTag, 0);
            if (normalizedTime >= previousTime && normalizedTime > .8f)
            {
                playerStateMachine.ReturnLocomotion();
            }

            previousTime = normalizedTime;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        
        }

        public override void Exit()
        {
        
        }
    }
}
