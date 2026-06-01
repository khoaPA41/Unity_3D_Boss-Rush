using System;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.Object_Pooling;
using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.StateMachine.Player.Base;
using Script.Design_Pattern.StateMachine.PlayerClone.Base;
using UnityEngine;
using UnityEngine.Timeline;

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
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            var spawnPos = caster.GetTransform().transform.position;
            var effect = caster.GetTransform().GetComponent<PlayerStateMachine>().Health;
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();

            var ironMaterials = new[] { player.IronMaterial1, player.IronMaterial2 };
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
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            caster.ComsumeMana(ManaCost);
            player.Invisible = true;
            var phantomMaterials = new[] { player.PhantomMaterial1, player.PhantomMaterial2 };
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
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
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
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            var spawnPos = caster.GetTransform().transform.position;
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
        private const string Clone = "PlayerClone";

        public void Cast(ICaster caster)
        {
            var getSkill = caster.GetTransform().GetComponent<GetSkill>();
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            caster.ComsumeMana(ManaCost);

            getSkill.SpawnSkill(SkillName, caster.GetTransform().transform.position);

            for (var i = 0; i < 1; i++)
            {
                getSkill.SpawnSkill(Clone, caster.GetTransform().transform.position);

                var playerClone = getSkill.Skill.GetComponent<PlayerCloneStateMachine>();
                playerClone.Target = player.Targeter.currentTarget.gameObject;
            }

            GameEventManagers.TriggerSkillCasted(caster, SkillEffect);
        }
    }

    /************************************************************************/

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
            var spawnPos = caster.GetTransform().transform.position + new Vector3(0f, 4f, 0f);
            Action situationAction = null;
            situationAction = () =>
            {
                getSkill.SpawnSkill(SkillName, spawnPos);
                var throwSword = getSkill.Skill.GetComponent<SwordSkill>();
                throwSword.TargetPosition = bossStateMachine.PlayerStateMachine.transform.position;
                bossStateMachine.Target = throwSword.transform;
                SkillSituationEvent.Instance.SituationEvent -= situationAction;
            };

            SkillSituationEvent.Instance.SituationEvent += situationAction;
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
            bossStateMachine.IsCanMove = true;

            Action situationAction = null;
            situationAction = () =>
            {
                bossStateMachine.IsCanMove = false;
                SkillSituationEvent.Instance.SituationEvent -= situationAction;
            };

            SkillSituationEvent.Instance.SituationEvent += situationAction;
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

            var spawnPos = caster.TargetCaster().transform.position + new Vector3(0f, 1f, 0f);
            getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
            getSkill.Skill.GetComponent<TriggerSkillForBoss>().Caster = caster;
        }
    }

    public class ChokeNeck : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "ChokeNeck";
        public string AnimationName => "ChokeNeck";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.GetTransform().GetComponent<FinalBossStateMachine>();
            var timelineAsset = (TimelineAsset)bossStateMachine.PlayableDirector.playableAsset;
            var playerTrack = timelineAsset.GetOutputTrack(bossStateMachine.PlayerIndexInTimeLine) as AnimationTrack;

            playerTrack.trackOffset = TrackOffset.ApplyTransformOffsets;
            playerTrack.position = bossStateMachine.BossHand.position;

            bossStateMachine.PlayableDirector.SetGenericBinding(playerTrack, bossStateMachine.PlayerStateMachine.GetComponent<Animator>());
            bossStateMachine.PlayableDirector.Play();

            bossStateMachine.Coroutine(8f, () =>
            {
                bossStateMachine.PlayableDirector.Stop();
                SkillSituationEvent.Instance.SendNextActionEvent();
            });
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