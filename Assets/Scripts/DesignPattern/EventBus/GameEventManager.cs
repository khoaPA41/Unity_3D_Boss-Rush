using System;
using Attack.SkillFactory;
using UnityEngine;

namespace DesignPattern.EventBus
{
    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance;
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