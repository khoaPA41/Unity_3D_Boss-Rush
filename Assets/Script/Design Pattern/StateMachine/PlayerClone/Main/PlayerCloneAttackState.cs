using Script.Design_Pattern.StateMachine.PlayerClone.Base;

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
            // IsFinished = false;
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
                    TryCombo(normalizeTime);
                }
            }
            else
            {
                cloneStateMachine.SwitchState(new PlayerCloneIdleState(cloneStateMachine));
            }

            _previousTime = normalizeTime;
            FaceTarget();
            Move(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            // IsFinished = false;
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

            cloneStateMachine.SwitchState(new PlayerCloneAttackState(
                cloneStateMachine,
                _attackData.NextAttackDataIndex
            ));
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


