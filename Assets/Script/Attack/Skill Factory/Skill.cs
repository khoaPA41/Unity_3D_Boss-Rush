using System;
using System.Collections;
using UnityEngine;


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
            FinalBossStateMachine finalBoss = player.Targeter.currentTarget?.GetComponent<FinalBossStateMachine>();
            finalBoss.SetMovement();
            finalBoss.ReturnLocomotion();
            caster.ComsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, player.Targeter.GetTargetPosition());
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

        Material[] tempMaterials = player.SkinnedMeshRenderer.materials;
        tempMaterials[0].SetFloat("_Metallic", 1f);
        tempMaterials[1].SetFloat("_Metallic", 1f);
        player.SkinnedMeshRenderer.materials = tempMaterials;
        //player.SkinnedMeshRenderer.materials[0].SetFloat("_Metallic", 1);
        //player.SkinnedMeshRenderer.materials[1].SetFloat("_Metallic", 1);
        ResetAfterUseSkill.instance.StartFeature(1f, () =>
        {
            tempMaterials[0].SetFloat("_Metallic", 0f);
            tempMaterials[1].SetFloat("_Metallic", 0f);
        });

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
        PlayerStateMachine player = caster.TargetCaster().GetComponent<PlayerStateMachine>();

        Material[] tempMaterials = player.SkinnedMeshRenderer.materials;
        tempMaterials[0] = player.PhantomMaterial;
        tempMaterials[1] = player.PhantomMaterial;

        Color newColor = tempMaterials[0].GetColor("_BaseColor");
        newColor.a = 0.4f;

        tempMaterials[0].SetColor("_BaseColor", newColor);
        tempMaterials[1].SetColor("_BaseColor", newColor);
        player.SkinnedMeshRenderer.materials = tempMaterials;

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
        getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
        Debug.Log("Worldbreaker");
        ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        skill.Play();
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

        Vector3 spawnPos = caster.TargetCaster().transform.position;
        spawnPos.y += 1f;
        spawnPos.x += .6f;

        caster.ComsumeMana(ManaCost);
        getSkill.SpawnSkill(SkillName, spawnPos);

        ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        skill.Play();
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
        getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
        ParticleSystem skill = GameObject.Find(SkillName).GetComponent<ParticleSystem>();
        skill.Play();
    }
}



