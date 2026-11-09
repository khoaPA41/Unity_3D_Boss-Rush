using Environment;
using DesignPattern.StateMachine.Player.Main;
using DesignPattern.StateMachine.Player.Base;
using Input;
using Managers;
using UnityEngine;

namespace Interact
{
    [RequireComponent(typeof(InputReader))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class CheckPoint : MonoBehaviour
    {
        private InputReader _inputReader;
        private PlayerStateMachine _playerStateMachine;

        public bool isAlreadyActive = false;

        private void Start()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _inputReader = GetComponent<InputReader>();
        }

        private void InteractCheckPoint()
        {
            if (isAlreadyActive) return;
            ResetStatus();
            isAlreadyActive = true;
            _playerStateMachine.SwitchState(new PlayerActivateCheckpointState(_playerStateMachine, true));
        }

        private void ResetStatus()
        {
            _playerStateMachine.Health.Reset();
            _playerStateMachine.Mana.Reset();
            _playerStateMachine.Stamina.Reset();
            _playerStateMachine.HealthPotion.Reset();
            _playerStateMachine.ManaPotion.Reset();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CheckPoint"))
            {
                var checkPoint = other.GetComponent<ActiveCheckPoint>();
                checkPoint.SubscribeEvent();
                _inputReader.ActiveCheckPointAction += InteractCheckPoint;
                GameManager.Instance.SetCheckpoint(checkPoint.checkpointID, transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("CheckPoint"))
            {
                other.GetComponent<ActiveCheckPoint>().UnsubscribeEvent();
                isAlreadyActive = false;
                _inputReader.ActiveCheckPointAction -= InteractCheckPoint;
            }
        }
    }
}