public enum SkillType
{
    NonSkill,
    Freeze,
    Shield
}


public class SkillFactory
{
    public static ISkill CreateSkill(int skillNumber)
    {
        switch (GetSkillName(skillNumber))
        {
            case SkillType.Freeze:
                return new Freeze();
            case SkillType.Shield:
                return new Shield();
            case SkillType.NonSkill:
                return null;
            default:
                return null;
        }
    }

    public static SkillType GetSkillName(int skillNumber)
    {
        switch (skillNumber)
        {
            case 1:
                return SkillType.Freeze;
            case 2:
                return SkillType.Shield;
            default:
                return SkillType.NonSkill;
        }
    }
}
