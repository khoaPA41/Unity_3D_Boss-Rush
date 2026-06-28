using Script.Design_Pattern.StateMachine.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.PlayerClone.Base
{
    public abstract class PlayerCloneBaseState : State
    {
        protected readonly PlayerCloneStateMachine cloneStateMachine;
        private AnimatorOverrideController overrideController;

        protected PlayerCloneBaseState(PlayerCloneStateMachine cloneStateMachine)
        {
            this.cloneStateMachine = cloneStateMachine;
        }


        protected void Move(Vector3 motion, float deltaTime)
        {
            cloneStateMachine.CharacterController.Move((motion + cloneStateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }


        protected Vector3 DirToTarget()
        {
            return (cloneStateMachine.Target.transform.position - cloneStateMachine.transform.position).normalized;
        }

        protected void FaceTarget()
        {
            cloneStateMachine.transform.rotation = Quaternion.LookRotation(DirToTarget());
        }

        protected bool IsAttackRange()
        {
            return (cloneStateMachine.Target.transform.position - cloneStateMachine.transform.position).sqrMagnitude <=
                                         cloneStateMachine.AttackRange * cloneStateMachine.AttackRange;
        }
    }
}