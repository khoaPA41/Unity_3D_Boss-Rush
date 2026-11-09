using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;

namespace DesignPattern.TreeBehavior.BossLeafNode
{
    public class CheckUltimateNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckUltimateNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }
        public override NodeState Evaluate()
        {
            if (bossBehaviorBrain.IsUsedUltimate) return NodeState.Failure;
            if (bossStateMachine.UltimateCombo.AttackData.Length == 0) return NodeState.Failure;

            bossStateMachine.IsUltimateAttack = true;
            return NodeState.Success;
        }
    }
}