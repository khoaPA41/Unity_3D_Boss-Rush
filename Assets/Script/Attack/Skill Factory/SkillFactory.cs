using System;
using UnityEngine;

namespace Script.Attack.Skill_Factory
{
    public enum SkillType
    {
        NonSkill,
        Inescapable,
        Indestructible,
        Invisible,
        WorldBreaker,
        PhantomRetreat,
        PhantomMirage
    }
    
    public enum SkillEffect
    {
        NonEffect,
        Inescapable,
        Stunned,
        ThrowUp,
        NoDamage,
        Invisible
    }
    
    public static class SkillFactory
    {
        public static ISkill CreateSkill(int skillNumber)
        {
            switch (GetSkillName(skillNumber))
            {
                case SkillType.Inescapable:
                    return new Inescapable();
                
                case SkillType.Indestructible:
       
                    return new Indestructible();

                case SkillType.Invisible:
                    return new Invisible();

                case SkillType.WorldBreaker:
                    return new WorldBreaker();

                case SkillType.PhantomRetreat:
                    return new PhantomRetreat();

                case SkillType.PhantomMirage:
                    return new PhantomMirage();

                case SkillType.NonSkill:
                default:
                    return null;
            }
        }

        private static SkillType GetSkillName(int skillNumber)
        {
            return skillNumber switch
            {
                1 => SkillType.Inescapable,
                2 => SkillType.Indestructible,
                3 => SkillType.Invisible,
                4 => SkillType.WorldBreaker,
                5 => SkillType.PhantomRetreat,
                6 => SkillType.PhantomMirage,
                _ => SkillType.NonSkill
            };
        }
    }
}