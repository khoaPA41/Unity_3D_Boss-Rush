using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");

    Vector2 dodgeDirection;

    float remainingTime;
    public PlayerDodgeState(PlayerStateMachine playerStateMachine, Vector2 dodgeDirection) : base(playerStateMachine)
    {
        this.dodgeDirection = dodgeDirection;
    }

    public override void Enter()
    {
        playerStateMachine.Animator.SetFloat(DodgeRightHash, dodgeDirection.x);
        playerStateMachine.Animator.SetFloat(DodgeForwardHash, dodgeDirection.y);
        playerStateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);
        remainingTime = playerStateMachine.DodgeDuration;
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        Move(movement, deltaTime);
        FaceTarget(deltaTime);

        remainingTime -= deltaTime;

        if (remainingTime <= 0f)
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

    Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += playerStateMachine.transform.right * dodgeDirection.x * playerStateMachine.DodgeLength / playerStateMachine.DodgeDuration;
        movement += playerStateMachine.transform.forward * dodgeDirection.y * playerStateMachine.DodgeLength / playerStateMachine.DodgeDuration;

        return movement;
    }
}
