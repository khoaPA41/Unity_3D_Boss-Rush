using System;
using System.Collections;
using Script.Attack;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManagers : MonoBehaviour
{
    [SerializeField] private Slider healthPrevSlider;
    [SerializeField] private Slider healthFollowingSlider;

    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private float transitionDuration = 0.5f;
    private Health health;
    private Mana mana;
    private Stamina stamina;
    bool isPrevHealthChanged;

    private void Awake()
    {
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        stamina = GetComponent<Stamina>();
    }
    
    private void Start()
    {
        SetupHealthSlider(health.currentHealth / health.maxHealth);
        SetupManaSlider(mana.currentMana / mana.maxMana);
        SetupStaminaSlider(stamina.currentStamina / stamina.maxStamina);
    }

    private void OnEnable()
    {
        health.OnChangeHealth += UpdateHealthSlider;
        mana.OnChangeMana += UpdateManaSlider;
        stamina.OnChangeStamina += UpdateStaminaSlider;
        stamina.OnRecoveryStamina += UpdateRecoveryStaminaSlider;
    }
    
    private void OnDisable()
    {
        health.OnChangeHealth -= UpdateHealthSlider;
        mana.OnChangeMana -= UpdateManaSlider;
        stamina.OnChangeStamina -= UpdateStaminaSlider;
        stamina.OnRecoveryStamina -= UpdateRecoveryStaminaSlider;
    }
    
    private void SetupHealthSlider(float value)
    {
        healthPrevSlider.value = value;
        healthFollowingSlider.value = value;
    }
    
    private void SetupManaSlider(float value)
    {
        manaSlider.value = value;
    }
    
    private void SetupStaminaSlider(float value)
    {
        staminaSlider.value = value;
    }
    
    private void UpdateHealthSlider(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(healthPrevSlider, value, health.reduceHealPrevDuration));
        StartCoroutine(SmoothTransition(healthFollowingSlider, value, health.reduceHealthFollowingDuration));
    }
    
    private void UpdateManaSlider(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(manaSlider, value, mana.reduceManaDuration));
    }
    
    private void UpdateStaminaSlider(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(staminaSlider, value, stamina.reduceStaminaDuration));
    }
    
    private void UpdateRecoveryStaminaSlider(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(staminaSlider, value, stamina.recoveryStaminaDuration));
    }
    
    private IEnumerator SmoothTransition(Slider slider, float value, float transitionDuration)
    {
        var elapsedTime = 0f;
        var startValue = slider.value;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            var t = elapsedTime / transitionDuration;

            slider.value = Mathf.Lerp(startValue, value, t);
            yield return null;
        }
        slider.value = value;
    }
}
