using UnityEngine;

public class FreeLookState : PlayerBaseState
{
    readonly int freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    Vector3 movement;
    float countTimeToChangeIdleLoop = 0;
    public FreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {

        //countTimeToChangeIdleLoop = playerStateMachine.TimeToBackIdleLoop;
        playerStateMachine.InputReader.TargetAction += EnterTargetState;
        playerStateMachine.InputReader.JumpAction += EnterJumpState;
        playerStateMachine.InputReader.DodgeAction += EnterDodgeState;


        playerStateMachine.Animator.CrossFadeInFixedTime(freeLookBlendTreeHash, playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        movement = CalculateMovementInFreeLook();
        float speed = playerStateMachine.InputReader.IsSprint ? playerStateMachine.FreeLookMovementSprintSpeed : playerStateMachine.FreeLookMovementSpeed;

        if (playerStateMachine.InputReader.IsAttack)
        {
            if (playerStateMachine.isAttackState)
            {
                playerStateMachine.EnterAttackState(0);
            }
            else
            {
                playerStateMachine.EnterChangeAction(true);

            }
        }

        //countTimeToChangeIdleLoop -= deltaTime;
        //if (countTimeToChangeIdleLoop <= 0f)
        //{
        //    playerStateMachine.EnterChangeAction();
        //}

        Move(movement * speed, deltaTime);
        UpdateAnimation(deltaTime);
        FaceDir(movement, deltaTime);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.TargetAction -= EnterTargetState;
        playerStateMachine.InputReader.JumpAction -= EnterJumpState;
        playerStateMachine.InputReader.DodgeAction -= EnterDodgeState;
    }

    void UpdateAnimation(float deltaTime)
    {
        if (playerStateMachine.InputReader.InputMovement == Vector2.zero)
        {
            playerStateMachine.Animator.SetFloat("Movement", 0f, playerStateMachine.AnimationCrossFade, deltaTime);
            return;

        }

        if (playerStateMachine.InputReader.IsSprint)
        {
            playerStateMachine.Animator.SetFloat("Movement", 1f, playerStateMachine.AnimationCrossFade, deltaTime);
            return;
        }

        playerStateMachine.Animator.SetFloat("Movement", .5f, playerStateMachine.AnimationCrossFade, deltaTime);
    }

    void EnterTargetState()
    {
        if (!playerStateMachine.Targeter.SelectedTarget()) { return; }
        playerStateMachine.SwitchState(new PlayerTargetState(playerStateMachine));
        return;
    }

    void EnterDodgeState()
    {
        playerStateMachine.SwitchState(new PlayerDodgeState(playerStateMachine, playerStateMachine.InputReader.InputMovement));
    }
}
