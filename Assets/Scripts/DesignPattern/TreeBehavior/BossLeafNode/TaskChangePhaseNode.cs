using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;

namespace DesignPattern.TreeBehavior.BossLeafNode
{
    public class TaskChangePhaseNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public TaskChangePhaseNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public override NodeState Evaluate()
        {
            bossBehaviorBrain.CurrentPhase++;
            bossBehaviorBrain.NextPhase = bossBehaviorBrain.CurrentPhase + 1;

            return NodeState.Success;
        }
    }
}