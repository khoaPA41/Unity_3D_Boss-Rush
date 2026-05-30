

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
        PhantomMirage,
        PhaseTwoUltimate,
        PhaseThreeUltimate,
        ThrowSword,
        JumpToSword,
        SwordAround
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
        public static ISkill CreateSkill(SkillType  skillType)
        {
            switch (skillType)
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
                
                case SkillType.ThrowSword:
                    return new ThrowSword();
                
                case SkillType.JumpToSword:
                    return new JumpToSword();
                
                case SkillType.SwordAround:
                    return new SwordAround();
                
                case SkillType.NonSkill:
                default:
                    return null;
            }
        }
    }
}