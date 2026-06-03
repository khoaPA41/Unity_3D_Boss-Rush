using System;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.Boss.Base;
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
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();

            caster.ComsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, player.Targeter.GetTargetPosition());

            if (player.Targeter.currentTarget is not null)
            {
                GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
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
            caster.ComsumeMana(ManaCost);
            
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            var spawnPos = caster.GetTransform().transform.position;
            var effect = caster.GetTransform().GetComponent<PlayerStateMachine>().Health;
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            var ironMaterials = new[] { player.IronMaterial1, player.IronMaterial2 };
            
            player.SkinnedMeshRenderer.materials = ironMaterials;
            spawnPos.y += 1f;
            getSkill.SpawnSkill(SkillName, spawnPos);
            effect.noDamage = true;
            GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
            
            Action situationAction = null;
            situationAction = () =>
            {
                effect.noDamage = false; 
                var tempMaterials = new Material[] { player.MainMaterial1, player.MainMaterial2 };
                player.SkinnedMeshRenderer.materials = tempMaterials;
                player.ManageAnimationSkillEvent.SituationEvent -= situationAction;
            };
            
            player.ManageAnimationSkillEvent.SituationEvent += situationAction;
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
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            caster.ComsumeMana(ManaCost);
            GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
            
            player.Coroutine(4f, () =>
            {
                player.Invisible = true;
                var phantomMaterials = new[] { player.PhantomMaterial1, player.PhantomMaterial2 };
                player.SkinnedMeshRenderer.materials = phantomMaterials;
            },
            () =>
            {
                player.Invisible = false;
                var tempMaterials = new Material[] {player.MainMaterial1, player.MainMaterial2};
                player.SkinnedMeshRenderer.materials = tempMaterials;
            });
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
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            caster.ComsumeMana(ManaCost);
            
            player.Invincible = true;
            player.InvincibleState();
            
            getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
            GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
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
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            var spawnPos = caster.GetTransform().transform.position;
            spawnPos.y += 1f;
            spawnPos.x += .6f;

            caster.ComsumeMana(ManaCost);

            getSkill.SpawnSkill(SkillName, spawnPos);
            var bullet = getSkill.Skill.GetComponentInChildren(typeof(Bullet)) as Bullet;
            bullet?.SetTarget(player.Targeter.currentTarget.gameObject);
            
            GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
        }
    }

    public class PhantomMirage : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "PhantomMirage";
        public int ManaCost => 30;
        public string AnimationName => "PhantomMirage";
        private const string Clone = "PlayerClone";

        public void Cast(ICaster caster)
        {
            // var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            // var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            caster.ComsumeMana(ManaCost);
            //
            // getSkill.SpawnSkill(SkillName, caster.GetTransform().transform.position);
            //
            // for (var i = 0; i < 1; i++)
            // {
            //     getSkill.SpawnSkill(Clone, caster.GetTransform().transform.position);
            //
            //     var playerClone = getSkill.Skill.GetComponent<PlayerCloneStateMachine>();
            //     playerClone.Target = player.Targeter.currentTarget.gameObject;
            // }
            GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
        }
    }

    /*****************************BOSS*SKILL*******************************************/

    public class ThrowSword : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "ThrowSword";
        public string AnimationName => "ThrowSword";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.GetTransform().GetComponent<FinalBossStateMachine>();
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            var spawnPos = caster.GetTransform().transform.position + new Vector3(0f, 4f, 0f);
            Action situationAction = null;
            situationAction = () =>
            {
                getSkill.SpawnSkill(SkillName, spawnPos);
                var throwSword = getSkill.Skill.GetComponent<SwordSkill>();
                throwSword.TargetPosition = bossStateMachine.PlayerStateMachine.transform.position;
                bossStateMachine.Target = throwSword.transform;
                manageEvent.SituationEvent -= situationAction;
            };

            manageEvent.SituationEvent += situationAction;
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
            var bossStateMachine = caster.GetTransform().GetComponent<FinalBossStateMachine>();
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            bossStateMachine.IsCanMove = true;

            Action situationAction = null;
            situationAction = () =>
            {
                bossStateMachine.IsCanMove = false;
                manageEvent.SituationEvent -= situationAction;
            };

            manageEvent.SituationEvent += situationAction;
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
            var bossStateMachine = caster.GetTransform().GetComponent<FinalBossStateMachine>();
            bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;
        }
    }

    /************************************************************************/
    public class PullBack : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.PullBack;
        public string SkillName => "PullBack";
        public string AnimationName => "PullBack";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.GetTransform().GetComponent<FinalBossStateMachine>();
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;
            
            GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
            
            Action situationAction = null;
            situationAction = () =>
            {
                getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
                getSkill.Skill.GetComponent<TriggerSkillForBoss>().Caster = caster;
              
                bossStateMachine.ManageAnimationSkillEvent.SituationEvent -= situationAction;
            };
            bossStateMachine.ManageAnimationSkillEvent.SituationEvent += situationAction;
        }
    }

    public class ChokeNeck : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.AttractiveForce;
        public string SkillName => "ChokeNeck";
        public string AnimationName => "ChokeNeck";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.GetTransform().GetComponent<FinalBossStateMachine>();
            var player =  bossStateMachine.PlayerStateMachine;
            // Debug.Log("In Skill: " + player.IsAttractiveForce );
            GameEventManagers.Instance.TriggerSkillCasted(caster, SkillEffect);
            // Action situationAction = null;
            // situationAction = () =>
            // {
            //     player.IsAttractiveForce = false;
            //     bossStateMachine.ManageAnimationSkillEvent.SituationEvent -= situationAction;
            // };
            // bossStateMachine.ManageAnimationSkillEvent.SituationEvent += situationAction;
        }
    }

    public class FirstAoe : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "FirstAoe";
        public string AnimationName => "FirstAoe";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            // var bossStateMachine = caster.TargetCaster().GetComponent<FinalBossStateMachine>();
            // bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;
            Debug.Log("FirstAoe");
        }
    }
}