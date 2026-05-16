public class PlayerUseSkillState : PlayerBaseState
{
    readonly string UseSkillAnimationString = "UseSkill";
    string AnimationName;

    public PlayerUseSkillState(PlayerStateMachine playerStateMachine, string animationName) : base(playerStateMachine)
    {
        AnimationName = animationName;
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(AnimationName, playerStateMachine.AnimationCrossFade, 0);
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
