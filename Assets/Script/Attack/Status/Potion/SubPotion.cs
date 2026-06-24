using System;
using System.Collections;
using System.Collections.Generic;
using Script.Attack;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;


[Serializable]
public class SubPotionValue
{
    public string potionName;
    public int quantity;
    public float timeToDone;
    public string hexColor;
}


public class SubPotion : MonoBehaviour
{
    [SerializeField] private List<SubPotionValue> subPotionList;
    private Health health;
    private Stamina stamina;

    public SubPotionValue currentPotion { get; set; }
    private int currentIndex = 1;
    
    private InputReader _inputReader;
    private PlayerStateMachine _playerStateMachine;
    
    private void Awake()
    {
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _inputReader = GetComponent<InputReader>();
        currentPotion = subPotionList[currentIndex];
    }

    private void OnEnable()
    {
        _inputReader.ChangeNextSubPotionAction += ChangeNextSubPotion;
        _inputReader.ChangePrevSubPotionAction += ChangePreviousSubPotion;
    }

    private void OnDisable()
    {
        _inputReader.ChangeNextSubPotionAction -= ChangeNextSubPotion;
        _inputReader.ChangePrevSubPotionAction -= ChangePreviousSubPotion;
    }
    
    private void ChangeNextSubPotion()
    {
        currentIndex += 1;
        if(currentIndex >= subPotionList.Count) currentIndex = 0;
        currentPotion = subPotionList[currentIndex];
    }

    private void ChangePreviousSubPotion()
    {
        currentIndex -= 1;
        if(currentIndex < 0) currentIndex = subPotionList.Count - 1;
        currentPotion = subPotionList[currentIndex];
    }

    public void SubtractPotion(string potionName, int amount)
    {
        var targetPotion = subPotionList.Find(potion => potion.potionName == potionName);
        targetPotion.quantity = Mathf.Max(targetPotion.quantity - amount, 0);
    }

    private SubPotionValue FindPotion(string potionName)
    {
        return subPotionList.Find(potion => potion.potionName == potionName);
    }

    public void ReduceDame(string potionName)
    {
        StartCoroutine(SubPotionCoroutine(FindPotion(potionName).timeToDone,
            () => health.isReduceDame = true,
            () => health.isReduceDame = false
        ));
    }
    
    public void ReduceStamina(string potionName)
    {
        StartCoroutine(SubPotionCoroutine(FindPotion(potionName).timeToDone,
            () => stamina.isReduceStamina = true,
            () => stamina.isReduceStamina = false
        ));
    }
    
    public void IncreaseDame(string potionName)
    {
        StartCoroutine(SubPotionCoroutine(FindPotion(potionName).timeToDone,
            () => _playerStateMachine.IsIncreaseDamePotion = true,
            () => _playerStateMachine.IsIncreaseDamePotion = false
        ));
    }

    private IEnumerator SubPotionCoroutine(float timeToDone, Action callback1, Action callback2)
    {
        callback1?.Invoke();
        yield return new WaitForSecondsRealtime(timeToDone);
        callback2?.Invoke();
    }

    public void AddSubPotionQuantity(string potionName)
    {
        var targetPotion = FindPotion(potionName);
        if (_playerStateMachine.isCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
        { 
            _playerStateMachine.isCanNotSubSpiritual = false;
            return;
        }
        targetPotion.quantity++;
    }
    
    public void SubtractSubPotionQuantity(string potionName)
    {
        var targetPotion = FindPotion(potionName);

        if (targetPotion.quantity == 0)
        {
            _playerStateMachine.isCanNotAddSpiritual = true;
            return;
        }

        targetPotion.quantity--;
    }
}