using Script.Design_Pattern.StateMachine.Player.Base;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerAttackState : PlayerBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;

        public PlayerAttackState(PlayerStateMachine playerStateMachine, int attackDataIndex) : base(playerStateMachine)
        {
            _attackData = playerStateMachine.AttackData[attackDataIndex];
        }

        public override void Enter()
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName, _attackData.AnimationTransition,
                0);
            playerStateMachine.WeaponDealDamage.SetDamage(_attackData.AttackDamage);
        }

        public override void Tick(float deltaTime)
        {
            float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _attackData.AnimationTag, 0);
            if (normalizeTime >= _previousTime && normalizeTime <= 1f)
            {
                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
                }

                if (playerStateMachine.InputReader.IsAttack)
                {
                    TryCombo(normalizeTime);
                }
            }
            else
            {
                playerStateMachine.ReturnLocomotion();
            }

            _previousTime = normalizeTime;
            FaceTarget(deltaTime);
            Move(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            //playerStateMachine.Animator.CrossFadeInFixedTime("Sword_Regular_A_Rec", playerStateMachine.AnimationCrossFade);
        }

        private void TryCombo(float normalizeTime)
        {
            if (_attackData.NextAttackDataIndex == -1)
            {
                return;
            }

            if (normalizeTime < _attackData.AttackAnimationTime)
            {
                return;
            }

            playerStateMachine.SwitchState(new PlayerAttackState(
                playerStateMachine,
                _attackData.NextAttackDataIndex
            ));
        }

        private void TryApplyForce()
        {
            if (_alreadyApplyForce)
            {
                return;
            }

            playerStateMachine.ForceReceiver.AddForce(playerStateMachine.transform.forward * _attackData.Force);
            _alreadyApplyForce = true;
        }
    }
}