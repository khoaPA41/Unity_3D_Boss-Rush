using System;
using System.Collections;
using System.Collections.Generic;
using Script.Attack;
using Script.Design_Pattern.StateMachine.Player.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI
{
    public TextMeshProUGUI text;
    public string name;
}

public class UIPlayerManagers : MonoBehaviour
{
    [Header("Status UI")] [SerializeField] 
    private Slider healthPrevSlider;
    [SerializeField] private Slider healthFollowingSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Slider staminaSlider;

    [Header("Item UI")] [SerializeField] 
    private Slider healthPotionSlider;
    [SerializeField] private Slider manaPotionSlider;

    [Header("Main Potion UI")] [SerializeField]
    private GameObject healthPotionUI;
    [SerializeField] private GameObject manaPotionUI;
    [SerializeField] private GameObject nextHealthPotionUI;
    [SerializeField] private GameObject nextManaPotionUI;

    [Header("Coroutines")] 
    private Coroutine _healthChangeCoroutine;
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

    [Header("Skill UI")] [SerializeField] 
    private Image icon_ChangingTheGame;
    [SerializeField] private Image icon_Escape;
    [SerializeField] private Image icon_Response;

    [Header("System UI")] [field:SerializeField] 
    public GameObject systemUI;
    [SerializeField] private List<Image> skillSystemList;
    
    [Header("Status UI")] 
    [SerializeField] private List<TextMeshProUGUI> statusTextList;

    [Header("Dodge Award UI")] 
    [SerializeField] private Image dodgeAwardImage1;
    [SerializeField] private Image dodgeAwardImage2;
    [SerializeField] private Image dodgeAwardImage3;
    
    [Header("Spiritual Power UI")] 
    [SerializeField] private TextMeshProUGUI spiritualPowerText;
    
    /************************************************************************/
    private InputReader _inputReader;
    private Health _health;
    private Mana _mana;
    private Stamina _stamina;
    private HealthPotion _healthPotion;
    private ManaPotion _manaPotion;
    private SubPotion _subPotion;
    private SkillActive _skillActive;
    private DodgeAward _dodgeAward;
    private PlayerStateMachine _playerStateMachine;

