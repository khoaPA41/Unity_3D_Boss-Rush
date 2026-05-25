using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

public class PlayerCloneAttackState : PlayerCloneBaseState
{
    private readonly AttackData attackData;
    private float previousTime = 0f;
    private bool alreadyApplyForce;
    public PlayerCloneAttackState(PlayerCloneStateMachine cloneStateMachine, int attackDataIndex) : base(cloneStateMachine)
    {
        attackData = cloneStateMachine.AttackData[attackDataIndex];
    }
    
     public override void Enter()
        {
            IsFinished = false;
            cloneStateMachine.Animator.CrossFadeInFixedTime(attackData.AnimationName, attackData.AnimationTransition);
            cloneStateMachine.WeaponDealDamage.SetDamage(attackData.AttackDamage);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(cloneStateMachine.Animator, attackData.AnimationTag, 0);
            if (normalizeTime >= previousTime && normalizeTime <= 1f)
            {
                if (normalizeTime >= attackData.ForceTime)
                {
                    TryApplyForce();
                }

                if (cloneStateMachine.IsAttack)
                {
                    TryCombo(normalizeTime);
                }
            }
            else
            {
                IsFinished = true;
                cloneStateMachine.IsAttack = false;
            }

            previousTime = normalizeTime;
            FaceTarget();
            Move(deltaTime);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            IsFinished = false;
        }

        private void TryCombo(float normalizeTime)
        {
            if (attackData.NextAttackDataIndex == -1)
            {
                return;
            }

            if (normalizeTime < attackData.AttackAnimationTime)
            {
                return;
            }

            cloneStateMachine.SwitchState(new PlayerCloneAttackState(
                cloneStateMachine,
                attackData.NextAttackDataIndex
            ));
        }

        private void TryApplyForce()
        {
            if (alreadyApplyForce)
            {
                return;
            }

            cloneStateMachine.ForceReceiver.AddForce(cloneStateMachine.transform.forward * attackData.Force);
            alreadyApplyForce = true;
        }
}


