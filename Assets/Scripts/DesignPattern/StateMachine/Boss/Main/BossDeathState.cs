using DesignPattern.StateMachine.Boss.Base;
using UnityEngine;


namespace DesignPattern.StateMachine.Boss.Main
{
    public class BossDeathState : BossBaseState
    {
        readonly int _deathAnimationOneHash = Animator.StringToHash("Death_1");
        readonly int _deathAnimationTwoHash = Animator.StringToHash("Death_2");
        readonly string _deathAnimationTag = "Death";

        private float _previousTime;
        int _randomAnimation;
        bool _isDead;
        public BossDeathState(BossStateMachine bossStateMachine) : base(bossStateMachine)
        {
        }

        public override void Enter()
        {

            _randomAnimation = Random.Range(0, 2);
            bossStateMachine.Animator.CrossFadeInFixedTime(
                _randomAnimation == 0 ? _deathAnimationOneHash : _deathAnimationTwoHash,
                bossStateMachine.AnimationCrossFade);
            bossStateMachine.CharacterController.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(bossStateMachine.Animator, _deathAnimationTag, 0);
            if (normalizeTime > _previousTime && normalizeTime >= .9f)
            {
                if (!_isDead)
                {
                    bossStateMachine.BossBehaviorBrain.IsDead = true;
                    _isDead = true;
                }
            }
            _previousTime = normalizeTime;
        }

        public override void Exit()
        {
        }
    }
}