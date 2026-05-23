using System;
using System.Collections;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.EventBus;
using Script.Design_Pattern.StateMachine.Player.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerUseSkillState : PlayerBaseState
    {
        private const string UseSkillAnimationString = "UseSkill";
        private int skillNumber;
        private ISkill currentSkill;
        
        public PlayerUseSkillState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }
        
        public override void Enter()
        {
            skillNumber = playerStateMachine.SkillNumber;
            IsFinished = false;
            currentSkill = UseSkill(skillNumber);

            if (currentSkill is null)
            {
                IsFinished = true;
                return;
            }
            
            playerStateMachine.Animator.CrossFadeInFixedTime(currentSkill.AnimationName, playerStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, UseSkillAnimationString, 0);
            if (normalizeTime is <= 0.8f or > 1f) return;
            ResetAfterSkill(currentSkill.SkillEffect);
            IsFinished = true;
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
            
        }

        public override void Exit()
        {
            IsFinished = true;
        }
        
        private ISkill UseSkill(int skillNumber)
        {
            var skill = SkillFactory.CreateSkill(skillNumber);
            
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