using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [field: SerializeField] public float maxStamina { get; set; }
    [field: SerializeField] public float recoveryStaminaDuration { get; set; }
    [field: SerializeField] public float reduceStaminaDuration { get; set; }
    [field: SerializeField] public float dodgeReduce { get; set; }
    [field: SerializeField] public float movementReduce { get; set; }
    [field: SerializeField] public float lightAttackReduce { get; set; }
    [field: SerializeField] public float heavyAttackReduce { get; set; }
    [field: SerializeField] public float jumpReduce { get; set; }
    [SerializeField] private float reduceStamina;
    public float currentStamina;
    
    public event Action<float> OnChangeStamina = delegate { };
    public event Action<float> OnRecoveryStamina = delegate { };

    public bool isReduceStamina { get; set; }
    private void Awake()
    {
        currentStamina = maxStamina;
    }
    
    public void ChangeStamina(float amount)
    {
        if (isReduceStamina) amount /= reduceStamina;
        currentStamina = Mathf.Max(currentStamina - amount, 0);
        OnChangeStamina?.Invoke((float)currentStamina / maxStamina);
    }

    public void RecoveryStamina()
    {
        currentStamina = maxStamina;
        OnRecoveryStamina?.Invoke(currentStamina / maxStamina);
    }

    public void DodgeAwardStamina()
    {
        var staminaLost = maxStamina - currentStamina;
        currentStamina += staminaLost * .5f;
        OnRecoveryStamina?.Invoke(currentStamina / maxStamina);
    }
}
