using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossChasingState : FinalBossBaseState
    {
        readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        readonly int MovementParam = Animator.StringToHash("Movement");
        
        private readonly float animationValue = 2;
        private readonly float speed;
        private bool isWalk;
        public FinalBossChasingState(FinalBossStateMachine finalBossStateMachine, bool isWalk) : base(finalBossStateMachine)
        {
            this.isWalk = isWalk;
            if (isWalk)
            {
                animationValue = 1;
                speed = finalBossStateMachine.MovementSpeed;
            }
            else
            {
                speed = finalBossStateMachine.SprintSpeed;
            }
        }

        public override void Enter()
        {
            IsFinished = false;
            finalBossStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, finalBossStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 dir = GetDirToPlayer();

            if (finalBossStateMachine.IsAttack)
            {
                IsFinished = true;
            }
            
            IsAttackRange();
            UpdateAnimation(deltaTime, animationValue);
            Move(dir * speed, deltaTime);
            FaceTarget(dir);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
        }

        public override void Exit()
        {
            finalBossStateMachine.IsChasing = false;
        }

        private void UpdateAnimation(float deltaTime, float value)
        {
            finalBossStateMachine.Animator.SetFloat(MovementParam, value, finalBossStateMachine.AnimationCrossFade, deltaTime);
        }
    }
}
