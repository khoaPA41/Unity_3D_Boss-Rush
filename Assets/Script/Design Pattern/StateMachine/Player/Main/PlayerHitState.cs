using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    readonly int HitAnimationHash = Animator.StringToHash("Hit");
    readonly int HitKnockbackAnimationHash = Animator.StringToHash("Hit_Knockback");

    readonly string HitAnimationTag = "Hit";
    float previousTime;

    bool isKnockBack = false;

    bool alreadyApplyForce = false;
    float force;
    public PlayerHitState(PlayerStateMachine playerStateMachine, bool isKnockBack) : base(playerStateMachine)
    {
        this.isKnockBack = isKnockBack;
    }

    public override void Enter()
    {
        if (isKnockBack)
        {
            force = playerStateMachine.HitKnockback;
            playerStateMachine.Animator.CrossFadeInFixedTime(HitKnockbackAnimationHash, playerStateMachine.AnimationCrossFade);
        }
        else
        {
            force = playerStateMachine.HitForce;
            playerStateMachine.Animator.CrossFadeInFixedTime(HitAnimationHash, playerStateMachine.AnimationCrossFade);
        }
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, HitAnimationTag, 0);

        if (normalizeTime >= previousTime && normalizeTime <= 1f)
        {
            if (normalizeTime >= playerStateMachine.HitForceTime)
            {
                TryApplyForce(force);
            }
        }
        else
        {
            playerStateMachine.ReturnLocomotion();
        }

        previousTime = normalizeTime;
        Move(deltaTime);
        FaceTarget(deltaTime);

    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }

    void TryApplyForce(float force)
    {
        if (alreadyApplyForce) { return; }
        playerStateMachine.ForceReceiver.AddForce(-playerStateMachine.transform.forward * force);
        alreadyApplyForce = true;
    }
}
