using System.Collections;
using UnityEngine;
public static class SpecialFeature
{
    public static IEnumerator ResetMaterial(float time, Material[] materials)
    {
        yield return new WaitForSecondsRealtime(time);
        foreach (Material material in materials)
        {
            material.SetFloat("_Metallic", 0);
        }
    }
}


public class Inescapable : ISkill
{
    public string SkillName => "Inescapable";

    public int ManaCost => 20;

    public string AnimationName => "Inescapable";

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
public class Indestructible : ISkill
{
    public string SkillName => "Indestructible";
    public int ManaCost => 30;

    public string AnimationName => "Indestructible";

    public void Cast(ICaster caster)
    {
        GetSkill getSkill = caster.TargetCaster().GetComponent<GetSkill>();
        caster.ComsumeMana(ManaCost);
        Vector3 spawnPos = caster.TargetCaster().transform.position;
        spawnPos.y += 1f;
        getSkill.SpawnSkill(SkillName, spawnPos);

        ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        skill.Play();
        PlayerStateMachine player = caster.TargetCaster().GetComponent<PlayerStateMachine>();

        player.SkinnedMeshRenderer.materials[0].SetFloat("_Metallic", 1);
        player.SkinnedMeshRenderer.materials[1].SetFloat("_Metallic", 1);
        SpecialFeature.ResetMaterial(1f, player.SkinnedMeshRenderer.materials);

    }
}
public class Invisible : ISkill
{
    public string SkillName => "Invisible";
    public int ManaCost => 20;
    public string AnimationName => "Invisible";

    public void Cast(ICaster caster)
    {
        GetSkill getSkill = caster.TargetCaster().GetComponent<GetSkill>();
        caster.ComsumeMana(ManaCost);
        //getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
        Debug.Log("Invisible");
        //ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        //skill.Play();
    }
}
public class Worldbreaker : ISkill
{
    public string SkillName => "Worldbreaker";
    public int ManaCost => 30;

    public string AnimationName => "Worldbreaker";

    public void Cast(ICaster caster)
    {
        GetSkill getSkill = caster.TargetCaster().GetComponent<GetSkill>();
        caster.ComsumeMana(ManaCost);
        //getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
        Debug.Log("Worldbreaker");
        //ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        //skill.Play();
    }
}
public class PhantomRetreat : ISkill
{
    public string SkillName => "PhantomRetreat";
    public int ManaCost => 30;

    public string AnimationName => "PhantomRetreat";

    public void Cast(ICaster caster)
    {
        GetSkill getSkill = caster.TargetCaster().GetComponent<GetSkill>();
        caster.ComsumeMana(ManaCost);
        //getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
        Debug.Log("PhantomRetreat");
        //ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        //skill.Play();
    }
}
public class PhantomMirage : ISkill
{
    public string SkillName => "PhantomMirage";
    public int ManaCost => 30;

    public string AnimationName => "PhantomMirage";

    public void Cast(ICaster caster)
    {
        GetSkill getSkill = caster.TargetCaster().GetComponent<GetSkill>();
        caster.ComsumeMana(ManaCost);
        //getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
        Debug.Log("PhantomMirage");
        //ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        //skill.Play();
    }
}



