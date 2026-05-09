using UnityEngine;
public class FireBall : ISkill
{
    public string SkillName => "FireBall";
    public int ManaCost => 20;

    public void Cast(ICaster caster)
    {
        Debug.Log("FireBall");
        caster.ComsumeMana(ManaCost);
    }
}

public class DashSword : ISkill
{
    public string SkillName => "DashSword";
    public int ManaCost => 30;

    public void Cast(ICaster caster)
    {
        Debug.Log("DashSword");
        caster.ComsumeMana(ManaCost);
    }
}
