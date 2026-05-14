using UnityEngine;

public class PlayerChangeAction : PlayerBaseState
{
    readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
    readonly int freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    readonly int MovementXParam = Animator.StringToHash("MovementX");
    readonly int MovementYParam = Animator.StringToHash("MovementY");

    readonly int SwordEnterAnimationHash = Animator.StringToHash("Sword_Enter");
    readonly int SwordExitAnimationHash = Animator.StringToHash("Sword_Exit");

    readonly string SwordChangeTag = "SwordChange";
    readonly string IdleAnimationName = "Idle_Loop";
    readonly string SwordIdleAnimationName = "Sword_Idle";
    bool isSwordEnter;
    Vector3 movement;
    public PlayerChangeAction(PlayerStateMachine playerStateMachine, bool isSwordEnter) : base(playerStateMachine)
    {
        this.isSwordEnter = isSwordEnter;
    }

    public override void Enter()
    {
        if (playerStateMachine.Targeter.currentTarget != null)
        {

            playerStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);
        }
        else
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(freeLookBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);

        }

        if (isSwordEnter)
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(SwordEnterAnimationHash, playerStateMachine.AnimationCrossFade, 1);
            playerStateMachine.isAttackState = true;
        }
        else
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(SwordExitAnimationHash, playerStateMachine.AnimationCrossFade, 1);
            playerStateMachine.isAttackState = false;
        }
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, SwordChangeTag, 1);

        if (normalizeTime > .9f && normalizeTime <= 1f)
        {
            playerStateMachine.ReturnLocomotion();
        }

        if (playerStateMachine.Targeter.currentTarget != null)
        {
            movement = CalculateMovementInTarget();
            FaceTarget(deltaTime);
        }
        else
        {
            movement = CalculateMovementInFreeLook();
            FaceDir(movement, deltaTime);
        }

        UpdateAnimation(deltaTime);
        Move(movement * playerStateMachine.FreeLookMovementSpeed, deltaTime);
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

    void UpdateAnimation(float deltaTime)
    {
        float dirX = 0f;
        float dirY = 0f;
        if (playerStateMachine.InputReader.InputMovement.x != 0)
        {
            dirX = Mathf.Sign(playerStateMachine.InputReader.InputMovement.x);

        }
        if (playerStateMachine.InputReader.InputMovement.y != 0)
        {
            dirY = Mathf.Sign(playerStateMachine.InputReader.InputMovement.y);
        }

        playerStateMachine.Animator.SetFloat(MovementXParam, dirX, playerStateMachine.AnimationCrossFade, deltaTime);
        playerStateMachine.Animator.SetFloat(MovementYParam, dirY, playerStateMachine.AnimationCrossFade, deltaTime);
    }
}
