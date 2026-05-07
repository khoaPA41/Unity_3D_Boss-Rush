using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    readonly int TargetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
    readonly int MovementXParam = Animator.StringToHash("MovementX");
    readonly int MovementYParam = Animator.StringToHash("MovementY");
    public PlayerTargetState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.InputReader.JumpAction += EnterJumpState;
        playerStateMachine.InputReader.TargetAction += OutTargetState;
        playerStateMachine.InputReader.DodgeAction += EnterDodgeState;
        playerStateMachine.Health.HitAction += playerStateMachine.EnterHitState;

        playerStateMachine.Animator.CrossFadeInFixedTime(TargetLookBlendTreeHash, playerStateMachine.AnimationCrossFade);

        if (!playerStateMachine.isAttackState)
        {
            playerStateMachine.EnterChangeAction(true);
        }
    }

    public override void Tick(float deltaTime)
    {
        if (playerStateMachine.Targeter.currentTarget == null)
        {
            OutTargetState();
        }

        if (playerStateMachine.InputReader.IsAttack)
        {
            playerStateMachine.EnterAttackState(0);
        }

        Vector3 movement = CalculateMovementInTarget();
        Move(movement * playerStateMachine.FreeLookMovementSpeed, deltaTime);
        UpdateAnimation(deltaTime);
        FaceTarget(deltaTime);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {
        playerStateMachine.InputReader.TargetAction -= OutTargetState;
        playerStateMachine.InputReader.JumpAction -= EnterJumpState;
        playerStateMachine.InputReader.DodgeAction -= EnterDodgeState;
        playerStateMachine.Health.HitAction -= playerStateMachine.EnterHitState;
    }

    void OutTargetState()
    {
        playerStateMachine.Targeter.CancelTarget();
        playerStateMachine.EnterChangeAction(false);
        return;
    }

    void EnterDodgeState()
    {
        playerStateMachine.SwitchState(new PlayerDodgeState(playerStateMachine, playerStateMachine.InputReader.InputMovement));
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
