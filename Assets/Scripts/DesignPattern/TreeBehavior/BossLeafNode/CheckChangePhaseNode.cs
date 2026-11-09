using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;

namespace DesignPattern.TreeBehavior.BossLeafNode
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
            if (bossBehaviorBrain.CurrentPhase + 1 == bossBehaviorBrain.TotalPhases)
            {
                return NodeState.Failure;
            }

            if (bossStateMachine.NormalCombo[bossBehaviorBrain.NextPhase].HealthThreshold >=
            bossStateMachine.Health.CurrentHealth / bossStateMachine.Health.MaxHealth)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}