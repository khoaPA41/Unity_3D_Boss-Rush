using System;
using Script.Design_Pattern.StateMachine.Player.Base;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private InputReader _inputReader;
    private UIPlayerManagers _uiPlayerManagers;
    private PlayerStateMachine _playerStateMachine;
    private void Start()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _uiPlayerManagers = GetComponent<UIPlayerManagers>();
        _inputReader = GetComponent<InputReader>();
    }

    private void ActiveCheckPointUI()
    {
        WorldUIManager.instance.ActiveSystemUI();
        _inputReader.SetCursor(!_uiPlayerManagers.systemUI.activeInHierarchy);

        if (_uiPlayerManagers.systemUI.activeInHierarchy)
        {
            _playerStateMachine.SwitchState(new PlayerEmptyState(_playerStateMachine));
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            _inputReader.ActiveCheckPointAction += ActiveCheckPointUI;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            _inputReader.ActiveCheckPointAction -= ActiveCheckPointUI;
        }
    }
}