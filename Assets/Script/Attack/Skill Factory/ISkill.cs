public interface ISkill
{
    string SkillName { get; }
    string AnimationName { get; }
    int ManaCost { get; }
    void Cast(ICaster caster);
}
