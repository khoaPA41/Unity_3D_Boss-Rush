using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.Tree_Behavior.Base;

namespace Script.Design_Pattern.Tree_Behavior.LeafNode
{
    public class CheckAttackRange : BehaviorNode
    {
        private FinalBossStateMachine bossSystem;
        
        public CheckAttackRange(FinalBossStateMachine bossSystem)
        {
            this.bossSystem = bossSystem;
        }
        
        public override NodeState Evaluate()
        {
            if (!bossSystem.IsChasingState) return NodeState.Failure;
            return bossSystem.IsAttackRange() && !bossSystem.IsActiveUltimate ? NodeState.Success : NodeState.Failure;
        }
    }
}
