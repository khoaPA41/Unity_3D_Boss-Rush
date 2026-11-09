using Attack;
using DesignPattern.StateMachine.PlayerClone.Base;
namespace DesignPattern.StateMachine.PlayerClone.Main
{
    public class PlayerCloneAttackState : PlayerCloneBaseState
    {
        private readonly AttackData _attackData;
        private float _previousTime;
        private bool _alreadyApplyForce;
        public PlayerCloneAttackState(PlayerCloneStateMachine cloneStateMachine, int attackDataIndex) : base(cloneStateMachine)
        {
            _attackData = cloneStateMachine.AttackData[attackDataIndex];
        }

        public override void Enter()
        {
            cloneStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName, _attackData.AnimationTransition);
            cloneStateMachine.WeaponDealDamage.SetDamage(_attackData.AttackDamage);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(cloneStateMachine.Animator, _attackData.AnimationTag, 0);
            if (normalizeTime >= _previousTime && normalizeTime <= 1f)
            {
                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
                }

                if (IsAttackRange())
                {
                    if (TryCombo(normalizeTime)) return;
                }
            }
            else
            {
                cloneStateMachine.SwitchState(new PlayerCloneIdleState(cloneStateMachine));
                return;
            }

            _previousTime = normalizeTime;
            FaceTarget();
            Move(deltaTime);
        }

        public override void Exit()
        {
        }

        private bool TryCombo(float normalizeTime)
        {
            if (_attackData.NextAttackDataIndex == -1)
            {
                return false;
            }

            if (normalizeTime < _attackData.AttackAnimationTime)
            {
                return false;
            }

            cloneStateMachine.SwitchState(new PlayerCloneAttackState(
                cloneStateMachine,
                _attackData.NextAttackDataIndex
            ));
            return true;
        }

        private void TryApplyForce()
        {
            if (_alreadyApplyForce)
            {
                return;
            }

            cloneStateMachine.ForceReceiver.AddForce(cloneStateMachine.transform.forward * _attackData.Force);
            _alreadyApplyForce = true;
        }
    }
}
