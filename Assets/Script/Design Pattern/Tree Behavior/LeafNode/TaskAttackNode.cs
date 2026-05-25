using Script.Design_Pattern.StateMachine.Boss.Base;
using Script.Design_Pattern.Tree_Behavior.Base;
using UnityEngine;

namespace Script.Design_Pattern.Tree_Behavior.LeafNode
{
    public class TaskAttackNode : BehaviorNode
    {
        private FinalBossStateMachine bossSystem;

        public TaskAttackNode( FinalBossStateMachine bossSystem)
        {
            this.bossSystem = bossSystem;
        }
        
        public override NodeState Evaluate()
        {
            bossSystem.IsAttack = true;
            if (Time.time - bossSystem.LastAttackTime >= bossSystem.TimeOutCombo)
            {
                bossSystem.CurrentComboIndex = 0;
            }
                
            return NodeState.Success;
        }
    }
}