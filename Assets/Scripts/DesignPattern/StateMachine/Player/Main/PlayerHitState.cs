using DesignPattern.StateMachine.Player.Base;
using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerHitState : PlayerBaseState
    {
        private readonly int _hitAnimationHash = Animator.StringToHash("Hit");
        private readonly int _hitKnockbackAnimationHash = Animator.StringToHash("Hit_Knockback");

        private const string HitAnimationTag = "Hit";

        private float _previousTime;

        private readonly bool _isKnockBack;

        private bool _alreadyApplyForce;
        private float _force;

        public PlayerHitState(PlayerStateMachine playerStateMachine, bool isKnockBack) : base(playerStateMachine)
        {
            _isKnockBack = isKnockBack;
        }

        public override void Enter()
        {
            _previousTime = 0;
            _alreadyApplyForce = false;
            if (_isKnockBack)
            {
                _force = playerStateMachine.HitKnockback;
                playerStateMachine.Animator.CrossFadeInFixedTime(_hitKnockbackAnimationHash,
                    playerStateMachine.AnimationCrossFade);
            }
            else
            {
                _force = playerStateMachine.HitForce;
                playerStateMachine.Animator.CrossFadeInFixedTime(_hitAnimationHash,
                    playerStateMachine.AnimationCrossFade);
            }
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, HitAnimationTag, 0);

            if (normalizeTime >= _previousTime && normalizeTime <= 1f)
            {
                if (normalizeTime >= playerStateMachine.HitForceTime)
                {
                    TryApplyForce(_force);
                }
            }
            else
            {
                playerStateMachine.ReturnLocomotion();
                return;
            }

            _previousTime = normalizeTime;
            Move(deltaTime);
            FaceTarget(deltaTime);
        }

        public override void Exit()
        {
        }

        private void TryApplyForce(float force)
        {
            if (_alreadyApplyForce)
            {
                return;
            }

            playerStateMachine.ForceReceiver.AddForce(-playerStateMachine.transform.forward * force);
            _alreadyApplyForce = true;
        }
    }
}