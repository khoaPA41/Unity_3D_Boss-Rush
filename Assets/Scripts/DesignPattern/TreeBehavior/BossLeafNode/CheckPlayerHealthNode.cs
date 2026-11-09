using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;


namespace DesignPattern.TreeBehavior.BossLeafNode
{
    public class CheckPlayerHealthNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckPlayerHealthNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public override NodeState Evaluate()
        {
            if (bossStateMachine.Player.CurrentHealth > 0) return NodeState.Failure;

            bossStateMachine.IsChasing = false;
            return NodeState.Success;
        }
    }
}