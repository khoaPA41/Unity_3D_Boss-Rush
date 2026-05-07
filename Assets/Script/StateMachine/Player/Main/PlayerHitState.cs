using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    readonly int HitAnimationHash = Animator.StringToHash("Hit");
    readonly int HitKnockbackAnimationHash = Animator.StringToHash("Hit_Knockback");

    readonly string HitAnimationTag = "Hit";

    bool isKnockBack = false;
    public PlayerHitState(PlayerStateMachine playerStateMachine, bool isKnockBack) : base(playerStateMachine)
    {
        this.isKnockBack = isKnockBack;
    }

    public override void Enter()
    {
        if (isKnockBack)
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(HitKnockbackAnimationHash, playerStateMachine.AnimationCrossFade);
        }
        else
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(HitAnimationHash, playerStateMachine.AnimationCrossFade);
        }

    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, HitAnimationTag);

        if (normalizeTime > .8f && normalizeTime <= 1f)
        {
            playerStateMachine.ReturnLocomotion();
        }
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }
}
