using UnityEngine;
public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine playerStateMachine;
    AnimatorOverrideController overrideController;
    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        playerStateMachine.CharacterController.Move((motion + playerStateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceDir(Vector3 movement, float deltaTime)
    {
        playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation, Quaternion.LookRotation(movement), playerStateMachine.RotationDamping * deltaTime);
    }

    protected void FaceTarget(float deltaTime)
    {
        Target currentTarget = playerStateMachine.Targeter.currentTarget;
        if (currentTarget == null) { return; }
        Vector3 dir = (currentTarget.transform.position - playerStateMachine.transform.position);
        dir.y = 0;
        playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation, Quaternion.LookRotation(dir), playerStateMachine.RotationDamping * deltaTime);
    }

    protected Vector3 CalculateMovementInFreeLook()
    {
        Vector3 forward = playerStateMachine.MainCameraTransform.forward;
        Vector3 right = playerStateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * playerStateMachine.InputReader.InputMovement.y + right * playerStateMachine.InputReader.InputMovement.x;
    }

    protected Vector3 CalculateMovementInTarget()
    {
        Vector3 movement = new Vector3();
        movement += playerStateMachine.transform.forward * playerStateMachine.InputReader.InputMovement.y;
        movement += playerStateMachine.transform.right * playerStateMachine.InputReader.InputMovement.x;
        return movement;
    }

    protected void EnterJumpState()
    {
        playerStateMachine.SwitchState(new PlayerStartJumpState(playerStateMachine));
        return;
    }

    protected void ChangeSwordIdle(string IdleAnimationName, AnimationClip animationClip)
    {
        overrideController = new AnimatorOverrideController(playerStateMachine.Animator.runtimeAnimatorController);
        playerStateMachine.Animator.runtimeAnimatorController = overrideController;
        overrideController[IdleAnimationName] = animationClip;
    }

    protected void UseSkill(int skillNumber)
    {
        ISkill skill = SkillFactory.CreateSkill(skillNumber);
        if (skill != null)
        {
            if (playerStateMachine.Mana.currentMana >= skill.ManaCost)
            {
                skill.Cast(playerStateMachine);
            }
            else
            {
                playerStateMachine.ReturnLocomotion();
            }
        }
        else
        {
            playerStateMachine.ReturnLocomotion();
        }
    }


}
