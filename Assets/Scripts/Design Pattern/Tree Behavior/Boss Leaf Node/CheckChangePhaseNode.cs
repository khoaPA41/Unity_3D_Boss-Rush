using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using Design_Pattern.Tree_Behavior.Boss;

namespace Design_Pattern.Tree_Behavior
{
    public class CheckChangePhaseNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckChangePhaseNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }
        public override NodeState Evaluate()
        {
            if (bossBehaviorBrain.CurrentPhase + 1 == bossBehaviorBrain.Phase)
            {
                return NodeState.Failure;
            }

            if (bossStateMachine.NormalCombo[bossBehaviorBrain.NextPhase].HealthThreshold >=
            bossStateMachine.Health.currentHealth / bossStateMachine.Health.maxHealth)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}