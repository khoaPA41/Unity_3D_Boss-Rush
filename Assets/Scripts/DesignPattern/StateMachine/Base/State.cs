using UnityEngine;

namespace DesignPattern.StateMachine.Base
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void Exit();

        protected static float GetNormalizeTime(Animator animator, string animationTag, int layer)
        {
            var CurrentState = animator.GetCurrentAnimatorStateInfo(layer);
            var nextState = animator.GetNextAnimatorStateInfo(layer);

            if (nextState.IsTag(animationTag) && animator.IsInTransition(layer)) // if blending (transition)
            {
                return nextState.normalizedTime;
            }
            else if (CurrentState.IsTag(animationTag) && !animator.IsInTransition(layer)) // if not blending
            {
                return CurrentState.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }
    }
}
