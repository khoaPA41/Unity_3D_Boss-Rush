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
            return bossSystem.IsAttackRange() ? NodeState.Success : NodeState.Failure;
        }
    }
}
