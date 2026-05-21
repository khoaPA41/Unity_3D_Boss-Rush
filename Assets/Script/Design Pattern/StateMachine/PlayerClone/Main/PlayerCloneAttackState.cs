using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.PlayerClone.Main
{
    public class PlayerCloneAttackState : PlayerCloneBaseState
    {
        private readonly AttackData attackData;
        private float previousTime;
        private bool alreadyApplyForce;

        public PlayerCloneAttackState(PlayerCloneStateMachine cloneStateMachine, int attackDataIndex) : base(
            cloneStateMachine)
        {
            attackData = cloneStateMachine.AttackData[attackDataIndex];
        }

        public override void Enter()
        {
            cloneStateMachine.Animator.CrossFadeInFixedTime(attackData.AnimationName, attackData.AnimationTransition);
        }

        public override void Tick(float deltaTime)
        {
            float normalizedTime = GetNormalizeTime(cloneStateMachine.Animator, attackData.AnimationTag, 0);

            if (normalizedTime > previousTime && normalizedTime <= 1f )
            {
                if (normalizedTime > attackData.ForceTime)
                {
                    TryApplyForce();
                }
                TryCombo(normalizedTime);
            }
            else
            {
                Debug.Log("Exit Attack");
            }
            
            Move(deltaTime);
            FaceTarget();
            previousTime = normalizedTime;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
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
}