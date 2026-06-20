using System;
using System.Collections;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerUseSkillState : PlayerBaseState
    {
        private ISkill currentSkill;
        private AttackData skillData;
        
        public PlayerUseSkillState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            
        }
        
        public override void Enter()
        {
            skillData = playerStateMachine.SkillData[playerStateMachine.SkillNumber - 1];
            currentSkill = UseSkill(skillData.SkillType);
            if (currentSkill is null)
            {
                playerStateMachine.ReturnLocomotion();
            }
            
            playerStateMachine.Animator.CrossFadeInFixedTime(skillData.AnimationName, skillData.AnimationTransition, 0);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, skillData.AnimationTag, 0);
            if (normalizeTime < skillData.AttackAnimationTime && normalizeTime < 1f) return;
            // ResetAfterSkill(currentSkill.SkillEffect);
            playerStateMachine.ReturnLocomotion();
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
            
        }

        public override void Exit()
        {
        }
        
        private ISkill UseSkill(SkillType skillType)
        {
            var skill = SkillFactory.CreateSkill(skillType);
            
            if (skill == null) return null;
            
            if (playerStateMachine.Mana.currentMana < skill.ManaCost) return null;
            
            skill.Cast(playerStateMachine);
            return skill;
        }
        
        private void ResetAfterSkill(SkillEffect skillEffect)
        {
            switch (skillEffect)
            {
                case SkillEffect.NonEffect:
                case SkillEffect.Inescapable:
                case SkillEffect.Stunned:
                case SkillEffect.ThrowUp:
                    break;
                case SkillEffect.NoDamage:
                    playerStateMachine.StartCoroutine(Count(1f, () =>
                    {
                        playerStateMachine.Health.noDamage = false;
                        ResetToMainMaterial();
                    }
                    ));
                    break;
                case SkillEffect.Invisible:
                    playerStateMachine.StartCoroutine(Count(5f, () =>
                    {
                        playerStateMachine.Invisible = false;
                        ResetToMainMaterial();
                    }));
                    break;
                default:
                    return;
            }
        }
        
        private static IEnumerator Count(float time, Action callback)
        {
            yield return new WaitForSecondsRealtime(time);
            callback?.Invoke();
        }
        
        private void ResetToMainMaterial()
        {
            var tempMaterials = new Material[] {playerStateMachine.MainMaterial1, playerStateMachine.MainMaterial2};
            playerStateMachine.SkinnedMeshRenderer.materials = tempMaterials;
        }
    }
}