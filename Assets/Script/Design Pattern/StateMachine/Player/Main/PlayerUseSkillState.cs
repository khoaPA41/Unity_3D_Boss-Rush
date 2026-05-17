using Script.Design_Pattern.StateMachine.Player.Base;

namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerUseSkillState : PlayerBaseState
    {
        private const string UseSkillAnimationString = "UseSkill";
        private readonly string AnimationName;

        public PlayerUseSkillState(PlayerStateMachine playerStateMachine, string animationName) : base(playerStateMachine)
        {
            this.AnimationName = animationName;
        }

        public override void Enter()
        {
            playerStateMachine.Animator.CrossFadeInFixedTime(AnimationName, playerStateMachine.AnimationCrossFade, 0);
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
        }
    }
}