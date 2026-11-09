using DesignPattern.StateMachine.PlayerClone.Base;
using UnityEngine;

namespace DesignPattern.StateMachine.PlayerClone.Main
{
    public class PlayerCloneChasingState : PlayerCloneBaseState
    {
        private static readonly int _movementAnimation = Animator.StringToHash("Movement");

        public PlayerCloneChasingState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
        {
        }

        public override void Enter()
        {
            cloneStateMachine.Animator.CrossFadeInFixedTime(_movementAnimation, cloneStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            Move(DirToTarget() * cloneStateMachine.MovementSpeed, deltaTime);
            if (IsAttackRange())
            {
                cloneStateMachine.SwitchState(new PlayerCloneAttackState(cloneStateMachine, 0));
                return;
            }
            FaceTarget();
        }

        public override void Exit()
        {
        }
    }
}
