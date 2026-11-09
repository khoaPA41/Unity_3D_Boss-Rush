using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;


namespace DesignPattern.TreeBehavior.BossLeafNode
{
    public class CheckAttackRangeNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckAttackRangeNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public bool CheckAttackRange()
        {
            return ((bossStateMachine.Player.transform.position - bossStateMachine.transform.position).sqrMagnitude <=
            bossBehaviorBrain.AttackRange * bossBehaviorBrain.AttackRange) && !bossStateMachine.PlayerStateMachine.Invisible;
        }

        public override NodeState Evaluate()
        {
            if (!bossBehaviorBrain.IsChasingState) return NodeState.Failure;

            if (CheckAttackRange() && bossStateMachine.Player.CurrentHealth > 0f)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}