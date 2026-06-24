using System;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [field: SerializeField] public float maxMana { get; set; }
    [field: SerializeField] public float reduceManaDuration { get; set; }
    public event Action<float> OnChangeMana = delegate { };
    public float currentMana;
    private PlayerStateMachine _playerStateMachine;

    private void Awake()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        currentMana = maxMana;
    }

    public void ChangeMana(int amount)
    {
        currentMana = Mathf.Max(currentMana - amount, 0);
        OnChangeMana?.Invoke(currentMana / maxMana);
    }

    public void RecoveryMana(float amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        OnChangeMana?.Invoke(currentMana / maxMana);
    }

    public void AddMana()
    {
        if (_playerStateMachine.isCanNotSubSpiritual || _playerStateMachine.PlayerSpiritualPower <= 0)
        {
            _playerStateMachine.isCanNotSubSpiritual = false;
            return;
        }
        maxMana += 1;
        currentMana = maxMana;
        OnChangeMana?.Invoke(currentMana / maxMana);
    }

    public void SubMana()
    {
        if (currentMana == 1000)
        {
            _playerStateMachine.isCanNotAddSpiritual = true;
            return;
        }
        maxMana -= 1;
        currentMana = maxMana;
        OnChangeMana?.Invoke(currentMana / maxMana);
    }
}
