using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossLocomotionState : FinalBossBaseState
    {
        private readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int movementParam = Animator.StringToHash("Movement");
        
        private Vector3 dir;
        private bool isWalk;
        private float animationValue;
        private float speed;
        public FinalBossLocomotionState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
        {
            
        }

        public override void Enter()
        {
            FinalBossStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash, FinalBossStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            Vector2 movementInput = FinalBossStateMachine.InputMovement;
            isWalk = FinalBossStateMachine.isWalking;
            
            if (movementInput == Vector2.zero)
            {
                animationValue = 0;
                speed = 0;
            }
            else
            {
                animationValue = isWalk ? 1 : 2;
                speed = isWalk ? FinalBossStateMachine.SprintSpeed : FinalBossStateMachine.MovementSpeed;
                Vector3 dir = new Vector3(movementInput.x, 0, movementInput.y);
                Move(dir * speed, deltaTime);
            }
            
            if (FinalBossStateMachine.IsAttack)
            {
                FinalBossStateMachine.SwitchState(new FinalBossAttackState(FinalBossStateMachine, FinalBossStateMachine.CurrentComboIndex));
            }
            
            UpdateAnimation(animationValue, deltaTime);
            FaceTarget(FinalBossStateMachine.GetDirToPlayer());
        }

        public override void PhysicTick(float fixedDeltaTime)
        {

        }

        public override void Exit()
        {
            FinalBossStateMachine.InputMovement = Vector2.zero;
        }

        private void UpdateAnimation(float value, float deltaTime)
        {
            FinalBossStateMachine.Animator.SetFloat(movementParam, value, FinalBossStateMachine.AnimationCrossFade, deltaTime);
        }
    }
}
