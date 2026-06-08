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
            if (!bossSystem.IsAttack)
            {
               bossSystem.IsAttack = true;
            }
            
            if (bossSystem.IsFinishedAttack)
            {
                bossSystem.IsAttack = false;
                bossSystem.LastAttackTime = Time.time;
                return NodeState.Failure;
            }
            
            if (Time.time - bossSystem.LastAttackTime >= bossSystem.TimeOutCombo)
            {
                bossSystem.NextAttackIndex = 0;
            }
            
            return NodeState.Running;
        }
    }
}