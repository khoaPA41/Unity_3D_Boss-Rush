using System;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine
{
    public class BossHitState : BossBaseState
    {
        readonly int HitAnimationOneHash = Animator.StringToHash("Hit");
        readonly string HitAnimationTag = "Hit";
        private float previousTime;
        public BossHitState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {
            previousTime = 0f;
            bossStateMachine.Animator.CrossFadeInFixedTime(HitAnimationOneHash, bossStateMachine.AnimationCrossFade);
            bossStateMachine.PlayerSFX.PlayHitSound();
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, HitAnimationTag, 0);
            if (normalizeTime >= previousTime && normalizeTime <= 1f)
            {
                bossStateMachine.ReturnLocomotion();
            }
            previousTime = normalizeTime;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}
