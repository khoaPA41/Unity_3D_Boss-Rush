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
        private readonly int skillNumber;
        private ISkill currentSkill;
        
        public PlayerUseSkillState(PlayerStateMachine playerStateMachine, int skillNumber) : base(
            playerStateMachine)
        {
            this.skillNumber = skillNumber;
        }
        
        public override void Enter()
        {
            currentSkill = UseSkill(skillNumber);

            if (currentSkill is null)
            {
                playerStateMachine.ReturnLocomotion();
            }
            
            playerStateMachine.Animator.CrossFadeInFixedTime(currentSkill.AnimationName, playerStateMachine.AnimationCrossFade, 0);
            // playerStateMachine.CountSkillTime = playerStateMachine.SkillTime;
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, UseSkillAnimationString, 0);
            if (normalizeTime is > 0.8f and <= 1f)
            {
                playerStateMachine.ReturnLocomotion();
            }
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
            
        }

        public override void Exit()
        {
            ResetAfterSkill(currentSkill.SkillEffect);
        }
        
        private ISkill UseSkill(int skillNumber)
        {
            var skill = SkillFactory.CreateSkill(skillNumber);
            
            if (playerStateMachine.Mana.currentMana <= 0 &&
                playerStateMachine.Mana.currentMana < skill.ManaCost) return null;
            if (skill == null) return null;
            
            skill.Cast(playerStateMachine);
            return skill;
        }
        
        
        private void ResetAfterSkill(SkillEffect skillEffect)
        {
            switch (skillEffect)
            {
                case SkillEffect.NonEffect: return;
                case SkillEffect.Inescapable:
                    break;
                case SkillEffect.Stunned:
                    break;
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