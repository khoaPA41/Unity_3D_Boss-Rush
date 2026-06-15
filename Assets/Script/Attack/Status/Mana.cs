using System;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [field: SerializeField] public int maxMana { get; set; }
    [field: SerializeField] public float reduceManaDuration { get; set; }
    public event Action<float> OnChangeMana = delegate { };
    public int currentMana;
    
    private void Awake()
    {
        currentMana = maxMana;
    }

    public void ChangeMana(int amount)
    {
        currentMana = Mathf.Max(currentMana - amount, 0);
        OnChangeMana?.Invoke((float)currentMana / maxMana);
    }
}
