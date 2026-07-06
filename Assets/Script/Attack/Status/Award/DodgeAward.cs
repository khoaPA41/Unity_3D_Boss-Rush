using System;
using System.Collections;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class DodgeAward : MonoBehaviour
{
    [field:SerializeField] public bool IsRecoveryStamina { get; set; }
    [field:SerializeField] public bool IsCounterAttack { get; set; }
    [field:SerializeField] public bool IsMovementPush { get; set; }

    private PlayerStateMachine _playerStateMachine;
    private Stamina _stamina;
    
    public event Action<int> UpdateDodgeAwardUIAction;
    
    private void Start()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _stamina = GetComponent<Stamina>();
    }
    
    private void SetFalseAllAward()
    {
        IsRecoveryStamina = false;
        IsCounterAttack =  false;
        IsMovementPush =  false;
    }

    public void SetDodgeAward(int awardNumber)
    {
        SetFalseAllAward();
        switch (awardNumber)
        {
            case 1:
                IsMovementPush = true;
                UpdateDodgeAwardUIAction?.Invoke(1);
                break;
            case 2:
                IsRecoveryStamina = true;
                UpdateDodgeAwardUIAction?.Invoke(2);
                break;
            case 3:
                IsCounterAttack = true;
                UpdateDodgeAwardUIAction?.Invoke(3);
                break;
        }
    }

    public void DodgeAwardActive()
    {
        RecoveryStamina();
        MovementPush();
        Counterattack();
    }
    
    private void RecoveryStamina()
    {
        if (!IsRecoveryStamina) return;
        _stamina.DodgeAwardStamina();
    }

    private void MovementPush()
    {
        if (!IsMovementPush) return;

        StartCoroutine(RecoveryStaminaRoutine(1.5f,
            () => _playerStateMachine.ForceReceiver.SetCoefficientOfMovement(3f),
            () => _playerStateMachine.ForceReceiver.SetCoefficientOfMovement(1)
        ));
    }
    
    private void Counterattack()
    {
        if (!IsCounterAttack) return;

        _playerStateMachine.IsCounterAttack = true;
    }
    
    private IEnumerator RecoveryStaminaRoutine(float time, Action callback1, Action callback2)
    {
        callback1?.Invoke();
        yield return new WaitForSecondsRealtime(time);
        callback2?.Invoke();
    }

    // This method will check exists dodge award and update UI
    public void CallUIEvent()
    {
        var dodgeAwardValue = 0;
        
        if (IsMovementPush)
        {
            dodgeAwardValue = 1;
        }
        if ( IsRecoveryStamina)
        {
            dodgeAwardValue = 2;
        }
        if (IsCounterAttack)
        {
            dodgeAwardValue = 3;
        }
        UpdateDodgeAwardUIAction?.Invoke(dodgeAwardValue);
    }
}

