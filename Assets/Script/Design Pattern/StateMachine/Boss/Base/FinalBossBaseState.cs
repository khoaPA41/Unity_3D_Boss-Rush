using Script.Design_Pattern.StateMachine.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Boss.Base
{
    public abstract class FinalBossBaseState : State
    {
        protected readonly FinalBossStateMachine finalBossStateMachine;

        protected FinalBossBaseState(FinalBossStateMachine finalBossStateMachine)
        {
            this.finalBossStateMachine = finalBossStateMachine;
        }


        protected void Move(Vector3 motion, float deltaTime)
        {
            if (finalBossStateMachine.PlayerStateMachine.Invincible) { return;}
            finalBossStateMachine.CharacterController.Move(
                (motion + finalBossStateMachine.ForceReceiver.Movement) *
                (finalBossStateMachine.ForceReceiver.GetCoefficientOfMovement() * deltaTime));
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void FaceTarget(Vector3 dir)
        {
            if (finalBossStateMachine.PlayerStateMachine.Invincible) { return;}
            finalBossStateMachine.transform.rotation = Quaternion.LookRotation(dir);
        }

        protected Vector3 GetDirToPlayer()
        {
            if (finalBossStateMachine.PlayerStateMachine.Invincible) { return Vector3.zero; }
            var dir = (finalBossStateMachine.Player.transform.position - finalBossStateMachine.transform.position)
                .normalized;
            dir.y = 0;
            return dir;
        }

        protected bool IsAttackRange()
        {
            return ((finalBossStateMachine.Player.transform.position - finalBossStateMachine.transform.position)
                .sqrMagnitude <= finalBossStateMachine.AttackRange * finalBossStateMachine.AttackRange) && 
                   !finalBossStateMachine.PlayerStateMachine.Invincible;
        }


        protected bool IsWalkRange()
        {
            return ((finalBossStateMachine.Player.transform.position - finalBossStateMachine.transform.position)
                .sqrMagnitude <= finalBossStateMachine.AttackRange * finalBossStateMachine.WalkRange) && 
                   !finalBossStateMachine.PlayerStateMachine.Invincible;
        }
    }
}