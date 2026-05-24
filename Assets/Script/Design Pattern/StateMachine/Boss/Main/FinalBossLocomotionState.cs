using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossLocomotionState : FinalBossBaseState
    {
        private readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        private readonly int movementParam = Animator.StringToHash("Movement");
        
        private Vector3 dir;
        private float countTimeToChangeChasing = 0;
        private bool isWalk = false;
        
        public FinalBossLocomotionState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
        {

        }

        public override void Enter()
        {
            IsFinished = false;
            countTimeToChangeChasing = finalBossStateMachine.TimeToEnterChasing;
            finalBossStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash, finalBossStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            countTimeToChangeChasing -= deltaTime;

            if (countTimeToChangeChasing <= 0)
            {
                finalBossStateMachine.IsChasing = true;
                // IsWalkRange();
                // IsFinished = true;
            }

            if (finalBossStateMachine.IsAttack)
            {
                // IsFinished = true;
            }

            dir = GetDirToPlayer();
            IsAttackRange();
            UpdateAnimation(deltaTime);
            FaceTarget(dir);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {

        }

        public override void Exit()
        {
           
            // finalBossStateMachine.Health.HitAction -= finalBossStateMachine.EnterHitState;
        }

        private void UpdateAnimation(float deltaTime)
        {
            finalBossStateMachine.Animator.SetFloat(movementParam, 0, finalBossStateMachine.AnimationCrossFade, deltaTime);
        }
    }
}
