using Attack.Skill_Factory;
using UnityEngine;
using Design_Pattern.StateMachine.Base;
using Status;

namespace Design_Pattern.StateMachine.Boss
{
    public abstract class BossBaseState : State
    {
        protected readonly BossStateMachine bossStateMachine;

        protected BossBaseState(BossStateMachine bossStateMachine)
        {
            this.bossStateMachine = bossStateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            if (bossStateMachine.PlayerStateMachine.Invisible)
            {
                return;
            }

            bossStateMachine.CharacterController.Move(
                (motion + bossStateMachine.ForceReceiver.Movement) *
                (bossStateMachine.ForceReceiver.GetCoefficientOfMovement() * deltaTime));
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        public Vector3 GetDirToPlayer(Transform target)
        {
            if (target.TryGetComponent(out Health _))
            {
                if (bossStateMachine.PlayerStateMachine.Invisible)
                {
                    return Vector3.zero;
                }
            }

            var dir = (target.position - bossStateMachine.transform.position)
                .normalized;
            dir.y = 0;
            return dir;
        }

        protected void FaceTarget(Vector3 dir, Transform target)
        {
            if (target.TryGetComponent(out Health _))
            {
                if (bossStateMachine.PlayerStateMachine.Invisible)
                {
                    return;
                }
            }

            if (dir == Vector3.zero) return;
            bossStateMachine.transform.rotation = Quaternion.LookRotation(dir);
        }

        protected void UseSkill(SkillType skillType)
        {
            var skill = SkillFactory.CreateSkill(skillType);
            skill?.Cast(bossStateMachine);
        }

    }
}

