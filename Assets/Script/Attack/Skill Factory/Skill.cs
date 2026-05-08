using UnityEngine;
public class FireBall : ISkill
{
    public int ManaCost => 20;

    public void Cast(ICaster caster)
    {
        Debug.Log("FireBall");
        caster.ComsumeMana(ManaCost);
    }
}

public class DashSword : ISkill
{
    public int ManaCost => 30;

    public void Cast(ICaster caster)
    {
        Debug.Log("DashSword");
        caster.ComsumeMana(ManaCost);
    }
}
