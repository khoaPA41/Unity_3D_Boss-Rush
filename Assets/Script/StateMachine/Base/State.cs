using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void PhysicTick(float fixedDeltaTime);
    public abstract void Exit();

    public float GetNormalizeTime(Animator animator, string animationTag)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);

        if (nextState.IsTag(animationTag) && animator.IsInTransition(0)) // if blending (transition)
        {
            return nextState.normalizedTime;
        }
        else if (currentState.IsTag(animationTag) && !animator.IsInTransition(0)) // if not blending
        {
            return currentState.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
