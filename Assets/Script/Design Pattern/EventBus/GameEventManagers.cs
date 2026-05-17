using Script.Attack.Skill_Factory;
using System;

namespace Script.Design_Pattern.EventBus
{
    public static class GameEventManagers
    {
        public static event Action<ICaster, SkillEffect> OnSkillCasted;
        
        public static void TriggerSkillCasted(ICaster caster, SkillEffect skillEffect)
        {
            OnSkillCasted?.Invoke(caster, skillEffect);
        }
    }
}
