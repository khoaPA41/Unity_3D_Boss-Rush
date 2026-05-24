using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

public class FinalBossHitState : FinalBossBaseState
{
    readonly int HitAnimationOneHash = Animator.StringToHash("Hit");
    readonly string HitAnimationTag = "Hit";
    public FinalBossHitState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
    {
    }

    public override void Enter()
    {
        IsFinished = false;
        finalBossStateMachine.Animator.CrossFadeInFixedTime(HitAnimationOneHash, finalBossStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(finalBossStateMachine.Animator, HitAnimationTag, 0);
        if (normalizeTime > 0.8 && normalizeTime <= 1f)
        {
            // finalBossStateMachine.ReturnLocomotion();
            IsFinished = true;
        }
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }
}
