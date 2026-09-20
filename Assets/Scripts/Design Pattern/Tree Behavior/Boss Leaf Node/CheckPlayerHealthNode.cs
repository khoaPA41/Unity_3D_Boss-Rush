using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using Design_Pattern.Tree_Behavior.Boss;


namespace Design_Pattern.Tree_Behavior
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
            if (bossStateMachine.Player.currentHealth > 0) return NodeState.Failure;

            bossStateMachine.IsChasing = false;
            return NodeState.Success;
        }
    }
}