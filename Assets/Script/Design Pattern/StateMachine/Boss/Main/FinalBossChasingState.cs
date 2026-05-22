using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Main
{
    public class FinalBossChasingState : FinalBossBaseState
    {
        readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
        readonly int MovementParam = Animator.StringToHash("Movement");
        //readonly int MovementYParam = Animator.StringToHash("MovementY");
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
            finalBossStateMachine.Health.HitAction += finalBossStateMachine.EnterHitState;
            finalBossStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, finalBossStateMachine.AnimationCrossFade);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 dir = GetDirToPlayer();
            //speed = animationValue == 1 ? finalBossStateMachine.SprintSpeed : finalBossStateMachine.MovementSpeed;
            UpdateAnimation(deltaTime, animationValue);
            Move(dir * speed, deltaTime);
            FaceTarget(dir);
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
            if (IsAttackRange())
            {
                finalBossStateMachine.EnterAttackState();
            }
        }

        public override void Exit()
        {
            finalBossStateMachine.Health.HitAction -= finalBossStateMachine.EnterHitState;

        }

        void UpdateAnimation(float deltaTime, float value)
        {
            finalBossStateMachine.Animator.SetFloat(MovementParam, value, finalBossStateMachine.AnimationCrossFade, deltaTime);
            //finalBossStateMachine.Animator.SetFloat(MovementYParam, value, finalBossStateMachine.AnimationCrossFade, deltaTime);
        }
    }
}