    public event Action DodgeAwardAction;
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
        _skillActive = GetComponent<SkillActive>();
        _dodgeAward = GetComponent<DodgeAward>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Start()
    {
        SetupHealthSlider(_health.currentHealth / _health.maxHealth);
        SetupManaSlider(_mana.currentMana / _mana.maxMana);
        SetupStaminaSlider(_stamina.currentStamina / _stamina.maxStamina);
        SetupHealthPotionSlider(_healthPotion.CurrentPotion / _healthPotion.maxPotion);
        SetupManaPotionSlider(_manaPotion.CurrentPotion / _manaPotion.maxPotion);
        ChangeOpacityImageSkill();
        ChangOpacityDodgeIcon();

        foreach (var text in statusTextList)
        {
            ReviewStatusTextList(text.gameObject.name);
        }
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
        // _inputReader.SystemUIAction += ActiveSystemUI;
        _inputReader.SkillAction += UpdateSkillFilled;
        _skillActive.OnUseSkill += OnUpdateByCoolDown;
        _playerStateMachine.UpdateSpiritualPower += UpdateSpiritualPower;
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
        // _inputReader.SystemUIAction -= ActiveSystemUI;
        _inputReader.SkillAction -= UpdateSkillFilled;
        _skillActive.OnUseSkill -= OnUpdateByCoolDown;
        _playerStateMachine.UpdateSpiritualPower -= UpdateSpiritualPower;
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

    /*********************************************Update Status*********************************************/
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

    /*********************************************System UI*********************************************/
    private void ActiveSystemUI()
    {
        systemUI.SetActive(!systemUI.activeInHierarchy);
        _inputReader.SetCursor(!systemUI.activeInHierarchy);
        if (systemUI.activeInHierarchy)
        {
            _playerStateMachine.SwitchState(new PlayerEmptyState(_playerStateMachine));
        }
    }

    /*********************************************Skill UI*********************************************/
    private void ChangeOpacityImageSkill()
    {
        foreach (var skillImage in skillSystemList)
        {
            var color = skillImage.color;
            color.a = .3f;
            skillImage.color = color;
        }
    }

    private void ChangOpacityImageUnActive()
    {
        foreach (var skillImage in skillSystemList)
        {
            if (_skillActive.changingTheGameSkill.skillIcon == skillImage.sprite) continue;
            if (_skillActive.escapeSkill.skillIcon == skillImage.sprite) continue;
            if (_skillActive.responseSkill.skillIcon == skillImage.sprite) continue;

            ChangeOpacity(skillImage, .3f);
        }
    }

    public void UpdateChangeTheGameSkillUI()
    {
        ChangOpacityImageUnActive();
        icon_ChangingTheGame.sprite = _skillActive.changingTheGameSkill.skillIcon;
        var skillImage = FindImage(icon_ChangingTheGame.sprite);
        ChangeOpacity(skillImage, 1f);
    }

    public void UpdateEscapeSkillUI()
    {
        ChangOpacityImageUnActive();
        icon_Escape.sprite = _skillActive.escapeSkill.skillIcon;
        var skillImage = FindImage(icon_Escape.sprite);
        ChangeOpacity(skillImage, 1f);
    }

    public void UpdateResponseSkillUI()
    {
        ChangOpacityImageUnActive();
        icon_Response.sprite = _skillActive.responseSkill.skillIcon;
        var skillImage = FindImage(icon_Response.sprite);
        ChangeOpacity(skillImage, 1f);
    }

    private void UpdateSkillFilled(int skillNumber)
    {
        switch (skillNumber)
        {
            case 1:
                if (_skillActive.changingTheGameSkill.canUse)
                    icon_ChangingTheGame.fillAmount = 0;
                break;
            case 2:
                if (_skillActive.escapeSkill.canUse)
                    icon_Escape.fillAmount = 0;
                break;
            case 3:
                if (_skillActive.responseSkill.canUse)
                    icon_Response.fillAmount = 0;
                break;
        }
    }

    private Image FindImage(Sprite targetSprite)
    {
        return skillSystemList.Find(sprite => sprite.sprite == targetSprite);
    }

    private Image FindSkillImage(int skillNumber)
    {
        return skillNumber switch
        {
            1 => icon_ChangingTheGame,
            2 => icon_Escape,
            3 => icon_Response
        };
    }

    private void OnUpdateByCoolDown(int skillNumber, SkillActiveType skillActiveType)
    {
        StartCoroutine(UpdateFillByCoolDown(skillNumber, skillActiveType));
    }

    private IEnumerator UpdateFillByCoolDown(int skillNumber, SkillActiveType type)
    {
        float coolDown = 0;
        var targetImage = FindSkillImage(skillNumber);
        while (coolDown < type.coolDown)
        {
            coolDown += Time.deltaTime;
            Debug.Log(coolDown);
            var t = coolDown / type.coolDown;
            targetImage.fillAmount = Mathf.Lerp(0, 1f, t);
            yield return null;
        }

        targetImage.fillAmount = 1f;
    }
    
    /*********************************************Dodge Award UI*********************************************/
    private void ChangOpacityDodgeIcon()
    {
        ChangeOpacity(dodgeAwardImage1, .3f);
        ChangeOpacity(dodgeAwardImage2, .3f);
        ChangeOpacity(dodgeAwardImage3, .3f);
    }
    
    public void ActiveDodgeAward(int buttonNumber)
    {
        ChangOpacityDodgeIcon();
        switch (buttonNumber)
        {
            case 1:
                ChangeOpacity(dodgeAwardImage1, 1f);
                break;
            case 2: 
                ChangeOpacity(dodgeAwardImage2, 1f);
                break;
            case 3: 
                ChangeOpacity(dodgeAwardImage3, 1f);
                break;
        }
    }

    private void ChangeOpacity(Image image, float opacity)
    {
        var tempColor = image.color;
        tempColor.a = opacity;
        image.color = tempColor;
    }
    
    /*********************************************Status UI*********************************************/

    private void ReviewStatusTextList(string name)
    {
        switch (name)
        {
            case "Health":
                UpdateStatus(_health.currentHealth, FindStatusText(name));
                break;
            case "Mana":
                UpdateStatus(_mana.currentMana, FindStatusText(name));
                break;
            case "Stamina":
                UpdateStatus(_stamina.currentStamina, FindStatusText(name));
                break;
            case "Resistance":
                UpdateStatus(_health.resistance, FindStatusText(name));
                break;
            case "Strength":
                UpdateStatus(_playerStateMachine.AttackData[0].AttackDamage, FindStatusText(name));
                break;
        }
    }

    private TextMeshProUGUI FindStatusText(string name)
    {
        return statusTextList.Find(text => text.gameObject.name == name);
    }
    private void UpdateStatus(float value, TextMeshProUGUI statusText)
    {
        statusText.text = value.ToString();
    }

    public void AddStatusButton(string name)
    {
        switch (name)
        {
            case "Health":
                _health.AddHealth();
                UpdateStatus(_health.currentHealth, FindStatusText(name));
                break;
            case "Mana":
                _mana.AddMana();
                UpdateStatus(_mana.currentMana, FindStatusText(name));
                break;
            case "Stamina":
                _stamina.AddStamina();
                UpdateStatus(_stamina.currentStamina, FindStatusText(name));
                break;
            case "Resistance":
                _health.AddResistance();
                UpdateStatus(_health.resistance, FindStatusText(name));
                break;
            case "Strength":
                _playerStateMachine.AddDamage();
                UpdateStatus(_playerStateMachine.AttackData[0].AttackDamage, FindStatusText(name));
                break;
        }
    }
    
    public void SubStatusButton(string name)
    {
        switch (name)
        {
            case "Health":
                _health.SubHealth();
                UpdateStatus(_health.currentHealth, FindStatusText(name));
                break;
            case "Mana":
                _mana.SubMana();
                UpdateStatus(_mana.currentMana, FindStatusText(name));
                break;
            case "Stamina":
                _stamina.SubStamina();
                UpdateStatus(_stamina.currentStamina, FindStatusText(name));
                break;
            case "Resistance":
                _health.SubResistance();
                UpdateStatus(_health.resistance, FindStatusText(name));
                break;
            case "Strength":
                _playerStateMachine.SubtractDamage();
                UpdateStatus(_playerStateMachine.AttackData[0].AttackDamage, FindStatusText(name));
                break;
        }
    }
    
    /*********************************************Spiritual Powe*********************************************/

    private void UpdateSpiritualPower(int value)
    {
        spiritualPowerText.SetText(": " +  value.ToString());
    }
}