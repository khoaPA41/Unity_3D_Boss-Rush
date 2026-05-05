using UnityEngine;

public class PlayerChangeAction : PlayerBaseState
{
    readonly int SwordEnterAnimationHash = Animator.StringToHash("Sword_Enter");
    readonly int SwordExitAnimationHash = Animator.StringToHash("Sword_Exit");
    readonly string SwordChangeTag = "SwordChange";
    readonly string IdleAnimationName = "Idle_Loop";
    readonly string SwordIdleAnimationName = "Sword_Idle";
    bool isSwordEnter;

    public PlayerChangeAction(PlayerStateMachine playerStateMachine, bool isSwordEnter) : base(playerStateMachine)
    {
        this.isSwordEnter = isSwordEnter;
    }

    public override void Enter()
    {
        if (isSwordEnter)
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(SwordEnterAnimationHash, playerStateMachine.AnimationCrossFade);
            playerStateMachine.isAttackState = true;
        }
        else
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(SwordExitAnimationHash, playerStateMachine.AnimationCrossFade);
            playerStateMachine.isAttackState = false;
        }
    }
    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, SwordChangeTag);
        if (normalizeTime > .9f && normalizeTime <= 1f)
        {
            playerStateMachine.ReturnLocomotion();
        }
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {
        if (isSwordEnter)
        {
            ChangeSwordIdle(IdleAnimationName, playerStateMachine.SwordIdleAnimationClip);
        }
        else
        {
            ChangeSwordIdle(SwordIdleAnimationName, playerStateMachine.IdleLoopAnimationClip);
        }
    }
}
