using Design_Pattern.StateMachine.Player;
using Manager;
using UnityEngine;

namespace Interact
{
    [RequireComponent(typeof(InputReader))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class CheckPoint : MonoBehaviour
    {
        public string checkpointID;
        private InputReader _inputReader;
        private PlayerStateMachine _playerStateMachine;

        public bool isAlreadyActive = false;
        public bool CanInteractCheckPoint { get; private set; }

        private void Start()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _inputReader = GetComponent<InputReader>();
        }

        private void ActiveCheckPointUI()
        {
            if (isAlreadyActive) return;
            ResetStatus();
            isAlreadyActive = true;
            _playerStateMachine.SwitchState(new PlayerActiveCheckPointState(_playerStateMachine, true));
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
                other.GetComponent<ActiveCheckPoint>().SubcribeEvent();
                _inputReader.ActiveCheckPointAction += ActiveCheckPointUI;
                GameManagers.Instance.SetCheckpoint(checkpointID, transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("CheckPoint"))
            {
                other.GetComponent<ActiveCheckPoint>().UnsubcribeEvent();
                isAlreadyActive = false;
                _inputReader.ActiveCheckPointAction -= ActiveCheckPointUI;
            }
        }
    }
}