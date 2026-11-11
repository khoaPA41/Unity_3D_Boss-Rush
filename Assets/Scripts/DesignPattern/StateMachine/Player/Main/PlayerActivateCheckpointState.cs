using DesignPattern.StateMachine.Player.Base;
using Managers;
using UI;
using UnityEngine;

namespace DesignPattern.StateMachine.Player.Main
{
    public class PlayerActivateCheckpointState : PlayerBaseState
    {
        private readonly int _interactAnimation = Animator.StringToHash("Interact");
        private const string InteractAnimationTag = "Interact";

        private float _previousTime;

        private bool _isActive;
        private bool _isCheckpoint;

        public PlayerActivateCheckpointState(PlayerStateMachine playerStateMachine, bool isCheckpoint) : base(playerStateMachine)
        {
            _isCheckpoint = isCheckpoint;
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
            playerStateMachine.ResetInvincible();
            GameManager.Instance.AutoSave();
        }

        public override void Tick(float deltaTime)
        {
            if (_isActive) return;

            if (!_isCheckpoint)
            {
                ActivateNonCheckpoint();
                return;
            }

            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, InteractAnimationTag, 0);
            if (normalizeTime > _previousTime && normalizeTime >= .9f)
            {
                ActivateOnCheckpoint();
            }
            _previousTime = normalizeTime;
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.ActiveCheckPointAction -= ExitSystemUI;
            playerStateMachine.InputReader.SettingsUIAction -= ExitSettingsUI;

            playerStateMachine.InputReader.SetCursor(true);
            playerStateMachine.CheckPoint.isAlreadyActive = false;
            GameManager.Instance.AutoSave();
            if (_isCheckpoint)
            {
                WorldUIManager.Instance.ShowSystemUI();
            }
        }

        private void ActivateOnCheckpoint()
        {
            WorldUIManager.Instance.ShowSystemUI();
            _isActive = true;
        }

        private void ActivateNonCheckpoint()
        {
            WorldUIManager.Instance.ShowSettingsUI();
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
                WorldUIManager.Instance.ShowSettingsUI();
                playerStateMachine.ReturnLocomotion();
            }
        }
    }
}