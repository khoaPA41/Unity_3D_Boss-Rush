public enum SkillType
{
    NonSkill,
    Inescapable,
    Indestructible,
    Invisible,
    Worldbreaker,
    PhantomRetreat,
    PhantomMirage
}


public class SkillFactory
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

            case SkillType.Worldbreaker:
                return new Worldbreaker();

            case SkillType.PhantomRetreat:
                return new PhantomRetreat();

            case SkillType.PhantomMirage:
                return new PhantomMirage();

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
                return SkillType.Inescapable;
            case 2:
                return SkillType.Indestructible;
            case 3:
                return SkillType.Invisible;
            case 4:
                return SkillType.Worldbreaker;
            case 5:
                return SkillType.PhantomRetreat;
            case 6:
                return SkillType.PhantomMirage;
            default:
                return SkillType.NonSkill;
        }
    }
}
