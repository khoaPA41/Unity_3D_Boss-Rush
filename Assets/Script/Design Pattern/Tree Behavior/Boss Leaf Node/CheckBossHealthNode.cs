using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using Design_Pattern.Tree_Behavior.Boss;

namespace Design_Pattern.Tree_Behavior
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