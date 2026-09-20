using Design_Pattern.StateMachine.Boss;
using Design_Pattern.Tree_Behavior.Base;
using Design_Pattern.Tree_Behavior.Boss;
using UnityEngine;

namespace Design_Pattern.Tree_Behavior
{
    public class TaskAttackingNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public TaskAttackingNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }
        public override NodeState Evaluate()
        {
            if (!bossStateMachine.IsAttack)
            {
                bossStateMachine.IsAttack = true;
            }

            if (bossStateMachine.IsFinishedAttack)
            {
                bossStateMachine.IsAttack = false;
                bossStateMachine.LastAttackTime = Time.time;
                return NodeState.Failure;
            }

            if (Time.time - bossStateMachine.LastAttackTime >= bossBehaviorBrain.TimeOutCombo)
            {
                bossStateMachine.NextAttackIndex = 0;
            }
            return NodeState.Running;
        }
    }
}