using System.Collections;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

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
        isAlreadyActive = true;
        _playerStateMachine.SwitchState(new PlayerActiveCheckPointState(_playerStateMachine));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            _inputReader.ActiveCheckPointAction += ActiveCheckPointUI;
            GameManagers.Instance.SetCheckpoint(checkpointID, transform.position);
            GameManagers.Instance.AutoSave();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            isAlreadyActive = false;
            _inputReader.ActiveCheckPointAction -= ActiveCheckPointUI;
            GameManagers.Instance.AutoSave();
        }
    }
}