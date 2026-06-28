using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.PlayerClone.Main
{
    public class PlayerCloneChasingState : PlayerCloneBaseState
    {
        private static readonly int _movementAnimation = Animator.StringToHash("Movement");
        
        private Vector3 _movement;

        public PlayerCloneChasingState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
        {
        }

        public override void Enter()
        {
            // IsFinished = false;
            cloneStateMachine.Animator.CrossFadeInFixedTime(_movementAnimation, cloneStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            Move(DirToTarget() * cloneStateMachine.MovementSpeed, deltaTime);
            if (IsAttackRange())
            {
                cloneStateMachine.SwitchState(new PlayerCloneAttackState(cloneStateMachine, 0));
            }
            FaceTarget();
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            // cloneStateMachine.IsChasing = false;
        }
    }
}