using System;
using System.Collections;
using System.Collections.Generic;
using Script.Attack.Skill_Factory;
using UnityEngine;

[Serializable]
public class SkillActiveType
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public string skillAnimationName;
    public string skillAnimationTag;
    public SkillType skillType;
    public float coolDown;
    public bool canUse;
    public float countCoolDown { get; set; }
}

public class SkillActive : MonoBehaviour
{
    [SerializeField] private List<SkillActiveType> changingTheGameList;
    [SerializeField] private List<SkillActiveType> escapeList;
    [SerializeField] private List<SkillActiveType> responseList;

    public SkillActiveType changingTheGameSkill;
    public SkillActiveType escapeSkill;
    public SkillActiveType responseSkill;

    private InputReader _inputReader;

    public event Action<int, SkillActiveType> OnUseSkill;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        _inputReader.SkillAction += CountCoolDown;
    }

    private void OnDisable()
    {
        _inputReader.SkillAction -= CountCoolDown;

    }
    
    private void CountCoolDown(int skillNumber)
    {
        switch (skillNumber)
        {
            case 1:
                if (changingTheGameSkill.canUse)
                {
                    OnUseSkill?.Invoke(skillNumber, changingTheGameSkill);
                    StartCoroutine(CountCoolDownCoroutine(changingTheGameSkill.coolDown, changingTheGameSkill));
                }

                break;
            case 2:
                if (escapeSkill.canUse)
                {
                    OnUseSkill?.Invoke(skillNumber, escapeSkill);
                    StartCoroutine(CountCoolDownCoroutine(escapeSkill.coolDown, escapeSkill));
                }
 
                break;
            case 3:
                if (responseSkill.canUse)
                {
                    OnUseSkill?.Invoke(skillNumber, responseSkill);
                    StartCoroutine(CountCoolDownCoroutine(responseSkill.coolDown, responseSkill));
                }
                break;
        }
    }

    private IEnumerator CountCoolDownCoroutine(float coolDown, SkillActiveType skill)
    {
        float countCoolDown = 0;
        while (countCoolDown < coolDown)
        {
            countCoolDown += Time.deltaTime;
            yield return null;
        }

        skill.canUse = true;
    }

    public void UpdateChangingTheGameSkill(string name)
    {
        changingTheGameSkill = changingTheGameList.Find(skill => skill.skillName == name);
    }

    public void UpdateEscapeSkill(string name)
    {
        escapeSkill = escapeList.Find(skill => skill.skillName == name);
    }

    public void UpdateResponseSkill(string name)
    {
        responseSkill = responseList.Find(skill => skill.skillName == name);
    }
}