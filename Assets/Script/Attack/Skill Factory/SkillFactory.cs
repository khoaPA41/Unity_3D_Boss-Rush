public enum SkillType
{
    NonSkill,
    FireBall,
    DashSword
}


public class SkillFactory
{
    public static ISkill CreateSkill(int skillNumber)
    {
        switch (GetSkillName(skillNumber))
        {
            case SkillType.FireBall:
                return new FireBall();
            case SkillType.DashSword:
                return new DashSword();
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
                return SkillType.FireBall;
            case 2:
                return SkillType.DashSword;
            default:
                return SkillType.NonSkill;
        }
    }
}
