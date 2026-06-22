using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [field: SerializeField] public float maxPotion;
    [field: SerializeField] public float potionUsage { get; set; }
    [field: SerializeField] public float reduceDuration { get; set; }
    public float currentPotion { get; set; }
    public event Action<float> OnChangePotion = delegate { };
    private void Awake()
    {
        currentPotion = maxPotion;
    }
    
    public void ChangePotion(float value)
    {
        currentPotion = Mathf.Max(currentPotion - value, 0f);
        OnChangePotion?.Invoke(currentPotion / maxPotion);
    }
}
