using UnityEngine;

public class FinalBossLocomotionState : FinalBossBaseState
{
    readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
    readonly int MovementXParam = Animator.StringToHash("MovementX");
    readonly int MovementYParam = Animator.StringToHash("MovementY");
    Vector3 dir;
    float countTimeToChangeChasing = 0;
    public FinalBossLocomotionState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
    {
    }

    public override void Enter()
    {
        countTimeToChangeChasing = finalBossStateMachine.TimeToEnterChasing;
        finalBossStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, finalBossStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        countTimeToChangeChasing -= deltaTime;

        if (countTimeToChangeChasing <= 0)
        {
            finalBossStateMachine.EnterChasingState();
        }

        dir = GetDirToPlayer();
        UpdateAnimation(deltaTime);
        FaceTarget(dir);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }

    void UpdateAnimation(float deltaTime)
    {
        finalBossStateMachine.Animator.SetFloat(MovementXParam, 0, finalBossStateMachine.AnimationCrossFade, deltaTime);
        finalBossStateMachine.Animator.SetFloat(MovementYParam, 0, finalBossStateMachine.AnimationCrossFade, deltaTime);
    }
}
