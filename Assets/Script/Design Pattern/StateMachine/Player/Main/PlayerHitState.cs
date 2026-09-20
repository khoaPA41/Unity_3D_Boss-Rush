using UnityEngine;

namespace Design_Pattern.StateMachine.Player
{
    public class PlayerHitState : PlayerBaseState
    {
        private readonly int hitAnimationHash = Animator.StringToHash("Hit");
        private readonly int hitKnockbackAnimationHash = Animator.StringToHash("Hit_Knockback");

        private const string HitAnimationTag = "Hit";

        private float _previousTime;

        private readonly bool _isKnockBack;

        private bool _alreadyApplyForce;
        private float force;

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
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, HitAnimationTag, 0);

            if (normalizeTime >= _previousTime && normalizeTime <= 1f)
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

            _previousTime = normalizeTime;
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
            if (_alreadyApplyForce)
            {
                return;
            }

            playerStateMachine.ForceReceiver.AddForce(-playerStateMachine.transform.forward * force);
            _alreadyApplyForce = true;
        }
    }
}