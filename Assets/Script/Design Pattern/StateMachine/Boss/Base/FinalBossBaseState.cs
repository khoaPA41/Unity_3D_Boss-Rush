using Script.Attack;
using Script.Attack.Skill_Factory;
using Script.Design_Pattern.StateMachine.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Base
{
    public abstract class FinalBossBaseState : State
    {
        protected readonly FinalBossStateMachine FinalBossStateMachine;

        protected FinalBossBaseState(FinalBossStateMachine finalBossStateMachine)
        {
            this.FinalBossStateMachine = finalBossStateMachine;
        }


        protected void Move(Vector3 motion, float deltaTime)
        {
            if (FinalBossStateMachine.PlayerStateMachine.Invisible)
            {
                return;
            }

            FinalBossStateMachine.CharacterController.Move(
                (motion + FinalBossStateMachine.ForceReceiver.Movement) *
                (FinalBossStateMachine.ForceReceiver.GetCoefficientOfMovement() * deltaTime));
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void FaceTarget(Vector3 dir, Transform target)
        {
            if (target.TryGetComponent(out Health _))
            {
                if (FinalBossStateMachine.PlayerStateMachine.Invisible)
                {
                    return;
                }
            }

            FinalBossStateMachine.transform.rotation = Quaternion.LookRotation(dir);
        }
        

        protected void UseSkill(SkillType skillType)
        {
            var skill = SkillFactory.CreateSkill(skillType);
            // if (skill is null) return;
            skill?.Cast(FinalBossStateMachine);
        }
    }
}