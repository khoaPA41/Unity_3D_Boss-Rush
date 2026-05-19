using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Base
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void PhysicTick(float fixedDeltaTime);
        public abstract void Exit();

        protected static float GetNormalizeTime(Animator animator, string animationTag, int layer)
        {
            var currentState = animator.GetCurrentAnimatorStateInfo(layer);
            var nextState = animator.GetNextAnimatorStateInfo(layer);

            if (nextState.IsTag(animationTag) && animator.IsInTransition(layer)) // if blending (transition)
            {
                return nextState.normalizedTime;
            }
            else if (currentState.IsTag(animationTag) && !animator.IsInTransition(layer)) // if not blending
            {
                return currentState.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }
    }
}
