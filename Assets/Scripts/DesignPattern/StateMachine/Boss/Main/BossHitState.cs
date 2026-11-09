using DesignPattern.StateMachine.Boss.Base;
using System;
using UnityEngine;

namespace DesignPattern.StateMachine.Boss.Main
{
    public class BossHitState : BossBaseState
    {
        readonly int _hitAnimationHash = Animator.StringToHash("Hit");
        readonly string _hitAnimationTag = "Hit";
        private float _previousTime;
        public BossHitState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {
            _previousTime = 0f;
            bossStateMachine.Animator.CrossFadeInFixedTime(_hitAnimationHash, bossStateMachine.AnimationCrossFade);
            bossStateMachine.BossBehaviorBrain.HitReceived++;
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, _hitAnimationTag, 0);
            if (normalizeTime > _previousTime && normalizeTime >= .9f)
            {
                bossStateMachine.ReturnLocomotion();
            }
            _previousTime = normalizeTime;
        }

        public override void Exit()
        {
        }
    }
}
