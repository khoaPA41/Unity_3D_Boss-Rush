using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;
using UnityEngine;

namespace DesignPattern.TreeBehavior.BossLeafNode
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