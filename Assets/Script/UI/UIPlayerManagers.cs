using System;
using System.Collections;
using Script.Attack;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManagers : MonoBehaviour
{
    [Header("Status UI")] [SerializeField] private Slider healthPrevSlider;
    [SerializeField] private Slider healthFollowingSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider staminaSlider;

    [Header("Item UI")] [SerializeField] private Slider healthPotionSlider;
    [SerializeField] private Slider manaPotionSlider;

    [Header("Main Potion UI")] [SerializeField]
    private GameObject healthPotionUI;

    [SerializeField] private GameObject manaPotionUI;
    [SerializeField] private GameObject nextHealthPotionUI;
    [SerializeField] private GameObject nextManaPotionUI;

    [Header("Coroutines")] private Coroutine _healthChangeCoroutine;
    private Coroutine _healthFollowingChangeCoroutine;
    private Coroutine _manaChangeCoroutine;
    private Coroutine _staminaChangeCoroutine;
    private Coroutine _staminaRecoveryCoroutine;
    private Coroutine _healthPotionChangeCoroutine;
    private Coroutine _manaPotionChangeCoroutine;


    [Header("Sub Potion")] [SerializeField]
    private GameObject subPotion;

    [Header("Image Of Sub Potion")] [SerializeField]
    private Image subPotionImage1;

    [SerializeField] private Image subPotionImage2;
    [SerializeField] private Image subPotionImage3;

    private InputReader _inputReader;
    private Health _health;
    private Mana _mana;
    private Stamina _stamina;
    private HealthPotion _healthPotion;
    private ManaPotion _manaPotion;
    private SubPotion _subPotion;
    // private bool _isPrevHealthChanged;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _health = GetComponent<Health>();
        _mana = GetComponent<Mana>();
        _stamina = GetComponent<Stamina>();
        _healthPotion = GetComponent<HealthPotion>();
        _manaPotion = GetComponent<ManaPotion>();
        _subPotion = GetComponent<SubPotion>();
    }

    private void Start()
    {
        SetupHealthSlider(_health.currentHealth / _health.maxHealth);
        SetupManaSlider(_mana.currentMana / _mana.maxMana);
        SetupStaminaSlider(_stamina.currentStamina / _stamina.maxStamina);
        SetupHealthPotionSlider(_healthPotion.currentPotion / _healthPotion.maxPotion);
        SetupManaPotionSlider(_manaPotion.currentPotion / _manaPotion.maxPotion);
    }

    private void OnEnable()
    {
        _health.OnChangeHealth += UpdateHealthSlider;
        _mana.OnChangeMana += UpdateManaSlider;
        _stamina.OnChangeStamina += UpdateStaminaSlider;
        _stamina.OnRecoveryStamina += UpdateRecoveryStaminaSlider;
        _healthPotion.OnChangePotion += UpdateHealthPotionSlider;
        _manaPotion.OnChangePotion += UpdateManaPotionSlider;
        _inputReader.ChangeHealthPotionAction += UpdateMainHealthPotion;
        _inputReader.ChangeManaPotionAction += UpdateMainManaPotion;
        _inputReader.ChangeNextSubPotionAction += NextSubPotionAnimation;
        _inputReader.ChangePrevSubPotionAction += PrevSubPotionAnimation;
    }

    private void OnDisable()
    {
        _health.OnChangeHealth -= UpdateHealthSlider;
        _mana.OnChangeMana -= UpdateManaSlider;
        _stamina.OnChangeStamina -= UpdateStaminaSlider;
        _stamina.OnRecoveryStamina -= UpdateRecoveryStaminaSlider;
        _healthPotion.OnChangePotion -= UpdateHealthPotionSlider;
        _manaPotion.OnChangePotion -= UpdateManaPotionSlider;
        _inputReader.ChangeHealthPotionAction -= UpdateMainHealthPotion;
        _inputReader.ChangeManaPotionAction -= UpdateMainManaPotion;
        _inputReader.ChangeNextSubPotionAction -= NextSubPotionAnimation;
        _inputReader.ChangePrevSubPotionAction -= PrevSubPotionAnimation;
    }

    /********************************************Setup*********************************************/

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

    private void SetupHealthPotionSlider(float value)
    {
        healthPotionSlider.value = value;
    }

    private void SetupManaPotionSlider(float value)
    {
        manaPotionSlider.value = value;
    }

    /*********************************************Update*********************************************/
    private void UpdateHealthSlider(float value)
    {
        if (_healthChangeCoroutine != null) StopCoroutine(_healthChangeCoroutine);
        if (_healthFollowingChangeCoroutine != null) StopCoroutine(_healthFollowingChangeCoroutine);
        _healthChangeCoroutine =
            StartCoroutine(SmoothTransition(healthPrevSlider, value, _health.reduceHealPrevDuration));
        _healthFollowingChangeCoroutine =
            StartCoroutine(SmoothTransition(healthFollowingSlider, value, _health.reduceHealthFollowingDuration));
    }

    private void UpdateManaSlider(float value)
    {
        if (_manaChangeCoroutine != null) StopCoroutine(_manaChangeCoroutine);
        _manaChangeCoroutine = StartCoroutine(SmoothTransition(manaSlider, value, _mana.reduceManaDuration));
    }

    private void UpdateStaminaSlider(float value)
    {
        if (_staminaChangeCoroutine != null) StopCoroutine(_staminaChangeCoroutine);
        _staminaChangeCoroutine =
            StartCoroutine(SmoothTransition(staminaSlider, value, _stamina.reduceStaminaDuration));
    }

    private void UpdateRecoveryStaminaSlider(float value)
    {
        if (_staminaRecoveryCoroutine != null) StopCoroutine(_staminaRecoveryCoroutine);
        _staminaRecoveryCoroutine =
            StartCoroutine(SmoothTransition(staminaSlider, value, _stamina.recoveryStaminaDuration));
    }

    private void UpdateHealthPotionSlider(float value)
    {
        if (_healthPotionChangeCoroutine != null) StopCoroutine(_healthPotionChangeCoroutine);
        _healthPotionChangeCoroutine =
            StartCoroutine(SmoothTransition(healthPotionSlider, value, _healthPotion.reduceDuration));
    }

    private void UpdateManaPotionSlider(float value)
    {
        if (_manaPotionChangeCoroutine != null) StopCoroutine(_manaPotionChangeCoroutine);
        _manaPotionChangeCoroutine =
            StartCoroutine(SmoothTransition(manaPotionSlider, value, _manaPotion.reduceDuration));
    }

    /*********************************************Update Item*********************************************/
    private void UpdateMainHealthPotion()
    {
        healthPotionUI.SetActive(true);
        manaPotionUI.SetActive(false);
        nextManaPotionUI.SetActive(true);
        nextHealthPotionUI.SetActive(false);
    }

    private void UpdateMainManaPotion()
    {
        manaPotionUI.SetActive(true);
        healthPotionUI.SetActive(false);
        nextHealthPotionUI.SetActive(true);
        nextManaPotionUI.SetActive(false);
    }

    private void NextSubPotionAnimation()
    {
        StartCoroutine(WaitToChangeSprite(.4f,
            () => subPotion.GetComponent<Animator>().Play("Next"),
            UpdateNextSubPotion
        ));
    }

    private void PrevSubPotionAnimation()
    {
        StartCoroutine(WaitToChangeSprite(.4f,
            () => subPotion.GetComponent<Animator>().Play("Prev"),
            UpdatePrevSubPotion
        ));
    }

    private IEnumerator WaitToChangeSprite(float time, Action action1, Action action2)
    {
        action1?.Invoke();
        yield return new WaitForSeconds(time);
        action2?.Invoke();
    }

    private void UpdateNextSubPotion()
    {
        var tempSprite = subPotionImage1.sprite;
        subPotionImage1.sprite = subPotionImage2.sprite;
        subPotionImage2.sprite = subPotionImage3.sprite;
        subPotionImage3.sprite = tempSprite;
    }

    private void UpdatePrevSubPotion()
    {
        var tempSprite = subPotionImage3.sprite;
        subPotionImage3.sprite = subPotionImage2.sprite;
        subPotionImage2.sprite = subPotionImage1.sprite;
        subPotionImage1.sprite = tempSprite;
        subPotionImage1.sprite = tempSprite;
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