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
    private Health _health;
    private Mana _mana;
    private Stamina _stamina;
    private bool _isPrevHealthChanged;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _mana = GetComponent<Mana>();
        _stamina = GetComponent<Stamina>();
    }
    
    private void Start()
    {
        SetupHealthSlider((float)_health.currentHealth / _health.maxHealth);
        SetupManaSlider((float)_mana.currentMana / _mana.maxMana);
        SetupStaminaSlider((float)_stamina.currentStamina / _stamina.maxStamina);
    }

    private void OnEnable()
    {
        _health.OnChangeHealth += UpdateHealthSlider;
        _mana.OnChangeMana += UpdateManaSlider;
        _stamina.OnChangeStamina += UpdateStaminaSlider;
        _stamina.OnRecoveryStamina += UpdateRecoveryStaminaSlider;
    }
    
    private void OnDisable()
    {
        _health.OnChangeHealth -= UpdateHealthSlider;
        _mana.OnChangeMana -= UpdateManaSlider;
        _stamina.OnChangeStamina -= UpdateStaminaSlider;
        _stamina.OnRecoveryStamina -= UpdateRecoveryStaminaSlider;
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
        StartCoroutine(SmoothTransition(healthPrevSlider, value, _health.reduceHealPrevDuration));
        StartCoroutine(SmoothTransition(healthFollowingSlider, value, _health.reduceHealthFollowingDuration));
    }
    
    private void UpdateManaSlider(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(manaSlider, value, _mana.reduceManaDuration));
    }
    
    private void UpdateStaminaSlider(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(staminaSlider, value, _stamina.reduceStaminaDuration));
    }
    
    private void UpdateRecoveryStaminaSlider(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(staminaSlider, value, _stamina.recoveryStaminaDuration));
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
