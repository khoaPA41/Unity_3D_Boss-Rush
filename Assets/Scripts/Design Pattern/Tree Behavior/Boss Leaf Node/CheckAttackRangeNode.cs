using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using Design_Pattern.Tree_Behavior.Boss;


namespace Design_Pattern.Tree_Behavior
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
            bossBehaviorBrain.AttackRange * bossBehaviorBrain.AttackRange) && !bossStateMachine.PlayerStateMachine.Invisible &&
            !bossStateMachine.PlayerStateMachine.Invisible;
        }

        public override NodeState Evaluate()
        {
            if (!bossBehaviorBrain.IsChasingState) return NodeState.Failure;

            if (CheckAttackRange() && bossStateMachine.Player.currentHealth > 0f)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}