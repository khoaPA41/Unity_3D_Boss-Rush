using System;
using UnityEngine;

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

public class SkillEvent: MonoBehaviour
 {
    public static SkillEvent Instance;

    public event Action Inescapable;
    public event Action Indestructible;
    public event Action Invisible;
    public event Action Worldbreaker;
    public event Action PhantomRetreat;
    public event Action PhantomMirage;

    private void Start()
    {
        Instance = this;
    }

    public void ActiveInescapable()
    {
        Inescapable?.Invoke();
    }
    public void ActiveIndestructible()
    {
        Indestructible?.Invoke();
    }
    public void ActiveInvisible()
    {
        Invisible?.Invoke();
    }
}


public class SkillFactory
{
    public static ISkill CreateSkill(int skillNumber)
    {
        switch (GetSkillName(skillNumber))
        {
            case SkillType.Inescapable:
                //SkillEvent.Instance.ActiveInescapable();
                return new Inescapable();

            case SkillType.Indestructible:
                //SkillEvent.Instance.ActiveIndestructible();
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
