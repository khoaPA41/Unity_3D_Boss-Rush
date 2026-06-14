using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [field: SerializeField] public int maxStamina { get; set; }
    [field: SerializeField] public float recoveryStaminaDuration { get; set; }
    [field: SerializeField] public float reduceStaminaDuration { get; set; }
    [field: SerializeField] public int dodgeReduce { get; set; }
    [field: SerializeField] public int movementReduce { get; set; }
    [field: SerializeField] public int lightAttackReduce { get; set; }
    [field: SerializeField] public int heavyAttackReduce { get; set; }
    [field: SerializeField] public int jumpReduce { get; set; }

    public float currentStamina  { get; set; }
    
    public event Action<float> OnChangeStamina = delegate { };
    public event Action<float> OnRecoveryStamina = delegate { };

    
    private void Start()
    {
        currentStamina = maxStamina;
    }
    
    public void ChangeStamina(int amount)
    {
        currentStamina = Mathf.Max(currentStamina - amount, 0);
        OnChangeStamina?.Invoke((float)currentStamina / maxStamina);
    }

    public void RecoveryStamina()
    {
        currentStamina = maxStamina;
        OnRecoveryStamina?.Invoke((float)currentStamina / maxStamina);
    }
}
