using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;

namespace DesignPattern.TreeBehavior.BossLeafNode
{
    public class CheckBossHealthNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckBossHealthNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }
        public override NodeState Evaluate()
        {
            if (!bossBehaviorBrain.IsDead) return NodeState.Failure;
            bossBehaviorBrain.CallDeadPhaseAction();
            return NodeState.Success;
        }
    }
}