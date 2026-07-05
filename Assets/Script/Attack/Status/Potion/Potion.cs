using System;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [field: SerializeField] public float maxPotion;
    [field: SerializeField] public float potionUsage { get; set; }
    [field: SerializeField] public float reduceDuration { get; set; }
    [field: SerializeField] public float maxPotionAdded { get; set; }
    [field: SerializeField] public float usageAdded { get; set; }

    public float CurrentPotion { get; private set; }
    public event Action<float> OnChangePotion = delegate { };
    public float PossibleUsage {get; private set;}
    
    private PlayerStateMachine _playerStateMachine;
    private float minimumPotion;
    private float minimumPotionUsage;
    
    private void Awake()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        minimumPotion = maxPotion;
        CurrentPotion = maxPotion;
        PossibleUsage =  potionUsage;
    }
    
    public void ChangePotion()
    {
        PossibleUsage = potionUsage < CurrentPotion ? potionUsage : CurrentPotion;
        CurrentPotion = Mathf.Max(CurrentPotion - PossibleUsage, 0f);
        OnChangePotion?.Invoke(CurrentPotion / maxPotion);
    }
    
    public void AddMaxPotion()
    {
        if (_playerStateMachine.isCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
        { 
            _playerStateMachine.isCanNotSubSpiritual = false;
            return;
        }
        maxPotion += maxPotionAdded;
        CurrentPotion = maxPotion;
        OnChangePotion?.Invoke(CurrentPotion / maxPotion);
    }
    
    public void SubtractMaxPotion()
    {
        if (maxPotion == minimumPotion)
        {
            _playerStateMachine.isCanNotAddSpiritual = true;
            return;
        }
        maxPotion -= maxPotionAdded;
        CurrentPotion = maxPotion;
        OnChangePotion?.Invoke(CurrentPotion / maxPotion);
    }
    
    public void AddUsagePotion()
    {
        if (_playerStateMachine.isCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
        { 
            _playerStateMachine.isCanNotSubSpiritual = false;
            return;
        }
        potionUsage += usageAdded;
    }
    
    public void SubtractUsagePotion()
    {
        if (potionUsage == minimumPotionUsage)
        {
            _playerStateMachine.isCanNotAddSpiritual = true;
            return;
        }
        potionUsage -= usageAdded;
    }

    public void Reset()
    {
        CurrentPotion = maxPotion;
        OnChangePotion?.Invoke(CurrentPotion / maxPotion);
    }
}
