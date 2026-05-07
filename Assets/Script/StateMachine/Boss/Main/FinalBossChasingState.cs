using UnityEngine;

public class FinalBossChasingState : FinalBossBaseState
{
    readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
    readonly int MovementXParam = Animator.StringToHash("MovementX");
    readonly int MovementYParam = Animator.StringToHash("MovementY");
    public FinalBossChasingState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
    {
    }

    public override void Enter()
    {
        finalBossStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, finalBossStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 dir = GetDirToPlayer();
        UpdateAnimation(deltaTime);
        Move(dir * finalBossStateMachine.MovementSpeed, deltaTime);
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

    }

    void UpdateAnimation(float deltaTime)
    {
        finalBossStateMachine.Animator.SetFloat(MovementXParam, 2, finalBossStateMachine.AnimationCrossFade, deltaTime);
        finalBossStateMachine.Animator.SetFloat(MovementYParam, 2, finalBossStateMachine.AnimationCrossFade, deltaTime);
    }
}
