using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class PlayerActiveCheckPointState : PlayerBaseState
{
    private readonly int _interactAnimation = Animator.StringToHash("Interact");
    private const string InteractAnimationTag = "Interact";

    private float _previousTime;

    private bool isActive;
    
    public PlayerActiveCheckPointState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.InputReader.ActiveCheckPointAction += ExitSystemUI;
        playerStateMachine.Animator.CrossFadeInFixedTime(_interactAnimation, playerStateMachine.AnimationCrossFade);
        playerStateMachine.InputReader.SetCursor(false);
    }

    public override void Tick(float deltaTime)
    {
        if (isActive) return;

        var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, InteractAnimationTag, 0);
        if (normalizeTime > _previousTime && normalizeTime >= .9f)
        {
            WorldUIManager.instance.ActiveSystemUI();
            isActive = true;
        }

        _previousTime = normalizeTime;
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.ActiveCheckPointAction -= ExitSystemUI;
        playerStateMachine.InputReader.SetCursor(true);
        WorldUIManager.instance.ActiveSystemUI();
        playerStateMachine.CheckPoint.isAlreadyActive = false;
    }

    private void ExitSystemUI()
    {
        if (isActive)
        {
            playerStateMachine.ReturnLocomotion();
        }
    }
}