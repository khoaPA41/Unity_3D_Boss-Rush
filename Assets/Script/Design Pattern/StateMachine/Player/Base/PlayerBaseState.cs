using Script.Attack.Skill_Factory;
using Script.Design_Pattern.StateMachine.Base;
using UnityEngine;

namespace Script.Design_Pattern.StateMachine.Player.Base
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine playerStateMachine;
        private AnimatorOverrideController overrideController;
        
        protected PlayerBaseState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            playerStateMachine.CharacterController.Move
            ((motion + playerStateMachine.ForceReceiver.Movement) *
             (playerStateMachine.ForceReceiver.GetCoefficientOfMovement() * deltaTime));
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void FaceDir(Vector3 movement, float deltaTime)
        {
            if (movement == Vector3.zero)
            {
                return;
            }

            playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation,
                Quaternion.LookRotation(movement), playerStateMachine.RotationDamping * deltaTime);
        }

        protected void FaceTarget(float deltaTime)
        {
            Target.Target currentTarget = playerStateMachine.Targeter.currentTarget;
            if (currentTarget is null)
            {
                return;
            }

            var dir = (currentTarget.transform.position - playerStateMachine.transform.position);
            dir.y = 0;
            playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation,
                Quaternion.LookRotation(dir), playerStateMachine.RotationDamping * deltaTime);
        }

        protected Vector3 CalculateMovementInFreeLook()
        {
            var forward = playerStateMachine.MainCameraTransform.forward;
            var right = playerStateMachine.MainCameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * playerStateMachine.InputReader.InputMovement.y +
                   right * playerStateMachine.InputReader.InputMovement.x;
        }

        protected Vector3 CalculateMovementInTarget()
        {
            var movement = new Vector3();
            movement += playerStateMachine.transform.forward * playerStateMachine.InputReader.InputMovement.y;
            movement += playerStateMachine.transform.right * playerStateMachine.InputReader.InputMovement.x;
            return movement;
        }
        
        protected void ChangeSwordIdle(string idleAnimationName, AnimationClip animationClip)
        {
            overrideController = new AnimatorOverrideController(playerStateMachine.Animator.runtimeAnimatorController);
            playerStateMachine.Animator.runtimeAnimatorController = overrideController;
            overrideController[idleAnimationName] = animationClip;
        }
        
    }
}