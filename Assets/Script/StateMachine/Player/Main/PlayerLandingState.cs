using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    readonly int LandingAnimationHash = Animator.StringToHash("Landing");
    readonly string LandingAnimationTag = "Landing";

    Vector3 momentum;

    public PlayerLandingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(LandingAnimationHash, playerStateMachine.AnimationCrossFade);
        momentum = playerStateMachine.CharacterController.velocity;
        momentum.y = 0;
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, LandingAnimationTag);
        if (normalizeTime > .9f && normalizeTime <= 1f)
        {
            playerStateMachine.ReturnLocomotion();
        }
        Move(deltaTime);
        FaceTarget(deltaTime);
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {

    }
}
