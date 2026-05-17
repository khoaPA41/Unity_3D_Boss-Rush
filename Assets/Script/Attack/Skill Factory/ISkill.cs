namespace Script.Attack.Skill_Factory
{
    public interface ISkill
    {
        SkillEffect SkillEffect { get; }
        string SkillName { get; }
        string AnimationName { get; }
        int ManaCost { get; }
        void Cast(ICaster caster);
    }
}




