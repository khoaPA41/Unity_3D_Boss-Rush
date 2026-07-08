using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerHitState : PlayerBaseState
    {
        private readonly int hitAnimationHash = Animator.StringToHash("Hit");
        private readonly int hitKnockbackAnimationHash = Animator.StringToHash("Hit_Knockback");

        private const string HitAnimationTag = "Hit";

        private float previousTime;

        private readonly bool isKnockBack;

        private bool alreadyApplyForce;
        private float force;

        public PlayerHitState(PlayerStateMachine playerStateMachine, bool isKnockBack) : base(playerStateMachine)
        {
            this.isKnockBack = isKnockBack;
        }

        public override void Enter()
        {
            previousTime = 0;
            alreadyApplyForce = false;
            if (isKnockBack)
            {
                force = playerStateMachine.HitKnockback;
                playerStateMachine.Animator.CrossFadeInFixedTime(hitKnockbackAnimationHash,
                    playerStateMachine.AnimationCrossFade);
            }
            else
            {
                force = playerStateMachine.HitForce;
                playerStateMachine.Animator.CrossFadeInFixedTime(hitAnimationHash,
                    playerStateMachine.AnimationCrossFade);
            }
            playerStateMachine.PlayerSFX.PlayHitSound();
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, HitAnimationTag, 0);

            if (normalizeTime >= previousTime && normalizeTime <= 1f)
            {
                if (normalizeTime >= playerStateMachine.HitForceTime)
                {
                    TryApplyForce(force);
                }
            }
            else
            {
                playerStateMachine.ReturnLocomotion();
            }

            previousTime = normalizeTime;
            Move(deltaTime);
            FaceTarget(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
        }

        private void TryApplyForce(float force)
        {
            if (alreadyApplyForce)
            {
                return;
            }

            playerStateMachine.ForceReceiver.AddForce(-playerStateMachine.transform.forward * force);
            alreadyApplyForce = true;
        }
    }
}