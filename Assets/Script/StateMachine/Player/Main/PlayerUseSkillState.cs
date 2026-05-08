using UnityEngine;

public class PlayerUseSkillState : PlayerBaseState
{
    readonly int UseSkillAnimationHash = Animator.StringToHash("UseSkill");
    readonly string UseSkillAnimationString = "UseSkill";
    int skillNumber;
    public PlayerUseSkillState(PlayerStateMachine playerStateMachine, int skillNumber) : base(playerStateMachine)
    {
        this.skillNumber = skillNumber;
    }

    public override void Enter()
    {
        UseSkill(skillNumber);

        playerStateMachine.Animator.CrossFadeInFixedTime(UseSkillAnimationHash, playerStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        float normalizeTime = GetNormalizeTime(playerStateMachine.Animator, UseSkillAnimationString);
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
