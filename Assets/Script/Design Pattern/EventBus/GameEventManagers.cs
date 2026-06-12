using Script.Attack.Skill_Factory;
using System;
using UnityEngine;

namespace Script.Design_Pattern.EventBus
{
    public class GameEventManagers : MonoBehaviour
    {
        public static GameEventManagers Instance;
        public event Action<ICaster, SkillEffect> OnSkillCasted;
        
        private void Awake()
        {
            Instance = this;
        }

        public void TriggerSkillCasted(ICaster caster, SkillEffect skillEffect)
        {
            OnSkillCasted?.Invoke(caster, skillEffect);
        }
    }
}