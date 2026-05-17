using Script.Design_Pattern.StateMachine.Boss.Base;
using UnityEngine;

public class FinalBossLocomotionState : FinalBossBaseState
{
    readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
    readonly int MovementParam = Animator.StringToHash("Movement");
    //readonly int MovementYParam = Animator.StringToHash("MovementY");
    Vector3 dir;
    float countTimeToChangeChasing = 0;
    bool isWalk = false;
    public FinalBossLocomotionState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
    {

    }

    public override void Enter()
    {
        finalBossStateMachine.Health.HitAction += finalBossStateMachine.EnterHitState;
        countTimeToChangeChasing = finalBossStateMachine.TimeToEnterChasing;
        finalBossStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, finalBossStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        countTimeToChangeChasing -= deltaTime;

        if (countTimeToChangeChasing <= 0)
        {
            if (IsWalkRange())
            {
                isWalk = true;
            }

            finalBossStateMachine.EnterChasingState(isWalk);
        }

        if (IsAttackRange())
        {
            finalBossStateMachine.EnterAttackState();
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
        finalBossStateMachine.Health.HitAction -= finalBossStateMachine.EnterHitState;
    }

    void UpdateAnimation(float deltaTime)
    {
        finalBossStateMachine.Animator.SetFloat(MovementParam, 0, finalBossStateMachine.AnimationCrossFade, deltaTime);
        //finalBossStateMachine.Animator.SetFloat(MovementYParam, 0, finalBossStateMachine.AnimationCrossFade, deltaTime);
    }
}
