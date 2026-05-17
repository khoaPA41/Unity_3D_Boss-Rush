using Script.Design_Pattern.StateMachine.Player.Base;


namespace Script.Design_Pattern.StateMachine.Player.Main
{
    public class PlayerUsePotionState : PlayerBaseState
    {
        public PlayerUsePotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Tick(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void PhysicTick(float fixedDeltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
