

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
        ThrowSword,
        JumpToSword,
        SwordAround,
        PullBack,
        ChokeNeck,
        FirstAoe
    }
    
    public enum SkillEffect
    {
        NonEffect,
        Inescapable,
        Stunned,
        ThrowUp,
        NoDamage,
        Invisible,
        PullBack
    }
    
    public static class SkillFactory
    {
        public static ISkill CreateSkill(SkillType  skillType)
        {
            return skillType switch
            {
                SkillType.Inescapable => new Inescapable(),
                SkillType.Indestructible => new Indestructible(),
                SkillType.Invisible => new Invisible(),
                SkillType.WorldBreaker => new WorldBreaker(),
                SkillType.PhantomRetreat => new PhantomRetreat(),
                SkillType.PhantomMirage => new PhantomMirage(),
                SkillType.ThrowSword => new ThrowSword(),
                SkillType.JumpToSword => new JumpToSword(),
                SkillType.SwordAround => new SwordAround(),
                SkillType.PullBack => new PullBack(),
                SkillType.ChokeNeck => new ChokeNeck(),
                SkillType.FirstAoe => new FirstAoe(),
                _ => null
            };
        }
    }
}