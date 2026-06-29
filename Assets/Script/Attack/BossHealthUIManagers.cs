using System;
using System.Collections;
using Script.Attack;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUIManagers : MonoBehaviour
{
    [Header("Status UI")] [SerializeField] 
    private Slider healthPrevSlider;
    [SerializeField] private Slider healthFollowingSlider;
    private Health _health;
    private Coroutine _healthChangeCoroutine;
    private Coroutine _healthFollowingChangeCoroutine;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.OnChangeHealth += UpdateHealthSlider;
    }

    private void OnDisable()
    {
        _health.OnChangeHealth -= UpdateHealthSlider;
    }

    private void SetupHealthSlider(float value)
    {
        healthPrevSlider.value = value;
        healthFollowingSlider.value = value;
    }
    
    private void UpdateHealthSlider(float value)
    {
        if (_healthChangeCoroutine != null) StopCoroutine(_healthChangeCoroutine);
        if (_healthFollowingChangeCoroutine != null) StopCoroutine(_healthFollowingChangeCoroutine);
        _healthChangeCoroutine =
            StartCoroutine(SmoothTransition(healthPrevSlider, value, _health.reduceHealPrevDuration));
        _healthFollowingChangeCoroutine =
            StartCoroutine(SmoothTransition(healthFollowingSlider, value, _health.reduceHealthFollowingDuration));
    }

    /*********************************************Transition Method*********************************************/
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
