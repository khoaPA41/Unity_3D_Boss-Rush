using DesignPattern.StateMachine.PlayerClone.Base;
using UnityEngine;

namespace DesignPattern.StateMachine.PlayerClone.Main
{
    public class PlayerCloneIdleState : PlayerCloneBaseState
    {
        private readonly int _idleSwordAnimationHash = Animator.StringToHash("Sword_Idle");
        public PlayerCloneIdleState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
        {
        }

        public override void Enter()
        {
            cloneStateMachine.Animator.CrossFadeInFixedTime(_idleSwordAnimationHash, cloneStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            cloneStateMachine.CountTime += deltaTime;

            if (!(cloneStateMachine.CountTime >= cloneStateMachine.ChangeChasingState)) return;

            if (IsAttackRange())
            {
                cloneStateMachine.SwitchState(new PlayerCloneAttackState(cloneStateMachine, 0));
                return;
            }
            cloneStateMachine.SwitchState(new PlayerCloneChasingState(cloneStateMachine));
        }

        public override void Exit()
        {
            cloneStateMachine.CountTime = 0f;
        }
    }
}
