using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;

public class PlayerCloneHitState : PlayerCloneBaseState
{
    private readonly int HitAnimationHash =  Animator.StringToHash("Hit");
    private readonly string HitAnimationTag = "Hit";
    
    float previousTime = 0;
    
    public PlayerCloneHitState(PlayerCloneStateMachine cloneStateMachine) : base(cloneStateMachine)
    {
    }

    public override void Enter()
    {
        IsFinished = false;
        cloneStateMachine.Animator.CrossFadeInFixedTime(HitAnimationHash, cloneStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizeTime(cloneStateMachine.Animator, HitAnimationTag, 0);
        if (normalizedTime > previousTime && normalizedTime > .8f)
        {
            IsFinished = true;
        }
        
        previousTime = normalizedTime;
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
    }
}
