public interface ISkill
{
    string SkillName { get; }
    int ManaCost { get; }
    void Cast(ICaster caster);
}
