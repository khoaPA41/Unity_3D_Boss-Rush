using System;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Attack.Skill_Factory
{
    public class Inescapable : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.Inescapable;
        public string SkillName => "Inescapable";
        public string AnimationName => "Inescapable";
        public int ManaCost => 20;
        

        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            var player = caster.TargetCaster().GetComponent<PlayerStateMachine>();
            
            caster.ComsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, player.Targeter.GetTargetPosition());
            
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
        }
    }

    public class Indestructible : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "Indestructible";
        public int ManaCost => 30;
        public string AnimationName => "Indestructible";

        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            caster.ComsumeMana(ManaCost);
            var spawnPos = caster.TargetCaster().transform.position;
            Debug.Log(caster.TargetCaster().transform.position);
            spawnPos.y += 1f;
            getSkill.SpawnSkill(SkillName, spawnPos);

            var tempMaterials = caster.TargetCaster().GetComponent<PlayerStateMachine>().SkinnedMeshRenderer.materials;
            
            tempMaterials[0].SetFloat("_Metallic", 1f);
            tempMaterials[1].SetFloat("_Metallic", 1f);
            
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
        }
    }

    public class Invisible : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "Invisible";
        public int ManaCost => 20;

        public string AnimationName => "Invisible";

        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            caster.ComsumeMana(ManaCost);
            var player = caster.TargetCaster().GetComponent<PlayerStateMachine>();

            var tempMaterials = player.SkinnedMeshRenderer.materials;
            tempMaterials[0] = player.PhantomMaterial;
            tempMaterials[1] = player.PhantomMaterial;

            Color newColor = tempMaterials[0].GetColor("_BaseColor");
            newColor.a = 0.4f;

            tempMaterials[0].SetColor("_BaseColor", newColor);
            tempMaterials[1].SetColor("_BaseColor", newColor);
            player.SkinnedMeshRenderer.materials = tempMaterials;
        }
    }

    public class WorldBreaker : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;

        public string SkillName => "WorldBreaker";
        public int ManaCost => 30;

        public string AnimationName => "WorldBreaker";

        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            caster.ComsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
            Debug.Log("WorldBreaker");
        }
    }

    public class PhantomRetreat : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;

        public string SkillName => "PhantomRetreat";
        public int ManaCost => 30;

        public string AnimationName => "PhantomRetreat";

        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();

            var spawnPos = caster.TargetCaster().transform.position;
            spawnPos.y += 1f;
            spawnPos.x += .6f;

            caster.ComsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, spawnPos);
            
        }
    }

    public class PhantomMirage : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "PhantomMirage";
        public int ManaCost => 30;

        public string AnimationName => "PhantomMirage";

        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            caster.ComsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
        }
    }
}