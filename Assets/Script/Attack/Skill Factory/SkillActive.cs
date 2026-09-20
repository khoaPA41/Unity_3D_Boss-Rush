using System;
using System.Collections;
using System.Collections.Generic;
using Attack.Skill_Factory;
using UnityEngine;

namespace Attack
{
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

        public event Action<string> UpdateSkillUIEvent;
        public event Action<int, SkillActiveType> OnUseSkill;
        public event Action<int> UseSkillSuccess;

        private void OnEnable()
        {
            UseSkillSuccess += CountCoolDown;
        }

        private void OnDisable()
        {
            UseSkillSuccess -= CountCoolDown;
        }

        public void CallUseSkillSuccess(int skillNumber)
        {
            UseSkillSuccess?.Invoke(skillNumber);
        }

        public void CountCoolDown(int skillNumber)
        {
            switch (skillNumber)
            {
                case 1:
                    if (changingTheGameSkill.canUse)
                    {
                        OnUseSkill?.Invoke(skillNumber, changingTheGameSkill);
                        changingTheGameSkill.canUse = false;
                        StartCoroutine(CountCoolDownCoroutine(changingTheGameSkill.coolDown, changingTheGameSkill));
                    }

                    break;
                case 2:
                    if (escapeSkill.canUse)
                    {
                        OnUseSkill?.Invoke(skillNumber, escapeSkill);
                        escapeSkill.canUse = false;
                        StartCoroutine(CountCoolDownCoroutine(escapeSkill.coolDown, escapeSkill));
                    }

                    break;
                case 3:
                    if (responseSkill.canUse)
                    {
                        OnUseSkill?.Invoke(skillNumber, responseSkill);
                        responseSkill.canUse = false;
                        StartCoroutine(CountCoolDownCoroutine(responseSkill.coolDown, responseSkill));
                    }
                    break;
                default:
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
            UpdateSkillUIEvent?.Invoke("ChangingTheGame");
        }

        public void UpdateEscapeSkill(string name)
        {
            escapeSkill = escapeList.Find(skill => skill.skillName == name);
            UpdateSkillUIEvent?.Invoke("Escape");
        }

        public void UpdateResponseSkill(string name)
        {
            responseSkill = responseList.Find(skill => skill.skillName == name);
            UpdateSkillUIEvent?.Invoke("Response");
        }

        public void UpdateSkillUIByType()
        {
            if (changingTheGameSkill != null)
            {
                UpdateChangingTheGameSkill(changingTheGameSkill.skillName);
            }
            if (escapeSkill != null)
            {
                UpdateEscapeSkill(escapeSkill.skillName);
            }
            if (responseSkill != null)
            {
                UpdateResponseSkill(responseSkill.skillName);
            }
        }
    }
}