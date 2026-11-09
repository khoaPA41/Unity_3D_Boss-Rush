using DesignPattern.StateMachine.Boss.Base;
using DesignPattern.TreeBehavior.Base;
using DesignPattern.TreeBehavior.Brain;
using UnityEngine;

namespace DesignPattern.TreeBehavior.BossLeafNode
{
    public class CheckCounterAttackNode : BehaviorNode
    {
        private BossStateMachine bossStateMachine;
        private BossBehaviorBrain bossBehaviorBrain;


        public CheckCounterAttackNode(BossStateMachine bossStateMachine, BossBehaviorBrain bossBehaviorBrain)
        {
            this.bossStateMachine = bossStateMachine;
            this.bossBehaviorBrain = bossBehaviorBrain;
        }

        public override NodeState Evaluate()
        {
            int hitsNeeded = Random.Range(bossBehaviorBrain.EnduredTimes / 2, bossBehaviorBrain.EnduredTimes + 1);
            if (hitsNeeded - bossBehaviorBrain.HitReceived == 0)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}