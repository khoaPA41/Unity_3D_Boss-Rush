using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.PlayerClone.Main
{
    public class PlayerCloneChasingState : PlayerCloneBaseState
    {
        private static readonly int Movement = Animator.StringToHash("Movement");
        
        private Vector3 movement;

        public PlayerCloneChasingState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
        {
        }

        public override void Enter()
        {
            
            cloneStateMachine.Animator.CrossFadeInFixedTime(Movement, cloneStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 dir = DirToTarget();
            if (IsAttackRange())
            {
                cloneStateMachine.SwitchState(new PlayerCloneAttackState(cloneStateMachine, 0));
            }

            Move(DirToTarget() * cloneStateMachine.MovementSpeed, deltaTime);
            FaceTarget();
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            
        }
    }
}