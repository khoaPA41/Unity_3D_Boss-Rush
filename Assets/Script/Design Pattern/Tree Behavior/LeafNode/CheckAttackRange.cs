using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

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
            
            if (bossSystem.IsAttackRange() && !bossSystem.IsChangePhase)
            {
                return NodeState.Success;
            }
            
            return NodeState.Failure;
        }
    }
}
