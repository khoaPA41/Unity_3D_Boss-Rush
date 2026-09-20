using System;
using UnityEngine;

namespace Design_Pattern.StateMachine.Boss
{
    public class BossHitState : BossBaseState
    {
        readonly int HitAnimationHash = Animator.StringToHash("Hit");
        readonly string HitAnimationTag = "Hit";
        private float previousTime;
        public BossHitState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {
            previousTime = 0f;
            bossStateMachine.Animator.CrossFadeInFixedTime(HitAnimationHash, bossStateMachine.AnimationCrossFade);
            // bossStateMachine.PlayerSFX.PlayHitSound();
            bossStateMachine.BossBehaviorBrain.HitReceived++;
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, HitAnimationTag, 0);
            if (normalizeTime > previousTime && normalizeTime >= .9f)
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
