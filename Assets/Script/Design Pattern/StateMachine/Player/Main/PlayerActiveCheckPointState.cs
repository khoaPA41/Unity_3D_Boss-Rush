using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class PlayerActiveCheckPointState : PlayerBaseState
{
    private readonly int _interactAnimation = Animator.StringToHash("Interact");
    private const string InteractAnimationTag = "Interact";

    private float _previousTime;

    private bool _isActive;
    private bool _isCheckpoint;
    
    public PlayerActiveCheckPointState(PlayerStateMachine playerStateMachine, bool isCheckpoint) : base(playerStateMachine)
    {
        _isCheckpoint =  isCheckpoint;
    }

    public override void Enter()
    {
        if (_isCheckpoint)
        {
            playerStateMachine.InputReader.ActiveCheckPointAction += ExitSystemUI;
        }
        else
        {
            playerStateMachine.InputReader.SettingsUIAction += ExitSettingsUI;
        }
        
        playerStateMachine.Animator.CrossFadeInFixedTime(_interactAnimation, playerStateMachine.AnimationCrossFade);
        playerStateMachine.InputReader.SetCursor(false);
        GameManagers.Instance.AutoSave();
    }

    public override void Tick(float deltaTime)
    {
        if (_isActive) return;
        
        if (!_isCheckpoint)
        {
            ActiveNonCheckpoint();
            return;
        }
        
        var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, InteractAnimationTag, 0);
        if (normalizeTime > _previousTime && normalizeTime >= .9f)
        {
                ActiveOnCheckpoint();
        }
        _previousTime = normalizeTime;
    }

    public override void PhysicTick(float fixedDeltaTime)
    {
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.ActiveCheckPointAction -= ExitSystemUI;
        playerStateMachine.InputReader.SettingsUIAction -= ExitSettingsUI;

        playerStateMachine.InputReader.SetCursor(true);
        playerStateMachine.CheckPoint.isAlreadyActive = false;
        GameManagers.Instance.AutoSave();
        if (_isCheckpoint)
        {
            WorldUIManager.instance.ActiveSystemUI();
        }
    }

    private void ActiveOnCheckpoint()
    {
        WorldUIManager.instance.ActiveSystemUI();
        _isActive = true;
    }
    
    private void ActiveNonCheckpoint()
    {
        WorldUIManager.instance.HandleActiveSettingsUI();
        _isActive = true;
    }

    private void ExitSystemUI()
    {
        if (_isActive)
        {
            playerStateMachine.ReturnLocomotion();
        }
    }
    
    private void ExitSettingsUI()
    {
        if (_isActive)
        {
            WorldUIManager.instance.HandleActiveSettingsUI();
            playerStateMachine.ReturnLocomotion();
        }
    }
}