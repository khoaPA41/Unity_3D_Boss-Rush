using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Boss.Main;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.StateMachine.PlayerClone.Base;
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

            if (player.Targeter.currentTarget is not null)
            {
                GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
            }
        }
    }

    public class Indestructible : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NoDamage;
        public string SkillName => "Indestructible";
        public int ManaCost => 30;
        public string AnimationName => "Indestructible";

        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            var spawnPos = caster.TargetCaster().transform.position;
            var effect = caster.TargetCaster().GetComponent<PlayerStateMachine>().Health;
            var player = caster.TargetCaster().GetComponent<PlayerStateMachine>();

            var ironMaterials = new[] {player.IronMaterial1, player.IronMaterial2 };
            player.SkinnedMeshRenderer.materials = ironMaterials;
            
            caster.ComsumeMana(ManaCost);
            
            spawnPos.y += 1f;
            getSkill.SpawnSkill(SkillName, spawnPos);
            effect.noDamage = true;
            
            
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
        }
    }

    public class Invisible : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.Invisible;
        public string SkillName => "Invisible";
        public int ManaCost => 20;

        public string AnimationName => "Invisible";

        public void Cast(ICaster caster)
        {
            // var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            var player = caster.TargetCaster().GetComponent<PlayerStateMachine>();
            caster.ComsumeMana(ManaCost);
            player.Invisible = true;
            var phantomMaterials = new[] {player.PhantomMaterial1, player.PhantomMaterial2 };
            player.SkinnedMeshRenderer.materials = phantomMaterials;
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
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
            var player = caster.TargetCaster().GetComponent<PlayerStateMachine>();
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            caster.ComsumeMana(ManaCost);
            player.Invincible = true;
            
            player.InvincibleState();
            
            getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
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
            var player = caster.TargetCaster().GetComponent<PlayerStateMachine>();
            var spawnPos = caster.TargetCaster().transform.position;
            spawnPos.y += 1f;
            spawnPos.x += .6f;

            caster.ComsumeMana(ManaCost);
            
            getSkill.SpawnSkill(SkillName, spawnPos);
            var bullet = getSkill.Skill.GetComponentInChildren(typeof(Bullet)) as Bullet;
            bullet?.SetTarget(player.Targeter.currentTarget.gameObject);
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
        }
    }

    public class PhantomMirage : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "PhantomMirage";
        public int ManaCost => 30;
        public string AnimationName => "PhantomMirage";
        private string Clone = "PlayerClone";  
        public void Cast(ICaster caster)
        {
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            var player = caster.TargetCaster().GetComponent<PlayerStateMachine>();
            caster.ComsumeMana(ManaCost);
            
            getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);

            for (var i = 0; i < 1; i++)
            {
                getSkill.SpawnSkill(Clone, caster.TargetCaster().transform.position);
                
                var playerClone = getSkill.Skill.GetComponent<PlayerCloneStateMachine>();
                playerClone.Target = player.Targeter.currentTarget.gameObject;
            }
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
        }
    }
    
    public class PhaseTwoUltimate : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "PhaseTwoUltimate";
        public string AnimationName => "PhaseTwoUltimate";
        public int ManaCost => 20;
        
        public void Cast(ICaster caster)
        {
            Debug.Log("PhaseTwoUltimate");
        }
    }
    
    public class PhaseThreeUltimate : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "PhaseThreeUltimate";
        public string AnimationName => "PhaseThreeUltimate";
        public int ManaCost => 20;
        
        public void Cast(ICaster caster)
        {
            Debug.Log("PhaseThreeUltimate");
        }
    }
    
    public class ThrowSword : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "ThrowSword";
        public string AnimationName => "ThrowSword";
        public int ManaCost => 20;
        
        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.TargetCaster().GetComponent<FinalBossStateMachine>();
            var getSkill = caster.TargetCaster().GetComponent<GetSkill>();
            Vector3 spawnPos = caster.TargetCaster().transform.position + new Vector3(0f, 4f, 0f);
            
            
            bossStateMachine.Coroutine(.5f, () =>
            {
                getSkill.SpawnSkill(SkillName, spawnPos);
                var throwSword = getSkill.Skill.GetComponent<SwordSkill>();
                throwSword.targetPosition = bossStateMachine.PlayerStateMachine.transform.position;
            });

        }
    }
    
    public class JumpToSword : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "JumpToSword";
        public string AnimationName => "Jump";
        public int ManaCost => 20;
        
        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.TargetCaster().GetComponent<FinalBossStateMachine>();
            bossStateMachine.IsCanMove = true;
            
            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
        }
    }
    public class SwordAround : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "SwordAround";
        public string AnimationName => "Attack5";
        public int ManaCost => 20;
        
        public void Cast(ICaster caster)
        {
            Debug.Log("SwordAround");
        }
    }
}