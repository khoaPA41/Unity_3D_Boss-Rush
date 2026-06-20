using System;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [field: SerializeField] public float maxMana { get; set; }
    [field: SerializeField] public float reduceManaDuration { get; set; }
    public event Action<float> OnChangeMana = delegate { };
    public float currentMana;
    
    private void Awake()
    {
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
}
