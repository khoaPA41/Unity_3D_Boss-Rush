using UnityEngine;

public class FinalBossLocomotionState : FinalBossBaseState
{
    readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
    readonly int MovementXParam = Animator.StringToHash("MovementX");
    readonly int MovementYParam = Animator.StringToHash("MovementY");
    Vector3 dir;

    public FinalBossLocomotionState(FinalBossStateMachine finalBossStateMachine) : base(finalBossStateMachine)
    {
    }

    public override void Enter()
    {
        finalBossStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, finalBossStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        dir = GetDirToPlayer();
        Move(GetDirToPlayer() * finalBossStateMachine.MovementSpeed, deltaTime);
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
        float dirX = 0f;
        float dirY = 0f;

        if (dir.x != 0)
        {
            dirX = Mathf.Sign(dir.x);

        }
        if (dir.y != 0)
        {
            dirY = Mathf.Sign(dir.y);
        }

        finalBossStateMachine.Animator.SetFloat(MovementXParam, dirX, finalBossStateMachine.AnimationCrossFade, deltaTime);
        finalBossStateMachine.Animator.SetFloat(MovementYParam, dirY, finalBossStateMachine.AnimationCrossFade, deltaTime);
    }
}
