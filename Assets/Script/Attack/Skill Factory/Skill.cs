using UnityEngine;

public class Freeze : ISkill
{
    public string SkillName => "Freeze";
    public int ManaCost => 20;

    public void Cast(ICaster caster)
    {
        GetSkill getSkill = caster.TargetCaster().GetComponent<GetSkill>();
        PlayerStateMachine player = caster.TargetCaster().GetComponent<PlayerStateMachine>();

        if (player.Targeter.currentTarget != null)
        {
            caster.ComsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, player.Targeter.currentTarget.transform.position);
        }

        ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        skill.Play();
    }
}

public class Shield : ISkill
{
    public string SkillName => "Shield";
    public int ManaCost => 30;

    public void Cast(ICaster caster)
    {
        GetSkill getSkill = caster.TargetCaster().GetComponent<GetSkill>();
        caster.ComsumeMana(ManaCost);
        getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);

        ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        skill.Play();
    }
}
