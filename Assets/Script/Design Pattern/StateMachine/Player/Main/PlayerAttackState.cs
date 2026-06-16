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
            playerStateMachine.CheckStamina();
            playerStateMachine.Stamina.ChangeStamina(playerStateMachine.Stamina.lightAttackReduce);
            playerStateMachine.Animator.CrossFadeInFixedTime(_attackData.AnimationName, _attackData.AnimationTransition,
                0);
            // playerStateMachine.WeaponDealDamage.SetDamage(_attackData.AttackDamage);
            playerStateMachine.DealDamage.SetDamage(_attackData.AttackDamage);
        }

        public override void Tick(float deltaTime)
        {
            
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _attackData.AnimationTag, 0);
            if (normalizeTime >= _previousTime && normalizeTime <= 1f)
            {
                
                if (normalizeTime >= _attackData.AttackAnimationTime)
                {
                    
                    if (playerStateMachine.InputBuffering.TryConsume(ActionType.Dodge))
                    {
                        playerStateMachine.SwitchState(new PlayerDodgeState(playerStateMachine));
                        return;
                    }
                    
                    if (playerStateMachine.InputBuffering.TryConsume(ActionType.Attack))
                    {
                        TryCombo(normalizeTime);
                    }
                    
                    if (playerStateMachine.InputBuffering.TryConsume(ActionType.Jump))
                    {
                        playerStateMachine.SwitchState(new PlayerStartJumpState(playerStateMachine));
                        return;
                    }
                }
                
                if (normalizeTime >= _attackData.ForceTime)
                {
                    TryApplyForce();
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


            // if (playerStateMachine.InputBuffering.TryConsume(ActionType.Attack))
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