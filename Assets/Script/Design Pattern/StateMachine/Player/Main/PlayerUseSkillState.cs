using UnityEngine;

public class PlayerUseSkillState : PlayerBaseState
{
    readonly int UseSkillAnimationHash = Animator.StringToHash("UseSkill");
    readonly string UseSkillAnimationString = "UseSkill";
    //int skillNumber;
    public PlayerUseSkillState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        //UseSkill(skillNumber);

        playerStateMachine.Animator.CrossFadeInFixedTime(UseSkillAnimationHash, playerStateMachine.AnimationCrossFade, 0);
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, UseSkillAnimationString, 0);
        if (normalizeTime > 0.8f && normalizeTime <= 1f)
        {
            playerStateMachine.ReturnLocomotion();
        }
    }

    public override void PhysicTick(float fixedDeltaTime)
    {

    }

    public override void Exit()
    {


    }
}
