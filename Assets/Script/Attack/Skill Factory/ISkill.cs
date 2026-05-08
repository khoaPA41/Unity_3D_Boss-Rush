public interface ISkill
{
    int ManaCost { get; }
    void Cast(ICaster caster);
}
