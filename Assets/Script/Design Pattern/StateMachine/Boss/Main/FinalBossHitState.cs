using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Boss.Main;
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
       FinalBossStateMachine.Animator.CrossFadeInFixedTime(HitAnimationOneHash, FinalBossStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(FinalBossStateMachine.Animator, HitAnimationTag, 0);
        if (normalizeTime >= 1f)
        {
            FinalBossStateMachine.ReturnLocomotion();
        }
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }
}
