using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    readonly int targetLookBlendTreeHash = Animator.StringToHash("TargetLookBlendTree");
    readonly int MovementXParam = Animator.StringToHash("MovementX");
    readonly int MovementYParam = Animator.StringToHash("MovementY");

    public PlayerTargetState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.InputReader.TargetAction += OutTargetState;
        playerStateMachine.Animator.CrossFadeInFixedTime(targetLookBlendTreeHash, playerStateMachine.AnimationCrossFade);
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

    }

    void OutTargetState()
    {
        playerStateMachine.Targeter.CancelTarget();
        playerStateMachine.SwitchState(new FreeLookState(playerStateMachine));
        return;
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
