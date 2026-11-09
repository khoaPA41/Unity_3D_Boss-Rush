using Audio;
using Attack.Separate;
using System;
using DesignPattern.EventBus;
using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.StateMachine.Player.Base;
using DesignPattern.StateMachine.PlayerClone.Base;
using DesignPattern.ObjectPooling;
using UnityEngine;

namespace Attack.SkillFactory
{
    public class Inescapable : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.Inescapable;
        public string SkillName => "Inescapable";
        public string AnimationName => "Inescapable";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();

            caster.ConsumeMana(ManaCost);
            getSkill.SpawnSkill(SkillName, player.Targeter.GetTargetPosition());

            if (player.Targeter.CurrentTarget != null)
            {
                GameEventManager.Instance.TriggerSkillCasted(caster, SkillEffect);
                AudioManager.Instance.PlaySound(caster.TargetCaster().transform, AudioManager.Instance.inescapableResource);
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
            caster.ConsumeMana(ManaCost);
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            var spawnPos = caster.GetTransform().transform.position;
            var effect = caster.GetTransform().GetComponent<PlayerStateMachine>().Health;
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();

            spawnPos.y += 1f;
            getSkill.SpawnSkill(SkillName, spawnPos);
            effect.noDamage = true;
            GameEventManager.Instance.TriggerSkillCasted(caster, SkillEffect);

            Action situationAction = null;
            situationAction = () =>
            {
                effect.noDamage = false;
                player.HeadIndestructible.SetActive(true);
                player.HeadNormal.SetActive(false);

                player.ArmIndestructible.SetActive(true);
                player.ArmNormal.SetActive(false);

                player.TorsoIndestructible.SetActive(true);
                player.TorsoNormal.SetActive(false);

                player.ManageAnimationSkillEvent.SituationEvent -= situationAction;
            };

            AudioManager.Instance.PlaySound(caster.GetTransform().transform, AudioManager.Instance.indestructibleResource);
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
            caster.ConsumeMana(ManaCost);
            GameEventManager.Instance.TriggerSkillCasted(caster, SkillEffect);

            player.StartTimedAction(4f, () =>
                {
                    player.Invisible = true;
                    player.ArmNormal.SetActive(false);
                    player.HeadNormal.SetActive(false);
                    player.TorsoNormal.SetActive(false);
                    player.ClothNormal.SetActive(false);
                    player.HairNormal.SetActive(false);
                    player.HarnessNormal.SetActive(false);
                    player.LegNormal.SetActive(false);

                    player.ArmInvisible.SetActive(true);
                    player.HeadInvisible.SetActive(true);
                    player.TorsoInvisible.SetActive(true);
                    player.ClothInvisible.SetActive(true);
                    player.HairInvisible.SetActive(true);
                    player.HarnessInvisible.SetActive(true);
                    player.LegInvisible.SetActive(true);

                    AudioManager.Instance.PlaySound(caster.GetTransform().transform, AudioManager.Instance.invisibleResource);

                },
                () =>
                {
                    player.Invisible = false;

                    player.ArmNormal.SetActive(true);
                    player.HeadNormal.SetActive(true);
                    player.TorsoNormal.SetActive(true);
                    player.ClothNormal.SetActive(true);
                    player.HairNormal.SetActive(true);
                    player.HarnessNormal.SetActive(true);
                    player.LegNormal.SetActive(true);

                    player.ArmInvisible.SetActive(false);
                    player.HeadInvisible.SetActive(false);
                    player.TorsoInvisible.SetActive(false);
                    player.ClothInvisible.SetActive(false);
                    player.HairInvisible.SetActive(false);
                    player.HarnessInvisible.SetActive(false);
                    player.LegInvisible.SetActive(false);
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
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            caster.ConsumeMana(ManaCost);

            player.Invincible = true;
            player.ApplyInvincibleBuff();

            getSkill.SpawnSkill(SkillName, caster.GetTransform().transform.position);
            AudioManager.Instance.PlaySound(caster.GetTransform().transform, AudioManager.Instance.worldBreakerResource);
            GameEventManager.Instance.TriggerSkillCasted(caster, SkillEffect);
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
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            var player = caster.GetTransform().GetComponent<PlayerStateMachine>();
            var spawnPos = caster.GetTransform().transform.position;
            spawnPos.y += 1f;
            spawnPos.x += .6f;

            caster.ConsumeMana(ManaCost);

            getSkill.SpawnSkill(SkillName, spawnPos);
            var bullet = getSkill.Skill.GetComponentInChildren(typeof(Bullet)) as Bullet;

            GameObject target = null;
            if (player.Targeter.CurrentTarget != null) target = player.Targeter.CurrentTarget.gameObject;

            bullet?.SetTarget(target);

            AudioManager.Instance.PlaySound(caster.GetTransform().transform, AudioManager.Instance.phantomRetreatResource);
            GameEventManager.Instance.TriggerSkillCasted(caster, SkillEffect);
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
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            caster.ConsumeMana(ManaCost);

            getSkill.SpawnSkill(SkillName, caster.GetTransform().transform.position);

            getSkill.SpawnSkill(Clone, caster.GetTransform().transform.position);

            AudioManager.Instance.PlaySound(caster.GetTransform().transform, AudioManager.Instance.phantomMirageResource);

            GameEventManager.Instance.TriggerSkillCasted(caster, SkillEffect);
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
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            var spawnPos = caster.GetTransform().transform.position + new Vector3(0f, 4f, 0f);
            Action situationAction = null;
            situationAction = () =>
            {
                getSkill.SpawnSkill(SkillName, spawnPos);

                var throwSword = getSkill.Skill.GetComponent<SwordSkill>();
                throwSword.TargetPosition = bossStateMachine.PlayerStateMachine.transform.position;
                throwSword.InitializeBullet();
                bossStateMachine.Target = throwSword.transform;
                AudioManager.Instance.PlaySound(caster.TargetCaster().transform, AudioManager.Instance.throwSwordResource);

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
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            bossStateMachine.IsCanMove = true;
            manageEvent.ClearSituationEvent();

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
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
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
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;

            GameEventManager.Instance.TriggerSkillCasted(caster, SkillEffect);

            Action situationAction = null;
            situationAction = () =>
            {
                getSkill.SpawnSkill(SkillName, caster.TargetCaster().transform.position);
                getSkill.Skill.GetComponent<TriggerSkillForBoss>().Caster = caster;

                AudioManager.Instance.PlaySound(caster.TargetCaster().transform, AudioManager.Instance.waveResource);
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
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();

            Action situationAction = null;
            var castCount = 0f;
            situationAction = () =>
            {
                bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;
                var spawnPosition = caster.TargetCaster().transform.position;
                getSkill.SpawnSkill(SkillName, spawnPosition);
                getSkill.Skill.GetComponent<TriggerSkillForBoss>().Caster = caster;
                castCount++;
                AudioManager.Instance.PlaySound(caster.TargetCaster().transform, AudioManager.Instance.firstAOEResource);

                if (castCount >= 4)
                {
                    manageEvent.SituationEvent -= situationAction;
                }
            };
            manageEvent.SituationEvent += situationAction;
        }
    }

    /************************************************************************/
    public class FireBullet : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "FireBullet";
        public string AnimationName => "FireBullet";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
            bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            var getSkill = caster.GetTransform().GetComponent<SkillSpawner>();
            var spawnPosition = caster.GetTransform().transform.position + new Vector3(2f, 3f, 0f);

            manageEvent.ClearSituationEvent();

            Action situationAction = null;
            situationAction = () =>
            {
                getSkill.SpawnSkill(SkillName, spawnPosition);

                var skill = getSkill.Skill.GetComponent<SwordSkill>();
                skill.TargetPosition = caster.TargetCaster().transform.position;
                skill.InitializeBullet();
                var weaponTrail = skill.GetComponent<WeaponTrail>();
                weaponTrail.SetDamage(500);
                AudioManager.Instance.PlaySound(caster.TargetCaster().transform, AudioManager.Instance.fireBulletResource);
                manageEvent.SituationEvent -= situationAction;
            };
            manageEvent.SituationEvent += situationAction;
        }
    }

    /************************************************************************/
    public class TransformToTwoSword : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "TransformToTwoSword";
        public string AnimationName => "TransformToTwoSword";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
            bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            var weaponHandler = bossStateMachine.GetComponent<WeaponHandler>();

            Action situationAction = null;
            situationAction = () =>
            {
                weaponHandler.OnStoreWeapon();
                weaponHandler.DeactivateDoubleEdgedSword();
                weaponHandler.ActivateLeftSingleSword();
                weaponHandler.ActivateRightSingleSword();
                AudioManager.Instance.PlaySound(caster.TargetCaster().transform, AudioManager.Instance.transformToTwoSwordResource);

                manageEvent.SituationEvent -= situationAction;
            };
            manageEvent.SituationEvent += situationAction;
        }
    }

    public class TransformToBlade : ISkill
    {
        public SkillEffect SkillEffect => SkillEffect.NonEffect;
        public string SkillName => "TransformToBlade";
        public string AnimationName => "TransformToBlade";
        public int ManaCost => 20;

        public void Cast(ICaster caster)
        {
            var bossStateMachine = caster.GetTransform().GetComponent<BossStateMachine>();
            bossStateMachine.Target = bossStateMachine.PlayerStateMachine.transform;
            var manageEvent = bossStateMachine.ManageAnimationSkillEvent;
            var weaponHandler = bossStateMachine.GetComponent<WeaponHandler>();

            Action situationAction = null;
            situationAction = () =>
            {
                weaponHandler.DeactivateLeftSingleSword();
                weaponHandler.DeactivateRightSingleSword();
                weaponHandler.OnStoreWeapon();
                weaponHandler.ActivateDoubleEdgedSword();
                AudioManager.Instance.PlaySound(caster.TargetCaster().transform, AudioManager.Instance.transformToBladeResource);

                manageEvent.SituationEvent -= situationAction;
            };
            manageEvent.SituationEvent += situationAction;
        }
    }
}